using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class ERMTimetrackingSite : ModelBase, IDisposable
    {
        #region Fields
        Int32 productionIdSite;
        string productionSite;
        Int32 originalIdSite;
        string originalSite;
        #endregion
        #region Constructor
        public ERMTimetrackingSite()
        {

        }
        #endregion

        #region Properties

        [DataMember]
        public Int32 ProductionIdSite
        {
            get
            {
                return productionIdSite;
            }
            set
            {
                productionIdSite = value;
                OnPropertyChanged("ProductionIdSite");
            }
        }

        [DataMember]
        public string ProductionSite
        {
            get
            {
                return productionSite;
            }
            set
            {
                productionSite = value;
                OnPropertyChanged("ProductionSite");
            }
        }

        [DataMember]
        public Int32 OriginalIdSite
        {
            get
            {
                return originalIdSite;
            }
            set
            {
                originalIdSite = value;
                OnPropertyChanged("OriginalIdSite");
            }
        }

        [DataMember]
        public string OriginalSite
        {
            get
            {
                return originalSite;
            }
            set
            {
                originalSite = value;
                OnPropertyChanged("OriginalSite");
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
