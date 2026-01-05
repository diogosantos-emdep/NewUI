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
    public class TotalWeekOrder : ModelBase, IDisposable
    {
        #region Fields
        WorkOrderType complaintsOrder;
        WorkOrderType urgentOrder;
        WorkOrderType incomingOrder;
        WorkOrderType futureOrder;
        #endregion

        #region Constructor
        public TotalWeekOrder()
        {

        }
        #endregion

        #region Properties


        [DataMember]
        public WorkOrderType ComplaintsOrder
        {
            get { return complaintsOrder; }
            set
            {
                complaintsOrder = value;
                OnPropertyChanged("ComplaintsOrder");
            }
        }


        [DataMember]
        public WorkOrderType UrgentOrder
        {
            get { return urgentOrder; }
            set
            {
                urgentOrder = value;
                OnPropertyChanged("UrgentOrder");
            }
        }

        [DataMember]
        public WorkOrderType IncomingOrder
        {
            get { return incomingOrder; }
            set
            {
                incomingOrder = value;
                OnPropertyChanged("IncomingOrder");
            }
        }

        [DataMember]
        public WorkOrderType FutureOrder
        {
            get { return futureOrder; }
            set
            {
                futureOrder = value;
                OnPropertyChanged("FutureOrder");
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
