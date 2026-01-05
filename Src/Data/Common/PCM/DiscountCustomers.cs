using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.PCM;

namespace Emdep.Geos.Data.Common.PCM
{
   public class DiscountCustomers : ModelBase, IDisposable
    {
        #region Fields
        string key;
        string groupName;
        string regionName;
        string parent;
        UInt32? idGroup;
        UInt32? idRegion;
        UInt32 idCreator;
        bool isChecked;
        string uniqueId;
        Country country;
        UInt32? idCountry;

        Site plant;
        UInt32? idPlant;
        UInt64 idCustomerDiscountCustomer;

        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;

        UInt64 idCustomerDiscount;
        Int32 isIncluded;
        #endregion

        #region Properties

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
        public uint? IdCountry
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
        public UInt64 IdCustomerDiscountCustomer
        {
            get
            {
                return idCustomerDiscountCustomer;
            }

            set
            {
                idCustomerDiscountCustomer = value;
                OnPropertyChanged("IdCustomerDiscountCustomer");
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
        public ulong IdCustomerDiscount
        {
            get
            {
                return idCustomerDiscount;
            }

            set
            {
                idCustomerDiscount = value;
                OnPropertyChanged("IdCustomerDiscount");
            }
        }

        #endregion

        #region Constructor
        public DiscountCustomers()
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
            DiscountCustomers discountCustomers = (DiscountCustomers)this.MemberwiseClone();
            if (Country != null)
                discountCustomers.Country = (Country)this.Country.Clone();

            if (plant != null)
                discountCustomers.plant = (Site)this.plant.Clone();

            return discountCustomers;
            //return this.MemberwiseClone();
        }
        #endregion
    }
}
