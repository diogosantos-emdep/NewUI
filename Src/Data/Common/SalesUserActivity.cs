using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class SalesUserActivity : ModelBase, IDisposable
    {
        #region Fields

        Int64 idActivity;
        Int32 idOwner;
        DateTime fromDate;
        DateTime toDate;
        DateTime closeDate;
        Int32 idActivityType;
        byte isCompleted;

        #endregion

        #region Properties

        [DataMember]
        public Int64 IdActivity
        {
            get { return idActivity; }
            set
            {
                idActivity = value;
                OnPropertyChanged("IdActivity");
            }
        }

        [DataMember]
        public Int32 IdOwner
        {
            get { return idOwner; }
            set
            {
                idOwner = value;
                OnPropertyChanged("IdOwner");
            }
        }

        [DataMember]
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        [DataMember]
        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }

        [DataMember]
        public DateTime CloseDate
        {
            get { return closeDate; }
            set
            {
                closeDate = value;
                OnPropertyChanged("CloseDate");
            }
        }

        [DataMember]
        public Int32 IdActivityType
        {
            get { return idActivityType; }
            set
            {
                idActivityType = value;
                OnPropertyChanged("IdActivityType");
            }
        }
       
        [DataMember]
        public byte IsCompleted
        {
            get { return isCompleted; }
            set
            {
                isCompleted = value;
                OnPropertyChanged("IsCompleted");
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
