using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class ScmDrawing : ModelBase, IDisposable
    {
        #region Fields
        private uint idDrawing;
        private string path;
        private ushort idSite;
        private string siteName;
        private string siteNameC;
        private string siteNameM;
        private byte idTemplate;
        private byte idCPType;
        private string createdBy;
        private DateTime? createdIn;
        private string modifiedBy;
        private DateTime? modifiedIn;
        private string others;
        private string comments;
        private bool debugged;
        private ushort debuggedBy;
        private bool isObsolete;
        private DateTime modifiedLDM;
        private ushort ldmModifiedBy;
        private uint? idSolidWorksProject;
        private bool isReadOnly;
        private uint? idRelatedDrawing;
        private bool notStandard;
        private ushort? idTechnicalTemplate;
        private string cptypeName;
        private string templateName;
        private List<SCMDetection> detectionList;
        private long idCPTypenew;
        private string countryIconUrl; //[nsatpute][24.07.2025][GEOS2-8090]
        #endregion

        #region Constructor
        public ScmDrawing()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public string CptypeName
        {
            get
            {
                return cptypeName;
            }

            set
            {
                cptypeName = value;
                OnPropertyChanged("CptypeName");
            }
        }
        [DataMember]
        public string TemplateName
        {
            get
            {
                return templateName;
            }

            set
            {
                templateName = value;
                OnPropertyChanged("TemplateName");
            }
        }
        [DataMember]
        public uint IdDrawing
        {
            get
            {
                return idDrawing;
            }

            set
            {
                idDrawing = value;
                OnPropertyChanged("IdDrawing");
            }
        }

        [DataMember]
        public string Path
        {
            get
            {
                return path;
            }

            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }

        [DataMember]
        public string SiteName
        {
            get { return siteName; }
            set
            {
                siteName = value;
                OnPropertyChanged(nameof(SiteName));
            }
        }

        [DataMember]
        public string SiteNameC
        {
            get { return siteNameC; }
            set
            {
                siteNameC = value;
                OnPropertyChanged(nameof(SiteNameC));
            }
        }

        [DataMember]
        public string SiteNameM
        {
            get { return siteNameM; }
            set
            {
                siteNameM = value;
                OnPropertyChanged(nameof(SiteNameM));
            }
        }
        [DataMember]
        public ushort IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged(nameof(IdSite));
            }
        }

        [DataMember]
        public byte IdTemplate
        {
            get { return idTemplate; }
            set
            {
                idTemplate = value;
                OnPropertyChanged(nameof(IdTemplate));
            }
        }

        [DataMember]
        public byte IdCPType
        {
            get { return idCPType; }
            set
            {
                idCPType = value;
                OnPropertyChanged(nameof(IdCPType));
            }
        }

        [DataMember]
        public long IdCPTypenew         //[rushikesh.gaikwad][18.062024]
        {
            get { return idCPTypenew; }
            set
            {
                idCPTypenew = value;
                OnPropertyChanged(nameof(IdCPTypenew));
            }
        }

        [DataMember]
        public string CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged(nameof(CreatedBy));
            }
        }

        [DataMember]
        public DateTime? CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged(nameof(CreatedIn));
            }
        }

        [DataMember]
        public string ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged(nameof(ModifiedBy));
            }
        }

        [DataMember]
        public DateTime? ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged(nameof(ModifiedIn));
            }
        }

        [DataMember]
        public string Others
        {
            get { return others; }
            set
            {
                others = value;
                OnPropertyChanged(nameof(Others));
            }
        }

        [DataMember]
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged(nameof(Comments));
            }
        }

        [DataMember]
        public bool Debugged
        {
            get { return debugged; }
            set
            {
                debugged = value;
                OnPropertyChanged(nameof(Debugged));
            }
        }

        [DataMember]
        public ushort DebuggedBy
        {
            get { return debuggedBy; }
            set
            {
                debuggedBy = value;
                OnPropertyChanged(nameof(DebuggedBy));
            }
        }

        [DataMember]
        public bool IsObsolete
        {
            get { return isObsolete; }
            set
            {
                isObsolete = value;
                OnPropertyChanged(nameof(IsObsolete));
            }
        }

        [DataMember]
        public DateTime ModifiedLDM
        {
            get { return modifiedLDM; }
            set
            {
                modifiedLDM = value;
                OnPropertyChanged(nameof(ModifiedLDM));
            }
        }

        [DataMember]
        public ushort LdmModifiedBy
        {
            get { return ldmModifiedBy; }
            set
            {
                ldmModifiedBy = value;
                OnPropertyChanged(nameof(LdmModifiedBy));
            }
        }

        [DataMember]
        public uint? IdSolidWorksProject
        {
            get { return idSolidWorksProject; }
            set
            {
                idSolidWorksProject = value;
                OnPropertyChanged(nameof(IdSolidWorksProject));
            }
        }

        [DataMember]
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set
            {
                isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        [DataMember]
        public uint? IdRelatedDrawing
        {
            get { return idRelatedDrawing; }
            set
            {
                idRelatedDrawing = value;
                OnPropertyChanged(nameof(IdRelatedDrawing));
            }
        }

        [DataMember]
        public bool NotStandard
        {
            get { return notStandard; }
            set
            {
                notStandard = value;
                OnPropertyChanged(nameof(NotStandard));
            }
        }

        [DataMember]
        public ushort? IdTechnicalTemplate
        {
            get { return idTechnicalTemplate; }
            set
            {
                idTechnicalTemplate = value;
                OnPropertyChanged(nameof(IdTechnicalTemplate));
            }
        }
        [DataMember]
        public List<SCMDetection> DetectionList
        {
            get { return detectionList; }
            set
            {
                detectionList = value;
                OnPropertyChanged(nameof(DetectionList));
            }
        }
		//[nsatpute][24.07.2025][GEOS2-8090]
        [NotMapped]
        [DataMember]
        public string CountryIconUrl
        {
            get { return countryIconUrl; }
            set
            {
                countryIconUrl = value;
                OnPropertyChanged(nameof(CountryIconUrl));
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
