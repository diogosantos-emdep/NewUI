using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{
     [Table("document_types")]
     [DataContract]
     public class DocumentType: ModelBase, IDisposable
    {
         #region Fields
         String name;
         Byte idDocumentType;
         String extension;
        #endregion

         #region Constructor
         public DocumentType()
         {
             this.GeosModuleDocumentations = new List<GeosModuleDocumentation>();
         }
         #endregion

         #region Properties
         [Key]
         [Column("IdDocumentType")]
         [DataMember]
         public Byte IdDocumentType
         {
             get { return idDocumentType; }
             set { idDocumentType = value;
                OnPropertyChanged("IdDocumentType");
            }
         }

         [Column("Name")]
         [DataMember]
         public String Name
         {
             get { return name; }
             set { name = value;
                OnPropertyChanged("Name");
            }
        }

         [Column("Extension")]
         [DataMember]
         public String Extension
         {
             get { return extension; }
             set { extension = value;
                OnPropertyChanged("Extension");
            }
         }

         [DataMember]
         public virtual List<GeosModuleDocumentation> GeosModuleDocumentations { get; set; }

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
