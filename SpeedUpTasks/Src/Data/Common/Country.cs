using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common
{
    [Table("countries")]
    [DataContract]
    public class Country : ModelBase, IDisposable
    {
        #region Fields

        byte idCountry;
        string name;
        Int32 idContinent;
        SByte eu_member;
        SByte euroZone;
        string iso;
        string printable_name;
        string iso3;
        Int16 numcode;
        string mainLanguage;
        string observations;
        Single standardVAT;
        SByte strictCustoms;
        SByte mercosur_member;
        SByte idCurrency;
        Int32 idZone;
        Zone zone;

        Int64 idCountryGroup;
        CountryGroup countryGroup;
        byte[] countryIconbytes ;
        ImageSource countryIconImage;
        #endregion

        #region Constructor
        public Country()
        {
            this.Sites = new List<Company>();
        }

        #endregion


        #region Properties

        [Key]
        [Column("IdCountry")]
        [DataMember]
        public byte IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
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

        [Column("IdContinent")]
        [DataMember]
        public Int32 IdContinent
        {
            get { return idContinent; }
            set
            {
                idContinent = value;
                OnPropertyChanged("IdContinent");
            }
        }

        [DataMember]
        public virtual List<Company> Sites { get; set; }

        [NotMapped]
        [DataMember]
        public sbyte Eu_member
        {
            get { return eu_member; }
            set
            {
                eu_member = value;
                OnPropertyChanged("Eu_member");
            }
        }

        [NotMapped]
        [DataMember]
        public sbyte EuroZone
        {
            get { return euroZone; }
            set
            {
                euroZone = value;
                OnPropertyChanged("EuroZone");
            }
        }

        [NotMapped]
        [DataMember]
        public string Iso
        {
            get { return iso; }
            set
            {
                iso = value;
                OnPropertyChanged("Iso");
            }
        }

        [NotMapped]
        [DataMember]
        public string Printable_name
        {
            get { return printable_name; }
            set
            {
                printable_name = value;
                OnPropertyChanged("Printable_name");
            }
        }

        [NotMapped]
        [DataMember]
        public string Iso3
        {
            get { return iso3; }
            set
            {
                iso3 = value;
                OnPropertyChanged("Iso3");
            }
        }

        [NotMapped]
        [DataMember]
        public short Numcode
        {
            get { return numcode; }
            set
            {
                numcode = value;
                OnPropertyChanged("Numcode");
            }
        }

        [NotMapped]
        [DataMember]
        public string MainLanguage
        {
            get { return mainLanguage; }
            set
            {
                mainLanguage = value;
                OnPropertyChanged("MainLanguage");
            }
        }

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public float StandardVAT
        {
            get { return standardVAT; }
            set
            {
                standardVAT = value;
                OnPropertyChanged("StandardVAT");
            }
        }

        [NotMapped]
        [DataMember]
        public sbyte StrictCustoms
        {
            get { return strictCustoms; }
            set
            {
                strictCustoms = value;
                OnPropertyChanged("StrictCustoms");
            }
        }

        [NotMapped]
        [DataMember]
        public sbyte Mercosur_member
        {
            get { return mercosur_member; }
            set
            {
                mercosur_member = value;
                OnPropertyChanged("Mercosur_member");
            }
        }

        [NotMapped]
        [DataMember]
        public sbyte IdCurrency
        {
            get { return idCurrency; }
            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        [NotMapped]
        [DataMember]
        public int IdZone
        {
            get { return idZone; }
            set
            {
                idZone = value;
                OnPropertyChanged("IdZone");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual Zone Zone
        {
            get { return zone; }
            set
            {
                zone = value;
                OnPropertyChanged("Zone");
            }
        }

        [NotMapped]
        [DataMember]
        public long IdCountryGroup
        {
            get { return idCountryGroup; }
            set
            {
                idCountryGroup = value;
                OnPropertyChanged("IdCountryGroup");
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
        public byte[] CountryIconBytes
        {
            get { return countryIconbytes; }
            set
            {
                countryIconbytes = value;
                OnPropertyChanged("CountryIconBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public ImageSource CountryIconImage
        {
            get { return countryIconImage; }
            set
            {
                countryIconImage = value;
                OnPropertyChanged("CountryIconImage");
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
            Country country = (Country)this.MemberwiseClone();
            return country;
        }

        #endregion
    }
}
