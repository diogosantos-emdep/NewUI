using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract(IsReference = true)]
    public class ConnectorFamilies : ModelBase, IDisposable
    {
        #region Fields
        UInt64 idFamily;
        string name;
        bool isCurrentFamily = false; //[sdeshpande][GEOS2-4098][26-12-2022]
        UInt32 orderNumber;//[Sudhir.Jangra][GEOS2-4221][12/04/2023]
        Int32 idSubFamilyConnector;
        string parent;
        string key;
        string parentName;
        #endregion

        #region Properties
        [DataMember]
        public UInt64 IdFamily
        {
            get { return idFamily; }
            set
            {
                idFamily = value;
                OnPropertyChanged("IdFamily");
            }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                this.name = value;
                OnPropertyChanged("Name");
            }
        }
        //[sdeshpande][GEOS2-4098][26-12-2022]
        [DataMember]
        public bool IsCurrentFamily
        {
            get
            {
                return isCurrentFamily;
            }

            set
            {
                isCurrentFamily = value;
                OnPropertyChanged("IsCurrentFamily");
            }
        }

        [DataMember]
        public string Parent
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
        [DataMember]
        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
                OnPropertyChanged("Key");
            }
        }

        [DataMember]
        public string ParentName
        {
            get
            {
                return parentName;
            }

            set
            {
                parentName = value;
                OnPropertyChanged("ParentName");
            }
        }

        [DataMember]
        public Int32 IdSubFamilyConnector
        {
            get
            {
                return idSubFamilyConnector;
            }

            set
            {
                idSubFamilyConnector = value;
                OnPropertyChanged("IdSubFamilyConnector");
            }
        }
        #region GEOS2-2596
        string name_es;
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

        string name_fr;
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

        string name_pt;
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

        string name_ro;
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

        string name_zh;
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

        string name_ru;
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

        string description;
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

        string description_es;
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

        string description_fr;
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

        string description_pt;
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

        string description_ro;
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

        string description_zh;
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

        string description_ru;
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
        //[Sudhir.Jangra][GEOS2-4221][12/04/2023]
        public UInt32 OrderNumber
        {
            get { return orderNumber; }
            set
            {
                orderNumber = value;
                OnPropertyChanged("OrderNumber");
            }
        }

        #endregion
        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

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
