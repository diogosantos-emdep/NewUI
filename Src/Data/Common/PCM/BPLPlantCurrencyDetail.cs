using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class BPLPlantCurrencyDetail : ModelBase, IDisposable
    {

        #region Fields


        string currencyName;
       string companyName;
      
        #endregion

        #region Constructor
        public BPLPlantCurrencyDetail()
        {

        }
        #endregion

        #region Properties

        [DataMember]
        public string CurrencyName
        {
            get { return currencyName; }
            set
            {
                currencyName = value;
                OnPropertyChanged("CurrencyName");
            }
        }

        [DataMember]
        public string CompanyName
        {
            get { return companyName; }
            set
            {
                companyName = value;
                OnPropertyChanged("CompanyName");
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
