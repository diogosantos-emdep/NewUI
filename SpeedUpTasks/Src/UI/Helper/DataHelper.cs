using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Helper
{
    public class DataHelper : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private byte byteValue;
        
        private TimeSpan spanValue0;
        private TimeSpan spanValue1;

        private float floatValue1;
        private float floatValue2;
        private float floatValue3;


        private string stringValue1;    
        private string stringValue2;
        private string stringValue3;

        private DateTime ?dateTime1;
        private DateTime ?dateTime2;

       

        private decimal decimalValue1;
 

        public TimeSpan SpanValue0
        {
            get
            {
                return spanValue0;
            }

            set
            {
                spanValue0 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SpanValue0"));
            }
        }

        public TimeSpan SpanValue1
        {
            get
            {
                return spanValue1;
            }

            set
            {
                spanValue1 = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SpanValue1"));
            }
        }

        public decimal DecimalValue1
        {
            get
            {
                return decimalValue1;
            }

            set
            {
                decimalValue1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DecimalValue1"));
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));
            }
        }

        public float FloatValue2
        {
            get
            {
                return floatValue2;
            }

            set
            {
                floatValue2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FloatValue2"));
            }
        }

        public float FloatValue1
        {
            get
            {
                return floatValue1;
            }

            set
            {
                floatValue1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FloatValue1"));
            }
        }

        public byte ByteValue
        {
            get
            {
                return byteValue;
            }

            set
            {
                byteValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ByteValue"));
            }
        }

        public float FloatValue3
        {
            get
            {
                return floatValue3;
            }

            set
            {
                floatValue3 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FloatValue3"));
            }
        
        }

        public string StringValue1
        {
            get { return stringValue1; }
            set
            {
                stringValue1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StringValue1"));
            }
        }
        public string StringValue2
        {
            get { return stringValue2; }
            set
            {
                stringValue2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StringValue2"));
            }
        }

        public string StringValue3
        {
            get
            {
                return stringValue3;
            }

            set
            {
                stringValue3 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StringValue3"));
            }
        }

        public DateTime ?DateTime1
        {
            get
            {
                return dateTime1;
            }

            set
            {
                dateTime1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DateTime1"));
            }
        }

        public DateTime ?DateTime2
        {
            get
            {
                return dateTime2;
            }

            set
            {
                dateTime2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DateTime2"));
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
