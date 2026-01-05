using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common
{
    [Table("sales_users")]
    [DataContract]
    public class SalesUser : ModelBase, IDisposable
    {

        #region Fields
        Int32 idSalesUser;
        Int32 idSalesTeam;
        double salesQuotaAmount;
        byte idSalesQuotaCurrency;
        People people;
        double wonValue;
        double? percentage;
        LookupValue lookupValue;
        List<SalesUserQuota> salesUserQuotas;
        Int32 idSalesPlant;
        Company company;
        List<UserPermission> userPermission;
        List<SiteUserPermission> siteUserPermission;
        DateTime? exchangeRateDate;
        int maxDiscountAllowed;

        #endregion

        #region Constructor
        public SalesUser()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdSalesUser")]
        [DataMember]
        public Int32 IdSalesUser
        {
            get
            {
                return idSalesUser;
            }

            set
            {
                idSalesUser = value;
                OnPropertyChanged("IdSalesUser");
            }
        }

        [Column("IdSalesTeam")]
        [DataMember]
        public Int32 IdSalesTeam
        {
            get
            {
                return idSalesTeam;
            }

            set
            {
                idSalesTeam = value;
                OnPropertyChanged("IdSalesTeam");
            }
        }

        [Column("MaxDiscountAllowed")]
        [DataMember]
        public int MaxDiscountAllowed
        {
            get
            {
                return maxDiscountAllowed;
            }

            set
            {
                maxDiscountAllowed = value;
                OnPropertyChanged("MaxDiscountAllowed");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? ExchangeRateDate
        {
            get
            {
                return exchangeRateDate;
            }

            set
            {
                exchangeRateDate = value;
                OnPropertyChanged("ExchangeRateDate");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdSalesPlant
        {
            get
            {
                return idSalesPlant;
            }

            set
            {
                idSalesPlant = value;
                OnPropertyChanged("IdSalesPlant");
            }
        }

        [NotMapped]
        [DataMember]
        public double SalesQuotaAmount
        {
            get
            {
                return salesQuotaAmount;
            }

            set
            {
                salesQuotaAmount = value;
                OnPropertyChanged("SalesQuotaAmount");
            }
        }

        [NotMapped]
        [DataMember]
        public byte IdSalesQuotaCurrency
        {
            get
            {
                return idSalesQuotaCurrency;
            }

            set
            {
                idSalesQuotaCurrency = value;
                OnPropertyChanged("IdSalesQuotaCurrency");
            }
        }

        [NotMapped]
        [DataMember]
        public Company Company
        {
            get
            {
                return company;
            }

            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }


        [NotMapped]
        [DataMember]
        public List<SalesUserQuota> SalesUserQuotas
        {
            get
            {
                return salesUserQuotas;
            }

            set
            {
                salesUserQuotas = value;
                OnPropertyChanged("SalesUserQuotas");
            }
        }

        [NotMapped]
        [DataMember]
        public List<UserPermission> UserPermission
        {
            get
            {
                return userPermission;
            }

            set
            {
                userPermission = value;
                OnPropertyChanged("UserPermission");
            }
        }


        [NotMapped]
        [DataMember]
        public List<SiteUserPermission> SiteUserPermission
        {
            get
            {
                return siteUserPermission;
            }

            set
            {
                siteUserPermission = value;
                OnPropertyChanged("SiteUserPermission");
            }
        }

        [NotMapped]
        [DataMember]
        public People People
        {
            get
            {
                return people;
            }
            set
            {
                people = value;
                OnPropertyChanged("People");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue LookupValue
        {
            get
            {
                return lookupValue;
            }
            set
            {
                lookupValue = value;
                OnPropertyChanged("LookupValue");
            }
        }

        [NotMapped]
        [DataMember]
        public double WonValue
        {
            get
            {
                return wonValue;
            }
            set
            {
                wonValue = value;
                OnPropertyChanged("WonValue");
            }
        }


        [NotMapped]
        [DataMember]
        public double? Percentage
        {
            get
            {
                return percentage;
            }
            set
            {
                percentage = value;
                OnPropertyChanged("Percentage");
            }
        }


     
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }


        #endregion
    }
}
