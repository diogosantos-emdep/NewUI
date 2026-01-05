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
using Emdep.Geos.Data.Common.Hrm;


namespace Emdep.Geos.Data.DataAccess
{
    public class HrmContext : DbContext
    {
        public HrmContext()
            : base("name=HRMContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<HrmContext>(null);
        }

        #region DBSET
        //public DbSet<JobDescription> JobDescriptions { get; set; }
        //public DbSet<Department> Departments { get; set; }
        
        //public DbSet<Employee> Employees { get; set; }
        //public DbSet<EmployeeContact> EmployeeContacts { get; set; }
        #endregion
    }
}
