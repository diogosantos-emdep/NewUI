using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.SRM
{
    [Table("contacts")]
    [DataContract]
    public class Contacts : ModelBase, IDisposable
    {
        #region Fields

        int idcontact;
        string firstname;
        string lastname;
        string jobTitle;
        string email;
        string phone;
        string phone2;  //[pramod.misal][GEOS2-4673][22-08-2023]
        int? iddepartment;
        byte? idGender;
        string imageText;
        ImageSource ownerImage;
        bool isMainContact;
        bool isRemove = false;
        string remarks = string.Empty;


        Int32 idArticleSupplier;//[Sudhir.jangra][GEOS2-4676][08/09/2023]
        string userGender;//[Sudhuir.jangra][GEOS2-4676]
        LookupValue companyDepartment;//[Sudhir.Jangra][GEOS2-4676]
        DateTime createdIn;//[Sudhir.Jangra][GEOS2-4676]
        string supplierName;//[Sudhir.Jangra][GEOS2-4676]
        Country countryList;//[Sudhir.jangra][GEOS2-4676]
        string region;//[Sudhir.Jangra][GEOS2-4676]
        byte isStillActive;//[Sudhir.Jangra][GEOS2-4676]
        string isActive;//[Sudhir.Jangra][GEOS2-4676]
        string address;//[Sudhir.Jangra][GEOS2-4676]
        string postCode;//[Sudhir.Jangra][GEOS2-4676]
        string city;//[Sudhir.Jangra][GEOS2-4676]
        Int32 idArticleSupplierContact;//[Sudhir.Jangra][GEOS2-4676]
        List<LogEntriesByArticleSuppliers> articleSuppliersComments;//[chitra.girigosavi][GEOS2-4692][09.10.2023]
        List<LogEntriesByArticleSuppliers> articleSuppliersLogEntries;//[chitra.girigosavi][GEOS2-4692][09.10.2023]
        string comments;
        UInt32 idArticleSupplierPOReceiver;//[Sudhir.Jangra][GEOS2-5491]
        UInt32 idType;//[Sudhir.Jangra][GEOS2-5491]
        #endregion

        #region Constructor

        public Contacts()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("Idcontact")]
        [DataMember]
        public int IdContact
        {
            get { return idcontact; }
            set
            {
                idcontact = value;
                OnPropertyChanged("Idcontact");
            }
        }

        [Column("Firstname")]
        [DataMember]
        public string Firstname
        {
            get { return firstname; }
            set
            {
                firstname = value;
                OnPropertyChanged("Firstname");
            }
        }

        [Column("Lastname")]
        [DataMember]
        public string Lastname
        {
            get { return lastname; }
            set
            {
                lastname = value;
                OnPropertyChanged("Lastname");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32? IdDepartment
        {
            get { return iddepartment; }
            set
            {
                iddepartment = value;
                OnPropertyChanged("IdDepartment");
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

        //[pramod.misal][GEOS2-4673][22-08-2023]
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
        public string JobTitle
        {
            get { return jobTitle; }
            set
            {
                jobTitle = value;
                OnPropertyChanged("JobTitle");
            }
        }
        [Column("IdGender")]
        [DataMember]
        public Byte? IdGender
        {
            get { return idGender; }
            set
            {
                idGender = value;
                OnPropertyChanged("IdGender");
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
        public string ImageText
        {
            get { return imageText; }
            set
            {
                imageText = value;
                OnPropertyChanged("ImageText");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsMainContact
        {
            get { return isMainContact; }
            set { isMainContact = value; OnPropertyChanged("IsMainContact"); }
        }

        [NotMapped]
        [DataMember]
        public bool IsRemove
        {
            get { return isRemove; }
            set { isRemove = value; OnPropertyChanged("IsRemove"); }
        }
        [DataMember]
        public string FullName
        {
            get { return Firstname + " " + Lastname; }
            set { }
        }


        //[Sudhir.jangra][GEOS2-4676][08/09/2023]
        [DataMember]

        public Int32 IdArticleSupplier
        {
            get { return idArticleSupplier; }
            set
            {
                idArticleSupplier = value;
                OnPropertyChanged("IdArticleSupplier");
            }
        }

        //[Sudhir.Jangra][GEOS2-4676]
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

        //[Sudhir.Jangra][GEOS2-4676]
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

        //[Sudhir.Jangra][GEOs2-4676]
        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        //[Sudhir.Jangra][GEOS2-4676]
        [DataMember]
        public string SupplierName
        {
            get { return supplierName; }
            set
            {
                supplierName = value;
                OnPropertyChanged("SupplierName");
            }
        }

        //[Sudhir.jangra][GEOS2-4676]
        [DataMember]
        public Country CountryList
        {
            get { return countryList; }
            set
            {
                countryList = value;
                OnPropertyChanged("CountryList");
            }
        }

        //[Sudhir.Jangra][GEOS2-4676]
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

        //[Sudhir.jangra][GEOS2-4676]
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

        //[Sudhir.Jangra][GEOS2-4676]
        [DataMember]
        public string IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        //[Sudhir.Jangra][GEOS2-4676]
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

        //[sudhir.Jangra][GEOS2-4676]
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

        //[Sudhir.Jangra][GEOS2-4676]
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
        //[Sudhir.Jangra][GEOS2-4676]
        [DataMember]
        public Int32 IdArticleSupplierContact
        {
            get { return idArticleSupplierContact; }
            set
            {
                idArticleSupplierContact = value;
                OnPropertyChanged("IdArticleSupplierContact");
            }
        }
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        [NotMapped]
        [DataMember]
        public List<LogEntriesByArticleSuppliers> ArticleSuppliersComments
        {
            get
            {
                return articleSuppliersComments;
            }

            set
            {
                articleSuppliersComments = value;
                OnPropertyChanged("ArticleSuppliersComments");
            }
        }
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public List<LogEntriesByArticleSuppliers> ArticleSuppliersLogEntries
        {
            get
            {
                return articleSuppliersLogEntries;
            }

            set
            {
                articleSuppliersLogEntries = value;
                OnPropertyChanged("ArticleSuppliersLogEntries");
            }
        }
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public string Comment
        {
            get
            {
                return comments;
            }

            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        //[Sudhir.Jangra][GEOS2-5491]
        [NotMapped]
        [DataMember]
        public UInt32 IdArticleSupplierPOReceiver
        {
            get { return idArticleSupplierPOReceiver; }
            set
            {
                idArticleSupplierPOReceiver = value;
                OnPropertyChanged("IdArticleSupplierPOReceiver");
            }
        }

        //[Sudhir.Jangra][GEOS2-5491]
        [NotMapped]
        [DataMember]
        public UInt32 IdType
        {
            get { return idType; }
            set
            {
                idType = value;
                OnPropertyChanged("IdType");
            }
        }


        List<Article_Supplier_Contacts> articleSupplierContacts;
        [NotMapped]
        [DataMember]
        public List<Article_Supplier_Contacts> ArticleSupplierContacts
        {
            get { return articleSupplierContacts; }
            set
            {
                articleSupplierContacts = value;
                OnPropertyChanged("ArticleSupplierContacts");
            }
        }


        List<ArticleSupplier> supplierListByIdContact;
        public List<ArticleSupplier> SupplierListByIdContact
        {
            get { return supplierListByIdContact; }
            set
            {
                supplierListByIdContact = value;
                OnPropertyChanged("SupplierListByIdContact");
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
            //return this.MemberwiseClone();
            Contacts contacts = (Contacts)this.MemberwiseClone();
            if (contacts.ArticleSupplierContacts != null)
                contacts.ArticleSupplierContacts = ArticleSupplierContacts.Select(x => (Article_Supplier_Contacts)x.Clone()).ToList();

            if (contacts.SupplierListByIdContact != null)
                contacts.SupplierListByIdContact = SupplierListByIdContact.Select(x => (ArticleSupplier)x.Clone()).ToList();

            return contacts;
        }

        public override string ToString()
        {
            return Firstname + " " + Lastname;
        }


        #endregion
    }
}

