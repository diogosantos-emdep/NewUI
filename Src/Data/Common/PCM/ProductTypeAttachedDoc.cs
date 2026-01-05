using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class ProductTypeAttachedDoc : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idCPTypeAttachedDoc;
        UInt64 idCPType;
        string savedFileName;
        string description;
        string originalFileName;

        UInt32 createdBy;
        DateTime createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;
        Int32 idDocType;
        
        DocumentType documentType;
        byte[] productTypeAttachedDocInBytes;
        DateTime? updatedDate;
        LookupValue attachmentType;
        #endregion

        #region Constructor

        public ProductTypeAttachedDoc()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public UInt32 IdCPTypeAttachedDoc
        {
            get
            {
                return idCPTypeAttachedDoc;
            }

            set
            {
                idCPTypeAttachedDoc = value;
                OnPropertyChanged("IdCPTypeAttachedDoc");
            }
        }

        [DataMember]
        public UInt64 IdCPType
        {
            get
            {
                return idCPType;
            }

            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
            }
        }

        [DataMember]
        public string SavedFileName
        {
            get
            {
                return savedFileName;
            }

            set
            {
                savedFileName = value;
                OnPropertyChanged("SavedFileName");
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
        public string OriginalFileName
        {
            get
            {
                return originalFileName;
            }

            set
            {
                originalFileName = value;
                OnPropertyChanged("OriginalFileName");
            }
        }

        [DataMember]
        public UInt32 CreatedBy
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

        [DataMember]
        public UInt32 ModifiedBy
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

        [DataMember]
        public Int32 IdDocType
        {
            get
            {
                return idDocType;
            }

            set
            {
                idDocType = value;
                OnPropertyChanged("IdDocType");
            }
        }

        [DataMember]
        public byte[] ProductTypeAttachedDocInBytes
        {
            get
            {
                return productTypeAttachedDocInBytes;
            }

            set
            {
                productTypeAttachedDocInBytes = value;
                OnPropertyChanged("ProductTypeAttachedDocInBytes");
            }
        }

        [DataMember]
        public DocumentType DocumentType
        {
            get
            {
                return documentType;
            }

            set
            {
                documentType = value;
                OnPropertyChanged("DocumentType");
            }
        }
        
        [DataMember]
        public DateTime? UpdatedDate
        {
            get
            {
                return updatedDate;
            }

            set
            {
                updatedDate = value;
                OnPropertyChanged("UpdatedDate");
            }
        }
        //[Sudhir.Jangra][GEOS2-4072][09/12/2022]
       
        [DataMember]
        public LookupValue AttachmentType
        {
            get { return attachmentType; }
            set
            {
                attachmentType = value;
                OnPropertyChanged("AttachmentType");
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
            ProductTypeAttachedDoc productTypeAttachedDoc = (ProductTypeAttachedDoc)this.MemberwiseClone();

            if (DocumentType != null)
                productTypeAttachedDoc.DocumentType = (DocumentType)this.DocumentType.Clone();

            return productTypeAttachedDoc;
        }

        #endregion
    }
}
