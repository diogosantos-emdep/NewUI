using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common.PCM;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_ProductionTimelineMonthData : ModelBase, IDisposable
    {
        #region Field
        //Stage
        private Int32 month;
        private string monthName;
        private Int32 cAD_CAMLogged;
        private Int32 productionLogged;
        private Int32 cAD_CAMAttandance;
        private double productionAttandance;

        #endregion
        #region Property


        [DataMember]
        public Int32 Month
        {
            get { return month; }
            set
            {
                month = value;
                OnPropertyChanged("Month");
            }
        }

        [DataMember]
        public string MonthName
        {
            get { return monthName; }
            set
            {
                monthName = value;
                OnPropertyChanged("MonthName");
            }
        }

        [DataMember]
        public Int32 CAD_CAMLogged
        {
            get { return cAD_CAMLogged; }
            set
            {
                cAD_CAMLogged = value;
                OnPropertyChanged("CAD_CAMLogged");
            }
        }
        [DataMember]
        public Int32 ProductionLogged
        {
            get { return productionLogged; }
            set
            {
                productionLogged = value;
                OnPropertyChanged("ProductionLogged");
            }
        }

        [DataMember]
        public Int32 CAD_CAMAttandance
        {
            get { return cAD_CAMAttandance; }
            set
            {
                cAD_CAMAttandance = value;
                OnPropertyChanged("CAD_CAMAttandance");
            }
        }
        [DataMember]
        public double ProductionAttandance
        {
            get { return productionAttandance; }
            set
            {
                productionAttandance = value;
                OnPropertyChanged("ProductionAttandance");
            }
        }
        #endregion
        #region Constructor
        public ERM_ProductionTimelineMonthData()
        {

        }
        #endregion
        #region Method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
