using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class DefaultWayType : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idDefaultWayType;
        string name;

        #endregion

        #region Constructor

        public DefaultWayType()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public uint IdDefaultWayType
        {
            get { return idDefaultWayType; }
            set
            {
                idDefaultWayType = value;
                OnPropertyChanged("IdDefaultWayType");
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

        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
