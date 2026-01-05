using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [Table("complaintstatus")]
    [DataContract]
    public class ComplaintStatus : ModelBase, IDisposable
    {
        #region Fields
        byte idComplaintStatus;
        string name;
        
        #endregion

        #region Constructor
        public ComplaintStatus()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdComplaintStatus")]
        [DataMember]
        public byte IdComplaintStatus
        {
            get
            {
                return idComplaintStatus;
            }

            set
            {
                idComplaintStatus = value;
                OnPropertyChanged("IdComplaintStatus");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
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
