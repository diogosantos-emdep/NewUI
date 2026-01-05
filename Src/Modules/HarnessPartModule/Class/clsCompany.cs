using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
    public class clsCompany 
    {
        private string _IdCustomer;

        public string IdCustomer
        {
            get { return _IdCustomer; }
            set
            {
                _IdCustomer = value;
                //RaisePropertyChanged(_IdCustomer);
            }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                //RaisePropertyChanged(_name);
            }


        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //public void RaisePropertyChanged(string PropertyName)
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        //}
    }
}
