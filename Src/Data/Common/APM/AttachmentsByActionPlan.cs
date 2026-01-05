using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.APM
{//[Sudhir.Jangra][GEos2-6019]
    [DataContract]
    public class AttachmentsByActionPlan : ModelBase, IDisposable
    {
        #region Fields
        private Int64 idActionPlanAttachment;
        private string originalFileName;
        private string savedFileName;
        private string filePath;
        private Int64 idActionPlan;
        private Int32 createdBy;
        private DateTime createdIn;
        private string description;
        private string fileType;
        private string fileSize;
        private byte? isDeleted;
        private string fileUploadName;
        private Byte[] fileByte;
        private ImageSource attachmentImage;
        private bool isUploaded;
        private string createdByName;
        private Int64? fileSizeInInt;
        private string fileExtension;
        private List<LogEntriesByActionPlan> changeLogList;//[Sudhir.Jangra][GEOS2-6019]
        private string previousFileName;
        private bool isDeleteButtonEnabled;
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public Int64 IdActionPlanAttachment
        {
            get { return idActionPlanAttachment; }
            set
            {
                idActionPlanAttachment = value;
                OnPropertyChanged("IdActionPlanAttachment");
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
        public Int64 IdActionPlan
        {
            get { return idActionPlan; }
            set
            {
                idActionPlan = value;
                OnPropertyChanged("IdActionPlan");
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
        public string FileSize
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

        [NotMapped]
        [DataMember]
        public Int64? FileSizeInInt
        {
            get { return fileSizeInInt; }
            set
            {
                fileSizeInInt = value;
                OnPropertyChanged("FileSizeInInt");
            }
        }

        [NotMapped]
        [DataMember]
        public string FileExtension
        {
            get { return fileExtension; }
            set
            {
                fileExtension = value;
                OnPropertyChanged("FileExtension");
            }
        }

        //[Sudhir.Jangra][GEOS2-6019]
        [NotMapped]
        [DataMember]
        public List<LogEntriesByActionPlan> ChangeLogList
        {
            get { return changeLogList; }
            set
            {
                changeLogList = value;
                OnPropertyChanged("ChangeLogList");
            }
        }


        //[Sudhir.Jangra][GEOS2-6019]
        [NotMapped]
        [DataMember]
        public string PreviousFileName
        {
            get { return previousFileName; }
            set
            {
                previousFileName = value;
                OnPropertyChanged("PreviousFileName");
            }
        }

        //[Sudhir.Jangra][GEOS2-6019]
        [NotMapped]
        [DataMember]
        public bool IsDeleteButtonEnabled
        {
            get { return isDeleteButtonEnabled; }
            set
            {
                isDeleteButtonEnabled = value;
                OnPropertyChanged("IsDeleteButtonEnabled");
            }
        }
        #endregion

        #region Constructor
        public AttachmentsByActionPlan()
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
