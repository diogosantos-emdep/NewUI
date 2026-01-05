using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{

    public class CustomerContacts : ModelBase, IDisposable
    {



        #region Fields
  
        string groupName;
        string plant;
        string email;





        #endregion
        #region Constructor
        public CustomerContacts()
        {

        }
        #endregion
        #region Properties

        [DataMember]
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                OnPropertyChanged("Email");
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
        public string Plant
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
        Int32 idCustomer;
        [DataMember]
        public Int32 IdCustomer
        {
            get { return idCustomer; }
            set { idCustomer = value; }
        }
        int idPlant;
        [DataMember]
        public int IdPlant
        {
            get { return idPlant; }
            set
            {
                idPlant = value;
                OnPropertyChanged("IdPlant");
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
