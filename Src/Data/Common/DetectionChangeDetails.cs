using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class DetectionChangeDetails
    {
        string changeType;
        string employee;
        string code;
        string status;
        string name;
        string originalValue;
        string newValue;
        UInt64 idCPType;
        DateTime date;
        string field;

        [DataMember]
        public string ChangeType
        {
            get
            {
                return changeType;
            }
            set
            {

                changeType = value;
                OnPropertyChanged("ChangeType");
            }
        }
        [DataMember]
        public string Employee
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
        public string Status
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
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        [DataMember]
        public string Field
        {
            get
            {
                return field;
            }

            set
            {
                field = value;
                OnPropertyChanged("Field");
            }
        }
        [DataMember]
        public string OriginalValue
        {
            get
            {
                return originalValue;
            }

            set
            {
                originalValue = value;
                OnPropertyChanged("OriginalValue");
            }
        }
        [DataMember]
        public string NewValue
        {
            get
            {
                return newValue;
            }

            set
            {
                newValue = value;
                OnPropertyChanged("NewValue");
            }
        }
        [DataMember]
        public UInt64 IdCPType
        {
            get { return idCPType; }
            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
            }
        }
        [DataMember]
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    
    }

}
