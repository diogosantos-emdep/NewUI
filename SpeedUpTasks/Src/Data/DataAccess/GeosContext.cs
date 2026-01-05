using System.Text;
using System.Data;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Configuration;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Infrastructure;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using System.Data.Entity;

namespace Emdep.Geos.Data.DataAccess
{
    public class GeosContext : DbContext
    {
        public GeosContext()
            : base("name=GeosContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<GeosContext>(null);
        }

        #region DBSET
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferType> OfferTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<GeosStatus> GeosStatus { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyConversion> CurrencyConversions { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<SalesStatusType> SalesStatusTypes { get; set; }
        public DbSet<OfferLostReason> OfferLostReasons { get; set; }
        public DbSet<LostReasonsByOffer> LostReasonsByOffer { get; set; }
        #endregion

    }
}
