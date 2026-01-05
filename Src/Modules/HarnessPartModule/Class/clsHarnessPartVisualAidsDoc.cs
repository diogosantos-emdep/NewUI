using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
  public  class clsHarnessPartVisualAidsDoc
    {
      public Guid Id { get; set; }

        private bool _idHarnessPart;

        public bool IdHarnessPart
        {
            get { return _idHarnessPart; }
            set { _idHarnessPart = value; }
        }

        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
        private string categoryName;

        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

    }
}
