using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OptimizedClass
{
    [DataContract]
   public class CompanyDetail
    {
        #region Fields

        Int32 idCompany;
        string name;
        string siteNameWithoutCountry;
        string countryName;
        string customerName;
        List<SalesStatusTypeDetail> salesStatusTypeDetail;
     
        #endregion

        #region Properties

        [DataMember]
        public Int32 IdCompany
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
        public string SiteNameWithoutCountry
        {
            get
            {
                return siteNameWithoutCountry;
            }

            set
            {
                siteNameWithoutCountry = value;
            }
        }

        [DataMember]
        public string CountryName
        {
            get
            {
                return  countryName;
            }

            set
            {
                countryName = value;
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
        public List<SalesStatusTypeDetail> SalesStatusTypeDetail
        {
            get
            {
                return salesStatusTypeDetail;
            }

            set
            {
                salesStatusTypeDetail = value;
            }
        }

       



        #endregion
    }
}
