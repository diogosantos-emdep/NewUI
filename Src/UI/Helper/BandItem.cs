using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Helper
{
    public class BandItem : GridControlBand
    {
        public string BandName { get; set; }

        public string BandHeader { get; set; }

        public FixedStyle FixedStyle { get; set; }

        public bool AllowBandMove { get; set; } = true;

        public bool OverlayHeaderByChildren { get; set; } = false;

        public ObservableCollection<ColumnItem> Columns { get; set; }

        public SettingsType Settings { get; set; }
    }
}
