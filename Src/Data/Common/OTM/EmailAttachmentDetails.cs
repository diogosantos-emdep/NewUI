using Emdep.Geos.Data.Common.TechnicalRestService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.OTM
{
    //[pooja.jadhav][20.02.2025][GEOS2-6724]
    [DataContract]
    public class EmailAttachmentDetails : ModelBase, IDisposable
    {
        #region feilds 
        private string attachments;
        private List<Emailattachment> attachmentList;
        private string attachmentCnt;
        private string attachmentName;
        private ImageSource attachmentImage;

        #endregion

        #region Properties
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

        public string AttachmentName
        {
            get { return attachmentName; }
            set
            {
                attachmentName = value;
                OnPropertyChanged("AttachmentName");
            }
        }
        public string Attachments
        {
            get
            {
                return attachments;
            }
            set
            {
                attachments = value;
                OnPropertyChanged("Attachments");
            }
        }


        public List<Emailattachment> AttachmentList
        {
            get { return attachmentList; }
            set
            {
                attachmentList = value;
                OnPropertyChanged("AttachmentList");
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
        #endregion

        #region Constructor
        public EmailAttachmentDetails()
        {

        }
        #endregion

        #region Methods 
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

