using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class DomainUser : ModelBase, IDisposable
    {
        #region Fields

        string firstName;
        string lastName;
        string displayName;
        string email;
        string company;//[Sudhir.Jangra][GEOS2-3418]
        #endregion

        #region Properties

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        //[Sudhir.Jangra][GEOS2-3418]
        public string Company
        {
            get { return company; }
            set { company = value; }
        }
        #endregion

        #region Constructor

        public DomainUser()
        {
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
