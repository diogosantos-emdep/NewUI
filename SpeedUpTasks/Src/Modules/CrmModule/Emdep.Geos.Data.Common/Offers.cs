using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Crm.Emdep.Geos.Data.Common
{
    public class Offers
    {
        private int srn;
        private string ot;
        private string group;
        private string plants; 
        private string title; 
        private string amount;
        private string dateofOffer;

        public string Ot
        {
            get { return ot; }
            set { ot = value; }
        }
        public string Group
        {
            get { return group; }
            set { group = value; }
        }
        public string Plants
        {
            get { return plants; }
            set { plants = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }        
        public string DateofOffer
        {
            get { return dateofOffer; }
            set { dateofOffer = value; }
        }

        public int Srn
        {
            get
            {
                return srn;
            }

            set
            {
                srn = value;
            }
        }
    }
}
