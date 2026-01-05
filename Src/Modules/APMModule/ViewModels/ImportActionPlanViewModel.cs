using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using DevExpress.Diagram.Core.Themes;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Helpers;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.APM.CommonClasses;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using DateTimeConverter = CsvHelper.TypeConversion.DateTimeConverter;
using TypeConverterAttribute = CsvHelper.Configuration.Attributes.TypeConverterAttribute;

namespace Emdep.Geos.Modules.APM.ViewModels
{
    public class ImportActionPlanViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //[GEOS2-6021][rdixit][17.02.2025]
        #region Services
        IAPMService APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IAPMService APMService = new APMServiceController("localhost:6699");
        #endregion

        #region Public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region Declarations
        private string windowHeader;
        private bool isNew;
        private string fileName;
        private string uniqueFileName;
        private AttachmentsByActionPlan actionPlanAttachmentFile;
        private string fileNameString;
        private string description;
        private ObservableCollection<AttachmentsByActionPlan> attachmentObjectList;
        private bool isSave;
        private Attachment attachment;
        private APMActionPlan actionPlan = new APMActionPlan();
        private ObservableCollection<Attachment> attachmentFiles;
        private List<LookupValue> tempBusinessUnitList;//[Pallavi.Kale][GEOS2-8216]
        private List<LookupValue> tempOriginList;//[Pallavi.Kale][GEOS2-8216]
        private List<LookupValue> tempPriorityList;//[Pallavi.Kale][GEOS2 - 8216]
        private List<LookupValue> tempThemeList;//[Pallavi.Kale][GEOS2 - 8216]
        private List<Company> tempLocationList;//[Pallavi.Kale][GEOS2 - 8216]
        private List<Department> tempDepartmentList;//[Pallavi.Kale][GEOS2 - 8216]
        private ObservableCollection<Responsible> tempListOfResponsible;//[Pallavi.Kale][GEOS2 - 8216]
        private ObservableCollection<APMCustomer> tempListOfCustomer;//[Pallavi.Kale][GEOS2 - 8216]
        private APMActionPlan actionPlanDetail;//[shweta.thube][GEOS2-9273][08.09.2025]
        private bool isUpdated;//[shweta.thube][GEOS2-9273][08.09.2025]
        private string originDescription;//[shweta.thube][GEOS2-9273][08.09.2025]
        private DateTime? lastUpdatedDate; //[pallavi.kale][GEOS2-8993][17.10.2025]
        #endregion

        #region Properties
        public ObservableCollection<Attachment> AttachmentFiles
        {
            get { return attachmentFiles; }
            set
            {
                attachmentFiles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentFiles"));
            }
        }
        public APMActionPlan ActionPlan
        {
            get { return actionPlan; }
            set
            {
                actionPlan = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlan"));
            }
        }
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        public Attachment Attachment
        {
            get { return attachment; }
            set
            {
                attachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Attachment"));
            }
        }
        //[Pallavi.Kale][GEOS2 - 8216]
        public List<LookupValue> TempBusinessUnitList

        {
            get { return tempBusinessUnitList; }
            set
            {
                tempBusinessUnitList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempBusinessUnitList"));
            }
        }
        //[Pallavi.Kale][GEOS2 - 8216]
        public List<LookupValue> TempOriginList

        {
            get { return tempOriginList; }
            set
            {
                tempOriginList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempOriginList"));
            }
        }
        //[Pallavi.Kale][GEOS2 - 8216]
        public List<LookupValue> TempThemeList

        {
            get { return tempThemeList; }
            set
            {
                tempThemeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempThemeList"));
            }
        }
        //[Pallavi.Kale][GEOS2 - 8216]
        public List<LookupValue> TempPriorityList

        {
            get { return tempPriorityList; }
            set
            {
                tempPriorityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempPriorityList"));
            }
        }
        //[Pallavi.Kale][GEOS2 - 8216]
        public List<Company> TempLocationList
        {
            get { return tempLocationList; }
            set
            {
                tempLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempLocationList"));
            }
        }
        //[Pallavi.Kale][GEOS2 - 8216]
        public List<Department> TempDepartmentList
        {
            get { return tempDepartmentList; }
            set
            {
                tempDepartmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempDepartmentList"));
            }
        }
        //[Pallavi.Kale][GEOS2 - 8216]
        public ObservableCollection<APMCustomer> TempListOfCustomer
        {
            get { return tempListOfCustomer; }
            set
            {
                tempListOfCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempListOfCustomer"));
            }
        }
        //[Pallavi.Kale][GEOS2 - 8216]
        public ObservableCollection<Responsible> TempListOfResponsible
        {
            get { return tempListOfResponsible; }
            set
            {
                tempListOfResponsible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempListOfResponsible"));
            }
        }
        public int IdOriginTemp { get; set; } //[Pallavi.Kale][GEOS2 - 8216]
        public int IdBusinessUnitTemp { get; set; } //[Pallavi.Kale][GEOS2 - 8216]
        public int IdLocationTemp { get; set; } //[Pallavi.Kale][GEOS2 - 8216]
        public int IdDepartmentTemp { get; set; } //[Pallavi.Kale][GEOS2 - 8216]
        public int IdResponsiblePersonTemp { get; set; } //[Pallavi.Kale][GEOS2 - 8216]
        public int IdSiteTemp { get; set; } //[shweta.thube][GEOS2-9273][08.09.2025]
        //[shweta.thube][GEOS2-9273][08.09.2025]
        public APMActionPlan ActionPlanDetail
        {
            get { return actionPlanDetail; }
            set
            {
                actionPlanDetail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanDetail"));
            }
        }
        public int IdActionPlanTemp { get; set; }//[shweta.thube][GEOS2-9273][08.09.2025]
        //[shweta.thube][GEOS2-9273][08.09.2025]
        public bool IsUpdated
        {
            get { return isUpdated; }
            set
            {
                isUpdated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUpdated"));
            }
        }
        //[shweta.thube][GEOS2-9273][08.09.2025]
        public string OriginDescription
        {
            get { return originDescription; }
            set
            {
                originDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginDescription"));
            }
        }
        //[pallavi.kale][GEOS2-8993][17.10.2025]
        public DateTime? LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set
            {
                lastUpdatedDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastUpdatedDate"));
            }
        }
        #endregion

        #region ICommands
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand ChooseFileActionCommand { get; set; }
        public ICommand DownloadExcelCommand { get; set; }
        #endregion

        #region Constructor
        public ImportActionPlanViewModel()
        {

            AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
            CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
            ChooseFileActionCommand = new DelegateCommand<object>(ChooseFileActionCommandAction);
            DownloadExcelCommand = new DelegateCommand<object>(DownloadExcelAction);
            WindowHeader = System.Windows.Application.Current.FindResource("APMActionImportHeader").ToString();
            AttachmentFiles = new ObservableCollection<Attachment>();
            TempBusinessUnitList = APMCommon.Instance.BusinessUnitList;//[Pallavi.Kale][GEOS2 - 8216]
            TempPriorityList = APMCommon.Instance.PriorityList;//[Pallavi.Kale][GEOS2 - 8216]
            TempThemeList = APMCommon.Instance.ThemeList;//[Pallavi.Kale][GEOS2 - 8216]
            TempOriginList = APMCommon.Instance.OriginList;//[Pallavi.Kale][GEOS2 - 8216]
            TempLocationList = APMCommon.Instance.LocationList;//[Pallavi.Kale][GEOS2 - 8216]
            TempDepartmentList = APMCommon.Instance.DepartmentList;//[Pallavi.Kale][GEOS2 - 8216]
            LoadLastUpdatedDate();//[pallavi.kale][GEOS2-8993][17.10.2025]
        }
        #endregion

        #region Methods

        public void ChooseFileActionCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ChooseFileActionCommandAction() started...", category: Category.Info, priority: Priority.Low);

            try
            {
                AttachmentFiles.Clear();
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
                {
                    DefaultExt = ".csv",
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
                };

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    string filePath = dlg.FileName;
                    using (var reader = new StreamReader(filePath))
                    {
                        string[] actionPlanHeaders = reader.ReadLine()?.Split(',') ?? Array.Empty<string>();
                        string[] actionPlanValues = reader.ReadLine()?.Split(',') ?? Array.Empty<string>();
                        //[rdixit][GEOS2-7883][15.04.2025]
                        //if (actionPlanHeaders.Length == 0 || actionPlanValues.Length == 0 ||  actionPlanHeaders.Select((header, index) => new { header = header.Trim(), 
                        //    value = actionPlanValues[index].Trim() }).Any(x => string.IsNullOrEmpty(x.value) && x.header != "action_plans.IdCustomer"))
                        //{
                        //    CustomMessageBox.Show(
                        //        string.Format(Application.Current.Resources["APMActionImportEmptyCellError"].ToString()),
                        //        "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                        //    return;
                        //}
                        //[rdixit][GEOS2-7883][15.04.2025]
                        // Only check if headers and values exist (i.e., file isn't malformed)
                        if (actionPlanHeaders.Length == 0 || actionPlanValues.Length == 0)
                        {
                            CustomMessageBox.Show(
                                string.Format(Application.Current.Resources["APMActionImportEmptyCellError"].ToString()),
                                "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                            return;
                        }


                        var actionPlan = new ImportActionPlan();

                        for (int i = 0; i < actionPlanHeaders.Length; i++)
                        {
                            string header = actionPlanHeaders[i].Trim();
                            string value = actionPlanValues[i].Trim();
                            //[shweta.thube][GEOS2-9273][08.09.2025]
                            switch (header)
                            {   
                                case "action_plans.Id":
                                    if (string.IsNullOrEmpty(value))
                                    {
                                        actionPlan.IdActionPlan = 0;
                                        IdActionPlanTemp = 0;
                                        ActionPlanDetail = new APMActionPlan();
                                    }
                                    else if (int.TryParse(value, out int idActionPlan))
                                    {
                                        actionPlan.IdActionPlan = idActionPlan;
                                        IdActionPlanTemp = actionPlan.IdActionPlan;
                                        //APMService = new APMServiceController("localhost:6699");
                                        //[Pallavi.Kale][GEOS2 - 8216]
                                        //Service GetResponsibleListAsPerLocation_V2600 changed to GetResponsibleListAsPerLocation_V2670 [rdixit][GEOS2-9354][01.09.2025]
                                        ActionPlanDetail = APMService.GetActionPlanInfoByIdActionPlan_V2670(IdActionPlanTemp);      //[shweta.thube][GEOS2-9273][08.09.2025]                           
                                    }
                                    break;

                                case "action_plans.Description":
                                    actionPlan.Description = value;
                                    break;

                                case "action_plans.IdLocation":
                                    if (string.IsNullOrEmpty(value))
                                    {
                                        actionPlan.IdLocation = 0;
                                        IdLocationTemp = 0;
                                        TempListOfResponsible = new ObservableCollection<Responsible>();
                                    }
                                    else if (int.TryParse(value, out int idLocation))
                                    {
                                        actionPlan.IdLocation = idLocation;
                                        IdLocationTemp = actionPlan.IdLocation;
                                        //APMService = new APMServiceController("localhost:6699");
                                        //[Pallavi.Kale][GEOS2 - 8216]
                                        //Service GetResponsibleListAsPerLocation_V2600 changed to GetResponsibleListAsPerLocation_V2670 [rdixit][GEOS2-9354][01.09.2025]
                                        var temp = APMService.GetResponsibleListAsPerLocation_V2670(IdLocationTemp.ToString());

                                        TempListOfResponsible = new ObservableCollection<Responsible>(
                                            temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                                    }
                                    break;

                                case "action_plans.IdOrigin":
                                    if (int.TryParse(value, out int idOrigin))
                                        actionPlan.IdOrigin = idOrigin;
                                    IdOriginTemp = actionPlan.IdOrigin;//[Pallavi.Kale][GEOS2 - 8216]

                                    break;
                                case "action_plans.IdDepartment":
                                    if (int.TryParse(value, out int idDepartment))
                                        actionPlan.IdDepartment = idDepartment;
                                    IdDepartmentTemp = actionPlan.IdDepartment;//[Pallavi.Kale][GEOS2 - 8216]
                                    break;
                                case "action_plans.IdBusinessUnit":
                                    if (int.TryParse(value, out int idBusinessUnit))
                                        actionPlan.IdBusinessUnit = idBusinessUnit;
                                    IdBusinessUnitTemp = actionPlan.IdBusinessUnit;//[Pallavi.Kale][GEOS2 - 8216]
                                    break;
                                case "action_plans.IdResponsibleEmployee":
                                    if (int.TryParse(value, out int idResponsibleEmployee))
                                        actionPlan.IdResponsibleEmployee = idResponsibleEmployee;
                                    IdResponsiblePersonTemp = actionPlan.IdResponsibleEmployee;//[Pallavi.Kale][GEOS2 - 8216]

                                    break;
                                case "action_plans.IdCustomer":
                                    if (int.TryParse(value, out int idCustomer))
                                    {
                                        actionPlan.IdSite = idCustomer;
                                        IdSiteTemp = actionPlan.IdSite;//[Pallavi.Kale][GEOS2 - 8216]
                                    }
                                    else
                                        actionPlan.IdSite = 0;
                                    break;
                            }
                        }
                        //[shweta.thube][GEOS2-9273][08.09.2025]
                        
                        #region [rdixit][Commented code as per pedro request]
                        using (var reader1 = new StreamReader(filePath))
                        {
                            reader1.ReadLine();
                            reader1.ReadLine();

                            using (var csv = new CsvReader(reader1, new CsvConfiguration(CultureInfo.InvariantCulture)
                            {
                                MissingFieldFound = null,
                                HeaderValidated = null,
                                PrepareHeaderForMatch = args => args.Header.ToLower(),

                            }))
                            {
                                var taskRecords = csv.GetRecords<ImportActionPlanTask>().ToList();
                                //[shweta.thube][GEOS2-9273][08.09.2025]
                                var taskList = taskRecords.Where(r => string.IsNullOrEmpty(r.task_Parent)).ToList();

                                // SubTasks = records with parent
                                var subTaskList = taskRecords
                                    .Where(r => !string.IsNullOrEmpty(r.task_Parent))
                                    .ToList();

                                //if (taskRecords.Any(task => string.IsNullOrEmpty(task.Title) || !task.DueDate.HasValue))
                                //{
                                //    CustomMessageBox.Show(string.Format(Application.Current.Resources["APMActionImportEmptyCellError"].ToString()),
                                //       "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                //    return;
                                //}
                                actionPlan.ImportActionPlanTaskList = new List<ImportActionPlanTask>(taskList);//[shweta.thube][GEOS2-9273][08.09.2025]
                                actionPlan.ImportActionPlanSubTaskList = new List<ImportActionPlanTask>(subTaskList);//[shweta.thube][GEOS2-9273][08.09.2025]
                                var emptyFields = new HashSet<string>();//[Pallavi.Kale][GEOS2 - 8216]
                                var invalidFields = new HashSet<string>();//[Pallavi.Kale][GEOS2 - 8216]

                                //if (actionPlan.ImportActionPlanTaskList == null || actionPlan.ImportActionPlanTaskList?.Count == 0
                                //    || actionPlan.ImportActionPlanTaskList.Any(i => string.IsNullOrEmpty(i.Title)
                                //    || string.IsNullOrEmpty(i.Description) || string.IsNullOrEmpty(i.IdResponsibleEmployee)
                                //    || string.IsNullOrEmpty(i.IdTheme) || string.IsNullOrEmpty(i.IdPriority)
                                //    || i.DueDate == null || string.IsNullOrEmpty(i.IdResponsibleEmployee)))
                                //{
                                //    CustomMessageBox.Show(string.Format(Application.Current.Resources["APMActionImportEmptyCellError"].ToString()),
                                //        "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                //    return;
                                //}
                                if (ActionPlanDetail.IdActionPlan != IdActionPlanTemp)
                                    invalidFields.Add("Action Plan");
                                //[Pallavi.Kale][GEOS2 - 8216]
                                if (string.IsNullOrEmpty(actionPlan.Description))
                                    emptyFields.Add("Action Plan Name");
                                //[Pallavi.Kale][GEOS2 - 8216]
                                if (IdLocationTemp == 0 || !TempLocationList.Any(loc => loc.IdCompany == IdLocationTemp))
                                    invalidFields.Add("Action Plan Location");
                                //[Pallavi.Kale][GEOS2 - 8216]
                                if (IdOriginTemp == 0 || !TempOriginList.Any(o => o.IdLookupValue == IdOriginTemp))
                                    invalidFields.Add("Action Plan Origin");
                                //[Pallavi.Kale][GEOS2 - 8216]
                                if (IdDepartmentTemp == 0 || !TempDepartmentList.Any(d => d.IdDepartment == IdDepartmentTemp))
                                    invalidFields.Add("Action Plan Department");
                                //[Pallavi.Kale][GEOS2 - 8216]
                                if (IdBusinessUnitTemp == 0 || !TempBusinessUnitList.Any(b => b.IdLookupValue == IdBusinessUnitTemp))
                                    invalidFields.Add("Action Plan BusinessUnit");
                                //[Pallavi.Kale][GEOS2 - 8216]
                                if (IdResponsiblePersonTemp == 0 || !TempListOfResponsible.Any(p => p.IdEmployee == IdResponsiblePersonTemp))
                                    invalidFields.Add("Action Plan Responsible");
                                //[shweta.thube][GEOS2-9273][08.09.2025]
                                if (IdSiteTemp != 0 && !APMService.ValidateCustomerBySites(IdSiteTemp))
                                {
                                    invalidFields.Add("Customer");
                                }

                                //[Pallavi.Kale][GEOS2 - 8216]
                                if (actionPlan.ImportActionPlanTaskList == null || !actionPlan.ImportActionPlanTaskList.Any())
                                {
                                    emptyFields.Add("Task List");
                                }
                                else
                                {
                                    foreach (var task in actionPlan.ImportActionPlanTaskList)
                                    {
                                        if (string.IsNullOrEmpty(task.Title)) emptyFields.Add("Task Title");
                                        if (string.IsNullOrEmpty(task.Description)) emptyFields.Add("Task Description");
                                        if (string.IsNullOrEmpty(task.IdResponsibleEmployee)) emptyFields.Add("Task Responsible");
                                        if (string.IsNullOrEmpty(task.IdTheme)) emptyFields.Add("Task Theme");
                                        if (string.IsNullOrEmpty(task.IdPriority)) emptyFields.Add("Task Priority");
                                        if (!task.DueDate.HasValue)
                                        {
                                            emptyFields.Add("Task DueDate");
                                        }
                                        else if (task.DueDate.Value == DateTime.MinValue)
                                        {
                                            invalidFields.Add("Task DueDate");
                                        }

                                        if (!string.IsNullOrEmpty(task.IdTheme) &&
                                            !TempThemeList.Any(t => Convert.ToString(t.IdLookupValue) == task.IdTheme))
                                            invalidFields.Add("Task Theme");

                                        if (!string.IsNullOrEmpty(task.IdPriority) &&
                                            !TempPriorityList.Any(p => Convert.ToString(p.IdLookupValue) == task.IdPriority))
                                            invalidFields.Add("Task Priority");

                                        if (!string.IsNullOrEmpty(task.IdResponsibleEmployee) &&
                                            !TempListOfResponsible.Any(p => Convert.ToString(p.IdEmployee) == task.IdResponsibleEmployee))
                                            invalidFields.Add("Task Responsible");
                                    }
                                }
                                //[shweta.thube][GEOS2-9273][08.09.2025]
                                if (subTaskList != null && subTaskList.Count > 0)
                                {
                                    // collect all valid parent task numbers from taskList (as strings)
                                    var validParentTaskNumbers = taskList.Select(t => t.Task_Number).ToHashSet();

                                    // also collect all subtask numbers (to avoid parent being another subtask)
                                    var subTaskNumbers = subTaskList.Select(st => st.Task_Number).ToHashSet();

                                    // check if any subtask has invalid/missing parent
                                    bool hasInvalidParent = subTaskList.Any(st =>
                                        string.IsNullOrWhiteSpace(st.task_Parent) ||             // parent is null or empty
                                        !validParentTaskNumbers.Contains(st.task_Parent) ||      // parent not in tasklist
                                        subTaskNumbers.Contains(st.task_Parent));                // parent wrongly points to another subtask

                                    if (hasInvalidParent)
                                    {
                                        invalidFields.Add("Parent Task");
                                    }
                                    foreach (var Subtask in actionPlan.ImportActionPlanSubTaskList)
                                    {
                                        if (string.IsNullOrEmpty(Subtask.Title)) emptyFields.Add("Sub-Task Title");
                                        if (string.IsNullOrEmpty(Subtask.Description)) emptyFields.Add("Sub-Task Description");
                                        if (string.IsNullOrEmpty(Subtask.IdResponsibleEmployee)) emptyFields.Add("Sub-Task Responsible");
                                        if (string.IsNullOrEmpty(Subtask.IdTheme)) emptyFields.Add("Sub-Task Theme");
                                        if (string.IsNullOrEmpty(Subtask.IdPriority)) emptyFields.Add("Sub-Task Priority");
                                        if (!Subtask.DueDate.HasValue)
                                        {
                                            emptyFields.Add("Sub-Task DueDate");
                                        }
                                        else if (Subtask.DueDate.Value == DateTime.MinValue)
                                        {
                                            invalidFields.Add("Sub-Task DueDate");
                                        }

                                        if (!string.IsNullOrEmpty(Subtask.IdTheme) &&
                                            !TempThemeList.Any(t => Convert.ToString(t.IdLookupValue) == Subtask.IdTheme))
                                            invalidFields.Add("Sub-Task Theme");

                                        if (!string.IsNullOrEmpty(Subtask.IdPriority) &&
                                            !TempPriorityList.Any(p => Convert.ToString(p.IdLookupValue) == Subtask.IdPriority))
                                            invalidFields.Add("Sub-Task Priority");

                                        if (!string.IsNullOrEmpty(Subtask.IdResponsibleEmployee) &&
                                            !TempListOfResponsible.Any(p => Convert.ToString(p.IdEmployee) == Subtask.IdResponsibleEmployee))
                                            invalidFields.Add("Sub-Task Responsible");
                                    }
                                }


                                //[shweta.thube][GEOS2-9273][08.09.2025]
                                if (emptyFields.Any() || invalidFields.Any())
                                {
                                    var combinedFields = emptyFields.Union(invalidFields).Distinct().ToList();

                                    string actionPlanError = combinedFields.Contains("Action Plan")
                                        ? Application.Current.Resources["APMActionImportInvalidActionPlan"].ToString()
                                        : null;
                                    if (combinedFields.Contains("Action Plan"))
                                    {
                                        combinedFields.Remove("Action Plan");
                                    }
                                    string combinedMsg = combinedFields.Count == 1
                                        ? string.Format(Application.Current.Resources["NewAPMActionImportEmptyOrInvalidFieldSingle"].ToString(), combinedFields.First())
                                        : string.Format(Application.Current.Resources["NewAPMActionImportEmptyOrInvalidFieldMultiple"].ToString(), string.Join(", ", combinedFields));

                                    string uploadMsg = Application.Current.Resources["APMActionImportUploadValidFile"].ToString();

                                    string finalMsg = string.IsNullOrEmpty(actionPlanError)
                                        ? combinedMsg + Environment.NewLine + uploadMsg
                                        : actionPlanError + Environment.NewLine + combinedMsg + Environment.NewLine + uploadMsg;

                                    CustomMessageBox.Show(finalMsg, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return;

                                }

                                //[shweta.thube][GEOS2-9273][13.09.2025]   

                                if (!TempOriginList.Any(o => o.IdLookupValue == IdOriginTemp))
                                {
                                    OriginDescription = "";
                                }
                                else
                                {
                                    CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                                    Calendar calendar = cultureInfo.Calendar;
                                    CalendarWeekRule weekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
                                    DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
                                    int weekNumber = calendar.GetWeekOfYear(DateTime.Now, weekRule, firstDayOfWeek);
                                    OriginDescription = APMCommon.Instance.OriginList.FirstOrDefault(x => x.IdLookupValue == actionPlan.IdOrigin).Abbreviation + DateTime.Now.Year + "CW" + weekNumber;
                                }

                                //APMService = new APMServiceController("localhost:6699");
                                ActionPlan = new APMActionPlan
                                {
                                    IdActionPlan = actionPlan.IdActionPlan,
                                    Description = actionPlan.Description,
                                    IdCompany = actionPlan.IdLocation,
                                    IdLookupOrigin = actionPlan.IdOrigin,
                                    IdDepartment = actionPlan.IdDepartment,
                                    IdLookupBusinessUnit = actionPlan.IdBusinessUnit,
                                    IdEmployee = actionPlan.IdResponsibleEmployee,
                                    IdCustomer = actionPlan.IdSite,
                                    TaskList = new List<APMActionPlanTask>(),
                                    Code = APMService.GetActionPlanLatestCode_V2560(),
                                    CreatedBy = GeosApplication.Instance.ActiveUser.IdUser,
                                    CreatedIn = DateTime.Now,
                                    OriginDescription = OriginDescription
                                };
                                if (ActionPlan.ActionPlanLogEntries == null)
                                {
                                    ActionPlan.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();
                                }
                                if (ActionPlanDetail.IdActionPlan == 0)
                                {
                                    ActionPlan.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                    {
                                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                        Datetime = DateTime.Now,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("AddChangeLogForActionPlan").ToString(),
                                    ActionPlan.Code)
                                    });
                                }
                                else
                                {

                                    if (ActionPlanDetail.IdActionPlan != 0)
                                    {
                                        ActionPlan.Code = ActionPlanDetail.Code;
                                        if (ActionPlanDetail.Description != ActionPlan.Description)
                                        {
                                            IsUpdated = true;
                                            ActionPlan.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = ActionPlan.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,
                                                Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanNameChangeLog").ToString(), ActionPlanDetail.Description, ActionPlan.Description)
                                            });
                                        }
                                        if (ActionPlanDetail.IdCompany != ActionPlan.IdCompany)
                                        {
                                            IsUpdated = true;
                                            string OldLoaction = APMCommon.Instance.LocationList.FirstOrDefault(i => i.IdCompany == ActionPlanDetail.IdCompany).Alias;
                                            string NewLocation = APMCommon.Instance.LocationList.FirstOrDefault(i => i.IdCompany == ActionPlan.IdCompany).Alias;
                                            ActionPlan.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = ActionPlan.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanLocationChangeLog").ToString(), OldLoaction, NewLocation)
                                            });
                                        }

                                        if (ActionPlanDetail.IdLookupOrigin != ActionPlan.IdLookupOrigin)
                                        {
                                            IsUpdated = true;
                                            string OldOrigin = APMCommon.Instance.OriginList.FirstOrDefault(i => i.IdLookupValue == ActionPlanDetail.IdLookupOrigin).Value;
                                            string NewOrigin = APMCommon.Instance.OriginList.FirstOrDefault(i => i.IdLookupValue == ActionPlan.IdLookupOrigin).Value;
                                            ActionPlan.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = ActionPlan.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanOriginChangeLog").ToString(), OldOrigin, NewOrigin)
                                            });
                                        }
                                        if (ActionPlanDetail.IdDepartment != ActionPlan.IdDepartment)
                                        {
                                            IsUpdated = true;
                                            string OldDepartment = APMCommon.Instance.DepartmentList.FirstOrDefault(i => i.IdDepartment == ActionPlanDetail.IdDepartment).DepartmentName;
                                            string NewDepartment = APMCommon.Instance.DepartmentList.FirstOrDefault(i => i.IdDepartment == ActionPlan.IdDepartment).DepartmentName;
                                            ActionPlan.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = ActionPlan.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanDepartmentChangeLog").ToString(), OldDepartment, NewDepartment)
                                            });
                                        }

                                        if (ActionPlanDetail.IdLookupValue != ActionPlan.IdLookupValue)
                                        {
                                            IsUpdated = true;
                                            string OldBusinessUnit = APMCommon.Instance.BusinessUnitList.FirstOrDefault(i => i.IdLookupValue == ActionPlanDetail.IdLookupValue).Value;
                                            string NewBusinessUnit = APMCommon.Instance.BusinessUnitList.FirstOrDefault(i => i.IdLookupValue == ActionPlan.IdLookupValue).Value;
                                            ActionPlan.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = ActionPlan.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanBusinessUnitChangeLog").ToString(), OldBusinessUnit, NewBusinessUnit)
                                            });
                                        }

                                        if (ActionPlanDetail.IdEmployee != ActionPlan.IdEmployee)
                                        {
                                            IsUpdated = true;
                                            var responsible = TempListOfResponsible.FirstOrDefault(i => i.IdEmployee == ActionPlan.IdEmployee);
                                            string NewResponsible = responsible?.FullName ?? string.Empty;
                                            ActionPlan.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = ActionPlan.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanResponsibleChangeLog").ToString(), ActionPlanDetail.FullName, NewResponsible)
                                            });
                                        }
                                        if (ActionPlanDetail?.IdSite != ActionPlan?.IdSite)
                                        {
                                            string OldCustomer = ActionPlanDetail.GroupName;
                                            string NewCustomer = ActionPlan.GroupName;
                                            ActionPlan.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = ActionPlan.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanCustomerChangeLog").ToString(), string.IsNullOrEmpty(OldCustomer) ? "None" : OldCustomer,
            string.IsNullOrEmpty(NewCustomer) ? "None" : NewCustomer)
                                            });
                                        }
                                        if (IsUpdated)
                                        {
                                            ActionPlan.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                        }
                                    }
                                }

                                ActionPlanDetail.MaxTaskNumber++;
                                foreach (var task in taskList)
                                {
                                    var apt = new APMActionPlanTask
                                    {
                                        IdEmployee = Convert.ToInt32(task.IdResponsibleEmployee),
                                        Description = task.Description,
                                        Title = task.Title,
                                        DueDate = Convert.ToDateTime(task.DueDate),
                                        IdLookupPriority = Convert.ToInt32(task.IdPriority),
                                        IdLookupTheme = Convert.ToInt32(task.IdTheme),
                                        TaskNumber = ActionPlanDetail.MaxTaskNumber++,
                                        IdCompany = actionPlan.IdLocation,
                                        IdLookupStatus = 1979,
                                        CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                        CreatedIn = DateTime.Now,
                                        OriginalDueDate = task.DueDate,
                                        DueDays = 0,
                                        Progress = 0,
                                        IdOTItem = int.TryParse(task.IdOTItem, out var otItemtask) ? otItemtask : 0,
                                        ActionPlanLogEntries = new List<LogEntriesByActionPlan>(),
                                        SubTaskList = new List<APMActionPlanSubTask>()
                                    };

                                    apt.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                    {
                                        IdActionPlan = apt.IdActionPlan,
                                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        Comments = string.Format(
                                            System.Windows.Application.Current.FindResource("ActionPlanAddNewTaskChangeLog").ToString(),
                                            apt.Title,
                                            apt.TaskNumber)
                                    });

                                    // add the task to the ActionPlan
                                    ActionPlan.TaskList.Add(apt);

                                    // 🔎 find only the subtasks that belong to this TaskNumber
                                    var relatedSubTasks = subTaskList
                                        .Where(s => s.task_Parent == task.Task_Number)
                                        .ToList();
                                    int subtaskno = 1;
                                    foreach (var subtask in relatedSubTasks)
                                    {

                                        var aptt = new APMActionPlanSubTask
                                        {
                                            IdEmployee = Convert.ToInt32(subtask.IdResponsibleEmployee),
                                            ParentTaskNumber = Int64.TryParse(subtask.task_Parent, out var parentNo) ? parentNo : 0,
                                            Description = subtask.Description,
                                            Title = subtask.Title,
                                            DueDate = Convert.ToDateTime(subtask.DueDate),
                                            IdLookupPriority = Convert.ToInt32(subtask.IdPriority),
                                            IdLookupTheme = Convert.ToInt32(subtask.IdTheme),
                                            subTaskCode = apt.TaskNumber + "." + subtaskno,
                                            TaskNumber = subtaskno,
                                            IdCompany = actionPlan.IdLocation,
                                            IdLookupStatus = 1979,
                                            CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                            CreatedIn = DateTime.Now,
                                            OriginalDueDate = subtask.DueDate,
                                            DueDays = 0,
                                            Progress = 0,
                                            IdOTItem = int.TryParse(subtask.IdOTItem, out var otItem) ? otItem : 0,
                                            ActionPlanLogEntries = new List<LogEntriesByActionPlan>()
                                        };
                                        subtaskno++;
                                        // optionally add a subtask log entry
                                        aptt.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = apt.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,
                                            Comments = string.Format(
                                                System.Windows.Application.Current.FindResource("ActionPlanAddNewSubTaskChangeLog").ToString(),
                                                aptt.Title,
                                                aptt.subTaskCode)
                                        });

                                        // ✅ attach the subtask only to its parent
                                        apt.SubTaskList.Add(aptt);
                                    }
                                }
                            }
                        }


                        #endregion
                    }

                    FileInfo file = new FileInfo(dlg.FileName);
                    Attachment Attachment = new Attachment();
                    Attachment.FilePath = file.FullName;
                    Attachment.OriginalFileName = file.Name;
                    Attachment.IsDeleted = false;
                    AttachmentFiles.Insert(0, Attachment);
                    AttachmentFiles = new ObservableCollection<Attachment>(AttachmentFiles);
                    GeosApplication.Instance.Logger.Log("File processed successfully.", category: Category.Info, priority: Priority.Low);
                }

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChooseFileActionCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }

            GeosApplication.Instance.Logger.Log("Method ChooseFileActionCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                //[shweta.thube][GEOS2-9273][08.09.2025]
                if (ActionPlanDetail.IdActionPlan != 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["ImportExistingActionPlanDetailsConfirmation"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        //APMService = new APMServiceController("localhost:6699");
                        IsSave = APMService.UpdateImportedActionPlan_V2670(ActionPlan); //[shweta.thube][GEOS2-9273][08.09.2025]
                    }
                }
                else
                {
                    //IsSave = APMService.AddImportedActionPlanDetails_V2630(ActionPlan);  //[rdixit][GEOS2-7883][15.04.2025]
                    //APMService = new APMServiceController("localhost:6699");
                    IsSave = APMService.AddImportedActionPlanDetails_V2670(ActionPlan); //[shweta.thube][GEOS2-9273][08.09.2025]
                }

                if (IsSave)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("APMActionImportSucess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DownloadExcelAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DownloadExcelAction() started...", category: Category.Info, priority: Priority.Low);
                DateTime? lastModified;//[pallavi.kale][GEOS2-8993][17.10.2025]
                //byte[] fileBytes = APMService.GetActionPlansImportFile();
                byte[] fileBytes = APMService.GetActionPlansImportFile_V2680(out lastModified);//[pallavi.kale][GEOS2-8993][17.10.2025]
                if (fileBytes == null)
                {
                    LastUpdatedDate = lastModified;
                    CustomMessageBox.Show(
                        $"{System.Windows.Application.Current.FindResource("APMActionDownloadFileNotFound")}",
                        Application.Current.Resources["PopUpNotifyColor"].ToString(),
                        CustomMessageBox.MessageImagePath.Info,
                        MessageBoxButton.OK);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Save As",
                    Filter = "Excel Macro-Enabled Workbook (*.xlsm)|*.xlsm|All Files (*.*)|*.*",
                    FileName = "ActionPlansImportFile.xlsm",
                    DefaultExt = ".xlsm"
                };

                bool? result = saveFileDialog.ShowDialog();
                if (result == true)
                {
                    string selectedFilePath = saveFileDialog.FileName;

                    try
                    {
                        File.WriteAllBytes(selectedFilePath, fileBytes);
                        CustomMessageBox.Show(
                            $"{System.Windows.Application.Current.FindResource("APMActionDownloadSucess")}",
                            Application.Current.Resources["PopUpSuccessColor"].ToString(),
                            CustomMessageBox.MessageImagePath.Ok,
                            MessageBoxButton.OK);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log($"Error writing file in DownloadExcelAction() - {ex.Message}", category: Category.Exception, priority: Priority.Low);
                    }
                }
                else
                {
                    GeosApplication.Instance.Logger.Log("Download canceled by user.", category: Category.Info, priority: Priority.Low);
                }
                GeosApplication.Instance.Logger.Log("Method DownloadExcelAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in DownloadExcelAction() - {ex.Message}", category: Category.Exception, priority: Priority.Low);
            }
        }
        //[pallavi.kale][GEOS2-8993][17.10.2025]
        private void LoadLastUpdatedDate()
        {
            try
            {
                //APMService = new APMServiceController("localhost:6699");
                DateTime? lastModified;
                APMService.GetActionPlansImportFile_V2680(out lastModified);
                LastUpdatedDate = lastModified;
                GeosApplication.Instance.Logger.Log("Method LoadLastUpdatedDate() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in LoadLastUpdatedDate() -{0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
    public class CustomDateTimeConverter : DateTimeConverter
    {
        private readonly string[] formats = { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd" , "dd-MM-yyyy", "yyyy-MM-dd",
                                    "dd/MM/yyyy", "M/d/yyyy", "dd MMM yyyy", "MMM dd, yyyy", "yyyy-MM-dd HH:mm:ss"};

        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            //[Pallavi.Kale][GEOS2 - 8216]
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            if (DateTime.TryParseExact(text, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            return null; // Handle empty dates safely
        }
    }

    public class ImportActionPlan
    {
        public string Description { get; set; }
        public int IdLocation { get; set; }
        public int IdOrigin { get; set; }
        public int IdDepartment { get; set; }
        public int IdBusinessUnit { get; set; }
        public int IdResponsibleEmployee { get; set; }
        public int IdSite { get; set; }
        public List<ImportActionPlanTask> ImportActionPlanTaskList { get; set; }
        public int IdActionPlan { get; set; } //[shweta.thube][GEOS2-9273][08.09.2025]
        public List<ImportActionPlanTask> ImportActionPlanSubTaskList { get; set; } //[shweta.thube][GEOS2-9273][08.09.2025]
    }

    public class ImportActionPlanTask
    { 
        //[shweta.thube][GEOS2-9273][08.09.2025]
        [Name("action_plan_task.Task_Number")]
        public string Task_Number { get; set; }
        //[shweta.thube][GEOS2-9273][08.09.2025]
        [Name("action_plan_task.task_Parent")]
        public string task_Parent { get; set; }

        [Name("action_plan_task.Title")]
        public string Title { get; set; }

        [Name("action_plan_task.Description")]
        public string Description { get; set; }

        [Name("action_plan_task.IdTheme")]
        public string IdTheme { get; set; }

        [Name("action_plan_task.IdPriority")]
        public string IdPriority { get; set; }

        [Name("action_plan_task.DueDate")]
        [TypeConverter(typeof(CustomDateTimeConverter))]
        public DateTime? DueDate { get; set; }

        [Name("action_plan_task.IdResponsibleEmployee")]
        public string IdResponsibleEmployee { get; set; }
        //[shweta.thube][GEOS2-9273][08.09.2025]
        [Name("action_plan_task.IdOTItem")]
        public string IdOTItem { get; set; }
    }
}
