using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMSparePartsGroups : ModelBase, IDisposable
    {
        #region Fields
        private Int32 idGroup;
        private Int32 idDetectionType;
        string keyName;
        string nameWithCount;
        private Int32 idworkOperationByStage;
        private Int32 idSequence;
        private string name;
        private Int32 idStatus;
        private string status;
        private string code;
        private string parent;
        UInt64? idParent;
        string parentName;
        Int32 position;
        #endregion
        #region Properties 
        [DataMember]
        public Int32 IdGroup
        {
            get { return idGroup; }
            set
            {
                idGroup = value;
                OnPropertyChanged("IdGroup");
            }
        }
        [DataMember]
        public Int32 IdDetectionType
        {
            get { return idDetectionType; }
            set
            {
                idDetectionType = value;
                OnPropertyChanged("IdDetectionType");
            }
        }

        [DataMember]
        public string KeyName
        {
            get
            {
                return keyName;
            }

            set
            {
                keyName = value;
                OnPropertyChanged("KeyName");
            }
        }

        [DataMember]
        public string NameWithDetectionCount
        {
            get
            {
                return nameWithCount;
            }

            set
            {
                nameWithCount = value;
                OnPropertyChanged("NameWithDetectionCount");
            }
        }
        [DataMember]
        public Int32 Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

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
        #endregion
        #region Constructor

        #endregion


        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
