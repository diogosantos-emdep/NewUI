using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
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
    public class OTAttachment : ModelBase, IDisposable
    {

        #region Fields

        Int64 idOT;
        string fileExtension;
        string fileName;
        string fileSize;
        Int64? fileSizeInInt;
        string fileType;
        string quotationCode;
        string quotationYear;
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
        private LookupValue structureDocumenttype;
        private bool isPdf;
        #endregion

        #region Constructor
        public OTAttachment()
        {
        }
        #endregion

        #region Properties


        [DataMember]
        public Int64 IdOT
        {
            get { return idOT; }
            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
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
        public string QuotationCode
        {
            get { return quotationCode; }
            set
            {
                quotationCode = value;
                OnPropertyChanged("QuotationCode");
            }
        }

        [DataMember]
        public string QuotationYear
        {
            get { return quotationYear; }
            set
            {
                quotationYear = value;
                OnPropertyChanged("QuotationYear");
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


        [DataMember]
        public LookupValue StructureDocumenttype
        {
            get { return structureDocumenttype; }
            set { structureDocumenttype = value; OnPropertyChanged("StructureDocumenttype"); }
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
