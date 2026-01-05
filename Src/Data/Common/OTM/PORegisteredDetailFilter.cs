using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    /// <summary>
    /// [pooja.jadhav][14-11-2024][GEOS2-GEOS2-6460]
    /// </summary>
    public class PORegisteredDetailFilter: ModelBase, IDisposable
    {

        #region Fields
        
        int? number;
        string type;
        int? idType;
        string group;
        int? idGroup;
        string customerPlant;
        int? idCustomerPlant;
        string sender;
        string currency;
        int? idCurrency;
        double? poValueRangeFrom;
        double? poValueRangeTo;
        string offer;
        DateTime? receptionDateFrom;
        DateTime? receptionDateTo;
        DateTime? creationDateFrom;
        DateTime? creationDateTo;
        DateTime? updateDateFrom;
        DateTime? updateDateTo;
        DateTime? cancellationDateFrom;
        DateTime? cancellationDateTo;

        #endregion

        #region Properties


        public int? Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        public int? IdType
        {
            get { return idType; }
            set
            {
                idType = value;
                OnPropertyChanged("IdType");
            }
        }

        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }

        public int? IdGroup
        {
            get { return idGroup; }
            set
            {
                idGroup = value;
                OnPropertyChanged("IdGroup");
            }
        }

        public string CustomerPlant
        {
            get { return customerPlant; }
            set
            {
                customerPlant = value;
                OnPropertyChanged("CustomerPlant");
            }
        }

        public int? IdCustomerPlant
        {
            get { return idCustomerPlant; }
            set
            {
                idCustomerPlant = value;
                OnPropertyChanged("IdCustomerPlant");
            }
        }

        public string Sender
        {
            get { return sender; }
            set
            {
                sender = value;
                OnPropertyChanged("Sender");
            }
        }

        public string Currency
        {
            get { return currency; }
            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }

        public int? IdCurrency
        {
            get { return idCurrency; }
            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        public double? PoValueRangeFrom
        {
            get { return poValueRangeFrom; }
            set
            {
                poValueRangeFrom = value;
                OnPropertyChanged("PoValueRangeFrom");
            }
        }

        public double? PoValueRangeTo
        {
            get { return poValueRangeTo; }
            set
            {
                poValueRangeTo = value;
                OnPropertyChanged("PoValueRangeTo");
            }
        }

        public string Offer
        {
            get { return offer; }
            set
            {
                offer = value;
                OnPropertyChanged("Offer");
            }
        }

        public DateTime? ReceptionDateFrom
        {
            get { return receptionDateFrom; }
            set
            {
                receptionDateFrom = value;
                OnPropertyChanged("ReceptionDateFrom");
            }
        }

        public DateTime? ReceptionDateTo
        {
            get { return receptionDateTo; }
            set
            {
                receptionDateTo = value;
                OnPropertyChanged("ReceptionDateTo");
            }
        }

        public DateTime? CreationDateFrom
        {
            get { return creationDateFrom; }
            set
            {
                creationDateFrom = value;
                OnPropertyChanged("CreationDateFrom");
            }
        }

        public DateTime? CreationDateTo
        {
            get { return creationDateTo; }
            set
            {
                creationDateTo = value;
                OnPropertyChanged("CreationDateTo");
            }
        }

        public DateTime? UpdateDateFrom
        {
            get { return updateDateFrom; }
            set
            {
                updateDateFrom = value;
                OnPropertyChanged("UpdateDateFrom");
            }
        }

        public DateTime? UpdateDateTo
        {
            get { return updateDateTo; }
            set
            {
                updateDateTo = value;
                OnPropertyChanged("UpdateDateTo");
            }
        }


        public DateTime? CancellationDateFrom
        {
            get { return cancellationDateFrom; }
            set
            {
                cancellationDateFrom = value;
                OnPropertyChanged("CancellationDateFrom");
            }
        }

        public DateTime? CancellationDateTo
        {
            get { return cancellationDateTo; }
            set
            {
                cancellationDateTo = value;
                OnPropertyChanged("CancellationDateTo");
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
