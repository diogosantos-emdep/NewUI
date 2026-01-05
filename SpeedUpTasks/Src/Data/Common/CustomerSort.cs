using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    public class CustomerSort : ModelBase, IDisposable
    {
        #region Fields
        Int32 idCustomer;
        Int32 idSite;
        double amount;
        #endregion

        #region Constructor
        public CustomerSort()
        {

        }
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public Int32 IdCustomer
        {
            get
            {
                return idCustomer;
            }

            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdSite
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

        [NotMapped]
        [DataMember]
        public double Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
                OnPropertyChanged("Amount");
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
