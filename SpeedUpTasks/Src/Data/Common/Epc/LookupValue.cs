using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("lookup_values")]
    [DataContract(IsReference = true)]
    public class LookupValue : ModelBase, IDisposable
    {
        #region  Fields

        Int32 idLookupValue;
        string value;
        string htmlColor;
        LookupKey lookupKey;
        byte idLookupKey;
        Int64? idImage;
        Int32? position;
        double salesQuotaAmount;
        byte idSalesQuotaCurrency;
        double? percentage;
        List<SalesStatusType> salesStatusTypes;
        object tag;
        bool inUse;
        DateTime? exchangeRateDate;
        double salesQuotaAmountWithExchangeRate;
        UInt64 count;
        double? average;
        decimal countValue;
        string abbreviation;
        List<String> lookupValueImages;
        #endregion

        #region Properties

        [Key]
        [Column("IdLookupValue")]
        [DataMember]
        public Int32 IdLookupValue
        {
            get { return idLookupValue; }
            set
            {
                idLookupValue = value;
                OnPropertyChanged("IdLookupValue");
            }
        }

        [Column("Value")]
        [DataMember]
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        [Column("HtmlColor")]
        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }

        [Column("IdLookupKey")]
        [ForeignKey("LookupKey")]
        [DataMember]
        public byte IdLookupKey
        {
            get { return idLookupKey; }
            set
            {
                idLookupKey = value;
                OnPropertyChanged("LookupKey");
            }
        }

        [Column("IdImage")]
        [DataMember]
        public Int64? IdImage
        {
            get { return idImage; }
            set
            {
                idImage = value;
                OnPropertyChanged("IdImage");
            }
        }

        [Column("Position")]
        [DataMember]
        public Int32? Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        [NotMapped]
        [DataMember]
        public Double SalesQuotaAmount
        {
            get { return salesQuotaAmount; }
            set
            {
                salesQuotaAmount = value;
                OnPropertyChanged("SalesQuotaAmount");
            }
        }

        [NotMapped]
        [DataMember]
        public Double SalesQuotaAmountWithExchangeRate
        {
            get { return salesQuotaAmountWithExchangeRate; }
            set
            {
                salesQuotaAmountWithExchangeRate = value;
                OnPropertyChanged("SalesQuotaAmountWithExchangeRate");
            }
        }

        [NotMapped]
        [DataMember]
        public object Tag
        {
            get { return tag; }
            set
            {
                tag = value;
                OnPropertyChanged("Tag");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? ExchangeRateDate
        {
            get { return exchangeRateDate; }
            set
            {
                exchangeRateDate = value;
                OnPropertyChanged("ExchangeRateDate");
            }
        }

        [NotMapped]
        [DataMember]
        public byte IdSalesQuotaCurrency
        {
            get { return idSalesQuotaCurrency; }
            set
            {
                idSalesQuotaCurrency = value;
                OnPropertyChanged("IdSalesQuotaCurrency");
            }
        }

        [NotMapped]
        [DataMember]
        public double? Percentage
        {
            get { return percentage; }
            set
            {
                percentage = value;
                OnPropertyChanged("Percentage");
            }
        }

        [NotMapped]
        [DataMember]
        public List<SalesStatusType> SalesStatusTypes
        {
            get { return salesStatusTypes; }
            set
            {
                salesStatusTypes = value;
                OnPropertyChanged("SalesStatusTypes");
            }
        }

        [DataMember]
        public virtual LookupKey LookupKey
        {
            get { return lookupKey; }
            set
            {
                lookupKey = value;
                OnPropertyChanged("LookupKey");
            }
        }

        [Column("InUse")]
        [DataMember]
        public bool InUse
        {
            get { return inUse; }
            set
            {
                inUse = value;
                OnPropertyChanged("InUse");
            }
        }

        [NotMapped]
        [DataMember]
        public ulong Count
        {
            get { return count; }
            set
            {
                count = value;
                OnPropertyChanged("Count");
            }
        }

        [NotMapped]
        [DataMember]
        public double? Average
        {
            get { return average; }
            set
            {
                average = value;
                OnPropertyChanged("Average");
            }
        }

        [NotMapped]
        [DataMember]
        public decimal CountValue
        {
            get { return countValue; }
            set
            {
                countValue = value;
                OnPropertyChanged("CountValue");
            }
        }

        [Column("Abbreviation")]
        [DataMember]
        public string Abbreviation
        {
            get { return abbreviation; }
            set
            {
                abbreviation = value;
                OnPropertyChanged("Abbreviation");
            }
        }

        [NotMapped]
        [DataMember]
        public List<String> LookupValueImages
        {
            get { return lookupValueImages; }
            set
            {
                lookupValueImages = value;
                OnPropertyChanged("LookupValueImages");
            }
        }
        #endregion

        #region Methods

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
