using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{

    //[pramod.misal] [GEOS2-5381] [02.04.2024]
    [DataContract]
    public class ConnectorComponents : ModelBase, IDisposable
    {
        #region Fields
        private Int64 idConnector;
        private string connectorRef;
        private int componentIdType;
        private ushort componentIdColor;
        private string componentRef;
        private Int32 idColor;
        string componentTypeName;
        private string imageName;
        private object componentType;
        private Color color;
        private ObservableCollection<ComponentType> componentTypeList;
        private ObservableCollection<Color> colorList;
        private string componentImagesUrl;
        bool isDelVisible;
        Int32 creatorId;
        private ComponentType type;
        private Int64 idComponentsByConnector;
        #endregion

        #region Properties
        [DataMember]
        public bool IsDelVisible
        {
            get { return isDelVisible; }
            set
            {
                if (isDelVisible != value)
                {
                    isDelVisible = value;
                    OnPropertyChanged(nameof(IsDelVisible));
                }
            }
        }

        [DataMember]
        public Int32 CreatorId
        {
            get { return creatorId; }
            set { creatorId = value; OnPropertyChanged("CreatorId"); }
        }

        [DataMember]
        public Int64 IdComponentsByConnector
        {
            get { return idComponentsByConnector; }
            set { idComponentsByConnector = value; OnPropertyChanged("IdComponentsByConnector"); }
        }

        [DataMember]
        public ComponentType Type
        {
            get { return type; }
            set { type = value; OnPropertyChanged("Type"); }
        }

        [DataMember]
        public string ComponentImagesUrl
        {
            get { return componentImagesUrl; }
            set
            {
                componentImagesUrl = value;
                OnPropertyChanged("ComponentImagesUrl");
            }
        }

        [DataMember]
        public Color Color
        {
            get { return color; }
            set { color = value; OnPropertyChanged("Color"); }
        }

        [DataMember]
        public object ComponentType
        {
            get { return componentType; }
            set { componentType = value; OnPropertyChanged("ComponentType"); }
        }


        [DataMember]
        public Int64 IdConnector
        {
            get { return idConnector; }
            set { idConnector = value; OnPropertyChanged("IdConnector"); }
        }

        [DataMember]
        public string ConnectorRef
        {
            get { return connectorRef; }
            set { connectorRef = value; OnPropertyChanged("ConnectorRef"); }
        }
        [DataMember]
        public int ComponentIdType
        {
            get { return componentIdType; }
            set
            {
                componentIdType = value;
                OnPropertyChanged("ComponentIdType");
            }
        }

        [DataMember]
        public ushort ComponentIdColor
        {
            get { return componentIdColor; }
            set { componentIdColor = value; OnPropertyChanged("ComponentIdColor"); }
        }

        [DataMember]
        public string ComponentRef
        {
            get { return componentRef; }
            set { componentRef = value; OnPropertyChanged("ComponentRef"); }
        }

        [DataMember]
        public Int32 IdColor
        {
            get { return idColor; }
            set { idColor = value; OnPropertyChanged("IdColor"); }
        }

        [DataMember]
        public string ComponentTypeName
        {
            get { return componentTypeName; }
            set
            {
                componentTypeName = value;
                OnPropertyChanged("ComponentTypeName");
            }
        }

        [DataMember]
        public string ImageName
        {
            get
            {
                return imageName;
            }

            set
            {
                imageName = value;
                OnPropertyChanged("ImageName");
            }
        }

        [DataMember]
        public ObservableCollection<ComponentType> ComponentTypeList
        {
            get { return componentTypeList; }
            set
            {
                componentTypeList = value;
                OnPropertyChanged("ComponentTypeList");
            }
        }

        [DataMember]
        public ObservableCollection<Color> ColorList
        {
            get { return colorList; }
            set
            {
                colorList = value;
                OnPropertyChanged("ListColor");
            }
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
