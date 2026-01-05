using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Drawing;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common
{
    [Table("warehouse_locations")]
    [DataContract]
    public class WarehouseLocation : ModelBase, IDisposable
    {
        #region Declaration

        long idWarehouseLocation;
        long idWarehouse;
        long parent;
        long position;

        string fullName;
        string name;

        string locationParent;
        string locationchild1;
        string locationchild2;
        Bitmap directionImage;
        ImageDirection imageDirection;

        bool isChecked;
        sbyte? isLead;
        string htmlColor;
        bool inUse;
        Int32? idDirection;
        LookupValue direction;
        Double latitude;
        Double longitude;
        Double width;
        Double height;
        string fileName;
        Int64 quantity;
        string levelFirstName;
        string levelSecondName;
        string levelThirdName;
        #endregion

        #region Properties

        [Key]
        [Column("IdWarehouseLocation")]
        [DataMember]
        public long IdWarehouseLocation
        {
            get
            {
                return idWarehouseLocation;
            }

            set
            {
               
                idWarehouseLocation = value;
                OnPropertyChanged("IdWarehouseLocation");
            }
        }

        [Column("IdWarehouse")]
        [DataMember]
        public long IdWarehouse
        {
            get
            {
                return idWarehouse;
            }

            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
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

        [NotMapped]
        [DataMember]
        public string LocationParent
        {
            get
            {
                return locationParent;
            }

            set
            {
                locationParent = value;
                OnPropertyChanged("LocationParent");
            }
        }


        [NotMapped]
        [DataMember]
        public string Locationchild1
        {
            get
            {
                return locationchild1;
            }

            set
            {
                locationchild1 = value;
                OnPropertyChanged("Locationchild1");
            }
        }

        [NotMapped]
        [DataMember]
        public string Locationchild2
        {
            get
            {
                return locationchild2;
            }

            set
            {
                locationchild2 = value;
                OnPropertyChanged("Locationchild2");
            }
        }

        [NotMapped]
        [DataMember]
        public Bitmap DirectionImage
        {
            get
            {
                return directionImage;
            }

            set
            {
                directionImage = value;
                OnPropertyChanged("DirectionImage");
            }
        }

        [NotMapped]
        [DataMember]
        public ImageDirection ImageDirection
        {
            get
            {
                return imageDirection;
            }

            set
            {
                imageDirection = value;
                OnPropertyChanged("ImageDirection");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }

            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        [Column("IsLead")]
        [DataMember]
        public sbyte? IsLead
        {
            get
            {
                return isLead;
            }

            set
            {
                isLead = value;
                OnPropertyChanged("IsLead");
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

        [Column("IdDirection")]
        [DataMember]
        public Int32? IdDirection
        {
            get
            {
                return idDirection;
            }

            set
            {
                idDirection = value;
                OnPropertyChanged("IdDirection");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue Direction
        {
            get
            {
                return direction;
            }

            set
            {
                direction = value;
                OnPropertyChanged("Direction");
            }
        }

        [NotMapped]
        [DataMember]
        public Double Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                OnPropertyChanged("Latitude");
            }
        }

        [NotMapped]
        [DataMember]
        public Double Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                OnPropertyChanged("Longitude");
            }
        }

        [NotMapped]
        [DataMember]
        public Double Width
        {
            get { return width; }
            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }

        [NotMapped]
        [DataMember]
        public Double Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }

        [NotMapped]
        [DataMember]
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        [NotMapped]
        [DataMember]
        public string LevelFirstName
        {
            get { return levelFirstName; }
            set
            {
                levelFirstName = value;
                OnPropertyChanged("LevelFirstName");
            }
        }

        [NotMapped]
        [DataMember]
        public string LevelSecondName
        {
            get { return levelSecondName; }
            set
            {
                levelSecondName = value;
                OnPropertyChanged("LevelSecondName");
            }
        }

        [NotMapped]
        [DataMember]
        public string LevelThirdName
        {
            get { return levelThirdName; }
            set
            {
                levelThirdName = value;
                OnPropertyChanged("LevelThirdName");
            }
        }
        #endregion

        #region Constructor

        public WarehouseLocation()
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
            return this.MemberwiseClone();
        }

        #endregion
    }
}
