using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{

    [DataContract]
    public class OrderPreparation : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idOT;
        string otCode;
        string customer;
        DateTime otDeliveryDate;
        Int64 idStatus;
        string statusColor;
   
        string articleStockModifiedBy;
        Int64 actualQty;
        Int64 downloadedQty;
        Int64 remainingQty;
        Int16 progress;
        byte idOfferType;
        List<OtItem> otItems;
        Int64 otDeliveryDateWeek;
        Int64 currentDateWeek;
        List<UserShortDetail> userShortDetails;
        Int32 idCarriageMethod;
        LookupValue carriageMethod;
        byte isCritical;
        #endregion

        #region Properties


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
        public String OTCode
        {
            get { return otCode; }
            set
            {
                otCode = value;
                OnPropertyChanged("OTCode");
            }
        }

        [DataMember]
        public String Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }


        [DataMember]
        public DateTime OTDeliveryDate
        {
            get { return otDeliveryDate; }
            set
            {
                otDeliveryDate = value;
                OnPropertyChanged("OTDeliveryDate");
            }
        }


        [DataMember]
        public Int64 IdStatus
        {
            get { return idStatus; }
            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }



        [DataMember]
        public string ArticleStockModifiedBy
        {
            get { return articleStockModifiedBy; }
            set
            {
                articleStockModifiedBy = value;
                OnPropertyChanged("ArticleStockModifiedBy");
            }
        }

        [DataMember]
        public string StatusColor
        {
            get { return statusColor; }
            set
            {
                statusColor = value;
                OnPropertyChanged("StatusColor");
            }
        }


        [DataMember]
        public Int64 ActualQty
        {
            get { return actualQty; }
            set
            {
                actualQty = value;
                OnPropertyChanged("ActualQty");
            }
        }

        [DataMember]
        public Int64 DownloadedQty
        {
            get { return downloadedQty; }
            set
            {
                downloadedQty = value;
                OnPropertyChanged("DownloadedQty");
            }
        }

        [DataMember]
        public Int64 RemainingQty
        {
            get { return remainingQty; }
            set
            {
                remainingQty = value;
                OnPropertyChanged("RemainingQty");
            }
        }

        [DataMember]
        public List<UserShortDetail> UserShortDetails
        {
            get { return userShortDetails; }
            set
            {
                userShortDetails = value;
                OnPropertyChanged("UserShortDetails");
            }
        }

        [DataMember]
        public Int16 Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }

        [DataMember]
        public List<OtItem> OtItems
        {
            get { return otItems; }
            set
            {
                otItems = value;
                OnPropertyChanged("OtItems");
            }
        }


        [DataMember]
        public byte IdOfferType
        {
            get { return idOfferType; }
            set
            {
                idOfferType = value;
                OnPropertyChanged("IdOfferType");
            }
        }



        [DataMember]
        public Int64 OtDeliveryDateWeek
        {
            get { return otDeliveryDateWeek; }
            set
            {
                otDeliveryDateWeek = value;
                OnPropertyChanged("OtDeliveryDateWeek");
            }
        }



        [DataMember]
        public Int64 CurrentDateWeek
        {
            get { return currentDateWeek; }
            set
            {
                currentDateWeek = value;
                OnPropertyChanged("CurrentDateWeek");
            }
        }


        [DataMember]
        public Int32 IdCarriageMethod
        {
            get { return idCarriageMethod; }
            set
            {
                idCarriageMethod = value;
                OnPropertyChanged("IdCarriageMethod");
            }
        }



        [DataMember]
        public LookupValue CarriageMethod
        {
            get { return carriageMethod; }
            set
            {
                carriageMethod = value;
                OnPropertyChanged("CarriageMethod");
            }
        }


      
        [DataMember]
        public byte IsCritical
        {
            get
            {
                return isCritical;
            }
            set
            {
                isCritical = value;
                OnPropertyChanged("IsCritical");
            }
        }


        #endregion

        #region Constructor

        public OrderPreparation()
        {
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
