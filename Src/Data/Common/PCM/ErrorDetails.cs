using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class ErrorDetails : ModelBase, IDisposable
    {

        #region Fields

      
        string error;
        string companyName;
        string articleReference;
        string currencyName;
        #endregion

        #region Constructor
        public ErrorDetails()
        {

        }
        #endregion

        #region Properties

        [DataMember]
        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                OnPropertyChanged("Error");
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

        [DataMember]
        public string ArticleReference
        {
            get { return articleReference; }
            set
            {
                articleReference = value;
                OnPropertyChanged("ArticleReference");
            }
        }

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
