using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{
    [Table("harness_part_types")]
    [DataContract]
    public class HarnessPartType
    {
        #region Fields
        Int16 idHarnessPartType;
        string name;
        Int16? idParent;
        Int16? sortOrder;
        #endregion

        #region Properties
        [Key]
        [Column("IdHarnessPartType")]
        [DataMember]
        public Int16 IdHarnessPartType
        {
            get { return idHarnessPartType; }
            set { idHarnessPartType = value; }
        }

        [Column("Name")]
        [DataMember]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("IdParent")]
        [DataMember]
        public Int16? IdParent
        {
            get { return idParent; }
            set { idParent = value; }
        }

        [Column("SortOrder")]
        [DataMember]
        public Int16? SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }
        #endregion
    }
}
