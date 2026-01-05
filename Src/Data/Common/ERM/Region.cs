using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class Region: ModelBase, IDisposable
    {
        #region Field
        private int idLookupValue;
        private string regionName;
        #endregion
        #region Property

        [DataMember]
        public int IdLookupValue
        {
            get
            {
                return idLookupValue;
            }

            set
            {
                idLookupValue = value;
                OnPropertyChanged("IdLookupValue");
            }
        }
        [DataMember]
        public string RegionName
        {
            get
            {
                return regionName;
            }

            set
            {
                regionName = value;
                OnPropertyChanged("RegionName");
            }
        }

        #endregion
        #region Constructor
        public Region()
        {

        }
        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }


    }
}
