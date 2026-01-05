using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Configuration;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Infrastructure;
using Emdep.Geos.Data.Common.HarnessPart;
namespace Emdep.Geos.Data.DataAccess
{
   public class HarnessPartContext : DbContext
    {

       public HarnessPartContext()
           : base("name=HarnessPartContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<HarnessPartContext>(null);
        }

        #region DBSET
       public DbSet<HarnessPart> HarnessParts { get; set; }
       public DbSet<HarnessPartAccessoryType> HarnessPartAccessoryTypes { get; set; }
       public DbSet<HarnessPartType> HarnessPartTypes { get; set; }
    
       public DbSet<Color> Colors { get; set; }
        #endregion
    }
}
