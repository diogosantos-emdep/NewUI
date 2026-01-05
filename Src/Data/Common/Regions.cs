using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class Regions : ModelBase, IDisposable
    {
        #region Fields
        Int32 idRegion;
        string regionName;
        #endregion

        #region Constructor
        public Regions()
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
            Regions region = (Regions)this.MemberwiseClone();

            return region;
        }

        #endregion
    }
}
