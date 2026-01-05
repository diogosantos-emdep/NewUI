using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class TimeTrackingProductionStageAPI
    {
        private int _IdStage = 0;
        [IgnoreDataMember]
        public int IdStage
        {
            get { return _IdStage; }
            set { _IdStage = value; }
        }

        private string _StageName = string.Empty;
        [IgnoreDataMember]
        public string StageName
        {
            get { return _StageName; }
            set { _StageName = value; }
        }
        private string _StageCode = string.Empty;
        [IgnoreDataMember]
        public string StageCode
        {
            get { return _StageCode; }
            set { _StageCode = value; }
        }
        private int _Sequence = 0;
        [IgnoreDataMember]
        public int Sequence
        {
            get { return _Sequence; }
            set { _Sequence = value; }
        }

        private string _ActiveInPlants = string.Empty;
        [IgnoreDataMember]
        public string ActiveInPlants
        {
            get { return _ActiveInPlants; }
            set { _ActiveInPlants = value; }
        }
    }
}
