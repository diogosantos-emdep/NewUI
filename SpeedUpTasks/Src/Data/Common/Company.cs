using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;

namespace Emdep.Geos.Data.Common
{
    [Table("companies")]
    [DataContract]
    public class Company : ModelBase, IDisposable
    {
        #region Fields

        Int32 idCompany;
        string name;
        Byte? isStillActive;
        Int32? createdBy;
        DateTime? createdIn;
        byte? idCountry;
        string fullName;
        string city;
        string _CIF;
        string address;
        string telephone;
        string fax;
        string email;
        string registeredName;
        string zipCode;
        string region;
        Int32? modifiedBy;
        DateTime? modifiedIn;
        string website;
        Double? latitude;
        Double? longitude;
        string alias;
        ImageSource siteImage;
        bool isPermission;
        Int32? idEnterpriseGroup;
        Int32? idSalesResponsible;
        People people;
        SalesTargetBySite salesTargetBySite;
        string postCode;
        string locationLatitude;
        List<SalesStatusType> salesStatusTypes;
        string locationLongitude;
        double? amount;
        string statusName;
        Int32? idSalesResponsibleAssemblyBU;
        User salesResponsible;
        User salesResponsibleAssemblyBU;
        People peopleSalesResponsibleAssemblyBU;
        bool isSiteImageAvailable;
        Int32? line;
        Int32? cuttingMachines;
        double? size;
        Int32? numberOfEmployees;
        byte? idBusinessField;
        byte? idBusinessCenter;
        LookupValue businessField;
        LookupValue businessCenter;
        Customer customer;
        bool isExist;
        bool isUpdate;
        Int32 idCustomer;
        string siteNameWithoutCountry;
        List<SalesTargetBySite> salesTargetBySiteLst;
        string connectPlantId;
        string connectPlantConstr;
        SalesStatusType salesStatusTypeWon;
        string languageForDocumentation;
        List<LogEntryBySite> logEntryBySites;
        string idBusinessProduct;
        byte? idBusinessType;
        string shortName;
        List<SitesByBusinessProduct> businessProductList;
        LookupValue businessType;
        string bothLongitudeLatitude;
        string businessProductString;
        double age;
        double decimalAge;
        string groupPlantName;
        CountryGroup countryGroup;
        UInt32 employeesCount;

        string timeZoneIdentifier;
        People peopleCreatedBy;
        List<CompanyChangelog> companyChangelogs;
        List<Employee> employees;
        DateTime establishmentDate;
        byte[] imageInBytes;
        bool isImageDeleted;
        string fileExtension;

        CompanyAnnualSchedule companyAnnualSchedule;
        CompanySetting companySetting;
        List<CompanySchedule> companySchedules;
        byte isCompany;
        byte isOrganization;
        byte isLocation;
        string serviceProviderUrl;
      
        #endregion

        #region Constructor
        public Company()
        {
            this.SiteUserPermissions = new List<SiteUserPermission>();
            this.Users = new List<User>();
            this.GeosProviders = new List<GeosProvider>();
            this.Suppliers = new List<Supplier>();
            this.Customers = new List<Customer>();
            this.Offers = new List<Offer>();
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdCompany")]
        [DataMember]
        public Int32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
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

        [Column("IsStillActive")]
        [DataMember]
        public Byte? IsStillActive
        {
            get { return isStillActive; }
            set
            {
                isStillActive = value;
                OnPropertyChanged("IsStillActive");
            }
        }

        [Column("CreatedBy")]
        [DataMember]
        public Int32? CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }

        }

        [Column("CreatedIn")]
        [DataMember]
        public DateTime? CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [Column("IdCountry")]
        [ForeignKey("Country")]
        [DataMember]
        public byte? IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

        [Column("City")]
        [DataMember]
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        }

        [Column("CIF")]
        [DataMember]
        public string CIF
        {
            get { return _CIF; }
            set
            {
                _CIF = value;
                OnPropertyChanged("CIF");
            }
        }

        [Column("Address")]
        [DataMember]
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }

        [Column("Telephone")]
        [DataMember]
        public string Telephone
        {
            get { return telephone; }
            set
            {
                telephone = value;
                OnPropertyChanged("Telephone");
            }
        }

        [Column("Fax")]
        [DataMember]
        public string Fax
        {
            get { return fax; }
            set
            {
                fax = value;
                OnPropertyChanged("Fax");
            }
        }

        [Column("Email")]
        [DataMember]
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        [Column("RegisteredName")]
        [DataMember]
        public string RegisteredName
        {
            get { return registeredName; }
            set
            {
                registeredName = value;
                OnPropertyChanged("RegisteredName");
            }
        }

        [Column("ZipCode")]
        [DataMember]
        public string ZipCode
        {
            get { return zipCode; }
            set
            {
                zipCode = value;
                OnPropertyChanged("ZipCode");
            }
        }

        [Column("Region")]
        [DataMember]
        public string Region
        {
            get { return region; }
            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public Int32? ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime? ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [Column("Website")]
        [DataMember]
        public string Website
        {
            get { return website; }
            set
            {
                website = value;
                OnPropertyChanged("Website");
            }
        }

        [Column("Latitude")]
        [DataMember]
        public Double? Latitude
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
        public Double? Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                OnPropertyChanged("Longitude");
            }
        }

        [Column("IsCompany")]
        [DataMember]
        public byte IsCompany
        {
            get { return isCompany; }
            set
            {
                isCompany = value;
                OnPropertyChanged("IsCompany");
            }
        }


        [Column("IsOrganization")]
        [DataMember]
        public byte IsOrganization
        {
            get { return isOrganization; }
            set
            {
                isOrganization = value;
                OnPropertyChanged("IsOrganization");
            }
        }

        [Column("IsLocation")]
        [DataMember]
        public byte IsLocation
        {
            get { return isLocation; }
            set
            {
                isLocation = value;
                OnPropertyChanged("IsLocation");
            }
        }

        [Column("Alias")]
        [DataMember]
        public string Alias
        {
            get { return alias; }
            set
            {
                alias = value;
                OnPropertyChanged("Alias");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsSiteImageAvailable
        {
            get { return isSiteImageAvailable; }
            set
            {
                isSiteImageAvailable = value;
                OnPropertyChanged("IsSiteImageAvailable");
            }
        }

        [ForeignKey("EnterpriseGroup")]
        [Column("IdEnterpriseGroup")]
        [DataMember]
        public Int32? IdEnterpriseGroup
        {
            get { return idEnterpriseGroup; }
            set
            {
                idEnterpriseGroup = value;
                OnPropertyChanged("IdEnterpriseGroup");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsExist
        {
            get { return isExist; }
            set
            {
                isExist = value;
                OnPropertyChanged("IsExist");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsUpdate
        {
            get { return isUpdate; }
            set
            {
                isUpdate = value;
                OnPropertyChanged("IsUpdate");
            }
        }

        [NotMapped]
        [DataMember]
        public List<SalesStatusType> SalesStatusTypes
        {
            get { return salesStatusTypes; }
            set
            {
                salesStatusTypes = value;
                OnPropertyChanged("SalesStatusTypes");
            }
        }

        [NotMapped]
        [DataMember]
        public string BusinessProductString
        {
            get { return businessProductString; }
            set
            {
                businessProductString = value;
                OnPropertyChanged("BusinessProductString");
            }
        }


        [NotMapped]
        [DataMember]
        public SalesTargetBySite SalesTargetBySite
        {
            get { return salesTargetBySite; }
            set
            {
                salesTargetBySite = value;
                OnPropertyChanged("SalesTargetBySite");
            }
        }

        [NotMapped]
        [DataMember]
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }

        [NotMapped]
        [DataMember]
        public Customer Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }

        }

        [NotMapped]
        [DataMember]
        public string PostCode
        {
            get { return postCode; }
            set
            {
                postCode = value;
                OnPropertyChanged("PostCode");
            }
        }

        [NotMapped]
        [DataMember]
        public string LocationLatitude
        {
            get { return locationLatitude; }
            set
            {
                locationLatitude = value;
                OnPropertyChanged("LocationLatitude");
            }
        }

        [NotMapped]
        [DataMember]
        public string ShortName
        {
            get { return shortName; }
            set
            {
                shortName = value;
                OnPropertyChanged("ShortName");
            }
        }

        [NotMapped]
        [DataMember]
        public string LocationLongitude
        {
            get { return locationLongitude; }
            set
            {
                locationLongitude = value;
                OnPropertyChanged("LocationLongitude");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32? IdSalesResponsible
        {
            get { return idSalesResponsible; }
            set
            {
                idSalesResponsible = value;
                OnPropertyChanged("IdSalesResponsible");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32? IdSalesResponsibleAssemblyBU
        {
            get { return idSalesResponsibleAssemblyBU; }
            set
            {
                idSalesResponsibleAssemblyBU = value;
                OnPropertyChanged("IdSalesResponsibleAssemblyBU");
            }
        }

        [NotMapped]
        [DataMember]
        public double? Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }

        [NotMapped]
        [DataMember]
        public string StatusName
        {
            get { return statusName; }
            set
            {
                statusName = value;
                OnPropertyChanged("StatusName");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32? Line
        {
            get { return line; }
            set
            {
                line = value;
                OnPropertyChanged("Line");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32? CuttingMachines
        {
            get { return cuttingMachines; }
            set
            {
                cuttingMachines = value;
                OnPropertyChanged("CuttingMachines");
            }
        }

        [NotMapped]
        [DataMember]
        public string ConnectPlantId
        {
            get { return connectPlantId; }
            set
            {
                connectPlantId = value;
                OnPropertyChanged("ConnectPlantId");
            }
        }

        [NotMapped]
        [DataMember]
        public string ConnectPlantConstr
        {
            get { return connectPlantConstr; }
            set
            {
                connectPlantConstr = value;
                OnPropertyChanged("ConnectPlantConstr");
            }
        }

        [NotMapped]
        [DataMember]
        public double? Size
        {
            get { return size; }
            set
            {
                size = value;
                OnPropertyChanged("Size");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32? NumberOfEmployees
        {
            get { return numberOfEmployees; }
            set
            {
                numberOfEmployees = value;
                OnPropertyChanged("NumberOfEmployees");
            }
        }

        [NotMapped]
        [DataMember]
        public byte? IdBusinessField
        {
            get { return idBusinessField; }
            set
            {
                idBusinessField = value;
                OnPropertyChanged("IdBusinessField");
            }
        }

        [NotMapped]
        [DataMember]
        public byte? IdBusinessCenter
        {
            get { return idBusinessCenter; }
            set
            {
                idBusinessCenter = value;
                OnPropertyChanged("IdBusinessCenter");
            }
        }

        [NotMapped]
        [DataMember]
        public string IdBusinessProduct
        {
            get { return idBusinessProduct; }
            set
            {
                idBusinessProduct = value;
                OnPropertyChanged("IdBusinessProduct");
            }
        }

        [NotMapped]
        [DataMember]
        public List<SitesByBusinessProduct> BusinessProductList
        {
            get { return businessProductList; }
            set
            {
                businessProductList = value;
                OnPropertyChanged("BusinessProductList");
            }
        }

        [NotMapped]
        [DataMember]
        public byte? IdBusinessType
        {
            get { return idBusinessType; }
            set
            {
                idBusinessType = value;
                OnPropertyChanged("IdBusinessType");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdCustomer
        {
            get { return idCustomer; }
            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue BusinessField
        {
            get { return businessField; }
            set
            {
                businessField = value;
                OnPropertyChanged("BusinessField");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue BusinessCenter
        {
            get { return businessCenter; }
            set
            {
                businessCenter = value;
                OnPropertyChanged("BusinessCenter");
            }
        }

        //[NotMapped]
        //[DataMember]
        //public LookupValue BusinessProduct
        //{
        //    get { return businessProduct; }
        //    set
        //    {
        //        businessProduct = value;
        //        OnPropertyChanged("BusinessProduct");
        //    }
        //}

        [NotMapped]
        [DataMember]
        public LookupValue BusinessType
        {
            get { return businessType; }
            set
            {
                businessType = value;
                OnPropertyChanged("BusinessType");
            }
        }

        [DataMember]
        [NotMapped]
        public ImageSource SiteImage
        {
            get { return siteImage; }
            set
            {
                siteImage = value;
                OnPropertyChanged("SiteImage");
            }
        }

        [DataMember]
        [NotMapped]
        public virtual User SalesResponsible
        {
            get { return salesResponsible; }
            set
            {
                salesResponsible = value;
                OnPropertyChanged("SalesResponsible");
            }
        }

        [DataMember]
        [NotMapped]
        public virtual User SalesResponsibleAssemblyBU
        {
            get { return salesResponsibleAssemblyBU; }
            set
            {
                salesResponsibleAssemblyBU = value;
                OnPropertyChanged("SalesResponsibleAssemblyBU");
            }
        }

        [DataMember]
        [NotMapped]
        public bool IsPermission
        {
            get { return isPermission; }
            set
            {
                isPermission = value;
                OnPropertyChanged("IsPermission");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual People People
        {
            get { return people; }
            set
            {
                people = value;
                OnPropertyChanged("People");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual People PeopleSalesResponsibleAssemblyBU
        {
            get { return peopleSalesResponsibleAssemblyBU; }
            set
            {
                peopleSalesResponsibleAssemblyBU = value;
                OnPropertyChanged("PeopleSalesResponsibleAssemblyBU");
            }
        }

        [NotMapped]
        [DataMember]
        public string SiteNameWithoutCountry
        {
            get { return siteNameWithoutCountry; }
            set
            {
                siteNameWithoutCountry = value;
                OnPropertyChanged("SiteNameWithoutCountry");
            }
        }

        [NotMapped]
        [DataMember]
        public string BothLongitudeLatitude
        {
            get { return bothLongitudeLatitude; }
            set
            {
                bothLongitudeLatitude = value;
                OnPropertyChanged("BothLongitudeLatitude");
            }
        }

        [NotMapped]
        [DataMember]
        public double Age
        {
            get { return age; }
            set
            {
                age = value;
                OnPropertyChanged("Age");
            }
        }

        [NotMapped]
        [DataMember]
        public double DecimalAge
        {
            get { return decimalAge; }
            set
            {
                decimalAge = value;
                OnPropertyChanged("DecimalAge");
            }
        }

        [NotMapped]
        [DataMember]
        public List<SalesTargetBySite> SalesTargetBySiteLst
        {
            get { return salesTargetBySiteLst; }
            set
            {
                salesTargetBySiteLst = value;
                OnPropertyChanged("SalesTargetBySiteLst");
            }
        }

        [NotMapped]
        [DataMember]
        public SalesStatusType SalesStatusTypeWon
        {
            get { return salesStatusTypeWon; }
            set
            {
                salesStatusTypeWon = value;
                OnPropertyChanged("SalesStatusTypeWon");
            }
        }

        [NotMapped]
        [DataMember]
        public string LanguageForDocumentation
        {
            get { return languageForDocumentation; }
            set
            {
                languageForDocumentation = value;
                OnPropertyChanged("LanguageForDocumentation");
            }
        }

        [NotMapped]
        [DataMember]
        public List<LogEntryBySite> LogEntryBySites
        {
            get { return logEntryBySites; }
            set
            {
                logEntryBySites = value;
                OnPropertyChanged("LogEntryBySites");
            }
        }

        [NotMapped]
        [DataMember]
        public string GroupPlantName
        {
            get { return groupPlantName; }
            set
            {
                groupPlantName = value;
                OnPropertyChanged("GroupPlantName");
            }
        }

        [DataMember]
        public virtual Country Country { get; set; }

        [DataMember]
        public virtual EnterpriseGroup EnterpriseGroup { get; set; }

        [DataMember]
        public virtual List<User> Users { get; set; }

        [DataMember]
        public virtual List<SiteUserPermission> SiteUserPermissions { get; set; }

        [DataMember]
        public virtual List<GeosProvider> GeosProviders { get; set; }

        [DataMember]
        public virtual List<Supplier> Suppliers { get; set; }

        [DataMember]
        public virtual List<Customer> Customers { get; set; }

        [DataMember]
        public virtual List<Offer> Offers { get; set; }

        [NotMapped]
        [DataMember]
        public CountryGroup CountryGroup
        {
            get { return countryGroup; }
            set
            {
                countryGroup = value;
                OnPropertyChanged("CountryGroup");
            }
        }

        [NotMapped]
        [DataMember]
        public string Coordinates
        {
            get
            {
                return (((Latitude != null) ? Latitude.Value.ToString() : "") + ' ' + ((Longitude != null) ? Longitude.Value.ToString() : ""));
            }
            set { }
        }

        [NotMapped]
        [DataMember]
        public uint EmployeesCount
        {
            get { return employeesCount; }
            set
            {
                employeesCount = value;
                OnPropertyChanged("EmployeesCount");
            }
        }

        [Column("TimeZoneIdentifier")]
        [DataMember]
        public string TimeZoneIdentifier
        {
            get { return timeZoneIdentifier; }
            set
            {
                timeZoneIdentifier = value;
                OnPropertyChanged("TimeZoneIdentifier");
            }
        }


        [NotMapped]
        [DataMember]
        public People PeopleCreatedBy
        {
            get { return peopleCreatedBy; }
            set
            {
                peopleCreatedBy = value;
                OnPropertyChanged("PeopleCreatedBy");
            }
        }

        [NotMapped]
        [DataMember]
        public List<CompanyChangelog> CompanyChangelogs
        {
            get { return companyChangelogs; }
            set
            {
                companyChangelogs = value;
                OnPropertyChanged("CompanyChangelogs");
            }
        }


        [NotMapped]
        [DataMember]
        public List<Employee> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChanged("Employees");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime EstablishmentDate
        {
            get { return establishmentDate; }
            set
            {
                establishmentDate = value;
                OnPropertyChanged("EstablishmentDate");
            }
        }

     
        [NotMapped]
        [DataMember]
        public byte[] ImageInBytes
        {
            get { return imageInBytes; }
            set
            {
                imageInBytes = value;
                OnPropertyChanged("ImageInBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsImageDeleted
        {
            get { return isImageDeleted; }
            set
            {
                isImageDeleted = value;
                OnPropertyChanged("IsImageDeleted");
            }
        }

        [NotMapped]
        [DataMember]
        public string FileExtension
        {
            get { return fileExtension; }
            set
            {
                fileExtension = value;
                OnPropertyChanged("FileExtension");
            }
        }

        [NotMapped]
        [DataMember]
        public CompanySetting CompanySetting
        {
            get { return companySetting; }
            set
            {
                companySetting = value;
                OnPropertyChanged("CompanySetting");
            }
        }

        [NotMapped]
        [DataMember]
        public CompanyAnnualSchedule CompanyAnnualSchedule
        {
            get { return companyAnnualSchedule; }
            set
            {
                companyAnnualSchedule = value;
                OnPropertyChanged("CompanyAnnualSchedule");
            }
        }


        [NotMapped]
        [DataMember]
        public List<CompanySchedule> CompanySchedules
        {
            get { return companySchedules; }
            set
            {
                companySchedules = value;
                OnPropertyChanged("CompanySchedules");
            }
        }


        [NotMapped]
        [DataMember]
        public string ServiceProviderUrl
        {
            get { return serviceProviderUrl; }
            set
            {
                serviceProviderUrl = value;
                OnPropertyChanged("ServiceProviderUrl");
            }
        }

        
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }


        #endregion
    }
}
