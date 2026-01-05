using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OptimizedClass
{
    [DataContract]
    public class CustomerTargetDetail
    {
        #region  Fields
        List<CustomerDetail> customerDetail;
        List<TargetDetail> targetDetail;
       
        #endregion
        #region Properties

        [DataMember]
        public List<CustomerDetail> CustomerDetail
        {
            get
            {
                return customerDetail;
            }

            set
            {
                customerDetail = value;
            }
        }

        [DataMember]
        public List<TargetDetail> TargetDetail
        {
            get
            {
                return targetDetail;
            }

            set
            {
                targetDetail = value;
            }
        }

       #endregion
    }
}
