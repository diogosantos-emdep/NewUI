using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PLM
{
    [DataContract]
    public class ArticleCostSellPrice : ModelBase, IDisposable
    {
        #region Declaration
   
        UInt64 idArticle;
        UInt32 idCompany;
        UInt32 idCurrency;
        double articleCostValue;
        double sellCostValue;
        #endregion

        #region Properties
       
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

        [DataMember]
        public uint IdCompany
        {
            get
            {
                return idCompany;
            }

            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [DataMember]
        public uint IdCurrency
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

        [DataMember]
        public double ArticleCostValue
        {
            get
            {
                return articleCostValue;
            }

            set
            {
                articleCostValue = value;
                OnPropertyChanged("ArticleCostValue");
            }
        }

        [DataMember]
        public double SellCostValue
        {
            get
            {
                return sellCostValue;
            }

            set
            {
                sellCostValue = value;
                OnPropertyChanged("SellCostValue");
            }
        }

       
        #endregion

        #region Constructor

        public ArticleCostSellPrice()
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
            ArticleCostPrice articleCostPrice = (ArticleCostPrice)this.MemberwiseClone();
            return articleCostPrice;
        }

        #endregion
    }
}
