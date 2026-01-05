using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
    public class clsBOM
    {
        string description;
        int qty;
        string reference;
        ImageSource imageLoad;
       

        public string Reference
        {
            get { return reference; }
            set { reference = value; }
        }

        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public ImageSource ImageLoad
        {
            get { return imageLoad; }
            set { imageLoad = value; }
        }

       
    }
}
