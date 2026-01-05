using System;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common.PLM
{
    [DataContract]
    public class UserPermissionByCPLPriceList : ModelBase, IDisposable
    {

        #region Declaration
        UInt64 idUserPermissionByCPLPriceList;
        Permission permission;
        CustomerPrice customerPrice;
        User user;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;
        #endregion

        #region Properties

        [DataMember]
        public UInt64 IdUserPermissionByCPLPriceList
        {
            get
            {
                return idUserPermissionByCPLPriceList;
            }

            set
            {
                idUserPermissionByCPLPriceList = value;
                OnPropertyChanged("IdUserPermissionByCPLPriceList");
            }
        }

        [DataMember]
        public Permission Permission
        {
            get
            {
                return permission;
            }

            set
            {
                permission = value;
                OnPropertyChanged("Permission");
            }
        }

        [DataMember]
        public CustomerPrice CustomerPrice
        {
            get
            {
                return customerPrice;
            }

            set
            {
                customerPrice = value;
                OnPropertyChanged("CustomerPrice");
            }
        }

        [DataMember]
        public User User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
                OnPropertyChanged("User");
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

        #endregion

        #region Constructor

        public UserPermissionByCPLPriceList()
        {
            this.Permission = new Permission();
            this.CustomerPrice = new CustomerPrice();
            this.User = new User();
        }

        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            UserPermissionByCPLPriceList userPermissionByCPLPriceList = (UserPermissionByCPLPriceList)this.MemberwiseClone();

            if (this.Permission != null)
                userPermissionByCPLPriceList.Permission = (Permission)this.Permission.Clone();

            if (this.CustomerPrice != null)
                userPermissionByCPLPriceList.CustomerPrice = (CustomerPrice)this.CustomerPrice.Clone();

            if (this.User != null)
                userPermissionByCPLPriceList.User = (User)this.User.Clone();

            return userPermissionByCPLPriceList;
        }

        #endregion
    }
}
