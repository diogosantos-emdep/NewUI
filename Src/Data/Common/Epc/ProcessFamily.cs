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
    [Table("process_family")]
    [DataContract]
    public class ProcessFamily : ModelBase,IDisposable
    {
        #region  Fields
        Int32 idProcessFamily;
        string processFamilyName;
        IList<Process> processes;
        #endregion

        #region Constructor
        public ProcessFamily()
        {
            this.Processes = new List<Process>();
          
        }
        #endregion

        #region Properties
        
        [Key]
        [Column("IdProcessFamily")]
        [DataMember]
        public Int32 IdProcessFamily
        {
            get
            {
                return idProcessFamily;
            }
            set
            {
                this.idProcessFamily = value;
                OnPropertyChanged("IdProcessFamily");
            }
        }

        [Column("ProcessFamilyName")]
        [DataMember]
        public string ProcessFamilyName
        {
            get
            {
                return processFamilyName;
            }
            set
            {
                processFamilyName = value;
                OnPropertyChanged("ProcessFamilyName");
            }
        }

       
        [DataMember]
        public virtual IList<Process> Processes
        {
            get
            {
                return processes;
            }
            set
            {
                processes = value;
                OnPropertyChanged("Processes");
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
