using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class UserSettings
    {
        #region Fields
        string _value;
        string name;
        #endregion

        #region Properties
       

        [NotMapped]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [NotMapped]
        [DataMember]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        #endregion
    }
}
