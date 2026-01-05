using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Crm.Emdep.Geos.Data.Common
{
    public class TileBarItemsHelper
    {

        #region Declaration

        public string Caption { get; set; }
        public ObservableCollection<TileBarItemsHelper> Children { get; set; }
        public ICommand NavigateCommand { get; set; }
        public bool IsHasChildren { get { return Children != null && Children.Count != 0; } }
        public String GlyphUri { get; set; }
        public String Group { get; set; }
        public string BackColor { get; set; }

        #endregion
        
        #region Constructor

        public TileBarItemsHelper()
        {
            Caption = String.Empty;
            Children = new ObservableCollection<TileBarItemsHelper>();
            NavigateCommand = null;
            GlyphUri = String.Empty;
            Group = String.Empty;
        }

        #endregion
    }
}
