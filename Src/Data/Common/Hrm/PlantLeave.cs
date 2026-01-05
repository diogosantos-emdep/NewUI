using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common.SCM;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class PlantLeave : ModelBase, IDisposable
    {
        #region Fields
        Int32 idLeaveType;
        LookupValue leaveType;
        Company company;    
        #endregion


        #region Properties
        [DataMember]
        public Int32 IdLeaveType
        {
            get { return idLeaveType; }
            set
            {
                idLeaveType = value;
                OnPropertyChanged("IdLeaveType");
            }
        }

        [DataMember]
        public LookupValue LeaveType
        {
            get { return leaveType; }
            set
            {
                leaveType = value;
                OnPropertyChanged("LeaveType");
            }
        }

        [DataMember]
        public Company Company
        {
            get { return company; }
            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }
        #endregion

        #region Constructor
        public PlantLeave()
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
