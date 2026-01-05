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
    [Table("country_regions")]
    [DataContract]
    public class CountryRegion : ModelBase, IDisposable
    {
        #region Declaration

        UInt64 idCountryRegion;
        string name;
        UInt16 idCountry;
        Country country;

        #endregion

        #region Properties

        [Key]
        [Column("IdCountryRegion")]
        [DataMember]
        public ulong IdCountryRegion
        {
            get { return idCountryRegion; }
            set
            {
                idCountryRegion = value;
                OnPropertyChanged("IdCountryRegion");
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

        [Column("IdCountry")]
        [DataMember]
        public ushort IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
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

        #endregion

        #region Constructor
        public CountryRegion()
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
