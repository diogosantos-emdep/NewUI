using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Emdep.Geos.Modules.APM.ViewModels
{
    /// <summary>
    /// DTO otimizado para Action Plans - versão moderna com virtualização
    /// </summary>
    public class ActionPlanModernDto : INotifyPropertyChanged
    {
        private long _idActionPlan;
        private string _code;
        private string _title;
        private string _responsible;
        private string _employeeCode;
        private int _idGender;
        private int _idEmployee;
        private string _location;
        private int _tasksCount;
        private int _totalActionItems;
        private int _totalOpenItems;
        private int _totalClosedItems;
        private double _percentage;
        private string _dueDateDisplay;
        private string _status;
        private string _priority;
        private string _groupName;
        private System.Drawing.Color _totalClosedColor;
        private bool _isExpanded;
        private bool _isLoadingTasks;
        private ObservableCollection<ActionPlanTaskModernDto> _tasks;
        private long _idCustomer;
        private string _customerName;
        private int _idDepartment;
        private string _department;
        private int _idLocation;
        private string _businessUnit;
        private string _origin;
        private int _idSite;
        private string _site;
        private string _countryIconUrl;
        private int _idLookupStatus;

        public long IdActionPlan
        {
            get => _idActionPlan;
            set { _idActionPlan = value; OnPropertyChanged(); }
        }

        public string Code
        {
            get => _code;
            set { _code = value; OnPropertyChanged(); }
        }
        public int IdLookupStatus
        {
            get => _idLookupStatus;
            set { _idLookupStatus = value; OnPropertyChanged(); }
        }
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string Responsible
        {
            get => _responsible;
            set { _responsible = value; OnPropertyChanged(); }
        }

        public string EmployeeCode
        {
            get => _employeeCode;
            set { _employeeCode = value; OnPropertyChanged(); }
        }

        public int IdGender
        {
            get => _idGender;
            set { _idGender = value; OnPropertyChanged(); }
        }

        public int IdEmployee
        {
            get => _idEmployee;
            set { _idEmployee = value; OnPropertyChanged(); }
        }

        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(); }
        }

        public int TasksCount
        {
            get => _tasksCount;
            set { _tasksCount = value; OnPropertyChanged(); }
        }

        public int TotalActionItems
        {
            get => _totalActionItems;
            set { _totalActionItems = value; OnPropertyChanged(); }
        }

        public int TotalOpenItems
        {
            get => _totalOpenItems;
            set { _totalOpenItems = value; OnPropertyChanged(); }
        }

        public int TotalClosedItems
        {
            get => _totalClosedItems;
            set { _totalClosedItems = value; OnPropertyChanged(); }
        }

        public double Percentage
        {
            get => _percentage;
            set { _percentage = value; OnPropertyChanged(); }
        }

        public string DueDateDisplay
        {
            get => _dueDateDisplay;
            set { _dueDateDisplay = value; OnPropertyChanged(); }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public string Priority
        {
            get => _priority;
            set { _priority = value; OnPropertyChanged(); }
        }

        public string GroupName
        {
            get => _groupName;
            set { _groupName = value; OnPropertyChanged(); }
        }

        public System.Drawing.Color TotalClosedColor
        {
            get => _totalClosedColor;
            set { _totalClosedColor = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Indica se o action plan está expandido (master/detail)
        /// </summary>
        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Indica se está carregando tasks (lazy-load)
        /// </summary>
        public bool IsLoadingTasks
        {
            get => _isLoadingTasks;
            set { _isLoadingTasks = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Coleção de Tasks deste Action Plan (lazy-loaded)
        /// </summary>
        public ObservableCollection<ActionPlanTaskModernDto> Tasks
        {
            get => _tasks;
            set { _tasks = value; OnPropertyChanged(); }
        }
        private string _businessUnitHTMLColor;
        public string BusinessUnitHTMLColor
        {
            get => _businessUnitHTMLColor;
            set { _businessUnitHTMLColor = value; OnPropertyChanged(); }
        }
        public long IdCustomer
        {
            get => _idCustomer;
            set { _idCustomer = value; OnPropertyChanged(); }
        }

        // Adicionar dentro da classe ActionPlanModernDto
        private int _statOverdue15;
        public int Stat_Overdue15
        {
            get => _statOverdue15;
            set { _statOverdue15 = value; OnPropertyChanged(); }
        }

        private int _statHighPriorityOverdue;
        public int Stat_HighPriorityOverdue
        {
            get => _statHighPriorityOverdue;
            set { _statHighPriorityOverdue = value; OnPropertyChanged(); }
        }

        private int _statMaxDueDays;
        public int Stat_MaxDueDays
        {
            get => _statMaxDueDays;
            set { _statMaxDueDays = value; OnPropertyChanged(); }
        }


        public string CustomerName
        {
            get => _customerName;
            set { _customerName = value; OnPropertyChanged(); }
        }

        public int IdDepartment
        {
            get => _idDepartment;
            set { _idDepartment = value; OnPropertyChanged(); }
        }

        public string Department
        {
            get => _department;
            set { _department = value; OnPropertyChanged(); }
        }

        public int IdLocation
        {
            get => _idLocation;
            set { _idLocation = value; OnPropertyChanged(); }
        }

        public string BusinessUnit
        {
            get => _businessUnit;
            set { _businessUnit = value; OnPropertyChanged(); }
        }

        public string Origin
        {
            get => _origin;
            set { _origin = value; OnPropertyChanged(); }
        }

        public int IdSite
        {
            get => _idSite;
            set { _idSite = value; OnPropertyChanged(); }
        }

        public string Site
        {
            get => _site;
            set { _site = value; OnPropertyChanged(); }
        }

        public string CountryIconUrl
        {
            get => _countryIconUrl;
            set { _countryIconUrl = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
