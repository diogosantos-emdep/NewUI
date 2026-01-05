using DevExpress.Mvvm;
using Emdep.Geos.Modules.Epc.Common.EPC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class ProjectSchedulerViewModel:NavigationViewModelBase
    {

        string _myFilterString;
        DateTime _startSelectionDate;
        DateTime _finishSelectionDate;
        public DateTime FinishSelectionDate
        {
            get { return _finishSelectionDate; }
            set
            {
                _finishSelectionDate = value;
                UpdateFilterString();
                RaisePropertyChanged("FinishSelectionDate");
            }
        }
        public DateTime StartSelectionDate
        {
            get { return _startSelectionDate; }
            set
            {
                _startSelectionDate = value;
                UpdateFilterString();
                RaisePropertyChanged("StartSelectionDate");
            }
        }
        public void UpdateFilterString()
        {
            if (StartSelectionDate > new DateTime(2000, 1, 1) && FinishSelectionDate > new DateTime(2000, 1, 1))
            {
                string st = string.Format("[Start] >= #{0}# And [Start] < #{1}#", StartSelectionDate.ToString("yyyy-MM-dd"), FinishSelectionDate.ToString("yyyy-MM-dd"));

                MyFilterString = st;
            }
        }
        public string MyFilterString
        {
            get { return _myFilterString; }
            set
            {
                _myFilterString = value;
                RaisePropertyChanged("MyFilterString");
            }
        }

        ObservableCollection<ProjectScheduler> listTasks = new ObservableCollection<ProjectScheduler>();

        public ObservableCollection<ProjectScheduler> ListTasks
        {
            get { return listTasks; }
            set { listTasks = value; }
        }

        public ProjectSchedulerViewModel()
        {
            ListTasks.Add(new ProjectScheduler() { ID = 1, ParentID = 0, Task = "Project" });
            ListTasks.Add(new ProjectScheduler() { ID = 2, ParentID = 1, Task = "Task 1", Start = new DateTime(2016, 04, 22), End = new DateTime(2016, 05, 30), Progress = .10f });
            ListTasks.Add(new ProjectScheduler() { ID = 3, ParentID = 1, Task = "Task 2", Start = new DateTime(2016, 05, 22), End = new DateTime(2016, 06, 28), Progress = .20f });
            ListTasks.Add(new ProjectScheduler() { ID = 4, ParentID = 1, Task = "Task 3", Start = new DateTime(2016, 08, 14), End = new DateTime(2016, 09, 28), Progress = .30f });
            ListTasks.Add(new ProjectScheduler() { ID = 5, ParentID = 1, Task = "Task 4", Start = new DateTime(2016, 11, 02), End = new DateTime(2016, 12, 30), Progress = .40f });
            ListTasks.Add(new ProjectScheduler() { ID = 6, ParentID = 1, Task = "Task 5", Start = new DateTime(2016, 05, 22), End = new DateTime(2016, 06, 30), Progress = .50f });

        }
    }
}
