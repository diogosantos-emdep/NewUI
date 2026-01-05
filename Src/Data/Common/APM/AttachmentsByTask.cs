using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.APM
{//[Sudhir.Jangra][GEOS2-6016]
    [DataContract]
    public class AttachmentsByTask : ModelBase, IDisposable
    {
        #region Fields
        private Int64 idActionPlanTaskAttachment;
        private string originalFileName;
        private string savedFileName;
        private string filePath;
        private Int64 idTask;
        private Int32 createdBy;
        private DateTime createdIn;
        private string description;
        private string fileType;
        private Int64? fileSize;
        private byte? isDeleted;
        private string fileUploadName;
        private Byte[] fileByte;
        private ImageSource attachmentImage;
        private bool isUploaded;
        private string createdByName;
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public Int64 IdActionPlanTaskAttachment
        {
            get { return idActionPlanTaskAttachment; }
            set
            {
                idActionPlanTaskAttachment = value;
                OnPropertyChanged("IdActionPlanTaskAttachment");
            }
        }

        [NotMapped]
        [DataMember]
        public string OriginalFileName
        {
            get { return originalFileName; }
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
            get { return savedFileName; }
            set
            {
                savedFileName = value;
                OnPropertyChanged("SavedFileName");
            }
        }

        [NotMapped]
        [DataMember]
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 IdTask
        {
            get { return idTask; }
            set
            {
                idTask = value;
                OnPropertyChanged("IdTask");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 CreatedBy
        {
            get { return createdBy; }
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
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [NotMapped]
        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [NotMapped]
        [DataMember]
        public string FileType
        {
            get { return fileType; }
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
            get { return fileSize; }
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
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged("IsDeleted");
            }
        }

        [NotMapped]
        [DataMember]
        public string FileUploadName
        {
            get { return fileUploadName; }
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
            get { return fileByte; }
            set
            {
                fileByte = value;
                OnPropertyChanged("FileByte");
            }
        }

        [NotMapped]
        [DataMember]
        public ImageSource AttachmentImage
        {
            get { return attachmentImage; }
            set
            {
                attachmentImage = value;
                OnPropertyChanged("AttachmentImage");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsUploaded
        {
            get { return isUploaded; }
            set
            {
                isUploaded = value;
                OnPropertyChanged("IsUploaded");
            }
        }

        [NotMapped]
        [DataMember]
        public string CreatedByName
        {
            get { return createdByName; }
            set
            {
                createdByName = value;
                OnPropertyChanged("CreatedByName");
            }
        }
        #endregion


        #region Constructor
        public AttachmentsByTask()
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
