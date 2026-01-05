using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Helper
{
    public class ChartDataItem: INotifyPropertyChanged
    {
        string series;
        string argument;
        double value;
        float time;
        DayOfWeek weekDay;
        DateTime currentDate;
        public string Series
        {
            get
            {
                return series;
            }

            set
            {
                series = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Series"));
            }
        }

        public string Argument
        {
            get
            {
                return argument;
            }

            set
            {
                argument = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Argument"));
            }
        }

        public double Value2
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Value2"));
            }
        }

        public float Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Time"));
            }
        }

        public DateTime CurrentDate
        {
            get
            {
                return currentDate;
            }

            set
            {
               currentDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentDate"));
            }
        }

        public DayOfWeek WeekDay
        {
            get
            {
                return weekDay;
            }

            set
            {
                weekDay = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WeekDay"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }

   
}
