using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.SRM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{

    //[pramod.misal][GEOS2-4815][]

    [Table("employee_Trips")]
    [DataContract]
    public class EmployeeTrips : ModelBase, IDisposable
    {
        #region Fields
        LookupValue deptTransportationType;
        LookupValue arrTransportationType;
        int idEmployeeTrip;
        string code;
        string traveller;
        string title;
        string type;
        string propose;
        string origin;
        string destination;
        UInt32 idEmployee;
        UInt32 idTripType;
        UInt32 idTripPropose;
        UInt32 idMainTransport;
        DateTime? arrivalDate;
        DateTime? departureDate;
        string status;
        UInt32 idOriginPlant;
        UInt32 idStatus;
        UInt32 idCurrency;
        UInt32 idAcommodationtype;
        string accomodationDetails;
        string remarks;
        UInt32 isEnabled;
        DateTime? creationDate;
        UInt32 idCreator;
        DateTime? modificationDate;
        UInt32 idModifier;
        string name;
        string linkedTripTitle; //[pramod.misal][GEOS2-4848][23-11-2023]         
        UInt32 idDestination;//[Sudhir.Jangra][GEOS2-4816]
        List<LogEntriesByEmployeeTrip> logEntriesList;//[Sudhir.Jangra][GEOS2-4816]
        int idEmployeeStatus;
        private string firstName;
        private string lastName;
        string mainTransport;
        LookupValue values;
        private string originalPlant;
        private UInt32 departuretransport;//[rushikesh.gaikwad][GEOS2-5927][29.08.2024]
        private UInt32 arrivaltransporttype;
        private string requestor;
        private UInt32 idLookupValue;
        private string _value;
        private UInt32 idLookupValue1;
        private string value1;
        UInt32 idResponsible;
        string employeeDepartments;
        string weekend;
        string customTraveler;
        string desciption;
        DateTime fromDate;
        DateTime toDate;
        int idWorkShift;
        uint idDepartment;
        string travelerEmail;
        bool visaRequired;
        string arrivalTransportationNumber;
        TimeSpan arrivalDateHours;
        string departureTransportationNumber;
        TimeSpan departureDateHours;
        int idArrivalTransport;
        string arrivalTransporterName;
        string arrivalTransferRemark;
        string arrivalProvider;
        string arrivalTransporterContact;
        int idDepartureTransport;
        string departureTransporterName;
        string departureTransferRemark;
        string departureProvider;
        string departureTransporterContact;
        string accomodationAddress;
        string accommodationRemarks;
        int idAccommodation;
        string partnerProvidedRoom;
        string accommodationOtherRoom;
        string emdepRoom;
        string accommodationCoordinates;
        bool plantCarProvided;
        bool mobilePhoneProvided;
        bool simCardProvided;
        bool moneyDeliveredAtOrigin;
        private TripAssets selectedCarAsset;
        private TripAssets selectedSimCardAsset;
        private TripAssets selectedMobileAsset;
        bool moneyDeliveredAtDestination;
        int idVehicle;
        int idPhone;
        int idSimCard;
        List<TripAttachment> listAttachment;
        List<TripAttachment> attachmentFilesToDelete;
        private WorkflowStatus workflowStatus;
        private byte idWorkflowStatus;
        #endregion


        #region Properties
        [Key]
        [Column("IdEmployeeTrip")]
        [DataMember]
        public int IdEmployeeTrip
        {
            get { return idEmployeeTrip; }
            set
            {
                idEmployeeTrip = value;
                OnPropertyChanged("IdEmployeeTrip");
            }
        }
        [Column("Value")]
        [DataMember]
        public LookupValue Value
        {
            get { return values; }
            set
            {
                values = value;
                OnPropertyChanged("Value");
            }
        }
        [Column("Code")]
        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }
        [Column("MainTransport")]
        [DataMember]
        public string MainTransport
        {
            get { return mainTransport; }
            set
            {
                mainTransport = value;
                OnPropertyChanged("MainTransport");
            }
        }

        [Column("Traveller")]
        [DataMember]
        public string Traveller
        {
            get { return traveller; }
            set
            {
                traveller = value;
                OnPropertyChanged("Traveller");
            }
        }

        [Column("Title")]
        [DataMember]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        [Column("Type")]
        [DataMember]
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }


        [Column("Type")]
        [DataMember]
        public string Propose
        {
            get { return propose; }
            set
            {
                propose = value;
                OnPropertyChanged("Propose");
            }
        }


        [Column("Origin")]
        [DataMember]
        public string Origin
        {
            get { return origin; }
            set
            {
                origin = value;
                OnPropertyChanged("Origin");
            }
        }
        [Column("Origin")]
        [DataMember]
        public string Destination
        {
            get { return destination; }
            set
            {
                destination = value;
                OnPropertyChanged("Destination");
            }
        }


        [Key]
        [Column("IdEmployee")]
        [DataMember]
        public UInt32 IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }

        [Key]
        [Column("IdTripType")]
        [DataMember]
        public UInt32 IdTripType
        {
            get { return idTripType; }
            set
            {
                idTripType = value;
                OnPropertyChanged("IdTripType");
            }
        }

        [Key]
        [Column("IdTripPropose")]
        [DataMember]
        public UInt32 IdTripPropose
        {
            get { return idTripPropose; }
            set
            {
                idTripPropose = value;
                OnPropertyChanged("IdTripPropose");
            }
        }

        [Key]
        [Column("IdMainTransport")]
        [DataMember]
        public UInt32 IdMainTransport
        {
            get { return idMainTransport; }
            set
            {
                idMainTransport = value;
                OnPropertyChanged("IdMainTransport");
            }
        }

        [Column("ArrivalDate")]
        [DataMember]
        public DateTime? ArrivalDate
        {
            get { return arrivalDate; }
            set
            {
                arrivalDate = value;
                OnPropertyChanged("ArrivalDate");
            }
        }

        [Column("DepartureDate")]
        [DataMember]
        public DateTime? DepartureDate
        {
            get { return departureDate; }
            set
            {
                departureDate = value;
                OnPropertyChanged("DepartureDate");
            }
        }
        [Column("OriginalPlant")]
        [DataMember]
        public string OriginalPlant
        {
            get { return originalPlant; }
            set
            {
                originalPlant = value;
                OnPropertyChanged("OriginalPlant");
            }
        }

        [Key]
        [Column("Status")]
        [DataMember]
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }


        [Key]
        [Column("IdOriginPlant")]
        [DataMember]
        public UInt32 IdOriginPlant
        {
            get { return idOriginPlant; }
            set
            {
                idOriginPlant = value;
                OnPropertyChanged("IdOriginPlant");
            }
        }


        [Key]
        [Column("IdStatus")]
        [DataMember]
        public UInt32 IdStatus
        {
            get { return idStatus; }
            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

        [Key]
        [Column("IdCurrency")]
        [DataMember]
        public UInt32 IdCurrency
        {
            get { return idCurrency; }
            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        [Key]
        [Column("IdAcommodationtype")]
        [DataMember]
        public UInt32 IdAcommodationtype
        {
            get { return idAcommodationtype; }
            set
            {
                idAcommodationtype = value;
                OnPropertyChanged("IdAcommodationtype");
            }
        }

        [Column("AccomodationDetails")]
        [DataMember]
        public string AccomodationDetails
        {
            get { return accomodationDetails; }
            set
            {
                accomodationDetails = value;
                OnPropertyChanged("AccomodationDetails");
            }
        }


        [Column("Remarks")]
        [DataMember]
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        [Key]
        [Column("IsEnabled")]
        [DataMember]
        public UInt32 IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        [Column("CreationDate")]
        [DataMember]
        public DateTime? CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [Key]
        [Column("IdCreator")]
        [DataMember]
        public UInt32 IdCreator
        {
            get { return idCreator; }
            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

        [Column("ModificationDate")]
        [DataMember]
        public DateTime? ModificationDate
        {
            get { return modificationDate; }
            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [Key]
        [Column("IdModifier")]
        [DataMember]
        public UInt32 IdModifier
        {
            get { return idModifier; }
            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        //[pramod.misal][GEOS2-4848][23-11-2023]     
        [Column("LinkedTripTitle")]
        [DataMember]
        public string LinkedTripTitle
        {
            get
            {
                return linkedTripTitle;
            }

            set
            {
                linkedTripTitle = value;
                OnPropertyChanged("LinkedTripTitle");
            }
        }

        //[Sudhir.Jangra][GEOS2-4816]
        [DataMember]
        public UInt32 IdDestination
        {
            get { return idDestination; }
            set
            {
                idDestination = value;
                OnPropertyChanged("IdDestination");
            }
        }


        //[Sudhir.Jangra][GEOS2-4816]
        [DataMember]
        public List<LogEntriesByEmployeeTrip> LogEntriesList
        {
            get { return logEntriesList; }
            set
            {
                logEntriesList = value;
                OnPropertyChanged("LogEntriesList");
            }
        }

        //[Sudhir.jangra][GEOS2-4816]
        [DataMember]
        public int IdEmployeeStatus
        {
            get { return idEmployeeStatus; }
            set
            {
                idEmployeeStatus = value;
                OnPropertyChanged("IdEmployeeStatus");
            }
        }
        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        [DataMember]
        public string FullName
        {
            get { return FirstName + ' ' + LastName; }
            set { }
        }

        //[rushikesh.gaikwad][GEOS2-5927][29.08.2024]

        [DataMember]
        public UInt32 DepartureTransportationType
        {
            get { return departuretransport; }
            set
            {
                departuretransport = value;
                OnPropertyChanged("DepartureTransportationType");
            }
        }

        [DataMember]
        public UInt32 ArrivalTransportationType
        {
            get { return arrivaltransporttype; }
            set
            {
                arrivaltransporttype = value;
                OnPropertyChanged("ArrivalTransportationType");
            }
        }

        [DataMember]
        public string Requestor
        {
            get { return requestor; }
            set
            {
                requestor = value;
                OnPropertyChanged("Requestor");
            }
        }


        [DataMember]
        public UInt32 IdResponsible
        {
            get { return idResponsible; }
            set
            {
                idResponsible = value;
                OnPropertyChanged("IdResponsible");
            }
        }

        //[rdixit][20.09.2024][GEOS2-5930]
        [DataMember]
        public bool MoneyDeliveredAtDestination
        {
            get { return moneyDeliveredAtDestination; }
            set
            {
                moneyDeliveredAtDestination = value;
                OnPropertyChanged("MoneyDeliveredAtDestination");
            }
        }

        [DataMember]
        public TripAssets SelectedCarAsset
        {
            get { return selectedCarAsset; }
            set
            {
                selectedCarAsset = value;
                OnPropertyChanged("SelectedCarAsset");
            }
        }

        [DataMember]
        public TripAssets SelectedSimCardAsset
        {
            get { return selectedSimCardAsset; }
            set
            {
                selectedSimCardAsset = value;
                OnPropertyChanged("SelectedSimCardAsset");
            }
        }

        [DataMember]
        public TripAssets SelectedMobileAsset
        {
            get { return selectedMobileAsset; }
            set
            {
                selectedMobileAsset = value;
                OnPropertyChanged("SelectedMobileAsset");
            }
        }

        [DataMember]
        public bool PlantCarProvided
        {
            get { return plantCarProvided; }
            set
            {
                plantCarProvided = value;
                OnPropertyChanged("PlantCarProvided");
            }
        }

        [DataMember]
        public bool MobilePhoneProvided
        {
            get { return mobilePhoneProvided; }
            set
            {
                mobilePhoneProvided = value;
                OnPropertyChanged("MobilePhoneProvided");
            }
        }

        [DataMember]
        public bool SimCardProvided
        {
            get { return simCardProvided; }
            set
            {
                simCardProvided = value;
                OnPropertyChanged("SimCardProvided");
            }
        }

        [DataMember]
        public bool MoneyDeliveredAtOrigin
        {
            get { return moneyDeliveredAtOrigin; }
            set
            {
                moneyDeliveredAtOrigin = value;
                OnPropertyChanged("MoneyDeliveredAtOrigin");
            }
        }



        [DataMember]
        public string AccommodationCoordinates
        {
            get { return accommodationCoordinates; }
            set
            {
                accommodationCoordinates = value;
                OnPropertyChanged("AccommodationCoordinates");
            }
        }

        [DataMember]
        public string AccommodationOtherRoom
        {
            get { return accommodationOtherRoom; }
            set
            {
                accommodationOtherRoom = value;
                OnPropertyChanged("AccommodationOtherRoom");
            }
        }
        [NotMapped]
        [DataMember]
        public string EmdepRoom
        {
            get { return emdepRoom; }
            set
            {
                emdepRoom = value;
                OnPropertyChanged("EmdepRoom");
            }
        }

        [DataMember]
        public string PartnerProvidedRoom
        {
            get { return partnerProvidedRoom; }
            set
            {
                partnerProvidedRoom = value;
                OnPropertyChanged("PartnerProvidedRoom");
            }
        }

        [DataMember]
        public int IdAccommodation
        {
            get { return idAccommodation; }
            set
            {
                idAccommodation = value;
                OnPropertyChanged("IdAccommodation");
            }
        }

        [DataMember]
        public string AccommodationRemarks
        {
            get { return accommodationRemarks; }
            set
            {
                accommodationRemarks = value;
                OnPropertyChanged("AccommodationRemarks");
            }
        }



        [DataMember]
        public string AccomodationAddress
        {
            get { return accomodationAddress; }
            set
            {
                accomodationAddress = value;
                OnPropertyChanged("AccomodationAddress");
            }
        }
        [DataMember]
        public string DepartureTransporterContact
        {
            get { return departureTransporterContact; }
            set
            {
                departureTransporterContact = value;
                OnPropertyChanged("DepartureTransporterContact");
            }
        }
        [DataMember]
        public string DepartureProvider
        {
            get { return departureProvider; }
            set
            {
                departureProvider = value;
                OnPropertyChanged("DepartureProvider");
            }
        }

        [DataMember]
        public string DepartureTransferRemark
        {
            get { return departureTransferRemark; }
            set
            {
                departureTransferRemark = value;
                OnPropertyChanged("DepartureTransferRemark");
            }
        }

        [DataMember]
        public string DepartureTransporterName
        {
            get { return departureTransporterName; }
            set
            {
                departureTransporterName = value;
                OnPropertyChanged("DepartureTransporterName");
            }
        }

        [DataMember]
        public int IdDepartureTransport
        {
            get { return idDepartureTransport; }
            set
            {
                idDepartureTransport = value;
                OnPropertyChanged("IdDepartureTransport");
            }
        }

        [DataMember]
        public string ArrivalTransporterContact
        {
            get { return arrivalTransporterContact; }
            set
            {
                arrivalTransporterContact = value;
                OnPropertyChanged("ArrivalTransporterContact");
            }
        }
        [DataMember]
        public string ArrivalProvider
        {
            get { return arrivalProvider; }
            set
            {
                arrivalProvider = value;
                OnPropertyChanged("ArrivalProvider");
            }
        }

        [DataMember]
        public string ArrivalTransferRemark
        {
            get { return arrivalTransferRemark; }
            set
            {
                arrivalTransferRemark = value;
                OnPropertyChanged("ArrivalTransferRemark");
            }
        }

        [DataMember]
        public string ArrivalTransporterName
        {
            get { return arrivalTransporterName; }
            set
            {
                arrivalTransporterName = value;
                OnPropertyChanged("ArrivalTransporterName");
            }
        }

        [DataMember]
        public int IdArrivalTransport
        {
            get { return idArrivalTransport; }
            set
            {
                idArrivalTransport = value;
                OnPropertyChanged("IdArrivalTransport");
            }
        }

        [DataMember]
        public string DepartureTransportationNumber
        {
            get { return departureTransportationNumber; }
            set
            {
                departureTransportationNumber = value;
                OnPropertyChanged("DepartureTransportationNumber");
            }
        }

        [DataMember]
        public string ArrivalTransportationNumber
        {
            get { return arrivalTransportationNumber; }
            set
            {
                arrivalTransportationNumber = value;
                OnPropertyChanged("ArrivalTransportationNumber");
            }
        }

        [DataMember]
        public string EmployeeDepartments
        {
            get { return employeeDepartments; }
            set
            {
                employeeDepartments = value;
                OnPropertyChanged("EmployeeDepartments");
            }
        }

        [DataMember]
        public string Weekend
        {
            get { return weekend; }
            set
            {
                weekend = value;
                OnPropertyChanged("Weekend");
            }
        }

        bool isWeekend;
        [DataMember]
        public bool IsWeekend
        {
            get { return isWeekend; }
            set
            {
                isWeekend = value;
                OnPropertyChanged("IsWeekend");
            }
        }

        [DataMember]
        public string CustomTraveler
        {
            get { return customTraveler; }
            set
            {
                customTraveler = value;
                OnPropertyChanged("CustomTraveler");
            }
        }

        [DataMember]
        public string Desciption
        {
            get { return desciption; }
            set
            {
                desciption = value;
                OnPropertyChanged("Desciption");
            }
        }

        [DataMember]
        public DateTime FromDate
        {
            get { return fromDate; }
            set { fromDate = value; OnPropertyChanged("FromDate"); }
        }

        [DataMember]
        public DateTime ToDate
        {
            get { return toDate; }
            set { toDate = value; OnPropertyChanged("ToDate"); }
        }

        [DataMember]
        public int IdWorkShift
        {
            get { return idWorkShift; }
            set { idWorkShift = value; OnPropertyChanged("IdWorkShift"); }
        }

        [DataMember]
        public UInt32 IdDepartment
        {
            get { return idDepartment; }
            set
            {
                idDepartment = value;
                OnPropertyChanged("IdDepartment");
            }
        }
        [DataMember]
        public string TravelerEmail
        {
            get { return travelerEmail; }
            set { travelerEmail = value; OnPropertyChanged("TravelerEmail"); }
        }

        [DataMember]
        public bool VisaRequired
        {
            get { return visaRequired; }
            set { visaRequired = value; OnPropertyChanged("VisaRequired"); }
        }
        [NotMapped]
        [DataMember]
        public int IdVehicle
        {
            get { return idVehicle; }
            set { idVehicle = value; OnPropertyChanged("IdVehicle"); }
        }
        [NotMapped]
        [DataMember]
        public int IdPhone
        {
            get { return idPhone; }
            set { idPhone = value; OnPropertyChanged("IdPhone"); }
        }
        [NotMapped]
        [DataMember]
        public int IdSimCard
        {
            get { return idSimCard; }
            set { idSimCard = value; OnPropertyChanged("IdSimCard"); }
        }

        [DataMember]
        public LookupValue DeptTransportationType
        {
            get { return deptTransportationType; }
            set
            {
                deptTransportationType = value;
                OnPropertyChanged("DeptTransportationType");
            }
        }

        [DataMember]
        public LookupValue ArrTransportationType
        {
            get { return arrTransportationType; }
            set
            {
                arrTransportationType = value;
                OnPropertyChanged("ArrTransportationType");
            }
        }
        [NotMapped]
        [DataMember]
        public List<TripAttachment> ListAttachment
        {
            get { return listAttachment; }

            set
            {
                listAttachment = value;
                OnPropertyChanged("ListAttachment");
            }
        }

        [NotMapped]
        [DataMember]
        public List<TripAttachment> AttachmentFilesToDelete
        {
            get { return attachmentFilesToDelete; }

            set
            {
                attachmentFilesToDelete = value;
                OnPropertyChanged("AttachmentFilesToDelete");
            }
        }

        [NotMapped]
        [DataMember]
        public WorkflowStatus WorkflowStatus
        {
            get
            {
                return workflowStatus;
            }
            set
            {
                workflowStatus = value;
                OnPropertyChanged("WorkflowStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public byte IdWorkflowStatus
        {
            get
            {
                return idWorkflowStatus;
            }
            set
            {
                idWorkflowStatus = value;
                OnPropertyChanged("IdWorkflowStatus");
            }
        }
        #endregion

        #region Constructor
        public EmployeeTrips()
        {
            ListAttachment = new List<TripAttachment>();
        }
        #endregion


        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            EmployeeTrips employeeTrips = (EmployeeTrips)this.MemberwiseClone();
            if (LogEntriesList != null)
            {
                employeeTrips.LogEntriesList = LogEntriesList.Select(x => (LogEntriesByEmployeeTrip)x.Clone()).ToList();
            }
            return employeeTrips;
        }
        #endregion
    }
}
