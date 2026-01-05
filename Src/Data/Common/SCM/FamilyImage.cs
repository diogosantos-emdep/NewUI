using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Media;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class FamilyImage : ModelBase, IDisposable
    {
        #region Fields
        UInt32 idSCMFamilyImage;
        UInt16 idFamily;
        string savedFileName;
        string description;
        string originalFileName;
        UInt64 imagePosition;
        int createdBy;
        int modifiedBy;
        DateTime createdIn;
        DateTime modifiedIn;
        byte[] connectorFamilyImageInBytes;
        DateTime? updatedDate;
        ImageSource attachmentImage;
        UInt64 positionParenttemp;
        UInt64 positiontemp;
        byte[] connectorFamilyImageInBytesParent;
        UInt64 idSCMSubFamilyImage;
        string subsavedFileName;
        string suboriginalFileName;
        UInt64 position;
        Int32 idSubFamily;
        #endregion

        #region Properties
        [DataMember]
        public UInt32 IdSCMFamilyImage
        {
            get { return idSCMFamilyImage; }
            set { idSCMFamilyImage = value; OnPropertyChanged("IdSCMFamilyImage"); }
        }

        [DataMember]
        public UInt16 IdFamily
        {
            get { return idFamily; }
            set { idFamily = value; OnPropertyChanged("IdFamily"); }
        }

        [DataMember]
        public string SavedFileName
        {
            get { return savedFileName; }
            set { savedFileName = value; OnPropertyChanged("SavedFileName"); }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }

        [DataMember]
        public string OriginalFileName
        {
            get { return originalFileName; }
            set { originalFileName = value; OnPropertyChanged("OriginalFileName"); }
        }

        
        [DataMember]
        public UInt64 ImagePosition
        {
            get { return imagePosition; }
            set { imagePosition = value; OnPropertyChanged("ImagePosition"); }
        }

        [DataMember]
        public int CreatedBy
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
        public int ModifiedBy
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


        [DataMember]
        [NotMapped]
        public UInt64 PositionParenttemp
        {
            get
            {
                return positionParenttemp;
            }

            set
            {
                positionParenttemp = value;
                OnPropertyChanged("positionParenttemp");
            }
        }

        [DataMember]
        [NotMapped]
        public UInt64 Positiontemp
        {
            get
            {
                return positiontemp;
            }

            set
            {
                positiontemp = value;
                OnPropertyChanged("Positiontemp");
            }
        }

        [DataMember]
        public byte[] ConnectorFamilyImageInBytesParent
        {
            get
            {
                return connectorFamilyImageInBytesParent;
            }

            set
            {
                connectorFamilyImageInBytesParent = value;
                OnPropertyChanged("ConnectorFamilyImageInBytesParent");
            }
        }

        [DataMember]
        public UInt64 IdSCMSubFamilyImage
        {
            get
            {
                return idSCMSubFamilyImage;
            }

            set
            {
                idSCMSubFamilyImage = value;
                OnPropertyChanged("IdSCMSubFamilyImage");
            }
        }

        [DataMember]
        public string SubSavedFileName
        {
            get
            {
                return subsavedFileName;
            }

            set
            {
                subsavedFileName = value;
                OnPropertyChanged("SubSavedFileName");
            }
        }



        [DataMember]
        public string SubOriginalFileName
        {
            get
            {
                return suboriginalFileName;
            }

            set
            {
                suboriginalFileName = value;
                OnPropertyChanged("SubOriginalFileName");
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
        public Int32 IdSubFamily
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
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public override object Clone()
        {
            return this.MemberwiseClone();
        }


    }
}
