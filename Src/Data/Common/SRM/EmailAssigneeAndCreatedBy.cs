using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
    [DataContract]
    public class EmailAssigneeAndCreatedBy : ModelBase, IDisposable
    {
        #region Declaration
        string assigneeEmail;
        string createdBYEmail;//[[GEOS2-4077]
        #endregion

        #region Properties
        [DataMember]
        public string AssigneeEmail
        {
            get { return assigneeEmail; }
            set
            {
                assigneeEmail = value;
                OnPropertyChanged("AssigneeEmail");
            }
        }

        [DataMember]
        public string CreatedByEmail
        {
            get { return createdBYEmail; }
            set
            {
                createdBYEmail = value;
                OnPropertyChanged("CreatedByEmail");
            }
        }
        #endregion

        #region Constructor
        public EmailAssigneeAndCreatedBy()
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
