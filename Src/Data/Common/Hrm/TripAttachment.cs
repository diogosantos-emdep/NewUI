using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common
{

    [DataContract]
    public class TripAttachment : ModelBase, IDisposable
    {

        #region Fields

        Int64 idAttachment;
        string fileExtension;
        string fileName;
        string fileSize;
        Int64? fileSizeInInt;
        string fileType;
        ImageSource attachmentImage;
        string originalFileName;
        string savedFileName;
        DateTime uploadedIn;
        byte? isDeleted;
        string fileUploadName;
        Byte[] fileByte;
        bool isUploaded;
        string filePath;
        string guidCode;
        private bool isSelected;
        private LookupValue tripAttachmentType;
        private bool isPdf;
        private string description;
        private Int32 idEmployeeTrip;
        private Int32 idAttachmentType;
        public double? amount;// [pallavi.kale][28-05-2025][GEOS2-7941]
        #endregion

        #region Constructor
        public TripAttachment()
        {
        }
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

        [DataMember]
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

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
        public string GuidCode
        {
            get { return guidCode; }
            set
            {
                guidCode = value;
                OnPropertyChanged("GuidCode");
            }
        }
        [DataMember]
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged("IsSelected"); }
        }

        [NotMapped]
        [DataMember]
        public LookupValue TripAttachmentType
        {
            get { return tripAttachmentType; }
            set { tripAttachmentType = value; OnPropertyChanged("TripAttachmentType"); }
        }
        [NotMapped]
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
        public bool IsPdf
        {
            get
            {
                if (OriginalFileName != null)
                {
                    string lastFourChars;

                    if (OriginalFileName.Length >= 4)
                    {
                        lastFourChars = OriginalFileName.Substring(OriginalFileName.Length - 4);
                    }
                    else
                    {
                        lastFourChars = OriginalFileName;
                    }
                    return (lastFourChars.ToUpper() == ".PDF");

                }
                else
                {
                    return false;
                };
            }set
            {
                isPdf = value;
            }
        }
        [NotMapped]
        [DataMember]
        public Int32 IdEmployeeTrip
        {
            get { return idEmployeeTrip; }
            set
            {
                idEmployeeTrip = value;
                OnPropertyChanged("IdEmployeeTrip");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdAttachmentType
        {
            get { return idAttachmentType; }
            set
            {
                idAttachmentType = value;
                OnPropertyChanged("IdAttachmentType");
            }
        }
        // [pallavi.kale][28-05-2025][GEOS2-7941]
        [NotMapped]
        [DataMember]
        public double? Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged("Amount");
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
		// [nsatpute][26-09-2024][GEOS2-6486]
        public TripAttachment DeepClone()
        {
            var clone = (TripAttachment)this.MemberwiseClone();

            if (this.FileByte != null)
            {
                clone.FileByte = (byte[])this.FileByte.Clone();
            }

            if (this.TripAttachmentType != null)
            {
                clone.TripAttachmentType = (LookupValue)this.TripAttachmentType.Clone(); 
            }

            return clone;
        }

        #endregion
    }
}
