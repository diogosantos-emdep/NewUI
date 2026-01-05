using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_IDStage_JobDescription : ModelBase, IDisposable
    {
        #region Field
        private Int32 iDStage;
        private Int32 jobDescription;
  
        #endregion
        #region Property
        [DataMember]
        public Int32 IDStage
        {
            get { return iDStage; }
            set
            {
                iDStage = value;
                OnPropertyChanged("IDStage");
            }
        }
        [DataMember]
        public Int32 JobDescription
        {
            get { return jobDescription; }
            set
            {
                jobDescription = value;
                OnPropertyChanged("JobDescription");
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
    }
}
