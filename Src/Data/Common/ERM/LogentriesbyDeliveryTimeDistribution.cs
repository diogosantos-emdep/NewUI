using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [Table("log_entriesby_Delivery_Time_Distribution")]
    [DataContract]
    public class LogentriesbyDeliveryTimeDistribution : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idLogEntryByDTD;
        UInt64 idDeliveryTimeDistribution;
        DateTime datetime;
        Int32 idUser;
        string comments;
        User changeLogUser;

        #endregion

        #region Constructor
        public LogentriesbyDeliveryTimeDistribution()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdLogEntryByDTD")]
        [DataMember]
        public ulong IdLogEntryByDTD
        {
            get { return idLogEntryByDTD; }
            set
            {
                idLogEntryByDTD = value;
                OnPropertyChanged("IdLogEntryByDTD");
            }
        }

        [Column("IdDeliveryTimeDistribution")]
        [DataMember]
        public ulong IdDeliveryTimeDistribution
        {
            get { return idDeliveryTimeDistribution; }
            set
            {
                idDeliveryTimeDistribution = value;
                OnPropertyChanged("IdDeliveryTimeDistribution");
            }
        }

        [Column("Datetime")]
        [DataMember]
        public DateTime Datetime
        {
            get { return datetime; }
            set
            {
                datetime = value;
                OnPropertyChanged("Datetime");
            }
        }

        [Column("IdUser")]
        [DataMember]
        public int IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }

        [Column("Comments")]
        [DataMember]
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        [NotMapped]
        [DataMember]
        public User ChangeLogUser
        {
            get { return changeLogUser; }
            set
            {
                changeLogUser = value;
                OnPropertyChanged("ChangeLogUser");
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
