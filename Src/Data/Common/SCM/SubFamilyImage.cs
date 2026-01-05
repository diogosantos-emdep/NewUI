using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.SCM
{//[Sudhir.Jangra][GEOS2-4565]

    [DataContract]
    public class SubFamilyImage : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idSubFamilyImage;
        UInt32 idSubFamily;
        string savedFileName;
        string description;
        string originalFileName;
        UInt32 createdBy;
        DateTime createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;
        UInt64 position;

        byte[] subFamilyImageInBytes;
        DateTime? updatedDate;
        ImageSource attachmentImage;
        UInt32 idSCMFamilyImage;
        UInt64 imagePosition;
        byte[] connectorSubFamilyImageInBytes;
        byte[] connectorFamilyImageInBytes;
        
        #endregion

        #region Properties

        [DataMember]
        public UInt32 IdSubFamilyImage
        {
            get
            {
                return idSubFamilyImage;
            }

            set
            {
                idSubFamilyImage = value;
                OnPropertyChanged("IdSubFamilyImage");
            }
        }

        [DataMember]
        public UInt32 IdSubFamily
        {
            get
            {
                return idSubFamily;
            }

            set
            {
                idSubFamily = value;
                OnPropertyChanged("IdSubFamily");
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
        public UInt64 Position
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

        [DataMember]
        public byte[] SubFamilyImageInBytes
        {
            get
            {
                return subFamilyImageInBytes;
            }

            set
            {
                subFamilyImageInBytes = value;
                OnPropertyChanged("SubFamilyImageInBytes");
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
        public ImageSource AttachmentImage
        {
            get
            {
                return attachmentImage;
            }

            set
            {
                attachmentImage = value;
                OnPropertyChanged("AttachmentImage");
            }
        }

        //[Aishwarya Ingale[23-08-23]]
        [DataMember]
        public byte[] ConnectorFamilyImageInBytes
        {
            get
            {
                return connectorFamilyImageInBytes;
            }

            set
            {
                connectorFamilyImageInBytes = value;
                OnPropertyChanged("ConnectorFamilyImageInBytes");
            }
        }

        //[Aishwarya ingale[23-08-23]]
        [DataMember]
        public byte[] ConnectorSubFamilyImageInBytes
        {
            get
            {
                return connectorFamilyImageInBytes;
            }

            set
            {
                connectorFamilyImageInBytes = value;
                OnPropertyChanged("ConnectorFamilyImageInBytes");
            }
        }


     

        [DataMember]
        public UInt64 ImagePosition
        {
            get { return imagePosition; }
            set { imagePosition = value; OnPropertyChanged("ImagePosition"); }
        }

        [DataMember]
        public UInt32 IdSCMFamilyImage
        {
            get { return idSCMFamilyImage; }
            set { idSCMFamilyImage = value; OnPropertyChanged("IdSCMFamilyImage"); }
        }
        #endregion

        #region Constructor
        public SubFamilyImage()
        {

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
