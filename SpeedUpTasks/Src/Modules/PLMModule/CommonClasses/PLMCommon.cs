using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.PLM.CommonClasses
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PLMCommon : Prism.Mvvm.BindableBase
    {
        #region  Declaration

        private static readonly PLMCommon instance = new PLMCommon();

        #endregion

        #region Public Properties

        public static PLMCommon Instance
        {
            get { return instance; }
        }

        #endregion

        #region Constructor

        public PLMCommon()
        {
        }

        #endregion
    }
}
