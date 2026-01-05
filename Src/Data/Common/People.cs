using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Windows.Media;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PLM;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Emdep.Geos.Data.Common
{
    [Table("people")]//
    [DataContract]
    public class People : ModelBase, IDisposable
    {
        #region Fields

        Int32 idPerson;
        Int32 idEmployee;
        Int32 idUser;
        string name;
        string surname;
        byte idPersonType;
        string email;
        Int32 idSite;
        byte isStillActive;
        string phone;
        string phone2;//[Sudhir.jangra][GEOS2-4676][29/08/2023]
        string observations;
        PeopleType peopleType;
        string annualSalesTargetAmount;
        string annualSalesTargetCurrency;
        string customerBusiness;
        Int64 customerSizeInSquareMeters;
        Int64 customerNumberOfEmployees;
        Company company;
        Company importContactCompany;
        ImageSource ownerImage;
        byte? idPersonGender;
        DateTime? createdIn;
        DateTime? disabledIn;
        bool isSelected;
        string userGender;
        bool isSalesResponsible;
        string login;
        byte[] imageBytes;
        Int32 idSalesTeam;
        Int32? idCompanyDepartment;
        List<LogEntriesByContact> logEntriesByContact;
        List<LogEntriesByContact> commentsByContact;
        LookupValue companyDepartment;
        string jobTitle;
        string imageText;
        Int32? idContactInfluenceLevel;
        Int32? idContactEmdepAffinity;
        Int32? idContactProductInvolved;
        Int32? idCompetitor;
        Competitor competitor;
        LookupValue influenceLevel;
        LookupValue emdepAffinity;
        LookupValue productInvolved;
        LookupValue gender;

        bool isSiteResponsibleExist = true;
        Int32 idCreator;
        People creator;
        string salesOwner;
        int idSiteSalesOwner;
        bool ischecked;
        System.Collections.ObjectModel.ObservableCollection<SitesWithCustomer> sitesList;
        bool isEnabled = false;
        private Group group;
        private Country country;
        private Customer customer;
        private List<Company> allCompanies;
        private ObservableCollection<Company> filteredCompanies;
        private bool isFirstToContact;
        private string employeeCode;  //[pallavi.kale][GEOS2-9792][09.10.2025]
        private People modifier;  //[pallavi.kale][GEOS2-8955][09.10.2025]
        private Int32 modifiedByPeople;  //[pallavi.kale][GEOS2-8955][09.10.2025]
        private DateTime? modifiedInPeople;  //[pallavi.kale][GEOS2-8955][09.10.2025]
        private string jobDescriptionTitle;  //[pallavi.kale][GEOS2-9792][09.10.2025]
       
        #endregion

        #region Constructor

        public People()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IsFirstToContact")]
        [DataMember]
        public bool IsFirstToContact
        {
            get
            {
                return isFirstToContact;
            }
            set
            {
                isFirstToContact = value;
                OnPropertyChanged("IsFirstToContact");
            }
        }

        [Key]
        [Column("IdPerson")]
        [DataMember]
        public Int32 IdPerson
        {
            get { return idPerson; }
            set
            {
                idPerson = value;
                OnPropertyChanged("IdPerson");
            }
        }

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


        [Key]
        [Column("IdUser")]
        [DataMember]
        public Int32 IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
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

        [Column("Surname")]
        [DataMember]
        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("Surname");
            }
        }

        [Column("IdPersonType")]
        [DataMember]
        public Byte IdPersonType
        {
            get { return idPersonType; }
            set
            {
                idPersonType = value;
                OnPropertyChanged("IdPersonType");
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

        [NotMapped]
        [DataMember]
        public string UserGender
        {
            get { return userGender; }
            set
            {
                userGender = value;
                OnPropertyChanged("UserGender");
            }
        }

        [Column("IdSite")]
        [DataMember]
        public Int32 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [Column("IsStillActive")]
        [DataMember]
        public byte IsStillActive
        {
            get { return isStillActive; }
            set
            {
                isStillActive = value;
                OnPropertyChanged("IsStillActive");
            }
        }

        [Column("Phone")]
        [DataMember]
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged("Phone");
            }
        }

        [Column("Observations")]
        [DataMember]
        public string Observations
        {
            get { return observations; }
            set
            {
                observations = value;
                OnPropertyChanged("Observations");
            }
        }

        [Column("IdPersonGender")]
        [DataMember]
        public Byte? IdPersonGender
        {
            get { return idPersonGender; }
            set
            {
                idPersonGender = value;
                OnPropertyChanged("IdPersonGender");
            }
        }



        [NotMapped]
        [DataMember]
        public string FullName
        {
            get { return Name + ' ' + Surname; }
            set { }
        }

        [NotMapped]
        [DataMember]
        public Int64 CustomerNumberOfEmployees
        {
            get { return customerNumberOfEmployees; }
            set
            {
                customerNumberOfEmployees = value;
                OnPropertyChanged("CustomerNumberOfEmployees");
            }
        }

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public DateTime? DisabledIn
        {
            get { return disabledIn; }
            set
            {
                disabledIn = value;
                OnPropertyChanged("DisabledIn");
            }
        }

        [NotMapped]
        [DataMember]
        public ImageSource OwnerImage
        {
            get { return ownerImage; }
            set
            {
                ownerImage = value;
                OnPropertyChanged("OwnerImage");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 CustomerSizeInSquareMeters
        {
            get { return customerSizeInSquareMeters; }
            set
            {
                customerSizeInSquareMeters = value;
                OnPropertyChanged("CustomerSizeInSquareMeters");
            }
        }

        [NotMapped]
        [DataMember]
        public string CustomerBusiness
        {
            get { return customerBusiness; }
            set
            {
                customerBusiness = value;
                OnPropertyChanged("CustomerBusiness");
            }
        }

        [NotMapped]
        [DataMember]
        public string AnnualSalesTargetCurrency
        {
            get { return annualSalesTargetCurrency; }
            set
            {
                annualSalesTargetCurrency = value;
                OnPropertyChanged("AnnualSalesTargetCurrency");
            }
        }

        [NotMapped]
        [DataMember]
        public string AnnualSalesTargetAmount
        {
            get { return annualSalesTargetAmount; }
            set
            {
                annualSalesTargetAmount = value;
                OnPropertyChanged("AnnualSalesTargetAmount");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual PeopleType PeopleType
        {
            get { return peopleType; }
            set
            {
                peopleType = value;
                OnPropertyChanged("PeopleType");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual Company Company
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
        public Company ImportContactCompany
        {
            get { return importContactCompany; }
            set
            {
                if (value != null)
                {
                    importContactCompany = value;
                    OnPropertyChanged("ImportContactCompany");
                }
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsSalesResponsible
        {
            get { return isSalesResponsible; }
            set
            {
                isSalesResponsible = value;
                OnPropertyChanged("IsSalesResponsible");
            }
        }

        [NotMapped]
        [DataMember]
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        [NotMapped]
        [DataMember]
        public List<LogEntriesByContact> LogEntriesByContact
        {
            get { return logEntriesByContact; }
            set
            {
                logEntriesByContact = value;
                OnPropertyChanged("LogEntriesByContact");
            }
        }

        [NotMapped]
        [DataMember]
        public List<LogEntriesByContact> CommentsByContact
        {
            get { return commentsByContact; }
            set
            {
                commentsByContact = value;
                OnPropertyChanged("CommentsByContact");
            }
        }

        [NotMapped]
        [DataMember]
        public byte[] ImageBytes
        {
            get { return imageBytes; }
            set
            {
                imageBytes = value;
                OnPropertyChanged("ImageBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdSalesTeam
        {
            get { return idSalesTeam; }
            set
            {
                idSalesTeam = value;
                OnPropertyChanged("IdSalesTeam");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32? IdCompanyDepartment
        {
            get { return idCompanyDepartment; }
            set
            {
                idCompanyDepartment = value;
                OnPropertyChanged("IdCompanyDepartment");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue CompanyDepartment
        {
            get { return companyDepartment; }
            set
            {
                companyDepartment = value;
                OnPropertyChanged("CompanyDepartment");
            }
        }

        [NotMapped]
        [DataMember]
        public string JobTitle
        {
            get { return jobTitle; }
            set
            {
                jobTitle = value;
                OnPropertyChanged("JobTitle");
            }
        }

        [NotMapped]
        [DataMember]
        public string ImageText
        {
            get { return imageText; }
            set
            {
                imageText = value;
                OnPropertyChanged("ImageText");
            }
        }

        [Column("IdContactInfluenceLevel")]
        [DataMember]
        public Int32? IdContactInfluenceLevel
        {
            get { return idContactInfluenceLevel; }
            set
            {
                idContactInfluenceLevel = value;
                OnPropertyChanged("IdContactInfluenceLevel");
            }
        }

        [Column("IdContactEmdepAffinity")]
        [DataMember]
        public Int32? IdContactEmdepAffinity
        {
            get { return idContactEmdepAffinity; }
            set
            {
                idContactEmdepAffinity = value;
                OnPropertyChanged("IdContactEmdepAffinity");
            }
        }

        [Column("IdContactProductInvolved")]
        [DataMember]
        public Int32? IdContactProductInvolved
        {
            get { return idContactProductInvolved; }
            set
            {
                idContactProductInvolved = value;
                OnPropertyChanged("IdContactProductInvolved");
            }
        }

        [Column("IdCompetitor")]
        [DataMember]
        public Int32? IdCompetitor
        {
            get { return idCompetitor; }
            set
            {
                idCompetitor = value;
                OnPropertyChanged("IdCompetitor");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue InfluenceLevel
        {
            get { return influenceLevel; }
            set
            {
                influenceLevel = value;
                OnPropertyChanged("InfluenceLevel");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue EmdepAffinity
        {
            get { return emdepAffinity; }
            set
            {
                emdepAffinity = value;
                OnPropertyChanged("EmdepAffinity");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue ProductInvolved
        {
            get { return productInvolved; }
            set
            {
                productInvolved = value;
                OnPropertyChanged("ProductInvolved");
            }
        }

        [NotMapped]
        [DataMember]
        public Competitor Competitor
        {
            get { return competitor; }

            set
            {
                competitor = value;
                OnPropertyChanged("Competitor");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsSiteResponsibleExist
        {
            get { return isSiteResponsibleExist; }
            set
            {
                isSiteResponsibleExist = value;
                OnPropertyChanged("IsSiteResponsibleExist");
            }
        }

        [Column("IdCreator")]
        [DataMember]
        public Int32 IdCreator
        {
            get { return idCreator; }
            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }


        [NotMapped]
        [DataMember]
        public People Creator
        {
            get { return creator; }
            set
            {
                creator = value;
                OnPropertyChanged("Creator");
            }
        }

        //concated string of all sales from Site_Sales_Owner for selected Contact
        [NotMapped]
        [DataMember]
        public string SalesOwner
        {
            get { return salesOwner; }
            set
            {
                salesOwner = value;
                OnPropertyChanged("SalesOwner");
            }
        }
        //Primary key of Site_Sales_Owner table
        [NotMapped]
        [DataMember]
        public int IdSiteSalesOwner
        {
            get { return idSiteSalesOwner; }
            set
            {
                idSiteSalesOwner = value;
                OnPropertyChanged("IdSiteSalesOwner");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsChecked
        {
            get { return ischecked; }
            set
            {
                ischecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        [NotMapped]
        [DataMember]
        public System.Collections.ObjectModel.ObservableCollection<SitesWithCustomer> SitesList
        {
            get { return sitesList; }
            set
            {
                sitesList = value;
                OnPropertyChanged("SitesList");
            }
        }

        //[sudhir.Jangra][GEOS2-4676][29/08/2023]
        [Column("Phone2")]
        [DataMember]
        public string Phone2
        {
            get { return phone2; }
            set
            {
                phone2 = value;
                OnPropertyChanged("Phone2");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        [NotMapped]
        [DataMember]
        public Group Group
        {
            get { return group; }
            set { group = value; OnPropertyChanged("Group"); }
        }

        [NotMapped]
        [DataMember]
        public LookupValue Gender
        {
            get { return gender; }
            set { gender = value; OnPropertyChanged("Gender"); }
        }


        public Country Country
        {
            get { return country; }
            set { country = value; OnPropertyChanged("Country"); }
        }

        public Customer Customer
        {
            get { return customer; }
            set { customer = value; OnPropertyChanged("Customer");
                if (Customer != null && AllCompanies != null)
                {
                    try
                    {
                        FilteredCompanies = new ObservableCollection<Company>(AllCompanies.Where(x => x.IdCustomer == Customer.IdCustomer));
                    }
                    catch { }
                }
            }
        }

        public List<Company> AllCompanies
        {
            get { return allCompanies; }
            set { allCompanies = value; OnPropertyChanged("AllCompanies"); }
        }
        public ObservableCollection<Company> FilteredCompanies
        {
            get { return filteredCompanies; }
            set { filteredCompanies = value; OnPropertyChanged("FilteredCompanies"); }
        }
        //[pallavi.kale][GEOS2-9792][09.10.2025]
        [NotMapped]
        [DataMember]
        public string EmployeeCode
        {
            get
            {
                return employeeCode;
            }
            set
            {
                employeeCode = value;
                OnPropertyChanged("EmployeeCode");
            }
        }
        //[pallavi.kale][GEOS2-9792][09.10.2025]
        public string SalesOwnerNameEmployeeCodesWithInitialLetters
        {
            get
            {
                return $"{EmployeeCode}_{GetInitials(Name)}";
            }
            //set
            //{
            //    toRecipientNameEmployeeCodes = value;
            //    OnPropertyChanged("ToRecipientNameEmployeeCodes");
            //}
        }

        //[pallavi.kale][GEOS2-8955][09.10.2025]
        [NotMapped]
        [DataMember]
        public Int32 ModifiedByPeople
        {
            get { return modifiedByPeople; }
            set
            {
                modifiedByPeople = value;
                OnPropertyChanged("ModifiedByPeople");
            }
        }
        //[pallavi.kale][GEOS2-8955][09.10.2025]
        [NotMapped]
        [DataMember]
        public People Modifier
        {
            get { return modifier; }
            set
            {
                modifier = value;
                OnPropertyChanged("Modifier");
            }
        }
        //[pallavi.kale][GEOS2-8955][09.10.2025]
        [NotMapped]
        [DataMember]
        public DateTime? ModifiedInPeople
        {
            get { return modifiedInPeople; }
            set
            {
                modifiedInPeople = value;
                OnPropertyChanged("ModifiedInPeople");
            }
        }
        //[pallavi.kale][GEOS2-9792][09.10.2025]
        [NotMapped]
        [DataMember]
        public string JobDescriptionTitle
        {
            get
            {
                return jobDescriptionTitle;
            }
            set
            {
                jobDescriptionTitle = value;
                OnPropertyChanged("JobDescriptionTitle");
            }
        }
       
        #endregion

        #region Methods

        public override string ToString()
        {
            return FullName;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }
        //[pallavi.kale][GEOS2-9792][09.10.2025]
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
        #endregion
    }
}
