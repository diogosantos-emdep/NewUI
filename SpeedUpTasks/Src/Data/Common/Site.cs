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
namespace Emdep.Geos.Data.Common
{
    [Table("sites")]
    [DataContract(IsReference=true)]
    public class Site:ModelBase
    {
        #region Fields
        Int32 idSite;
        Int32 idCustomer;
        string name;
        Byte isStillActive;
        Int32 createdBy;
        DateTime createdIn;
        Byte? idCountry;
        string city;
        string _CIF;
        string address;
        string telephone;
        string fax;
        string email;
        string registeredName;
        string zipCode;
        string region;
        DateTime? modifiedBy;
        DateTime? modifiedIn;
        string website;
        Double latitude;
        Double longitude;
        string alias;
        ImageSource siteImage;
        bool isPermission;

        #endregion

        #region Constructor
        public Site()
        {
            this.SiteUserPermissions = new List<SiteUserPermission>();
            this.Users = new List<User>();
            this.EmdepSites = new List<EmdepSite>();
        }
        #endregion

        #region Properties

        [Key]
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

        [Column("IdCustomer")]
        [ForeignKey("Customer")]
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
        public Byte IsStillActive
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
        public Int32 CreatedBy
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
        public DateTime CreatedIn
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
        public Byte? IdCountry
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
        public DateTime? ModifiedBy
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

        [Column("latitude")]
        [DataMember]
        public Double Latitude
        {
            get { return latitude; }
            set 
            { 
                latitude = value;
                OnPropertyChanged("Latitude");
            }
        }

        [Column("longitude")]
        [DataMember]
        public Double Longitude
        {
            get { return longitude; }
            set 
            { 
                longitude = value;
                OnPropertyChanged("Longitude");
            }
        }

        [Column("alias")]
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
        public bool IsPermission
        {
            get { return isPermission; }
            set 
            { 
                isPermission = value;
                OnPropertyChanged("IsPermission");
            }
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

      
        [DataMember]
        public virtual Country Country { get; set; }

     
        [DataMember]
        public virtual Customer Customer { get; set; }

        [DataMember]
        public virtual List<User> Users { get; set; }

        [DataMember]
        public virtual List<SiteUserPermission> SiteUserPermissions { get; set; }

        [DataMember]
        public virtual List<EmdepSite> EmdepSites { get; set; }

        #endregion
    }
}
