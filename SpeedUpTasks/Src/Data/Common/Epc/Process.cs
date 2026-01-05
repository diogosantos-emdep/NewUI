using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("process")]
    [DataContract]
    public class Process : ModelBase,IDisposable
    {
        #region  Fields
        Int32 idProcess;
        Int32 idProcessFamily;
        string processCode;
        string processName;
        ProcessFamily processFamily;
        #endregion

        #region Properties
        [Key]
        [Column("IdProcess")]
        [DataMember]
        public Int32 IdProcess
        {
            get
            {
                return idProcess;
            }

            set
            {
                idProcess = value;
                OnPropertyChanged("IdProcess");
            }
        }

        [Column("IdProcessFamily")]
        [ForeignKey("ProcessFamily")]
        [DataMember]
        public Int32 IdProcessFamily
        {
            get
            {
                return idProcessFamily;
            }
            set
            {
                idProcessFamily = value;
                OnPropertyChanged("IdProcessFamily");
            }
        }

        [Column("ProcessCode")]
        [DataMember]
        public string ProcessCode
        {
            get
            {
                return processCode;
            }
            set
            {
                processCode = value;
                OnPropertyChanged("ProcessCode");
            }
        }


        [Column("ProcessName")]
        [DataMember]
        public string ProcessName
        {
            get
            {
                return processName;
            }
            set
            {
                processName = value;
                OnPropertyChanged("processName");
            }
        }

        [DataMember]
        public virtual ProcessFamily ProcessFamily
        {
            get
            {
                return processFamily;
            }
            set
            {
                processFamily = value;
                OnPropertyChanged("ProcessFamily");
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
