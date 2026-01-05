using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common
{
    [Table("currency")]
    [DataContract]
    public class Currency : ModelBase, IDisposable
    {
        #region Fields
        byte idCurrency;
        string name;
        string symbol;
        string description;
        Int64 codeN;
        IList<Offer> offers;
        IList<CurrencyConversion> currencyConversions;

        UInt64 idArticle;
        float currencyConversionRate;
        float sellPrice;
        UInt64 idBasePriceList;
        float? sellPriceValue;

        UInt64 idCustomerPriceList;
        float? currencyConversionRate_New;

        UInt32 idDetection;
        ImageSource currencyIconImage;
        byte[] currencyIconbytes;
        string iSOCode;//[Sudhir.Jangra][GEOS2-4816]
        #endregion

        #region Constructor
        public Currency()
        {
            this.Offers = new List<Offer>();
            this.CurrencyConversions = new List<CurrencyConversion>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdCurrency")]
        [DataMember]
        public byte IdCurrency
        {
            get
            {
                return idCurrency;
            }

            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }


        [Column("Name")]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("Symbol")]
        [DataMember]
        public string Symbol
        {
            get
            {
                return symbol;
            }

            set
            {
                symbol = value;
                OnPropertyChanged("Symbol");
            }
        }


        [Column("Description")]
        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [Column("CodeN")]
        [DataMember]
        public Int64 CodeN
        {
            get
            {
                return codeN;
            }

            set
            {
                codeN = value;
                OnPropertyChanged("CodeN");
            }
        }

        [DataMember]
        public virtual IList<Offer> Offers
        {
            get
            {
                return offers;
            }

            set
            {
                offers = value;
                OnPropertyChanged("Offers");
            }
        }

        [DataMember]
        public virtual IList<CurrencyConversion> CurrencyConversions
        {
            get
            {
                return currencyConversions;
            }

            set
            {
                currencyConversions = value;
                OnPropertyChanged("CurrencyConversions");
            }
        }
        [NotMapped]
        [DataMember]
        public ulong IdArticle
        {
            get
            {
                return idArticle;
            }

            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        [NotMapped]
        [DataMember]
        public float CurrencyConversionRate
        {
            get
            {
                return currencyConversionRate;
            }

            set
            {
                currencyConversionRate = value;
                OnPropertyChanged("CurrencyConversionRate");
            }
        }

        [NotMapped]
        [DataMember]
        public float SellPrice
        {
            get
            {
                return sellPrice;
            }

            set
            {
                sellPrice = value;
                OnPropertyChanged("SellPrice");
            }
        }

        [NotMapped]
        [DataMember]
        public ulong IdBasePriceList
        {
            get
            {
                return idBasePriceList;
            }

            set
            {
                idBasePriceList = value;
                OnPropertyChanged("IdBasePriceList");
            }
        }

        [NotMapped]
        [DataMember]
        public float? SellPriceValue
        {
            get
            {
                return sellPriceValue;
            }

            set
            {
                sellPriceValue = value;
                OnPropertyChanged("SellPriceValue");
            }
        }

        [NotMapped]
        [DataMember]
        public ulong IdCustomerPriceList
        {
            get
            {
                return idCustomerPriceList;
            }

            set
            {
                idCustomerPriceList = value;
                OnPropertyChanged("IdCustomerPriceList");
            }
        }
        [NotMapped]
        [DataMember]
        public float? CurrencyConversionRate_New
        {
            get
            {
                return currencyConversionRate_New;
            }

            set
            {
                currencyConversionRate_New = value;
                OnPropertyChanged("CurrencyConversionRate_New");
            }
        }

        [NotMapped]
        [DataMember]
        public uint IdDetection
        {
            get
            {
                return idDetection;
            }

            set
            {
                idDetection = value;
                OnPropertyChanged("IdDetection");
            }
        }


        [NotMapped]
        [DataMember]
        public byte[] CurrencyIconbytes
        {
            get { return currencyIconbytes; }
            set
            {
                currencyIconbytes = value;
                OnPropertyChanged("CurrencyIconbytes");
            }
        }

        [NotMapped]
        [DataMember]
        public ImageSource CurrencyIconImage
        {
            get { return currencyIconImage; }
            set
            {
                currencyIconImage = value;
                OnPropertyChanged("CurrencyIconImage");
            }
        }


        [NotMapped]
        [DataMember]
        public string ISOCode
        {
            get { return iSOCode; }
            set
            {
                iSOCode = value;
                OnPropertyChanged("ISOCode");
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
        public override string ToString()
        {
            return ISOCode;
        }


        #endregion
    }
}
