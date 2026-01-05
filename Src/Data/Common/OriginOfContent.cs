using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("originofcontent")]
    [DataContract]
    public class OriginOfContent : ModelBase, IDisposable
    {
        #region Fields
        Int32 idCountryGroup;
        string name;
        private string htmlColor;
        #endregion Fields

        #region Constructor
        public OriginOfContent()
        {

        }
        #endregion 

        #region Properties
        [Key]
        [Column("IdCountryGroup")]
        [DataMember]
        public Int32 IdCountryGroup
        {
            get { return idCountryGroup; }
            set
            {
                idCountryGroup = value;
                OnPropertyChanged("IdCountryGroup");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        [Column("HtmlColor")]
        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }
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
