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
    [Table("activities_recurrence")]
    [DataContract]
    public class ActivitiesRecurrence : ModelBase, IDisposable
    {
        #region Fields
        Int64 idActivityRecurrence;
        Int32 idSite;
        Int32 idActivityType;
        string weekDay;
        Int32? periodicity;
        string activityType;
        string siteName;
        string siteNameWithoutCountry;
        string country;
        string region;
        string group;
        Int32 idSalesOwner;
        string salesOwner;
        string location;
        string subject;
        string description;
        Int32 idCustomer;
        Company site;
        Double? latitude;
        Double? longitude;
        byte isSalesResponsible;
        DateTime? minDueDate;
        DateTime? maxDueDate;
        string countryImageUrl;
        string iso;
        #endregion

        #region Constructor
        public ActivitiesRecurrence()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdActivityRecurrence")]
        [DataMember]
        public Int64 IdActivityRecurrence
        {
            get
            {
                return idActivityRecurrence;
            }

            set
            {
                idActivityRecurrence = value;
                OnPropertyChanged("IdActivityRecurrence");
            }
        }

        [Column("IdSite")]
        [DataMember]
        public Int32 IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [Column("IdActivityType")]
        [DataMember]
        public Int32 IdActivityType
        {
            get
            {
                return idActivityType;
            }

            set
            {
                idActivityType = value;
                OnPropertyChanged("IdActivityType");
            }
        }


        [Column("WeekDay")]
        [DataMember]
        public string WeekDay
        {
            get
            {
                return weekDay;
            }

            set
            {
                weekDay = value;
                OnPropertyChanged("WeekDay");
            }
        }


        [Column("Periodicity")]
        [DataMember]
        public Int32? Periodicity
        {
            get
            {
                return periodicity;
            }

            set
            {
                periodicity = value;
                OnPropertyChanged("Periodicity");
            }
        }

        [NotMapped]
        [DataMember]
        public string ActivityType
        {
            get
            {
                return activityType;
            }

            set
            {
                activityType = value;
                OnPropertyChanged("ActivityType");
            }
        }

        [NotMapped]
        [DataMember]
        public string SiteName
        {
            get
            {
                return siteName;
            }

            set
            {
                siteName = value;
                OnPropertyChanged("SiteName");
            }
        }


        [NotMapped]
        [DataMember]
        public string SiteNameWithoutCountry
        {
            get
            {
                return siteNameWithoutCountry;
            }

            set
            {
                siteNameWithoutCountry = value;
                OnPropertyChanged("SiteNameWithoutCountry");
            }
        }


        [NotMapped]
        [DataMember]
        public string Country
        {
            get
            {
                return country;
            }

            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        [NotMapped]
        [DataMember]
        public string Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }


        [NotMapped]
        [DataMember]
        public string Group
        {
            get
            {
                return group;
            }

            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdSalesOwner
        {
            get
            {
                return idSalesOwner;
            }

            set
            {
                idSalesOwner = value;
                OnPropertyChanged("IdSalesOwner");
            }
        }

        [NotMapped]
        [DataMember]
        public string Subject
        {
            get
            {
                return subject;
            }

            set
            {
                subject = value;
                OnPropertyChanged("Subject");
            }
        }

        [NotMapped]
        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [NotMapped]
        [DataMember]
        public string Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }

        [NotMapped]
        [DataMember]
        public string SalesOwner
        {
            get
            {
                return salesOwner;
            }

            set
            {
                salesOwner = value;
                OnPropertyChanged("SalesOwner");
            }
        }


        [NotMapped]
        [DataMember]
        public Int32 IdCustomer
        {
            get
            {
                return idCustomer;
            }

            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }


        [NotMapped]
        [DataMember]
        public Company Site
        {
            get
            {
                return site;
            }

            set
            {
                site = value;
                OnPropertyChanged("Site");
            }
        }


        [NotMapped]
        [DataMember]
        public Double? Latitude
        {
            get
            {
                return latitude;
            }

            set
            {
                latitude = value;
                OnPropertyChanged("Latitude");
            }
        }

        [NotMapped]
        [DataMember]
        public Double? Longitude
        {
            get
            {
                return longitude;
            }

            set
            {
                longitude = value;
                OnPropertyChanged("Longitude");
            }
        }


        [NotMapped]
        [DataMember]
        public byte IsSalesResponsible
        {
            get
            {
                return isSalesResponsible;
            }

            set
            {
                isSalesResponsible = value;
                OnPropertyChanged("IsSalesResponsible");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? MinDueDate
        {
            get
            {
                return minDueDate;
            }

            set
            {
                minDueDate = value;
                OnPropertyChanged("MinDueDate");
            }
        }


        [NotMapped]
        [DataMember]
        public DateTime? MaxDueDate
        { 
            get
            {
                return maxDueDate;
            }

            set
            {
                maxDueDate = value;
                OnPropertyChanged("MaxDueDate");
            }
        }

        [DataMember]
        public string CountryImageUrl
        {
            get
            {
                return countryImageUrl;
            }

            set
            {
                countryImageUrl = value;
                OnPropertyChanged("CountryImageUrl");
            }
        }

        [DataMember]
        public string Iso
        {
            get
            {
                return iso;
            }

            set
            {
                iso = value;
                OnPropertyChanged("Iso");
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
