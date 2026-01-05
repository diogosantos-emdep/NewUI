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

namespace Emdep.Geos.Data.Common
{
    [Table("warehousepurchaseorders")]
    [DataContract]
    [KnownType(typeof(System.Windows.Media.Imaging.BitmapImage))]
    public class WarehousePurchaseOrder : ModelBase, IDisposable
    {
        #region Declaration
        string dEPostCode = string.Empty;
        string dEAddress = string.Empty;
        string dETelephone = string.Empty;
        string dECity = string.Empty;
        string deliveryAddressCountry = string.Empty;
        string purchasingContactName = string.Empty;
        string purchasingContactEmail = string.Empty;
        string remarks = string.Empty;
        string deliveryAddressState = string.Empty;
        string incoterms;
        string paymentTerms;
        byte[] templatefilebytes;
        Int64 idWarehousePurchaseOrder;
        string serie;
        Int64 number;
        string code;
        Int64 idArticleSupplier;
        ArticleSupplier articleSupplier;
        DateTime? deliveryDate;
        Int64 idPaymentType;
        Int32 idTransportAgency;
        string comments;
        DateTime createdIn;
        Int64 createdBy;
        DateTime modifiedIn;
        Int64 modifiedBy;
        Byte isClosed;
        Int64? idWarehouse;
        string reasonClosed;
        Byte idCurrency;
        Byte attachedPO;
        int? delay;
        DateTime? warehouseHolidayStartdate;
        DateTime? warehouseHolidayEnddate;
        string holidaytype;
        //Not Mapped
        Int32 deliveries;
        DateTime? reminderEmailDate;
        DateTime? latestDeliveryDate;
        DateTime? firstDeliveryDate;
        Int32 status;
        List<WarehousePurchaseOrderItem> warehousePurchaseOrderItems;
        List<WarehousePurchaseOrderItemWithComments> warehousePurchaseOrderItemWithComments;
        List<WarehouseDeliveryNote> warehouseDeliveryNotes;
        int itemCount;
        #region GEOS2-4451
        //-----[pramod.misal][GEOS2-4451][24/07/2023]
        List<WarehouseShippingAddress> warehouseShippingAddress;//[pramod.misal][GEOS2-4451][24/07/2023]
        //Int64 idShippingAddress;
        string name;
        string address;
        string zipCode;
        string city;
        string region;
        Int64 idCountry;
        //string comments;
        Int64 idSite;
        Int64 isDefault;
        Int64 isDisabled;
        //-----[pramod.misal][GEOS2-4451][24/07/2023]

        #endregion GEOS2-4451

        bool isPartialPending;
        Warehouses warehouse;

        decimal totalAmount;
        string attachPdf;
        People creator;
        People modifier;

        List<LogEntriesByWarehousePO> warehousePOLogEntries;
        List<LogEntriesByWarehousePO> warehousePOComments;
        //List<LogEntriesByWarehousePO> warehouseShippingAddress;  //[pramod.misal][GEOS2-4451][24/07/2023]

        Currency currency;

        WorkflowStatus workflowStatus;
        byte idWorkflowStatus;

        string emailBody;
        byte[] attachmentBytes;

        Dictionary<string, List<long>> supplierEmailId;
        List<string> suppliername;
        bool isSendMail;

        UInt32? idAssignee;
        UInt32? idApprover;
        List<WorkflowStatus> lstWorkflowStatus;
        List<WorkflowStatus> lstWorkflowStatusTransition;
        List<WorkflowTransition> lstAllWorkflowStatusTransitions;
        bool isUpdatedRow;
        string statusHTMLColor; //[Sudhir.jangra][GEOS2-4308][11/04/2023]
        string assigneeEmail; //[Sudhir.Jangra][GEOS2-4407][19/05/2023]
        string creatorEmail; //[Sudhir.Jangra][GEOS2-4407][19/05/2023]
        string warehouseName; //[Sudhir.jangra][GEOS2-4487][30/05/2023]
        string supplierComments; //[pramod.misal][GEOS2-4449][04/06/2023]

        Int64 idShippingAddress; //[pramod.misal][GEOS2-4451][24/07/2023]
        byte[] countryIconbytes;  // [pallavi.kale][26-02-2025][GEOS2-7012]
        string currencyIcon;
        #endregion

        #region Properties

        [Key]
        [Column("IdWarehousePurchaseOrder")]
        [DataMember]
        public Int64 IdWarehousePurchaseOrder
        {
            get
            {
                return idWarehousePurchaseOrder;
            }
            set
            {
                idWarehousePurchaseOrder = value;
                OnPropertyChanged("IdWarehousePurchaseOrder");
            }
        }

        [DataMember]
        public bool IsUpdatedRow
        {
            get
            {
                return isUpdatedRow;
            }

            set
            {
                isUpdatedRow = value;
                OnPropertyChanged("IsUpdatedRow");
            }
        }

        [Column("Serie")]
        [DataMember]
        public string Serie
        {
            get
            {
                return serie;
            }
            set
            {
                serie = value;
                OnPropertyChanged("Serie");
            }
        }

        [Column("Holidaytype")]
        [DataMember]
        public string Holidaytype
        {
            get
            {
                return holidaytype;
            }
            set
            {
                holidaytype = value;
                OnPropertyChanged("Holidaytype");
            }
        }
        [Column("Number")]
        [DataMember]
        public Int64 Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }
        [Column("ItemCount")]
        [DataMember]
        public Int32 ItemCount
        {
            get
            {
                return itemCount;
            }
            set
            {
                itemCount = value;
                OnPropertyChanged("ItemCount");
            }
        }

        [Column("Code")]
        [DataMember]
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [Column("IdArticleSupplier")]
        [DataMember]
        public Int64 IdArticleSupplier
        {
            get
            {
                return idArticleSupplier;
            }
            set
            {
                idArticleSupplier = value;
                OnPropertyChanged("IdArticleSupplier");
            }
        }

        [NotMapped]
        [DataMember]
        public ArticleSupplier ArticleSupplier
        {
            get
            {
                return articleSupplier;
            }
            set
            {
                articleSupplier = value;
                OnPropertyChanged("ArticleSupplier");
            }
        }

        [Column("DeliveryDate")]
        [DataMember]
        public DateTime? DeliveryDate
        {
            get
            {
                return deliveryDate;
            }
            set
            {
                deliveryDate = value;
                OnPropertyChanged("DeliveryDate");
            }
        }
        [Column("WarehouseHolidayStartdate")]
        [DataMember]
        public DateTime? WarehouseHolidayStartdate
        {
            get
            {
                return warehouseHolidayStartdate;
            }
            set
            {
                warehouseHolidayStartdate = value;
                OnPropertyChanged("WarehouseHolidayStartdate");
            }
        }


        [Column("WarehouseHolidayEndDate")]
        [DataMember]
        public DateTime? WarehouseHolidayEndDate
        {
            get
            {
                return warehouseHolidayEnddate;
            }
            set
            {
                warehouseHolidayEnddate = value;
                OnPropertyChanged("WarehouseHolidayEndDate");
            }
        }

        [Column("IdPaymentType")]
        [DataMember]
        public Int64 IdPaymentType
        {
            get
            {
                return idPaymentType;
            }
            set
            {
                idPaymentType = value;
                OnPropertyChanged("IdPaymentType");
            }
        }

        [Column("IdTransportAgency")]
        [DataMember]
        public Int32 IdTransportAgency
        {
            get
            {
                return idTransportAgency;
            }
            set
            {
                idTransportAgency = value;
                OnPropertyChanged("IdTransportAgency");
            }
        }

        [Column("Comments")]
        [DataMember]
        public string Comments
        {
            get
            {
                return comments;
            }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        [Column("CreatedIn")]
        [DataMember]
        public DateTime CreatedIn
        {
            get
            {
                return createdIn;
            }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [Column("CreatedBy")]
        [DataMember]
        public Int64 CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime ModifiedIn
        {
            get
            {
                return modifiedIn;
            }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public Int64 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [Column("IsClosed")]
        [DataMember]
        public Byte IsClosed
        {
            get
            {
                return isClosed;
            }
            set
            {
                isClosed = value;
                OnPropertyChanged("IsClosed");
            }
        }

        [Column("IdWarehouse")]
        [DataMember]
        public Int64? IdWarehouse
        {
            get
            {
                return idWarehouse;
            }
            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }

        [Column("ReasonClosed")]
        [DataMember]
        public string ReasonClosed
        {
            get
            {
                return reasonClosed;
            }
            set
            {
                reasonClosed = value;
                OnPropertyChanged("ReasonClosed");
            }
        }

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

        [Column("AttachedPO")]
        [DataMember]
        public Byte AttachedPO
        {
            get
            {
                return attachedPO;
            }
            set
            {
                attachedPO = value;
                OnPropertyChanged("AttachedPO");
            }
        }

        [Column("ReminderEmailDate")]
        [DataMember]
        public DateTime? ReminderEmailDate
        {
            get
            {
                return reminderEmailDate;
            }
            set
            {
                reminderEmailDate = value;
                OnPropertyChanged("ReminderEmailDate");
            }
        }

        [NotMapped]
        [DataMember]
        public int? Delay
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
        public Int32 Deliveries
        {
            get
            {
                return deliveries;
            }
            set
            {
                deliveries = value;
                OnPropertyChanged("Deliveries");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? LatestDeliveryDate
        {
            get
            {
                return latestDeliveryDate;
            }
            set
            {
                latestDeliveryDate = value;
                OnPropertyChanged("LatestDeliveryDate");
            }
        }
        [NotMapped]
        [DataMember]
        public DateTime? FirstDeliveryDate
        {
            get
            {
                return firstDeliveryDate;
            }
            set
            {
                firstDeliveryDate = value;
                OnPropertyChanged("FirstDeliveryDate");
            }
        }
        [NotMapped]
        [DataMember]
        public Int32 Status
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
        public List<WarehousePurchaseOrderItem> WarehousePurchaseOrderItems
        {
            get
            {
                return warehousePurchaseOrderItems;
            }
            set
            {
                warehousePurchaseOrderItems = value;
                OnPropertyChanged("WarehousePurchaseOrderItems");
            }
        }

        [NotMapped]
        [DataMember]
        public List<WarehousePurchaseOrderItemWithComments> WarehousePurchaseOrderItemWithComments
        {
            get
            {
                return warehousePurchaseOrderItemWithComments;
            }
            set
            {
                warehousePurchaseOrderItemWithComments = value;
                OnPropertyChanged("WarehousePurchaseOrderItems");
            }
        }

        [NotMapped]
        [DataMember]
        public List<WarehouseDeliveryNote> WarehouseDeliveryNotes
        {
            get
            {
                return warehouseDeliveryNotes;
            }
            set
            {
                warehouseDeliveryNotes = value;
                OnPropertyChanged("WarehouseDeliveryNotes");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsPartialPending
        {
            get
            {
                return isPartialPending;
            }
            set
            {
                isPartialPending = value;
                OnPropertyChanged("IsPartialPending");
            }
        }

        [NotMapped]
        [DataMember]
        public Warehouses Warehouse
        {
            get
            {
                return warehouse;
            }
            set
            {
                warehouse = value;
                OnPropertyChanged("Warehouse");
            }
        }

        [NotMapped]
        [DataMember]
        public decimal TotalAmount
        {
            get
            {
                return totalAmount;
            }

            set
            {
                totalAmount = value;
                OnPropertyChanged("TotalAmount");
            }
        }

        [NotMapped]
        [DataMember]
        public string AttachPdf
        {
            get
            {
                return attachPdf;
            }

            set
            {
                attachPdf = value;
                OnPropertyChanged("AttachPdf");
            }
        }

        [NotMapped]
        [DataMember]
        public People Creator
        {
            get
            {
                return creator;
            }

            set
            {
                creator = value;
                OnPropertyChanged("Creator");
            }
        }

        [NotMapped]
        [DataMember]
        public People Modifier
        {
            get
            {
                return modifier;
            }

            set
            {
                modifier = value;
                OnPropertyChanged("Modifier");
            }
        }

        [NotMapped]
        [DataMember]
        public List<LogEntriesByWarehousePO> WarehousePOLogEntries
        {
            get
            {
                return warehousePOLogEntries;
            }

            set
            {
                warehousePOLogEntries = value;
                OnPropertyChanged("WarehousePOLogEntries");
            }
        }


        //[pramod.misal][GEOS2-4550][05/08/2023]
        [NotMapped]
        [DataMember]
        public List<LogEntriesByWarehousePO> WarehousePOComments
        {
            get
            {
                return warehousePOComments;
            }

            set
            {
                warehousePOComments = value;
                OnPropertyChanged("WarehousePOComments");
            }
        }



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
        public string EmailBody
        {
            get
            {
                return emailBody;
            }

            set
            {
                emailBody = value;
                OnPropertyChanged("EmailBody");
            }
        }
        [NotMapped]
        [DataMember]
        public byte[] AttachmentBytes
        {
            get
            {
                return attachmentBytes;
            }

            set
            {
                attachmentBytes = value;
                OnPropertyChanged("AttachmentBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public Dictionary<string, List<long>> SupplierEmailId
        {
            get
            {
                return supplierEmailId;
            }

            set
            {
                supplierEmailId = value;
                OnPropertyChanged("SupplierEmailId");
            }
        }

        [NotMapped]
        [DataMember]
        public List<string> Suppliername
        {
            get
            {
                return suppliername;
            }

            set
            {
                suppliername = value;
                OnPropertyChanged("Suppliername");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsSendMail
        {
            get
            {
                return isSendMail;
            }

            set
            {
                isSendMail = value;
                OnPropertyChanged("IsSendMail");
            }
        }

        [NotMapped]
        [DataMember]
        public uint? IdAssignee
        {
            get
            {
                return idAssignee;
            }

            set
            {
                idAssignee = value;
                OnPropertyChanged("IdAssignee");
            }
        }

        [NotMapped]
        [DataMember]
        public uint? IdApprover
        {
            get
            {
                return idApprover;
            }

            set
            {
                idApprover = value;
                OnPropertyChanged("IdApprover");
            }
        }

        [NotMapped]
        [DataMember]
        public List<WorkflowStatus> LstWorkflowStatus
        {
            get
            {
                return lstWorkflowStatus;
            }

            set
            {
                lstWorkflowStatus = value;
                OnPropertyChanged("LstWorkflowStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public List<WorkflowStatus> LstWorkflowStatusTransition
        {
            get
            {
                return lstWorkflowStatusTransition;
            }

            set
            {
                lstWorkflowStatusTransition = value;
                OnPropertyChanged("LstWorkflowStatusTransition");
            }
        }

        [NotMapped]
        [DataMember]
        public List<WorkflowTransition> LstAllWorkflowStatusTransitions
        {
            get
            {
                return lstAllWorkflowStatusTransitions;
            }

            set
            {
                lstAllWorkflowStatusTransitions = value;
                OnPropertyChanged("LstAllWorkflowStatusTransitions");
            }
        }
        //[Sudhir.Jangra][GEOS2-4308][11/04/2023]
        [DataMember]
        public string StatusHTMLColor
        {
            get
            {
                return statusHTMLColor;
            }
            set
            {
                statusHTMLColor = value;
                OnPropertyChanged("StatusHTMLColor");
            }
        }

        //[Sudhir.Jangra][GEOS2-4407][19/05/2023]
        [Column("CompanyEmail")]
        [DataMember]
        public string AssigneeEmail
        {
            get
            {
                return assigneeEmail;
            }
            set
            {
                assigneeEmail = value;
                OnPropertyChanged("AssigneeEmail");
            }
        }

        //[Sudhir.Jangra][GEOS2-4407][19/05/2023]
        [Column("Email")]
        [DataMember]
        public string CreatorEmail
        {
            get
            {
                return creatorEmail;
            }
            set
            {
                creatorEmail = value;
                OnPropertyChanged("CreatorEmail");
            }
        }

        [NotMapped]
        [DataMember]
        public string WarehouseName
        {
            get
            {
                return warehouseName;
            }
            set
            {
                warehouseName = value;
                OnPropertyChanged("WarehouseName");
            }
        }

        //[pramod.misal][GEOS2-4449][04/06/2023]
        [Column("SupplierComments")]
        [DataMember]
        public string SupplierComments
        {
            get
            {
                return supplierComments;
            }
            set
            {
                supplierComments = value;
                OnPropertyChanged("SupplierComments");
            }
        }
        // [pallavi.kale][26-02-2025][GEOS2-7012]
        public byte[] CountryIconBytes
        {
            get { return countryIconbytes; }
            set
            {
                countryIconbytes = value;
                OnPropertyChanged("CountryIconBytes");
            }
        }
        // [pallavi.kale][26-02-2025][GEOS2-7012]
        [DataMember]
        public string CurrencyIcon
        {
            get { return currencyIcon; }
            set
            {
                currencyIcon = value;
                OnPropertyChanged("CurrencyIcon");
            }
        }

        #region GEOS2-4453
        [NotMapped]
        [DataMember]
        public string DEPostCode
        {
            get
            {
                return dEPostCode;
            }
            set
            {
                dEPostCode = value;
                OnPropertyChanged("DEPostCode");
            }
        }
        [NotMapped]
        [DataMember]
        public string DEAddress
        {
            get
            {
                return dEAddress;
            }
            set
            {
                dEAddress = value;
                OnPropertyChanged("DEAddress");
            }
        }
        [NotMapped]
        [DataMember]
        public string DETelephone
        {
            get
            {
                return dETelephone;
            }
            set
            {
                dETelephone = value;
                OnPropertyChanged("DETelephone");
            }
        }
        [NotMapped]
        [DataMember]
        public string DECity
        {
            get
            {
                return dECity;
            }
            set
            {
                dECity = value;
                OnPropertyChanged("DECity");
            }
        }
        [NotMapped]
        [DataMember]
        public string DeliveryAddressCountry
        {
            get
            {
                return deliveryAddressCountry;
            }
            set
            {
                deliveryAddressCountry = value;
                OnPropertyChanged("DeliveryAddressCountry");
            }
        }
        [NotMapped]
        [DataMember]
        public string PurchasingContactName
        {
            get
            {
                return purchasingContactName;
            }
            set
            {
                purchasingContactName = value;
                OnPropertyChanged("PurchasingContactName");
            }
        }
        [NotMapped]
        [DataMember]
        public string PurchasingContactEmail
        {
            get
            {
                return purchasingContactEmail;
            }
            set
            {
                purchasingContactEmail = value;
                OnPropertyChanged("PurchasingContactEmail");
            }
        }
        [NotMapped]
        [DataMember]
        public string DeliveryAddressState
        {
            get
            {
                return deliveryAddressState;
            }
            set
            {
                deliveryAddressState = value;
                OnPropertyChanged("DeliveryAddressState");
            }
        }
        [NotMapped]
        [DataMember]
        public string Remarks
        {
            get
            {
                return remarks;
            }
            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        [NotMapped]
        [DataMember]
        public string Incoterms
        {
            get
            {
                return incoterms;
            }
            set
            {
                incoterms = value;
                OnPropertyChanged("Incoterms");
            }
        }

        [NotMapped]
        [DataMember]
        public string PaymentTerms
        {
            get
            {
                return paymentTerms;
            }
            set
            {
                paymentTerms = value;
                OnPropertyChanged("PaymentTerms");
            }
        }
        [NotMapped]
        [DataMember]
        public byte[] Templatefilebytes
        {
            get
            {
                return templatefilebytes;
            }
            set
            {
                templatefilebytes = value;
                OnPropertyChanged("Templatefilebytes");
            }
        }
        string year;
        [NotMapped]
        [DataMember]
        public string Year
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
        #endregion


        #region GEOS-4451
        //-------------[pramod.misal][GEOS2-4451][24/07/2023]
        [Key]
        [Column("IdShippingAddress")]
        [DataMember]
        public Int64 IdShippingAddress
        {
            get { return idShippingAddress; }
            set
            {
                idShippingAddress = value;
                OnPropertyChanged("IdShippingAddress");
            }
        }


        //[pramod.misal][GEOS2-4451][24/07/2023]
        [NotMapped]
        [DataMember]
        public List<WarehouseShippingAddress> WarehouseShippingAddress
        {
            get
            {
                return warehouseShippingAddress;
            }
            set
            {
                warehouseShippingAddress = value;
                OnPropertyChanged("WarehouseShippingAddress");
            }
        }


        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }


        [Column("Address")]
        [DataMember]
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }

        [Column("ZipCode")]
        [DataMember]
        public string ZipCode
        {
            get { return zipCode; }
            set
            {
                zipCode = value;
                OnPropertyChanged("ZipCode");
            }
        }


        [Column("City")]
        [DataMember]
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        }


        [Column("Region")]
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

        [Key]
        [Column("IdCountry")]
        [DataMember]
        public Int64 IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }


        [Key]
        [Column("IdSite")]
        [DataMember]
        public Int64 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }


        [Key]
        [Column("IsDefault")]
        [DataMember]
        public Int64 IsDefault
        {
            get { return isDefault; }
            set
            {
                isDefault = value;
                OnPropertyChanged("IsDefault");
            }
        }

        [Key]
        [Column("IsDisabled")]
        [DataMember]
        public Int64 IsDisabled
        {
            get { return isDisabled; }
            set
            {
                isDisabled = value;
                OnPropertyChanged("IsDisabled");
            }
        }

        #endregion GEOS-4451

        //Shubham[skadam]  GEOS2-4713 Missing Delivery Dates 31 07 2023 
        DateTime? pODeliveryDate;
        [DataMember]
        public DateTime? PODeliveryDate
        {
            get
            {
                return pODeliveryDate;
            }
            set
            {
                pODeliveryDate = value;
                OnPropertyChanged("PODeliveryDate");
            }
        }
        //[Rahul.gadhave][GEOS2-7243][Date:17-07-2025]
        private string deliveryAddressFormatted = string.Empty;

        [NotMapped]
        [DataMember]
        public string DeliveryAddressFormatted
        {
            get
            {
                return deliveryAddressFormatted;
            }
            set
            {
                deliveryAddressFormatted = value;
                OnPropertyChanged("DeliveryAddressFormatted");
            }
        }

        EmdepSite emdepSite;
        Int32 idCustomer;
        [NotMapped]
        [DataMember]
        public EmdepSite EmdepSite
        {
            get
            {
                return emdepSite;
            }
            set
            {
                emdepSite = value;
                OnPropertyChanged("EmdepSite");
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
        string customerName;
        [NotMapped]
        [DataMember]
        public string CustomerName
        {
            get
            {
                return customerName;
            }
            set
            {
                customerName = value;
                OnPropertyChanged("CustomerName");
            }
        }
        #endregion

        #region Constructor

        public WarehousePurchaseOrder() { }

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