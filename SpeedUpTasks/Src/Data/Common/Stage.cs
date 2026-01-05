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
     [Table("stages")]
     [DataContract]
    public class Stage : ModelBase, IDisposable
    {
        #region Fields
        string name;
         Byte idStage;
         Byte sequence;
        #endregion

        #region Constructor
         public Stage()
        {
            this.Workstations = new List<Workstation>();
        }
         #endregion

        #region Properties

         [Column("Name")]
         [DataMember]
         public string Name
         {
             get { return name; }
             set { name = value; }
         }
         [Key]
        [Column("IdStage")]
        [DataMember]
        public Byte IdStage
        {
            get { return idStage; }
            set { idStage = value; }
        }

        [Column("Sequence")]
        [DataMember]
        public Byte Sequence
        {
            get { return sequence; }
            set { sequence = value; }
        }

        [DataMember]
        public virtual List<Workstation> Workstations { get; set; }
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
