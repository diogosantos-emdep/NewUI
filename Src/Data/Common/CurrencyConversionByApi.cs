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
    [DataContract]
    public class CurrencyConversionByApi : ModelBase, IDisposable
    {

        #region Fields
        bool success;
        string terms;
        string privacy;
        long timestamp;
        string source;
        Dictionary<string, float> quotes;

        #endregion

        #region Constructor
        public CurrencyConversionByApi()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public bool Success
        {
            get { return success; }
            set
            {
                success = value;
                OnPropertyChanged("Success");
            }
        }

        [DataMember]
        public string Terms
        {
            get { return terms; }
            set
            {
                terms = value;
                OnPropertyChanged("Terms");
            }
        }

        [DataMember]
        public string Privacy
        {
            get { return privacy; }
            set
            {
                privacy = value;
                OnPropertyChanged("Privacy");
            }
        }

        [DataMember]
        public long Timestamp
        {
            get { return timestamp; }
            set
            {
                timestamp = value;
                OnPropertyChanged("Timestamp");
            }
        }

        [DataMember]
        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }

        [DataMember]
        public Dictionary<string, float> Quotes
        {
            get { return quotes; }
            set
            {
                quotes = value;
                OnPropertyChanged("Quotes");
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
