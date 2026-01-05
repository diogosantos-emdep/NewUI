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
    [Table("currency_conversions")]
    [DataContract(IsReference = true)]
    public class DailyCurrencyConversion : ModelBase, IDisposable
    {
        #region Fields
        float lastMonthAVGRate;
        Int64 idCurrencyConversion;
        DateTime currencyConversionDate;
        byte idCurrencyConversionFrom;
        byte idCurrencyConversionTo;
        Single currencyConversationRate;
        Currency currencyTo;
        Currency currencyFrom;
        byte? isCorrect;
        string axisYName;

        #endregion

        #region Constructor
        public DailyCurrencyConversion()
        {

        }

        #endregion

        #region Properties

        [Key]
        [Column("IdCurrencyConversion")]
        [DataMember]
        public Int64 IdCurrencyConversion
        {
            get { return idCurrencyConversion; }
            set
            {
                idCurrencyConversion = value;
                OnPropertyChanged("IdCurrencyConversion");
            }
        }

        [Column("CurrencyConversionDate")]
        [DataMember]
        public DateTime CurrencyConversionDate
        {
            get { return currencyConversionDate; }
            set
            {
                currencyConversionDate = value;
                OnPropertyChanged("CurrencyConversionDate");
            }
        }

        [Column("IdCurrencyConversionFrom")]
        [ForeignKey("CurrencyFrom")]
        [DataMember]
        public byte IdCurrencyConversionFrom
        {
            get { return idCurrencyConversionFrom; }
            set
            {
                idCurrencyConversionFrom = value;
                OnPropertyChanged("IdCurrencyConversionFrom");
            }
        }

        [Column("IdCurrencyConversionTo")]
        [ForeignKey("CurrencyTo")]
        [DataMember]
        public byte IdCurrencyConversionTo
        {
            get { return idCurrencyConversionTo; }
            set
            {
                idCurrencyConversionTo = value;
                OnPropertyChanged("IdCurrencyConversionTo");
            }
        }

        [Column("CurrencyConversationRate")]
        [DataMember]
        public Single CurrencyConversationRate
        {
            get { return currencyConversationRate; }
            set
            {
                currencyConversationRate = value;
                OnPropertyChanged("CurrencyConversationRate");
            }
        }

        [Column("IsCorrect")]
        [DataMember]
        public byte? IsCorrect
        {
            get { return isCorrect; }
            set
            {
                isCorrect = value;
                OnPropertyChanged("IsCorrect");
            }
        }

        [Column("LastMonthAVGRate")]
        [DataMember]
        public float LastMonthAVGRate
        {
            get { return lastMonthAVGRate; }
            set
            {
                lastMonthAVGRate = value;
                OnPropertyChanged("LastMonthAVGRate");
            }
        }

        [DataMember]
        public virtual Currency CurrencyTo
        {
            get { return currencyTo; }
            set
            {
                currencyTo = value;
                OnPropertyChanged("CurrencyTo");
            }
        }

        [DataMember]
        public virtual Currency CurrencyFrom
        {
            get { return currencyFrom; }
            set
            {
                currencyFrom = value;
                OnPropertyChanged("CurrencyFrom");
            }
        }

        [DataMember]
        [NotMapped]
        public string AxisYName
        {
            get { return axisYName; }
            set
            {
                axisYName = value;
                OnPropertyChanged("AxisYName");
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
