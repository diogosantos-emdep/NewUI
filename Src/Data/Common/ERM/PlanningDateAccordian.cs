using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class PlanningDateAccordian
    {
        [DataMember]
        public string deliveryWeek { get; set; }
        [DataMember]
        public string copyDeliveryWeek { get; set; }
        [DataMember]
        public ObservableCollection<PlanningDeliveryDate> PlanningDeliveryDate { get; set; }
        public override string ToString()
        {
            return deliveryWeek;
        }

        public PlanningDateAccordian()
        {
        }
        public PlanningDateAccordian(string DeliveryWeek, string CopyDeliveryWeek, ObservableCollection<PlanningDeliveryDate> PlanningDeliveryDate)
        {
            DeliveryWeek = deliveryWeek;
            CopyDeliveryWeek = copyDeliveryWeek;
            PlanningDeliveryDate = new ObservableCollection<PlanningDeliveryDate>(PlanningDeliveryDate);
        }
    }
}
