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
        string fileType;
        string quotationCode;
        string quotationYear;
        ImageSource attachmentImage;
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
        public string FileType
        {
            get { return fileType; }
            set
            {
                fileType = value;
                OnPropertyChanged("FileType");
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
        public ImageSource AttachmentImage
        {
            get { return attachmentImage; }
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
