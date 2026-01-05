using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common
{
    [Table("activity_attachments")]
    [DataContract]
    public class ActivityAttachment : ModelBase, IDisposable
    {
        #region Fields
        Int64 idActivityAttachment;
        string originalFileName;
        string savedFileName;
        DateTime uploadedIn;
        string fileType;
        Int64? fileSize;
        byte? isDeleted;
        Int64 idActivity;
        string fileUploadName;
        Byte[] fileByte;
        bool isUploaded;
        string filePath;
        ImageSource attachmentImage;
        #endregion

        #region Constructor
        public ActivityAttachment()
        {

        }
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public Int64 IdActivityAttachment
        {
            get
            {
                return idActivityAttachment;
            }

            set
            {
                idActivityAttachment = value;
                OnPropertyChanged("IdActivityAttachment");
            }
        }

        [NotMapped]
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

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public DateTime UploadedIn
        {
            get
            {
                return uploadedIn;
            }

            set
            {
                uploadedIn = value;
                OnPropertyChanged("UploadedIn");
            }
        }


        [NotMapped]
        [DataMember]
        public string FileType
        {
            get
            {
                return fileType;
            }

            set
            {
                fileType = value;
                OnPropertyChanged("FileType");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64? FileSize
        {
            get
            {
                return fileSize;
            }

            set
            {
                fileSize = value;
                OnPropertyChanged("FileSize");
            }
        }


        [NotMapped]
        [DataMember]
        public byte? IsDeleted
        {
            get
            {
                return isDeleted;
            }

            set
            {
               isDeleted = value;
                OnPropertyChanged("IsDeleted");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 IdActivity
        {
            get
            {
                return idActivity;
            }

            set
            {
                idActivity = value;
                OnPropertyChanged("IdActivity");
            }
        }

        [NotMapped]
        [DataMember]
        public string FileUploadName
        {
            get
            {
                return fileUploadName;
            }

            set
            {
                fileUploadName = value;
                OnPropertyChanged("FileUploadName");
            }
        }

        [NotMapped]
        [DataMember]
        public Byte[] FileByte
        {
            get
            {
                return fileByte;
            }

            set
            {
                fileByte = value;
                OnPropertyChanged("FileByte");
            }
        }


        [NotMapped]
        [DataMember]
        public bool IsUploaded
        {
            get
            {
                return isUploaded;
            }

            set
            {
                isUploaded = value;
                OnPropertyChanged("IsUploaded");
            }
        }

        [NotMapped]
        [DataMember]
        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        [NotMapped]
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
