using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class PCMArticleImage : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idPCMArticleImage;
        UInt32 idArticle;
        string savedFileName;
        string description;
        string originalFileName;
        UInt32 createdBy;
        DateTime createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;
        UInt64 position;

        byte[] pCMArticleImageInBytes;
        DateTime? updatedDate;
        ImageSource attachmentImage;
        byte isWarehouseImage;
        byte isImageShareWithCustomer;
        string imagePath;//[Sudhir.Jangra][GEOS2-2922][09/03/2023]
        bool isImageButtonEnabled;//[Sudhir.Jangra][GEOS2-2922][09/03/2023]
        Visibility isImageVisible;//[Sudhir.Jangra][GEOS2-2922][09/03/2023]

        #endregion

        #region Constructor

        public PCMArticleImage()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public UInt32 IdPCMArticleImage
        {
            get
            {
                return idPCMArticleImage;
            }

            set
            {
                idPCMArticleImage = value;
                OnPropertyChanged("IdPCMArticleImage");
            }
        }

        [DataMember]
        public UInt32 IdArticle
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
        public byte[] PCMArticleImageInBytes
        {
            get
            {
                return pCMArticleImageInBytes;
            }

            set
            {
                pCMArticleImageInBytes = value;
                OnPropertyChanged("PCMArticleImageInBytes");
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
        public byte IsWarehouseImage
        {
            get
            {
                return isWarehouseImage;
            }

            set
            {
                isWarehouseImage = value;
                OnPropertyChanged("AttachmentImage");
            }
        }

        [DataMember]
        public byte IsImageShareWithCustomer
        {
            get
            {
                return isImageShareWithCustomer;
            }

            set
            {
                isImageShareWithCustomer = value;
                OnPropertyChanged("IsImageShareWithCustomer");
            }
        }
        [DataMember]
        public string ImagePath//[Sudhir.Jangra][GEOS2-2922][09/03/2023]
        {
            get
            {
                return imagePath;
            }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }
        [DataMember]
        public bool IsImageButtonEnabled
        {
            get { return isImageButtonEnabled; }
            set
            {
                isImageButtonEnabled = value;
                OnPropertyChanged("IsImageButtonEnabled");
            }
        }
        [DataMember]
        public Visibility IsImageVisible
        {
            get { return isImageVisible; }
            set
            {
                isImageVisible = value;
                OnPropertyChanged("IsImageVisible");
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
