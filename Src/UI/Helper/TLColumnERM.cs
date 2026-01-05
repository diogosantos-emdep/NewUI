#region Assembly Emdep.Geos.UI.Helper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;

namespace Emdep.Geos.UI.Helper
{
   public class TLColumnERM
    {
        public string UnboundType { get; set; }
        public SettingsTreeListColumnERM Settings { get; set; }
        public bool AllowBestFit { get; set; }
        public bool AllowCellMerge { get; set; }
        public bool AllowEditing { get; set; }
        public double BestFitWidth { get; set; }
        public string FieldName { get; set; }
        public bool FixedWidth { get; set; }
        public string HeaderText { get; set; }
        public Image Image { get; set; }
        public int ImageIndex { get; set; }
        public ImageSource ImageSource { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsStatus { get; set; }
        public bool IsVertical { get; set; }
        public bool Visible { get; set; }
        public double Width { get; set; }
    }
}
