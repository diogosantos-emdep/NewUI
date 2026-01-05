using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common.OTM
{
    [DataContract]
    public class Emailattachment : ModelBase, IDisposable
    {
        #region Fields
        private long idAttachment;
        private long idEmail;
        private string attachmentName;
        private string attachmentCnt;
        private string attachmentPath;
        private DateTime createdIn;
        private DateTime? modifiedIn;
        private int createdBy;
        private int? modifiedBy;
        private bool isDeleted;
        private string attachmentExtension;
        private byte[] fileContent;
        private string fileText;
        byte[] fileDocInBytes;
        private string excelFileText;
        private string locationFileText;
        private ImageSource attachmentImage;      
        private ObservableCollection<LookupValue> attachmentTypeList;//[pramod.misal][22.04.2025][GEOS2-7248]
        private Int32 selectedIndexAttachementType;
        private Int64 idAttachementType;
        private string type;

        private string xmlFileText;
        byte[] xmlDocInBytes;
        #endregion


        #region Properties
        [DataMember]
        public long IdAttachment
        {
            get { return idAttachment; }
            set
            {
                idAttachment = value;
                OnPropertyChanged("IdAttachment");
            }
        }

        [DataMember]
        public long IdEmail
        {
            get { return idEmail; }
            set
            {
                idEmail = value;
                OnPropertyChanged("IdEmail");
            }
        }

        [DataMember]
        public string AttachmentName
        {
            get { return attachmentName; }
            set
            {
                attachmentName = value;
                OnPropertyChanged("AttachmentName");
            }
        }

        [DataMember]
        public string AttachmentCnt
        {
            get
            {
                return attachmentCnt;
            }
            set
            {
                attachmentCnt = value;
                OnPropertyChanged("AttachmentCnt");
            }
        }

        [DataMember]
        public string AttachmentPath
        {
            get { return attachmentPath; }
            set
            {
                attachmentPath = value;
                OnPropertyChanged("AttachmentPath");
            }
        }

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

        [DataMember]
        public DateTime? ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [DataMember]
        public int CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [DataMember]
        public int? ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

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

        [DataMember]
        public string AttachmentExtension
        {
            get { return attachmentExtension; }
            set
            {
                attachmentExtension = value;
                OnPropertyChanged("AttachmentExtension");
            }
        }

        [DataMember]
        public byte[] FileContent
        {
            get { return fileContent; }
            set
            {
                fileContent = value;
                OnPropertyChanged("FileContent");
            }
        }

        [DataMember]
        public string FileText
        {
            get { return fileText; }
            set
            {
                fileText = value;
                OnPropertyChanged("FileText");
            }
        }

        [DataMember]
        public byte[] FileDocInBytes
        {
            get
            {
                return fileDocInBytes;
            }

            set
            {
                fileDocInBytes = value;
                OnPropertyChanged("FileDocInBytes");
            }
        }

        [DataMember]
        public string ExcelFileText
        {
            get { return excelFileText; }
            set
            {
                excelFileText = value;
                OnPropertyChanged("ExcelFileText");
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
        public string LocationFileText
        {
            get { return locationFileText; }
            set
            {
                locationFileText = value;
                OnPropertyChanged("LocationFileText");
            }
        }

        [DataMember]
        public ObservableCollection<LookupValue> AttachmentTypeList
        {
            get { return attachmentTypeList; }
            set
            {
                attachmentTypeList = value;
                OnPropertyChanged("AttachmentTypeList");

            }
        }

        [DataMember]
        public Int32 SelectedIndexAttachementType
        {
            get { return selectedIndexAttachementType; }
            set
            {
                selectedIndexAttachementType = value;
                OnPropertyChanged("SelectedIndexAttachementType");
            }
        }

        [DataMember]
        public Int64 IdAttachementType
        {
            get
            {
                return idAttachementType;
            }
            set
            {
                idAttachementType = value;
                OnPropertyChanged("IdAttachementType");
            }
        }

        string pOIdAttachment;
        [DataMember]
        public string POIdAttachment
        {
            get { return pOIdAttachment; }
            set
            {
                pOIdAttachment = value;
                OnPropertyChanged("pOIdAttachment");
            }
        }

        [DataMember]
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }
        #endregion

        //[rahul.gadhave][GEOS2-9020][23.07.2025] 
        [DataMember]
        public string XmlFileText
        {
            get { return xmlFileText; }
            set
            {
                xmlFileText = value;
                OnPropertyChanged("XmlFileText");
            }
        }

        [DataMember]
        public byte[] XmlFileDocInBytes
        {
            get
            {
                return xmlDocInBytes;
            }

            set
            {
                xmlDocInBytes = value;
                OnPropertyChanged("XmlFileDocInBytes");
            }
        }

        #region Constructor
        public Emailattachment()
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
