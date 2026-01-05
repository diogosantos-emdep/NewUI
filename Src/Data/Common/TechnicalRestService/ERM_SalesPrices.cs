using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    //[rani dhamankar][APIGEOS-1317][28-01-2025][Created New Class https://helpdesk.emdep.com/browse/APIGEOS-1317]
    [DataContract]
    public class ERM_SalesPrices
    {
        private int _IdCurrency = 0;
        [DataMember(Order = 1)]
        public int IdCurrency
        {
            get { return _IdCurrency; }
            set { _IdCurrency = value; }
        }
        private string _Currency = string.Empty;
        [DataMember(Order = 2)]
        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }
        private Double _UnitSalePrice = 0;
        [DataMember(Order = 3)]
        public Double UnitSalePrice
        {
            get { return _UnitSalePrice; }
            set { _UnitSalePrice = value; }
        }
        private Double _TotalSalePrice = 0;
        [DataMember(Order = 4)]
        public Double TotalSalePrice
        {
            get { return _TotalSalePrice; }
            set { _TotalSalePrice = value; }
        }




    }
}
