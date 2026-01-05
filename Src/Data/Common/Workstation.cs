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
     [Table("workstations")]
     [DataContract(IsReference = true)]
    public class Workstation
    {
        #region Fields
        Int32 idWorkStation;
         Byte? idStage;
         Int32? number;
         string _IP;
         SByte? isManufacturingStation;
        #endregion

        #region Properties
         [Key]
        [Column("IdWorkStation")]
        [DataMember]
         public Int32 IdWorkStation
        {
            get { return idWorkStation; }
            set{idWorkStation=value;}
        }

         [Column("IdStage")]
         [ForeignKey("Stage")]
        [DataMember]
        public Byte? IdStage
        {
            get { return idStage; }
            set { idStage = value; }
        }

        [Column("Number")]
        [DataMember]
        public Int32? Number
        {
            get { return number; }
            set { number = value; }
        }

        [Column("IP")]
        [DataMember]
        public string IP
        {
            get { return _IP; }
            set { _IP = value; }
        }

        [Column("IsManufacturingStation")]
        [DataMember]
        public SByte? IsManufacturingStation
        {
            get { return isManufacturingStation; }
            set {isManufacturingStation = value; }
        }
        
        [DataMember]
        public virtual Stage Stage { get; set; }
         #endregion
    }
}
