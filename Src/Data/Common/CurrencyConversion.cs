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
    [Table("currencyconversions")]
    [DataContract(IsReference = true)]
   public class CurrencyConversion : ModelBase, IDisposable
    {
        #region Fields
        double convertedAmount;
        Int64 idcurrencyconversion;
        byte idcurrencyfrom;
        byte idcurrencyto;
        DateTime lastUpdate;
        Single exchangeRate;
        Single lastMonthAVGRate;
        IList<SalesTargetBySite> salesTargetBySites;
        Currency currencyTo;
        Currency currencyFrom;

        #endregion

        #region Constructor
        public CurrencyConversion()
        {
          this.SalesTargetBySites = new List<SalesTargetBySite>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("idcurrencyconversion")]
        [DataMember]
        public Int64 Idcurrencyconversion
        {
            get
            {
                return idcurrencyconversion;
            }

            set
            {
                idcurrencyconversion = value;
                OnPropertyChanged("Idcurrencyconversion");
            }
        }

        
        [Column("idcurrencyfrom")]
        [ForeignKey("CurrencyFrom")]
        [DataMember]
        public byte Idcurrencyfrom
        {
            get
            {
                return idcurrencyfrom;
            }

            set
            {
                idcurrencyfrom = value;
                OnPropertyChanged("Idcurrencyfrom");
            }
        }

        [Column("idcurrencyto")]
        [ForeignKey("CurrencyTo")]
        [DataMember]
        public byte Idcurrencyto
        {
            get
            {
                return idcurrencyto;
            }

            set
            {
                idcurrencyto = value;
                OnPropertyChanged("Idcurrencyto");
            }
        }

        [Column("exchangerate")]
        [DataMember]
        public Single ExchangeRate
        {
            get
            {
                return exchangeRate;
            }

            set
            {
                exchangeRate = value;
                OnPropertyChanged("ExchangeRate");
            }
        }

        [Column("LastMonthAVGRate")]
        [DataMember]
        public Single LastMonthAVGRate
        {
            get
            {
                return lastMonthAVGRate;
            }

            set
            {
                lastMonthAVGRate = value;
                OnPropertyChanged("LastMonthAVGRate");
            }
        }

        

        [Column("lastupdate")]
        [DataMember]
        public DateTime LastUpdate
        {
            get
            {
                return lastUpdate;
            }

            set
            {
                lastUpdate = value;
                OnPropertyChanged("LastUpdate");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual IList<SalesTargetBySite> SalesTargetBySites
        {
            get
            {
                return salesTargetBySites;
            }

            set
            {
                salesTargetBySites = value;
                OnPropertyChanged("SalesTargetBySites");
            }
        }

        [DataMember]
        public virtual Currency CurrencyTo
        {
            get
            {
                return currencyTo;
            }

            set
            {
                currencyTo = value;
                OnPropertyChanged("CurrencyTo");
            }
        }

        [DataMember]
        public virtual Currency CurrencyFrom
        {
            get
            {
                return currencyFrom;
            }

            set
            {
                currencyFrom = value;
                OnPropertyChanged("CurrencyFrom");
            }
        }

        [DataMember]
        public double ConvertedAmount
        {
            get
            {
                return convertedAmount;
            }

            set
            {
                convertedAmount = value;
                OnPropertyChanged("ConvertedAmount");
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
