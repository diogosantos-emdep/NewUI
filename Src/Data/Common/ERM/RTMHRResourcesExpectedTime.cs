using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.ERM
{
    public class RTMHRResourcesExpectedTime : ModelBase, IDisposable
    {

        #region Field
        private Int64 idOT;
        private Int64 idCP;
        private Int64 idRevisionItem;
        private DateTime? plannedDeliveryDate;
        private DateTime? deliveryDate;
        private string deliveryWeek;
        private Int64 idCPType;
        private Int64 idCounterpart;
        private string serialNumber;
        List<RTMHRResourcesCurrentStage> rTMCurrentStageList;


        #region [GEOS2-4862][Rupali Sarode][25-09-2023]

        private string customer;
        private string project;
        private int idOfferSite;
        private string offerSiteName;
        private string offer;
        private int idCustomer;
        private int idProject;
        #endregion [GEOS2-4862][Rupali Sarode][25-09-2023]

        #endregion

        #region Property
        [DataMember]
        public Int64 IdOT
        {
            get { return idOT; }
            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }

        [DataMember]
        public Int64 IdCP
        {
            get { return idCP; }
            set
            {
                idCP = value;
                OnPropertyChanged("IdCP");
            }
        }

        [DataMember]
        public Int64 IdRevisionItem
        {
            get { return idRevisionItem; }
            set
            {
                idRevisionItem = value;
                OnPropertyChanged("IdRevisionItem");
            }
        }

        [DataMember]
        public DateTime? PlannedDeliveryDate
        {
            get { return plannedDeliveryDate; }
            set
            {
                plannedDeliveryDate = value;
                OnPropertyChanged("PlannedDeliveryDate");
            }
        }

        [DataMember]
        public DateTime? DeliveryDate
        {
            get { return deliveryDate; }
            set
            {
                deliveryDate = value;
                OnPropertyChanged("DeliveryDate");
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
        public Int64 IdCPType
        {
            get
            {
                return idCPType;
            }

            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
            }
        }

        [DataMember]
        public Int64 IdCounterpart
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
        public List<RTMHRResourcesCurrentStage> RTMCurrentStageList
        {
            get
            {
                return rTMCurrentStageList;
            }

            set
            {
                rTMCurrentStageList = value;
                OnPropertyChanged("RTMCurrentStageList");
            }
        }

        #region [GEOS2-4862][Rupali Sarode][25-09-2023]

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
        public int IdOfferSite
        {
            get
            {
                return idOfferSite;
            }

            set
            {
                idOfferSite = value;
                OnPropertyChanged("IdOfferSite");
            }
        }

        [DataMember]
        public string OfferSiteName
        {
            get
            {
                return offerSiteName;
            }

            set
            {
                offerSiteName = value;
                OnPropertyChanged("OfferSiteName");
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
        public int IdCustomer
        {
            get
            {
                return idCustomer;
            }

            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }

        [DataMember]
        public int IdProject
        {
            get
            {
                return idProject;
            }

            set
            {
                idProject = value;
                OnPropertyChanged("IdProject");
            }
        }
        #endregion



        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}
