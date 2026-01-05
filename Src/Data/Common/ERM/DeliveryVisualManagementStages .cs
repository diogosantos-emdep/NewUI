using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.ERM
{
    public class DeliveryVisualManagementStages : ModelBase, IDisposable
    {
        #region Field
        private int idStage;

        private string stageName;
        private string stageCode;
        private int sequence;
        private string activeInPlants;
        private int newSequence; //[GEOS2-4821][Rupali Sarode][14-09-2023]
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

        //[GEOS2-4821][Rupali Sarode][14-09-2023]
        [DataMember]
        public int NewSequence
        {
            get
            {
                return newSequence;
            }

            set
            {
                newSequence = value;
                OnPropertyChanged("NewSequence");
            }
        }
        #endregion

        #region Constructor
        public DeliveryVisualManagementStages()
        {

        }
        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
