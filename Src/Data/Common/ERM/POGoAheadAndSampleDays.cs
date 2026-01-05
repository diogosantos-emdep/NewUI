using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
   public class POGoAheadAndSampleDays: ModelBase
    {
        #region declaration
        private string plant;
        private string year;
        private Int32 poGoheadDays;
        private Int32 sampleDays;
        #endregion

        #region Properties

        public string Plant
        {
            get { return plant; }
            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }

        public string Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged("Year");
            }
        }

        public Int32 PoGoheadDays
        {
            get { return poGoheadDays; }
            set
            {
                poGoheadDays = value;
                OnPropertyChanged("PoGoheadDays");
            }
        }

        public Int32 SampleDays
        {
            get { return sampleDays; }
            set
            {
                sampleDays = value;
                OnPropertyChanged("SampleDays");
            }
        }


        #endregion

    }
}
