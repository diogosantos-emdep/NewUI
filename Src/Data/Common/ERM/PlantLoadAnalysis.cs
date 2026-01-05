using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class PlantLoadAnalysis : ModelBase, IDisposable
    {
        #region Field

        private Int32 idOT;
        private Int32 productionIdSite;
        private string productionSiteName;
        private Int32 originalIdSite;
        private string originalSiteName;
        private string oTCode;
        private string cPType;
        private string template;
        private string oTItemStatus;
        private string project;
        private string region;
        private string customerPlant;
        private string customerGroup;
        private string connectorFamily;
        private Int32 idFamily;
        private string deliveryWeek;
        private Int32 qTY;
        private Int32 idStage;
        private string code;
        Int32 idCustomer;
        private string customerWithPlant;
        private Int32 idOTItem;
        private int idTemplate;
        private int itemNumber;
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
        public Int32 ProductionIdSite
        {
            get
            {
                return productionIdSite;
            }

            set
            {
                productionIdSite = value;
                OnPropertyChanged("productionIdSite");
            }
        }


        [DataMember]
        public string ProductionSiteName
        {
            get
            {
                return productionSiteName;
            }

            set
            {
                productionSiteName = value;
                OnPropertyChanged("ProductionSiteName");
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
        public string OriginalSiteName
        {
            get
            {
                return originalSiteName;
            }

            set
            {
                originalSiteName = value;
                OnPropertyChanged("OriginalSiteName");
            }
        }


        [DataMember]
        public string CPType
        {
            get
            {
                return cPType;
            }

            set
            {
                cPType = value;
                OnPropertyChanged("CPType");
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
        public string OTItemStatus
        {
            get
            {
                return oTItemStatus;
            }

            set
            {
                oTItemStatus = value;
                OnPropertyChanged("OTItemStatus");
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
        public string Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }

        
         [DataMember]
        public string CustomerPlant
        {
            get
            {
                return customerPlant;
            }

            set
            {
                customerPlant = value;
                OnPropertyChanged("CustomerPlant");
            }
        }
        
        [DataMember]
        public string CustomerGroup
        {
            get
            {
                return customerGroup;
            }

            set
            {
                customerGroup = value;
                OnPropertyChanged("CustomerGroup");
            }
        }
        
        [DataMember]
        public string ConnectorFamily
        {
            get
            {
                return connectorFamily;
            }

            set
            {
                connectorFamily = value;
                OnPropertyChanged("ConnectorFamily");
            }
        }

        [DataMember]
        public Int32 IdFamily
        {
            get
            {
                return idFamily;
            }

            set
            {
                idFamily = value;
                OnPropertyChanged("IdFamily");
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
        public Int32 IdStage
        {
            get { return idStage; }
            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
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
        public Int32 IdCustomer
        {
            get { return idCustomer; }
            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }

        [DataMember]
        public string CustomerWithPlant
        {
            get { return customerWithPlant; }
            set
            {
                customerWithPlant = value;
                OnPropertyChanged("CustomerWithPlant");
            }
        }
        public Int32 IdOTItem
        {
            get
            {
                return idOTItem;
            }

            set
            {
                idOTItem = value;
                OnPropertyChanged("IdOTItem");
            }
        }

        public int IdTemplate
        {
            get
            {
                return idTemplate;
            }

            set
            {
                idTemplate = value;
                OnPropertyChanged("IdTemplate");
            }
        }

        public int ItemNumber
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
        #endregion

        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}
