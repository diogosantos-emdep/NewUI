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
    public class ManufacturersByArticle : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idManufacturerByArticle;
        Int32 idArticle;
        Int64 idManufacturer;
        Int64 idCountry;
        byte isActive;

        Manufacturer manufacturer;
        Country country;
        bool isNew;
        string manufacturerWithCountry;

        CountryGroup countryGroup;
        bool isSameCountryGroup;

        #endregion

        #region Properties

        [Key]
        [Column("IdManufacturerByArticle")]
        [DataMember]
        public long IdManufacturerByArticle
        {
            get { return idManufacturerByArticle; }
            set
            {
                idManufacturerByArticle = value;
                OnPropertyChanged("IdManufacturerByArticle");
            }
        }

        [Column("IdArticle")]
        [DataMember]
        public int IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        [Column("IdManufacturer")]
        [DataMember]
        public long IdManufacturer
        {
            get { return idManufacturer; }
            set
            {
                idManufacturer = value;
                OnPropertyChanged("IdManufacturer");
            }
        }

        [Column("IdCountry")]
        [DataMember]
        public long IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

        [Column("IsActive")]
        [DataMember]
        public byte IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        [NotMapped]
        [DataMember]
        public Manufacturer Manufacturer
        {
            get { return manufacturer; }
            set
            {
                manufacturer = value;
                OnPropertyChanged("IsActive");
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

        [NotMapped]
        [DataMember]
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged("IsNew");
            }
        }

        [NotMapped]
        [DataMember]
        public string ManufacturerWithCountry
        {
            get { return manufacturerWithCountry; }
            set
            {
                manufacturerWithCountry = value;
                OnPropertyChanged("ManufacturerWithCountry");
            }
        }

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
        public bool IsSameCountryGroup
        {
            get { return isSameCountryGroup; }
            set
            {
                isSameCountryGroup = value;
                OnPropertyChanged("CountryGroup");
            }
        }

        #endregion

        #region Constructor

        public ManufacturersByArticle()
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
