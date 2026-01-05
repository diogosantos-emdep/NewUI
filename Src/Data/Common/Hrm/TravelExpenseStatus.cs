using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
   public class TravelExpenseStatus : ModelBase, IDisposable
    {
        #region Fields
        bool isEnable = true;
        byte idWorkflowStatus;
        UInt32 idWorkflow;
        string name;
        string htmlColor;
        UInt32 idWorkflowScope;

        #endregion

        #region Constructor

        public TravelExpenseStatus()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public byte IdWorkflowStatus
        {
            get
            {
                return idWorkflowStatus;
            }

            set
            {
                idWorkflowStatus = value;
                OnPropertyChanged("IdWorkflowStatus");
            }
        }

        [DataMember]
        public uint IdWorkflow
        {
            get
            {
                return idWorkflow;
            }

            set
            {
                idWorkflow = value;
                OnPropertyChanged("IdWorkflow");
            }
        }

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

        [DataMember]
        public string HtmlColor
        {
            get
            {
                return htmlColor;
            }

            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }

        [DataMember]
        public uint IdWorkflowScope
        {
            get
            {
                return idWorkflowScope;
            }

            set
            {
                idWorkflowScope = value;
                OnPropertyChanged("IdWorkflowScope");
            }
        }

        [DataMember]
        public bool IsEnable
        {
            get
            {
                return isEnable;
            }

            set
            {
                isEnable = value;
                OnPropertyChanged("IsEnable");
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
