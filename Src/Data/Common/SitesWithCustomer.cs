using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class SitesWithCustomer : ModelBase,IDisposable
    {
        #region Fields

        UInt32 idSpareParts;
        string name;
        string orderNumber;
        string parent;
        UInt32? idGroup;
        string groupName;
        string key;
        Int32? idSite;
        int idRegion;
        string regionName;
        bool isChecked;
        ObservableCollection<People> salesOwnerList;
        Int32 idStatus;
        #endregion

        #region Constructor

        public SitesWithCustomer()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public Int32 IdStatus
        {
            get
            {
                return idStatus;
            }
            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

        [DataMember]
        public uint IdSpareParts
        {
            get { return idSpareParts; }
            set
            {
                idSpareParts = value;
                OnPropertyChanged("IdSpareParts");
            }
        }
        [DataMember]
        public int IdRegion
        {
            get { return idRegion; }
            set
            {
                idRegion = value;
                OnPropertyChanged("IdRegion");
            }
        }

        //[DataMember]
        //public string Key
        //{
        //    get { return key; }
        //    set
        //    {
        //        key = value;
        //        OnPropertyChanged("Key");
        //    }
        //}

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
        public string OrderNumber
        {
            get { return orderNumber; }
            set
            {
                orderNumber = value;
                OnPropertyChanged("OrderNumber");
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
                if (value != null)
                {
                    parent = value;
                    OnPropertyChanged("Parent");
                }
            }
        }

        [DataMember]
        public uint? IdGroup
        {
            get
            {
                return idGroup;
            }

            set
            {
                idGroup = value;
                OnPropertyChanged("IdGroup");
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
        public int? IdSite
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
        int idCountry;
        [DataMember]
        public int IdCountry
        {
            get
            {
                return idCountry;
            }

            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }
        string countryName;
        [DataMember]
        public string CountryName
        {
            get
            {
                return countryName;
            }

            set
            {
                countryName = value;
                OnPropertyChanged("CountryName");
            }
        }
        [DataMember]
        public string GroupName
        {
            get
            {
                return groupName;
            }

            set
            {
                groupName = value;
                OnPropertyChanged("GroupName");
            }
        }

        [DataMember]
        public string RegionName
        {
            get
            {
                return regionName;
            }

            set
            {
                regionName = value;
                OnPropertyChanged("RegionName");
            }
        }
        [DataMember]
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
     
        [DataMember]
        public ObservableCollection<People> SalesOwnerList
        {
            get { return salesOwnerList; }
            set
            {
                salesOwnerList = value;
                OnPropertyChanged("SalesOwnerList");
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

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
