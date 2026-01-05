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
    [Table("job_descriptions")]
    [DataContract]
    public class JobDescription
    {
        #region Fields
        Int32 idJobDescription;
        string name;
        Int32? idParent;
        string code;
        #endregion

        #region Constructor
        public JobDescription()
        {
            this.Users = new HashSet<User>();
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdJobDescription")]
        [DataMember]
        public Int32 IdJobDescription
        {
            get { return idJobDescription; }
            set { idJobDescription = value; }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("IdParent")]
        [DataMember]
        public Int32? IdParent
        {
            get { return idParent; }
            set { idParent = value; }
        }

        [Column("Code")]
        [DataMember]
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        [DataMember]
        public virtual ICollection<User> Users { get; set; }
        #endregion
    }
}
