using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class LogEntriesByTravelExpense : ModelBase, IDisposable
    {
        #region Fields
        Int64 idEmployeeExpenseReportChangeLog;
        Int64 idEmployeeExpenseReport;
        Int32 idUser;
        DateTime? datetime;
        string comments;
        string userName;
        Int64 idEmployeeExpense;
        //Comments
        bool isRtfText;//[chitra.girigosavi][GEOS2-4824][02.11.2023]
        Int32 idLogEntryType;//[chitra.girigosavi][GEOS2-4824][02.11.2023]
        People people;//[chitra.girigosavi][GEOS2-4824][02.11.2023]
        int idEntryType;//[chitra.girigosavi][GEOS2-4824][02.11.2023]
        //End Comments
        #endregion

        #region Constructor

        public LogEntriesByTravelExpense()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public Int64 IdEmployeeExpenseReportChangeLog
        {
            get { return idEmployeeExpenseReportChangeLog; }
            set
            {
                idEmployeeExpenseReportChangeLog = value;
                OnPropertyChanged("IdEmployeeExpenseReportChangeLog");
            }
        }

        [DataMember]
        public Int64 IdEmployeeExpenseReport
        {
            get { return idEmployeeExpenseReport; }
            set
            {
                idEmployeeExpenseReport = value;
                OnPropertyChanged("IdEmployeeExpenseReport");
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
        public Int64 IdEmployeeExpense
        {
            get { return idEmployeeExpense; }
            set
            {
                idEmployeeExpense = value;
                OnPropertyChanged("IdEmployeeExpense");
            }
        }
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
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
        public Int32 IdLogEntryType
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
        public int IdEntryType
        {
            get { return idEntryType; }
            set
            {
                idEntryType = value;
                OnPropertyChanged("IdEntryType");
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
