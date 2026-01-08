using System;
using System.Collections.Generic; // Necessário para HashSet
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq; // Necessário para Any()
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

        public string StatusAggregates { get; set; }

        // ... [Propriedades existentes mantidas iguais] ...

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

        private ObservableCollection<ActionPlanTaskModernDto> _visibleTasks;
        public ObservableCollection<ActionPlanTaskModernDto> VisibleTasks
        {
            get => _visibleTasks;
            set { _visibleTasks = value; OnPropertyChanged(); }
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

        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChanged(); }
        }

        public bool IsLoadingTasks
        {
            get => _isLoadingTasks;
            set { _isLoadingTasks = value; OnPropertyChanged(); }
        }

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

        private int _stat_Overdue15;
        public int Stat_Overdue15
        {
            get => _stat_Overdue15;
            set { _stat_Overdue15 = value; OnPropertyChanged(); }
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

        private int _idLookupBusinessUnit;
        public int IdLookupBusinessUnit
        {
            get => _idLookupBusinessUnit;
            set { _idLookupBusinessUnit = value; OnPropertyChanged(); }
        }

        private int _idLookupOrigin;
        public int IdLookupOrigin
        {
            get => _idLookupOrigin;
            set { _idLookupOrigin = value; OnPropertyChanged(); }
        }

        private string _themeAggregates;
        public string ThemeAggregates
        {
            get => _themeAggregates;
            set { _themeAggregates = value; OnPropertyChanged(); }
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

        // ====================================================================================
        // NOVO CÓDIGO: MÉTODOS DE FILTRAGEM RÁPIDA (PASSO 1)
        // ====================================================================================

        /// <summary>
        /// Verifica se este DTO corresponde aos filtros de dropdown passados como HashSets (O(1)).
        /// </summary>
        public bool MatchesFilters(
            HashSet<int> locationIds,
            HashSet<int> businessUnitIds,
            HashSet<int> originIds,
            HashSet<int> departmentIds,
            HashSet<int> customerIds,    // Baseado em IdSite
            HashSet<string> customerGroups, // Baseado em GroupName
            bool includeBlankCustomers,
            HashSet<string> responsibleNames, // Para match por nome
            HashSet<int> responsibleIds       // Para match por ID
        )
        {
            // 1. Verificações Simples (IDs) - Rápidas
            if (locationIds != null && !locationIds.Contains(this.IdLocation)) return false;
            if (businessUnitIds != null && !businessUnitIds.Contains(this.IdLookupBusinessUnit)) return false;
            if (originIds != null && !originIds.Contains(this.IdLookupOrigin)) return false;
            if (departmentIds != null && !departmentIds.Contains(this.IdDepartment)) return false;

            // 2. Verificação de Customer (Lógica Complexa: ID OU Grupo OU Blank)
            bool hasCustomerFilter = (customerIds != null || customerGroups != null || includeBlankCustomers);
            if (hasCustomerFilter)
            {
                bool match = false;
                // a) Blank
                if (includeBlankCustomers && this.IdSite == 0) match = true;
                // b) ID Site
                else if (customerIds != null && this.IdSite > 0 && customerIds.Contains(this.IdSite)) match = true;
                // c) Group Name
                else if (customerGroups != null && !string.IsNullOrWhiteSpace(this.GroupName) && customerGroups.Contains(this.GroupName)) match = true;

                if (!match) return false;
            }

            // 3. Verificação de Responsável (Deep Check: Plano OU Task OU Subtask)
            bool hasRespFilter = (responsibleNames != null && responsibleNames.Count > 0) ||
                                 (responsibleIds != null && responsibleIds.Count > 0);

            if (hasRespFilter)
            {
                // Checa responsável do Plano (match por ID é preferencial, Nome é fallback)
                if (IsResponsibleMatch(this.Responsible, this.IdEmployee, responsibleNames, responsibleIds)) return true;

                // Se o plano não bateu, verifica se alguma Task ou SubTask bate
                // Isso permite filtrar "Tudo o que tem a ver com o Responsável X"
                if (this.Tasks != null)
                {
                    foreach (var task in this.Tasks)
                    {
                        // Task (Geralmente só tem nome no DTO, ID não disponível facilmente aqui)
                        if (IsResponsibleMatch(task.Responsible, 0, responsibleNames, responsibleIds)) return true;

                        if (task.SubTasks != null)
                        {
                            foreach (var sub in task.SubTasks)
                            {
                                if (IsResponsibleMatch(sub.Responsible, 0, responsibleNames, responsibleIds)) return true;
                            }
                        }
                    }
                }

                // Se percorreu tudo e não encontrou, falha
                return false;
            }

            return true;
        }

        /// <summary>
        /// Helper para verificar se um responsável bate com os filtros (ID ou Nome)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsResponsibleMatch(string name, int id, HashSet<string> names, HashSet<int> ids)
        {
            // Prioridade ID
            if (ids != null && id > 0 && ids.Contains(id)) return true;

            // Fallback Nome
            if (names != null && !string.IsNullOrEmpty(name) && names.Contains(name)) return true;

            return false;
        }

        // ====================================================================================

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}