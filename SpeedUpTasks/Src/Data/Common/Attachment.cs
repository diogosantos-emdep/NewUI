using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    //[DataContract(IsReference = true)]
    public class Attachment : ModelBase // INotifyPropertyChanged, ICloneable
    {
        #region Fields

        Int64 idAttachment;
        string originalFileName;
        bool isNew;
        bool isDeleted;
        Byte[] fileByte;
        bool isUploaded;
        string filePath;

        //string savedFileName;
        //DateTime uploadedIn;
        //string fileType;
        //Int64 fileSize;
        //string fileUploadName;

        #endregion

        #region Properties

        [NotMapped]
        [DataMember]
        public Int64 IdAttachment
        {
            get { return idAttachment; }
            set
            {
                idAttachment = value;
                OnPropertyChanged("IdAttachment");
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

        //[NotMapped]
        //[DataMember]
        //public string SavedFileName
        //{
        //    get { return savedFileName; }
        //    set
        //    {
        //        savedFileName = value;
        //        OnPropertyChanged("SavedFileName");
        //    }
        //}

        //[NotMapped]
        //[DataMember]
        //public DateTime UploadedIn
        //{
        //    get { return uploadedIn; }
        //    set
        //    {
        //        uploadedIn = value;
        //        OnPropertyChanged("UploadedIn");
        //    }
        //}

        //[NotMapped]
        //[DataMember]
        //public string FileType
        //{
        //    get { return fileType; }
        //    set
        //    {
        //        fileType = value;
        //        OnPropertyChanged("FileType");
        //    }
        //}

        //[NotMapped]
        //[DataMember]
        //public Int64 FileSize
        //{
        //    get { return fileSize; }
        //    set
        //    {
        //        fileSize = value;
        //        OnPropertyChanged("FileSize");
        //    }
        //}

        [NotMapped]
        [DataMember]
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged("IsNew");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged("IsDeleted");
            }
        }

        //[NotMapped]
        //[DataMember]
        //public string FileUploadName
        //{
        //    get { return fileUploadName; }
        //    set
        //    {
        //        fileUploadName = value;
        //        OnPropertyChanged("FileUploadName");
        //    }
        //}

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
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        #endregion

        #region Methods

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
