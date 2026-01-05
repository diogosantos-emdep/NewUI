using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{
    [DataContract]
    public class ArticleMaterialType : ModelBase, IDisposable
    {

        #region Declaration
        Int64 stockWeek;
        double articleStockAmount;
        string materialTypeValue;
        Int64 materialTypeId;
        string htmlColor;
        string abbreviation;
        #endregion

        #region Properties
        [DataMember]
        public Int64 StockWeek
        {
            get
            {
                return stockWeek;
            }

            set
            {
                stockWeek = value;
            }
        }
        [DataMember]
        public double ArticleStockAmount
        {
            get
            {
                return articleStockAmount;
            }

            set
            {
                articleStockAmount = value;
                OnPropertyChanged("ArticleStockAmount");
            }
        }

        [DataMember]
        public string MaterialTypeValue
        {
            get
            {
                return materialTypeValue;
            }

            set
            {
                materialTypeValue = value;
                OnPropertyChanged("MaterialTypeValue");
            }
        }

        [DataMember]
        public Int64 MaterialTypeId
        {
            get
            {
                return materialTypeId;
            }

            set
            {
                materialTypeId = value;
                OnPropertyChanged("MaterialTypeId");
            }
        }

        [DataMember]
        public string HtmlColor
        {
            get
            {
                return htmlColor;
            }

            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }
        [DataMember]
        public string Abbreviation
        {
            get
            {
                return abbreviation;
            }

            set
            {
                abbreviation = value;
                OnPropertyChanged("Abbreviation");
            }
        }
        #endregion

        #region Constructor

        public ArticleMaterialType()
        {
        }

        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        //public override object Clone()
        //{
        //    ArticleCostPrice articleCostPrice = (ArticleCostPrice)this.MemberwiseClone();
        //    return articleCostPrice;
        //}

        #endregion
    }
}
