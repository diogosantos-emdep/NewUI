using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Windows;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class Components : ModelBase, IDisposable
    {
        #region Fields  
        private ObservableCollection<Color> colorList;
        private string selectedColor;
        private ObservableCollection<string> conditionList;
        private ObservableCollection<ComponentType> componentTypeList;
        private string selectedComponentType;
        private string selectedCondition;
        private string reference;
        Visibility clearColor;
        Visibility clearType;
        int idType;
        int idColor;
        int idConnector;
        #endregion

        #region Properties
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
        public string SelectedComponentType
        {
            get { return selectedComponentType; }
            set
            {
                selectedComponentType = value;
                OnPropertyChanged("SelectedComponentType");
                if (value == null)
                    ClearType = Visibility.Hidden;
                else
                    ClearType = Visibility.Visible;
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
        public ObservableCollection<Color> ColorList
        {
            get { return colorList; }
            set
            {
                colorList = value;
                OnPropertyChanged("ListColor");
            }
        }

        [DataMember]
        public string SelectedColor
        {
            get { return selectedColor; }
            set
            {
                selectedColor = value;
                OnPropertyChanged("SelectedColor");
                if (value == null)
                    ClearColor = Visibility.Hidden;
                else
                    ClearColor = Visibility.Visible;
            }
        }
        [DataMember]
        public Visibility ClearColor
        {
            get { return clearColor; }
            set
            {
                clearColor = value;
                OnPropertyChanged("ClearColor");
            }
        }
        [DataMember]
        public Visibility ClearType
        {
            get { return clearType; }
            set
            {
                clearType = value;
                OnPropertyChanged("ClearType");
            }
        }
        //[rdixit][GEOS2-5802][05.09.2024]
        [DataMember]
        public int IdType
        {
            get { return idType; }
            set
            {
                idType = value;
                OnPropertyChanged("IdType");
            }
        }
        [DataMember]
        public int IdColor
        {
            get { return idColor; }
            set
            {
                idColor = value;
                OnPropertyChanged("IdColor");
            }
        }
        [DataMember]
        public int IdConnector
        {
            get { return idConnector; }
            set
            {
                idConnector = value;
                OnPropertyChanged("IdConnector");
            }
        }
        
        #endregion

        #region Constructor

        public Components()
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
