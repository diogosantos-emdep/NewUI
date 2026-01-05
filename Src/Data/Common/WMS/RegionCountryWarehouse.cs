using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{

    public class RegionCountryWarehouse : ModelBase, IDisposable
    {

        Int64 idSite;
        string name;
        Int64 childCount;
        Int64? parentId;
        string levelType;
        private static int _counter = 1;
        string key;
        string parentKey;
        string nameWithArticleCount;
        bool isChecked;
        [DataMember]
        public Int64 IdSite
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

        [DataMember]
        public String Name
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
        public Int64 ChildCount
        {
            get
            {
                return childCount;
            }

            set
            {
                childCount = value;
                OnPropertyChanged("ChildCount");
            }
        }

        [DataMember]
        public Int64? ParentId
        {
            get
            {
                return parentId;
            }

            set
            {
                parentId = value;
                OnPropertyChanged("ParentId");
            }
        }

        [DataMember]
        public string LevelType
        {
            get
            {
                return levelType;
            }

            set
            {
                levelType = value;
                OnPropertyChanged("LevelType");
            }
        }

        //[rdixit][GEOS2-8200][18.09.2025]
        [DataMember]
        public string Key
        {
            get { return key; }
            set
            {
                key = value;
                OnPropertyChanged("Key");
            }
        }

        [DataMember]
        public string ParentKey
        {
            get { return parentKey; }
            set
            {
                parentKey = value;
                OnPropertyChanged("ParentKey");
            }
        }

        [DataMember]
        public string NameWithCount
        {
            get { return nameWithArticleCount; }
            set
            {
                if (ChildCount == 0)
                    nameWithArticleCount = Name;
                else
                    nameWithArticleCount = $"{Name} [{ChildCount}]";
                OnPropertyChanged("NameWithCount");
            }
        }
        public int UniqueKey { get; }

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
        #region Constructor
        public RegionCountryWarehouse()
        {
            UniqueKey = _counter++;
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
