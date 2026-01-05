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
     public class DocumentType
     {
         Byte sortOrder;
         String name;
         Byte idDocumentType;
         String htmlColor;
         String extension;

         public DocumentType()
        {
            this.GeosModuleDocumentations = new List<GeosModuleDocumentation>();
        }
         [Key]
         [Column("IdDocumentType")]
         public Byte IdDocumentType
         {
             get;
             set;
         }

         [Column("sortOrder")]
         public Byte SortOrder
         {
             get;
             set;
         }

         [Column("name")]
         public String Name
         {
             get;
             set;
         }

         [Column("htmlColor")]
         public String HtmlColor
         {
             get;
             set;
         }

         [Column("extension")]
         public String Extension
         {
             get;
             set;
         }

         public virtual List<GeosModuleDocumentation> GeosModuleDocumentations { get; set; }

     }
}
