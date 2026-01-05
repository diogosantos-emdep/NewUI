using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
    [DataContract]
    public class LogEntriesByWarehousePO : ModelBase, IDisposable
    {
        #region Fields

        Int64 idLogEntryByPurchaseOrder;
        Int64 idWarehousePurchaseOrder;
        Int32 idUser;
        DateTime? datetime;
        string comments;
        byte? idLogEntryType;
        People people;
        bool isRtfText;
        string realText;
        string userName;
        string logDatetime;   //[pramod.misal][GEOS2-4431][16/06/2023]
        Int64 idShippingAddress; //[pramod.misal][GEOS2-4451][06/06/2023]
        int idEntryType;//[pramod.misal][GEOS2-4450][31/07/2023]
        TransactionOperations transactionOperation;//[pramod.misal][GEOS2-4450][31/07/2023]



        #endregion

        #region Constructor

        public LogEntriesByWarehousePO()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public Int64 IdLogEntryByPurchaseOrder
        {
            get { return idLogEntryByPurchaseOrder; }
            set
            {
                idLogEntryByPurchaseOrder = value;
                OnPropertyChanged("IdLogEntryByPurchaseOrder");
            }
        }

        [DataMember]
        public Int64 IdWarehousePurchaseOrder
        {
            get { return idWarehousePurchaseOrder; }
            set
            {
                idWarehousePurchaseOrder = value;
                OnPropertyChanged("IdWarehousePurchaseOrder");
            }
        }

        [DataMember]
        public Int32 IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }

        [DataMember]
        public DateTime? Datetime
        {
            get { return datetime; }
            set
            {
                datetime = value;
                OnPropertyChanged("Datetime");
            }
        }

        [DataMember]
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        [DataMember]
        public byte? IdLogEntryType
        {
            get { return idLogEntryType; }
            set
            {
                idLogEntryType = value;
                OnPropertyChanged("IdLogEntryType");
            }
        }

        [DataMember]
        public People People
        {
            get { return people; }
            set
            {
                people = value;
                OnPropertyChanged("People");
            }
        }

        [DataMember]
        public bool IsRtfText
        {
            get { return isRtfText; }
            set
            {
                isRtfText = value;
                OnPropertyChanged("IsRtfText");
            }
        }

        [DataMember]
        public string RealText
        {
            get { return realText; }
            set
            {
                realText = value;
                OnPropertyChanged("RealText");
            }
        }


        [DataMember]
        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }


        //[pramod.misal][GEOS2-4431][16/06/2023]
        [DataMember]
        public string LogDatetime
        {
            get { return logDatetime; }
            set
            {
                logDatetime = value;
                OnPropertyChanged("LogDatetime");
            }
        }

        //[pramod.misal][GEOS2-4451][04/06/2023]
      
        [DataMember]
        public Int64 IdShippingAddress
        {
            get { return idShippingAddress; }
            set
            {
                idShippingAddress = value;
                OnPropertyChanged("IdShippingAddress");
            }
        }

        [DataMember]
        public int IdEntryType
        {
            get { return idEntryType; }
            set
            {
                idEntryType = value;
                OnPropertyChanged("IdEntryType");
            }
        }


        //[pramod.misal][GEOS2-4450][31/07/2023]
        [DataMember]
        public TransactionOperations TransactionOperation
        {
            get
            {
                return transactionOperation;
            }
            set
            {
                transactionOperation = value;
                this.OnPropertyChanged("TransactionOperation");
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
