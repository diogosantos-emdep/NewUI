using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.ERM
{
    public class PlantOperationProductionStage : ModelBase, IDisposable
    {
        #region Field
        private int idStage;

        private string stageName;
        private string stageCode;
        private int sequence;
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


        #endregion

        #region Constructor
        public PlantOperationProductionStage()
        {

        }
        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
