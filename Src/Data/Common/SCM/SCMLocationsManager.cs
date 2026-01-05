using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{
    [Table("SCM_locations")]
    [DataContract]
    public class SCMLocationsManager : ModelBase, IDisposable
    {

        //[pramod.misal][GEOS2-5524][03.07.2024]

        #region Declaration
        string fullName;
        string name;
        string title;
        string htmlColor;
        bool inUse;
        sbyte? isleaf;
        long idLocationByConnector;
        long idConnector;
        long idSite;
        string location;
        long position;
        long parent;
        Int64 idSampleLocation;
        Double width;
        Double height;

        #endregion
        #region Properties

        [Column("Height")]
        [DataMember]
        public Double Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }

        [Column("Width")]
        [DataMember]
        public Double Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }

        [Column("IdSampleLocation")]
        [DataMember]
        public Int64 IdSampleLocation
        {
            get
            {
                return idSampleLocation;
            }

            set
            {
                idSampleLocation = value;
                OnPropertyChanged("IdSampleLocation");
            }
        }

        [Column("IdConnector")]
        [DataMember]
        public long IdConnector
        {
            get
            {
                return idConnector;
            }

            set
            {
                idConnector = value;
                OnPropertyChanged("IdConnector");
            }
        }

        [Column("IdLocationByConnector")]
        [DataMember]
        public long IdLocationByConnector
        {
            get
            {
                return idLocationByConnector;
            }

            set
            {
                idLocationByConnector = value;
                OnPropertyChanged("IdLocationByConnector");
            }
        }

        [Column("IdSite")]
        [DataMember]
        public long IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [Column("FullName")]
        [DataMember]
        public string FullName
        {
            get
            {
                return fullName;
            }

            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("Location")]
        [DataMember]
        public string Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }

        [Column("Position")]
        [DataMember]
        public long Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        [Column("Parent")]
        [DataMember]
        public long Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
                OnPropertyChanged("Parent");
            }
        }


        [Column("HtmlColor")]
        [DataMember]
        public string HtmlColor
        {
            get
            {
                return htmlColor;
            }

            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }

        [Column("Title")]
        [DataMember]
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        [Column("InUse")]
        [DataMember]
        public bool InUse
        {
            get
            {
                return inUse;
            }

            set
            {
                inUse = value;
                OnPropertyChanged("InUse");
            }
        }

        [Column("IsLeaf")]
        [DataMember]
        public sbyte? IsLeaf
        {
            get
            {
                return isleaf;
            }

            set
            {
                isleaf = value;
                OnPropertyChanged("IsLeaf");
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

