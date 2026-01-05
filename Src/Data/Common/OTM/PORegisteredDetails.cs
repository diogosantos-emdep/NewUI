using Emdep.Geos.Data.Common.Crm;
using Emdep.Geos.Data.Common.SCM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    /// <summary>
    /// [001][ashish.malkhede][07.11.2024][GEOS2-6460]
    /// </summary>
    public class PORegisteredDetails: ModelBase,IDisposable
    {
        #region Fields
        Int64 idPO;
        string code;
        int idpotype;
        string type;
        string group;
        string plant;
        string country;
        string region;
        DateTime receptiondate;
        string sender;
        double povalue;
        double amount;
        string currency;
        string remarks;
        string linkedoffers;
        string shippingaddress;
        string isok;
        string confirmation;
        DateTime creationdate;
        string creator;
        DateTime? updaterdate;
        string updater;
        string iscancelled;
        string canceler;
        DateTime? cancellationdate;
        byte[] countryIconbytes;
        byte[] currencyIconbytes;
        string countryiso;
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        string creatorcode;
        string updatercode;
        string cancelercode;
        string attachmentFileName;
        private int idCustomerPlant;
        private int idSite;
        Int64 idShippingAddress;
        int idCurency;
        Int32 idsender;
        private int idGender;
        private int idPerson;
        private string firstName;
        private string lastName;
       
        public List<LogEntryByPOOffer> logEntriesByPO;
        public ObservableCollection<LinkedOffers> offersLinked;
        private bool isSave;
        private string fullName;
        private Int32 updatedBy;
        private Int32 idCustomer;
        private string siteName;

        //[Rahul.Gadhave][GEOS2-7850][Date:09-04-2025]
        string address;
        string zipCode;
        string city;
        string isocode;
        string countryIconUrl;
        string countriesName;

        DateTime? receptiondatenew;

        long idOfferCustomerGroup;
        

        #endregion

        #region Constructor
        public PORegisteredDetails()
        {

        }
        #endregion

        #region Properties

        [DataMember]
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }
        [DataMember]
        public Int64 IdPO
        {
            get { return idPO; }
            set
            {
                idPO = value;
                OnPropertyChanged("IdPO");
            }
        }
        [DataMember]
        public Int32 IdSender
        {
            get { return idsender; }
            set
            {
                idsender = value;
                OnPropertyChanged("IdSender");
            }
        }
        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }
        [DataMember]
        public int IdPOType
        {
            get { return idpotype; }
            set
            {
                idpotype = value;
                OnPropertyChanged("IdPOType");
            }
        }
        [DataMember]
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }
        [DataMember]
        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }
        [DataMember]
        public string Plant
        {
            get { return plant; }
            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }
        [DataMember]
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }
        [DataMember]
        public string CountryISO
        {
            get { return countryiso; }
            set
            {
                countryiso = value;
                OnPropertyChanged("CountryISO");
            }
        }
        [DataMember]
        public string Region
        {
            get { return region; }
            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }
        [DataMember]
        public DateTime ReceptionDate
        {
            get { return receptiondate; }
            set
            {
                receptiondate = value;
                OnPropertyChanged("ReceptionDate");
            }
        }
        [DataMember]
        public string Sender
        {
            get { return sender; }
            set
            {
                sender = value;
                OnPropertyChanged("Sender");
            }
        }
        [DataMember]
        public double Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged("Amount");
                OnPropertyChanged("FormattedAmount");
            }
        }
        [DataMember]
        public double POValue
        {
            get { return povalue; }
            set
            {
                povalue = value;
                OnPropertyChanged("POValue");
            }
        }
        [DataMember]
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
            }
        }
        [DataMember]
        public string Currency
        {
            get { return currency; }
            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }
        [DataMember]
        public string LinkedOffer
        {
            get { return linkedoffers; }
            set
            {
                linkedoffers = value;
                OnPropertyChanged("LinkedOffer");
            }
        }
        [DataMember]
        public string ShippingAddress
        {
            get { return shippingaddress; }
            set
            {
                shippingaddress = value;
                OnPropertyChanged("ShippingAddress");
            }
        }
        [DataMember]
        public string IsOK
        {
            get { return isok; }
            set
            {
                isok = value;
                OnPropertyChanged("IsOK");
            }
        }
        [DataMember]
        public string Confirmation
        {
            get { return confirmation; }
            set
            {
                confirmation = value;
                OnPropertyChanged("Confirmation");
            }
        }
        [DataMember]
        public DateTime CreationDate
        {
            get { return creationdate; }
            set
            {
                creationdate = value;
                OnPropertyChanged("CreationDate");
            }
        }
        [DataMember]
        public string Creator
        {
            get { return creator; }
            set
            {
                creator = value;
                OnPropertyChanged("Creator");
            }
        }
        [DataMember]
        public DateTime? UpdaterDate
        {
            get { return updaterdate; }
            set
            {
                updaterdate = value;
                OnPropertyChanged("UpdaterDate");
            }
        }
        [DataMember]
        public string Updater
        {
            get { return updater; }
            set
            {
                updater = value;
                OnPropertyChanged("Updater");
            }
        }
        [DataMember]
        public string IsCancelled
        {
            get { return iscancelled; }
            set
            {
                iscancelled = value;
                OnPropertyChanged("IsCancelled");
            }
        }
        [DataMember]
        public string Canceler
        {
            get { return canceler; }
            set 
            {
                canceler = value;
                OnPropertyChanged("Canceler");
            }
        }
        [DataMember]
        public DateTime? CancellationDate
        {
            get { return cancellationdate; }
            set
            {
                cancellationdate = value;
                OnPropertyChanged("CancellationDate");
            }
        }
        [DataMember]
        public byte[] CountryIconBytes
        {
            get { return countryIconbytes; }
            set
            {
                countryIconbytes = value;
                OnPropertyChanged("CountryIconBytes");
            }
        }
        [DataMember]
        public byte[] CurrencyIconBytes
        {
            get { return currencyIconbytes; }
            set
            {
                currencyIconbytes = value;
                OnPropertyChanged("CurrencyIconBytes");
            }
        }

        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        [DataMember]
        public string CreatorCode
        {
            get { return creatorcode; }
            set
            {
                creatorcode = value;
                OnPropertyChanged("CreatorCode");
            }
        }
        [DataMember]
        public string UpdaterCode
        {
            get { return updatercode; }
            set
            {
                updatercode = value;
                OnPropertyChanged("UpdaterCode");
            }
        }
        [DataMember]
        public string CancelerCode
        {
            get { return cancelercode; }
            set
            {
                cancelercode = value;
                OnPropertyChanged("CancelerCode");
            }
        }
        [DataMember]
        public string AttachmentFileName
        {
            get { return attachmentFileName; }
            set
            {
                attachmentFileName = value;
                OnPropertyChanged("AttachmentFileName");
            }
        }

        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        [DataMember]
        public int IdCustomerPlant
        {
            get
            {
                return idCustomerPlant;
            }
            set
            {

                idCustomerPlant = value;
                OnPropertyChanged("IdCustomerPlant");
            }
        }
        [DataMember]
        public int IdSite
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
        [DataMember]
        public Int64 IdShippingAddress
        {
            get
            {
                return idShippingAddress;
            }

            set
            {
                idShippingAddress = value;
                OnPropertyChanged("IdShippingAddress");
            }
        }
        [DataMember]
        public int IdCurrency
        {
            get
            {
                return idCurency;
            }
            set
            {

                idCurency = value;
                OnPropertyChanged("IdCurrency");
            }
        }
        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        [DataMember]
        public int IdGender
        {
            get { return idGender; }
            set
            {
                idGender = value;
                OnPropertyChanged("IdGender");
            }
        }
        [DataMember]
        public int IdPerson
        {
            get { return idPerson; }
            set
            {
                idPerson = value;
                OnPropertyChanged("IdPerson");
            }
        }
        [DataMember]
        public List<LogEntryByPOOffer> LogEntriesByPO
        {
            get { return logEntriesByPO; }
            set
            {
                logEntriesByPO = value;
                OnPropertyChanged("LogEntriesByPO");
            }
        }
        [DataMember]
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
            }
        }
        [DataMember]
        public ObservableCollection<LinkedOffers> OffersLinked
        {
            get { return offersLinked; }
            set
            {
                offersLinked = value;
                OnPropertyChanged("OffersLinked");
            }
        }
        [DataMember]
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set
            {
                updatedBy = value;
                OnPropertyChanged("UpdatedBy");
            }
        }

        public Int32 IdCustomer
        {
            get { return idCustomer; }
            set { idCustomer = value; }
        }

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
        //[Rahul.Gadhave][GEOS2-7850][Date:09-04-2025]
      
        [DataMember]
        public string Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }


        [DataMember]
        public string ZipCode
        {
            get
            {
                return zipCode;
            }

            set
            {
                zipCode = value;
                OnPropertyChanged("ZipCode");
            }
        }

        [DataMember]
        public string City
        {
            get
            {
                return city;
            }

            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        }
        [DataMember]
        public string IsoCode
        {
            get
            {
                return isocode;
            }

            set
            {
                isocode = value;
                OnPropertyChanged("IsoCode");
            }
        }

        [DataMember]
        public string CountryIconUrl
        {
            get
            {
                return countryIconUrl;
            }

            set
            {
                countryIconUrl = value;
                OnPropertyChanged("CountryIconUrl");
            }
        }

        [DataMember]
        public string CountriesName
        {
            get
            {
                return countriesName;
            }

            set
            {
                countriesName = value;
                OnPropertyChanged("CountriesName");
            }
        }
        //[DataMember]
        //public string Region
        //{
        //    get
        //    {
        //        return region;
        //    }

        //    set
        //    {
        //        region = value;
        //        OnPropertyChanged("Region");
        //    }
        //}
        [DataMember]
        public DateTime? ReceptionDateNew
        {
            get { return receptiondatenew; }
            set
            {
                receptiondatenew = value;
                OnPropertyChanged("ReceptionDateNew");
            }
        }

        private int idStatus;
        [DataMember]
        public int IdStatus
        {
            get
            {
                return idStatus;
            }
            set
            {

                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }
        Int64 idOffer;

        [DataMember]
        public Int64 IdOffer
        {
            get { return idOffer; }
            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }
        private string offercode;
        [DataMember]
        public string OfferCode
        {
            get
            {
                return offercode;
            }

            set
            {
                offercode = value;
                OnPropertyChanged("OfferCode");
            }
        }

        [DataMember]
        public Int64 IdOfferCustomerGroup
        {
            get
            {
                return idOfferCustomerGroup;
            }
            set
            {
                idOfferCustomerGroup = value;
                OnPropertyChanged(("IdOfferCustomerGroup"));
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
