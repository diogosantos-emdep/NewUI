using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{//[Sudhir.Jangra][GEOS2-4441][21/09/2023]
    [DataContract]
    public class HardLockPlugins : ModelBase, IDisposable
    {
        #region Declaration
        UInt32 idPlugin;
        string name;
        bool isCurrentPlugin;
        #endregion

        #region Properties
        [DataMember]
        public UInt32 IdPlugin
        {
            get { return idPlugin; }
            set
            {
                idPlugin = value;
                OnPropertyChanged("IdPlugin");
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

        [DataMember]
        public bool IsCurrentPlugin
        {
            get { return isCurrentPlugin; }
            set
            {
                isCurrentPlugin = value;
                OnPropertyChanged("IsCurrentPlugin");
            }
        }
        #endregion

        #region Constructor
        public HardLockPlugins()
        {

        }
        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

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
