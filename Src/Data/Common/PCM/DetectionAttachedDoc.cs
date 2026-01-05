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
    public class DetectionAttachedDoc : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idDetectionAttachedDoc;
        UInt32 idDetection;
        string savedFileName;
        string description;
        string originalFileName;
        UInt32 createdBy;
        DateTime createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;
        UInt32 idDocType;

        DocumentType documentType;
        byte[] detectionAttachedDocInBytes;
        DateTime? updatedDate;
        LookupValue attachmentType;
        #endregion

        #region Constructor

        public DetectionAttachedDoc()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public UInt32 IdDetectionAttachedDoc
        {
            get
            {
                return idDetectionAttachedDoc;
            }

            set
            {
                idDetectionAttachedDoc = value;
                OnPropertyChanged("IdDetectionAttachedDoc");
            }
        }

        [DataMember]
        public UInt32 IdDetection
        {
            get
            {
                return idDetection;
            }

            set
            {
                idDetection = value;
                OnPropertyChanged("IdDetection");
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
        public byte[] DetectionAttachedDocInBytes
        {
            get
            {
                return detectionAttachedDocInBytes;
            }

            set
            {
                detectionAttachedDocInBytes = value;
                OnPropertyChanged("DetectionAttachedDocInBytes");
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

        [DataMember]
        public LookupValue AttachmentType
        {
            get
            {
                return attachmentType;
            }

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
            DetectionAttachedDoc detectionAttachedDoc = (DetectionAttachedDoc)this.MemberwiseClone();

            if (DocumentType != null)
                detectionAttachedDoc.DocumentType = (DocumentType)this.DocumentType.Clone();

            return detectionAttachedDoc;
        }

        #endregion
    }
}
