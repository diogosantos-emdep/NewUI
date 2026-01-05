using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class OtItemSameReference
    {
        Int64 idArticle;
        Int64 idWarehouseDeliveryNoteItem;
        Int64 qty;
        Int64 articleCurrentStock;
        Int64 idWarehouseLocation;


        public Int64 IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
             
            }
        }

        public Int64 IdWarehouseDeliveryNoteItem
        {
            get { return idWarehouseDeliveryNoteItem; }
            set
            {
                idWarehouseDeliveryNoteItem = value;
             
            }
        }


        public Int64 Qty
        {
            get { return qty; }
            set
            {
                qty = value;

            }
        }


        public Int64 ArticleCurrentStock
        {
            get { return articleCurrentStock; }
            set
            {
                articleCurrentStock = value;

            }
        }
        public Int64 IdWarehouseLocation
        {
            get { return idWarehouseLocation; }
            set
            {
                idWarehouseLocation = value;

            }
        }
    }
}
