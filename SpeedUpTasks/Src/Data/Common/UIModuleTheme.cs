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
    [Table("ui_module_themes")]
    [DataContract(IsReference = true)]
    public class UIModuleTheme
    {
         #region Fields
         Int32 id;
         Byte? idGeosModule;
         Int32? idTheme;
         string foreColor;
         string backColor;
         #endregion

         #region Constructor
        public UIModuleTheme()
        {
         
        }
        #endregion

        #region Properties
        [Key]
        [Column("Id")]
        [DataMember]
        public Int32 Id
        {
            get { return id; }
            set { id = value; }
        }

        [Column("IdGeosModule")]
        [ForeignKey("GeosModule")]
        [DataMember]
        public Byte? IdGeosModule
        {
            get { return idGeosModule; }
            set { idGeosModule = value; }
        }

        [Column("IdTheme")]
        [ForeignKey("UITheme")]
        [DataMember]
        public Int32? IdTheme
        {
            get { return idTheme; }
            set { idTheme = value; }
        }

        [Column("ForeColor")]
        [DataMember]
        public string ForeColor
        {
            get { return foreColor; }
            set { foreColor = value; }
        }

        [Column("BackColor")]
        [DataMember]
        public string BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        [DataMember]
        public virtual GeosModule GeosModule { get; set; }

        [DataMember]
        public virtual UITheme UITheme { get; set; }
        #endregion
    }
}
