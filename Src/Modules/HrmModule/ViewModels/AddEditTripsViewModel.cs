using DevExpress.CodeParser;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Layout.Core;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Printing;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Model;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Modules.Hrm.Reports;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Utility;
using iTextSharp.text.pdf.codec;
using Prism.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;



namespace Emdep.Geos.Modules.Hrm.ViewModels
{//[Sudhir.Jangra][GEOS2-4816]
    public class AddEditTripsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services

        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        // IHrmService HrmService = new HrmServiceController("localhost:6699");
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

        #endregion // End Of Events

        #region Declaration
        List<Department> departmentList;
        Department selectedDepartment;
        GroupBoxState attachmentvisiblevisibility;
        private bool weekend;
        private string accommodationOtherRoom;
        private string partnerProvidedRoom;
        bool isEnableField = false;
        private bool isNew;
        private string windowHeader;
        MaximizedElementPosition maximizedElementPosition;
        private double dialogHeight;
        private double dialogWidth;
        private string code;
        private ObservableCollection<Traveller> travellerList;
        private Traveller selectedTraveller;
        private List<LookupValue> typeList;
        private LookupValue selectedType;
        private List<LookupValue> proposeList;
        private LookupValue selectedPropose;
        private List<LookupValue> mainTransportList;
        private LookupValue selectedArrivalTransport;
        private LookupValue selectedDepartureTransport;
        private ObservableCollection<Company> originList;
        private Company selectedOrigin;
        private Company oldSelectedOrigin;
        private DateTime arrivalDate;
        private TimeSpan arrivalDateHours;
        private DateTime departureDate;
        private TimeSpan departureDateHours;
        private int durationDays;
        private string title;
        private List<LookupValue> accommodationTypeList;
        private LookupValue selectedAccommodationType;
        private ObservableCollection<Destination> destinationList;
        private Destination selectedDestination;
        private List<Currency> tripCurrencyList;
        private Currency selectedTripCurrency;
        private string myFilterString;
        private string organization;
        private string accommodationDetails;
        private string remarks;
        private EmployeeTrips newEmployeeTrip;
        private EmployeeTrips updatedEmployeeTrip;
        private string error = string.Empty;
        private bool isSaveChanges;
        private EmployeeTrips employeeTrip;
        private List<LogEntriesByEmployeeTrip> tripChangeLogList;
        private int idEmployeeTrip;
        private EmployeeTrips editEmployeeTrip;
        private bool isTravellerSamePlant;
        private string previousTravellerFullName;
        private int count;
        private bool isAttachmentModified = false;
        private bool isInActive;
        private List<int> uniqueIdCompanies;  //pramod.misal GEOS2-5793 24.06.2024
        private Company selectedCompany;  //pramod.misal GEOS2-5793 24.06.2024


        #region [nsatpute][11-09-2024][GEOS2-5929]
        private bool isCustomTravelerEnable;
        private string customTraveler;
        private bool isEmployeeTravelerEnable;
        private bool isOriginEnable;
        private bool isDestinationEnable;
        private ObservableCollection<Employee> responsibleList;
        private Employee selectedResponsible;
        private string travelerEmail;
        private DateTime fromDate;
        private DateTime toDate;
        private string desciption;
        private bool visaRequired;
        private string arrivalTransportationNumber;
        private string departureTransportationNumber;
        private string arrivalTransferRemark;
        private string departureTransferRemark;
        private string arrivalTransporterContact;
        private string arrivalProvider;
        private string departureTransporterContact;
        private string departureTransporterName;
        private string arrivalTransporterName;
        private string departureProvider;
        private bool partnerProvidedRoomEnable;
        private bool otherRoomEnable;
        private string otherRoom;
        private string accommodationAddress;
        private string accommodationCoordinates;
        private string accommodationRemarks;
        private bool simCardProvided;
        private bool moneyDeliveredAtOrigin;
        private bool moneyDeliveredAtDestination;
        private bool plantCarProvided;
        private bool mobilePhoneProvided;
        private bool plantCarControlEnable;
        private bool mobilePhoneControlEnable;
        private bool simCardControlEnable;
        private List<LookupValue> workShiftList;
        private LookupValue selectedWorkShift;
        private List<Destination> supplierList;
        private List<LookupValue> transportationTypeList;
        private LookupValue selectedArrivalTransportationType;
        private LookupValue selectedDepartureTransportationType;
        private ObservableCollection<TripAssets> carAssetList;
        private ObservableCollection<TripAssets> simCardAssetList;
        private ObservableCollection<TripAssets> mobilePhoneAssetList;
        private ObservableCollection<TripAssets> accommodationAssetList;
        private TripAssets selectedCarAsset;
        private TripAssets selectedSimCardAsset;
        private TripAssets selectedMobileAsset;
        private TripAssets selectedAccommodationAsset;
        private bool isEmdepRoomEnable;
        private List<Destination> allCustomer;
        private List<Destination> allPlantList;
        private bool initComplete = false;
        private ObservableCollection<TripAttachment> listAttachment;
        private List<TripStatus> tripStatusList;
        private bool isBusy;
        private ObservableCollection<LookupValue> attachmentTypeList;
        private List<TripAttachment> attachmentFilesToDelete;
        private List<TripAttachment> updatedFileList;
        public List<TripAttachment> originalListAttachment;
        private bool isStatusChanged;//[Sudhir.Jangra][GEOS2-5933]
        private bool isControlsEnable;
        private TripAttachment selectedAttachment;
        UInt32 idEmployeeTripNew;
        private Visibility exportExpenseReportButtonCommandVisibility;
        EmployeeTrips selectedEmployeeTrips;
        #endregion

        #region [nsatpute][24-09-2024][GOES2-6473]
        private WorkflowStatus workflowStatus;
        private List<WorkflowStatus> workflowStatusList;
        private List<WorkflowStatus> workflowStatusButtons;
        private WorkflowStatus selectedWorkflowStatusButton;
        private List<WorkflowTransition> workflowTransitionList;
        private bool isAllControlDisabled;
        private Visibility clearButtonVisibility;
        #endregion

        private Employee_trips_transfers selectedTransfers;
        private ObservableCollection<EmployeeTripsAccommodations> accommodationsList;// [shweta.thube][GEOS2-7989][18-09-2025]
        #endregion

        #region Properties
        //[Sudhir.Jangra][GEOS2-5933]

        public Employee_trips_transfers SelectedTransfers
        {
            get { return selectedTransfers; }
            set
            {
                selectedTransfers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTransfers"));
            }

        }

        public bool IsStatusChanged
        {
            get { return isStatusChanged; }
            set
            {
                isStatusChanged = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsStatusChanged"));
            }
        }
        public GroupBoxState Attachmentvisiblevisibility
        {
            get { return attachmentvisiblevisibility; }
            set
            {
                attachmentvisiblevisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Attachmentvisiblevisibility"));
            }
        }
        //[rdixit][GEOS2-5930][20.09.2024]
        public string AccommodationOtherRoom
        {
            get { return accommodationOtherRoom; }
            set { accommodationOtherRoom = value; OnPropertyChanged(new PropertyChangedEventArgs("AccommodationOtherRoom")); }
        }
        public string PartnerProvidedRoom
        {
            get { return partnerProvidedRoom; }
            set { partnerProvidedRoom = value; OnPropertyChanged(new PropertyChangedEventArgs("PartnerProvidedRoom")); }
        }
        public bool Weekend
        {
            get { return weekend; }
            set
            {
                weekend = value; OnPropertyChanged(new PropertyChangedEventArgs("Weekend"));
            }
        }
        public bool IsInActive
        {
            get
            {
                return isInActive;
            }
            set
            {
                isInActive = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActive"));
            }
        }
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
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
        public bool IsEnableField
        {
            get
            {
                return isEnableField;
            }

            set
            {

                isEnableField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnableField"));
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
        public ObservableCollection<Traveller> TravellerList
        {
            get { return travellerList; }
            set
            {
                travellerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TravellerList"));

            }
        }
        // [nsatpute][11-09-2024][GEOS2-5929]
        public Traveller SelectedTraveller
        {
            get { return selectedTraveller; }
            set
            {
                selectedTraveller = value;
                if (SelectedTraveller != null)
                {
                    TravelerEmail = SelectedTraveller.Email; // [nsatpute][22-10-2024][GEOS2-6656]
                    //[GEOS2-6760][rdixit][09.01.2025]
                    if (SelectedTraveller != null && IsNew)
                        SelectedDepartment = DepartmentList.FirstOrDefault(i => i.IdDepartment == SelectedTraveller.IdDepartment);

                    if (SelectedTraveller.OrganizationRegion != "" && !string.IsNullOrEmpty(SelectedTraveller.OrganizationRegion))
                    {
                        Organization = SelectedTraveller.Organization + "-" + SelectedTraveller.OrganizationRegion;
                    }
                    else
                    {
                        Organization = SelectedTraveller.Organization;
                    }
                    if (SelectedTraveller.Currency != null)
                    {
                        if (SelectedTraveller.Currency.IdCurrency != 0)
                            SelectedTripCurrency = TripCurrencyList.FirstOrDefault(x => x.IdCurrency == SelectedTraveller.Currency.IdCurrency);
                    }

                }
                if (initComplete && SelectedTraveller != null)
                {
                    IsCustomTravelerEnable = false;
                }
                else if (initComplete)
                {
                    IsCustomTravelerEnable = true;
                    TravelerEmail = string.Empty;
                    ClearButtonVisibility = Visibility.Hidden;
                }
                if (SelectedTraveller != null)
                    ClearButtonVisibility = Visibility.Visible;
                else
                    ClearButtonVisibility = Visibility.Hidden;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTraveller"));
            }
        }
        public List<LookupValue> TypeList
        {
            get { return typeList; }
            set
            {
                typeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TypeList"));
            }
        }
        // [nsatpute][11-09-2024][GEOS2-5929]
        public LookupValue SelectedType
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedType"));
                FillDestination();
                ReFillOriginDestination();
                //[GEOS2-6760][rdixit][14.01.2025]
                if (SelectedType != null)
                {
                    IsOriginEnable = true;
                    IsDestinationEnable = true;
                }
                else
                {
                    IsOriginEnable = false;
                    IsDestinationEnable = false;
                }
            }
        }
        public List<Department> DepartmentList
        {
            get
            {
                return departmentList;
            }
            set
            {
                departmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DepartmentList"));
            }
        }
        public Department SelectedDepartment
        {
            get
            {
                return selectedDepartment;
            }
            set
            {
                selectedDepartment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDepartment"));
                //[GEOS2-6760][rdixit][14.01.2025]
                if ((SelectedOrigin != null || SelectedDepartment != null))
                    IsEmployeeTravelerEnable = true;
                else
                    IsEmployeeTravelerEnable = false;
            }
        }
        public List<LookupValue> TransportationTypeList
        {
            get { return transportationTypeList; }
            set { transportationTypeList = value; OnPropertyChanged(new PropertyChangedEventArgs("TransportationTypeList")); }
        }
        public LookupValue SelectedArrivalTransportationType
        {
            get { return selectedArrivalTransportationType; }
            set { selectedArrivalTransportationType = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedArrivalTransportationType")); }
        }
        public LookupValue SelectedDepartureTransportationType
        {
            get { return selectedDepartureTransportationType; }
            set { selectedDepartureTransportationType = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedDepartureTransportationType")); }
        }
        public List<LookupValue> ProposeList
        {
            get { return proposeList; }
            set
            {
                proposeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProposeList"));
            }
        }
        public LookupValue SelectedPropose
        {
            get { return selectedPropose; }
            set
            {
                selectedPropose = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPropose"));
            }
        }
        public List<LookupValue> MainTransportList
        {
            get { return mainTransportList; }
            set
            {
                mainTransportList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainTransportList"));
            }
        }
        public LookupValue SelectedArrivalTransport
        {
            get { return selectedArrivalTransport; }
            set
            {
                selectedArrivalTransport = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArrivalTransport"));
            }
        }
        public ObservableCollection<Company> OriginList
        {
            get { return originList; }
            set
            {
                originList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginList"));
            }
        }
        public Company SelectedOrigin
        {
            get { return selectedOrigin; }
            set
            {
                selectedOrigin = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOrigin"));
                //[GEOS2-6760][rdixit][14.01.2025]
                if ((SelectedOrigin != null || SelectedDepartment != null))
                    IsEmployeeTravelerEnable = true;
                else
                    IsEmployeeTravelerEnable = false;
            }
        }
        public Company OldSelectedCompany
        {
            get { return oldSelectedOrigin; }
            set
            {
                oldSelectedOrigin = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldSelectedCompany"));
            }
        }
        public DateTime ArrivalDate
        {
            get { return arrivalDate; }
            set
            {
                arrivalDate = value;
                if (DepartureDate > ArrivalDate)
                {
                    DurationDays = Convert.ToInt32((DepartureDate - ArrivalDate).Days) + 1;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("ArrivalDate"));
            }
        }
        public TimeSpan ArrivalDateHours
        {
            get { return arrivalDateHours; }
            set
            {
                arrivalDateHours = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArrivalDateHours"));
            }
        }
        public DateTime DepartureDate
        {
            get { return departureDate; }
            set
            {
                departureDate = value;
                if (DepartureDate > ArrivalDate)
                {
                    DurationDays = Convert.ToInt32((DepartureDate - ArrivalDate).Days) + 1;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("DepartureDate"));
            }
        }
        public string DepartureDateWithFormat
        {
            get { return $@"{departureDate.Year}/{departureDate.Month.ToString().PadLeft(2, '0')}/{departureDate.Day.ToString().PadLeft(2, '0')}"; }
            set
            {
                departureDate = Convert.ToDateTime(value);

                OnPropertyChanged(new PropertyChangedEventArgs("DepartureDateWithFormat"));
            }
        }
        public TimeSpan DepartureDateHours
        {
            get { return departureDateHours; }
            set
            {
                departureDateHours = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DepartureDateHours"));
            }
        }
        public int DurationDays
        {
            get { return durationDays; }
            set
            {
                durationDays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DurationDays"));
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
            }
        }
        public List<LookupValue> AccommodationTypeList
        {
            get { return accommodationTypeList; }
            set
            {
                accommodationTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AccommodationTypeList"));
            }
        }
        public LookupValue SelectedAccommodationType
        {
            get { return selectedAccommodationType; }
            set
            {
                selectedAccommodationType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAccommodationType"));
                if (selectedAccommodationType?.Value == "Partner Provided")
                {
                    PartnerProvidedRoomEnable = true;
                }
                else
                {
                    PartnerProvidedRoomEnable = false;
                }
                if (selectedAccommodationType?.Value == "Hotel" || selectedAccommodationType?.Value == "Other")
                {
                    OtherRoomEnable = true;
                }
                else
                {
                    OtherRoomEnable = false;
                }
                if (selectedAccommodationType?.Value == "Apartment" || selectedAccommodationType?.Value?.Trim() == "Apartment & Hotel")
                {
                    IsEmdepRoomEnable = true;
                }
                else
                {
                    IsEmdepRoomEnable = false;
                }

            }
        }
        public ObservableCollection<Destination> DestinationList
        {
            get { return destinationList; }
            set
            {
                destinationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DestinationList"));
            }
        }
        public Destination SelectedDestination
        {
            get { return selectedDestination; }
            set
            {
                selectedDestination = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDestination"));
            }
        }
        public ObservableCollection<TripAttachment> ListAttachment
        {
            get { return listAttachment; }

            set
            {
                listAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListAttachment"));
            }
        }
        public List<TripAttachment> OriginalListAttachment
        {
            get { return originalListAttachment; }

            set
            {
                originalListAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginalListAttachment"));
            }
        }
        public List<Currency> TripCurrencyList
        {
            get { return tripCurrencyList; }
            set
            {
                tripCurrencyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TripCurrencyList"));
            }
        }
        public Currency SelectedTripCurrency
        {
            get { return selectedTripCurrency; }
            set
            {
                selectedTripCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTripCurrency"));
            }
        }
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        public string Organization
        {
            get { return organization; }
            set
            {
                organization = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Organization"));
            }
        }
        public string AccommodationDetails
        {
            get { return accommodationDetails; }
            set
            {
                accommodationDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AccommodationDetails"));
            }
        }
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remarks"));
            }
        }
        public EmployeeTrips NewEmployeeTrip
        {
            get { return newEmployeeTrip; }
            set
            {
                newEmployeeTrip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewEmployeeTrip"));
            }
        }
        public EmployeeTrips UpdatedEmployeeTrip
        {
            get { return updatedEmployeeTrip; }
            set
            {
                updatedEmployeeTrip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedEmployeeTrip"));
            }
        }
        public bool IsSaveChanges
        {
            get { return isSaveChanges; }
            set
            {
                isSaveChanges = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaveChanges"));
            }
        }
        public EmployeeTrips EmployeeTrip
        {
            get { return employeeTrip; }
            set
            {
                employeeTrip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeTrip"));
            }
        }
        public List<LogEntriesByEmployeeTrip> TripChangeLogList
        {
            get { return tripChangeLogList; }
            set
            {
                tripChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TripChangeLogList"));
            }
        }
        public int IdEmployeeTrip
        {
            get { return idEmployeeTrip; }
            set
            {
                idEmployeeTrip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdEmployeeTrip"));
            }
        }
        public EmployeeTrips EditEmployeeTrips
        {
            get { return editEmployeeTrip; }
            set
            {
                editEmployeeTrip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EditEmployeeTrips"));
            }
        }
        public bool IsTravellerSamePlant
        {
            get { return isTravellerSamePlant; }
            set
            {
                isTravellerSamePlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTravellerSamePlant"));
            }
        }
        public string PreviousTravellerFullName
        {
            get { return previousTravellerFullName; }
            set
            {
                previousTravellerFullName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousTravellerFullName"));
            }
        }
        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            }
        }
        public EmployeeTrips SelectedEmployeeTrips
        {
            get
            {
                return selectedEmployeeTrips;
            }
            set
            {
                selectedEmployeeTrips = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEmployeeTrips"));
            }
        }
        public UInt32 IdEmployeeTripNew
        {
            get
            {
                return idEmployeeTripNew;
            }
            set
            {
                idEmployeeTripNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdEmployeeTripNew"));
            }
        }
        public Visibility ExportExpenseReportButtonCommandVisibility
        {
            get
            {
                return exportExpenseReportButtonCommandVisibility;
            }
            set
            {
                exportExpenseReportButtonCommandVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExportExpenseReportButtonCommandVisibility"));
            }
        }
        //[pramod.misal][GEOS2-5793][24.06.2024]
        public List<int> UniqueIdCompanies
        {
            get { return uniqueIdCompanies; }
            set
            {
                uniqueIdCompanies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UniqueIdCompanies"));
            }
        }
        public ObservableCollection<LookupValue> AttachmentTypeList
        {
            get { return attachmentTypeList; }
            set { attachmentTypeList = value; OnPropertyChanged(new PropertyChangedEventArgs("AttachmentTypeList")); }
        }
        public List<TripAttachment> AttachmentFilesToDelete
        {
            get { return attachmentFilesToDelete; }

            set
            {
                attachmentFilesToDelete = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentFilesToDelete"));
            }
        }
        #region [nsatpute][11-09-2024][GEOS2-5929]
        public Company SelectedCompany
        {
            get
            {
                return selectedCompany;
            }

            set
            {
                selectedCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompany"));
            }
        }
        // [nsatpute][11-09-2024][GEOS2-5929]
        public bool IsCustomTravelerEnable
        {
            get { return isCustomTravelerEnable; }
            set
            {
                isCustomTravelerEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCustomTravelerEnable"));
            }
        }
        public bool IsEmployeeTravelerEnable
        {
            get { return isEmployeeTravelerEnable; }
            set { isEmployeeTravelerEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("IsEmployeeTravelerEnable")); }
        }
        public string CustomTraveler
        {
            get { return customTraveler; }
            set
            {
                customTraveler = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomTraveler"));
                if (initComplete && CustomTraveler == null && (SelectedOrigin != null || SelectedDepartment != null))
                {
                    IsEmployeeTravelerEnable = true;
                }
                else if (initComplete)
                {
                    IsEmployeeTravelerEnable = false;
                }
            }
        }
        public bool IsOriginEnable
        {
            get { return isOriginEnable; }
            set { isOriginEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("IsOriginEnable")); }
        }
        public bool IsDestinationEnable
        {
            get { return isDestinationEnable; }
            set { isDestinationEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("IsDestinationEnable")); }
        }
        public ObservableCollection<Employee> ResponsibleList
        {
            get { return responsibleList; }
            set { responsibleList = value; OnPropertyChanged(new PropertyChangedEventArgs("ResponsibleList")); }
        }
        public Employee SelectedResponsible
        {
            get { return selectedResponsible; }
            set
            {
                selectedResponsible = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedResponsible"));
            }
        }
        public string TravelerEmail
        {
            get { return travelerEmail; }
            set { travelerEmail = value; OnPropertyChanged(new PropertyChangedEventArgs("TravelerEmail")); }
        }
        public DateTime FromDate
        {
            get { return fromDate; }
            set { fromDate = value; OnPropertyChanged(new PropertyChangedEventArgs("FromDate")); }
        }
        public DateTime ToDate
        {
            get { return toDate; }
            set { toDate = value; OnPropertyChanged(new PropertyChangedEventArgs("ToDate")); }
        }
        public string Desciption

        {
            get { return desciption; }
            set { desciption = value; OnPropertyChanged(new PropertyChangedEventArgs("Desciption")); }
        }
        public bool VisaRequired
        {
            get { return visaRequired; }
            set { visaRequired = value; OnPropertyChanged(new PropertyChangedEventArgs("VisaRequired")); }
        }
        public string ArrivalTransportationNumber
        {
            get { return arrivalTransportationNumber; }
            set
            {
                try
                {
                    arrivalTransportationNumber = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ArrivalTransportationNumber"));
                }
                catch (Exception ex)
                {

                }
            }
        }
        public string DepartureTransportationNumber
        {
            get { return departureTransportationNumber; }
            set
            {
                try
                {
                    departureTransportationNumber = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("DepartureTransportationNumber"));
                }
                catch (Exception ex)
                {

                }
            }
        }
        public string ArrivalTransferRemark
        {
            get { return arrivalTransferRemark; }
            set { arrivalTransferRemark = value; OnPropertyChanged(new PropertyChangedEventArgs("ArrivalTransferRemark")); }
        }
        public string DepartureTransferRemark
        {
            get { return departureTransferRemark; }
            set { departureTransferRemark = value; OnPropertyChanged(new PropertyChangedEventArgs("DepartureTransferRemark")); }
        }
        public string ArrivalTransporterContact
        {
            get { return arrivalTransporterContact; }
            set { arrivalTransporterContact = value; OnPropertyChanged(new PropertyChangedEventArgs("ArrivalTransporterContact")); }
        }
        public string ArrivalProvider
        {
            get { return arrivalProvider; }
            set { arrivalProvider = value; OnPropertyChanged(new PropertyChangedEventArgs("ArrivalProvider")); }
        }
        public string DepartureTransporterContact
        {
            get { return departureTransporterContact; }
            set { departureTransporterContact = value; OnPropertyChanged(new PropertyChangedEventArgs("DepartureTransporterContact")); }
        }
        public string DepartureTransporterName
        {
            get { return departureTransporterName; }
            set { departureTransporterName = value; OnPropertyChanged(new PropertyChangedEventArgs("DepartureTransporterName")); }
        }
        public string ArrivalTransporterName
        {
            get { return arrivalTransporterName; }
            set { arrivalTransporterName = value; OnPropertyChanged(new PropertyChangedEventArgs("ArrivalTransporterName")); }
        }
        public string DepartureProvider
        {
            get { return departureProvider; }
            set { departureProvider = value; OnPropertyChanged(new PropertyChangedEventArgs("DepartureProvider")); }
        }
        public bool PartnerProvidedRoomEnable
        {
            get { return partnerProvidedRoomEnable; }
            set { partnerProvidedRoomEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("PartnerProvidedRoomEnable")); }
        }
        public bool OtherRoomEnable
        {
            get { return otherRoomEnable; }
            set { otherRoomEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("OtherRoomEnable")); }
        }
        public string OtherRoom
        {
            get { return otherRoom; }
            set { otherRoom = value; OnPropertyChanged(new PropertyChangedEventArgs("OtherRoom")); }
        }
        public string AccommodationAddress
        {
            get { return accommodationAddress; }
            set { accommodationAddress = value; OnPropertyChanged(new PropertyChangedEventArgs("AccommodationAddress")); }
        }
        public string AccommodationCoordinates
        {
            get { return accommodationCoordinates; }
            set { accommodationCoordinates = value; OnPropertyChanged(new PropertyChangedEventArgs("AccommodationCoordinates")); }
        }
        public string AccommodationRemarks
        {
            get { return accommodationRemarks; }
            set { accommodationRemarks = value; OnPropertyChanged(new PropertyChangedEventArgs("AccommodationRemarks")); }
        }
        public bool PlantCarProvided
        {
            get { return plantCarProvided; }
            set
            {
                plantCarProvided = value;
                PlantCarControlEnable = value;
                if (!PlantCarProvided)
                { SelectedCarAsset = null; }
                OnPropertyChanged(new PropertyChangedEventArgs("PlantCarProvided"));
            }
        }
        public bool MobilePhoneProvided
        {
            get { return mobilePhoneProvided; }
            set
            {
                mobilePhoneProvided = value;
                MobilePhoneControlEnable = value;
                if (!MobilePhoneProvided)
                { SelectedMobileAsset = null; }
                OnPropertyChanged(new PropertyChangedEventArgs("MobilePhoneProvided"));
            }
        }
        public bool SimCardProvided
        {
            get { return simCardProvided; }
            set
            {
                simCardProvided = value;
                SimCardControlEnable = value;
                if (!SimCardProvided)
                { SelectedSimCardAsset = null; }
                OnPropertyChanged(new PropertyChangedEventArgs("SimCardProvided"));
            }
        }
        public bool MoneyDeliveredAtOrigin
        {
            get { return moneyDeliveredAtOrigin; }
            set { moneyDeliveredAtOrigin = value; OnPropertyChanged(new PropertyChangedEventArgs("MoneyDeliveredAtOrigin")); }
        }
        public bool MoneyDeliveredAtDestination
        {
            get { return moneyDeliveredAtDestination; }
            set { moneyDeliveredAtDestination = value; OnPropertyChanged(new PropertyChangedEventArgs("MoneyDeliveredAtDestination")); }
        }
        public LookupValue SelectedDepartureTransport
        {
            get { return selectedDepartureTransport; }
            set
            {
                selectedDepartureTransport = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDepartureTransport"));
            }
        }
        public List<LookupValue> WorkShiftList
        {
            get { return workShiftList; }
            set
            {
                workShiftList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkShiftList"));
            }
        }
        public LookupValue SelectedWorkShift
        {
            get { return selectedWorkShift; }
            set { selectedWorkShift = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkShift")); }
        }
        public List<Destination> SupplierList
        {
            get { return supplierList; }
            set { supplierList = value; OnPropertyChanged(new PropertyChangedEventArgs("SupplierList")); }
        }
        public ObservableCollection<TripAssets> CarAssetsList
        {
            get { return carAssetList; }
            set { carAssetList = value; OnPropertyChanged(new PropertyChangedEventArgs("CarAssetsList")); }
        }
        public ObservableCollection<TripAssets> SimCardAssetList
        {
            get { return simCardAssetList; }
            set { simCardAssetList = value; OnPropertyChanged(new PropertyChangedEventArgs("SimCardAssetList")); }
        }
        public ObservableCollection<TripAssets> MobilePhoneAssetList
        {
            get { return mobilePhoneAssetList; }
            set { mobilePhoneAssetList = value; OnPropertyChanged(new PropertyChangedEventArgs("MobilePhoneAssetList")); }
        }
        public ObservableCollection<TripAssets> AccommodationAssetList
        {
            get { return accommodationAssetList; }
            set { accommodationAssetList = value; OnPropertyChanged(new PropertyChangedEventArgs("AccommodationAssetList")); }
        }
        public TripAssets SelectedCarAsset
        {
            get { return selectedCarAsset; }
            set { selectedCarAsset = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedCarAsset")); }
        }
        public TripAssets SelectedSimCardAsset
        {
            get { return selectedSimCardAsset; }
            set { selectedSimCardAsset = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedSimCardAsset")); }
        }
        public TripAssets SelectedMobileAsset
        {
            get { return selectedMobileAsset; }
            set { selectedMobileAsset = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedMobileAsset")); }
        }
        public TripAssets SelectedAccommodationAsset
        {
            get { return selectedAccommodationAsset; }
            set
            {
                selectedAccommodationAsset = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAccommodationAsset"));
                if (SelectedAccommodationAsset != null)
                    AccommodationAddress = SelectedAccommodationAsset.Location;
            }
        }
        public bool IsEmdepRoomEnable
        {
            get { return isEmdepRoomEnable; }
            set { isEmdepRoomEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("IsEmdepRoomEnable")); }
        }
        public List<Destination> AllCustomer
        {
            get { return allCustomer; }
            set { allCustomer = value; OnPropertyChanged(new PropertyChangedEventArgs("AllCustomer")); }
        }
        public List<Destination> AllPlantList
        {
            get { return allPlantList; }
            set { allPlantList = value; OnPropertyChanged(new PropertyChangedEventArgs("AllPlantList")); }
        }
        public List<TripStatus> TripRequestStatusList
        {
            get
            {
                return tripStatusList;
            }

            set
            {
                tripStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TripRequestStatusList"));
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
        protected ISaveFileDialogService SaveFileDialogService
        {
            get
            {
                return this.GetService<ISaveFileDialogService>();
            }
        }
        #region [nsatpute][24-09-2024][GOES2-6473]
        public WorkflowStatus WorkflowStatus
        {
            get
            {
                return workflowStatus;
            }

            set
            {
                workflowStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatus"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsControlEnabled"));
                if (value?.IdWorkflowStatus == 95 || value?.IdWorkflowStatus == 96)
                    IsAllControlDisabled = true;
                else
                    IsAllControlDisabled = false;
            }
        }
        public bool IsControlsEnable
        {
            get { return isControlsEnable; }
            set { isControlsEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("IsControlsEnable")); }
        }
        List<WorkflowStatus> originalWorkflowStatusList;
        public List<WorkflowStatus> OriginalWorkflowStatusList
        {
            get
            {
                return originalWorkflowStatusList;
            }

            set
            {
                originalWorkflowStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginalWorkflowStatusList"));
            }
        }
        public List<WorkflowStatus> WorkflowStatusList
        {
            get
            {
                return workflowStatusList;
            }

            set
            {
                workflowStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatusList"));
            }
        }
        public List<WorkflowStatus> WorkflowStatusButtons
        {
            get
            {
                return workflowStatusButtons;
            }

            set
            {
                workflowStatusButtons = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatusButtons"));
            }
        }
        public WorkflowStatus SelectedWorkflowStatusButton
        {
            get
            {
                return selectedWorkflowStatusButton;
            }

            set
            {
                selectedWorkflowStatusButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkflowStatusButton"));
            }
        }
        public List<WorkflowTransition> WorkflowTransitionList
        {
            get
            {
                return workflowTransitionList;
            }

            set
            {
                workflowTransitionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowTransitionList"));
            }
        }
        public bool PlantCarControlEnable
        {
            get { return plantCarControlEnable; }
            set { plantCarControlEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("PlantCarControlEnable")); }
        }
        public bool MobilePhoneControlEnable
        {
            get { return mobilePhoneControlEnable; }
            set { mobilePhoneControlEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("MobilePhoneControlEnable")); }
        }
        public bool SimCardControlEnable
        {
            get { return simCardControlEnable; }
            set { simCardControlEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("SimCardControlEnable")); }
        }
        public bool IsAllControlDisabled
        {
            get { return isAllControlDisabled; }
            set
            {
                isAllControlDisabled = value;
                IsControlsEnable = !value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAllControlDisabled"));
                if (value == true)
                {
                    IsEnableField = false;
                    IsCustomTravelerEnable = false;
                    IsEmployeeTravelerEnable = false;
                    PartnerProvidedRoomEnable = false;
                    OtherRoomEnable = false;
                    IsEmdepRoomEnable = false;
                    PlantCarControlEnable = false;
                    MobilePhoneControlEnable = false;
                    SimCardControlEnable = false;
                }
            }
        }
        #endregion
        #endregion

        #region  [nsatpute][24-09-2024][GEOS2-6486]
        public List<TripAttachment> UpdatedFileList
        {
            get { return updatedFileList; }

            set
            {
                updatedFileList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedFileList"));
            }
        }
        public TripAttachment SelectedAttachment
        {
            get { return selectedAttachment; }
            set { selectedAttachment = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttachment")); }
        }
        // [nsatpute][22-10-2024][GEOS2-6656]
        public Visibility ClearButtonVisibility
        {
            get { return clearButtonVisibility; }
            set { clearButtonVisibility = value; OnPropertyChanged(new PropertyChangedEventArgs("ClearButtonVisibility")); }
        }
        private ObservableCollection<Employee_trips_transfers> transfersList;
        public ObservableCollection<Employee_trips_transfers> TransfersList
        {
            get { return transfersList; }

            set
            {
                transfersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TransfersList"));
            }
        }
        #endregion
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        public string GuidCode { get; set; }
        // [shweta.thube][GEOS2-7989][18-09-2025]
        public ObservableCollection<EmployeeTripsAccommodations> AccommodationsList
        {
            get { return accommodationsList; }

            set
            {
                accommodationsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AccommodationsList"));
            }
        }

        #endregion

        #region ICommand        
        public ICommand DepartmentPopupClosedCommand { get; set; }
        public ICommand TravellerPopupClosedCommand { get; set; }
        public ICommand DestinationPopupClosedCommand { get; set; }
        public ICommand OriginPopupClosedCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand ChangeTypeCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand ClearTravelerCommand { get; set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }

        public ICommand ExportExpenseReportButtonCommand { get; set; }
        public ICommand AddAttachmentFileCommand { get; set; }
        public ICommand OpenWorkflowDiagramCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }
        public ICommand DownloadFileCommand { get; set; }
        public ICommand EditFileCommand { get; set; }
        public ICommand WorkflowButtonClickCommand { get; set; }
        public ICommand StatusCancelButtonCommand { get; set; }
        public ICommand EditTransferCommand { get; set; } //[pramod.misal][GEOS2-7989][17-09-2025]
        public ICommand AddTransferCommand { get; set; } //[pramod.misal][GEOS2-7989][17-09-2025]
        public ICommand EditAccommodationCommand { get; set; } //[pramod.misal][GEOS2-7989][17-09-2025]
        public ICommand AddAccommodationCommand { get; set; } //[pramod.misal][GEOS2-7989][17-09-2025]

        #endregion

        #region Constructor
        // [nsatpute][05-09-2024][GEOS2-5928]
        public AddEditTripsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditTrainingsViewModel ...", category: Category.Info, priority: Priority.Low);

                DialogWidth = SystemParameters.WorkArea.Width - 20;
                DialogHeight = SystemParameters.WorkArea.Height - (SystemParameters.WorkArea.Height < 840 ? 90 : 190);
                //[GEOS2-6760][rdixit][14.01.2025]
                DepartmentPopupClosedCommand = new DelegateCommand<object>(TravellerCommandAction);
                TravellerPopupClosedCommand = new DelegateCommand<object>(TravellerCommandAction);
                DestinationPopupClosedCommand = new DelegateCommand<object>(OriginORDestinationCommandAction);
                OriginPopupClosedCommand = new DelegateCommand<object>(OriginORDestinationCommandAction);
                AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                AddAttachmentFileCommand = new RelayCommand(new Action<object>(AddAttachmentFileCommandAction));
                CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
                ChangeTypeCommand = new RelayCommand(new Action<object>(ChangeTypeCommandAction));
                ClearCommand = new RelayCommand(new Action<object>(ClearCommandAction));
                ClearTravelerCommand = new RelayCommand(new Action<object>(ClearTravelerCommandAction)); // [nsatpute][22-10-2024][GEOS2-6656]
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                ExportExpenseReportButtonCommand = new RelayCommand(ExportExpenseReportButtonCommandAction);
                OpenWorkflowDiagramCommand = new RelayCommand(OpenWorkflowDiagramCommandAction);
                DeleteFileCommand = new DelegateCommand<object>(DeleteFileAction);
                DownloadFileCommand = new RelayCommand(DownloadFileCommandAction);
                EditFileCommand = new DelegateCommand<object>(EditFileCommandAction);
                WorkflowButtonClickCommand = new DelegateCommand<object>(WorkflowButtonClickCommandAction);
                StatusCancelButtonCommand = new DelegateCommand<object>(StatusCancelButtonCommandAction);
                EditTransferCommand = new DelegateCommand<object>(EditTransferCommandAction);//[pramod.misal][GEOS2-7989][17-09-2025]
                AddTransferCommand = new DelegateCommand<object>(AddTransferCommandAction); //[pramod.misal][GEOS2-7989][17-09-2025]             
                EditAccommodationCommand = new DelegateCommand<object>(EditAccommodationCommandAction);//[shweta.thube][GEOS2-7989][18-09-2025]
                AddAccommodationCommand = new DelegateCommand<object>(AddAccommodationCommandAction);//[shweta.thube][GEOS2-7989][18-09-2025]                                                          
                AttachmentFilesToDelete = new List<TripAttachment>();
                UpdatedFileList = new List<TripAttachment>();
                //[rdixit][GEOS2-6979][02.04.2025]
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up =>
                up.IdPermission == 30
                || up.IdPermission == 38
                || up.IdPermission == 143))
                    IsEnableField = true;
                else
                    isEnableField = false;

                Count = 0;
                // ExportExpenseReportButtonCommandVisibility = Visibility.Hidden;
                ExportExpenseReportButtonCommandVisibility = Visibility.Visible;
                GeosApplication.Instance.Logger.Log("Constructor AddEditTrainingsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditTrainingsViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region Methods  
        //[GEOS2-6760][rdixit][14.01.2025]
        private void TravellerCommandAction(object obj)
        {
            try
            {
                if (obj != null && obj is DevExpress.Xpf.Editors.ClosePopupEventArgs)
                {
                    DevExpress.Xpf.Editors.ClosePopupEventArgs arg = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;
                    if (arg.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Normal)
                    {
                        GeosApplication.Instance.Logger.Log("Method TravellerCommandAction()...", category: Category.Info, priority: Priority.Low);
                        FillResponsible();
                        if (ResponsibleList.Any(i => i.EmployeeJobDescription?.IdJobDescription == SelectedTraveller?.IdApprovalResponsibleJD))
                            SelectedResponsible = ResponsibleList.FirstOrDefault(i => i.EmployeeJobDescription?.IdJobDescription == SelectedTraveller?.IdApprovalResponsibleJD);
                        else
                            SelectedResponsible = null;
                        if ((!ResponsibleList.Any(i => i.IdEmployee == SelectedResponsible?.IdEmployee)))
                            SelectedResponsible = null;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method TravellerCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor OriginChangeCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[GEOS2-6760][rdixit][14.01.2025]
        private void OriginORDestinationCommandAction(object obj)
        {
            try
            {
                if (obj != null && obj is DevExpress.Xpf.Editors.ClosePopupEventArgs)
                {
                    DevExpress.Xpf.Editors.ClosePopupEventArgs arg = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;
                    if (arg.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Normal)
                    {
                        GeosApplication.Instance.Logger.Log("Method OriginORDestinationCommandAction()...", category: Category.Info, priority: Priority.Low);
                        FillTravellerList();
                        FillResponsible();

                        if (ResponsibleList != null)
                        {
                            if (ResponsibleList.Any(i => i.EmployeeJobDescription?.IdJobDescription == SelectedTraveller?.IdApprovalResponsibleJD))
                                SelectedResponsible = ResponsibleList.FirstOrDefault(i => i.EmployeeJobDescription?.IdJobDescription == SelectedTraveller?.IdApprovalResponsibleJD);
                            else
                                SelectedResponsible = null;

                            if ((!ResponsibleList.Any(i => i.IdEmployee == SelectedResponsible?.IdEmployee)))
                                SelectedResponsible = null;
                        }
                        else
                            SelectedResponsible = null;

                        if (TravellerList != null)
                        {
                            if (TravellerList.Any(i => i.IdEmployee == SelectedTraveller?.IdEmployee))
                                SelectedTraveller = TravellerList.FirstOrDefault(i => i.IdEmployee == SelectedTraveller?.IdEmployee);
                            else
                                SelectedTraveller = null;
                        }
                        else
                            SelectedTraveller = null;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method OriginORDestinationCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor OriginORDestinationCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportExpenseReportButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportExpenseReportButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
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
                double totalReimbursement = 0;
                double convertedTotalReimbursement = 0;
                double plantCurrencyTotalReimbursement = 0;
                TripExpensesReport tripExpensesReport = new TripExpensesReport();
                //  HrmService = new HrmServiceController("localhost:6699");
                //Servide commited for GEOS2-5793
                //TripExpensesReport tripExpensesReport = (HrmService.GetTripExpensesReport_V2490(IdEmployeeTripNew));
                //pramod.misal GEOS2-5793 24.06.2024
                //[nsatpute][09.10.2025][GEOS2-6367]
                string serverCultureName = HrmService.GetServerCultureInfo();
                CultureInfo serverCulture = CultureInfo.GetCultureInfo(serverCultureName);
                List<TravelExpenses> TravelExpenseList = (HrmService.GetExpensesReportByTrip(IdEmployeeTripNew));
                UniqueIdCompanies = TravelExpenseList?.Select(expense => expense.IdCompany).Distinct().ToList();
                Destination destination = new Destination();

                #region GEOS2-5328
                if (TravelExpenseList?.Count > 0 && UniqueIdCompanies?.Count > 1)
                {

                    ExpenseReportsPlantsView expenseReportsPlantsView = new ExpenseReportsPlantsView();
                    ExpenseReportsPlantsViewModel expenseReportsPlantsViewModel = new ExpenseReportsPlantsViewModel();
                    EventHandler handle = delegate { expenseReportsPlantsView.Close(); };
                    expenseReportsPlantsViewModel.RequestClose += handle;
                    expenseReportsPlantsView.DataContext = expenseReportsPlantsViewModel;
                    expenseReportsPlantsViewModel.Init(UniqueIdCompanies);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    expenseReportsPlantsView.ShowDialog();
                    if (expenseReportsPlantsViewModel.IsPlantSelected == true)
                    {
                        #region GEOS2-5792
                        try
                        {
                            //Shubham[skadam] GEOS2-5792 HRM - Travel Reports currency exchange based on ticket IESD-86619 17 07 2024
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
                            //[Rahul.Gadhave][GEOS2-6085] [Date:07-10-2024]
                            // tripExpensesReport = (HrmService.GetTripExpensesReport_V2540(IdEmployeeTripNew, expenseReportsPlantsViewModel.NewSelectedCompany.IdCurrency));

                            if (SelectedType.IdLookupValue == 2096 || SelectedType.IdLookupValue == 2099 || SelectedType.IdLookupValue == 2100)
                            {
                                if (SelectedDestination.IdCountry != null)
                                {

                                    destination = HrmService.GetIdCurrrencyFromIdCountry_V2570(SelectedDestination.IdCountry);
                                    //[Rahul.Gadhave][GEOS2-6085] [Date:07-10-2024]
                                    //[nsatpute][31.10.2025][GEOS2-6367]
                                    tripExpensesReport = (HrmService.GetTripExpensesReport_V2680(IdEmployeeTripNew, destination.IdCurrency));
                                }
                            }
                            //For Origin
                            if (SelectedType.IdLookupValue == 2097 || SelectedType.IdLookupValue == 2098)
                            {
                                if (SelectedOrigin.IdCurrency != 0)
                                {
                                    //[Rahul.Gadhave][GEOS2-6085] [Date:07-10-2024]
                                    //[nsatpute][31.10.2025][GEOS2-6367]
                                    tripExpensesReport = (HrmService.GetTripExpensesReport_V2680(IdEmployeeTripNew, SelectedOrigin.IdCurrency));
                                    destination = HrmService.GetIdCurrrencyFromIdCountry_V2570(SelectedOrigin.IdCountry);
                                }
                            }
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Error in Constructor ExportExpenseReportButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                        #endregion

                        SelectedCompany = expenseReportsPlantsViewModel.NewSelectedCompany;
                        tripExpensesReport.TravelLiquidation = tripExpensesReport.TravelLiquidation.Where(e => e.IdCompany == SelectedCompany.IdCompany).ToList(); //[GEOS2-5794][rdixit][16.07.2024]
                    }
                    else
                    {
                        return;
                    }
                }
                else if (TravelExpenseList?.Count > 0 && UniqueIdCompanies?.Count == 1)
                {
                    //tripExpensesReport = (HrmService.GetTripExpensesReport_V2530(IdEmployeeTripNew));
                    //tripExpensesReport = (HrmService.GetTripExpensesReport_V2540(IdEmployeeTripNew, 0));
                    //[rgadhave][GEOS2-6385][27-09-2024]
                    // tripExpensesReport = (HrmService.GetTripExpensesReport_V2540(IdEmployeeTripNew, expenseReportsPlantsViewModel.NewSelectedCompany.IdCurrency));
                    if (SelectedType.IdLookupValue == 2096 || SelectedType.IdLookupValue == 2099 || SelectedType.IdLookupValue == 2100)
                    {
                        if (SelectedDestination.IdCountry != null)
                        {

                            destination = HrmService.GetIdCurrrencyFromIdCountry_V2570(SelectedDestination.IdCountry);
                            //[Rahul.Gadhave][GEOS2-6085] [Date:07-10-2024]
                            tripExpensesReport = (HrmService.GetTripExpensesReport_V2680(IdEmployeeTripNew, destination.IdCurrency));
                        }
                    }
                    //For Origin
                    if (SelectedType.IdLookupValue == 2097 || SelectedType.IdLookupValue == 2098)
                    {
                        if (SelectedOrigin.IdCurrency != 0)
                        {

                            //[Rahul.Gadhave][GEOS2-6085] [Date:07-10-2024]
                            tripExpensesReport = (HrmService.GetTripExpensesReport_V2680(IdEmployeeTripNew, SelectedOrigin.IdCurrency));
                            destination = HrmService.GetIdCurrrencyFromIdCountry_V2570(SelectedOrigin.IdCountry);

                        }
                    }
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EmployeeLinkedTripNotFound").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                EmployeeTripExpenseReport empTripExpenseReport = new EmployeeTripExpenseReport();
                ClearExportExpenseReportButtonCommandAction(empTripExpenseReport);
                empTripExpenseReport.xrTravelExpenseReportHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 14, System.Drawing.FontStyle.Bold);
                empTripExpenseReport.xrTravelExpenseReportHeaderLabel.Text = Application.Current.Resources["TravelExpensesReport"].ToString();
                empTripExpenseReport.xrTravelExpenseReportHeaderLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                try
                {
                    Bitmap BitmapimgLogo = new Bitmap(Emdep.Geos.Modules.Hrm.Properties.Resources.Emdep_logo_mini);
                    empTripExpenseReport.xrEmdepTestboardPicture.Image = BitmapimgLogo;
                    empTripExpenseReport.xrEmdepTestboardPicture.Height = BitmapimgLogo.Height;
                    empTripExpenseReport.xrEmdepTestboardPicture.WidthF = BitmapimgLogo.Width;
                    //TripExpenseReportImgLogo
                    Bitmap imgLogo = new Bitmap(Emdep.Geos.Modules.Hrm.Properties.Resources.TripExpenseReportLogo);
                    empTripExpenseReport.xrPictureBox1.Image = imgLogo;
                    empTripExpenseReport.xrPictureBox1.Height = imgLogo.Height;
                    empTripExpenseReport.xrPictureBox1.WidthF = imgLogo.Width;
                    empTripExpenseReport.xrEmdepSiteLabel.Text = "www.emdep.com";
                    empTripExpenseReport.xrFooterDateTimeLabel.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Error in Constructor ExportExpenseReportButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

                try
                {
                    if (tripExpensesReport.TravelLiquidation.Count() > 1)
                    {
                        // [nsatpute][01-10-2024][GEOS2-6367]
                        #region EmployeeTripTravelLiquidationDynamicCurrencyReport
                        EmployeeTripTravelLiquidationDynamicCurrencyReport rptDynamicCurrency = new EmployeeTripTravelLiquidationDynamicCurrencyReport();
                        rptDynamicCurrency.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 14, System.Drawing.FontStyle.Bold);
                        empTripExpenseReport.xrTravelLiquidationSubreport.ReportSource = rptDynamicCurrency;
                        Font regularNine = new Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        Font boldNine = new Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                        XRTable tblDynamicCurrencies = rptDynamicCurrency.tblCurrencyDetails;


                        foreach (TripExpensesReportTravelLiquidation liquidation in tripExpensesReport.TravelLiquidation)
                        {

                            #region addColumns
                            foreach (XRTableRow row in tblDynamicCurrencies.Rows)
                            {
                                while (row.Cells.Count < 2)
                                {
                                    row.Cells.Add(new XRTableCell { Text = string.Empty, WidthF = 100f });
                                }
                                XRTableCell newCell = new XRTableCell
                                {
                                    WidthF = 100f,
                                    Borders = BorderSide.All,
                                    BorderWidth = 1f,
                                    BorderColor = System.Drawing.Color.Black
                                };
                                List<XRTableCell> cells = new List<XRTableCell>();

                                cells.Add(row.Cells[0]);
                                cells.Add(row.Cells[1]);

                                cells.Add(newCell);

                                for (int i = 2; i < row.Cells.Count; i++)
                                {
                                    cells.Add(row.Cells[i]);
                                }
                                row.Cells.Clear();
                                row.Cells.AddRange(cells.ToArray());
                            }
                            #endregion
                        }

                        #region AddData
                        int colIndex = 2;
                        DevExpress.XtraPrinting.TextAlignment centerAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                        DevExpress.XtraPrinting.TextAlignment rightAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                        //[nsatpute][09.10.2025][GEOS2-6367]
                        for (int i = 0; i < tripExpensesReport.TravelLiquidation.Count(); i++)
                        {

                            tblDynamicCurrencies.Rows[0].Cells[colIndex].Text = tripExpensesReport.TravelLiquidation[i].CurrencyName;
                            tblDynamicCurrencies.Rows[1].Cells[colIndex].Text = (tripExpensesReport.TravelLiquidation[i].CashAdvanced).ToString("n2", serverCulture);
                            // tblDynamicCurrencies.Rows[2].Cells[colIndex].Text = (tripExpensesReport.TravelLiquidation[i].TotalExpense).ToString("n2", serverCulture);
                            // tblDynamicCurrencies.Rows[3].Cells[colIndex].Text = (tripExpensesReport.TravelLiquidation[i].TotalCash).ToString("n2", serverCulture);
                            // tblDynamicCurrencies.Rows[4].Cells[colIndex].Text = (tripExpensesReport.TravelLiquidation[i].CompanyCard).ToString("n2", serverCulture);
                            // tblDynamicCurrencies.Rows[5].Cells[colIndex].Text = (tripExpensesReport.TravelLiquidation[i].PersonalCredit).ToString("n2", serverCulture);
                            // tblDynamicCurrencies.Rows[6].Cells[colIndex].Text = (tripExpensesReport.TravelLiquidation[i].Companyvoucher).ToString("n2", serverCulture);
                            // tblDynamicCurrencies.Rows[7].Cells[colIndex].Text = (tripExpensesReport.TravelLiquidation[i].BankTransfer).ToString("n2", serverCulture);
                            // tblDynamicCurrencies.Rows[8].Cells[colIndex].Text = (tripExpensesReport.TravelLiquidation[i].PaidByOthers).ToString("n2", serverCulture);

                            //PaidByOthers
                            double totalCash = 0;
                            double companyCard = 0;
                            double personalCredit = 0;
                            double companyvoucher = 0;
                            double bankTransfer = 0;
                            double paidByOthers = 0;

                            /*
                             IdLookupValue	Value
                             1538	Cash
                             1539	Personal credit/debit card
                             1548	Personal debit card
                             1549	Bank Transfer
                             1624	Company credit card
                             1714	Company voucher
                             */
                            foreach (TripTravelExpenseReport exp in tripExpensesReport.TravelExpenses)
                            {
                                if (exp.TicketCurrency == tripExpensesReport.TravelLiquidation[i].CurrencyName)
                                {
                                    if (exp.IdPayment == 1538 && exp.PaymentMethod != "Paid by Other")
                                    {
                                        totalCash = totalCash + double.Parse(exp.TotalTicketAmount, NumberStyles.Any, serverCulture); ;
                                    }
                                    if ((exp.IdPayment == 1539 || exp.IdPayment == 1548) && exp.PaymentMethod != "Paid by Other")
                                    {
                                        personalCredit = personalCredit + double.Parse(exp.TotalTicketAmount, serverCulture); ;
                                    }
                                    if (exp.IdPayment == 1549 && exp.PaymentMethod != "Paid by Other")
                                    {
                                        bankTransfer = bankTransfer + double.Parse(exp.TotalTicketAmount, NumberStyles.Any, serverCulture); ;
                                    }
                                    if (exp.IdPayment == 1624 && exp.PaymentMethod != "Paid by Other")
                                    {
                                        companyCard = companyCard + double.Parse(exp.TotalTicketAmount, NumberStyles.Any, serverCulture); ;
                                    }
                                    if (exp.IdPayment == 1714 && exp.PaymentMethod != "Paid by Other")
                                    {
                                        companyvoucher = companyvoucher + double.Parse(exp.TotalTicketAmount, NumberStyles.Any, serverCulture); ;
                                    }
                                    if (exp.PaymentMethod == "Paid by Other")
                                    {
                                        paidByOthers = paidByOthers + double.Parse(exp.TotalTicketAmount, NumberStyles.Any, serverCulture); ;
                                    }
                                }
                            }
                            tblDynamicCurrencies.Rows[2].Cells[colIndex].Text = (totalCash + companyCard + personalCredit + companyvoucher + bankTransfer + paidByOthers).ToString("n2", serverCulture); //TotalExpense
                            tblDynamicCurrencies.Rows[3].Cells[colIndex].Text = (totalCash).ToString("n2", serverCulture); //TotalCash
                            tblDynamicCurrencies.Rows[4].Cells[colIndex].Text = (companyCard).ToString("n2", serverCulture); //CompanyCard
                            tblDynamicCurrencies.Rows[5].Cells[colIndex].Text = (personalCredit).ToString("n2", serverCulture); // PersonalCredit
                            tblDynamicCurrencies.Rows[6].Cells[colIndex].Text = (companyvoucher).ToString("n2", serverCulture); // Companyvoucher
                            tblDynamicCurrencies.Rows[7].Cells[colIndex].Text = (bankTransfer).ToString("n2", serverCulture);  // BankTransfer
                            tblDynamicCurrencies.Rows[8].Cells[colIndex].Text = (paidByOthers).ToString("n2", serverCulture); // PaidByOthers


                            tblDynamicCurrencies.Rows[9].Cells[colIndex].Text = Math.Round((tripExpensesReport.TravelLiquidation[i].CashAdvanced -
                                 totalCash - personalCredit - bankTransfer), 2, MidpointRounding.AwayFromZero).ToString("n2", serverCulture);



                            tblDynamicCurrencies.Rows[0].Cells[colIndex].TextAlignment = centerAlignment;
                            tblDynamicCurrencies.Rows[1].Cells[colIndex].TextAlignment = rightAlignment;
                            tblDynamicCurrencies.Rows[2].Cells[colIndex].TextAlignment = rightAlignment;
                            tblDynamicCurrencies.Rows[3].Cells[colIndex].TextAlignment = rightAlignment;
                            tblDynamicCurrencies.Rows[4].Cells[colIndex].TextAlignment = rightAlignment;
                            tblDynamicCurrencies.Rows[5].Cells[colIndex].TextAlignment = rightAlignment;
                            tblDynamicCurrencies.Rows[6].Cells[colIndex].TextAlignment = rightAlignment;
                            tblDynamicCurrencies.Rows[7].Cells[colIndex].TextAlignment = rightAlignment;
                            tblDynamicCurrencies.Rows[8].Cells[colIndex].TextAlignment = rightAlignment;
                            tblDynamicCurrencies.Rows[9].Cells[colIndex].TextAlignment = rightAlignment;
                            colIndex++;
                        }

                        double totalCashConverted = 0;
                        double companyCardConverted = 0;
                        double personalCreditConverted = 0;
                        double companyvoucherConverted = 0;
                        double bankTransferConverted = 0;
                        double paidByOthersConverted = 0;
                        /*
                        1540	Dinner
                        1541	Lunch
                        1554	Breakfast
                        1555	Other Meal
                         */
                        tripExpensesReport.MealBudget.MealExpense = 0;
                        foreach (TripTravelExpenseReport exp in tripExpensesReport.TravelExpenses)
                        {
                            if (exp.IdType == 1540 || exp.IdType == 1541 || exp.IdType == 1554 || exp.IdType == 1555)
                            {
                                tripExpensesReport.MealBudget.MealExpense += double.Parse(exp.TotalCountedAmountCurrencyPlant, serverCulture);
                            }
                            if (exp.IdPayment == 1538 && exp.PaymentMethod != "Paid by Other")
                            {
                                totalCashConverted = totalCashConverted + (double.Parse(exp.TotalTicketAmount, NumberStyles.Any, serverCulture) / double.Parse(exp.ConversionRate, NumberStyles.Any, serverCulture));
                            }
                            if ((exp.IdPayment == 1539 || exp.IdPayment == 1548) && exp.PaymentMethod != "Paid by Other")
                            {
                                personalCreditConverted = personalCreditConverted + (double.Parse(exp.TotalTicketAmount, serverCulture) / double.Parse(exp.ConversionRate, NumberStyles.Any, serverCulture));
                            }
                            if (exp.IdPayment == 1549 && exp.PaymentMethod != "Paid by Other")
                            {
                                bankTransferConverted = bankTransferConverted + (double.Parse(exp.TotalTicketAmount, NumberStyles.Any, serverCulture) / double.Parse(exp.ConversionRate, NumberStyles.Any, serverCulture));
                            }
                            if (exp.IdPayment == 1624 && exp.PaymentMethod != "Paid by Other")
                            {
                                companyCardConverted = companyCardConverted + (double.Parse(exp.TotalTicketAmount, NumberStyles.Any, serverCulture) / double.Parse(exp.ConversionRate, NumberStyles.Any, serverCulture));
                            }
                            if (exp.IdPayment == 1714 && exp.PaymentMethod != "Paid by Other")
                            {
                                companyvoucherConverted = companyvoucherConverted + (double.Parse(exp.TotalTicketAmount, NumberStyles.Any, serverCulture) / double.Parse(exp.ConversionRate, NumberStyles.Any, serverCulture));
                            }
                            if (exp.PaymentMethod == "Paid by Other")
                            {
                                paidByOthersConverted = paidByOthersConverted + (double.Parse(exp.TotalTicketAmount, NumberStyles.Any, serverCulture) / double.Parse(exp.ConversionRate, NumberStyles.Any, serverCulture));
                            }
                        }


                        int lastColumn = tblDynamicCurrencies.Rows[0].Cells.Count - 1;
                        tblDynamicCurrencies.Rows[1].Cells[lastColumn].Text = tripExpensesReport.TravelLiquidation.Sum(x => x.ConvertedCashAdvanced).ToString("n2", serverCulture);
                        tblDynamicCurrencies.Rows[2].Cells[lastColumn].Text = (totalCashConverted + companyCardConverted + personalCreditConverted + companyvoucherConverted + bankTransferConverted + paidByOthersConverted).ToString("n2", serverCulture);
                        tblDynamicCurrencies.Rows[3].Cells[lastColumn].Text = totalCashConverted.ToString("n2", serverCulture);
                        tblDynamicCurrencies.Rows[4].Cells[lastColumn].Text = companyCardConverted.ToString("n2", serverCulture);
                        tblDynamicCurrencies.Rows[5].Cells[lastColumn].Text = personalCreditConverted.ToString("n2", serverCulture);
                        tblDynamicCurrencies.Rows[6].Cells[lastColumn].Text = companyvoucherConverted.ToString("n2", serverCulture);
                        tblDynamicCurrencies.Rows[7].Cells[lastColumn].Text = bankTransferConverted.ToString("n2", serverCulture);
                        tblDynamicCurrencies.Rows[8].Cells[lastColumn].Text = paidByOthersConverted.ToString("n2", serverCulture);
                        plantCurrencyTotalReimbursement = Math.Round(
                           tripExpensesReport.TravelLiquidation.Sum(s => s.ConvertedCashAdvanced) -
                           totalCashConverted - personalCreditConverted - bankTransferConverted
                           , 2, MidpointRounding.AwayFromZero);
                        tblDynamicCurrencies.Rows[9].Cells[lastColumn].Text = plantCurrencyTotalReimbursement.ToString("n2", serverCulture);


                        tblDynamicCurrencies.Rows[0].Cells[lastColumn].TextAlignment = centerAlignment;
                        tblDynamicCurrencies.Rows[1].Cells[lastColumn].TextAlignment = rightAlignment;
                        tblDynamicCurrencies.Rows[2].Cells[lastColumn].TextAlignment = rightAlignment;
                        tblDynamicCurrencies.Rows[3].Cells[lastColumn].TextAlignment = rightAlignment;
                        tblDynamicCurrencies.Rows[4].Cells[lastColumn].TextAlignment = rightAlignment;
                        tblDynamicCurrencies.Rows[5].Cells[lastColumn].TextAlignment = rightAlignment;
                        tblDynamicCurrencies.Rows[6].Cells[lastColumn].TextAlignment = rightAlignment;
                        tblDynamicCurrencies.Rows[7].Cells[lastColumn].TextAlignment = rightAlignment;
                        tblDynamicCurrencies.Rows[8].Cells[lastColumn].TextAlignment = rightAlignment;
                        tblDynamicCurrencies.Rows[9].Cells[lastColumn].TextAlignment = rightAlignment;
                        #endregion

                        #region ApplyRowStyles

                        for (int i = 0; i < tblDynamicCurrencies.Rows.Count; i++)
                        {
                            for (int j = 0; j < tblDynamicCurrencies.Rows[i].Cells.Count; j++)
                            {
                                XRTableCell cell = tblDynamicCurrencies.Rows[i].Cells[j];
                                if (i == 0 && j == 1)
                                {
                                    cell.Borders = DevExpress.XtraPrinting.BorderSide.Right;
                                    cell.BorderWidth = 1f;
                                    cell.BorderColor = System.Drawing.Color.Black;
                                }
                                if (i == 0 && (j == 0 || j == 1))
                                    continue;

                                if (i == 0 || i == 2)
                                    cell.Font = boldNine;
                                else
                                    cell.Font = regularNine;

                                if (i == 1 || i == 2 || i == 9)
                                    cell.BackColor = System.Drawing.Color.LightGray;
                                else
                                    cell.BackColor = System.Drawing.Color.Transparent;

                                cell.Borders = DevExpress.XtraPrinting.BorderSide.All;
                                cell.BorderWidth = 1f;
                                cell.BorderColor = System.Drawing.Color.Black;
                            }
                        }
                        #endregion
                        int totalColCount = tblDynamicCurrencies.Rows[0].Cells.Count;
                        float colWidth = (tblDynamicCurrencies.WidthF - 41.54F - 234.47F) / (totalColCount - 2);


                        #region RearrangeColumnWidth
                        if (tblDynamicCurrencies.Rows.Count > 0)
                        {
                            for (int rowIndex = 0; rowIndex < tblDynamicCurrencies.Rows.Count; rowIndex++)
                            {
                                for (int colInd = 0; colInd < tblDynamicCurrencies.Rows[0].Cells.Count; colInd++)
                                {
                                    if (colInd == 0)
                                        tblDynamicCurrencies.Rows[rowIndex].Cells[colInd].WidthF = 41.54F;
                                    else if (colInd == 1)
                                        tblDynamicCurrencies.Rows[rowIndex].Cells[colInd].WidthF = 234.47F;
                                    else
                                        tblDynamicCurrencies.Rows[rowIndex].Cells[colInd].WidthF = colWidth;
                                }
                            }
                        }
                        #endregion

                        #endregion
                    }
                    #region EmployeeTripTravelLiquidationSingleCurrencyReport
                    else
                    {
                        EmployeeTripTravelLiquidationSingleCurrencyReport employeeTripTravelLiquidationSingleCurrencyReport = new EmployeeTripTravelLiquidationSingleCurrencyReport();
                        employeeTripTravelLiquidationSingleCurrencyReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 14, System.Drawing.FontStyle.Bold);
                        empTripExpenseReport.xrTravelLiquidationSubreport.ReportSource = employeeTripTravelLiquidationSingleCurrencyReport;

                        //Travel Liquidation Header
                        employeeTripTravelLiquidationSingleCurrencyReport.xrARSTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrARSTableCellHeader.Text = "";
                        employeeTripTravelLiquidationSingleCurrencyReport.xrTotalPlantCurrencyTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);

                        //(a)Cash Advance
                        employeeTripTravelLiquidationSingleCurrencyReport.xraTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCashAdvanceTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCashAdvanceARSTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCashAdvanceTPCTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                        //(b)Total Expenses
                        //employeeTripTravelLiquidationSingleCurrencyReport.xrbTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrbTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                        //employeeTripTravelLiquidationSingleCurrencyReport.xrTotalExpensesTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrTotalExpensesTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                        //employeeTripTravelLiquidationSingleCurrencyReport.xrTotalExpensesARSTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrTotalExpensesARSTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                        //employeeTripTravelLiquidationSingleCurrencyReport.xrTotalExpensesTPCTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrTotalExpensesTPCTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);


                        //(C)Cash
                        employeeTripTravelLiquidationSingleCurrencyReport.xrcTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCashTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCashARSTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCashTPCTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);


                        //(d)Company Credit Card
                        employeeTripTravelLiquidationSingleCurrencyReport.xrdTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCompanyCreditCardTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCompanyCreditCardARSTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCompanyCreditCardTPCTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);


                        //(e)Personal debit/credit card
                        employeeTripTravelLiquidationSingleCurrencyReport.xreTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrPersonalDebitCreditCardTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrPersonalDebitCreditCardARSTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrPersonalDebitCreditCardTPCTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);


                        //(f)Company Voucher
                        employeeTripTravelLiquidationSingleCurrencyReport.xrfTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCompanyVoucherTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCompanyVoucherARSTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCompanyVoucherTPCTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);


                        //(g)Bank Transfer
                        employeeTripTravelLiquidationSingleCurrencyReport.xrgTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrBankTransferTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrBankTransferARSTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrBankTransferTPCTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                        //(h)Paid by Others
                        employeeTripTravelLiquidationSingleCurrencyReport.xrhTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrPaidByOthersTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrPaidByOthersARSTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrPaidByOthersTPCTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                        #region TravelLiquidation
                        if (tripExpensesReport.TravelLiquidation != null)
                        {
                            //CurrencyName
                            employeeTripTravelLiquidationSingleCurrencyReport.xrARSTableCellHeader.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().CurrencyName;
                            //CashAdvanced
                            employeeTripTravelLiquidationSingleCurrencyReport.xrCashAdvanceARSTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().CashAdvanced.ToString("n2", serverCulture);
                            //TotalExpense
                            employeeTripTravelLiquidationSingleCurrencyReport.xrTotalExpensesARSTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().TotalExpense.ToString("n2", serverCulture);
                            //TotalCash
                            employeeTripTravelLiquidationSingleCurrencyReport.xrCashARSTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().TotalCash.ToString("n2", serverCulture);
                            //CompanyCard
                            employeeTripTravelLiquidationSingleCurrencyReport.xrCompanyCreditCardARSTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().CompanyCard.ToString("n2", serverCulture);
                            //PersonalCredit
                            employeeTripTravelLiquidationSingleCurrencyReport.xrPersonalDebitCreditCardARSTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().PersonalCredit.ToString("n2", serverCulture);
                            //Companyvoucher
                            employeeTripTravelLiquidationSingleCurrencyReport.xrCompanyVoucherARSTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().Companyvoucher.ToString("n2", serverCulture);
                            //BankTransfer
                            employeeTripTravelLiquidationSingleCurrencyReport.xrBankTransferARSTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().BankTransfer.ToString("n2", serverCulture);
                            //PaidByOthers
                            employeeTripTravelLiquidationSingleCurrencyReport.xrPaidByOthersARSTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().PaidByOthers.ToString("n2", serverCulture);


                            //CurrencyName
                            employeeTripTravelLiquidationSingleCurrencyReport.xrARSTableCellHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                            //CashAdvanced
                            employeeTripTravelLiquidationSingleCurrencyReport.xrCashAdvanceARSTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                            //TotalExpense
                            employeeTripTravelLiquidationSingleCurrencyReport.xrTotalExpensesARSTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                            //TotalCash
                            employeeTripTravelLiquidationSingleCurrencyReport.xrCashARSTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                            //CompanyCard
                            employeeTripTravelLiquidationSingleCurrencyReport.xrCompanyCreditCardARSTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                            //PersonalCredit
                            employeeTripTravelLiquidationSingleCurrencyReport.xrPersonalDebitCreditCardARSTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                            //Companyvoucher
                            employeeTripTravelLiquidationSingleCurrencyReport.xrCompanyVoucherARSTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                            //BankTransfer
                            employeeTripTravelLiquidationSingleCurrencyReport.xrBankTransferARSTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                            //PaidByOthers
                            employeeTripTravelLiquidationSingleCurrencyReport.xrPaidByOthersARSTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

                        }
                        #endregion
                        //PlantCurrencyCashAdvanced
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCashAdvanceTPCTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().PlantCurrencyCashAdvanced.ToString("n2", serverCulture);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCashAdvanceTPCTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                        //PlantCurrencyTotalExpense
                        employeeTripTravelLiquidationSingleCurrencyReport.xrTotalExpensesTPCTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().PlantCurrencyTotalExpense.ToString("n2", serverCulture);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrTotalExpensesTPCTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                        //PlantCurrencyTotalCash
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCashTPCTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().PlantCurrencyTotalCash.ToString("n2", serverCulture);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCashTPCTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                        //PlantCurrencyCompanyCard
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCompanyCreditCardTPCTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().PlantCurrencyCompanyCard.ToString("n2", serverCulture);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCompanyCreditCardTPCTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                        //PlantCurrencyPersonalCredit
                        employeeTripTravelLiquidationSingleCurrencyReport.xrPersonalDebitCreditCardTPCTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().PlantCurrencyPersonalCredit.ToString("n2", serverCulture);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrPersonalDebitCreditCardTPCTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                        //PlantCurrencyCompanyvoucher
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCompanyVoucherTPCTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().PlantCurrencyCompanyvoucher.ToString("n2", serverCulture);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrCompanyVoucherTPCTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                        //PlantCurrencyBankTransfer
                        employeeTripTravelLiquidationSingleCurrencyReport.xrBankTransferTPCTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().PlantCurrencyBankTransfer.ToString("n2", serverCulture);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrBankTransferTPCTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

                        employeeTripTravelLiquidationSingleCurrencyReport.xrPaidByOthersTPCTableCellText.Text = tripExpensesReport.TravelLiquidation.FirstOrDefault().PlantCurrencyPaidByOthers.ToString("n2", serverCulture);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrPaidByOthersTPCTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

                        //(i)Total reimbursement
                        employeeTripTravelLiquidationSingleCurrencyReport.xriTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrTotalReimbursementTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrTotalReimbursementARSTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrTotalReimbursementTPCTableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                        totalReimbursement = Math.Round(
                            tripExpensesReport.TravelLiquidation.FirstOrDefault().CashAdvanced -
                          tripExpensesReport.TravelLiquidation.FirstOrDefault().TotalCash -
                          tripExpensesReport.TravelLiquidation.FirstOrDefault().PersonalCredit -
                          tripExpensesReport.TravelLiquidation.FirstOrDefault().BankTransfer
                             , 2, MidpointRounding.AwayFromZero);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrTotalReimbursementARSTableCellText.Text = totalReimbursement.ToString("n2");
                        employeeTripTravelLiquidationSingleCurrencyReport.xrTotalReimbursementARSTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;


                        convertedTotalReimbursement = Math.Round(
                            tripExpensesReport.TravelLiquidation.FirstOrDefault().ConvertedCashAdvanced -
                          tripExpensesReport.TravelLiquidation.FirstOrDefault().ConvertedTotalCash -
                          tripExpensesReport.TravelLiquidation.FirstOrDefault().ConvertedPersonalCredit -
                          tripExpensesReport.TravelLiquidation.FirstOrDefault().ConvertedBankTransfer
                             , 2, MidpointRounding.AwayFromZero);


                        plantCurrencyTotalReimbursement = Math.Round(
                            tripExpensesReport.TravelLiquidation.FirstOrDefault().PlantCurrencyCashAdvanced -
                            tripExpensesReport.TravelLiquidation.FirstOrDefault().PlantCurrencyTotalCash -
                            tripExpensesReport.TravelLiquidation.FirstOrDefault().PlantCurrencyPersonalCredit -
                            tripExpensesReport.TravelLiquidation.FirstOrDefault().PlantCurrencyBankTransfer
                            , 2, MidpointRounding.AwayFromZero);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrTotalReimbursementTPCTableCellText.Text = plantCurrencyTotalReimbursement.ToString("n2", serverCulture);
                        employeeTripTravelLiquidationSingleCurrencyReport.xrTotalReimbursementTPCTableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Error in Constructor ExportExpenseReportButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }


                #region
                //Header
                empTripExpenseReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                //CompanyPlant
                empTripExpenseReport.xrExpenseReportPlantRichText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                //empTripExpenseReport.xrExpenseReportPlantRichText.Text = OriginList.FirstOrDefault(f => f.IdCompany == EditEmployeeTrips.IdOriginPlant).Alias;
                if (SelectedCompany != null)
                    empTripExpenseReport.xrExpenseReportPlantRichText.Text = SelectedCompany.Alias; //[GEOS2-5794][rdixit][16.07.2024]
                else
                    empTripExpenseReport.xrExpenseReportPlantRichText.Text = OriginList?.FirstOrDefault(f => f.IdCompany == EditEmployeeTrips.IdOriginPlant)?.Alias;
                //pramod.misal GEOS2-5793 21.06.2024
                //if (UniqueIdCompanies.Count > 1)
                //{
                //    empTripExpenseReport.xrExpenseReportPlantRichText.Text = SelectedCompany.Alias;
                //}
                //else
                //{
                //    empTripExpenseReport.xrExpenseReportPlantRichText.Text = tripExpensesReport.CompanyPlant;
                //}
                empTripExpenseReport.xrExpenseReportPlantRichText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

                //Employee Details
                //Company
                empTripExpenseReport.xrCompanyHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                empTripExpenseReport.xrCompanyTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrCompanyTextLabel.Text = tripExpensesReport.Company;
                //empTripExpenseReport.xrExpenseReportPlantRichText.Text = tripExpensesReport.Company;
                //SubmitDate
                empTripExpenseReport.xrSubmitDateHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                empTripExpenseReport.xrSubmitDateTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrSubmitDateTextLabel.Text = tripExpensesReport.SubmitDate.ToString("dd/MM/yyyy");

                //FromDate
                empTripExpenseReport.xrFromHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                empTripExpenseReport.xrFromTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrFromTextLabel.Text = tripExpensesReport.FromDate.ToString("dd/MM/yyyy");

                //EmployeeName
                empTripExpenseReport.xrEmployeeNameHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                empTripExpenseReport.xrEmployeeNameTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrEmployeeNameTextLabel.Text = tripExpensesReport.EmployeeName;

                //ToDate
                empTripExpenseReport.xrToHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                empTripExpenseReport.xrToTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrToTextLabel.Text = tripExpensesReport.ToDate.ToString("dd/MM/yyyy") + " (" + tripExpensesReport.MealBudget.TripDays + " days)";
                //TripTitle
                empTripExpenseReport.xrTripTitleHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                empTripExpenseReport.xrTripTitleTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrTripTitleTextLabel.Text = tripExpensesReport.TripTitle.ToString();
                //TripReason
                empTripExpenseReport.xrTripReasonHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                empTripExpenseReport.xrTripReasonTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrTripReasonTextLabel.Text = tripExpensesReport.TripReason.ToString();

                //Organization
                empTripExpenseReport.xrOrganizationHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                empTripExpenseReport.xrOrganizationTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrOrganizationTextLabel.Text = tripExpensesReport.Organization.ToString();
                //DepartmentName
                empTripExpenseReport.xrDepartmentNameHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                empTripExpenseReport.xrDepartmentNameTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrDepartmentNameTextLabel.Text = tripExpensesReport.DepartmentName.ToString();

                //JobTitle
                empTripExpenseReport.xrJobTitleHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                empTripExpenseReport.xrJobTitleTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrJobTitleTextLabel.Text = tripExpensesReport.JobTitle.ToString();


                //Final Statement
                empTripExpenseReport.xrFinalStatementHeaderLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 10, System.Drawing.FontStyle.Bold);
                empTripExpenseReport.xrFinalStatementTextLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 10, System.Drawing.FontStyle.Bold);
                empTripExpenseReport.xrFinalStatementTextLabel.Text = "";
                if (plantCurrencyTotalReimbursement < 0)
                {
                    empTripExpenseReport.xrFinalStatementTextLabel.Text = "The company must return " + Math.Abs(plantCurrencyTotalReimbursement).ToString("n2", serverCulture) + " "
                        + destination.Name + " to the employee";
                }
                else if (plantCurrencyTotalReimbursement > 0)
                {
                    empTripExpenseReport.xrFinalStatementTextLabel.Text = "The employee must return  " + Math.Abs(plantCurrencyTotalReimbursement).ToString("n2", serverCulture) + " "
                        + destination.Name + " to the company";
                }
                else
                {
                    empTripExpenseReport.xrFinalStatementTextLabel.Text = "";
                }
                //Protocol Analysis
                empTripExpenseReport.xrTripExpenseTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                empTripExpenseReport.xrMealAllowanceTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                //Total Meal Expenses
                empTripExpenseReport.xrTotalMealExpensesTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrTotalMealExpensesTETableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrTotalMealExpensesMATableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                if (tripExpensesReport.MealBudget != null)
                {
                    //Trip Expenses
                    empTripExpenseReport.xrTotalMealExpensesTETableCellText.Text = Math.Round(tripExpensesReport.MealBudget.MealExpense, 2, MidpointRounding.AwayFromZero).ToString("N0", serverCulture);
                    //TotalMeal
                    empTripExpenseReport.xrTotalMealExpensesMATableCellText.Text = Math.Round(tripExpensesReport.MealBudget.Amount * tripExpensesReport.MealBudget.TripDays, 2, MidpointRounding.AwayFromZero).ToString("N0", serverCulture);
                }
                if (tripExpensesReport.MealBudget.MealExpense > (tripExpensesReport.MealBudget.Amount * tripExpensesReport.MealBudget.TripDays))
                {
                    empTripExpenseReport.xrTotalMealExpensesTETableCellText.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    empTripExpenseReport.xrTotalMealExpensesTETableCellText.ForeColor = System.Drawing.Color.Black;
                }
                //TotalMeal
                empTripExpenseReport.xrTotalMealExpensesMATableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                //Trip Expenses
                empTripExpenseReport.xrTotalMealExpensesTETableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

                //Average Meal/Day
                empTripExpenseReport.xrAverageMealDayTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrAverageMealDayTETableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrAverageMealDayMATableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                if (tripExpensesReport.MealBudget != null)
                {
                    // [nsatpute][01-10-2024][GEOS2-6367]
                    double averageDayMeal = Math.Round(tripExpensesReport.MealBudget.MealExpense / tripExpensesReport.MealBudget.TripDays, 2, MidpointRounding.AwayFromZero);
                    if (double.IsNaN(averageDayMeal))
                        averageDayMeal = 0;


                    empTripExpenseReport.xrAverageMealDayTETableCellText.Text = averageDayMeal.ToString("N0", serverCulture);
                    empTripExpenseReport.xrAverageMealDayMATableCellText.Text = tripExpensesReport.MealBudget.Amount.ToString("N0", serverCulture);
                }
                if ((tripExpensesReport.MealBudget.MealExpense / tripExpensesReport.MealBudget.TripDays) > tripExpensesReport.MealBudget.Amount)
                {
                    empTripExpenseReport.xrAverageMealDayTETableCellText.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    empTripExpenseReport.xrAverageMealDayTETableCellText.ForeColor = System.Drawing.Color.Black;
                }
                //Trip Expenses
                empTripExpenseReport.xrAverageMealDayTETableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                //TotalMeal
                empTripExpenseReport.xrAverageMealDayMATableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

                //Average week
                empTripExpenseReport.xrAverageWeekTableCellHeader.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                if (tripExpensesReport.MealBudget != null)
                {
                    empTripExpenseReport.xrAverageWeekTableCellHeader.Text = tripExpensesReport.MealBudget.TripWeek;
                    empTripExpenseReport.xrAverageWeekTableCellHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                }

                empTripExpenseReport.xrAverageWeekTETableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                empTripExpenseReport.xrAverageWeekMATableCellText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                if (tripExpensesReport.MealBudget != null)
                {
                    // [nsatpute][01-10-2024][GEOS2-6367]
                    double averageWeekMat = (Math.Round(tripExpensesReport.MealBudget.Amount * tripExpensesReport.MealBudget.TripDays, 2, MidpointRounding.AwayFromZero) / tripExpensesReport.MealBudget.TripDays);
                    if (double.IsNaN(averageWeekMat))
                        averageWeekMat = 0;

                    // [nsatpute][01-10-2024][GEOS2-6367]
                    double averageWeekTet = Math.Round(tripExpensesReport.MealBudget.MealExpense / tripExpensesReport.MealBudget.TripDays, 2, MidpointRounding.AwayFromZero);
                    if (double.IsNaN(averageWeekTet))
                        averageWeekTet = 0;

                    empTripExpenseReport.xrAverageWeekMATableCellText.Text = averageWeekMat.ToString("N0", serverCulture);
                    empTripExpenseReport.xrAverageWeekTETableCellText.Text = averageWeekTet.ToString("N0", serverCulture);
                }
                if ((tripExpensesReport.MealBudget.MealExpense / tripExpensesReport.MealBudget.TripDays) > ((tripExpensesReport.MealBudget.Amount * tripExpensesReport.MealBudget.TripDays) / tripExpensesReport.MealBudget.TripDays))
                {
                    empTripExpenseReport.xrAverageWeekTETableCellText.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    empTripExpenseReport.xrAverageWeekTETableCellText.ForeColor = System.Drawing.Color.Black;
                }

                empTripExpenseReport.xrAverageWeekMATableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                empTripExpenseReport.xrAverageWeekTETableCellText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

                //TravelExpenses
                //EmployeeTripTravelExpensesReport employeeTripTravelExpensesReport = new EmployeeTripTravelExpensesReport();
                //employeeTripTravelExpensesReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                //employeeTripTravelExpensesReport.xrTable2.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                //Expense Remarks
                EmployeeTripExpenseRemarksNew employeeTripExpenseRemarks = new EmployeeTripExpenseRemarksNew();
                employeeTripExpenseRemarks.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                employeeTripExpenseRemarks.table2.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                //Travel Expense Data
                //employeeTripTravelExpensesReport.objectDataSource1.DataSource = tripExpensesReport.TravelExpenses;
                //empTripExpenseReport.xrTravelExpenseSubreport.ReportSource = employeeTripTravelExpensesReport;

                EmployeeTripTravelExpensesReportNew EmployeeTripTravelExpensesReportNew = new EmployeeTripTravelExpensesReportNew();
                EmployeeTripTravelExpensesReportNew.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 5, System.Drawing.FontStyle.Bold);
                EmployeeTripTravelExpensesReportNew.table2.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 5, System.Drawing.FontStyle.Regular);
                EmployeeTripTravelExpensesReportNew.table1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 5, System.Drawing.FontStyle.Regular);
                EmployeeTripTravelExpensesReportNew.objectDataSource1.DataSource = tripExpensesReport.TravelExpenses;
                empTripExpenseReport.xrTravelExpenseSubreport.ReportSource = EmployeeTripTravelExpensesReportNew;

                if (tripExpensesReport.MealBudget != null)
                {
                    if (tripExpensesReport.TravelLiquidation.FirstOrDefault().PlantCurrency != tripExpensesReport.TravelLiquidation.FirstOrDefault().CurrencyName)
                    {
                        //Total
                        EmployeeTripTravelExpensesReportNew.tableCell19.Text = "Total" + " " + "(" + destination.Name + ")";
                        EmployeeTripTravelExpensesReportNew.tableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                        // Total Attendee
                        EmployeeTripTravelExpensesReportNew.tableCell20.Text = "Total Attendee" + " " + "(" + destination.Name + ")";
                        EmployeeTripTravelExpensesReportNew.tableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;


                    }
                    else
                    {
                        EmployeeTripTravelExpensesReportNew.tableCell19.Text = "Total" + " " + "(" + destination.Name + ")";
                        EmployeeTripTravelExpensesReportNew.tableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                        // Total Attendee
                        EmployeeTripTravelExpensesReportNew.tableCell20.Text = "Total Attendee" + " " + "(" + destination.Name + ")";
                        EmployeeTripTravelExpensesReportNew.tableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    }
                    EmployeeTripTravelExpensesReportNew.tableCell38.Text = tripExpensesReport.TravelExpenses.FirstOrDefault().AttendeeCount;
                    EmployeeTripTravelExpensesReportNew.tableCell38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                }

                #endregion

                //Expense Remarks
                employeeTripExpenseRemarks.objectDataSource1.DataSource = tripExpensesReport.ExpenseRemark;
                empTripExpenseReport.xrExpenseRemarksSubreport.ReportSource = employeeTripExpenseRemarks;
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = empTripExpenseReport;
                empTripExpenseReport.CreateDocument();
                window.Show();
                #endregion

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ExportExpenseReportButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor ExportExpenseReportButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ClearExportExpenseReportButtonCommandAction(EmployeeTripExpenseReport empTripExpenseReport)
        {
            empTripExpenseReport.xrTravelExpenseReportHeaderLabel.Text = "";
            empTripExpenseReport.xrCompanyTextLabel.Text = "";
            empTripExpenseReport.xrSubmitDateTextLabel.Text = "";
            empTripExpenseReport.xrFromTextLabel.Text = "";
            empTripExpenseReport.xrEmployeeNameTextLabel.Text = "";
            empTripExpenseReport.xrToTextLabel.Text = "";
            empTripExpenseReport.xrOrganizationTextLabel.Text = "";
            empTripExpenseReport.xrTripTitleTextLabel.Text = "";
            empTripExpenseReport.xrDepartmentNameTextLabel.Text = "";
            empTripExpenseReport.xrTripReasonTextLabel.Text = "";
            empTripExpenseReport.xrJobTitleTextLabel.Text = "";
            empTripExpenseReport.xrFinalStatementTextLabel.Text = "";
        }
        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (OldSelectedCompany != SelectedOrigin)
            {
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


                TravellerList = new ObservableCollection<Traveller>();
                // [nsatpute][22-10-2024][GEOS2-6656]
                var temp = HrmService.GetActiveEmployeesForTraveller_V2570(SelectedOrigin.IdCompany.ToString(), HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization,
                           HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission);
                TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                OldSelectedCompany = SelectedOrigin;
                SelectedTraveller = null;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }




            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        // [nsatpute][11-09-2024][GEOS2-5929]
        public void EditInit(UInt32 idEmployeeTrip)
        {
            try
            {
                MaximizedElementPosition = MaximizedElementPosition.Right;
                OriginalListAttachment = new List<TripAttachment>();
                TravellerList = new ObservableCollection<Traveller>();
                TransfersList = new ObservableCollection<Employee_trips_transfers>();
                EditEmployeeTrips = new EmployeeTrips();
                IdEmployeeTripNew = idEmployeeTrip;
                // var temp = HrmService.GetEditEmployeeTripDetails_V2600(idEmployeeTrip);//[GEOS2-6760][rdixit][09.01.2025]
                var temp = HrmService.GetEditEmployeeTripDetails_V2650(idEmployeeTrip); // [pallavi.kale][28-05-2025][GEOS2-7941]
                //HrmService = new HrmServiceController("localhost:6699");
                List<Employee_trips_transfers> listFromService = HrmService.HRM_GetArrivalTransferDetails_V2670(idEmployeeTrip);
                TransfersList = new ObservableCollection<Employee_trips_transfers>(listFromService);
                //TransfersList = HrmService.HRM_GetArrivalTransferDetails_V2670(idEmployeeTrip);
                //TransfersList = new ObservableCollection<Employee_trips_transfers>
                //{
                //        transfer
                //};
                //HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                EditEmployeeTrips = temp;
                ListAttachment = new ObservableCollection<TripAttachment>(EditEmployeeTrips.ListAttachment);
                FillAttachmentTypes();
                ListAttachment.ForEach(x => x.TripAttachmentType = AttachmentTypeList.FirstOrDefault(y => y.IdLookupValue == x.IdAttachmentType));
                ListAttachment.ForEach(x => x.AttachmentImage = IconFromExtension(Path.GetExtension(x.FileName)));
                FillType();
                //FillTraveller(EditEmployeeTrips);
                FillDepartment();
                SelectedDepartment = DepartmentList.FirstOrDefault(i => i.IdDepartment == EditEmployeeTrips.IdDepartment);
                FillPropose();
                FillStatus();
                FillMainTransport();
                FillOrigin();
                FillAccommodationType();
                //FillResponsible();
                FillCurrency();
                FillWorkShift();
                FillTransportationType();
                FillTripAssets();
                TripRequestStatusList = new List<TripStatus>(HrmService.GetAllTripStatusWorkflow());
                WorkflowTransitionList = new List<WorkflowTransition>(HrmService.GetAllTripStatusWorkflowTransitions());
                TransitionWorkflowStatus(EditEmployeeTrips);
                EmployeeTrip = (EmployeeTrips)EditEmployeeTrips.Clone();
                //if (WorkflowStatus?.IdWorkflowStatus == 85)
                //    ExportExpenseReportButtonCommandVisibility = Visibility.Hidden;
                //else
                //    ExportExpenseReportButtonCommandVisibility = Visibility.Visible;
                List<LogEntriesByEmployeeTrip> temp1 = HrmService.GetLogEntriesByEmployeeTrip_V2570((UInt32)EditEmployeeTrips.IdEmployeeTrip); // [nsatpute][21-10-2024][GEOS2-5933]
                TripChangeLogList = new List<LogEntriesByEmployeeTrip>(temp1.OrderByDescending(x => x.DateTimeChangeLog));
                OldSelectedCompany = SelectedOrigin;
                FillExistiongTripData(EditEmployeeTrips);
                EnableDisableControls();
                initComplete = true;
                IsOriginEnable = true;
                IsDestinationEnable = true;
                if (ListAttachment?.Count > 0)
                    Attachmentvisiblevisibility = GroupBoxState.Maximized;
                else
                    Attachmentvisiblevisibility = GroupBoxState.Minimized;
                if (ListAttachment?.Count > 0)
                    OriginalListAttachment = ListAttachment.Select(i => (TripAttachment)i.Clone()).ToList();
                var accommodations = HrmService.GetAccommodationsDetails_V2670(idEmployeeTrip);
                AccommodationsList = new ObservableCollection<EmployeeTripsAccommodations>(accommodations);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor EditInit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][11-09-2024][GEOS2-5929]
        public void Init()
        {
            try
            {
                MaximizedElementPosition = MaximizedElementPosition.Right;
                FillCode();
                TravellerList = new ObservableCollection<Traveller>();
                EmployeeTrips temp = new EmployeeTrips();
                CustomTraveler = string.Empty;
                FillAttachmentTypes();
                //FillTraveller(temp);
                FillType();
                FillPropose();
                FillStatus();
                FillMainTransport();
                FillOrigin();
                FillAccommodationType();
                //FillResponsible();
                FillCurrency();
                FillDepartment();
                FillWorkShift();
                FillTransportationType();
                FillTripAssets();
                TripRequestStatusList = new List<TripStatus>(HrmService.GetAllTripStatusWorkflow());
                WorkflowTransitionList = new List<WorkflowTransition>(HrmService.GetAllTripStatusWorkflowTransitions());
                TransitionWorkflowStatus(temp);
                if (isNew)
                {
                    FromDate = DateTime.Now;
                    ToDate = DateTime.Now;
                }
                //if (WorkflowStatus.IdWorkflowStatus == 85)
                //    ExportExpenseReportButtonCommandVisibility = Visibility.Hidden;
                //else
                //    ExportExpenseReportButtonCommandVisibility = Visibility.Visible;
                ArrivalDate = DateTime.Now;
                DepartureDate = DateTime.Now.AddDays(7);
                DurationDays = Convert.ToInt32((DepartureDate - ArrivalDate).Days) + 1;
                ArrivalDateHours = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                DepartureDateHours = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                OldSelectedCompany = SelectedOrigin;
                IsInActive = false;
                //[rdixit][25.09.2024][GEOS2-6476]
                IsCustomTravelerEnable = true;
                if ((SelectedOrigin != null || SelectedDepartment != null))
                    IsEmployeeTravelerEnable = true;
                else
                    IsEmployeeTravelerEnable = false;
                initComplete = true;
                ListAttachment = new ObservableCollection<TripAttachment>();
                if (ListAttachment?.Count > 0)
                    Attachmentvisiblevisibility = GroupBoxState.Maximized;
                else
                    Attachmentvisiblevisibility = GroupBoxState.Minimized;
                IsOriginEnable = false;
                IsDestinationEnable = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ClearCommandAction(object obj)
        {

            MyFilterString = string.Empty;
            SelectedTraveller = new Traveller();
        }
        // [nsatpute][22-10-2024][GEOS2-6656]
        private void ClearTravelerCommandAction(object obj)
        {
            SelectedTraveller = null;
            FillResponsible();
            if (ResponsibleList.Any(i => i.EmployeeJobDescription?.IdJobDescription == SelectedTraveller?.IdApprovalResponsibleJD))
                SelectedResponsible = ResponsibleList.FirstOrDefault(i => i.EmployeeJobDescription?.IdJobDescription == SelectedTraveller?.IdApprovalResponsibleJD);
            else
                SelectedResponsible = null;
            if ((!ResponsibleList.Any(i => i.IdEmployee == SelectedResponsible?.IdEmployee)))
                SelectedResponsible = null;
        }
        private void FillCurrency()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrency()...", category: Category.Info, priority: Priority.Low);
                TripCurrencyList = new List<Currency>(HrmService.GetCurrencyListForTrips_V2480());
                if (IsNew)
                {
                    if (SelectedTraveller?.Currency != null)
                    {
                        SelectedTripCurrency = TripCurrencyList.FirstOrDefault(x => x.IdCurrency == SelectedTraveller.Currency.IdCurrency);
                    }
                    else
                    {
                        SelectedTripCurrency = TripCurrencyList.FirstOrDefault(x => x.IdCurrency == SelectedOrigin?.IdCurrency);
                    }

                }
                GeosApplication.Instance.Logger.Log("Method FillCurrency()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrency() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrency() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCurrency() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillDepartment()//[GEOS2-6760][rdixit][09.01.2025]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDepartment()...", category: Category.Info, priority: Priority.Low);
                DepartmentList = new List<Department>(HrmService.GetAllDepartments());
                GeosApplication.Instance.Logger.Log("Method FillDepartment()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillDepartment() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeTypeCommandAction(object obj)
        {
            try
            {
                //  FillDestination();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeTypeCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][11-09-2024][GEOS2-5929]
        private void FillDestination()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainTransport()...", category: Category.Info, priority: Priority.Low);
                SelectedDestination = null;
                if (SelectedType?.Value == "EMDEP to Plant" || SelectedType?.Value == "Customer to EMDEP" || SelectedType?.Value == "Provider to EMDEP")
                {
                    if (HrmCommon.Instance.UserAuthorizedPlantsList != null)
                    {
                        DestinationList = new ObservableCollection<Destination>();
                        if (AllPlantList == null || AllPlantList.Count == 0)
                        {
                            AllPlantList = new List<Destination>(HrmService.GetPlantListForDestination_V2560((UInt32)GeosApplication.Instance.ActiveUser.IdUser));
                            AllPlantList.ForEach(x => x.Country.CountryIconUrl = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Countries/" + x.Country?.Iso + ".png");
                        }
                        DestinationList.AddRange(AllPlantList);
                    }
                }
                else if (SelectedType?.Value == "EMDEP to Customer")
                {
                    DestinationList = new ObservableCollection<Destination>();
                    if (AllCustomer == null || AllCustomer.Count == 0)
                    {
                        AllCustomer = new List<Destination>(HrmService.GetCustomersForTripDestination_V2560());
                        AllCustomer.ForEach(x => x.Country.CountryIconUrl = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Countries/" + x.Country?.Iso + ".png");
                    }
                    DestinationList.AddRange(AllCustomer);
                }
                else if (SelectedType?.Value == "EMDEP to Provider")
                {
                    DestinationList = new ObservableCollection<Destination>();
                    if (SupplierList == null || SupplierList.Count == 0)
                    {
                        SupplierList = new List<Destination>(HrmService.HRM_GetSuppliersForDestination());
                        SupplierList.ForEach(x => x.Country.CountryIconUrl = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Countries/" + x.Country?.Iso + ".png");
                    }
                    DestinationList.AddRange(SupplierList);
                }


                GeosApplication.Instance.Logger.Log("Method FillMainTransport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillMainTransport() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][11-09-2024][GEOS2-5929]
        private void ReFillOriginDestination()
        {
            if (SelectedType != null && SelectedType.Value.Contains("EMDEP to"))
            {
                OriginList = new ObservableCollection<Company>(HrmCommon.Instance.UserAuthorizedPlantsList);
                if (SelectedResponsible != null)
                {
                    //OriginList = new ObservableCollection<Company>(HrmCommon.Instance.UserAuthorizedPlantsList);
                    if (SelectedResponsible.IdEmployee != 0 && !OriginList.Any(x => x.IdCompany == SelectedResponsible?.CompanyLocation?.IdCompany))
                    {
                        OriginList.Add(SelectedResponsible.CompanyLocation);
                    }
                    if (selectedResponsible.IdEmployee != 0)
                        SelectedOrigin = OriginList.FirstOrDefault(x => x.IdCompany == SelectedResponsible.CompanyLocation?.IdCompany);
                }
            }
            else if (SelectedType != null && SelectedType.Value.Contains("to EMDEP"))
            {
                if (SelectedResponsible != null && selectedResponsible.IdEmployee != 0)
                {
                    if (!DestinationList.Any(x => x.IsCompany == SelectedResponsible?.CompanyLocation?.IdCompany))
                    {
                        Destination dest = new Destination();
                        dest.IdDestination = SelectedResponsible.CompanyLocation.IdCompany;
                        dest.Name = SelectedResponsible.CompanyLocation.Alias;
                        DestinationList.Add(dest);
                    }
                    SelectedDestination = DestinationList.FirstOrDefault(x => x.IdDestination == SelectedResponsible.CompanyLocation.IdCompany);
                }
                if (SelectedType.Value == "Customer to EMDEP")
                {
                    OriginList = new ObservableCollection<Company>();
                    if (AllCustomer == null || AllCustomer.Count == 0)
                    {
                        AllCustomer = new List<Destination>(HrmService.GetCustomersForTripDestination_V2560());
                        AllCustomer.ForEach(x => x.Country.CountryIconUrl = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Countries/" + x.Country?.Iso + ".png");
                    }
                    AllCustomer.ForEach(x => { OriginList.Add(new Company() { IdCompany = x.IdDestination, Alias = x.Name, Country = new Country() { CountryIconUrl = x.Country?.CountryIconUrl } }); });
                    SelectedOrigin = OriginList.FirstOrDefault();
                }
                else if (SelectedType.Value == "Provider to EMDEP")
                {
                    OriginList = new ObservableCollection<Company>();
                    if (SupplierList == null || SupplierList.Count == 0)
                    {
                        SupplierList = new List<Destination>(HrmService.HRM_GetSuppliersForDestination());
                        SupplierList.ForEach(x => x.Country.CountryIconUrl = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Countries/" + x.Country?.Iso + ".png");
                    }
                    SupplierList.ForEach(x => { OriginList.Add(new Company() { IdCompany = x.IdDestination, Alias = x.Name, Country = new Country() { CountryIconUrl = x.Country?.CountryIconUrl } }); });
                    SelectedOrigin = OriginList.FirstOrDefault();
                }
            }
        }
        private void FillAccommodationType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainTransport()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempTypeList = HrmService.GetLookupValues(138);
                AccommodationTypeList = new List<LookupValue>(tempTypeList);

                if (IsNew)
                {
                    SelectedAccommodationType = null;
                }

                GeosApplication.Instance.Logger.Log("Method FillMainTransport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillMainTransport() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillOrigin()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainTransport()...", category: Category.Info, priority: Priority.Low);
                if (HrmCommon.Instance.UserAuthorizedPlantsList != null)
                {
                    OriginList = new ObservableCollection<Company>(HrmCommon.Instance.UserAuthorizedPlantsList);
                    OriginList.ToList().ForEach(x => x.Country.CountryIconUrl = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Countries/" + x.Country?.Iso + ".png");
                }

                GeosApplication.Instance.Logger.Log("Method FillMainTransport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillMainTransport() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][11-09-2024][GEOS2-5929]
        private void FillMainTransport()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainTransport()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempTypeList = HrmService.GetLookupValues(163);
                MainTransportList = new List<LookupValue>();

                MainTransportList.AddRange(tempTypeList);
                if (IsNew)
                {
                    SelectedArrivalTransport = null;
                    SelectedDepartureTransport = null;
                }

                GeosApplication.Instance.Logger.Log("Method FillMainTransport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillMainTransport() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillStatus()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatus()...", category: Category.Info, priority: Priority.Low);
                WorkflowStatusList = HrmService.HRM_GetAllTripWorkflowStatus();
                OriginalWorkflowStatusList = new List<WorkflowStatus>(WorkflowStatusList.Select(i => (WorkflowStatus)i.Clone()).ToList());

                if (IsNew)
                    WorkflowStatus = WorkflowStatusList.FirstOrDefault(x => x.Name == "Draft"); // Draft
                GeosApplication.Instance.Logger.Log("Method FillStatus()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatus() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatus() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillStatus() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillPropose()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillType()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempTypeList = HrmService.GetLookupValues(104);
                ProposeList = new List<LookupValue>();

                ProposeList.AddRange(tempTypeList);
                if (IsNew)
                {
                    SelectedPropose = null;
                }

                GeosApplication.Instance.Logger.Log("Method FillType()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPropose() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPropose() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillPropose() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillType()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempTypeList = HrmService.GetLookupValues(162);
                TypeList = new List<LookupValue>(tempTypeList);

                if (IsNew)
                {
                    SelectedType = null;
                }

                GeosApplication.Instance.Logger.Log("Method FillType()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillType() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillType() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillType() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillTraveller(EmployeeTrips selectedEmployee)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTraveller()...", category: Category.Info, priority: Priority.Low);
                if (IsNew)
                {
                    if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                    {
                        List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                        if (HrmCommon.Instance.TravellerList == null || HrmCommon.Instance.TravellerList.Count() == 0)
                        {
                            HrmCommon.Instance.TravellerList = new List<Traveller>();
                            foreach (var item in plantOwners)
                            {
                                //[nsatpute][22-10-2024][GEOS2-6656]
                                //[GEOS2-6760][rdixit][09.01.2025]
                                var temp = HrmService.GetActiveEmployeesForTraveller_V2600(item.IdCompany.ToString(),
                                    HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization,
                                    HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission);
                                HrmCommon.Instance.TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                            }
                        }
                        TravellerList.AddRange(HrmCommon.Instance.TravellerList.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                        if (IsNew)
                        {
                            SelectedTraveller = null;
                            CustomTraveler = "";
                        }
                    }
                }
                else
                {
                    if (selectedEmployee.IdEmployeeStatus == 136)
                    {
                        if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                        {
                            List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();

                            if (HrmCommon.Instance.TravellerList == null || HrmCommon.Instance.TravellerList.Count() == 0)
                            {
                                HrmCommon.Instance.TravellerList = new List<Traveller>();
                                foreach (var item in plantOwners)
                                {
                                    // [nsatpute][22-10-2024][GEOS2-6656]
                                    //[GEOS2-6760][rdixit][09.01.2025]
                                    var temp = HrmService.GetActiveEmployeesForTraveller_V2600(item.IdCompany.ToString(),
                                        HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization,
                                        HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission);
                                    HrmCommon.Instance.TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                                }
                            }
                            TravellerList.AddRange(HrmCommon.Instance.TravellerList.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                        }
                    }
                    else
                    {
                        if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                        {
                            List<Traveller> activeUserList = new List<Traveller>();
                            List<Traveller> inActiveUserList = new List<Traveller>();
                            UInt32 Inactive = selectedEmployee.IdEmployee;
                            List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                            if (HrmCommon.Instance.TravellerList == null || HrmCommon.Instance.TravellerList.Count() == 0)
                            {
                                HrmCommon.Instance.TravellerList = new List<Traveller>();
                                foreach (var item in plantOwners)
                                {
                                    // [nsatpute][22-10-2024][GEOS2-6656]
                                    //[GEOS2-6760][rdixit][09.01.2025]
                                    var temp = HrmService.GetActiveEmployeesForTraveller_V2600(item.IdCompany.ToString(),
                                        HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization,
                                        HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission);
                                    HrmCommon.Instance.TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                                  //  HrmCommon.Instance.TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus != 136).OrderBy(x => x.FullName));
                                }
                            }
                            activeUserList.AddRange(HrmCommon.Instance.TravellerList.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                            inActiveUserList.AddRange(HrmCommon.Instance.TravellerList.Where(x => x.IdEmployeeStatus != 136).OrderBy(x => x.FullName));

                            IsInActive = true;
                            List<Traveller> usrlst = new List<Traveller>();
                            if (IsInActive)
                            {

                                var filteredInactiveUsers = inActiveUserList.Where(x => x.IdEmployee == Inactive).ToList();
                                filteredInactiveUsers.ForEach(e => e.IsInActive = true);
                                usrlst.AddRange(filteredInactiveUsers);
                            }

                            TravellerList.AddRange(usrlst);
                            TravellerList.AddRange(activeUserList);
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillTraveller()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTraveller() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTraveller() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillTraveller() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillCode()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCode()...", category: Category.Info, priority: Priority.Low);
                Code = HrmService.GetLatestCodeForTrip_V2480();

                GeosApplication.Instance.Logger.Log("Method FillCode()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCode() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCode() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillCode() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][11-09-2024][GEOS2-5929]
        //private void FillResponsible()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillResponsible()...", category: Category.Info, priority: Priority.Low);
        //        List<Employee> tempResponsibles = HrmService.HRM_GetResponsibleForAddEditTrip();
        //        ResponsibleList = new ObservableCollection<Employee>(tempResponsibles);
        //        if (IsNew)
        //            SelectedResponsible = null;

        //        GeosApplication.Instance.Logger.Log("Method FillResponsible()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillResponsible() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillResponsible() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in method FillResponsible() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        // [nsatpute][11-09-2024][GEOS2-5929]
        private void FillWorkShift()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWorkShift()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempWorkShiftList = HrmService.GetLookupValues(105);
                WorkShiftList = new List<LookupValue>(tempWorkShiftList);
                if (IsNew)
                {
                    SelectedWorkShift = null;
                }

                GeosApplication.Instance.Logger.Log("Method FillWorkShift()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkShift() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkShift() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillWorkShift() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][11-09-2024][GEOS2-5929]
        private void FillTransportationType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWorkShift()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempWorkShiftList = HrmService.GetLookupValues(164); // Transport Type
                TransportationTypeList = new List<LookupValue>(tempWorkShiftList);
                if (IsNew)
                {
                    SelectedArrivalTransportationType = null;
                    SelectedDepartureTransportationType = null;
                }

                GeosApplication.Instance.Logger.Log("Method FillWorkShift()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkShift() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkShift() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillWorkShift() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][11-09-2024][GEOS2-5929]
        private void FillTripAssets()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTripAssets()...", category: Category.Info, priority: Priority.Low);
                IList<TripAssets> allTripassets = HrmService.HRM_GetEmployeeTripAssets();
                CarAssetsList = new ObservableCollection<TripAssets>(allTripassets.Where(x => x.IdType == 2135));
                SimCardAssetList = new ObservableCollection<TripAssets>(allTripassets.Where(x => x.IdType == 2137));
                MobilePhoneAssetList = new ObservableCollection<TripAssets>(allTripassets.Where(x => x.IdType == 2138));
                AccommodationAssetList = new ObservableCollection<TripAssets>(allTripassets.Where(x => x.IdType == 2139));

                if (IsNew)
                {
                    SelectedCarAsset = null;
                    SelectedSimCardAsset = null;
                    SelectedMobileAsset = null;
                    SelectedAccommodationAsset = null;
                }

                GeosApplication.Instance.Logger.Log("Method FillTripAssets()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTripAssets() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTripAssets() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillTripAssets() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void TransitionWorkflowStatus(EmployeeTrips employeeTrip)
        {
            if (WorkflowStatusList.FirstOrDefault(x => x.IdWorkflowStatus == employeeTrip.IdStatus) != null)
            {
                int currentStatus = (int)employeeTrip.IdStatus;
                employeeTrip.WorkflowStatus = WorkflowStatusList.FirstOrDefault(x => x.IdWorkflowStatus == employeeTrip.IdStatus);
                WorkflowTransition workflowTransition = new WorkflowTransition();
                workflowTransition = WorkflowTransitionList.FirstOrDefault(a => a.IdWorkflowStatusTo == currentStatus);
                WorkflowStatus = WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == currentStatus);

                List<byte> GetCurrentButtons = WorkflowTransitionList.Where(a => a.IdWorkflowStatusFrom == employeeTrip.WorkflowStatus.IdWorkflowStatus).Select(a => a.IdWorkflowStatusTo).Distinct().ToList();
                WorkflowStatusButtons = new List<WorkflowStatus>();

                foreach (byte statusbutton in GetCurrentButtons)
                {
                    if (statusbutton == 96) // [nsatpute][22-10-2024][GEOS2-6556]
                        WorkflowStatusButtons.Add(WorkflowStatusList.FirstOrDefault(a => a.IdWorkflowStatus == statusbutton));
                }
                WorkflowStatusButtons = new List<WorkflowStatus>(WorkflowStatusButtons);
            }
        }
        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelButtonCommandAction()..."), category: Category.Info, priority: Priority.Low);
                if (isAttachmentModified)
                    SavechangesInAttachmentGrid();
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method CancelButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][11-09-2024][GEOS2-5929]
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                allowValidation = true;
                error = EnableValidationAndGetError();
                #region Validation

                if (SelectedTraveller == null || SelectedTraveller.IdEmployeeStatus != 136 || string.IsNullOrEmpty(Code) || SelectedType == null ||
                    SelectedPropose == null || SelectedArrivalTransport == null || SelectedAccommodationType == null ||
                    SelectedOrigin == null || SelectedOrigin == null || string.IsNullOrEmpty(SelectedOrigin.ShortName) || SelectedDestination == null ||
                    string.IsNullOrEmpty(SelectedDestination.Name) || SelectedTripCurrency == null || string.IsNullOrEmpty(SelectedTripCurrency.ISOCode))
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedType"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedPropose"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedTraveller"));
                    PropertyChanged(this, new PropertyChangedEventArgs("TravelerEmail"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedWorkShift"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedOrigin"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedDestination"));
                    PropertyChanged(this, new PropertyChangedEventArgs("FromDate"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ToDate"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedResponsible"));
                }
                //[GEOS2-5932][rdixit][21.09.2024]
                PropertyChanged(this, new PropertyChangedEventArgs("ArrivalTransportationNumber"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedArrivalTransportationType"));
                PropertyChanged(this, new PropertyChangedEventArgs("DepartureTransportationNumber"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedDepartureTransportationType"));
                //PropertyChanged(this, new PropertyChangedEventArgs("ArrivalTransporterName"));
                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedArrivalTransport"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedDestination"));
                //PropertyChanged(this, new PropertyChangedEventArgs("ArrivalProvider"));
                //PropertyChanged(this, new PropertyChangedEventArgs("ArrivalTransporterContact"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedDepartureTransport"));
                PropertyChanged(this, new PropertyChangedEventArgs("DepartureTransporterName"));
                PropertyChanged(this, new PropertyChangedEventArgs("DepartureProvider"));
                PropertyChanged(this, new PropertyChangedEventArgs("DepartureTransporterContact"));
                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedAccommodationType"));
                //PropertyChanged(this, new PropertyChangedEventArgs("AccommodationAddress"));
                //PropertyChanged(this, new PropertyChangedEventArgs("AccommodationCoordinates"));
                if (error != null)
                {
                    return;
                }

                #endregion
                #region Add/delete Attachment
                if (ListAttachment?.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add).Count() > 0)
                {
                    ListAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add).ToList().ForEach(x => { if (x.FilePath != null) SaveAttachmentFileToServer(Path.GetDirectoryName(x.FilePath), Code, x.FileName); });
                }
                if (AttachmentFilesToDelete.Count() > 0)
                {
                    AttachmentFilesToDelete.ForEach(x => { HrmService.DeleteTripAttachmentFile(Code, x.FileName); });
                }
                if (UpdatedFileList.Count > 0)
                {
                    UpdatedFileList.ForEach(x =>
                    {
                        if (x.IdAttachment != 0)
                        {
                            if (x.FileName != ListAttachment.FirstOrDefault(y => y.IdAttachment == x.IdAttachment)?.FileName)
                            {
                                HrmService.DeleteTripAttachmentFile(Code, x.FileName);
                                TripAttachment newFile = ListAttachment.FirstOrDefault(y => y.IdAttachment == x.IdAttachment);
                                if (newFile != null)
                                    SaveAttachmentFileToServer(Path.GetDirectoryName(newFile.FilePath), Code, newFile.FileName);
                            }
                        }
                    });
                }
                #endregion
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
                            Background = new System.Windows.Media.SolidColorBrush(Colors.Transparent),
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                if (IsNew) //[rdixit][GEOS2-5930][20.09.2024]
                {
                    #region Add Trip
                    NewEmployeeTrip = new EmployeeTrips();
                    //INFORMATION
                    NewEmployeeTrip.Code = Code;
                    NewEmployeeTrip.IdTripType = (UInt32)SelectedType?.IdLookupValue;
                    NewEmployeeTrip.IdTripPropose = (UInt32)SelectedPropose?.IdLookupValue;
                    NewEmployeeTrip.IdOriginPlant = (UInt32)SelectedOrigin?.IdCompany;

                    NewEmployeeTrip.IdResponsible = (UInt32)SelectedResponsible?.IdEmployee;
                    NewEmployeeTrip.EmployeeDepartments = SelectedResponsible?.EmployeeDepartments;
                    NewEmployeeTrip.IsWeekend = Weekend;
                    NewEmployeeTrip.IdDestination = (UInt32)SelectedDestination?.IdDestination;
                    if (SelectedTraveller != null)
                        NewEmployeeTrip.IdEmployee = SelectedTraveller.IdEmployee;
                    NewEmployeeTrip.CustomTraveler = CustomTraveler;
                    NewEmployeeTrip.Desciption = Desciption;
                    NewEmployeeTrip.FromDate = FromDate;
                    if (SelectedDepartment != null)
                    {
                        NewEmployeeTrip.IdDepartment = SelectedDepartment.IdDepartment;
                    }
                    NewEmployeeTrip.IdWorkShift = SelectedWorkShift.IdLookupValue;
                    NewEmployeeTrip.TravelerEmail = TravelerEmail;
                    NewEmployeeTrip.ToDate = ToDate;
                    NewEmployeeTrip.IdStatus = WorkflowStatusList.FirstOrDefault(x => x.IdWorkflowStatus == 85).IdWorkflowStatus;
                    NewEmployeeTrip.WorkflowStatus = WorkflowStatusList.FirstOrDefault(x => x.IdWorkflowStatus == 85);
                    NewEmployeeTrip.VisaRequired = VisaRequired;
                    //Arrival Details      
                    NewEmployeeTrip.ArrivalTransportationNumber = ArrivalTransportationNumber;
                    NewEmployeeTrip.ArrivalDate = ArrivalDate;
                    NewEmployeeTrip.ArrivalTransportationType = (UInt32)SelectedArrivalTransportationType.IdLookupValue;
                    NewEmployeeTrip.DepartureTransportationNumber = DepartureTransportationNumber;
                    NewEmployeeTrip.DepartureDate = DepartureDate;
                    NewEmployeeTrip.DepartureTransportationType = (UInt32)SelectedDepartureTransportationType.IdLookupValue;

                    //Arrival Transport Details
                    //NewEmployeeTrip.IdArrivalTransport = SelectedArrivalTransport.IdLookupValue;
                    //NewEmployeeTrip.ArrivalTransporterName = ArrivalTransporterName;
                    //NewEmployeeTrip.ArrivalTransferRemark = ArrivalTransferRemark;
                    //NewEmployeeTrip.ArrivalProvider = ArrivalProvider;
                    //NewEmployeeTrip.ArrivalTransporterContact = ArrivalTransporterContact;
                    //NewEmployeeTrip.IdDepartureTransport = SelectedDepartureTransport.IdLookupValue;
                    //NewEmployeeTrip.DepartureTransporterName = DepartureTransporterName;
                    //NewEmployeeTrip.DepartureTransferRemark = DepartureTransferRemark;
                    //NewEmployeeTrip.DepartureProvider = DepartureProvider;
                    //NewEmployeeTrip.DepartureTransporterContact = DepartureTransporterContact;

                    //Accommodation Details                                    
                    //NewEmployeeTrip.IdAcommodationtype = (UInt32)SelectedAccommodationType?.IdLookupValue;
                    //NewEmployeeTrip.AccomodationAddress = AccommodationAddress;
                    //NewEmployeeTrip.AccommodationRemarks = AccommodationRemarks;
                    //if (SelectedAccommodationAsset != null)
                    //    NewEmployeeTrip.IdAccommodation = SelectedAccommodationAsset.IdAsset;
                    //NewEmployeeTrip.PartnerProvidedRoom = PartnerProvidedRoom;
                    //NewEmployeeTrip.AccommodationOtherRoom = AccommodationOtherRoom;
                    //NewEmployeeTrip.AccommodationCoordinates = AccommodationCoordinates;

                    //Other Details
                    NewEmployeeTrip.PlantCarProvided = PlantCarProvided;
                    NewEmployeeTrip.MobilePhoneProvided = MobilePhoneProvided;
                    NewEmployeeTrip.SimCardProvided = SimCardProvided;
                    NewEmployeeTrip.MoneyDeliveredAtOrigin = MoneyDeliveredAtOrigin;
                    NewEmployeeTrip.SelectedCarAsset = SelectedCarAsset;
                    NewEmployeeTrip.SelectedMobileAsset = SelectedMobileAsset;
                    NewEmployeeTrip.SelectedSimCardAsset = SelectedSimCardAsset;
                    NewEmployeeTrip.MoneyDeliveredAtDestination = MoneyDeliveredAtDestination;

                    if (!string.IsNullOrEmpty(Remarks))
                    {
                        NewEmployeeTrip.Remarks = Remarks.Trim();
                    }

                    NewEmployeeTrip.CreationDate = DateTime.Now;
                    NewEmployeeTrip.IdCreator = (UInt32)GeosApplication.Instance.ActiveUser.IdUser;
                    NewEmployeeTrip.LogEntriesList = new List<LogEntriesByEmployeeTrip>();
                    NewEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                    {
                        IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                        DateTimeChangeLog = DateTime.Now,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("AddEmployeeTripChangedLog").ToString(), Code)
                    });
                    if (ListAttachment == null)
                    {
                        ListAttachment = new ObservableCollection<TripAttachment>();
                    }
                    NewEmployeeTrip.ListAttachment = ListAttachment.ToList();
                    //[GEOS2-6760][rdixit][09.01.2025]
                    //NewEmployeeTrip = HrmService.AddEmployeeTripDetails_V2600(NewEmployeeTrip);
                    //HrmService = new HrmServiceController("localhost:6699");
                    //NewEmployeeTrip = HrmService.AddEmployeeTripDetails_V2650(NewEmployeeTrip); // [pallavi.kale][28-05-2025][GEOS2-7941]
                    //[Rahul.Gadhave][GEOS2-7989][Date:22-09-2025]
                    //HrmService = new HrmServiceController("localhost:6699");
                    if (TransfersList == null)
                    {
                        TransfersList = new ObservableCollection<Employee_trips_transfers>();
                    }
                    if (AccommodationsList == null)
                    {
                        AccommodationsList = new ObservableCollection<EmployeeTripsAccommodations>();
                    }
                    NewEmployeeTrip = HrmService.AddEmployeeTripDetails_V2670(NewEmployeeTrip, TransfersList.ToList(), AccommodationsList.ToList());
                    //HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    ListAttachment = new ObservableCollection<TripAttachment>(NewEmployeeTrip.ListAttachment);
                    ListAttachment.ForEach(x => x.TripAttachmentType = AttachmentTypeList.FirstOrDefault(y => y.IdLookupValue == x.IdAttachmentType));
                    IsSaveChanges = true;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TripAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                    #endregion
                }
                else
                {
                    #region Edit Trip
                    UpdatedEmployeeTrip = new EmployeeTrips();
                    UpdatedEmployeeTrip.LogEntriesList = new List<LogEntriesByEmployeeTrip>();
                    UpdatedEmployeeTrip.IdEmployeeTrip = IdEmployeeTrip;
                    UpdatedEmployeeTrip.Code = Code;
                    #region INFORMATION REMAINING
                    #region IdEmployee
                    if (SelectedTraveller != null)
                    {
                        if (EmployeeTrip.IdEmployee != SelectedTraveller.IdEmployee)//Traveller
                        {
                            UpdatedEmployeeTrip.IdEmployee = SelectedTraveller.IdEmployee;
                            if (IsTravellerSamePlant)
                            {
                                UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                                {
                                    IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTimeChangeLog = DateTime.Now,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeTripTravellerChangeLog").ToString(), PreviousTravellerFullName, SelectedTraveller.FullName)
                                });
                            }
                            else
                            {
                                UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                                {
                                    IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTimeChangeLog = DateTime.Now,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeTripTravellerChangeLog").ToString(), EmployeeTrip.FullName, SelectedTraveller.FullName)
                                });
                            }

                        }
                        else
                        {
                            UpdatedEmployeeTrip.IdEmployee = SelectedTraveller.IdEmployee;
                        }
                    }
                    #endregion

                    #region Type
                    if (EmployeeTrip.IdTripType != SelectedType.IdLookupValue)//Type
                    {
                        UpdatedEmployeeTrip.IdTripType = (UInt32)SelectedType.IdLookupValue;
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeTripTypeChangeLog").ToString(), TypeList.FirstOrDefault(x => x.IdLookupValue == EmployeeTrip.IdTripType).Value, SelectedType.Value)
                        });
                        IsSaveChanges = true;
                    }
                    else
                    {
                        UpdatedEmployeeTrip.IdTripType = (UInt32)SelectedType.IdLookupValue;
                    }
                    #endregion

                    #region IdTripPurpose
                    if (EmployeeTrip.IdTripPropose != SelectedPropose.IdLookupValue)//Propose
                    {
                        //[GEOS2-6760][rdixit][09.01.2025]
                        LookupValue oldval = ProposeList.FirstOrDefault(x => x.IdLookupValue == EmployeeTrip.IdTripPropose);
                        UpdatedEmployeeTrip.IdTripPropose = (UInt32)SelectedPropose.IdLookupValue;
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeTripProposeChangeLog").ToString(),
                            (string.IsNullOrEmpty(oldval?.Value) ? "None" : oldval?.Value),
                            (string.IsNullOrEmpty(SelectedPropose?.Value) ? "None" : SelectedPropose?.Value))
                        });
                    }
                    else
                    {
                        UpdatedEmployeeTrip.IdTripPropose = (UInt32)SelectedPropose.IdLookupValue;
                    }
                    #endregion

                    #region status
                    if (EmployeeTrip.IdStatus != WorkflowStatus?.IdWorkflowStatus)//Status
                    {
                        var OldStatus = OriginalWorkflowStatusList.FirstOrDefault(i => i.IdWorkflowStatus == EmployeeTrip.IdStatus);
                        UpdatedEmployeeTrip.IdStatus = WorkflowStatus.IdWorkflowStatus;
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeTripStatusChangeLog").ToString(),
                            (string.IsNullOrEmpty(OldStatus?.Name) ? "None" : OldStatus?.Name),
                            (string.IsNullOrEmpty(WorkflowStatus?.Name) ? "None" : WorkflowStatus?.Name))
                        });
                    }
                    else
                    {
                        UpdatedEmployeeTrip.IdStatus = (UInt32)WorkflowStatus.IdWorkflowStatus;
                    }
                    #endregion

                    #region Origin
                    if (EditEmployeeTrips.IdOriginPlant != SelectedOrigin.IdCompany)//Origin
                    {
                        Company old = null;
                        #region old
                        if (EditEmployeeTrips.IdTripType == 2096 || EditEmployeeTrips.IdTripType == 2097 || EditEmployeeTrips.IdTripType == 2098)
                            old = OriginList.FirstOrDefault(x => x.IdCompany == EditEmployeeTrips.IdOriginPlant);
                        else if (EditEmployeeTrips.IdTripType == 2099)
                        {
                            var cust = AllCustomer.FirstOrDefault(x => x.IdDestination == EditEmployeeTrips.IdOriginPlant);
                            old = new Company() { IdCompany = cust.IdDestination, Alias = cust.Name, Country = new Country() { CountryIconUrl = cust.Country?.CountryIconUrl } };
                        }
                        else if (EditEmployeeTrips.IdTripType == 2100)
                        {
                            var sup = SupplierList.FirstOrDefault(x => x.IdDestination == EditEmployeeTrips.IdOriginPlant);
                            old = new Company() { IdCompany = sup.IdDestination, Alias = sup.Name, Country = new Country() { CountryIconUrl = sup.Country?.CountryIconUrl } };
                        }
                        #endregion                 
                        UpdatedEmployeeTrip.IdOriginPlant = (UInt32)SelectedOrigin.IdCompany;
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeTripOriginChangeLog").ToString(),
                             (string.IsNullOrEmpty(old?.Alias) ? "None" : old?.Alias),
                             SelectedOrigin?.Alias)
                        });
                    }
                    else
                    {
                        UpdatedEmployeeTrip.IdOriginPlant = (UInt32)SelectedOrigin?.IdCompany;
                    }
                    #endregion

                    #region Destination
                    if (EmployeeTrip.IdDestination != SelectedDestination.IdDestination)//Destination
                    {
                        UpdatedEmployeeTrip.IdDestination = (UInt32)SelectedDestination.IdDestination;
                        if (EditEmployeeTrips.IdTripType == SelectedType.IdLookupValue)
                        {
                            UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                            {
                                IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                DateTimeChangeLog = DateTime.Now,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeTripDestinationChangeLog").ToString(),
                                DestinationList.FirstOrDefault(x => x.IdDestination == EmployeeTrip.IdDestination)?.Name, SelectedDestination?.Name)
                            });
                        }
                        else
                        {
                            if (EditEmployeeTrips.IdTripType == 2096 || EditEmployeeTrips.IdTripType == 2099 || EditEmployeeTrips.IdTripType == 2100)//Plant
                            {
                                var temp = new List<Destination>(HrmService.GetPlantListForDestination_V2560((UInt32)GeosApplication.Instance.ActiveUser.IdUser));
                                UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                                {
                                    IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTimeChangeLog = DateTime.Now,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeTripDestinationChangeLog").ToString(),
                               temp.FirstOrDefault(x => x.IdDestination == EmployeeTrip.IdDestination)?.Name, SelectedDestination?.Name)
                                });
                            }
                            else if (EditEmployeeTrips.IdTripType == 2099)//Customer
                            {
                                var temp = new List<Destination>(HrmService.GetCustomersForTripDestination_V2560());
                                UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                                {
                                    IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTimeChangeLog = DateTime.Now,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeTripDestinationChangeLog").ToString(),
                               temp.FirstOrDefault(x => x.IdDestination == EmployeeTrip.IdDestination)?.Name, SelectedDestination?.Name)
                                });
                            }
                            else if (EditEmployeeTrips.IdTripType == 2100)
                            {
                                UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                                {
                                    IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTimeChangeLog = DateTime.Now,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeTripDestinationChangeLog").ToString(),
                               SupplierList.FirstOrDefault(x => x.IdDestination == EditEmployeeTrips.IdDestination)?.Name, SelectedDestination?.Name)
                                });
                            }
                            else//Private
                            {
                                UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                                {
                                    IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTimeChangeLog = DateTime.Now,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeTripDestinationChangeLog").ToString(),
                              "Private", SelectedDestination?.Name)
                                });
                            }
                        }

                    }
                    else
                    {
                        UpdatedEmployeeTrip.IdDestination = (UInt32)SelectedDestination.IdDestination;
                    }
                    #endregion

                    #region ArrivalDate
                    //[rahul.gadhave][GEOS2-5443][05-03-2024]
                    TimeSpan arrivalTime = new TimeSpan(EditEmployeeTrips.ArrivalDate.Value.TimeOfDay.Hours, EditEmployeeTrips.ArrivalDate.Value.TimeOfDay.Minutes, 0);
                    TimeSpan arrivalDateHours = new TimeSpan(ArrivalDateHours.Hours, ArrivalDateHours.Minutes, 0);
                    if (EditEmployeeTrips.ArrivalDate != ArrivalDate || arrivalTime != arrivalDateHours)
                    {
                        DateTime oldArrivalDateTime = EmployeeTrip.ArrivalDate ?? DateTime.MinValue;
                        DateTime newArrivalDateTime = new DateTime(ArrivalDate.Year, ArrivalDate.Month, ArrivalDate.Day,
                                                                    ArrivalDateHours.Hours, ArrivalDateHours.Minutes, 0);

                        UpdatedEmployeeTrip.ArrivalDate = newArrivalDateTime;
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeTripArrivalDateChangeLog").ToString(),
                                                    oldArrivalDateTime.ToString("g"), newArrivalDateTime.ToString("g"))
                        });
                    }
                    else
                    {
                        UpdatedEmployeeTrip.ArrivalDate = ArrivalDate;
                    }
                    #endregion

                    #region DepartureDate
                    //[rahul.gadhave][GEOS2-5443][05-03-2024]
                    TimeSpan departureTime = new TimeSpan(EditEmployeeTrips.DepartureDate.Value.TimeOfDay.Hours, EditEmployeeTrips.DepartureDate.Value.TimeOfDay.Minutes, 0);
                    TimeSpan departureDateHours = new TimeSpan(DepartureDateHours.Hours, DepartureDateHours.Minutes, 0);
                    if (EditEmployeeTrips.DepartureDate != DepartureDate || departureTime != departureDateHours)
                    {
                        DateTime oldDepartureDateTime = EmployeeTrip.DepartureDate ?? DateTime.MinValue;
                        DateTime newDepartureDateTime = new DateTime(DepartureDate.Year, DepartureDate.Month, DepartureDate.Day,
                                                                     DepartureDateHours.Hours, DepartureDateHours.Minutes, 0);

                        UpdatedEmployeeTrip.DepartureDate = newDepartureDateTime;
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeTripDepartureDateChangeLog").ToString(),
                                                    oldDepartureDateTime.ToString("g"), newDepartureDateTime.ToString("g"))
                        });
                    }
                    else
                    {
                        UpdatedEmployeeTrip.DepartureDate = DepartureDate;
                    }
                    #endregion

                    #region Remarks
                    if (EmployeeTrip.Remarks != Remarks)
                    {
                        if (!string.IsNullOrEmpty(Remarks))
                        {
                            if (string.IsNullOrEmpty(EmployeeTrip.Remarks) || EmployeeTrip.Remarks == "")
                            {
                                UpdatedEmployeeTrip.Remarks = Remarks.Trim();

                                UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                                {
                                    IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTimeChangeLog = DateTime.Now,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeRemarksChangeLog").ToString(), "None", Remarks)
                                });
                            }
                            else
                            {
                                UpdatedEmployeeTrip.Remarks = Remarks.Trim();

                                UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                                {
                                    IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTimeChangeLog = DateTime.Now,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeRemarksChangeLog").ToString(), EmployeeTrip.Remarks, Remarks)
                                });
                            }


                        }
                        else
                        {
                            UpdatedEmployeeTrip.Remarks = Remarks;

                            UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                            {
                                IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                DateTimeChangeLog = DateTime.Now,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmployeeRemarksChangeLog").ToString(), EmployeeTrip.Remarks, "None")
                            });
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Remarks))
                            UpdatedEmployeeTrip.Remarks = Remarks.Trim();
                    }
                    #endregion

                    //[rdixit][GEOS2-5930][20.09.2024] [GEOS2-5932][rdixit][21.09.2024]

                    #region Responsible
                    UpdatedEmployeeTrip.IdResponsible = (UInt32)SelectedResponsible?.IdEmployee;
                    if (SelectedResponsible?.IdEmployee != EditEmployeeTrips.IdResponsible)
                    {
                        var oldResponsible = ResponsibleList.FirstOrDefault(x => x.IdEmployee == EditEmployeeTrips.IdResponsible);
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmpTripResponsibleChangeLog").ToString(),
                            (string.IsNullOrEmpty(oldResponsible?.FullName) ? "None" : oldResponsible?.FullName), (string.IsNullOrEmpty(SelectedResponsible?.FullName) ? "None" : SelectedResponsible?.FullName))
                        });
                    }
                    #endregion

                    #region Departments
                    //[GEOS2-6760][rdixit][09.01.2025
                    if (SelectedDepartment != null)
                    {
                        UpdatedEmployeeTrip.IdDepartment = SelectedDepartment.IdDepartment;
                    }

                    var OldDept = DepartmentList.FirstOrDefault(x => x.IdDepartment == EditEmployeeTrips.IdDepartment);
                    if (SelectedDepartment != null && OldDept != null)
                    {
                        if (SelectedDepartment.IdDepartment != OldDept?.IdDepartment)
                        {
                            UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                            {
                                IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                DateTimeChangeLog = DateTime.Now,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripDepartmentsChangeLog").ToString(),
                                OldDept?.DepartmentName, SelectedDepartment?.DepartmentName)
                            });
                        }
                    }

                    #endregion

                    #region Weekend
                    UpdatedEmployeeTrip.IsWeekend = Weekend;
                    if (Weekend != EditEmployeeTrips.IsWeekend)
                    {
                        string oldval = EditEmployeeTrips.IsWeekend ? "Yes" : "No";
                        string newval = Weekend ? "Yes" : "No";
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripWeekendChangeLog").ToString(), oldval, newval)
                        });
                    }
                    #endregion

                    #region CustomTraveler
                    UpdatedEmployeeTrip.CustomTraveler = CustomTraveler;
                    if (CustomTraveler != EditEmployeeTrips.CustomTraveler)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripCustomTravelerChangeLog").ToString(),
                           (string.IsNullOrEmpty(EditEmployeeTrips.CustomTraveler) ? "None" : EditEmployeeTrips.CustomTraveler), (string.IsNullOrEmpty(CustomTraveler) ? "None" : CustomTraveler))
                        });
                    }
                    #endregion

                    #region Desciption
                    UpdatedEmployeeTrip.Desciption = Desciption?.Trim();
                    if (Desciption?.Trim() != EditEmployeeTrips.Desciption?.Trim())
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripDesciptionChangeLog").ToString(),
                           (string.IsNullOrEmpty(EditEmployeeTrips.Desciption) ? "None" : EditEmployeeTrips.Desciption), (string.IsNullOrEmpty(Desciption) ? "None" : Desciption?.Trim()))
                        });
                    }
                    #endregion

                    #region FromDate
                    UpdatedEmployeeTrip.FromDate = FromDate;
                    if (FromDate != EditEmployeeTrips.FromDate)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripFromDateChangeLog").ToString(),
                            EditEmployeeTrips.FromDate.ToShortDateString(), FromDate.ToShortDateString())
                        });
                    }
                    #endregion

                    #region Shift
                    UpdatedEmployeeTrip.IdWorkShift = SelectedWorkShift.IdLookupValue;
                    if (SelectedWorkShift.IdLookupValue != EditEmployeeTrips.IdWorkShift)
                    {
                        var OldShift = WorkShiftList.FirstOrDefault(i => i.IdLookupValue == EditEmployeeTrips.IdWorkShift);
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripShiftChangeLog").ToString(),
                            OldShift.Value, SelectedWorkShift.Value)
                        });
                    }
                    #endregion

                    #region TravelerEmail
                    UpdatedEmployeeTrip.TravelerEmail = TravelerEmail;
                    if (TravelerEmail != EditEmployeeTrips.TravelerEmail)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripTravelerEmailChangeLog").ToString(),
                           (string.IsNullOrEmpty(EditEmployeeTrips.TravelerEmail) ? "None" : EditEmployeeTrips.TravelerEmail), (string.IsNullOrEmpty(TravelerEmail) ? "None" : TravelerEmail))
                        });
                    }
                    #endregion

                    #region ToDate
                    UpdatedEmployeeTrip.ToDate = ToDate;
                    if (ToDate != EditEmployeeTrips.ToDate)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripToDateChangeLog").ToString(),
                            EditEmployeeTrips.ToDate.ToShortDateString(), ToDate.ToShortDateString())
                        });
                    }

                    #region VisaRequired
                    UpdatedEmployeeTrip.VisaRequired = VisaRequired;
                    if (VisaRequired != EditEmployeeTrips.VisaRequired)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripVisaRequiredChangeLog").ToString(),
                            EditEmployeeTrips.VisaRequired.ToString(), VisaRequired.ToString())
                        });
                    }
                    #endregion

                    #endregion

                    #endregion

                    #region  Arrival Details    
                    #region ArrivalTransportationNumber
                    UpdatedEmployeeTrip.ArrivalTransportationNumber = ArrivalTransportationNumber;
                    if (ArrivalTransportationNumber != EditEmployeeTrips.ArrivalTransportationNumber)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripArrivalTransportationNumberhangeLog").ToString(),
                            (string.IsNullOrEmpty(EditEmployeeTrips.ArrivalTransportationNumber) ? "None" : EditEmployeeTrips.ArrivalTransportationNumber),
                            (string.IsNullOrEmpty(ArrivalTransportationNumber) ? "None" : ArrivalTransportationNumber))
                        });
                    }
                    #endregion

                    #region ArrivalTransportationType
                    UpdatedEmployeeTrip.ArrivalTransportationType = (UInt32)SelectedArrivalTransportationType.IdLookupValue;
                    if (SelectedArrivalTransportationType?.IdLookupValue != EditEmployeeTrips.ArrivalTransportationType)
                    {
                        var oldType = TransportationTypeList.First(i => i.IdLookupValue == EditEmployeeTrips.ArrivalTransportationType);
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripArrivalTransportationTypeChangeLog").ToString(),
                            (string.IsNullOrEmpty(oldType?.Value) || oldType?.Value == "---" ? "None" : oldType?.Value),
                            (string.IsNullOrEmpty(SelectedArrivalTransportationType?.Value) || SelectedArrivalTransportationType?.Value == "---" ? "None" : SelectedArrivalTransportationType?.Value))
                        });
                    }
                    #endregion

                    #region DepartureTransportationNumber
                    UpdatedEmployeeTrip.DepartureTransportationNumber = DepartureTransportationNumber;
                    if (DepartureTransportationNumber != EditEmployeeTrips.DepartureTransportationNumber)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripDepartureTransportationNumberChangeLog").ToString(),
                            (string.IsNullOrEmpty(EditEmployeeTrips.DepartureTransportationNumber) ? "None" : EditEmployeeTrips.DepartureTransportationNumber),
                            (string.IsNullOrEmpty(DepartureTransportationNumber) ? "None" : DepartureTransportationNumber))
                        });
                    }
                    #endregion

                    #region DepartureTransportationType
                    UpdatedEmployeeTrip.DepartureTransportationType = (UInt32)SelectedDepartureTransportationType.IdLookupValue;
                    if (SelectedDepartureTransportationType?.IdLookupValue != EditEmployeeTrips.DepartureTransportationType)
                    {
                        var oldType = TransportationTypeList.First(i => i.IdLookupValue == EditEmployeeTrips.DepartureTransportationType);
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripDepartureTransportationTypeChangeLog").ToString(),
                            (string.IsNullOrEmpty(oldType?.Value) || oldType?.Value == "---" ? "None" : oldType?.Value),
                            (string.IsNullOrEmpty(SelectedDepartureTransportationType?.Value) || SelectedDepartureTransportationType?.Value == "---" ? "None" : SelectedDepartureTransportationType?.Value))
                        });
                    }
                    #endregion
                    #endregion

                    //Arrival Transport Details
                    #region SelectedArrivalTransport
                    //UpdatedEmployeeTrip.IdArrivalTransport = SelectedArrivalTransport.IdLookupValue;
                    //if (SelectedArrivalTransport?.IdLookupValue != EditEmployeeTrips.IdArrivalTransport)
                    //{
                    //    var oldType = MainTransportList.First(i => i.IdLookupValue == EditEmployeeTrips.IdArrivalTransport);
                    //    UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                    //    {
                    //        IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                    //        DateTimeChangeLog = DateTime.Now,
                    //        Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripArrivalTransportTypeChangeLog").ToString(),
                    //        (string.IsNullOrEmpty(oldType?.Value) || oldType?.Value == "---" ? "None" : oldType?.Value),
                    //        (string.IsNullOrEmpty(SelectedArrivalTransport?.Value) ? "None" : SelectedArrivalTransport?.Value))
                    //    });
                    //}
                    #endregion

                    //#region ArrivalTransporterName
                    //UpdatedEmployeeTrip.ArrivalTransporterName = ArrivalTransporterName;
                    //if (ArrivalTransporterName != EditEmployeeTrips.ArrivalTransporterName)
                    //{
                    //    UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                    //    {
                    //        IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                    //        DateTimeChangeLog = DateTime.Now,
                    //        Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripArrivalTransportNameChangeLog").ToString(),
                    //        (string.IsNullOrEmpty(EditEmployeeTrips.ArrivalTransporterName) ? "None" : EditEmployeeTrips.ArrivalTransporterName),
                    //        (string.IsNullOrEmpty(ArrivalTransporterName) ? "None" : ArrivalTransporterName))
                    //    });
                    //}
                    //#endregion

                    //#region ArrivalTransferRemark
                    //UpdatedEmployeeTrip.ArrivalTransferRemark = ArrivalTransferRemark?.Trim();
                    //if (ArrivalTransferRemark?.Trim() != EditEmployeeTrips.ArrivalTransferRemark?.Trim())
                    //{
                    //    UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                    //    {
                    //        IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                    //        DateTimeChangeLog = DateTime.Now,
                    //        Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripArrivalTransportRemarkChangeLog").ToString(),
                    //        (string.IsNullOrEmpty(EditEmployeeTrips.ArrivalTransferRemark) ? "None" : EditEmployeeTrips.ArrivalTransferRemark),
                    //        (string.IsNullOrEmpty(ArrivalTransferRemark) ? "None" : ArrivalTransferRemark?.Trim()))
                    //    });
                    //}
                    //#endregion

                    //#region ArrivalProvider
                    //UpdatedEmployeeTrip.ArrivalProvider = ArrivalProvider;
                    //if (ArrivalProvider != EditEmployeeTrips.ArrivalProvider)
                    //{
                    //    UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                    //    {
                    //        IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                    //        DateTimeChangeLog = DateTime.Now,
                    //        Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripArrivalTransportProviderChangeLog").ToString(),
                    //        (string.IsNullOrEmpty(EditEmployeeTrips.ArrivalProvider) ? "None" : EditEmployeeTrips.ArrivalProvider),
                    //        (string.IsNullOrEmpty(ArrivalProvider) ? "None" : ArrivalProvider))
                    //    });
                    //}
                    //#endregion

                    //#region ArrivalTransporterContact
                    //UpdatedEmployeeTrip.ArrivalTransporterContact = ArrivalTransporterContact;
                    //if (ArrivalTransporterContact != EditEmployeeTrips.ArrivalTransporterContact)
                    //{
                    //    UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                    //    {
                    //        IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                    //        DateTimeChangeLog = DateTime.Now,
                    //        Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripArrivalTransporterContactChangeLog").ToString(),
                    //        (string.IsNullOrEmpty(EditEmployeeTrips.ArrivalTransporterContact) ? "None" : EditEmployeeTrips.ArrivalTransporterContact),
                    //        (string.IsNullOrEmpty(ArrivalTransporterContact) ? "None" : ArrivalTransporterContact))
                    //    });
                    //}
                    //#endregion

                    #region SelectedDepartureTransport
                    //UpdatedEmployeeTrip.IdDepartureTransport = SelectedDepartureTransport.IdLookupValue;
                    //if (SelectedDepartureTransport?.IdLookupValue != EditEmployeeTrips.IdDepartureTransport)
                    //{
                    //    var oldType = MainTransportList.First(i => i.IdLookupValue == EditEmployeeTrips.IdDepartureTransport);
                    //    UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                    //    {
                    //        IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                    //        DateTimeChangeLog = DateTime.Now,
                    //        Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripDepartureTransportTypeChangeLog").ToString(),
                    //        (string.IsNullOrEmpty(oldType?.Value) || oldType?.Value == "---" ? "None" : oldType?.Value),
                    //        (string.IsNullOrEmpty(SelectedDepartureTransport?.Value) || SelectedDepartureTransport?.Value == "---" ? "None" : SelectedDepartureTransport?.Value))
                    //    });
                    //}
                    #endregion

                    #region DepartureTransporterName
                    UpdatedEmployeeTrip.DepartureTransporterName = DepartureTransporterName;
                    if (DepartureTransporterName != EditEmployeeTrips.DepartureTransporterName)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripDepartureTransporterNameChangeLog").ToString(),
                            (string.IsNullOrEmpty(EditEmployeeTrips.DepartureTransporterName) || EditEmployeeTrips.DepartureTransporterName == "---" ? "None" : EditEmployeeTrips.DepartureTransporterName),
                            (string.IsNullOrEmpty(DepartureTransporterName) || DepartureTransporterName == "---" ? "None" : DepartureTransporterName))
                        });
                    }
                    #endregion

                    #region DepartureTransferRemark
                    UpdatedEmployeeTrip.DepartureTransferRemark = DepartureTransferRemark?.Trim();
                    if (DepartureTransferRemark?.Trim() != EditEmployeeTrips.DepartureTransferRemark?.Trim())
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripDepartureTransferRemarkChangeLog").ToString(),
                            (string.IsNullOrEmpty(EditEmployeeTrips.DepartureTransferRemark) || EditEmployeeTrips.DepartureTransferRemark == "---" ? "None" : EditEmployeeTrips.DepartureTransferRemark),
                            (string.IsNullOrEmpty(DepartureTransferRemark) || DepartureTransferRemark == "---" ? "None" : DepartureTransferRemark?.Trim()))
                        });
                    }
                    #endregion

                    #region DepartureProvider
                    UpdatedEmployeeTrip.DepartureProvider = DepartureProvider;
                    if (DepartureProvider != EditEmployeeTrips.DepartureProvider)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripDepartureProviderChangeLog").ToString(),
                            (string.IsNullOrEmpty(EditEmployeeTrips.DepartureProvider) || EditEmployeeTrips.DepartureProvider == "---" ? "None" : EditEmployeeTrips.DepartureProvider),
                            (string.IsNullOrEmpty(DepartureProvider) || DepartureProvider == "---" ? "None" : DepartureProvider))
                        });
                    }
                    #endregion

                    #region DepartureTransporterContact
                    UpdatedEmployeeTrip.DepartureTransporterContact = DepartureTransporterContact;
                    if (DepartureTransporterContact != EditEmployeeTrips.DepartureTransporterContact)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("DepartureTransporterContact").ToString(),
                            (string.IsNullOrEmpty(EditEmployeeTrips.DepartureTransporterContact) || EditEmployeeTrips.DepartureTransporterContact == "---" ? "None" : EditEmployeeTrips.DepartureTransporterContact),
                            (string.IsNullOrEmpty(DepartureTransporterContact) || DepartureTransporterContact == "---" ? "None" : DepartureTransporterContact))
                        });
                    }
                    #endregion 

                    //#region Accommodation Details        
                    //#region IdAcommodationtype                            
                    //UpdatedEmployeeTrip.IdAcommodationtype = (UInt32)SelectedAccommodationType?.IdLookupValue;
                    //if (SelectedAccommodationType?.IdLookupValue != EditEmployeeTrips.IdAcommodationtype)
                    //{
                    //    var oldAccType = AccommodationTypeList.FirstOrDefault(x => x.IdLookupValue == EditEmployeeTrips.IdAcommodationtype);
                    //    UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                    //    {
                    //        IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                    //        DateTimeChangeLog = DateTime.Now,
                    //        Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripAccommodationTypeChangeLog").ToString(),
                    //        (string.IsNullOrEmpty(oldAccType?.Value) || oldAccType?.Value == "---" ? "None" : oldAccType?.Value),
                    //        (string.IsNullOrEmpty(SelectedAccommodationType?.Value) || SelectedAccommodationType?.Value == "---" ? "None" : SelectedAccommodationType?.Value))
                    //    });
                    //}
                    //#endregion

                    //#region AccomodationAddress
                    //UpdatedEmployeeTrip.AccomodationAddress = AccommodationAddress;
                    //if (AccommodationAddress != EditEmployeeTrips.AccomodationAddress)
                    //{
                    //    UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                    //    {
                    //        IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                    //        DateTimeChangeLog = DateTime.Now,
                    //        Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripAccommodationAddressChangeLog").ToString(),
                    //        (string.IsNullOrEmpty(EditEmployeeTrips.AccomodationAddress) || EditEmployeeTrips.AccomodationAddress == "---" ? "None" : EditEmployeeTrips.AccomodationAddress),
                    //        (string.IsNullOrEmpty(AccommodationAddress) || AccommodationAddress == "---" ? "None" : AccommodationAddress))
                    //    });
                    //}
                    //#endregion

                    //#region AccommodationRemarks
                    //UpdatedEmployeeTrip.AccommodationRemarks = AccommodationRemarks?.Trim();
                    //if (AccommodationRemarks?.Trim() != EditEmployeeTrips.AccommodationRemarks?.Trim())
                    //{
                    //    UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                    //    {
                    //        IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                    //        DateTimeChangeLog = DateTime.Now,
                    //        Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripAccommodationRemarksChangeLog").ToString(),
                    //        (string.IsNullOrEmpty(EditEmployeeTrips.AccommodationRemarks) || EditEmployeeTrips.AccommodationRemarks == "---" ? "None" : EditEmployeeTrips.AccommodationRemarks),
                    //        (string.IsNullOrEmpty(AccommodationRemarks) || AccommodationRemarks == "---" ? "None" : AccommodationRemarks?.Trim()))
                    //    });
                    //}
                    //#endregion

                    //#region SelectedAccommodationAsset
                    //if (SelectedAccommodationAsset != null)
                    //{
                    //    UpdatedEmployeeTrip.IdAccommodation = SelectedAccommodationAsset.IdAsset;
                    //    if (EditEmployeeTrips.IdAccommodation != SelectedAccommodationAsset.IdAsset)
                    //    {
                    //        var oldAccomodation = AccommodationAssetList.FirstOrDefault(i => i.IdAsset == EditEmployeeTrips.IdAccommodation);

                    //        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                    //        {
                    //            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                    //            DateTimeChangeLog = DateTime.Now,
                    //            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripAccommodationAssetChangeLog").ToString(),
                    //         (string.IsNullOrEmpty(oldAccomodation?.PublicIdentifier) || oldAccomodation?.PublicIdentifier == "---" ? "None" : oldAccomodation?.PublicIdentifier),
                    //         (string.IsNullOrEmpty(SelectedAccommodationAsset?.PublicIdentifier) || SelectedAccommodationAsset?.PublicIdentifier == "---" ? "None" : SelectedAccommodationAsset?.PublicIdentifier))
                    //        });
                    //    }
                    //}
                    //#endregion

                    //#region PartnerProvidedRoom
                    //UpdatedEmployeeTrip.PartnerProvidedRoom = PartnerProvidedRoom;
                    //if (PartnerProvidedRoom != EditEmployeeTrips.PartnerProvidedRoom)
                    //{
                    //    UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                    //    {
                    //        IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                    //        DateTimeChangeLog = DateTime.Now,
                    //        Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripPartnerProvidedRoomChangeLog").ToString(),
                    //        (string.IsNullOrEmpty(EditEmployeeTrips.PartnerProvidedRoom) || EditEmployeeTrips.PartnerProvidedRoom == "---" ? "None" : EditEmployeeTrips.PartnerProvidedRoom),
                    //        (string.IsNullOrEmpty(PartnerProvidedRoom) || PartnerProvidedRoom == "---" ? "None" : PartnerProvidedRoom))
                    //    });
                    //}
                    //#endregion

                    //#region AccommodationOtherRoom
                    //UpdatedEmployeeTrip.AccommodationOtherRoom = AccommodationOtherRoom;
                    //if (AccommodationOtherRoom != EditEmployeeTrips.AccommodationOtherRoom)
                    //{
                    //    UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                    //    {
                    //        IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                    //        DateTimeChangeLog = DateTime.Now,
                    //        Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripAccommodationOtherRoomChangeLog").ToString(),
                    //        (string.IsNullOrEmpty(EditEmployeeTrips.AccommodationOtherRoom) || EditEmployeeTrips.AccommodationOtherRoom == "---" ? "None" : EditEmployeeTrips.AccommodationOtherRoom),
                    //        (string.IsNullOrEmpty(AccommodationOtherRoom) || AccommodationOtherRoom == "---" ? "None" : AccommodationOtherRoom))
                    //    });
                    //}
                    //#endregion

                    //#region AccommodationCoordinates
                    //UpdatedEmployeeTrip.AccommodationCoordinates = AccommodationCoordinates;
                    //if (AccommodationCoordinates != EditEmployeeTrips.AccommodationCoordinates)
                    //{
                    //    UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                    //    {
                    //        IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                    //        DateTimeChangeLog = DateTime.Now,
                    //        Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripAccommodationCoordinatesChangeLog").ToString(),
                    //        (string.IsNullOrEmpty(EditEmployeeTrips.AccommodationCoordinates) || EditEmployeeTrips.AccommodationCoordinates == "---" ? "None" : EditEmployeeTrips.AccommodationCoordinates),
                    //        (string.IsNullOrEmpty(AccommodationCoordinates) || AccommodationCoordinates == "---" ? "None" : AccommodationCoordinates))
                    //    });
                    //}
                    //#endregion

                    //#endregion

                    #region Other Details
                    #region PlantCarProvided
                    UpdatedEmployeeTrip.PlantCarProvided = PlantCarProvided;
                    if (PlantCarProvided != EditEmployeeTrips.PlantCarProvided)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripPlantCarProvidedChangeLog").ToString(),
                            EditEmployeeTrips.PlantCarProvided.ToString(), PlantCarProvided.ToString())
                        });
                    }
                    #endregion

                    #region MobilePhoneProvided
                    UpdatedEmployeeTrip.MobilePhoneProvided = MobilePhoneProvided;
                    if (MobilePhoneProvided != EditEmployeeTrips.MobilePhoneProvided)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripMobilePhoneProvidedChangeLog").ToString(),
                            EditEmployeeTrips.MobilePhoneProvided.ToString(), MobilePhoneProvided.ToString())
                        });
                    }
                    #endregion

                    #region SimCardProvided
                    UpdatedEmployeeTrip.SimCardProvided = SimCardProvided;
                    if (SimCardProvided != EditEmployeeTrips.SimCardProvided)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripSimCardProvidedChangeLog").ToString(),
                            EditEmployeeTrips.SimCardProvided.ToString(), SimCardProvided.ToString())
                        });
                    }
                    #endregion

                    #region MoneyDeliveredAtOrigin
                    UpdatedEmployeeTrip.MoneyDeliveredAtOrigin = MoneyDeliveredAtOrigin;
                    if (MoneyDeliveredAtOrigin != EditEmployeeTrips.MoneyDeliveredAtOrigin)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripMoneyDeliveredAtOriginChangeLog").ToString(),
                            EditEmployeeTrips.MoneyDeliveredAtOrigin.ToString(), MoneyDeliveredAtOrigin.ToString())
                        });
                    }
                    #endregion

                    #region SelectedCarAsset
                    UpdatedEmployeeTrip.SelectedCarAsset = SelectedCarAsset;
                    var oldCar = CarAssetsList.FirstOrDefault(x => x.IdAsset == EditEmployeeTrips.IdVehicle);
                    if (SelectedCarAsset?.IdAsset != oldCar?.IdAsset)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripCarAssetChangeLog").ToString(),
                             (string.IsNullOrEmpty(oldCar?.PublicIdentifier) || oldCar?.PublicIdentifier == "---" ? "None" : oldCar?.PublicIdentifier),
                            (string.IsNullOrEmpty(SelectedCarAsset?.PublicIdentifier) || SelectedCarAsset?.PublicIdentifier == "---" ? "None" : SelectedCarAsset?.PublicIdentifier))
                        });
                    }
                    #endregion

                    #region SelectedMobileAsset
                    UpdatedEmployeeTrip.SelectedMobileAsset = SelectedMobileAsset;
                    var oldMobile = MobilePhoneAssetList.FirstOrDefault(x => x.IdAsset == EditEmployeeTrips.IdPhone);
                    if (SelectedMobileAsset?.IdAsset != oldMobile?.IdAsset)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripMobileAssetChangeLog").ToString(),
                             (string.IsNullOrEmpty(oldMobile?.PublicIdentifier) || oldMobile?.PublicIdentifier == "---" ? "None" : oldMobile?.PublicIdentifier),
                            (string.IsNullOrEmpty(SelectedMobileAsset?.PublicIdentifier) || SelectedMobileAsset?.PublicIdentifier == "---" ? "None" : SelectedMobileAsset?.PublicIdentifier))
                        });
                    }
                    #endregion

                    #region SelectedSimCardAsset
                    UpdatedEmployeeTrip.SelectedSimCardAsset = SelectedSimCardAsset;
                    var oldSimCardAsset = SimCardAssetList.FirstOrDefault(x => x.IdAsset == EditEmployeeTrips.IdSimCard);
                    if (SelectedSimCardAsset?.IdAsset != oldSimCardAsset?.IdAsset)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripSimCardAssetChangeLog").ToString(),
                             (string.IsNullOrEmpty(oldSimCardAsset?.PublicIdentifier) || oldSimCardAsset?.PublicIdentifier == "---" ? "None" : oldSimCardAsset?.PublicIdentifier),
                            (string.IsNullOrEmpty(SelectedSimCardAsset?.PublicIdentifier) || SelectedSimCardAsset?.PublicIdentifier == "---" ? "None" : SelectedSimCardAsset?.PublicIdentifier))
                        });
                    }
                    #endregion

                    #region MoneyDeliveredAtDestination
                    UpdatedEmployeeTrip.MoneyDeliveredAtDestination = MoneyDeliveredAtDestination;
                    if (MoneyDeliveredAtDestination != EditEmployeeTrips.MoneyDeliveredAtDestination)
                    {
                        UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                        {
                            IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                            DateTimeChangeLog = DateTime.Now,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("EditEmptripMoneyDeliveredAtDestinationChangeLog").ToString(),
                            EditEmployeeTrips.MoneyDeliveredAtDestination.ToString(), MoneyDeliveredAtDestination.ToString())
                        });
                    }
                    #endregion
                    UpdatedEmployeeTrip.IdModifier = (UInt32)GeosApplication.Instance.ActiveUser.IdUser;
                    #endregion

                    #region Atachment [GEOS2-5932] [rdixit][23.09.2024]
                    if (ListAttachment?.Count > 0)
                    {
                        UpdatedEmployeeTrip.ListAttachment = ListAttachment.ToList();
                        foreach (var item in ListAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add))
                        {
                            UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                            {
                                IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                DateTimeChangeLog = DateTime.Now,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("TripChangeLogAttachmentAdd").ToString(), item.FileName)
                            });
                        }
                    }
                    if (AttachmentFilesToDelete?.Count > 0)
                    {
                        UpdatedEmployeeTrip.AttachmentFilesToDelete = AttachmentFilesToDelete.ToList();
                        foreach (var item in AttachmentFilesToDelete)
                        {
                            UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                            {
                                IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                DateTimeChangeLog = DateTime.Now,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("TripChangeLogAttachmentDelete").ToString(), item.FileName)
                            });
                        }
                    }

                    //[GEOS2-5932][rdixit][23.09.2024]                  
                    foreach (var item in ListAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Update))
                    {
                        var temp = OriginalListAttachment.FirstOrDefault(i => i.IdAttachment == item.IdAttachment);
                        if (temp != null)
                        {
                            if (item.FileName != temp.FileName)
                            {
                                item.TransactionOperation = Data.Common.ModelBase.TransactionOperations.Update;
                                UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                                {
                                    IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTimeChangeLog = DateTime.Now,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("TripChangeLogFilesNameUpdate").ToString(), item.FileName,
                                    string.IsNullOrEmpty(temp.FileName) ? "None" : temp.FileName, string.IsNullOrEmpty(item.FileName) ? "None" : item.FileName)
                                });
                            }
                            if (item.Description != temp.Description)
                            {
                                item.TransactionOperation = Data.Common.ModelBase.TransactionOperations.Update;
                                UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                                {
                                    IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTimeChangeLog = DateTime.Now,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("TripChangeLogFilesDescriptionUpdate").ToString(), item.FileName,
                                    string.IsNullOrEmpty(temp.Description) ? "None" : temp.Description, string.IsNullOrEmpty(item.Description) ? "None" : item.Description)
                                });
                            }

                            if ((item.TripAttachmentType?.Value != temp.TripAttachmentType?.Value))
                            {
                                item.TransactionOperation = Data.Common.ModelBase.TransactionOperations.Update;
                                UpdatedEmployeeTrip.LogEntriesList.Add(new LogEntriesByEmployeeTrip()
                                {
                                    IdUser = (UInt32)GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTimeChangeLog = DateTime.Now,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("TripChangeLogFilesTypeUpdate").ToString(), item.FileName,
                                     string.IsNullOrEmpty(temp.TripAttachmentType?.Value) ? "None" : temp.TripAttachmentType?.Value, string.IsNullOrEmpty(item.TripAttachmentType?.Value) ? "None" : item.TripAttachmentType?.Value)
                                });
                            }
                        }
                    }

                    #endregion
                    //[rdixit][GEOS2-5930][20.09.2024]
                    //[GEOS2-6760][rdixit][09.01.2025]
                    //UpdatedEmployeeTrip = HrmService.EditEmployeeTripDetails_V2600(UpdatedEmployeeTrip);
                    //HrmService = new HrmServiceController("localhost:6699");
                    //UpdatedEmployeeTrip = HrmService.EditEmployeeTripDetails_V2650(UpdatedEmployeeTrip);  // [pallavi.kale][28-05-2025][GEOS2-7941]
                    //HrmService = new HrmServiceController("localhost:6699");
                    if (TransfersList == null)
                    {
                        TransfersList = new ObservableCollection<Employee_trips_transfers>();
                    }
                    if (AccommodationsList == null)
                    {
                        AccommodationsList = new ObservableCollection<EmployeeTripsAccommodations>();
                    }
                    UpdatedEmployeeTrip = HrmService.EditEmployeeTripDetails_V2670(UpdatedEmployeeTrip, TransfersList.ToList(), AccommodationsList.ToList());   //[Rahul.Gadhave][GEOS2-7989][Date:22-09-2025]
                    //HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    IsSaveChanges = true;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TripUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    if (IsStatusChanged)//[Sudhir.Jangra][GEOS2-5933]
                    {
                        try
                        {
                            #region 
                            double workingDaysAmount = 25.00;
                            double weekendAmount = 35.00;
                            string currency = "EUR";

                            string workingDaysAmountWithCurrency = workingDaysAmount + " " + currency;
                            string weekendAmountWithCurrency = weekendAmount + " " + currency;

                            //Emdep Logo
                            EmployeeTripStatusEmailReport employeeTripStatusEmailReport = new EmployeeTripStatusEmailReport();
                            employeeTripStatusEmailReport.imgLogo.Image = ResizeImage(new Bitmap(Emdep.Geos.Modules.Hrm.Properties.Resources.Emdep_logo_mini));

                            //Header
                            employeeTripStatusEmailReport.xrHeaderText.Text = "REQUEST OF WORKERS FROM OTHER PLANTS PROTOCOL";

                            //Requested By
                            employeeTripStatusEmailReport.xrRequestedbyText.Text = "testing";

                            //Plant
                            employeeTripStatusEmailReport.xrPlantText.Text = "EAES";

                            //From
                            employeeTripStatusEmailReport.xrFromText.Text = "Spain";

                            //To
                            employeeTripStatusEmailReport.xrToText.Text = "India";

                            //Department
                            employeeTripStatusEmailReport.xrDepartmentText.Text = "Engineering";

                            //Work Time
                            employeeTripStatusEmailReport.xrWorkTimeText.Text = "10:18";

                            //Weekend
                            employeeTripStatusEmailReport.xrWeekendText.Text = "Sunday";

                            //Purpose
                            employeeTripStatusEmailReport.xrPurposeText.Text = "Random";

                            //Work Assigned Name
                            employeeTripStatusEmailReport.xrWorkerNameText.Text = "Sudhir";

                            //Work Assigned Plant
                            employeeTripStatusEmailReport.xrWorkerPlantText.Text = "EPIN";

                            //Worker Phone Provided
                            employeeTripStatusEmailReport.xrWorkerPhoneProvidedText.Text = "No";

                            //Worker Phone Number
                            employeeTripStatusEmailReport.xrWorkerPhoneNumberText.Text = "+91 9874561231";

                            // Trip Travel Method
                            employeeTripStatusEmailReport.xrTripTravelMethodText.Text = "Air";

                            //Trip Arrival Date
                            employeeTripStatusEmailReport.xrTripArrivalDateText.Text = "10/09/2024";

                            //Trip Arrival Time
                            employeeTripStatusEmailReport.xrTripArrivalTimeText.Text = "10:18";

                            //Trip Arrival Travel Method
                            employeeTripStatusEmailReport.xrTripArrivalTravelMethodText.Text = "Yes";

                            //Trip Departure Date
                            employeeTripStatusEmailReport.xrTripDepartureDateText.Text = "20/09/2024";

                            //Trip Departure Time
                            employeeTripStatusEmailReport.xrTripDepartureTimeText.Text = "18:10";

                            //Trip Departure Travel Method
                            employeeTripStatusEmailReport.xrTripDepartureTravelMethodText.Text = "NO";

                            //Transfer On Arrival
                            employeeTripStatusEmailReport.xrTransferonArrivalText.Text = "Air";

                            //Transfer On Arrival Provider Name
                            employeeTripStatusEmailReport.xrArrivalProviderNameText.Text = "Pramod";

                            //Transfer On Arrival Provider Contact
                            employeeTripStatusEmailReport.xrArrivalProviderContactText.Text = "+91 569872236";

                            //Transfer On Departure
                            employeeTripStatusEmailReport.xrTransferonDepartureText.Text = "Air";

                            //Transfer On Departure Provider Name
                            employeeTripStatusEmailReport.xrDepartureProviderNameText.Text = "Pramod";

                            //Transfer On Departure Provider Contact
                            employeeTripStatusEmailReport.xrDepartureProviderContactText.Text = "+91 569872236";

                            //Transfer Hosting Address
                            employeeTripStatusEmailReport.xrTransferHostingAddressText.Text = "Hinjewadi Phase 1, Pune, Maharastra, India";

                            //Transfer Car Provided
                            employeeTripStatusEmailReport.xrTransferCarProvidedText.Text = "Yes";

                            //Transfer Transport to Plant
                            employeeTripStatusEmailReport.xrTransferToPlantText.Text = "NO";

                            //Information Meals
                            employeeTripStatusEmailReport.xrMealsText.Text = "You will be given you" + " " + workingDaysAmountWithCurrency + " " + " / Working Days + Lunch Meal Ticket and" + " " + weekendAmountWithCurrency + " " + "/ Weekend or Public Holiday.";

                            //Information Meals Other
                            employeeTripStatusEmailReport.xrMealsOtherText.Text = "All that expenses must be covered with a ticket or invoice and the difference will be returned to the company.It is imperative that all expenses are closed at each plant. It means that every balance of expenses should be returned to the company or paid to the worker on the same plant where the money has been provided.";
                            //Information Addtional Information
                            employeeTripStatusEmailReport.xrAdditionalInformationText.Text = "take into account during his trip:";
                            //INformation Additional Information For Eaxample
                            employeeTripStatusEmailReport.xrForExampleText.Text = "For example, if the worker has some health problem. This information is optional, but the company believes that this could be very important for the welfare of the worker.";
                            //Information Protential Problems
                            employeeTripStatusEmailReport.xrPotentialProblemText.Text = "Potential problems during the trip: If you have any problems during the trip and you can't contact with our Plant telephone because you are out of working hours, such as:";
                            //Information Lost of Flight
                            employeeTripStatusEmailReport.xrLossofFlightText.Text = "Loss of flight: Please, go to the aerial company helpdesk or contact by phone to it so that they can relocate you in a new flight. Whether it’s his fault, as the airline or customs control, you should try to get this relocation. If you claim for it, surely you get it.";

                            //Information 2nd line
                            employeeTripStatusEmailReport.xr2lineText.Text = "If you don't get this relocation and you need to buy a new flight, be accommodated in a hotel (because until next day there aren't new flights), pay it with the money provided by your plant.";

                            //Information 3rd line
                            employeeTripStatusEmailReport.xr3lineText.Text = "If you don't have enough money for it, please contact with your Plant Manager and he will solve the situation by himself or contacting the CEO, GM or COO.";

                            //Information 4rd line
                            employeeTripStatusEmailReport.xr4lineText.Text = "In addition to these actions, you must contact with the driver who picks you up at the airport in order to inform him of this issue.";


                            //Date 
                            employeeTripStatusEmailReport.xrDateText.Text = "21/09/2024";
                            #endregion
                            DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };

                            window.PreviewControl.DocumentSource = employeeTripStatusEmailReport;
                            employeeTripStatusEmailReport.CreateDocument();
                            window.Show();
                            IsBusy = false;

                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenWorkflowDiagramCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }

                    }

                    RequestClose(null, null);
                    #endregion
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][16-09-2024][GEOS2-5931]
        private void SaveAttachmentFileToServer(string sourceFilePath, string destinationFilePath, string fileName)
        {
            const int partSize = 1048576; // 1 MB
            byte[] buffer = new byte[partSize];
            FileInfo fileInfo = new FileInfo(Path.Combine(sourceFilePath, fileName));
            HrmService.StartSavingTripAttachmentFile(destinationFilePath, fileInfo.Name);

            using (FileStream fileStream = new FileStream(Path.Combine(sourceFilePath, fileName), FileMode.Open, FileAccess.Read))
            {
                int bytesRead;
                while ((bytesRead = fileStream.Read(buffer, 0, partSize)) > 0)
                {
                    HrmService.SaveTripAttachmentPartData(destinationFilePath, fileName, buffer.Take(bytesRead).ToArray());
                }
            }
        }
        private void EnableDisableControls()
        {
            //[rdixit][25.09.2024][GEOS2-6476]
            if (SelectedTraveller == null)
            {
                IsCustomTravelerEnable = true;
                IsEmployeeTravelerEnable = false;
            }
            else
            {
                IsCustomTravelerEnable = false;
                IsEmployeeTravelerEnable = true;
            }
        }
        // [nsatpute][16-09-2024][GEOS2-5931]
        private void AddAttachmentFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AddAttachmentFileCommandAction..."), category: Category.Info, priority: Priority.Low);
                if (ListAttachment == null)
                    ListAttachment = new ObservableCollection<TripAttachment>();
                AddAttachmentInTripsView addAttachmentInTripsView = new AddAttachmentInTripsView();
                AddAttachmentInTripsViewModel addAttachmentInTripsViewModel = new AddAttachmentInTripsViewModel(ListAttachment, AttachmentTypeList, UpdatedFileList);
                addAttachmentInTripsViewModel.AddInit();// [pallavi.kale][28-05-2025][GEOS2-7941]
                addAttachmentInTripsView.DataContext = addAttachmentInTripsViewModel;
                EventHandler handle = delegate { addAttachmentInTripsView.Close(); };
                addAttachmentInTripsViewModel.RequestClose += handle;
                addAttachmentInTripsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddFileHeader").ToString();
                addAttachmentInTripsViewModel.IsNew = true;
                addAttachmentInTripsView.ShowDialog();
                ListAttachment = addAttachmentInTripsViewModel.ListAttachment;
                GeosApplication.Instance.Logger.Log(string.Format("Method AddAttachmentFileCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddAttachmentFileCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][11-09-2024][GEOS2-5929]
        // [001] [cpatil][28-10-2025][GEOS2-8344]
        private void FillExistiongTripData(EmployeeTrips EditEmployeeTrips)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillExistiongTripData()...", category: Category.Info, priority: Priority.Low);

                IdEmployeeTrip = EditEmployeeTrips.IdEmployeeTrip;
                Code = EditEmployeeTrips.Code;
                Title = EditEmployeeTrips.Title;
                //[GEOS2-6760][rdixit][14.01.2025]
                SelectedType = TypeList.FirstOrDefault(x => x.IdLookupValue == EditEmployeeTrips.IdTripType);
                FillDestination();
                ReFillOriginDestination();
                SelectedPropose = ProposeList.FirstOrDefault(x => x.IdLookupValue == EditEmployeeTrips.IdTripPropose);
                WorkflowStatus = WorkflowStatusList.FirstOrDefault(x => x.IdWorkflowStatus == EditEmployeeTrips.IdStatus);
                SelectedArrivalTransport = MainTransportList.FirstOrDefault(x => x.IdLookupValue == EditEmployeeTrips.IdMainTransport);
                SelectedOrigin = OriginList?.FirstOrDefault(x => x.IdCompany == EditEmployeeTrips.IdOriginPlant);
                SelectedDestination = DestinationList?.FirstOrDefault(x => x.IdDestination == EditEmployeeTrips.IdDestination);
                FillTravellerList();
                if (TravellerList != null)
                {
                    SelectedTraveller = TravellerList.FirstOrDefault(x => x.IdEmployee == EditEmployeeTrips.IdEmployee);

                    if (SelectedTraveller == null)
                    {
                        // [001] for inactive employee 
                        if (HrmCommon.Instance.TravellerList.Any(i => i.IdEmployee == EditEmployeeTrips.IdEmployee))
                        {
                            TravellerList.Add(HrmCommon.Instance.TravellerList.FirstOrDefault(x => x.IdEmployee == EditEmployeeTrips.IdEmployee));
                            TravellerList=new ObservableCollection<Traveller>( TravellerList.OrderBy(x => x.FullName));
                            SelectedTraveller = TravellerList.FirstOrDefault(x => x.IdEmployee == EditEmployeeTrips.IdEmployee);
                            IsTravellerSamePlant = false;
                        }
                        else
                        {
                            IsTravellerSamePlant = true;
                            PreviousTravellerFullName = EditEmployeeTrips.FullName;
                        }
                    }
                    else
                    {
                        IsTravellerSamePlant = false;
                    }
                }
                ArrivalDate = (DateTime)EditEmployeeTrips.ArrivalDate;
                DepartureDate = (DateTime)EditEmployeeTrips.DepartureDate;
                ArrivalDateHours = (new TimeSpan(ArrivalDate.Hour, ArrivalDate.Minute, 0));
                DepartureDateHours = new TimeSpan(DepartureDate.Hour, DepartureDate.Minute, 0);
                DurationDays = Convert.ToInt32((DepartureDate - ArrivalDate).Days + 1);
                SelectedTripCurrency = TripCurrencyList.FirstOrDefault(x => x.IdCurrency == EditEmployeeTrips.IdCurrency);
                SelectedAccommodationType = AccommodationTypeList.FirstOrDefault(x => x.IdLookupValue == EditEmployeeTrips.IdAcommodationtype);
                AccommodationDetails = EditEmployeeTrips.AccomodationDetails;
                Remarks = EditEmployeeTrips.Remarks;
                ArrivalTransporterName = EditEmployeeTrips.ArrivalTransporterName;
                ArrivalTransferRemark = EditEmployeeTrips.ArrivalTransferRemark;
                ArrivalProvider = EditEmployeeTrips.ArrivalProvider;
                ArrivalTransporterContact = EditEmployeeTrips.ArrivalTransporterContact;
                SelectedDepartureTransport = MainTransportList.FirstOrDefault(x => x.IdLookupValue == EditEmployeeTrips.IdDepartureTransport);
                DepartureTransporterName = EditEmployeeTrips.DepartureTransporterName;
                DepartureTransferRemark = EditEmployeeTrips.DepartureTransferRemark;
                DepartureProvider = EditEmployeeTrips.DepartureProvider;
                DepartureTransporterContact = EditEmployeeTrips.DepartureTransporterContact;
                AccommodationAddress = EditEmployeeTrips.AccomodationAddress;
                AccommodationRemarks = EditEmployeeTrips.AccommodationRemarks;
                PartnerProvidedRoom = EditEmployeeTrips.PartnerProvidedRoom;
                AccommodationOtherRoom = EditEmployeeTrips.AccommodationOtherRoom;
                AccommodationCoordinates = EditEmployeeTrips.AccommodationCoordinates;
                PlantCarProvided = EditEmployeeTrips.PlantCarProvided;
                MobilePhoneProvided = EditEmployeeTrips.MobilePhoneProvided;
                SimCardProvided = EditEmployeeTrips.SimCardProvided;
                MoneyDeliveredAtOrigin = EditEmployeeTrips.MoneyDeliveredAtOrigin;
                SelectedCarAsset = CarAssetsList.FirstOrDefault(x => x.IdAsset == EditEmployeeTrips?.SelectedCarAsset?.IdAsset);
                SelectedSimCardAsset = CarAssetsList.FirstOrDefault(x => x.IdAsset == EditEmployeeTrips?.SelectedSimCardAsset?.IdAsset);
                SelectedMobileAsset = CarAssetsList.FirstOrDefault(x => x.IdAsset == EditEmployeeTrips?.SelectedMobileAsset?.IdAsset);
                MoneyDeliveredAtDestination = EditEmployeeTrips.MoneyDeliveredAtDestination;
                SelectedWorkShift = WorkShiftList.FirstOrDefault(x => x.IdLookupValue == EditEmployeeTrips.IdWorkShift);
                if (SelectedTraveller == null || SelectedTraveller.IdEmployee == 0)
                    EditEmployeeTrips.CustomTraveler = EditEmployeeTrips.FirstName;
                CustomTraveler = EditEmployeeTrips.CustomTraveler;
                if (EditEmployeeTrips.TravelerEmail != null) // [nsatpute][22-10-2024][GEOS2-6656]
                    TravelerEmail = EditEmployeeTrips.TravelerEmail;
                Weekend = EditEmployeeTrips.IsWeekend;
                Desciption = EditEmployeeTrips.Desciption;
                FromDate = EditEmployeeTrips.FromDate;
                ToDate = EditEmployeeTrips.ToDate;
                VisaRequired = EditEmployeeTrips.VisaRequired;
                ArrivalTransportationNumber = EditEmployeeTrips.ArrivalTransportationNumber;
                DepartureTransportationNumber = EditEmployeeTrips.DepartureTransportationNumber;
                SelectedArrivalTransportationType = TransportationTypeList.FirstOrDefault(x => x.IdLookupValue == EditEmployeeTrips.ArrivalTransportationType);
                SelectedDepartureTransportationType = TransportationTypeList.FirstOrDefault(x => x.IdLookupValue == EditEmployeeTrips.DepartureTransportationType);
                SelectedArrivalTransport = MainTransportList.FirstOrDefault(x => x.IdLookupValue == EditEmployeeTrips.IdArrivalTransport);
                SelectedDepartureTransport = MainTransportList.FirstOrDefault(x => x.IdLookupValue == EditEmployeeTrips.IdDepartureTransport);
                ArrivalTransferRemark = EditEmployeeTrips.ArrivalTransferRemark;
                DepartureTransferRemark = EditEmployeeTrips.DepartureTransferRemark;
                SelectedCarAsset = CarAssetsList.FirstOrDefault(x => x.IdAsset == EditEmployeeTrips.IdVehicle);
                SelectedMobileAsset = MobilePhoneAssetList.FirstOrDefault(x => x.IdAsset == EditEmployeeTrips.IdPhone);
                SelectedSimCardAsset = SimCardAssetList.FirstOrDefault(x => x.IdAsset == EditEmployeeTrips.IdSimCard);
                SelectedWorkShift = WorkShiftList.FirstOrDefault(x => x.IdLookupValue == EditEmployeeTrips.IdWorkShift);
                if (!string.IsNullOrEmpty(EditEmployeeTrips.EmdepRoom))
                {
                    EditEmployeeTrips.IdAccommodation = Convert.ToInt32(EditEmployeeTrips.EmdepRoom);
                    SelectedAccommodationAsset = AccommodationAssetList?.FirstOrDefault(i => i.IdAsset == EditEmployeeTrips.IdAccommodation);
                }
                FillResponsible();//[GEOS2-6760][rdixit][14.01.2025]
                SelectedResponsible = ResponsibleList?.FirstOrDefault(x => x.IdEmployee == EditEmployeeTrips.IdResponsible);
                GeosApplication.Instance.Logger.Log("Method FillExistiongTripData()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExistiongTripData() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExistiongTripData() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillFillExistiongTripDataTripAssets() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[GEOS2-6760][rdixit][14.01.2025]
        // [001] [cpatil][28-10-2025][GEOS2-8344]
        void FillTravellerList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTravellerList()...", category: Category.Info, priority: Priority.Low);
                if (HrmCommon.Instance.TravellerList == null)
                    HrmCommon.Instance.TravellerList = new List<Traveller>();
                TravellerList = new ObservableCollection<Traveller>();
                if (SelectedType?.Value == "Customer to EMDEP" || SelectedType?.Value == "Provider to EMDEP")
                {
                    if (SelectedDestination != null)
                    {
                        var temp = HrmService.GetActiveEmployeesForTraveller_V2600(SelectedDestination.IdDestination.ToString(),
                                     HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization,
                                     HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission);

                        if (temp.Any())
                        {
                            HrmCommon.Instance.TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                            HrmCommon.Instance.TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus != 136).OrderBy(x => x.FullName));
                            //TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus != 136).OrderBy(x => x.FullName));// [001]
                            TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                        }
                    }
                }
                else if (SelectedType?.Value == "EMDEP to Plant")
                {
                    string companies = (SelectedDestination != null ? SelectedDestination.IdDestination.ToString() : string.Empty) +
                        (SelectedDestination != null && SelectedOrigin != null ? "," : string.Empty) +
                        (SelectedOrigin != null ? SelectedOrigin.IdCompany.ToString() : string.Empty);

                    var temp = HrmService.GetActiveEmployeesForTraveller_V2600(companies,
                                     HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization,
                                     HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission);
                    if (temp.Any())
                    {
                        HrmCommon.Instance.TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                        HrmCommon.Instance.TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus != 136).OrderBy(x => x.FullName));
                        //TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus != 136).OrderBy(x => x.FullName));// [001]
                        TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                    }
                }
                else
                {
                    if (SelectedOrigin != null)
                    {
                        var temp = HrmService.GetActiveEmployeesForTraveller_V2600(SelectedOrigin.IdCompany.ToString(),
                                  HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization,
                                  HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission);

                        if (temp.Any())
                        {
                            HrmCommon.Instance.TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                           HrmCommon.Instance.TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus != 136).OrderBy(x => x.FullName));
                            // TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus != 136).OrderBy(x => x.FullName));// [001]
                            TravellerList.AddRange(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillTravellerList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillTravellerList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[GEOS2-6760][rdixit][14.01.2025]
        void FillResponsible()
        {
            try
            {
                UInt32 idEmp = SelectedTraveller == null ? (UInt32)0 : Convert.ToUInt32(SelectedTraveller.IdEmployee);
                UInt32 idDept = SelectedDepartment == null ? (UInt32)0 : Convert.ToUInt32(SelectedDepartment.IdDepartment);
                GeosApplication.Instance.Logger.Log("Method FillResponsible()...", category: Category.Info, priority: Priority.Low);
                if (SelectedType?.Value == "Customer to EMDEP" || SelectedType?.Value == "Provider to EMDEP")
                {
                    if (SelectedDestination != null)
                    {
                        ResponsibleList = new ObservableCollection<Employee>(
                        HrmService.HRM_GetResponsibleByPlantAndIdEmployee_V2600(
                            SelectedDestination.IdDestination.ToString(), idEmp, idDept));
                    }
                }
                else if (SelectedType?.Value == "EMDEP to Plant")
                {
                    string companies = (SelectedDestination != null ? SelectedDestination.IdDestination.ToString() : string.Empty) +
                        (SelectedDestination != null && SelectedOrigin != null ? "," : string.Empty) +
                        (SelectedOrigin != null ? SelectedOrigin.IdCompany.ToString() : string.Empty);
                    ResponsibleList = new ObservableCollection<Employee>(
                     HrmService.HRM_GetResponsibleByPlantAndIdEmployee_V2600(companies, idEmp, idDept));

                }
                else
                {
                    if (SelectedOrigin != null)
                    {
                        ResponsibleList = new ObservableCollection<Employee>(
                            HrmService.HRM_GetResponsibleByPlantAndIdEmployee_V2600(SelectedOrigin.IdCompany.ToString(), idEmp, idDept));
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillResponsible()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillResponsible() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OpenWorkflowDiagramCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenWorkflowDiagramCommandAction()...", category: Category.Info, priority: Priority.Low);
                HRMTripDetailsWorkflowDiagramViewModel hrmTripDetailsWorkflowDiagramViewModel = new HRMTripDetailsWorkflowDiagramViewModel();
                HRMTripDetailsWorkflowDiagramView hrmTripDetailsWorkflowDiagramView = new HRMTripDetailsWorkflowDiagramView();
                EventHandler handle = delegate { hrmTripDetailsWorkflowDiagramView.Close(); };
                hrmTripDetailsWorkflowDiagramViewModel.RequestClose += handle;
                hrmTripDetailsWorkflowDiagramViewModel.TripRequestStatusList = TripRequestStatusList;
                hrmTripDetailsWorkflowDiagramViewModel.WorkflowTransitionList = WorkflowTransitionList.OrderByDescending(a => a.IdWorkflowTransition).ToList();
                hrmTripDetailsWorkflowDiagramView.DataContext = hrmTripDetailsWorkflowDiagramViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                hrmTripDetailsWorkflowDiagramView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method OpenWorkflowDiagramCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenWorkflowDiagramCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][16-09-2024][GEOS2-5931]
        public void DownloadFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                bool isDownload = false;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["AttachmentsFileDownloadMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    TripAttachment attachmentObject = (TripAttachment)obj;
                    SaveFileDialogService saveFileDialogService = new SaveFileDialogService();
                    saveFileDialogService.DefaultExt = Path.GetExtension(attachmentObject.FileName);
                    saveFileDialogService.DefaultFileName = attachmentObject.FileName;
                    saveFileDialogService.Filter = "All Files|*.*";
                    saveFileDialogService.FilterIndex = 1;
                    DialogResult = saveFileDialogService.ShowDialog();

                    if (!DialogResult)
                    {
                        ResultFileName = string.Empty;
                    }
                    else
                    {
                        IsBusy = true;
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
                                //WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                                win.Topmost = false;
                                return win;
                            }, x =>
                            {
                                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                            }, null, null);
                        }

                        Byte[] byteObj = null;

                        if (attachmentObject.FilePath == null)
                            byteObj = HrmService.GetTripAttachmentFile(Code, attachmentObject.FileName);
                        else
                            byteObj = File.ReadAllBytes(attachmentObject.FilePath);

                        ResultFileName = saveFileDialogService.GetFullFileName();
                        isDownload = SaveData(ResultFileName, byteObj);
                    }
                    if (isDownload)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AttachmentsFileDownloadSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                        IsBusy = false;
                    }
                    else
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AttachmentsFileDownloadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        IsBusy = false;
                    }
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    IsBusy = false;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][16-09-2024][GEOS2-5931]
        private void DeleteFileAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteFileAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteProfDocsMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    isAttachmentModified = true;
                    TripAttachment attachment = ListAttachment.FirstOrDefault(x => x.FileName == ((TripAttachment)obj).FileName);
                    if (attachment != null)
                    {
                        AttachmentFilesToDelete.Add(attachment);
                        ListAttachment.Remove(attachment);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DeleteFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteFile()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][24-09-2024][GEOS2-6473]
        private void EditFileCommandAction(object obj)
        {
            try
            {
                if (IsAllControlDisabled)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Tripdetailsattachment_ModifyingRestricted").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method EditFileAction..."), category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                TripAttachment attachment = (TripAttachment)detailView.DataControl.CurrentItem;
                int selectedIndex = ListAttachment.IndexOf(attachment);
                AddAttachmentInTripsView addAttachmentInTripsView = new AddAttachmentInTripsView();
                AddAttachmentInTripsViewModel addAttachmentInTripsViewModel = new AddAttachmentInTripsViewModel(ListAttachment, attachmentTypeList, UpdatedFileList);
                addAttachmentInTripsView.DataContext = addAttachmentInTripsViewModel;
                EventHandler handle = delegate { addAttachmentInTripsView.Close(); };
                addAttachmentInTripsViewModel.RequestClose += handle;
                addAttachmentInTripsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditFileHeader").ToString();
                addAttachmentInTripsViewModel.IsNew = false;
                addAttachmentInTripsViewModel.EditInit(attachment.DeepClone(), selectedIndex);
                var ownerInfo = (detailView as FrameworkElement);
                addAttachmentInTripsView.Owner = Window.GetWindow(ownerInfo);
                addAttachmentInTripsView.ShowDialog();
                SelectedAttachment = ListAttachment[selectedIndex];
                GeosApplication.Instance.Logger.Log(string.Format("Method EditFileAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                isAttachmentModified = true;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][24-09-2024][GEOS2-6473]
        private void WorkflowButtonClickCommandAction(object obj)
        {
            try
            {
                int status_id = Convert.ToInt32(obj);
                int IdWorkflowStatus = WorkflowStatus.IdWorkflowStatus;
                GeosApplication.Instance.Logger.Log("Method WorkflowButtonClickCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (EditEmployeeTrips != null)
                {
                    EditEmployeeTrips.IdStatus = (uint)status_id;
                    TransitionWorkflowStatus(EditEmployeeTrips);
                }
                GeosApplication.Instance.Logger.Log("Method WorkflowButtonClickCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkflowButtonClickCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkflowButtonClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method WorkflowButtonClickCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][24-09-2024][GEOS2-6473]
        private void StatusCancelButtonCommandAction(object obj)
        {
            try
            {
                int IdWorkflowStatus = WorkflowStatus.IdWorkflowStatus;
                GeosApplication.Instance.Logger.Log("Method StatusCancelButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (EditEmployeeTrips != null)
                {
                    EditEmployeeTrips.IdStatus = 96;
                    TransitionWorkflowStatus(EditEmployeeTrips);
                }
                GeosApplication.Instance.Logger.Log("Method StatusCancelButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in StatusCancelButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in StatusCancelButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method StatusCancelButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillAttachmentTypes()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAttachmentTypes()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempTypeList = HrmService.GetLookupValues(165);
                AttachmentTypeList = new ObservableCollection<LookupValue>(tempTypeList);
                GeosApplication.Instance.Logger.Log("Method FillAttachmentTypes()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAttachmentTypes() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAttachmentTypes() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillAttachmentTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][03-10-2024][GEOS2-6525]
        private void SavechangesInAttachmentGrid()
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["LeaveGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {

                    AcceptButtonCommandAction(null);

                }
                else
                {
                    isAttachmentModified = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        // [nsatpute][22-09-2024][GEOS2-5931]
        protected bool SaveData(string FileName, byte[] Data)
        {
            BinaryWriter Writer = null;
            string Name = FileName;

            try
            {
                // Create a new stream to write to the file
                Writer = new BinaryWriter(File.OpenWrite(Name));

                // Writer raw data
                Writer.Write(Data);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                //...
                return false;
            }

            return true;
        }
        //[Sudhir.Jangra][GEOS2-5933]
        private Bitmap ResizeImage(Bitmap bitmap)
        {
            GeosApplication.Instance.Logger.Log("Method ResizeImage ...", category: Category.Info, priority: Priority.Low);

            Bitmap resized = new Bitmap(128, 45);

            try
            {
                Graphics g = Graphics.FromImage(resized);

                g.DrawImage(bitmap, new Rectangle(0, 0, resized.Width, resized.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);
                g.Dispose();

                //Save picture in users temp folder.
                string myTempFile = Path.Combine(Path.GetTempPath(), "EmdepLogo.jpg");

                //delete if already image exist there.
                if (File.Exists(myTempFile))
                {
                    File.Delete(myTempFile);
                }

                resized.Save(myTempFile, ImageFormat.Jpeg);

                return resized;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ResizeImage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            return resized;
        }
        private static string ConvertHtmlToRtf(string html)
        {
            // Implement conversion if needed
            return html;
        }
        /// <summary>
        /// //[pramod.misal][GEOS2-7989][17-09-2025]https://helpdesk.emdep.com/browse/GEOS2-7989
        /// </summary>
        /// <param name="obj"></param>
        private void EditTransferCommandAction(object obj)
        {
            try
            {
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
                GeosApplication.Instance.Logger.Log("Method EditTransferCommandAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                Employee_trips_transfers employee_trips_transfers = (Employee_trips_transfers)detailView.DataControl.CurrentItem;
                int selectedIndex = TransfersList.IndexOf(employee_trips_transfers);
                AddEditTransferView addEditTransferView = new AddEditTransferView();
                AddEditTransferViewModel addEditTransferViewModel = new AddEditTransferViewModel();
                EventHandler handle = delegate { addEditTransferView.Close(); };
                addEditTransferViewModel.RequestClose += handle;
                addEditTransferViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditTransfer").ToString();
                addEditTransferViewModel.IsNew = false;
                if (FromDate != EditEmployeeTrips.FromDate)
                {
                    EditEmployeeTrips.FromDate = FromDate;
                }
                if (ToDate != EditEmployeeTrips.ToDate)
                {
                    EditEmployeeTrips.ToDate = ToDate;
                }
                //addEditAttachmentViewModel.EditInit(IdProfessionalTraining, professionalTrainingAttachments);
                addEditTransferViewModel.EditInit(employee_trips_transfers, EditEmployeeTrips);
                addEditTransferView.DataContext = addEditTransferViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditTransferView.Owner = Window.GetWindow(ownerInfo);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addEditTransferView.ShowDialog();

                if (addEditTransferViewModel.IsSave)
                {
                    if (addEditTransferViewModel.SelectedTransfer != null)
                    {
                        if (TransfersList == null)
                        {
                            TransfersList = new ObservableCollection<Employee_trips_transfers>();
                        }
                        var existingIndex = TransfersList.ToList().FindIndex(x => x.IdEmployeeTripsTransfers == addEditTransferViewModel.SelectedTransfer.IdEmployeeTripsTransfers);
                        if (existingIndex >= 0)
                        {
                            // Replace the old record with the new one
                            TransfersList[existingIndex] = addEditTransferViewModel.SelectedTransfer;
                        }

                    }
                }

                if (addEditTransferViewModel.IsDuplicat)
                {
                    if (TransfersList == null)
                        TransfersList = new ObservableCollection<Employee_trips_transfers>();
                    foreach (var item in HrmCommon.Instance.TransfersList)
                    {
                        TransfersList.Add(item);
                    }
                    HrmCommon.Instance.TransfersList = new ObservableCollection<Employee_trips_transfers>();
                }



                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditTransferCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditTransferCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// //[pramod.misal][GEOS2-7989][17-09-2025]https://helpdesk.emdep.com/browse/GEOS2-7989
        /// </summary>
        /// <param name="obj"></param>
        private void AddTransferCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddTransferCommandAction()...", category: Category.Info, priority: Priority.Low);

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

                AddEditTransferView addEditTransferView = new AddEditTransferView();
                AddEditTransferViewModel addEditTransferViewModel = new AddEditTransferViewModel();
                EventHandler handle = delegate { addEditTransferView.Close(); };
                addEditTransferViewModel.RequestClose += handle;
                addEditTransferViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddTransfer").ToString();
                addEditTransferViewModel.IsNew = true;
                if (EditEmployeeTrips == null)
                {

                    EditEmployeeTrips = new EmployeeTrips();
                    EditEmployeeTrips.FromDate = FromDate;
                    EditEmployeeTrips.ToDate = ToDate;
                }
                if (FromDate != EditEmployeeTrips.FromDate)
                {
                    EditEmployeeTrips.FromDate = FromDate;
                }
                if (ToDate != EditEmployeeTrips.ToDate)
                {
                    EditEmployeeTrips.ToDate = ToDate;
                }
                addEditTransferViewModel.AddInit(null, IdEmployeeTripNew, EditEmployeeTrips);
                addEditTransferView.DataContext = addEditTransferViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditTransferView.Owner = Window.GetWindow(ownerInfo);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addEditTransferView.ShowDialog();
                if (addEditTransferViewModel.IsSave)
                {
                    if (addEditTransferViewModel.SelectedTransfer != null)
                    {
                        if (TransfersList == null)
                        {
                            TransfersList = new ObservableCollection<Employee_trips_transfers>();
                        }
                        TransfersList.Add(addEditTransferViewModel.SelectedTransfer);
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddTransferCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddTransferCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [shweta.thube][GEOS2-7989][18-09-2025] https://helpdesk.emdep.com/browse/GEOS2-7989
        /// </summary>
        /// <param name="obj"></param>
        private void EditAccommodationCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditAccommodationCommandAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                EmployeeTripsAccommodations SelectedAccomaodation = (EmployeeTripsAccommodations)detailView.DataControl.CurrentItem;
                AddEditAccommodationView addEditAccomodationView = new AddEditAccommodationView();
                AddEditAccommodationViewModel addEditAccommodationViewModel = new AddEditAccommodationViewModel();
                EventHandler handle = delegate { addEditAccomodationView.Close(); };
                addEditAccommodationViewModel.RequestClose += handle;
                addEditAccommodationViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditAccommodation").ToString();
                addEditAccommodationViewModel.IsNew = false;
                addEditAccommodationViewModel.EditInit(SelectedAccomaodation, EditEmployeeTrips);
                addEditAccomodationView.DataContext = addEditAccommodationViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditAccomodationView.Owner = Window.GetWindow(ownerInfo);
                addEditAccomodationView.ShowDialog();

                if (addEditAccommodationViewModel.IsSave)
                {
                    if (addEditAccommodationViewModel.SelectedAccommodations != null)
                    {
                        if (AccommodationsList == null)
                        {
                            AccommodationsList = new ObservableCollection<EmployeeTripsAccommodations>();
                        }
                        var existingIndex = AccommodationsList.ToList().FindIndex(x => x.IdAccommodations == addEditAccommodationViewModel.SelectedAccommodations.IdAccommodations);
                        if (existingIndex >= 0)
                        {
                            // Replace the old record with the new one
                            AccommodationsList[existingIndex] = addEditAccommodationViewModel.SelectedAccommodations;
                        }

                    }
                }



                GeosApplication.Instance.Logger.Log("Method EditAccommodationCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditAccommodationCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [shweta.thube][GEOS2-7989][18-09-2025]https://helpdesk.emdep.com/browse/GEOS2-7989
        /// </summary>
        /// <param name="obj"></param>
        private void AddAccommodationCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddAccommodationCommandAction()...", category: Category.Info, priority: Priority.Low);

                AddEditAccommodationView addEditAccommodationView = new AddEditAccommodationView();
                AddEditAccommodationViewModel addEditAccommodationViewModel = new AddEditAccommodationViewModel();
                EventHandler handle = delegate { addEditAccommodationView.Close(); };
                addEditAccommodationViewModel.RequestClose += handle;
                addEditAccommodationViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddAccommodation").ToString();
                addEditAccommodationViewModel.IsNew = true;
                if (EditEmployeeTrips == null)
                {
                    EditEmployeeTrips = new EmployeeTrips();
                    EditEmployeeTrips.FromDate = FromDate;
                    EditEmployeeTrips.ToDate = ToDate;
                }
                addEditAccommodationViewModel.Init(IdEmployeeTripNew, EditEmployeeTrips);
                addEditAccommodationView.DataContext = addEditAccommodationViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditAccommodationView.Owner = Window.GetWindow(ownerInfo);
                addEditAccommodationView.ShowDialog();
                if (addEditAccommodationViewModel.IsSave)
                {
                    if (addEditAccommodationViewModel.SelectedAccommodations != null)
                    {
                        if (AccommodationsList == null)
                        {
                            AccommodationsList = new ObservableCollection<EmployeeTripsAccommodations>();
                        }
                        AccommodationsList.Add(addEditAccommodationViewModel.SelectedAccommodations);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddAccommodationCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddAccommodationCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                                me[BindableBase.GetPropertyName(() => SelectedTraveller)] +
                                 me[BindableBase.GetPropertyName(() => SelectedType)] +
                                 me[BindableBase.GetPropertyName(() => SelectedPropose)] +
                                 me[BindableBase.GetPropertyName(() => SelectedWorkShift)] +
                                 me[BindableBase.GetPropertyName(() => SelectedOrigin)] +
                                 me[BindableBase.GetPropertyName(() => SelectedDestination)] +
                                 me[BindableBase.GetPropertyName(() => FromDate)] +
                                 me[BindableBase.GetPropertyName(() => ToDate)] +
                                 me[BindableBase.GetPropertyName(() => TravelerEmail)] +
                                 me[BindableBase.GetPropertyName(() => ArrivalTransportationNumber)] +
                                 me[BindableBase.GetPropertyName(() => SelectedArrivalTransportationType)] +
                                 me[BindableBase.GetPropertyName(() => DepartureTransportationNumber)] +
                                 me[BindableBase.GetPropertyName(() => SelectedDepartureTransportationType)] +
                                 //me[BindableBase.GetPropertyName(() => ArrivalTransporterName)] +
                                 //me[BindableBase.GetPropertyName(() => SelectedArrivalTransport)] +
                                 me[BindableBase.GetPropertyName(() => SelectedDestination)] +
                                 //me[BindableBase.GetPropertyName(() => ArrivalProvider)] +
                                 //me[BindableBase.GetPropertyName(() => ArrivalTransporterContact)] +
                                 //me[BindableBase.GetPropertyName(() => SelectedDepartureTransport)] +
                                 //me[BindableBase.GetPropertyName(() => DepartureTransporterName)] +
                                 //me[BindableBase.GetPropertyName(() => DepartureProvider)] +
                                 //me[BindableBase.GetPropertyName(() => DepartureTransporterContact)] +
                                 //me[BindableBase.GetPropertyName(() => SelectedAccommodationType)] +
                                 //me[BindableBase.GetPropertyName(() => AccommodationAddress)] +
                                 me[BindableBase.GetPropertyName(() => SelectedResponsible)];
                //me[BindableBase.GetPropertyName(() => AccommodationCoordinates)


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

                string selectedType = BindableBase.GetPropertyName(() => SelectedType);
                string selectedPropose = BindableBase.GetPropertyName(() => SelectedPropose);
                string selectedTraveller = BindableBase.GetPropertyName(() => SelectedTraveller);
                string travelerEmail = BindableBase.GetPropertyName(() => TravelerEmail);

                #region [rdixit][21.09.2024][GEOS2-5930]
                string customTraveler = BindableBase.GetPropertyName(() => CustomTraveler); //[rdixit][25.09.2024][GEOS2-6476]
                string arrivalTransportationNumber = BindableBase.GetPropertyName(() => ArrivalTransportationNumber);
                string selectedArrivalTransportationType = BindableBase.GetPropertyName(() => SelectedArrivalTransportationType);
                string departureTransportationNumber = BindableBase.GetPropertyName(() => DepartureTransportationNumber);
                string selectedDepartureTransportationType = BindableBase.GetPropertyName(() => SelectedDepartureTransportationType);
                string arrivalTransporterName = BindableBase.GetPropertyName(() => ArrivalTransporterName);
                string selectedArrivalTransport = BindableBase.GetPropertyName(() => SelectedArrivalTransport);
                string selectedDestination = BindableBase.GetPropertyName(() => SelectedDestination);
                string arrivalProvider = BindableBase.GetPropertyName(() => ArrivalProvider);
                string arrivalTransporterContact = BindableBase.GetPropertyName(() => ArrivalTransporterContact);
                string selectedDepartureTransport = BindableBase.GetPropertyName(() => SelectedDepartureTransport);
                string departureTransporterName = BindableBase.GetPropertyName(() => DepartureTransporterName);
                string departureProvider = BindableBase.GetPropertyName(() => DepartureProvider);
                string departureTransporterContact = BindableBase.GetPropertyName(() => DepartureTransporterContact);
                string selectedAccommodationType = BindableBase.GetPropertyName(() => SelectedAccommodationType);
                string accommodationAddress = BindableBase.GetPropertyName(() => AccommodationAddress);
                string accommodationCoordinates = BindableBase.GetPropertyName(() => AccommodationCoordinates);
                string selectedResponsible = BindableBase.GetPropertyName(() => SelectedResponsible);

                if (columnName == selectedType)
                {
                    return AddEditTripValidations.GetErrorMessage(selectedType, SelectedType, string.Empty);
                }
                //[GEOS2-6760][rdixit][14.01.2025]
                if (columnName == selectedResponsible)
                {
                    return AddEditTripValidations.GetErrorMessage(selectedResponsible, SelectedResponsible, string.Empty);
                }
                if (columnName == selectedPropose)
                {
                    return AddEditTripValidations.GetErrorMessage(selectedPropose, SelectedPropose, string.Empty);
                }
                if (columnName == selectedTraveller)
                {
                    return AddEditTripValidations.GetErrorMessage(SelectedTraveller?.FullName, selectedTraveller, CustomTraveler);
                }
                if (columnName == customTraveler)
                {
                    return AddEditTripValidations.GetErrorMessage(customTraveler, CustomTraveler, SelectedTraveller?.FullName);
                }
                if (columnName == travelerEmail)
                {
                    return AddEditTripValidations.GetErrorMessage(travelerEmail, TravelerEmail, SelectedTraveller?.FullName);
                }
                if (columnName == arrivalTransportationNumber)
                {
                    return AddEditTripValidations.GetErrorMessage(arrivalTransportationNumber, ArrivalTransportationNumber, string.Empty);
                }
                if (columnName == selectedArrivalTransportationType)
                {
                    return AddEditTripValidations.GetErrorMessage(selectedArrivalTransportationType, SelectedArrivalTransportationType, string.Empty);
                }
                if (columnName == departureTransportationNumber)
                {
                    return AddEditTripValidations.GetErrorMessage(departureTransportationNumber, DepartureTransportationNumber, string.Empty);
                }
                if (columnName == selectedDepartureTransportationType)
                {
                    return AddEditTripValidations.GetErrorMessage(selectedDepartureTransportationType, SelectedDepartureTransportationType, string.Empty);
                }
                if (columnName == arrivalTransporterName)
                {
                    return AddEditTripValidations.GetErrorMessage(arrivalTransporterName, ArrivalTransporterName, string.Empty);
                }
                if (columnName == selectedArrivalTransport)
                {
                    return AddEditTripValidations.GetErrorMessage(selectedArrivalTransport, SelectedArrivalTransport, string.Empty);
                }
                if (columnName == selectedDestination)
                {
                    return AddEditTripValidations.GetErrorMessage(selectedDestination, SelectedDestination, string.Empty);
                }
                if (columnName == arrivalProvider)
                {
                    return AddEditTripValidations.GetErrorMessage(arrivalProvider, ArrivalProvider, string.Empty);
                }
                if (columnName == arrivalTransporterContact)
                {
                    return AddEditTripValidations.GetErrorMessage(arrivalTransporterContact, ArrivalTransporterContact, string.Empty);
                }
                if (columnName == selectedDepartureTransport)
                {
                    return AddEditTripValidations.GetErrorMessage(selectedDepartureTransport, SelectedDepartureTransport, string.Empty);
                }
                if (columnName == departureTransporterName)
                {
                    return AddEditTripValidations.GetErrorMessage(departureTransporterName, DepartureTransporterName, string.Empty);
                }
                if (columnName == departureProvider)
                {
                    return AddEditTripValidations.GetErrorMessage(departureProvider, DepartureProvider, string.Empty);
                }
                if (columnName == departureTransporterContact)
                {
                    return AddEditTripValidations.GetErrorMessage(departureTransporterContact, DepartureTransporterContact, string.Empty);
                }
                if (columnName == selectedAccommodationType)
                {
                    return AddEditTripValidations.GetErrorMessage(selectedAccommodationType, SelectedAccommodationType, string.Empty);
                }
                if (columnName == accommodationAddress)
                {
                    return AddEditTripValidations.GetErrorMessage(accommodationAddress, AccommodationAddress, string.Empty);
                }
                if (columnName == accommodationCoordinates)
                {
                    return AddEditTripValidations.GetErrorMessage(accommodationCoordinates, AccommodationCoordinates, string.Empty);
                }
                #endregion
                if (columnName == selectedType)
                {
                    return AddEditTripValidations.GetErrorMessage(selectedType, SelectedType, string.Empty);
                }
                if (columnName == selectedPropose)
                {
                    return AddEditTripValidations.GetErrorMessage(selectedPropose, SelectedPropose, string.Empty);
                }
                if (columnName == selectedTraveller && CustomTraveler == "")
                {
                    return AddEditTripValidations.GetErrorMessage(selectedPropose, SelectedTraveller, string.Empty);
                }
                if (columnName == "SelectedWorkShift")
                {
                    return AddEditTripValidations.GetErrorMessage("SelectedWorkShift", SelectedWorkShift, string.Empty);
                }
                if (columnName == "SelectedOrigin" && SelectedOrigin == null)
                {
                    return AddEditTripValidations.GetErrorMessage("SelectedOrigin", SelectedOrigin, string.Empty);
                }
                if (columnName == "SelectedDestination" && SelectedDestination == null)
                {
                    return AddEditTripValidations.GetErrorMessage("SelectedDestination", SelectedDestination, string.Empty);
                }
                if (columnName == "FromDate" && FromDate == null)
                {
                    return AddEditTripValidations.GetErrorMessage("FromDate", FromDate, string.Empty);
                }
                if (columnName == "ToDate" && ToDate == null)
                {
                    return AddEditTripValidations.GetErrorMessage("ToDate", ToDate, string.Empty);
                }
                return null;
            }
        }
        // [nsatpute][22-09-2024][GEOS2-5931]
        private BitmapImage IconFromExtension(string fileExtension)
        {
            // Create a temporary file with the given extension to extract the icon.
            string tempFile = Path.Combine(Path.GetTempPath(), "temp" + fileExtension);

            // Create an empty file with the specified extension if it doesn't exist.
            if (!File.Exists(tempFile))
            {
                File.Create(tempFile).Dispose(); // Create and immediately close the file
            }

            // Extract the associated icon from the file
            Icon icon = Icon.ExtractAssociatedIcon(tempFile);

            // Convert the Icon to a BitmapImage in memory
            if (icon != null)
            {
                using (MemoryStream iconStream = new MemoryStream())
                {
                    icon.Save(iconStream);
                    iconStream.Seek(0, SeekOrigin.Begin); // Reset the stream position

                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = iconStream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();

                    // Cleanup temp file
                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }

                    return bitmapImage;
                }
            }

            return null; // Return null if no icon is found
        }

        /// <summary>
        /// If any feild is of Information has error set isInformationError = true;
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>

        #endregion
    }
}
