using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Data.Common.SAM;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.TSM;

namespace Emdep.Geos.Data.Common
{
    [Table("ots")]
    [DataContract]
    public class Ots : ModelBase, IDisposable
    {
        #region Declaration

        float? timeEstimationInHours;
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
        ObservableCollection<OtItem> otItemsOb;

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
        double? realDurationDouble;
        Int64 producedModules;
        Int64 idOffer;
        string offerCode;
        List<Attachment> lstAttachmentFiles;
        WorkflowStatus workflowStatus;
        byte idWorkflowStatus;

        List<LogEntriesByOT> otLogEntries;
        List<LogEntriesByOT> otComments;
        List<Detection> lstDetection;
        bool isRemarkReadOnly;
        string deliveryWeek;
        Workflow workflow;
        Int32 reviewed;//[Sudhir.Jangra][GEOS2-5634]
        Int32 srmProgress;//[Sudhir.Jangra][GEOS2-5634]
        Int32 totalValidateItems;//[Sudhir.Jangra][GEOS2-5635]
        private ObservableCollection<OtItem> deletedOtItemList;
        private ObservableCollection<OtItem> addedOtItemList;

        bool isReviewValidated;//[Sudhir.Jangra][GEOS2-5635]
        int validatedCount;//[Sudhir.Jangra][GEOS2-5635]
        double quantity;//[Sudhir.Jangra][GEOS2-5636]
        private ObservableCollection<OtItem> updatedOtItemList; //[rushikesh.gaikwad][GEOS2-5473][18.07.2024]
        string offerOwner;//[Sudhir.Jangra][GEOS2-5637]
        string customerNameForReport;//[Sudhir.Jangra][GEOS2-5637]
        byte idCurrency;//[Sudhir.Jangra][GEOS2-5637]
        Currency currency;//[Sudhir.Jangra][GEOS2-5637]
        Int64 idRevisionItem;//[Sudhir.jangra][GEOS2-5636]
        private string assignee; //[GEOS2-8965][pallavi.kale][28.11.2025]
        private Int32 services; //[GEOS2-8963][pallavi.kale][28.11.2025]
        private List<TSMLogEntriesByOT> tsmOTComments; //[GEOS2-8965][pallavi.kale][28.11.2025]
        private List<TSMLogEntriesByOT> tsmOTLogEntries; //[GEOS2-8965][pallavi.kale][28.11.2025]
        private TSMWorkflowStatus tsmWorkflowStatus; //[GEOS2-8965][pallavi.kale][28.11.2025]
        #endregion


        #region Properties
        [DataMember]
        public ObservableCollection<OtItem> DeletedOtItemList
        {
            get
            {
                return deletedOtItemList;
            }

            set
            {
                deletedOtItemList = value;
                OnPropertyChanged("DeletedOtItemList");
            }
        }

        [DataMember]
        public ObservableCollection<OtItem> AddedOtItemList
        {
            get
            {
                return addedOtItemList;
            }

            set
            {
                addedOtItemList = value;
                OnPropertyChanged("AddedOtItemList");
            }
        }

        [Column("TimeEstimation")]
        [DataMember]
        public float? TimeEstimationInHours
        {
            get { return timeEstimationInHours; }
            set
            {
                timeEstimationInHours = value;
                OnPropertyChanged("TimeEstimationInHours");
            }
        }

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


        [NotMapped]
        [DataMember]
        public List<Attachment> LstAttachmentFiles
        {
            get { return lstAttachmentFiles; }
            set
            {
                lstAttachmentFiles = value;
                OnPropertyChanged("LstAttachmentFiles");
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
        public ObservableCollection<OtItem> OtItemsOb
        {
            get
            {
                return otItemsOb;
            }

            set
            {
                otItemsOb = value;
                OnPropertyChanged("OtItemsOb");
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
        public double? RealDurationDouble
        {
            get
            {
                return realDurationDouble;
            }
            set
            {
                realDurationDouble = value;
                OnPropertyChanged("RealDurationDouble");
            }
        }

        [NotMapped]
        [DataMember]
        public double? CostDeviation
        {
            get
            {
                float timeEstimationFloat = 0;
                double? costDeviation = null;

                if (RealDuration != null && RealDurationDouble > 0)
                {
                    if (TimeEstimationInHours != null)
                    {
                        timeEstimationFloat = TimeEstimationInHours.Value;

                        if (Math.Abs(timeEstimationFloat) > 0)
                        {
                            costDeviation = Math.Round((RealDurationDouble.Value - timeEstimationFloat) / timeEstimationFloat, 2);
                        }
                    }
                }
                return costDeviation;
            }
            set { }

        }

        [NotMapped]
        public string CostDeviationPecentage
        {
            get
            {
                string costDeviationPecentage = null;

                if (CostDeviation != null)
                {
                    costDeviationPecentage = Math.Round(CostDeviation.Value * 100, 0) + "%";
                }
                return costDeviationPecentage;
            }
            set
            {

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

        [NotMapped]
        [DataMember]
        public long IdOffer
        {
            get
            {
                return idOffer;
            }

            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }

        [NotMapped]
        [DataMember]
        public string OfferCode
        {
            get
            {
                return offerCode;
            }

            set
            {
                offerCode = value;
                OnPropertyChanged("OfferCode");
            }
        }

        [NotMapped]
        [DataMember]
        public WorkflowStatus WorkflowStatus
        {
            get
            {
                return workflowStatus;
            }

            set
            {
                workflowStatus = value;
                OnPropertyChanged("WorkflowStatus");
            }
        }
        [NotMapped]
        [DataMember]
        public byte IdWorkflowStatus
        {
            get
            {
                return idWorkflowStatus;
            }

            set
            {
                idWorkflowStatus = value;
                OnPropertyChanged("IdWorkflowStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public List<Detection> LstDetection
        {
            get
            {
                return lstDetection;
            }

            set
            {
                lstDetection = value;
                OnPropertyChanged("LstDetection");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsRemarkReadOnly
        {
            get
            {
                return isRemarkReadOnly;
            }

            set
            {
                isRemarkReadOnly = value;
                OnPropertyChanged("IsRemarkReadOnly");
            }
        }



        [NotMapped]
        [DataMember]
        public string DeliveryWeek
        {
            get
            {
                return deliveryWeek;
            }

            set
            {
                deliveryWeek = value;
                OnPropertyChanged("DeliveryWeek");
            }
        }

        [NotMapped]
        [DataMember]
        public Workflow Workflow
        {
            get
            {
                return workflow;
            }

            set
            {
                workflow = value;
                OnPropertyChanged("Workflow");
            }
        }

        //[Sudhir.Jangra][GEOS2-5634]
        [NotMapped]
        [DataMember]
        public Int32 Reviewed
        {
            get { return reviewed; }
            set
            {
                reviewed = value;
                OnPropertyChanged("Reviewed");
            }
        }

        //[Sudhir.jangra][GEOS2-5634]
        [NotMapped]
        [DataMember]
        public Int32 SRMProgress
        {
            get { return srmProgress; }
            set
            {
                srmProgress = value;
                OnPropertyChanged("SRMProgress");
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


        //[Sudhir.Jangra][GEOS2-5634]
        [NotMapped]
        [DataMember]
        public Int32 TotalValidateItems
        {
            get { return totalValidateItems; }
            set
            {
                totalValidateItems = value;
                OnPropertyChanged("TotalValidateItems");
            }
        }


        //[Sudhir.Jangra][GEOS2-5635]
        [NotMapped]
        [DataMember]
        public bool IsReviewValidated
        {
            get { return isReviewValidated; }
            set
            {
                isReviewValidated = value;
                OnPropertyChanged("IsReviewValidated");
            }
        }

        //[Sudhir.Jangra][GEOS2-5635]
        [NotMapped]
        [DataMember]
        public int ValidatedCount
        {
            get { return validatedCount; }
            set
            {
                validatedCount = value;
                OnPropertyChanged("ValidatedCount");
            }
        }

        //[Sudhir.Jangra][GEOS2-5636]
        [NotMapped]
        [DataMember]
        public double Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }


        //[rushikesh.gaikwad][GEOS2-5473][18.07.2024]
        [DataMember]
        public ObservableCollection<OtItem> UpdatedOtItemList
        {
            get
            {
                return updatedOtItemList;
            }

            set
            {
                updatedOtItemList = value;
                OnPropertyChanged("UpdatedOtItemList");
            }
        }


        //[Sudhir.Jangra][GEOS2-5637]
        [NotMapped]
        [DataMember]
        public string OfferOwner
        {
            get { return offerOwner; }
            set
            {
                offerOwner = value;
                OnPropertyChanged("OfferOwner");
            }
        }

        //[Sudhir.Jangra][GEOS2-5637]
        [NotMapped]
        [DataMember]
        public string CustomerNameForReport
        {
            get { return customerNameForReport; }
            set
            {
                customerNameForReport = value;
                OnPropertyChanged("CustomerNameForReport");
            }
        }

        //[Sudhir.Jangra][GEOS2-5637]
        [Column("idCurrency")]
        [DataMember]
        public Byte IdCurrency
        {
            get
            {
                return idCurrency;
            }
            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        //[Sudhir.Jangra][GEOS2-5637]
        [NotMapped]
        [DataMember]
        public Currency Currency
        {
            get
            {
                return currency;
            }

            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }

        //[Sudhir.Jangra][GEOS2-5636]
        [NotMapped]
        [DataMember]
        public Int64 IdRevisionItem
        {
            get { return idRevisionItem; }
            set
            {
                idRevisionItem = value;
                OnPropertyChanged("IdRevisionItem");
            }
        }
        byte? idCarriageMethod;
        [NotMapped]
        [DataMember]
        public byte? IdCarriageMethod
        {
            get { return idCarriageMethod; }
            set
            {
                idCarriageMethod = value;
                OnPropertyChanged("IdCarriageMethod");
            }
        }
        //[GEOS2-8965][pallavi.kale][28.11.2025]
        [DataMember]
        [NotMapped]
        public string Assignee
        {
            get { return assignee; }
            set
            {
                assignee = value;
                OnPropertyChanged("Assignee");
            }
        }
        //[GEOS2-8965][pallavi.kale][28.11.2025]
        [DataMember]
        [NotMapped]
        public Int32 Services
        {
            get { return services; }
            set
            {
                services = value;
                OnPropertyChanged("Services");
            }
        }
        //[GEOS2-8965][pallavi.kale][28.11.2025]
        [NotMapped]
        [DataMember]
        public List<TSMLogEntriesByOT> TSMOTComments
        {
            get
            {
                return tsmOTComments;
            }

            set
            {
                tsmOTComments = value;
                OnPropertyChanged("TSMOTComments");
            }
        }
        //[GEOS2-8965][pallavi.kale][28.11.2025]
        [NotMapped]
        [DataMember]
        public List<TSMLogEntriesByOT> TSMOTLogEntries
        {
            get
            {
                return tsmOTLogEntries;
            }

            set
            {
                tsmOTLogEntries = value;
                OnPropertyChanged("TSMOTLogEntries");
            }
        }
        //[GEOS2-8965][pallavi.kale][28.11.2025]
        [NotMapped]
        [DataMember]
        public TSMWorkflowStatus TSMWorkflowStatus
        {
            get
            {
                return tsmWorkflowStatus;
            }

            set
            {
                tsmWorkflowStatus = value;
                OnPropertyChanged("TSMWorkflowStatus");
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
            // return this.MemberwiseClone();

            var newOtsClone = (Ots)this.MemberwiseClone();

            if (OtItems != null)
            {
                newOtsClone.OtItems = OtItems.Select(x => (OtItem)x.Clone()).ToList();
            }
            return newOtsClone;
        }

        #endregion


        #region  GEOS2-2250 Orders Preparation

        private string poCode;
        [NotMapped]
        [DataMember]
        public string PoCode
        {
            get
            {
                return poCode;
            }

            set
            {
                poCode = value;
                OnPropertyChanged("PoCode");
            }
        }

        private Shipment firstShipment;
        [NotMapped]
        [DataMember]
        public Shipment FirstShipment

        {
            get { return firstShipment; }
            set
            {
                firstShipment = value;
                OnPropertyChanged("FirstShipment");
            }
        }

        private TimeSpan? totalTimeForPickingOT;
        [NotMapped]
        [DataMember]
        public TimeSpan? TotalTimeForPickingOT
        {
            get
            {
                return totalTimeForPickingOT;
            }

            set
            {
                this.totalTimeForPickingOT = value;
                OnPropertyChanged(nameof(TotalTimeForPickingOT));
            }
        }

        private TimeSpan? totalTimeForShipmentDelivery;
        [NotMapped]
        [DataMember]
        public TimeSpan? TotalTimeForShipmentDelivery
        {
            get
            {
                return totalTimeForShipmentDelivery;
            }

            set
            {
                this.totalTimeForShipmentDelivery = value;
                OnPropertyChanged(nameof(TotalTimeForShipmentDelivery));
            }
        }

        private TimeSpan? totalTimeForOTDelivery;
        [NotMapped]
        [DataMember]
        public TimeSpan? TotalTimeForOTDelivery
        {
            get
            {
                return totalTimeForOTDelivery;
            }

            set
            {
                this.totalTimeForOTDelivery = value;
                OnPropertyChanged(nameof(TotalTimeForOTDelivery));
            }
        }

        private TimeSpan? totalTimeForShipmentCreatedIn;
        [NotMapped]
        [DataMember]
        public TimeSpan? TotalTimeForShipmentCreatedIn
        {
            get
            {
                return totalTimeForShipmentCreatedIn;
            }

            set
            {
                this.totalTimeForShipmentCreatedIn = value;
                OnPropertyChanged(nameof(TotalTimeForShipmentCreatedIn));
            }
        }

        [NotMapped]
        [DataMember]
        public List<LogEntriesByOT> OTLogEntries
        {
            get
            {
                return otLogEntries;
            }

            set
            {
                otLogEntries = value;
                OnPropertyChanged("OTLogEntries");
            }
        }

        [NotMapped]
        [DataMember]
        public List<LogEntriesByOT> OTComments
        {
            get
            {
                return otComments;
            }

            set
            {
                otComments = value;
                OnPropertyChanged("OTComments");
            }
        }

        //[rdixit][GEOS2-3150][05.08.2022]
        private string operatorUserName;
        [NotMapped]
        [DataMember]
        public string OperatorUserName
        {
            get
            {
                return operatorUserName;
            }

            set
            {
                operatorUserName = value;
                OnPropertyChanged("OperatorUserName");
            }
        }


        DateTime? endDate;
        [NotMapped]
        [DataMember]
        public DateTime? EndDateTime
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDateTime");
            }
        }

        Int32 endTimeFlag;
        [NotMapped]
        [DataMember]
        public Int32 EndTimeFlag
        {
            get { return endTimeFlag; }
            set
            {
                endTimeFlag = value;
                OnPropertyChanged("EndTimeFlag");
            }
        }

        #endregion
    }
}
