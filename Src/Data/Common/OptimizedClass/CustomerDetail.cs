using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OptimizedClass
{
    [DataContract]
    public class CustomerDetail
    {
        #region  Fields
        Int32 idCustomer;
        Int32? idCompany;
        string customerName;
        List<CompanyDetail> companies;
        List<CustomerSort> customerSort;
        #endregion
        #region Properties
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


        [DataMember]
        public Int32? IdCompany
        {
            get
            {
                return idCompany;
            }

            set
            {
                idCompany = value;
            }
        }

        [DataMember]
        public string CustomerName
        {
            get
            {
                return customerName;
            }

            set
            {
                customerName = value;
            }
        }

        [DataMember]
        public List<CompanyDetail> Companies
        {
            get
            {
                return companies;
            }

            set
            {
                companies = value;
            }
        }


        [DataMember]
        public List<CustomerSort> CustomerSort
        {
            get
            {
                return customerSort;
            }

            set
            {
                customerSort = value;
            }
        }

        #endregion
    }
}
