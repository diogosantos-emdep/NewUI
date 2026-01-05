using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{//[Sudhir.Jangra][GEOS2-5203]
    [DataContract]
    public class WorkflowStatus : ModelBase, IDisposable
    {
        #region Fileds
        private Int32 idWorkflowStatus;
        private string name;
        private string hTMLColor;
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public Int32 IdWorkflowStatus
        {
            get { return idWorkflowStatus; }
            set
            {
                idWorkflowStatus = value;
                OnPropertyChanged("IdWorkflowStatus");
            }
        }

        [NotMapped]
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
        public string HTMLColor
        {
            get { return hTMLColor; }
            set
            {
                hTMLColor = value;
                OnPropertyChanged("HTMLColor");
            }
        }
        #endregion

        #region Constructor
        public WorkflowStatus()
        {

        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
