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
    public class LanguageContext :DbContext
    {
        public LanguageContext()
            : base("name=WorkbenchContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<LanguageContext>(null);
        }

        #region DBSET
        public DbSet<Language>Languages { get; set; }
        public DbSet<LanguageDependentEntry> LanguageDependentEntries { get; set; }
        #endregion
    }
}
