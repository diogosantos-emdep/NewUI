using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class EmployeeTripStatus : ModelBase, IDisposable
    {
        //[nsatpute][15-10-2024][GEOS2-5933]
        #region Fields
        private int idEmployeeTrip;
        private string requestedBy;
        private DateTime fromDate;
        private DateTime toDate;
        private string departmentName;
        private string shiftName;
        private string weekend;
        private string purpose;
        private string travelerName;
        private string travelerPlant;
        private string workerName;
        private string plant;
        private string workerPlant;
        private bool phoneProvided;
        private string phoneNumber;
        private string travelMethod;
        private DateTime arrivalDate;
        private string arrivalTransportationNumber;
        private DateTime departureDate;
        private string departureTransportationNumber;
        private bool transferOnArrival;
        private string arrivalProviderName;
        private string arrivalProviderContact;
        private bool transferOnDeparture;
        private string departureProviderName;
        private string departureProviderContact;
        private string hostingAddress;
        private bool carProvided;
        private string transportToPlant;
        private string workTime;
        private string emailTo;
        private string emailCC;
        private string workingDayMeal;
        private string weekendMeal;

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
        public string Plant
        {
            get { return plant; }
            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }


        [DataMember]
        public string WorkerPlant
        {
            get { return workerPlant; }
            set
            {
                workerPlant = value;
                OnPropertyChanged("WorkerPlant");
            }
        }
        [DataMember]
        public bool PhoneProvided
        {
            get { return phoneProvided; }
            set
            {
                phoneProvided = value;
                OnPropertyChanged("PhoneProvided");
            }
        }

        [DataMember]
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        }

        [DataMember]
        public string TravelMethod
        {
            get { return travelMethod; }
            set
            {
                travelMethod = value;
                OnPropertyChanged("TravelMethod");
            }
        }

        [DataMember]
        public DateTime ArrivalDate
        {
            get { return arrivalDate; }
            set
            {
                arrivalDate = value;
                OnPropertyChanged("ArrivalDate");
            }
        }

        [DataMember]
        public string ArrivalTransportationNumber
        {
            get { return arrivalTransportationNumber; }
            set
            {
                arrivalTransportationNumber = value;
                OnPropertyChanged("ArrivalTransportationNumber");
            }
        }

        [DataMember]
        public DateTime DepartureDate
        {
            get { return departureDate; }
            set
            {
                departureDate = value;
                OnPropertyChanged("DepartureDate");
            }
        }

        [DataMember]
        public string DepartureTransportationNumber
        {
            get { return departureTransportationNumber; }
            set
            {
                departureTransportationNumber = value;
                OnPropertyChanged("DepartureTransportationNumber");
            }
        }

        [DataMember]
        public bool TransferOnArrival
        {
            get { return transferOnArrival; }
            set
            {
                transferOnArrival = value;
                OnPropertyChanged("TransferOnArrival");
            }
        }

        [DataMember]
        public string ArrivalProviderName
        {
            get { return arrivalProviderName; }
            set
            {
                arrivalProviderName = value;
                OnPropertyChanged("ArrivalProviderName");
            }
        }

        [DataMember]
        public string ArrivalProviderContact
        {
            get { return arrivalProviderContact; }
            set
            {
                arrivalProviderContact = value;
                OnPropertyChanged("ArrivalProviderContact");
            }
        }

        [DataMember]
        public bool TransferOnDeparture
        {
            get { return transferOnDeparture; }
            set
            {
                transferOnDeparture = value;
                OnPropertyChanged("TransferOnDeparture");
            }
        }

        [DataMember]
        public string DepartureProviderName
        {
            get { return departureProviderName; }
            set
            {
                departureProviderName = value;
                OnPropertyChanged("DepartureProviderName");
            }
        }

        [DataMember]
        public string DepartureProviderContact
        {
            get { return departureProviderContact; }
            set
            {
                departureProviderContact = value;
                OnPropertyChanged("DepartureProviderContact");
            }
        }

        [DataMember]
        public string HostingAddress
        {
            get { return hostingAddress; }
            set
            {
                hostingAddress = value;
                OnPropertyChanged("HostingAddress");
            }
        }

        [DataMember]
        public bool CarProvided
        {
            get { return carProvided; }
            set
            {
                carProvided = value;
                OnPropertyChanged("CarProvided");
            }
        }

        [DataMember]
        public string TransportToPlant
        {
            get { return transportToPlant; }
            set
            {
                transportToPlant = value;
                OnPropertyChanged("TransportToPlant");
            }
        }

        [DataMember]
        public string RequestedBy
        {
            get { return requestedBy; }
            set
            {
                requestedBy = value;
                OnPropertyChanged("RequestedBy");
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
        public string DepartmentName
        {
            get { return departmentName; }
            set
            {
                departmentName = value;
                OnPropertyChanged("DepartmentName");
            }
        }

        [DataMember]
        public string ShiftName
        {
            get { return shiftName; }
            set
            {
                shiftName = value;
                OnPropertyChanged("ShiftName");
            }
        }

        [DataMember]
        public string Weekend
        {
            get { return weekend; }
            set
            {
                weekend = value;
                OnPropertyChanged("Weekend");
            }
        }

        [DataMember]
        public string Purpose
        {
            get { return purpose; }
            set
            {
                purpose = value;
                OnPropertyChanged("Purpose");
            }
        }

        [DataMember]
        public string TravelerName
        {
            get { return travelerName; }
            set
            {
                travelerName = value;
                OnPropertyChanged("TravelerName");
            }
        }


        [DataMember]
        public string TravelerPlant
        {
            get { return travelerPlant; }
            set
            {
                travelerPlant = value;
                OnPropertyChanged("TravelerPlant");
            }
        }
        [DataMember]
        public string WorkerName
        {
            get { return workerName; }
            set
            {
                workerName = value;
                OnPropertyChanged("WorkerName");
            }
        }

        [DataMember]
        public string WorkTime
        {
            get { return workTime; }
            set
            {
                workTime = value;
                OnPropertyChanged("WorkTime");
            }
        }

        [DataMember]
        public string EmailTo
        {
            get { return emailTo; }
            set
            {
                emailTo = value;
                OnPropertyChanged("EmailTo");
            }
        }

        [DataMember]
        public string EmailCC

        {
            get { return emailCC; }
            set
            {
                emailCC = value;
                OnPropertyChanged("EmailCC");
            }
        }

        [DataMember]
        public string WorkingDayMeal

        {
            get { return workingDayMeal; }
            set
            {
                workingDayMeal = value;
                OnPropertyChanged("WorkingDayMeal");
            }
        }

        [DataMember]
        public string WeekendMeal

        {
            get { return weekendMeal; }
            set
            {
                weekendMeal = value;
                OnPropertyChanged("WeekendMeal");
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
