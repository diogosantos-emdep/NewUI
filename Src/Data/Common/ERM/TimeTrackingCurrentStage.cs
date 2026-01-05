using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Mysqlx.Expr;

namespace Emdep.Geos.Data.Common.ERM
{
    public class TimeTrackingCurrentStage: ModelBase, IDisposable
    {
        #region Field
        private int? idStage;
     
        private decimal? real;
        private decimal? expected;
      
        private float? remianing;
        #region [gulab lakade][GEOS2-5466][12 03 2024]
        private DateTime? startdate;
        private DateTime? enddate;
        private int rework;
        private Int32 idCounterparttracking;
        private Double timeDifference;
        private bool paused;
        private decimal? production;
        private int newIdStage;
        private string employeeName;
        #endregion
        #region[pallavi jadhav][GEOS2-5320][24 10 2024]
        private DateTime? plannedDeliveryDateByStage;
        private Int32 days;
        #endregion
        #endregion
        #region [rani dhamankar][31-03-2025][GEOS2-7097]
        private int productionActivityTimeType;
        private string operatorName;
        private TimeSpan timeTrackDifference;
        private Int32 timeTrackIdCounterparttracking;
        private Int64 timeTrackIdCounterpart;
        #endregion
        #region [rani dhamankar][11-09-2025][GEOS2-7091]
        private Int32 idOperator; 
        private string stageCode;
        #endregion

        #region Property
        [DataMember]
        public decimal? Real
        {
            get
            {
                return real;
            }

            set
            {
                real = value;
                OnPropertyChanged("Real");
            }
        }
        public decimal? Expected
        {
            get
            {
                return expected;
            }

            set
            {
                expected = value;
                OnPropertyChanged("Expected");
            }
        }
        public float? Remianing
        {
            get
            {
                return remianing;
            }

            set
            {
                remianing = value;
                OnPropertyChanged("Remianing");
            }
        }

        public int? IdStage
        {
            get
            {
                return idStage;
            }

            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }

        #region [gulab lakade][GEOS2-5466][12 03 2024]
        [DataMember]
        public DateTime? Startdate
        {
            get
            {
                return startdate; 
            }

            set
            {
                startdate = value;
                OnPropertyChanged("Startdate");
            }
        }
        [DataMember]
        public DateTime? Enddate
        {
            get
            {
                return enddate;
            }

            set
            {
                enddate = value;
                OnPropertyChanged("Enddate");
            }
        }
        [DataMember]
        public int Rework
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
        public Int32 IdCounterparttracking
        {
            get
            {
                return idCounterparttracking;
            }

            set
            {
                idCounterparttracking = value;
                OnPropertyChanged("IdCounterparttracking");
            }
        }
        [DataMember]
        public double TimeDifference
        {
            get
            {
                return timeDifference;
            }

            set
            {
                timeDifference = value;
                OnPropertyChanged("TimeDifference");
            }
        }
        [DataMember]
        public bool Paused
        {
            get
            {
                return paused;
            }

            set
            {
                paused = value;
                OnPropertyChanged("Paused");
            }
        }
        [DataMember]
        public decimal? Production
        {
            get
            {
                return production;
            }

            set
            {
                production = value;
                OnPropertyChanged("Production");
            }
        }
        #endregion

        public int NewIdStage
        {
            get { return newIdStage; }
            set { newIdStage = value;
                OnPropertyChanged("NewIdStage");
            }

        }

        [DataMember]
        public string EmployeeName
        {
            get
            {
                return employeeName;
            }

            set
            {
                employeeName = value;
                OnPropertyChanged("EmployeeName");
            }
        }
        #region[pallavi jadhav][GEOS2-5320][24 10 2024]

        [DataMember]
        public DateTime? PlannedDeliveryDateByStage
        {
            get
            {
                return plannedDeliveryDateByStage;
            }

            set
            {
                plannedDeliveryDateByStage = value;
                OnPropertyChanged("PlannedDeliveryDateByStage");
            }
        }
        [DataMember]
        public Int32 Days
        {
            get
            {
                return days;
            }

            set
            {
                days = value;
                OnPropertyChanged("Days");
            }
        }

        [DataMember]
        private string plannedDeliveryDateHtmlColor;
        public string PlannedDeliveryDateHtmlColor
        {
            get
            {
                return plannedDeliveryDateHtmlColor;
            }

            set
            {
                plannedDeliveryDateHtmlColor = value;
                OnPropertyChanged("PlannedDeliveryDateHtmlColor");
            }
        }
        #endregion


        #region [rani dhamankar][31-03-2025][GEOS2-7097]
        [DataMember]
        public Int32 ProductionActivityTimeType
        {
            get
            {
                return productionActivityTimeType;
            }

            set
            {
                productionActivityTimeType = value;
                OnPropertyChanged("ProductionActivityTimeType");
            }
        }
        [DataMember]
        public string OperatorName
        {
            get
            {
                return operatorName;
            }

            set
            {
                operatorName = value;
                OnPropertyChanged("OperatorName");
            }
        }
        [DataMember]
        public TimeSpan TimeTrackDifference
        {
            get
            {
                return timeTrackDifference;
            }

            set
            {
                timeTrackDifference = value;
                OnPropertyChanged("TimeTrackDifference");
            }
        }
        [DataMember]
        public int TimeTrackIdCounterparttracking
        {
            get
            {
                return timeTrackIdCounterparttracking;
            }

            set
            {
                timeTrackIdCounterparttracking = value;
                OnPropertyChanged("TimeTrackIdCounterparttracking");
            }
        }
        [DataMember]
        public Int64 TimeTrackIdCounterpart
        {
            get
            {
                return timeTrackIdCounterpart;
            }

            set
            {
                timeTrackIdCounterpart = value;
                OnPropertyChanged("TimeTrackIdCounterpart");
            }
        }
        #endregion

        #region [rani dhamankar][11-09-2025][GEOS2-7091]
        [DataMember]
        public Int32 IdOperator
        {
            get
            {
                return idOperator;
            }

            set
            {
                idOperator = value;
                OnPropertyChanged("IdOperator");
            }
        }

        [DataMember]
        public string StageCode
        {
            get
            {
                return stageCode;
            }

            set
            {
                stageCode = value;
                OnPropertyChanged("StageCode");
            }
        }
        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}
