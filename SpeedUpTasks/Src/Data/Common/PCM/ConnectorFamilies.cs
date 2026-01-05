using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract(IsReference = true)]
    public class ConnectorFamilies : ModelBase, IDisposable
    {
        #region Fields
        UInt64 idFamily;
        string name;
        #endregion

        #region Properties
        [DataMember]
        public UInt64 IdFamily
        {
            get { return idFamily; }
            set
            {
                idFamily = value;
                OnPropertyChanged("IdFamily");
            }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                this.name = value;
                OnPropertyChanged("Name");
            }
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
