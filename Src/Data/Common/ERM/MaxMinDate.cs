using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common.PCM;
using System.Collections.ObjectModel;
using System;

namespace Emdep.Geos.Data.Common.ERM
{
    public class MaxMinDate : ModelBase, IDisposable
    {

        //[GEOS2-6891][pallavi jadhav][05 02 2025]
        #region Field
        private Int32 idStage;
       
        private DateTime? minStartDate;
        private DateTime? maxEndDate;
        
       
        #endregion
        #region Property
        [DataMember]
        public Int32 IdStage
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
        public DateTime? MinStartDate
        {
            get
            {
                return minStartDate;
            }

            set
            {
                minStartDate = value;
                OnPropertyChanged("MinStartDate");
            }
        }
        

        [DataMember]
        public DateTime? MaxEndDate
        {
            get
            {
                return maxEndDate;
            }

            set
            {
                maxEndDate = value;
                OnPropertyChanged("MaxEndDate");
            }
        }


        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
