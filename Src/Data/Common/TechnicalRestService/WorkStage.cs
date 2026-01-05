using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    //[Sudhir.Jangra][APIGEOS-674][24/11/2022][Created New Class https://helpdesk.emdep.com/browse/APIGEOS-674]
    [DataContract]
    public class WorkStage
    {
        private int _Id = 0;
        [DataMember(Order = 1)]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private string _Code = string.Empty;
        [DataMember(Order = 2)]
        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        private string _Name = string.Empty;
        [DataMember(Order = 3)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }

        }

        #region [pallavi jadhav][APIGEOS-1074][22 01 2025]

        private string _PlannedDeliveryDate;
        [DataMember(Order = 4)]
        public string PlannedDeliveryDate
        {
            get { return _PlannedDeliveryDate; }
            set { _PlannedDeliveryDate = value; }
        }
        private Int32 _Days;
        [DataMember(Order = 5)]
        public Int32 Days
        {
            get { return _Days; }
            set { _Days = value; }
        }
        #endregion

        private TimeSpan _Expecteds = TimeSpan.Parse("0");
        [IgnoreDataMember]
        public TimeSpan Expecteds
        {
            get { return _Expecteds; }
            set { _Expecteds = value; }

        }

        private string _ExpectedTime = "0";
        [DataMember(Order = 6)]
        public string ExpectedTime
        {
            get { return _ExpectedTime; }
            set { _ExpectedTime = value; }

        }

        private TimeSpan _Reals = TimeSpan.Parse("0");
        [IgnoreDataMember]
        public TimeSpan Reals
        {
            get { return _Reals; }
            set { _Reals = value; }

        }

        private string _RealTime = "0";
        [DataMember(Order = 7)]
        public string RealTime
        {
            get { return _RealTime; }
            set { _RealTime = value; }

        }
        private TimeSpan _Remaining = TimeSpan.Parse("0");
        [IgnoreDataMember]
        public TimeSpan Remaining
        {
            get { return _Remaining; }
            set { _Remaining = value; }

        }

        private string _RemainingTime = "0";
        [DataMember(Order = 8)]
        public string RemainingTime
        {
            get { return _RemainingTime; }
            set { _RemainingTime = value; }

        }


        [IgnoreDataMember]
        public int Sequence { get; set; }

        [IgnoreDataMember]
        public string ActiveInPlants { get; set; }

        private Int64 _IdCP = 0;
        public Int64 IdCP
        {
            get { return _IdCP; }
            set { _IdCP = value; }
        }
        private Int64 _IdCounterpart = 0;
        public Int64 IdCounterpart
        {
            get { return _IdCounterpart; }
            set { _IdCounterpart = value; }
        }

        #region [gulab lakade][GEOS2-5466][12 03 2024]
        private DateTime? _startdate;
        [IgnoreDataMember]
        public DateTime? Startdate
        {
            get
            {
                return _startdate;
            }
            set
            {
                _startdate = value;
                
            }
        }
        private DateTime? _enddate;
        [IgnoreDataMember]
        public DateTime? Enddate
        {
            get
            {
                return _enddate;
            }

            set
            {
                _enddate = value;
            }
        }
        private TimeSpan _productionTime;
        [IgnoreDataMember]
        public TimeSpan ProductionTime
        {
            get
            {
                return _productionTime;
            }

            set
            {
                _productionTime = value;
            }
        }
        private string _production;
        [DataMember(Order = 5)]
        public string Production
        {
            get
            {
                return _production;
            }

            set
            {
                _production = value;
            }
        }
        #region  [pallavi.jadhav][24-04-2025][APIGEOS-1389]
        private int _ProductionActivityTimeType = 0;
        private string _OperatorName = string.Empty;
        private TimeSpan _TimeTrackDifference = TimeSpan.Zero;
        private Int32 _TimeTrackIdCounterparttracking = 0;
        private Int64 _TimeTrackIdCounterpart = 0;
        private string _Download = string.Empty;
        private string _Transferred = string.Empty;
        private string _Addin = string.Empty;
        private string _PostServer = string.Empty;
        private string _EDS = string.Empty;
        #endregion


        #region [pallavi.jadhav][24-04-2025][APIGEOS-1389]

        [IgnoreDataMember]
        public string Download
        {
            get
            {
                return _Download;
            }

            set
            {
                _Download = value;

            }
        }
        [IgnoreDataMember]
        public string Transferred
        {
            get
            {
                return _Transferred;
            }

            set
            {
                _Transferred = value;

            }
        }

        [IgnoreDataMember]
        public string Addin
        {
            get
            {
                return _Addin;
            }

            set
            {
                _Addin = value;

            }
        }

        [IgnoreDataMember]
        public string PostServer
        {
            get
            {
                return _PostServer;
            }

            set
            {
                _PostServer = value;

            }
        }
        [IgnoreDataMember]
        public string EDS
        {
            get
            {
                return _EDS;
            }

            set
            {
                _EDS = value;

            }
        }
        #endregion
        private TimeSpan _reworkTime;
        [IgnoreDataMember]
        public TimeSpan ReworkTime
        {
            get
            {
                return _reworkTime;
            }

            set
            {
                _reworkTime = value;
            }
        }

        private string _rework;
        [DataMember(Order = 12)]
        public string Rework
        {
            get
            {
                return _rework;
            }

            set
            {
                _rework = value;
            }
        }
        private TimeSpan _ProductionOWSTime;
        [IgnoreDataMember]
        public TimeSpan Production_OWSTime
        {
            get
            {
                return _ProductionOWSTime;
            }

            set
            {
                _ProductionOWSTime = value;
            }
        }
        private string _ProductionOWS;
        [DataMember(Order = 11)]
        public string Production_OWS
        {
            get
            {
                return _ProductionOWS;
            }

            set
            {
                _ProductionOWS = value;
            }
        }

        private TimeSpan _ReworkOWSTime;
        [IgnoreDataMember]
        public TimeSpan Rework_OWSTime
        {
            get
            {
                return _ReworkOWSTime;
            }

            set
            {
                _ReworkOWSTime = value;
            }
        }


        private string _ReworkOWS;
        [DataMember(Order = 13)]
        public string Rework_OWS
        {
            get
            {
                return _ReworkOWS;
            }

            set
            {
                _ReworkOWS = value;
            }
        }

        private string _FirstValidatedDate;
        [DataMember(Order = 14)]
        public string FirstValidatedDate
        {
            get
            {
                return _FirstValidatedDate;
            }

            set
            {
                _FirstValidatedDate = value;
            }
        }

        private int _reworks;
        [IgnoreDataMember]
        public int Reworks
        {
            get
            {
                return _reworks;
            }

            set
            {
                _reworks = value;
            }
        }
        private Int32 _idCounterparttracking;
        [IgnoreDataMember]
        public Int32 IdCounterparttracking
        {
            get
            {
                return _idCounterparttracking;
            }

            set
            {
                _idCounterparttracking = value;
            }
        }
        private double _timeDifference;
        [IgnoreDataMember]
        public double TimeDifference
        {
            get
            {
                return _timeDifference;
            }

            set
            {
                _timeDifference = value;
            }
        }
        private bool _paused;
        [IgnoreDataMember]
        public bool Paused
        {
            get
            {
                return _paused;
            }

            set
            {
                _paused = value;
            }
        }

        private decimal? _Expected;
        [IgnoreDataMember]
        public decimal? Expected
        {
            get
            {
                return _Expected;
            }

            set
            {
                _Expected = value;
            }
        }
        private decimal? _Real;
        [IgnoreDataMember]
        public decimal? Real
        {
            get
            {
                return _Real;
            }

            set
            {
                _Real= value;
            }
        }

        private int newIdStage;
        [IgnoreDataMember]
        public int NewIdStage
        {
            get { return newIdStage; }
            set
            {
                newIdStage = value;
            }

        }

        private decimal? _Productions;
        [IgnoreDataMember]
        public decimal? Productions
        {
            get
            {
                return _Productions;
            }

            set
            {
                _Productions = value;
            }
        }

        private List<TimetrakingEmployees> _Employees;
        [DataMember(Order = 16)]
        public List<TimetrakingEmployees> Employees
        {
            get { return _Employees; }
            set { _Employees = value; }

        }
        private string _EmployeeCode = string.Empty;
        [IgnoreDataMember]
        public string EmployeeCode
        {
            get
            {
                return _EmployeeCode;
            }

            set
            {
                _EmployeeCode = value;
            }
        }
        private Int32 _IdUser = 0;
        [IgnoreDataMember]
        public Int32 IdUser
        {
            get
            {
                return _IdUser;
            }

            set
            {
                _IdUser = value;
            }
        }
        private Int32 _IdEmployee = 0;
        [IgnoreDataMember]
        public Int32 IdEmployee
        {
            get
            {
                return _IdEmployee;
            }

            set
            {
                _IdEmployee = value;
            }
        }

        #endregion





    }
}
