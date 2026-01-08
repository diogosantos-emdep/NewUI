using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Emdep.Geos.Modules.APM.ViewModels
{
    /// <summary>
    /// DTO otimizado para Tasks - versão moderna com virtualização
    /// </summary>
    public class ActionPlanTaskModernDto : INotifyPropertyChanged
    {
        private long _idTask;
        private long _idActionPlan;
        private int _taskNumber;
        private string _code;
        private string _title;
        private string _description;
        private string _responsible;
        private string _status;
        private string _priority;
        private string _theme;
        private DateTime? _openDate;
        private DateTime? _originalDueDate;
        private DateTime? _dueDate;
        private string _openDateDisplay;
        private string _originalDueDateDisplay;
        private string _dueDateDisplay;
        private int _duration;
        private int _dueDays;
        private DateTime? _lastUpdated;
        private string _lastUpdatedDisplay;
        private int _changeCount;
        private string _delegatedTo;
        private string _otItem;
        private string _originWeek;
        private int _commentsCount;
        private int _fileCount;
        private int _participantCount;
        private double _percentage;
        private int _totalSubTasks;
        private int _completedSubTasks;
        private System.Drawing.Color _statusColor;
        private string _dueDaysColor;
        private bool _isExpanded;
        private bool _isLoadingSubTasks;
        private int _idLookupStatus;
        private ObservableCollection<ActionPlanTaskModernDto> _subTasks;

        public long IdTask
        {
            get => _idTask;
            set { _idTask = value; OnPropertyChanged(); }
        }

        public long IdActionPlan
        {
            get => _idActionPlan;
            set { _idActionPlan = value; OnPropertyChanged(); }
        }
        public int IdLookupStatus
        {
            get => _idLookupStatus;
            set { _idLookupStatus = value; OnPropertyChanged(); }
        }
        private ObservableCollection<SubTaskModernDto> _visibleSubTasks;
        public ObservableCollection<SubTaskModernDto> VisibleSubTasks
        {
            get => _visibleSubTasks;
            set { _visibleSubTasks = value; OnPropertyChanged(); }
        }

        private bool _isPartOfFilterPath;
        public bool IsPartOfFilterPath
        {
            get => _isPartOfFilterPath;
            set { _isPartOfFilterPath = value; OnPropertyChanged(); }
        }
        private long? _idParent; // Nullable porque tarefas principais não têm pai
        public long? IdParent
        {
            get => _idParent;
            set { _idParent = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ActionPlanTaskModernDto> SubTasks
        {
            get => _subTasks;
            set { _subTasks = value; OnPropertyChanged(); }
        }
        public int TaskNumber
        {
            get => _taskNumber;
            set { _taskNumber = value; OnPropertyChanged(); }
        }

        public string Code
        {
            get => _code;
            set { _code = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public string Responsible
        {
            get => _responsible;
            set { _responsible = value; OnPropertyChanged(); }
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

        public string Theme
        {
            get => _theme;
            set { _theme = value; OnPropertyChanged(); }
        }

        public DateTime? OpenDate
        {
            get => _openDate;
            set { _openDate = value; OnPropertyChanged(); }
        }

        public string OpenDateDisplay
        {
            get => _openDateDisplay;
            set { _openDateDisplay = value; OnPropertyChanged(); }
        }

        public DateTime? OriginalDueDate
        {
            get => _originalDueDate;
            set { _originalDueDate = value; OnPropertyChanged(); }
        }

        public string OriginalDueDateDisplay
        {
            get => _originalDueDateDisplay;
            set { _originalDueDateDisplay = value; OnPropertyChanged(); }
        }

        public DateTime? DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(); }
        }

        public string DueDateDisplay
        {
            get => _dueDateDisplay;
            set { _dueDateDisplay = value; OnPropertyChanged(); }
        }

        public int Duration
        {
            get => _duration;
            set { _duration = value; OnPropertyChanged(); }
        }

        public int DueDays
        {
            get => _dueDays;
            set { _dueDays = value; OnPropertyChanged(); }
        }

        public string DueDaysColor
        {
            get => _dueDaysColor;
            set { _dueDaysColor = value; OnPropertyChanged(); }
        }

        public DateTime? LastUpdated
        {
            get => _lastUpdated;
            set { _lastUpdated = value; OnPropertyChanged(); }
        }

        public string LastUpdatedDisplay
        {
            get => _lastUpdatedDisplay;
            set { _lastUpdatedDisplay = value; OnPropertyChanged(); }
        }

        public int ChangeCount
        {
            get => _changeCount;
            set { _changeCount = value; OnPropertyChanged(); }
        }

        public string DelegatedTo
        {
            get => _delegatedTo;
            set { _delegatedTo = value; OnPropertyChanged(); }
        }

        public string OTItem
        {
            get => _otItem;
            set { _otItem = value; OnPropertyChanged(); }
        }

        public string OriginWeek
        {
            get => _originWeek;
            set { _originWeek = value; OnPropertyChanged(); }
        }

        public int CommentsCount
        {
            get => _commentsCount;
            set { _commentsCount = value; OnPropertyChanged(); }
        }

        public int FileCount
        {
            get => _fileCount;
            set { _fileCount = value; OnPropertyChanged(); }
        }

        public int ParticipantCount
        {
            get => _participantCount;
            set { _participantCount = value; OnPropertyChanged(); }
        }

        public double Percentage
        {
            get => _percentage;
            set { _percentage = value; OnPropertyChanged(); }
        }

        public int TotalSubTasks
        {
            get => _totalSubTasks;
            set { _totalSubTasks = value; OnPropertyChanged(); }
        }

        public int CompletedSubTasks
        {
            get => _completedSubTasks;
            set { _completedSubTasks = value; OnPropertyChanged(); }
        }

        public System.Drawing.Color StatusColor
        {
            get => _statusColor;
            set { _statusColor = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Indica se a task está expandida (para mostrar sub-tasks)
        /// </summary>
        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Indica se está carregando sub-tasks (lazy-load)
        /// </summary>
        public bool IsLoadingSubTasks
        {
            get => _isLoadingSubTasks;
            set { _isLoadingSubTasks = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
