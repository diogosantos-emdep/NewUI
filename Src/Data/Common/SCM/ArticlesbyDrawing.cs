using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class ArticlesbyDrawing : ModelBase, IDisposable
    {

        #region Declaration
        Int32 idArticle;
        string reference;
        string description;
        byte isObsolete;
        private uint idDrawing;
        private uint? quantity;
        private uint? purpose;
        private uint position;
        private uint? idArticleSubfamilyType;
        private uint idArticleSubfamily;
        private string comments;
        private byte? parent;
        private uint weldOrder;
        private bool unlinkFromTechnicalTemplate;
        private string unlinkedFeature;
        private bool visible;
        private bool connectInSerial;
        private bool inverseLink;
        private string ways;
        private string subfamilyname;
        private float? workingTravel;
        private float? terminalHeight;
        private byte? commonPointGroup;
        private bool contactZone;
        private uint pinPosition;
        private string waysGroup;
        private string subfamliytypename;
        private string detectionname;
        private string detectionname_es;
        private string detectionname_fr;
        private string detcode;
        private int downloadable;
        private string keyName;
        #endregion

        #region Properties       
        [DataMember]
        public Int32 IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        [DataMember]
        public string KeyName
        {
            get { return keyName; }
            set
            {
                keyName = value;
                OnPropertyChanged("KeyName");
            }
        }
        string parentName;
        [DataMember]
        public string ParentName
        {
            get { return parentName; }
            set
            {
                parentName = value;
                OnPropertyChanged("ParentName");
            }
        }
        [DataMember]
        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }
        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [DataMember]
        public uint IdDrawing
        {
            get { return idDrawing; }
            set
            {
                idDrawing = value;
                OnPropertyChanged(nameof(IdDrawing));
            }
        }

        [DataMember]
        public uint? Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        [DataMember]
        public uint? Purpose
        {
            get { return purpose; }
            set
            {
                purpose = value;
                OnPropertyChanged(nameof(Purpose));
            }
        }

        [DataMember]
        public uint Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        [DataMember]
        public uint? IdArticleSubfamilyType
        {
            get { return idArticleSubfamilyType; }
            set
            {
                idArticleSubfamilyType = value;
                OnPropertyChanged(nameof(IdArticleSubfamilyType));
            }
        }
      
        [DataMember]
        public uint IdArticleSubfamily
        {
            get { return idArticleSubfamily; }
            set
            {
                idArticleSubfamily = value;
                OnPropertyChanged(nameof(IdArticleSubfamily));
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
        public byte? Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                OnPropertyChanged(nameof(Parent));
            }
        }

        [DataMember]
        public uint WeldOrder
        {
            get { return weldOrder; }
            set
            {
                weldOrder = value;
                OnPropertyChanged(nameof(WeldOrder));
            }
        }

        [DataMember]
        public bool UnlinkFromTechnicalTemplate
        {
            get { return unlinkFromTechnicalTemplate; }
            set
            {
                unlinkFromTechnicalTemplate = value;
                OnPropertyChanged(nameof(UnlinkFromTechnicalTemplate));
            }
        }

        [DataMember]
        public string UnlinkedFeature
        {
            get { return unlinkedFeature; }
            set
            {
                unlinkedFeature = value;
                OnPropertyChanged(nameof(UnlinkedFeature));
            }
        }

        [DataMember]
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                OnPropertyChanged(nameof(Visible));
            }
        }

        [DataMember]
        public bool ConnectInSerial
        {
            get { return connectInSerial; }
            set
            {
                connectInSerial = value;
                OnPropertyChanged(nameof(ConnectInSerial));
            }
        }

        [DataMember]
        public bool InverseLink
        {
            get { return inverseLink; }
            set
            {
                inverseLink = value;
                OnPropertyChanged(nameof(InverseLink));
            }
        }

        [DataMember]
        public string Ways
        {
            get { return ways; }
            set
            {
                ways = value;
                OnPropertyChanged(nameof(Ways));
            }
        }

        [DataMember]
        public float? WorkingTravel
        {
            get { return workingTravel; }
            set
            {
                workingTravel = value;
                OnPropertyChanged(nameof(WorkingTravel));
            }
        }

        [DataMember]
        public float? TerminalHeight
        {
            get { return terminalHeight; }
            set
            {
                terminalHeight = value;
                OnPropertyChanged(nameof(TerminalHeight));
            }
        }

        [DataMember]
        public byte? CommonPointGroup
        {
            get { return commonPointGroup; }
            set
            {
                commonPointGroup = value;
                OnPropertyChanged(nameof(CommonPointGroup));
            }
        }

        [DataMember]
        public bool ContactZone
        {
            get { return contactZone; }
            set
            {
                contactZone = value;
                OnPropertyChanged(nameof(ContactZone));
            }
        }

        [DataMember]
        public uint PinPosition
        {
            get { return pinPosition; }
            set
            {
                pinPosition = value;
                OnPropertyChanged(nameof(PinPosition));
            }
        }

        [DataMember]
        public string WaysGroup
        {
            get { return waysGroup; }
            set
            {
                waysGroup = value;
                OnPropertyChanged(nameof(WaysGroup));
            }
        }
        [DataMember]
        public string Subfamliytypename
        {
            get { return subfamliytypename; }
            set
            {
                subfamliytypename = value;
                OnPropertyChanged("Subfamliytypename");
            }
        }
        [DataMember]
        public string Detectionname
        {
            get { return detectionname; }
            set
            {
                detectionname = value;
                OnPropertyChanged("detectionname");
            }
        }
        [DataMember]
        public string Detectionname_es
        {
            get { return detectionname_es; }
            set
            {
                detectionname_es = value;
                OnPropertyChanged("Detectionname_es");
            }
        }
        [DataMember]
        public string Detectionname_fr
        {
            get { return detectionname_fr; }
            set
            {
                detectionname_fr = value;
                OnPropertyChanged("Detectionname_fr");
            }
        }
        [DataMember]
        public int Downloadable
        {
            get { return downloadable; }
            set
            {
                downloadable = value;
                OnPropertyChanged("Downloadable");
            }
        }
        [DataMember]
        public string Detcode
        {
            get { return detcode; }
            set
            {
                detcode = value;
                OnPropertyChanged("Detcode");
            }
        }
        [DataMember]
        public byte IsObsolete
        {
            get { return isObsolete; }
            set
            {
                isObsolete = value;
                OnPropertyChanged("IsObsolete");
            }
        }

        [DataMember]
        public string Subfamilyname
        {
            get { return subfamilyname; }
            set
            {
                subfamilyname = value;
                OnPropertyChanged("Subfamilyname");
            }
        }
        #endregion

        #region Constructor

        public ArticlesbyDrawing()
        {
        }

        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            Article article = (Article)this.MemberwiseClone();       
            return article;
        }

        #endregion
    }
}
