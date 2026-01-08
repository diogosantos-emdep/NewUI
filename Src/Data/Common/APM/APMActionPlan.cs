using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Emdep.Geos.Data.Common.APM
{
    [DataContract]
    public class APMActionPlan : ModelBase, IDisposable
    {
        #region Declaration
        Int64 idActionPlan;
        string code;
        string description;
        Int32 idCompany;
        string location;
        Int32 idEmployee;
        string firstName;
        string lastName;
        Int32 idLookupValue;
        string businessUnit;
        Int32 idLookupOrigin;
        string origin;
        int totalActionItems;
        int totalOpenItems;
        int totalClosedItems;
        int percentage;
        string totalClosedColor;
        string name;
        List<APMActionPlanTask> taskList;
        Int32 modifiedBy;
        DateTime modifiedIn;
        Int32 createdBy;
        DateTime? createdIn;
        private Country country;//[Sudhir.Jangra][GEOS2-6397] 
        private string employeeCode;//[Sudhir.Jangra][GEOS2-6397]
        private int idGender;//[Sudhir.Jangra][GEOS2-6397]

        List<LogEntriesByActionPlan> actionPlanLogEntries;//[Shweta.Thube][GEOS2-6020]
        private List<ActionPlanComment> commentList;
        private string createdByName;//[Sudhir.jangra][GEOS2-6595]
        Int32 idLookupBusinessUnit;//[Shweta.Thube][GEOS2-6586]
        private Int32 idDepartment;//[Sudhir.Jangra][GEOS2-6596]
        private string department;//[Sudhir.Jangra][GEOS2-6596]
        private string businessUnitHTMLColor;//[Shweta.Thube][GEOS2-6587]
        string originDescription;//[Shweta.Thube][GEOS2-6794]
        bool isActionPlanDeleted;//[Shweta.Thube][GEOS2-6795]
        private Int32 responsibleIdUser;//[Sudhir.Jangra][GEOS2-6882]
        private string actionPlanResponsibleDisplayName;//[Sudhir.Jangra][GEOS2-6897]
        private Int32 idCustomer;//[Shweta.Thube][GEOS2-6911]
        private Int32 idSite;//[Shweta.Thube][GEOS2-6911]
        private string site;//[Shweta.Thube][GEOS2-6911]
        private string group;//[Shweta.Thube][GEOS2-6911]
        private string groupName;//[Shweta.Thube][GEOS2-6911]
        private Int32 idZone;//[Shweta.Thube][GEOS2-6911]
        private string zone;//[Shweta.Thube][GEOS2-6911]
        private Int32 maxTaskNumber; //[shweta.thube][GEOS2-9273][08.09.2025]
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public Int64 IdActionPlan
        {
            get { return idActionPlan; }
            set
            {
                idActionPlan = value;
                OnPropertyChanged("IdActionPlan");
            }
        }


        #region Novas Propriedades para Filtros Modernos (Adicionar Manualmente)

        [DataMember]
        public string ThemeAggregates { get; set; }

        [DataMember]
        public string StatusAggregates { get; set; }

        [DataMember]
        public int Stat_Overdue15 { get; set; }

        [DataMember]
        public int Stat_HighPriorityOverdue { get; set; }

        [DataMember]
        public int Stat_MaxDueDays { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        // Se já tens IdCompany, podes adicionar IdLocation a apontar para o mesmo ou como propriedade nova
        [DataMember]
        public int IdLocation
        {
            get { return IdCompany; }
            set { IdCompany = value; }
        }

        #endregion


       

        [DataMember]
        public string Stat_ThemesList { get; set; }
        [DataMember]
        public string Responsible { get; set; }

        [NotMapped]
        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }


        [NotMapped]
        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }


        [NotMapped]
        [DataMember]
        public Int32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [NotMapped]
        [DataMember]
        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }


        [NotMapped]
        [DataMember]
        public Int32 IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }


        [NotMapped]
        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        [NotMapped]
        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        [NotMapped]
        [DataMember]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
            set
            {

            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdLookupValue
        {
            get { return idLookupValue; }
            set
            {
                idLookupValue = value;
                OnPropertyChanged("IdLookupValue");
            }
        }

        [NotMapped]
        [DataMember]
        public string BusinessUnit
        {
            get { return businessUnit; }
            set
            {
                businessUnit = value;
                OnPropertyChanged("BusinessUnit");
            }
        }


        [NotMapped]
        [DataMember]
        public int TotalActionItems
        {
            get { return totalActionItems; }
            set
            {
                totalActionItems = value;
                OnPropertyChanged("TotalActionItems");
            }
        }

        [NotMapped]
        [DataMember]
        public int TotalOpenItems
        {
            get { return totalOpenItems; }
            set
            {
                totalOpenItems = value;
                OnPropertyChanged("TotalOpenItems");
            }
        }


        [NotMapped]
        [DataMember]
        public int TotalClosedItems
        {
            get { return totalClosedItems; }
            set
            {
                totalClosedItems = value;
                OnPropertyChanged("TotalClosedItems");
            }
        }

        [NotMapped]
        [DataMember]
        public int Percentage
        {
            get { return percentage; }
            set
            {
                percentage = value;
                OnPropertyChanged("Percentage");
            }
        }
        [NotMapped]
        [DataMember]
        public string TotalClosedColor
        {
            get { return totalClosedColor; }
            set
            {
                totalClosedColor = value;
                OnPropertyChanged("TotalClosedColor");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdLookupOrigin
        {
            get { return idLookupOrigin; }
            set
            {
                idLookupOrigin = value;
                OnPropertyChanged("IdLookupOrigin");
            }
        }

        [NotMapped]
        [DataMember]
        public string Origin
        {
            get { return origin; }
            set
            {
                origin = value;
                OnPropertyChanged("Origin");
            }
        }
        [NotMapped]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }


        [NotMapped]
        [DataMember]
        public List<APMActionPlanTask> TaskList
        {
            get { return taskList; }
            set
            {
                taskList = value;
                OnPropertyChanged("TaskList");
            }
        }


        [NotMapped]
        [DataMember]
        public Int32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }
        //[Sudhir.jangra][GEOS2-6397]
        [NotMapped]
        [DataMember]
        public Country Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        //[Sudhir.jangra][GEOS2-6397]
        [NotMapped]
        [DataMember]
        public string EmployeeCode
        {
            get { return employeeCode; }
            set
            {
                employeeCode = value;
                OnPropertyChanged("EmployeeCode");
            }
        }

        //[Sudhir.jangra][GEOS2-6397]
        [NotMapped]
        [DataMember]
        public int IdGender
        {
            get { return idGender; }
            set
            {
                idGender = value;
                OnPropertyChanged("IdGender");
            }
        }
        //[Shweta.Thube][GEOS2-6020]
        [NotMapped]
        [DataMember]
        public List<LogEntriesByActionPlan> ActionPlanLogEntries
        {
            get
            {
                return actionPlanLogEntries;
            }

            set
            {
                actionPlanLogEntries = value;
                OnPropertyChanged("ActionPlanLogEntries");
            }
        }
        //[nsatpute][25-10-2024][GEOS2-6018]
        [NotMapped]
        [DataMember]
        //[nsatpute][24-10-2024][GEOS2-6018]
        public List<ActionPlanComment> CommentList
        {
            get { return commentList; }
            set
            {
                commentList = value; OnPropertyChanged("CommentList");
            }
        }

        //[Sudhir.jangra][GEOS2-6595]
        [NotMapped]
        [DataMember]
        public string CreatedByName
        {
            get { return createdByName; }
            set
            {
                createdByName = value;
                OnPropertyChanged("CreatedByName");
            }
        }
        //[Shweta.Thube][GEOS2-6586]
        [NotMapped]
        [DataMember]
        public Int32 IdLookupBusinessUnit
        {
            get { return idLookupBusinessUnit; }
            set
            {
                idLookupBusinessUnit = value;
                OnPropertyChanged("IdLookupBusinessUnit");
            }
        }

        //[Sudhir.Jangra][GEOS2-6596]
        [NotMapped]
        [DataMember]
        public Int32 IdDepartment
        {
            get { return idDepartment; }
            set
            {
                idDepartment = value;
                OnPropertyChanged("IdDepartment");
            }
        }

        //[Sudhir.Jangra][GEOS2-6596]
        [NotMapped]
        [DataMember]
        public string Department
        {
            get { return department; }
            set
            {
                department = value;
                OnPropertyChanged("Department");
            }
        }
        //[Shweta.Thube][GEOS2-6587]

        [NotMapped]
        [DataMember]
        public string BusinessUnitHTMLColor
        {
            get { return businessUnitHTMLColor; }
            set
            {
                businessUnitHTMLColor = value;
                OnPropertyChanged("BusinessUnitHTMLColor");
            }
        }

        //[Shweta.Thube][GEOS2-6794]
        [NotMapped]
        [DataMember]
        public string OriginDescription
        {
            get { return originDescription; }
            set
            {
                originDescription = value;
                OnPropertyChanged("OriginDescription");
            }
        }

        //[Shweta.Thube][GEOS2-6795]
        [NotMapped]
        [DataMember]
        public bool IsActionPlanDeleted
        {
            get { return isActionPlanDeleted; }
            set
            {
                isActionPlanDeleted = value;
                OnPropertyChanged("IsActionPlanDeleted");
            }
        }

        //[Sudhir.Jangra][GEOS2-6882]
        [NotMapped]
        [DataMember]
        public Int32 ResponsibleIdUser
        {
            get { return responsibleIdUser; }
            set
            {
                responsibleIdUser = value;
                OnPropertyChanged("ResponsibleIdUser");
            }
        }

        //[Sudhir.Jangra][GEOS2-6897]
        [NotMapped]
        [DataMember]
        public string ActionPlanResponsibleDisplayName
        {
            get { return actionPlanResponsibleDisplayName; }
            set
            {
                actionPlanResponsibleDisplayName = value;
                OnPropertyChanged("ActionPlanResponsibleDisplayName");
            }
        }

        //[Shweta.Thube][GEOS2-6911]
        [NotMapped]
        [DataMember]
        public Int32 IdCustomer
        {
            get { return idCustomer; }
            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }
        //[Shweta.Thube][GEOS2-6911]
        [NotMapped]
        [DataMember]
        public string Site
        {
            get { return site; }
            set
            {
                site = value;
                OnPropertyChanged("Site");
            }
        }
        //[Shweta.Thube][GEOS2-6911]
        [NotMapped]
        [DataMember]
        public Int32 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }
        //[Shweta.Thube][GEOS2-6911]
        [NotMapped]
        [DataMember]
        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }

        //[Shweta.Thube][GEOS2-6911]
        [NotMapped]
        [DataMember]
        public string GroupName
        {
            get { return groupName; }
            set
            {
                groupName = value;
                OnPropertyChanged("GroupName");
            }
        }

        //[Shweta.Thube][GEOS2-6911]
        [NotMapped]
        [DataMember]
        public Int32 IdZone
        {
            get { return idZone; }
            set
            {
                idZone = value;
                OnPropertyChanged("IdZone");
            }
        }

        //[Shweta.Thube][GEOS2-6911]
        [NotMapped]
        [DataMember]
        public string Zone
        {
            get { return zone; }
            set
            {
                zone = value;
                OnPropertyChanged("Zone");
            }
        }
        //[shweta.thube][GEOS2-9273][08.09.2025]
        [NotMapped]
        [DataMember]
        public Int32 MaxTaskNumber
        {
            get { return maxTaskNumber; }
            set
            {
                maxTaskNumber = value;
                OnPropertyChanged("MaxTaskNumber");
            }
        }




        #endregion



        #region Constructor
        public APMActionPlan()
        {

        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            APMActionPlan actionDetails = (APMActionPlan)this.MemberwiseClone();
            if (TaskList != null)
            {
                actionDetails.TaskList = this.TaskList.Select(x => (APMActionPlanTask)x.Clone()).ToList();
            }
            return actionDetails;
        }
        #endregion
    }
}
