using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class CatalogueItemAttachedDoc : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idCatalogueItemAttachedDoc;
        UInt32 idCatalogueItem;
        string savedFileName;
        string description;
        string originalFileName;
        UInt32 createdBy;
        DateTime createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;
        UInt32 idDocType;

        DocumentType documentType;
        byte[] catalogueItemAttachedDocInBytes;
        DateTime? updatedDate;

        #endregion

        #region Constructor

        public CatalogueItemAttachedDoc()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public UInt32 IdCatalogueItemAttachedDoc
        {
            get
            {
                return idCatalogueItemAttachedDoc;
            }

            set
            {
                idCatalogueItemAttachedDoc = value;
                OnPropertyChanged("IdCatalogueItemAttachedDoc");
            }
        }

        [DataMember]
        public UInt32 IdCatalogueItem
        {
            get
            {
                return idCatalogueItem;
            }

            set
            {
                idCatalogueItem = value;
                OnPropertyChanged("IdCatalogueItem");
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
        public UInt32 IdDocType
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
        public byte[] CatalogueItemAttachedDocInBytes
        {
            get
            {
                return catalogueItemAttachedDocInBytes;
            }

            set
            {
                catalogueItemAttachedDocInBytes = value;
                OnPropertyChanged("CatalogueItemAttachedDocInBytes");
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

        #endregion

        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            CatalogueItemAttachedDoc catalogueItemAttachedDoc = (CatalogueItemAttachedDoc)this.MemberwiseClone();

            if (DocumentType != null)
                catalogueItemAttachedDoc.DocumentType = (DocumentType)this.DocumentType.Clone();

            return catalogueItemAttachedDoc;
        }

        #endregion
    }
}
