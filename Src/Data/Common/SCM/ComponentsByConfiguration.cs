using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{//[Sudhir.Jangra][GEOS2-4971]
    [DataContract]
    public class ComponentsByConfiguration : ModelBase, IDisposable
    {
        #region Declarations
        private int idSearchConfiguration;
        private Int32? idColor;
        private Int32? idType;
        private string reference=string.Empty;
        private Int32 createdBy;
        private DateTime createdIn;
        private Int32 modifiedBy;
        private DateTime modifiedIn;   
        private ObservableCollection<Color> colorList;
        private ObservableCollection<ComponentType> componentTypeList;
        private ObservableCollection<string> conditionList;
        private Color selectedColor;
        private string selectedCondition;
        #endregion

        #region Properties

        [DataMember]
        public int IdSearchConfiguration
        {
            get { return idSearchConfiguration; }
            set
            {
                idSearchConfiguration = value;
                OnPropertyChanged("IdSearchConfiguration");
            }
        }

        [DataMember]
        public Int32? IdColor
        {
            get { return idColor; }
            set
            {
                idColor = value;
                OnPropertyChanged("IdColor");
            }
        }
        [DataMember]
        public Int32? IdType
        {
            get { return idType; }
            set
            {
                idType = value;
                OnPropertyChanged("IdType");
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
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [DataMember]
        public Int32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }
        [DataMember]
        public DateTime ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [DataMember]
        public ObservableCollection<Data.Common.SCM.Color> ColorList
        {
            get { return colorList; }
            set
            {
                colorList = value;
                OnPropertyChanged("ColorList");
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
        public ObservableCollection<string> ConditionList
        {
            get { return conditionList; }
            set
            {
                conditionList = value;
                OnPropertyChanged("ConditionList");
            }
        }

        [DataMember]
        public string SelectedCondition
        {
            get { return selectedCondition; }
            set
            {
                selectedCondition = value;
                OnPropertyChanged("SelectedCondition");
            }
        }

        [DataMember]
        public Color SelectedColor
        {
            get { return selectedColor; }
            set
            {
                selectedColor = value;
                OnPropertyChanged("SelectedColor");
            }
        }
        ComponentType selectedComponentType;
        [DataMember]
        public ComponentType SelectedComponentType
        {
            get { return selectedComponentType; }
            set
            {
                selectedComponentType = value;
                OnPropertyChanged("SelectedComponentType");
            }
        }
        #endregion

        #region Constructor
        public ComponentsByConfiguration()
        {

        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
