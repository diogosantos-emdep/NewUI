using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{

    public class CountryData : ModelBase, IDisposable
    {

        Int32 idCountry;
        string countryName;

        [DataMember]
        public Int32 IdCountry
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


        #region Constructor
        public CountryData()
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
