using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
    class clsFamilies
    {
        private string reference;
        private ImageSource imageConnector;
       
        private int cavities;
        private bool connector;
        private bool component;
        private bool terminal;

        private string colorName;

        public string ColorName
        {
            get { return colorName; }
            set { colorName = value; }
        }

        public string Reference
        {
            get { return reference; }
            set { reference = value; }
        }


        public ImageSource ImageConnector
        {
            get { return imageConnector; }
            set { imageConnector = value; }
        }

        public int Cavities
        {
            get { return cavities; }
            set { cavities = value; }
        }

        

        public bool Connector
        {
            get { return connector; }
            set { connector = value; }
        }


        public bool Component
        {
            get { return component; }
            set { component = value; }
        }


        public bool Terminal
        {
            get { return terminal; }
            set { terminal = value; }
        }
    }
}
