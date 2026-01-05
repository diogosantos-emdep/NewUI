using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common.HarnessPart
{
    [Table("colors")]
    [DataContract]
    public class Color
    {
        #region Fields
        Byte? idColor;
        string name;
        Int16? sortOrder;
        string htmlColor;
        #endregion

         #region Constructor
        public Color()
        {
            this.HarnessPartAccessories = new List<HarnessPartAccessory>();
            this.HarnessParts = new List<HarnessPart>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdColor")]
        [DataMember]
        public Byte? IdColor
        {
            get { return idColor; }
            set { idColor = value; }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("SortOrder")]
        [DataMember]
        public Int16? SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        [Column("HtmlColor")]
        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set { htmlColor = value; }
        }

        [DataMember]
        public virtual ICollection<HarnessPartAccessory> HarnessPartAccessories { get; set; }

        [DataMember]
        public virtual ICollection<HarnessPart> HarnessParts { get; set; }
        #endregion
    }
}
