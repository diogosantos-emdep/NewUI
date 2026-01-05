using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMArticleStockPlanning : ModelBase, IDisposable
    {
        // [GEOS2-6886][Pallavi jadhav][14 02 2025]
        #region Field    
        private Int32 idArticle;
        private string reference;
        private string description;
        private Int32 stock;
        private Int32 minimumStock;
        private Int32 maximumStock;
        private Int32 incomingStock;
        private Int32 outgoingStock;
        private Int32 projectedStock;
        private string warehousName;
        private string projectedStockHtmlColor;//[GEOS2-6887][rani dhamankar][26-02-2025]

        #endregion

        #region Property
        [DataMember]
        public Int32 IdArticle
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
        public string WarehousName
        {
            get
            {
                return warehousName;
            }

            set
            {
                warehousName = value;
                OnPropertyChanged("WarehousName");
            }
        }
        [DataMember]
        public string Reference
        {
            get
            {
                return reference;
            }

            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }
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

        [DataMember]
        public Int32 Stock
        {
            get
            {
                return stock;
            }

            set
            {
                stock = value;
                OnPropertyChanged("Stock");
            }
        }

        [DataMember]
        public Int32 MinimumStock
        {
            get
            {
                return minimumStock;
            }

            set
            {
                minimumStock = value;
                OnPropertyChanged("MinimumStock");
            }
        }

        [DataMember]
        public Int32 MaximumStock
        {
            get
            {
                return maximumStock;
            }

            set
            {
                maximumStock = value;
                OnPropertyChanged("MaximumStock");
            }
        }

        [DataMember]
        public Int32 IncomingStock
        {
            get
            {
                return incomingStock;
            }

            set
            {
                incomingStock = value;
                OnPropertyChanged("IncomingStock");
            }
        }

        [DataMember]
        public Int32 OutgoingStock
        {
            get
            {
                return outgoingStock;
            }

            set
            {
                outgoingStock = value;
                OnPropertyChanged("OutgoingStock");
            }
        }

        [DataMember]
        public Int32 ProjectedStock
        {
            get
            {
                return projectedStock;
            }

            set
            {
                projectedStock = value;
                OnPropertyChanged("ProjectedStock");
            }
        }

        //[GEOS2-6887][rani dhamankar][26-02-2025]
        [DataMember]
        public string ProjectedStockHtmlColor
        {
            get
            {
                return projectedStockHtmlColor;
            }

            set
            {
                projectedStockHtmlColor = value;
                OnPropertyChanged("ProjectedStockHtmlColor");
            }
        }


        #endregion

        #region Constructor
        public ERMArticleStockPlanning()
        {

        }
        #endregion


        #region Method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
