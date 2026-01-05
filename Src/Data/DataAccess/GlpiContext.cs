
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
using Emdep.Geos.Data.Common.Glpi;

namespace Emdep.Geos.Data.DataAccess
{
    public class GLPIContext : DbContext
    {
        public GLPIContext()
            : base("name=GLPIContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        #region DBSET
        public DbSet<GlpiTicket> GLPITickets { get; set; }
        public DbSet<GlpiDocument> GLPIDocuments { get; set; }
        public DbSet<GlpiDocumentItem> GLPIDocumentItems { get; set; }
        public DbSet<GlpiLog> GLPILogs { get; set; }
        public DbSet<GlpiUser> GLPIUsers { get; set; }
        public DbSet<GlpiLocation> GLPILocations { get; set; }
        public DbSet<GlpiDocumentType> GLPIDocumentTypes { get; set; }
        public DbSet<GlpiItilCategory> GLPIItilCategories { get; set; }
        #endregion
    }
}
