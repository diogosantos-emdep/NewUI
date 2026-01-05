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
    [Table("log_entries_by_contact_types")]
    [DataContract]
    public class LogEntriesByContactType : ModelBase, IDisposable
    {
        #region Fields

        Int16 idLogType;
        string name;
        string htmlColor;

        #endregion

        #region Constructor

        public LogEntriesByContactType()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("idLogType")]
        [DataMember]
        public Int16 IdLogType
        {
            get { return idLogType; }
            set
            {
                idLogType = value;
                OnPropertyChanged("IdLogType");
            }
        }

        [Column("name")]
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
