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
    [Table("suppliercomplaints")]
    [DataContract]
    public class SupplierComplaint : ModelBase, IDisposable
    {
        #region Fields
        string reasonClosed;
        DateTime modifiedIn;
        Int64 modifiedBy;
        sbyte isClosed;
        Int64 idWarehouse;
        Int64 idSupplierComplaint;
        byte idCurrency;
        Int64 idComplaintStatus;
        byte idComplaintSolution;
        Int64 idComplaintReason;
        Int64 idArticleSupplier;
        DateTime expectedDate;
        string description;
        DateTime createdIn;
        Int64 createdBy;
        string comments;
        string code;
        Int32 attachedPo;
        string attachedDocuments;
        ArticleSupplier supplier;
        ComplaintStatus complaintStatus;
        double actualQuantity;
        double downloadedQuantity;
        double remainingQuantity;
        Int16 progress;
        People createdByPerson;
        People modifiedByPerson;
        List<SupplierComplaintItem> supplierComplaintItems;
        #endregion

        #region Constructor
        public SupplierComplaint()
        {

        }
        #endregion

        #region Properties
        [Key]
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

        [Column("ReasonClosed")]
        [DataMember]
        public string ReasonClosed
        {
            get
            {
                return reasonClosed;
            }

            set
            {
                reasonClosed = value;
                OnPropertyChanged("ReasonClosed");
            }
        }

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime ModifiedIn
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
        public Int64 ModifiedBy
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

        [Column("IsClosed")]
        [DataMember]
        public sbyte IsClosed
        {
            get
            {
                return isClosed;
            }

            set
            {
                isClosed = value;
                OnPropertyChanged("IsClosed");
            }
        }

        [Column("IdWarehouse")]
        [DataMember]
        public Int64 IdWarehouse
        {
            get
            {
                return idWarehouse;
            }

            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }

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


        [Column("IdComplaintStatus")]
        [DataMember]
        public Int64 IdComplaintStatus
        {
            get
            {
                return idComplaintStatus;
            }

            set
            {
                idComplaintStatus = value;
                OnPropertyChanged("IdComplaintStatus");
            }
        }

        [Column("IdComplaintSolution")]
        [DataMember]
        public byte IdComplaintSolution
        {
            get
            {
                return idComplaintSolution;
            }

            set
            {
                idComplaintSolution = value;
                OnPropertyChanged("IdComplaintSolution");
            }
        }

        [Column("IdComplaintReason")]
        [DataMember]
        public Int64 IdComplaintReason
        {
            get
            {
                return idComplaintReason;
            }

            set
            {
                idComplaintReason = value;
                OnPropertyChanged("IdComplaintReason");
            }
        }

        [Column("IdArticleSupplier")]
        [DataMember]
        public Int64 IdArticleSupplier
        {
            get
            {
                return idArticleSupplier;
            }

            set
            {
                idArticleSupplier = value;
                OnPropertyChanged("IdArticleSupplier");
            }
        }

        [Column("ExpectedDate")]
        [DataMember]
        public DateTime ExpectedDate
        {
            get
            {
                return expectedDate;
            }

            set
            {
               expectedDate = value;
                OnPropertyChanged("ExpectedDate");
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

        [Column("CreatedIn")]
        [DataMember]
        public DateTime CreatedIn
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
        public Int64 CreatedBy
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

        [Column("Comments")]
        [DataMember]
        public string Comments
        {
            get
            {
                return comments;
            }

            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        [Column("Code")]
        [DataMember]
        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [Column("AttachedPo")]
        [DataMember]
        public Int32 AttachedPo
        {
            get
            {
                return attachedPo;
            }

            set
            {
                attachedPo = value;
                OnPropertyChanged("AttachedPo");
            }
        }

        [Column("AttachedDocuments")]
        [DataMember]
        public string AttachedDocuments
        {
            get
            {
                return attachedDocuments;
            }

            set
            {
                attachedDocuments = value;
                OnPropertyChanged("AttachedDocuments");
            }
        }

        [NotMapped]
        [DataMember]
        public ArticleSupplier Supplier
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
        public ComplaintStatus ComplaintStatus
        {
            get
            {
                return complaintStatus;
            }

            set
            {
                complaintStatus = value;
                OnPropertyChanged("ComplaintStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public double ActualQuantity
        {
            get
            {
                return actualQuantity;
            }

            set
            {
                actualQuantity = value;
                OnPropertyChanged("ActualQuantity");
            }
        }

        [NotMapped]
        [DataMember]
        public double DownloadedQuantity
        {
            get
            {
                return downloadedQuantity;
            }

            set
            {
                downloadedQuantity = value;
                OnPropertyChanged("DownloadedQuantity");
            }
        }

        [NotMapped]
        [DataMember]
        public double RemainingQuantity
        {
            get
            {
                return remainingQuantity;
            }

            set
            {
                remainingQuantity = value;
                OnPropertyChanged("RemainingQuantity");
            }
        }

        [NotMapped]
        [DataMember]
        public Int16 Progress
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
        public People CreatedByPerson
        {
            get
            {
                return createdByPerson;
            }

            set
            {
                createdByPerson = value;
                OnPropertyChanged("CreatedByPerson");
            }
        }


        [NotMapped]
        [DataMember]
        public People ModifiedByPerson
        {
            get
            {
                return modifiedByPerson;
            }

            set
            {
                modifiedByPerson = value;
                OnPropertyChanged("ModifiedByPerson");
            }
        }


        [NotMapped]
        [DataMember]
        public List<SupplierComplaintItem> SupplierComplaintItems
        {
            get
            {
                return supplierComplaintItems;
            }

            set
            {
                supplierComplaintItems = value;
                OnPropertyChanged("SupplierComplaintItems");
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
