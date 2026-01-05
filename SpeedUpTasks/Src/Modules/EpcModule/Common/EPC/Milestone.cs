using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Epc.Common.EPC
{
    public class Milestone //:INotifyPropertyChanged
    {
        #region Properties
        private string product;
        private string projectName;
        private string projectCode;
        private string workingOrder;
        ProjectType projectType;
        private string owner;
        //public CustomCellValue milestoneDate;
        private DateTime milestoneDate;
        private string milestoneDescription;


        private DateTime milestoneNewDate;


        private int attempts;
        private MilestoneMailType milestoneMail;

        public string Product
        {
            get { return product; }
            set { product = value; }
        }
        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
        public string ProjectCode
        {
            get { return projectCode; }
            set { projectCode = value; }
        }
        public string WorkingOrder
        {
            get { return workingOrder; }
            set { workingOrder = value; }
        }
        public ProjectType ProjectType
        {
            get { return projectType; }
            set { projectType = value; }
        }
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        private string milestoneName;

        public string MilestoneName
        {
            get { return milestoneName; }
            set { milestoneName = value; }
        }
        public DateTime MilestoneDate
        {
            get { return milestoneDate; }
            set { milestoneDate = value; }
        }
        public int Attempts
        {
            get { return attempts; }
            set { attempts = value; }
        }

        public MilestoneMailType MilestoneMail
        {
            get { return milestoneMail; }
            set { milestoneMail = value; }
        }
        public string MilestoneDescription
        {
            get { return milestoneDescription; }
            set { milestoneDescription = value; }
        }
        public DateTime MilestoneNewDate
        {
            get { return milestoneNewDate; }
            set { milestoneNewDate = value; }
        }

        #endregion
    }
}
