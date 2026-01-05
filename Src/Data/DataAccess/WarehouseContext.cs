using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.DataAccess
{
    public class WarehouseContext : DbContext
    {
        public WarehouseContext()
            : base("name=WarehouseContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<WarehouseContext>(null);
        }

        #region DBSet

        #endregion
    }
}
