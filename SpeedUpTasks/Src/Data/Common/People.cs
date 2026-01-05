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

namespace Emdep.Geos.Data.Common
{
    [Table("people")]
    [DataContract]
    public class People : ModelBase, IDisposable
    {
        #region Fields

        Int32 idPerson;
        string name;
        string surname;
        byte idPersonType;
        string email;
        Int32 idSite;
        byte isStillActive;
        string phone;
        string observations;
        PeopleType peopleType;
        string annualSalesTargetAmount;
        string annualSalesTargetCurrency;
        string customerBusiness;
        Int64 customerSizeInSquareMeters;
        Int64 customerNumberOfEmployees;
        Company company;
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

        bool isSiteResponsibleExist = true;
        Int32 idCreator;
        People creator;
        #endregion

        #region Constructor

        public People()
        {
        }

        #endregion

        #region Properties

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
