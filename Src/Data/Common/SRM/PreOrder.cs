using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
    public class PreOrder : ModelBase, IDisposable
    {
        #region Declaration
        string observation;
        string po;
        Int32 idWarehouseReOrder;
        string code;
        DateTime calculationDate;
        int createdId;
        string createdBy;
        DateTime creationDate;
        Warehouses warehouse;
        WorkflowStatus status;
        LookupValue logistic;
        int totalReOrder;
        double totalValue;
        Currency currency;
        List<WarehousePurchaseOrder> poList;
        #endregion

        #region Properties

        [DataMember]
        public string PO
        {
            get
            {
                return po;
            }

            set
            {
                po = value;
                OnPropertyChanged("PO");
            }
        }

        [DataMember]
        public List<WarehousePurchaseOrder> PoList
        {
            get
            {
                return poList;
            }

            set
            {
                poList = value;
                OnPropertyChanged("PoList");
            }
        }

        [DataMember]
        public Int32 IdWarehouseReOrder
        {
            get
            {
                return idWarehouseReOrder;
            }

            set
            {
                idWarehouseReOrder = value;
                OnPropertyChanged("IdWarehouseReOrder");
            }
        }

        [DataMember]
        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [DataMember]
        public DateTime CalculationDate
        {
            get
            {
                return calculationDate;
            }

            set
            {
                calculationDate = value;
                OnPropertyChanged("CalculationDate");
            }
        }

        [DataMember]
        public int CreatedId
        {
            get
            {
                return createdId;
            }

            set
            {
                createdId = value;
                OnPropertyChanged("CreatedId");
            }
        }

        [DataMember]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public Warehouses Warehouse
        {
            get
            {
                return warehouse;
            }

            set
            {
                warehouse = value;
                OnPropertyChanged("Warehouse");
            }
        }

        [DataMember]
        public WorkflowStatus Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [DataMember]
        public LookupValue Logistic
        {
            get
            {
                return logistic;
            }

            set
            {
                logistic = value;
                OnPropertyChanged("Logistic");
            }
        }

        [DataMember]
        public int TotalReOrder
        {
            get
            {
                return totalReOrder;
            }

            set
            {
                totalReOrder = value;
                OnPropertyChanged("TotalReOrder");
            }
        }

        [DataMember]
        public double TotalValue
        {
            get
            {
                return totalValue;
            }

            set
            {
                totalValue = value;
                OnPropertyChanged("TotalValue");
            }
        }

        [DataMember]
        public Currency Currency
        {
            get
            {
                return currency;
            }

            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }
        [DataMember]
        public string Observation
        {
            get
            {
                return observation;
            }

            set
            {
                observation = value;
                OnPropertyChanged("Observation");
            }
        }
        #endregion

        #region Constructor

        public PreOrder()
        {
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
