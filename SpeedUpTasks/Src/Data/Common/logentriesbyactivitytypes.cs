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
    [Table("log_entries_by_activity_types")]
    [DataContract]    
   public class LogEntriesByActivityTypes : ModelBase, IDisposable
    {
        #region Fields
        Int64 idLogType;
        string name;
        string htmlColor;
        List<LogEntryByOffer> logEntryByOffers;
        #endregion

        #region Constructor
        public LogEntriesByActivityTypes()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("idLogType")]
        [DataMember]
        public Int64 IdLogType
        {
            get { return idLogType; }
            set { idLogType = value;
                OnPropertyChanged("IdLogType");
            }
        }

        [Column("name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("HtmlColor")]
        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set { htmlColor = value;
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
