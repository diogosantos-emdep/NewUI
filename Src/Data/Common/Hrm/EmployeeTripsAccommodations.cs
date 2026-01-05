using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Org.BouncyCastle.Tls;

namespace Emdep.Geos.Data.Common.Hrm
{  //[shweta.thube][GEOS2-7989][26.09.2025]
    [DataContract]
    public class EmployeeTripsAccommodations : ModelBase, IDisposable
    {
        #region Fields
        string type;
        string name;
        DateTime checkInDate;
        DateTime checkOutDate;
        string address;
        string coordinates;
        string remarks;
        int idEmployeeTrip;
        UInt32 idType;
        UInt32 idAccommodations;        
        Int32 createdBy;
        Int32 updatedBy;
        UInt32 idEmployee;
        string employeeName;
        string accommodationsType;
        string apartmentName;
        Int32 apartmentId;
        int numberOfNights;
        DateTime? updatedAt;
        DateTime? createdAt;
        TimeSpan checkOutTime;
        TimeSpan checkInTime;
        string accommodationName;
        #endregion
        #region Properties
        [DataMember]
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        [DataMember]
        public DateTime CheckInDate
        {
            get { return checkInDate; }
            set
            {
                checkInDate = value;
                OnPropertyChanged("CheckInDate");
            }
        }
        [DataMember]
        public DateTime CheckOutDate
        {
            get { return checkOutDate; }
            set
            {
                checkOutDate = value;
                OnPropertyChanged("CheckOutDate");
            }
        }
        [DataMember]
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }

        [DataMember]
        public string Coordinates
        {
            get { return coordinates; }
            set
            {
                coordinates = value;
                OnPropertyChanged("Coordinates");
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
        public UInt32 IdType
        {
            get { return idType; }
            set
            {
                idType = value;
                OnPropertyChanged("IdType");
            }
        }
        [DataMember]
        public UInt32 IdAccommodations
        {
            get { return idAccommodations; }
            set
            {
                idAccommodations= value;
                OnPropertyChanged("IdAccommodations");
            }
        }      
        [DataMember]
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }

        }
        [DataMember]
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set
            {
                updatedBy = value;
                OnPropertyChanged("UpdatedBy");
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
        public string EmployeeName
        {
            get { return employeeName; }
            set
            {
                employeeName = value;
                OnPropertyChanged("EmployeeName");
            }
        }
        [DataMember]
        public string AccommodationsType
        {
            get { return accommodationsType; }
            set
            {
                accommodationsType = value;
                OnPropertyChanged("AccommodationsType");
            }
        }
        
        [DataMember]
        public string ApartmentName
        {
            get { return apartmentName; }
            set
            {
                apartmentName = value;
                OnPropertyChanged("ApartmentName");
            }
        }
        [DataMember]
        public Int32 ApartmentId
        {
            get { return apartmentId; }
            set
            {
                apartmentId = value;
                OnPropertyChanged("ApartmentId");
            }
        }

        [DataMember]
        public int NumberOfNights
        {
            get { return numberOfNights; }
            set
            {
                numberOfNights = value;
                OnPropertyChanged("NumberOfNights");
            }
        }
        [DataMember]
        public DateTime? CreatedAt
        {
            get { return createdAt; }
            set
            {
                createdAt = value;
                OnPropertyChanged("CreatedAt");
            }
        }
        [DataMember]
        public DateTime? UpdatedAt
        {
            get { return updatedAt; }
            set
            {
                updatedAt = value;
                OnPropertyChanged("UpdatedAt");
            }
        }
        [DataMember]
        public TimeSpan CheckInTime
        {
            get { return checkInTime; }
            set
            {
                checkInTime = value;
                OnPropertyChanged("CheckInTime");
            }
        }
        [DataMember]
        public TimeSpan CheckOutTime
        {
            get { return checkOutTime; }
            set
            {
                checkOutTime = value;
                OnPropertyChanged("CheckOutTime");
            }
        }
        [DataMember]
        public string AccommodationName
        {
            get { return accommodationName; }
            set
            {
                accommodationName = value;
                OnPropertyChanged("AccommodationName");
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
