using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("contract_situations")]
    [DataContract]
    public class ContractSituation : ModelBase, IDisposable
    {

        #region Fields

        UInt16 idContractSituation;
        string name;
        DateTime? hireDate;
        UInt64 employeeCount;

        #endregion

        #region Properties

        [Key]
        [Column("IdContractSituation")]
        [DataMember]
        public ushort IdContractSituation
        {
            get { return idContractSituation; }
            set
            {
                idContractSituation = value;
                OnPropertyChanged("IdContractSituation");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }


        [NotMapped]
        [DataMember]
        public DateTime? HireDate
        {
            get { return hireDate; }
            set
            {
                hireDate = value;
                OnPropertyChanged("HireDate");
            }
        }

        [NotMapped]
        [DataMember]
        public ulong EmployeeCount
        {
            get { return employeeCount; }
            set
            {
                employeeCount = value;
                OnPropertyChanged("EmployeeCount");
            }
        }

        #endregion

        #region Constructor

        public ContractSituation()
        {
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
