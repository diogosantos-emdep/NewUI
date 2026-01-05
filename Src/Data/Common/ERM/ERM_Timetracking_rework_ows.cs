using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class ERM_Timetracking_rework_ows : ModelBase, IDisposable
    {
        #region Fields
        private Int32 oWS_ID;
        private Int32 checkStageId;
        private Int32 detectedStageId;
        #endregion
        #region Properites
        [DataMember]
        public Int32 OWS_ID
        {
            get
            {
                return oWS_ID;
            }

            set
            {
                oWS_ID = value;
                OnPropertyChanged("OWS_ID");
            }
        }

        [DataMember]
        public Int32 CheckStageId
        {
            get
            {
                return checkStageId;
            }

            set
            {
                checkStageId = value;
                OnPropertyChanged("CheckStageId");
            }
        }

        [DataMember]
        public Int32 DetectedStageId
        {
            get
            {
                return detectedStageId;
            }

            set
            {
                detectedStageId = value;
                OnPropertyChanged("DetectedStageId");
            }
        }
        #endregion

        #region Constructor

        public ERM_Timetracking_rework_ows()
        {
        }

        #endregion
        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        #endregion
    }
}
