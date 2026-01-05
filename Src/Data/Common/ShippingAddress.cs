using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [Table("shippingaddresses")]
    [DataContract]
    public class ShippingAddress : ModelBase, IDisposable
    {
        #region  Fields
        Int64 idShippingAddress;
        string name;
        string address;
        string zipCode;
        string city;
        string region;
        string countryName;
        Country country;
        Int64 idCountry;
        string remark;
        string comments;
        Int32 idSite;
        byte isDefault;
        string zipcityregion;

        #endregion

        #region Constructor
        public ShippingAddress()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdShippingAddress")]

        [DataMember]
        public Int64 IdShippingAddress
        {
            get
            {
                return idShippingAddress;
            }

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
            get
            {
                return name;
            }

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
            get
            {
                return countryName;
            }

            set
            {
                countryName = value;
                OnPropertyChanged("CountryName");
            }
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

        [Column("Remark")]
        [DataMember]
        public string Remark
        {
            get
            {
                return remark;
            }

            set
            {
                remark = value;
                OnPropertyChanged("Remark");
            }
        }

        [Column("Address")]
        [DataMember]
        public string Address
        {
            get
            {
                return address;
            }

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
            get
            {
                return zipCode;
            }

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
            get
            {
                return city;
            }

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
            get
            {
                return region;
            }

            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }

        [Column("IdCountry")]
        [DataMember]
        public Int64 IdCountry
        {
            get
            {
                return idCountry;
            }

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

        [Column("IdSite")]
        [DataMember]
        public Int32 IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [Column("IsDefault")]
        [DataMember]
        public byte IsDefault
        {
            get
            {
                return isDefault;
            }

            set
            {
                isDefault = value;
                OnPropertyChanged("IsDefault");
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
