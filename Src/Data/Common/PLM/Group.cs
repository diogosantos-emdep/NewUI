using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PLM
{
    [DataContract]
    public class Group : ModelBase, IDisposable
    {

        #region Fields
        Int32 idGroup;
        string groupName;
        #endregion

        #region Constructor
        public Group()
        {
        }
        #endregion

        #region Properties
        [DataMember]
        public Int32 IdGroup
        {
            get { return idGroup; }
            set { idGroup = value;
                OnPropertyChanged("IdGroup");
            }
        }

        [DataMember]
        public string GroupName
        {
            get { return groupName; }
            set { groupName = value;
                OnPropertyChanged("GroupName");
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
            Group group = (Group)this.MemberwiseClone();

            return group;
        }

        #endregion
    }
}
