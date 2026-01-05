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
    [Table("log_entry_types")]
    [DataContract]
    public class LogEntryType
    {
        #region Fields
        Byte idLogEntryType;
        string name;
        string htmlColor;
        #endregion

        #region Constructor
        public LogEntryType()
        {
            this.UserLogs = new List<UserLog>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdLogEntryType")]
        [DataMember]
        public Byte IdLogEntryType
        {
            get { return idLogEntryType; }
            set { idLogEntryType = value; }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("HtmlColor")]
        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set { htmlColor = value; }
        }

        [DataMember]
        public virtual List<UserLog> UserLogs { get; set; }
        #endregion
    }
}