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
    public class BPLDocument : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idBasepricedoc;
        Int32 idBasePriceList;
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

        [Column("IdBasepricedoc")]
        [DataMember]
        public Int64 IdBasepricedoc
        {
            get { return idBasepricedoc; }
            set
            {
                idBasepricedoc = value;
                OnPropertyChanged("IdBasepricedoc");
            }
        }

        [Column("IdBasePriceList")]
        [DataMember]
        public Int32 IdBasePriceList
        {
            get { return idBasePriceList; }
            set
            {
                idBasePriceList = value;
                OnPropertyChanged("IdBasePriceList");
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

        public BPLDocument()
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
