using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class ExpenseReportCurrency : ModelBase, IDisposable
    {
        #region Fields
        Int64 _IdEmployeeExpenseReport;
        Int64 _IdCurrencyFrom;
        Int64 _IdCurrencyTo;
        string _Code;
        string _fromIsoCode;
        string _ToIsoCode;

        #endregion
        #region Properties
        [DataMember]
        public Int64 IdEmployeeExpenseReport
        {
            get { return _IdEmployeeExpenseReport; }
            set
            {
                _IdEmployeeExpenseReport = value;
                OnPropertyChanged("IdEmployeeExpenseReport");
            }
        }
        [DataMember]
        public Int64 IdCurrencyFrom
        {
            get { return _IdCurrencyFrom; }
            set
            {
                _IdCurrencyFrom = value;
                OnPropertyChanged("IdCurrencyFrom");
            }
        }
        [DataMember]
        public Int64 IdCurrencyTo
        {
            get { return _IdCurrencyTo; }
            set
            {
                _IdCurrencyTo = value;
                OnPropertyChanged("IdCurrencyTo");
            }
        }
        [DataMember]
        public string Code
        {
            get { return _Code; }
            set
            {
                _Code = value;
                OnPropertyChanged("Code");
            }
        }
        [DataMember]
        public string CurrencyFrom
        {
            get { return _fromIsoCode; }
            set
            {
                _fromIsoCode = value;
                OnPropertyChanged("fromIsoCode");
            }
        }
        [DataMember]
        public string CurrencyTo
        {
            get { return _ToIsoCode; }
            set
            {
                _ToIsoCode = value;
                OnPropertyChanged("ToIsoCode");
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
