using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OptimizedClass
{
    [DataContract]
    public class CarProjectDetail
    {
        #region Fields
        Int64 idCarProject;
        string name;
        List<OfferDetail> offers;
        double? projectOfferAmount;
        Int32 idCustomer;
        #endregion

        #region Properties
        [DataMember]
        public long IdCarProject
        {
            get
            {
                return idCarProject;
            }

            set
            {
                idCarProject = value;
            }
        }

        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        [DataMember]
        public List<OfferDetail> Offers
        {
            get
            {
                return offers;
            }

            set
            {
                offers = value;
            }
        }

        [DataMember]
        public double? ProjectOfferAmount
        {
            get
            {
                return projectOfferAmount;
            }

            set
            {
                projectOfferAmount = value;
            }
        }

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
            }
        }
        #endregion
    }
}
