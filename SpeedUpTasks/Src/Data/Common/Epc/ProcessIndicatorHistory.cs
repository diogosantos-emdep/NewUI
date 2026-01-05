using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("process_indicator")]
    [DataContract]
    public class ProcessIndicatorHistory :ModelBase,IDisposable
    {
        #region  Fields
        Int32 idProcessIndicatorHistory;
        decimal targetValue;
        decimal currentValue;
        DateTime date;
        #endregion

        #region Properties

        [Key]
        [Column("IdProcessIndicatorHistory")]
        [DataMember]
        public Int32 IdProcessIndicatorHistory
        {
            get
            {
                return idProcessIndicatorHistory;
            }
            set
            {
                this.idProcessIndicatorHistory = value;
                OnPropertyChanged("IdProcessIndicatorHistory");
            }
        }

        [Column("Date")]
        [DataMember]
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        [Column("TargetValue")]
        [DataMember]
        public decimal TargetValue
        {
            get
            {
                return targetValue;
            }
            set
            {
                targetValue = value;
                OnPropertyChanged("TargetValue");
            }
        }

        [Column("CurrentValue")]
        [DataMember]
        public decimal CurrentValue
        {
            get
            {
                return currentValue;
            }
            set
            {
                currentValue = value;
                OnPropertyChanged("CurrentValue");
            }
        }

        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
