using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.DataAccess
{
    public class EpcContext : DbContext
    {
        public EpcContext() : base("name=EpcContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<EpcContext>(null);
        }

        #region DBSet
        public DbSet<LookupValue> LookupValues { get; set; }
        public DbSet<LookupKey> LookupKeys { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<ProjectTeam> ProjectTeams { get; set; }
        public DbSet<ProductRoadmap> ProductRoadmaps { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<TaskAssistance> TaskAssistances { get; set; }
        public DbSet<TaskWatcher> TaskWatchers { get; set; }
        public DbSet<TaskAttachment> TaskAttachments { get; set; }
        public DbSet<TaskUser> TaskUsers { get; set; }
        public DbSet<TaskLog> TaskLogs { get; set; }
        public DbSet<TaskWorkingTime> TaskWorkingTimes { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TaskHistory> TaskHistories { get; set; }
        public DbSet<Customer> Customers { get; set; }
         public DbSet<ProjectMilestone> ProjectMilestones { get; set; }
        public DbSet<ProjectMilestoneDate> ProjectMilestoneDates { get; set; }
        public DbSet<ProjectAnalysis> ProjectAnalysis { get; set; }
        public DbSet<ProductVersion> ProductVersions { get; set; }
        public DbSet<ProductVersionItem> ProductVersionItems { get; set; }
        public DbSet<ProductVersionValidation> ProductVersionValidations { get; set; }
        public DbSet<GeosStatus> GeosStatus { get; set; }
        public DbSet<ProjectFollowup> ProjectFollowups { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ProjectScope> ProjectScopes { get; set; }
        #endregion

    }
}
