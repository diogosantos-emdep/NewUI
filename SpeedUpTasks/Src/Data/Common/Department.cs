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
    [Table("emdep_hrm.departments")]
    [DataContract]
    public class Department
    {
        #region Fields
        Int32 idDepartment;
        string name;
        #endregion

         #region Constructor
        public Department()
        {
            this.Users = new HashSet<User>();
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdDepartment")]
        [DataMember]
        public Int32 IdDepartment
        {
            get { return idDepartment; }
            set { idDepartment = value; }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public virtual ICollection<User> Users { get; set; }
        #endregion
    }
}
