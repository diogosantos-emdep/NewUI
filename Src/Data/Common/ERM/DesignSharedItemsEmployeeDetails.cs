using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.ERM
{
    public class DesignSharedItemsEmployeeDetails : ModelBase, IDisposable
    {
        #region Field [GEOS2-7091][rani dhamankar] [03-09-2025]

        private string designerName;
        private Int64 idCounterpart;
        private Int64 idCounterpartTracking;
        private Int32 idDesignerFromConsumerPlant;
        private Int64 idOTItem;
        private Int32 idSiteOwnersPlant;
        private Int32 idSiteConsumerPlant;
       // private Int32 idOperator;
        private Int32 idStage;
        private DateTime? sharedDate;
        private DateTime? returnedDate;
        private Int32 itemNumber;
        private string oTCode;
      
        #endregion

        #region Property
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
        public Int32 IdStage
        {
            get
            {
                return idStage;
            }

            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }
        [DataMember]
        public Int32 IdDesignerFromConsumerPlant
        {
            get
            {
                return idDesignerFromConsumerPlant;
            }

            set
            {
                idDesignerFromConsumerPlant = value;
                OnPropertyChanged("IdDesignerFromConsumerPlant");
            }
        }

        [DataMember]
        public Int32 IdSiteOwnersPlant
        {
            get
            {
                return idSiteOwnersPlant;
            }

            set
            {
                idSiteOwnersPlant = value;
                OnPropertyChanged("IdSiteOwnersPlant");
            }
        }

        [DataMember]
        public Int32 IdSiteConsumerPlant
        {
            get
            {
                return idSiteConsumerPlant;
            }

            set
            {
                idSiteConsumerPlant = value;
                OnPropertyChanged("idSiteConsumerPlant");
            }
        }


        [DataMember]
        public DateTime? SharedDate
        {
            get
            {
                return sharedDate; 
            }

            set
            {
                sharedDate = value;
                OnPropertyChanged("SharedDate");
            }
        }
        [DataMember]
        public DateTime? ReturnedDate
        {
            get
            {
                return returnedDate;
            }

            set
            {
                returnedDate = value;
                OnPropertyChanged("ReturnedDate");
            }
        }
        
        [DataMember]
        public Int64 IdCounterparttracking
        {
            get
            {
                return idCounterpartTracking;
            }

            set
            {
                idCounterpartTracking = value;
                OnPropertyChanged("IdCounterpartTracking");
            }
        }
       

        [DataMember]
        public string DesignerName
        {
            get
            {
                return designerName;
            }

            set
            {
                designerName = value;
                OnPropertyChanged("DesignerName");
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

        public Int64 IdOTItem
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}
