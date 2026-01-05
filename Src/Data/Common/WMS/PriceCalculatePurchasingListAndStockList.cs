using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{
	
	//[nsatpute][08.09.2025][GEOS2-9210]
    public class PriceCalculatePurchasingListAndStockList
    {
        int quantity;
        double unitPrice;
        double discount;
        double? exchangeRate;

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        public double UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }
        public double Discount
        {
            get { return discount; }
            set { discount = value; }
        }
        public double? ExchangeRate
        {
            get { return exchangeRate; }
            set { exchangeRate = value; }
        }

        public double CostPriceWithoutIVA()
        {
            return (UnitPrice * ((100 - Discount) / 100));
        }
    }
}
