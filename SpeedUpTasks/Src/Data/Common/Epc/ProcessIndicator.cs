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
    public class ProcessIndicator : ModelBase,IDisposable
    {
        #region  Fields
        Int32 idProcessIndicator;
        string processIndicatorCode;
        string processIndicatorName;
        decimal targetValue;
        decimal format;
        #endregion

        #region Properties

        [Key]
        [Column("IdProcessIndicator")]
        [DataMember]
        public Int32 IdProcessIndicator
        {
            get
            {
                return idProcessIndicator;
            }
            set
            {
                this.idProcessIndicator = value;
                OnPropertyChanged("IdProcessIndicator");
            }
        }

        [Column("ProcessIndicatorCode")]
        [DataMember]
        public string ProcessIndicatorCode
        {
            get
            {
                return processIndicatorCode;
            }
            set
            {
                processIndicatorCode = value;
                OnPropertyChanged("ProcessIndicatorCode");
            }
        }

        [Column("ProcessIndicatorName")]
        [DataMember]
        public string ProcessIndicatorName
        {
            get
            {
                return processIndicatorName;
            }
            set
            {
                processIndicatorName = value;
                OnPropertyChanged("ProcessIndicatorName");
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

        [Column("Format")]
        [DataMember]
        public decimal Format
        {
            get
            {
                return format;
            }
            set
            {
                format = value;
                OnPropertyChanged("Format");
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
