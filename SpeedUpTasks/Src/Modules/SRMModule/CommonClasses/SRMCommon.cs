using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.SRM
{
    /// <summary>
    /// [001][skhade][2020-02-24][GEOS2-1799] New module SRM - 1.
    /// </summary>
    public sealed class SRMCommon : Prism.Mvvm.BindableBase
    {

        #region Declaration
        private static readonly SRMCommon instance = new SRMCommon();
        private List<Warehouses> warehouseList;
        private Warehouses selectedwarehouse;
        private int articleSleepDays;

        #endregion

        #region Public Properties

        public static SRMCommon Instance
        {
            get { return instance; }
        }
        public bool IsWarehouseChangedEventCanOccur { get; set; }
        public List<Warehouses> WarehouseList
        {
            get { return warehouseList; }
            set
            {
                warehouseList = value;
                this.OnPropertyChanged("WarehouseList");
            }
        }

        public Warehouses Selectedwarehouse
        {
            get { return selectedwarehouse; }
            set
            {
                selectedwarehouse = value;
                OnPropertyChanged("Selectedwarehouse");
            }
        }
        public int ArticleSleepDays
        {
            get
            {
                return articleSleepDays;
            }

            set
            {
                articleSleepDays = value;
                OnPropertyChanged("ArticleSleepDays");
            }
        }
        #endregion



        #region Constructor

        public SRMCommon()
        {
            IsWarehouseChangedEventCanOccur = true;
        }

        #endregion
    }
}
