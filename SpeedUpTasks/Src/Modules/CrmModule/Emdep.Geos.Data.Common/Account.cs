using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.Emdep.Geos.Data.Common
{
    public class Account
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string country;

        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        private string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        private string phoneno;

        public string Phoneno
        {
            get { return phoneno; }
            set { phoneno = value; }
        }
        private string website;

        public string Website
        {
            get { return website; }
            set { website = value; }
        }
        private string parent;

        public string Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        private string contactEmail;

        public string ContactEmail
        {
            get { return contactEmail; }
            set { contactEmail = value; }
        }

        private string salesOwner;

        public string SalesOwner
        {
            get { return salesOwner; }
            set { salesOwner = value; }
        }
        private string salesTarget;

        public string SalesTarget
        {
            get { return salesTarget; }
            set { salesTarget = value; }
        }

        private ImageSource accountPhoto;
        public ImageSource AccountPhoto
        {
            get
            {
                return accountPhoto;
            }

            set
            {
                accountPhoto = value;
            }
        }

        private string fullName;
        private string email;
        public string FullName
        {
            get
            {
                return fullName;
            }

            set
            {
                fullName = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }
        private ObservableCollection<Contact> contact;

        public ObservableCollection<Contact> Contact
        {
            get { return contact; }
            set { contact = value; }
        }




    }

        public class Contact
    {
        string fullName;
        string email;
        string phoneNumber;

        public string FullName
        {
            get
            {
                return fullName;
            }

            set
            {
                fullName = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }

        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }

            set
            {
                phoneNumber = value;
            }
        }
    }

}
