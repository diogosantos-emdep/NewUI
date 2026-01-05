using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.ERM
{
    public class RTMHRResourcesCurrentStage : ModelBase, IDisposable
    {
        #region Field
        private int? idStage;
        private decimal? real;
        private decimal? expected;
 
        #endregion
        #region Property
        [DataMember]
        public decimal? Real
        {
            get
            {
                return real;
            }

            set
            {
                real = value;
                OnPropertyChanged("Real");
            }
        }
        public decimal? Expected
        {
            get
            {
                return expected;
            }

            set
            {
                expected = value;
                OnPropertyChanged("Expected");
            }
        }
      

        public int? IdStage
        {
            get
            {
                return idStage;
            }

            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
