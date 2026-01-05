using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using Emdep.Geos.Data.Common;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Services.Contracts;
using System.ComponentModel;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using Emdep.Geos.Data.Common.Epc;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.APM.Views;
using DevExpress.Mvvm;
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.Editors.Popups.Calendar;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;
using System.IO;
using System.Windows.Documents;
using DevExpress.Printing.ExportHelpers;
using DevExpress.Export;
using DevExpress.Export.Xl;
using System.Drawing;
using DevExpress.XtraPrinting;
using System.Globalization;
using DevExpress.Data.Filtering;
using System.Windows.Controls;
using System.Windows.Data;
using System.Net;
using System.Runtime.Serialization.Json;
using Emdep.Geos.UI.Helper;
using System.Data;
using DevExpress.Spreadsheet;
using DevExpress.Data.Filtering.Helpers;

using System.Windows.Media.Imaging;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Modules.APM.CommonClasses;
using Emdep.Geos.Utility;
using System.Text.RegularExpressions;
using System.Reflection;
using DevExpress.Xpf.Grid.DragDrop;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common.Hrm;
using DevExpress.DXBinding.Native;
using System.Runtime.InteropServices.Expando;
using DevExpress.Xpf.Core.Native;
using System.Threading;
using Emdep.Geos.Data.Common.PLM;
using System.Diagnostics;


namespace Emdep.Geos.Modules.APM.ViewModels
{
    public partial class ActionPlansViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IAPMService APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IAPMService  APMService = new APMServiceController("localhost:6699");
        #endregion

        #region public Events
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public void Dispose()
        {
        }

        #endregion

        #region Declaration
        private ObservableCollection<APMActionPlan> actionPlanList;
        private List<APMActionPlan> tempActionPlanList;
        bool isBusy;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        private bool isTopDockChecked;
        private bool isTopTaskDockChecked;//[Sudhir.Jangra][GEOS2-6453]
        List<string> APMActionPlanFilter = new List<string>();
        private List<GridColumn> GridColumnList;
        Dictionary<string, string> ActionFilterFilter = new Dictionary<string, string>();
        private string userSettingsKeyForActionPlan = "APM_ActionPlan_Filter_";
        private ObservableCollection<TileBarFilters> topListOfFilterTile;
        private TileBarFilters selectedTopTileBarItem;
        private TileBarFilters previousSelectedTopTileBarItem;
        private bool isEdit;
        private string customFilterStringName;
        private string customFilterHTMLColor;
        private int visibleRowCount;
        private string myFilterString;
        private ObservableCollection<TileBarFilters> listOfFilterTile;
        private ObservableCollection<Responsible> listOfPerson;
        private List<object> selectedPerson;
        private List<object> previousSelectedPerson;
        private List<object> selectedBussinessUnit;
        private List<object> selectedOrigin;
        private string myChildFilterString;
        private List<object> selectedLocation;
        private bool isTopTileBarVisible;
        private bool isTopTaskTileBarVisible;//[Sudhir.Jangra][GEOS2-6453]
        private bool isAddButtonEnabled;
        private TileBarFilters selectedTileBarItem;
        private TileBarFilters previousSelectedTileBarItem;//[Sudhir.Jangra][GEOS2-6789]
        private bool isActionPlanColumnChooserVisible;
        private bool isActionPlanChildColumnChooserVisible;
        public string ActionPlanGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ActionPlanGridSetting.Xml";
        public string ActionPlanChildGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ActionPlanChildGridSetting.Xml";
        public string ActionPlanTaskGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ActionPlanTaskGridSetting.Xml";//[shweta.thube][GEOS2-6453]
        private APMActionPlan selectedActionPlan;
        private APMActionPlanTask selectedActionPlanTask;
        private bool isDeleted;
        // [nsatpute][09-10-2024][GEOS2-5975]
        private ObservableCollection<TaskStatuswise> listStatus;
        private Visibility gridViewVisibility;
        private Visibility taskViewVisibility;
        private List<LookupValue> statusList;
        private string draggedTaskComment;
        TaskStatuswise taskStatusType;
        public bool IsStatusSaved { get; set; }
        private List<object> selectedDepartment;//[Sudhir.Jangra][GEOS2-6596]
        private Visibility taskGridVisibility;//[shweta.thube][GEOS2-6453]
        private ObservableCollection<APMActionPlanTask> taskGridList;//[shweta.thube][GEOS2-6453]
        private List<APMActionPlanTask> clonedTaskGridList;//[shweta.thube][GEOS2-6453]
        private List<APMActionPlanTask> tempTaskGridList;//[shweta.thube][GEOS2-6453]
        private Int64 idActionPlan;//[shweta.thube][GEOS2-6453]
        bool isTaskGridVisibility;//[shweta.thube][GEOS2-6453]
        private bool isActionPlanTaskGridColumnChooserVisible;//[shweta.thube][GEOS2-6453]
        private object _selectedItem;
        private List<APMActionPlan> clonedActionPlanList;//[Sudhir.Jangra][GEOS2-6789]
        private bool isActionPlanEdit;//[Sudhir.Jangra][GEOS2-6789]
        private List<APMActionPlan> actionPlanCodeList;//[shweta.thube][GEOS2-6453]
        private List<object> selectedActionPlansCode;//[shweta.thube][GEOS2-6453]
        private bool isActionPlanAllLocationSelected;//[Sudhir.Jangra][GEOS2-6792]
        private bool isActionPlanAllResponsibleSelected;//[Sudhir.Jangra][GEOS2-6792]
        private bool isActionPlanAllBUSelected;//[Sudhir.Jangra][GEOS2-6792]
        private bool isActionPlanAllOriginSelected;//[Sudhir.Jangra][GEOS2-6792]
        private bool isActionPlanAllDepartmentSelected;//[Sudhir.Jangra][GEOS2-6792]
        private bool isActionPlanAllActionPlanSelected;//[Sudhir.Jangra][GEOS2-6792]
        private ObservableCollection<APMAlertTileBarFilters> alertListOfFilterTile;//[Sudhir.Jangra][GEOS2-5983]
        private APMAlertTileBarFilters selectedAlertTileBarItem;//[Sudhir.Jangra][GEOS2-5983]
        private APMAlertTileBarFilters previousSelectedAlertTileBarItem;//[Sudhir.Jangra][GEOS2-5983]
        private bool isAlertSectionCollapsed;//[Sudhir.Jangra][GEOS2-5983]
        private bool isExpand;//[Sudhir.Jangra][GEOS2-5983]
        private ObservableCollection<TileBarFilters> topTaskListOfFilterTile;//[shweta.thube][GEOS2-6453]
        private TileBarFilters selectedTaskTopTileBarItem;
        private string userSettingsKeyForActionPlanTask = "APM_ActionPlanTask_Filter_";
        private TileBarFilters previousSelectedTaskTopTileBarItem;
        private string myTaskFilterString;
        private string customTaskFilterStringName;
        private string customTaskFilterHTMLColor;
        //Shubham[skadam] GEOS2-6883 HRM Expenses report not working properly.  24 01 2024
        private static GridControl _gridControl; // Reference to the GridControl from the View
        private static TableView _actionPlansTableView;
        bool isActionPlanAllOriginSelectedForTask;
        bool isActionPlanAllBUSelectedForTask;
        bool isActionPlanAllResponsibleSelectedForTask;
        bool isActionPlanAllLocationSelectedForTask;
        bool isActionPlanAllActionPlanSelectedForTask;
        private List<object> previousSelectedPersonForTask;
        private List<object> selectedBussinessUnitForTask;
        private List<object> selectedOriginForTask;
        private List<object> selectedPersonForTask;
        private List<object> selectedLocationForTask;
        private List<object> selectedActionPlansCodeForTask;//[shweta.thube][GEOS2-6453]
        private ObservableCollection<Responsible> listOfPersonForTask;
        private ObservableCollection<APMCustomer> listOfCustomer;//[shweta.thube][GEOS2-6911]
        private List<object> selectedCustomer;//[shweta.thube][GEOS2-6911]
        private bool isActionPlanAllCustomerSelected;//[shweta.thube][GEOS2-6911]
        private APMActionPlanTask previousSelectedActionPlanTask;
        private TileBarFilters temPreviousSelectedTileBarItem;//[shweta.thube][GEOS2-7904]
        private List<object> tempSelectedLocation;//[shweta.thube][GEOS2-7904]
        private List<object> tempSelectedPerson;//[shweta.thube][GEOS2-7904]
        private List<object> tempSelectedBussinessUnit;//[shweta.thube][GEOS2-7904]
        private List<object> tempSelectedOrigin;//[shweta.thube][GEOS2-7904]
        private List<object> tempSelectedActionPlansCodeForTask;//[shweta.thube][GEOS2-7904]
        private APMActionPlanSubTask previousSelectedActionPlanSubTask;//[pallavi.kale][GEOS2-7002][19.06.2025]
        private APMActionPlanSubTask selectedActionPlanSubTask;//[pallavi.kale][GEOS2-7002][19.06.2025]
        private APMActionPlan selectedActionPlanForTask;//[pallavi.kale][GEOS2-7002][19.06.2025]
        private string toolTipSelectedLocation;//[pallavi.kale][GEOS2-7215][21.07.2025]
        private string toolTipSelectedPerson;//[pallavi.kale][GEOS2-7215][21.07.2025]
        private string toolTipSelectedCustomer;//[pallavi.kale][GEOS2-7215][21.07.2025]
        private string toolTipSelectedActionPlansCodeForTask;//[pallavi.kale][GEOS2-7215][21.07.2025]
        private string toolTipSelectedLocationForTask;//[pallavi.kale][GEOS2-7215][21.07.2025]
        private string toolTipSelectedPersonForTask;//[pallavi.kale][GEOS2-7215][21.07.2025]
        private APMAlertTileBarFilters selectedActionPlansAlertTileBarItem;//[shweta.thube][GEOS2-7217][23.07.2025]
        private APMAlertTileBarFilters previousSelectedActionPlansAlertTileBarItem;//[shweta.thube][GEOS2-7217][23.07.2025]
        private ObservableCollection<APMAlertTileBarFilters> alertListOfFilterTileForGridView;//[shweta.thube][GEOS2-7217][23.07.2025]
        private ObservableCollection<APMActionPlanTask> tempTaskGridListForAlert;//[shweta.thube][GEOS2-7217][23.07.2025]
        private Int64 lastUpdatedIdActionPlan;//[pallavi.kale][GEOS2-7213][25.07.2025]
        private Int64 lastUpdatedIdActionPlanTask;//[pallavi.kale][GEOS2-7213][25.07.2025]
        private bool IsExpandTaskLevelOnly = false;//[pallavi.kale][GEOS2-7213][25.07.2025]
        private ObservableCollection<APMCustomer> listOfCustomerForTask;//[pallavi.kale][GEOS2-8084][05.08.2025]
        private List<object> selectedCustomerForTask;//[pallavi.kale][GEOS2-8084][05.08.2025]
        bool isActionPlanAllCustomerSelectedForTask;//[pallavi.kale][GEOS2-8084][05.08.2025]
        private string toolTipSelectedCustomerForTask;//[pallavi.kale][GEOS2-8084][06.08.2025]
        #endregion

        #region  public Properties
        public ObservableCollection<APMActionPlan> ActionPlanList
        {
            get { return actionPlanList; }
            set
            {
                actionPlanList = value;
                try { //APMServiceLogger.LogServiceCall("SetActionPlanList_Property", 0, $"Count={value?.Count ?? 0}");
                     } catch { }
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanList"));

            }
        }
        // [nsatpute][09-10-2024][GEOS2-5975]
        public ObservableCollection<TaskStatuswise> ListStatus
        {
            get { return listStatus; }
            set { listStatus = value; OnPropertyChanged(new PropertyChangedEventArgs("ListStatus")); }
        }

        public List<APMActionPlan> TempActionPlanList
        {
            get { return tempActionPlanList; }
            set
            {
                tempActionPlanList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempActionPlanList"));
            }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public bool IsTopDockChecked
        {
            get { return isTopDockChecked; }
            set
            {
                isTopDockChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTopDockChecked"));
            }
        }

        public bool IsTopTaskDockChecked
        {
            get { return isTopTaskDockChecked; }
            set
            {
                isTopTaskDockChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTopTaskDockChecked"));
            }
        }

        public ObservableCollection<TileBarFilters> TopListOfFilterTile
        {
            get { return topListOfFilterTile; }
            set
            {
                topListOfFilterTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TopListOfFilterTile"));

                if (TopListOfFilterTile.Count > 0 && TopListOfFilterTile != null)
                {
                    IsTopTileBarVisible = true;
                }
                else
                {
                    IsTopTileBarVisible = false;
                }

            }
        }

        public TileBarFilters SelectedTopTileBarItem
        {
            get { return selectedTopTileBarItem; }
            set
            {
                selectedTopTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTopTileBarItem"));
                if (SelectedTopTileBarItem != null)
                {
                    PreviousSelectedTopTileBarItem = SelectedTopTileBarItem;
                }
            }
        }

        public TileBarFilters PreviousSelectedTopTileBarItem
        {
            get { return previousSelectedTopTileBarItem; }
            set
            {
                previousSelectedTopTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedTopTileBarItem"));
            }
        }


        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
            }
        }
        public string CustomFilterStringName
        {
            get { return customFilterStringName; }
            set
            {
                customFilterStringName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomFilterStringName"));
            }
        }

        public int VisibleRowCount
        {
            get { return visibleRowCount; }
            set
            {
                visibleRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleRowCount"));
            }
        }

        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }

        public string CustomFilterHTMLColor
        {
            get { return customFilterHTMLColor; }
            set
            {
                customFilterHTMLColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomFilterHTMLColor"));
            }
        }
        public ObservableCollection<TileBarFilters> ListOfFilterTile
        {
            get { return listOfFilterTile; }
            set
            {
                listOfFilterTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListOfFilterTile"));
            }
        }
        public List<object> SelectedBussinessUnit
        {
            get
            {
                return selectedBussinessUnit;
            }

            set
            {
                selectedBussinessUnit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBussinessUnit"));
            }
        }
        public List<object> SelectedOrigin
        {
            get
            {
                return selectedOrigin;
            }

            set
            {
                selectedOrigin = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOrigin"));
            }
        }

        public ObservableCollection<Responsible> ListOfPerson
        {
            get { return listOfPerson; }
            set
            {
                listOfPerson = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListOfPerson"));
            }
        }

        public List<object> SelectedLocation
        {
            get { return selectedLocation; }
            set
            {
                selectedLocation = value;
                ToolTipSelectedLocation = selectedLocation != null ? string.Join(", ", selectedLocation.Select(x => x?.ToString()).Where(s => !string.IsNullOrEmpty(s))) : string.Empty;//[pallavi.kale][GEOS2-7215][21.07.2025]
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocation"));
            }
        }

        public List<object> SelectedPerson
        {
            get { return selectedPerson; }
            set
            {
                selectedPerson = value;
                ToolTipSelectedPerson = selectedPerson != null ? string.Join(", ", selectedPerson.OfType<Responsible>().Select(x => x?.FullName).Where(s => !string.IsNullOrEmpty(s))) : string.Empty;//[pallavi.kale][GEOS2-7215][21.07.2025]
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPerson"));
                PreviousSelectedPerson = SelectedPerson;
            }
        }

        public List<object> PreviousSelectedPerson
        {
            get { return previousSelectedPerson; }
            set
            {
                previousSelectedPerson = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedPerson"));
            }
        }

        public bool IsTopTileBarVisible
        {
            get { return isTopTileBarVisible; }
            set
            {
                isTopTileBarVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTopTileBarVisible"));
            }
        }
        public bool IsTopTaskTileBarVisible
        {
            get { return isTopTaskTileBarVisible; }
            set
            {
                isTopTaskTileBarVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTopTaskTileBarVisible"));
            }
        }
        //[GEOS2-5948][ shweta.thube]
        public bool IsAddButtonEnabled
        {
            get { return isAddButtonEnabled; }
            set
            {
                isAddButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddButtonEnabled"));
            }
        }

        public string MyChildFilterString

        {

            get { return myChildFilterString; }

            set

            {

                myChildFilterString = value;

                OnPropertyChanged(new PropertyChangedEventArgs("MyChildFilterString"));

            }

        }

        public TileBarFilters SelectedTileBarItem
        {
            get { return selectedTileBarItem; }
            set
            {
                selectedTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileBarItem"));
                if (SelectedTileBarItem != null)
                {
                    PreviousSelectedTileBarItem = SelectedTileBarItem;
                    TemPreviousSelectedTileBarItem = SelectedTileBarItem; //[shweta.thube][GEOS2-7904]
                }
            }
        }

        //[Sudhir.Jangra][GEOS2-6789]
        public TileBarFilters PreviousSelectedTileBarItem
        {
            get { return previousSelectedTileBarItem; }
            set
            {
                previousSelectedTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedTileBarItem"));
            }
        }

        public bool IsActionPlanColumnChooserVisible
        {
            get { return isActionPlanColumnChooserVisible; }
            set
            {
                isActionPlanColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanColumnChooserVisible"));
            }
        }
        public bool IsActionPlanChildColumnChooserVisible
        {
            get { return isActionPlanChildColumnChooserVisible; }
            set
            {
                isActionPlanChildColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanChildColumnChooserVisible"));
            }
        }

        public APMActionPlan SelectedActionPlan
        {
            get { return selectedActionPlan; }
            set
            {
                selectedActionPlan = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActionPlan"));
                //[pallavi.kale][GEOS2-7002][19.06.2025]
                if (SelectedActionPlan != null)
                {
                    SelectedActionPlanForTask = SelectedActionPlan;
                }
            }
        }
        public APMActionPlanTask SelectedActionPlanTask
        {
            get { return selectedActionPlanTask; }
            set
            {
                //PreviousSelectedActionPlanTask = selectedActionPlanTask;

                if (PreviousSelectedActionPlanTask != null)
                    PreviousSelectedActionPlanTask.IsSelectedRow = false;

                selectedActionPlanTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActionPlanTask"));
                //[shweta.thube][GEOS2 - 7219]
                if (selectedActionPlanTask != null && selectedActionPlanTask.IsShowIcon)
                {
                    selectedActionPlanTask.IsSelectedRow = true;
                    PreviousSelectedActionPlanTask = selectedActionPlanTask;
                }
            }
        }
        public APMActionPlanTask PreviousSelectedActionPlanTask
        {
            get { return previousSelectedActionPlanTask; }
            set
            {
                previousSelectedActionPlanTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedActionPlanTask"));

            }
        }
        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }

            set
            {
                isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
            }
        }
        // [nsatpute][09-10-2024][GEOS2-5975]
        public Visibility GridViewVisibility
        {
            get { return gridViewVisibility; }
            set
            {
                gridViewVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridViewVisibility"));
            }
        }

        public Visibility TaskViewVisibility
        {
            get { return taskViewVisibility; }
            set
            {
                taskViewVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskViewVisibility"));
            }
        }
        public APMActionPlanTask CurrentItem { get; private set; } //[shweta.thube][GEOS2-5976]

        public string DraggedTaskComment
        {
            get { return draggedTaskComment; }
            set
            {
                draggedTaskComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DraggedTaskComment"));
            }
        }

        //[Sudhir.Jangra][GEOS2-6596]
        public List<object> SelectedDepartment
        {
            get { return selectedDepartment; }
            set
            {
                selectedDepartment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDepartment"));
            }
        }
        //[shweta.thube][GEOS2-6453]
        public Visibility TaskGridVisibility
        {
            get { return taskGridVisibility; }
            set
            {
                taskGridVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskGridVisibility"));
            }
        }
        //[shweta.thube][GEOS2-6453]
        public ObservableCollection<APMActionPlanTask> TaskGridList
        {
            get { return taskGridList; }
            set
            {
                taskGridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskGridList"));
            }
        }

        public List<APMActionPlanTask> ClonedTaskGridList
        {
            get { return clonedTaskGridList; }
            set
            {
                clonedTaskGridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedTaskGridList"));
            }
        }


        //[shweta.thube][GEOS2-6453]
        public List<APMActionPlanTask> TempTaskGridList
        {
            get { return tempTaskGridList; }
            set
            {
                tempTaskGridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempTaskGridList"));
            }
        }
        //[shweta.thube][GEOS2-6453]
        public Int64 IdActionPlan
        {
            get { return idActionPlan; }
            set
            {
                idActionPlan = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdActionPlan"));
            }
        }
        //[shweta.thube][GEOS2-6453]
        public bool IsTaskGridVisibility
        {
            get { return isTaskGridVisibility; }
            set
            {
                if (isTaskGridVisibility != value)
                {
                    isTaskGridVisibility = value;
                    // Reset filters/state when switching between Action Plan and Task views.
                    HandleTaskGridVisibilityChanged(value);
                    OnPropertyChanged(new PropertyChangedEventArgs("IsTaskGridVisibility"));
                }
            }
        }

        public bool IsActionPlanTaskGridColumnChooserVisible
        {
            get { return isActionPlanTaskGridColumnChooserVisible; }
            set
            {
                isActionPlanTaskGridColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanTaskGridColumnChooserVisible"));
            }
        }

        public virtual object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
            }
        }

        //[Sudhir.Jangra][GEOS2-6789]
        public List<APMActionPlan> ClonedActionPlanList
        {
            get { return clonedActionPlanList; }
            set
            {
                clonedActionPlanList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedActionPlanList"));
            }
        }
        //[Sudhir.Jangra][GEOS2-6789]
        public bool IsActionPlanEdit
        {
            get { return isActionPlanEdit; }
            set
            {
                isActionPlanEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanEdit"));
            }
        }

        //[shweta.thube][GEOS2-6453]
        public List<APMActionPlan> ActionPlanCodeList
        {
            get { return actionPlanCodeList; }
            set
            {
                actionPlanCodeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanCodeList"));
            }
        }
        //[shweta.thube][GEOS2-6453]
        public List<object> SelectedActionPlansCode
        {
            get { return selectedActionPlansCode; }
            set
            {
                selectedActionPlansCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActionPlansCode"));
            }
        }

        //[Sudhir.Jangra][GEOS2-6792]
        public bool IsActionPlanAllLocationSelected
        {
            get { return isActionPlanAllLocationSelected; }
            set
            {
                isActionPlanAllLocationSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanAllLocationSelected"));
            }
        }
        //[Sudhir.Jangra][GEOS2-6792]
        public bool IsActionPlanAllResponsibleSelected
        {
            get { return isActionPlanAllResponsibleSelected; }
            set
            {
                isActionPlanAllResponsibleSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanAllResponsibleSelected"));
            }
        }
        //[Sudhir.Jangra][GEOS2-6792]
        public bool IsActionPlanAllBUSelected
        {
            get { return isActionPlanAllBUSelected; }
            set
            {
                isActionPlanAllBUSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanAllBUSelected"));
            }
        }
        //[Sudhir.Jangra][GEOS2-6792]
        public bool IsActionPlanAllOriginSelected
        {
            get { return isActionPlanAllOriginSelected; }
            set
            {
                isActionPlanAllOriginSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanAllOriginSelected"));
            }
        }
        //[Sudhir.Jangra][GEOS2-6792]
        public bool IsActionPlanAllDepartmentSelected
        {
            get { return isActionPlanAllDepartmentSelected; }
            set
            {
                isActionPlanAllDepartmentSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanAllDepartmentSelected"));
            }
        }

        //[Sudhir.Jangra][GEOS2-6792]
        public bool IsActionPlanAllActionPlanSelected
        {
            get { return isActionPlanAllActionPlanSelected; }
            set
            {
                isActionPlanAllActionPlanSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanAllActionPlanSelected"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5983]
        public ObservableCollection<APMAlertTileBarFilters> AlertListOfFilterTile
        {
            get { return alertListOfFilterTile; }
            set
            {
                alertListOfFilterTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AlertListOfFilterTile"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5983]
        public APMAlertTileBarFilters SelectedAlertTileBarItem
        {
            get { return selectedAlertTileBarItem; }
            set
            {
                selectedAlertTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAlertTileBarItem"));
                if (SelectedAlertTileBarItem != null)
                {
                    PreviousSelectedAlertTileBarItem = SelectedAlertTileBarItem;
                }
            }
        }

        //[Sudhir.Jangra][GEOS2-5983]
        public APMAlertTileBarFilters PreviousSelectedAlertTileBarItem
        {
            get { return previousSelectedAlertTileBarItem; }
            set
            {
                previousSelectedAlertTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedAlertTileBarItem"));
            }
        }


        //[Sudhir.Jangra][GEOS2-5983]

        public bool IsAlertSectionCollapsed
        {
            get { return isAlertSectionCollapsed; }
            set
            {
                isAlertSectionCollapsed = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAlertSectionCollapsed"));
                //[Rahul.Gadhave][https://helpdesk.emdep.com/browse/GEOS2-9774][Date:03/11/2025]
                if (isAlertSectionCollapsed == true)
                {
                    IsExpandGrid = true;
                }
                else
                {
                    IsExpandGrid = false;
                }
            }
        }
        //[Rahul.Gadhave][https://helpdesk.emdep.com/browse/GEOS2-9774][Date:03/11/2025]
        private bool isExpandGrid;
        public bool IsExpandGrid
        {
            get { return isExpandGrid; }
            set
            {
                isExpandGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsExpandGrid"));
            }
        }
        //[Sudhir.Jangra][GEOS2-5983]
        public bool IsExpand
        {
            get { return isExpand; }
            set
            {
                isExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsExpand"));
            }
        }

        public ObservableCollection<TileBarFilters> TopTaskListOfFilterTile
        {
            get { return topTaskListOfFilterTile; }
            set
            {
                topTaskListOfFilterTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TopTaskListOfFilterTile"));

                if (TopTaskListOfFilterTile.Count > 0 && TopTaskListOfFilterTile != null)
                {
                    IsTopTaskTileBarVisible = true;
                }
                else
                {
                    IsTopTaskTileBarVisible = false;
                }

            }
        }

        public TileBarFilters SelectedTaskTopTileBarItem
        {
            get { return selectedTaskTopTileBarItem; }
            set
            {
                selectedTaskTopTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTaskTopTileBarItem"));
                if (SelectedTaskTopTileBarItem != null)
                {
                    PreviousSelectedTaskTopTileBarItem = SelectedTaskTopTileBarItem;
                }

            }
        }

        public TileBarFilters PreviousSelectedTaskTopTileBarItem
        {
            get { return previousSelectedTaskTopTileBarItem; }
            set
            {
                previousSelectedTaskTopTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedTaskTopTileBarItem"));
            }
        }

        public string MyTaskFilterString
        {
            get { return myTaskFilterString; }
            set
            {
                myTaskFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyTaskFilterString"));
            }
        }

        public string CustomTaskFilterStringName
        {
            get { return customTaskFilterStringName; }
            set
            {
                customTaskFilterStringName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomTaskFilterStringName"));
            }
        }

        public string CustomTaskFilterHTMLColor
        {
            get { return customTaskFilterHTMLColor; }
            set
            {
                customTaskFilterHTMLColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomTaskFilterHTMLColor"));
            }
        }

        //[shweta.thube][GEOS2-6453]
        public List<object> SelectedActionPlansCodeForTask
        {
            get { return selectedActionPlansCodeForTask; }
            set
            {
                selectedActionPlansCodeForTask = value;
                ToolTipSelectedActionPlansCodeForTask = selectedActionPlansCodeForTask != null ? string.Join(", ", selectedActionPlansCodeForTask.Select(x => x?.ToString()).Where(s => !string.IsNullOrEmpty(s))) : string.Empty;//[pallavi.kale][GEOS2-7215][21.07.2025]
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActionPlansCodeForTask"));
            }
        }

        public List<object> SelectedLocationForTask
        {
            get { return selectedLocationForTask; }
            set
            {
                selectedLocationForTask = value;
                ToolTipSelectedLocationForTask = selectedLocationForTask != null ? string.Join(", ", selectedLocationForTask.Select(x => x?.ToString()).Where(s => !string.IsNullOrEmpty(s))) : string.Empty;//[pallavi.kale][GEOS2-7215][21.07.2025]
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocationForTask"));
            }
        }

        public List<object> SelectedPersonForTask
        {
            get { return selectedPersonForTask; }
            set
            {
                selectedPersonForTask = value;
                ToolTipSelectedPersonForTask = selectedPersonForTask != null ? string.Join(", ", selectedPersonForTask.OfType<Responsible>().Select(x => x?.FullName).Where(s => !string.IsNullOrEmpty(s))) : string.Empty;//[pallavi.kale][GEOS2-7215][21.07.2025]
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPersonForTask"));
                PreviousSelectedPersonForTask = SelectedPersonForTask;
            }
        }
        public List<object> PreviousSelectedPersonForTask
        {
            get { return previousSelectedPersonForTask; }
            set
            {
                previousSelectedPersonForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedPersonForTask"));
            }
        }

        public List<object> SelectedBussinessUnitForTask
        {
            get
            {
                return selectedBussinessUnitForTask;
            }

            set
            {
                selectedBussinessUnitForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBussinessUnitForTask"));
            }
        }
        public List<object> SelectedOriginForTask
        {
            get
            {
                return selectedOriginForTask;
            }

            set
            {
                selectedOriginForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOriginForTask"));
            }
        }

        public bool IsActionPlanAllActionPlanSelectedForTask
        {
            get { return isActionPlanAllActionPlanSelectedForTask; }
            set
            {
                isActionPlanAllActionPlanSelectedForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanAllActionPlanSelectedForTask"));
            }
        }

        public bool IsActionPlanAllLocationSelectedForTask
        {
            get { return isActionPlanAllLocationSelectedForTask; }
            set
            {
                isActionPlanAllLocationSelectedForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanAllLocationSelectedForTask"));
            }
        }

        public bool IsActionPlanAllResponsibleSelectedForTask
        {
            get { return isActionPlanAllResponsibleSelectedForTask; }
            set
            {
                isActionPlanAllResponsibleSelectedForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanAllResponsibleSelectedForTask"));
            }
        }
        public bool IsActionPlanAllBUSelectedForTask
        {
            get { return isActionPlanAllBUSelectedForTask; }
            set
            {
                isActionPlanAllBUSelectedForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanAllBUSelectedForTask"));
            }
        }

        public bool IsActionPlanAllOriginSelectedForTask
        {
            get { return isActionPlanAllOriginSelectedForTask; }
            set
            {
                isActionPlanAllOriginSelectedForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanAllOriginSelectedForTask"));
            }
        }

        public ObservableCollection<Responsible> ListOfPersonForTask
        {
            get { return listOfPersonForTask; }
            set
            {
                listOfPersonForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListOfPersonForTask"));
            }
        }
        //[shweta.thube][GEOS2-6911]
        public ObservableCollection<APMCustomer> ListOfCustomer
        {
            get { return listOfCustomer; }
            set
            {
                listOfCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListOfCustomer"));
            }
        }
        //[shweta.thube][GEOS2-6911]
        public List<object> SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                ToolTipSelectedCustomer = selectedCustomer != null ? string.Join(", ", selectedCustomer.OfType<APMCustomer>().Select(x => x?.GroupName).Where(s => !string.IsNullOrEmpty(s))) : string.Empty;//[pallavi.kale][GEOS2-7215][21.07.2025]
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomer"));
                //PreviousSelectedPerson = SelectedCustomer;
            }
        }

        //[shweta.thube][GEOS2-6911]
        public bool IsActionPlanAllCustomerSelected
        {
            get { return isActionPlanAllCustomerSelected; }
            set
            {
                isActionPlanAllCustomerSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanAllCustomerSelected"));
            }
        }
        //[shweta.thube][GEOS2-7904]
        public TileBarFilters TemPreviousSelectedTileBarItem
        {
            get { return temPreviousSelectedTileBarItem; }
            set
            {
                temPreviousSelectedTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemPreviousSelectedTileBarItem"));
            }
        }
        //[shweta.thube][GEOS2-7904]
        public List<object> TempSelectedLocation
        {
            get { return tempSelectedLocation; }
            set
            {
                tempSelectedLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempSelectedLocation"));
            }
        }
        //[shweta.thube][GEOS2-7904]
        public List<object> TempSelectedPerson
        {
            get { return tempSelectedPerson; }
            set
            {
                tempSelectedPerson = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempSelectedPerson"));
            }
        }
        //[shweta.thube][GEOS2-7904]
        public List<object> TempSelectedBussinessUnit
        {
            get
            {
                return tempSelectedBussinessUnit;
            }

            set
            {
                tempSelectedBussinessUnit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempSelectedBussinessUnit"));
            }
        }
        //[shweta.thube][GEOS2-7904]
        public List<object> TempSelectedOrigin
        {
            get
            {
                return tempSelectedOrigin;
            }

            set
            {
                tempSelectedOrigin = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempSelectedOrigin"));
            }
        }
        //[shweta.thube][GEOS2-7904]
        public List<object> TempSelectedActionPlansCodeForTask
        {
            get
            {
                return tempSelectedActionPlansCodeForTask;
            }

            set
            {
                tempSelectedActionPlansCodeForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempSelectedActionPlansCodeForTask"));
            }
        }
        //[pallavi.kale][GEOS2-7002][19.06.2025]
        public APMActionPlanSubTask SelectedActionPlanSubTask
        {
            get { return selectedActionPlanSubTask; }
            set
            {
                if (PreviousSelectedActionPlanSubTask != null)
                    PreviousSelectedActionPlanSubTask.IsSelectedRow = false;

                selectedActionPlanSubTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActionPlanSubTask"));

                if (selectedActionPlanSubTask != null && selectedActionPlanSubTask.IsShowIcon)
                {
                    selectedActionPlanSubTask.IsSelectedRow = true;
                    PreviousSelectedActionPlanSubTask = selectedActionPlanSubTask;
                }
            }
        }
        //[pallavi.kale][GEOS2-7002][19.06.2025]
        public APMActionPlanSubTask PreviousSelectedActionPlanSubTask
        {
            get { return previousSelectedActionPlanSubTask; }
            set
            {
                previousSelectedActionPlanSubTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedActionPlanSubTask"));

            }
        }

        //[pallavi.kale][GEOS2-7002][19.06.2025]
        public APMActionPlan SelectedActionPlanForTask
        {
            get { return selectedActionPlanForTask; }
            set
            {
                selectedActionPlanForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActionPlanForTask"));
            }
        }
        //[pallavi.kale][GEOS2-7215][21.07.2025]
        public string ToolTipSelectedLocation
        {
            get { return toolTipSelectedLocation; }
            set
            {
                toolTipSelectedLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToolTipSelectedLocation"));
            }
        }
        //[pallavi.kale][GEOS2-7215][21.07.2025]
        public string ToolTipSelectedCustomer
        {
            get { return toolTipSelectedCustomer; }
            set
            {
                toolTipSelectedCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToolTipSelectedCustomer"));
            }
        }
        //[pallavi.kale][GEOS2-7215][21.07.2025]
        public string ToolTipSelectedPerson
        {
            get { return toolTipSelectedPerson; }
            set
            {
                toolTipSelectedPerson = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToolTipSelectedPerson"));
            }
        }
        //[pallavi.kale][GEOS2-7215][21.07.2025]
        public string ToolTipSelectedActionPlansCodeForTask
        {
            get { return toolTipSelectedActionPlansCodeForTask; }
            set
            {
                toolTipSelectedActionPlansCodeForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToolTipSelectedActionPlansCodeForTask"));
            }
        }
        //[pallavi.kale][GEOS2-7215][21.07.2025]
        public string ToolTipSelectedLocationForTask
        {
            get { return toolTipSelectedLocationForTask; }
            set
            {
                toolTipSelectedLocationForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToolTipSelectedLocationForTask"));
            }
        }
        //[pallavi.kale][GEOS2-7215][21.07.2025]
        public string ToolTipSelectedPersonForTask
        {
            get { return toolTipSelectedPersonForTask; }
            set
            {
                toolTipSelectedPersonForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToolTipSelectedPersonForTask"));
            }
        }
        //[shweta.thube][GEOS2-7217][23.07.2025]
        public APMAlertTileBarFilters SelectedActionPlansAlertTileBarItem
        {
            get { return selectedActionPlansAlertTileBarItem; }
            set
            {
                selectedActionPlansAlertTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActionPlansAlertTileBarItem"));
            }
        }
        //[shweta.thube][GEOS2-7217][23.07.2025]
        public APMAlertTileBarFilters PreviousSelectedActionPlansAlertTileBarItem
        {
            get { return previousSelectedActionPlansAlertTileBarItem; }
            set
            {
                previousSelectedActionPlansAlertTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedActionPlansAlertTileBarItem"));
            }
        }
        //[shweta.thube][GEOS2-7217][23.07.2025]
        public ObservableCollection<APMAlertTileBarFilters> AlertListOfFilterTileForGridView
        {
            get { return alertListOfFilterTileForGridView; }
            set
            {
                alertListOfFilterTileForGridView = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AlertListOfFilterTileForGridView"));
            }
        }
        //[shweta.thube][GEOS2-7217][23.07.2025]
        public ObservableCollection<APMActionPlanTask> TempTaskGridListForAlert
        {
            get { return tempTaskGridListForAlert; }
            set
            {
                tempTaskGridListForAlert = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempTaskGridListForAlert"));
            }
        }
        //[pallavi.kale][GEOS2-7213][25.07.2025]
        public Int64 LastUpdatedIdActionPlan
        {
            get { return lastUpdatedIdActionPlan; }
            set
            {
                lastUpdatedIdActionPlan = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastUpdatedIdActionPlan"));
            }
        }
        //[pallavi.kale][GEOS2-7213][25.07.2025]
        public Int64 LastUpdatedIdActionPlanTask
        {
            get { return lastUpdatedIdActionPlanTask; }
            set
            {
                lastUpdatedIdActionPlanTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastUpdatedIdActionPlanTask"));
            }
        }

        //[pallavi.kale][GEOS2-8084][05.08.2025]
        public ObservableCollection<APMCustomer> ListOfCustomerForTask
        {
            get { return listOfCustomerForTask; }
            set
            {
                listOfCustomerForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListOfCustomerForTask"));
            }
        }

        //[pallavi.kale][GEOS2-8084][05.08.2025]
        public List<object> SelectedCustomerForTask
        {
            get { return selectedCustomerForTask; }
            set
            {
                selectedCustomerForTask = value;
                ToolTipSelectedCustomerForTask = selectedCustomerForTask != null ? string.Join(", ", selectedCustomerForTask.OfType<APMCustomer>().Select(x => x?.GroupName).Where(s => !string.IsNullOrEmpty(s))) : string.Empty;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerForTask"));

            }
        }

        //[pallavi.kale][GEOS2-8084][05.08.2025]
        public bool IsActionPlanAllCustomerSelectedForTask
        {
            get { return isActionPlanAllCustomerSelectedForTask; }
            set
            {
                isActionPlanAllCustomerSelectedForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanAllCustomerSelectedForTask"));
            }
        }

        //[pallavi.kale][GEOS2-8084][05.08.2025]
        public string ToolTipSelectedCustomerForTask
        {
            get { return toolTipSelectedCustomerForTask; }
            set
            {
                toolTipSelectedCustomerForTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToolTipSelectedCustomerForTask"));
            }
        }
        #endregion

        #region Public Commands
        //[GEOS2-6021][rdixit][17.02.2025]
        public ICommand ImportButtonCommand { get; set; }
        public ICommand MouseDoubleClickActionCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }

        public ICommand SelectedYearChangedCommand { get; set; }

        public ICommand FilterEditorCreatedCommand { get; set; }

        public ICommand CommandFilterTileClick { get; set; }

        public ICommand CommandTileBarDoubleClick { get; set; }

        public ICommand LocationListClosedCommand { get; set; }

        public ICommand ResponsibleListClosedCommand { get; set; }

        public ICommand OriginListClosedCommand { get; set; }

        public ICommand LeftCommandFilterTileClick { get; set; }

        public ICommand BusinessUnitListClosedCommand { get; set; }

        public ICommand EditActionPlanHyperLinkCommand { get; set; }

        public ICommand ActionPlanGridControlLoadedCommand { get; set; }

        public ICommand ActionPlanItemListTableViewLoadedCommand { get; set; }

        public ICommand ActionPlanGridControlUnloadedCommand { get; set; }
        public ICommand AddActionPlanTaskCommand { get; set; }

        public ICommand AddActionPlanButtonCommand { get; set; }
        public ICommand EditTaskHyperLinkCommand { get; set; }
        public ICommand DeleteTaskCommand { get; set; }

        // [nsatpute][09-10-2024][GEOS2-5975]
        public ICommand SwitchToGridViewCommand { get; set; }
        public ICommand SwitchToTaskViewCommand { get; set; }

        public ICommand CommentsHyperLinkCommand { get; set; }//[Sudhir.Jangra][GEOS2-6015]

        public ICommand UpdateTaskStatusCommand { get; set; } //[shweta.thube][GEOS2-5976]
        public ICommand OnDroppedCommand { get; set; } //[shweta.thube][GEOS2-5976]

        public ICommand AttachmentsHyperClickCommand { get; set; }//[Sudhir.Jangra][GEOS2-6016]

        public ICommand ActionPlanChildGridControlLoadedCommand { get; set; }//[Sudhir.jangra][GEOS2-6593]

        public ICommand ActionPlanChildItemListTableViewLoadedCommand { get; set; }//[Sudhir.jangra][GEOS2-6593]

        public ICommand ActionPlanChildGridControlUnloadedCommand { get; set; }//[Sudhir.jangra][GEOS2-6593]

        public ICommand SelectedDepartmentListClosedCommand { get; set; }//[Sudhir.Jangra][GEOS2-6596]
        public ICommand SwitchToTaskGridCommand { get; set; } //[shweta.thube][GEOS2-6453]

        public ICommand ActionPlanTaskGridControlLoadedCommand { get; set; } //[shweta.thube][GEOS2-6453]
        public ICommand ActionPlanTaskItemListTableViewLoadedCommand { get; set; } //[shweta.thube][GEOS2-6453]
        public ICommand ActionPlanTaskGridControlUnloadedCommand { get; set; } //[shweta.thube][GEOS2-6453]
        public ICommand SelectedActionPlansCodeClosedCommand { get; set; } //[shweta.thube][GEOS2-6453]

        public ICommand CommandAlertFilterTileClick { get; set; }//[Sudhir.Jangra][GEOS2-5983]

        public ICommand ExpandAndCollapseActionPlanCommand { get; set; }//[Sudhir.Jangra][GEOS2-5983]
        public ICommand DeleteActionPlanCommand { get; set; }//[shweta.thube][GEOS2-6795]
        public ICommand TaskFilterEditorCreatedCommand { get; set; }
        public ICommand CommandTaskFilterTileClick { get; set; }
        public ICommand CommandTaskTileBarDoubleClick { get; set; }

        public ICommand SelectedActionPlansCodeClosedCommandForTask { get; set; }
        public ICommand LocationListClosedCommandForTask { get; set; }
        public ICommand ResponsibleListClosedCommandForTask { get; set; }
        public ICommand BusinessUnitListClosedCommandForTask { get; set; }
        public ICommand OriginListClosedCommandForTask { get; set; }

        //public ICommand MasterRowExpandedCommand { get; set; }
        //Shubham[skadam] GEOS2-6883 HRM Expenses report not working properly.  24 01 2024
        // Commands
        public DelegateCommand<RowEventArgs> MasterRowExpandedCommand { get; }
        public DelegateCommand<DevExpress.Data.SubstituteFilterEventArgs> SubstituteFilterCommand { get; }
        public ICommand LeftCommandFilterTileClickForTask { get; set; }

        public ICommand SelectedCustomerListClosedCommand { get; set; }

        public ICommand ParticipantsHyperLinkClickCommand { get; set; }//[shweta.thube][GEOS2-7008]
        public ICommand AddActionPlanSubTaskCommand { get; set; } //[pallavi.kale][GEOS2-7003][19.06.2025]
        public ICommand EditSubTaskHyperLinkCommand { get; set; } //[pallavi.kale][GEOS2-7002][19.06.2025]
        public ICommand SubTaskCommentsHyperLinkCommand { get; set; }//[shweta.thube][GEOS2-7004][25.06.2025]
        public ICommand SubTaskAttachmentsHyperClickCommand { get; set; }//[shweta.thube][GEOS2-7004][25.06.2025]
        public ICommand SubTaskParticipantsHyperLinkClickCommand { get; set; }//[shweta.thube][GEOS2-7004][25.06.2025]
        public ICommand EditActionPlanTaskHyperLinkCommand { get; set; }
        public ICommand DeleteSubTaskCommand { get; set; }  //[shweta.thube][GEOS2-8683][01.07.2025]
        public ICommand CommandActionPlansAlertFilterTileClick { get; set; }//[shweta.thube][GEOS2-7217][23.07.2025]
        public ICommand CustomerListClosedCommandForTask { get; set; }//[pallavi.kale][GEOS2-8084][05.08.2025
        public ICommand ExportCustomerButtonCommand { get; set; }//[pallavi.kale][GEOS2-8084][05.08.2025]
        public ICommand PrintCustomerButtonCommand { get; set; }//[pallavi.kale][GEOS2-8084][05.08.2025]
        #endregion

        #region Constructor
        public ActionPlansViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ActionsViewModel ...", category: Category.Info, priority: Priority.Low);
                ImportButtonCommand = new DelegateCommand<object>(OpenAttendanceFile);//[GEOS2-6021][rdixit][17.02.2025]
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshButtonCommandAction));//[Sudhir.Jangra][GEOS2-5974]
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintButtonCommandAction));//[Sudhir.Jangra][GEOS2-5974]
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportButtonCommandAction));//[Sudhir.Jangra][GEOS2-5974]
                SelectedYearChangedCommand = new RelayCommand(new Action<object>(SelectedYearChangedCommandAction));//[Sudhir.Jangra][GEOS2-5974]
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);//[Sudhir.Jangra][GEOS2-5972]
                //Obsolete on new cache version
                //CommandFilterTileClick = new RelayCommand(new Action<object>(CommandFilterTileClickAction));//[Sudhir.Jangra][GEOS2-5974]
                CommandTileBarDoubleClick = new RelayCommand(new Action<object>(CommandTileBarDoubleClickAction));//[Sudhir.Jangra][GEOS2-5972]
                LocationListClosedCommand = new RelayCommand(new Action<object>(FilterCommandAction));//[Sudhir.Jangra][GEOS2-5972]
                ResponsibleListClosedCommand = new RelayCommand(new Action<object>(FilterCommandAction));//[Sudhir.Jangra][GEOS2-5972]
                OriginListClosedCommand = new RelayCommand(new Action<object>(FilterCommandAction));//[Sudhir.Jangra][GEOS2-5972]
                LeftCommandFilterTileClick = new RelayCommand(new Action<object>(FilterCommandAction));//[Sudhir.Jangra][GEOS2-5972]
                BusinessUnitListClosedCommand = new RelayCommand(new Action<object>(FilterCommandAction));//[Sudhir.Jangra][GEOS2-5972]
                EditActionPlanHyperLinkCommand = new RelayCommand(new Action<object>(EditActionPlanHyperLinkCommandAction));//[Sudhir.Jangra][GEOS2-5977]
                ActionPlanGridControlLoadedCommand = new RelayCommand(new Action<object>(ActionPlanGridControlLoadedCommandAction));//[Sudhir.Jangra][GEOS2-5972]
                ActionPlanItemListTableViewLoadedCommand = new RelayCommand(new Action<object>(ActionPlanItemListTableViewLoadedCommandAction));//[Sudhir.Jangra][GEOS2-5972]
                ActionPlanGridControlUnloadedCommand = new RelayCommand(new Action<object>(ActionPlanGridControlUnloadedCommandAction));//[Sudhir.Jangra][GEOS2-5972]
                AddActionPlanTaskCommand = new RelayCommand(new Action<object>(AddActionPlanTaskViewWindowShow));//[shweta.thube][GEOS2-5979]
                AddActionPlanButtonCommand = new RelayCommand(new Action<object>(AddActionPlanButtonCommandAction));
                EditTaskHyperLinkCommand = new RelayCommand(new Action<object>(EditTaskHyperLinkCommandAction));//[shweta.thube][GEOS2-5980]
                MouseDoubleClickActionCommand = new RelayCommand(new Action<object>(MouseDoubleClickActionCommandAction));//[Sudhir.Jangra][GEOS2-5978]
                DeleteTaskCommand = new RelayCommand(new Action<object>(DeleteTaskCommandAction));//[shweta.thube][GEOS2-5981] 
                                                                                                  // [nsatpute][09-10-2024][GEOS2-5975]
                SwitchToGridViewCommand = new RelayCommand(new Action<object>(SwitchToGridViewCommandAction));
                SwitchToTaskViewCommand = new RelayCommand(new Action<object>(SwitchToTaskViewCommandAction));
                CommentsHyperLinkCommand = new RelayCommand(new Action<object>(CommentsHyperLinkCommandAction));

                UpdateTaskStatusCommand = new Prism.Commands.DelegateCommand<ListBoxDropEventArgs>(UpdateTaskStatus); //[shweta.thube][GEOS2-5976]
                OnDroppedCommand = new Prism.Commands.DelegateCommand<ListBoxDroppedEventArgs>(OnDroppedAction); //[shweta.thube][GEOS2-5976]

                AttachmentsHyperClickCommand = new RelayCommand(new Action<object>(AttachmentsHyperClickCommandAction));

                ActionPlanChildGridControlLoadedCommand = new RelayCommand(new Action<object>(ActionPlanChildGridControlLoadedCommandAction));//[Sudhir.Jangra][GEOS2-6593]
                ActionPlanChildItemListTableViewLoadedCommand = new RelayCommand(new Action<object>(ActionPlanChildItemListTableViewLoadedCommandAction));//[Sudhir.Jangra][GEOS2-6593]
                ActionPlanChildGridControlUnloadedCommand = new RelayCommand(new Action<object>(ActionPlanChildGridControlUnloadedCommandAction));//[Sudhir.Jangra][GEOS2-6593]
                SelectedDepartmentListClosedCommand = new RelayCommand(new Action<object>(FilterCommandAction));
                SwitchToTaskGridCommand = new RelayCommand(new Action<object>(SwitchToTaskGridCommandAction));//[shweta.thube][GEOS2-6453]
                ActionPlanTaskGridControlLoadedCommand = new RelayCommand(new Action<object>(ActionPlanTaskGridControlLoadedCommandAction));
                ActionPlanTaskItemListTableViewLoadedCommand = new RelayCommand(new Action<object>(ActionPlanTaskItemListTableViewLoadedCommandAction));
                ActionPlanTaskGridControlUnloadedCommand = new RelayCommand(new Action<object>(ActionPlanTaskGridControlUnloadedCommandAction));
                SelectedActionPlansCodeClosedCommand = new RelayCommand(new Action<object>(FilterCommandAction));
                CommandAlertFilterTileClick = new RelayCommand(new Action<object>(CommandAlertFilterTileClickAction));//[Sudhir.Jangra][GEOS2-5983]

                ExpandAndCollapseActionPlanCommand = new RelayCommand(new Action<object>(ExpandAndCollapseActionPlanCommandAction));//[Sudhir.Jangra][GEOS2-5983]
                DeleteActionPlanCommand = new RelayCommand(new Action<object>(DeleteActionPlanCommandAction));//[shweta.thube][GEOS2-6795]

                TaskFilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(TaskFilterEditorCreatedCommandAction);
                //Obsolete on new cache version
                //CommandTaskFilterTileClick = new RelayCommand(new Action<object>(CommandTaskFilterTileClickAction));

                CommandTaskTileBarDoubleClick = new RelayCommand(new Action<object>(CommandTaskTileBarDoubleClickAction));
                SelectedCustomerListClosedCommand = new RelayCommand(new Action<object>(FilterCommandAction));//[shweta.thube][GEOS2-6911]
                ParticipantsHyperLinkClickCommand = new RelayCommand(new Action<object>(ParticipantsHyperLinkClickCommandAction));//[shweta.thube][GEOS2-7008]
                AddActionPlanSubTaskCommand = new RelayCommand(new Action<object>(AddActionPlanSubTaskViewWindowShow)); //[pallavi.kale][GEOS2-7003][19.06.2025]
                EditSubTaskHyperLinkCommand = new RelayCommand(new Action<object>(EditSubTaskHyperLinkCommandAction)); //[pallavi.kale][GEOS2-7002][19.06.2025]
                EditActionPlanTaskHyperLinkCommand = new RelayCommand(new Action<object>(EditActionPlanTaskHyperLinkCommandAction));
                GridViewVisibility = Visibility.Visible;
                TaskViewVisibility = Visibility.Collapsed;
                TaskGridVisibility = Visibility.Collapsed;
                IsTopDockChecked = false;
                IsTopTaskDockChecked = false;
                IsAlertSectionCollapsed = false;
                IsExpand = false;
                #region //[shweta.thube][GEOS2-6453]
                try
                {
                    //APMCommon.Instance.LocationList = null;
                    //SelectedActionPlansCodeClosedCommandForTask = new RelayCommand(new Action<object>(FilterCommandActionForTask));
                    //LocationListClosedCommandForTask = new RelayCommand(new Action<object>(FilterCommandActionForTask));
                    //ResponsibleListClosedCommandForTask = new RelayCommand(new Action<object>(FilterCommandActionForTask));
                    //BusinessUnitListClosedCommandForTask = new RelayCommand(new Action<object>(FilterCommandActionForTask));
                    //OriginListClosedCommandForTask = new RelayCommand(new Action<object>(FilterCommandActionForTask));
                    LeftCommandFilterTileClickForTask = new RelayCommand(new Action<object>(FilterCommandActionForTask));
                    // Initialize commands
                    //Shubham[skadam] GEOS2-6883 HRM Expenses report not working properly.  24 01 2024
                    MasterRowExpandedCommand = new DelegateCommand<RowEventArgs>(OnMasterRowExpanded);
                    SubstituteFilterCommand = new DelegateCommand<DevExpress.Data.SubstituteFilterEventArgs>(OnSubstituteFilter);
                    CustomerListClosedCommandForTask = new RelayCommand(new Action<object>(FilterCommandActionForTask));//[pallavi.kale][GEOS2-8084][05.08.2025]
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in ActionsViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                #endregion
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 122 && up.Permission.IdGeosModule == 15))// admin
                {
                    IsAddButtonEnabled = true;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 134 && up.Permission.IdGeosModule == 15))// Location Manager
                {
                    IsAddButtonEnabled = true;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 135 && up.Permission.IdGeosModule == 15))// Department Manager
                {
                    IsAddButtonEnabled = true;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 136 && up.Permission.IdGeosModule == 15))// Base User
                {
                    IsAddButtonEnabled = true;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 121 && up.Permission.IdGeosModule == 15))//View
                {
                    IsAddButtonEnabled = false;
                }

                SubTaskCommentsHyperLinkCommand = new RelayCommand(new Action<object>(SubTaskCommentsHyperLinkCommandAction));  //[shweta.thube][GEOS2-7004][25.06.2025]
                SubTaskAttachmentsHyperClickCommand = new RelayCommand(new Action<object>(SubTaskAttachmentsHyperClickCommandAction));//[shweta.thube][GEOS2-7004][25.06.2025]
                SubTaskParticipantsHyperLinkClickCommand = new RelayCommand(new Action<object>(SubTaskParticipantsHyperLinkClickCommandAction));//[shweta.thube][GEOS2-7004][25.06.2025]
                DeleteSubTaskCommand = new RelayCommand(new Action<object>(DeleteSubTaskCommandAction));  //[shweta.thube][GEOS2-8683][01.07.2025]
                CommandActionPlansAlertFilterTileClick = new RelayCommand(new Action<object>(CommandActionPlansAlertFilterTileClickAction)); //[shweta.thube][GEOS2-7217][23.07.2025]
                ExportCustomerButtonCommand = new RelayCommand(new Action<object>(ExportCustomerButtonCommandAction));//[pallavi.kale][GEOS2-8084][07.08.2025]
                PrintCustomerButtonCommand = new RelayCommand(new Action<object>(PrintCustomerButtonCommandAction));//[pallavi.kale][GEOS2-8084][07.08.2025]
                GeosApplication.Instance.Logger.Log("Constructor ActionsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ActionsViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        //[GEOS2-6021][rdixit][17.02.2025]
        private void OpenAttendanceFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenAttendanceFile()...", category: Category.Info, priority: Priority.Low);

                Employee employee = (Employee)SelectedItem;
                ImportActionPlanView importAttendanceFileView = new ImportActionPlanView();
                ImportActionPlanViewModel importAttendanceFileViewModel = new ImportActionPlanViewModel();
                //importAttendanceFileViewModel.Init(EmployeeAttendanceList);
                EventHandler handle = delegate { importAttendanceFileView.Close(); };
                importAttendanceFileViewModel.RequestClose += handle;
                importAttendanceFileView.DataContext = importAttendanceFileViewModel;
                importAttendanceFileView.ShowDialog();

                if (importAttendanceFileViewModel.IsSave)
                {
                    ListOfFilterTile = new ObservableCollection<TileBarFilters>();
                    Init();
                    if (IsTaskGridVisibility)
                    {
                        SelectedAlertTileBarItem = new APMAlertTileBarFilters();
                        MyTaskFilterString = string.Empty;
                    }
                    if (ListOfFilterTile != null)
                    {
                        SelectedTileBarItem = ListOfFilterTile.FirstOrDefault();
                    }
                    SelectedLocation = new List<object>();
                    SelectedPerson = new List<object>();
                    SelectedBussinessUnit = new List<object>();
                    SelectedOrigin = new List<object>();
                    SelectedDepartment = new List<object>();
                    SelectedActionPlansCodeForTask = new List<object>();
                    SelectedLocationForTask = new List<object>();
                    SelectedPersonForTask = new List<object>();
                    SelectedBussinessUnitForTask = new List<object>();
                    SelectedOriginForTask = new List<object>();
                    IsActionPlanAllLocationSelected = false;
                    IsActionPlanAllResponsibleSelected = false;
                    IsActionPlanAllBUSelected = false;
                    IsActionPlanAllOriginSelected = false;
                    IsActionPlanAllDepartmentSelected = false;
                    IsActionPlanAllActionPlanSelected = false;
                    IsActionPlanAllActionPlanSelectedForTask = false;
                    IsActionPlanAllLocationSelectedForTask = false;
                    IsActionPlanAllResponsibleSelectedForTask = false;
                    IsActionPlanAllBUSelectedForTask = false;
                    IsActionPlanAllOriginSelectedForTask = false;
                    IsActionPlanAllCustomerSelected = false;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OpenAttendanceFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenAttendanceFile()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[GEOS2-6882][rdixit][23.01.2025]
        private void MouseDoubleClickActionCommandAction(object obj)
        {
            try
            {
                if (obj != null)
                {
                    GeosApplication.Instance.Logger.Log("Method MouseDoubleClickActionCommandAction....", category: Category.Info, priority: Priority.Low);
                    APMActionPlanTask selectedTask = (APMActionPlanTask)obj;
                    AddEditTaskView addEditTaskView = new AddEditTaskView();
                    AddEditTaskViewModel addEditTaskViewModel = new AddEditTaskViewModel();
                    EventHandler handle = delegate { addEditTaskView.Close(); };
                    addEditTaskViewModel.RequestClose += handle;
                    addEditTaskViewModel.IsNew = false;
                    addEditTaskViewModel.WindowHeader = Application.Current.FindResource("EditActionPlansHeader").ToString();
                    List<APMActionPlan> temp = new List<APMActionPlan>();
                    temp = new List<APMActionPlan>(TempActionPlanList);
                    APMCommon.Instance.ActionPlanList = new List<APMActionPlan>(temp.OrderBy(ap => ap.Code));
                    var ActionPlan = APMCommon.Instance.ActionPlanList.FirstOrDefault(ap => ap.IdActionPlan == selectedTask.IdActionPlan);
                    selectedTask.IdActionPlanResponsible = ActionPlan.IdEmployee;
                    addEditTaskViewModel.OtItemVisibility = Visibility.Collapsed; //[shweta.thube][GEOS2-6912]
                    addEditTaskViewModel.EditInit(selectedTask);
                    addEditTaskView.DataContext = addEditTaskViewModel;
                    addEditTaskView.ShowDialog();

                    if (addEditTaskViewModel.IsSave)
                    {
                        IsActionPlanEdit = true;
                        SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == selectedTask.IdActionPlan);
                        var index = SelectedActionPlan.TaskList.FindIndex(x => x.IdActionPlanTask == selectedTask.IdActionPlanTask);
                        if (index >= 0)
                        {
                            SelectedActionPlan.TaskList[index] = addEditTaskViewModel.UpdatedTask;
                        }

                        if (ClonedActionPlanList != null)
                        {
                            ClonedActionPlanList.FirstOrDefault(x => x.IdActionPlan == SelectedActionPlan.IdActionPlan).TaskList[index] = addEditTaskViewModel.UpdatedTask;
                        }
                        FillLeftTileList();
                        SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                        FilterCommandAction(obj);
                        CommandAlertFilterTileClickAction(obj);
                        if (PreviousSelectedAlertTileBarItem != null)
                            SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedAlertTileBarItem.Caption);
                        IsActionPlanEdit = false;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method MouseDoubleClickActionCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method MouseDoubleClickActionCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Sudhir.Jangra][GEOS2-5972]
        private void CommandTileBarDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                if (SelectedTopTileBarItem.Caption == "CUSTOM FILTERS" || SelectedTopTileBarItem.Caption == "All")
                {
                    return;
                }
                TableView table = (TableView)obj;
                GridControl gridControl = (table).Grid;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();

                string filterContent = SelectedTopTileBarItem.FilterCriteria;

                string columnName = string.Empty;
                string columnValue = string.Empty;
                if (filterContent.StartsWith("StartsWith("))
                {
                    string filterContents = filterContent.Substring("StartsWith(".Length).TrimEnd(')');
                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']');
                    columnValue = filterParts[1].Trim(' ', '\'');
                }
                else if (filterContent.StartsWith("EndsWith("))
                {
                    string filterContents = filterContent.Substring("EndsWith(".Length).TrimEnd(')');
                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']');
                    columnValue = filterParts[1].Trim(' ', '\'');
                }
                else if (filterContent.Contains("Contains(") && !filterContent.Contains("Not Contains("))
                {
                    string filterContents = filterContent.Substring("Contains(".Length).TrimEnd(')');
                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']');
                    columnValue = filterParts[1].Trim(' ', '\'');
                }
                else if (filterContent.Contains("Not Contains("))
                {
                    string filterContents = filterContent.Substring("Not Contains(".Length).TrimEnd(')');
                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']');
                    columnValue = filterParts[1].Trim(' ', '\'');
                }
                else if (filterContent.Contains(" In ") && !(filterContent.Contains("Not") && filterContent.Contains("In")))
                {
                    int startColumnIndex = filterContent.IndexOf('[') + 1;
                    int endColumnIndex = filterContent.IndexOf(']');
                    columnName = filterContent.Substring(startColumnIndex, endColumnIndex - startColumnIndex).Trim();
                }
                else if (filterContent.Contains("Not") && filterContent.Contains("In"))
                {
                    int startIndex = filterContent.IndexOf('(') + 1;
                    int endIndex = filterContent.LastIndexOf(')');
                    string filterContents = filterContent.Substring(startIndex, endIndex - startIndex).Trim();

                    string[] filterParts = filterContent.Split(new[] { "Not", "In" }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']', ' ');

                }
                else if (filterContent.Contains("Not [") && filterContent.Contains("Between("))
                {
                    var filterContents = filterContent.Substring(filterContent.IndexOf("Not [") + "Not [".Length,
                        filterContent.IndexOf("Between(") - (filterContent.IndexOf("Not [") + "Not [".Length)).Trim();

                    var betweenContent = filterContent.Substring(filterContent.IndexOf("Between(") + "Between(".Length).TrimEnd(')');
                    var filterParts = betweenContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (filterParts.Length == 2)
                    {
                        columnName = filterContents.Trim(' ', '[', ']');
                        var lowerBound = filterParts[0].Trim().Trim('\'');
                        var upperBound = filterParts[1].Trim().Trim('\'');
                    }
                }
                else if (filterContent.Contains("Between("))
                {
                    string filterContents = filterContent.Substring(filterContent.IndexOf("Between(") + "Between(".Length).TrimEnd(')');

                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (filterParts.Length == 2)
                    {
                        columnName = filterContent.Substring(1, filterContent.IndexOf("]") - 1).Trim('[', ' ', ']');
                    }
                }
                else if (filterContent.Contains("<>"))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*<>?\s*(-?\d+|'[^']*')", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains(">") && !(filterContent.Contains(">=")))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*>\s*'(.*?)'", RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*>\s*(-?\d+)", RegexOptions.IgnoreCase);
                    }

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*>\s*#(.*?)#", RegexOptions.IgnoreCase);
                    }
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }

                else if (filterContent.Contains("<") && !(filterContent.Contains("<=")))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*<\s*'(.*?)'", RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*<\s*(-?\d+)", RegexOptions.IgnoreCase);
                    }

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*<\s*#(.*?)#", RegexOptions.IgnoreCase);
                    }
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains(">="))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*>=\s*'(.*?)'", RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*>=\s*(-?\d+)", RegexOptions.IgnoreCase);
                    }

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*>=\s*#(.*?)#", RegexOptions.IgnoreCase);
                    }
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains("<="))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*<=\s*'(.*?)'", RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*<=\s*(-?\d+)", RegexOptions.IgnoreCase);
                    }

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*<=\s*#(.*?)#", RegexOptions.IgnoreCase);
                    }
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains("Not IsNullOrEmpty"))
                {
                    columnName = filterContent.Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(']')[0].Trim();
                }
                else if (filterContent.Contains("IsNullOrEmpty"))
                {
                    columnName = filterContent.Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(']')[0].Trim();
                }
                else if (filterContent.Contains("="))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*=\s*(?:'(.*?)'|(-?\d+(\.\d+)?)|#(.*?)#)", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains("!=") || filterContent.Contains("<>"))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*(!=|<>)\s*'([^']*)'", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[3].Value;
                    }
                }
                else if (filterContent.Contains(" Like ") || filterContent.Contains(" Not Like "))
                {
                    columnName = filterContent.Substring(1, filterContent.IndexOf("]") - 1).Trim('[', ' ', ']');
                }
                else if (filterContent.Contains("Is Null"))
                {
                    int startIndex = filterContent.IndexOf('[');
                    int endIndex = filterContent.IndexOf(']');
                    columnName = filterContent.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                }

                else if (filterContent.Contains("Is Not Null"))
                {
                    int startIndex = filterContent.IndexOf('[');
                    int endIndex = filterContent.IndexOf(']');
                    columnName = filterContent.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                }

                string normalizedColumnName = columnName.Replace(" ", "").ToLower();

                GridColumn column = GridColumnList.FirstOrDefault(x =>
                    x.Header.ToString().Replace(" ", "").ToLower().Equals(normalizedColumnName));

                if (column == null)
                {
                    column = GridColumnList.FirstOrDefault(x =>
                        x.FieldName.Replace(" ", "").ToLower().Equals(normalizedColumnName));
                }

                if (column != null)
                {
                    IsEdit = true;
                    table.ShowFilterEditor(column);
                }

                GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTileBarDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5974]
        //private void CommandFilterTileClickAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method CommandFilterTileClickAction....", category: Category.Info, priority: Priority.Low);
        //        if (ActionPlanList.Count > 0)
        //        {
        //            var temp = (System.Windows.Controls.SelectionChangedEventArgs)obj;
        //            if (temp.AddedItems.Count > 0)
        //            {
        //                string str = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;
        //                string Template = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Type;
        //                string _FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
        //                string CustomFilter = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;
        //                CustomFilterHTMLColor = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).BackColor;


        //                if (CustomFilter.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
        //                    return;


        //                if (str == null)
        //                {
        //                    if (!string.IsNullOrEmpty(_FilterString))
        //                    {

        //                        if (!string.IsNullOrEmpty(_FilterString))
        //                            MyFilterString = _FilterString;
        //                        else
        //                            MyFilterString = string.Empty;
        //                    }
        //                    else
        //                        MyFilterString = string.Empty;
        //                }
        //                else
        //                {
        //                    if (str.Equals("All"))
        //                    {
        //                        MyFilterString = string.Empty;
        //                        ActionPlanList = new ObservableCollection<APMActionPlan>(TempActionPlanList);
        //                        SetDeletionFlags(ActionPlanList);
        //                    }
        //                    else
        //                    {
        //                        if (!string.IsNullOrEmpty(_FilterString))
        //                        {

        //                            if (!string.IsNullOrEmpty(_FilterString))
        //                                MyFilterString = _FilterString;
        //                            else
        //                                MyFilterString = string.Empty;
        //                        }
        //                        else
        //                            MyFilterString = string.Empty;
        //                    }
        //                }

        //            }
        //        }
        //        GeosApplication.Instance.Logger.Log("Method CommandFilterTileClickAction....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method CommandFilterTileClickAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //[Sudhir.Jangra][GEOS2-5972]
        private void FilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            try
            {
                obj.Handled = true;
                TableView table = (TableView)obj.OriginalSource;
                GridControl gridControl = (table).Grid;
                ShowFilterEditor(obj);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in FilterEditorCreatedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor()...", category: Category.Info, priority: Priority.Low);
                CustomActionPlanFilterEditorView customFilterEditorView = new CustomActionPlanFilterEditorView();
                CustomActionPlanFilterEditorViewModel customFilterEditorViewModel = new CustomActionPlanFilterEditorViewModel();
                string titleText = DevExpress.Xpf.Grid.GridControlLocalizer.Active.GetLocalizedString(GridControlStringId.FilterEditorTitle);
                if (IsEdit)
                {
                    customFilterEditorViewModel.FilterName = SelectedTopTileBarItem.Caption;
                    customFilterEditorViewModel.HTMLColor = SelectedTopTileBarItem.BackColor;
                    customFilterEditorViewModel.IsSave = true;
                    customFilterEditorViewModel.IsNew = false;
                    IsEdit = false;
                }
                else
                    customFilterEditorViewModel.IsNew = true;
                if (TopListOfFilterTile == null)
                {
                    TopListOfFilterTile = new ObservableCollection<TileBarFilters>();
                }


                customFilterEditorViewModel.Init(e.FilterControl, TopListOfFilterTile);


                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");

                customFilterEditorView.Grid.Children.Add(e.FilterControl);


                customFilterEditorView.ShowDialog();

                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    if (TopListOfFilterTile == null)
                    {
                        TopListOfFilterTile = new ObservableCollection<TileBarFilters>();
                    }
                    TileBarFilters tileBarItem = TopListOfFilterTile.FirstOrDefault(x => x.Caption.Equals(customFilterEditorViewModel.FilterName));

                    if (tileBarItem != null)
                    {
                        TopListOfFilterTile.Remove(tileBarItem);
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;
                            string htmlColor = "";
                            string keyName = "";
                            if (setting.Key.Contains(userSettingsKeyForActionPlan))
                            {
                                key = setting.Key.Replace(userSettingsKeyForActionPlan, "");
                                string _html = key.Replace(tileBarItem.Caption, "");
                                htmlColor = _html.Replace("_", "");
                                if (!string.IsNullOrEmpty(htmlColor))
                                {
                                    keyName = key.Replace(_html, "");
                                }

                            }
                            if (keyName.Equals(tileBarItem.Caption) && !string.IsNullOrEmpty(htmlColor))
                            {
                                continue;
                            }



                            if (!key.Equals(tileBarItem.Caption))
                            {
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            }

                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                        TopListOfFilterTile = new ObservableCollection<TileBarFilters>();
                        AddCustomSetting();
                        SelectedTopTileBarItem = TopListOfFilterTile.FirstOrDefault();
                        if (TopListOfFilterTile.Count == 1)
                        {
                            TopListOfFilterTile = new ObservableCollection<TileBarFilters>();
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && !customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    if (TopListOfFilterTile == null)
                    {
                        TopListOfFilterTile = new ObservableCollection<TileBarFilters>();
                    }
                    TileBarFilters tileBarItem = TopListOfFilterTile.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        CustomFilterHTMLColor = customFilterEditorViewModel.HTMLColor;
                        TableView table = (TableView)e.OriginalSource;
                        GridControl gridControl = (table).Grid;
                        List<DevExpress.Xpf.Grid.GridTotalSummaryData> summary = new List<GridTotalSummaryData>(gridControl.View.FixedSummariesLeft);
                        VisibleRowCount = (Int32)summary.FirstOrDefault().Value;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customFilterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = VisibleRowCount;
                        tileBarItem.EntitiesCountVisibility = Visibility.Visible;
                        tileBarItem.FilterCriteria = customFilterEditorViewModel.FilterCriteria;
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;
                            string htmlColor = "";
                            string keyName = "";

                            if (setting.Key.Contains(userSettingsKeyForActionPlan))
                            {
                                key = setting.Key.Replace(userSettingsKeyForActionPlan, "");
                                string[] splitted = key.Split('_');
                                if (splitted != null && splitted.Length > 1)
                                {
                                    string _html = key.Replace(filterCaption, "");
                                    htmlColor = _html.Replace("_", "");
                                    if (!string.IsNullOrEmpty(htmlColor))
                                    {
                                        keyName = key.Replace(_html, "");
                                    }
                                }
                            }

                            if (keyName.Equals(filterCaption) && !string.IsNullOrEmpty(htmlColor))
                            {
                                TopListOfFilterTile.Where(a => a.Caption == SelectedTopTileBarItem.Caption).ToList().ForEach(b => b.BackColor = CustomFilterHTMLColor);
                                PreviousSelectedTopTileBarItem = SelectedTopTileBarItem;
                                string name = userSettingsKeyForActionPlan + tileBarItem.Caption + "_" + CustomFilterHTMLColor;
                                lstUserConfiguration.Add(new Tuple<string, string>(name.ToString(), setting.Value.ToString()));
                                continue;
                            }


                            if (!key.Equals(filterCaption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            else
                                lstUserConfiguration.Add(new Tuple<string, string>((userSettingsKeyForActionPlan + tileBarItem.Caption), tileBarItem.FilterCriteria));


                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                        TopListOfFilterTile = new ObservableCollection<TileBarFilters>(TopListOfFilterTile);
                    }
                }
                else if (customFilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    TableView table = (TableView)e.OriginalSource;
                    GridControl gridControl = (table).Grid;
                    List<DevExpress.Xpf.Grid.GridTotalSummaryData> summary = new List<GridTotalSummaryData>(gridControl.View.FixedSummariesLeft);
                    VisibleRowCount = (Int32)summary.FirstOrDefault().Value;
                    if (TopListOfFilterTile == null)
                    {
                        TopListOfFilterTile = new ObservableCollection<TileBarFilters>();
                    }
                    TopListOfFilterTile.Add(new TileBarFilters()
                    {
                        Caption = customFilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = customFilterEditorViewModel.HTMLColor,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = customFilterEditorViewModel.FilterCriteria,
                        Height = 50,
                        width = 150,
                        EntitiesCount = VisibleRowCount
                    });

                    string filterName = "";

                    filterName = userSettingsKeyForActionPlan + customFilterEditorViewModel.FilterName;

                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);

                    if (customFilterEditorViewModel.HTMLColor != null)
                    {
                        string FilterName = "";
                        filterName = userSettingsKeyForActionPlan + customFilterEditorViewModel.FilterName + "_" + customFilterEditorViewModel.HTMLColor;
                        GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                        ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                    TopListOfFilterTile = new ObservableCollection<TileBarFilters>();
                    AddCustomSetting();
                    SelectedTopTileBarItem = TopListOfFilterTile.LastOrDefault();
                    TopListOfFilterTile = new ObservableCollection<TileBarFilters>(TopListOfFilterTile.OrderBy(a => a.Caption != "All").ThenBy(a => a.Caption));
                    //SelectedTopTileBarItem = TopListOfFilterTile.LastOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = new List<KeyValuePair<string, string>>();
                if (TopListOfFilterTile == null)
                {
                    TopListOfFilterTile = new ObservableCollection<TileBarFilters>();
                }
                tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKeyForActionPlan)).ToList();


                if (tempUserSettings != null && tempUserSettings.Count > 0)
                {
                    if (!TopListOfFilterTile.Any(x => x.Caption == "All"))
                    {

                        int totalTasksAndSubTasks = ClonedActionPlanList?.Sum(ap => ap.TaskList?.Sum(t => 1 + (t.SubTaskList?.Count ?? 0)) ?? 0) ?? 0;

                        TopListOfFilterTile.Add(
                                      new TileBarFilters()
                                      {
                                          Caption = "All",
                                          Id = 0,
                                          BackColor = null,
                                          ForeColor = null,
                                          FilterCriteria = null,
                                          EntitiesCount = totalTasksAndSubTasks,
                                          EntitiesCountVisibility = Visibility.Visible,
                                          Height = 50,
                                          width = 150
                                      });
                    }

                    var colorMapping = new Dictionary<string, string>();
                    foreach (var item in tempUserSettings)
                    {
                        var parts = item.Key.Split(new[] { "_#" }, StringSplitOptions.None);

                        if (parts.Length > 1)
                        {
                            string baseKey = parts[0].Replace(userSettingsKeyForActionPlan, "");
                            string colorCode = "#" + parts[1];

                            colorMapping[baseKey] = colorCode;
                        }
                    }
                    foreach (var item in tempUserSettings)
                    {
                        try
                        {
                            string filter = item.Value.Replace("[Status]", "Status");
                            var parts = item.Key.Split(new[] { "_#" }, StringSplitOptions.None);
                            string baseKey = parts[0].Replace(userSettingsKeyForActionPlan, "");

                            string backColor = colorMapping.ContainsKey(baseKey) ? colorMapping[baseKey] : null;
                            bool isDuplicate = TopListOfFilterTile.Any(tile => tile.Caption == baseKey && tile.FilterCriteria == item.Value);

                            if (!isDuplicate)
                            {
                                CriteriaOperator op = CriteriaOperator.Parse(filter);

                                int count = 0;
                                if (filter.StartsWith("StartsWith("))
                                {
                                    string filterContent = filter.Substring("StartsWith(".Length).TrimEnd(')');
                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']');
                                    string columnValue = filterParts[1].Trim(' ', '\'');

                                    count = ClonedActionPlanList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        var value = property?.GetValue(ap)?.ToString();
                                        return value != null && value.StartsWith(columnValue, StringComparison.OrdinalIgnoreCase);
                                    });
                                }
                                else if (filter.StartsWith("EndsWith("))
                                {
                                    string filterContent = filter.Substring("EndsWith(".Length).TrimEnd(')');
                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']');
                                    string columnValue = filterParts[1].Trim(' ', '\'');

                                    count = ClonedActionPlanList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        var value = property?.GetValue(ap)?.ToString();
                                        return value != null && value.EndsWith(columnValue, StringComparison.OrdinalIgnoreCase);
                                    });
                                }
                                else if (filter.Contains("Contains(") && !filter.Contains("Not Contains("))
                                {
                                    string filterContent = filter.Substring("Contains(".Length).TrimEnd(')');
                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']');
                                    string columnValue = filterParts[1].Trim(' ', '\'');

                                    count = ClonedActionPlanList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        var value = property?.GetValue(ap)?.ToString();
                                        return value != null && value.IndexOf(columnValue, StringComparison.OrdinalIgnoreCase) >= 0;
                                    });
                                }
                                else if (filter.Contains("Not Contains("))
                                {
                                    string filterContent = filter.Substring("Not Contains(".Length).TrimEnd(')');
                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']');
                                    string columnValue = filterParts[1].Trim(' ', '\'');

                                    count = ClonedActionPlanList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        var value = property?.GetValue(ap)?.ToString();
                                        return value == null || value.IndexOf(columnValue, StringComparison.OrdinalIgnoreCase) < 0;
                                    });
                                }
                                else if (filter.Contains(" In ") && !(filter.Contains("Not") && filter.Contains("In")))
                                {
                                    int startColumnIndex = filter.IndexOf('[') + 1;
                                    int endColumnIndex = filter.IndexOf(']');
                                    string columnName = filter.Substring(startColumnIndex, endColumnIndex - startColumnIndex).Trim();

                                    int startValuesIndex = filter.IndexOf("In (", StringComparison.OrdinalIgnoreCase) + "In (".Length;
                                    int endValuesIndex = filter.IndexOf(')', startValuesIndex);
                                    string filterContent = filter.Substring(startValuesIndex, endValuesIndex - startValuesIndex).Trim();

                                    var columnValues = filterContent
          .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
          .Select(v => v.Trim().Trim('\'', '#'))
          .ToList();

                                    count = ClonedActionPlanList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        if (property != null)
                                        {
                                            var value = property.GetValue(ap);

                                            if (value != null)
                                            {
                                                if (property.PropertyType == typeof(string))
                                                {
                                                    return columnValues.Contains(value.ToString(), StringComparer.OrdinalIgnoreCase);
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    var dateValue = (DateTime)value;
                                                    return columnValues.Contains(dateValue.ToString("yyyy-MM-dd"), StringComparer.OrdinalIgnoreCase);
                                                }
                                                else if (typeof(IComparable).IsAssignableFrom(property.PropertyType))
                                                {
                                                    foreach (var columnValue in columnValues)
                                                    {
                                                        try
                                                        {
                                                            var targetValue = Convert.ChangeType(columnValue, property.PropertyType);
                                                            if (value.Equals(targetValue))
                                                            {
                                                                return true;
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            // Handle conversion exceptions if necessary
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        return false;
                                    });
                                }
                                else if (filter.Contains("Not") && filter.Contains("In"))
                                {
                                    int startIndex = filter.IndexOf('(') + 1;
                                    int endIndex = filter.LastIndexOf(')');
                                    string filterContent = filter.Substring(startIndex, endIndex - startIndex).Trim();

                                    string[] filterParts = filter.Split(new[] { "Not", "In" }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']', ' ');

                                    var columnValues = filterContent
                                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(v => v.Trim().Trim('\'', ' ', '#'))
                                        .ToArray();

                                    count = ClonedActionPlanList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        if (property != null)
                                        {
                                            var value = property.GetValue(ap);

                                            if (value != null)
                                            {
                                                if (property.PropertyType == typeof(string))
                                                {
                                                    return !columnValues.Contains(value.ToString(), StringComparer.OrdinalIgnoreCase);
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    var dateValue = (DateTime)value;
                                                    return !columnValues.Contains(dateValue.ToString("yyyy-MM-dd"), StringComparer.OrdinalIgnoreCase);
                                                }
                                                else if (typeof(IComparable).IsAssignableFrom(property.PropertyType))
                                                {
                                                    foreach (var columnValue in columnValues)
                                                    {
                                                        try
                                                        {
                                                            var targetValue = Convert.ChangeType(columnValue, property.PropertyType);
                                                            if (value.Equals(targetValue))
                                                            {
                                                                return false;
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            // Handle conversion exceptions if necessary
                                                        }
                                                    }
                                                    return true;
                                                }
                                            }
                                        }
                                        return true;
                                    });
                                }

                                else if (filter.Contains("Not [") && filter.Contains("Between("))
                                {
                                    var filterContent = filter.Substring(filter.IndexOf("Not [") + "Not [".Length,
                                        filter.IndexOf("Between(") - (filter.IndexOf("Not [") + "Not [".Length)).Trim();

                                    var betweenContent = filter.Substring(filter.IndexOf("Between(") + "Between(".Length).TrimEnd(')');
                                    var filterParts = betweenContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                    if (filterParts.Length == 2)
                                    {
                                        var columnName = filterContent.Trim(' ', '[', ']');
                                        var lowerBound = filterParts[0].Trim().Trim('\'');
                                        var upperBound = filterParts[1].Trim().Trim('\'');

                                        count = ClonedActionPlanList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property != null ? property.GetValue(ap) : null;

                                            if (value != null)
                                            {
                                                if (property.PropertyType == typeof(string))
                                                {
                                                    var stringValue = value as string;
                                                    return string.Compare(stringValue, lowerBound, StringComparison.OrdinalIgnoreCase) < 0 ||
                                                           string.Compare(stringValue, upperBound, StringComparison.OrdinalIgnoreCase) > 0;
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    DateTime lowerDate, upperDate;

                                                    bool lowerParsed = DateTime.TryParse(lowerBound, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out lowerDate);
                                                    bool upperParsed = DateTime.TryParse(upperBound, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out upperDate);

                                                    if (value is DateTime?)
                                                    {
                                                        DateTime? nullableDateTime = (DateTime?)value;
                                                        return !nullableDateTime.HasValue ||
                                                               nullableDateTime.Value.CompareTo(lowerDate) < 0 ||
                                                               nullableDateTime.Value.CompareTo(upperDate) > 0;
                                                    }
                                                    else
                                                    {
                                                        return ((DateTime)value).CompareTo(lowerDate) < 0 ||
                                                               ((DateTime)value).CompareTo(upperDate) > 0;
                                                    }
                                                }
                                                else if (typeof(IComparable).IsAssignableFrom(property.PropertyType))
                                                {
                                                    var comparable = (IComparable)value;

                                                    var lowerBoundValue = Convert.ChangeType(lowerBound, property.PropertyType);
                                                    var upperBoundValue = Convert.ChangeType(upperBound, property.PropertyType);

                                                    return comparable.CompareTo(lowerBoundValue) < 0 ||
                                                           comparable.CompareTo(upperBoundValue) > 0;
                                                }
                                            }

                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains("Between("))
                                {
                                    string filterContent = filter.Substring(filter.IndexOf("Between(") + "Between(".Length).TrimEnd(')');

                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                    if (filterParts.Length == 2)
                                    {
                                        string columnName = filter.Substring(1, filter.IndexOf("]") - 1).Trim('[', ' ', ']');

                                        var lowerBound = filterParts[0].Trim().Trim('\'', ' ');
                                        var upperBound = filterParts[1].Trim().Trim('\'', ' ');

                                        count = ClonedActionPlanList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property?.GetValue(ap);

                                            if (value != null)
                                            {
                                                if (property.PropertyType == typeof(string))
                                                {
                                                    var stringValue = value as string;
                                                    return string.Compare(stringValue, lowerBound, StringComparison.OrdinalIgnoreCase) >= 0 &&
                                                           string.Compare(stringValue, upperBound, StringComparison.OrdinalIgnoreCase) <= 0;
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    DateTime lowerDate, upperDate;

                                                    bool lowerParsed = DateTime.TryParse(lowerBound, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out lowerDate);
                                                    bool upperParsed = DateTime.TryParse(upperBound, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out upperDate);

                                                    if (value is DateTime?)
                                                    {
                                                        DateTime? nullableDateTime = (DateTime?)value;
                                                        return nullableDateTime.HasValue &&
                                                               nullableDateTime.Value.CompareTo(lowerDate) >= 0 &&
                                                               nullableDateTime.Value.CompareTo(upperDate) <= 0;
                                                    }
                                                    else
                                                    {
                                                        return ((DateTime)value).CompareTo(lowerDate) >= 0 &&
                                                               ((DateTime)value).CompareTo(upperDate) <= 0;
                                                    }
                                                }
                                                else if (typeof(IComparable).IsAssignableFrom(property.PropertyType))
                                                {
                                                    var comparable = (IComparable)value;
                                                    var lowerBoundValue = Convert.ChangeType(lowerBound, property.PropertyType);
                                                    var upperBoundValue = Convert.ChangeType(upperBound, property.PropertyType);

                                                    return comparable.CompareTo(lowerBoundValue) >= 0 &&
                                                           comparable.CompareTo(upperBoundValue) <= 0;
                                                }
                                            }

                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains("<>"))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*<>?\s*(-?\d+|'[^']*')", RegexOptions.IgnoreCase);
                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value.Trim('\'');

                                        count = ClonedActionPlanList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property?.GetValue(ap)?.ToString();

                                            return value != null && !string.Equals(value, columnValue, StringComparison.OrdinalIgnoreCase);
                                        });
                                    }
                                }

                                else if (filter.Contains(">") && !filter.Contains(">="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*>\s*'(.*?)'", RegexOptions.IgnoreCase);

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*>\s*(-?\d+)", RegexOptions.IgnoreCase);
                                    }

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*>\s*#(.*?)#", RegexOptions.IgnoreCase);
                                    }

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value;

                                        count = ClonedActionPlanList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property != null ? property.GetValue(ap) : null;

                                            if (value != null && value is IComparable)
                                            {
                                                var comparable = (IComparable)value;
                                                object targetValue;

                                                if (property.PropertyType == typeof(string))
                                                {
                                                    targetValue = columnValue;
                                                    return string.Compare(value.ToString(), targetValue.ToString(), StringComparison.OrdinalIgnoreCase) > 0;
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    DateTime dateValue;

                                                    if (DateTime.TryParse(columnValue, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out dateValue))
                                                    {

                                                        if (value is DateTime?)
                                                        {
                                                            DateTime? nullableDateTime = (DateTime?)value;
                                                            return nullableDateTime.HasValue && comparable.CompareTo(dateValue) > 0;
                                                        }
                                                        else
                                                        {
                                                            return comparable.CompareTo(dateValue) > 0;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    targetValue = Convert.ChangeType(columnValue, property.PropertyType);
                                                    return comparable.CompareTo(targetValue) > 0;
                                                }
                                            }
                                            return false;
                                        });
                                    }
                                }

                                else if (filter.Contains("<") && !filter.Contains("<="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*<\s*'(.*?)'", RegexOptions.IgnoreCase);

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*<\s*(-?\d+)", RegexOptions.IgnoreCase);
                                    }

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*<\s*#(.*?)#", RegexOptions.IgnoreCase);
                                    }

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value;

                                        count = ClonedActionPlanList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property != null ? property.GetValue(ap) : null;

                                            if (value != null && value is IComparable)
                                            {
                                                var comparable = (IComparable)value;
                                                object targetValue;

                                                if (property.PropertyType == typeof(string))
                                                {
                                                    targetValue = columnValue;
                                                    return string.Compare(value.ToString(), targetValue.ToString(), StringComparison.OrdinalIgnoreCase) < 0;
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    DateTime dateValue;

                                                    if (DateTime.TryParse(columnValue, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out dateValue))
                                                    {
                                                        if (value is DateTime?)
                                                        {
                                                            DateTime? nullableDateTime = (DateTime?)value;
                                                            return nullableDateTime.HasValue && comparable.CompareTo(dateValue) < 0;
                                                        }
                                                        else
                                                        {
                                                            return comparable.CompareTo(dateValue) < 0;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    targetValue = Convert.ChangeType(columnValue, property.PropertyType);
                                                    return comparable.CompareTo(targetValue) < 0;
                                                }
                                            }
                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains(">="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*>=\s*'(.*?)'", RegexOptions.IgnoreCase);

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*>=\s*(-?\d+)", RegexOptions.IgnoreCase);
                                    }

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*>=\s*#(.*?)#", RegexOptions.IgnoreCase);
                                    }

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value;

                                        count = ClonedActionPlanList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property != null ? property.GetValue(ap) : null;

                                            if (value != null && value is IComparable)
                                            {
                                                var comparable = (IComparable)value;
                                                object targetValue;

                                                if (property.PropertyType == typeof(string))
                                                {
                                                    targetValue = columnValue;
                                                    return string.Compare(value.ToString(), targetValue.ToString(), StringComparison.OrdinalIgnoreCase) >= 0;
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    DateTime dateValue;

                                                    if (DateTime.TryParse(columnValue, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out dateValue))
                                                    {
                                                        if (value is DateTime?)
                                                        {
                                                            DateTime? nullableDateTime = (DateTime?)value;
                                                            return nullableDateTime.HasValue && comparable.CompareTo(dateValue) >= 0;
                                                        }
                                                        else
                                                        {
                                                            return comparable.CompareTo(dateValue) >= 0;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    targetValue = Convert.ChangeType(columnValue, property.PropertyType);
                                                    return comparable.CompareTo(targetValue) >= 0;
                                                }
                                            }
                                            return false;
                                        });
                                    }
                                }

                                else if (filter.Contains("<="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*<=\s*'(.*?)'", RegexOptions.IgnoreCase);

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*<=\s*(-?\d+)", RegexOptions.IgnoreCase);
                                    }

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*<=\s*#(.*?)#", RegexOptions.IgnoreCase);
                                    }

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value;

                                        count = ClonedActionPlanList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property != null ? property.GetValue(ap) : null;

                                            if (value != null)
                                            {
                                                if (property.PropertyType == typeof(string))
                                                {
                                                    var stringValue = value as string;
                                                    return string.Compare(stringValue, columnValue, StringComparison.OrdinalIgnoreCase) <= 0;
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    DateTime dateValue;

                                                    if (DateTime.TryParse(columnValue, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out dateValue))
                                                    {
                                                        if (value is DateTime?)
                                                        {
                                                            DateTime? nullableDateTime = (DateTime?)value;
                                                            return nullableDateTime.HasValue && nullableDateTime.Value.CompareTo(dateValue) <= 0;
                                                        }
                                                        else
                                                        {
                                                            return ((DateTime)value).CompareTo(dateValue) <= 0;
                                                        }
                                                    }
                                                }
                                                else if (typeof(IComparable).IsAssignableFrom(property.PropertyType))
                                                {
                                                    var comparable = value as IComparable;
                                                    var targetValue = Convert.ChangeType(columnValue, property.PropertyType);
                                                    return comparable.CompareTo(targetValue) <= 0;
                                                }
                                            }
                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains("Not IsNullOrEmpty"))
                                {
                                    string columnName = filter.Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(']')[0].Trim();
                                    count = ClonedActionPlanList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        var value = property?.GetValue(ap)?.ToString();
                                        return !string.IsNullOrEmpty(value);
                                    });
                                }
                                else if (filter.Contains("IsNullOrEmpty"))
                                {
                                    string columnName = filter.Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(']')[0].Trim();
                                    count = ClonedActionPlanList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        var value = property?.GetValue(ap)?.ToString();
                                        return string.IsNullOrEmpty(value);
                                    });
                                }
                                else if (filter.Contains("Is Null"))
                                {
                                    int startIndex = filter.IndexOf('[');
                                    int endIndex = filter.IndexOf(']');


                                    string columnName = filter.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();

                                    count = ClonedActionPlanList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        if (property == null)
                                        {
                                            return false;
                                        }

                                        var value = property.GetValue(ap);

                                        return value == null;
                                    });

                                }

                                else if (filter.Contains("Is Not Null"))
                                {
                                    int startIndex = filter.IndexOf('[');
                                    int endIndex = filter.IndexOf(']');


                                    string columnName = filter.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();

                                    count = ClonedActionPlanList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        if (property == null)
                                        {
                                            return false;
                                        }

                                        var value = property.GetValue(ap);

                                        return value != null;
                                    });

                                }

                                else if (filter.Contains("="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*=\s*(?:'(.*?)'|(-?\d+(\.\d+)?)|#(.*?)#)", RegexOptions.IgnoreCase);

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string stringValue = match.Groups[2].Value;
                                        string numericValue = match.Groups[3].Value;
                                        string dateValue = match.Groups[4].Value;

                                        count = ClonedActionPlanList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property?.GetValue(ap);

                                            if (value != null)
                                            {
                                                if (value is string)
                                                {
                                                    return string.Equals(value.ToString(), stringValue, StringComparison.OrdinalIgnoreCase);
                                                }
                                                else if (value is int || value is long || value is decimal || value is double)
                                                {
                                                    return value.ToString() == numericValue;
                                                }
                                            }
                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains("!=") || filter.Contains("<>"))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*(!=|<>)\s*'([^']*)'", RegexOptions.IgnoreCase);
                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[3].Value;

                                        count = ClonedActionPlanList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property?.GetValue(ap).ToString();

                                            return value == null || !string.Equals(value, columnValue, StringComparison.OrdinalIgnoreCase);
                                        });
                                    }
                                }
                                else if (filter.Contains(" Like ") && !filter.Contains(" Not Like "))
                                {
                                    var columnNameStartIndex = filter.IndexOf("[") + 1;
                                    var columnNameEndIndex = filter.IndexOf("]", columnNameStartIndex);
                                    var columnName = filter.Substring(columnNameStartIndex, columnNameEndIndex - columnNameStartIndex).Trim();

                                    var likeValueStartIndex = filter.IndexOf("Like", columnNameEndIndex) + "Like".Length;
                                    var likeValue = filter.Substring(likeValueStartIndex).Trim().Trim('\'');

                                    count = 0;
                                }
                                else if (filter.Contains(" Not Like "))
                                {
                                    var columnName = filter.Substring(1, filter.IndexOf("]") - 1).Trim('[', ' ', ']');
                                    var notLikeValue = filter.Substring(filter.IndexOf("Not Like") + "Not Like".Length).Trim().Trim('\'');

                                    count = ClonedActionPlanList.Count();
                                }


                                TopListOfFilterTile.Add(
                                    new TileBarFilters()
                                    {
                                        Caption = baseKey,
                                        Id = 0,
                                        BackColor = !string.IsNullOrEmpty(backColor) ? backColor : null,
                                        ForeColor = null,
                                        FilterCriteria = item.Value,
                                        EntitiesCount = count,
                                        EntitiesCountVisibility = Visibility.Visible,
                                        Height = 50,
                                        width = 150
                                    });
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSetting() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }



                GeosApplication.Instance.Logger.Log("Method AddCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                var parameters = obj as object[];

                TableView actionPlansView = parameters[0] as TableView;
                TableView tasksGridView = parameters[1] as TableView;
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog
                {
                    DefaultExt = "xlsx",
                    FileName = IsTaskGridVisibility ? "Tasks List" : "Action Plans List",
                    Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*",
                    FilterIndex = 1,
                    Title = "Save Excel Report"

                };
                DialogResult = (Boolean)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    IsBusy = true;
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Show(x =>
                        {
                            Window win = new Window()
                            {
                                ShowActivated = false,
                                WindowStyle = WindowStyle.None,
                                ResizeMode = ResizeMode.NoResize,
                                AllowsTransparency = true,
                                Background = new SolidColorBrush(Colors.Transparent),
                                ShowInTaskbar = false,
                                Topmost = true,
                                SizeToContent = SizeToContent.WidthAndHeight,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                            };
                            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                            win.Topmost = false;
                            return win;
                        }, x =>
                        {
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }

                    ResultFileName = saveFile.FileName;
                    TableView viewToExport = IsTaskGridVisibility ? tasksGridView : actionPlansView;
                    viewToExport.ShowTotalSummary = false;
                    viewToExport.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();

                    viewToExport.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    viewToExport.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    (viewToExport).DataControl.RefreshData();
                    (viewToExport).DataControl.UpdateLayout();

                    GeosApplication.Instance.Logger.Log("Method ExportButtonCommandAction() executed successfully.", category: Category.Info, priority: Priority.Low);
                }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Error in Method ExportButtonCommandAction(): " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[shweta.thube][GEOS2-6453]
        private void PrintButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                var parameters = obj as object[];
                TableView actionPlansView = parameters[0] as TableView;
                TableView tasksGridView = parameters[1] as TableView;

                TableView viewToExport = IsTaskGridVisibility ? tasksGridView : actionPlansView;

                PrintableControlLink pcl = new PrintableControlLink(viewToExport);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.BPlus;
                if (!IsTaskGridVisibility)
                {
                    pcl.PageHeaderTemplate = (DataTemplate)(viewToExport.Resources["ActionsViewCustomPrintHeaderTemplate"]);
                    pcl.PageFooterTemplate = (DataTemplate)(viewToExport.Resources["ActionsViewCustomPrintFooterTemplate"]);
                }
                else
                {
                    pcl.PageHeaderTemplate = (DataTemplate)(viewToExport.Resources["ActionsTaskViewCustomPrintHeaderTemplate"]);
                    pcl.PageFooterTemplate = (DataTemplate)(viewToExport.Resources["ActionsTaskViewCustomPrintFooterTemplate"]);
                }
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5974]
        private void SelectedYearChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new System.Windows.Media.SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                InvalidateActionPlanCache();
                InvalidateTaskCache();
                FillActionPlan();
                FillTaskGrid();
                FillBusinessUnitList();
                FillOriginList();
                FillLocationList();
                FillResponsibleList();
                FillLeftTileList();

                TopListOfFilterTile = new ObservableCollection<TileBarFilters>();
                AddCustomSetting();
                TopTaskListOfFilterTile = new ObservableCollection<TileBarFilters>();
                AddTaskCustomSetting();

                FillStatus(); // [nsatpute][09-10-2024][GEOS2-5975]
                              //[GEOS2-6496][27.09.2024][rdixit]
                              //SelectedLocation = new List<object>() ;
                              //  SelectedOrigin = new List<object>();
                              // SelectedPerson = new List<object>();
                              //  SelectedBussinessUnit = new List<object>();
                              // SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == "All");
                              //  MyFilterString = string.Empty;
                              //  MyChildFilterString = string.Empty;
                              //  MyTaskFilterString = string.Empty;
                FillDepartmentList();//[Sudhir.jangra][GEOS2-6596]
                if (IsTaskGridVisibility)
                {
                    SelectedAlertTileBarItem = new APMAlertTileBarFilters();
                    MyTaskFilterString = string.Empty;
                    //FilterCommandActionForTask(obj);
                }
                //else
                //{
                //    FilterCommandAction(obj);
                //}
                if (ListOfFilterTile != null)
                {
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault();
                }
                SelectedTopTileBarItem = TopListOfFilterTile.FirstOrDefault();
                SelectedTaskTopTileBarItem = TopTaskListOfFilterTile.FirstOrDefault();
                SelectedLocation = new List<object>();
                SelectedPerson = new List<object>();
                SelectedBussinessUnit = new List<object>();
                SelectedOrigin = new List<object>();
                SelectedDepartment = new List<object>();
                SelectedActionPlansCodeForTask = new List<object>();
                SelectedLocationForTask = new List<object>();
                SelectedPersonForTask = new List<object>();
                SelectedBussinessUnitForTask = new List<object>();
                SelectedOriginForTask = new List<object>();

                IsActionPlanAllLocationSelected = false;
                IsActionPlanAllResponsibleSelected = false;
                IsActionPlanAllBUSelected = false;
                IsActionPlanAllOriginSelected = false;
                IsActionPlanAllDepartmentSelected = false;
                IsActionPlanAllActionPlanSelected = false;
                IsActionPlanAllActionPlanSelectedForTask = false;
                IsActionPlanAllLocationSelectedForTask = false;
                IsActionPlanAllResponsibleSelectedForTask = false;
                IsActionPlanAllBUSelectedForTask = false;
                IsActionPlanAllOriginSelectedForTask = false;
                IsActionPlanAllCustomerSelected = false;
                IsExpand = false;


                var existingLocationIds = APMCommon.Instance.LocationList.Select(location => location.IdCompany).ToHashSet();


                //            var missingIds = ActionPlanList
                //.SelectMany(actionPlan =>
                //    new[] { actionPlan.IdCompany }
                //    .Concat(actionPlan.TaskList.Select(task => task.IdCompany)) // Include TaskList IdCompany
                //)
                //.Where(idCompany => !existingLocationIds.Contains(idCompany))
                //.Distinct() // Ensure distinct values
                //.ToList();

                var missingIds = (ActionPlanList ?? new ObservableCollection<APMActionPlan>()) // Ensure ActionPlanList is not null
  .SelectMany(actionPlan =>
      new[] { actionPlan.IdCompany } // Include ActionPlan IdCompany
      .Concat(actionPlan.TaskList != null
          ? actionPlan.TaskList.Where(task => task != null).Select(task => task.IdCompany)
          : Enumerable.Empty<int>()) // Handle null TaskList and null tasks safely
  )
  .Where(idCompany => existingLocationIds != null && !existingLocationIds.Contains(idCompany))
  .Distinct()
  .ToList();

                // Join the distinct IDs into a comma-separated string
                var missingIdsString = string.Join(",", missingIds);

                if (!string.IsNullOrEmpty(missingIdsString) && missingIdsString != "")
                {
                    var temp = APMService.GetUnAuthorizedLocationListByIdCompany_V2590(missingIdsString);

                    if (temp != null)
                    {
                        List<Company> locationData = new List<Company>(APMCommon.Instance.LocationList);
                        locationData.AddRange(temp);
                        APMCommon.Instance.LocationList = new List<Company>(locationData);

                    }
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedYearChangedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectedYearChangedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedYearChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5974]
        private void RefreshButtonCommandAction(object obj)
        {
            ListOfFilterTile = new ObservableCollection<TileBarFilters>();
            Init();
            TableView detailView = (TableView)obj;
            detailView.SearchString = string.Empty;
            if (IsTaskGridVisibility)
            {
                SelectedAlertTileBarItem = new APMAlertTileBarFilters();
                MyTaskFilterString = string.Empty;
                //FilterCommandActionForTask(obj);
            }
            //else
            //{
            //    FilterCommandAction(obj);
            //}
            if (ListOfFilterTile != null)
            {
                SelectedTileBarItem = ListOfFilterTile.FirstOrDefault();
            }
            SelectedLocation = new List<object>();
            SelectedPerson = new List<object>();
            SelectedBussinessUnit = new List<object>();
            SelectedOrigin = new List<object>();
            SelectedDepartment = new List<object>();
            SelectedActionPlansCodeForTask = new List<object>();
            SelectedLocationForTask = new List<object>();
            SelectedPersonForTask = new List<object>();
            SelectedBussinessUnitForTask = new List<object>();
            SelectedOriginForTask = new List<object>();

            IsActionPlanAllLocationSelected = false;
            IsActionPlanAllResponsibleSelected = false;
            IsActionPlanAllBUSelected = false;
            IsActionPlanAllOriginSelected = false;
            IsActionPlanAllDepartmentSelected = false;
            IsActionPlanAllActionPlanSelected = false;
            IsActionPlanAllActionPlanSelectedForTask = false;
            IsActionPlanAllLocationSelectedForTask = false;
            IsActionPlanAllResponsibleSelectedForTask = false;
            IsActionPlanAllBUSelectedForTask = false;
            IsActionPlanAllOriginSelectedForTask = false;
            IsActionPlanAllCustomerSelected = false;

            var existingLocationIds = APMCommon.Instance.LocationList.Select(location => location.IdCompany).ToHashSet();


            //            var missingIds = ActionPlanList
            //.SelectMany(actionPlan =>
            //    new[] { actionPlan.IdCompany }
            //    .Concat(actionPlan.TaskList.Select(task => task.IdCompany)) // Include TaskList IdCompany
            //)
            //.Where(idCompany => !existingLocationIds.Contains(idCompany))
            //.Distinct() // Ensure distinct values
            //.ToList();

            var missingIds = (ActionPlanList ?? new ObservableCollection<APMActionPlan>()) // Ensure ActionPlanList is not null
  .SelectMany(actionPlan =>
      new[] { actionPlan.IdCompany } // Include ActionPlan IdCompany
      .Concat(actionPlan.TaskList != null
          ? actionPlan.TaskList.Where(task => task != null).Select(task => task.IdCompany)
          : Enumerable.Empty<int>()) // Handle null TaskList and null tasks safely
  )
  .Where(idCompany => existingLocationIds != null && !existingLocationIds.Contains(idCompany))
  .Distinct()
  .ToList();

            // Join the distinct IDs into a comma-separated string
            var missingIdsString = string.Join(",", missingIds);

            if (!string.IsNullOrEmpty(missingIdsString) && missingIdsString != "")
            {
                var temp = APMService.GetUnAuthorizedLocationListByIdCompany_V2590(missingIdsString);

                if (temp != null)
                {
                    List<Company> locationData = new List<Company>(APMCommon.Instance.LocationList);
                    locationData.AddRange(temp);
                    APMCommon.Instance.LocationList = new List<Company>(locationData);

                }
            }

        }
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);


                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new System.Windows.Media.SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }


                //[shweta.thube][GEOS2-5981]
                List<Tuple<string, string>> userConfigurations = new List<Tuple<string, string>>();
                if (GeosApplication.Instance.UserSettings.ContainsKey("APMActionPlan_IsFileDeleted_V2570"))
                {
                    if (GeosApplication.Instance.UserSettings["APMActionPlan_IsFileDeleted_V2570"].ToString() == "0")
                    {
                        if (File.Exists(@ActionPlanGridSettingFilePath))
                        {
                            File.Delete(@ActionPlanGridSettingFilePath);
                            GeosApplication.Instance.UserSettings["APMActionPlan_IsFileDeleted_V2570"] = "1";
                            foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                            {
                                userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            }
                            ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        }
                    }
                }


                //[Sudhir.jangra] 

                if (GeosApplication.Instance.UserSettings.ContainsKey("APMActionPlanChild_IsFileDeleted_V2590"))
                {
                    if (GeosApplication.Instance.UserSettings["APMActionPlanChild_IsFileDeleted_V2590"].ToString() == "0")
                    {
                        if (File.Exists(ActionPlanChildGridSettingFilePath))
                        {
                            File.Delete(ActionPlanChildGridSettingFilePath);
                            GeosApplication.Instance.UserSettings["APMActionPlanChild_IsFileDeleted_V2590"] = "1";
                            foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                            {
                                userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            }
                            ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        }
                    }
                }

                //[Sudhir.jangra] 

                if (GeosApplication.Instance.UserSettings.ContainsKey("APMActionPlanTask_IsFileDeleted_V2620"))
                {
                    if (GeosApplication.Instance.UserSettings["APMActionPlanTask_IsFileDeleted_V2620"].ToString() == "0")
                    {
                        if (File.Exists(ActionPlanTaskGridSettingFilePath))
                        {
                            File.Delete(ActionPlanTaskGridSettingFilePath);
                            GeosApplication.Instance.UserSettings["APMActionPlanTask_IsFileDeleted_V2620"] = "1";
                            foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                            {
                                userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            }
                            ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        }
                    }
                }

                GeosApplication.Instance.FillFinancialYear();
                if (APMCommon.Instance.SelectedPeriod == null)
                {
                    APMCommon.Instance.SelectedPeriod = new List<object>();
                }
                //if (APMCommon.Instance.SelectedPeriod?.Count > 0)
                //{
                //    if (APMCommon.Instance.SelectedPeriod.Any(i => (long)i != DateTime.Now.Year) && GeosApplication.Instance.FinancialYearLst != null)
                //    {
                //        var selectedYear = GeosApplication.Instance.FinancialYearLst.FirstOrDefault(x => x == DateTime.Now.Year);

                //        if (selectedYear != 0)
                //        {
                //            APMCommon.Instance.SelectedPeriod.Clear(); // Clear existing items in the list
                //            APMCommon.Instance.SelectedPeriod.Add(selectedYear); // Add the selected year as object
                //        }
                //    }
                //}

                FillLocationList();
                InvalidateActionPlanCache();
                InvalidateTaskCache();
                FillActionPlan();
                FillTaskGrid();
                FillBusinessUnitList();
                FillOriginList();

                FillResponsibleList();
                FillLeftTileList();
                if (!IsTaskGridVisibility)
                {
                    AddCustomSetting();
                    SelectedTopTileBarItem = TopListOfFilterTile.FirstOrDefault();
                }
                else
                {
                    AddTaskCustomSetting();
                    SelectedTaskTopTileBarItem = TopTaskListOfFilterTile.FirstOrDefault();
                }
                FillStatus(); // [nsatpute][09-10-2024][GEOS2-5975]
                              //[GEOS2-6496][27.09.2024][rdixit]
                SelectedLocation = null;
                SelectedOrigin = null;
                SelectedPerson = null;
                SelectedBussinessUnit = null;
                SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == "All");
                MyFilterString = string.Empty;
                MyChildFilterString = string.Empty;
                MyTaskFilterString = string.Empty;



                FillDepartmentList();//[Sudhir.jangra][GEOS2-6596]

                var existingLocationIds = APMCommon.Instance.LocationList.Select(location => location.IdCompany).ToHashSet();


                //            var missingIds = ActionPlanList
                //.SelectMany(actionPlan =>
                //    new[] { actionPlan.IdCompany }
                //    .Concat(actionPlan.TaskList.Select(task => task.IdCompany)) // Include TaskList IdCompany
                //)
                //.Where(idCompany => !existingLocationIds.Contains(idCompany))
                //.Distinct() // Ensure distinct values
                //.ToList();
                var missingIds = (ActionPlanList ?? new ObservableCollection<APMActionPlan>()) // Ensure ActionPlanList is not null
    .SelectMany(actionPlan =>
        new[] { actionPlan.IdCompany } // Include ActionPlan IdCompany
        .Concat(actionPlan.TaskList != null
            ? actionPlan.TaskList.Where(task => task != null).Select(task => task.IdCompany)
            : Enumerable.Empty<int>()) // Handle null TaskList and null tasks safely
    )
    .Where(idCompany => existingLocationIds != null && !existingLocationIds.Contains(idCompany))
    .Distinct()
    .ToList();

                // Join the distinct IDs into a comma-separated string
                var missingIdsString = string.Join(",", missingIds);

                if (!string.IsNullOrEmpty(missingIdsString) && missingIdsString != "")
                {
                    var temp = APMService.GetUnAuthorizedLocationListByIdCompany_V2590(missingIdsString);

                    if (temp != null)
                    {
                        List<Company> locationData = new List<Company>(APMCommon.Instance.LocationList);
                        locationData.AddRange(temp);
                        APMCommon.Instance.LocationList = new List<Company>(locationData);

                    }
                }

                FillCustomerList();//[shweta.thube][GEOS2-6911]
                FillPriorityList();//[Pallavi.Kale][GEOS2 - 8216]
                FillThemeList();//[Pallavi.Kale][GEOS2 - 8216]
                FillActionPlansAlertSectionList(); //[GEOS2-7217][shweta.thube][28.07.2024]
                FillCustomerListForTask();//[pallavi.kale][GEOS2-8084][07.08.2025]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //private void FillActionPlan()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillActionPlan ...", category: Category.Info, priority: Priority.Low);
        //        if (APMCommon.Instance.SelectedPeriod != null)
        //        {
        //            // ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2550(APMCommon.Instance.SelectedPeriod));
        //            //[Sudhir.Jangra][GEOS2-5972]
        //            //  ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2560(APMCommon.Instance.SelectedPeriod));
        //            //[Sudhir.Jangra][GEOS2-5982][Related TO performance there is no task and i have time in this task that's why i am covering in this]
        //            //ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2570(APMCommon.Instance.SelectedPeriod));

        //            //[Sudhir.Jangra][GEOS2-5976]
        //            string idPeriods = string.Empty;
        //            if (APMCommon.Instance.SelectedPeriod != null)
        //            {
        //                List<long> selectedPeriod = APMCommon.Instance.SelectedPeriod.Cast<long>().ToList();
        //                idPeriods = string.Join(",", selectedPeriod);
        //            }
        //            string idLocations = string.Empty;
        //            if (APMCommon.Instance.LocationList != null)
        //            {
        //                idLocations = string.Join(",", APMCommon.Instance.LocationList.Select(x => x.IdCompany.ToString()));
        //            }

        //            //ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2570(idPeriods));
        //            //[Sudhir.Jangra][GEOS2-6016][Changed FIle Count as per IsDeleted]
        //            //ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2580(idPeriods,idLocations));

        //            // ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2590(idPeriods, idLocations));//[shweta.thube][GEOS2-6589]

        //            //[Sudhir.Jangra] [GEOS2-6697]
        //            //ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2590(idPeriods, GeosApplication.Instance.ActiveUser.IdUser));

        //            //[shweta.thube][GEOS2-6453]
        //            //ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2600(idPeriods, GeosApplication.Instance.ActiveUser.IdUser));
        //            //[Sudhir.Jangra][GEOS2-6616]
        //            //ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2610(idPeriods, GeosApplication.Instance.ActiveUser.IdUser));

        //            //[shweta.thube][GEOS2-6912]
        //            // ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2620(idPeriods, GeosApplication.Instance.ActiveUser.IdUser));
        //            //[Sudhir.Jangra][GEOS2-7209]
        //            //ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2630(idPeriods, GeosApplication.Instance.ActiveUser.IdUser));

        //            //[shweta.thube][GEOS2-7212][23.04.2025]
        //            // IAPMService APMService = new APMServiceController("localhost:6699");
        //            // ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2640(idPeriods, GeosApplication.Instance.ActiveUser.IdUser));

        //            //[pallavi.kale][GEOS2-7002][19.06.2025]
        //            //APMService = new APMServiceController("localhost:6699");
        //            //ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2650(idPeriods, GeosApplication.Instance.ActiveUser.IdUser));
        //            //[shweta.thube][GEOS2-7217][23.07.2025]
        //            //ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2660(idPeriods, GeosApplication.Instance.ActiveUser.IdUser));
        //            //[shweta.thube][GEOS2-9237][14.08.2025]
        //            //ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2670(idPeriods, GeosApplication.Instance.ActiveUser.IdUser));
        //            ActionPlanList = new ObservableCollection<APMActionPlan>(APMService.GetActionPlanDetails_V2680(idPeriods, GeosApplication.Instance.ActiveUser.IdUser));//[pallavi.kale][GEOS2-8995][28.10.2025]

        //            //[Sudhir.Jangra][GEOS2-6453]
        //            //TaskGridList = new ObservableCollection<APMActionPlanTask>(APMService.GetActionPlanTaskDetails_V2600(idPeriods, GeosApplication.Instance.ActiveUser.IdUser));
        //    }
        //        //[shweta.thube][GEOS2-6795]
        //        foreach (APMActionPlan item in ActionPlanList)
        //        {
        //            if (item.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser || GeosApplication.Instance.IsAPMActionPlanPermission)
        //                item.IsActionPlanDeleted = true;
        //            else
        //                item.IsActionPlanDeleted = false;
        //            //[shweta.thube][GEOS2-9910][16.10.2025]
        //            if (item.TaskList != null && item.TaskList.Count > 0)
        //            {
        //                foreach (APMActionPlanTask task in item.TaskList)
        //                {
        //                    if (task.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser ||
        //                        item.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser ||
        //                        GeosApplication.Instance.IsAPMActionPlanPermission)
        //                    {
        //                        task.IsTaskDeleted = true;
        //                    }
        //                    else
        //                    {
        //                        task.IsTaskDeleted = false;
        //                    }

        //                    if (task.SubTaskList != null && task.SubTaskList.Count > 0)
        //                    {
        //                        foreach (APMActionPlanSubTask subtask in task.SubTaskList)
        //                        {
        //                            subtask.IsSubTaskDeleted = (
        //                                subtask.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser ||
        //                                task.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser ||
        //                                item.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser ||
        //                                GeosApplication.Instance.IsAPMActionPlanPermission
        //                            );
        //                        }
        //                    }

        //                }
        //            }

        //        }
        //        //[shweta.thube][GEOS2-6453]
        //        if (ActionPlanList.Count > 0 && ActionPlanCodeList == null)
        //        {
        //            ActionPlanCodeList = new List<APMActionPlan>(ActionPlanList.OrderBy(ap => ap.Code));
        //        }
        //        //foreach (var item in ActionPlanList.GroupBy(tpa => tpa.Country.Iso))
        //        //{
        //        //    ImageSource countryFlagImage = ByteArrayToBitmapImage(item.ToList().FirstOrDefault().Country.CountryIconBytes);
        //        //    item.ToList().Where(oti => oti.Country.Iso == item.Key).ToList().ForEach(oti => oti.Country.CountryIconImage = countryFlagImage);
        //        //}

        //        //[shweta.thube][GEOS2-6453]
        //        //TempActionPlanList = new List<APMActionPlan>(ActionPlanList);
        //        TempActionPlanList = new List<APMActionPlan>();


        //        if (ListOfPersonForTask == null)
        //        {
        //            ListOfPersonForTask = new ObservableCollection<Responsible>();
        //        }
        //        if (ActionPlanList.Count() != 0)
        //        {
        //            foreach (var actionPlan in ActionPlanList)
        //            {
        //                TempActionPlanList.Add(actionPlan);
        //                if (actionPlan.TaskList != null)
        //                {
        //                    foreach (var task in actionPlan.TaskList)
        //                    {
        //                        // TaskGridList.Add(task);

        //                        if (!ListOfPersonForTask.Any(person => person.EmployeeCode == task.EmployeeCode))
        //                        {
        //                            ListOfPersonForTask.Add(new Responsible()
        //                            {
        //                                IdEmployee = (UInt32)task.IdEmployee,
        //                                EmployeeCode = task.EmployeeCode,
        //                                IdGender = task.IdGender,
        //                                FullName = task.Responsible,
        //                                EmployeeCodeWithIdGender = task.EmployeeCode + "_" + task.IdGender,
        //                                IsTaskField = true,
        //                                ResponsibleDisplayName = task.TaskResponsibleDisplayName
        //                            });
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //        try
        //        {
        //            //[shweta.thube][GEOS2-6453]
        //            if (TempActionPlanList.Count > 0 && TempActionPlanList != null)
        //            {
        //                // Create a distinct list based on IdActionPlan
        //                ActionPlanCodeList = TempActionPlanList.Where(ap => ap.TaskList != null) // Ensure TaskList is not null
        //                                        .GroupBy(ap => ap.IdActionPlan)  // Group by IdActionPlan
        //                                        .Select(g => g.OrderBy(ap => ap.Code).First()) // Order by Code and select the first item
        //                                        .OrderBy(ap => ap.Code) // Order the final list by Code
        //                                        .ToList();                    // Convert the result to a list
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            GeosApplication.Instance.Logger.Log("Get an error in Method FillActionPlan()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        }


        //        if (ActionPlanList != null && ActionPlanList.Count > 0)//[Sudhir.Jangra][GEOS2-6789]
        //        {
        //            ClonedActionPlanList = new List<APMActionPlan>();
        //            foreach (APMActionPlan item in ActionPlanList)
        //            {
        //                ClonedActionPlanList.Add((APMActionPlan)item.Clone());
        //            }

        //        }

        //        GeosApplication.Instance.Logger.Log("Method FillActionPlan() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillActionPlan() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillActionPlan() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in Method FillActionPlan()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        //[GEOS2-5971][shweta.thube][1.07.2024]
        private void FillLeftTileList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLeftTileList ...", category: Category.Info, priority: Priority.Low);
                //APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                IList<LookupValue> temp = APMService.GetLookupValues_V2550(155).ToList();//TaskThemes

                temp.OrderBy(x => x.Position);

                ListOfFilterTile = new ObservableCollection<TileBarFilters>();
                if (!IsTaskGridVisibility)
                {
                    if (!IsActionPlanEdit)
                    {
                        ListOfFilterTile.Add(
                        new TileBarFilters()

                        {
                            Caption = "All",
                            Id = 0,
                            BackColor = null,
                            EntitiesCount = ActionPlanList?.Sum(actionPlan => actionPlan.TaskList?.Count() ?? 0) ?? 0,
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 60,
                            width = 230
                        });

                        foreach (var item in temp)
                        {
                            ListOfFilterTile.Add(new TileBarFilters()
                            {
                                Caption = item.Value,
                                Id = item.IdLookupValue,
                                BackColor = item.HtmlColor,
                                ForeColor = null,
                                FilterCriteria = $"[Theme] IN ('{item.Value}')",
                                EntitiesCount = ActionPlanList
                                    .SelectMany(a => a.TaskList != null ? a.TaskList.AsEnumerable() : Enumerable.Empty<APMActionPlanTask>()) // Convert to IEnumerable<Task>
                                    .Count(b => b.IdLookupTheme == item.IdLookupValue),
                                EntitiesCountVisibility = Visibility.Visible,
                                Height = 60,
                                width = 230
                            });
                        }
                    }
                    else
                    {
                        ListOfFilterTile.Add(
                        new TileBarFilters()

                        {
                            Caption = "All",
                            Id = 0,
                            BackColor = null,
                            EntitiesCount = ClonedActionPlanList?.Sum(actionPlan => actionPlan.TaskList?.Count() ?? 0) ?? 0,
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 60,
                            width = 230
                        });

                        foreach (var item in temp)
                        {
                            ListOfFilterTile.Add(new TileBarFilters()
                            {
                                Caption = item.Value,
                                Id = item.IdLookupValue,
                                BackColor = item.HtmlColor,
                                ForeColor = null,
                                FilterCriteria = $"[Theme] IN ('{item.Value}')",
                                EntitiesCount = ClonedActionPlanList
                                    .SelectMany(a => a.TaskList != null ? a.TaskList.AsEnumerable() : Enumerable.Empty<APMActionPlanTask>()) // Convert to IEnumerable<Task>
                                    .Count(b => b.IdLookupTheme == item.IdLookupValue),
                                EntitiesCountVisibility = Visibility.Visible,
                                Height = 60,
                                width = 230
                            });
                        }

                    }



                }
                else
                {

                    if (!IsActionPlanEdit)
                    {
                        ListOfFilterTile.Add(
                    new TileBarFilters()
                    {
                        Caption = "All",
                        Id = 0,
                        BackColor = null,
                        EntitiesCount = TaskGridList?.Count() ?? 0,
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 60,
                        width = 230
                    });

                        foreach (var item in temp)
                        {
                            ListOfFilterTile.Add(new TileBarFilters()
                            {
                                Caption = item.Value,
                                Id = item.IdLookupValue,
                                BackColor = item.HtmlColor,
                                ForeColor = null,
                                FilterCriteria = $"[Theme] IN ('{item.Value}')",
                                EntitiesCount = TaskGridList
        .Count(b => b.IdLookupTheme == item.IdLookupValue),
                                EntitiesCountVisibility = Visibility.Visible,
                                Height = 60,
                                width = 230
                            });
                        }
                    }
                    else
                    {
                        ListOfFilterTile.Add(
                   new TileBarFilters()
                   {
                       Caption = "All",
                       Id = 0,
                       BackColor = null,
                       EntitiesCount = ClonedTaskGridList?.Count() ?? 0,
                       EntitiesCountVisibility = Visibility.Visible,
                       Height = 60,
                       width = 230
                   });

                        foreach (var item in temp)
                        {
                            ListOfFilterTile.Add(new TileBarFilters()
                            {
                                Caption = item.Value,
                                Id = item.IdLookupValue,
                                BackColor = item.HtmlColor,
                                ForeColor = null,
                                FilterCriteria = $"[Theme] IN ('{item.Value}')",
                                EntitiesCount = ClonedTaskGridList
        .Count(b => b.IdLookupTheme == item.IdLookupValue),
                                EntitiesCountVisibility = Visibility.Visible,
                                Height = 60,
                                width = 230
                            });
                        }
                    }
                }


                //ListOfFilterTile = new ObservableCollection<TileBarFilters>(ListOfFilterTile.Where(x => x.EntitiesCount != 0));
                //ListOfFilterTile = new ObservableCollection<TileBarFilters>(APMService.GetLookupValues_V2550(155));
                GeosApplication.Instance.Logger.Log("Method FillLeftTileList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeftTileList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLeftTileList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLeftTileList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillBusinessUnitList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillBusinessUnitList ...", category: Category.Info, priority: Priority.Low);
                if (APMCommon.Instance.BusinessUnitList == null)
                {
                    //APMCommon.Instance.BusinessUnitList = new List<LookupValue>(APMService.GetLookupValues_V2550(156));//TaskBusinessUnits
                    //APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    APMCommon.Instance.BusinessUnitList = new List<LookupValue>(APMService.GetLookupValues_V2550(2));//TaskBusinessUnits

                }
                GeosApplication.Instance.Logger.Log("Method FillBusinessUnitList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillBusinessUnitList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillOriginList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOriginList ...", category: Category.Info, priority: Priority.Low);
                if (APMCommon.Instance.OriginList == null)
                {
                    //APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    APMCommon.Instance.OriginList = new List<LookupValue>(APMService.GetLookupValues_V2550(154));//TaskOrigins
                }
                GeosApplication.Instance.Logger.Log("Method FillOriginList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOriginList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOriginList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOriginList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5971]
        private void FillLocationList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLocationList ...", category: Category.Info, priority: Priority.Low);
                // if (APMCommon.Instance.LocationList == null)
                // {
                //LocationList = new ObservableCollection<Company>(APMService.GetAuthorizedLocationListByIdUser_V2550(GeosApplication.Instance.ActiveUser.IdUser));
                //[Sudhir.Jangra][GEOS2-6397]
                // APMCommon.Instance.LocationList = new List<Company>(APMService.GetAuthorizedLocationListByIdUser_V2560(GeosApplication.Instance.ActiveUser.IdUser));

                //[Sudhir.Jangra][GEOS2-6015]
                APMCommon.Instance.LocationList = new List<Company>(APMService.GetAuthorizedLocationListByIdUser_V2570(GeosApplication.Instance.ActiveUser.IdUser));


                //foreach (var item in APMCommon.Instance.LocationList)
                //{
                //    ImageSource countryFlagImage = ByteArrayToBitmapImage(item.ImageInBytes);
                //    item.SiteImage = countryFlagImage;
                //}
                // }
                GeosApplication.Instance.Logger.Log("Method FillLocationList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLocationList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLocationList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLocationList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FilterCommandActionForTask(object obj)
        {
            try
            {
                // 1. Manter a lógica de clique nos Tiles (Custom Filters)
                if (obj is MouseButtonEventArgs mouseArgs)
                {
                    if (mouseArgs.Source is System.Windows.Controls.Control clickedControl &&
                        clickedControl.DataContext is TileBarFilters tile)
                    {
                        CustomFilterStringName = tile.Caption;
                    }
                }

                // 2. Atualizar as Flags visuais "Select All" / "Partial Selection"
                // (Isto garante que as caixas de seleção na UI ficam corretas)
                if (ListOfPersonForTask != null && SelectedPersonForTask != null)
                    IsActionPlanAllResponsibleSelectedForTask = SelectedPersonForTask.Count > 0 && SelectedPersonForTask.Count < ListOfPersonForTask.Count;
                else IsActionPlanAllResponsibleSelectedForTask = false;

                if (APMCommon.Instance.LocationList != null && SelectedLocationForTask != null)
                    IsActionPlanAllLocationSelectedForTask = SelectedLocationForTask.Count > 0 && SelectedLocationForTask.Count < APMCommon.Instance.LocationList.Count;
                else IsActionPlanAllLocationSelectedForTask = false;

                if (APMCommon.Instance.BusinessUnitList != null && SelectedBussinessUnitForTask != null)
                    IsActionPlanAllBUSelectedForTask = SelectedBussinessUnitForTask.Count > 0 && SelectedBussinessUnitForTask.Count < APMCommon.Instance.BusinessUnitList.Count;
                else IsActionPlanAllBUSelectedForTask = false;

                if (APMCommon.Instance.OriginList != null && SelectedOriginForTask != null)
                    IsActionPlanAllOriginSelectedForTask = SelectedOriginForTask.Count > 0 && SelectedOriginForTask.Count < APMCommon.Instance.OriginList.Count;
                else IsActionPlanAllOriginSelectedForTask = false;

                if (ListOfCustomerForTask != null && SelectedCustomerForTask != null)
                    IsActionPlanAllCustomerSelectedForTask = SelectedCustomerForTask.Count > 0 && SelectedCustomerForTask.Count < ListOfCustomerForTask.Count;
                else IsActionPlanAllCustomerSelectedForTask = false;


                // 3. A CORREÇÃO REAL: Usar ContextIdle
                // Usamos o Dispatcher para adiar a CHAMADA do método.
                // Assim, quando o 'ApplyInMemoryFiltersAsync' for executado, a propriedade SelectedPersonForTask já terá o valor novo.
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    // Chamamos sem delay interno, pois o Dispatcher já garantiu o timing correto
                    ApplyInMemoryFiltersAsync();
                }), System.Windows.Threading.DispatcherPriority.ContextIdle);

                // Log para confirmar que passou por aqui
                GeosApplication.Instance.Logger.Log("FilterCommandActionForTask: Delegated to ApplyInMemoryFiltersAsync", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("FilterCommandActionForTask error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }

        //[GEOS2-6496][27.09.2024][rdixit]
        private void FilterCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FilterCommandAction()...", category: Category.Info, priority: Priority.Low);
                LookupValue selectedBusinessUnit = null;
                LookupValue selectedOrigin = null;
                Responsible selectedPerson = null;
                Company selectedCompany = null;
                List<LookupValue> origin = null;
                List<Responsible> person = null;
                List<Company> plant = null;
                List<LookupValue> businessUnit = null;
                Department selectedDepartment = null; //[Sudhir.Jangra][GEOS2-6596]
                List<Department> department = null;//[Sudhir.Jangra][GEOS2-6596]
                List<APMCustomer> apmCustomer = null;//[shweta.thube][GEOS2-6911]
                APMCustomer selectedCustomer = null;//[shweta.thube][GEOS2-6911]
                #region
                if (SelectedOrigin != null)
                {
                    origin = SelectedOrigin.Cast<LookupValue>().ToList();
                    selectedOrigin = origin.FirstOrDefault();
                }
                //SelectedPerson
                if (SelectedPerson != null)
                {
                    person = SelectedPerson.Cast<Responsible>().ToList();
                    selectedPerson = person.FirstOrDefault();
                }
                //Location
                if (SelectedLocation != null)
                {
                    plant = SelectedLocation.Cast<Company>().ToList();
                    selectedCompany = plant.FirstOrDefault();
                }

                //BusinessUnit
                if (SelectedBussinessUnit != null)
                {
                    businessUnit = SelectedBussinessUnit.Cast<LookupValue>().ToList();
                    selectedBusinessUnit = businessUnit.FirstOrDefault();
                }
                //[Department]
                if (SelectedDepartment != null)
                {
                    department = SelectedDepartment.Cast<Department>().ToList();
                    selectedDepartment = department.FirstOrDefault();
                }
                //[shweta.thube][GEOS2-6911]
                //SelectedCustomer
                if (SelectedCustomer != null)
                {
                    apmCustomer = SelectedCustomer.Cast<APMCustomer>().ToList();
                    selectedCustomer = apmCustomer.FirstOrDefault();
                }
                #endregion

                #region GEOS2-6792
                if ((APMCommon.Instance.LocationList != null && APMCommon.Instance.LocationList.Count > 0) && (plant != null && plant.Count > 0)) // Location
                {
                    if (APMCommon.Instance.LocationList.Count == plant.Count)
                    {
                        IsActionPlanAllLocationSelected = false;
                    }
                    else
                    {
                        IsActionPlanAllLocationSelected = true;
                    }
                }
                else
                {
                    IsActionPlanAllLocationSelected = false;
                }

                if ((ListOfPerson != null && ListOfPerson.Count > 0) && (person != null && person.Count > 0)) //Responsible
                {
                    if (ListOfPerson.Count == person.Count)
                    {
                        IsActionPlanAllResponsibleSelected = false;
                    }
                    else
                    {
                        IsActionPlanAllResponsibleSelected = true;
                    }
                }
                else
                {
                    IsActionPlanAllResponsibleSelected = false;
                }

                if ((APMCommon.Instance.BusinessUnitList != null && APMCommon.Instance.BusinessUnitList.Count > 0) && (businessUnit != null && businessUnit.Count > 0)) //BusinessUnit
                {
                    if (APMCommon.Instance.BusinessUnitList.Count == businessUnit.Count)
                    {
                        IsActionPlanAllBUSelected = false;
                    }
                    else
                    {
                        IsActionPlanAllBUSelected = true;
                    }
                }
                else
                {
                    IsActionPlanAllBUSelected = false;
                }


                if ((APMCommon.Instance.OriginList != null && APMCommon.Instance.OriginList.Count > 0) && (origin != null && origin.Count > 0)) //Origin
                {
                    if (APMCommon.Instance.OriginList.Count == origin.Count)
                    {
                        IsActionPlanAllOriginSelected = false;
                    }
                    else
                    {
                        IsActionPlanAllOriginSelected = true;
                    }
                }
                else
                {
                    IsActionPlanAllOriginSelected = false;
                }


                if ((APMCommon.Instance.DepartmentList != null && APMCommon.Instance.DepartmentList.Count > 0) && (department != null && department.Count > 0)) //Department
                {
                    if (APMCommon.Instance.DepartmentList.Count == department.Count)
                    {
                        IsActionPlanAllDepartmentSelected = false;
                    }
                    else
                    {
                        IsActionPlanAllDepartmentSelected = true;
                    }
                }
                else
                {
                    IsActionPlanAllDepartmentSelected = false;
                }
                // Customer
                if ((ListOfCustomer != null && ListOfCustomer.Count > 0) && (apmCustomer != null && apmCustomer.Count > 0)) //Customer
                {
                    if (ListOfCustomer.Count == apmCustomer.Count)
                    {
                        IsActionPlanAllCustomerSelected = false;
                    }
                    else
                    {
                        IsActionPlanAllCustomerSelected = true;
                    }
                }
                else
                {
                    IsActionPlanAllCustomerSelected = false;
                }
                #endregion

                #region Custom filter
                // CustomFilterStringName = SelectedTileBarItem?.Caption;
                if (!IsTaskGridVisibility)
                {
                    if (obj is MouseButtonEventArgs)
                    {
                        MouseButtonEventArgs detailView = (MouseButtonEventArgs)obj;
                        if (detailView != null)
                        {
                            var tileBarItem = detailView.Source as DevExpress.Xpf.Navigation.TileBarItem;
                            if (tileBarItem != null)
                            {
                                var temp = tileBarItem.Content as Emdep.Geos.UI.Helper.TileBarFilters;
                                if (temp != null)
                                {
                                    CustomFilterStringName = temp.Caption;
                                }
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(CustomFilterStringName))
                    {
                        CustomFilterStringName = "All";
                    }

                    if (!string.IsNullOrEmpty(CustomFilterStringName) && CustomFilterStringName.Equals("All"))
                    {
                        InvalidateActionPlanCache();
                        InvalidateTaskCache();
                        FillActionPlan();

                        // ActionPlanList = new ObservableCollection<APMActionPlan>(ActionPlanList);
                    }
                    else
                    {
                        FillActionPlan();
                        ActionPlanList = new ObservableCollection<APMActionPlan>(TempActionPlanList.Where(x => x.TaskList != null && x.TaskList.Any(a => a.Theme == CustomFilterStringName)) // Filter plans where any task matches
                                                                  .Select(x =>
                                                                  {
                                                                      x.TaskList = new List<APMActionPlanTask>(
                                                                      x.TaskList.Where(a => a.Theme == CustomFilterStringName).ToList());
                                                                      return x;
                                                                  }));
                        SetDeletionFlags(ActionPlanList);
                        TaskGridList = new ObservableCollection<APMActionPlanTask>(
TempTaskGridList
.Where(a => a.Theme == CustomFilterStringName)  // Filter tasks based on the theme
.ToList()  // Convert the filtered tasks to a List
);
                    }
                }
                //[shweta.thube][GEOS2-6453]          
                else
                {
                    if (obj is MouseButtonEventArgs)
                    {
                        MouseButtonEventArgs TaskisGridTableView = (MouseButtonEventArgs)obj;
                        if (TaskisGridTableView != null)
                        {
                            var tileBarItem = TaskisGridTableView.Source as DevExpress.Xpf.Navigation.TileBarItem;
                            if (tileBarItem != null)
                            {
                                var temp = tileBarItem.Content as Emdep.Geos.UI.Helper.TileBarFilters;
                                if (temp != null)
                                {
                                    CustomFilterStringName = temp.Caption;
                                }
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(CustomFilterStringName))
                    {
                        CustomFilterStringName = "All";
                    }

                    if (CustomFilterStringName == "All")
                        TaskGridList = new ObservableCollection<APMActionPlanTask>(TempTaskGridList);
                    else
                        TaskGridList = new ObservableCollection<APMActionPlanTask>(
                            TempTaskGridList.Where(t => t.Theme == CustomFilterStringName).ToList());
                }
                #endregion

                #region  selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null&& selectedDepartment == null && selectedCustomer == null
                if (selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer == null)
                {
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == CustomFilterStringName);
                }
                #endregion

                #region selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null&& selectedCustomer == null
                else if (selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer == null)
                {
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();
                    var filteredActionPlans = ActionPlanList
                        .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                        .ToList();

                    // Update ActionPlanList with the filtered results
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionPlans);
                    SetDeletionFlags(ActionPlanList);
                    if (ActionPlanList.Count() != 0)
                    {
                        foreach (var actionPlan in ActionPlanList)
                        {
                            if (actionPlan.TaskList != null)
                            {
                                foreach (var task in actionPlan.TaskList)
                                {
                                    TaskGridList.Add(task);

                                }
                            }
                        }
                    }
                }
                #endregion

                #region selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null&& selectedDepartment == null && selectedCustomer == null
                else if (selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer == null)
                {

                    var filteredAction = ActionPlanList.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredAction);
                    SetDeletionFlags(ActionPlanList);
                }
                #endregion

                #region selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer == null
                else if (selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer == null)
                {
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();
                    var filteredActionPlans = ActionPlanList
                        .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                        .ToList();



                    var filteredAction = filteredActionPlans.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredAction);
                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null&& selectedCustomer == null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer == null)
                {


                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();

                    // Filter ActionPlanList where TaskList contains tasks matching the BusinessUnit values
                    var filteredActionPlans = ActionPlanList
                         .Where(actionPlan => businessUnit.Any(company => company.Value == actionPlan.BusinessUnit)).ToList();
                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionPlant);
                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer == null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer == null)
                {

                    var filteredActionPlan = ActionPlanList.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionPlan);
                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null&& selectedCustomer == null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer == null)
                {

                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();
                    var filteredActionPlans = ActionPlanList
                        .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                        .ToList();

                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredAction = filteredActionPlant.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredAction);
                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer == null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer == null)
                {

                    var filteredActionPlan = ActionPlanList.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredAction = filteredActionPlan.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredAction);
                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer == null)
                {

                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredAction = filteredActionPlans.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredAction);
                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer == null)
                {

                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();



                    var filteredActionPlan = ActionPlanList
                        .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                        .ToList();

                    var filteredAction = filteredActionPlan.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in filteredAction)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionPlans);
                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer == null)
                {

                    var filteredActionPlans = new List<APMActionPlan>();

                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }

                    // Update the ActionPlanList with filtered and merged results
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionPlans);
                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer == null)
                {

                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();

                    var filteredActionPlan = ActionPlanList
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();

                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in filteredActionPlan)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionPlans);
                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer == null)
                {

                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredActionPlan = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionPlan);
                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer == null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();


                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();
                    var filteredActionBusiness = filteredActionPlant
                        .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                        .ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionBusiness);
                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer == null)
                {
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();


                    var filteredActionPlan = ActionPlanList
                        .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                        .ToList();
                    var filteredAction = filteredActionPlan.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filteredActionPlant = filteredAction.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in filteredActionPlant)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionPlans);
                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }

                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredAction = filteredActionPlant.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();

                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();


                    var filteredActionPlan = filteredAction
                        .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                        .ToList();

                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filterDepartment);

                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer == null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredActionPlant = ActionPlanList.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredAction = filteredActionPlant.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();

                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();


                    var filteredActionPlan = filteredAction
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();


                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filterDepartment);

                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }

                    var filteredAction = filteredActionPlans.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();


                    var filteredActionPlan = filteredAction
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();


                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filterDepartment);


                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer == null 
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }

                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();



                    var filteredActionPlan = filteredActionPlant
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();


                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filterDepartment);


                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }

                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredAction = filteredActionPlant.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filterDepartment = filteredAction.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filterDepartment);

                }
                #endregion

                #region selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer == null
                else if (selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredAction = ActionPlanList.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();



                    var filteredActionPlan = filteredAction
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();


                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filterDepartment);

                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null  && selectedCustomer == null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredActionPlant = ActionPlanList.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();

                    var filteredActionPlan = filteredActionPlant
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();



                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filterDepartment);

                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null  && selectedCustomer == null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredActionPlant = ActionPlanList.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredAction = filteredActionPlant.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();

                    var filterDepartment = filteredAction.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filterDepartment);
                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null  && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();


                    var filteredActionPlan = filteredActionPlans
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();


                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filterDepartment);


                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null  && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredAction = filteredActionPlans.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filterDepartment = filteredAction.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filterDepartment);

                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null  && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filterDepartment = filteredActionPlant.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filterDepartment);


                }
                #endregion

                #region selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null  && selectedCustomer == null
                else if (selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer == null)
                {
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();

                    var filteredActionPlan = ActionPlanList
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();


                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filterDepartment);

                }
                #endregion

                #region selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null  && selectedCustomer == null
                else if (selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredAction = ActionPlanList.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filterDepartment = filteredAction.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filterDepartment);
                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null  && selectedCustomer == null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredActionPlant = ActionPlanList.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filterDepartment = filteredActionPlant.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filterDepartment);
                }
                #endregion

                #region selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null  && selectedCustomer == null
                else if (selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredActionPlan = ActionPlanList.Where(actionPlan => department.Any(company => company.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionPlan);
                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null  && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer == null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredActionPlan = filteredActionPlans.Where(actionPlan => department.Any(company => company.DepartmentName == actionPlan.Department)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionPlan);
                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment == null  && selectedCustomer == null
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer == null)
                {

                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredAction = filteredActionPlant.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredAction);
                }

                #endregion

                //[shweta.thube][GEOS2-6911]

                #region  selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null&& selectedDepartment == null && selectedCustomer != null
                if (selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer != null)
                {
                    var filteredCustomers = ActionPlanList
               .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null&& selectedCustomer != null
                else if (selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer != null)
                {
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();
                    var filteredActionPlans = ActionPlanList
                        .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                        .ToList();

                    var filteredCustomers = filteredActionPlans
              .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                    if (ActionPlanList.Count() != 0)
                    {
                        foreach (var actionPlan in ActionPlanList)
                        {
                            if (actionPlan.TaskList != null)
                            {
                                foreach (var task in actionPlan.TaskList)
                                {
                                    TaskGridList.Add(task);

                                }
                            }
                        }
                    }
                }
                #endregion

                #region selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null&& selectedDepartment == null && selectedCustomer != null
                else if (selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer != null)
                {

                    var filteredAction = ActionPlanList.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filteredCustomers = filteredAction
              .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer != null
                else if (selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer != null)
                {
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();
                    var filteredActionPlans = ActionPlanList
                        .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                        .ToList();



                    var filteredAction = filteredActionPlans.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filteredCustomers = filteredAction
              .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null&& selectedCustomer != null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer != null)
                {


                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();

                    // Filter ActionPlanList where TaskList contains tasks matching the BusinessUnit values
                    var filteredActionPlans = ActionPlanList
                         .Where(actionPlan => businessUnit.Any(company => company.Value == actionPlan.BusinessUnit)).ToList();
                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredCustomers = filteredActionPlant
              .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer != null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer != null)
                {

                    var filteredActionPlan = ActionPlanList.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredCustomers = filteredActionPlan
              .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null&& selectedCustomer != null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer != null)
                {

                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();
                    var filteredActionPlans = ActionPlanList
                        .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                        .ToList();

                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredAction = filteredActionPlant.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filteredCustomers = filteredAction
              .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);

                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer != null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer != null)
                {

                    var filteredActionPlan = ActionPlanList.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredAction = filteredActionPlan.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filteredCustomers = filteredAction
              .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer != null)
                {

                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredAction = filteredActionPlans.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filteredCustomers = filteredAction
              .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer != null)
                {

                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();



                    var filteredActionPlan = ActionPlanList
                        .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                        .ToList();

                    var filteredAction = filteredActionPlan.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in filteredAction)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredCustomers = filteredActionPlans
              .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer != null)
                {

                    var filteredActionPlans = new List<APMActionPlan>();

                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }

                    var filteredCustomers = filteredActionPlans
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer != null)
                {

                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();

                    var filteredActionPlan = ActionPlanList
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();

                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in filteredActionPlan)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredCustomers = filteredActionPlans
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer != null)
                {

                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredActionPlan = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredCustomers = filteredActionPlan
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer != null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();


                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();
                    var filteredActionBusiness = filteredActionPlant
                        .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                        .ToList();
                    var filteredCustomers = filteredActionBusiness
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);

                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment == null && selectedCustomer != null)
                {
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();


                    var filteredActionPlan = ActionPlanList
                        .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                        .ToList();
                    var filteredAction = filteredActionPlan.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filteredActionPlant = filteredAction.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in filteredActionPlant)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredCustomers = filteredActionPlans
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }

                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredAction = filteredActionPlant.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();

                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();


                    var filteredActionPlan = filteredAction
                        .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                        .ToList();

                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filterDepartment
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);

                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer != null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredActionPlant = ActionPlanList.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredAction = filteredActionPlant.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();

                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();


                    var filteredActionPlan = filteredAction
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();


                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filterDepartment
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);

                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }

                    var filteredAction = filteredActionPlans.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();


                    var filteredActionPlan = filteredAction
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();


                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filterDepartment
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);


                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer != null 
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }

                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();



                    var filteredActionPlan = filteredActionPlant
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();


                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filterDepartment
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);


                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }

                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredAction = filteredActionPlant.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filterDepartment = filteredAction.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filterDepartment
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);

                }
                #endregion

                #region selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer != null
                else if (selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredAction = ActionPlanList.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();



                    var filteredActionPlan = filteredAction
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();


                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filterDepartment
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);

                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null  && selectedCustomer != null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredActionPlant = ActionPlanList.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();

                    var filteredActionPlan = filteredActionPlant
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();



                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filterDepartment
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);

                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null  && selectedCustomer != null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredActionPlant = ActionPlanList.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredAction = filteredActionPlant.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();

                    var filterDepartment = filteredAction.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filterDepartment
                 .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null  && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();


                    var filteredActionPlan = filteredActionPlans
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();


                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filterDepartment
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);


                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null  && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredAction = filteredActionPlans.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filterDepartment = filteredAction.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filterDepartment
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);

                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null  && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filterDepartment = filteredActionPlant.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();

                    var filteredCustomers = filterDepartment
                .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);


                }
                #endregion

                #region selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null  && selectedCustomer != null
                else if (selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit != null && selectedDepartment != null && selectedCustomer != null)
                {
                    // Get all unique BusinessUnit values from the provided businessUnit list
                    var businessUnitValues = businessUnit.Select(bu => bu.Value).Distinct().ToList();

                    var filteredActionPlan = ActionPlanList
                       .Where(a => businessUnitValues.Contains(a.BusinessUnit)) // Filter based on BusinessUnit
                       .ToList();


                    var filterDepartment = filteredActionPlan.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filterDepartment
                 .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);

                }
                #endregion

                #region selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null  && selectedCustomer != null
                else if (selectedPerson == null && selectedCompany == null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredAction = ActionPlanList.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filterDepartment = filteredAction.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filterDepartment
                  .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null  && selectedCustomer != null
                else if (selectedPerson == null && selectedCompany != null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredActionPlant = ActionPlanList.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filterDepartment = filteredActionPlant.Where(actionPlan => department.Any(a => a.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filterDepartment
                        .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null  && selectedCustomer != null
                else if (selectedPerson == null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredActionPlan = ActionPlanList.Where(actionPlan => department.Any(company => company.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filteredActionPlan
                         .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);

                }
                #endregion

                #region selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null  && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany == null && selectedOrigin == null && selectedBusinessUnit == null && selectedDepartment != null && selectedCustomer != null)
                {
                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredActionPlan = filteredActionPlans.Where(actionPlan => department.Any(company => company.DepartmentName == actionPlan.Department)).ToList();
                    var filteredCustomers = filteredActionPlan
                         .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }
                #endregion

                #region selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment == null  && selectedCustomer != null
                else if (selectedPerson != null && selectedCompany != null && selectedOrigin != null && selectedBusinessUnit == null && selectedDepartment == null && selectedCustomer != null)
                {

                    var filteredActionPlans = new List<APMActionPlan>();
                    foreach (var actionPlan in ActionPlanList)
                    {
                        // Filter tasks in TaskList based on matching Responsible in person list
                        var matchingTasks = actionPlan.TaskList?
                            .Where(task => person.Any(per => task.Responsible == per.FullName))
                            .ToList();

                        // Check if ActionPlan should be included
                        var includeActionPlan =
                            (matchingTasks != null && matchingTasks.Any()) ||
                            person.Any(per => per.FullName == actionPlan.FullName);

                        if (includeActionPlan)
                        {
                            // Clone the original ActionPlan
                            var clonedActionPlan = actionPlan;

                            // Replace TaskList with filtered tasks
                            clonedActionPlan.TaskList = matchingTasks ?? new List<APMActionPlanTask>();

                            filteredActionPlans.Add(clonedActionPlan);
                        }
                    }
                    var filteredActionPlant = filteredActionPlans.Where(actionPlan => plant.Any(company => company.Alias == actionPlan.Location)).ToList();
                    var filteredAction = filteredActionPlant.Where(actionPlan => origin.Any(company => company.Value == actionPlan.Origin)).ToList();
                    var filteredCustomers = filteredAction
                         .Where(actionPlan => apmCustomer.Any(x => x.IdSite == actionPlan.IdSite)).ToList();
                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredCustomers);
                }

                #endregion
                //[shweta.thube][GEOS2-7217][23.07.9077]               
                SelectedAlertTileBarItem = null;
                FillActionPlansAlertSectionList();
                FillStatus();
                if (ListOfPerson != null)
                {
                    if (person == null)
                    {
                        person = new List<Responsible>();
                    }
                    var personNames = person.Select(p => p.FullName).ToList();
                    if (SelectedPerson == null)
                    {
                        SelectedPerson = new List<object>();
                    }
                    SelectedPerson = new List<object>(ListOfPerson.Where(x => personNames.Contains(x.FullName)));
                }
                else
                    SelectedPerson = null;

                SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == CustomFilterStringName);
                IsExpand = false;
                GeosApplication.Instance.Logger.Log("Method FilterCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FilterCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //private void FillResponsibleList()
        //{
        //    ListOfPerson = new ObservableCollection<Responsible>();

        //    if (ActionPlanList != null && ActionPlanList.Count > 0)
        //    {
        //        foreach (var item in ActionPlanList)
        //        {
        //            if (!ListOfPerson.Any(person => person.EmployeeCode == item.EmployeeCode))
        //            {
        //                ListOfPerson.Add(new Responsible()
        //                {
        //                    IdEmployee = (UInt32)item.IdEmployee,
        //                    EmployeeCode = item.EmployeeCode,
        //                    IdGender = item.IdGender,
        //                    FullName = item.FullName,
        //                    EmployeeCodeWithIdGender = item.EmployeeCode + "_" + item.IdGender,
        //                    IsTaskField = false,
        //                    ResponsibleDisplayName = item.ActionPlanResponsibleDisplayName
        //                });
        //            }
        //            if (item.TaskList != null)
        //            {
        //                foreach (var task in item.TaskList)
        //                {
        //                    if (!ListOfPerson.Any(person => person.EmployeeCode == task.EmployeeCode))
        //                    {
        //                        ListOfPerson.Add(new Responsible()
        //                        {
        //                            IdEmployee = (UInt32)task.IdEmployee,
        //                            EmployeeCode = task.EmployeeCode,
        //                            IdGender = task.IdGender,
        //                            FullName = task.Responsible,
        //                            EmployeeCodeWithIdGender = task.EmployeeCode + "_" + task.IdGender,
        //                            IsTaskField = true,
        //                            ResponsibleDisplayName = task.TaskResponsibleDisplayName
        //                        });
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    else
        //    {
        //        ListOfPerson = new ObservableCollection<Responsible>();
        //    }

        //    if (SelectedPerson == null)
        //    {
        //        SelectedPerson = new List<object>();
        //    }
        //    // ListOfPerson = new ObservableCollection<Responsible>(ListOfPerson);

        //    //if (ListOfPerson != null && ListOfPerson.Count > 0)
        //    //{
        //    //    foreach (var item in ListOfPerson)
        //    //    {
        //    //        // string filePath = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Employees/" + item.EmployeeCode + ".png";
        //    //        string filePath = string.Empty;
        //    //        filePath = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Employees/Rounded/" + item.EmployeeCode + ".png";

        //    //        if (!string.IsNullOrEmpty(filePath))
        //    //        {
        //    //            using (System.Net.WebClient webClient = new WebClient())
        //    //            {
        //    //                item.ImageInBytes = webClient.DownloadData(filePath);
        //    //            }
        //    //        }

        //    //        if (item.ImageInBytes.Length == 0)
        //    //        {
        //    //            filePath = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Employees/" + item.EmployeeCode + ".png";


        //    //            if (!string.IsNullOrEmpty(filePath))
        //    //            {
        //    //                using (WebClient webClient = new WebClient())
        //    //                {
        //    //                    item.ImageInBytes = webClient.DownloadData(filePath);
        //    //                }
        //    //            }
        //    //        }


        //    //        if (item.ImageInBytes != null && item.ImageInBytes.Length > 0)
        //    //        {
        //    //            item.OwnerImage = byteArrayToImage(item.ImageInBytes);
        //    //        }
        //    //        else
        //    //        {
        //    //            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
        //    //            {
        //    //                if (item.IdGender == 1)
        //    //                    item.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/FemaleUser_White.png");
        //    //                else
        //    //                    item.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/MaleUser_White.png");
        //    //            }
        //    //            else
        //    //            {
        //    //                if (item.IdGender == 1)
        //    //                    item.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/FemaleUser_Blue.png");
        //    //                else
        //    //                    item.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/MaleUser_Blue.png");
        //    //            }
        //    //        }
        //    //    }

        //    //    ListOfPerson = new ObservableCollection<Responsible>(ListOfPerson);
        //    //}
        //}

        private void FillResponsibleList()
        {
            try
            {
                var source = ActionPlanList;

                if (source == null || source.Count == 0)
                {
                    ListOfPerson = new ObservableCollection<Responsible>();
                    if (SelectedPerson == null) SelectedPerson = new List<object>();
                    return;
                }

                var temp = new List<Responsible>();
                var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // --- FASE 1: Construção da Lista ---
                foreach (var ap in source)
                {
                    if (ap == null) continue;

                    // 1. Processar Responsável do Action Plan
                    if (!string.IsNullOrEmpty(ap.EmployeeCode))
                    {
                        string apKey = ap.EmployeeCode + "_" + ap.IdGender;
                        if (seen.Add(apKey))
                        {
                            // Tenta obter da cache imediatamente
                            ImageSource cachedImg = null;
                            if (_inMemoryImageCache != null && _inMemoryImageCache.ContainsKey(ap.EmployeeCode))
                            {
                                cachedImg = _inMemoryImageCache[ap.EmployeeCode];
                            }

                            temp.Add(new Responsible
                            {
                                IdEmployee = (UInt32)ap.IdEmployee,
                                EmployeeCode = ap.EmployeeCode,
                                IdGender = ap.IdGender,
                                FullName = ap.FullName,
                                EmployeeCodeWithIdGender = apKey,
                                IsTaskField = false,
                                ResponsibleDisplayName = ap.ActionPlanResponsibleDisplayName,
                                OwnerImage = cachedImg
                            });
                        }
                    }

                    // Processar Tarefas e Sub-tarefas
                    if (ap.TaskList != null)
                    {
                        foreach (var task in ap.TaskList)
                        {
                            if (task == null) continue;

                            // 2. Processar Responsável da Tarefa
                            string empCode = task.EmployeeCode ?? string.Empty;
                            if (!string.IsNullOrEmpty(empCode))
                            {
                                string key = empCode + "_" + task.IdGender;
                                if (seen.Add(key))
                                {
                                    ImageSource cachedImgTask = null;
                                    if (_inMemoryImageCache != null && _inMemoryImageCache.ContainsKey(empCode))
                                    {
                                        cachedImgTask = _inMemoryImageCache[empCode];
                                    }

                                    temp.Add(new Responsible
                                    {
                                        IdEmployee = (UInt32)task.IdEmployee,
                                        EmployeeCode = empCode,
                                        IdGender = task.IdGender,
                                        FullName = task.Responsible,
                                        EmployeeCodeWithIdGender = key,
                                        IsTaskField = true,
                                        ResponsibleDisplayName = task.TaskResponsibleDisplayName,
                                        OwnerImage = cachedImgTask
                                    });
                                }
                            }

                            // 3. [NOVO] Processar Responsável da Sub-tarefa
                            if (task.SubTaskList != null)
                            {
                                foreach (var sub in task.SubTaskList)
                                {
                                    if (sub == null) continue;
                                    string subEmpCode = sub.EmployeeCode ?? string.Empty;

                                    if (!string.IsNullOrEmpty(subEmpCode))
                                    {
                                        string subKey = subEmpCode + "_" + sub.IdGender;
                                        if (seen.Add(subKey))
                                        {
                                            ImageSource cachedImgSub = null;
                                            if (_inMemoryImageCache != null && _inMemoryImageCache.ContainsKey(subEmpCode))
                                            {
                                                cachedImgSub = _inMemoryImageCache[subEmpCode];
                                            }

                                            temp.Add(new Responsible
                                            {
                                                IdEmployee = (UInt32)sub.IdEmployee,
                                                EmployeeCode = subEmpCode,
                                                IdGender = sub.IdGender,
                                                FullName = sub.Responsible,
                                                EmployeeCodeWithIdGender = subKey,
                                                IsTaskField = true,
                                                ResponsibleDisplayName = sub.Responsible, // Normalmente subtask usa Responsible
                                                OwnerImage = cachedImgSub
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // --- FASE 2: Atualizar a UI ---
                // Ordena para ficar bonito na dropdown
                var sortedTemp = temp
                    .OrderBy(r => r.ResponsibleDisplayName, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(r => r.EmployeeCode, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                ListOfPerson = new ObservableCollection<Responsible>(sortedTemp);

                if (SelectedPerson == null)
                    SelectedPerson = new List<object>();

                // --- FASE 3: Carregar imagens em falta ---
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _ = LoadResponsibleImagesBackgroundAsync(ListOfPerson);
                }), System.Windows.Threading.DispatcherPriority.ContextIdle);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("FillResponsibleList optimized error: " + ex.Message, Category.Exception, Priority.Low);
            }
        }

        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        //[Sudhir.Jangra][GEOS2-5977]
        private void EditActionPlanHyperLinkCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditActionPlanHyperLinkCommandAction....", category: Category.Info, priority: Priority.Low);
                /*
                GridControl gridControl = (GridControl)obj;
                TableView detailView = (TableView)gridControl.View;
                */
                GridControl gridControl;
                TableView detailView = new TableView();
                if (obj is GridControl gc)
                {
                    gridControl = gc;
                    //gridControl = (GridControl)obj;
                    detailView = (TableView)gridControl.View;
                }
                else if (obj is TableView tv)
                {
                    // Handle the case where obj is directly a TableView
                    detailView = tv;
                }
                long? lastUpdatedId = null;//[pallavi.kale][GEOS2-7213][25.07.2025]
                //[shweta.thube][GEOS2-6453]
                if (IsTaskGridVisibility)
                {
                    APMActionPlanTask selectedTask = (APMActionPlanTask)detailView.DataControl.CurrentItem;
                    long Idactionplan = selectedTask.IdActionPlan;
                    APMActionPlan SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == Idactionplan);
                    AddEditActionPlanView addEditActionPlanView = new AddEditActionPlanView();
                    AddEditActionPlanViewModel addEditActionPlanViewModel = new AddEditActionPlanViewModel();
                    EventHandler handle = delegate { addEditActionPlanView.Close(); };
                    addEditActionPlanViewModel.RequestClose += handle;
                    addEditActionPlanViewModel.IsNew = false;
                    addEditActionPlanViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditActionPlanTitle").ToString();
                    List<APMActionPlan> temp = new List<APMActionPlan>();
                    temp = new List<APMActionPlan>(TempActionPlanList);
                    APMCommon.Instance.ActionPlanList = new List<APMActionPlan>(temp.OrderBy(ap => ap.Code));
                    addEditActionPlanViewModel.EditInit(SelectedActionPlan);
                    addEditActionPlanView.DataContext = addEditActionPlanViewModel;
                    var ownerInfo = (detailView as FrameworkElement);
                    addEditActionPlanView.Owner = Window.GetWindow(ownerInfo);

                    addEditActionPlanView.ShowDialog();
                    if (addEditActionPlanViewModel.IsSave)
                    {
                        SelectedActionPlan.Code = addEditActionPlanViewModel.Code;
                        SelectedActionPlan.Description = addEditActionPlanViewModel.Description;
                        SelectedActionPlan.IdCompany = APMCommon.Instance.LocationList[addEditActionPlanViewModel.SelectedLocationIndex].IdCompany;
                        SelectedActionPlan.Location = APMCommon.Instance.LocationList[addEditActionPlanViewModel.SelectedLocationIndex].Alias;
                        SelectedActionPlan.IdEmployee = (Int32)APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].IdEmployee;
                        SelectedActionPlan.FirstName = APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].FirstName;
                        SelectedActionPlan.LastName = APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].LastName;
                        SelectedActionPlan.FullName = APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].FullName;
                        SelectedActionPlan.IdLookupBusinessUnit = APMCommon.Instance.BusinessUnitList[addEditActionPlanViewModel.SelectedBusinessIndex].IdLookupValue;
                        SelectedActionPlan.BusinessUnit = APMCommon.Instance.BusinessUnitList[addEditActionPlanViewModel.SelectedBusinessIndex].Value;
                        SelectedActionPlan.CreatedByName = addEditActionPlanViewModel.CreatedBy;
                        SelectedActionPlan.CreatedIn = addEditActionPlanViewModel.CreatedIn;
                        SelectedActionPlan.IdLookupOrigin = APMCommon.Instance.OriginList[addEditActionPlanViewModel.SelectedOriginIndex].IdLookupValue;
                        SelectedActionPlan.Origin = APMCommon.Instance.OriginList[addEditActionPlanViewModel.SelectedOriginIndex].Value;
                        SelectedActionPlan.IdDepartment = (Int32)APMCommon.Instance.DepartmentList[addEditActionPlanViewModel.SelectedDepartmentIndex].IdDepartment;
                        SelectedActionPlan.Department = APMCommon.Instance.DepartmentList[addEditActionPlanViewModel.SelectedDepartmentIndex].DepartmentName;
                        SelectedActionPlan.OriginDescription = addEditActionPlanViewModel.OriginDescription;
                        SelectedActionPlan.TaskList = new List<APMActionPlanTask>(addEditActionPlanViewModel.TaskList);
                        lastUpdatedId = SelectedActionPlan?.IdActionPlan;//[pallavi.kale][GEOS2-7213][25.07.2025]
                        IsActionPlanEdit = true;
                        ClonedActionPlanList.FirstOrDefault(x => x.IdActionPlan == SelectedActionPlan.IdActionPlan).TaskList = new List<APMActionPlanTask>(addEditActionPlanViewModel.TaskList);
                        FillLeftTileList();
                        SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                        //FilterCommandAction(obj);
                        FilterCommandActionForTask(obj);
                        CommandAlertFilterTileClickAction(obj);
                        if (PreviousSelectedAlertTileBarItem != null)
                            SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedAlertTileBarItem.Caption);
                        IsActionPlanEdit = false;

                    }
                    //else
                    //{
                    //    SelectedActionPlan.TaskList = new List<APMActionPlanTask>(addEditActionPlanViewModel.TempTaskList);
                    //    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                    //    FilterCommandAction(obj);
                    //}
                }
                else
                {
                    AddEditActionPlanView addEditActionPlanView = new AddEditActionPlanView();
                    AddEditActionPlanViewModel addEditActionPlanViewModel = new AddEditActionPlanViewModel();
                    EventHandler handle = delegate { addEditActionPlanView.Close(); };
                    addEditActionPlanViewModel.RequestClose += handle;
                    addEditActionPlanViewModel.IsNew = false;
                    addEditActionPlanViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditActionPlanTitle").ToString();
                    List<APMActionPlan> temp = new List<APMActionPlan>();
                    temp = new List<APMActionPlan>(TempActionPlanList);
                    APMCommon.Instance.ActionPlanList = new List<APMActionPlan>(temp.OrderBy(ap => ap.Code));
                    addEditActionPlanViewModel.EditInit(SelectedActionPlan);
                    addEditActionPlanView.DataContext = addEditActionPlanViewModel;
                    var ownerInfo = (detailView as FrameworkElement);
                    addEditActionPlanView.Owner = Window.GetWindow(ownerInfo);

                    addEditActionPlanView.ShowDialog();
                    if (addEditActionPlanViewModel.IsSave)
                    {
                        SelectedActionPlan.Code = addEditActionPlanViewModel.Code;
                        SelectedActionPlan.Description = addEditActionPlanViewModel.Description;
                        SelectedActionPlan.IdCompany = APMCommon.Instance.LocationList[addEditActionPlanViewModel.SelectedLocationIndex].IdCompany;
                        SelectedActionPlan.Location = APMCommon.Instance.LocationList[addEditActionPlanViewModel.SelectedLocationIndex].Alias;
                        SelectedActionPlan.IdEmployee = (Int32)APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].IdEmployee;
                        SelectedActionPlan.FirstName = APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].FirstName;
                        SelectedActionPlan.LastName = APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].LastName;
                        SelectedActionPlan.FullName = APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].FullName;
                        SelectedActionPlan.IdLookupBusinessUnit = APMCommon.Instance.BusinessUnitList[addEditActionPlanViewModel.SelectedBusinessIndex].IdLookupValue;
                        SelectedActionPlan.BusinessUnit = APMCommon.Instance.BusinessUnitList[addEditActionPlanViewModel.SelectedBusinessIndex].Value;
                        SelectedActionPlan.CreatedByName = addEditActionPlanViewModel.CreatedBy;
                        SelectedActionPlan.CreatedIn = addEditActionPlanViewModel.CreatedIn;
                        SelectedActionPlan.IdLookupOrigin = APMCommon.Instance.OriginList[addEditActionPlanViewModel.SelectedOriginIndex].IdLookupValue;
                        SelectedActionPlan.Origin = APMCommon.Instance.OriginList[addEditActionPlanViewModel.SelectedOriginIndex].Value;
                        SelectedActionPlan.IdDepartment = (Int32)APMCommon.Instance.DepartmentList[addEditActionPlanViewModel.SelectedDepartmentIndex].IdDepartment;
                        SelectedActionPlan.Department = APMCommon.Instance.DepartmentList[addEditActionPlanViewModel.SelectedDepartmentIndex].DepartmentName;
                        SelectedActionPlan.OriginDescription = addEditActionPlanViewModel.OriginDescription;
                        SelectedActionPlan.TaskList = new List<APMActionPlanTask>(addEditActionPlanViewModel.TaskList);
                        lastUpdatedId = SelectedActionPlan?.IdActionPlan;//[pallavi.kale][GEOS2-7213][25.07.2025]
                        IsActionPlanEdit = true;
                        ClonedActionPlanList.FirstOrDefault(x => x.IdActionPlan == SelectedActionPlan.IdActionPlan).TaskList = new List<APMActionPlanTask>(addEditActionPlanViewModel.TaskList);
                        FillLeftTileList();
                        SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                        FilterCommandAction(obj);
                        // CommandAlertFilterTileClickAction(obj);
                        //SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedAlertTileBarItem.Caption);
                        IsActionPlanEdit = false;
                        IsExpand = false;
                        // ExpandAndCollapseActionPlanCommandAction(obj);
                    }
                    else
                    {
                        SelectedActionPlan.TaskList = new List<APMActionPlanTask>(addEditActionPlanViewModel.TempTaskList);
                        SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                        FilterCommandAction(obj);
                        IsExpand = false;
                        // ExpandAndCollapseActionPlanCommandAction(obj);
                    }
                    //[pallavi.kale][GEOS2-7213][25.07.2025]
                    if (lastUpdatedId.HasValue)
                    {
                        var updatedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == lastUpdatedId.Value)
                                             ?? TempActionPlanList.FirstOrDefault(x => x.IdActionPlan == lastUpdatedId.Value);

                        if (updatedActionPlan != null && !ActionPlanList.Contains(updatedActionPlan))
                        {
                            ActionPlanList.Add(updatedActionPlan);
                        }

                        LastUpdatedIdActionPlan = updatedActionPlan?.IdActionPlan ?? 0;
                        LastUpdatedIdActionPlanTask = -1;
                        if (updatedActionPlan?.TaskList != null && updatedActionPlan.TaskList.Any())
                        {
                            IsExpand = true;
                            ExpandLastUpdatedActionPlan();
                        }
                        else
                        {
                            IsExpand = false;
                        }
                    }
                }



                GeosApplication.Instance.Logger.Log("Method EditActionPlanHyperLinkCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditActionPlanHyperLinkCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }
        //[shweta.thube][GEOS2-5979]
        private void AddActionPlanTaskViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddActionPlansViewWindowShow()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new System.Windows.Media.SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                AddEditTaskViewModel addEditTaskViewModel = new AddEditTaskViewModel();
                AddEditTaskView addEditTaskView = new AddEditTaskView();
                EventHandler handle = delegate { addEditTaskView.Close(); };
                addEditTaskViewModel.RequestClose += handle;
                addEditTaskViewModel.IsNew = true;
                addEditTaskViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddActionPlansHeader").ToString();
                List<APMActionPlan> temp = new List<APMActionPlan>();
                temp = new List<APMActionPlan>(TempActionPlanList);
                APMCommon.Instance.ActionPlanList = new List<APMActionPlan>(temp.OrderBy(ap => ap.Code));
                addEditTaskViewModel.OtItemVisibility = Visibility.Collapsed;//[shweta.thube][GEOS2-6912]
                addEditTaskViewModel.Init(SelectedActionPlan);
                addEditTaskView.DataContext = addEditTaskViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addEditTaskView.ShowDialogWindow();

                if (addEditTaskViewModel.IsSave)
                {
                    var lastUpdatedId = SelectedActionPlan?.IdActionPlan;//[pallavi.kale][GEOS2-7213][25.07.2025]
                    if (SelectedActionPlan.TaskList == null)
                    {
                        SelectedActionPlan.TaskList = new List<APMActionPlanTask>();
                    }
                    SelectedActionPlan.TaskList.Add(addEditTaskViewModel.TempTask);

                    // Set deletion flags for the newly added task
                    SetDeletionFlags(new[] { SelectedActionPlan });

                    //[shweta.thube][11/ 06/2025][GEOS2-8388]
                    SelectedActionPlan.TotalActionItems = SelectedActionPlan.TaskList.Count();
                    SelectedActionPlan.TotalClosedItems = SelectedActionPlan.TaskList.Where(x => x.CloseDate != null).Count();
                    SelectedActionPlan.TotalOpenItems = SelectedActionPlan.TaskList.Where(x => x.CloseDate == null).Count();

                    if (SelectedActionPlan.TotalActionItems != 0)
                    {
                        SelectedActionPlan.Percentage = (int)(((float)SelectedActionPlan.TotalClosedItems / SelectedActionPlan.TotalActionItems) * 100);
                    }
                    else
                    {
                        SelectedActionPlan.Percentage = 0;
                    }

                    if (SelectedActionPlan.Percentage <= 24)
                    {
                        SelectedActionPlan.TotalClosedColor = "Red";
                    }
                    else if (SelectedActionPlan.Percentage >= 25 && SelectedActionPlan.Percentage <= 49)
                    {
                        SelectedActionPlan.TotalClosedColor = "Orange";
                    }
                    else if (SelectedActionPlan.Percentage >= 50 && SelectedActionPlan.Percentage <= 74)
                    {
                        SelectedActionPlan.TotalClosedColor = "Yellow";
                    }
                    else if (SelectedActionPlan.Percentage >= 75 && SelectedActionPlan.Percentage <= 99)
                    {
                        SelectedActionPlan.TotalClosedColor = "LightGreen";
                    }
                    else if (SelectedActionPlan.Percentage == 100)
                    {
                        SelectedActionPlan.TotalClosedColor = "Green";
                    }

                    if (ClonedActionPlanList != null)
                    {
                        ClonedActionPlanList.FirstOrDefault(x => x.IdActionPlan == SelectedActionPlan.IdActionPlan).TaskList = new List<APMActionPlanTask>(SelectedActionPlan.TaskList);
                    }
                    IsActionPlanEdit = true;
                    FillLeftTileList();
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                    FilterCommandAction(obj);
                    CommandAlertFilterTileClickAction(obj);
                    if (PreviousSelectedAlertTileBarItem != null)
                        SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedAlertTileBarItem.Caption);
                    IsActionPlanEdit = false;
                    //[pallavi.kale][GEOS2-7213][25.07.2025]
                    if (SelectedActionPlan == null || SelectedActionPlan.IdActionPlan != lastUpdatedId)
                    {
                        SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == lastUpdatedId);
                    }
                    LastUpdatedIdActionPlan = SelectedActionPlan?.IdActionPlan ?? 0;
                    LastUpdatedIdActionPlanTask = -1;
                    IsExpandTaskLevelOnly = true;
                    if (SelectedActionPlan?.TaskList != null && SelectedActionPlan.TaskList.Any())
                    {
                        IsExpand = true;
                        ExpandLastUpdatedActionPlan();
                    }
                    else
                    {
                        IsExpand = false;
                    }
                    //Init();
                }


                GeosApplication.Instance.Logger.Log("Method AddActionPlansViewWindowShow()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddActionPlansViewWindowShow()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public ImageSource byteArrayToImage(byte[] byteArrayIn)
        {
            ImageSource imgSrc = null;
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(byteArrayIn);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                biImg.DecodePixelHeight = 10;
                biImg.DecodePixelWidth = 10;

                imgSrc = biImg as ImageSource;

                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return imgSrc;
        }

        //[shweta.thube][GEOS2-5980]
        private void EditTaskHyperLinkCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditTaskHyperLinkCommandAction....", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = (GridControl)obj;
                TableView detailView = (TableView)gridControl.View;
                APMActionPlanTask selectedTask = (APMActionPlanTask)detailView.DataControl.CurrentItem;
                AddEditTaskView addEditTaskView = new AddEditTaskView();
                AddEditTaskViewModel addEditTaskViewModel = new AddEditTaskViewModel();
                EventHandler handle = delegate { addEditTaskView.Close(); };
                addEditTaskViewModel.RequestClose += handle;
                addEditTaskViewModel.IsNew = false;
                addEditTaskViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditActionPlansHeader").ToString();
                List<APMActionPlan> temp = new List<APMActionPlan>();
                temp = new List<APMActionPlan>(TempActionPlanList);
                APMCommon.Instance.ActionPlanList = new List<APMActionPlan>(temp.OrderBy(ap => ap.Code));
                var ActionPlan = APMCommon.Instance.ActionPlanList.FirstOrDefault(ap => ap.IdActionPlan == selectedTask.IdActionPlan);
                selectedTask.IdActionPlanResponsible = ActionPlan.IdEmployee;
                selectedTask.ActionPlanResponsibleIdUser = ActionPlan.ResponsibleIdUser;
                selectedTask.IdSite = ActionPlan.IdSite;  //[shweta.thube][GEOS2-6912]
                selectedTask.IdActionPlanLocation = ActionPlan.IdCompany;//[shweta.thube][GEOS2-6912]
                addEditTaskViewModel.OtItemVisibility = Visibility.Collapsed;//[shweta.thube][GEOS2-6912]
                selectedTask.CustomerName = ActionPlan.GroupName;
                selectedTask.IsTaskAdded = true;  //[shweta.thube][GEOS2-8394]
                addEditTaskViewModel.EditInit(selectedTask);
                addEditTaskView.DataContext = addEditTaskViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addEditTaskView.Owner = Window.GetWindow(ownerInfo);

                addEditTaskView.ShowDialog();
                int? lastUpdatedId = null;//[pallavi.kale][GEOS2-7213][25.07.2025]
                if (addEditTaskViewModel.IsSave)
                {
                    IsActionPlanEdit = true;
                    SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == selectedTask.IdActionPlan);
                    lastUpdatedId = (int?)SelectedActionPlan?.IdActionPlan;//[pallavi.kale][GEOS2-7213][25.07.2025]
                    var index = SelectedActionPlan.TaskList.FindIndex(x => x.IdActionPlanTask == selectedTask.IdActionPlanTask);
                    if (index >= 0)
                    {
                        SelectedActionPlan.TaskList[index] = addEditTaskViewModel.UpdatedTask;
                    }

                    if (ClonedActionPlanList != null)
                    {
                        ClonedActionPlanList.FirstOrDefault(x => x.IdActionPlan == SelectedActionPlan.IdActionPlan).TaskList[index] = addEditTaskViewModel.UpdatedTask;
                    }
                    FillLeftTileList();
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                    FilterCommandAction(obj);
                    FilterCommandActionForTask(obj);
                    CommandAlertFilterTileClickAction(obj);
                    if (PreviousSelectedAlertTileBarItem != null)
                        SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedAlertTileBarItem.Caption);
                    IsActionPlanEdit = false;
                    IsExpand = false;
                    // ExpandAndCollapseActionPlanCommandAction(obj);

                    //Init();
                }
                //[pallavi.kale][GEOS2-7213][25.07.2025]
                if (lastUpdatedId.HasValue)
                {
                    if (SelectedActionPlan == null || SelectedActionPlan.IdActionPlan != lastUpdatedId)
                        SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == lastUpdatedId.Value);

                    LastUpdatedIdActionPlan = SelectedActionPlan?.IdActionPlan ?? 0;

                    if (SelectedActionPlan?.TaskList != null && SelectedActionPlan.TaskList.Any())
                    {
                        var updatedTask = SelectedActionPlan.TaskList.FirstOrDefault(t => t.IdActionPlanTask == selectedTask.IdActionPlanTask);
                        IsExpand = true;
                        LastUpdatedIdActionPlanTask = selectedTask.IdActionPlanTask;
                        if (updatedTask != null && updatedTask.SubTaskList != null && updatedTask.SubTaskList.Any())
                        {
                            IsExpandTaskLevelOnly = false;
                        }
                        else
                        {
                            IsExpandTaskLevelOnly = true;
                        }

                        ExpandLastUpdatedActionPlan();
                    }
                    else
                    {
                        IsExpand = false;
                    }
                }
                else
                {
                    IsExpand = false;
                }

                GeosApplication.Instance.Logger.Log("Method EditTaskHyperLinkCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditTaskHyperLinkCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-5981]
        public void DeleteTaskCommandAction(object obj)
        {
            try
            {
                APMActionPlanTask Temp = (APMActionPlanTask)obj;
                int? lastUpdatedId = (int?)Temp?.IdActionPlan;//[pallavi.kale][GEOS2-7213][25.07.2025]
                if (Temp.SubTaskList == null || Temp.SubTaskList.Count == 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteTasksDetailsMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {

                        //[shweta.thube][GEOS2-6020]
                        Temp.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();
                        Temp.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = Temp.IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,

                            Comments = string.Format(System.Windows.Application.Current.FindResource("TaskDeletedChangeLog").ToString(), Temp.TaskNumber)
                        });
                        //IsDeleted = APMService.DeleteTaskForActionPlan_V2570(Temp.IdActionPlanTask);
                        //APMService = new APMServiceController("localhost:6699");
                        IsDeleted = APMService.DeleteTaskForActionPlan_V2580(Temp.IdActionPlanTask, Temp.ActionPlanLogEntries);//[shweta.thube][GEOS2-6020]

                        var temp = ActionPlanList.FirstOrDefault(j => (j.TaskList != null &&
                                     j.TaskList.Any(i => i.IdActionPlanTask == Temp.IdActionPlanTask)));
                        temp.TaskList = temp.TaskList.Where(i => i.IdActionPlanTask != Temp.IdActionPlanTask).ToList();
                        //[shweta.thube][11/ 06/2025][GEOS2-8388]
                        temp.TotalActionItems = temp.TaskList.Count();
                        temp.TotalClosedItems = temp.TaskList.Where(x => x.CloseDate != null).Count();
                        temp.TotalOpenItems = temp.TaskList.Where(x => x.CloseDate == null).Count();

                        if (temp.TotalActionItems != 0)
                        {
                            temp.Percentage = (int)(((float)temp.TotalClosedItems / temp.TotalActionItems) * 100);
                        }
                        else
                        {
                            temp.Percentage = 0;
                        }

                        if (temp.Percentage <= 24)
                        {
                            temp.TotalClosedColor = "Red";
                        }
                        else if (temp.Percentage >= 25 && temp.Percentage <= 49)
                        {
                            temp.TotalClosedColor = "Orange";
                        }
                        else if (temp.Percentage >= 50 && temp.Percentage <= 74)
                        {
                            temp.TotalClosedColor = "Yellow";
                        }
                        else if (temp.Percentage >= 75 && temp.Percentage <= 99)
                        {
                            temp.TotalClosedColor = "LightGreen";
                        }
                        else if (temp.Percentage == 100)
                        {
                            temp.TotalClosedColor = "Green";
                        }

                        ActionPlanList = new ObservableCollection<APMActionPlan>(ActionPlanList);       //[shweta.thube][GEOS2-8985]

                        TaskGridList.Remove(Temp);
                        TaskGridList = new ObservableCollection<APMActionPlanTask>(TaskGridList);      //[shweta.thube][GEOS2-8985]
                        int index = ClonedTaskGridList.FindIndex(x => x.IdActionPlanTask == Temp.IdActionPlanTask);
                        if (index != -1)
                        {
                            ClonedTaskGridList.RemoveAt(index);
                        }

                        TopTaskListOfFilterTile = new ObservableCollection<TileBarFilters>();
                        AddTaskCustomSetting();
                        if (PreviousSelectedTaskTopTileBarItem != null)
                        {
                            SelectedTaskTopTileBarItem = TopTaskListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTaskTopTileBarItem.Caption);
                            if (SelectedTaskTopTileBarItem.Caption != "All")
                            {
                                MyTaskFilterString = SelectedTaskTopTileBarItem.FilterCriteria;
                            }
                            else
                            {
                                MyTaskFilterString = string.Empty;
                            }
                        }


                        SelectedActionPlan = temp;
                        FillStatus();
                        if (IsDeleted)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TaskDetailsDeletedsuccessfully").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                    IsActionPlanEdit = true;
                    APMActionPlanTask Temp1 = (APMActionPlanTask)obj;
                    var actionPlan = ClonedActionPlanList.FirstOrDefault(x => x.IdActionPlan == Temp1.IdActionPlan);
                    if (actionPlan != null)
                    {
                        var taskToRemove = actionPlan.TaskList.FirstOrDefault(t => t.IdActionPlanTask == Temp1.IdActionPlanTask); // Assuming IdTask uniquely identifies tasks
                        if (taskToRemove != null)
                        {
                            actionPlan.TaskList.Remove(taskToRemove);
                        }
                    }

                    if (IsTaskGridVisibility)
                    {
                        if (ActionPlanCodeList != null)
                        {
                            ActionPlanCodeList = new List<APMActionPlan>(ClonedActionPlanList.Where(x => x.TaskList != null && x.TaskList.Count > 0).OrderBy(ap => ap.Code));
                        }
                    }
                    FillLeftTileList();
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                    //FilterCommandAction(obj);
                    FilterCommandActionForTask(obj);
                    CommandAlertFilterTileClickAction(obj);
                    if (PreviousSelectedAlertTileBarItem != null)
                        SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedAlertTileBarItem.Caption);
                    IsActionPlanEdit = false;
                    IsExpand = false;
                    //[pallavi.kale][GEOS2-7213][25.07.2025]
                    if (lastUpdatedId.HasValue)
                    {
                        SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == lastUpdatedId.Value);
                        LastUpdatedIdActionPlan = lastUpdatedId.Value;

                        if (SelectedActionPlan?.TaskList != null && SelectedActionPlan.TaskList.Any())
                        {
                            IsExpand = true;
                            IsExpandTaskLevelOnly = true;
                            ExpandLastUpdatedActionPlan();
                        }
                        else
                        {
                            IsExpand = false;
                        }
                    }
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteActionplanTaskMessage").ToString(), Temp.TaskNumber), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    IsDeleted = false;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DeleteTaskCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteTaskCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteTaskCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteTaskCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        // [nsatpute][09-10-2024][GEOS2-5975]
        private void FillStatus()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatus ...", category: Category.Info, priority: Priority.Low);
                //APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                if (statusList == null)
                    statusList = new List<LookupValue>(APMService.GetLookupValues_V2550(152));// Task Status
                if (statusList != null)
                {
                    ListStatus = new ObservableCollection<TaskStatuswise>();
                    statusList.ForEach(x => ListStatus.Add(new TaskStatuswise { IdLookupValue = x.IdLookupValue, Value = x.Value, HtmlColor = x.HtmlColor }));
                    foreach (APMActionPlan actionPlan in ActionPlanList)
                    {
                        if (actionPlan.TaskList != null)
                        {
                            foreach (APMActionPlanTask task in actionPlan.TaskList.OrderBy(x => x.TaskNumber))
                            {
                                task.IdActionPlanResponsible = actionPlan.IdEmployee;//[Sudhir.Jangra][GEOS2-6557]
                                TaskStatuswise taskStatusWise = ListStatus.FirstOrDefault(p => p.IdLookupValue == task.IdLookupStatus);
                                if (taskStatusWise != null)
                                {
                                    task.BusinessUnit = actionPlan.BusinessUnit;
                                    task.BusinessUnitHTMLColor = actionPlan.BusinessUnitHTMLColor;
                                    task.ActionPlanCode = actionPlan.Code; task.Origin = actionPlan.OriginDescription;

                                    if (DateTime.Now.Date < task.DueDate.Date.AddDays(2))
                                    {
                                        task.TaskTabColor = System.Drawing.Color.Green;
                                        task.TaskTabBrush = new SolidColorBrush(Colors.Green);
                                        //task.DueColor = "#008000";
                                        task.CardDueColor = "#008000";
                                    }
                                    else if (DateTime.Now.Date < task.DueDate.Date.AddDays(7))
                                    {
                                        task.TaskTabColor = System.Drawing.Color.Orange;
                                        task.TaskTabBrush = new SolidColorBrush(Colors.Orange);
                                        // task.DueColor = "#FFFF00";
                                        task.CardDueColor = "#FFB913";

                                    }
                                    else //if (DateTime.Now.Date > task.DueDate.Date.AddDays(7))
                                    {
                                        task.TaskTabColor = System.Drawing.Color.Red;
                                        task.TaskTabBrush = new SolidColorBrush(Colors.Red);
                                        // task.DueColor = "#FF0000";
                                        task.CardDueColor = "#FF0000";

                                    }
                                    taskStatusWise.TaskList.Add(task);
                                }
                            }
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillStatus() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatus() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatus() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatus() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][09-10-2024][GEOS2-5975]
        public void SwitchToGridViewCommandAction(object obj)
        {
            try
            {
                ResetFilterState();
                GridViewVisibility = Visibility.Visible;
                TaskViewVisibility = Visibility.Collapsed;
                TaskGridVisibility = Visibility.Collapsed;//[shweta.thube][GEOS2-6453]
                IsTaskGridVisibility = false;
                //AddCustomSetting();
                Init();
                if (ListOfFilterTile != null)
                {
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault();
                }
                CustomFilterStringName = "All";
                FilterCommandAction(null);
                MyFilterString = string.Empty;
                MyChildFilterString = string.Empty;
                MyTaskFilterString = string.Empty;
                SelectedLocation = new List<object>();
                SelectedPerson = new List<object>();
                SelectedBussinessUnit = new List<object>();
                SelectedOrigin = new List<object>();
                SelectedDepartment = new List<object>();
                SelectedActionPlansCodeForTask = new List<object>();
                SelectedLocationForTask = new List<object>();
                SelectedPersonForTask = new List<object>();
                SelectedBussinessUnitForTask = new List<object>();
                SelectedOriginForTask = new List<object>();
                SelectedCustomer = new List<object>();

                IsActionPlanAllLocationSelected = false;
                IsActionPlanAllResponsibleSelected = false;
                IsActionPlanAllBUSelected = false;
                IsActionPlanAllOriginSelected = false;
                IsActionPlanAllDepartmentSelected = false;
                IsActionPlanAllActionPlanSelected = false;
                IsActionPlanAllActionPlanSelectedForTask = false;
                IsActionPlanAllLocationSelectedForTask = false;
                IsActionPlanAllResponsibleSelectedForTask = false;
                IsActionPlanAllBUSelectedForTask = false;
                IsActionPlanAllOriginSelectedForTask = false;
                IsActionPlanAllCustomerSelected = false;
                SelectedActionPlansAlertTileBarItem = null;    //[shweta.thube][GEOS2-7217][28.07.2025]

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SwitchToGridViewCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SwitchToGridViewCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SwitchToGridViewCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SwitchToGridViewCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][09-10-2024][GEOS2-5975]
        public void SwitchToTaskViewCommandAction(object obj)
        {
            try
            {
                ResetFilterState();
                TaskViewVisibility = Visibility.Visible;
                GridViewVisibility = Visibility.Collapsed;
                TaskGridVisibility = Visibility.Collapsed;//[shweta.thube][GEOS2-6453]
                IsTaskGridVisibility = false;
                Init();
                if (ListOfFilterTile != null)
                {
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault();
                }
                CustomFilterStringName = "All";
                MyFilterString = string.Empty;
                MyChildFilterString = string.Empty;
                MyTaskFilterString = string.Empty;

                SelectedLocation = new List<object>();
                SelectedPerson = new List<object>();
                SelectedBussinessUnit = new List<object>();
                SelectedOrigin = new List<object>();
                SelectedDepartment = new List<object>();
                SelectedActionPlansCodeForTask = new List<object>();
                SelectedLocationForTask = new List<object>();
                SelectedPersonForTask = new List<object>();
                SelectedBussinessUnitForTask = new List<object>();
                SelectedOriginForTask = new List<object>();
                SelectedCustomer = new List<object>();

                IsActionPlanAllLocationSelected = false;
                IsActionPlanAllResponsibleSelected = false;
                IsActionPlanAllBUSelected = false;
                IsActionPlanAllOriginSelected = false;
                IsActionPlanAllDepartmentSelected = false;
                IsActionPlanAllActionPlanSelected = false;
                IsActionPlanAllActionPlanSelectedForTask = false;
                IsActionPlanAllLocationSelectedForTask = false;
                IsActionPlanAllResponsibleSelectedForTask = false;
                IsActionPlanAllBUSelectedForTask = false;
                IsActionPlanAllOriginSelectedForTask = false;
                IsActionPlanAllCustomerSelected = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SwitchToTaskViewCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SwitchToTaskViewCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SwitchToTaskViewCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SwitchToTaskViewCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //[Sudhir.Jangra][GEOS2-6015]
        private void CommentsHyperLinkCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommentsHyperLinkCommandAction....", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                GridControl gridControl = (GridControl)obj;
                TableView detailView = (TableView)gridControl.View;
                APMActionPlanTask selectedTask = (APMActionPlanTask)detailView.DataControl.CurrentItem;
                TaskCommentsView taskCommentsView = new TaskCommentsView();
                TaskCommentsViewModel taskCommentsViewModel = new TaskCommentsViewModel();
                EventHandler handle = delegate { taskCommentsView.Close(); };
                taskCommentsViewModel.RequestClose += handle;
                taskCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("TaskCommentsHeader").ToString();
                taskCommentsViewModel.Init(selectedTask);
                taskCommentsView.DataContext = taskCommentsViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                taskCommentsView.Owner = Window.GetWindow(ownerInfo);
                taskCommentsView.ShowDialog();
                if (taskCommentsViewModel.IsSave)
                {
                    IsActionPlanEdit = true;
                    SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == selectedTask.IdActionPlan);
                    var index = SelectedActionPlan.TaskList.FindIndex(x => x.IdActionPlanTask == selectedTask.IdActionPlanTask);
                    if (index >= 0)
                    {
                        SelectedActionPlan.TaskList[index].CommentsCount = taskCommentsViewModel.TaskCommentsList.Count;
                        SelectedActionPlan.TaskList[index].TaskLastComment = taskCommentsViewModel.TaskCommentsList.OrderByDescending(a => a.CreatedIn).FirstOrDefault().Comments;
                    }

                    if (ClonedActionPlanList != null)
                    {
                        ClonedActionPlanList.FirstOrDefault(x => x.IdActionPlan == SelectedActionPlan.IdActionPlan).TaskList[index].CommentsCount = taskCommentsViewModel.TaskCommentsList.Count;
                        ClonedActionPlanList.FirstOrDefault(x => x.IdActionPlan == SelectedActionPlan.IdActionPlan).TaskList[index].TaskLastComment = taskCommentsViewModel.TaskCommentsList.OrderByDescending(a => a.CreatedIn).FirstOrDefault().Comments;

                    }
                    FillLeftTileList();
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                    FilterCommandAction(obj);
                    CommandAlertFilterTileClickAction(obj);
                    if (PreviousSelectedAlertTileBarItem != null)
                        SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedAlertTileBarItem.Caption);
                    IsActionPlanEdit = false;
                    IsExpand = false;
                    // ExpandAndCollapseActionPlanCommandAction(obj);

                    //Init();
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method CommentsHyperLinkCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method CommentsHyperLinkCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-5976]
        private void UpdateTaskStatus(ListBoxDropEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateTaskStatus ...", category: Category.Info, priority: Priority.Low);

                // [IMPORTANTE] Define Handled = true para impedir que o DevExpress tente mover o item visualmente sozinho.
                // Isto resolve o problema do "cartão cinzento" ou "fantasma".
                // Nós vamos tratar da movimentação das listas manualmente abaixo.
                obj.Handled = true;

                if (!GeosApplication.Instance.IsAPMActionPlanPermission) return;

                // 1. Obter dados do Drag & Drop
                ListBoxEdit sourceListBox = (ListBoxEdit)obj.SourceControl;
                ListBoxEdit destListBox = (ListBoxEdit)obj.ListBoxEdit;

                TaskStatuswise sourceStatus = (TaskStatuswise)sourceListBox.Tag;
                TaskStatuswise destStatus = (TaskStatuswise)destListBox.Tag;

                // Se a origem e o destino forem iguais, não fazemos nada
                if (sourceStatus.IdLookupValue == destStatus.IdLookupValue) return;

                var draggedItems = obj.DraggedRows.Cast<APMActionPlanTask>().ToList();
                var item = draggedItems.FirstOrDefault();

                if (item == null) return;

                CurrentItem = item;

                // 2. Validação: Apenas o responsável pode mover para "Done"
                if (destStatus.Value == "Done")
                {
                    if (item.IdActionPlanResponsible != item.IdEmployee)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("APMActionPlanTaskDragDropValidation").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                }

                // 3. Abrir Popup de Comentário
                StatusUpdateCommentView statusUpdateCommentView = new StatusUpdateCommentView();
                StatusUpdateCommentViewModel statusUpdateCommentViewModel = new StatusUpdateCommentViewModel();
                EventHandler handle = delegate { statusUpdateCommentView.Close(); };
                statusUpdateCommentViewModel.RequestClose += handle;
                statusUpdateCommentView.DataContext = statusUpdateCommentViewModel;

                // O código pausa aqui até o utilizador fechar a janela
                statusUpdateCommentView.ShowDialog();

                // 4. Se o utilizador clicou em "Accept" (IsSave = true)
                if (statusUpdateCommentViewModel.IsSave)
                {
                    // --- A. Atualizar Base de Dados ---
                    DraggedTaskComment = statusUpdateCommentViewModel.Comment;
                    item.TaskTabBrush = null;
                    item.TaskTabColor = new System.Drawing.Color();
                    item.TaskAttachmentList = new List<AttachmentsByTask>(statusUpdateCommentViewModel.AttachmentObjectList);

                    if (item.ActionPlanLogEntries == null) item.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();

                    item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                    {
                        IdActionPlan = item.IdActionPlan,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("TaskLStatusChangeLog").ToString(), sourceStatus.Value, destStatus.Value, item.TaskNumber)
                    });
                    item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                    {
                        IdActionPlan = item.IdActionPlan,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("AddActionPlanTaskChangeLogComment").ToString(), item.TaskNumber, statusUpdateCommentViewModel.Comment)
                    });

                    // Gravar na BD
                    IsStatusSaved = APMService.UpdateStatusByIdTask_V2620(item, destStatus.IdLookupValue, DraggedTaskComment, GeosApplication.Instance.ActiveUser.IdUser);

                    // --- B. Atualizar Objeto em Memória ---
                    item.IdLookupStatus = destStatus.IdLookupValue;
                    item.Status = destStatus.Value;

                    // Cores
                    if (destStatus.Value == "Done" || destStatus.Value == "Closed")
                    {
                        item.TaskTabColor = System.Drawing.Color.Green;
                        item.TaskTabBrush = new SolidColorBrush(Colors.Green);
                        item.CardDueColor = "#008000";
                    }
                    else
                    {
                        if (DateTime.Now.Date < item.DueDate.Date.AddDays(2))
                        {
                            item.CardDueColor = "#008000";
                            item.TaskTabColor = System.Drawing.Color.Green;
                            item.TaskTabBrush = new SolidColorBrush(Colors.Green);
                        }
                        else if (DateTime.Now.Date < item.DueDate.Date.AddDays(7))
                        {
                            item.CardDueColor = "#FFB913";
                            item.TaskTabColor = System.Drawing.Color.Orange;
                            item.TaskTabBrush = new SolidColorBrush(Colors.Orange);
                        }
                        else
                        {
                            item.CardDueColor = "#FF0000";
                            item.TaskTabColor = System.Drawing.Color.Red;
                            item.TaskTabBrush = new SolidColorBrush(Colors.Red);
                        }
                    }

                    // --- C. ATUALIZAÇÃO VISUAL DAS COLUNAS (Sem Refresh de página) ---
                    if (ListStatus != null)
                    {
                        // 1. Remover da coluna de Origem
                        var colOrigem = ListStatus.FirstOrDefault(c => c.IdLookupValue == sourceStatus.IdLookupValue);
                        if (colOrigem != null && colOrigem.TaskList != null)
                        {
                            var itemToRemove = colOrigem.TaskList.FirstOrDefault(t => t.IdActionPlanTask == item.IdActionPlanTask);
                            if (itemToRemove != null)
                            {
                                // Criar nova lista para forçar o binding a atualizar
                                var novaLista = new List<APMActionPlanTask>(colOrigem.TaskList);
                                novaLista.Remove(itemToRemove);
                                colOrigem.TaskList = novaLista;
                            }
                        }

                        // 2. Adicionar na coluna de Destino
                        var colDestino = ListStatus.FirstOrDefault(c => c.IdLookupValue == destStatus.IdLookupValue);
                        if (colDestino != null)
                        {
                            var novaLista = new List<APMActionPlanTask>(colDestino.TaskList ?? new List<APMActionPlanTask>());
                            if (!novaLista.Any(t => t.IdActionPlanTask == item.IdActionPlanTask))
                            {
                                novaLista.Add(item);
                                colDestino.TaskList = novaLista; // Força atualização visual da coluna destino
                            }
                        }
                    }

                    // --- D. Atualizar Caches para consistência noutras vistas ---
                    if (_apCache != null)
                    {
                        foreach (var plan in _apCache)
                        {
                            if (plan.TaskList != null)
                            {
                                var t = plan.TaskList.FirstOrDefault(x => x.IdActionPlanTask == item.IdActionPlanTask);
                                if (t != null)
                                {
                                    t.IdLookupStatus = item.IdLookupStatus;
                                    t.Status = item.Status;
                                    t.CardDueColor = item.CardDueColor;
                                    t.TaskTabColor = item.TaskTabColor;
                                    t.TaskTabBrush = item.TaskTabBrush;
                                }
                            }
                        }
                    }
                    if (TaskGridList != null)
                    {
                        var t = TaskGridList.FirstOrDefault(x => x.IdActionPlanTask == item.IdActionPlanTask);
                        if (t != null)
                        {
                            t.IdLookupStatus = item.IdLookupStatus;
                            t.Status = item.Status;
                            t.CardDueColor = item.CardDueColor;
                            t.TaskTabColor = item.TaskTabColor;
                            t.TaskTabBrush = item.TaskTabBrush;
                        }
                    }
                }
                else
                {
                    // O utilizador clicou em "Cancel" no popup.
                    // Como definimos 'obj.Handled = true' no início e não mudámos as listas,
                    // o cartão volta automaticamente para a posição original.
                    return;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in UpdateTaskStatus: " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[shweta.thube][GEOS2-5976]
        private void OnDroppedAction(ListBoxDroppedEventArgs e)
        {
            // Mantenha este método VAZIO.
            // Toda a lógica (Popup, Save, Refresh UI) foi movida para o método UpdateTaskStatus 
            // para resolver o problema do cartão cinzento e do refresh.

            GeosApplication.Instance.Logger.Log("OnDroppedAction skipped (Logic handled in UpdateTaskStatus).", category: Category.Info, priority: Priority.Low);
        }

        private void AttachmentsHyperClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AttachmentsHyperClickCommandAction....", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                GridControl gridControl = (GridControl)obj;
                TableView detailView = (TableView)gridControl.View;
                APMActionPlanTask selectedTask = (APMActionPlanTask)detailView.DataControl.CurrentItem;
                TaskAttachmentsView taskAttachmentsView = new TaskAttachmentsView();
                TaskAttachmentsViewModel taskAttachmentsViewModel = new TaskAttachmentsViewModel();
                EventHandler handle = delegate { taskAttachmentsView.Close(); };
                taskAttachmentsViewModel.RequestClose += handle;
                taskAttachmentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("TaskAttachmentsHeader").ToString();
                taskAttachmentsViewModel.Init(selectedTask.IdActionPlanTask);
                taskAttachmentsView.DataContext = taskAttachmentsViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                taskAttachmentsView.Owner = Window.GetWindow(ownerInfo);
                taskAttachmentsView.ShowDialog();

                if (taskAttachmentsViewModel.IsSave)
                {
                    IsActionPlanEdit = true;
                    SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == selectedTask.IdActionPlan);
                    var index = SelectedActionPlan.TaskList.FindIndex(x => x.IdActionPlanTask == selectedTask.IdActionPlanTask);
                    if (index >= 0)
                    {
                        SelectedActionPlan.TaskList[index].FileCount = taskAttachmentsViewModel.ListAttachment.Count;
                    }

                    if (ClonedActionPlanList != null)
                    {
                        ClonedActionPlanList.FirstOrDefault(x => x.IdActionPlan == SelectedActionPlan.IdActionPlan).TaskList[index].FileCount = taskAttachmentsViewModel.ListAttachment.Count;
                    }
                    FillLeftTileList();
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                    FilterCommandAction(obj);
                    CommandAlertFilterTileClickAction(obj);
                    if (PreviousSelectedAlertTileBarItem != null)
                        SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedAlertTileBarItem.Caption);
                    IsActionPlanEdit = false;
                    IsExpand = false;
                    //ExpandAndCollapseActionPlanCommandAction(obj);
                    // Init();
                }


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AttachmentsHyperClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AttachmentsHyperClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.jangra][GEOS2-6593]
        private void ActionPlanChildGridControlLoadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ActionPlanChildGridControlLoadedCommandAction...", category: Category.Info, priority: Priority.Low);
                int visibleFalseColumn = 0;
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;

                gridControl.BeginInit();

                if (File.Exists(ActionPlanChildGridSettingFilePath))
                {
                    gridControl.RestoreLayoutFromXml(ActionPlanChildGridSettingFilePath);
                }

                //This code for save grid layout.
                gridControl.SaveLayoutToXml(ActionPlanChildGridSettingFilePath);

                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, ActionPlanChildVisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, ActionPlanChildVisibleIndexChanged);
                    }

                    if (!column.Visible)
                    {
                        visibleFalseColumn++;
                    }
                }

                if (visibleFalseColumn > 0)
                {
                    IsActionPlanChildColumnChooserVisible = true;
                }
                else
                {
                    IsActionPlanChildColumnChooserVisible = false;
                }
                gridControl.EndInit();
                tableView.SearchString = null;
                tableView.ShowGroupPanel = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ActionPlanChildGridControlLoadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on ActionPlanChildGridControlLoadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.jangra][GEOS2-6593]
        private void ActionPlanChildItemListTableViewLoadedCommandAction(object obj)
        {
            TableView tableView = obj as TableView;
            tableView.ColumnChooserState = new DefaultColumnChooserState
            {
                Location = new System.Windows.Point(20, 180),
                Size = new System.Windows.Size(250, 250)
            };
        }

        //[Sudhir.jangra][GEOS2-6593]
        private void ActionPlanChildGridControlUnloadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ActionPlanChildGridControlUnloadedCommandAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(ActionPlanChildGridSettingFilePath);
                GeosApplication.Instance.Logger.Log("Method ActionPlanChildGridControlUnloadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on ActionPlanChildGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-6596]
        private void FillDepartmentList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDepartmentList ...", category: Category.Info, priority: Priority.Low);
                if (APMCommon.Instance.DepartmentList == null)
                {
                    //APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    //APMCommon.Instance.DepartmentList = new List<LookupValue>(APMService.GetLookupValues_V2550(21));
                    APMCommon.Instance.DepartmentList = new List<Department>(APMService.GetDepartmentsForActionPlan_V2590());

                }
                GeosApplication.Instance.Logger.Log("Method FillDepartmentList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartmentList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartmentList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartmentList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6453]
        public void SwitchToTaskGridCommandAction(object obj)
        {
            try
            {
                ResetFilterState();

                TaskGridVisibility = Visibility.Visible;
                TaskViewVisibility = Visibility.Collapsed;
                GridViewVisibility = Visibility.Collapsed;
                IsTaskGridVisibility = true;
                IsActionPlanColumnChooserVisible = false;
                TopTaskListOfFilterTile = new ObservableCollection<TileBarFilters>();
                AddTaskCustomSetting();
                SelectedTaskTopTileBarItem = TopTaskListOfFilterTile.FirstOrDefault();
                IsTopTaskDockChecked = false;

                if (ListOfFilterTile != null)
                {
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault();
                }
                CustomFilterStringName = "All";
                MyFilterString = string.Empty;
                MyChildFilterString = string.Empty;
                MyTaskFilterString = string.Empty;

                FilterCommandActionForTask(null);

                SelectedLocation = new List<object>();
                SelectedPerson = new List<object>();
                SelectedBussinessUnit = new List<object>();
                SelectedOrigin = new List<object>();
                SelectedDepartment = new List<object>();
                SelectedActionPlansCodeForTask = new List<object>();
                SelectedLocationForTask = new List<object>();
                SelectedPersonForTask = new List<object>();
                SelectedBussinessUnitForTask = new List<object>();
                SelectedOriginForTask = new List<object>();

                IsActionPlanAllLocationSelected = false;
                IsActionPlanAllResponsibleSelected = false;
                IsActionPlanAllBUSelected = false;
                IsActionPlanAllOriginSelected = false;
                IsActionPlanAllDepartmentSelected = false;
                IsActionPlanAllActionPlanSelected = false;
                IsActionPlanAllActionPlanSelectedForTask = false;
                IsActionPlanAllLocationSelectedForTask = false;
                IsActionPlanAllResponsibleSelectedForTask = false;
                IsActionPlanAllBUSelectedForTask = false;
                IsActionPlanAllOriginSelectedForTask = false;
                IsActionPlanAllCustomerSelected = false;
                //[shweta.thube][GEOS2-7217][28.07.2025]
                SelectedAlertTileBarItem = null;
                FillAlertSectionList();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SwitchToTaskGridCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SwitchToTaskGridCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SwitchToTaskGridCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SwitchToTaskGridCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5978]
        private void AddActionPlanButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddActionPlanButtonCommandAction....", category: Category.Info, priority: Priority.Low);
                if (IsTaskGridVisibility)
                {
                    AddEditTaskViewModel addEditTaskViewModel = new AddEditTaskViewModel();
                    AddEditTaskView addEditTaskView = new AddEditTaskView();
                    EventHandler handle = delegate { addEditTaskView.Close(); };
                    addEditTaskViewModel.RequestClose += handle;
                    addEditTaskViewModel.IsNew = true;
                    addEditTaskViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddActionPlansHeader").ToString();
                    List<APMActionPlan> temp = new List<APMActionPlan>();
                    temp = new List<APMActionPlan>(TempActionPlanList);
                    APMCommon.Instance.ActionPlanList = new List<APMActionPlan>(temp.OrderBy(ap => ap.Code));
                    APMActionPlan getAPMActionPlan = new APMActionPlan();
                    if (SelectedActionPlanTask != null)
                    {
                        getAPMActionPlan = TempActionPlanList.Where(w => w.IdActionPlan == SelectedActionPlanTask.IdActionPlan).FirstOrDefault();
                    }
                    else
                    {
                        getAPMActionPlan = TempActionPlanList.Where(w => w.IdActionPlan == TaskGridList.FirstOrDefault().IdActionPlan).FirstOrDefault();
                        //getAPMActionPlan.Code = string.Empty;
                    }
                    addEditTaskViewModel.OtItemVisibility = Visibility.Collapsed; //[shweta.thube][GEOS2-6912]
                    addEditTaskViewModel.Init(getAPMActionPlan);
                    //addEditTaskViewModel.Init(SelectedActionPlan);
                    addEditTaskView.DataContext = addEditTaskViewModel;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    addEditTaskView.ShowDialogWindow();

                    if (addEditTaskViewModel.IsSave)
                    {
                        try
                        {
                            //TaskGridList.Add(addEditTaskViewModel.TempTask);
                            //SelectedActionPlanTask = addEditTaskViewModel.TempTask;
                            APMActionPlan aPMActionPlan = ActionPlanList.Where(w => w.IdActionPlan == TaskGridList.FirstOrDefault().IdActionPlan).FirstOrDefault();
                            APMActionPlanTask aPMActionPlanTask = TaskGridList.Where(w => w.IdActionPlan == addEditTaskViewModel.TempTask.IdActionPlan).FirstOrDefault();
                            if (aPMActionPlan.TaskList == null)
                            {
                                aPMActionPlan.TaskList = new List<APMActionPlanTask>();
                            }
                            aPMActionPlan.TaskList.Add(addEditTaskViewModel.TempTask);
                            SelectedActionPlan = aPMActionPlan;
                            SelectedActionPlanTask = addEditTaskViewModel.TempTask;
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in Method AddActionPlanButtonCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                        //if (SelectedActionPlan.TaskList == null)
                        //{
                        //    SelectedActionPlan.TaskList = new List<APMActionPlanTask>();
                        //}
                        //SelectedActionPlan.TaskList.Add(addEditTaskViewModel.TempTask);


                        TaskGridList.Add(addEditTaskViewModel.TempTask);
                        if (ClonedActionPlanList != null)
                        {
                            var cloneddata = ClonedActionPlanList.FirstOrDefault(x => x.IdActionPlan == addEditTaskViewModel.SelectedActionPlans.IdActionPlan);
                            if (cloneddata.TaskList == null)
                            {
                                cloneddata.TaskList = new List<APMActionPlanTask>();
                            }
                            cloneddata.TaskList.Add(addEditTaskViewModel.TempTask);
                        }
                        IsActionPlanEdit = true;
                        FillLeftTileList();
                        SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                        //FilterCommandAction(obj);
                        FilterCommandActionForTask(obj);
                        FillAlertSectionList(); //[shweta.thube][GEOS2-7217][28.07.2025]
                        CommandAlertFilterTileClickAction(obj);
                        if (PreviousSelectedAlertTileBarItem != null)
                            SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedAlertTileBarItem.Caption);
                        TopTaskListOfFilterTile = new ObservableCollection<TileBarFilters>();
                        AddTaskCustomSetting();
                        if (PreviousSelectedTaskTopTileBarItem != null)
                        {
                            SelectedTaskTopTileBarItem = TopTaskListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTaskTopTileBarItem.Caption);
                            if (SelectedTopTileBarItem.Caption != "All")
                            {
                                MyTaskFilterString = SelectedTaskTopTileBarItem.FilterCriteria;
                            }
                            else
                            {
                                MyTaskFilterString = string.Empty;
                            }
                        }


                        if (!ListOfPersonForTask.Any(person => person.EmployeeCode == addEditTaskViewModel.TempTask.EmployeeCode))
                        {
                            ListOfPersonForTask.Add(new Responsible()
                            {
                                IdEmployee = (UInt32)addEditTaskViewModel.TempTask.IdEmployee,
                                EmployeeCode = addEditTaskViewModel.TempTask.EmployeeCode,
                                IdGender = addEditTaskViewModel.TempTask.IdGender,
                                FullName = addEditTaskViewModel.TempTask.Responsible,
                                EmployeeCodeWithIdGender = addEditTaskViewModel.TempTask.EmployeeCode + "_" + addEditTaskViewModel.TempTask.IdGender,
                                IsTaskField = true,
                                ResponsibleDisplayName = addEditTaskViewModel.TempTask.TaskResponsibleDisplayName
                            });
                        }

                        if (ActionPlanCodeList != null)
                        {
                            if (!ActionPlanCodeList.Any(x => x.Code == addEditTaskViewModel.TempTask.Code))
                            {
                                ActionPlanCodeList = new List<APMActionPlan>(ClonedActionPlanList.Where(ap => ap.TaskList != null && ap.TaskList.Count > 0)
                                    .GroupBy(ap => ap.IdActionPlan)
                                    .Select(g => g.OrderBy(ap => ap.Code).First()) // Order by Code and select the first item
                                    .OrderBy(ap => ap.Code) // Order the final list by Code
                                    .ToList());
                            }
                        }
                        IsActionPlanEdit = false;
                        //Init();
                    }
                }
                //[shweta.thube][GEOS2-6453]
                else
                {
                    TableView detailView = (TableView)obj;
                    AddEditActionPlanView addEditActionPlanView = new AddEditActionPlanView();
                    AddEditActionPlanViewModel addEditActionPlanViewModel = new AddEditActionPlanViewModel();
                    EventHandler handle = delegate { addEditActionPlanView.Close(); };
                    addEditActionPlanViewModel.RequestClose += handle;
                    addEditActionPlanViewModel.IsNew = true;
                    addEditActionPlanViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddActionPlanTitle").ToString();
                    List<APMActionPlan> temp = new List<APMActionPlan>();
                    temp = new List<APMActionPlan>(TempActionPlanList);
                    APMCommon.Instance.ActionPlanList = new List<APMActionPlan>(temp.OrderBy(ap => ap.Code));
                    addEditActionPlanView.DataContext = addEditActionPlanViewModel;
                    var ownerInfo = (detailView as FrameworkElement);
                    addEditActionPlanView.Owner = Window.GetWindow(ownerInfo);
                    addEditActionPlanViewModel.Init();
                    addEditActionPlanView.ShowDialog();

                    if (addEditActionPlanViewModel.IsSave)
                    {
                        if (ActionPlanList == null)
                        {
                            ActionPlanList = new ObservableCollection<APMActionPlan>();
                        }
                        if (ClonedActionPlanList == null)
                        {
                            ClonedActionPlanList = new List<APMActionPlan>();
                        }
                        ActionPlanList.Add(addEditActionPlanViewModel.AddedActionPlan);
                        ClonedActionPlanList.Add((APMActionPlan)addEditActionPlanViewModel.AddedActionPlan.Clone());
                        IsActionPlanEdit = true;
                        FillLeftTileList();
                        SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                        FilterCommandAction(obj);
                        TopListOfFilterTile = new ObservableCollection<TileBarFilters>();
                        AddCustomSetting();
                        if (PreviousSelectedTopTileBarItem != null)
                        {
                            SelectedTopTileBarItem = TopListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTopTileBarItem.Caption);
                            if (SelectedTopTileBarItem.Caption != "All")
                            {
                                MyFilterString = SelectedTopTileBarItem.FilterCriteria;
                            }
                            else
                            {
                                MyFilterString = string.Empty;
                            }
                        }
                        FillActionPlansAlertSectionList(); //[shweta.thube][GEOS2-7217][28.07.2025]
                        CommandAlertFilterTileClickAction(obj);
                        if (PreviousSelectedAlertTileBarItem != null)
                            SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedAlertTileBarItem.Caption);
                        FillResponsibleList();
                        IsActionPlanEdit = false;
                        FillCustomerList();//[shweta.thube][GEOS2-6911]
                        //[pallavi.kale][GEOS2-7213][25.07.2025]
                        LastUpdatedIdActionPlan = addEditActionPlanViewModel.AddedActionPlan.IdActionPlan;

                        if (addEditActionPlanViewModel.AddedActionPlan != null)
                        {
                            var newAddedActionPlan = addEditActionPlanViewModel.AddedActionPlan;
                            if (newAddedActionPlan.TaskList != null && newAddedActionPlan.TaskList.Any())
                            {
                                LastUpdatedIdActionPlan = newAddedActionPlan.IdActionPlan;
                                //  LastUpdatedIdActionPlanTask = -1;
                                IsExpandTaskLevelOnly = true;
                                IsExpand = true;
                                ExpandLastUpdatedActionPlan();
                            }
                            else
                            {
                                IsExpand = false;
                            }
                        }
                        // Init();
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddActionPlanButtonCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddActionPlanButtonCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5983]
        private void FillAlertSectionList()
        {
            try
            {
                if (TaskGridList != null && TaskGridList.Count > 0)
                {
                    AlertListOfFilterTile = new ObservableCollection<APMAlertTileBarFilters>();

                    #region Longest Over Due
                    //[shweta.thube][GEOS2-7904]
                    var overdueTasks = TaskGridList
    .Where(task => task.DueDate < DateTime.Now && task.IdLookupStatus != 1982);

                    var longestOverdueTask = overdueTasks
                        .OrderByDescending(task => (DateTime.Now - task.DueDate).Days)
                        .FirstOrDefault();

                    var longestOverdueDays = longestOverdueTask != null
                        ? longestOverdueTask.DueDays : 0;

                    var backColor = longestOverdueDays >= 5
                        ? "Red" : longestOverdueDays > 0
                            ? "Orange" : "Green";

                    var filterCriteria = longestOverdueTask != null && longestOverdueDays >= 5
                          ? $"[Code] = '{longestOverdueTask.Code}' AND [TaskNumber] = '{longestOverdueTask.TaskNumber}'" : "[Code] IS NULL";
                    //[GEOS2-7238][shweta.thube]
                    AlertListOfFilterTile.Add(new APMAlertTileBarFilters()
                    {
                        Caption = "Longest Overdue Days",
                        Id = 0,
                        BackColor = backColor,
                        EntitiesCount = longestOverdueDays >= 5
                            ? $"{longestOverdueDays}"
                            : "0",
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 50,
                        width = 150,
                        FilterCriteria = filterCriteria,
                    });
                    #endregion

                    #region Over Due
                    int overdueThresholdDays = 15;
                    DateTime thresholdDate = DateTime.Now.AddDays(-overdueThresholdDays);
                    //[shweta.thube][GEOS2-7904]
                    var overdueThresholdCount = TaskGridList
                        .Count(task => (DateTime.Now - task.DueDate).TotalDays >= overdueThresholdDays && task.IdLookupStatus != 1982);
                    var backColorOverDue = overdueThresholdCount == 0 ? "Green" :
                                    overdueThresholdCount < 5 ? "Orange" : "Red";

                    AlertListOfFilterTile.Add(new APMAlertTileBarFilters()
                    {
                        Caption = "Overdue >= 15 days",
                        Id = 1,
                        BackColor = backColorOverDue,
                        EntitiesCount = $"{overdueThresholdCount}",
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 50,
                        width = 150,
                        FilterCriteria = $"[DueDate] <= #{thresholdDate:yyyy-MM-ddTHH:mm:ss}#"
                    });
                    #endregion

                    #region High Priority Over Due
                    int highPriorityOverdueDays = 5;
                    DateTime highPriorityThresholdDate = DateTime.Now.AddDays(-highPriorityOverdueDays);
                    //[shweta.thube][GEOS2-7904]
                    var highPriorityOverdueTask = TaskGridList
                        .Where(task => (DateTime.Now - task.DueDate).TotalDays >= highPriorityOverdueDays
                                       && task.Priority == "High" && task.IdLookupStatus != 1982)
                        .OrderByDescending(task => (DateTime.Now - task.DueDate).TotalDays)
                        .FirstOrDefault();
                    //[shweta.thube][GEOS2-7904]
                    var highPriorityOverdueCount = highPriorityOverdueTask != null
                        ? TaskGridList.Count(task => (DateTime.Now - task.DueDate).TotalDays >= highPriorityOverdueDays && task.Priority == "High" && task.IdLookupStatus != 1982)
                        : 0;

                    var backColorHighPriority = highPriorityOverdueCount == 0 ? "Green" :
                        highPriorityOverdueCount < 5 ? "Orange" : "Red";

                    var highPriorityFilterCriteria = highPriorityOverdueTask != null
                        ? $"[DueDate] <= #{highPriorityThresholdDate:yyyy-MM-ddTHH:mm:ss}# AND [Priority] = 'High'"
                        : $"[DueDate] <= #{highPriorityThresholdDate:yyyy-MM-ddTHH:mm:ss}# AND [Priority] = 'High'";

                    AlertListOfFilterTile.Add(new APMAlertTileBarFilters()
                    {
                        Caption = "High Priority Overdue",
                        Id = 2,
                        BackColor = backColorHighPriority,
                        EntitiesCount = $"{highPriorityOverdueCount}",
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 50,
                        width = 150,
                        FilterCriteria = highPriorityFilterCriteria
                    });
                    #endregion

                    #region Epic With Most Over Due
                    //[GEOS2-7238][shweta.thube]
                    var overdueThreshold = DateTime.Now.AddDays(-5);
                    var formattedThresholdDate = overdueThreshold.ToString("yyyy-MM-dd HH:mm:ss");

                    var overdueTasksFilterCriteria = $"[DueDate] < #{formattedThresholdDate}#";
                    //[shweta.thube][GEOS2-7904]
                    var groupedTasks = TaskGridList
                        .Where(task => task.DueDate < overdueThreshold && task.IdLookupStatus != 1982)
                        .GroupBy(task => task.Theme)
                        .OrderByDescending(group => group.Count())
                        .ToList();

                    var mostOverdueGroup = groupedTasks.FirstOrDefault();

                    var themeWithMostOverdueTasks = mostOverdueGroup?.Key ?? "---";
                    var overdueCount = mostOverdueGroup?.Count() ?? 0;

                    var specificTaskFilterCriteria = mostOverdueGroup != null
                        ? $"[Theme] = '{themeWithMostOverdueTasks.Replace("'", "''")}'"
                        : "[Theme] IS NULL";

                    var combinedFilterCriteria = mostOverdueGroup != null
                        ? $"{overdueTasksFilterCriteria} AND {specificTaskFilterCriteria}"
                        : overdueTasksFilterCriteria;

                    AlertListOfFilterTile.Add(new APMAlertTileBarFilters()
                    {
                        Caption = "Most Overdue Theme",
                        Id = 0,
                        BackColor = backColor,
                        EntitiesCount = $"{themeWithMostOverdueTasks}",
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 50,
                        width = 150,
                        FilterCriteria = combinedFilterCriteria,
                    });
                    #endregion

                    #region Responsible with Most Over Due
                    int overdueDaysThreshold = 5;

                    DateTime overdueThresholdDate = DateTime.Now.AddDays(-overdueDaysThreshold);
                    //[shweta.thube][GEOS2-7904]
                    var responsibleWithMostOverdue = TaskGridList
                        .Where(task => (DateTime.Now - task.DueDate).TotalDays >= overdueDaysThreshold && task.IdLookupStatus != 1982)
                        .GroupBy(task => task.FirstName)
                        .OrderByDescending(group => group.Count())
                        .FirstOrDefault();

                    string responsibleName = responsibleWithMostOverdue?.Key ?? "---";

                    int overdueTaskCount = responsibleWithMostOverdue?.Count() ?? 0;

                    AlertListOfFilterTile.Add(new APMAlertTileBarFilters()
                    {
                        Caption = "Most Overdue Responsible",
                        Id = 3,
                        BackColor = backColor,
                        EntitiesCount = $"{responsibleName}",
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 50,
                        width = 150,
                        FilterCriteria = $"[FirstName] = '{responsibleName}' AND [DueDate] <= #{overdueThresholdDate:yyyy-MM-ddTHH:mm:ss}#"
                    });
                    #endregion

                    #region Sorting Indicators
                    AlertListOfFilterTile = new ObservableCollection<APMAlertTileBarFilters>(AlertListOfFilterTile
                        .Take(5).OrderBy(tile =>
                        {
                            switch (tile.BackColor)
                            {
                                case "Red": return 1;
                                case "Orange": return 2;
                                case "Green": return 3;
                                default: return 4;
                            }
                        }).Concat(AlertListOfFilterTile.Skip(5)).ToList());
                    #endregion

                    #region Status
                    //APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    IList<LookupValue> temp = APMService.GetLookupValues_V2550(152).ToList();//TaskThemes

                    temp.OrderBy(x => x.Position);

                    foreach (var item in temp)
                    {
                        AlertListOfFilterTile.Add(new APMAlertTileBarFilters()
                        {
                            Caption = item.Value,
                            Id = item.IdLookupValue,
                            BackColor = item.HtmlColor,
                            EntitiesCount = TaskGridList.Count(task => task.IdLookupStatus == item.IdLookupValue).ToString(),
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 50,
                            width = 150,
                            FilterCriteria = $"StartsWith([Status], '{item.Value}')"
                        });
                    }

                    #endregion



                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAlertSectionList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAlertSectionList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAlertSectionList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //[Sudhir.Jangra][GEOS2-5983]
        private void CommandAlertFilterTileClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandAlertFilterTileClickAction....", category: Category.Info, priority: Priority.Low);

                if (obj is MouseButtonEventArgs mouseArgs)
                {
                    if (mouseArgs.Source is System.Windows.Controls.Control clickedControl)
                    {
                        var clickedItem = clickedControl.DataContext as APMAlertTileBarFilters;

                        if (clickedItem != null)
                        {
                            string str = clickedItem.DisplayText;
                            string _FilterString = clickedItem.FilterCriteria;
                            string normalizedMyTaskFilter = RemoveTimeFromFilterString(MyTaskFilterString);
                            string normalizedFilter = RemoveTimeFromFilterString(_FilterString);
                            // bool isEqual = normalizedMyTaskFilter == normalizedFilter;

                            if (!string.IsNullOrEmpty(MyTaskFilterString))
                            {
                                //[shweta.thube][GEOS2-7904]
                                if (normalizedMyTaskFilter == normalizedFilter)
                                {
                                    MyTaskFilterString = string.Empty;
                                    ActionPlanList = new ObservableCollection<APMActionPlan>(TempActionPlanList);
                                    SelectedAlertTileBarItem = new APMAlertTileBarFilters();
                                    PreviousSelectedAlertTileBarItem = null;//[shweta.thube][GEOS2-7904]
                                    TemPreviousSelectedTileBarItem = null;
                                    FilterCommandActionForTask(obj);//[shweta.thube][GEOS2-7217][28.07.2025]
                                    FillAlertSectionList();//[shweta.thube][GEOS2-7217][28.07.2025]
                                }
                                else
                                {
                                    FilterCommandActionForTask(obj);//[shweta.thube][GEOS2-7217][28.07.2025]
                                    if (string.IsNullOrEmpty(str))
                                    {
                                        MyTaskFilterString = !string.IsNullOrEmpty(_FilterString) ? _FilterString : string.Empty;
                                    }
                                    else
                                    {
                                        if (str.Equals("All"))
                                        {
                                            MyTaskFilterString = string.Empty;
                                            ActionPlanList = new ObservableCollection<APMActionPlan>(TempActionPlanList);
                                        }
                                        else
                                        {
                                            MyTaskFilterString = !string.IsNullOrEmpty(_FilterString) ? _FilterString : string.Empty;
                                        }
                                    }
                                    //[shweta.thube][GEOS2-7904]
                                    if (clickedItem != null)
                                    {
                                        if (clickedItem.Caption == "Longest Overdue Days" || clickedItem.Caption == "Overdue >= 15 days" || clickedItem.Caption == "High Priority Overdue" || clickedItem.Caption == "Most Overdue Theme"
                                                            || clickedItem.Caption == "Most Overdue Responsible")
                                            TaskGridList = new ObservableCollection<APMActionPlanTask>(
                                           TaskGridList.Where(task => task.IdLookupStatus != 1982));

                                        SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == clickedItem.Caption);
                                    }
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(str))
                                {
                                    MyTaskFilterString = !string.IsNullOrEmpty(_FilterString) ? _FilterString : string.Empty;
                                }
                                else
                                {
                                    if (str.Equals("All"))
                                    {
                                        MyTaskFilterString = string.Empty;
                                        ActionPlanList = new ObservableCollection<APMActionPlan>(TempActionPlanList);
                                    }
                                    else
                                    {
                                        MyTaskFilterString = !string.IsNullOrEmpty(_FilterString) ? _FilterString : string.Empty;
                                    }
                                }
                                //[shweta.thube][GEOS2-7904]
                                if (clickedItem != null)
                                {
                                    if (clickedItem.Caption == "Longest Overdue Days" || clickedItem.Caption == "Overdue >= 15 days" || clickedItem.Caption == "High Priority Overdue" || clickedItem.Caption == "Most Overdue Theme"
                                                        || clickedItem.Caption == "Most Overdue Responsible")
                                        TaskGridList = new ObservableCollection<APMActionPlanTask>(
                                       TaskGridList.Where(task => task.IdLookupStatus != 1982));

                                    SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == clickedItem.Caption);
                                }
                            }
                            if (TemPreviousSelectedTileBarItem == null && PreviousSelectedAlertTileBarItem == null && TempSelectedLocation == null && TempSelectedBussinessUnit == null && TempSelectedOrigin == null && TempSelectedPerson == null && TempSelectedActionPlansCodeForTask == null)
                            {

                                TaskGridList = new ObservableCollection<APMActionPlanTask>(TempTaskGridList);
                            }

                            if (SelectedTileBarItem != null && SelectedTileBarItem.Caption != "All")
                            {
                                TaskGridList = new ObservableCollection<APMActionPlanTask>(
                    TempTaskGridList
                    .Where(a => a.Theme == CustomFilterStringName)  // Filter tasks based on the theme
                    .ToList()  // Convert the filtered tasks to a List
                    );
                                //FillAlertSectionList();                                                             
                            }

                            //TaskGridList = new ObservableCollection<APMActionPlanTask>(TaskGridList);

                            //FillAlertSectionList();
                            if (PreviousSelectedAlertTileBarItem != null)
                            {
                                SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == clickedItem.Caption);
                            }

                        }

                    }
                }

                //FilterCommandActionForTask(obj);


                GeosApplication.Instance.Logger.Log("Method CommandAlertFilterTileClickAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method CommandAlertFilterTileClickAction: " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private string RemoveTimeFromFilterString(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return string.Empty;

            return Regex.Replace(filter, @"#(\d{4}-\d{2}-\d{2})[ T]\d{2}:\d{2}:\d{2}#", "#$1#");
        }

        //[shweta.thube][GEOS2-7904]
        private string GetFilterAfterAnd(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return string.Empty;

            var parts = filter.Split(new[] { "AND" }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 1 ? parts[1].Trim() : string.Empty;
        }
        //[shweta.thube][GEOS2-7904]
        private string GetFilterBeforeAnd(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return string.Empty;

            var parts = filter.Split(new[] { "AND" }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 0 ? parts[0].Trim() : string.Empty;
        }
        //[Sudhir.Jangra][GEOS2-5983]
        //private void CommandAlertFilterTileClickAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method CommandAlertFilterTileClickAction....", category: Category.Info, priority: Priority.Low);
        //        if (IsTaskGridVisibility)
        //        {
        //            if (TaskGridList.Count > 0)
        //            {
        //                var temp = (System.Windows.Controls.SelectionChangedEventArgs)obj;
        //                if (temp.AddedItems.Count > 0)
        //                {
        //                    string str = ((APMAlertTileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;
        //                    string Template = ((APMAlertTileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Type;
        //                    string _FilterString = ((APMAlertTileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
        //                    //  CustomFilterStringName = ((APMAlertTileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;
        //                    // CustomFilterHTMLColor = ((APMAlertTileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).BackColor;


        //                    if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
        //                        return;


        //                    if (str == null)
        //                    {
        //                        if (!string.IsNullOrEmpty(_FilterString))
        //                        {

        //                            if (!string.IsNullOrEmpty(_FilterString))
        //                                MyFilterString = _FilterString;
        //                            else
        //                                MyFilterString = string.Empty;
        //                        }
        //                        else
        //                            MyFilterString = string.Empty;
        //                    }
        //                    else
        //                    {
        //                        if (str.Equals("All"))
        //                        {
        //                            MyFilterString = string.Empty;
        //                            ActionPlanList = new ObservableCollection<APMActionPlan>(TempActionPlanList);
        //                        }
        //                        else
        //                        {
        //                            if (!string.IsNullOrEmpty(_FilterString))
        //                            {

        //                                if (!string.IsNullOrEmpty(_FilterString))
        //                                    MyFilterString = _FilterString;
        //                                else
        //                                    MyFilterString = string.Empty;
        //                            }
        //                            else
        //                                MyFilterString = string.Empty;
        //                        }
        //                    }

        //                }

        //            }
        //        }

        //        GeosApplication.Instance.Logger.Log("Method CommandAlertFilterTileClickAction....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method CommandAlertFilterTileClickAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //[Sudhir.Jangra][GEOS2-5983]
        private void ExpandAndCollapseActionPlanCommandAction(object obj)
        {
            if (!DXSplashScreen.IsActive)
            {
                DXSplashScreen.Show(x =>
                {
                    Window win = new Window()
                    {
                        ShowActivated = false,
                        WindowStyle = WindowStyle.None,
                        ResizeMode = ResizeMode.NoResize,
                        AllowsTransparency = true,
                        Background = new SolidColorBrush(Colors.Transparent),
                        ShowInTaskbar = false,
                        Topmost = true,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    };
                    WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = false;
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }

            GridControl t = (GridControl)obj;
            if (IsExpand)
            {
                for (int i = 0; i < t.VisibleRowCount; i++)
                {
                    var rowHandle = t.GetRowHandleByVisibleIndex(i);
                    t.CollapseMasterRow(rowHandle);
                }
                IsExpand = false;
            }
            else
            {
                for (int i = 0; i < t.VisibleRowCount; i++)
                {
                    var rowHandle = t.GetRowHandleByVisibleIndex(i);
                    t.ExpandMasterRow(rowHandle);
                }
                IsExpand = true;
            }
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        }

        //[shweta.thube][GEOS2-6795]
        public void DeleteActionPlanCommandAction(object obj)
        {
            try
            {
                APMActionPlan Temp = (APMActionPlan)obj;
                if (Temp.TaskList == null || Temp.TaskList.Count == 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteActionplanDetailsMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        IsDeleted = APMService.DeleteActionPlan_V2600(Temp.IdActionPlan);//[shweta.thube][GEOS2-6795]
                        var temp = ActionPlanList.FirstOrDefault(i => i.IdActionPlan == Temp.IdActionPlan);
                        ActionPlanList.Remove(temp);

                        int index = ClonedActionPlanList.FindIndex(x => x.IdActionPlan == temp.IdActionPlan);
                        if (index != -1)
                        {
                            ClonedActionPlanList.RemoveAt(index);
                        }

                        TopListOfFilterTile = new ObservableCollection<TileBarFilters>();
                        AddCustomSetting();
                        if (PreviousSelectedTopTileBarItem != null)
                        {
                            SelectedTopTileBarItem = TopListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTopTileBarItem.Caption);
                            if (SelectedTopTileBarItem.Caption != "All")
                            {
                                MyFilterString = SelectedTopTileBarItem.FilterCriteria;
                            }
                            else
                            {
                                MyFilterString = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        IsDeleted = false;
                    }

                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteActionplanMessage").ToString(), Temp.Code), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    IsDeleted = false;
                }
                if (IsDeleted)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionPlanDetailsDeletedsuccessfully").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    IsExpand = false;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DeleteActionPlanCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteActionPlanCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteActionPlanCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteActionPlanCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6453]
        private void TaskFilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            try
            {
                obj.Handled = true;
                TableView table = (TableView)obj.OriginalSource;
                GridControl gridControl = (table).Grid;
                ShowTaskFilterEditor(obj);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in TaskFilterEditorCreatedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6453]
        private void ShowTaskFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowTaskFilterEditor()...", category: Category.Info, priority: Priority.Low);
                CustomActionPlanFilterEditorView customFilterEditorView = new CustomActionPlanFilterEditorView();
                CustomActionPlanFilterEditorViewModel customFilterEditorViewModel = new CustomActionPlanFilterEditorViewModel();
                string titleText = DevExpress.Xpf.Grid.GridControlLocalizer.Active.GetLocalizedString(GridControlStringId.FilterEditorTitle);
                if (IsEdit)
                {
                    if (!string.IsNullOrEmpty(CustomTaskFilterStringName))
                    {
                        customFilterEditorViewModel.FilterName = CustomTaskFilterStringName;
                    }
                    else
                    {
                        customFilterEditorViewModel.FilterName = SelectedTaskTopTileBarItem.Caption;
                    }
                    if (!string.IsNullOrEmpty(CustomTaskFilterHTMLColor))
                    {
                        customFilterEditorViewModel.HTMLColor = CustomTaskFilterHTMLColor;
                    }
                    else
                    {
                        customFilterEditorViewModel.HTMLColor = SelectedTaskTopTileBarItem.BackColor;
                    }


                    customFilterEditorViewModel.IsSave = true;
                    customFilterEditorViewModel.IsNew = false;
                    IsEdit = false;
                }
                else
                    customFilterEditorViewModel.IsNew = true;
                if (TopTaskListOfFilterTile == null)
                {
                    TopTaskListOfFilterTile = new ObservableCollection<TileBarFilters>();
                }


                customFilterEditorViewModel.Init(e.FilterControl, TopTaskListOfFilterTile);


                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");

                customFilterEditorView.Grid.Children.Add(e.FilterControl);


                customFilterEditorView.ShowDialog();

                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    if (TopTaskListOfFilterTile == null)
                    {
                        TopTaskListOfFilterTile = new ObservableCollection<TileBarFilters>();
                    }
                    TileBarFilters tileBarItem = TopTaskListOfFilterTile.FirstOrDefault(x => x.Caption.Equals(CustomTaskFilterStringName));

                    if (tileBarItem != null)
                    {
                        TopTaskListOfFilterTile.Remove(tileBarItem);
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;
                            string htmlColor = "";
                            string keyName = "";
                            if (setting.Key.Contains(userSettingsKeyForActionPlanTask))
                            {
                                key = setting.Key.Replace(userSettingsKeyForActionPlanTask, "");
                                string _html = key.Replace(tileBarItem.Caption, "");
                                htmlColor = _html.Replace("_", "");
                                if (!string.IsNullOrEmpty(htmlColor))
                                {
                                    keyName = key.Replace(_html, "");
                                }

                            }
                            if (keyName.Equals(tileBarItem.Caption) && !string.IsNullOrEmpty(htmlColor))
                            {
                                continue;
                            }



                            if (!key.Equals(tileBarItem.Caption))
                            {
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            }

                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                        TopTaskListOfFilterTile = new ObservableCollection<TileBarFilters>();
                        AddTaskCustomSetting();
                        if (TopTaskListOfFilterTile.Count == 1)
                        {
                            TopTaskListOfFilterTile = new ObservableCollection<TileBarFilters>();
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && !customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    if (TopTaskListOfFilterTile == null)
                    {
                        TopTaskListOfFilterTile = new ObservableCollection<TileBarFilters>();
                    }
                    TileBarFilters tileBarItem = TopTaskListOfFilterTile.FirstOrDefault(x => x.Caption.Equals(CustomTaskFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomTaskFilterStringName = customFilterEditorViewModel.FilterName;
                        CustomTaskFilterHTMLColor = customFilterEditorViewModel.HTMLColor;
                        TableView table = (TableView)e.OriginalSource;
                        GridControl gridControl = (table).Grid;
                        List<DevExpress.Xpf.Grid.GridTotalSummaryData> summary = new List<GridTotalSummaryData>(gridControl.View.FixedSummariesLeft);
                        VisibleRowCount = (Int32)summary.FirstOrDefault().Value;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customFilterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = VisibleRowCount;
                        tileBarItem.EntitiesCountVisibility = Visibility.Visible;
                        tileBarItem.FilterCriteria = customFilterEditorViewModel.FilterCriteria;
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;
                            string htmlColor = "";
                            string keyName = "";

                            if (setting.Key.Contains(userSettingsKeyForActionPlanTask))
                            {
                                key = setting.Key.Replace(userSettingsKeyForActionPlanTask, "");
                                string[] splitted = key.Split('_');
                                if (splitted != null && splitted.Length > 1)
                                {
                                    string _html = key.Replace(filterCaption, "");
                                    htmlColor = _html.Replace("_", "");
                                    if (!string.IsNullOrEmpty(htmlColor))
                                    {
                                        keyName = key.Replace(_html, "");
                                    }
                                }
                            }

                            if (keyName.Equals(filterCaption) && !string.IsNullOrEmpty(htmlColor))
                            {
                                TopTaskListOfFilterTile.Where(a => a.Caption == SelectedTaskTopTileBarItem.Caption).ToList().ForEach(b => b.BackColor = CustomTaskFilterHTMLColor);
                                PreviousSelectedTaskTopTileBarItem = SelectedTaskTopTileBarItem;
                                string name = userSettingsKeyForActionPlanTask + tileBarItem.Caption + "_" + CustomTaskFilterHTMLColor;
                                lstUserConfiguration.Add(new Tuple<string, string>(name.ToString(), setting.Value.ToString()));
                                continue;
                            }


                            if (!key.Equals(filterCaption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            else
                                lstUserConfiguration.Add(new Tuple<string, string>((userSettingsKeyForActionPlanTask + tileBarItem.Caption), tileBarItem.FilterCriteria));


                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                        TopTaskListOfFilterTile = new ObservableCollection<TileBarFilters>(TopTaskListOfFilterTile);
                    }
                }
                else if (customFilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    TableView table = (TableView)e.OriginalSource;
                    GridControl gridControl = (table).Grid;
                    List<DevExpress.Xpf.Grid.GridTotalSummaryData> summary = new List<GridTotalSummaryData>(gridControl.View.FixedSummariesLeft);
                    VisibleRowCount = (Int32)summary.FirstOrDefault().Value;
                    if (TopTaskListOfFilterTile == null)
                    {
                        TopTaskListOfFilterTile = new ObservableCollection<TileBarFilters>();
                    }
                    //[shweta.thube][GEOS2-7219]
                    TopTaskListOfFilterTile.Add(new TileBarFilters()
                    {
                        Caption = customFilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = customFilterEditorViewModel.HTMLColor,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = customFilterEditorViewModel.FilterCriteria,
                        Height = 50,
                        width = 150,
                        EntitiesCount = VisibleRowCount
                    });

                    string filterName = "";

                    filterName = userSettingsKeyForActionPlanTask + customFilterEditorViewModel.FilterName;

                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);

                    if (customFilterEditorViewModel.HTMLColor != null)
                    {
                        string FilterName = "";
                        filterName = userSettingsKeyForActionPlanTask + customFilterEditorViewModel.FilterName + "_" + customFilterEditorViewModel.HTMLColor;
                        GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                        ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                    TopTaskListOfFilterTile = new ObservableCollection<TileBarFilters>();
                    AddTaskCustomSetting();
                    SelectedTaskTopTileBarItem = TopTaskListOfFilterTile.LastOrDefault();
                    TopTaskListOfFilterTile = new ObservableCollection<TileBarFilters>(TopTaskListOfFilterTile.OrderBy(a => a.Caption != "All").ThenBy(a => a.Caption));
                    //SelectedTaskTopTileBarItem = TopTaskListOfFilterTile.LastOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method ShowTaskFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowTaskFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6453]
        private void AddTaskCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddTaskCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = new List<KeyValuePair<string, string>>();
                if (TopTaskListOfFilterTile == null)
                {
                    TopTaskListOfFilterTile = new ObservableCollection<TileBarFilters>();
                }
                tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKeyForActionPlanTask)).ToList();

                //[shweta.thube][GEOS2-7219]
                if (tempUserSettings != null && tempUserSettings.Count > 0)
                {
                    if (!TopTaskListOfFilterTile.Any(x => x.Caption == "All"))
                    {
                        TopTaskListOfFilterTile.Add(
                                      new TileBarFilters()
                                      {
                                          Caption = "All",
                                          Id = 0,
                                          BackColor = null,
                                          ForeColor = null,
                                          FilterCriteria = null,
                                          EntitiesCount = ClonedTaskGridList.Count(),
                                          EntitiesCountVisibility = Visibility.Visible,
                                          Height = 50,
                                          width = 150
                                      });
                    }



                    var colorMapping = new Dictionary<string, string>();
                    foreach (var item in tempUserSettings)
                    {
                        var parts = item.Key.Split(new[] { "_#" }, StringSplitOptions.None);

                        if (parts.Length > 1)
                        {
                            string baseKey = parts[0].Replace(userSettingsKeyForActionPlanTask, "");
                            string colorCode = "#" + parts[1];

                            colorMapping[baseKey] = colorCode;
                        }
                    }
                    foreach (var item in tempUserSettings)
                    {
                        try
                        {
                            string filter = item.Value.Replace("[Status]", "Status");
                            var parts = item.Key.Split(new[] { "_#" }, StringSplitOptions.None);
                            string baseKey = parts[0].Replace(userSettingsKeyForActionPlanTask, "");

                            string backColor = colorMapping.ContainsKey(baseKey) ? colorMapping[baseKey] : null;
                            bool isDuplicate = TopTaskListOfFilterTile.Any(tile => tile.Caption == baseKey && tile.FilterCriteria == item.Value);

                            if (!isDuplicate)
                            {
                                CriteriaOperator op = CriteriaOperator.Parse(filter);

                                int count = 0;
                                if (filter.StartsWith("StartsWith("))
                                {
                                    string filterContent = filter.Substring("StartsWith(".Length).TrimEnd(')');
                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']');
                                    string columnValue = filterParts[1].Trim(' ', '\'');

                                    count = ClonedTaskGridList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        var value = property?.GetValue(ap)?.ToString();
                                        return value != null && value.StartsWith(columnValue, StringComparison.OrdinalIgnoreCase);
                                    });
                                }
                                else if (filter.StartsWith("EndsWith("))
                                {
                                    string filterContent = filter.Substring("EndsWith(".Length).TrimEnd(')');
                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']');
                                    string columnValue = filterParts[1].Trim(' ', '\'');

                                    count = ClonedTaskGridList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        var value = property?.GetValue(ap)?.ToString();
                                        return value != null && value.EndsWith(columnValue, StringComparison.OrdinalIgnoreCase);
                                    });
                                }
                                else if (filter.Contains("Contains(") && !filter.Contains("Not Contains("))
                                {
                                    string filterContent = filter.Substring("Contains(".Length).TrimEnd(')');
                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']');
                                    string columnValue = filterParts[1].Trim(' ', '\'');

                                    count = ClonedTaskGridList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        var value = property?.GetValue(ap)?.ToString();
                                        return value != null && value.IndexOf(columnValue, StringComparison.OrdinalIgnoreCase) >= 0;
                                    });
                                }
                                else if (filter.Contains("Not Contains("))
                                {
                                    string filterContent = filter.Substring("Not Contains(".Length).TrimEnd(')');
                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']');
                                    string columnValue = filterParts[1].Trim(' ', '\'');

                                    count = ClonedTaskGridList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        var value = property?.GetValue(ap)?.ToString();
                                        return value == null || value.IndexOf(columnValue, StringComparison.OrdinalIgnoreCase) < 0;
                                    });
                                }
                                else if (filter.Contains(" In ") && !(filter.Contains("Not") && filter.Contains("In")))
                                {
                                    int startColumnIndex = filter.IndexOf('[') + 1;
                                    int endColumnIndex = filter.IndexOf(']');
                                    string columnName = filter.Substring(startColumnIndex, endColumnIndex - startColumnIndex).Trim();

                                    int startValuesIndex = filter.IndexOf("In (", StringComparison.OrdinalIgnoreCase) + "In (".Length;
                                    int endValuesIndex = filter.IndexOf(')', startValuesIndex);
                                    string filterContent = filter.Substring(startValuesIndex, endValuesIndex - startValuesIndex).Trim();

                                    var columnValues = filterContent
          .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
          .Select(v => v.Trim().Trim('\'', '#'))
          .ToList();

                                    count = ClonedTaskGridList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        if (property != null)
                                        {
                                            var value = property.GetValue(ap);

                                            if (value != null)
                                            {
                                                if (property.PropertyType == typeof(string))
                                                {
                                                    return columnValues.Contains(value.ToString(), StringComparer.OrdinalIgnoreCase);
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    var dateValue = (DateTime)value;
                                                    return columnValues.Contains(dateValue.ToString("yyyy-MM-dd"), StringComparer.OrdinalIgnoreCase);
                                                }
                                                else if (typeof(IComparable).IsAssignableFrom(property.PropertyType))
                                                {
                                                    foreach (var columnValue in columnValues)
                                                    {
                                                        try
                                                        {
                                                            var targetValue = Convert.ChangeType(columnValue, property.PropertyType);
                                                            if (value.Equals(targetValue))
                                                            {
                                                                return true;
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            // Handle conversion exceptions if necessary
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        return false;
                                    });
                                }
                                else if (filter.Contains("Not") && filter.Contains("In"))
                                {
                                    int startIndex = filter.IndexOf('(') + 1;
                                    int endIndex = filter.LastIndexOf(')');
                                    string filterContent = filter.Substring(startIndex, endIndex - startIndex).Trim();

                                    string[] filterParts = filter.Split(new[] { "Not", "In" }, StringSplitOptions.RemoveEmptyEntries);
                                    string columnName = filterParts[0].Trim('[', ']', ' ');

                                    var columnValues = filterContent
                                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(v => v.Trim().Trim('\'', ' ', '#'))
                                        .ToArray();

                                    count = ClonedTaskGridList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        if (property != null)
                                        {
                                            var value = property.GetValue(ap);

                                            if (value != null)
                                            {
                                                if (property.PropertyType == typeof(string))
                                                {
                                                    return !columnValues.Contains(value.ToString(), StringComparer.OrdinalIgnoreCase);
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    var dateValue = (DateTime)value;
                                                    return !columnValues.Contains(dateValue.ToString("yyyy-MM-dd"), StringComparer.OrdinalIgnoreCase);
                                                }
                                                else if (typeof(IComparable).IsAssignableFrom(property.PropertyType))
                                                {
                                                    foreach (var columnValue in columnValues)
                                                    {
                                                        try
                                                        {
                                                            var targetValue = Convert.ChangeType(columnValue, property.PropertyType);
                                                            if (value.Equals(targetValue))
                                                            {
                                                                return false;
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            // Handle conversion exceptions if necessary
                                                        }
                                                    }
                                                    return true;
                                                }
                                            }
                                        }
                                        return true;
                                    });
                                }

                                else if (filter.Contains("Not [") && filter.Contains("Between("))
                                {
                                    var filterContent = filter.Substring(filter.IndexOf("Not [") + "Not [".Length,
                                        filter.IndexOf("Between(") - (filter.IndexOf("Not [") + "Not [".Length)).Trim();

                                    var betweenContent = filter.Substring(filter.IndexOf("Between(") + "Between(".Length).TrimEnd(')');
                                    var filterParts = betweenContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                    if (filterParts.Length == 2)
                                    {
                                        var columnName = filterContent.Trim(' ', '[', ']');
                                        var lowerBound = filterParts[0].Trim().Trim('\'');
                                        var upperBound = filterParts[1].Trim().Trim('\'');

                                        count = ClonedTaskGridList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property != null ? property.GetValue(ap) : null;

                                            if (value != null)
                                            {
                                                if (property.PropertyType == typeof(string))
                                                {
                                                    var stringValue = value as string;
                                                    return string.Compare(stringValue, lowerBound, StringComparison.OrdinalIgnoreCase) < 0 ||
                                                           string.Compare(stringValue, upperBound, StringComparison.OrdinalIgnoreCase) > 0;
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    DateTime lowerDate, upperDate;

                                                    bool lowerParsed = DateTime.TryParse(lowerBound, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out lowerDate);
                                                    bool upperParsed = DateTime.TryParse(upperBound, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out upperDate);

                                                    if (value is DateTime?)
                                                    {
                                                        DateTime? nullableDateTime = (DateTime?)value;
                                                        return !nullableDateTime.HasValue ||
                                                               nullableDateTime.Value.CompareTo(lowerDate) < 0 ||
                                                               nullableDateTime.Value.CompareTo(upperDate) > 0;
                                                    }
                                                    else
                                                    {
                                                        return ((DateTime)value).CompareTo(lowerDate) < 0 ||
                                                               ((DateTime)value).CompareTo(upperDate) > 0;
                                                    }
                                                }
                                                else if (typeof(IComparable).IsAssignableFrom(property.PropertyType))
                                                {
                                                    var comparable = (IComparable)value;

                                                    var lowerBoundValue = Convert.ChangeType(lowerBound, property.PropertyType);
                                                    var upperBoundValue = Convert.ChangeType(upperBound, property.PropertyType);

                                                    return comparable.CompareTo(lowerBoundValue) < 0 ||
                                                           comparable.CompareTo(upperBoundValue) > 0;
                                                }
                                            }

                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains("Between("))
                                {
                                    string filterContent = filter.Substring(filter.IndexOf("Between(") + "Between(".Length).TrimEnd(')');

                                    string[] filterParts = filterContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                    if (filterParts.Length == 2)
                                    {
                                        string columnName = filter.Substring(1, filter.IndexOf("]") - 1).Trim('[', ' ', ']');

                                        var lowerBound = filterParts[0].Trim().Trim('\'', ' ');
                                        var upperBound = filterParts[1].Trim().Trim('\'', ' ');

                                        count = ClonedTaskGridList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property?.GetValue(ap);

                                            if (value != null)
                                            {
                                                if (property.PropertyType == typeof(string))
                                                {
                                                    var stringValue = value as string;
                                                    return string.Compare(stringValue, lowerBound, StringComparison.OrdinalIgnoreCase) >= 0 &&
                                                           string.Compare(stringValue, upperBound, StringComparison.OrdinalIgnoreCase) <= 0;
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    DateTime lowerDate, upperDate;

                                                    bool lowerParsed = DateTime.TryParse(lowerBound, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out lowerDate);
                                                    bool upperParsed = DateTime.TryParse(upperBound, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out upperDate);

                                                    if (value is DateTime?)
                                                    {
                                                        DateTime? nullableDateTime = (DateTime?)value;
                                                        return nullableDateTime.HasValue &&
                                                               nullableDateTime.Value.CompareTo(lowerDate) >= 0 &&
                                                               nullableDateTime.Value.CompareTo(upperDate) <= 0;
                                                    }
                                                    else
                                                    {
                                                        return ((DateTime)value).CompareTo(lowerDate) >= 0 &&
                                                               ((DateTime)value).CompareTo(upperDate) <= 0;
                                                    }
                                                }
                                                else if (typeof(IComparable).IsAssignableFrom(property.PropertyType))
                                                {
                                                    var comparable = (IComparable)value;
                                                    var lowerBoundValue = Convert.ChangeType(lowerBound, property.PropertyType);
                                                    var upperBoundValue = Convert.ChangeType(upperBound, property.PropertyType);

                                                    return comparable.CompareTo(lowerBoundValue) >= 0 &&
                                                           comparable.CompareTo(upperBoundValue) <= 0;
                                                }
                                            }

                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains("<>"))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*<>?\s*(-?\d+|'[^']*')", RegexOptions.IgnoreCase);
                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value.Trim('\'');

                                        count = ClonedTaskGridList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property?.GetValue(ap)?.ToString();

                                            return value != null && !string.Equals(value, columnValue, StringComparison.OrdinalIgnoreCase);
                                        });
                                    }
                                }

                                else if (filter.Contains(">") && !filter.Contains(">="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*>\s*'(.*?)'", RegexOptions.IgnoreCase);

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*>\s*(-?\d+)", RegexOptions.IgnoreCase);
                                    }

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*>\s*#(.*?)#", RegexOptions.IgnoreCase);
                                    }

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value;

                                        count = ClonedTaskGridList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property != null ? property.GetValue(ap) : null;

                                            if (value != null && value is IComparable)
                                            {
                                                var comparable = (IComparable)value;
                                                object targetValue;

                                                if (property.PropertyType == typeof(string))
                                                {
                                                    targetValue = columnValue;
                                                    return string.Compare(value.ToString(), targetValue.ToString(), StringComparison.OrdinalIgnoreCase) > 0;
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    DateTime dateValue;

                                                    if (DateTime.TryParse(columnValue, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out dateValue))
                                                    {

                                                        if (value is DateTime?)
                                                        {
                                                            DateTime? nullableDateTime = (DateTime?)value;
                                                            return nullableDateTime.HasValue && comparable.CompareTo(dateValue) > 0;
                                                        }
                                                        else
                                                        {
                                                            return comparable.CompareTo(dateValue) > 0;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    targetValue = Convert.ChangeType(columnValue, property.PropertyType);
                                                    return comparable.CompareTo(targetValue) > 0;
                                                }
                                            }
                                            return false;
                                        });
                                    }
                                }

                                else if (filter.Contains("<") && !filter.Contains("<="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*<\s*'(.*?)'", RegexOptions.IgnoreCase);

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*<\s*(-?\d+)", RegexOptions.IgnoreCase);
                                    }

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*<\s*#(.*?)#", RegexOptions.IgnoreCase);
                                    }

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value;

                                        count = ClonedTaskGridList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property != null ? property.GetValue(ap) : null;

                                            if (value != null && value is IComparable)
                                            {
                                                var comparable = (IComparable)value;
                                                object targetValue;

                                                if (property.PropertyType == typeof(string))
                                                {
                                                    targetValue = columnValue;
                                                    return string.Compare(value.ToString(), targetValue.ToString(), StringComparison.OrdinalIgnoreCase) < 0;
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    DateTime dateValue;

                                                    if (DateTime.TryParse(columnValue, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out dateValue))
                                                    {
                                                        if (value is DateTime?)
                                                        {
                                                            DateTime? nullableDateTime = (DateTime?)value;
                                                            return nullableDateTime.HasValue && comparable.CompareTo(dateValue) < 0;
                                                        }
                                                        else
                                                        {
                                                            return comparable.CompareTo(dateValue) < 0;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    targetValue = Convert.ChangeType(columnValue, property.PropertyType);
                                                    return comparable.CompareTo(targetValue) < 0;
                                                }
                                            }
                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains(">="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*>=\s*'(.*?)'", RegexOptions.IgnoreCase);

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*>=\s*(-?\d+)", RegexOptions.IgnoreCase);
                                    }

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*>=\s*#(.*?)#", RegexOptions.IgnoreCase);
                                    }

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value;

                                        count = ClonedTaskGridList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property != null ? property.GetValue(ap) : null;

                                            if (value != null && value is IComparable)
                                            {
                                                var comparable = (IComparable)value;
                                                object targetValue;

                                                if (property.PropertyType == typeof(string))
                                                {
                                                    targetValue = columnValue;
                                                    return string.Compare(value.ToString(), targetValue.ToString(), StringComparison.OrdinalIgnoreCase) >= 0;
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    DateTime dateValue;

                                                    if (DateTime.TryParse(columnValue, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out dateValue))
                                                    {
                                                        if (value is DateTime?)
                                                        {
                                                            DateTime? nullableDateTime = (DateTime?)value;
                                                            return nullableDateTime.HasValue && comparable.CompareTo(dateValue) >= 0;
                                                        }
                                                        else
                                                        {
                                                            return comparable.CompareTo(dateValue) >= 0;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    targetValue = Convert.ChangeType(columnValue, property.PropertyType);
                                                    return comparable.CompareTo(targetValue) >= 0;
                                                }
                                            }
                                            return false;
                                        });
                                    }
                                }

                                else if (filter.Contains("<="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*<=\s*'(.*?)'", RegexOptions.IgnoreCase);

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*<=\s*(-?\d+)", RegexOptions.IgnoreCase);
                                    }

                                    if (!match.Success)
                                    {
                                        match = Regex.Match(filter, @"\[(.*?)\]\s*<=\s*#(.*?)#", RegexOptions.IgnoreCase);
                                    }

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[2].Value;

                                        count = ClonedTaskGridList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property != null ? property.GetValue(ap) : null;

                                            if (value != null)
                                            {
                                                if (property.PropertyType == typeof(string))
                                                {
                                                    var stringValue = value as string;
                                                    return string.Compare(stringValue, columnValue, StringComparison.OrdinalIgnoreCase) <= 0;
                                                }
                                                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                                                {
                                                    DateTime dateValue;

                                                    if (DateTime.TryParse(columnValue, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out dateValue))
                                                    {
                                                        if (value is DateTime?)
                                                        {
                                                            DateTime? nullableDateTime = (DateTime?)value;
                                                            return nullableDateTime.HasValue && nullableDateTime.Value.CompareTo(dateValue) <= 0;
                                                        }
                                                        else
                                                        {
                                                            return ((DateTime)value).CompareTo(dateValue) <= 0;
                                                        }
                                                    }
                                                }
                                                else if (typeof(IComparable).IsAssignableFrom(property.PropertyType))
                                                {
                                                    var comparable = value as IComparable;
                                                    var targetValue = Convert.ChangeType(columnValue, property.PropertyType);
                                                    return comparable.CompareTo(targetValue) <= 0;
                                                }
                                            }
                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains("Not IsNullOrEmpty"))
                                {
                                    string columnName = filter.Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(']')[0].Trim();
                                    count = ClonedTaskGridList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        var value = property?.GetValue(ap)?.ToString();
                                        return !string.IsNullOrEmpty(value);
                                    });
                                }
                                else if (filter.Contains("IsNullOrEmpty"))
                                {
                                    string columnName = filter.Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(']')[0].Trim();
                                    count = ClonedTaskGridList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        var value = property?.GetValue(ap)?.ToString();
                                        return string.IsNullOrEmpty(value);
                                    });
                                }
                                else if (filter.Contains("Is Null"))
                                {
                                    int startIndex = filter.IndexOf('[');
                                    int endIndex = filter.IndexOf(']');


                                    string columnName = filter.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();

                                    count = ClonedTaskGridList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        if (property == null)
                                        {
                                            return false;
                                        }

                                        var value = property.GetValue(ap);

                                        return value == null;
                                    });

                                }

                                else if (filter.Contains("Is Not Null"))
                                {
                                    int startIndex = filter.IndexOf('[');
                                    int endIndex = filter.IndexOf(']');


                                    string columnName = filter.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();

                                    count = ClonedTaskGridList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        if (property == null)
                                        {
                                            return false;
                                        }

                                        var value = property.GetValue(ap);

                                        return value != null;
                                    });

                                }

                                else if (filter.Contains("="))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*=\s*(?:'(.*?)'|(-?\d+(\.\d+)?)|#(.*?)#)", RegexOptions.IgnoreCase);

                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string stringValue = match.Groups[2].Value;
                                        string numericValue = match.Groups[3].Value;
                                        string dateValue = match.Groups[4].Value;

                                        count = ClonedTaskGridList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property?.GetValue(ap);

                                            if (value != null)
                                            {
                                                if (value is string)
                                                {
                                                    return string.Equals(value.ToString(), stringValue, StringComparison.OrdinalIgnoreCase);
                                                }
                                                else if (value is int || value is long || value is decimal || value is double)
                                                {
                                                    return value.ToString() == numericValue;
                                                }
                                            }
                                            return false;
                                        });
                                    }
                                }
                                else if (filter.Contains("!=") || filter.Contains("<>"))
                                {
                                    var match = Regex.Match(filter, @"\[(.*?)\]\s*(!=|<>)\s*'([^']*)'", RegexOptions.IgnoreCase);
                                    if (match.Success)
                                    {
                                        string columnName = match.Groups[1].Value;
                                        string columnValue = match.Groups[3].Value;

                                        count = ClonedTaskGridList.Count(ap =>
                                        {
                                            var property = ap.GetType().GetProperty(columnName);
                                            var value = property?.GetValue(ap).ToString();

                                            return value == null || !string.Equals(value, columnValue, StringComparison.OrdinalIgnoreCase);
                                        });
                                    }
                                }
                                else if (filter.Contains(" Like ") && !filter.Contains(" Not Like "))
                                {
                                    var columnNameStartIndex = filter.IndexOf("[") + 1;
                                    var columnNameEndIndex = filter.IndexOf("]", columnNameStartIndex);
                                    var columnName = filter.Substring(columnNameStartIndex, columnNameEndIndex - columnNameStartIndex).Trim();

                                    var likeValueStartIndex = filter.IndexOf("Like", columnNameEndIndex) + "Like".Length;
                                    var likeValue = filter.Substring(likeValueStartIndex).Trim().Trim('\'');

                                    count = 0;
                                }
                                else if (filter.Contains(" Not Like "))
                                {
                                    var columnName = filter.Substring(1, filter.IndexOf("]") - 1).Trim('[', ' ', ']');
                                    var notLikeValue = filter.Substring(filter.IndexOf("Not Like") + "Not Like".Length).Trim().Trim('\'');

                                    count = ClonedTaskGridList.Count();
                                }

                                //[shweta.thube][GEOS2-7219]
                                TopTaskListOfFilterTile.Add(
                                    new TileBarFilters()
                                    {
                                        Caption = baseKey,
                                        Id = 0,
                                        BackColor = !string.IsNullOrEmpty(backColor) ? backColor : null,
                                        ForeColor = null,
                                        FilterCriteria = item.Value,
                                        EntitiesCount = count,
                                        EntitiesCountVisibility = Visibility.Visible,
                                        Height = 50,
                                        width = 150
                                    });

                                IsTopTaskDockChecked = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddTaskCustomSetting() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }


                GeosApplication.Instance.Logger.Log("Method AddTaskCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddTaskCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6453]
        //private void CommandTaskFilterTileClickAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method CommandTaskFilterTileClickAction....", category: Category.Info, priority: Priority.Low);
        //        if (TaskGridList.Count > 0)
        //        {
        //            var temp = (System.Windows.Controls.SelectionChangedEventArgs)obj;
        //            if (temp.AddedItems.Count > 0)
        //            {
        //                string str = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;
        //                string Template = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Type;
        //                string _FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
        //                CustomTaskFilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;
        //                CustomTaskFilterHTMLColor = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).BackColor;


        //                if (CustomTaskFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
        //                    return;


        //                if (str == null)
        //                {
        //                    if (!string.IsNullOrEmpty(_FilterString))
        //                    {

        //                        if (!string.IsNullOrEmpty(_FilterString))
        //                            MyTaskFilterString = _FilterString;
        //                        else
        //                            MyTaskFilterString = string.Empty;
        //                    }
        //                    else
        //                        MyTaskFilterString = string.Empty;
        //                }
        //                else
        //                {
        //                    if (str.Equals("All"))
        //                    {
        //                        MyTaskFilterString = string.Empty;
        //                        TaskGridList = new ObservableCollection<APMActionPlanTask>(TempTaskGridList);
        //                    }
        //                    else
        //                    {
        //                        if (!string.IsNullOrEmpty(_FilterString))
        //                        {

        //                            if (!string.IsNullOrEmpty(_FilterString))
        //                                MyTaskFilterString = _FilterString;
        //                            else
        //                                MyTaskFilterString = string.Empty;
        //                        }
        //                        else
        //                            MyTaskFilterString = string.Empty;
        //                    }
        //                }

        //            }
        //        }
        //        GeosApplication.Instance.Logger.Log("Method CommandTaskFilterTileClickAction....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method CommandTaskFilterTileClickAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //[shweta.thube][GEOS2-6453]
        private void CommandTaskTileBarDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTaskTileBarDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                if (CustomTaskFilterStringName == "CUSTOM FILTERS" || CustomTaskFilterStringName == "All")
                {
                    return;
                }
                TableView table = (TableView)obj;
                GridControl gridControl = (table).Grid;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();

                string filterContent = SelectedTaskTopTileBarItem.FilterCriteria;

                string columnName = string.Empty;
                string columnValue = string.Empty;
                if (filterContent.StartsWith("StartsWith("))
                {
                    string filterContents = filterContent.Substring("StartsWith(".Length).TrimEnd(')');
                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']');
                    columnValue = filterParts[1].Trim(' ', '\'');
                }
                else if (filterContent.StartsWith("EndsWith("))
                {
                    string filterContents = filterContent.Substring("EndsWith(".Length).TrimEnd(')');
                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']');
                    columnValue = filterParts[1].Trim(' ', '\'');
                }
                else if (filterContent.Contains("Contains(") && !filterContent.Contains("Not Contains("))
                {
                    string filterContents = filterContent.Substring("Contains(".Length).TrimEnd(')');
                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']');
                    columnValue = filterParts[1].Trim(' ', '\'');
                }
                else if (filterContent.Contains("Not Contains("))
                {
                    string filterContents = filterContent.Substring("Not Contains(".Length).TrimEnd(')');
                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']');
                    columnValue = filterParts[1].Trim(' ', '\'');
                }
                else if (filterContent.Contains(" In ") && !(filterContent.Contains("Not") && filterContent.Contains("In")))
                {
                    int startColumnIndex = filterContent.IndexOf('[') + 1;
                    int endColumnIndex = filterContent.IndexOf(']');
                    columnName = filterContent.Substring(startColumnIndex, endColumnIndex - startColumnIndex).Trim();
                }
                else if (filterContent.Contains("Not") && filterContent.Contains("In"))
                {
                    int startIndex = filterContent.IndexOf('(') + 1;
                    int endIndex = filterContent.LastIndexOf(')');
                    string filterContents = filterContent.Substring(startIndex, endIndex - startIndex).Trim();

                    string[] filterParts = filterContent.Split(new[] { "Not", "In" }, StringSplitOptions.RemoveEmptyEntries);
                    columnName = filterParts[0].Trim('[', ']', ' ');

                }
                else if (filterContent.Contains("Not [") && filterContent.Contains("Between("))
                {
                    var filterContents = filterContent.Substring(filterContent.IndexOf("Not [") + "Not [".Length,
                        filterContent.IndexOf("Between(") - (filterContent.IndexOf("Not [") + "Not [".Length)).Trim();

                    var betweenContent = filterContent.Substring(filterContent.IndexOf("Between(") + "Between(".Length).TrimEnd(')');
                    var filterParts = betweenContent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (filterParts.Length == 2)
                    {
                        columnName = filterContents.Trim(' ', '[', ']');
                        var lowerBound = filterParts[0].Trim().Trim('\'');
                        var upperBound = filterParts[1].Trim().Trim('\'');
                    }
                }
                else if (filterContent.Contains("Between("))
                {
                    string filterContents = filterContent.Substring(filterContent.IndexOf("Between(") + "Between(".Length).TrimEnd(')');

                    string[] filterParts = filterContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (filterParts.Length == 2)
                    {
                        columnName = filterContent.Substring(1, filterContent.IndexOf("]") - 1).Trim('[', ' ', ']');
                    }
                }
                else if (filterContent.Contains("<>"))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*<>?\s*(-?\d+|'[^']*')", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains(">") && !(filterContent.Contains(">=")))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*>\s*'(.*?)'", RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*>\s*(-?\d+)", RegexOptions.IgnoreCase);
                    }

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*>\s*#(.*?)#", RegexOptions.IgnoreCase);
                    }
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }

                else if (filterContent.Contains("<") && !(filterContent.Contains("<=")))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*<\s*'(.*?)'", RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*<\s*(-?\d+)", RegexOptions.IgnoreCase);
                    }

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*<\s*#(.*?)#", RegexOptions.IgnoreCase);
                    }
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains(">="))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*>=\s*'(.*?)'", RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*>=\s*(-?\d+)", RegexOptions.IgnoreCase);
                    }

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*>=\s*#(.*?)#", RegexOptions.IgnoreCase);
                    }
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains("<="))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*<=\s*'(.*?)'", RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*<=\s*(-?\d+)", RegexOptions.IgnoreCase);
                    }

                    if (!match.Success)
                    {
                        match = Regex.Match(filterContent, @"\[(.*?)\]\s*<=\s*#(.*?)#", RegexOptions.IgnoreCase);
                    }
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains("Not IsNullOrEmpty"))
                {
                    columnName = filterContent.Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(']')[0].Trim();
                }
                else if (filterContent.Contains("IsNullOrEmpty"))
                {
                    columnName = filterContent.Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(']')[0].Trim();
                }
                else if (filterContent.Contains("="))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*=\s*(?:'(.*?)'|(-?\d+(\.\d+)?)|#(.*?)#)", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[2].Value;
                    }
                }
                else if (filterContent.Contains("!=") || filterContent.Contains("<>"))
                {
                    var match = Regex.Match(filterContent, @"\[(.*?)\]\s*(!=|<>)\s*'([^']*)'", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        columnName = match.Groups[1].Value;
                        columnValue = match.Groups[3].Value;
                    }
                }
                else if (filterContent.Contains(" Like ") || filterContent.Contains(" Not Like "))
                {
                    columnName = filterContent.Substring(1, filterContent.IndexOf("]") - 1).Trim('[', ' ', ']');
                }
                else if (filterContent.Contains("Is Null"))
                {
                    int startIndex = filterContent.IndexOf('[');
                    int endIndex = filterContent.IndexOf(']');
                    columnName = filterContent.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                }

                else if (filterContent.Contains("Is Not Null"))
                {
                    int startIndex = filterContent.IndexOf('[');
                    int endIndex = filterContent.IndexOf(']');
                    columnName = filterContent.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                }

                string normalizedColumnName = columnName.Replace(" ", "").ToLower();

                GridColumn column = GridColumnList.FirstOrDefault(x =>
                    x.Header.ToString().Replace(" ", "").ToLower().Equals(normalizedColumnName));

                if (column == null)
                {
                    column = GridColumnList.FirstOrDefault(x =>
                        x.FieldName.Replace(" ", "").ToLower().Equals(normalizedColumnName));
                }

                if (column != null)
                {
                    IsEdit = true;
                    table.ShowFilterEditor(column);
                }

                GeosApplication.Instance.Logger.Log("Method CommandTaskTileBarDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTaskTileBarDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillTaskGrid()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTaskGrid() .", category: Category.Info, priority: Priority.Low);
                if (APMCommon.Instance.SelectedPeriod != null)
                {
                    var selectedPeriod = APMCommon.Instance.SelectedPeriod.Cast<long>().ToList();
                    string idPeriods = string.Join(",", selectedPeriod);

                    EnsureTaskCacheLoaded(idPeriods);

                    var ordered = _taskCache
                        .Select(t => (APMActionPlanTask)t.Clone())
                        .OrderBy(t => t.DueDays)
                        .ToList();

                    TaskGridList = new ObservableCollection<APMActionPlanTask>(ordered);
                    TempTaskGridList = ordered.Select(t => (APMActionPlanTask)t.Clone()).ToList();
                    SelectedActionPlanTask = TaskGridList.FirstOrDefault();

                    ClonedTaskGridList = ordered.Select(t => (APMActionPlanTask)t.Clone()).ToList();
                }
                else
                {
                    TaskGridList = new ObservableCollection<APMActionPlanTask>();
                    TempTaskGridList = new List<APMActionPlanTask>();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method FillTaskGrid() error: " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[shweta.thube][GEOS2-6911]
        private void FillCustomerList()
        {
            ListOfCustomer = new ObservableCollection<APMCustomer>();

            if (ActionPlanList != null && ActionPlanList.Count > 0)
            {
                foreach (var item in ActionPlanList)
                {
                    if (item.IdSite > 0)
                    {
                        if (!ListOfCustomer.Any(x => x.IdSite == item.IdSite))
                        {
                            ListOfCustomer.Add(new APMCustomer()
                            {
                                GroupName = item.GroupName,
                                Site = item.Site,
                                IdSite = item.IdSite
                            });


                        }
                    }
                    else
                    {
                        if (!ListOfCustomer.Any(x => x.IdSite == item.IdSite))
                        {
                            ListOfCustomer.Insert(0, new APMCustomer()
                            {
                                GroupName = "Blanks",
                                Site = item.Site,
                                IdSite = item.IdSite
                            });


                        }
                    }

                }
            }
            else
            {
                ListOfCustomer = new ObservableCollection<APMCustomer>();
            }

            if (SelectedCustomer == null)
            {
                SelectedCustomer = new List<object>();
            }
        }
        //[shweta.thube][GEOS2-7008]
        private void ParticipantsHyperLinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ParticipantsHyperLinkClickCommandAction....", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                GridControl gridControl = (GridControl)obj;
                TableView detailView = (TableView)gridControl.View;
                APMActionPlanTask selectedTask = (APMActionPlanTask)detailView.DataControl.CurrentItem;
                TaskParticipantsView taskParticipantsView = new TaskParticipantsView();
                TaskParticipantsViewModel taskParticipantsViewModel = new TaskParticipantsViewModel();
                EventHandler handle = delegate { taskParticipantsView.Close(); };
                taskParticipantsViewModel.RequestClose += handle;
                taskParticipantsViewModel.Init(selectedTask);
                taskParticipantsView.DataContext = taskParticipantsViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                taskParticipantsView.Owner = Window.GetWindow(ownerInfo);
                taskParticipantsView.ShowDialog();

                if (taskParticipantsViewModel.IsSave)
                {
                    IsActionPlanEdit = true;
                    SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == selectedTask.IdActionPlan);
                    var index = SelectedActionPlan.TaskList.FindIndex(x => x.IdActionPlanTask == selectedTask.IdActionPlanTask);
                    if (index >= 0)
                    {
                        SelectedActionPlan.TaskList[index].ParticipantCount = taskParticipantsViewModel.ListParticipants.Count;
                    }

                    if (ClonedActionPlanList != null)
                    {
                        ClonedActionPlanList.FirstOrDefault(x => x.IdActionPlan == SelectedActionPlan.IdActionPlan).TaskList[index].ParticipantCount = taskParticipantsViewModel.ListParticipants.Count;

                    }
                    FillLeftTileList();
                    SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                    FilterCommandAction(obj);
                    CommandAlertFilterTileClickAction(obj);
                    if (PreviousSelectedAlertTileBarItem != null)
                        SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedAlertTileBarItem.Caption);
                    IsActionPlanEdit = false;
                    IsExpand = false;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ParticipantsHyperLinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ParticipantsHyperLinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Pallavi.Kale][GEOS2 - 8216]
        private void FillThemeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillThemeList ...", category: Category.Info, priority: Priority.Low);
                if (APMCommon.Instance.ThemeList == null)
                {
                    List<LookupValue> temp = new List<LookupValue>();
                    //APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    APMCommon.Instance.ThemeList = new List<LookupValue>(APMService.GetLookupValues_V2550(155));//TaskThemes

                }
                GeosApplication.Instance.Logger.Log("Method FillThemeList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillThemeList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillThemeList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillThemeList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Pallavi.Kale][GEOS2 - 8216]
        private void FillPriorityList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPriorityList ...", category: Category.Info, priority: Priority.Low);
                if (APMCommon.Instance.PriorityList == null)
                {
                    List<LookupValue> temp = new List<LookupValue>();
                    //APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    APMCommon.Instance.PriorityList = new List<LookupValue>(APMService.GetLookupValues_V2550(153));//TaskPriority                    

                }
                GeosApplication.Instance.Logger.Log("Method FillPriorityList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPriorityList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPriorityList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPriorityList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[pallavi.kale][GEOS2-7002][19.06.2025]
        private void AddActionPlanSubTaskViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddActionPlanSubTaskViewWindowShow()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new System.Windows.Media.SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                //[shweta.thube][GEOS2-8985]
                GridControl gridControl = (GridControl)obj;
                TableView detailView = (TableView)gridControl.View;
                APMActionPlanTask SelectedActionPlanTask = (APMActionPlanTask)detailView.DataControl.CurrentItem;
                AddEditSubTaskViewModel addEditSubTaskViewModel = new AddEditSubTaskViewModel();
                AddEditSubTaskView addEditSubTaskView = new AddEditSubTaskView();
                EventHandler handle = delegate { addEditSubTaskView.Close(); };
                addEditSubTaskViewModel.RequestClose += handle;
                addEditSubTaskViewModel.IsNew = true;
                addEditSubTaskViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddSubTaskHeader").ToString();
                List<APMActionPlan> temp = new List<APMActionPlan>();
                temp = new List<APMActionPlan>(TempActionPlanList);
                APMCommon.Instance.ActionPlanList = new List<APMActionPlan>(temp.OrderBy(ap => ap.Code));
                // APMCommon.Instance.SubTaskList = new List<APMActionPlanSubTask>(temp.OrderBy(apt => apt.Code));
                // SelectedActionPlanTask.IdSite = APMCommon.Instance.ActionPlanList.FirstOrDefault(x => x.IdActionPlan == SelectedActionPlanTask.IdActionPlan).IdSite;
                //[pallavi.kale][GEOS2-7213][25.07.2025]
                var parentActionPlan = APMCommon.Instance.ActionPlanList.FirstOrDefault(x => x.IdActionPlan == SelectedActionPlanTask.IdActionPlan);

                if (parentActionPlan != null)
                {
                    SelectedActionPlanTask.IdSite = parentActionPlan.IdSite;
                    SelectedActionPlanTask.ActionPlanCode = parentActionPlan.Code;
                }

                addEditSubTaskViewModel.OtItemVisibility = Visibility.Collapsed;
                //SelectedActionPlanTask.ActionPlanCode = SelectedActionPlanTask.Code;
                addEditSubTaskViewModel.Init(SelectedActionPlanTask);
                addEditSubTaskView.DataContext = addEditSubTaskViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addEditSubTaskView.ShowDialogWindow();

                if (addEditSubTaskViewModel.IsSave)
                {
                    //[shweta.thube][GEOS2-8845]
                    if (!IsTaskGridVisibility)
                    {
                        SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == addEditSubTaskViewModel.TempSubTask.IdActionPlan);

                        var task = SelectedActionPlan?.TaskList?.FirstOrDefault(t => t.IdActionPlanTask == addEditSubTaskViewModel.TempSubTask.IdParent);

                        if (task != null)
                        {
                            if (task.SubTaskList == null)
                            {
                                task.SubTaskList = new List<APMActionPlanSubTask>();
                            }
                            if (task != null && task.SubTaskList != null)
                            {
                                task.SubTaskList.Add(addEditSubTaskViewModel.TempSubTask);

                            }
                            SelectedActionPlan.TaskList = new ObservableCollection<APMActionPlanTask>(SelectedActionPlan.TaskList).ToList();
                            SelectedActionPlan.TaskList = SelectedActionPlan.TaskList.OrderBy(t => t.TaskNumber).ToList();//[pallavi.kale][GEOS2-7213][25.07.2025]
                            ActionPlanList = new ObservableCollection<APMActionPlan>(ActionPlanList);
                            // Set deletion flags for the newly added subtask
                            SetDeletionFlags(new[] { SelectedActionPlan });
                            LastUpdatedIdActionPlan = SelectedActionPlan.IdActionPlan;//[pallavi.kale][GEOS2-7213][25.07.2025]
                            LastUpdatedIdActionPlanTask = addEditSubTaskViewModel.TempSubTask.IdParent;//[pallavi.kale][GEOS2-7213][25.07.2025]
                            IsExpandTaskLevelOnly = false;//[pallavi.kale][GEOS2-7213][25.07.2025]
                            IsExpand = true;//[pallavi.kale][GEOS2-7213][25.07.2025]
                            ExpandLastUpdatedActionPlan();//[pallavi.kale][GEOS2-7213][25.07.2025]
                        }

                    }
                    else
                    {
                        SelectedActionPlanTask = TaskGridList.FirstOrDefault(x => x.IdActionPlanTask == addEditSubTaskViewModel.TempSubTask.IdParent);
                        if (SelectedActionPlanTask.SubTaskList == null)
                        {
                            SelectedActionPlanTask.SubTaskList = new List<APMActionPlanSubTask>();
                        }
                        addEditSubTaskViewModel.TempSubTask.ActionPlanDescription = SelectedActionPlanTask.ActionPlanDescription;
                        addEditSubTaskViewModel.TempSubTask.BusinessUnit = SelectedActionPlanTask.BusinessUnit;
                        addEditSubTaskViewModel.TempSubTask.TaskOrigin = SelectedActionPlanTask.TaskOrigin;
                        addEditSubTaskViewModel.TempSubTask.Location = SelectedActionPlanTask.Location;
                        addEditSubTaskViewModel.TempSubTask.Country = new Country();
                        addEditSubTaskViewModel.TempSubTask.Country.CountryIconUrl = SelectedActionPlanTask.Country.CountryIconUrl;
                        SelectedActionPlanTask.SubTaskList.Add(addEditSubTaskViewModel.TempSubTask);

                        TaskGridList = new ObservableCollection<APMActionPlanTask>(TaskGridList);

                        SelectedActionPlanTask.SubTaskList = new List<APMActionPlanSubTask>(SelectedActionPlanTask.SubTaskList);//[shweta.thube][GEOS2-7217][24.07.2025]

                        // Set deletion flags for the newly added subtask
                        var currentUserId = GeosApplication.Instance.ActiveUser.IdUser;
                        var hasPermission = GeosApplication.Instance.IsAPMActionPlanPermission;
                        var newSubTask = addEditSubTaskViewModel.TempSubTask;
                        var parentTask = SelectedActionPlanTask;
                        var parentPlan = ActionPlanList?.FirstOrDefault(ap => ap.IdActionPlan == parentTask.IdActionPlan);

                        if (newSubTask != null)
                        {
                            newSubTask.IsSubTaskDeleted = (newSubTask.CreatedBy == currentUserId ||
                                                          parentTask?.CreatedBy == currentUserId ||
                                                          parentPlan?.CreatedBy == currentUserId ||
                                                          hasPermission);
                        }

                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddActionPlanSubTaskViewWindowShow()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddActionPlanSubTaskViewWindowShow()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pallavi.kale][GEOS2-7002][19.06.2025]
        private void EditSubTaskHyperLinkCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditSubTaskHyperLinkCommandAction....", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = (GridControl)obj;
                TableView detailView = (TableView)gridControl.View;
                APMActionPlanSubTask selectedSubTask = (APMActionPlanSubTask)detailView.DataControl.CurrentItem;
                AddEditSubTaskView addEditSubTaskView = new AddEditSubTaskView();
                AddEditSubTaskViewModel addEditSubTaskViewModel = new AddEditSubTaskViewModel();
                EventHandler handle = delegate { addEditSubTaskView.Close(); };
                addEditSubTaskViewModel.RequestClose += handle;
                addEditSubTaskViewModel.IsNew = false;
                addEditSubTaskViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditSubTaskHeader").ToString();
                List<APMActionPlan> temp = new List<APMActionPlan>();
                temp = new List<APMActionPlan>(TempActionPlanList);
                APMCommon.Instance.ActionPlanList = new List<APMActionPlan>(temp.OrderBy(ap => ap.Code));
                var ActionPlan = APMCommon.Instance.ActionPlanList.FirstOrDefault(ap => ap.IdActionPlan == selectedSubTask.IdActionPlan);
                selectedSubTask.IdActionPlanResponsible = ActionPlan.IdEmployee;
                selectedSubTask.ActionPlanResponsibleIdUser = ActionPlan.ResponsibleIdUser;
                selectedSubTask.IdSite = ActionPlan.IdSite;
                selectedSubTask.IdActionPlanLocation = ActionPlan.IdCompany;
                addEditSubTaskViewModel.OtItemVisibility = Visibility.Collapsed;
                selectedSubTask.CustomerName = ActionPlan.GroupName;
                selectedSubTask.IsSubTaskAdded = true;
                addEditSubTaskViewModel.EditInit(selectedSubTask);
                addEditSubTaskView.DataContext = addEditSubTaskViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addEditSubTaskView.Owner = Window.GetWindow(ownerInfo);

                addEditSubTaskView.ShowDialog();

                if (addEditSubTaskViewModel.IsSave)
                {
                    if (IsTaskGridVisibility)
                    {
                        FillTaskGrid();
                    }
                    else
                    {
                        FillActionPlan();
                        LastUpdatedIdActionPlan = selectedSubTask.IdActionPlan;//[pallavi.kale][GEOS2-7213][25.07.2025]
                        LastUpdatedIdActionPlanTask = selectedSubTask.IdParent;//[pallavi.kale][GEOS2-7213][25.07.2025]
                        IsExpandTaskLevelOnly = false;//[pallavi.kale][GEOS2-7213][25.07.2025]
                        IsExpand = true;//[pallavi.kale][GEOS2-7213][25.07.2025]
                        ExpandLastUpdatedActionPlan();//[pallavi.kale][GEOS2-7213][25.07.2025]
                    }
                }


                GeosApplication.Instance.Logger.Log("Method EditSubTaskHyperLinkCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditSubTaskHyperLinkCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-7004][25.06.2025]
        private void SubTaskCommentsHyperLinkCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SubTaskCommentsHyperLinkCommandAction....", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                GridControl gridControl = (GridControl)obj;
                TableView detailView = (TableView)gridControl.View;
                APMActionPlanSubTask selectedSubTask = (APMActionPlanSubTask)detailView.DataControl.CurrentItem;
                SubTaskCommentsView subTaskCommentsView = new SubTaskCommentsView();
                SubTaskCommentsViewModel subTaskCommentsViewModel = new SubTaskCommentsViewModel();
                EventHandler handle = delegate { subTaskCommentsView.Close(); };
                subTaskCommentsViewModel.RequestClose += handle;
                subTaskCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("SubTaskCommentsHeader").ToString();
                subTaskCommentsViewModel.Init(selectedSubTask);
                subTaskCommentsView.DataContext = subTaskCommentsViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                subTaskCommentsView.Owner = Window.GetWindow(ownerInfo);
                subTaskCommentsView.ShowDialog();
                if (subTaskCommentsViewModel.IsSave)
                {
                    //IsActionPlanEdit = true;                                
                    if (IsTaskGridVisibility)
                    {
                        SelectedActionPlanTask = TaskGridList.FirstOrDefault(x => x.IdActionPlanTask == selectedSubTask.IdParent);
                        //var Subtask = SelectedActionPlanTask?.SubTaskList?.FirstOrDefault(t => t.IdActionPlanTask == selectedSubTask.IdActionPlanTask);
                        var index = SelectedActionPlanTask.SubTaskList.FindIndex(x => x.IdActionPlanTask == selectedSubTask.IdActionPlanTask);
                        if (index >= 0)
                        {
                            var subtask = SelectedActionPlanTask.SubTaskList[index];
                            subtask.CommentsCount = subTaskCommentsViewModel.SubTaskCommentsList.Count;
                            subtask.TaskLastComment = subTaskCommentsViewModel.SubTaskCommentsList.OrderByDescending(a => a.CreatedIn).FirstOrDefault().Comments;

                            SelectedActionPlanTask.SubTaskList = new ObservableCollection<APMActionPlanSubTask>(SelectedActionPlanTask.SubTaskList).ToList();
                        }
                        SelectedActionPlanSubTask = null;
                    }
                    else
                    {
                        SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == selectedSubTask.IdActionPlan);
                        var task = SelectedActionPlan?.TaskList?.FirstOrDefault(t => t.IdActionPlanTask == selectedSubTask.IdParent);

                        if (task != null)
                        {
                            var subtask = task.SubTaskList?.FirstOrDefault(st => st.IdActionPlanTask == selectedSubTask.IdActionPlanTask);

                            if (subtask != null)
                            {
                                subtask.CommentsCount = subTaskCommentsViewModel.SubTaskCommentsList.Count;
                                subtask.TaskLastComment = subTaskCommentsViewModel.SubTaskCommentsList.OrderByDescending(a => a.CreatedIn).FirstOrDefault().Comments;
                                task.SubTaskList = new ObservableCollection<APMActionPlanSubTask>(task.SubTaskList).ToList();
                            }
                            SelectedActionPlan.TaskList = new ObservableCollection<APMActionPlanTask>(SelectedActionPlan.TaskList).ToList();
                        }
                    }
                    //IsActionPlanEdit = false;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SubTaskCommentsHyperLinkCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method SubTaskCommentsHyperLinkCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-7004][25.06.2025]
        private void SubTaskAttachmentsHyperClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SubTaskAttachmentsHyperClickCommandAction....", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                GridControl gridControl = (GridControl)obj;
                TableView detailView = (TableView)gridControl.View;
                APMActionPlanSubTask selectedSubTask = (APMActionPlanSubTask)detailView.DataControl.CurrentItem;
                SubTaskAttachmentsView subTaskAttachmentsView = new SubTaskAttachmentsView();
                SubTaskAttachmentsViewModel subTaskAttachmentsViewModel = new SubTaskAttachmentsViewModel();
                EventHandler handle = delegate { subTaskAttachmentsView.Close(); };
                subTaskAttachmentsViewModel.RequestClose += handle;
                subTaskAttachmentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("TaskAttachmentsHeader").ToString();
                subTaskAttachmentsViewModel.Init(selectedSubTask.IdActionPlanTask);
                subTaskAttachmentsView.DataContext = subTaskAttachmentsViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                subTaskAttachmentsView.Owner = Window.GetWindow(ownerInfo);
                subTaskAttachmentsView.ShowDialog();

                if (subTaskAttachmentsViewModel.IsSave)
                {
                    if (IsTaskGridVisibility)
                    {
                        SelectedActionPlanTask = TaskGridList.FirstOrDefault(x => x.IdActionPlanTask == selectedSubTask.IdParent);
                        //var Subtask = SelectedActionPlanTask?.SubTaskList?.FirstOrDefault(t => t.IdActionPlanTask == selectedSubTask.IdActionPlanTask);
                        var index = SelectedActionPlanTask.SubTaskList.FindIndex(x => x.IdActionPlanTask == selectedSubTask.IdActionPlanTask);
                        if (index >= 0)
                        {
                            var subtask = SelectedActionPlanTask.SubTaskList[index];
                            subtask.FileCount = subTaskAttachmentsViewModel.ListAttachment.Count;

                            SelectedActionPlanTask.SubTaskList = new ObservableCollection<APMActionPlanSubTask>(SelectedActionPlanTask.SubTaskList).ToList();
                        }
                        TaskGridList = new ObservableCollection<APMActionPlanTask>(TaskGridList);
                    }
                    else
                    {
                        SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == selectedSubTask.IdActionPlan);
                        var task = SelectedActionPlan?.TaskList?.FirstOrDefault(t => t.IdActionPlanTask == selectedSubTask.IdParent);

                        if (task != null)
                        {
                            var subtask = task.SubTaskList?.FirstOrDefault(st => st.IdActionPlanTask == selectedSubTask.IdActionPlanTask);

                            if (subtask != null)
                            {
                                subtask.FileCount = subTaskAttachmentsViewModel.ListAttachment.Count;
                                //subtask.TaskLastComment = subTaskAttachmentsViewModel.SubTaskAttachmentList.OrderByDescending(a => a.CreatedIn).FirstOrDefault().Comments;
                                task.SubTaskList = new ObservableCollection<APMActionPlanSubTask>(task.SubTaskList).ToList();
                            }
                            SelectedActionPlan.TaskList = new ObservableCollection<APMActionPlanTask>(SelectedActionPlan.TaskList).ToList();
                        }
                    }

                }


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SubTaskAttachmentsHyperClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method SubTaskAttachmentsHyperClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-7004][25.06.2025]
        private void SubTaskParticipantsHyperLinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SubTaskParticipantsHyperLinkClickCommandAction....", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                GridControl gridControl = (GridControl)obj;
                TableView detailView = (TableView)gridControl.View;
                APMActionPlanSubTask selectedSubTask = (APMActionPlanSubTask)detailView.DataControl.CurrentItem;
                SubTaskParticipantsView subTaskParticipantsView = new SubTaskParticipantsView();
                SubTaskParticipantsViewModel subTaskParticipantsViewModel = new SubTaskParticipantsViewModel();
                EventHandler handle = delegate { subTaskParticipantsView.Close(); };
                subTaskParticipantsViewModel.RequestClose += handle;
                subTaskParticipantsViewModel.Init(selectedSubTask);
                subTaskParticipantsView.DataContext = subTaskParticipantsViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                subTaskParticipantsView.Owner = Window.GetWindow(ownerInfo);
                subTaskParticipantsView.ShowDialog();

                if (subTaskParticipantsViewModel.IsSave)
                {
                    if (IsTaskGridVisibility)
                    {
                        SelectedActionPlanTask = TaskGridList.FirstOrDefault(x => x.IdActionPlanTask == selectedSubTask.IdParent);
                        //var Subtask = SelectedActionPlanTask?.SubTaskList?.FirstOrDefault(t => t.IdActionPlanTask == selectedSubTask.IdActionPlanTask);
                        var index = SelectedActionPlanTask.SubTaskList.FindIndex(x => x.IdActionPlanTask == selectedSubTask.IdActionPlanTask);
                        if (index >= 0)
                        {
                            var subtask = SelectedActionPlanTask.SubTaskList[index];
                            subtask.ParticipantCount = subTaskParticipantsViewModel.ListParticipants.Count;

                            SelectedActionPlanTask.SubTaskList = new ObservableCollection<APMActionPlanSubTask>(SelectedActionPlanTask.SubTaskList).ToList();
                        }
                        TaskGridList = new ObservableCollection<APMActionPlanTask>(TaskGridList);
                    }
                    else
                    {
                        SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == selectedSubTask.IdActionPlan);
                        var task = SelectedActionPlan?.TaskList?.FirstOrDefault(t => t.IdActionPlanTask == selectedSubTask.IdParent);

                        if (task != null)
                        {
                            var subtask = task.SubTaskList?.FirstOrDefault(st => st.IdActionPlanTask == selectedSubTask.IdActionPlanTask);

                            if (subtask != null)
                            {
                                subtask.ParticipantCount = subTaskParticipantsViewModel.ListParticipants.Count;
                                //subtask.TaskLastComment = subTaskAttachmentsViewModel.SubTaskAttachmentList.OrderByDescending(a => a.CreatedIn).FirstOrDefault().Comments;
                                task.SubTaskList = new ObservableCollection<APMActionPlanSubTask>(task.SubTaskList).ToList();
                            }
                            SelectedActionPlan.TaskList = new ObservableCollection<APMActionPlanTask>(SelectedActionPlan.TaskList).ToList();
                        }
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SubTaskParticipantsHyperLinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method SubTaskParticipantsHyperLinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pallavi.kale][GEOS2-7003][30.06.2025]
        private void EditActionPlanTaskHyperLinkCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditActionPlanTaskHyperLinkCommandAction....", category: Category.Info, priority: Priority.Low);

                GridControl gridControl;
                TableView detailView = new TableView();
                if (obj is GridControl gc)
                {
                    gridControl = gc;
                    detailView = (TableView)gridControl.View;
                }
                else if (obj is TableView tv)
                {
                    // Handle the case where obj is directly a TableView
                    detailView = tv;
                }

                if (IsTaskGridVisibility)
                {
                    APMActionPlanSubTask selectedTask = (APMActionPlanSubTask)detailView.DataControl.CurrentItem;
                    long Idactionplan = selectedTask.IdActionPlan;
                    APMActionPlan SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == Idactionplan);
                    AddEditActionPlanView addEditActionPlanView = new AddEditActionPlanView();
                    AddEditActionPlanViewModel addEditActionPlanViewModel = new AddEditActionPlanViewModel();
                    EventHandler handle = delegate { addEditActionPlanView.Close(); };
                    addEditActionPlanViewModel.RequestClose += handle;
                    addEditActionPlanViewModel.IsNew = false;
                    addEditActionPlanViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditActionPlanTitle").ToString();
                    List<APMActionPlan> temp = new List<APMActionPlan>();
                    temp = new List<APMActionPlan>(TempActionPlanList);
                    APMCommon.Instance.ActionPlanList = new List<APMActionPlan>(temp.OrderBy(ap => ap.Code));
                    addEditActionPlanViewModel.EditInit(SelectedActionPlan);
                    addEditActionPlanView.DataContext = addEditActionPlanViewModel;
                    var ownerInfo = (detailView as FrameworkElement);
                    addEditActionPlanView.Owner = Window.GetWindow(ownerInfo);

                    addEditActionPlanView.ShowDialog();
                    if (addEditActionPlanViewModel.IsSave)
                    {
                        SelectedActionPlan.Code = addEditActionPlanViewModel.Code;
                        SelectedActionPlan.Description = addEditActionPlanViewModel.Description;
                        SelectedActionPlan.IdCompany = APMCommon.Instance.LocationList[addEditActionPlanViewModel.SelectedLocationIndex].IdCompany;
                        SelectedActionPlan.Location = APMCommon.Instance.LocationList[addEditActionPlanViewModel.SelectedLocationIndex].Alias;
                        SelectedActionPlan.IdEmployee = (Int32)APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].IdEmployee;
                        SelectedActionPlan.FirstName = APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].FirstName;
                        SelectedActionPlan.LastName = APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].LastName;
                        SelectedActionPlan.FullName = APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].FullName;
                        SelectedActionPlan.IdLookupBusinessUnit = APMCommon.Instance.BusinessUnitList[addEditActionPlanViewModel.SelectedBusinessIndex].IdLookupValue;
                        SelectedActionPlan.BusinessUnit = APMCommon.Instance.BusinessUnitList[addEditActionPlanViewModel.SelectedBusinessIndex].Value;
                        SelectedActionPlan.CreatedByName = addEditActionPlanViewModel.CreatedBy;
                        SelectedActionPlan.CreatedIn = addEditActionPlanViewModel.CreatedIn;
                        SelectedActionPlan.IdLookupOrigin = APMCommon.Instance.OriginList[addEditActionPlanViewModel.SelectedOriginIndex].IdLookupValue;
                        SelectedActionPlan.Origin = APMCommon.Instance.OriginList[addEditActionPlanViewModel.SelectedOriginIndex].Value;
                        SelectedActionPlan.IdDepartment = (Int32)APMCommon.Instance.DepartmentList[addEditActionPlanViewModel.SelectedDepartmentIndex].IdDepartment;
                        SelectedActionPlan.Department = APMCommon.Instance.DepartmentList[addEditActionPlanViewModel.SelectedDepartmentIndex].DepartmentName;
                        SelectedActionPlan.OriginDescription = addEditActionPlanViewModel.OriginDescription;
                        SelectedActionPlan.TaskList = new List<APMActionPlanTask>(addEditActionPlanViewModel.TaskList);

                        IsActionPlanEdit = true;
                        ClonedActionPlanList.FirstOrDefault(x => x.IdActionPlan == SelectedActionPlan.IdActionPlan).TaskList = new List<APMActionPlanTask>(addEditActionPlanViewModel.TaskList);
                        FillLeftTileList();
                        SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                        FilterCommandActionForTask(obj);
                        FillAlertSectionList(); //[shweta.thube][GEOS2-7217][28.07.2025]
                        CommandAlertFilterTileClickAction(obj);
                        if (PreviousSelectedAlertTileBarItem != null)
                            SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedAlertTileBarItem.Caption);
                        IsActionPlanEdit = false;

                    }

                }
                else
                {
                    AddEditActionPlanView addEditActionPlanView = new AddEditActionPlanView();
                    AddEditActionPlanViewModel addEditActionPlanViewModel = new AddEditActionPlanViewModel();
                    EventHandler handle = delegate { addEditActionPlanView.Close(); };
                    addEditActionPlanViewModel.RequestClose += handle;
                    addEditActionPlanViewModel.IsNew = false;
                    addEditActionPlanViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditActionPlanTitle").ToString();
                    List<APMActionPlan> temp = new List<APMActionPlan>();
                    temp = new List<APMActionPlan>(TempActionPlanList);
                    APMCommon.Instance.ActionPlanList = new List<APMActionPlan>(temp.OrderBy(ap => ap.Code));
                    addEditActionPlanViewModel.EditInit(SelectedActionPlan);
                    addEditActionPlanView.DataContext = addEditActionPlanViewModel;
                    var ownerInfo = (detailView as FrameworkElement);
                    addEditActionPlanView.Owner = Window.GetWindow(ownerInfo);

                    addEditActionPlanView.ShowDialog();
                    if (addEditActionPlanViewModel.IsSave)
                    {
                        SelectedActionPlan.Code = addEditActionPlanViewModel.Code;
                        SelectedActionPlan.Description = addEditActionPlanViewModel.Description;
                        SelectedActionPlan.IdCompany = APMCommon.Instance.LocationList[addEditActionPlanViewModel.SelectedLocationIndex].IdCompany;
                        SelectedActionPlan.Location = APMCommon.Instance.LocationList[addEditActionPlanViewModel.SelectedLocationIndex].Alias;
                        SelectedActionPlan.IdEmployee = (Int32)APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].IdEmployee;
                        SelectedActionPlan.FirstName = APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].FirstName;
                        SelectedActionPlan.LastName = APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].LastName;
                        SelectedActionPlan.FullName = APMCommon.Instance.ResponsibleList[addEditActionPlanViewModel.SelectedResponsibleIndex].FullName;
                        SelectedActionPlan.IdLookupBusinessUnit = APMCommon.Instance.BusinessUnitList[addEditActionPlanViewModel.SelectedBusinessIndex].IdLookupValue;
                        SelectedActionPlan.BusinessUnit = APMCommon.Instance.BusinessUnitList[addEditActionPlanViewModel.SelectedBusinessIndex].Value;
                        SelectedActionPlan.CreatedByName = addEditActionPlanViewModel.CreatedBy;
                        SelectedActionPlan.CreatedIn = addEditActionPlanViewModel.CreatedIn;
                        SelectedActionPlan.IdLookupOrigin = APMCommon.Instance.OriginList[addEditActionPlanViewModel.SelectedOriginIndex].IdLookupValue;
                        SelectedActionPlan.Origin = APMCommon.Instance.OriginList[addEditActionPlanViewModel.SelectedOriginIndex].Value;
                        SelectedActionPlan.IdDepartment = (Int32)APMCommon.Instance.DepartmentList[addEditActionPlanViewModel.SelectedDepartmentIndex].IdDepartment;
                        SelectedActionPlan.Department = APMCommon.Instance.DepartmentList[addEditActionPlanViewModel.SelectedDepartmentIndex].DepartmentName;
                        SelectedActionPlan.OriginDescription = addEditActionPlanViewModel.OriginDescription;
                        SelectedActionPlan.TaskList = new List<APMActionPlanTask>(addEditActionPlanViewModel.TaskList);

                        IsActionPlanEdit = true;
                        ClonedActionPlanList.FirstOrDefault(x => x.IdActionPlan == SelectedActionPlan.IdActionPlan).TaskList = new List<APMActionPlanTask>(addEditActionPlanViewModel.TaskList);
                        FillLeftTileList();
                        SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                        FillActionPlansAlertSectionList();//[shweta.thube][GEOS2-7217][28.07.2025]
                        FilterCommandAction(obj);

                        // CommandAlertFilterTileClickAction(obj);
                        //SelectedAlertTileBarItem = AlertListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedAlertTileBarItem.Caption);
                        IsActionPlanEdit = false;
                        IsExpand = false;
                        // ExpandAndCollapseActionPlanCommandAction(obj);
                    }
                    else
                    {
                        SelectedActionPlan.TaskList = new List<APMActionPlanTask>(addEditActionPlanViewModel.TempTaskList);
                        SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == PreviousSelectedTileBarItem.Caption);
                        FillActionPlansAlertSectionList();//[shweta.thube][GEOS2-7217][28.07.2025]
                        FilterCommandAction(obj);
                        IsExpand = false;
                        // ExpandAndCollapseActionPlanCommandAction(obj);
                    }
                }



                GeosApplication.Instance.Logger.Log("Method EditActionPlanTaskHyperLinkCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditActionPlanTaskHyperLinkCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-8683][01.07.2025]
        public void DeleteSubTaskCommandAction(object obj)
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteSubTasksDetailsMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    APMActionPlanSubTask Temp = (APMActionPlanSubTask)obj;
                    Temp.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();
                    Temp.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                    {
                        IdActionPlan = Temp.IdActionPlan,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Datetime = GeosApplication.Instance.ServerDateTime,

                        Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDeletedChangeLog").ToString(), Temp.SubTaskCode)
                    });
                    //APMService = new APMServiceController("localhost:6699");
                    IsDeleted = APMService.DeleteSubTaskForActionPlan_V2650(Temp.IdActionPlanTask, Temp.ActionPlanLogEntries);
                    if (!IsTaskGridVisibility)
                    {
                        SelectedActionPlan = ActionPlanList.FirstOrDefault(x => x.IdActionPlan == Temp.IdActionPlan);
                        var task = SelectedActionPlan?.TaskList?.FirstOrDefault(t => t.IdActionPlanTask == Temp.IdParent);

                        if (task != null)
                        {
                            if (task != null && task.SubTaskList != null)
                            {
                                task.SubTaskList.RemoveAll(sub => sub.IdActionPlanTask == Temp.IdActionPlanTask);

                            }
                            SelectedActionPlan.TaskList = new ObservableCollection<APMActionPlanTask>(SelectedActionPlan.TaskList).ToList();
                            ActionPlanList = new ObservableCollection<APMActionPlan>(ActionPlanList);       //[shweta.thube][GEOS2-8985]
                            LastUpdatedIdActionPlan = Temp.IdActionPlan;//[pallavi.kale][GEOS2-7213][25.07.2025]
                            LastUpdatedIdActionPlanTask = task.IdActionPlanTask;//[pallavi.kale][GEOS2-7213][25.07.2025]
                        }
                    }
                    else
                    {
                        SelectedActionPlanTask = TaskGridList.FirstOrDefault(x => x.IdActionPlanTask == Temp.IdParent);
                        if (SelectedActionPlanTask != null)
                        {
                            var taskToRemove = SelectedActionPlanTask.SubTaskList.FirstOrDefault(t => t.IdActionPlanTask == Temp.IdActionPlanTask); // Assuming IdTask uniquely identifies tasks
                            if (taskToRemove != null)
                            {
                                SelectedActionPlanTask.SubTaskList.Remove(taskToRemove);
                            }
                            SelectedActionPlanTask.SubTaskList = new ObservableCollection<APMActionPlanSubTask>(SelectedActionPlanTask.SubTaskList).ToList();
                            TaskGridList = new ObservableCollection<APMActionPlanTask>(TaskGridList);       //[shweta.thube][GEOS2-8985]
                        }
                    }
                    if (IsDeleted)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SubTaskDetailsDeletedsuccessfully").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                    //[pallavi.kale][GEOS2-7213][25.07.2025]
                    if (LastUpdatedIdActionPlan > 0)
                    {
                        var updatedTask = SelectedActionPlan?.TaskList?.FirstOrDefault(t => t.IdActionPlanTask == LastUpdatedIdActionPlanTask);
                        if (updatedTask != null && updatedTask.SubTaskList != null && updatedTask.SubTaskList.Any())
                        {
                            IsExpandTaskLevelOnly = false;

                        }
                        else
                        {
                            IsExpandTaskLevelOnly = true;
                        }
                        ExpandLastUpdatedActionPlan();
                    }

                }
                IsExpand = false;
                IsDeleted = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DeleteSubTaskCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteSubTaskCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteSubTaskCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteSubTaskCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-7217][23.07.2025]
        //[shweta.thube][GEOS2-7217][23.07.2025]
        private void FillActionPlansAlertSectionList()
        {
            try
            {
                var baseline = BuildBaselineForAlertTiles();
                if (baseline == null || baseline.Count == 0)
                {
                    baseline = ActionPlanList?.Select(ap => (APMActionPlan)ap.Clone()).ToList() ?? new List<APMActionPlan>();
                }

                var personIdSet = ToResponsibleIdSet(IsTaskGridVisibility ? SelectedPersonForTask : SelectedPerson);
                var personNameSet = ToResponsibleHashSet(IsTaskGridVisibility ? SelectedPersonForTask : SelectedPerson);
                bool hasResponsibleFilter = (personIdSet != null && personIdSet.Count > 0) || (personNameSet != null && personNameSet.Count > 0);

                var strictCalculationList = new List<APMActionPlanTask>();

                foreach (var ap in baseline)
                {
                    if (ap.TaskList == null) continue;

                    foreach (var t in ap.TaskList)
                    {
                        bool parentIsMatch = true;
                        if (hasResponsibleFilter)
                        {
                            parentIsMatch = false;
                            if (personIdSet != null && (
                                personIdSet.Contains(t.ActionPlanResponsibleIdUser) ||
                                personIdSet.Contains(t.IdEmployee) ||
                                personIdSet.Contains(t.ResponsibleIdUser)))
                            {
                                parentIsMatch = true;
                            }
                            else if (personNameSet != null)
                            {
                                var tName = (t.TaskResponsibleDisplayName ?? t.Responsible ?? string.Empty).Trim();
                                if (!string.IsNullOrWhiteSpace(tName) && personNameSet.Contains(tName))
                                    parentIsMatch = true;
                            }
                        }

                        if (parentIsMatch)
                        {
                            strictCalculationList.Add(t);
                        }

                        if (t.SubTaskList != null && t.SubTaskList.Count > 0)
                        {
                            foreach (var st in t.SubTaskList)
                            {
                                bool subIsMatch = true;
                                if (hasResponsibleFilter)
                                {
                                    subIsMatch = false;
                                    if (personIdSet != null && personIdSet.Contains(st.IdEmployee))
                                    {
                                        subIsMatch = true;
                                    }
                                    else if (personNameSet != null)
                                    {
                                        var stName = (st.Responsible ?? string.Empty).Trim();
                                        if (!string.IsNullOrWhiteSpace(stName) && personNameSet.Contains(stName))
                                            subIsMatch = true;
                                    }
                                }

                                if (subIsMatch)
                                {

                                    var subTaskAsTask = (APMActionPlanTask)t.Clone();


                                    subTaskAsTask.DueDays = st.DueDays;
                                    subTaskAsTask.DueDate = st.DueDate;
                                    subTaskAsTask.Status = st.Status;
                                    subTaskAsTask.IdLookupStatus = st.IdLookupStatus;
                                    subTaskAsTask.Priority = st.Priority;
                                    subTaskAsTask.Theme = st.Theme;
                                    subTaskAsTask.TaskResponsibleDisplayName = st.Responsible;
                                    subTaskAsTask.Responsible = st.Responsible;
                                    subTaskAsTask.FirstName = st.Responsible;

                                    strictCalculationList.Add(subTaskAsTask);
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(MyTaskFilterString))
                {
                    strictCalculationList = ApplyCustomFilterToTasks(strictCalculationList, MyTaskFilterString);
                }

                TempTaskGridListForAlert = new ObservableCollection<APMActionPlanTask>(strictCalculationList);

                if (TempTaskGridListForAlert != null)
                {
                    AlertListOfFilterTileForGridView = new ObservableCollection<APMAlertTileBarFilters>();

                    #region Longest Over Due
                    var overdueTasks = TempTaskGridListForAlert
                        .Where(task => task.DueDate < DateTime.Now && task.IdLookupStatus != 1982);

                    var longestOverdueTask = overdueTasks
                        .OrderByDescending(task => task.DueDays)
                        .FirstOrDefault();

                    var longestOverdueDays = longestOverdueTask != null ? longestOverdueTask.DueDays : 0;

                    var backColor = longestOverdueDays >= 5 ? "Red" : longestOverdueDays > 0 ? "Orange" : "Green";

                    var filterCriteria = longestOverdueTask != null && longestOverdueDays >= 5
                          ? $"[Code] = '{longestOverdueTask.Code}'" : "[Code] IS NULL";

                    AlertListOfFilterTileForGridView.Add(new APMAlertTileBarFilters()
                    {
                        Caption = "Longest Overdue Days",
                        Id = 0,
                        BackColor = backColor,
                        EntitiesCount = longestOverdueDays >= 5 ? $"{longestOverdueDays}" : "0",
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 50,
                        width = 150,
                        FilterCriteria = filterCriteria,
                    });
                    #endregion

                    #region Over Due
                    int overdueThresholdDays = 15;
                    DateTime thresholdDate = DateTime.Now.AddDays(-overdueThresholdDays);
                    var overdueThresholdCount = TempTaskGridListForAlert
                        .Count(task => (DateTime.Now - task.DueDate).TotalDays >= overdueThresholdDays && task.IdLookupStatus != 1982);
                    var backColorOverDue = overdueThresholdCount == 0 ? "Green" :
                                    overdueThresholdCount < 5 ? "Orange" : "Red";

                    AlertListOfFilterTileForGridView.Add(new APMAlertTileBarFilters()
                    {
                        Caption = "Overdue >= 15 days",
                        Id = 1,
                        BackColor = backColorOverDue,
                        EntitiesCount = $"{overdueThresholdCount}",
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 50,
                        width = 150,
                        FilterCriteria = $"[DueDate] <= #{thresholdDate:yyyy-MM-ddTHH:mm:ss}#"
                    });
                    #endregion

                    #region High Priority Over Due
                    int highPriorityOverdueDays = 5;
                    DateTime highPriorityThresholdDate = DateTime.Now.AddDays(-highPriorityOverdueDays);
                    var highPriorityOverdueTask = TempTaskGridListForAlert
                        .Where(task => (DateTime.Now - task.DueDate).TotalDays >= highPriorityOverdueDays
                                       && task.Priority == "High" && task.IdLookupStatus != 1982)
                        .OrderByDescending(task => (DateTime.Now - task.DueDate).TotalDays)
                        .FirstOrDefault();
                    var highPriorityOverdueCount = highPriorityOverdueTask != null
                        ? TempTaskGridListForAlert.Count(task => (DateTime.Now - task.DueDate).TotalDays >= highPriorityOverdueDays && task.Priority == "High" && task.IdLookupStatus != 1982)
                        : 0;

                    var backColorHighPriority = highPriorityOverdueCount == 0 ? "Green" :
                        highPriorityOverdueCount < 5 ? "Orange" : "Red";

                    var highPriorityFilterCriteria = highPriorityOverdueTask != null
                        ? $"[DueDate] <= #{highPriorityThresholdDate:yyyy-MM-ddTHH:mm:ss}# AND [Priority] = 'High'"
                        : $"[DueDate] <= #{highPriorityThresholdDate:yyyy-MM-ddTHH:mm:ss}# AND [Priority] = 'High'";

                    AlertListOfFilterTileForGridView.Add(new APMAlertTileBarFilters()
                    {
                        Caption = "High Priority Overdue",
                        Id = 2,
                        BackColor = backColorHighPriority,
                        EntitiesCount = $"{highPriorityOverdueCount}",
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 50,
                        width = 150,
                        FilterCriteria = highPriorityFilterCriteria
                    });
                    #endregion

                    #region Epic With Most Over Due
                    var overdueThreshold = DateTime.Now.AddDays(-5);
                    var formattedThresholdDate = overdueThreshold.ToString("yyyy-MM-dd HH:mm:ss");

                    var overdueTasksFilterCriteria = $"[DueDate] < #{formattedThresholdDate}#";
                    var groupedTasks = TempTaskGridListForAlert
                        .Where(task => task.DueDate < overdueThreshold && task.IdLookupStatus != 1982)
                        .GroupBy(task => task.Theme)
                        .OrderByDescending(group => group.Count())
                        .ToList();

                    var mostOverdueGroup = groupedTasks.FirstOrDefault();

                    var themeWithMostOverdueTasks = mostOverdueGroup?.Key ?? "---";
                    var overdueCount = mostOverdueGroup?.Count() ?? 0;

                    var specificTaskFilterCriteria = mostOverdueGroup != null
                        ? $"[Theme] = '{themeWithMostOverdueTasks.Replace("'", "''")}'"
                        : "[Theme] IS NULL";

                    var combinedFilterCriteria = mostOverdueGroup != null
                        ? $"{overdueTasksFilterCriteria} AND {specificTaskFilterCriteria}"
                        : overdueTasksFilterCriteria;

                    AlertListOfFilterTileForGridView.Add(new APMAlertTileBarFilters()
                    {
                        Caption = "Most Overdue Theme",
                        Id = 0,
                        BackColor = backColor,
                        EntitiesCount = $"{themeWithMostOverdueTasks}",
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 50,
                        width = 150,
                        FilterCriteria = combinedFilterCriteria,
                    });
                    #endregion

                    #region Responsible with Most Over Due
                    int overdueDaysThreshold = 5;

                    DateTime overdueThresholdDate = DateTime.Now.AddDays(-overdueDaysThreshold);
                    var responsibleWithMostOverdue = TempTaskGridListForAlert
                        .Where(task => (DateTime.Now - task.DueDate).TotalDays >= overdueDaysThreshold && task.IdLookupStatus != 1982)
                        .GroupBy(task => task.FirstName)
                        .OrderByDescending(group => group.Count())
                        .FirstOrDefault();

                    string responsibleName = responsibleWithMostOverdue?.Key ?? "---";

                    int overdueTaskCount = responsibleWithMostOverdue?.Count() ?? 0;

                    AlertListOfFilterTileForGridView.Add(new APMAlertTileBarFilters()
                    {
                        Caption = "Most Overdue Responsible",
                        Id = 3,
                        BackColor = backColor,
                        EntitiesCount = $"{responsibleName}",
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 50,
                        width = 150,
                        FilterCriteria = $"[DueDate] <= #{overdueThresholdDate:yyyy-MM-ddTHH:mm:ss}# AND [FirstName] = '{responsibleName}'"
                    });
                    #endregion

                    #region Sorting Indicators
                    AlertListOfFilterTileForGridView = new ObservableCollection<APMAlertTileBarFilters>(AlertListOfFilterTileForGridView
                        .Take(5).OrderBy(tile =>
                        {
                            switch (tile.BackColor)
                            {
                                case "Red": return 1;
                                case "Orange": return 2;
                                case "Green": return 3;
                                default: return 4;
                            }
                        }).Concat(AlertListOfFilterTileForGridView.Skip(5)).ToList());
                    #endregion

                    #region Status
                    IList<LookupValue> temp = APMService.GetLookupValues_V2550(152).ToList();
                    temp.OrderBy(x => x.Position);

                    foreach (var item in temp)
                    {
                        AlertListOfFilterTileForGridView.Add(new APMAlertTileBarFilters()
                        {
                            Caption = item.Value,
                            Id = item.IdLookupValue,
                            BackColor = item.HtmlColor,
                            EntitiesCount = TempTaskGridListForAlert.Count(task => task.IdLookupStatus == item.IdLookupValue).ToString(),
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 50,
                            width = 150,
                            FilterCriteria = $"StartsWith([Status], '{item.Value}')"
                        });
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillActionPlansAlertSectionList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-7217][23.07.2025]
        private void CommandActionPlansAlertFilterTileClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandActionPlansAlertFilterTileClickAction....", category: Category.Info, priority: Priority.Low);

                if (obj is MouseButtonEventArgs mouseArgs)
                {
                    if (mouseArgs.Source is System.Windows.Controls.Control clickedControl)
                    {
                        var clickedItem = clickedControl.DataContext as APMAlertTileBarFilters;

                        if (clickedItem != null)
                        {
                            string filter = clickedItem.FilterCriteria;
                            string normalizedMyTaskFilter = RemoveTimeFromFilterString(MyChildFilterString);
                            string normalizedFilter = RemoveTimeFromFilterString(filter);
                            if (!string.IsNullOrEmpty(MyChildFilterString))
                            {
                                if (normalizedMyTaskFilter == normalizedFilter)
                                {
                                    MyChildFilterString = string.Empty;
                                    ActionPlanList = new ObservableCollection<APMActionPlan>(TempActionPlanList);
                                    FilterCommandAction(obj);
                                    FillActionPlansAlertSectionList();  //[shweta.thube][GEOS2-9077][23.07.2025]
                                    SelectedActionPlansAlertTileBarItem = new APMAlertTileBarFilters();
                                    PreviousSelectedActionPlansAlertTileBarItem = null;
                                    MyChildFilterString = null;
                                }
                                else
                                {
                                    FilterCommandAction(obj);
                                    MyChildFilterString = !string.IsNullOrEmpty(filter) ? filter : string.Empty;
                                    if (clickedItem.Caption == "Longest Overdue Days")
                                    {
                                        string code = ExtractFilterValue(filter, "Code");
                                        string taskNumber = ExtractFilterValue(filter, "TaskNumber");
                                        int taskNum = int.Parse(taskNumber);
                                        var filteredActionPlans = ActionPlanList
                                            .Where(ap => ap.Code == code && ap.TaskList != null)
                                            .Select(ap =>
                                            {
                                                var filteredTasks = ap.TaskList
                                                    .Where(t => t.TaskNumber == taskNum && t.DueDays >= 15 && t.IdLookupStatus != 1982)
                                                    .ToList();

                                                if (filteredTasks.Any())
                                                {
                                                    ap.TaskList = filteredTasks;
                                                    return ap;
                                                }
                                                return null;
                                            })
                                            .Where(ap => ap != null)
                                            .ToList();

                                        ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionPlans);
                                        SelectedActionPlansAlertTileBarItem = AlertListOfFilterTileForGridView.FirstOrDefault(x => x.Caption == clickedItem.Caption);
                                    }
                                    else
                                    {
                                        var filteredActionPlans = ActionPlanList
                                            .Where(ap => ap.TaskList != null)
                                            .Select(ap =>
                                            {
                                                List<APMActionPlanTask> filteredTasks = new List<APMActionPlanTask>();

                                                if (clickedItem.Caption == "Longest Overdue Days")
                                                {
                                                    string code = ExtractFilterValue(filter, "Code");
                                                    string TaskNumber = ExtractFilterValue(filter, "TaskNumber");
                                                    filteredTasks = ap.TaskList
                                                        .Where(t => t.DueDays >= 15 && t.IdLookupStatus != 1982)
                                                        .ToList();
                                                }

                                                else if (clickedItem.Caption == "Most Overdue Theme")
                                                {
                                                    DateTime dueDate = ExtractDateFromFilter(filter);
                                                    string theme = ExtractFilterValue(filter, "Theme");
                                                    filteredTasks = ap.TaskList
                                                        .Where(t => t.DueDate < dueDate && t.Theme == theme && t.IdLookupStatus != 1982)
                                                        .ToList();
                                                }
                                                else if (clickedItem.Caption == "High Priority Overdue")
                                                {
                                                    DateTime dueDate = ExtractDateFromFilter(filter);
                                                    string Priority = ExtractFilterValue(filter, "Priority");
                                                    filteredTasks = ap.TaskList
                                                        .Where(t => t.DueDate < dueDate && t.Priority == Priority && t.IdLookupStatus != 1982)
                                                        .ToList();
                                                }
                                                else if (clickedItem.Caption == "Overdue >= 15 days")
                                                {
                                                    filteredTasks = ap.TaskList
                                                        .Where(t => (DateTime.Now - t.DueDate).TotalDays >= 15 && t.IdLookupStatus != 1982)
                                                        .ToList();
                                                }
                                                else if (clickedItem.Caption == "Most Overdue Responsible")
                                                {
                                                    DateTime dueDate = ExtractDateFromFilter(filter);
                                                    string Responsible = ExtractFilterValue(filter, "FirstName");
                                                    filteredTasks = ap.TaskList
                                                        .Where(t => t.DueDate < dueDate && t.FirstName == Responsible && t.IdLookupStatus != 1982)
                                                        .ToList();

                                                }
                                                else if (clickedItem.Caption == "To Do" || clickedItem.Caption == "In Progress" || clickedItem.Caption == "Blocked" || clickedItem.Caption == "Done")
                                                {
                                                    filteredTasks = ap.TaskList
                                                        .Where(t => t.IdLookupStatus == clickedItem.Id)
                                                        .ToList();
                                                }
                                                if (filteredTasks.Any())
                                                {
                                                    ap.TaskList = filteredTasks;
                                                    return ap;
                                                }
                                                return null;
                                            })
                                            .Where(ap => ap != null)
                                            .ToList();


                                        ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionPlans);

                                        int totalTaskCount = ActionPlanList
            .Where(ap => ap.TaskList != null)
            .Sum(ap => ap.TaskList.Count);

                                        SelectedActionPlansAlertTileBarItem = AlertListOfFilterTileForGridView.FirstOrDefault(x => x.Caption == clickedItem.Caption);
                                    }

                                }
                            }
                            else
                            {
                                MyChildFilterString = !string.IsNullOrEmpty(filter) ? filter : string.Empty;
                                if (clickedItem.Caption == "Longest Overdue Days")
                                {
                                    string code = ExtractFilterValue(filter, "Code");
                                    string taskNumber = ExtractFilterValue(filter, "TaskNumber");
                                    int taskNum = int.Parse(taskNumber);
                                    var filteredActionPlans = ActionPlanList
                                        .Where(ap => ap.Code == code && ap.TaskList != null)
                                        .Select(ap =>
                                        {
                                            var filteredTasks = ap.TaskList
                                                .Where(t => t.TaskNumber == taskNum && t.DueDays >= 15 && t.IdLookupStatus != 1982)
                                                .ToList();

                                            if (filteredTasks.Any())
                                            {
                                                ap.TaskList = filteredTasks;
                                                return ap;
                                            }
                                            return null;
                                        })
                                        .Where(ap => ap != null)
                                        .ToList();

                                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionPlans);
                                    SelectedActionPlansAlertTileBarItem = AlertListOfFilterTileForGridView.FirstOrDefault(x => x.Caption == clickedItem.Caption);
                                }
                                else
                                {
                                    var filteredActionPlans = ActionPlanList
                                        .Where(ap => ap.TaskList != null)
                                        .Select(ap =>
                                        {
                                            List<APMActionPlanTask> filteredTasks = new List<APMActionPlanTask>();

                                            if (clickedItem.Caption == "Longest Overdue Days")
                                            {
                                                string code = ExtractFilterValue(filter, "Code");
                                                string TaskNumber = ExtractFilterValue(filter, "TaskNumber");
                                                filteredTasks = ap.TaskList
                                                    .Where(t => t.DueDays >= 15 && t.IdLookupStatus != 1982)
                                                    .ToList();
                                            }

                                            else if (clickedItem.Caption == "Most Overdue Theme")
                                            {
                                                DateTime dueDate = ExtractDateFromFilter(filter);
                                                string theme = ExtractFilterValue(filter, "Theme");
                                                filteredTasks = ap.TaskList
                                                    .Where(t => t.DueDate < dueDate && t.Theme == theme && t.IdLookupStatus != 1982)
                                                    .ToList();
                                            }
                                            else if (clickedItem.Caption == "High Priority Overdue")
                                            {
                                                DateTime dueDate = ExtractDateFromFilter(filter);
                                                string Priority = ExtractFilterValue(filter, "Priority");
                                                filteredTasks = ap.TaskList
                                                    .Where(t => t.DueDate < dueDate && t.Priority == Priority && t.IdLookupStatus != 1982)
                                                    .ToList();
                                            }
                                            else if (clickedItem.Caption == "Overdue >= 15 days")
                                            {
                                                filteredTasks = ap.TaskList
                                                         .Where(t => (DateTime.Now - t.DueDate).TotalDays >= 15 && t.IdLookupStatus != 1982)
                                                         .ToList();
                                            }
                                            else if (clickedItem.Caption == "Most Overdue Responsible")
                                            {
                                                DateTime dueDate = ExtractDateFromFilter(filter);
                                                string Responsible = ExtractFilterValue(filter, "FirstName");
                                                filteredTasks = ap.TaskList
                                                    .Where(t => t.DueDate < dueDate && t.FirstName == Responsible && t.IdLookupStatus != 1982)
                                                    .ToList();

                                            }
                                            else if (clickedItem.Caption == "To Do" || clickedItem.Caption == "In Progress" || clickedItem.Caption == "Blocked" || clickedItem.Caption == "Done")
                                            {
                                                filteredTasks = ap.TaskList
                                                    .Where(t => t.IdLookupStatus == clickedItem.Id)
                                                    .ToList();
                                            }
                                            if (filteredTasks.Any())
                                            {
                                                ap.TaskList = filteredTasks;
                                                return ap;
                                            }
                                            return null;
                                        })
                                        .Where(ap => ap != null)
                                        .ToList();


                                    ActionPlanList = new ObservableCollection<APMActionPlan>(filteredActionPlans);

                                    int totalTaskCount = ActionPlanList
        .Where(ap => ap.TaskList != null)
        .Sum(ap => ap.TaskList.Count);

                                    SelectedActionPlansAlertTileBarItem = AlertListOfFilterTileForGridView.FirstOrDefault(x => x.Caption == clickedItem.Caption);
                                }

                            }
                        }


                    }
                }




                GeosApplication.Instance.Logger.Log("Method CommandActionPlansAlertFilterTileClickAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method CommandActionPlansAlertFilterTileClickAction: " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-7217][23.07.2025]
        private DateTime ExtractDateFromFilter(string filter)
        {
            var match = System.Text.RegularExpressions.Regex.Match(filter, @"#(.*?)#");
            if (match.Success && DateTime.TryParse(match.Groups[1].Value, out DateTime result))
            {
                return result;
            }
            return DateTime.Now;
        }

        //[shweta.thube][GEOS2-7217][23.07.2025]
        private string ExtractFilterValue(string filter, string fieldName)
        {
            var match = System.Text.RegularExpressions.Regex.Match(filter, $@"\[{fieldName}\]\s*=\s*'(.*?)'");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        //[pallavi.kale][GEOS2-7213][25.07.2025]
        private void ExpandLastUpdatedActionPlan()
        {
            if (LastUpdatedIdActionPlan == 0 || _gridControl == null)
                return;

            for (int i = 0; i < _gridControl.VisibleRowCount; i++)
            {
                int apHandle = _gridControl.GetRowHandleByVisibleIndex(i);
                var actionPlan = _gridControl.GetRow(apHandle) as APMActionPlan;

                if (actionPlan != null && actionPlan.IdActionPlan == LastUpdatedIdActionPlan)
                {

                    _gridControl.ExpandMasterRow(apHandle);
                    _gridControl.UpdateLayout();

                    if (!IsExpandTaskLevelOnly && LastUpdatedIdActionPlanTask > 0)
                    {
                        var taskGrid = _gridControl.GetDetail(apHandle) as GridControl;
                        if (taskGrid != null)
                        {
                            taskGrid.UpdateLayout();

                            for (int j = 0; j < taskGrid.VisibleRowCount; j++)
                            {
                                int taskHandle = taskGrid.GetRowHandleByVisibleIndex(j);
                                var task = taskGrid.GetRow(taskHandle) as APMActionPlanTask;

                                if (task != null && task.IdActionPlanTask == LastUpdatedIdActionPlanTask)
                                {
                                    if (task?.SubTaskList?.Any() == true)
                                    {
                                        taskGrid.ExpandMasterRow(taskHandle);
                                    }

                                    break;
                                }
                            }
                        }
                    }

                    break;
                }
            }
        }
        //[pallavi.kale][GEOS2-8084][05.08.2025]
        private void FillCustomerListForTask()
        {
            ListOfCustomerForTask = new ObservableCollection<APMCustomer>();

            if (TaskGridList != null && TaskGridList.Count > 0)
            {
                foreach (var item in TaskGridList)
                {
                    if (item.IdSite > 0)
                    {
                        if (!ListOfCustomerForTask.Any(x => x.IdSite == item.IdSite))
                        {
                            ListOfCustomerForTask.Add(new APMCustomer()
                            {
                                GroupName = item.GroupName,
                                Site = item.Site,
                                IdSite = item.IdSite
                            });


                        }
                    }
                    else
                    {
                        if (!ListOfCustomerForTask.Any(x => x.IdSite == item.IdSite))
                        {
                            ListOfCustomerForTask.Insert(0, new APMCustomer()
                            {
                                GroupName = "Blanks",
                                Site = item.Site,
                                IdSite = item.IdSite
                            });


                        }
                    }

                }
            }
            else
            {
                ListOfCustomerForTask = new ObservableCollection<APMCustomer>();
            }

            if (SelectedCustomerForTask == null)
            {
                SelectedCustomerForTask = new List<object>();
            }
        }
        //[pallavi.kale][GEOS2-8084][29.08.2025]
        private void ExportCustomerButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportCustomerButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                var parameters = obj as object[];
                TableView actionPlansView = parameters[0] as TableView;
                TableView tasksGridView = parameters[1] as TableView;
                List<APMActionPlan> actionPlans = new List<APMActionPlan>();
                //[pallavi.kale][GEOS2-9205][03.09.2025]
                if (IsTaskGridVisibility)
                {
                    var tasks = GetFilteredItems<APMActionPlanTask>(tasksGridView);
                    var actionPlanIds = tasks.Select(t => t.IdActionPlan).Distinct().ToList();
                    var actionPlanInfos = GetFilteredItems<APMActionPlan>(actionPlansView)
                        .Where(ap => actionPlanIds.Contains(ap.IdActionPlan))
                        .ToList();

                    actionPlans = actionPlanInfos.Select(ap =>
                    {
                        ap.TaskList = tasks.Where(t => t.IdActionPlan == ap.IdActionPlan).ToList();
                        return ap;
                    }).ToList();
                }
                else
                {
                    actionPlans = GetFilteredItems<APMActionPlan>(actionPlansView);
                }
                if (actionPlans == null || actionPlans.Count == 0)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                        DXSplashScreen.Close();

                    CustomMessageBox.Show(
                            string.Format(Application.Current.Resources["APMActionPlanViewEmptyError"].ToString()),
                            "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    IsBusy = false;
                    return;

                }
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog
                {
                    DefaultExt = "xlsx",
                    FileName = IsTaskGridVisibility ? "Tasks List" : "Action Plans List",
                    Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*",
                    FilterIndex = 1,
                    Title = "Save Excel Report"
                };
                DialogResult = (Boolean)saveFile.ShowDialog();
                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                    return;
                }
                else
                {

                    IsBusy = true;
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Show(x =>
                        {
                            Window win = new Window()
                            {
                                ShowActivated = false,
                                WindowStyle = WindowStyle.None,
                                ResizeMode = ResizeMode.NoResize,
                                AllowsTransparency = true,
                                Background = new SolidColorBrush(Colors.Transparent),
                                ShowInTaskbar = false,
                                Topmost = true,
                                SizeToContent = SizeToContent.WidthAndHeight,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                            };
                            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                            win.Topmost = false;
                            return win;
                        }, x =>
                        {
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }

                    ResultFileName = saveFile.FileName;

                    Workbook mainWorkbook = new Workbook();
                    mainWorkbook.CreateNewDocument();
                    byte[] excelBytes = APMService.GetCustomerActionPlanExcel();
                    if (excelBytes == null || excelBytes.Length == 0)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                            DXSplashScreen.Close();

                        string templatePath = APMService.GetAPMTemplateFilePath("APMCustomerExcelTemplate.xlsx");
                        CustomMessageBox.Show(string.Format(Application.Current.Resources["APMActionPlanTemplateError"].ToString()) + " : " + templatePath, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                        IsBusy = false;
                        return;

                    }


                    Workbook templateWorkbook = new Workbook();
                    using (MemoryStream stream = new MemoryStream(excelBytes))
                    {
                        templateWorkbook.LoadDocument(stream, DocumentFormat.OpenXml);
                    }

                    CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                    System.Globalization.Calendar calendar = cultureInfo.Calendar;
                    CalendarWeekRule weekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
                    DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
                    int weekNumber = calendar.GetWeekOfYear(DateTime.Now, weekRule, firstDayOfWeek);
                    string YearWeek = DateTime.Now.Year + "CW" + weekNumber;

                    foreach (var group in actionPlans.GroupBy(ap => ap.Code))
                    {
                        Worksheet templateSheet = templateWorkbook.Worksheets[0];
                        Worksheet sheet = mainWorkbook.Worksheets.Add();
                        sheet.CopyFrom(templateSheet);

                        var firstAp = group.First();
                        sheet.Name = "ETM_" + firstAp.Code;

                        sheet.Cells["B7"].Value = firstAp.Code;
                        sheet.Cells["C7"].Value = firstAp.Location;
                        sheet.Cells["D7"].Value = firstAp.Description;
                        sheet.Cells["E7"].Value = firstAp.Origin;
                        sheet.Cells["F7"].Value = firstAp.BusinessUnit;
                        sheet.Cells["G7"].Value = firstAp.Department;
                        sheet.Cells["I7"].Value = firstAp.ActionPlanResponsibleDisplayName;
                        sheet.Cells["K7"].Value = firstAp.Group;
                        sheet.Cells["L7"].Value = firstAp.Site;
                        sheet.Cells["N7"].Value = firstAp.CreatedIn?.ToShortDateString();
                        sheet.Cells["N3"].Value = YearWeek;
                        sheet.Cells["N2"].Value = DateTime.Now;

                        string assetsFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
                        string imagePath = Path.Combine(assetsFolder, @"Modules\APMModule\Assets\Images\logo.png");

                        if (File.Exists(imagePath))
                        {
                            sheet.Pictures.AddPicture(imagePath, sheet.Range["B3:C4"]);
                        }

                        var allTasks = group
                            .Where(g => g.TaskList != null)
                            .SelectMany(g => g.TaskList)
                            .Where(t => ShouldIncludeTask(t))
                            .ToList();

                        int startRow = 9;
                        foreach (var task in allTasks)
                        {
                            sheet.Cells[startRow, 1].Value = task.TaskNumber;
                            sheet.Cells[startRow, 2].Value = task.Title;
                            sheet.Cells[startRow, 3].Value = task.Description;
                            sheet.Cells[startRow, 4].Value = task.Responsible;
                            sheet.Cells[startRow, 5].Value = task.TaskLastComment;
                            sheet.Cells[startRow, 6].Value = task.Theme;
                            sheet.Cells[startRow, 7].Value = task.Priority;
                            sheet.Cells[startRow, 8].Value = GetTaskStatus(task);
                            sheet.Cells[startRow, 9].Value = task.OpenDate?.ToShortDateString();
                            sheet.Cells[startRow, 10].Value = task.LastUpdated?.ToShortDateString();
                            sheet.Cells[startRow, 11].Value = task.DueDate.ToShortDateString();
                            sheet.Cells[startRow, 12].Value = task.Progress + "%";
                            sheet.Cells[startRow, 13].Value = task.CodeNumber;

                            startRow++;
                        }

                        if (allTasks.Any())
                        {
                            var range = sheet.Range.FromLTRB(1, 9, 13, startRow - 1);
                            range.Alignment.WrapText = true;
                        }
                    }

                    if (mainWorkbook.Worksheets.Count > 1 && mainWorkbook.Worksheets[0].Name == "Sheet1")
                        mainWorkbook.Worksheets.RemoveAt(0);

                    mainWorkbook.SaveDocument(ResultFileName, DocumentFormat.OpenXml);

                    IsBusy = false;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportCustomerButtonCommandAction() executed successfully.", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Error in Method ExportCustomerButtonCommandAction(): " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pallavi.kale][GEOS2-8084][29.08.2025]
        private string GetTaskStatus(APMActionPlanTask task)
        {
            DateTime today = DateTime.Now;
            DateTime createdDate = task.OpenDate ?? today;
            DateTime dueDate = task.DueDate;

            if (task.Progress >= 100)
            {
                DateTime? closeDate = task.CloseDate;
                if (closeDate != null && closeDate.Value >= today.AddMonths(-3))
                    return "Completed";
                return "";
            }

            if (dueDate < today)
                return "Overdue";

            if ((dueDate - today).TotalDays <= 7)
                return "Caution";

            if ((today - createdDate).TotalDays > 7 && (dueDate - today).TotalDays > 7)
                return "On Going";

            if ((today - createdDate).TotalDays <= 7)
                return "New";

            return "";
        }
        //[pallavi.kale][GEOS2-8084][29.08.2025]
        private void PrintCustomerButtonCommandAction(object obj)
        {
            try
            {
                IsBusy = true;
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                var parameters = obj as object[];
                TableView actionPlansView = parameters[0] as TableView;
                TableView tasksGridView = parameters[1] as TableView;
                List<APMActionPlan> actionPlans = new List<APMActionPlan>();
                //[pallavi.kale][GEOS2-9205][03.09.2025]
                if (IsTaskGridVisibility)
                {
                    var tasks = GetFilteredItems<APMActionPlanTask>(tasksGridView);

                    var actionPlanIds = tasks.Select(t => t.IdActionPlan).Distinct().ToList();

                    var filteredActionPlans = GetFilteredItems<APMActionPlan>(actionPlansView)
                        .Where(ap => actionPlanIds.Contains(ap.IdActionPlan))
                        .ToList();

                    actionPlans = filteredActionPlans.Select(ap =>
                    {
                        ap.TaskList = tasks.Where(t => t.IdActionPlan == ap.IdActionPlan).ToList();
                        return ap;
                    }).ToList();
                }
                else
                {
                    actionPlans = GetFilteredItems<APMActionPlan>(actionPlansView);
                }
                if (actionPlans == null || actionPlans.Count == 0)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                        DXSplashScreen.Close();

                    CustomMessageBox.Show(
                            string.Format(Application.Current.Resources["APMActionPlanViewEmptyError"].ToString()),
                            "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    IsBusy = false;
                    return;

                }
                byte[] excelBytes = APMService.GetActionPlanExcel();
                if (excelBytes == null || excelBytes.Length == 0)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                        DXSplashScreen.Close();
                    string templatePath = APMService.GetAPMTemplateFilePath("APMExcelTemplate.xlsx");
                    CustomMessageBox.Show(string.Format(Application.Current.Resources["APMActionPlanTemplateError"].ToString()) + " : " + templatePath, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                    IsBusy = false;
                    return;

                }


                Workbook templateWorkbook = new Workbook();
                using (MemoryStream ms = new MemoryStream(excelBytes))
                {
                    templateWorkbook.LoadDocument(ms, DocumentFormat.OpenXml);
                }

                Workbook mainWorkbook = new Workbook();
                mainWorkbook.CreateNewDocument();
                Worksheet templateSheet = templateWorkbook.Worksheets[0];

                var cultureInfo = CultureInfo.CurrentCulture;
                var calendar = cultureInfo.Calendar;
                int weekNumber = calendar.GetWeekOfYear(DateTime.Now, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek);
                string YearWeek = DateTime.Now.Year + "CW" + weekNumber;

                mainWorkbook.BeginUpdate();

                foreach (var group in actionPlans.GroupBy(ap => ap.Code))
                {
                    var firstAp = group.First();
                    Worksheet sheet = mainWorkbook.Worksheets.Add();
                    sheet.CopyFrom(templateSheet);
                    sheet.Name = "ETM_" + firstAp.Code;


                    sheet.Cells["B7"].SetValue(firstAp.Code);
                    sheet.Cells["C7"].SetValue(firstAp.Location);
                    sheet.Cells["D7"].SetValue(firstAp.Description);
                    sheet.Cells["E7"].SetValue(firstAp.Origin);
                    sheet.Cells["F7"].SetValue(firstAp.BusinessUnit);
                    sheet.Cells["G7"].SetValue(firstAp.Department);
                    sheet.Cells["I7"].SetValue(firstAp.ActionPlanResponsibleDisplayName);
                    sheet.Cells["K7"].SetValue(firstAp.Group);
                    sheet.Cells["L7"].SetValue(firstAp.Site);
                    sheet.Cells["N7"].SetValue(firstAp.CreatedIn?.ToShortDateString());
                    sheet.Cells["N3"].SetValue(YearWeek);
                    sheet.Cells["N2"].SetValue(DateTime.Now);

                    var allTasks = group
                        .Where(g => g.TaskList != null)
                        .SelectMany(g => g.TaskList)
                        .Where(t => ShouldIncludeTask(t))
                        .ToList();

                    int startRow = 9;
                    foreach (var task in allTasks)
                    {
                        sheet.Cells[startRow, 1].SetValue(task.TaskNumber.ToString());
                        sheet.Cells[startRow, 2].SetValue(task.Title ?? "");
                        sheet.Cells[startRow, 3].SetValue(task.Description ?? "");
                        sheet.Cells[startRow, 4].SetValue(task.Responsible ?? "");
                        sheet.Cells[startRow, 5].SetValue(task.TaskLastComment ?? "");
                        sheet.Cells[startRow, 6].SetValue(task.Theme ?? "");
                        sheet.Cells[startRow, 7].SetValue(task.Priority ?? "");
                        sheet.Cells[startRow, 8].SetValue(GetTaskStatus(task));
                        sheet.Cells[startRow, 9].SetValue(task.OpenDate?.ToShortDateString() ?? "");
                        sheet.Cells[startRow, 10].SetValue(task.LastUpdated?.ToShortDateString() ?? "");
                        sheet.Cells[startRow, 11].SetValue(task.DueDate.ToShortDateString());
                        sheet.Cells[startRow, 12].SetValue(task.Progress.ToString() + "%");
                        sheet.Cells[startRow, 13].SetValue(task.CodeNumber ?? "");
                        startRow++;
                    }
                    if (allTasks.Any())
                    {
                        var taskRange = sheet.Range.FromLTRB(1, 9, 13, startRow - 1);
                        taskRange.Alignment.WrapText = true;
                    }
                }

                mainWorkbook.EndUpdate();

                string pdfFile = Path.Combine(Path.GetTempPath(), IsTaskGridVisibility ? "Tasks.pdf" : "ActionPlans.pdf");
                mainWorkbook.ExportToPdf(pdfFile);
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                System.Diagnostics.Process.Start(new ProcessStartInfo(pdfFile) { UseShellExecute = true });

                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Error in PrintCustomerButtonCommandAction: " + ex.Message, Category.Exception, Priority.Low);
            }
        }

        //[pallavi.kale][GEOS2-8084][29.08.2025]
        private bool ShouldIncludeTask(APMActionPlanTask task)
        {
            DateTime today = DateTime.Now;
            DateTime createdDate = task.OpenDate ?? today;
            DateTime dueDate = task.DueDate;

            if (task.Progress >= 100)
            {
                DateTime? closeDate = task.CloseDate;
                if (closeDate != null && closeDate.Value >= today.AddMonths(-3))
                    return true;
                else
                    return false;
            }

            // Overdue
            if (dueDate < today)
                return true;

            // Caution
            if ((dueDate - today).TotalDays <= 7)
                return true;

            // On Going
            if ((today - createdDate).TotalDays > 7 && (dueDate - today).TotalDays > 7)
                return true;

            // New
            if ((today - createdDate).TotalDays <= 7)
                return true;

            return false;
        }

        //[pallavi.kale][GEOS2-9205][03.09.2025]
        private List<T> GetFilteredItems<T>(TableView view)
        {
            if (view == null || view.DataControl == null || view.DataControl.ItemsSource == null)
                return new List<T>();

            var collectionView = CollectionViewSource.GetDefaultView(view.DataControl.ItemsSource);
            return collectionView?.Cast<T>().ToList() ?? new List<T>();
        }

        #endregion

        #region Column Chooser

        //[Sudhir.Jangra][GEOS2-5972]
        private void ActionPlanGridControlLoadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ActionPlanGridControlLoadedCommandAction...", category: Category.Info, priority: Priority.Low);
                int visibleFalseColumn = 0;
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;

                gridControl.BeginInit();

                if (File.Exists(ActionPlanGridSettingFilePath))
                {
                    gridControl.RestoreLayoutFromXml(ActionPlanGridSettingFilePath);
                }

                //This code for save grid layout.
                gridControl.SaveLayoutToXml(ActionPlanGridSettingFilePath);

                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, ActionPlanVisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, ActionPlanVisibleIndexChanged);
                    }

                    if (!column.Visible)
                    {
                        visibleFalseColumn++;
                    }
                }

                if (visibleFalseColumn > 0)
                {
                    IsActionPlanColumnChooserVisible = true;
                }
                else
                {
                    IsActionPlanColumnChooserVisible = false;
                }
                gridControl.EndInit();
                tableView.SearchString = null;
                tableView.ShowGroupPanel = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ActionPlanGridControlLoadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on ActionPlanGridControlLoadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void ActionPlanVisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ActionPlanVisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;

                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ActionPlanGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsActionPlanColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method ActionPlanVisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ActionPlanVisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void ActionPlanChildVisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ActionPlanChildVisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;

                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ActionPlanChildGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsActionPlanChildColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method ActionPlanChildVisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ActionPlanChildVisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void ActionPlanVisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ActionPlanVisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                if (((DevExpress.Xpf.Grid.ColumnBase)sender).ActualColumnChooserHeaderCaption.ToString() != "")
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ActionPlanGridSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method ActionPlanVisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ActionPlanVisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void ActionPlanChildVisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ActionPlanChildVisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                if (((DevExpress.Xpf.Grid.ColumnBase)sender).ActualColumnChooserHeaderCaption.ToString() != "")
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ActionPlanChildGridSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method ActionPlanChildVisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ActionPlanChildVisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5972]
        private void ActionPlanItemListTableViewLoadedCommandAction(object obj)
        {
            TableView tableView = obj as TableView;
            tableView.ColumnChooserState = new DefaultColumnChooserState
            {
                Location = new System.Windows.Point(20, 180),
                Size = new System.Windows.Size(250, 250)
            };
        }

        //[Sudhir.Jangra][GEOS2-5972]
        private void ActionPlanGridControlUnloadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ActionPlanGridControlUnloadedCommandAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(ActionPlanGridSettingFilePath);
                GeosApplication.Instance.Logger.Log("Method ActionPlanGridControlUnloadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on ActionPlanGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6453]
        private void ActionPlanTaskGridControlLoadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ActionPlanTaskGridControlLoadedCommandAction...", category: Category.Info, priority: Priority.Low);
                int visibleFalseColumn = 0;
                GridControl gridControl1 = obj as GridControl;
                TableView tableView = (TableView)gridControl1.View;

                gridControl1.BeginInit();

                if (File.Exists(ActionPlanTaskGridSettingFilePath))
                {
                    gridControl1.RestoreLayoutFromXml(ActionPlanTaskGridSettingFilePath);
                }

                //This code for save grid layout.
                gridControl1.SaveLayoutToXml(ActionPlanTaskGridSettingFilePath);

                foreach (GridColumn column in gridControl1.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, ActionPlanTaskVisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, ActionPlanTaskVisibleIndexChanged);
                    }

                    if (!column.Visible)
                    {
                        visibleFalseColumn++;
                    }
                }

                if (visibleFalseColumn > 0)
                {
                    IsActionPlanTaskGridColumnChooserVisible = true;
                }
                else
                {
                    IsActionPlanTaskGridColumnChooserVisible = false;
                }
                gridControl1.EndInit();
                tableView.SearchString = null;
                tableView.ShowGroupPanel = false;
                //[shweta.thube][GEOS2-7819]
                if (IsTaskGridVisibility)
                {
                    IsActionPlanChildColumnChooserVisible = false;
                    IsActionPlanColumnChooserVisible = false;
                    IsActionPlanTaskGridColumnChooserVisible = false;
                }
                else
                {
                    IsActionPlanChildColumnChooserVisible = false;
                    IsActionPlanColumnChooserVisible = false;
                    IsActionPlanTaskGridColumnChooserVisible = false;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ActionPlanTaskGridControlLoadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on ActionPlanTaskGridControlLoadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6453]
        private void ActionPlanTaskItemListTableViewLoadedCommandAction(object obj)
        {


            TableView tableView = obj as TableView;
            tableView.ColumnChooserState = new DefaultColumnChooserState
            {
                Location = new System.Windows.Point(20, 180),
                Size = new System.Windows.Size(250, 250)
            };
        }
        //[shweta.thube][GEOS2-6453]
        private void ActionPlanTaskGridControlUnloadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ActionPlanTaskGridControlUnloadedCommandAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl1 = obj as GridControl;
                TableView tableView = (TableView)gridControl1.View;
                tableView.SearchString = string.Empty;
                if (gridControl1.GroupCount > 0)
                    gridControl1.ClearGrouping();
                gridControl1.ClearSorting();
                gridControl1.FilterString = null;
                gridControl1.SaveLayoutToXml(ActionPlanTaskGridSettingFilePath);
                GeosApplication.Instance.Logger.Log("Method ActionPlanTaskGridControlUnloadedCommandAction executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on ActionPlanTaskGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6453]
        void ActionPlanTaskVisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ActionPlanTaskVisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;

                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ActionPlanTaskGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsActionPlanTaskGridColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method ActionPlanTaskVisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ActionPlanTaskVisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6453]
        void ActionPlanTaskVisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ActionPlanTaskVisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);
                GridColumn column = sender as GridColumn;
                if (((DevExpress.Xpf.Grid.ColumnBase)sender).ActualColumnChooserHeaderCaption.ToString() != "")
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ActionPlanTaskGridSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method ActionPlanTaskVisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ActionPlanTaskVisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //Shubham[skadam] GEOS2-6883 HRM Expenses report not working properly.  24 01 2024
        /// <summary>
        /// Attach the GridControl from the View to the ViewModel
        /// </summary>
        public void SetGridControl(GridControl gridControl, TableView actionPlansTableView)
        {
            _gridControl = gridControl;
            _actionPlansTableView = actionPlansTableView;
        }
        //Shubham[skadam] GEOS2-6883 HRM Expenses report not working properly.  24 01 2024
        // MasterRowExpanded logic moved to ViewModel
        private void OnMasterRowExpanded(RowEventArgs e)
        {
            try
            {
                var detailView = GetDetailView(e.RowHandle);
                if (detailView == null)
                    return;

                detailView.ShowSearchPanelMode = ShowSearchPanelMode.Never;
                BindingOperations.SetBinding(detailView, DataViewBase.SearchStringProperty, new Binding("SearchString") { Source = _actionPlansTableView });
            }
            catch (Exception ex)
            {
                // Handle exceptions if necessary
            }
        }
        //Shubham[skadam] GEOS2-6883 HRM Expenses report not working properly.  24 01 2024
        // Logic to retrieve the detail view
        private TableView GetDetailView(int rowHandle)
        {
            try
            {
                var detail = _gridControl?.GetDetail(rowHandle) as GridControl;
                return detail?.View as TableView;
            }
            catch (Exception ex)
            {
                // Handle exceptions if necessary
                return null;
            }
        }
        //Shubham[skadam] GEOS2-6883 HRM Expenses report not working properly.  24 01 2024
        // SubstituteFilter logic moved to ViewModel
        private void OnSubstituteFilter(DevExpress.Data.SubstituteFilterEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_actionPlansTableView?.SearchString))
                    return;

                e.Filter = new GroupOperator(GroupOperatorType.Or, e.Filter, GetDetailFilter(_actionPlansTableView.SearchString));
                IsExpand = false;
            }
            catch (Exception ex)
            {
                // Handle exceptions if necessary
            }
        }

        List<OperandProperty> operands = new List<OperandProperty>
        { new OperandProperty("TaskNumber"), new OperandProperty("Title"), new OperandProperty("Description"), new OperandProperty("Responsible") };
        // Logic to construct the detail filter
        //Shubham[skadam] GEOS2-6883 HRM Expenses report not working properly.  24 01 2024
        private AggregateOperand GetDetailFilter(string searchString)
        {
            var detailOperator = new GroupOperator(GroupOperatorType.Or);
            try
            {
                foreach (var op in operands)
                {
                    detailOperator.Operands.Add(new FunctionOperator(FunctionOperatorType.Contains, op, new OperandValue(searchString)));
                }

                return new AggregateOperand("TaskList", Aggregate.Exists, detailOperator);
            }
            catch (Exception ex)
            {
                // Handle exceptions if necessary
                return new AggregateOperand("TaskList", Aggregate.Exists, detailOperator);
            }
        }


        #endregion
    }
}

