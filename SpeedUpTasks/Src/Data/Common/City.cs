using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common
{
    [Table("cities")]
    [DataContract]
    public class City : ModelBase, IDisposable
    {
        #region Fields

        Int64 idCity;
        string name;
        byte idCountry;
       
        //Country country;
        #endregion

        #region Constructor
        public City()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdCity")]
        [DataMember]
        public Int64 IdCity
        {
            get { return idCity; }
            set
            {
                idCity = value;
                OnPropertyChanged("IdCity");
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
        public byte IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

        //[NotMapped]
        //[DataMember]
        //public Country Country
        //{
        //    get { return country; }
        //    set
        //    {
        //        country = value;
        //        OnPropertyChanged("Country");
        //    }
        //}

      

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
