using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class WorkorderItemPrintDetails : ModelBase, IDisposable
    {
        #region Fields

        string customer;
        string workorder;
        string carriageMethod;
        Image headerImage;
        DateTime? deliveryDate;
        string deliveryDateString;
        Image carriageType;
        string carriageText;
        string tradeGroup;

        #endregion

        #region Constructor
        public WorkorderItemPrintDetails()
        {
        }
        #endregion

        #region Properties

        public string Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }

        public string Workorder
        {
            get { return workorder; }
            set
            {
                workorder = value;
                OnPropertyChanged("Workorder");
            }
        }

        public string CarriageMethod
        {
            get { return carriageMethod; }
            set
            {
                carriageMethod = value;
                OnPropertyChanged("CarriageMethod");
            }
        }

        public string CarriageText
        {
            get { return carriageText; }
            set
            {
                carriageText = value;
                OnPropertyChanged("CarriageMethod");
            }
        }

        public Image HeaderImage
        {
            get { return headerImage; }
            set
            {
                headerImage = value;
                OnPropertyChanged("HeaderImage");
            }
        }

        public Image CarriageType
        {
            get { return carriageType; }
            set
            {
                carriageType = value;
                OnPropertyChanged("CarriageType");
            }
        }

        public DateTime? DeliveryDate
        {
            get { return deliveryDate; }
            set
            {
                deliveryDate = value;
                OnPropertyChanged("DeliveryDate");
            }
        }

        public string DeliveryDateString
        {
            get { return deliveryDateString; }
            set
            {
                deliveryDateString = value;
                OnPropertyChanged("DeliveryDateString");
            }
        }

        public string TradeGroup
        {
            get { return tradeGroup; }
            set
            {
                tradeGroup = value;
                OnPropertyChanged("TradeGroup");
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
