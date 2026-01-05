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
     [Table("workstation_types")]
     [DataContract]
    public class WorkstationType
    {
        #region Fields
        Byte idWorkstationType;
         string image;
         SByte? isProduction;
        #endregion

        #region Constructor
         public WorkstationType()
        {
        }
         #endregion

        #region Properties
         [Key]
        [Column("IdWorkstationType")]
        [DataMember]
         public Byte IdWorkstationType
        {
            get { return idWorkstationType; }
            set { idWorkstationType = value; }
        }

        [Column("Image")]
        [DataMember]
        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        [Column("IsProduction")]
        [DataMember]
        public SByte? IsProduction
        {
            get { return isProduction; }
            set { isProduction = value; }
        }

         #endregion
    }
}
