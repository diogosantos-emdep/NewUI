using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [Table("documenttypes")]
    [DataContract]
    public class ArticleDocumentType : ModelBase, IDisposable
    {
        #region Fields
        Int64 idDocType;
        string documentType;
        string documentType_es;
        string documentType_fr;
        #endregion

        #region Constructor
        public ArticleDocumentType()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("idDocType")]
        [DataMember]
        public Int64 IdDocType
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

        [Column("DocumentType")]
        [DataMember]
        public string DocumentType
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

        [Column("DocumentType_es")]
        [DataMember]
        public string DocumentType_es
        {
            get
            {
                return documentType_es;
            }

            set
            {
                documentType_es = value;
                OnPropertyChanged("DocumentType_es");
            }
        }

        [Column("DocumentType_fr")]
        [DataMember]
        public string DocumentType_fr
        {
            get
            {
                return documentType_fr;
            }

            set
            {
                documentType_fr = value;
                OnPropertyChanged("DocumentType_fr");
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
