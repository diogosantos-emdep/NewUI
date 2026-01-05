using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Emdep.Geos.Data.DataAccess
{
   public class CrmContext : DbContext
    {

        public CrmContext() : base("name=CrmContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<CrmContext>(null);
        }

        #region DBSet
        public DbSet<Company> Companies { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
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
