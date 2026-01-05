using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PLM
{
    [DataContract]
    public class Region : ModelBase, IDisposable
    {
        #region Fields
        Int32 idRegion;
        string regionName;
        #endregion

        #region Constructor
        public Region()
        {
        }
        #endregion

        #region Properties
        [DataMember]
        public Int32 IdRegion
        {
            get { return idRegion; }
            set
            {
                idRegion = value;
                OnPropertyChanged("IdRegion");
            }
        }

        [DataMember]
        public string RegionName
        {
            get { return regionName; }
            set
            {
                regionName = value;
                OnPropertyChanged("RegionName");
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
            Region region = (Region)this.MemberwiseClone();

            return region;
        }

        #endregion
    }
}
