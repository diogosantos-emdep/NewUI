using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Helper
{
    public class Column
    {
        public string FieldName { get; set; }


        public SettingsType Settings { get; set; }
        public bool AllowCellMerge { get; set; }
        public double Width { get; set; }
        public bool AllowEditing { get; set; }

        public double BestFitWidth { get; set; }
        public bool AllowBestFit { get; set; }
        public bool FixedWidth { get; set; }

        public string HeaderText { get; set; }
        public bool Visible { get; set; }
        public bool IsVertical { get; set; }
        public bool IsStatus { get; set; }
        public int ImageIndex { get; set; }
        public System.Drawing.Image Image { get; set; }
        public System.Windows.Media.ImageSource ImageSource { get; set; }

        public bool IsReadOnly { get; set; }

        // public string LastUpdate { get; set; }
        //public Emdep.Geos.UI.Helper.CustomCellValue Current { get; set; }
        public Column()
        {
            Width = 50;
            // BestFitWidth = 50;
            AllowBestFit = true;
            AllowEditing = false;
            FixedWidth = false;
            Visible = false;
            IsStatus = false;
            ImageIndex = 0;
            Image = null;
            ImageSource = null;
            
            // LastUpdate = null;
            //Current = null;

        }

    }
}
