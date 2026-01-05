using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Data.Common
{
	//[nsatpute][25.11.2025][GEOS2-9342]
    public class BusyCursor : IDisposable
    {
        public BusyCursor()
        {
            Mouse.OverrideCursor = Cursors.Wait;
        }

        public void Dispose()
        {
            Mouse.OverrideCursor = null;
        }
    }
}
