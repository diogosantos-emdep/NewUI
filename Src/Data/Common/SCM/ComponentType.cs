using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class ComponentType : ModelBase, IDisposable
    {
        #region Fields  
        private int idType;
        private string name;
        private string name_es;
        private string name_fr;
        private string visualAidPATH;
        private string description;
        private string description_es;
        private string description_fr; 
        #endregion

        #region Properties
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
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        [DataMember]
        public string Name_es
        {
            get { return name_es; }
            set
            {
                name_es = value;
                OnPropertyChanged("Name_es");
            }
        }
        [DataMember]
        public string Name_fr
        {
            get { return name_fr; }
            set
            {
                name_fr = value;
                OnPropertyChanged("Name_fr");
            }
        }
        [DataMember]
        public string VisualAidPATH
        {
            get { return visualAidPATH; }
            set
            {
                visualAidPATH = value;
                OnPropertyChanged("VisualAidPATH");
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
        public string Description_es
        {
            get { return description_es; }
            set
            {
                description_es = value;
                OnPropertyChanged("Description_es");
            }
        }
        [DataMember]
        public string Description_fr
        {
            get { return description_fr; }
            set
            {
                description_fr = value;
                OnPropertyChanged("Description_fr");
            }
        }
        #endregion

        #region Constructor

        public ComponentType()
        {
        }

        #endregion

        #region Methods
        public override string ToString()
        {
            return Name;
        }

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
