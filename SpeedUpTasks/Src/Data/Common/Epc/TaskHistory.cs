using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("task_histories")]
    [DataContract]
    public class TaskHistory : ModelBase,IDisposable
    {
        #region  Fields
        Int64 idHistory;
        #endregion

        #region Properties
        [Key]
        [Column("IdHistory")]
        [DataMember]
        public Int64 IdHistory
        {
            get
            {
                return idHistory;
            }

            set
            {
                idHistory = value;
                OnPropertyChanged("IdHistory");
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
