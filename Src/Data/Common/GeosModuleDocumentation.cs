using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Emdep.Geos.Data.Common.HarnessPart;

namespace Emdep.Geos.Data.Common
{

     [Table("geos_module_documentations")]
     [DataContract]
    public class GeosModuleDocumentation
    {
        #region Fields
        Int32 idGeosModuleDocumentation;
            string title;
            Int32? idGeosWorkbenchVersion;
            Byte? idGeosModule;
            Byte? idDocumentType;
            string filePath;
            String fileName;
        #endregion

        #region Properties
            [Key]
            [Column("IdGeosModuleDocumentation")]
            [DataMember]
            public Int32 IdGeosModuleDocumentation
            {
                get { return idGeosModuleDocumentation; }
                set { idGeosModuleDocumentation = value; }
            }

            [Column("Title")]
            [DataMember]
            public String Title
            {
                get { return title; }
                set { title = value; }
            }

            [DataMember]
            public Int32? IdGeosWorkbenchVersion
            {
                get { return idGeosWorkbenchVersion; }
                set { idGeosWorkbenchVersion = value; }
            }

            [DataMember]
            public Byte? IdGeosModule
            {
                get { return idGeosModule; }
                set { idGeosModule = value; }
            }

             [DataMember]
            public Byte? IdDocumentType
            {
                get { return idDocumentType; }
                set { idDocumentType = value; }
            }

            [Column("FilePath")]
            [DataMember]
            public String FilePath
            {
                get { return filePath; }
                set { filePath = value; }
            }

            [Column("FileName")]
            [DataMember]
            public String FileName
            {
                get { return fileName; }
                set { fileName = value; }
            }

            [ForeignKey("IdGeosWorkbenchVersion")]
            [DataMember]
            public virtual GeosWorkbenchVersion GeosWorkbenchVersions { get; set; }

            [ForeignKey("IdDocumentType")]
            [DataMember]
            public virtual DocumentType DocumentTypes { get; set; }

            [ForeignKey("IdGeosModule")]
            [DataMember]
            public virtual GeosModule GeosModule { get; set; }

            #endregion

     }
}
