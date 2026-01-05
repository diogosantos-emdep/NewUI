using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class TimetrakingEmployees
    {
        private string _EmployeeCode = string.Empty;
        [DataMember(Order = 1)]
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
        private string _RealTime = "0";
        [DataMember(Order = 2)]
        public string RealTime
        {
            get
            {
                return _RealTime;
            }

            set
            {
                _RealTime = value;
            }
        }
        private string _Production = "0";
        [DataMember(Order = 3)]
        public string Production
        {
            get
            {
                return _Production;
            }

            set
            {
                _Production = value;
            }
        }
        private string _Production_OWS = "0";
        [DataMember(Order = 4)]
        public string Production_OWS
        {
            get
            {
                return _Production_OWS;
            }

            set
            {
                _Production_OWS = value;
            }
        }
        private string _Rework = "0";
        [DataMember(Order = 5)]
        public string Rework
        {
            get
            {
                return _Rework;
            }

            set
            {
                _Rework = value;
            }
        }
        private string _ReworkOWS;
        [IgnoreDataMember]
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

        private Int32 _IdUser =0;
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

        private TimeSpan _Reals = TimeSpan.Parse("0");
        [IgnoreDataMember]
        public TimeSpan Reals
        {
            get { return _Reals; }
            set { _Reals = value; }

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
        private TimeSpan _Rework_OWSTime;
        [IgnoreDataMember]
        public TimeSpan Rework_OWSTime
        {
            get
            {
                return _Rework_OWSTime;
            }

            set
            {
                _Rework_OWSTime = value;
            }
        }
        private Int32 _IdStage = 0;
        [IgnoreDataMember]
        public Int32 IdStage
        {
            get
            {
                return _IdStage;
            }

            set
            {
                _IdStage = value;
            }
        }
        private Int32 _IDCounterpart = 0;
        [IgnoreDataMember]
        public Int32 IDCounterpart
        {
            get
            {
                return _IDCounterpart;
            }

            set
            {
                _IDCounterpart = value;
            }
        }
        private Int32 _IDCounterpartTracking = 0;
        [IgnoreDataMember]
        public Int32 IDCounterpartTracking
        {
            get
            {
                return _IDCounterpartTracking;
            }

            set
            {
                _IDCounterpartTracking = value;
            }
        }
    }
}
