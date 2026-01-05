using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Windows;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class ExpenseAttendees : ModelBase, IDisposable
    {
        #region Fields
        int idEmployeeExpense;
        int idEmployeeExpenseAttendee;
        string attendeeType;
        HrmCustomer customer;
        Employee employee;
        Visibility isReporter;
        string attendeeName;
        int attendeeId;
        int idAttendeeType;
        #endregion

        #region Constructor
        public ExpenseAttendees()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdEmployeeExpense")]
        [DataMember]
        public int IdEmployeeExpense
        {
            get
            {
                return idEmployeeExpense;
            }

            set
            {
                idEmployeeExpense = value;
                OnPropertyChanged("IdEmployeeExpense");
            }
        }

        [DataMember]
        public int IdEmployeeExpenseAttendee
        {
            get
            {
                return idEmployeeExpenseAttendee;
            }

            set
            {
                idEmployeeExpenseAttendee = value;
                OnPropertyChanged("IdEmployeeExpenseAttendee");
            }
        }

        [DataMember]
        public string AttendeeType
        {
            get
            {
                return attendeeType;
            }

            set
            {
                attendeeType = value;
                OnPropertyChanged("AttendeeType");
            }
        }

        [DataMember]
        public HrmCustomer Customer
        {
            get
            {
                return customer;
            }

            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }

        [DataMember]
        public Employee Employee
        {
            get
            {
                return employee;
            }

            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }

        [DataMember]
        public Visibility IsReporter
        {
            get
            {
                return isReporter;
            }

            set
            {
                isReporter = value;
                OnPropertyChanged("IsReporter");
            }
        }

        [DataMember]
        public string AttendeeName
        {
            get
            {
                return attendeeName;
            }

            set
            {
                attendeeName = value;
                OnPropertyChanged("AttendeeName");
            }
        }

        [DataMember]
        public int AttendeeId
        {
            get
            {
                return attendeeId;
            }

            set
            {
                attendeeId = value;
                OnPropertyChanged("AttendeeId");
            }
        }
        [DataMember]
        public int IdAttendeeType
        {
            get
            {
                return idAttendeeType;
            }

            set
            {
                idAttendeeType = value;
                OnPropertyChanged("IdAttendeeType");
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
