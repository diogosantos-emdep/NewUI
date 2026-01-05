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
using Emdep.Geos.Data.Common.Glpi;
using Emdep.Geos.Data.Common.HarnessPart;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.DataAccess
{
    public class WorkbenchContext : DbContext
    {

        public WorkbenchContext()
            : base("name=WorkbenchContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<WorkbenchContext>(null);
        }

        #region DBSET
        public DbSet<UserManagerDtl> UserManagers { get; set; }
        public DbSet<Workstation> Workstations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LookupValue> LookupValues { get; set; }
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
        public DbSet<Country> Countries { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<PermissionTemplate> PermissionTemplates { get; set; }
        public DbSet<PermissionTemplateType> PermissionTemplateTypes { get; set; }
        public DbSet<GeosProvider> GeosProviders { get; set; }
        public DbSet<GeosGlpiSite> GeosGlpiSites { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<GeosGlpiItilCategory> GeosGlpiItilCategories { get; set; }
        public DbSet<UIModuleTheme> UIModuleThemes { get; set; }
        public DbSet<UITheme> UIThemes { get; set; }
        public DbSet<SiteUserPermission> SiteUserPermissions { get; set; }
        public DbSet<GeosVersionBetaTester> GeosVersionBetaTesters { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<EnterpriseGroup> EnterpriseGroups { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
       
        public DbSet<Offer> Offers { get; set; }
        #endregion


    }
    

   
}
