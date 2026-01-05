using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Epc.Common.EPC
{
    public class ProjectTask : ViewModelBase
    {
        private string product;
        private string projectName;
        private string projectCode;
        private string taskName;
        private string workingOrder;
        private string taskType;
        private string owner;
        private LookupValue priority;
        private LookupValue status;
        private TimeSpan expectedTime;
        private TimeSpan currentTime;
        private DateTime statDate;
        private DateTime requestDate;

        public DateTime RequestDate
        {
            get { return requestDate; }
            set { requestDate = value; }
        }
        private string watcher;

        public string Watcher
        {
            get { return watcher; }
            set { watcher = value; }
        }
        private string requester;

        public string Requester
        {
            get { return requester; }
            set { requester = value; }
        }
        private int efforts;

        public int Efforts
        {
            get { return efforts; }
            set { efforts = value; }
        }


        private int estimate;

        public int Estimate
        {
            get { return estimate; }
            set { estimate = value; }
        }

        private DateTime StatDate
        {
            get { return statDate; }
            set
            {
                statDate = value;
            }
        }
        private DateTime dueDate;
        private float progress;


        public bool IsBlock { get; set; }

        public string Product
        {
            get { return product; }
            set { product = value; }
        }

        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                if (projectName == value) return;
                projectName = value;
                RaisePropertyChanged("ProjectName");
            }
        }


        public string ProjectCode
        {
            get { return projectCode; }
            set { projectCode = value; }
        }


        public string TaskName
        {
            get { return taskName; }
            set
            {
                taskName = value;

            }
        }

        public string WorkingOrder
        {
            get { return workingOrder; }
            set { workingOrder = value; }
        }


        public string TaskType
        {
            get { return taskType; }
            set { taskType = value; }
        }


        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }


        public LookupValue Priority
        {
            get { return priority; }
            set { priority = value; }
        }


        public LookupValue Status
        {
            get { return status; }
            set { status = value; }
        }


        public TimeSpan ExpectedTime
        {
            get { return expectedTime; }
            set { expectedTime = value; }
        }


        public TimeSpan CurrentTime
        {
            get { return currentTime; }
            set
            {
                currentTime = value;
                SetProperty(ref currentTime, value, () => CurrentTime);
            }
        }

        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        public float Progress
        {
            get { return progress; }
            set { progress = value; }
        }

        public ObservableCollection<ChangeLog> listChangeLog = new ObservableCollection<ChangeLog>();

        public ObservableCollection<ChangeLog> ListChangeLog
        {
            get { return listChangeLog; }
            set
            {
                listChangeLog = value;
                SetProperty(ref listChangeLog, value, () => ListChangeLog);
            }
        }

        public ObservableCollection<TrackerHistory> listTrakerHistory = new ObservableCollection<TrackerHistory>();

        public ObservableCollection<TrackerHistory> ListTrakerHistory
        {
            get { return listTrakerHistory; }
            set
            {
                listTrakerHistory = value;
                SetProperty(ref listTrakerHistory, value, () => ListTrakerHistory);
            }
        }

        public ObservableCollection<Comments> listComments = new ObservableCollection<Comments>();

        public ObservableCollection<Comments> ListComments
        {
            get { return listComments; }
            set
            {
                listComments = value;
                SetProperty(ref listComments, value, () => ListComments);
            }
        }



        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<FileInfo> attachmentList; //supportWindow attachment document List
        public ObservableCollection<FileInfo> AttachmentList
        {
            get { return attachmentList; }
            set { attachmentList = value; OnPropertyChanged(new PropertyChangedEventArgs("AttachmentList")); }
        }

    }



    public class ChangeLog
    {
        string user;

        public string User
        {
            get { return user; }
            set { user = value; }
        }
        string action;

        public string Action
        {
            get { return action; }
            set { action = value; }
        }
        DateTime logDate;

        public DateTime LogDate
        {
            get { return logDate; }
            set { logDate = value; }
        }
    }



    public class TrackerHistory
    {
        string user;

        public string User
        {
            get { return user; }
            set { user = value; }
        }
        DateTime trackingDate;

        public DateTime TrackingDate
        {
            get { return trackingDate; }
            set { trackingDate = value; }
        }


        TimeSpan trackedTime;

        public TimeSpan TrackedTime
        {
            get { return trackedTime; }
            set { trackedTime = value; }
        }

    }

    public class Comments
    {
        string user;

        public string User
        {
            get { return user; }
            set { user = value; }
        }
        DateTime commentDate;

        public DateTime CommentDate
        {
            get { return commentDate; }
            set { commentDate = value; }
        }
        private string comment;

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                //SetProperty(ref comment, value, () => Comment); 
            }
        }


    }
}
