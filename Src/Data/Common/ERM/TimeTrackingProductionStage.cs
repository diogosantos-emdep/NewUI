using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;

namespace Emdep.Geos.Data.Common.ERM
{
    public class TimeTrackingProductionStage : ModelBase, IDisposable
    {
        #region Field
        private int idStage;

        private string stageName;
        private string stageCode;
        private int sequence;
        private string activeInPlants;  //[Rupali Sarode][GEOS2-4347][05-05-2023]
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

        //[Rupali Sarode][GEOS2-4347][05-05-2023]
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

        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    
}
