using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.SCM
{
    public class ConnectorWorkflowTransitions : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idWorkflowTransition;
        UInt32 idWorkflow;
        string name;
        byte idWorkflowStatusFrom;
        byte idWorkflowStatusTo;
        byte isCommentRequired;
        Int32 idCreator;
        DateTime creationDate;
        Int32? idModifier;
        DateTime? modificationDate;
        UInt32 idWorkflowScope;
        UInt32 isNotificationRaised;
        #endregion

        #region Constructor

        public ConnectorWorkflowTransitions()
        {

        }

        #endregion

        #region Properties
        [DataMember]
        public uint IdWorkflowTransition
        {
            get
            {
                return idWorkflowTransition;
            }

            set
            {
                idWorkflowTransition = value;
                OnPropertyChanged("IdWorkflowTransition");
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

        public byte IdWorkflowStatusFrom
        {
            get
            {
                return idWorkflowStatusFrom;
            }

            set
            {
                idWorkflowStatusFrom = value;
                OnPropertyChanged("IdWorkflowStatusFrom");
            }
        }

        [DataMember]

        public byte IdWorkflowStatusTo
        {
            get
            {
                return idWorkflowStatusTo;
            }

            set
            {
                idWorkflowStatusTo = value;
                OnPropertyChanged("IdWorkflowStatusTo");
            }
        }

        [DataMember]
        public byte IsCommentRequired
        {
            get
            {
                return isCommentRequired;
            }

            set
            {
                isCommentRequired = value;
                OnPropertyChanged("IsCommentRequired");
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
        public int IdCreator
        {
            get
            {
                return idCreator;
            }

            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public int? IdModifier
        {
            get
            {
                return idModifier;
            }

            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }
        }

        [DataMember]
        public DateTime? ModificationDate
        {
            get
            {
                return modificationDate;
            }

            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }
        [DataMember]
        public uint IsNotificationRaised
        {
            get
            {
                return isNotificationRaised;
            }

            set
            {
                isNotificationRaised = value;
                OnPropertyChanged("IsNotificationRaised");
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
