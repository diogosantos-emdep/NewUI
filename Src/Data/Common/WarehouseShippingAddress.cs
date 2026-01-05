using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Emdep.Geos.Data.Common
{
    [Table("warehouseShippingAddress")]
    [DataContract]
    public class WarehouseShippingAddress : ModelBase, IDisposable
    {

        #region Declaration

        Int64 idShippingAddress; 
        string name;
        string countryname;
        string address;
        string zipCode;
        string city;
        string region;
        Int64 idCountry;
        Int64 idWarehousePurchaseOrder;
        string comments;
        Int64 idSite;
        Int64 isDefault;
        Int64 isDisabled;
        bool isMainContact;//[pramod.misal][GEOS2-4451][01-07-2023]
        Country country;
        string zipcityregion;
        #endregion


        #region Properties

        [Key]
        [Column("IdShippingAddress")]
        [DataMember]
        public Int64 IdShippingAddress
        {
            get { return idShippingAddress; }
            set
            {
                idShippingAddress = value;
                OnPropertyChanged("IdShippingAddress");
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


        [Column("CountryName")]
        [DataMember]
        public string CountryName
        {
            get { return countryname; }
            set
            {
                countryname = value;
                OnPropertyChanged("CountryName");
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

        [Key]
        [Column("IdCountry")]
        [DataMember]
        public Int64 IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }


        [Column("Comments")]
        [DataMember]
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }


        [Column("IdWarehousePurchaseOrder")]
        [DataMember]
        public Int64 IdWarehousePurchaseOrder
        {
            get { return idWarehousePurchaseOrder; }
            set
            {
                idWarehousePurchaseOrder = value;
                OnPropertyChanged("IdWarehousePurchaseOrder");
            }
        }

        [Key]
        [Column("IdSite")]
        [DataMember]
        public Int64 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }


        [Key]
        [Column("IsDefault")]
        [DataMember]
        public Int64 IsDefault
        {
            get { return isDefault; }
            set
            {
                isDefault = value;
                OnPropertyChanged("IsDefault");
            }
        }

        [Key]
        [Column("IsDisabled")]
        [DataMember]
        public Int64 IsDisabled
        {
            get { return isDisabled; }
            set
            {
                isDisabled = value;
                OnPropertyChanged("IsDisabled");
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
        public Country Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        [DataMember]
        public string Zipcityregion
        {
            get { return zipcityregion; }
            set
            {
                zipcityregion = value;                
                OnPropertyChanged("Zipcityregion");
            }
        }
        #endregion


        #region Constructor

        public WarehouseShippingAddress()
        {

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
