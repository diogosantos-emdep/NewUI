using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Emdep.Geos.Modules.APM.ViewModels
{
    /// <summary>
    /// DTO otimizado para Sub-Tasks - versão moderna com virtualização
    /// </summary>
    public class SubTaskModernDto : INotifyPropertyChanged
    {
        private long _idSubTask;
        private long _idTask;
        private long _idActionPlan;
        private string _code;
        private string _title;
        private string _description;
        private string _responsible;
        private string _status;
        private string _priority;
        private DateTime? _dueDate;
        private string _dueDateDisplay;
        private double _percentage;
        private System.Drawing.Color _statusColor;

        public long IdSubTask
        {
            get => _idSubTask;
            set { _idSubTask = value; OnPropertyChanged(); }
        }

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

        public double Percentage
        {
            get => _percentage;
            set { _percentage = value; OnPropertyChanged(); }
        }

        public System.Drawing.Color StatusColor
        {
            get => _statusColor;
            set { _statusColor = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
