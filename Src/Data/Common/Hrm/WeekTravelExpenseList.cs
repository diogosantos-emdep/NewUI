using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class WeekTravelExpenseList : ModelBase, IDisposable
    {
        #region Fields
        string monDate;
        string expenseType;
        string monExpenses;
        string tueExpenses;
        string wedExpenses;
        string thuExpenses;
        string friExpenses;
        string satExpenses;
        string sunExpenses;
        string weekTotalExpenses;
        double monExpensescal;
        double tueExpensesCal;
        double wedExpensesCal;
        double thuExpensesCal;
        double friExpensesCal;
        double satExpensesCal;
        double sunExpensesCal;
        double weekTotalExpensesCal;
        string curSymbol;
        string category;
        double monMealExpenses;
        double tueMealExpenses;
        double wedMealExpenses;
        double thuMealExpenses;
        double friMealExpenses;
        double satMealExpenses;
        double sunMealExpenses;
        double mealExpense;
        Visibility monIsAttendee = Visibility.Hidden;
        Visibility tueIsAttendee = Visibility.Hidden;
        Visibility wedIsAttendee = Visibility.Hidden;
        Visibility thuIsAttendee = Visibility.Hidden;
        Visibility friIsAttendee = Visibility.Hidden;
        Visibility satIsAttendee = Visibility.Hidden;
        Visibility sunIsAttendee = Visibility.Hidden;
        string monToolTip = string.Empty;
        string tueToolTip = string.Empty;
        string wedToolTip = string.Empty;
        string thuToolTip = string.Empty;
        string friToolTip = string.Empty;
        string satToolTip = string.Empty;
        string sunToolTip = string.Empty;
        #endregion
        #region Properties
        [DataMember]
        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }
        [DataMember]
        public string CurSymbol
        {
            get
            {
                return curSymbol;
            }

            set
            {
                curSymbol = value;
                OnPropertyChanged("CurSymbol");
            }
        }

        [DataMember]
        public string ExpenseType
        {
            get
            {
                return expenseType;
            }

            set
            {
                expenseType = value;
                OnPropertyChanged("ExpenseType");
            }
        }
     
        //[DataMember]
        //public string MonDate
        //{
        //    get
        //    {
        //        return monDate;
        //    }

        //    set
        //    {
        //        monDate = value;
        //        OnPropertyChanged("MonDate");
        //    }
        //}

        [DataMember]
        public string MonExpenses
        {
            get
            {
                return monExpenses;
            }

            set
            {
                monExpenses = value;
                OnPropertyChanged("MonExpenses");
            }
        }
        [DataMember]
        public string TueExpenses
        {
            get
            {
                return tueExpenses;
            }

            set
            {
                tueExpenses = value;
                OnPropertyChanged("TueExpenses");
            }
        }
        [DataMember]
        public string WedExpenses
        {
            get
            {
                return wedExpenses;
            }

            set
            {
                wedExpenses = value;
                OnPropertyChanged("WedExpenses");
            }
        }
        [DataMember]
        public string ThuExpenses
        {
            get
            {
                return thuExpenses;
            }

            set
            {
                thuExpenses = value;
                OnPropertyChanged("ThuExpenses");
            }
        }
        [DataMember]
        public string FriExpenses
        {
            get
            {
                return friExpenses;
            }

            set
            {
                friExpenses = value;
                OnPropertyChanged("FriExpenses");
            }
        }
        [DataMember]
        public string SatExpenses
        {
            get
            {
                return satExpenses;
            }

            set
            {
                satExpenses = value;
                OnPropertyChanged("SatExpenses");
            }
        }
        [DataMember]
        public string SunExpenses
        {
            get
            {
                return sunExpenses;
            }

            set
            {
                sunExpenses = value;
                OnPropertyChanged("SunExpenses");
            }
        }
        [DataMember]
        public string WeekTotalExpenses
        {
            get
            {
                return weekTotalExpenses;
            }

            set
            {
                weekTotalExpenses = value;
                OnPropertyChanged("WeekTotalExpenses");
            }
        }        

        [DataMember]
        public double MonExpensescal
        {
            get
            {
                return monExpensescal;
            }

            set
            {
                monExpensescal = value;
                OnPropertyChanged("MonExpensescal");
            }
        }
        [DataMember]
        public double TueExpensesCal
        {
            get
            {
                return tueExpensesCal;
            }

            set
            {
                tueExpensesCal = value;
                OnPropertyChanged("TueExpensesCal");
            }
        }
        [DataMember]
        public double WedExpensesCal
        {
            get
            {
                return wedExpensesCal;
            }

            set
            {
                wedExpensesCal = value;
                OnPropertyChanged("WedExpensesCal");
            }
        }
        [DataMember]
        public double ThuExpensesCal
        {
            get
            {
                return thuExpensesCal;
            }

            set
            {
                thuExpensesCal = value;
                OnPropertyChanged("ThuExpensesCal");
            }
        }
        [DataMember]
        public double FriExpensesCal
        {
            get
            {
                return friExpensesCal;
            }

            set
            {
                friExpensesCal = value;
                OnPropertyChanged("FriExpensesCal");
            }
        }
        [DataMember]
        public double SatExpensesCal
        {
            get
            {
                return satExpensesCal;
            }

            set
            {
                satExpensesCal = value;
                OnPropertyChanged("SatExpensesCal");
            }
        }
        [DataMember]
        public double SunExpensesCal
        {
            get
            {
                return sunExpensesCal;
            }

            set
            {
                sunExpensesCal = value;
                OnPropertyChanged("SunExpensesCal");
            }
        }
        [DataMember]
        public double WeekTotalExpensesCal
        {
            get
            {
                return weekTotalExpensesCal;
            }

            set
            {
                weekTotalExpensesCal = value;
                OnPropertyChanged("WeekTotalExpensesCal");
            }
        }

        [DataMember]
        public double MealExpense
        {
            get
            {
                return mealExpense;
            }

            set
            {
                mealExpense = value;
                OnPropertyChanged("MealExpense");
            }
        }
        [DataMember]
        public double MonMealExpenses
        {
            get
            {
                return monMealExpenses;
            }

            set
            {
                monMealExpenses = value;
                OnPropertyChanged("MonMealExpenses");
            }
        }
        [DataMember]
        public double TueMealExpenses
        {
            get
            {
                return tueMealExpenses;
            }

            set
            {
                tueMealExpenses = value;
                OnPropertyChanged("TueMealExpenses");
            }
        }
        [DataMember]
        public double WedMealExpenses
        {
            get
            {
                return wedMealExpenses;
            }

            set
            {
                wedMealExpenses = value;
                OnPropertyChanged("WedMealExpenses");
            }
        }
        [DataMember]
        public double ThuMealExpenses
        {
            get
            {
                return thuMealExpenses;
            }

            set
            {
                thuMealExpenses = value;
                OnPropertyChanged("ThuMealExpenses");
            }
        }
        [DataMember]
        public double FriMealExpenses
        {
            get
            {
                return friMealExpenses;
            }

            set
            {
                friMealExpenses = value;
                OnPropertyChanged("FriMealExpenses");
            }
        }
        [DataMember]
        public double SatMealExpenses
        {
            get
            {
                return satMealExpenses;
            }

            set
            {
                satMealExpenses = value;
                OnPropertyChanged("SatMealExpenses");
            }
        }
        [DataMember]
        public double SunMealExpenses
        {
            get
            {
                return sunMealExpenses;
            }

            set
            {
                sunMealExpenses = value;
                OnPropertyChanged("SunMealExpenses");
            }
        }

        [DataMember]
        public Visibility MonIsAttendee
        {
            get
            {
                return monIsAttendee;
            }

            set
            {
                monIsAttendee = value;
                OnPropertyChanged("MonIsAttendee");
            }
        }

        [DataMember]
        public Visibility TueIsAttendee
        {
            get
            {
                return tueIsAttendee;
            }

            set
            {
                tueIsAttendee = value;
                OnPropertyChanged("TueIsAttendee");
            }
        }

        [DataMember]
        public Visibility WedIsAttendee
        {
            get
            {
                return wedIsAttendee;
            }

            set
            {
                wedIsAttendee = value;
                OnPropertyChanged("WedIsAttendee");
            }
        }

        [DataMember]
        public Visibility ThuIsAttendee
        {
            get
            {
                return thuIsAttendee;
            }

            set
            {
                thuIsAttendee = value;
                OnPropertyChanged("ThuIsAttendee");
            }
        }

        [DataMember]
        public Visibility FriIsAttendee
        {
            get
            {
                return friIsAttendee;
            }

            set
            {
                friIsAttendee = value;
                OnPropertyChanged("FriIsAttendee");
            }
        }

        [DataMember]
        public Visibility SatIsAttendee
        {
            get
            {
                return satIsAttendee;
            }

            set
            {
                satIsAttendee = value;
                OnPropertyChanged("SatIsAttendee");
            }
        }

        [DataMember]
        public Visibility SunIsAttendee
        {
            get
            {
                return sunIsAttendee;
            }

            set
            {
                sunIsAttendee = value;
                OnPropertyChanged("SunIsAttendee");
            }
        }

        [DataMember]
        public string MonToolTip
        {
            get
            {
                return monToolTip;
            }

            set
            {
                monToolTip = value;
                OnPropertyChanged("MonToolTip");
            }
        }
        [DataMember]
        public string TueToolTip
        {
            get
            {
                return tueToolTip;
            }

            set
            {
                tueToolTip = value;
                OnPropertyChanged("TueToolTip");
            }
        }
        [DataMember]
        public string WedToolTip
        {
            get
            {
                return wedToolTip;
            }

            set
            {
                wedToolTip = value;
                OnPropertyChanged("WedToolTip");
            }
        }
        [DataMember]
        public string ThuToolTip
        {
            get
            {
                return thuToolTip;
            }

            set
            {
                thuToolTip = value;
                OnPropertyChanged("ThuToolTip");
            }
        }
        [DataMember]
        public string FriToolTip
        {
            get
            {
                return friToolTip;
            }

            set
            {
                friToolTip = value;
                OnPropertyChanged("FriToolTip");
            }
        }
        [DataMember]
        public string SatToolTip
        {
            get
            {
                return satToolTip;
            }

            set
            {
                satToolTip = value;
                OnPropertyChanged("SatToolTip");
            }
        }
        [DataMember]
        public string SunToolTip
        {
            get
            {
                return sunToolTip;
            }

            set
            {
                sunToolTip = value;
                OnPropertyChanged("SunToolTip");
            }
        }
        //[Rahul.Gadhave][GEOS2-8022][Date:11/06/2025]
       
        public bool IsMonExchangeRateZero { get; set; }
        public bool IsTueExchangeRateZero { get; set; }
        public bool IsWedExchangeRateZero { get; set; }
        public bool IsThuExchangeRateZero { get; set; }
        public bool IsFriExchangeRateZero { get; set; }
        public bool IsSatExchangeRateZero { get; set; }
        public bool IsSunExchangeRateZero { get; set; }
        #endregion

        #region Constructor
        public WeekTravelExpenseList()
        { }
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
