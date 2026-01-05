using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Emdep.Geos.Modules.HarnessPart.Class
{
    public class clslogs
    {
        private string logEntry;

        public string LogEntry
        {
            get { return logEntry; }
            set { logEntry = value; }
        }
        private string logEntryDate;

        public string LogEntryDate
        {
            get { return logEntryDate; }
            set { logEntryDate = value; }
        }


        private ObservableCollection<clslogs> _listclslogs;

        public ObservableCollection<clslogs> Listclslogs
        {
            get { return _listclslogs; }
            set { _listclslogs = value; }
        }


    }
}
