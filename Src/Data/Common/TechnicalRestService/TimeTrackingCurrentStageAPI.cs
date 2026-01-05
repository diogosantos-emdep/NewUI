using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class TimeTrackingCurrentStageAPI
    {

        private int _IdStage = 0;
        [IgnoreDataMember]
        public int IdStage
        {
            get { return _IdStage; }
            set { _IdStage = value; }
        }

        private int _NewIdStage = 0;
        [IgnoreDataMember]
        public int NewIdStage
        {
            get { return _NewIdStage; }
            set { _NewIdStage = value; }
        }
        private string _Code = string.Empty;
        [IgnoreDataMember]
        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        private string _Name = string.Empty;
        [IgnoreDataMember]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }

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
        private decimal? _production;
        [DataMember(Order = 7)]
        public decimal? Production
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
        private int _rework;
        [IgnoreDataMember]
        public int Rework
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

        #endregion


    }
}
