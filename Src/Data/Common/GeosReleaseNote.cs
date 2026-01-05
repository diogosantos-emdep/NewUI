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
     [Table("geos_release_notes")]
     [DataContract]
    public class GeosReleaseNote
    {
        #region Fields
        Int32 idReleaseNote;
         Int32? idGeosWorkbenchVersion;
         Byte? idGeosModule;
         Int32? idType;
         string description;
         DateTime? date;
         Int32? idRequestVersion;
        #endregion

         #region Properities
         [Key]
        [Column("IdReleaseNote")]
        [DataMember]
         public Int32 IdReleaseNote
        {
            get { return idReleaseNote; }
            set { idReleaseNote = value; }
        }

          [Column("IdGeosWorkbenchVersion")]
          [DataMember]
        public Int32? IdGeosWorkbenchVersion
        {
            get { return idGeosWorkbenchVersion; }
            set { idGeosWorkbenchVersion = value; }
        }

        [ForeignKey("GeosModule")]
        [Column("IdGeosModule")]
          [DataMember]
        public Byte? IdGeosModule
        {
            get { return idGeosModule; }
            set { idGeosModule = value; }
        }

        [Column("IdType")]
        [DataMember]
        public Int32? IdType
        {
            get { return idType; }
            set { idType = value; }
        }

        [Column("Description")]
        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [Column("Date")]
        [DataMember]
        public DateTime? Date
        {
            get { return date; }
            set { date = value; }
        }

          [Column("IdRequestVersion")]
          [DataMember]
        public Int32? IdRequestVersion
        {
            get { return idRequestVersion; }
            set { idRequestVersion = value; }
        }

        
         [DataMember]
         public virtual GeosModule GeosModule { get; set; }
         
         [ForeignKey("IdGeosWorkbenchVersion")]
         [DataMember]
         public virtual GeosWorkbenchVersion GeosWorkbenchVersions { get; set; }
         //[ForeignKey("IdRequestVersion")]
        //public virtual GeosWorkbenchVersion GeosWorkbenchOldVersions { get; set; }
         #endregion
    }
}
