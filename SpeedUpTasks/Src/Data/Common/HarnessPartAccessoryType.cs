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
    [Table("harness_part_accessory_types")]
    [DataContract]
    public class HarnessPartAccessoryType
    {
        #region Fields
        Byte idHarnessPartAccessoryType;
        string name;
        string description;
        #endregion

        #region Properties
        [Key]
        [Column("IdHarnessPartAccessoryType")]
        [DataMember]
        public Byte IdHarnessPartAccessoryType
        {
            get { return idHarnessPartAccessoryType; }
            set { idHarnessPartAccessoryType = value; }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("Description")]
        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

         #endregion
    }
}
