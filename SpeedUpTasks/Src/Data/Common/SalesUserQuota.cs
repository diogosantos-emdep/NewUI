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
    [Table("sales_user_quotas")]
    [DataContract]
    public class SalesUserQuota : ModelBase, IDisposable
    {
        #region Fields
        Int32 idSalesUser;
        Int32 year;
        double salesQuotaAmount;
        byte idSalesQuotaCurrency;
        People people;
        double wonValue;
        double? percentage;
        object tag;
        Int64? plannedAppointment;
        Int64? actualAppointment;
        List<SalesUserActivity> salesUserActivity;
        Int64 total;
        double? ytd;
        double? period;
        DateTime? exchangeRateDate;
        double salesQuotaAmountWithExchangeRate;
        #endregion

        #region Constructor
        public SalesUserQuota()
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

        [Column("Year")]
        [DataMember]
        public Int32 Year
        {
            get
            {
                return year;
            }

            set
            {
                year = value;
                OnPropertyChanged("Year");
            }
        }

        [Column("ExchangeRateDate")]
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
        public object Tag
        {
            get
            {
                return tag;
            }

            set
            {
                tag = value;
                OnPropertyChanged("Tag");
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

        [NotMapped]
        [DataMember]
        public Int64? PlannedAppointment
        {
            get
            {
                return plannedAppointment;
            }

            set
            {
                plannedAppointment = value;
                OnPropertyChanged("PlannedAppointment");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64? ActualAppointment
        {
            get
            {
                return actualAppointment;
            }

            set
            {
                actualAppointment = value;
                OnPropertyChanged("ActualAppointment");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 Total
        {
            get
            {
                return total;
            }

            set
            {
                total = value;
                OnPropertyChanged("Total");
            }
        }

        [NotMapped]
        [DataMember]
        public double? YTD
        {
            get
            {
                return ytd;
            }

            set
            {
                ytd = value;
                OnPropertyChanged("YTD");
            }
        }

        [NotMapped]
        [DataMember]
        public double? Period
        {
            get
            {
                return period;
            }

            set
            {
                period = value;
                OnPropertyChanged("Period");
            }
        }

        [NotMapped]
        [DataMember]
        public double SalesQuotaAmountWithExchangeRate
        {
            get
            {
                return salesQuotaAmountWithExchangeRate;
            }

            set
            {
                salesQuotaAmountWithExchangeRate = value;
                OnPropertyChanged("SalesQuotaAmountWithExchangeRate");
            }
        }


        [NotMapped]
        [DataMember]
        public List<SalesUserActivity> SalesUserActivity
        {
            get
            {
                return salesUserActivity;
            }
            set
            {
                salesUserActivity = value;
                OnPropertyChanged("SalesUserActivity");
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
