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
