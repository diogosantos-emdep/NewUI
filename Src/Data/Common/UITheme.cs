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
    [Table("ui_themes")]
    public class UITheme
    {
        #region Fields
        Int32 idTheme;
        string themeName;
        string fontFamily;
        #endregion

        #region Constructor
        public UITheme()
        {
            this.UIModuleThemes = new List<UIModuleTheme>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdTheme")]
        [DataMember]
        public Int32 IdTheme
        {
            get { return idTheme; }
            set { idTheme = value; }
        }

        [Column("ThemeName")]
        [DataMember]
        public string ThemeName
        {
            get { return themeName; }
            set { themeName = value; }
        }

        [Column("FontFamily")]
        [DataMember]
        public string FontFamily
        {
            get { return fontFamily; }
            set { fontFamily = value; }
        }

        [DataMember]
        public virtual List<UIModuleTheme> UIModuleThemes { get; set; }
        #endregion
    }
}
