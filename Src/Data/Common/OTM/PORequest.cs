using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common.OTM
{
    public class PORequest : ModelBase, IDisposable
    {
        #region Fields
        long idEmail;
        Int64 idPORequest;
        private DateTime? createdIn;
        private DateTime? modifiedIn;        
        private LookupValue poRequestStatus;
        #endregion     

        #region Constructor
        public PORequest()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public Int64 IdPORequest
        {
            get
            {
                return idPORequest;
            }

            set
            {
                idPORequest = value;
                OnPropertyChanged("IdPORequest");
            }
        }

        [DataMember]
        public LookupValue PORequestStatus
        {
            get
            {
                return poRequestStatus;
            }

            set
            {
                poRequestStatus = value;
                OnPropertyChanged("PORequestStatus");
            }
        }

        [DataMember]
        public Int64 IdEmail
        {
            get
            {
                return idEmail;
            }

            set
            {
                idEmail = value;
                OnPropertyChanged("IdEmail");
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
