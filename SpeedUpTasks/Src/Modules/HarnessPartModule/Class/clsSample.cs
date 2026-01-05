using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
  public  class clsSample
    {
        private int qty;
        private string location;
        private string locationIn;

        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        public string LocationIn
        {
            get { return locationIn; }
            set { locationIn = value; }
        }
        
    }
}
