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


namespace Emdep.Geos.Data.DataAccess
{
    public class WorkbenchContext : DbContext
    {

        public WorkbenchContext()
            : base("name=WorkbenchContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        #region DBSET
        public DbSet<Workstation> Workstations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GeosModule> GeosModule { get; set; }
        public DbSet<GeosModuleDocumentation> GeosModuleDocumentations { get; set; }
        public DbSet<GeosWorkbenchVersionDownload> GeosWorkbenchVersionDownload { get; set; }
        public DbSet<GeosWorkbenchVersion> GeosWorkbenchVersions { get; set; }
        public DbSet<GeosWorkbenchVersionsFile> GeosWorkbenchVersionsFiles { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<GeosReleaseNote> GeosReleaseNotes { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<UserLoginEntry> UserLoginEntries { get; set; }
        public DbSet<WorkstationType> WorkstationTypes { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Country> Countries { get; set; }  
        #endregion

    }

   
}
