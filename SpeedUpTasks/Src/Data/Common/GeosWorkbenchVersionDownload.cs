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

    [Table("geos_workbench_version_download")]
    [DataContract]
    public class GeosWorkbenchVersionDownload
    {
        #region Fields
        Int64 idGeosWorkbenchVersionDownload;
        String userIP;
        Int32 idGeosModuleVersion;
        DateTime downloadedIn;
        #endregion

        #region Properties
        [Key]
        [Column("IdGeosWorkbenchVersionDownload")]
        [DataMember]
        public Int64 IdGeosWorkbenchVersionDownload
        {
            get { return idGeosWorkbenchVersionDownload; }
            set { idGeosWorkbenchVersionDownload = value; }
        }

        [Column("UserIP")]
        [DataMember]
        public String UserIP
        {
            get { return userIP; }
            set { userIP = value; }
        }

        [Column("IdGeosModuleVersion")]
        [ForeignKey("GeosWorkbenchVersion")]
        [DataMember]
        public Int32 IdGeosModuleVersion
        {
            get { return idGeosModuleVersion; }
            set { idGeosModuleVersion = value; }
        }

        [Column("DownloadedIn")]
        [DataMember]
        public DateTime DownloadedIn
        {
            get { return downloadedIn; }
            set { downloadedIn = value; }
        }

       
        [DataMember]
        public virtual GeosWorkbenchVersion GeosWorkbenchVersion { get; set; }
        #endregion

    }
}
