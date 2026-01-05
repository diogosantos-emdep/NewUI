using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    public class ModelAppointment : ModelBase, IDisposable
    {
        #region  Fields

        DateTime? startTime;
        DateTime? endTime;
        string subject;
        Int32 status;
        string description;
        Int64 label;
        bool allDay;
        Int32 eventType;
        string recurrenceInfo;
        string reminderInfo;
        object resourceId;
        decimal price;
        Int64 idOffer;
        GeosStatus geosStatus;
        bool isOfferDonePO;
        string connectPlantConstr;
        string connectPlantId;

        DateTime? offerExpectedDate;
        DateTime? otsDeliveryDate;
        DateTime? poReceivedInDate;
        DateTime? rfqReception;
        DateTime? sendIn;
        byte? isGoAhead;
        object tag;

        Int64 appointmentId;

        string tooltipTitle;
        string tooltipSubject;
        string owner;

        bool isCritical;

        #endregion

        #region Constructor
        public ModelAppointment()
        {

        }
        #endregion

        #region Properties

        [NotMapped]
        [DataMember]
        public object Tag
        {
            get { return tag; }
            set
            {
                tag = value;
                OnPropertyChanged("Tag");
            }
        }

        [DataMember]
        public Int64 IdOffer
        {
            get { return idOffer; }
            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }

        [DataMember]
        public DateTime? StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        [DataMember]
        public DateTime? EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
            }
        }


        [DataMember]
        public string Subject
        {
            get { return subject; }
            set
            {
                subject = value;
                OnPropertyChanged("Subject");
            }
        }


        [DataMember]
        public Int32 Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }



        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }


        [DataMember]
        public Int64 Label
        {
            get { return label; }
            set
            {
                label = value;
                OnPropertyChanged("Label");
            }
        }

        [DataMember]
        public bool AllDay
        {
            get { return allDay; }
            set
            {
                allDay = value;
                OnPropertyChanged("AllDay");
            }
        }

        [DataMember]
        public Int32 EventType
        {
            get { return eventType; }
            set
            {
                eventType = value;
                OnPropertyChanged("EventType");
            }
        }


        [DataMember]
        public string RecurrenceInfo
        {
            get { return recurrenceInfo; }
            set
            {
                recurrenceInfo = value;
                OnPropertyChanged("RecurrenceInfo");
            }
        }

        [DataMember]
        public string ReminderInfo
        {
            get { return reminderInfo; }
            set
            {
                reminderInfo = value;
                OnPropertyChanged("ReminderInfo");
            }
        }


        [DataMember]
        public object ResourceId
        {
            get { return resourceId; }
            set
            {
                resourceId = value;
                OnPropertyChanged("ResourceId");
            }
        }

        [DataMember]
        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }

        [NotMapped]
        [DataMember]
        public GeosStatus GeosStatus
        {
            get { return geosStatus; }
            set
            {
                geosStatus = value;
                OnPropertyChanged("GeosStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public string ConnectPlantId
        {
            get { return connectPlantId; }
            set
            {
                connectPlantId = value;
                OnPropertyChanged("ConnectPlantId");
            }
        }

        [NotMapped]
        [DataMember]
        public string ConnectPlantConstr
        {
            get { return connectPlantConstr; }
            set
            {
                connectPlantConstr = value;
                OnPropertyChanged("ConnectPlantConstr");
            }
        }

        [DataMember]
        public bool IsOfferDonePO
        {
            get { return isOfferDonePO; }
            set
            {
                isOfferDonePO = value;
                OnPropertyChanged("IsOfferDonePO");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? OfferExpectedDate
        {
            get { return offerExpectedDate; }
            set
            {
                offerExpectedDate = value;
                OnPropertyChanged("OfferExpectedDate");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? OtsDeliveryDate
        {
            get { return otsDeliveryDate; }
            set
            {
                otsDeliveryDate = value;
                OnPropertyChanged("OtsDeliveryDate");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? PoReceivedInDate
        {
            get { return poReceivedInDate; }
            set
            {
                poReceivedInDate = value;
                OnPropertyChanged("PoReceivedInDate");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? RfqReception
        {
            get { return rfqReception; }
            set
            {
                rfqReception = value;
                OnPropertyChanged("RfqReception");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? SendIn
        {
            get { return sendIn; }
            set
            {
                sendIn = value;
                OnPropertyChanged("SendIn");
            }
        }

        [DataMember]
        public byte? IsGoAhead
        {
            get { return isGoAhead; }
            set
            {
                isGoAhead = value;
                OnPropertyChanged("IsGoAhead");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 AppointmentId
        {
            get { return appointmentId; }
            set
            {
                appointmentId = value;
                OnPropertyChanged("AppointmentId");
            }
        }

        [NotMapped]
        [DataMember]
        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
                OnPropertyChanged("Owner");
            }
        }

        [NotMapped]
        [DataMember]
        public string TooltipSubject
        {
            get { return tooltipSubject; }
            set
            {
                tooltipSubject = value;
                OnPropertyChanged("TooltipSubject");
            }
        }

        [NotMapped]
        [DataMember]
        public string TooltipTitle
        {
            get { return tooltipTitle; }
            set
            {
                tooltipTitle = value;
                OnPropertyChanged("TooltipTitle");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsCritical
        {
            get { return isCritical; }
            set
            {
                isCritical = value;
                OnPropertyChanged("IsCritical");
            }
        }

        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }



    public class TempAppointment : ModelBase, IDisposable
    {
        #region  Fields

        DateTime? startTime;
        DateTime? endTime;
        string subject;
        Int32 status;
        string description;
        Int64 label;
        bool allDay;
        Int32 eventType;
        string recurrenceInfo;
        string reminderInfo;
        object resourceId;
        decimal price;
        Int64 idOffer;
        GeosStatus geosStatus;
        bool isOfferDonePO;
        string connectPlantConstr;
        string connectPlantId;

        DateTime? offerExpectedDate;
        DateTime? otsDeliveryDate;
        DateTime? poReceivedInDate;
        DateTime? rfqReception;
        DateTime? sendIn;
        byte? isGoAhead;
        object tag;

        Int64 appointmentId;

        string tooltipTitle;
        string tooltipSubject;
        string owner;

        bool isCritical;

        #endregion

        #region Constructor
        public TempAppointment()
        {

        }
        #endregion

        #region Properties

        [NotMapped]
        [DataMember]
        public object Tag
        {
            get { return tag; }
            set
            {
                tag = value;
                //OnPropertyChanged("Tag");
            }
        }

        [DataMember]
        public Int64 IdOffer
        {
            get { return idOffer; }
            set
            {
                idOffer = value;
                //OnPropertyChanged("IdOffer");
            }
        }

        [DataMember]
        public DateTime? StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                //OnPropertyChanged("StartTime");
            }
        }

        [DataMember]
        public DateTime? EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                //OnPropertyChanged("EndTime");
            }
        }


        [DataMember]
        public string Subject
        {
            get { return subject; }
            set
            {
                subject = value;
                //OnPropertyChanged("Subject");
            }
        }


        [DataMember]
        public Int32 Status
        {
            get { return status; }
            set
            {
                status = value;
                //OnPropertyChanged("Status");
            }
        }



        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                //OnPropertyChanged("Description");
            }
        }


        [DataMember]
        public Int64 Label
        {
            get { return label; }
            set
            {
                label = value;
                //OnPropertyChanged("Label");
            }
        }

        [DataMember]
        public bool AllDay
        {
            get { return allDay; }
            set
            {
                allDay = value;
                //OnPropertyChanged("AllDay");
            }
        }

        [DataMember]
        public Int32 EventType
        {
            get { return eventType; }
            set
            {
                eventType = value;
                //OnPropertyChanged("EventType");
            }
        }


        [DataMember]
        public string RecurrenceInfo
        {
            get { return recurrenceInfo; }
            set
            {
                recurrenceInfo = value;
                //OnPropertyChanged("RecurrenceInfo");
            }
        }

        [DataMember]
        public string ReminderInfo
        {
            get { return reminderInfo; }
            set
            {
                reminderInfo = value;
                //OnPropertyChanged("ReminderInfo");
            }
        }


        [DataMember]
        public object ResourceId
        {
            get { return resourceId; }
            set
            {
                resourceId = value;
                //OnPropertyChanged("ResourceId");
            }
        }

        [DataMember]
        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                //OnPropertyChanged("Price");
            }
        }

        [NotMapped]
        [DataMember]
        public GeosStatus GeosStatus
        {
            get { return geosStatus; }
            set
            {
                geosStatus = value;
                //OnPropertyChanged("GeosStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public string ConnectPlantId
        {
            get { return connectPlantId; }
            set
            {
                connectPlantId = value;
                //OnPropertyChanged("ConnectPlantId");
            }
        }

        [NotMapped]
        [DataMember]
        public string ConnectPlantConstr
        {
            get { return connectPlantConstr; }
            set
            {
                connectPlantConstr = value;
                //OnPropertyChanged("ConnectPlantConstr");
            }
        }

        [DataMember]
        public bool IsOfferDonePO
        {
            get { return isOfferDonePO; }
            set
            {
                isOfferDonePO = value;
                //OnPropertyChanged("IsOfferDonePO");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? OfferExpectedDate
        {
            get { return offerExpectedDate; }
            set
            {
                offerExpectedDate = value;
                //OnPropertyChanged("OfferExpectedDate");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? OtsDeliveryDate
        {
            get { return otsDeliveryDate; }
            set
            {
                otsDeliveryDate = value;
                //OnPropertyChanged("OtsDeliveryDate");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? PoReceivedInDate
        {
            get { return poReceivedInDate; }
            set
            {
                poReceivedInDate = value;
                //OnPropertyChanged("PoReceivedInDate");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? RfqReception
        {
            get { return rfqReception; }
            set
            {
                rfqReception = value;
                //OnPropertyChanged("RfqReception");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? SendIn
        {
            get { return sendIn; }
            set
            {
                sendIn = value;
                //OnPropertyChanged("SendIn");
            }
        }

        [DataMember]
        public byte? IsGoAhead
        {
            get { return isGoAhead; }
            set
            {
                isGoAhead = value;
                //OnPropertyChanged("IsGoAhead");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 AppointmentId
        {
            get { return appointmentId; }
            set
            {
                appointmentId = value;
                //OnPropertyChanged("AppointmentId");
            }
        }

        [NotMapped]
        [DataMember]
        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
                //OnPropertyChanged("Owner");
            }
        }

        [NotMapped]
        [DataMember]
        public string TooltipSubject
        {
            get { return tooltipSubject; }
            set
            {
                tooltipSubject = value;
                //OnPropertyChanged("TooltipSubject");
            }
        }

        [NotMapped]
        [DataMember]
        public string TooltipTitle
        {
            get { return tooltipTitle; }
            set
            {
                tooltipTitle = value;
                //OnPropertyChanged("TooltipTitle");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsCritical
        {
            get { return isCritical; }
            set
            {
                isCritical = value;
                //OnPropertyChanged("IsCritical");
            }
        }

        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }

}
