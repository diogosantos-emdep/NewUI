using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.SRM.CommonClasses
{
    /// <summary>
    /// [001][skhade][2020-02-24][GEOS2-1799] New module SRM - 1.
    /// </summary>
    public sealed class SRMCommon : Prism.Mvvm.BindableBase
    {
        #region  Declaration

        private static readonly SRMCommon instance = new SRMCommon();

        #endregion

        #region Public Properties

        public static SRMCommon Instance
        {
            get { return instance; }
        }

        #endregion

        #region Constructor

        public SRMCommon()
        {
        }

        #endregion
    }
}
