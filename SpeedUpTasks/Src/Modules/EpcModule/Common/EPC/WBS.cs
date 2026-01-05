using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Epc.Common.EPC
{
    public class WBS
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Group { get; set; }
        private string resources;
        public string Resources
        {
            get { return resources; }
            set
            {
                if (resources == value)
                    return;
                resources = value;
                RaisePropertyChanged("Resources");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string fieldName)
        {
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(fieldName));
        }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string Hours { get; set; }
        public string MaterialUP { get; set; }
        public string Quantity { get; set; }
        public string Total { get; set; }


        public WBS()
        {
            Group = "NewTask";
            Resources = "";
            Reference = "";
            Description = "";
            Hours = "";
            MaterialUP = "";
            Quantity = "";
            Total = "";
        }
    }

}
