using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{
    //[pramod.misal][GEOS2-5387][05-04-2024]
    [DataContract]
    public class ConnectorAttachements : ModelBase, IDisposable
    {
        #region Fields
        Int32 idconnector;
        string reference;
        Int32 idconnectordoc;
        string originalFileName;
        string savedFileName;
        DateTime? createdDate;
        DateTime? modifiedDate;
        UInt32 createdBy;
        string description;
        UInt32 modifiedBy;
        Int32 idCustomer;
        UInt32 idDocType;
        DocumentType documentType;
        byte[] connectorAttachementsDocInBytes;
        DateTime? updatedDate;
        ConnectorAttachements attachmentType;
        string customerName;
        bool isDelVisible;


        #endregion

        #region Constructor

        public ConnectorAttachements()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public Int32 Idconnector
        {
            get
            {
                return idconnector;
            }

            set
            {
                idconnector = value;
                OnPropertyChanged("Idconnector");
            }
        }

        [DataMember]
        public Int32 IdCustomer
        {
            get
            {
                return idCustomer;
            }

            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }


        [DataMember]
        public string Reference
        {
            get
            {
                return reference;
            }

            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }

        [DataMember]
        public Int32 Idconnectordoc
        {
            get
            {
                return idconnectordoc;
            }

            set
            {
                idconnectordoc = value;
                OnPropertyChanged("Idconnectordoc");
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
        public DateTime? CreatedDate
        {
            get
            {
                return createdDate;
            }

            set
            {
                createdDate = value;
                OnPropertyChanged("CreatedDate");
            }
        }



        [DataMember]
        public DateTime? ModifiedDate
        {
            get
            {
                return modifiedDate;
            }

            set
            {
                modifiedDate = value;
                OnPropertyChanged("ModifiedDate");
            }
        }

        [DataMember]
        public UInt32 CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

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
        public UInt32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }

            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }


        [DataMember]
        public UInt32 IdDocType
        {
            get
            {
                return idDocType;
            }

            set
            {
                idDocType = value;
                OnPropertyChanged("IdDocType");
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
        public DocumentType DocumentType
        {
            get
            {
                return documentType;
            }

            set
            {
                documentType = value;
                OnPropertyChanged("DocumentType");
            }
        }

        [DataMember]
        public DateTime? UpdatedDate
        {
            get
            {
                return updatedDate;
            }

            set
            {
                updatedDate = value;
                OnPropertyChanged("UpdatedDate");
            }
        }

        [DataMember]
        public ConnectorAttachements AttachmentType
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
        public string CustomerName
        {
            get
            {
                return customerName;
            }

            set
            {
                customerName = value;
                OnPropertyChanged("CustomerName");
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

        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
