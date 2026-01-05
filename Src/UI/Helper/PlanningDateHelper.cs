using System;
using DevExpress.Xpf.Accordion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.ERM;

namespace Emdep.Geos.UI.Helper
{
   public class PlanningDateHelper : DevExpress.Xpf.Accordion.IChildrenSelector
    {
        public IEnumerable SelectChildren(object item)
        {
            if (item is PlanningDateAccordian)
            {
                if(((PlanningDateAccordian)item).PlanningDeliveryDate != null && ((PlanningDateAccordian)item).PlanningDeliveryDate.Count()>0)
                {
                    //string tempdate = Convert.ToString(((PlanningDateAccordian)item).PlanningDeliwaryDate[0].DeliwaryDate);
                    //return ((PlanningDateAccordian)item).PlanningDeliwaryDate[0].DeliwaryDate.ToString();
                    string tempdate = Convert.ToString(((PlanningDateAccordian)item).PlanningDeliveryDate);
                    return ((PlanningDateAccordian)item).PlanningDeliveryDate;
                }
                else
                {
                    return null;
                }
                
            }
            else if (item is PlanningDeliveryDate)
            {
                string ot = Convert.ToString(((PlanningDeliveryDate)item).OtCodeList);
                return ((PlanningDeliveryDate)item).OtCodeList;
            }
            else return null;
        }
    }
}
