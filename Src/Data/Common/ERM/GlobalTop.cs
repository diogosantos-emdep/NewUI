using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
   public  class GlobalTop : ModelBase, IDisposable
    {
        #region Fields
        private Int32 idtop;
        private int top;
        #endregion
        #region Properties 

        [DataMember]
        public Int32 IdTop
        {
            get { return idtop; }
            set
            {
                idtop = value;
                OnPropertyChanged("Idtop");
            }
        }
        public int Top
        {
            get { return top; }
            set
            {
                top = value;
                OnPropertyChanged("Top");
            }
        }
        #endregion

        #region Constructor
        public GlobalTop()
        {

        }
        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }
      
    }
}
