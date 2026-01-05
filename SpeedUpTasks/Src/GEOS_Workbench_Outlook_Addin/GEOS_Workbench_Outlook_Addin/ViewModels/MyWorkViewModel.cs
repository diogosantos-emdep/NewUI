using System;
using System.Collections.Generic;
using DevExpress.Mvvm;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Emdep.Geos.Modules.Crm.ViewModels
{

    public class MyWorkViewModel : ViewModelBase
    {

        #region Declaration

        private ObservableCollection<MyWork> myWorkList;

        #endregion 

        #region  public Properties
        public ObservableCollection<MyWork> MyWorkList
        {
            get
            {
                return myWorkList;
            }

            set
            {
                myWorkList = value;
            }
        }

        #endregion

        #region  Constructor

        public MyWorkViewModel()
        {
            fillMyWork();

        }

        #endregion 
    
        #region  Methods

        private void fillMyWork()
        {
            MyWorkList = new ObservableCollection<MyWork>();

            MyWorkList.Add(new MyWork() { Group = "YAZAKI", Company = "Yazaki Romania", ActivityType = "Visit", Subject = "Schedule an appointment with Customer(sample)", Status = "Closed", Priority = "High", StartDate = "9/14/2013 10:00 AM", DueDate = "9/14/2013 10:00 AM" });
            MyWorkList.Add(new MyWork() { Group = "LEONI", Company = "Leoni Pitesti", ActivityType = "Phone Call", Subject = "call the customer with relevant information(sample)", Status = "On Going", Priority = "High", StartDate = "9/17/2013 10:00 AM", DueDate = "9/17/2013 10:00 AM" });
            MyWorkList.Add(new MyWork() { Group = " ", Company = "Harnesses Auto", ActivityType = "Phone Call", Subject = "Call back to understand the problem(sample)", Status = "On Going", Priority = "High", StartDate = "9/17/2013 10:00 AM", DueDate = "9/17/2013 10:00 AM" });

            MyWorkList.Add(new MyWork() { Group = "YAZAKI", Company = "Yazaki Romania", ActivityType = "Visit", Subject = "Ask Regionan Manager to call back(sample)", Status = "On Going", Priority = "High", StartDate = "9/18/2013 10:00 AM", DueDate = "9/18/2013 10:00 AM" });
            MyWorkList.Add(new MyWork() { Group = "YAZAKI", Company = "Yazaki Romania", ActivityType = "Phone Call", Subject = "Call back to understand the request(sample)", Status = "On Going", Priority = "High", StartDate = "9/18/2013 10:00 AM", DueDate = "9/18/2013 10:00 AM" });
            MyWorkList.Add(new MyWork() { Group = "YAZAKI", Company = "Yazaki Romania", ActivityType = "Phone Call", Subject = "Call back to understand the problem(sample)", Status = "New", Priority = "Low", StartDate = "9/18/2013 10:00 AM", DueDate = "9/18/2013 10:00 AM" });

            MyWorkList.Add(new MyWork() { Group = "Yazaki", Company = "Yazaki Romania", ActivityType = "Visit", Subject = "Schedule an appointment with Customer(sample)", Status = "Closed", Priority = "High", StartDate = "9/14/2013 10:00 AM", DueDate = "9/14/2013 10:00 AM" });
            MyWorkList.Add(new MyWork() { Group = "Yazaki", Company = "Yazaki Romania", ActivityType = "Visit", Subject = "Schedule an appointment with Customer(sample)", Status = "Closed", Priority = "High", StartDate = "9/14/2013 10:00 AM", DueDate = "9/14/2013 10:00 AM" });
            MyWorkList.Add(new MyWork() { Group = "YAZAKI", Company = "Yazaki Romania", ActivityType = "Visit", Subject = "Schedule an appointment with Customer(sample)", Status = "Closed", Priority = "High", StartDate = "9/14/2013 10:00 AM", DueDate = "9/14/2013 10:00 AM" });

            MyWorkList.Add(new MyWork() { Group = "LEONI", Company = "Leoni Pitesti", ActivityType = "Phone Call", Subject = "call the customer with relevant information(sample)", Status = "On Going", Priority = "High", StartDate = "9/17/2013 10:00 AM", DueDate = "9/17/2013 10:00 AM" });
            MyWorkList.Add(new MyWork() { Group = " ", Company = "Harnesses Auto", ActivityType = "Phone Call", Subject = "Call back to understand the problem(sample)", Status = "On Going", Priority = "High", StartDate = "9/17/2013 10:00 AM", DueDate = "9/17/2013 10:00 AM" });
            MyWorkList.Add(new MyWork() { Group = "YAZAKI", Company = "Yazaki Romania", ActivityType = "Visit", Subject = "Ask Regionan Manager to call back(sample)", Status = "On Going", Priority = "High", StartDate = "9/18/2013 10:00 AM", DueDate = "9/18/2013 10:00 AM" });

            MyWorkList.Add(new MyWork() { Group = "YAZAKI", Company = "Yazaki Romania", ActivityType = "Phone Call", Subject = "Call back to understand the request(sample)", Status = "On Going", Priority = "High", StartDate = "9/18/2013 10:00 AM", DueDate = "9/18/2013 10:00 AM" });
            MyWorkList.Add(new MyWork() { Group = "YAZAKI", Company = "Yazaki Romania", ActivityType = "Phone Call", Subject = "Call back to understand the problem(sample)", Status = "New", Priority = "Low", StartDate = "9/18/2013 10:00 AM", DueDate = "9/18/2013 10:00 AM" });

            MyWorkList.Add(new MyWork() { Group = "Yazaki", Company = "Yazaki Romania", ActivityType = "Visit", Subject = "Schedule an appointment with Customer(sample)", Status = "Closed", Priority = "High", StartDate = "9/14/2013 10:00 AM", DueDate = "9/14/2013 10:00 AM" });
            MyWorkList.Add(new MyWork() { Group = "Yazaki", Company = "Yazaki Romania", ActivityType = "Visit", Subject = "Schedule an appointment with Customer(sample)", Status = "Closed", Priority = "High", StartDate = "9/14/2013 10:00 AM", DueDate = "9/14/2013 10:00 AM" });
        }

        #endregion 
    }
}
