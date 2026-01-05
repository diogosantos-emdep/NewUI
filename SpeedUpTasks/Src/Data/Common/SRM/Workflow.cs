using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
    [DataContract]
    public class Workflow : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idWorkflow;
        string name;
        UInt32 idWorkflowScope;
        Int32 idCreator;
        DateTime creationDate;
        Int32? idModifier;
        DateTime? modificationDate;

        #endregion

        #region Constructor

        public Workflow()
        {

        }

        #endregion

        #region Properties

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
