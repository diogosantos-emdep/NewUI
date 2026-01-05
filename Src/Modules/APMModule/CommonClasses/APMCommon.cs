using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.APM.ViewModels;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.APM.CommonClasses
{
    public sealed class APMCommon : Prism.Mvvm.BindableBase
    {
        #region Services
        IAPMService APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private List<object> selectedPeriod;
        private Responsible activeEmployee;
        private static readonly APMCommon instance = new APMCommon();
        private int idUserPermission;
        private List<Company> locationList;
        private List<LookupValue> originList;
        private List<LookupValue> businessUnitList;
        private List<Responsible> responsibleList;
        private List<APMActionPlan> actionPlanList;
        private List<LookupValue> statusList;
        private List<LookupValue> priorityList;
        private List<LookupValue> themeList;
        private List<Responsible> delegatedToList;
        private List<Department> departmentList;//[Sudhir.Jangra][GEOS2-6596]
        private VisibilityPerBUViewModel getVisibilityPerBUViewModelDetails;
        private List<UserSiteGeosServiceProvider> commonAllPlanList;//[Sudhir.Jangra][GEOS2-6912]
        private List<APMActionPlanTask> taskList; //[pallavi.kale][GEOS2-7002][19.06.2025]
        private List<APMActionPlanSubTask> subTaskList; //[pallavi.kale][GEOS2-7002][19.06.2025]
        #endregion

        #region Properties
        public List<object> SelectedPeriod
        {
            get { return selectedPeriod; }
            set
            {
                if (selectedPeriod == null || !selectedPeriod.SequenceEqual(value))
                {
                    selectedPeriod = value;
                    this.OnPropertyChanged("SelectedPeriod");
                }
            }
        }
        public Responsible ActiveEmployee
        {
            get { return activeEmployee; }
            set
            {
                activeEmployee = value;
                this.OnPropertyChanged("ActiveEmployee");
            }
        }

        public int IdUserPermission
        {
            get { return idUserPermission; }
            set
            {
                idUserPermission = value;
                this.OnPropertyChanged("IdUserPermission");
            }
        }
        public List<Company> LocationList
        {
            get { return locationList; }
            set
            {
                locationList = value;
                this.OnPropertyChanged("LocationList");
            }
        }

        public List<LookupValue> OriginList
        {
            get { return originList; }
            set
            {
                originList = value;
                this.OnPropertyChanged("OriginList");
            }
        }

        public List<LookupValue> BusinessUnitList
        {
            get { return businessUnitList; }
            set
            {
                businessUnitList = value;
                this.OnPropertyChanged("BusinessUnitList");
            }
        }

        public List<Responsible> ResponsibleList
        {
            get { return responsibleList; }
            set
            {
                responsibleList = value;
                this.OnPropertyChanged("ResponsibleList");
            }
        }

        public List<APMActionPlan> ActionPlanList
        {
            get { return actionPlanList; }
            set
            {
                actionPlanList = value;
                this.OnPropertyChanged("ActionPlanList");
            }
        }

        public List<LookupValue> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                this.OnPropertyChanged("StatusList");
            }
        }

        public List<LookupValue> PriorityList
        {
            get { return priorityList; }
            set
            {
                priorityList = value;
                this.OnPropertyChanged("PriorityList");
            }
        }

        public List<LookupValue> ThemeList
        {
            get { return themeList; }
            set
            {
                themeList = value;
                this.OnPropertyChanged("ThemeList");
            }
        }

        public List<Responsible> DelegatedToList
        {
            get { return delegatedToList; }
            set
            {
                delegatedToList = value;
                this.OnPropertyChanged("DelegatedToList");
            }
        }

        //[Sudhir.Jangra][GEOS2-6596]
        public List<Department> DepartmentList
        {
            get { return departmentList; }
            set
            {
                departmentList = value;
                this.OnPropertyChanged("DepartmentList");
            }
        }

        public static APMCommon Instance
        {
            get { return instance; }
        }
        public VisibilityPerBUViewModel GetVisibilityPerBUViewModelDetails
        {
            get
            {
                return getVisibilityPerBUViewModelDetails;
            }

            set
            {
                getVisibilityPerBUViewModelDetails = value;
            }
        }


        //[Sudhir.Jangra][GEOS2-6912]
        public List<UserSiteGeosServiceProvider> CommonAllPlantList
        {
            get { return commonAllPlanList; }
            set
            {
                commonAllPlanList = value;
                OnPropertyChanged("CommonAllPlantList");
            }
        } 
        //[pallavi.kale][GEOS2-7002][19.06.2025]
        public List<APMActionPlanTask> TaskList
        {
            get { return taskList; }
            set
            {
                taskList = value;
                OnPropertyChanged("TaskList");
            }
        }
        //[pallavi.kale][GEOS2-7002][19.06.2025]
        public List<APMActionPlanSubTask> SubTaskList
        {
            get { return subTaskList; }
            set
            {
                subTaskList = value;
                OnPropertyChanged("SubTaskList");
            }
        }
       
        #endregion

        #region Constructor
        public APMCommon()
        {

        }
        #endregion

        #region Methods

        #endregion


    }
}
