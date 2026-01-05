using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Emdep.Geos.UI.Helper
{
    public class ColumnItem
    {
        public string ColumnFieldName{get;set;}
        public string Binding{get;set;}
        public string HeaderText { get; set; }
        public double Width { get; set; }
        public string Tag { get; set; }
        public SettingsType Settings { get; set; }
        public ProductTypeSettingsType ProductTypeSettings { get; set; }
        public bool Visible { get; set; }
        public bool IsVertical { get; set; }


    }
  
}
