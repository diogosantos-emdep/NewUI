using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.DataAccess
{
    public class MainServerWorkbenchContext : DbContext
    {
        public MainServerWorkbenchContext()
            : base("name=MainServerWorkbenchContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<MainServerWorkbenchContext>(null);
        }

        #region DBSet

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }
        #endregion
    }
}

