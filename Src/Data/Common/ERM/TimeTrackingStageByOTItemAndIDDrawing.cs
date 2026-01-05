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
    public class TimeTrackingStageByOTItemAndIDDrawing : ModelBase, IDisposable
    {
        #region Field
        List<TimeTrackingProductionStage> allStagesList;
        List<TimeTrackingProductionStage> oTITemStagesList;
        List<TimeTrackingProductionStage> drawingIdStagesList;
        #endregion
        #region Property


        [DataMember]
        public List<TimeTrackingProductionStage> AllStagesList
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
        public List<TimeTrackingProductionStage> OTITemStagesList
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
        public List<TimeTrackingProductionStage> DrawingIdStagesList
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
        public TimeTrackingStageByOTItemAndIDDrawing()
        {

        }
        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
