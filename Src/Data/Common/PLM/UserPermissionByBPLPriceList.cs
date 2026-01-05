using System;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common.PLM
{
    [DataContract]
    public class UserPermissionByBPLPriceList : ModelBase, IDisposable
    {

        #region Declaration
        UInt64 idUserPermissionByBPLPriceList;
        Permission permission;
        BasePrice basePrice;
        User user;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;
        #endregion

        #region Properties

        [DataMember]
        public UInt64 IdUserPermissionByBPLPriceList
        {
            get
            {
                return idUserPermissionByBPLPriceList;
            }

            set
            {
                idUserPermissionByBPLPriceList = value;
                OnPropertyChanged("IdUserPermissionByBPLPriceList");
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
        public BasePrice BasePrice
        {
            get
            {
                return basePrice;
            }

            set
            {
                basePrice = value;
                OnPropertyChanged("BasePrice");
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

        public UserPermissionByBPLPriceList()
        {
            this.Permission = new Permission();
            this.BasePrice = new BasePrice();
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
            UserPermissionByBPLPriceList userPermissionByBPLPriceList = (UserPermissionByBPLPriceList)this.MemberwiseClone();

            if (this.Permission != null)
                userPermissionByBPLPriceList.Permission = (Permission)this.Permission.Clone();

            if (this.BasePrice != null)
                userPermissionByBPLPriceList.BasePrice = (BasePrice)this.BasePrice.Clone();

            if (this.User != null)
                userPermissionByBPLPriceList.User = (User)this.User.Clone();
            
            return userPermissionByBPLPriceList;
        }

        #endregion
    }
}
