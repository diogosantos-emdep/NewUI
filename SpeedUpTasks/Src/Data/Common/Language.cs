using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common
{
    [Table("languages")]
    [DataContract]
    public class Language
    {
        #region Fields
        Int32 idLanguage;
        string twoLetterISOLanguage;
        string name;
        string cultureName;
        ImageSource languageImage;
        #endregion

        #region Constructor
        public Language()
        {
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdLanguage")]
        [DataMember]
        public Int32 IdLanguage
        {
            get { return idLanguage; }
            set { idLanguage = value; }
        }

        [Column("TwoLetterISOLanguage")]
        [DataMember]
        public string TwoLetterISOLanguage
        {
            get { return twoLetterISOLanguage; }
            set { twoLetterISOLanguage = value; }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("CultureName")]
        [DataMember]
        public string CultureName
        {
            get { return cultureName; }
            set { cultureName = value; }
        }

        [NotMapped]
        [DataMember]
        public ImageSource LanguageImage
        {
            get { return languageImage; }
            set { languageImage = value; }
        }
        #endregion
    }
}
