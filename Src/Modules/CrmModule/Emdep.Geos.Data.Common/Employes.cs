using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.Emdep.Geos.Data.Common
{
    public class Employes
    {
        private string firstName;
        private string lastName;
        private string emailid;
        private string position;
        private ImageSource photo;
        private string site;

        public string Site
        {
            get { return site; }
            set { site = value; }
        }
        public ImageSource Photo
        {
            get { return photo; }
            set { photo = value; }
        }

        public string Position
        {
            get { return position; }
            set { position = value; }
        }


        public string Emailid
        {
            get { return emailid; }
            set { emailid = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        
    }
}
