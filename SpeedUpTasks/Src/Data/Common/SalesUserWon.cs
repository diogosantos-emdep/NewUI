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
    [DataContract]
   public class SalesUserWon : ModelBase, IDisposable
    {

        #region Fields

        double amount;
        string currency;
        Int32 idsalesresponsible;
        Int32 idSalesResponsibleAssemblyBU;
        Int32 idSalesOwner;
     
        #endregion

        #region Properties

        [DataMember]
        public double Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }

        [DataMember]
        public string Currency
        {
            get { return currency; }
            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }

        [DataMember]
        public Int32 Idsalesresponsible
        {
            get { return idsalesresponsible; }
            set
            {
                idsalesresponsible = value;
                OnPropertyChanged("Idsalesresponsible");
            }
        }

        [DataMember]
        public Int32 IdSalesResponsibleAssemblyBU
        {
            get { return idSalesResponsibleAssemblyBU; }
            set
            {
                idSalesResponsibleAssemblyBU = value;
                OnPropertyChanged("IdSalesResponsibleAssemblyBU");
            }
        }

        [DataMember]
        public Int32 IdSalesOwner
        {
            get { return idSalesOwner; }
            set
            {
                idSalesOwner = value;
                OnPropertyChanged("IdSalesOwner");
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
