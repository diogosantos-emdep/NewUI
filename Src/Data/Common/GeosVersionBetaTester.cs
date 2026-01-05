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
    [Table("geos_version_beta_testers")]
    [DataContract]
    public class GeosVersionBetaTester
    {
        #region Fields
        Int32 idGeosVersionBetaTester;
        Int32 idUser;
        Int32 idGeosVersion;
        #endregion

        #region Properties
        [Key]
        [Column("IdGeosVersionBetaTester")]
        [DataMember]
        public Int32 IdGeosVersionBetaTester
        {
            get { return idGeosVersionBetaTester; }
            set { idGeosVersionBetaTester = value; }
        }

        [Column("IdGeosVersion")]
        [ForeignKey("GeosWorkbenchVersion")]
        [DataMember]
        public Int32 IdGeosVersion
        {
            get { return idGeosVersion; }
            set { idGeosVersion = value; }
        }

        [Column("IdUser")]
        [ForeignKey("User")]
        [DataMember]
        public Int32 IdUser
        {
            get { return idUser; }
            set { idUser = value; }
        }

        [DataMember]
        public virtual GeosWorkbenchVersion GeosWorkbenchVersion { get; set; }

        [DataMember]
        public virtual User User { get; set; }
        #endregion
    }
}
