using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{
    // [nsatpute][07-04-2025][GEOS2-7015]
    public class ArticleStockData
    {
        public string StockMonth { get; set; }
        public int IdArticle { get; set; }
        public double ArticlePriceInEuro { get; set; }
        public double ClosingStock { get; set; }
        public int MonthNumber { get; set; }
        public string Reference { get; set; }
        //[nsatpute][05.09.2025][GEOS2-9210]
        public int Warehouse { get; set; }
        public int RegularizationPointId { get; set; }
		//[nsatpute][08.09.2025][GEOS2-9210]
        public double Discount { get; set; }
        public int RegularisationPoint { get; set; }
        public int Stock { get; set; }

        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        public double Price { get; set; }

        public double Value { get; set; }

        public int stock { get; set; }

    }
}
