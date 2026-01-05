using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class DetectionGroupWithStructures : ModelBase, IDisposable
    {
        public string Name { get; set; }
        public List<ERMStructures> Structures { get; set; }

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
