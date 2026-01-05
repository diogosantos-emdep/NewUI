using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class WeeklyProductionReport : ModelBase, IDisposable
    {
        #region Field
        private Int64 idOT;
        //private Int64 idRevisionItem;
        private Int64 idCPType;
        //private Int64 idItemOtStatus;
        private Int32 idTemplate;
        private Int32 idSite;
        // private Int32 orderNumber;
        //private float? equivalentWeight;
        private double equivalentWeight;
        private double finalEquivalentWeight;
        //private string code;
        private DateTime? itemStatustrackingStartdate;
        private DateTime? itemStatustrackingEndDate;
        //private DateTime? startDate;
       // private DateTime? endDate;
        private string cPTypeName;
        //private string status;
        private string startDateWeek;
        private string endDateWeek;
        private string templateName;
        private string siteName;
        private string dayName;
        private Int64 qTY;
        List<WindowsServicesHolidays> windowsServicesHolidaysList;
        private double? unit;   //[GEOS2-4483][gulab lakade][31 05 2023]
        private double? unitInEUR;   //[GEOS2-4483][gulab lakade][31 05 2023]
        private double? unitPriceInEUR;   //[GEOS2-4483][gulab lakade][31 05 2023]
        private double? unitPrice;   //[GEOS2-4483][gulab lakade][31 05 2023]
        private string currenceSymbol;   //[GEOS2-4483][gulab lakade][31 05 2023]
        private string currencyName;   //[GEOS2-4483][gulab lakade][31 05 2023]
        private string offercurrenceSymbol;   //[GEOS2-4605][gulab lakade][26 06 2023]
        private string offercurrencyName;   //[GEOS2-4605][gulab lakade][26 06 2023]
        private Int64 rework;  //[GEOS2-4921][gulab lakade][30 10 2023]
        private string serialNumber; // [Rupali Sarode][GEOS2-5521][27-03-2024]
        private bool isBatch; // [Rupali Sarode][GEOS2-5521][27-03-2024]
        private Int64 idOTItem; // [Rupali Sarode][GEOS2-5521][27-03-2024]
        private Int64 idDrawing; // [Rupali Sarode][GEOS2-5521][27-03-2024]
        private Int64 idCounterpart; // [Rupali Sarode][GEOS2-5521][27-03-2024]
        private List<Counterpartstracking> counterpartsTrackingList; // [Rupali Sarode][GEOS2-5521][27-03-2024]
        private string currentWorkStation;   //[GEOS2-5518][gulab lakade][22 04 2024]
        private Int32? currentIdStage;   //[GEOS2-5518][gulab lakade][22 04 2024]
        private string availableDayName;//[GEOS2-6058][gulab lakade][02 10 2024]
        private Int32? endDate_Idstage;//[GEOS2-6058][gulab lakade][02 10 2024
        private Int32? startDateWeekINT;//[GEOS2-6058][gulab lakade][02 10 2024]
        private Int32? endDateWeekINT;//[GEOS2-6058][gulab lakade][02 10 2024]
        private DateTime? oTItemStatusChangesDate;//[GEOS2-6900][gulab lakade][28 01 2025]
        private Int32? oTItemStatusChangesDateWeek;//[GEOS2-6900][gulab lakade][28 01 2025]
        private string itemStatus;//[GEOS2-6900][gulab lakade][28 01 2025]
        private Int32 idItemstatus;//[GEOS2-6900][gulab lakade][28 01 2025]
        private Int64 idCP;//[pallavi.jadhav][29 04 2025][GEOS2-7066]
        private string drawingType; //[GEOS2-8382][gulab lakade][09 06 2025]
        private bool expectedHtmlColorFlag;//[GEOS2-8376][gulab lakade][09 06 2025]
        private List<Counterpartstracking> counterpartsTrackingList_SCAN_RW_STD; // [GEOS2-8378][gulab lakade][12 06 2025]
        #endregion
        #region Property
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
        //[DataMember]
        //public Int64 IdRevisionItem
        //{
        //    get { return idRevisionItem; }
        //    set
        //    {
        //        idRevisionItem = value;
        //        OnPropertyChanged("IdRevisionItem");
        //    }
        //}
        [DataMember]
        public Int64 IdCPType
        {
            get { return idCPType; }
            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
            }
        }
        //[DataMember]
        //public Int64 IdItemOtStatus
        //{
        //    get { return idItemOtStatus; }
        //    set
        //    {
        //        idItemOtStatus = value;
        //        OnPropertyChanged("IdItemOtStatus");
        //    }
        //}
        [DataMember]
        public Int32 IdTemplate
        {
            get { return idTemplate; }
            set
            {
                idTemplate = value;
                OnPropertyChanged("IdTemplate");
            }
        }
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
        //[DataMember]
        //public Int32 OrderNumber
        //{
        //    get { return orderNumber; }
        //    set
        //    {
        //        orderNumber = value;
        //        OnPropertyChanged("OrderNumber");
        //    }
        //}
        //[DataMember]
        //public float? EquivalentWeight
        //{
        //    get
        //    {
        //        return equivalentWeight;
        //    }

        //    set
        //    {
        //        equivalentWeight = value;
        //        OnPropertyChanged("EquivalentWeight");
        //    }
        //}
        [DataMember]
        public double FinalEquivalentWeight
        {
            get
            {
                return finalEquivalentWeight;
            }

            set
            {
                finalEquivalentWeight = value;
                OnPropertyChanged("FinalEquivalentWeight");
            }
        }

        [DataMember]
        public double EquivalentWeight
        {
            get
            {
                return equivalentWeight;
            }

            set
            {
                equivalentWeight = value;
                OnPropertyChanged("EquivalentWeight");
            }
        }


        


        //[DataMember]
        //public string Code
        //{
        //    get
        //    {
        //        return code;
        //    }

        //    set
        //    {
        //        code = value;
        //        OnPropertyChanged("Code");
        //    }
        //}
        [DataMember]
        public DateTime? ItemStatustrackingStartdate
        {
            get
            {
                return itemStatustrackingStartdate;
            }

            set
            {
                itemStatustrackingStartdate = value;
                OnPropertyChanged("ItemStatustrackingStartdate");
            }
        }
        [DataMember]
        public DateTime? ItemStatustrackingEndDate
        {
            get
            {
                return itemStatustrackingEndDate;
            }

            set
            {
                itemStatustrackingEndDate = value;
                OnPropertyChanged("ItemStatustrackingEndDate");
            }
        }
        //[DataMember]
        //public DateTime? StartDate
        //{
        //    get
        //    {
        //        return startDate;
        //    }

        //    set
        //    {
        //        startDate = value;
        //        OnPropertyChanged("StartDate");
        //    }
        //}
        //[DataMember]
        //public DateTime? EndDate
        //{
        //    get
        //    {
        //        return endDate;
        //    }

        //    set
        //    {
        //        endDate = value;
        //        OnPropertyChanged("EndDate");
        //    }
        //}
        [DataMember]
        public string CPTypeName
        {
            get
            {
                return cPTypeName;
            }

            set
            {
                cPTypeName = value;
                OnPropertyChanged("CPTypeName");
            }
        }
        //[DataMember]
        //public string Status
        //{
        //    get
        //    {
        //        return status;
        //    }

        //    set
        //    {
        //        status = value;
        //        OnPropertyChanged("Status");
        //    }
        //}
        [DataMember]
        public string StartDateWeek
        {
            get
            {
                return startDateWeek;
            }

            set
            {
                startDateWeek = value;
                OnPropertyChanged("StartDateWeek");
            }
        }
        [DataMember]
        public string EndDateWeek
        {
            get
            {
                return endDateWeek;
            }

            set
            {
                endDateWeek = value;
                OnPropertyChanged("EndDateWeek");
            }
        }
        [DataMember]
        public string TemplateName
        {
            get
            {
                return templateName;
            }

            set
            {
                templateName = value;
                OnPropertyChanged("TemplateName");
            }
        }
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
        [DataMember]
        public string DayName
        {
            get
            {
                return dayName;
            }

            set
            {
                dayName = value;
                OnPropertyChanged("DayName");
            }
        }

        [DataMember]
        public Int64 QTY
        {
            get { return qTY; }
            set
            {
                qTY = value;
                OnPropertyChanged("QTY");
            }
        }
        [DataMember]
        public List<WindowsServicesHolidays> WindowsServicesHolidaysList
        {
            get
            {
                return windowsServicesHolidaysList;
            }

            set
            {
                windowsServicesHolidaysList = value;
                OnPropertyChanged("WindowsServicesHolidaysList");
            }
        }
        [DataMember]        //[GEOS2-4483][gulab lakade][31 05 2023]
        public double? Unit
        {
            get
            {
                return unit;
            }

            set
            {
                unit = value;
                OnPropertyChanged("Unit");
            }
        }
        [DataMember]        //[GEOS2-4483][gulab lakade][31 05 2023]
        public double? UnitInEUR
        {
            get
            {
                return unitInEUR;
            }

            set
            {
                unitInEUR = value;
                OnPropertyChanged("UnitInEUR");
            }
        }
        [DataMember]        //[GEOS2-4483][gulab lakade][31 05 2023]
        public double? UnitPriceInEUR
        {
            get
            {
                return unitPriceInEUR;
            }

            set
            {
                unitPriceInEUR = value;
                OnPropertyChanged("UnitPriceInEUR");
            }
        }
        [DataMember]        //[GEOS2-4483][gulab lakade][31 05 2023]
        public double? UnitPrice
        {
            get
            {
                return unitPrice;
            }

            set
            {
                unitPrice = value;
                OnPropertyChanged("UnitPrice");
            }
        }
        [DataMember]        //[GEOS2-4483][gulab lakade][31 05 2023]
        public string CurrenceSymbol
        {
            get
            {
                return currenceSymbol;
            }

            set
            {
                currenceSymbol = value;
                OnPropertyChanged("CurrenceSymbol");
            }
        }
        [DataMember]        //[GEOS2-4483][gulab lakade][31 05 2023]
        public string CurrencyName
        {
            get
            {
                return currencyName;
            }

            set
            {
                currencyName = value;
                OnPropertyChanged("CurrencyName");
            }
        }

        [DataMember]        //[GEOS2-4605][gulab lakade][26 06 2023]
        public string OffercurrenceSymbol
        {
            get
            {
                return offercurrenceSymbol;
            }

            set
            {
                offercurrenceSymbol = value;
                OnPropertyChanged("OffercurrenceSymbol");
            }
        }
        [DataMember]         //[GEOS2-4605][gulab lakade][26 06 2023]
        public string OffercurrencyName
        {
            get
            {
                return offercurrencyName;
            }

            set
            {
                offercurrencyName = value;
                OnPropertyChanged("OffercurrencyName");
            }
        }
        [DataMember]         //[GEOS2-4921][gulab lakade][30 10 2023]
        public Int64 Rework
        {
            get
            {
                return rework;
            }

            set
            {
                rework = value;
                OnPropertyChanged("Rework");
            }
        }

        [DataMember]
        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }

            set
            {
                serialNumber = value;
                OnPropertyChanged("SerialNumber");
            }
        }

        [DataMember]
        public bool IsBatch
        {
            get
            {
                return isBatch;
            }

            set
            {
                isBatch = value;
                OnPropertyChanged("IsBatch");
            }
        }

        [DataMember]
        public Int64 IdOTItem
        {
            get
            {
                return idOTItem;
            }

            set
            {
                idOTItem = value;
                OnPropertyChanged("IdOTItem");
            }
        }

        [DataMember]
        public Int64 IdDrawing
        {
            get
            {
                return idDrawing;
            }

            set
            {
                idDrawing = value;
                OnPropertyChanged("IdDrawing");
            }
        }

        [DataMember]
        public Int64 IdCounterpart
        {
            get
            {
                return idCounterpart;
            }

            set
            {
                idCounterpart = value;
                OnPropertyChanged("IdCounterpart");
            }
        }

        [DataMember]
        public List<Counterpartstracking> CounterpartsTrackingList
        {
            get
            {
                return counterpartsTrackingList;
            }
            set
            {
                counterpartsTrackingList = value;
                OnPropertyChanged("CounterpartsTrackingList");
            }
        }
        #region [GEOS2-5518][gulab lakade][22 04 2024]
        [DataMember]
        public string CurrentWorkStation
        {
            get
            {
                return currentWorkStation;
            }

            set
            {
                currentWorkStation = value;
                OnPropertyChanged("CurrentWorkStation");
            }
        }
        [DataMember]
        public Int32? CurrentIdStage
        {
            get
            {
                return currentIdStage;
            }

            set
            {
                currentIdStage = value;
                OnPropertyChanged("CurrentIdStage");
            }
        }
        #endregion
        //[GEOS2-6058][gulab lakade][02 10 2024]
        [DataMember]
        public string AvailableDayName
        {
            get
            {
                return availableDayName;
            }

            set
            {
                availableDayName = value;
                OnPropertyChanged("AvailableDayName");
            }
        }
        [DataMember]
        public Int32? EndDate_Idstage
        {
            get
            {
                return endDate_Idstage;
            }

            set
            {
                endDate_Idstage = value;
                OnPropertyChanged("EndDate_Idstage");
            }
        }
        [DataMember]
        public Int32? StartDateWeekINT
        {
            get
            {
                return startDateWeekINT;
            }

            set
            {
                startDateWeekINT = value;
                OnPropertyChanged("StartDateWeekINT");
            }
        }
        [DataMember]
        public Int32? EndDateWeekINT
        {
            get
            {
                return endDateWeekINT;
            }

            set
            {
                endDateWeekINT = value;
                OnPropertyChanged("EndDateWeekINT");
            }
        }
        //[GEOS2-6058][gulab lakade][02 10 2024]
        #region [GEOS2-6900][gulab lakade][28 01 2025]
        [DataMember]
        public DateTime? OTItemStatusChangesDate
        {
            get
            {
                return oTItemStatusChangesDate;
            }

            set
            {
                oTItemStatusChangesDate = value;
                OnPropertyChanged("OTItemStatusChangesDate");
            }
        }
        [DataMember]
        public Int32? OTItemStatusChangesDateWeek
        {
            get
            {
                return oTItemStatusChangesDateWeek;
            }

            set
            {
                oTItemStatusChangesDateWeek = value;
                OnPropertyChanged("OTItemStatusChangesDateWeek");
            }
        }
        [DataMember]
        public string ItemStatus
        {
            get
            {
                return itemStatus;
            }

            set
            {
                itemStatus = value;
                OnPropertyChanged("ItemStatus");
            }
        }
        [DataMember]
        public Int32 IdItemstatus
        {
            get { return idItemstatus; }
            set
            {
                idItemstatus = value;
                OnPropertyChanged("IdItemstatus");
            }
        }
        #endregion

        #region //[pallavi.jadhav][29 04 2025][GEOS2-7066]

        [DataMember]
        public Int64 IdCP
        {
            get
            {
                return idCP;
            }

            set
            {
                idCP = value;
                OnPropertyChanged("IdCP");
            }
        }
        #endregion
        //[GEOS2-8382][gulab lakade][09 06 2025]
        [DataMember]
        public string DrawingType
        {
            get
            {
                return drawingType;
            }

            set
            {
                drawingType = value;
                OnPropertyChanged("DrawingType");
            }
        }
        //start [GEOS2-8376][gulab lakade][04 06 2025]
        [DataMember]
        public bool ExpectedHtmlColorFlag
        {
            get
            {
                return expectedHtmlColorFlag;
            }

            set
            {
                expectedHtmlColorFlag = value;
                OnPropertyChanged("ExpectedHtmlColorFlag");
            }
        }
        //end[GEOS2-8376][gulab lakade][04 06 2025]
        // [GEOS2-8378][gulab lakade][12 06 2025]
        [DataMember]
        public List<Counterpartstracking> CounterpartsTrackingList_SCAN_RW_STD
        {
            get
            {
                return counterpartsTrackingList_SCAN_RW_STD;
            }
            set
            {
                counterpartsTrackingList_SCAN_RW_STD = value;
                OnPropertyChanged("CounterpartsTrackingList_SCAN_RW_STD");
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
