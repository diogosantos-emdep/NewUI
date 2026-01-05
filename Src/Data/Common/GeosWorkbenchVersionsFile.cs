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
     [Table("geos_workbench_versions_files")]
     [DataContract]
    public class GeosWorkbenchVersionsFile
    {
        #region Fields
        Int64 idGeosModuleVersionFile;
         Int32? idGeosWorkbenchVersion;
         String fileName;
         String filePath;
         String assemblyVersion;
         Int32? repsitoryVersion;
         Int32? fileSize;
         Byte? idGeosModule;
        #endregion

        #region Properties
         [Key]
            [Column("IdGeosModuleVersionFile")]
            [DataMember]
            public Int64 IdGeosModuleVersionFile
            {
                get { return idGeosModuleVersionFile; }
                 set { idGeosModuleVersionFile = value; }
            }

             [DataMember]
             public Int32? IdGeosWorkbenchVersion
            {
                 get { return idGeosWorkbenchVersion; }
                 set { idGeosWorkbenchVersion = value; }
            }

             [Column("FileName")]
            [DataMember]
             public String FileName
            {
                get { return fileName; }
                set { fileName = value; }
             }

            [Column("FilePath")]
             [DataMember]
             public String FilePath
             {
                 get { return filePath; }
                 set { filePath = value; }
            }

            [Column("AssemblyVersion")]
             [DataMember]
             public String AssemblyVersion
             {
                 get { return assemblyVersion; }
                 set { assemblyVersion = value; }
             }

             [Column("RepsitoryVersion")]
             [DataMember]
             public Int32? RepsitoryVersion
            {
                get { return repsitoryVersion; }
                set { repsitoryVersion = value; }
            }

            [Column("FileSize")]
            [DataMember]
            public Int32? FileSize
            {
                get { return fileSize; }
                set { fileSize = value; }
            }

            [DataMember]
            public Byte? IdGeosModule
            {
                get { return idGeosModule; }
                set { idGeosModule = value; }
            }

           [ForeignKey("IdGeosModule")]
           [DataMember]
           public virtual GeosModule GeosModule { get; set; }
           [ForeignKey("IdGeosWorkbenchVersion")]
           [DataMember]
           public virtual GeosWorkbenchVersion GeosWorkbenchVersions { get; set; }
         #endregion
    }
}
