using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
   public class PlanningDateReviewStages: ModelBase, IDisposable
    {
        #region Field
        private int idStage;
        private decimal? real;
        private decimal? expected;
        private float? remianing;
        private string stageName;
        private string stageCode;
        private int sequence;
        private string activeInPlants;
        private Int64 idCounterpart;
        #endregion
        #region Property

        [DataMember]
        public int IdStage
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
        [DataMember]
        public string StageName
        {
            get
            {
                return stageName;
            }

            set
            {
                stageName = value;
                OnPropertyChanged("StageName");
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

        [DataMember]
        public int Sequence
        {
            get
            {
                return sequence;
            }

            set
            {
                sequence = value;
                OnPropertyChanged("Sequence");
            }
        }
        [DataMember]
        public string ActiveInPlants
        {
            get
            {
                return activeInPlants;
            }

            set
            {
                activeInPlants = value;
                OnPropertyChanged("ActiveInPlants");
            }
        }


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
        #endregion

        #region Constructor
        public PlanningDateReviewStages()
        {

        }
        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
