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
    [Table("OtItem")]
    [DataContract]
    public class OtItem : ModelBase, IDisposable
    {
        #region Fields

        byte rework;
        DateTime? modifiedIn;
        Int32 modifiedBy;
        DateTime? latestStatusChange;
        byte isBatch;
        Int64 idRevisionItem;
        Int64 idOTItem;
        Int64 idOT;
        byte idItemOtStatus;
        DateTime? docGeneratedIn;
        byte customerCommentRead;
        string customerComment;
        DateTime? createdIn;
        Int32 createdBy;
        string attachedFiles;
        Int32 assignedTo;
        People assignedToUser;
        RevisionItem revisionItem;
        ItemOTStatusType itemOTStatusType;
        Ots ot;
        Quotation quotation;

        Int32 keyId;
        Int32 parentId;

        List<PickingMaterials> pickingMaterialsList;

        DateTime? shippingDate;

        List<Counterpart> counterparts;

        List<OtItem> articleDecomposedList;
        Int64 articleStock;
        Int64 articleMinimumStock;
        Int64 parentArticleType;
        #endregion

        #region Constructor
        public OtItem()
        {

        }
        #endregion

        #region Properties
        [Column("Rework")]
        [DataMember]
        public byte Rework
        {
            get
            {
                return rework;
            }

            set
            {
                rework = value;
                OnPropertyChanged("Rework");
            }
        }

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime? ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }
        [Column("ModifiedBy")]
        [DataMember]
        public Int32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }

            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }
        [Column("LatestStatusChange")]
        [DataMember]
        public DateTime? LatestStatusChange
        {
            get
            {
                return latestStatusChange;
            }

            set
            {
                latestStatusChange = value;
                OnPropertyChanged("LatestStatusChange");
            }
        }

        [Column("IsBatch")]
        [DataMember]
        public byte IsBatch
        {
            get
            {
                return isBatch;
            }

            set
            {
                isBatch = value;
                OnPropertyChanged("IsBatch");
            }
        }

        [Column("IdRevisionItem")]
        [DataMember]
        public Int64 IdRevisionItem
        {
            get
            {
                return idRevisionItem;
            }

            set
            {
                idRevisionItem = value;
                OnPropertyChanged("IdRevisionItem");
            }
        }

        [Column("IdOTItem")]
        [DataMember]
        public Int64 IdOTItem
        {
            get
            {
                return idOTItem;
            }

            set
            {
                idOTItem = value;
                OnPropertyChanged("IdOTItem");
            }
        }

        [Column("IdOT")]
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

        [Column("IdItemOtStatus")]
        [DataMember]
        public byte IdItemOtStatus
        {
            get
            {
                return idItemOtStatus;
            }

            set
            {
                idItemOtStatus = value;
                OnPropertyChanged("IdItemOtStatus");
            }
        }

        [Column("DocGeneratedIn")]
        [DataMember]
        public DateTime? DocGeneratedIn
        {
            get
            {
                return docGeneratedIn;
            }

            set
            {
                docGeneratedIn = value;
                OnPropertyChanged("DocGeneratedIn");
            }
        }

        [Column("CustomerCommentRead")]
        [DataMember]
        public byte CustomerCommentRead
        {
            get
            {
                return customerCommentRead;
            }

            set
            {
                customerCommentRead = value;
                OnPropertyChanged("CustomerCommentRead");
            }
        }

        [Column("CustomerComment")]
        [DataMember]
        public string CustomerComment
        {
            get
            {
                return customerComment;
            }

            set
            {
                customerComment = value;
                OnPropertyChanged("CustomerComment");
            }
        }

        [Column("CreatedIn")]
        [DataMember]
        public DateTime? CreatedIn
        {
            get
            {
                return createdIn;
            }

            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [Column("CreatedBy")]
        [DataMember]
        public Int32 CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [Column("AttachedFiles")]
        [DataMember]
        public string AttachedFiles
        {
            get
            {
                return attachedFiles;
            }

            set
            {
                attachedFiles = value;
                OnPropertyChanged("AttachedFiles");
            }
        }

        [Column("AssignedTo")]
        [DataMember]
        public Int32 AssignedTo
        {
            get
            {
                return assignedTo;
            }

            set
            {
                assignedTo = value;
                OnPropertyChanged("AssignedTo");
            }
        }

        [NotMapped]
        [DataMember]
        public People AssignedToUser
        {
            get { return assignedToUser; }
            set
            {
                assignedToUser = value;
                OnPropertyChanged("AssignedToUser");
            }
        }

        [NotMapped]
        [DataMember]
        public RevisionItem RevisionItem
        {
            get
            {
                return revisionItem;
            }

            set
            {
                revisionItem = value;
                OnPropertyChanged("RevisionItem");
            }
        }

        [NotMapped]
        [DataMember]
        public ItemOTStatusType Status
        {
            get
            {
                return itemOTStatusType;
            }

            set
            {
                itemOTStatusType = value;
                OnPropertyChanged("Status");
            }
        }

        [NotMapped]
        [DataMember]
        public Ots Ot
        {
            get
            {
                return ot;
            }

            set
            {
                ot = value;
                OnPropertyChanged("Ot");
            }
        }

        [NotMapped]
        [DataMember]
        public Quotation Quotation
        {
            get
            {
                return quotation;
            }

            set
            {
                quotation = value;
                OnPropertyChanged("Quotation");
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
        public List<PickingMaterials> PickingMaterialsList
        {
            get
            {
                return pickingMaterialsList;
            }

            set
            {
                pickingMaterialsList = value;
                OnPropertyChanged("Quotation");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? ShippingDate
        {
            get { return shippingDate; }
            set
            {
                shippingDate = value;
                OnPropertyChanged("ShippingDate");
            }
        }

        [NotMapped]
        [DataMember]
        public List<Counterpart> Counterparts
        {
            get { return counterparts; }
            set
            {
                counterparts = value;
                OnPropertyChanged("Counterparts");
            }
        }


        [NotMapped]
        [DataMember]
        public List<OtItem> ArticleDecomposedList
        {
            get { return articleDecomposedList; }
            set
            {
                articleDecomposedList = value;
                OnPropertyChanged("ArticleDecomposedList");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 ArticleStock
        {
            get { return articleStock; }
            set
            {
                articleStock = value;
                OnPropertyChanged("ArticleStock");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 ArticleMinimumStock
        {
            get { return articleMinimumStock; }
            set
            {
                articleMinimumStock = value;
                OnPropertyChanged("ArticleMinimumStock");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 ParentArticleType
        {
            get { return parentArticleType; }
            set
            {
                parentArticleType = value;
                OnPropertyChanged("ParentArticleType");
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
