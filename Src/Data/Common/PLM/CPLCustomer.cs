using Emdep.Geos.Data.Common.PCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PLM
{
    [DataContract]
    public class CPLCustomer : ModelBase, IDisposable
    {
        #region Fields

        string key;
        string groupName;
        string regionName;
        string parent;
        UInt32 idGroup;
        UInt32? idRegion;
        UInt32 idCreator;
        bool isChecked;
        string uniqueId;
        Country country;
        byte? idCountry;

        Site plant;
        UInt32? idPlant;
        UInt64 idCustomerPriceListCustomer;

        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;

        UInt64 idCustomerPriceList;
        Int32 isIncluded;
        #endregion

        #region Constructor

        public CPLCustomer()
        {

        }

        #endregion


        #region Properties

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
        public uint IdGroup
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
        public uint? IdRegion
        {
            get
            {
                return idRegion;
            }

            set
            {
                idRegion = value;
                OnPropertyChanged("IdRegion");
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
        public uint IdCreator
        {
            get
            {
                return idCreator;
            }

            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }
        [DataMember]
        public string UniqueId
        {
            get
            {
                return uniqueId;
            }

            set
            {
                uniqueId = value;
                OnPropertyChanged("UniqueId");
            }
        }

        [DataMember]
        public Country Country
        {
            get
            {
                return country;
            }

            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        [DataMember]
        public byte? IdCountry
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

        [DataMember]
        public Site Plant
        {
            get
            {
                return plant;
            }

            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }

        [DataMember]
        public uint? IdPlant
        {
            get
            {
                return idPlant;
            }

            set
            {
                idPlant = value;
                OnPropertyChanged("IdPlant");
            }
        }

        [DataMember]
        public UInt64 IdCustomerPriceListCustomer
        {
            get
            {
                return idCustomerPriceListCustomer;
            }

            set
            {
                idCustomerPriceListCustomer = value;
                OnPropertyChanged("IdCustomerPriceListCustomer");
            }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public uint? IdModifier
        {
            get
            {
                return idModifier;
            }

            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }
        }

        [DataMember]
        public DateTime? ModificationDate
        {
            get
            {
                return modificationDate;
            }

            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [DataMember]
        public ulong IdCustomerPriceList
        {
            get
            {
                return idCustomerPriceList;
            }

            set
            {
                idCustomerPriceList = value;
                OnPropertyChanged("IdCustomerPriceList");
            }
        }

        [DataMember]
        public Int32 IsIncluded
        {
            get
            {
                return isIncluded;
            }

            set
            {
                isIncluded = value;
                OnPropertyChanged("IsIncluded");
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
            CPLCustomer cPLCustomer = (CPLCustomer)this.MemberwiseClone();
            if (Country != null)
                cPLCustomer.Country = (Country)this.Country.Clone();

            if (plant != null)
                cPLCustomer.plant = (Site)this.plant.Clone();

            return cPLCustomer;
        }

        #endregion

    }
}
