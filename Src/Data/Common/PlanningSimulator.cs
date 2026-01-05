using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
   public class PlanningSimulator : ModelBase, IDisposable
    {
        #region Fields
        Int32 idArticle;
        string reference;
        string week;
        Int64 idOT;
        string otCode;
        string customer;
        Int64 actualQty;
        Int64 downloadedQty;
        Int64? otRemainingQty;
        DateTime? otDeliveryDate;
        Int64 idWarehousePurchaseOrder;
        string poCode;
        string supplier;
        Int64 orderQty;
        Int64 receivedQuantity;
        Int64? poRemainingQty;
        DateTime? poDeliveryDate;
        Int64 expectedStock;
        bool isOut;
        Int64 currentArticleStock;
        Int64? nextOtItemPickingStock;
        Int64 rowPointer;
        Int64 articleCurrentStock;
        Int64? articleMinimumStock;
        Int64? articleMaximumStock;
        #endregion

        #region Constructor
        public PlanningSimulator()
        {
        }

        #endregion

        #region Properties
        [NotMapped]
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

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public string Week
        {
            get
            {
                return week;
            }

            set
            {
                week = value;
                OnPropertyChanged("Week");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 IdOT
        {
            get
            {
                return idOT;
            }

            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }


        [NotMapped]
        [DataMember]
        public string OTCode
        {
            get
            {
                return otCode;
            }

            set
            {
                otCode = value;
                OnPropertyChanged("OTCode");
            }
        }


        [NotMapped]
        [DataMember]
        public string Customer
        {
            get
            {
                return customer;
            }

            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }


        [NotMapped]
        [DataMember]
        public Int64 ActualQty
        {
            get
            {
                return actualQty;
            }

            set
            {
                actualQty = value;
                OnPropertyChanged("ActualQty");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 DownloadedQty
        {
            get
            {
                return downloadedQty;
            }

            set
            {
                downloadedQty = value;
                OnPropertyChanged("DownloadedQty");
            }
        }


        [NotMapped]
        [DataMember]
        public Int64? OTRemainingQty
        {
            get
            {
                return otRemainingQty;
            }

            set
            {
                otRemainingQty = value;
                OnPropertyChanged("OTRemainingQty");
            }
        }


        [NotMapped]
        [DataMember]
        public DateTime? OTDeliveryDate
        {
            get
            {
                return otDeliveryDate;
            }

            set
            {
                otDeliveryDate = value;
                OnPropertyChanged("OTDeliveryDate");
            }
        }


        [NotMapped]
        [DataMember]
        public Int64 IdWarehousePurchaseOrder
        {
            get
            {
                return idWarehousePurchaseOrder;
            }

            set
            {
                idWarehousePurchaseOrder = value;
                OnPropertyChanged("IdWarehousePurchaseOrder");
            }
        }


        [NotMapped]
        [DataMember]
        public string POCode
        {
            get
            {
                return poCode;
            }

            set
            {
                poCode = value;
                OnPropertyChanged("POCode");
            }
        }

        [NotMapped]
        [DataMember]
        public string Supplier
        {
            get
            {
                return supplier;
            }

            set
            {
                supplier = value;
                OnPropertyChanged("Supplier");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 OrderQty
        {
            get
            {
                return orderQty;
            }

            set
            {
                orderQty = value;
                OnPropertyChanged("OrderQty");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 ReceivedQuantity
        {
            get
            {
                return receivedQuantity;
            }

            set
            {
                receivedQuantity = value;
                OnPropertyChanged("ReceivedQuantity");
            }
        }


        [NotMapped]
        [DataMember]
        public Int64? PORemainingQty
        {
            get
            {
                return poRemainingQty;
            }

            set
            {
                poRemainingQty = value;
                OnPropertyChanged("PORemainingQty");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? PODeliveryDate
        {
            get
            {
                return poDeliveryDate;
            }

            set
            {
                poDeliveryDate = value;
                OnPropertyChanged("PODeliveryDate");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 ExpectedStock
        {
            get
            {
                return expectedStock;
            }

            set
            {
                expectedStock = value;
                OnPropertyChanged("ExpectedStock");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsOut
        {
            get
            {
                return isOut;
            }

            set
            {
                isOut = value;
                OnPropertyChanged("IsOut");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 CurrentArticleStock
        {
            get
            {
                return currentArticleStock;
            }

            set
            {
                currentArticleStock = value;
                OnPropertyChanged("CurrentArticleStock");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64? NextOtItemPickingStock
        {
            get
            {
                return nextOtItemPickingStock;
            }

            set
            {
                nextOtItemPickingStock = value;
                OnPropertyChanged("NextOtItemPickingStock");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 RowPointer
        {
            get
            {
                return rowPointer;
            }

            set
            {
                rowPointer = value;
                OnPropertyChanged("RowPointer");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 ArticleCurrentStock
        {
            get
            {
                return articleCurrentStock;
            }

            set
            {
                articleCurrentStock = value;
                OnPropertyChanged("ArticleCurrentStock");
            }
        }


        [NotMapped]
        [DataMember]
        public Int64? ArticleMinimumStock
        {
            get
            {
                return articleMinimumStock;
            }

            set
            {
                articleMinimumStock = value;
                OnPropertyChanged("ArticleMinimumStock");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64? ArticleMaximumStock
        {
            get
            {
                return articleMaximumStock;
            }

            set
            {
                articleMaximumStock = value;
                OnPropertyChanged("ArticleMaximumStock");
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
