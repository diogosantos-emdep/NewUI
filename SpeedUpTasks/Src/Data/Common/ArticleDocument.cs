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
    public class ArticleDocument : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idArticleDoc;
        Int32 idArticle;
        string originalFileName;
        string savedFileName;
        DateTime createdIn;
        DateTime modifiedIn;
        Int32 createdBy;
        string description;
        Int32 modifiedBy;
        Int64 idDocType;

        People documentCreatedBy;
        People documentModifiedBy;

        ArticleDocumentType articleDocumentType;
        #endregion

        #region Properties

        [Column("IdArticleDoc")]
        [DataMember]
        public Int64 IdArticleDoc
        {
            get { return idArticleDoc; }
            set
            {
                idArticleDoc = value;
                OnPropertyChanged("IdArticleDoc");
            }
        }

        [Column("IdArticle")]
        [DataMember]
        public Int32 IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
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


        [Column("idDocType")]
        [DataMember]
        public Int64 IdDocType
        {
            get { return idDocType; }
            set
            {
                idDocType = value;
                OnPropertyChanged("IdDocType");
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

        #endregion

        #region Constructor

        public ArticleDocument()
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
