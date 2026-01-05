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
    [Table("logentriesbyoffertypes")]
    [DataContract]
    public class LogEntryByOfferType
    {

        #region Fields
        Int64 idLogType;
        string name;
        string htmlColor;
        List<LogEntryByOffer> logEntryByOffers;
        #endregion

        #region Constructor
        public LogEntryByOfferType()
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
            set { idLogType = value; }
        }

        [Column("name")]
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

        [NotMapped]
        [DataMember]
        public  virtual List<LogEntryByOffer> LogEntryByOffers
        {
            get { return logEntryByOffers; }
            set { logEntryByOffers = value; }
        }
        #endregion
    }
}
