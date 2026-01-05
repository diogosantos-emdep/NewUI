using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PLM
{
    [DataContract]
    public class ArticleCostPrice : ModelBase, IDisposable
    {
        #region Declaration
        string alias;
        UInt64 idArticleCostPrice;
        UInt64 idArticle;
        UInt32 idCompany;
        UInt32 idCurrency;
        double articleCostValue;
        DateTime? exchangeRateDate;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;
        double articleAdditionalCost;
        double articleCostValueAVG;
        #endregion

        #region Properties
        [DataMember]
        public ulong IdArticleCostPrice
        {
            get
            {
                return idArticleCostPrice;
            }

            set
            {
                idArticleCostPrice = value;
                OnPropertyChanged("IdArticleCostPrice");
            }
        }

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
        public string Alias
        {
            get
            {
                return alias;
            }

            set
            {
                alias = value;
                OnPropertyChanged("Alias");
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
        public DateTime? ExchangeRateDate
        {
            get
            {
                return exchangeRateDate;
            }

            set
            {
                exchangeRateDate = value;
                OnPropertyChanged("ExchangeRateDate");
            }
        }
        [DataMember]
        public uint IdCreator
        {
            get
            {
                return idCreator;
            }

            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public uint? IdModifier
        {
            get
            {
                return idModifier;
            }

            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }
        }

        [DataMember]
        public DateTime? ModificationDate
        {
            get
            {
                return modificationDate;
            }

            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [DataMember]
        public double ArticleAdditionalCost
        {
            get
            {
                return articleAdditionalCost;
            }

            set
            {
                articleAdditionalCost = value;
                OnPropertyChanged("ArticleAdditionalCost");
            }
        }


        [DataMember]
        public double ArticleCostValueAVG
        {
            get
            {
                return articleCostValueAVG;
            }

            set
            {
                articleCostValueAVG = value;
                OnPropertyChanged("ArticleCostValueAVG");
            }
        }
        #endregion

        #region Constructor

        public ArticleCostPrice()
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
