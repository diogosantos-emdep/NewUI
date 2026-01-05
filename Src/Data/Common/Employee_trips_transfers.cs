using Emdep.Geos.Data.Common.Hrm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class Employee_trips_transfers : ModelBase, IDisposable
    {
        #region Fields
        int idEmployeeTripsTransfers;
        int idEmployeeTrip;
        int idTransferOriginType;
        string code;
        UInt32 idEmployee;
        string origin;
        int idTransferDestinationType;
        string destination;
        int transportMethodId;
        string transport_Method;
        int providerId;
        string provider;
        string contactPersonNumber;
        int estimatedDuration;
        string remarks;
        string contactPerson;
        private TimeSpan arrivalDateHours;
        DateTime fromDate;
        Int32? createdBy;
        Int32? updatedBy;
        #endregion
        #region Properties
        [DataMember]
        public int IdEmployeeTrip
        {
            get { return idEmployeeTrip; }
            set
            {
                idEmployeeTrip = value;
                OnPropertyChanged("IdEmployeeTrip");
            }
        }
        [DataMember]
        public int IdEmployeeTripsTransfers
        {
            get { return idEmployeeTripsTransfers; }
            set
            {
                idEmployeeTripsTransfers = value;
                OnPropertyChanged("IdEmployeeTripsTransfers");
            }
        }
        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }
        [DataMember]
        public UInt32 IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }
        [DataMember]
        public int IdTransferOriginType
        {
            get { return idTransferOriginType; }
            set
            {
                idTransferOriginType = value;
                OnPropertyChanged("IdTransferOriginType");
            }
        }
        [DataMember]
        public string Origin
        {
            get { return origin; }
            set
            {
                origin = value;
                OnPropertyChanged("Origin");
            }
        }
        [DataMember]
        public int IdTransferDestinationType
        {
            get { return idTransferDestinationType; }
            set
            {
                idTransferDestinationType = value;
                OnPropertyChanged("IdTransferDestinationType");
            }
        }
       
        [DataMember]
        public string Destination
        {
            get { return destination; }
            set
            {
                destination = value;
                OnPropertyChanged("Destination");
            }
        }
        [DataMember]
        public int TransportMethodId
        {
            get { return transportMethodId; }
            set
            {
                transportMethodId = value;
                OnPropertyChanged("TransportMethodId");
            }
        }
        [DataMember]
        public string Transport_Method
        {
            get { return transport_Method; }
            set
            {
                transport_Method = value;
                OnPropertyChanged("Transport_Method");
            }
        }
        [DataMember]
        public int ProviderId
        {
            get { return providerId; }
            set
            {
                providerId = value;
                OnPropertyChanged("ProviderId");
            }
        }
        [DataMember]
        public string Provider
        {
            get { return provider; }
            set
            {
                provider = value;
                OnPropertyChanged("Provider");
            }
        }
        [DataMember]
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
            }
        }
        [DataMember]
        public string ContactPersonNumber
        {
            get { return contactPersonNumber; }
            set
            {
                contactPersonNumber = value;
                OnPropertyChanged("ContactPersonNumber");
            }
        }
        [DataMember]
        public int EstimatedDuration
        {
            get { return estimatedDuration; }
            set
            {
                estimatedDuration = value;
                OnPropertyChanged("EstimatedDuration");
            }
        }
        [DataMember]
        public string ContactPerson
        {
            get { return contactPerson; }
            set
            {
                contactPerson = value;
                OnPropertyChanged("ContactPerson");
            }
        }
        [DataMember]
        public TimeSpan ArrivalDateHours
        {
            get { return arrivalDateHours; }
            set
            {
                arrivalDateHours = value;
                OnPropertyChanged("ArrivalDateHours");

            }
        }
        [DataMember]
        public DateTime FromDate
        {
            get { return fromDate; }
            set { fromDate = value; OnPropertyChanged("FromDate"); }
        }

        [DataMember]
        public Int32? CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }

        }
        [DataMember]
        public Int32? UpdatedBy
        {
            get { return updatedBy; }
            set
            {
                updatedBy = value;
                OnPropertyChanged("UpdatedBy");
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
