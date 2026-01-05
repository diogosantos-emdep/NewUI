using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
   public class ERMWorkStageWiseJobDescription : ModelBase, IDisposable
    {
        #region Field
        private Int32 idWorkStage;
        //private string idJobDescription;
        private List<string> idJobDescription;
        #endregion
        #region Property
        [DataMember]
        public Int32 IdWorkStage
        {
            get { return idWorkStage; }
            set
            {
                idWorkStage = value;
                OnPropertyChanged("IdWorkStage");
            }
        }


        [DataMember]
        public List<string> IdJobDescription
        {
            get { return idJobDescription; }
            set
            {
                idJobDescription = value;
                OnPropertyChanged("IdJobDescription");
            }
        }
       

        #endregion
        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {

            return this.MemberwiseClone();
        }
        #endregion
        #region Constructor
        public ERMWorkStageWiseJobDescription()
        {

        }
        #endregion
    }
}
