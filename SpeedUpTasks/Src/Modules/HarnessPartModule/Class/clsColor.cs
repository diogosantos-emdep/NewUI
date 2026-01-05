using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
  public  class clsColor
    {
        private int Id;
        private string htmlCode;
        private string name;
        
        public int ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public string HtmlCode
        {
            get { return htmlCode; }
            set { htmlCode = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }
}
