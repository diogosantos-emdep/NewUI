using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Epc.Common.EPC
{
    public class DummyProjects : INotifyPropertyChanged
    {
        int idProject;
        string projectName;
        string projectCode;
        string analysisOwner;
        ProjectStatus projectStatus;
        ProjectStatusType projectStatusTypes;
        ProjectType projectTypes;


        string oTCode;
        string customer;
        string projectOwner;
        string requestDate;
        string requester;
        string startDate;
        string expectedEndDate;
        string estimatedTime;
        string colorProperty;
        string pr;
        private Team teams;

        public Team Teams
        {
            get { return teams; }
            set { teams = value; }
        }



        public int IdProject
        {
            get
            {
                return idProject;
            }

            set
            {
                idProject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdProject"));
            }
        }

        public string ProjectName
        {
            get
            {
                return projectName;
            }

            set
            {
                projectName = value;
            }
        }

        public ProjectStatus Status
        {
            get
            {
                return projectStatus;
            }

            set
            {
                projectStatus = value;
            }
        }

        public string OTCode
        {
            get
            {
                return oTCode;
            }

            set
            {
                oTCode = value;
            }
        }


        public string Customer
        {
            get { return customer; }
            set { customer = value; }
        }

        public string ProjectOwner
        {
            get { return projectOwner; }
            set { projectOwner = value; }
        }

        public string RequestDate
        {
            get { return requestDate; }
            set { requestDate = value; }
        }
        public string Requester
        {
            get { return requester; }
            set { requester = value; }
        }

        public string StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        public string ExpectedEndDate
        {
            get { return expectedEndDate; }
            set { expectedEndDate = value; }
        }
        public string Pr
        {
            get { return pr; }
            set { pr = value; }
        }

        public string EstimatedTime
        {
            get { return estimatedTime; }
            set { estimatedTime = value; }
        }

        public string ColorProperty
        {
            get { return colorProperty; }
            set { colorProperty = value; }
        }

        public string ProjectCode
        {
            get { return projectCode; }
            set { projectCode = value; }
        }
        public string AnalysisOwner
        {
            get { return analysisOwner; }
            set { analysisOwner = value; }
        }

        public ProjectStatusType ProjectStatusTypes
        {
            get { return projectStatusTypes; }
            set { projectStatusTypes = value; }
        }
        public ProjectType ProjectTypes
        {
            get { return projectTypes; }
            set { projectTypes = value; }
        }



        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }


    public class Team
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Teams { get; set; }
        public string Department { get; set; }

        //public Team()
        //{

        //}

        //public Team(int id,int parentId,string teamName,string departmentName)
        //{
        //  ID = id;
        //  //ParentID = parentId;
        //  Teams = teamName;
        //  Department = departmentName;

        //}

    }
}
