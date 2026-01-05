using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{

    [DataContract]
    public class CPLDocument : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idCustomerPriceListDoc;
        Int32 idCustomerPriceList;
        string originalFileName;
        string savedFileName;
        DateTime createdIn;
        DateTime modifiedIn;
        Int32 createdBy;
        string description;
        Int32 modifiedBy;
        Int64 idAttachmentType;

        People documentCreatedBy;
        People documentModifiedBy;

        ArticleDocumentType articleDocumentType;

        byte isShareWithCustomer;

        byte[] pLMArticleFileInBytes;
        #endregion

        #region Properties
        [Column("IdCustomerPriceListDoc")]
        [DataMember]
        public Int64 IdCustomerPriceListDoc
        {
            get { return idCustomerPriceListDoc; }
            set
            {
                idCustomerPriceListDoc = value;
                OnPropertyChanged("IdCustomerPriceListDoc");
            }
        }

        [Column("IdCustomerPriceList")]
        [DataMember]
        public Int32 IdCustomerPriceList
        {
            get { return idCustomerPriceList; }
            set
            {
                idCustomerPriceList = value;
                OnPropertyChanged("IdCustomerPriceList");
            }
        }

        
        [Column("OriginalFileName")]
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

        [Column("SavedFileName")]
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

        [Column("CreatedIn")]
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

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [Column("CreatedBy")]
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

        [Column("ModifiedBy")]
        [DataMember]
        public Int32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [Column("Description")]
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


        [Column("IdAttachmentType")]
        [DataMember]
        public Int64 IdAttachmentType
        {
            get { return idAttachmentType; }
            set
            {
                idAttachmentType = value;
                OnPropertyChanged("IdAttachmentType");
            }
        }

        [NotMapped]
        [DataMember]
        public People DocumentCreatedBy
        {
            get { return documentCreatedBy; }
            set
            {
                documentCreatedBy = value;
                OnPropertyChanged("DocumentCreatedBy");
            }
        }

        [NotMapped]
        [DataMember]
        public People DocumentModifiedBy
        {
            get { return documentModifiedBy; }
            set
            {
                documentModifiedBy = value;
                OnPropertyChanged("DocumentModifiedBy");
            }
        }

        [NotMapped]
        [DataMember]
        public ArticleDocumentType ArticleDocumentType
        {
            get { return articleDocumentType; }
            set
            {
                articleDocumentType = value;
                OnPropertyChanged("ArticleDocumentType");
            }
        }

        [NotMapped]
        [DataMember]
        public byte IsShareWithCustomer
        {
            get
            {
                return isShareWithCustomer;
            }

            set
            {
                isShareWithCustomer = value;
                OnPropertyChanged("IsShareWithCustomer");
            }
        }

        [NotMapped]
        [DataMember]
        public byte[] PLMArticleFileInBytes
        {
            get
            {
                return pLMArticleFileInBytes;
            }

            set
            {
                pLMArticleFileInBytes = value;
                OnPropertyChanged("PLMArticleFileInBytes");
            }
        }
        #endregion

        #region Constructor

        public CPLDocument()
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
