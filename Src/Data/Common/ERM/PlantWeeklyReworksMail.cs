using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class PlantWeeklyReworksMail : ModelBase, IDisposable
    {

        #region Field
        private Int32 idOT;
        private Int32 idCounterpart;
        private Int32 idSite;
        private Int32 rework;
        private string stageName;
        private string stageCode;
        private string activeInPlants;
        private DateTime? deliveryDate;
        private DateTime? plannedDeliveryDate;
        private string oTCode;
        private string deliveryWeek;
        private DateTime? pODate;
        private string pOType;
        private string customer;
        private string project;
        private string offer;
        private string originPlant;
        private string customerReference;
        private string template;
        private string type;
        private Int32 qTY;
        private string serialNumber;
        private Int32 itemNumber;
        private string itemStatus;
        private string currentWorkStation;
        List<Counterpartstracking> counterpartstrackingList;
        #endregion
        #region Property

      [DataMember]
        public Int32 IdOT
        {
            get
            {
                return idOT;
            }

            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }
        [DataMember]
        public Int32 IdCounterpart
        {
            get
            {
                return idCounterpart;
            }

            set
            {
                idCounterpart = value;
                OnPropertyChanged("IdCounterpart");
            }
        }
        [DataMember]
        public Int32 IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }
        [DataMember]
        public Int32 Rework
        {
            get
            {
                return rework;
            }

            set
            {
                rework = value;
                OnPropertyChanged("Rework");
            }
        }
        [DataMember]
        public string StageName
        {
            get
            {
                return stageName;
            }

            set
            {
                stageName = value;
                OnPropertyChanged("StageName");
            }
        }
        [DataMember]
        public string StageCode
        {
            get
            {
                return stageCode;
            }

            set
            {
                stageCode = value;
                OnPropertyChanged("StageCode");
            }
        }
        [DataMember]
        public string OTCode
        {
            get
            {
                return oTCode;
            }

            set
            {
                oTCode = value;
                OnPropertyChanged("OTCode");
            }
        }


        [DataMember]
        public string ActiveInPlants
        {
            get
            {
                return activeInPlants;
            }

            set
            {
                activeInPlants = value;
                OnPropertyChanged("ActiveInPlants");
            }
        }
        [DataMember]
        public DateTime? DeliveryDate
        {
            get
            {
                return deliveryDate;
            }

            set
            {
                deliveryDate = value;
                OnPropertyChanged("DeliveryDate");
            }
        }
        [DataMember]
        public DateTime? PlannedDeliveryDate
        {
            get
            {
                return plannedDeliveryDate;
            }

            set
            {
                plannedDeliveryDate = value;
                OnPropertyChanged("PlannedDeliveryDate");
            }
        }

        [DataMember]
        public string DeliveryWeek
        {
            get
            {
                return deliveryWeek;
            }

            set
            {
                deliveryWeek = value;
                OnPropertyChanged("DeliveryWeek");
            }
        }
        [DataMember]
        public DateTime? PODate
        {
            get
            {
                return pODate;
            }

            set
            {
                pODate = value;
                OnPropertyChanged("PODate");
            }
        }

        [DataMember]
        public string POType
        {
            get
            {
                return pOType;
            }

            set
            {
                pOType = value;
                OnPropertyChanged("POType");
            }
        }

        [DataMember]
        public string Customer
        {
            get
            {
                return customer;
            }

            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }
        [DataMember]
        public string Project
        {
            get
            {
                return project;
            }

            set
            {
                project = value;
                OnPropertyChanged("Project");
            }
        }

        [DataMember]
        public string Offer
        {
            get
            {
                return offer;
            }

            set
            {
                offer = value;
                OnPropertyChanged("Offer");
            }
        }
        [DataMember]
        public string OriginPlant
        {
            get
            {
                return originPlant;
            }

            set
            {
                originPlant = value;
                OnPropertyChanged("OriginPlant");
            }
        }

        [DataMember]
        public string CustomerReference
        {
            get
            {
                return customerReference;
            }

            set
            {
                customerReference = value;
                OnPropertyChanged("CustomerReference");
            }
        }

        [DataMember]
        public string Template
        {
            get
            {
                return template;
            }

            set
            {
                template = value;
                OnPropertyChanged("Template");
            }
        }

        [DataMember]
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }
        [DataMember]
        public Int32 QTY
        {
            get
            {
                return qTY;
            }

            set
            {
                qTY = value;
                OnPropertyChanged("QTY");
            }
        }

        [DataMember]
        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }

            set
            {
                serialNumber = value;
                OnPropertyChanged("SerialNumber");
            }
        }
        [DataMember]
        public Int32 ItemNumber
        {
            get
            {
                return itemNumber;
            }

            set
            {
                itemNumber = value;
                OnPropertyChanged("ItemNumber");
            }
        }

        [DataMember]
        public string ItemStatus
        {
            get
            {
                return itemStatus;
            }

            set
            {
                itemStatus = value;
                OnPropertyChanged("ItemStatus");
            }
        }

        [DataMember]
        public string CurrentWorkStation
        {
            get
            {
                return currentWorkStation;
            }

            set
            {
                currentWorkStation = value;
                OnPropertyChanged("CurrentWorkStation");
            }
        }

        [DataMember]
        public List<Counterpartstracking> CounterpartstrackingList
        {
            get
            {
                return counterpartstrackingList;
            }

            set
            {
                counterpartstrackingList = value;
                OnPropertyChanged("CounterpartstrackingList");
            }
        }
        #endregion

        #region Constructor
        public PlantWeeklyReworksMail()
        {

        }
        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
