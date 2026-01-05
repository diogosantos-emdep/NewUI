using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.ERM
{
    public class PlantWeeklyReworksMailStage : ModelBase, IDisposable
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

        #endregion

        #region Constructor
        public PlantWeeklyReworksMailStage()
        {

        }
        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class ReworksMailStageByOTItemAndIDDrawing : ModelBase, IDisposable
    {
        #region Field
        List<PlantWeeklyReworksMailStage> allStagesList;
        List<PlantWeeklyReworksMailStage> oTITemStagesList;
        List<PlantWeeklyReworksMailStage> drawingIdStagesList;
        #endregion
        #region Property


        [DataMember]
        public List<PlantWeeklyReworksMailStage> AllStagesList
        {
            get
            {
                return allStagesList;
            }

            set
            {
                allStagesList = value;
                OnPropertyChanged("AllStagesList");
            }
        }

        [DataMember]
        public List<PlantWeeklyReworksMailStage> OTITemStagesList
        {
            get
            {
                return oTITemStagesList;
            }

            set
            {
                oTITemStagesList = value;
                OnPropertyChanged("OTITemStagesList");
            }
        }

        [DataMember]
        public List<PlantWeeklyReworksMailStage> DrawingIdStagesList
        {
            get
            {
                return drawingIdStagesList;
            }

            set
            {
                drawingIdStagesList = value;
                OnPropertyChanged("DrawingIdStagesList");
            }
        }

        #endregion

        #region Constructor
        public ReworksMailStageByOTItemAndIDDrawing()
        {

        }
        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
