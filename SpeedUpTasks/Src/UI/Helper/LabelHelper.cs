using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.UI.Helper
{
    public class LabelHelper
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public Brush Color { get; set; }
        public Color ColorA { get; set; }

    }

    public class StatusHelper
    {
        public int Id { get; set; }

        public string Caption { get; set; }

        public Brush Brush { get; set; }

        public Color ColorA { get; set; }

    }
}
