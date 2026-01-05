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
    public class DashboardInventory : ModelBase, IDisposable
    {
        #region Fields

        Int64 count;
        Int16 progress;
        Int64 totalItemsToRefill;
        Int64 totalItemsToRefillOutOfStock;
        #endregion

        #region Constructor
        public DashboardInventory()
        {

        }
        #endregion

        #region Properties


        [DataMember]
        public Int64 Count
        {
            get { return count; }
            set
            {
                count = value;
                OnPropertyChanged("Count");
            }
        }

        [DataMember]
        public Int16 Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }

        [DataMember]
        public Int64 TotalItemsToRefill
        {
            get { return totalItemsToRefill; }
            set
            {
                totalItemsToRefill = value;
                OnPropertyChanged("totalItemsToRefill");
            }
        }

        [DataMember]
        public Int64 TotalItemsToRefillOutOfStock
        {
            get { return totalItemsToRefillOutOfStock; }
            set
            {
                totalItemsToRefillOutOfStock = value;
                OnPropertyChanged("TotalItemsToRefillOutOfStock");
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
