using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [Table("ots")]
    [DataContract]
    public class Ots : ModelBase, IDisposable
    {
        #region Declaration

        Int32 year;
        string wareHouseLockSession;
        Int32 reviewedBy;
        string observations;
        byte numOT;
        Int32 number;
        DateTime? modifiedIn;
        Int32 modifiedBy;
        People modifiedByPerson;
        byte isClosed;
        byte idTemplate;
        Int32 idSite;
        Int64? idShippingAddress;
        Int64 idQuotation;
        Int64 idOT;
        DateTime? deliveryDate;
        DateTime? prevDeliveryDate;

        Byte delivered;
        Int32 delay;
        DateTime? creationDate;
        Int32 createdBy;
        People createdByPerson;
        string comments;
        string code;
        string attachedFiles;
        Quotation quotation;
        Dictionary<string, OtItem> items;
        Company site;

        List<OtItem> otItems;

        DateTime? poDate;
        double actualQuantity;
        double downloadedQuantity;
        double remainingQuantity;
        Int16 status;

        string mergeCode;
        Int64? producerIdCountryGroup;
        CountryGroup countryGroup;
        Int64 otitemCount;
        List<ArticlesStock> articleStocks;
        Int64 outOfStockItemCount;
        List<UserShortDetail> userShortDetails;
        string operators;
        string operatorNames;
        DateTime? expectedStartDate;
        DateTime? expectedEndDate;
        byte progress;
        Int64 modules;
        //  object geosAppSettings;
        bool isSendReadyExpeditionEmail;
        bool isUpdatedRow;
        DateTime? realStartDate;
        DateTime? realEndDate;
        string realDuration;
        Int64 producedModules;
        #endregion


        #region Properties

        [Column("Year")]
        [DataMember]
        public Int32 Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged("Year");
            }
        }

        [Column("WareHouseLockSession")]
        [DataMember]
        public string WareHouseLockSession
        {
            get { return wareHouseLockSession; }
            set
            {
                wareHouseLockSession = value;
                OnPropertyChanged("WareHouseLockSession");
            }
        }

        [Column("ReviewedBy")]
        [DataMember]
        public Int32 ReviewedBy
        {
            get { return reviewedBy; }
            set
            {
                reviewedBy = value;
                OnPropertyChanged("ReviewedBy");
            }
        }

        [Column("Observations")]
        [DataMember]
        public string Observations
        {
            get { return observations; }
            set
            {
                observations = value;
                OnPropertyChanged("Observations");
            }
        }

        [Key]
        [Column("NumOT")]
        [DataMember]
        public byte NumOT
        {
            get { return numOT; }
            set
            {
                numOT = value;
                OnPropertyChanged("NumOT");
            }
        }

        [Column("Number")]
        [DataMember]
        public Int32 Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime? ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public Int32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [Column("IsClosed")]
        [DataMember]
        public byte IsClosed
        {
            get { return isClosed; }
            set
            {
                isClosed = value;
                OnPropertyChanged("IsClosed");
            }
        }

        [Column("IdTemplate")]
        [DataMember]
        public byte IdTemplate
        {
            get { return idTemplate; }
            set
            {
                idTemplate = value;
                OnPropertyChanged("IdTemplate");
            }
        }

        [Column("IdSite")]
        [DataMember]
        public Int32 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [Column("IdShippingAddress")]
        [DataMember]
        public Int64? IdShippingAddress
        {
            get { return idShippingAddress; }
            set
            {
                idShippingAddress = value;
                OnPropertyChanged("IdShippingAddress");
            }
        }

        [Key]
        [Column("IdQuotation")]
        [DataMember]
        public Int64 IdQuotation
        {
            get { return idQuotation; }
            set
            {
                idQuotation = value;
                OnPropertyChanged("IdQuotation");
            }
        }

        [Column("IdOT")]
        [DataMember]
        public Int64 IdOT
        {
            get { return idOT; }
            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }

        [Column("DeliveryDate")]
        [DataMember]
        public DateTime? DeliveryDate
        {
            get { return deliveryDate; }

            set
            {
                deliveryDate = value;
                OnPropertyChanged("DeliveryDate");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? PrevDeliveryDate
        {
            get { return prevDeliveryDate; }
            set
            {
                prevDeliveryDate = value;
                OnPropertyChanged("PrevDeliveryDate");
            }
        }

        [Column("Delivered")]
        [DataMember]
        public byte Delivered
        {
            get { return delivered; }
            set
            {
                delivered = value;
                OnPropertyChanged("Delivered");
            }
        }

        [Column("CreationDate")]
        [DataMember]
        public DateTime? CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [Column("CreatedBy")]
        [DataMember]
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [Column("Comments")]
        [DataMember]
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        [Column("Code")]
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

        [Column("AttachedFiles")]
        [DataMember]
        public string AttachedFiles
        {
            get { return attachedFiles; }
            set
            {
                attachedFiles = value;
                OnPropertyChanged("AttachedFiles");
            }
        }

        [NotMapped]
        [DataMember]
        public Quotation Quotation
        {
            get { return quotation; }
            set
            {
                quotation = value;
                OnPropertyChanged("Quotation");
            }
        }

        [NotMapped]
        [DataMember]
        public Dictionary<string, OtItem> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        [NotMapped]
        [DataMember]
        public List<OtItem> OtItems
        {
            get
            {
                return otItems;
            }

            set
            {
                otItems = value;
                OnPropertyChanged("OtItems");
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
                OnPropertyChanged("Company");
            }
        }

        [NotMapped]
        [DataMember]
        public double ActualQuantity
        {
            get
            {
                return actualQuantity;
            }

            set
            {
                actualQuantity = value;
                OnPropertyChanged("ActualQuantity");
            }
        }

        [NotMapped]
        [DataMember]
        public double DownloadedQuantity
        {
            get
            {
                return downloadedQuantity;
            }

            set
            {
                downloadedQuantity = value;
                OnPropertyChanged("DownloadedQuantity");
            }
        }

        [NotMapped]
        [DataMember]
        public double RemainingQuantity
        {
            get
            {
                return remainingQuantity;
            }

            set
            {
                remainingQuantity = value;
                OnPropertyChanged("RemainingQuantity");
            }
        }

        [NotMapped]
        [DataMember]
        public Int16 Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [NotMapped]
        [DataMember]
        public int Delay
        {
            get
            {
                return delay;
            }

            set
            {
                delay = value;
                OnPropertyChanged("Delay");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? PoDate
        {
            get
            {
                return poDate;
            }

            set
            {
                poDate = value;
                OnPropertyChanged("PoDate");
            }
        }

        [NotMapped]
        [DataMember]
        public People CreatedByPerson
        {
            get { return createdByPerson; }
            set
            {
                createdByPerson = value;
                OnPropertyChanged("CreatedByPerson");
            }
        }

        [NotMapped]
        [DataMember]
        public People ModifiedByPerson
        {
            get { return modifiedByPerson; }
            set
            {
                modifiedByPerson = value;
                OnPropertyChanged("ModifiedByPerson");
            }
        }

        [NotMapped]
        [DataMember]
        public string MergeCode
        {
            get { return mergeCode; }
            set
            {
                mergeCode = value;
                OnPropertyChanged("MergeCode");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64? ProducerIdCountryGroup
        {
            get { return producerIdCountryGroup; }
            set
            {
                producerIdCountryGroup = value;
                OnPropertyChanged("ProducerIdCountryGroup");
            }
        }

        [NotMapped]
        [DataMember]
        public CountryGroup CountryGroup
        {
            get { return countryGroup; }
            set
            {
                countryGroup = value;
                OnPropertyChanged("CountryGroup");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 OtItemCount
        {
            get { return otitemCount; }
            set
            {
                otitemCount = value;
                OnPropertyChanged("OtItemCount");
            }
        }


        [NotMapped]
        [DataMember]
        public List<ArticlesStock> ArticleStocks
        {
            get { return articleStocks; }
            set
            {
                articleStocks = value;
                OnPropertyChanged("ArticleStocks");
            }
        }


        [NotMapped]
        [DataMember]
        public Int64 OutOfStockItemCount
        {
            get { return outOfStockItemCount; }
            set
            {
                outOfStockItemCount = value;
                OnPropertyChanged("OutOfStockItemCount");
            }
        }

        [NotMapped]
        [DataMember]
        public List<UserShortDetail> UserShortDetails
        {
            get { return userShortDetails; }
            set
            {
                userShortDetails = value;
                OnPropertyChanged("UserShortDetails");
            }
        }

        [NotMapped]
        [DataMember]
        public string Operators
        {
            get { return operators; }
            set
            {
                operators = value;
                OnPropertyChanged("Operators");
            }
        }


        [NotMapped]
        [DataMember]
        public string OperatorNames
        {
            get { return operatorNames; }
            set
            {
                operatorNames = value;
                OnPropertyChanged("OperatorNames");
            }
        }

        [Column("ExpectedStartDate")]
        [DataMember]
        public DateTime? ExpectedStartDate
        {
            get { return expectedStartDate; }
            set
            {
                expectedStartDate = value;
                OnPropertyChanged("ExpectedStartDate");
            }
        }

        [Column("ExpectedEndDate")]
        [DataMember]
        public DateTime? ExpectedEndDate
        {
            get { return expectedEndDate; }
            set
            {
                expectedEndDate = value;
                OnPropertyChanged("ExpectedEndDate");
            }
        }

        [Column("Progress")]
        [DataMember]
        public byte Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 Modules
        {
            get { return modules; }
            set
            {
                modules = value;
                OnPropertyChanged("Modules");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsSendReadyExpeditionEmail
        {
            get { return isSendReadyExpeditionEmail; }
            set
            {
                isSendReadyExpeditionEmail = value;
                OnPropertyChanged("IsSendReadyExpeditionEmail");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsUpdatedRow
        {
            get { return isUpdatedRow; }
            set
            {
                isUpdatedRow = value;
                OnPropertyChanged("IsUpdatedRow");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? RealStartDate
        {
            get { return realStartDate; }
            set
            {
                realStartDate = value;
                OnPropertyChanged("RealStartDate");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? RealEndDate
        {
            get { return realEndDate; }
            set
            {
                realEndDate = value;
                OnPropertyChanged("RealEndDate");
            }
        }

        [NotMapped]
        [DataMember]
        public string RealDuration
        {
            get { return realDuration; }
            set
            {
                realDuration = value;
                OnPropertyChanged("RealDuration");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 ProducedModules
        {
            get { return producedModules; }
            set
            {
                producedModules = value;
                OnPropertyChanged("ProducedModules");
            }
        }

        //[NotMapped]
        //[DataMember]
        //public object GeosAppSettings
        //{
        //    get { return geosAppSettings; }
        //    set
        //    {
        //        geosAppSettings = value;
        //        OnPropertyChanged("GeosAppSettings");
        //    }
        //}

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
