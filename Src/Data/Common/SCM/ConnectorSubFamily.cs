using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Data.Common.SCM
{
    public class ConnectorSubFamily : ModelBase, IDisposable
    {
        #region Fields
        string familyName;
        string oldSubFamilyName;
        string oldFamilyName;
        private uint id;
        private string name;
        private uint idFamily;
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
        string isInUse;
        ObservableCollection<SubFamilyImage> imageList;
        UInt32 createdBy;
        List<FamilyImage> familyImagesList;
        #endregion

        #region Properties
        [DataMember]
        public uint Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }
        [DataMember]
        public uint IdFamily
        {
            get { return idFamily; }
            set { idFamily = value; OnPropertyChanged("IdFamily"); }
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
        public string IsInUse
        {
            get { return isInUse; }
            set
            {
                isInUse = value;
                OnPropertyChanged("IsInUse");
            }
        }
        [DataMember]
        public ObservableCollection<SubFamilyImage> ImageList
        {
            get { return imageList; }
            set
            {
                imageList = value;
                OnPropertyChanged("ImageList");
            }
        }
        [DataMember]
        public UInt32 CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [DataMember]
        private string path;
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
        public string FamilyName
        {
            get
            {
                return familyName;
            }

            set
            {
                familyName = value;
                OnPropertyChanged("FamilyName");
            }
        }

        [DataMember]
        public string OldSubFamilyName
        {
            get
            {
                return oldSubFamilyName;
            }

            set
            {
                oldSubFamilyName = value;
                OnPropertyChanged("OldName");
            }
        }
        [DataMember]
        public string OldFamilyName
        {
            get
            {
                return oldFamilyName;
            }

            set
            {
                oldFamilyName = value;
                OnPropertyChanged("OldName");
            }
        }
        #endregion

        #region Constructor
        public ConnectorSubFamily()
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
