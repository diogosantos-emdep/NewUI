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
    [Table("suppliercomplaintsitems")]
    [DataContract]
    public class SupplierComplaintItem : ModelBase, IDisposable
    {
        #region Fields
        double unitPrice;
        sbyte replacementType;
        Int64 quantity;
        Int64 position;
        Int64 idWarehousepurchaseOrderItem;
        Int64 idSupplierComplaintItem;
        Int64 idSupplierComplaint;
        Int32 idArticle;
        string description;
        Int32 keyId;
        Int32 parentId;
        Article article;
        Int64 downloadedQuantity;
        Int64 remainingQuantity;
        Int64 progress;
        List<PickingMaterialsSC> pickingMaterialsSCList;
        #endregion

        #region Constructor
        public SupplierComplaintItem()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdSupplierComplaintItem")]
        [DataMember]
        public Int64 IdSupplierComplaintItem
        {
            get
            {
                return idSupplierComplaintItem;
            }

            set
            {
                idSupplierComplaintItem = value;
                OnPropertyChanged("IdSupplierComplaintItem");
            }
        }

        [Column("IdSupplierComplaint")]
        [DataMember]
        public Int64 IdSupplierComplaint
        {
            get
            {
                return idSupplierComplaint;
            }

            set
            {
                idSupplierComplaint = value;
                OnPropertyChanged("IdSupplierComplaint");
            }
        }

        [Column("IdArticle")]
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

        [Column("unitPrice")]
        [DataMember]
        public double UnitPrice
        {
            get
            {
                return unitPrice;
            }

            set
            {
                unitPrice = value;
                OnPropertyChanged("UnitPrice");
            }
        }

        [Column("ReplacementType")]
        [DataMember]
        public sbyte ReplacementType
        {
            get
            {
                return replacementType;
            }

            set
            {
                replacementType = value;
                OnPropertyChanged("ReplacementType");
            }
        }

        [Column("quantity")]
        [DataMember]
        public Int64 Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        [Column("IdWarehousepurchaseOrderItem")]
        [DataMember]
        public Int64 IdWarehousepurchaseOrderItem
        {
            get
            {
                return idWarehousepurchaseOrderItem;
            }

            set
            {
                idWarehousepurchaseOrderItem = value;
                OnPropertyChanged("IdWarehousepurchaseOrderItem");
            }
        }


        [Column("description")]
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

        [Column("position")]
        [DataMember]
        public Int64 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        [NotMapped]
        [DataMember]
        public int KeyId
        {
            get { return keyId; }
            set
            {
                keyId = value;
                OnPropertyChanged("KeyId");
            }
        }

        [NotMapped]
        [DataMember]
        public int ParentId
        {
            get { return parentId; }
            set
            {
                parentId = value;
                OnPropertyChanged("ParentId");
            }
        }


        [NotMapped]
        [DataMember]
        public Article Article
        {
            get { return article; }
            set
            {
                article = value;
                OnPropertyChanged("Article");
            }
        }


        [NotMapped]
        [DataMember]
        public Int64 DownloadedQuantity
        {
            get { return downloadedQuantity; }
            set
            {
                downloadedQuantity = value;
                OnPropertyChanged("DownloadedQuantity");
            }
        }


        [NotMapped]
        [DataMember]
        public Int64 RemainingQuantity
        {
            get { return remainingQuantity; }
            set
            {
                remainingQuantity = value;
                OnPropertyChanged("RemainingQuantity");
            }
        }


        [NotMapped]
        [DataMember]
        public Int64 Progress
        {
            get
            {
                return progress;
            }

            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }


        [NotMapped]
        [DataMember]
        public List<PickingMaterialsSC> PickingMaterialsSCList
        {
            get
            {
                return pickingMaterialsSCList;
            }

            set
            {
                pickingMaterialsSCList = value;
                OnPropertyChanged("PickingMaterialsSCList");
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
