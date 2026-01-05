using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OptimizedClass
{
    [DataContract]
    public class BusinessUnitTargetDetail
    {
        #region  Fields
        List<BusinessUnitDetail> businessUnitDetail;
        List<BusinessUnitDetail> targetDetail;

        #endregion
        #region Properties

        [DataMember]
        public List<BusinessUnitDetail> BusinessUnitDetail
        {
            get
            {
                return businessUnitDetail;
            }

            set
            {
                businessUnitDetail = value;
            }
        }

        [DataMember]
        public List<BusinessUnitDetail> TargetDetail
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
