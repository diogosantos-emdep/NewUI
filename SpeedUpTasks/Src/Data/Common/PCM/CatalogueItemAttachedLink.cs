using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
   [DataContract]
    public class CatalogueItemAttachedLink : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idCatalogueItemAttachedLink;
        UInt32 idCatalogueItem;
        string name;
        string description;
        string address;
        UInt32 createdBy;
        DateTime createdIn;
        UInt32 modifiedBy;
        DateTime? modifiedIn;

        DateTime? updatedDate;
        #endregion

        #region Constructor

        public CatalogueItemAttachedLink()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public UInt32 IdCatalogueItemAttachedLink
        {
            get
            {
                return idCatalogueItemAttachedLink;
            }

            set
            {
                idCatalogueItemAttachedLink = value;
                OnPropertyChanged("IdCatalogueItemAttachedLink");
            }
        }

        [DataMember]
        public UInt32 IdCatalogueItem
        {
            get
            {
                return idCatalogueItem;
            }

            set
            {
                idCatalogueItem = value;
                OnPropertyChanged("IdCatalogueItem");
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
        public string Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
                OnPropertyChanged("Address");
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
        public DateTime CreatedIn
        {
            get
            {
                return createdIn;
            }

            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [DataMember]
        public UInt32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }

            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [DataMember]
        public DateTime? ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [DataMember]
        public DateTime? UpdatedDate
        {
            get
            {
                return updatedDate;
            }

            set
            {
                updatedDate = value;
                OnPropertyChanged("UpdatedDate");
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

        #endregion
    }
}
