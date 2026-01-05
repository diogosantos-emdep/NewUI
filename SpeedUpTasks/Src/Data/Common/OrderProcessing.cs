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
    public class OrderProcessing : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idOffer;
        string offerCode;
        string customer;
        DateTime poReceivedDate;
        Int64 idStatus;
        string assignedLoginName;
        string statusColor;
        byte[] assignedToImageInBytes;
        byte? idUserGender;
        string userName;
        string poCode;
        byte isCritical;
        #endregion

        #region Properties


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

        [DataMember]
        public String OfferCode
        {
            get { return offerCode; }
            set
            {
                offerCode = value;
                OnPropertyChanged("OfferCode");
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
        public DateTime POReceivedDate
        {
            get { return poReceivedDate; }
            set
            {
                poReceivedDate = value;
                OnPropertyChanged("POReceivedDate");
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
        public string AssignedLoginName
        {
            get { return assignedLoginName; }
            set
            {
                assignedLoginName = value;
                OnPropertyChanged("AssignedLoginName");
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
        public byte[] AssignedToImageInBytes
        {
            get { return assignedToImageInBytes; }
            set
            {
                assignedToImageInBytes = value;
                OnPropertyChanged("AssignedToImageInBytes");
            }
        }

        [DataMember]
        public byte? IdUserGender
        {
            get { return idUserGender; }
            set
            {
                idUserGender = value;
                OnPropertyChanged("IdUserGender");
            }
        }


        [DataMember]
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }


        [DataMember]
        public string POCode
        {
            get { return poCode; }
            set
            {
                poCode = value;
                OnPropertyChanged("POCode");
            }
        }

        #endregion

        #region Constructor

        public OrderProcessing()
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
