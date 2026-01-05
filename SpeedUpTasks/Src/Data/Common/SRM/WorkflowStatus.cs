using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
    [DataContract]
   public class WorkflowStatus: ModelBase, IDisposable
    {
        #region Fields

        byte idWorkflowStatus;
        UInt32 idWorkflow;
        string name;
        string htmlColor;
        Int32 idCreator;
        DateTime creationDate;
        Int32? idModifier;
        DateTime? modificationDate;
        UInt32 idWorkflowScope;

        #endregion

        #region Constructor

        public WorkflowStatus()
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
