using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class EmployeeExpensePhotoInfo : ModelBase, IDisposable
    {
        #region Fields
        byte[] imageInByte;
        byte[] pdfInByte;
        ImageSource expenseImage;
        Int32 idEmployeeExpenseAttachment;
        Int32 idEmployeeExpense;
        string originalFileName;
        string savedFileName;
        string fileType;
        Int64 fileSize;
        int idAttachmentType;
        string attachmentType;
        #endregion

        #region Constructor
        public EmployeeExpensePhotoInfo()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public Int32 IdEmployeeExpense
        {
            get
            {
                return idEmployeeExpense;
            }

            set
            {
                idEmployeeExpense = value;
                OnPropertyChanged("IdEmployeeExpense");
            }
        }
        [DataMember]
        public Int32 IdEmployeeExpenseAttachment
        {
            get
            {
                return idEmployeeExpenseAttachment;
            }

            set
            {
                idEmployeeExpenseAttachment = value;
                OnPropertyChanged("IdEmployeeExpenseAttachment");
            }
        }
        [DataMember]
        public byte[] ImageInByte
        {
            get { return imageInByte; }
            set
            {
                imageInByte = value;
                OnPropertyChanged("ImageInByte");
            }
        }
        [DataMember]
        public byte[] PdfInByte
        {
            get { return pdfInByte; }
            set
            {
                pdfInByte = value;
                OnPropertyChanged("PdfInByte");
            }
        }
        [DataMember]
        public ImageSource ExpenseImage
        {
            get { return expenseImage; }
            set
            {
                expenseImage = value;
                OnPropertyChanged("ExpenseImage");
            }
        }
        [DataMember]
        public int IdAttachmentType
        {
            get
            {
                return idAttachmentType;
            }

            set
            {
                idAttachmentType = value;
                OnPropertyChanged("IdAttachmentType");
            }
        }
        [DataMember]
        public string AttachmentType
        {
            get
            {
                return attachmentType;
            }

            set
            {
                attachmentType = value;
                OnPropertyChanged("AttachmentType");
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
        public Int64 FileSize
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

        Int64 newHeight;
        [DataMember]
        public Int64 NewHeight
        {
            get
            {
                return newHeight;
            }

            set
            {
                newHeight = value;
                OnPropertyChanged("NewHeight");
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