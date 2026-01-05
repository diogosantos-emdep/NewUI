using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{//[Sudhir.Jangra][GEOS2-4502][19/07/2023]

    [DataContract]
    public class CustomProperty : ModelBase, IDisposable
    {
        #region Fields
        int idCustomConnectorProperty;
        string name;
        string name_es;
        string name_fr;
        string name_pt;
        string name_ro;
        string name_zh;
        string name_ru;
        string description;
        string description_es;
        string description_fr;
        string description_pt;
        string description_ro;
        string description_zh;
        string description_ru;
        Int32 idConnectorType;
        Int32 idLookupKey;
        Int32 idConnectorCategory;
        ValueType valueType;
        ValueKey valueKey;
        Int32 idFamily;
        #endregion

        #region Constructor
        public CustomProperty()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public int IdCustomConnectorProperty
        {
            get { return idCustomConnectorProperty; }
            set
            {
                idCustomConnectorProperty = value;
                OnPropertyChanged("IdCustomConnectorProperty");
            }
        }
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

        [DataMember]
        public string Name_es
        {
            get
            {
                return name_es;
            }

            set
            {
                name_es = value;
                OnPropertyChanged("Name_es");
            }
        }

        [DataMember]
        public string Name_fr
        {
            get
            {
                return name_fr;
            }

            set
            {
                name_fr = value;
                OnPropertyChanged("Name_fr");
            }
        }

        [DataMember]
        public string Name_pt
        {
            get
            {
                return name_pt;
            }

            set
            {
                name_pt = value;
                OnPropertyChanged("Name_pt");
            }
        }

        [DataMember]
        public string Name_ro
        {
            get
            {
                return name_ro;
            }

            set
            {
                name_ro = value;
                OnPropertyChanged("Name_ro");
            }
        }

        [DataMember]
        public string Name_zh
        {
            get
            {
                return name_zh;
            }

            set
            {
                name_zh = value;
                OnPropertyChanged("Name_zh");
            }
        }

        [DataMember]
        public string Name_ru
        {
            get
            {
                return name_ru;
            }

            set
            {
                name_ru = value;
                OnPropertyChanged("Name_ru");
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [DataMember]
        public string Description_es
        {
            get
            {
                return description_es;
            }

            set
            {
                description_es = value;
                OnPropertyChanged("Description_es");
            }
        }

        [DataMember]
        public string Description_fr
        {
            get
            {
                return description_fr;
            }

            set
            {
                description_fr = value;
                OnPropertyChanged("Description_fr");
            }
        }

        [DataMember]
        public string Description_pt
        {
            get
            {
                return description_pt;
            }

            set
            {
                description_pt = value;
                OnPropertyChanged("Description_pt");
            }
        }

        [DataMember]
        public string Description_ro
        {
            get
            {
                return description_ro;
            }

            set
            {
                description_ro = value;
                OnPropertyChanged("Description_ro");
            }
        }

        [DataMember]
        public string Description_zh
        {
            get
            {
                return description_zh;
            }

            set
            {
                description_zh = value;
                OnPropertyChanged("Description_zh");
            }
        }

        [DataMember]
        public string Description_ru
        {
            get
            {
                return description_ru;
            }

            set
            {
                description_ru = value;
                OnPropertyChanged("Description_ru");
            }
        }
        [DataMember]
        public Int32 IdConnectorType
        {
            get { return idConnectorType; }
            set
            {
                idConnectorType = value;
                OnPropertyChanged("IdConnectorType");
            }
        }
        [DataMember]
        public Int32 IdLookupKey
        {
            get { return idLookupKey; }
            set
            {
                idLookupKey = value;
                OnPropertyChanged("IdLookupKey");
            }
        }
        [DataMember]
        public Int32 IdConnectorCategory
        {
            get { return idConnectorCategory; }
            set
            {
                idConnectorCategory = value;
                OnPropertyChanged("IdConnectorCategory");
            }
        }
        [DataMember]
        public ValueType ValueType
        {
            get { return valueType; }
            set
            {
                valueType = value;
                OnPropertyChanged("ValueType");
            }
        }
        [DataMember]
        public ValueKey ValueKey
        {
            get { return valueKey; }
            set
            {
                valueKey = value;
                OnPropertyChanged("ValueKey");
            }
        }

        [DataMember]
        public Int32 IdFamily
        {
            get { return idFamily; }
            set
            {
                idFamily = value;
                OnPropertyChanged("IdFamily");
            }
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
