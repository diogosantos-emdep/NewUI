using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class WeeklyTravelExpenses : ModelBase, IDisposable
    {
        #region Fields
        string header;
        ObservableCollection<WeekTravelExpenseList> weekTravelExpenseList;
        string monDate;
        string tuesday;
        string wednesday;
        string thursday;
        string friday;
        string saturday;
        string sunday;
        #endregion

        #region Constructor
        public WeeklyTravelExpenses()
        {

        }
        #endregion

        #region Properties

        [DataMember]
        public string Header
        {
            get
            {
                return header;
            }

            set
            {
                header = value;
                OnPropertyChanged("Header");
            }
        }

        [DataMember]
        public ObservableCollection<WeekTravelExpenseList> WeekTravelExpenseList
        {
            get
            {
                return weekTravelExpenseList;
            }

            set
            {
                weekTravelExpenseList = value;
                OnPropertyChanged("WeekTravelExpenseList");
            }
        }

        [DataMember]
        public string MonDate
        {
            get
            {
                return monDate;
            }

            set
            {
                monDate = value;
                OnPropertyChanged("MonDate");
            }
        }
        [DataMember]
        public string TueDate
        {
            get
            {
                return tuesday;
            }

            set
            {
                tuesday = value;
                OnPropertyChanged("TueDate");
            }
        }
        [DataMember]
        public string WedDate
        {
            get
            {
                return wednesday;
            }

            set
            {
                wednesday = value;
                OnPropertyChanged("WedDate");
            }
        }
            [DataMember]
            public string ThuDate
        {
            get
            {
                return thursday;
            }

            set
            {
                thursday = value;
                OnPropertyChanged("ThuDate");
            }
        }
        [DataMember]
        public string FriDate
        {
            get
            {
                return friday;
            }

            set
            {
                friday = value;
                OnPropertyChanged("FriDate");
            }
        }
        [DataMember]
        public string SatDate
        {
            get
            {
                return saturday;
            }

            set
            {
                saturday = value;
                OnPropertyChanged("SatDate");
            }
        }
        [DataMember]
        public string SunDate
        {
            get
            {
                return sunday;
            }

            set
            {
                sunday = value;
                OnPropertyChanged("SunDate");
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
            //var newWeeklyTravelExpensesClone = (WeeklyTravelExpenses)this.MemberwiseClone();

            //if (WeekTravelExpenseList != null)
            //    newWeeklyTravelExpensesClone.WeekTravelExpenseList = new List<WeekTravelExpenseList>(WeekTravelExpenseList.Select(x => (WeekTravelExpenseList)x.Clone()));
            //return newWeeklyTravelExpensesClone;
        }

        #endregion
    }
}
