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
    [Table("geos_workbench_versions")]
    [DataContract(IsReference=true)]
    public class GeosWorkbenchVersion 
    {
        #region Fields

        Int32 idGeosWorkbenchVersion;
        Int32? idReleaser;
        Int32? idTester;
        SByte? isBeta;
        SByte? isPublish;
        SByte? isTraining;
        DateTime? releasedIn;
        DateTime? testingFinishedIn;
        DateTime? testingStartedIn;
        String versionNumber;
        Int32? idFullVersion;
        long? fileSize;
        DateTime? expiryDate;
        #endregion

        #region Constructor

        public GeosWorkbenchVersion()
        {
            this.GeosModuleDocumentations = new List<GeosModuleDocumentation>();
            this.GeosWorkbenchVersionDownload = new List<GeosWorkbenchVersionDownload>();
            this.GeosWorkbenchVersionsFiles = new List<GeosWorkbenchVersionsFile>();
            this.GeosReleaseNotes = new List<GeosReleaseNote>();
            this.GeosVersionBetaTesters = new List<GeosVersionBetaTester>();
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdGeosWorkbenchVersion")]
        [DataMember]
        public Int32 IdGeosWorkbenchVersion
        {
            get { return idGeosWorkbenchVersion; }
            set { idGeosWorkbenchVersion = value; }
        }

        [Column("IdReleaser")]
        [DataMember]
        public Int32? IdReleaser
        {
            get { return idReleaser; }
            set { idReleaser = value; }
        }

        [Column("IdTester")]
        [DataMember]
        public Int32? IdTester
        {
            get { return idTester; }
            set { idTester = value;}
        }

        [Column("IsBeta")]
        [DataMember]
        public SByte? IsBeta
        {
            get { return isBeta; }
            set { isBeta = value;  }
        }

        [Column("IsPublish")]
        [DataMember]
        public SByte? IsPublish
        {
            get { return isPublish; }
            set { isPublish = value;  }
        }

        [Column("IsTraining")]
        [DataMember]
        public SByte? IsTraining
        {
            get { return isTraining; }
            set { isTraining = value;}
        }

        [Column("ReleasedIn")]
        [DataMember]
        public DateTime? ReleasedIn
        {
            get { return releasedIn; }
            set { releasedIn = value;  }
        }

        [Column("TestingFinishedIn")]
        [DataMember]
        public DateTime? TestingFinishedIn
        {
            get { return testingFinishedIn; }
            set { testingFinishedIn = value;  }
        }

        [Column("TestingStartedIn")]
        [DataMember]
        public DateTime? TestingStartedIn
        {
            get { return testingStartedIn; }
            set { testingStartedIn = value; }
        }


        [Column("VersionNumber")]
        [DataMember]
        public String VersionNumber
        {
            get { return versionNumber; }
            set {versionNumber = value; }
        }

        [Column("IdFullVersion")]
        [DataMember]
        public Int32? IdFullVersion
        {
            get { return idFullVersion; }
            set { idFullVersion = value; }
        }

        [Column("FileSize")]
        [DataMember]
        public long? FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        [Column("ExpiryDate")]
        [DataMember]
        public DateTime? ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; }
        }

        [DataMember]
        public virtual List<GeosModuleDocumentation> GeosModuleDocumentations { get; set; }
        [DataMember]
        public virtual List<GeosWorkbenchVersionDownload> GeosWorkbenchVersionDownload { get; set; }
        [DataMember]
        public virtual List<GeosWorkbenchVersionsFile> GeosWorkbenchVersionsFiles { get; set; }
        [DataMember]
        public virtual List<GeosReleaseNote> GeosReleaseNotes { get; set; }
        [DataMember]
        public virtual List<GeosVersionBetaTester> GeosVersionBetaTesters { get; set; }

        #endregion

    }
}
