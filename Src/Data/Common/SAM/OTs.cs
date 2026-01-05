using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.SRM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SAM
{
    [Table("ots")]
    [DataContract]
    public class OTs : ModelBase, IDisposable
    {
        #region Fields
        //private Status itemStatus;
        Int32 idItemStatus;
        private LookupValue itemStatus;
        Int64 idOT;
        string siteName;
        string customer;
        string code;
        string numItem;
        string reference;
        string description;
        Int64 idArticle;
        string imagePath;
        Int64 quantity;
        Int64 idpartNumberTracking;
        byte[] imageInBytes;
        string partNumberCode;
        bool isEnabled;
        string barcodestring;
       
        ValidateItem validateItem;
        Country country;
        WOItem wOItem;
        Article article;
        WorkflowStatus workflowStatus;
        byte idWorkflowStatus;
        DateTime? deliveryDate;
        Int32 delay;
        string expectedDeliveryWeek;
        private ObservableCollection<OtItem> updatedOtItemList; //[rushikesh.gaikwad][GEOS2-5473][18.07.2024]


        #region GEOS2-3682
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
        DateTime? prevDeliveryDate;

        Byte delivered;
        DateTime? creationDate;
        Int32 createdBy;
        People createdByPerson;
        string comments;
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

        List<SAMLogEntries> otLogEntries;
        List<SAMLogEntries> otComments;
        List<Detection> lstDetection;
        bool isRemarkReadOnly;
        string deliveryWeek;
        Workflow workflow;
        #endregion
        #endregion

        #region Constructor

        public OTs()
        {

        }

        #endregion

        #region Properties

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

        [DataMember]
        public string SiteName
        {
            get { return siteName; }
            set
            {
                siteName = value;
                OnPropertyChanged("SiteName");
            }
        }

        [DataMember]
        public string Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }

        [DataMember]
        public string NumItem
        {
            get { return numItem; }
            set
            {
                numItem = value;
                OnPropertyChanged("NumItem");
            }
        }

        [DataMember]
        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }

        [DataMember]
        public Int64 IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        [DataMember]
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        [DataMember]
        public Int64 Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
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
        public Int64 IdPartNumberTracking
        {
            get { return idpartNumberTracking; }
            set
            {
                idpartNumberTracking = value;
                OnPropertyChanged("IdPartNumberTracking");
            }
        }

        [DataMember]
        public byte[] ImageInBytes
        {
            get { return imageInBytes; }
            set
            {
                imageInBytes = value;
                OnPropertyChanged("ImageInBytes");
            }
        }

        [DataMember]
        public string PartNumberCode
        {
            get { return partNumberCode; }
            set
            {
                partNumberCode = value;
                OnPropertyChanged("PartNumberCode");
            }
        }

        [DataMember]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [DataMember]
        public string Barcodestring
        {
            get { return barcodestring; }
            set
            {
                barcodestring = value;
                OnPropertyChanged("Barcodestring");
            }
        }

        [NotMapped]
        [DataMember]
        public ValidateItem ValidateItem
        {
            get
            {
                return validateItem;
            }

            set
            {
                validateItem = value;
                OnPropertyChanged("ValidateItem");
            }
        }

        [NotMapped]
        [DataMember]
        public Country Country
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
        public WOItem WOItem
        {
            get
            {
                return wOItem;
            }

            set
            {
                wOItem = value;
                OnPropertyChanged("WOItem");
            }
        }

        [NotMapped]
        [DataMember]
        public Article Article
        {
            get
            {
                return article;
            }

            set
            {
                article = value;
                OnPropertyChanged("Article");
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
        public string ExpectedDeliveryWeek
        {
            get
            {
                return expectedDeliveryWeek;
            }

            set
            {
                expectedDeliveryWeek = value;
                OnPropertyChanged("ExpectedDeliveryWeek");
            }
        }

       
        #region GEOS2-3682
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
        #endregion
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

        byte[] articleImageInBytes;
        [DataMember]
        public byte[] ArticleImageInBytes
        {
            get { return articleImageInBytes; }
            set
            {
                articleImageInBytes = value;
                OnPropertyChanged("ArticleImageInBytes");
            }
        }

        List<PCMArticleImage> pCMArticleImageList;
        [DataMember]
        public List<PCMArticleImage> PCMArticleImageList
        {
            get
            {
                return pCMArticleImageList;
            }

            set
            {
                pCMArticleImageList = value;
                OnPropertyChanged("PCMArticleImageList");
            }
        }
        [DataMember]
        public List<SAMLogEntries> OtLogEntries
        {
            get
            {
                return otLogEntries;
            }

            set
            {
                otLogEntries = value;
                OnPropertyChanged("OtLogEntries");
            }
        }
        [DataMember]
        public List<SAMLogEntries> OtComments
        {
            get
            {
                return otComments;
            }

            set
            {
                otComments = value;
                OnPropertyChanged("OtComments");
            }
        }
        List<ArticleDecomposition> articleDecompostionList;
        [DataMember]
        public List<ArticleDecomposition> ArticleDecompostionList
        {
            get
            {
                return articleDecompostionList;
            }

            set
            {
                articleDecompostionList = value;
                OnPropertyChanged("ArticleDecompostionList");
            }
        }
        Int32 idRevisionItem;
        [Column("IdArticle")]
        [DataMember]
        public Int32 IdRevisionItem
        {
            get { return idRevisionItem; }
            set
            {
                idRevisionItem = value;
                OnPropertyChanged("IdRevisionItem");
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
    }
}
