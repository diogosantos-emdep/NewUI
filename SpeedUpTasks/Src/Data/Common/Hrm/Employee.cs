using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("employees")]
    [DataContract]
    public class Employee : ModelBase, IDisposable
    {
        #region Fields

        Int32 idEmployee;
        string employeeCode;
        string firstName;
        string lastName;
        string nativeName;
        DateTime birthDate;
        DateTime? dateOfBirth;
        UInt32 idGender;
        UInt32 idMaritalStatus;
        Int64 idNationality;

        //Address
        string addressStreet;
        string addressCity;
        string addressRegion;
        string addressZipCode;

        string fullAddress;

        Int64 addressIdCountry;

        UInt16 disability;
        byte isEnabled;
        string remarks;

        LookupValue gender;
        LookupValue maritalStatus;
        Country nationality;
        Country addressCountry;

        List<EmployeeContact> employeePersonalContacts;
        List<EmployeeDocument> employeeDocuments;
        List<EmployeeLanguage> employeeLanguages;
        List<EmployeeEducationQualification> employeeEducationQualifications;
        List<EmployeeContractSituation> employeeContractSituations;
        List<EmployeeFamilyMember> employeeFamilyMembers;
        List<EmployeeJobDescription> employeeJobDescriptions;
        List<EmployeeChangelog> employeeChangelogs;
        List<EmployeeContact> employeeProfessionalContacts;
        List<EmployeeProfessionalEducation> employeeProfessionalEducations;

        byte[] profileImageInBytes;
        bool isProfileImageDeleted;

        EmployeeJobDescription employeeJobDescription;
        string employeeContactEmail;
        string employeeContactMobile;
        string employeeContactHome;
        string employeeContactSkype;

        List<string> employeeContactCompanyEmailList;
        string employeeContactCompanyMobiles;
        List<string> employeeContactCompanyMobileList;

        //private contacts.
        List<string> employeeContactPrivateEmailList;
        string employeeContactPrivateMobiles;
        List<string> employeeContactPrivateMobileList;

        string employeeJobTitles;

        ContractSituation contractSituation;
        Int32 lengthOfService;
        Int32 idParent;
        Int32 idCompanyShift;
        CompanyShift companyShift;
        List<EmployeeAnnualLeave> employeeAnnualLeaves;
        string totalWorkedHours;
        TimeSpan? dayWorkedHours;
        DateTime? hireDate;
        Int32? idEmployeeStatus;
        LookupValue employeeStatus;

        //Exit fields
        DateTime? exitDate;
        Int32? exitIdReason;
        LookupValue exitReason;
        string exitRemarks;

        List<string> employeeJobCodes;
        EmployeeDocument employeeDocument;
        
        string languages;

        Double? latitude;
        Double? longitude;

        EmployeeContractSituation employeeContractSituation;

        byte[] employeeExitDocInBytes;
        bool isemployeeExitDocDeleted;
        string exitFileName;
        string empJobCodes;
        string empJobCodeAbbreviations;
        string lengthOfServiceString;
        List<EmployeeShift> employeeShifts;
        string employeeDocNationalId;
        string employeeDocPassport;
        string employeeDocDrLicense;
        string employeeDocPubIns;
        string employeeDocPriIns;
        string employeeDocVisa;
        string employeeDocOther;
        string employeeDocClockId;
        List<EmployeeLeave> employeeLeave;
        string employeeDepartments;
        string employeeCompanyAlias;
        List<EmployeePolyvalence> employeePolyvalences;
        byte isLongTermAbsent;
        Int32? idCompanyLocation;
        Company companyLocation;
        List<Department> lstEmployeeDepartments;
        string organization;
        string company;
        string employeeDepartmentsHtmlColors;
        string employeeCompanyIds;
        EmployeeAnnualLeave employeeAnnualLeave;
        EmployeeExitEvent employeeExitEvent;
        List<EmployeeExitEvent> employeeExitEvents;
        string employeeShiftScheduleNames;
        string employeeShiftNames;
        byte hasWelcomeMessageBeenReceived;
        string displayName;
        string employeeExitDateInCurrentMonth;
       string employeeTransferredCompanyInCurrentMonth;
        bool isEmployeeExitDateInCurrentMonth;
        bool isEmployeeTransferredCompanyInCurrentMonth;
        #endregion


        #region Properties

        [Key]
        [Column("IdEmployee")]
        [DataMember]
        public Int32 IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }

        [Column("EmployeeCode")]
        [DataMember]
        public string EmployeeCode
        {
            get { return employeeCode; }
            set
            {
                employeeCode = value;
                OnPropertyChanged("EmployeeCode");
            }
        }

        [Column("FirstName")]
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

        [Column("LastName")]
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

        [Column("DisplayName")]
        [DataMember]
        public string DisplayName
        {
            get { return displayName; }
            set
            {
                displayName = value;
                OnPropertyChanged("DisplayName");
            }
        }

        [Column("NativeName")]
        [DataMember]
        public string NativeName
        {
            get { return nativeName; }
            set
            {
                nativeName = value;
                OnPropertyChanged("NativeName");
            }
        }

        [Column("BirthDate")]
        [DataMember]
        public DateTime BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }

        [Column("IdGender")]
        [DataMember]
        public UInt32 IdGender
        {
            get { return idGender; }
            set
            {
                idGender = value;
                OnPropertyChanged("IdGender");
            }
        }

        [Column("IdMaritalStatus")]
        [DataMember]
        public UInt32 IdMaritalStatus
        {
            get { return idMaritalStatus; }
            set
            {
                idMaritalStatus = value;
                OnPropertyChanged("IdMaritalStatus");
            }
        }


        [Column("IdNationality")]
        [DataMember]
        public Int64 IdNationality
        {
            get { return idNationality; }
            set
            {
                idNationality = value;
                OnPropertyChanged("IdNationality");
            }
        }

        [Column("AddressStreet")]
        [DataMember]
        public string AddressStreet
        {
            get { return addressStreet; }
            set
            {
                addressStreet = value;
                OnPropertyChanged("AddressStreet");
            }
        }

        [Column("AddressCity")]
        [DataMember]
        public string AddressCity
        {
            get { return addressCity; }
            set
            {
                addressCity = value;
                OnPropertyChanged("AddressCity");
            }
        }

        [Column("AddressRegion")]
        [DataMember]
        public string AddressRegion
        {
            get { return addressRegion; }
            set
            {
                addressRegion = value;
                OnPropertyChanged("AddressRegion");
            }
        }

        [Column("AddressZipCode")]
        [DataMember]
        public string AddressZipCode
        {
            get { return addressZipCode; }
            set
            {
                addressZipCode = value;
                OnPropertyChanged("AddressZipCode");
            }
        }

        [Column("AddressIdCountry")]
        [DataMember]
        public Int64 AddressIdCountry
        {
            get { return addressIdCountry; }
            set
            {
                addressIdCountry = value;
                OnPropertyChanged("AddressIdCountry");
            }
        }

        [Column("IsEnabled")]
        [DataMember]
        public byte IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        [Column("Disability")]
        [DataMember]
        public UInt16 Disability
        {
            get { return disability; }
            set
            {
                disability = value;
                OnPropertyChanged("Disability");
            }
        }

        [Column("IdCompanyShift")]
        [DataMember]
        public Int32 IdCompanyShift
        {
            get { return idCompanyShift; }
            set
            {
                idCompanyShift = value;
                OnPropertyChanged("IdCompanyShift");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged("Gender");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue MaritalStatus
        {
            get { return maritalStatus; }
            set
            {
                maritalStatus = value;
                OnPropertyChanged("MaritalStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public Country Nationality
        {
            get { return nationality; }
            set
            {
                nationality = value;
                OnPropertyChanged("Nationality");
            }
        }

        [NotMapped]
        [DataMember]
        public Country AddressCountry
        {
            get { return addressCountry; }
            set
            {
                addressCountry = value;
                OnPropertyChanged("AddressCountry");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeContact> EmployeePersonalContacts
        {
            get { return employeePersonalContacts; }
            set
            {
                employeePersonalContacts = value;
                OnPropertyChanged("EmployeeContacts");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeDocument> EmployeeDocuments
        {
            get { return employeeDocuments; }
            set
            {
                employeeDocuments = value;
                OnPropertyChanged("EmployeeDocuments");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeLanguage> EmployeeLanguages
        {
            get { return employeeLanguages; }
            set
            {
                employeeLanguages = value;
                OnPropertyChanged("EmployeeLanguages");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeEducationQualification> EmployeeEducationQualifications
        {
            get { return employeeEducationQualifications; }
            set
            {
                employeeEducationQualifications = value;
                OnPropertyChanged("EmployeeEducationQualifications");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeContractSituation> EmployeeContractSituations
        {
            get { return employeeContractSituations; }
            set
            {
                employeeContractSituations = value;
                OnPropertyChanged("EmployeeContractSituations");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeFamilyMember> EmployeeFamilyMembers
        {
            get { return employeeFamilyMembers; }
            set
            {
                employeeFamilyMembers = value;
                OnPropertyChanged("EmployeeFamilyMembers");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeJobDescription> EmployeeJobDescriptions
        {
            get { return employeeJobDescriptions; }
            set
            {
                employeeJobDescriptions = value;
                OnPropertyChanged("EmployeeJobDescriptions");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeChangelog> EmployeeChangelogs
        {
            get { return employeeChangelogs; }
            set
            {
                employeeChangelogs = value;
                OnPropertyChanged("EmployeeChangelogs");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeContact> EmployeeProfessionalContacts
        {
            get { return employeeProfessionalContacts; }
            set
            {
                employeeProfessionalContacts = value;
                OnPropertyChanged("EmployeeProfessionalContacts");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeProfessionalEducation> EmployeeProfessionalEducations
        {
            get { return employeeProfessionalEducations; }
            set
            {
                employeeProfessionalEducations = value;
                OnPropertyChanged("EmployeeProfessionalEducations");
            }
        }

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public byte[] ProfileImageInBytes
        {
            get { return profileImageInBytes; }

            set
            {
                profileImageInBytes = value;
                OnPropertyChanged("ProfileImageInBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsProfileImageDeleted
        {
            get { return isProfileImageDeleted; }
            set
            {
                isProfileImageDeleted = value;
                OnPropertyChanged("IsProfileImageDeleted");
            }
        }

        [NotMapped]
        [DataMember]
        public EmployeeJobDescription EmployeeJobDescription
        {
            get { return employeeJobDescription; }
            set
            {
                employeeJobDescription = value;
                OnPropertyChanged("EmployeeJobDescription");
            }
        }

        /// <summary>
        /// The employee company email ids.
        /// </summary>
        [NotMapped]
        [DataMember]
        public string EmployeeContactEmail
        {
            get { return employeeContactEmail; }
            set
            {
                employeeContactEmail = value;
                OnPropertyChanged("EmployeeContactEmail");
            }
        }

        /// <summary>
        /// The employee company mobile numbers.
        /// </summary>
        [NotMapped]
        [DataMember]
        public string EmployeeContactMobile
        {
            get { return employeeContactMobile; }
            set
            {
                employeeContactMobile = value;
                OnPropertyChanged("EmployeeContactMobile");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmployeeContactSkype
        {
            get { return employeeContactSkype; }
            set
            {
                employeeContactSkype = value;
                OnPropertyChanged("EmployeeContactSkype");
            }
        }


        [NotMapped]
        [DataMember]
        public string EmployeeContactHome
        {
            get { return employeeContactHome; }
            set
            {
                employeeContactHome = value;
                OnPropertyChanged("EmployeeContactHome");
            }
        }

        [NotMapped]
        [DataMember]
        public ContractSituation ContractSituation
        {
            get { return contractSituation; }
            set
            {
                contractSituation = value;
                OnPropertyChanged("ContractSituation");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 LengthOfService
        {
            get { return lengthOfService; }
            set
            {
                lengthOfService = value;
                OnPropertyChanged("LengthOfService");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdParent
        {
            get { return idParent; }
            set
            {
                idParent = value;
                OnPropertyChanged("IdParent");
            }
        }

        [NotMapped]
        [DataMember]
        public string FullAddress
        {
            get { return fullAddress; }
            set
            {
                fullAddress = value;
                OnPropertyChanged("FullAddress");
            }
        }

        [NotMapped]
        [DataMember]
        public CompanyShift CompanyShift
        {
            get { return companyShift; }
            set
            {
                companyShift = value;
                OnPropertyChanged("CompanyShift");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeAnnualLeave> EmployeeAnnualLeaves
        {
            get { return employeeAnnualLeaves; }
            set
            {
                employeeAnnualLeaves = value;
                OnPropertyChanged("EmployeeAnnualLeaves");
            }
        }

        [NotMapped]
        [DataMember]
        public string TotalWorkedHours
        {
            get { return totalWorkedHours; }
            set
            {
                totalWorkedHours = value;
                OnPropertyChanged("TotalWorkedHours");
            }
        }

        [NotMapped]
        [DataMember]
        public TimeSpan? DayWorkedHours
        {
            get { return dayWorkedHours; }
            set
            {
                dayWorkedHours = value;
                OnPropertyChanged("DayWorkedHours");
            }
        }

        [NotMapped]
        [DataMember]
        public string FullName
        {
            get { return FirstName + ' ' + LastName; }
            set { }
        }

        [Column("IdEmployeeStatus")]
        [DataMember]
        public Int32? IdEmployeeStatus
        {
            get { return idEmployeeStatus; }
            set
            {
                idEmployeeStatus = value;
                OnPropertyChanged("IdEmployeeStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue EmployeeStatus
        {
            get { return employeeStatus; }
            set
            {
                employeeStatus = value;
                OnPropertyChanged("EmployeeStatus");
            }
        }

        [Column("ExitDate")]
        [DataMember]
        public DateTime? ExitDate
        {
            get { return exitDate; }
            set
            {
                exitDate = value;
                OnPropertyChanged("ExitDate");
            }
        }

        [Column("ExitIdReason")]
        [DataMember]
        public int? ExitIdReason
        {
            get { return exitIdReason; }
            set
            {
                exitIdReason = value;
                OnPropertyChanged("ExitIdReason");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue ExitReason
        {
            get { return exitReason; }
            set
            {
                exitReason = value;
                OnPropertyChanged("ExitReason");
            }
        }

        [Column("ExitRemarks")]
        [DataMember]
        public string ExitRemarks
        {
            get { return exitRemarks; }
            set
            {
                exitRemarks = value;
                OnPropertyChanged("ExitRemarks");
            }
        }

        /// <summary>
        /// The employee private email ids.
        /// </summary>
        [NotMapped]
        [DataMember]
        public List<string> EmployeeContactPrivateEmailList
        {
            get { return employeeContactPrivateEmailList; }
            set
            {
                employeeContactPrivateEmailList = value;
                OnPropertyChanged("EmployeeContactPrivateEmailList");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmployeeContactPrivateMobiles
        {
            get { return employeeContactPrivateMobiles; }
            set
            {
                employeeContactPrivateMobiles = value;
                OnPropertyChanged("EmployeeContactPrivateMobiles");
            }
        }

        /// <summary>
        /// The employee private mobile numbers.
        /// </summary>
        [NotMapped]
        [DataMember]
        public List<string> EmployeeContactPrivateMobileList
        {
            get { return employeeContactPrivateMobileList; }
            set
            {
                employeeContactPrivateMobileList = value;
                OnPropertyChanged("EmployeeContactPrivateMobileList");
            }
        }

        /// <summary>
        /// The comma seperated employee job titles.
        /// </summary>
        [NotMapped]
        [DataMember]
        public string EmployeeJobTitles
        {
            get { return employeeJobTitles; }
            set
            {
                employeeJobTitles = value;
                OnPropertyChanged("EmployeeJobTitles");
            }
        }

        [NotMapped]
        [DataMember]
        public List<string> EmployeeContactCompanyEmailList
        {
            get { return employeeContactCompanyEmailList; }
            set
            {
                employeeContactCompanyEmailList = value;
                OnPropertyChanged("EmployeeContactCompanyEmailList");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmployeeContactCompanyMobiles
        {
            get { return employeeContactCompanyMobiles; }
            set
            {
                employeeContactCompanyMobiles = value;
                OnPropertyChanged("EmployeeContactCompanyMobiles");
            }
        }

        [NotMapped]
        [DataMember]
        public List<string> EmployeeContactCompanyMobileList
        {
            get { return employeeContactCompanyMobileList; }
            set
            {
                employeeContactCompanyMobileList = value;
                OnPropertyChanged("EmployeeContactCompanyMobileList");
            }
        }


        /// <summary>
        /// The comma seperated employee job titles.
        /// </summary>
        [NotMapped]
        [DataMember]
        public List<string> EmployeeJobCodes
        {
            get { return employeeJobCodes; }
            set
            {
                employeeJobCodes = value;
                OnPropertyChanged("EmployeeJobCodes");
            }
        }

        [NotMapped]
        [DataMember]
        public EmployeeDocument EmployeeDocument
        {
            get { return employeeDocument; }
            set
            {
                employeeDocument = value;
                OnPropertyChanged("EmployeeDocument");
            }
        }

        [NotMapped]
        [DataMember]
        public string Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged("Languages");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                dateOfBirth = value;
                OnPropertyChanged("DateOfBirth");
            }
        }

        [Column("Latitude")]
        [DataMember]
        public double? Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                OnPropertyChanged("Latitude");
            }
        }

        [Column("Longitude")]
        [DataMember]
        public double? Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                OnPropertyChanged("Longitude");
            }
        }

        [NotMapped]
        [DataMember]
        public EmployeeContractSituation EmployeeContractSituation
        {
            get { return employeeContractSituation; }
            set
            {
                employeeContractSituation = value;
                OnPropertyChanged("EmployeeContractSituation");
            }
        }

        [NotMapped]
        [DataMember]
        public string ExitFileName
        {
            get { return exitFileName; }
            set
            {
               exitFileName = value;
                OnPropertyChanged("ExitFileName");
            }
        }

        [NotMapped]
        [DataMember]
        public byte[] EmployeeExitDocInBytes
        {
            get { return employeeExitDocInBytes; }
            set
            {
                employeeExitDocInBytes = value;
                OnPropertyChanged("EmployeeExitDocInBytes");
            }
        }


        [NotMapped]
        [DataMember]
        public bool IsemployeeExitDocDeleted
        {
            get { return isemployeeExitDocDeleted; }
            set
            {
                isemployeeExitDocDeleted = value;
                OnPropertyChanged("IsemployeeExitDocDeleted");
            }
        }


        [NotMapped]
        [DataMember]
        public string EmpJobCodes
        {
            get { return empJobCodes; }
            set
            {
                empJobCodes = value;
                OnPropertyChanged("EmpJobCodes");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmpJobCodeAbbreviations
        {
            get { return empJobCodeAbbreviations; }
            set
            {
                empJobCodeAbbreviations = value;
                OnPropertyChanged("EmpJobCodeAbbreviations");
            }
        }

        [NotMapped]
        [DataMember]
        public string LengthOfServiceString
        {
            get { return lengthOfServiceString; }
            set
            {
                lengthOfServiceString = value;
                OnPropertyChanged("LengthOfServiceString");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeShift> EmployeeShifts
        {
            get { return employeeShifts; }
            set
            {
                employeeShifts = value;
                OnPropertyChanged("EmployeeShifts");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmployeeDocPassport
        {
            get { return employeeDocPassport; }
            set
            {
                employeeDocPassport = value;
                OnPropertyChanged("EmployeeDocPassport");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmployeeDocDrLicense
        {
            get { return employeeDocDrLicense; }
            set
            {
                employeeDocDrLicense = value;
                OnPropertyChanged("EmployeeDocDrLicense");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmployeeDocPubIns
        {
            get { return employeeDocPubIns; }
            set
            {
                employeeDocPubIns = value;
                OnPropertyChanged("EmployeeDocPubIns");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmployeeDocPriIns
        {
            get { return employeeDocPriIns; }
            set
            {
                employeeDocPriIns = value;
                OnPropertyChanged("EmployeeDocPriIns");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmployeeDocVisa
        {
            get { return employeeDocVisa; }
            set
            {
                employeeDocVisa = value;
                OnPropertyChanged("EmployeeDocVisa");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmployeeDocOther
        {
            get { return employeeDocOther; }
            set
            {
                employeeDocOther = value;
                OnPropertyChanged("EmployeeDocOther");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmployeeDocClockId
        {
            get { return employeeDocClockId; }
            set
            {
                employeeDocClockId = value;
                OnPropertyChanged("EmployeeDocClockId");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmployeeDocNationalId
        {
            get { return employeeDocNationalId; }
            set
            {
                employeeDocNationalId = value;
                OnPropertyChanged("EmployeeDocNationalId");
            }
        }


        [NotMapped]
        [DataMember]
        public List<EmployeeLeave> EmployeeLeave
        {
            get { return employeeLeave; }
            set
            {
                employeeLeave = value;
                OnPropertyChanged("EmployeeLeave");
            }
        }

        [NotMapped]
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


        [NotMapped]
        [DataMember]
        public string EmployeeCompanyAlias
        {
            get { return employeeCompanyAlias; }
            set
            {
                employeeCompanyAlias = value;
                OnPropertyChanged("EmployeeCompanyAlias");
            }
        }


        [NotMapped]
        [DataMember]
        public List<EmployeePolyvalence> EmployeePolyvalences
        {
            get { return employeePolyvalences; }
            set
            {
                employeePolyvalences = value;
                OnPropertyChanged("EmployeePolyvalences");
            }
        }


        [NotMapped]
        [DataMember]
        public byte IsLongTermAbsent
        {
            get { return isLongTermAbsent; }
            set
            {
                isLongTermAbsent = value;
                OnPropertyChanged("IsLongTermAbsent");
            }
        }


        [Column("IdCompanyLocation")]
        [DataMember]
        public Int32? IdCompanyLocation
        {
            get { return idCompanyLocation; }
            set
            {
                idCompanyLocation = value;
                OnPropertyChanged("IdCompanyLocation");
            }
        }


        [NotMapped]
        [DataMember]
        public Company CompanyLocation
        {
            get { return companyLocation; }
            set
            {
                companyLocation = value;
                OnPropertyChanged("CompanyLocation");
            }
        }


        [NotMapped]
        [DataMember]
        public List<Department> LstEmployeeDepartments
        {
            get { return lstEmployeeDepartments; }
            set
            {
                lstEmployeeDepartments = value;
                OnPropertyChanged("LstEmployeeDepartments");
            }
        }


        [NotMapped]
        [DataMember]
        public string Organization
        {
            get { return organization; }
            set
            {
                organization = value;
                OnPropertyChanged("Organization");
            }
        }


        [NotMapped]
        [DataMember]
        public string Company
        {
            get { return company; }
            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmployeeDepartmentsHtmlColors
        {
            get { return employeeDepartmentsHtmlColors; }
            set
            {
                employeeDepartmentsHtmlColors = value;
                OnPropertyChanged("EmployeeDepartmentsHtmlColors");
            }
        }


        [NotMapped]
        [DataMember]
        public string EmployeeCompanyIds
        {
            get { return employeeCompanyIds; }
            set
            {
                employeeCompanyIds = value;
                OnPropertyChanged("EmployeeCompanyIds");
            }
        }

        [NotMapped]
        [DataMember]
        public EmployeeAnnualLeave EmployeeAnnualLeave
        {
            get { return employeeAnnualLeave; }
            set
            {
                employeeAnnualLeave = value;
                OnPropertyChanged("EmployeeAnnualLeave");
            }
        }


        [NotMapped]
        [DataMember]
        public EmployeeExitEvent EmployeeExitEvent
        {
            get { return employeeExitEvent; }
            set
            {
                employeeExitEvent = value;
                OnPropertyChanged("EmployeeExitEvent");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeExitEvent> EmployeeExitEvents
        {
            get { return employeeExitEvents; }
            set
            {
                employeeExitEvents = value;
                OnPropertyChanged("EmployeeExitEvents");
            }
        }


        [NotMapped]
        [DataMember]
        public string EmployeeShiftNames
        {
            get { return employeeShiftNames; }
            set
            {
                employeeShiftNames = value;
                OnPropertyChanged("EmployeeShiftNames");
            }
        }


        [NotMapped]
        [DataMember]
        public string EmployeeShiftScheduleNames
        {
            get { return employeeShiftScheduleNames; }
            set
            {
                employeeShiftScheduleNames = value;
                OnPropertyChanged("EmployeeShiftScheduleNames");
            }
        }

        [NotMapped]
        [DataMember]
        public byte HasWelcomeMessageBeenReceived
        {
            get { return hasWelcomeMessageBeenReceived; }
            set
            {
                hasWelcomeMessageBeenReceived = value;
                OnPropertyChanged("HasWelcomeMessageBeenReceived");
            }
        }


        [NotMapped]
        [DataMember]
        public DateTime? HireDate
        {
            get { return hireDate; }
            set
            {
                hireDate = value;
                OnPropertyChanged("HireDate");
            }
        }


        [NotMapped]
        [DataMember]
        public string EmployeeExitDateInCurrentMonth
        {
            get { return employeeExitDateInCurrentMonth; }
            set
            {
                employeeExitDateInCurrentMonth = value;
                OnPropertyChanged("EmployeeExitDateInCurrentMonth");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmployeeTransferredCompanyInCurrentMonth
        {
            get { return employeeTransferredCompanyInCurrentMonth; }
            set
            {
                employeeTransferredCompanyInCurrentMonth = value;
                OnPropertyChanged("EmployeeTransferredCompanyInCurrentMonth");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsEmployeeExitDateInCurrentMonth
        {
            get { return isEmployeeExitDateInCurrentMonth; }
            set
            {
                isEmployeeExitDateInCurrentMonth = value;
                OnPropertyChanged("IsEmployeeExitDateInCurrentMonth");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsEmployeeTransferredCompanyInCurrentMonth
        {
            get { return isEmployeeTransferredCompanyInCurrentMonth; }
            set
            {
                isEmployeeTransferredCompanyInCurrentMonth = value;
                OnPropertyChanged("IsEmployeeTransferredCompanyInCurrentMonth");
            }
        }
        #endregion

        #region Constructor

        public Employee()
        {
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            if (employeeJobDescription != null && employeeJobDescription.JobDescription != null
                && !string.IsNullOrEmpty(employeeJobDescription.JobDescription.JobDescriptionCode))
            {
                return string.Format("{0} - {1} {2}", employeeJobDescription.JobDescription.JobDescriptionCode, FirstName, LastName);
            }
            else
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            Employee employee = (Employee)this.MemberwiseClone();

            if (employee.Gender != null)
                employee.Gender = (LookupValue)this.Gender.Clone();

            if (employee.MaritalStatus != null)
                employee.MaritalStatus = (LookupValue)this.MaritalStatus.Clone();

            if (employee.Nationality != null)
                employee.Nationality = (Country)this.Nationality.Clone();

            if (employee.AddressCountry != null)
                employee.AddressCountry = (Country)this.AddressCountry.Clone();

            if (employee.EmployeePersonalContacts != null)
                employee.EmployeePersonalContacts = EmployeePersonalContacts.Select(x => (EmployeeContact)x.Clone()).ToList();

            if (employee.EmployeeDocuments != null)
                employee.EmployeeDocuments = EmployeeDocuments.Select(x => (EmployeeDocument)x.Clone()).ToList();

            if (employee.EmployeeLanguages != null)
                employee.EmployeeLanguages = EmployeeLanguages.Select(x => (EmployeeLanguage)x.Clone()).ToList();

            if (employee.EmployeeEducationQualifications != null)
                employee.EmployeeEducationQualifications = EmployeeEducationQualifications.Select(x => (EmployeeEducationQualification)x.Clone()).ToList();

            if (employee.EmployeeContractSituations != null)
                employee.EmployeeContractSituations = EmployeeContractSituations.Select(x => (EmployeeContractSituation)x.Clone()).ToList();

            if (employee.EmployeeFamilyMembers != null)
                employee.EmployeeFamilyMembers = EmployeeFamilyMembers.Select(x => (EmployeeFamilyMember)x.Clone()).ToList();

            if (employee.EmployeeJobDescriptions != null)
                employee.EmployeeJobDescriptions = EmployeeJobDescriptions.Select(x => (EmployeeJobDescription)x.Clone()).ToList();

            if (employee.EmployeeProfessionalContacts != null)
                employee.EmployeeProfessionalContacts = EmployeeProfessionalContacts.Select(x => (EmployeeContact)x.Clone()).ToList();

            if (employee.EmployeeProfessionalEducations != null)
                employee.EmployeeProfessionalEducations = EmployeeProfessionalEducations.Select(x => (EmployeeProfessionalEducation)x.Clone()).ToList();

            if (employee.EmployeeJobDescription != null)
                employee.EmployeeJobDescription = (EmployeeJobDescription)this.EmployeeJobDescription.Clone();

            if (employee.ContractSituation != null)
                employee.ContractSituation = (ContractSituation)this.ContractSituation.Clone();

            if (employee.EmployeeAnnualLeaves != null)
                employee.EmployeeAnnualLeaves = EmployeeAnnualLeaves.Select(x => (EmployeeAnnualLeave)x.Clone()).ToList();

            if (employee.EmployeePolyvalences != null)
                employee.EmployeePolyvalences = EmployeePolyvalences.Select(x => (EmployeePolyvalence)x.Clone()).ToList();

            if (employee.CompanyLocation != null)
                employee.CompanyLocation = (Company)this.CompanyLocation.Clone();

            if (employee.EmployeeShifts != null)
                employee.EmployeeShifts = EmployeeShifts.Select(x => (EmployeeShift)x.Clone()).ToList();

            if (employee.EmployeeAnnualLeave != null)
                employee.EmployeeAnnualLeave = (EmployeeAnnualLeave)this.EmployeeAnnualLeave.Clone();

            return employee;
        }

        #endregion

    }
}
