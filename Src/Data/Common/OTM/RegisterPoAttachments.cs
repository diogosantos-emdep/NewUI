using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.OTM
{
    public class RegisterPoAttachments : ModelBase, IDisposable
    {
        #region Fields
       
        string savedFileName;
        byte[] connectorAttachementsDocInBytes;
        bool isDelVisible;
        private string fileExtension;
        private ImageSource attachmentImage;
        #endregion

        #region Constructor
        public RegisterPoAttachments()
        {

        }
        #endregion

        #region Properties

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
        public byte[] ConnectorAttachementsDocInBytes
        {
            get
            {
                return connectorAttachementsDocInBytes;
            }

            set
            {
                connectorAttachementsDocInBytes = value;
                OnPropertyChanged("ConnectorAttachementsDocInBytes");
            }
        }
      
        [DataMember]
        public bool IsDelVisible
        {
            get { return isDelVisible; }
            set
            {
                if (isDelVisible != value)
                {
                    isDelVisible = value;
                    OnPropertyChanged(nameof(IsDelVisible));
                }
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

        #endregion
    }
}
