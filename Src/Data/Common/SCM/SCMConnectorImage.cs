using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class SCMConnectorImage : ModelBase, IDisposable
    {
        #region Fields
        private Int64 idConnector;
        private Int64 idConnectorImage;
        private string refe;

        string savedFileName;
        string description;
        string originalFileName;
        UInt32 createdBy;
        DateTime createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;
        UInt64 position;

        byte[] connectorsImageInBytes;
        DateTime? updatedDate;
        ImageSource attachmentImage;
        byte isWarehouseImage;
        byte isImageShareWithCustomer;
        string imagePath;
        bool isImageButtonEnabled;
        Visibility isImageVisible;
        Visibility isRatingVisible=  Visibility.Collapsed;
        bool isBreak =false;
        int idPictureType;
        string creator;
        int idSite;
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
       
        public bool isDelVisible;
        public bool isEditVisible;
        

        #endregion

        #region Constructor

        public SCMConnectorImage()
        {

        }

        #endregion

        #region Properties
        [DataMember]
        public bool IsBreak
        {
            get { return isBreak; }
            set
            {
                isBreak = value;
                OnPropertyChanged("IsBreak");
            }
        }
        [DataMember]
        public Visibility IsRatingVisible
        {
            get { return isRatingVisible; }
            set
            {
                isRatingVisible = value;
                OnPropertyChanged("IsRatingVisible");
            }
        }
        [DataMember]
        public Int64 IdConnector
        {
            get { return idConnector; }
            set { idConnector = value; OnPropertyChanged("IdConnector"); }
        }

        [DataMember]
        public Int64 IdConnectorImage
        {
            get
            {
                return idConnectorImage;
            }
            set
            {
                idConnectorImage = value;
                OnPropertyChanged("IdConnectorImage");
            }
        }

        [DataMember]
        public string Ref
        {
            get { return refe; }
            set { refe = value; OnPropertyChanged("Ref"); }
        }

        private string oldFileName;
        [DataMember]
        public string OldFileName
        {
            get { return oldFileName; }
            set { oldFileName = value; OnPropertyChanged("OldFileName"); }
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
        public byte[] ConnectorsImageInBytes
        {
            get
            {
                return connectorsImageInBytes;
            }

            set
            {
                connectorsImageInBytes = value;
                OnPropertyChanged("ConnectorsImageInBytes");
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
        public string ImagePath
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

        [DataMember]
        public int IdPictureType
        {
            get
            {
                return idPictureType;
            }
            set
            {
                idPictureType = value;
                if (idPictureType == 0)
                    PictureType = "Sample Image";
                else if (idPictureType == 1)
                    PictureType = "WTG Image";
                OnPropertyChanged("IdPictureType");
            }
        }
        string pictureType;
        [DataMember]
        public string PictureType
        {
            get
            {
                return pictureType;
            }
            set
            {
                pictureType = value;
                OnPropertyChanged("PictureType");
            }
        }
        bool isPictureMaximize;
        [DataMember]
        public bool IsPictureMaximize
        {
            get
            {
                return isPictureMaximize;
            }
            set
            {
                isPictureMaximize = value;
                OnPropertyChanged("IsPictureMaximize");
            }
        }

        [DataMember]
        public int IdSite
        {
            get
            {
                return idSite;
            }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [DataMember]
        public string Creator
        {
            get
            {
                return creator;
            }
            set
            {
                creator = value;
                OnPropertyChanged("Creator");
            }
        }

        [DataMember]
        public bool IsDelVisible
        {
            get { return isDelVisible; }
            set { isDelVisible = value; OnPropertyChanged("IsDelVisible"); }
        }
        [DataMember]
        public bool IsEditVisible
        {
            get { return isEditVisible; }
            set { isEditVisible = value; OnPropertyChanged("IsEditVisible"); }
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
