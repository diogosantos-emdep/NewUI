using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.ERM
{
    public class WeeklyReworksReportSummary : ModelBase, IDisposable
    {

        #region Declarations
        private UInt32 numberOfReworks;
        private UInt64 totalItems;
        private UInt32 idCompany;

        #endregion

        #region Properties

        [DataMember]
        public UInt32 NumberOfReworks
        {
            get { return numberOfReworks; }
            set
            {
                numberOfReworks = value;
                OnPropertyChanged("NumberOfReworks");
            }
        }

        [DataMember]
        public UInt64 TotalItems
        {
            get { return totalItems; }
            set
            {
                totalItems = value;
                OnPropertyChanged("TotalItems");
            }
        }

        [DataMember]
        public UInt32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
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
