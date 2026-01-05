using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Data;
using System.Windows.Media;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common
{
    [Table("article_comments")]
    [DataContract]
    public class ArticleComment : ModelBase, IDisposable
    {
        #region Declaration
        string comment;
        DateTime creationDate;
        Int32 idArticle;
        Int64 idArticleComment;
        Int32 idCreator;
        Int32? idModifier;
        byte idStage;
        Int64 idWarehouse;
        DateTime? modificationDate;
        Stage stage;
        DateTime? dateOfExpiry;
        Offer workOrder;
        #endregion

        #region Properties

        [Column("Comment")]
        [DataMember]
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged("Comment");
            }
        }

        [Column("CreationDate")]
        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
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

     
        [Key]
        [Column("IdArticleComment")]
        [DataMember]
        public Int64 IdArticleComment
        {
            get { return idArticleComment; }
            set
            {
                idArticleComment = value;
                OnPropertyChanged("IdArticleComment");
            }
        }

        [Column("IdCreator")]
        [DataMember]
        public Int32 IdCreator
        {
            get { return idCreator; }
            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

        [Column("IdModifier")]
        [DataMember]
        public Int32? IdModifier
        {
            get { return idModifier; }
            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }
        }

        [Column("IdStage")]
        [DataMember]
        public byte IdStage
        {
            get { return idStage; }
            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }

        [Column("IdWarehouse")]
        [DataMember]
        public Int64 IdWarehouse
        {
            get { return idWarehouse; }
            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }

        [Column("ModificationDate")]
        [DataMember]
        public DateTime? ModificationDate
        {
            get { return modificationDate; }
            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [NotMapped]
        [DataMember]
        public Stage Stage
        {
            get { return stage; }
            set
            {
                stage = value;
                OnPropertyChanged("Stage");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? DateOfExpiry
        {
            get { return dateOfExpiry; }
            set
            {
                dateOfExpiry = value;
                OnPropertyChanged("DateOfExpiry");
            }
        }

        [DataMember]
        public Offer WorkOrder
        {
            get { return workOrder; }
            set
            {
                workOrder = value;
                OnPropertyChanged("WorkOrder");
            }
        }

        
        #endregion

        #region Constructor

        public ArticleComment()
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
            ArticleComment articleComment = (ArticleComment)this.MemberwiseClone();
         
            return articleComment;
        }

        #endregion
    }
}
