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
  
    [DataContract]
    public class Revision : ModelBase, IDisposable
    {
        #region Fields
        DateTime approvedIn;
        string attachedFiles;
        string comments;
        Int32 createdBy;
        DateTime createdIn;
        decimal discount;
        DateTime expireDate;
        Int64 id;
        bool itemModified;
        bool modified;
        Int32 numRevision;
        Int32 reviewedBy;
        bool sentToClient;
        string sentToComments;
        Dictionary<string, RevisionItem> items;
        Quotation quotation;
        Int64 maxNumItem;
        //[Rahul.Gadhave][GEOS2-9141][Date:02-08-2025]
        int idCurency;
        #endregion

        #region Constructor
        public Revision()
        {

        }
        #endregion

        #region Properties
         [NotMapped]  
        [DataMember]
        public Int64 Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime ApprovedIn
        {
            get
            {
                return approvedIn;
            }

            set
            {
                approvedIn = value;
                OnPropertyChanged("ApprovedIn");
            }
        }

        [NotMapped]
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

        [NotMapped]
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

        [NotMapped]
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

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public decimal Discount
        {
            get
            {
                return discount;
            }

            set
            {
                discount = value;
                OnPropertyChanged("Discount");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime ExpireDate
        {
            get
            {
                return expireDate;
            }

            set
            {
                expireDate = value;
                OnPropertyChanged("ExpireDate");
            }
        }

        [NotMapped]
        [DataMember]
        public bool ItemModified
        {
            get
            {
                return itemModified;
            }

            set
            {
                itemModified = value;
                OnPropertyChanged("itemModified");
            }
        }

        [NotMapped]
        [DataMember]
        public bool Modified
        {
            get
            {
                return modified;
            }

            set
            {
                modified = value;
                OnPropertyChanged("Modified");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 NumRevision
        {
            get
            {
                return numRevision;
            }

            set
            {
                numRevision = value;
                OnPropertyChanged("NumRevision");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 ReviewedBy
        {
            get
            {
                return reviewedBy;
            }

            set
            {
                reviewedBy = value;
                OnPropertyChanged("ReviewedBy");
            }
        }

        [NotMapped]
        [DataMember]
        public bool SentToClient
        {
            get
            {
                return sentToClient;
            }

            set
            {
                sentToClient = value;
                OnPropertyChanged("SentToClient");
            }
        }

        [NotMapped]
        [DataMember]
        public string SentToComments
        {
            get
            {
                return sentToComments;
            }

            set
            {
                sentToComments = value;
                OnPropertyChanged("SentToComments");
            }
        }

        [NotMapped]
        [DataMember]
        public Dictionary<string, RevisionItem> Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
                OnPropertyChanged("Items");
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
        public Int64 MaxNumItem
        {
            get
            {
                return maxNumItem;
            }

            set
            {
                maxNumItem = value;
                OnPropertyChanged("MaxNumItem");
            }
        }

        //[Rahul.Gadhave][GEOS2-9141][Date:02-08-2025]
        [NotMapped]
        [DataMember]
        public int IdCurrency
        {
            get
            {
                return idCurency;
            }
            set
            {

                idCurency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        DateTime closed;
        [DataMember]
        public DateTime Closed
        {
            get
            {
                return closed;
            }

            set
            {
                closed = value;
                OnPropertyChanged("Closed");
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
