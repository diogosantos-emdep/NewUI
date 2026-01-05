using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    [DataContract]
    //[pjadhav][APIGEOS-698][27/01/2023]
    public class TimetrackingAPI
    {
        private int _Id = 0;
        [IgnoreDataMember]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private string _DeliveryWeek = string.Empty;
        [DataMember(Order = 1)]
        public string DeliveryWeek
        {
            get { return _DeliveryWeek; }
            set { _DeliveryWeek = value; }
        }
        private string _DeliveryDate;
        [DataMember(Order = 2)]
        public string DeliveryDate
        {
            get { return _DeliveryDate; }
            set { _DeliveryDate = value; }
        }
        private string _PlannedDeliveryDate;
        [DataMember(Order = 3)]
        public string PlannedDeliveryDate
        {
            get { return _PlannedDeliveryDate; }
            set { _PlannedDeliveryDate = value; }
        }
        private string _FirstDeliveryDate;
        [DataMember(Order = 4)]
        public string FirstDeliveryDate
        {
            get { return _FirstDeliveryDate; }
            set { _FirstDeliveryDate = value; }
        }
        private string _QuoteSendDate = string.Empty;
        [DataMember(Order = 5)]
        public string QuoteSendDate
        {
            get { return _QuoteSendDate; }
            set { _QuoteSendDate = value; }
        }
        private string _Samples = string.Empty;//[plahange][01/03/2023][APIGEOS-701]
        [DataMember(Order = 6)]
        public string Samples
        {
            get { return _Samples; }
            set { _Samples = value; }
        }
        private string _SamplesDate = string.Empty;
        [DataMember(Order = 7)]
        public string SamplesDate
        {
            get { return _SamplesDate; }
            set { _SamplesDate = value; }
        }
        private string _GoAheadDate = string.Empty;
        [DataMember(Order = 8)]
        public string GoAheadDate
        {
            get { return _GoAheadDate; }
            set { _GoAheadDate = value; }
        }
        private string _PODate = string.Empty;
        [DataMember(Order = 9)]
        public string PODate
        {
            get { return _PODate; }
            set { _PODate = value; }
        }
        private string _AvailForDesignDate = string.Empty;
        [DataMember(Order = 10)]
        public string AvailableforDesignDate
        {
            get { return _AvailForDesignDate; }
            set { _AvailForDesignDate = value; }
        }
        private string _POType = string.Empty;
        [DataMember(Order = 11)]
        public string POType
        {
            get { return _POType; }
            set { _POType = value; }
        }
        private string _Customer = string.Empty;
        [DataMember(Order = 12)]
        public string Customer
        {
            get { return _Customer; }
            set { _Customer = value; }
        }
        private string _Offer = string.Empty;
        [DataMember(Order = 13)]
        public string Offer
        {
            get { return _Offer; }
            set { _Offer = value; }
        }
        private string _Project = string.Empty;
        [DataMember(Order = 14)]
        public string Project
        {
            get { return _Project; }
            set { _Project = value; }
        }
        private string _OTCode = string.Empty;
        [DataMember(Order = 15)]
        public string OTCode
        {
            get { return _OTCode; }
            set { _OTCode = value; }
        }
        private string _OriginalPlant = string.Empty;
        [DataMember(Order = 16)]
        public string OriginPlant
        {
            get { return _OriginalPlant; }
            set { _OriginalPlant = value; }
        }
        private string _ProductionPlant = string.Empty;
        [DataMember(Order = 17)]
        public string ProductionPlant
        {
            get { return _ProductionPlant; }
            set { _ProductionPlant = value; }
        }
        private string _CustomerReference = string.Empty;
        [DataMember(Order = 18)]
        public string CustomerReference
        {
            get { return _CustomerReference; }
            set { _CustomerReference = value; }
        }
        private string _Template = string.Empty;
        [DataMember(Order = 19)]
        public string Template
        {
            get { return _Template; }
            set { _Template = value; }
        }
        private string _Type = string.Empty;
        [DataMember(Order = 20)]
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        private Int32 _QTY = 0;
        [DataMember(Order = 21)]
        public Int32 QTY
        {
            get { return _QTY; }
            set { _QTY = value; }
        }
        //private string _CurrencySalePrice = string.Empty;
        //[DataMember(Order = 22)]
        //public string CurrencySalePrice
        //{
        //    get { return _CurrencySalePrice; }
        //    set { _CurrencySalePrice = value; }
        //}

        //rani dhamankar][APIGEOS-1317][28-01-2025]
        [DataMember(Order = 22)]
        public List<ERM_SalesPrices> SalesPrices { get; set; }

        private Double _UnitSalePrice = 0;
        [IgnoreDataMember]
        public Double UnitSalePrice
        {
            get { return _UnitSalePrice; }
            set { _UnitSalePrice = value; }
        }
        private Double _TotalSalePrice = 0;
        [IgnoreDataMember]
        public Double TotalSalePrice
        {
            get { return _TotalSalePrice; }
            set { _TotalSalePrice = value; }
        }
        private string _SerialNumber = string.Empty;
        [DataMember(Order = 25)]
        public string SerialNumber
        {
            get { return _SerialNumber; }
            set { _SerialNumber = value; }
        }
        private string _ItemStatus = string.Empty;
        [DataMember(Order = 26)]
        public string ItemStatus
        {
            get { return _ItemStatus; }
            set { _ItemStatus = value; }
        }
        private string _DesignType = string.Empty;
        [DataMember(Order = 27)]
        public string DesignType
        {
            get { return _DesignType; }
            set { _DesignType = value; }
        }
        private string _Tray = string.Empty;
        [DataMember(Order = 28)]
        public string Tray
        {
            get { return _Tray; }
            set { _Tray = value; }
        }
        private string _CurrentWorkStation = string.Empty;
        [DataMember(Order = 29)]
        public string CurrentWorkStation
        {
            get { return _CurrentWorkStation; }
            set { _CurrentWorkStation = value; }
        }
        private Int32 _TotalRework = 0;
        [DataMember(Order = 30)]
        public Int32 TotalRework
        {
            get { return _TotalRework; }
            set { _TotalRework = value; }
        }

        #region rani changes 
        private Int64 _IdDrawing = 0;
        [DataMember(Order = 31)]
        public Int64 IdDrawing
        {
            get { return _IdDrawing; }
            set { _IdDrawing = value; }
        }
        private string _WorkbookDrawing = string.Empty;
        [DataMember(Order = 32)]
        public string WorkbookDrawing
        {
            get { return _WorkbookDrawing; }
            set { _WorkbookDrawing = value; }
        }
        private string _DesignSystem = string.Empty;
        [DataMember(Order = 33)]
        public string DesignSystem
        {
            get { return _DesignSystem; }
            set { _DesignSystem = value; }
        }
        #endregion

        private Int32 _IdCounterpart = 0;
        [IgnoreDataMember]
        public Int32 IdCounterpart
        {
            get { return _IdCounterpart; }
            set { _IdCounterpart = value; }
        }

        private Int32 _CPProductID = 0;
        [IgnoreDataMember]
        public Int32 CPProductID
        {
            get { return _CPProductID; }
            set { _CPProductID = value; }
        }

        private List<WorkStage> _workStage = new List<WorkStage>();

        [DataMember(Order = 34)]
        public List<WorkStage> WorkStage
        {
            get => _workStage;
            set => _workStage = value ?? new List<WorkStage>();
        }
        //private List<WorkStage> _workStage;
        //[DataMember(Order = 34)]
        //public List<WorkStage> WorkStage
        //{
        //    get
        //    {
        //        return _workStage;
        //    }

        //    set
        //    {
        //        _workStage = value;
        //    }
        //}
        // public List<WorkStage> WorkStage { get; set; }
        public string IdProductSubCategory { get; set; }
        public string IdSalesOwner { get; set; }
        //[IgnoreDataMember]
        //public Int64 IdCP { get; set; }
        private Int64 _IdCP = 0;
        public Int64 IdCP
        {
            get { return _IdCP; }

            set { _IdCP = value; }
        }
        private UInt64 idShippingAddress;

        [IgnoreDataMember]
        public UInt64 IdShippingAddress
        {
            get
            {
                return idShippingAddress;
            }

            set
            {
                idShippingAddress = value;
            }
        }
        public List<SiteAPI> siteList { get; set; }

        private bool isBatch;
        [IgnoreDataMember]
        public bool IsBatch
        {
            get
            {
                return isBatch;
            }

            set
            {
                isBatch = value;

            }
        }

        //private Int64 idDrawing;
        //[IgnoreDataMember]
        //public Int64 IdDrawing
        //{
        //    get
        //    {
        //        return idDrawing;
        //    }

        //    set
        //    {
        //        idDrawing = value;
        //    }
        //}
        private Int64 idOTItem;
        [IgnoreDataMember]
        public Int64 IdOTItem
        {
            get
            {
                return idOTItem;
            }

            set
            {
                idOTItem = value;
            }
        }
        private Int64 idOt;
        [IgnoreDataMember]
        public Int64 IdOt
        {
            get
            {
                return idOt;
            }

            set
            {
                idOt = value;
            }
        }
        [IgnoreDataMember]
        public List<WorkStage> TimeTrackingStage { get; set; }
        private Int32 detectiondrawing = 0;
        [IgnoreDataMember]
        public Int32 Detectiondrawing
        {
            get
            {
                return detectiondrawing;
            }

            set
            {
                detectiondrawing = value;
            }
        }

        private Int32 draw_quantity = 0;
        [IgnoreDataMember]
        public Int32 Draw_quantity
        {
            get
            {
                return draw_quantity;
            }

            set
            {
                draw_quantity = value;
            }
        }
        private Int32 cpQty = 0;
        [IgnoreDataMember]
        public Int32 CpQty
        {
            get
            {
                return cpQty;
            }

            set
            {
                cpQty = value;
            }
        }
        private Int32 cp_Detection = 0;
        [IgnoreDataMember]
        public Int32 Cp_Detection
        {
            get
            {
                return cp_Detection;
            }

            set
            {
                cp_Detection = value;
            }
        }
    }
}
