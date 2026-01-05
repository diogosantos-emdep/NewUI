using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class StageByOTItemAndIDDrawing : ModelBase, IDisposable
    {
        #region Field
        List<Stages> oTITemStagesList;
        List<Stages> drawingIdStagesList;
        #endregion
        #region Property


        [DataMember]
        public List<Stages> OTITemStagesList
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
        public List<Stages> DrawingIdStagesList
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
        public StageByOTItemAndIDDrawing()
        {

        }
        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
