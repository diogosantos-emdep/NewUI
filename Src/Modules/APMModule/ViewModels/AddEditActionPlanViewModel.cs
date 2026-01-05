using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraRichEdit.Commands;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.APM.CommonClasses;
using Emdep.Geos.Modules.APM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Utility;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Runtime.InteropServices.Expando;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Export.Xl;
using DevExpress.Xpf.Printing;
using System.Data;
using Emdep.Geos.Modules.APM.Interface;
using System.Drawing;
using DevExpress.XtraReports.UI;
using System.Diagnostics;

namespace Emdep.Geos.Modules.APM.ViewModels
{//[Sudhir.Jangra][GEOS2-5977]
    public class AddEditActionPlanViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo, ISupportServices
    {
        #region Services
        IAPMService APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IAPMService APMService = new APMServiceController("localhost:6699");
        #endregion

        #region Events
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
        List<Responsible> responsibleList;
        private string windowHeader;
        private bool isNew;
        private bool isSave;
        private MaximizedElementPosition maximizedElementPosition;
        private double dialogHeight;
        private double dialogWidth;
        private string code;
        private string description;
        private Company selectedLocation;
        // private List<Responsible> responsibleList;
        private List<Responsible> tempResponsibleList;
        private int selectedResponsibleIndex;
        private int selectedOriginIndex;
        private ObservableCollection<TileBarFilters> listOfFilterTile;
        private TileBarFilters selectedTileBarItem;
        private ObservableCollection<APMActionPlanTask> taskList;
        private List<APMActionPlanTask> tempTaskList;
        private APMActionPlanTask selectedTask;
        private string customFilterStringName;
        private string customFilterHTMLColor;
        private string myFilterString;
        private string previousDescription;
        private Company previousSelectedLocation;
        private Responsible previousSelectedResponsible;
        private LookupValue previousSelectedOrigin;
        private Int64 idActionPlan;
        private bool isSaveChanges;
        private bool isEdit;
        private List<GridColumn> GridColumnList;
        private string userSettingsKeyForActionPlan = "APM_AddEditActionPlan_Filter_";
        private int visibleRowCount;
        private TileBarFilters previousSelectedTopTileBarItem;
        public string ActionPlanGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "AddEditActionPlanGridSetting.Xml";
        private bool isActionPlanColumnChooserVisible;
        private bool isToggleButtonExpanded;//[Sudhir.Jangra][GEOS2-5978]
        private string error = string.Empty;
        private int selectedLocationIndex;
        private List<APMActionPlanTask> addEditTaskList;
        private bool isAddButtonEnabled;
        private List<APMActionPlanTask> newTempTaskList;
        private int previousSelectedLocationIndex;//[Sudhir.Jangra][GEOS2-6017]
        private List<LogEntriesByActionPlan> actionPlanChangeLogList;
        // [nsatpute][24-10-2024][GEOS2-6018]
        private ObservableCollection<ActionPlanComment> commentList;
        private List<ActionPlanComment> clonedCommentList;//[Sudhir.Jangra][GEOS2-6020]
        private List<ActionPlanComment> addCommentsList;
        private ActionPlanComment selectedComment;
        private string commentText;
        private DateTime? commentDateTimeText;
        private string commentFullNameText;
        private ObservableCollection<ActionPlanComment> deleteCommentsList;
        private List<ActionPlanComment> updatedCommentList;
        private byte[] UserProfileImageByte = null;
        private bool deleteCommentButtonVisible;

        private ObservableCollection<LookupValue> statusList;
        private List<APMActionPlanTask> oldTaskList;

        private ObservableCollection<AttachmentsByActionPlan> actionPlanAttachmentList;//[Sudhir.Jangra][GEOS2-6019]

        private AttachmentsByActionPlan selectedActionPlanAttachment;//[Sudhir.Jangra][GEOS2-6019]

        private List<AttachmentsByActionPlan> clonedActionPlanAttachmentList;//[Sudhir.Jangra][GEOS2-6019]

        private bool isBusy;

        GroupBoxState attachmentvisiblevisibility;

        public Int32 InactiveSelectedResponsibleToIdEmployee { get; set; }

        private string createdBy;//[Sudhir.Jangra][GEOS2-6595]
        private DateTime? createdIn;//[Sudhir.jangra][GEOS2-6595]

        private int selectedBusinessIndex;//[shweta.thube][GEOS2-6586]
        private bool isBusinessUnitEditable;//[shweta.thube][GEOS2-6587]

        private int selectedDepartmentIndex;//[Sudhir.Jangra][GEOS2-6596]
        private bool isDepartmentEditable;//[Sudhir.jangra][GEOS2-6596]
        private bool isLocationEditable;//[shweta.thube][GEOS2-6587]
        private bool isResponsibleEditable;//[shweta.thube][GEOS2-6587]
        private bool isOriginEditable;//[shweta.thube][GEOS2-6587]
        private Department previousSelectedDepartment;//[shweta.thube][GEOS2-6587]
        public LookupValue previousSelectedBusiness;//[shweta.thube][GEOS2-6587]
        private APMActionPlan addedActionPlan;//[Sudhir.Jangra][GEOS2-6789]
        private string originDescription;//[shweta.thube][GEOS2-6794]
        private string previousOriginDescription;//[shweta.thube][GEOS2-6794]
        private bool isDescriptionEditable;
        private ObservableCollection<APMCustomer> customerList;//[shweta.thube][GEOS2-6911]
        private APMCustomer selectedCustomer;//[shweta.thube][GEOS2-6911]
        private APMCustomer previousSelectedCustomer;//[shweta.thube][GEOS2-6911]
        private List<APMCustomer> tempCustomerList;//[shweta.thube][GEOS2-6911]
        private List<APMCustomer> clonedCustomerList;//[shweta.thube][GEOS2-6911]
        private string searchCustomer;//[shweta.thube][GEOS2-6911]
        private ObservableCollection<APMCustomer> activeUserList;//[shweta.thube][GEOS2-6911]
        private TileBarFilters selectedRegion;//[shweta.thube][GEOS2-6911]
        private List<TileBarFilters> regionList;//[Sudhir.Jangra][GEOS2-6911] 
        private Int32 idSiteCustomer;//[shweta.thube][GEOS2-6912]
        private Int64 idResponsibleLocation;//[shweta.thube][GEOS2-6912]
        private string customerName;//[shweta.thube][GEOS2-6912]
        private APMActionPlanTask previousSelectedActionPlanTask;
        private Int32 idActionPlanResponsible;//[shweta.thube][GEOS2-8066]
        private bool isEmailIcon;   //[shweta.thube][GEOS2-8063]
        private APMActionPlanSubTask selectedActionPlanSubTask;
        public List<APMActionPlanSubTask> addEditSubTaskList; //[pallavi.kale][GEOS2-7003]
        public List<APMActionPlanSubTask> oldSubTaskList;//[pallavi.kale][GEOS2-7003]
        public bool isDeleted; //[shweta.thube][GEOS2-8683]
        private List<APMActionPlanSubTask> newTempSubTaskList; //[shweta.thube][GEOS2-8683]
        private APMActionPlanSubTask previousSelectedActionPlanSubTask;
        private Int64 previousIdParent; //[shweta.thube][GEOS2-7005][08.07.2025]
        public bool isNewActionAction;//[shweta.thube][GEOS2-7218][23.07.2025]
        private APMActionPlan actionPlanInfo;//[shweta.thube][GEOS2-7218][23.07.2025]
        IServiceContainer serviceContainer = null; //[rdixit][GEOS2-9316][26.08.2025]

        #endregion

        #region Properties
        List<Responsible> actionPlanResponsibleList;

        public List<Responsible> ActionPlanResponsibleList
        {
            get { return actionPlanResponsibleList; }
            set
            {
                actionPlanResponsibleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanResponsibleList"));
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

        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
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
        //[GEOS2-6496][27.09.2024][rdixit]
        bool isAddTask;
        public bool IsAddTask
        {
            get { return isAddTask; }
            set
            {
                isAddTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddTask"));
            }
        }

        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElementPosition"));
            }
        }

        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }

        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));

            }
        }

        //public List<Responsible> ResponsibleList
        //{
        //    get { return responsibleList; }
        //    set
        //    {
        //        responsibleList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ResponsibleList"));
        //    }
        //}

        public int SelectedResponsibleIndex
        {
            get { return selectedResponsibleIndex; }
            set
            {
                selectedResponsibleIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedResponsibleIndex"));
            }
        }


        public int SelectedOriginIndex
        {
            get { return selectedOriginIndex; }
            set
            {
                selectedOriginIndex = value;
                //[GEOS2-6496][27.09.2024][rdixit]
                if (selectedOriginIndex >= 0)
                {
                    CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                    Calendar calendar = cultureInfo.Calendar;
                    CalendarWeekRule weekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
                    DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
                    int weekNumber = calendar.GetWeekOfYear(DateTime.Now, weekRule, firstDayOfWeek);
                    OriginDescription = APMCommon.Instance.OriginList[selectedOriginIndex].Abbreviation + DateTime.Now.Year + "CW" + weekNumber;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOriginIndex"));
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

        public TileBarFilters SelectedTileBarItem
        {
            get { return selectedTileBarItem; }
            set
            {
                selectedTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileBarItem"));
            }
        }

        public ObservableCollection<APMActionPlanTask> TaskList
        {
            get { return taskList; }
            set
            {
                taskList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskList"));
                if (TaskList.Count > 0 && TaskList != null)
                {
                    IsEmailIcon = true;
                }
                else
                {
                    IsEmailIcon = false;
                }
            }
        }

        public APMActionPlanTask SelectedTask
        {
            get { return selectedTask; }
            set
            {


                if (PreviousSelectedActionPlanTask != null)
                    PreviousSelectedActionPlanTask.IsSelectedRow = false;

                selectedTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTask"));
                //[shweta.thube][GEOS2 - 7219]
                if (SelectedTask != null && SelectedTask.IsShowIcon)
                {
                    SelectedTask.IsSelectedRow = true;
                    PreviousSelectedActionPlanTask = selectedTask;
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
        public string CustomFilterStringName
        {
            get { return customFilterStringName; }
            set
            {
                customFilterStringName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomFilterStringName"));
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

        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }

        public List<APMActionPlanTask> TempTaskList
        {
            get { return tempTaskList; }
            set
            {
                tempTaskList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempTaskList"));
            }
        }

        public List<Responsible> TempResponsibleList
        {
            get { return tempResponsibleList; }
            set
            {
                tempResponsibleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempResponsibleList"));
            }
        }

        public string PreviousDescription
        {
            get { return previousDescription; }
            set
            {
                previousDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousDescription"));
            }
        }

        public Company PreviousSelectedLocation
        {
            get { return previousSelectedLocation; }
            set
            {
                previousSelectedLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedLocation"));
            }
        }

        public Responsible PreviousSelectedResponsible
        {
            get { return previousSelectedResponsible; }
            set
            {
                previousSelectedResponsible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedResponsible"));
            }
        }

        public LookupValue PreviousSelectedOrigin
        {
            get { return previousSelectedOrigin; }
            set
            {
                previousSelectedOrigin = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedOrigin"));
            }
        }

        public Int64 IdActionPlan
        {
            get { return idActionPlan; }
            set
            {
                idActionPlan = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdActionPlan"));
            }
        }

        public bool IsSaveChanges
        {
            get { return isSaveChanges; }
            set
            {
                isSaveChanges = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaveChanges"));
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

        public int VisibleRowCount
        {
            get { return visibleRowCount; }
            set
            {
                visibleRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleRowCount"));
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

        public bool IsActionPlanColumnChooserVisible
        {
            get { return isActionPlanColumnChooserVisible; }
            set
            {
                isActionPlanColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActionPlanColumnChooserVisible"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5978]
        public bool IsToggleButtonExpanded
        {
            get { return isToggleButtonExpanded; }
            set
            {
                isToggleButtonExpanded = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsToggleButtonExpanded"));
            }
        }

        public int SelectedLocationIndex
        {
            get { return selectedLocationIndex; }
            set
            {
                selectedLocationIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocationIndex"));
            }
        }
        public List<APMActionPlanTask> AddEditTaskList
        {
            get { return addEditTaskList; }
            set
            {
                addEditTaskList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddEditTaskList"));
            }
        }

        public bool IsAddButtonEnabled
        {
            get { return isAddButtonEnabled; }
            set
            {
                isAddButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddButtonEnabled"));
            }
        }
        public List<APMActionPlanTask> NewTempTaskList
        {
            get { return newTempTaskList; }
            set
            {
                newTempTaskList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewTempTaskList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-6017]
        public int PreviousSelectedLocationIndex
        {
            get { return previousSelectedLocationIndex; }
            set
            {
                previousSelectedLocationIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedLocationIndex"));
            }
        }
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        //[shweta.thube][GEOS2-6020]
        public List<LogEntriesByActionPlan> ActionPlanChangeLogList
        {
            get { return actionPlanChangeLogList; }
            set
            {
                actionPlanChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanChangeLogList"));
            }
        }
        // [nsatpute][24-10-2024][GEOS2-6018]
        public ObservableCollection<ActionPlanComment> CommentsList
        {
            get { return commentList; }
            set
            {
                commentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentsList"));

                if (CommentsList != null && CommentsList.Count > 0)
                {
                    if (GeosApplication.Instance.IsAPMActionPlanPermission)
                    {
                        CommentsList.ToList().ForEach(x => x.IsDeleteButtonEnabled = true);
                    }
                    else
                    {
                        foreach (ActionPlanComment item in CommentsList)
                        {
                            if (GeosApplication.Instance.ActiveUser.IdUser == item.CreatedBy)
                            {
                                item.IsDeleteButtonEnabled = true;
                            }
                            else
                            {
                                item.IsDeleteButtonEnabled = false;
                            }
                        }
                    }

                }

            }
        }

        //[Sudhir.Jangra][GEOS2-6020]
        public List<ActionPlanComment> ClonedCommentList
        {
            get { return clonedCommentList; }
            set
            {
                clonedCommentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedCommentList"));
            }
        }

        public List<ActionPlanComment> AddCommentsList
        {
            get { return addCommentsList; }
            set
            {
                addCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddCommentsList"));
            }
        }
        public ActionPlanComment SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }
        public string CommentText
        {
            get { return commentText; }
            set
            {
                commentText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentText"));
            }
        }
        public DateTime? CommentDateTimeText
        {
            get { return commentDateTimeText; }
            set
            {
                commentDateTimeText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentDateTimeText"));
            }
        }
        public string CommentFullNameText
        {
            get { return commentFullNameText; }
            set
            {
                commentFullNameText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentFullNameText"));
            }
        }

        public ObservableCollection<ActionPlanComment> DeleteCommentsList
        {
            get { return deleteCommentsList; }
            set
            {
                deleteCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeleteCommentsList"));
            }
        }

        public List<ActionPlanComment> UpdatedCommentsList
        {
            get { return updatedCommentList; }
            set
            {
                updatedCommentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedCommentsList"));
            }
        }
        public bool DeleteCommentButtonVisible
        {
            get { return deleteCommentButtonVisible; }
            set
            {
                deleteCommentButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeleteCommentButtonVisible"));
            }
        }
        //[shweta.thube][GEOS2-6020]
        public ObservableCollection<LookupValue> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }
        //[Shweta.thube][GEOS2-6020]
        public List<APMActionPlanTask> OldTaskList
        {
            get { return oldTaskList; }
            set
            {
                oldTaskList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldTaskList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-6019]

        public ObservableCollection<AttachmentsByActionPlan> ActionPlanAttachmentList
        {
            get { return actionPlanAttachmentList; }
            set
            {
                actionPlanAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanAttachmentList"));

                if (ActionPlanAttachmentList != null && ActionPlanAttachmentList.Count > 0)
                {
                    if (GeosApplication.Instance.IsAPMActionPlanPermission)
                    {
                        ActionPlanAttachmentList.ToList().ForEach(x => x.IsDeleteButtonEnabled = true);
                    }
                    else
                    {
                        foreach (AttachmentsByActionPlan item in ActionPlanAttachmentList)
                        {
                            if (GeosApplication.Instance.ActiveUser.IdUser == item.CreatedBy)
                            {
                                item.IsDeleteButtonEnabled = true;
                            }
                            else
                            {
                                item.IsDeleteButtonEnabled = false;
                            }
                        }
                    }
                }
            }
        }

        //[Sudhir.Jangra][GEOS2-6019]
        public AttachmentsByActionPlan SelectedActionPlanAttachment
        {
            get { return selectedActionPlanAttachment; }
            set
            {
                selectedActionPlanAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActionPlanAttachment"));
            }
        }

        //[Sudhir.Jangra][GEOS2-6019]
        public List<AttachmentsByActionPlan> ClonedActionPlanAttachmentList
        {
            get { return clonedActionPlanAttachmentList; }
            set
            {
                clonedActionPlanAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedActionPlanAttachmentList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-6019]
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public GroupBoxState Attachmentvisiblevisibility
        {
            get { return attachmentvisiblevisibility; }
            set
            {
                attachmentvisiblevisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Attachmentvisiblevisibility"));
            }
        }

        public Company SelectedLocation
        {
            get { return selectedLocation; }
            set
            {
                selectedLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocation"));
            }
        }

        //[Sudhir.jangra][GEOS2-6595]
        public string CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreatedBy"));
            }
        }
        //[Sudhir.Jangra][GEOS2-6595]
        public DateTime? CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreatedIn"));
            }
        }
        //[shweta.thube][GEOS2-6586]
        public int SelectedBusinessIndex
        {
            get { return selectedBusinessIndex; }
            set
            {
                selectedBusinessIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBusinessIndex"));
            }
        }
        //[shweta.thube][GEOS2-6587]
        public bool IsBusinessUnitEditable
        {
            get { return isBusinessUnitEditable; }
            set
            {
                isBusinessUnitEditable = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IsBusinessUnitEditable"));
            }
        }

        //[Sudhir.jangra][GEOS2-6596]
        public int SelectedDepartmentIndex
        {
            get { return selectedDepartmentIndex; }
            set
            {
                selectedDepartmentIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDepartmentIndex"));
            }
        }

        //[Sudhir.Jangra][GEOS2-6596]
        public bool IsDepartmentEditable
        {
            get { return isDepartmentEditable; }
            set
            {
                isDepartmentEditable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDepartmentEditable"));
            }
        }
        //[shweta.thube][GEOS2-6587]
        public bool IsLocationEditable
        {
            get { return isLocationEditable; }
            set
            {
                isLocationEditable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLocationEditable"));
            }
        }
        //[shweta.thube][GEOS2-6587]
        public bool IsResponsibleEditable
        {
            get { return isResponsibleEditable; }
            set
            {
                isResponsibleEditable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsResponsibleEditable"));
            }
        }
        //[shweta.thube][GEOS2-6587]
        public bool IsOriginEditable
        {
            get { return isOriginEditable; }
            set
            {
                isOriginEditable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOriginEditable"));
            }
        }

        public Department PreviousSelectedDepartment
        {
            get { return previousSelectedDepartment; }
            set
            {
                previousSelectedDepartment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedDepartment"));
            }
        }

        public LookupValue PreviousSelectedBusiness
        {
            get { return previousSelectedBusiness; }
            set
            {
                previousSelectedBusiness = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedBusiness"));
            }
        }

        //[Sudhir.Jangra][GEOS2-6789]
        public APMActionPlan AddedActionPlan
        {
            get { return addedActionPlan; }
            set
            {
                addedActionPlan = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddedActionPlan"));
            }
        }
        //[shweta.thube][GEOS2-6794]
        public string OriginDescription
        {
            get { return originDescription; }
            set
            {
                originDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginDescription"));
            }
        }
        //[shweta.thube][GEOS2-6794]
        public string PreviousOriginDescription
        {
            get { return previousOriginDescription; }
            set
            {
                previousOriginDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousOriginDescription"));
            }
        }
        //[shweta.thube][GEOS2-6794]
        public bool IsDescriptionEditable
        {
            get { return isDescriptionEditable; }
            set
            {
                isDescriptionEditable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDescriptionEditable"));
            }
        }
        //[shweta.thube][GEOS2-6911]
        public ObservableCollection<APMCustomer> CustomerList
        {
            get { return customerList; }
            set
            {
                customerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerList"));
            }
        }
        //[shweta.thube][GEOS2-6911]
        public APMCustomer SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomer"));
            }
        }
        //[shweta.thube][GEOS2-6911]
        public APMCustomer PreviousSelectedCustomer
        {
            get { return previousSelectedCustomer; }
            set
            {
                previousSelectedCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedCustomer"));
            }
        }
        //[shweta.thube][GEOS2-6911]
        public List<APMCustomer> TempCustomerList
        {
            get { return tempCustomerList; }
            set
            {
                tempCustomerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempCustomerList"));
            }
        }
        //[shweta.thube][GEOS2-6911]
        public List<APMCustomer> ClonedCustomerList
        {
            get { return clonedCustomerList; }
            set
            {
                clonedCustomerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedCustomerList"));
            }
        }

        public string SearchCustomer
        {
            get { return searchCustomer; }
            set
            {
                searchCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SearchCustomer"));
            }
        }
        //[shweta.thube][GEOS2-6911]
        public ObservableCollection<APMCustomer> ActiveUserList
        {
            get { return activeUserList; }
            set
            {
                activeUserList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActiveUserList"));
            }
        }

        public TileBarFilters SelectedRegion
        {
            get { return selectedRegion; }
            set
            {
                selectedRegion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRegion"));
            }
        }
        //[shweta.thube][GEOS2-6911]
        public List<TileBarFilters> RegionList
        {
            get { return regionList; }
            set
            {
                regionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegionList"));
            }
        }
        //[shweta.thube][GEOS2-6912]
        public Int32 IdSiteCustomer
        {
            get { return idSiteCustomer; }
            set
            {
                idSiteCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdSiteCustomer"));
            }
        }
        //[shweta.thube][GEOS2-6912]  
        public Int64 IdResponsibleLocation
        {
            get { return idResponsibleLocation; }
            set
            {
                idResponsibleLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdResponsibleLocation"));
            }
        }

        //[shweta.thube][GEOS2-6912]  
        public string CustomerName
        {
            get { return customerName; }
            set
            {
                customerName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerName"));
            }
        }
        //[shweta.thube][GEOS2-8066]
        public Int32 IdActionPlanResponsible
        {
            get { return idActionPlanResponsible; }
            set
            {
                idActionPlanResponsible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdActionPlanResponsible"));
            }
        }
        //[shweta.thube][GEOS2-8063]
        public bool IsEmailIcon
        {
            get { return isEmailIcon; }
            set
            {
                isEmailIcon = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmailIcon"));
            }
        }

        public APMActionPlanSubTask SelectedActionPlanSubTask
        {
            get { return selectedActionPlanSubTask; }
            set
            {
                if (PreviousSelectedActionPlanSubTask != null)
                    PreviousSelectedActionPlanSubTask.IsSelectedRow = false;

                selectedActionPlanSubTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActionPlanSubTask"));
                //[shweta.thube][GEOS2 - 7219]
                if (SelectedActionPlanSubTask != null && SelectedActionPlanSubTask.IsShowIcon)
                {
                    SelectedActionPlanSubTask.IsSelectedRow = true;
                    PreviousSelectedActionPlanSubTask = SelectedActionPlanSubTask;
                }
            }
        }
        public APMActionPlanSubTask PreviousSelectedActionPlanSubTask
        {
            get { return previousSelectedActionPlanSubTask; }
            set
            {
                previousSelectedActionPlanSubTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedActionPlanSubTask"));

            }
        }
        //[pallavi.kale][GEOS2-7003]
        public List<APMActionPlanSubTask> AddEditSubTaskList
        {
            get { return addEditSubTaskList; }
            set
            {
                addEditSubTaskList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddEditSubTaskList"));
            }
        }
        //[pallavi.kale][GEOS2-7003]
        public List<APMActionPlanSubTask> OldSubTaskList
        {
            get { return oldSubTaskList; }
            set
            {
                oldSubTaskList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldSubTaskList"));
            }
        }
        //[shweta.thube][GEOS2-8683]
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
        //[shweta.thube][GEOS2-8683]
        public List<APMActionPlanSubTask> NewTempSubTaskList
        {
            get { return newTempSubTaskList; }
            set
            {
                newTempSubTaskList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewTempSubTaskList"));
            }
        }
        //[shweta.thube][GEOS2-7005][08.07.2025]
        public Int64 PreviousIdParent
        {
            get { return previousIdParent; }
            set
            {
                previousIdParent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousIdParent"));
            }
        }
        //[shweta.thube][GEOS2-7218][23.07.2025]
        public bool IsNewActionAction
        {
            get
            {
                return isNewActionAction;
            }

            set
            {
                isNewActionAction = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNewActionAction"));
            }
        }
        //[shweta.thube][GEOS2-7218][23.07.2025]
        public APMActionPlan ActionPlanInfo
        {
            get { return actionPlanInfo; }
            set
            {
                actionPlanInfo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanInfo"));
            }
        }
        //[rdixit][GEOS2-9316][26.08.2025]
        protected IServiceContainer ServiceContainer
        {
            get
            {
                if (serviceContainer == null)
                    serviceContainer = new ServiceContainer(this);
                return serviceContainer;
            }
        }
        //[rdixit][GEOS2-9316][26.08.2025]
        IServiceContainer ISupportServices.ServiceContainer { get { return ServiceContainer; } }
        //[rdixit][GEOS2-9316][26.08.2025]
        IGridExportService GridExportService { get { return ServiceContainer.GetService<IGridExportService>(); } }
        #endregion

        #region ICommands
        public ICommand CancelButtonActionCommand { get; set; }
        public ICommand LeftCommandFilterTileClick { get; set; }

        public ICommand AcceptButtonActionCommand { get; set; }

        public ICommand FilterEditorCreatedCommand { get; set; }

        public ICommand CommandTileBarDoubleClick { get; set; }

        public ICommand AddActionPlanTaskCommand { get; set; }
        public ICommand EditTaskHyperLinkCommand { get; set; }

        public ICommand DelegatedToCommand { get; set; }//[Sudhir.Jangra][GEOS2-6017]
        public ICommand DeleteTaskCommand { get; set; }//[shweta.thube][GEOS2-5981]
        public ICommand CommentsHyperLinkCommand { get; set; }//[Sudhir.Jangra][GEOS2-6015]
        public ICommand ExportToExcelCommand { get; set; }//[shweta.thube][GEOS2-6020]
                                                          // [nsatpute][24-10-2024][GEOS2-6018]
        public ICommand AddCommentsCommand { get; set; }
        public ICommand DeleteCommentRowCommand { get; set; }
        public ICommand CommentsGridDoubleClickCommand { get; set; }

        public ICommand AttachmentsHyperClickCommand { get; set; }

        public ICommand AddAttachmentFileCommand { get; set; }//[Sudhir.Jangra][GEOS2-6019]
        public ICommand EditActionPlanFileCommand { get; set; }//[Sudhir.Jangra][GEOS2-6019]
        public ICommand DownloadFileCommand { get; set; }//[Sudhir.Jangra][GEOS2-6019]
        public ICommand DeleteFileCommand { get; set; }//[Sudhir.Jangra][GEOS2-6019]
        public ICommand LocationListClosedCommand { get; set; }//[Shweta.thube][GEOS2-6591]
        public ICommand SelectedRegionCommand { get; set; }//[Shweta.thube][GEOS2-6591]
        public ICommand ParticipantsHyperLinkClickCommand { get; set; }
        public ICommand SendNotificationCommand { get; set; }  //[shweta.thube][GEOS2-8063][27/05/2025]
        public ICommand SubTaskCommentsHyperLinkCommand { get; set; }
        public ICommand SubTaskAttachmentsHyperClickCommand { get; set; }
        public ICommand SubTaskParticipantsHyperLinkClickCommand { get; set; }
        public ICommand AddActionPlanSubTaskCommand { get; set; }//[pallavi.kale][GEOS2-7003]
        public ICommand EditSubTaskHyperLinkCommand { get; set; }//[pallavi.kale][GEOS2-7003]
        public ICommand DeleteSubTaskCommand { get; set; } //[shweta.thube][GEOS2-8683]
        public ICommand PrintButtonCommand { get; set; }//[pallavi.kale][GEOS2-8084][29.08.2025]
        public ICommand ExportButtonCommand { get; set; }//[pallavi.kale][GEOS2-8084][29.08.2025]
        public ICommand ExportCustomerButtonCommand { get; set; }//[pallavi.kale][GEOS2-8084][29.08.2025]
        public ICommand PrintCustomerButtonCommand { get; set; }//[pallavi.kale][GEOS2-8084][29.08.2025]

        #endregion

        #region Constructor
        public AddEditActionPlanViewModel()
        {
            try
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
                MaximizedElementPosition = MaximizedElementPosition.Top;
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 90;
                CancelButtonActionCommand = new RelayCommand(new Action<object>(CancelButtonActionCommandAction));
                LeftCommandFilterTileClick = new RelayCommand(new Action<object>(LeftCommandFilterTileClickAction));
                AcceptButtonActionCommand = new RelayCommand(new Action<object>(AcceptButtonActionCommandAction));
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);
                CommandTileBarDoubleClick = new RelayCommand(new Action<object>(CommandTileBarDoubleClickAction));
                AddActionPlanTaskCommand = new RelayCommand(new Action<object>(AddActionPlanTaskViewWindowShow));//[Sudhir.Jangra][GEOS2-5979]
                EditTaskHyperLinkCommand = new RelayCommand(new Action<object>(EditTaskHyperLinkCommandAction));//[Sudhir.Jangra][GEOS2-5980]
                                                                                                                // DelegatedToCommand = new RelayCommand(new Action<object>(DelegatedToCommandAction));
                DeleteTaskCommand = new RelayCommand(new Action<object>(DeleteTaskCommandAction));//[shweta.thube][GEOS2-5981]
                CommentsHyperLinkCommand = new RelayCommand(new Action<object>(CommentsHyperLinkCommandAction));
                ExportToExcelCommand = new DelegateCommand<object>(ExportToExcel);//[shweta.thube][GEOS2-6020]
                                                                                  // [nsatpute][24-10-2024][GEOS2-6018]
                AddCommentsCommand = new RelayCommand(new Action<object>(AddCommentsCommandAction));
                DeleteCommentRowCommand = new RelayCommand(new Action<object>(DeleteCommentRowCommandAction));
                CommentsGridDoubleClickCommand = new RelayCommand(new Action<object>(CommentDoubleClickCommandAction));

                AttachmentsHyperClickCommand = new RelayCommand(new Action<object>(AttachmentsHyperClickCommandAction));//[Sudhir.Jangra][GEOS2-6016]

                AddAttachmentFileCommand = new RelayCommand(new Action<object>(AddAttachmentFileCommandAction));
                //[Sudhir.Jangra][GEOS2-6019]
                EditActionPlanFileCommand = new RelayCommand(new Action<object>(EditActionPlanFileCommandAction));
                DownloadFileCommand = new RelayCommand(new Action<object>(DownloadFileCommandAction));//[Sudhir.Jangra][GEOS2-6019]
                DeleteFileCommand = new RelayCommand(new Action<object>(DeleteAttachmentRowCommandAction));//[Sudhir.Jangra][GEOS2-6019]
                LocationListClosedCommand = new RelayCommand(new Action<object>(LocationListClosedCommandAction));
                SelectedRegionCommand = new RelayCommand(new Action<object>(SelectedRegionCommandAction));//[Shweta.thube][GEOS2-6591]
                ParticipantsHyperLinkClickCommand = new RelayCommand(new Action<object>(ParticipantsHyperLinkClickCommandAction));//[shweta.thube][GEOS2-7008]
                SendNotificationCommand = new RelayCommand(new Action<object>(SendNotificationCommandAction));  //[shweta.thube][GEOS2-8063][27/05/2025]
                SubTaskCommentsHyperLinkCommand = new RelayCommand(new Action<object>(SubTaskCommentsHyperLinkCommandAction));
                SubTaskAttachmentsHyperClickCommand = new RelayCommand(new Action<object>(SubTaskAttachmentsHyperClickCommandAction));
                SubTaskParticipantsHyperLinkClickCommand = new RelayCommand(new Action<object>(SubTaskParticipantsHyperLinkClickCommandAction));
                AddActionPlanSubTaskCommand = new RelayCommand(new Action<object>(AddActionPlanSubTaskViewWindowShow));//[pallavi.kale][GEOS2-7003]
                EditSubTaskHyperLinkCommand = new RelayCommand(new Action<object>(EditSubTaskHyperLinkCommandAction));//[pallavi.kale][GEOS2-7003]
                DeleteSubTaskCommand = new RelayCommand(new Action<object>(DeleteSubTaskCommandAction)); //[shweta.thube][GEOS2-8683]
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintButtonCommandAction));//[pallavi.kale][GEOS2-8084][29.08.2025]
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportButtonCommandAction));//[pallavi.kale][GEOS2-8084][29.08.2025]
                ExportCustomerButtonCommand = new RelayCommand(new Action<object>(ExportCustomerButtonCommandAction));//[pallavi.kale][GEOS2-8084][29.08.2025]
                PrintCustomerButtonCommand = new RelayCommand(new Action<object>(PrintCustomerButtonCommandAction));//[pallavi.kale][GEOS2-8084][29.08.2025]
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AddEditActionPlanViewModel...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()..."), category: Category.Info, priority: Priority.Low);

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
                IsNewActionAction = true;
                //APMService = new APMServiceController("localhost:6699");
                Code = APMService.GetActionPlanLatestCode_V2560();
                IsAddTask = false;
                if (IsNew)
                {
                    SelectedLocationIndex = -1;
                }

                #region [Sudhir.Jangra] [GEOS2-6833]
                // FillResponsibleList();
                APMCommon.Instance.ResponsibleList = new List<Responsible>();

                //List<int> idCompanies = APMCommon.Instance.LocationList.Select(x => x.IdCompany).ToList();

                // Convert the list of integers into a comma-separated string
                // string idCompanyString = string.Join(",", idCompanies);
                //FillResponsibleList(idCompanyString);//[shweta.thube][GEOS2-6591]
                #endregion


                if (IsNew)
                {
                    SelectedResponsibleIndex = -1;
                    SelectedOriginIndex = -1;
                    SelectedBusinessIndex = -1;
                    SelectedDepartmentIndex = -1;

                    //[shweta.thube][GEOS2-6587]
                    IsDepartmentEditable = true;
                    IsLocationEditable = true;
                    IsOriginEditable = true;
                    IsResponsibleEditable = true;
                    IsBusinessUnitEditable = true;
                    IsDescriptionEditable = true;
                }




                TaskList = new ObservableCollection<APMActionPlanTask>();
                TempTaskList = new List<APMActionPlanTask>();
                IsToggleButtonExpanded = false;
                ActionPlanAttachmentList = new ObservableCollection<AttachmentsByActionPlan>();

                if (ActionPlanAttachmentList.Count > 0)
                {
                    Attachmentvisiblevisibility = GroupBoxState.Maximized;
                }
                else
                {
                    Attachmentvisiblevisibility = GroupBoxState.Minimized;
                }
                SetUserPermission();//[Sudhir.Jangra][GEOS2-6697]

                FillCustomerList();//[shweta.thube][GEOS2-6911]
                FillRegionList();
                SelectedRegion = RegionList.FirstOrDefault(x => x.Caption == ActiveUserList.FirstOrDefault().Region);

                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                    GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed successfully"), category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void EditInit(APMActionPlan selectedActionPlan)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit()..."), category: Category.Info, priority: Priority.Low);

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
                IdActionPlan = selectedActionPlan.IdActionPlan;
                IsAddTask = true;
                Code = selectedActionPlan.Code;
                CreatedBy = selectedActionPlan.CreatedByName;
                CreatedIn = selectedActionPlan.CreatedIn;

                SelectedLocationIndex = APMCommon.Instance.LocationList.FindIndex(x => x.IdCompany == selectedActionPlan.IdCompany);
                PreviousSelectedLocationIndex = SelectedLocationIndex;
                PreviousSelectedLocation = APMCommon.Instance.LocationList.FirstOrDefault(x => x.IdCompany == selectedActionPlan.IdCompany);
                //FillResponsibleList();
                SelectedLocation = APMCommon.Instance.LocationList.FirstOrDefault(x => x.IdCompany == selectedActionPlan.IdCompany);
                InactiveSelectedResponsibleToIdEmployee = selectedActionPlan.IdEmployee;
                string idCompanyString = Convert.ToString(SelectedLocation.IdCompany);

                FillResponsibleList(idCompanyString);

                APMCommon.Instance.ResponsibleList.RemoveAll(x => x.IdEmployee == selectedActionPlan.IdEmployee && x.IsInActive == true);
                if (APMCommon.Instance.ResponsibleList.Any(x => x.IdEmployee == selectedActionPlan.IdEmployee))
                {
                    if (APMCommon.Instance.ResponsibleList.Any(x => x.IsInActive == true))
                    {
                        List<Responsible> isInactiveResponsible = APMCommon.Instance.ResponsibleList.Where(x => x.IsInActive == true).ToList();
                        foreach (var item in isInactiveResponsible)
                        {
                            APMCommon.Instance.ResponsibleList.Remove(item);
                        }
                    }
                    SelectedResponsibleIndex = APMCommon.Instance.ResponsibleList.FindIndex(x => x.IdEmployee == selectedActionPlan.IdEmployee);
                    PreviousSelectedResponsible = APMCommon.Instance.ResponsibleList.FirstOrDefault(x => x.IdEmployee == selectedActionPlan.IdEmployee);
                }
                else
                {
                    //APMService = new APMServiceController("localhost:6699");
                    Responsible temp = APMService.GetInactiveResponsibleForActionPlan_V2560(selectedActionPlan.IdEmployee);
                    temp.IsInActive = true;
                    temp.EmployeeCodeWithIdGender = temp.EmployeeCode + "_" + temp.IdGender;
                    if (ActionPlanResponsibleList != null)
                    {
                        ActionPlanResponsibleList.Add(temp);
                    }
                    APMCommon.Instance.ResponsibleList.Add(temp);

                    SelectedResponsibleIndex = APMCommon.Instance.ResponsibleList.FindIndex(x => x.IdEmployee == selectedActionPlan.IdEmployee);
                    PreviousSelectedResponsible = APMCommon.Instance.ResponsibleList.FirstOrDefault(x => x.IdEmployee == selectedActionPlan.IdEmployee); ;
                }
                SelectedOriginIndex = APMCommon.Instance.OriginList.FindIndex(x => x.IdLookupValue == selectedActionPlan.IdLookupOrigin);
                PreviousSelectedOrigin = APMCommon.Instance.OriginList.FirstOrDefault(x => x.IdLookupValue == selectedActionPlan.IdLookupOrigin);

                SelectedBusinessIndex = APMCommon.Instance.BusinessUnitList.FindIndex(x => x.IdLookupValue == selectedActionPlan.IdLookupBusinessUnit);//[shweta.thube][GEOS2-6586]
                PreviousSelectedBusiness = APMCommon.Instance.BusinessUnitList.FirstOrDefault(x => x.IdLookupValue == selectedActionPlan.IdLookupBusinessUnit);//[shweta.thube][GEOS2-6587]

                SelectedDepartmentIndex = APMCommon.Instance.DepartmentList.FindIndex(x => x.IdDepartment == selectedActionPlan.IdDepartment);//[Sudhir.Jangra][GEOS2-6596]
                PreviousSelectedDepartment = APMCommon.Instance.DepartmentList.FirstOrDefault(x => x.IdDepartment == selectedActionPlan.IdDepartment);//[shweta.thube][GEOS2-6587]

                Description = selectedActionPlan.Description;
                PreviousDescription = selectedActionPlan.Description;

                OriginDescription = selectedActionPlan.OriginDescription;//[shweta.thube][GEOS2-6794]
                PreviousOriginDescription = selectedActionPlan.OriginDescription;//[shweta.thube][GEOS2-6794]
                FillCustomerList();
                FillRegionList();
                SelectedCustomer = ClonedCustomerList.FirstOrDefault(x => x.IdSite == selectedActionPlan.IdSite);
                if (SelectedCustomer != null)
                {
                    if (SelectedCustomer.Region == ActiveUserList.FirstOrDefault().Region)
                    {
                        SelectedRegion = RegionList.FirstOrDefault(x => x.Caption == ActiveUserList.FirstOrDefault().Region);
                    }
                    else
                    {
                        SelectedRegion = RegionList.FirstOrDefault(x => x.Caption == SelectedCustomer.Region);
                        CustomerList = new ObservableCollection<APMCustomer>(ClonedCustomerList.Where(c => c.Region == SelectedCustomer.Region));

                    }
                    PreviousSelectedCustomer = ClonedCustomerList.FirstOrDefault(x => x.IdSite == selectedActionPlan.IdSite);//[shweta.thube][GEOS2-6911]
                }
                else
                {
                    SelectedRegion = RegionList.FirstOrDefault(x => x.Caption == ActiveUserList.FirstOrDefault().Region);
                }
                //TaskList = new ObservableCollection<APMActionPlanTask>(selectedActionPlan.TaskList);
                FillTaskList();//[Sudhir.Jangra][GEOS2-6016]


                if (TempTaskList == null)
                {
                    TempTaskList = new List<APMActionPlanTask>();
                }
                //[shweta.thube][GEOS2-7218][23.07.2025]
                if (TaskList != null)
                {
                    foreach (var item in TaskList)
                    {
                        bool alreadyExists = TempTaskList.Any(x => x.IdActionPlanTask == item.IdActionPlanTask);
                        if (!alreadyExists)
                        {
                            item.Code = Code;
                            TempTaskList.Add((APMActionPlanTask)item.Clone());
                        }
                    }
                }

                // TempTaskList = new List<APMActionPlanTask>(selectedActionPlan.TaskList);
                IsToggleButtonExpanded = true;
                if (TaskList != null && TaskList.Count > 0)
                {
                    FillLeftTileList();
                    AddCustomSetting();
                    // FillComentsList();

                    //[shweta.thube][GEOS2-6587]
                    //[GEOS2-6882][rdixit][23.01.2025] Commented the desable properties
                    //IsDepartmentEditable = false;
                    //IsLocationEditable = false;
                    IsOriginEditable = false;
                    //IsResponsibleEditable = false;
                    //IsBusinessUnitEditable = false;
                    //IsDescriptionEditable = false;
                    //IsAddButtonEnabled = false;//[Sudhir.Jangra][GEOS2-6697]

                }
                else
                {
                    IsDepartmentEditable = true;
                    IsLocationEditable = true;
                    IsOriginEditable = true;
                    IsResponsibleEditable = true;
                    IsBusinessUnitEditable = true;
                    IsDescriptionEditable = true;
                    //IsAddButtonEnabled = true;//[Sudhir.Jangra][GEOS2-6697]
                }
                FillComentsList();


                if (ClonedCommentList == null)
                {
                    ClonedCommentList = new List<ActionPlanComment>();
                }

                if (CommentsList != null && CommentsList.Count > 0)
                {
                    foreach (var item in CommentsList)
                    {
                        ClonedCommentList.Add((ActionPlanComment)item.Clone());
                    }
                }
                //APMService = new APMServiceController("localhost:6699");
                List<LogEntriesByActionPlan> temp1 = APMService.GetLogEntriesByActionPlan_V2580((UInt32)IdActionPlan);//[shweta.thube][GEOS2-6020]
                ActionPlanChangeLogList = new List<LogEntriesByActionPlan>(temp1.OrderByDescending(x => x.Datetime));//[shweta.thube][GEOS2-6020]

                FillStatusList();//[shweta.thube][GEOS2-6020]

                //[Sudhir.Jangra][GEOS2-6019]
                FillActionPlanAttachmentList(selectedActionPlan.IdActionPlan);

                if (ActionPlanAttachmentList.Count > 0)
                {
                    Attachmentvisiblevisibility = GroupBoxState.Maximized;
                }
                else
                {
                    Attachmentvisiblevisibility = GroupBoxState.Minimized;
                }

                SetUserPermission();//[Sudhir.Jangra][GEOS2-6697]
                IdSiteCustomer = selectedActionPlan.IdSite;//[shweta.thube][GEOS2-6912]
                IdResponsibleLocation = selectedActionPlan.IdCompany;//[shweta.thube][GEOS2-6912]
                CustomerName = selectedActionPlan.GroupName;//[shweta.thube][GEOS2-6912]
                IdActionPlanResponsible = selectedActionPlan.IdEmployee;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CancelButtonActionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelButtonActionCommandAction()..."), category: Category.Info, priority: Priority.Low);
                if (!IsNew)
                {
                    APMActionPlan updated = new APMActionPlan();

                    updated.IdActionPlan = IdActionPlan;

                    if (Description != PreviousDescription)
                    {
                        updated.Description = Description;
                        IsSaveChanges = true;
                    }
                    else
                    {
                        updated.Description = Description;
                    }

                    if (APMCommon.Instance.LocationList[SelectedLocationIndex].IdCompany != PreviousSelectedLocation.IdCompany)
                    {
                        updated.IdCompany = APMCommon.Instance.LocationList[SelectedLocationIndex].IdCompany;
                        IsSaveChanges = true;
                    }
                    else
                    {
                        updated.IdCompany = APMCommon.Instance.LocationList[SelectedLocationIndex].IdCompany;
                    }
                    if (SelectedResponsibleIndex != -1)
                    {
                        if (ActionPlanResponsibleList != null && PreviousSelectedResponsible != null)  //[GEOS2-6021][rdixit][17.02.2025]
                        {
                            if (ActionPlanResponsibleList[SelectedResponsibleIndex].IdEmployee != PreviousSelectedResponsible.IdEmployee)
                            {
                                updated.IdEmployee = (Int32)ActionPlanResponsibleList[SelectedResponsibleIndex].IdEmployee;
                                IsSaveChanges = true;
                            }
                            else
                            {
                                updated.IdEmployee = (Int32)ActionPlanResponsibleList[SelectedResponsibleIndex].IdEmployee;
                            }
                        }
                    }
                    if (APMCommon.Instance.OriginList[SelectedOriginIndex].IdLookupValue != PreviousSelectedOrigin.IdLookupValue)
                    {
                        updated.IdLookupOrigin = APMCommon.Instance.OriginList[SelectedOriginIndex].IdLookupValue;
                        IsSaveChanges = true;
                    }
                    else
                    {
                        updated.IdLookupOrigin = APMCommon.Instance.OriginList[SelectedOriginIndex].IdLookupValue;
                    }
                    if (SelectedBusinessIndex != -1 && PreviousSelectedBusiness != null)
                    {
                        if (APMCommon.Instance.BusinessUnitList[SelectedBusinessIndex].IdLookupValue != PreviousSelectedBusiness.IdLookupValue)
                        {
                            updated.IdLookupBusinessUnit = APMCommon.Instance.BusinessUnitList[SelectedBusinessIndex].IdLookupValue;
                            IsSaveChanges = true;
                        }
                        else
                        {
                            updated.IdLookupBusinessUnit = APMCommon.Instance.BusinessUnitList[SelectedBusinessIndex].IdLookupValue;
                        }
                    }

                    if (SelectedDepartmentIndex != -1 && PreviousSelectedDepartment != null)
                    {
                        if (APMCommon.Instance.DepartmentList[SelectedDepartmentIndex].IdDepartment != PreviousSelectedDepartment.IdDepartment)
                        {
                            updated.IdDepartment = (Int32)APMCommon.Instance.DepartmentList[SelectedDepartmentIndex].IdDepartment;
                            IsSaveChanges = true;
                        }
                        else
                        {
                            updated.IdDepartment = (Int32)APMCommon.Instance.DepartmentList[SelectedDepartmentIndex].IdDepartment;
                        }
                    }
                    if (PreviousSelectedCustomer != null && SelectedCustomer!=null)
                    {
                        if (SelectedCustomer?.IdSite != PreviousSelectedCustomer?.IdSite)
                        {
                            updated.IdSite = SelectedCustomer == null ? 0 : SelectedCustomer.IdSite;
                            IsSaveChanges = true;
                        }
                        else
                        {
                            updated.IdSite = SelectedCustomer == null ? 0 : SelectedCustomer.IdSite;
                        }
                    }
                    if (TempTaskList == null)
                    {
                        TempTaskList = new List<APMActionPlanTask>();
                    }
                    if (TaskList == null)
                    {
                        TaskList = new ObservableCollection<APMActionPlanTask>();
                    }
                    if (TempTaskList.Count != TaskList.Count)
                    {
                        IsSaveChanges = true;
                    }

                    if (IsSaveChanges)
                    {
                        MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ActionPlanUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            allowValidation = true;
                            error = EnableValidationAndGetError();

                            PropertyChanged(this, new PropertyChangedEventArgs("Description"));

                            if (error != null)
                            {

                                return;
                            }
                            AcceptButtonActionCommandAction(null);
                        }
                        else
                        {
                            IsSave = false;
                            //TaskList = new ObservableCollection<APMActionPlanTask>(TempTaskList);
                        }
                    }
                    //[shweta.thube][GEOS2-7218][23.07.2025]
                    if (IsNewActionAction)
                    {
                        AddedActionPlan.TaskList = TaskList.ToList();
                    }
                }

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelButtonActionCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelButtonActionCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void FillLeftTileList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLeftTileList ...", category: Category.Info, priority: Priority.Low);
                APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                IList<LookupValue> temp = APMService.GetLookupValues_V2550(155).ToList();
                if (ListOfFilterTile == null)
                {
                    ListOfFilterTile = new ObservableCollection<TileBarFilters>();
                }

                if (TaskList != null)
                {
                    ListOfFilterTile.Add(
                   new TileBarFilters()
                   {
                       Caption = "All",
                       Id = 0,
                       BackColor = null,
                       EntitiesCount = TaskList.Count(),
                       EntitiesCountVisibility = Visibility.Visible,
                       Height = 60,
                       width = 240
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
                            EntitiesCount = TaskList.Count(x => x.IdLookupTheme == item.IdLookupValue),
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 60,
                            width = 240
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
                       EntitiesCount = 0,
                       EntitiesCountVisibility = Visibility.Visible,
                       Height = 60,
                       width = 240
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
                            EntitiesCount = 0,
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 60,
                            width = 240
                        });
                    }
                }

                //ListOfFilterTile = new ObservableCollection<TileBarFilters>(ListOfFilterTile.Where(x => x.EntitiesCount != 0));

                ListOfFilterTile.Add(new TileBarFilters()
                {
                    Caption = (System.Windows.Application.Current.FindResource("CustomFilters").ToString()),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    Height = 30,
                    width = 240,
                });


                SelectedTileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption == "All");
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
        //private void FillResponsibleList()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillResponsibleList ...", category: Category.Info, priority: Priority.Low);
        //        if (APMCommon.Instance.ResponsibleList == null && ActionPlanResponsibleList == null)
        //        {
        //            APMCommon.Instance.ResponsibleList = new List<Responsible>();
        //            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
        //            Company usrDefault = GeosApplication.Instance.PlantOwnerUsersList.FirstOrDefault(x => x.ShortName == serviceurl);
        //            //var temp = APMService.GetActiveInactiveResponsibleForActionPlan_V2560(usrDefault.IdCompany, APMCommon.Instance.SelectedPeriod,
        //            //    APMCommon.Instance.ActiveEmployee.Organization, APMCommon.Instance.ActiveEmployee.EmployeeDepartments, APMCommon.Instance.IdUserPermission);
        //            string idPeriods = string.Empty;
        //            if (APMCommon.Instance.SelectedPeriod != null)
        //            {
        //                List<long> selectedPeriod = APMCommon.Instance.SelectedPeriod.Cast<long>().ToList();
        //                idPeriods = string.Join(",", selectedPeriod);
        //            }



        //            //[Sudhir.Jangra][GEOS2-6017]
        //            //var temp = APMService.GetActiveInactiveResponsibleForActionPlan_V2570(usrDefault.IdCompany, APMCommon.Instance.SelectedPeriod,
        //            //  APMCommon.Instance.ActiveEmployee.Organization, APMCommon.Instance.ActiveEmployee.EmployeeDepartments, APMCommon.Instance.IdUserPermission);

        //            //[Sudhir.Jangra][GEOS2-5976]
        //            var temp = APMService.GetActiveInactiveResponsibleForActionPlan_V2570(usrDefault.IdCompany, idPeriods,
        //           APMCommon.Instance.ActiveEmployee.Organization, APMCommon.Instance.ActiveEmployee.EmployeeDepartments, APMCommon.Instance.IdUserPermission);

        //            ActionPlanResponsibleList = new List<Responsible>(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));

        //            APMCommon.Instance.ResponsibleList.AddRange(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
        //            TempResponsibleList = new List<Responsible>(temp);
        //            APMCommon.Instance.ResponsibleList = new List<Responsible>(APMCommon.Instance.ResponsibleList);

        //        }

        //        if (ActionPlanResponsibleList == null)
        //        {
        //            ActionPlanResponsibleList = new List<Responsible>(APMCommon.Instance.ResponsibleList);
        //        }


        //        if (isNew)
        //        {
        //            SelectedResponsibleIndex = 0;
        //        }


        //        GeosApplication.Instance.Logger.Log("Method FillResponsibleList() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void LeftCommandFilterTileClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LeftCommandFilterTileClickAction....", category: Category.Info, priority: Priority.Low);
                if (TaskList.Count > 0)
                {
                    var temp = (System.Windows.Controls.SelectionChangedEventArgs)obj;
                    if (temp.AddedItems.Count > 0)
                    {
                        string str = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;
                        string Template = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Type;
                        string _FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                        CustomFilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;
                        CustomFilterHTMLColor = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).BackColor;
                        if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
                            return;
                        if (str == null)
                        {
                            if (!string.IsNullOrEmpty(_FilterString))
                            {
                                if (!string.IsNullOrEmpty(_FilterString))
                                    MyFilterString = _FilterString;
                                else
                                    MyFilterString = string.Empty;
                            }
                            else
                                MyFilterString = string.Empty;
                        }
                        else
                        {
                            if (str.Equals("All"))
                            {
                                MyFilterString = string.Empty;
                                TaskList = new ObservableCollection<APMActionPlanTask>(TempTaskList);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(_FilterString))
                                {
                                    if (!string.IsNullOrEmpty(_FilterString))
                                        MyFilterString = _FilterString;
                                    else
                                        MyFilterString = string.Empty;
                                }
                                else
                                    MyFilterString = string.Empty;
                            }
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method LeftCommandFilterTileClickAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method LeftCommandFilterTileClickAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AcceptButtonActionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonActionCommandAction....", category: Category.Info, priority: Priority.Low);
                allowValidation = true;
                error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("SelectedLocationIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedResponsibleIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedOriginIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedBusinessIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedDepartmentIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("Description")); //[pallavi.kale][GEOS2-8218]

                if (error != null)
                {
                    return;
                }
                if (IsNew)
                {
                    AddedActionPlan = new APMActionPlan();
                    APMActionPlan added = new APMActionPlan();
                    added.Code = Code;
                    added.Description = Description;
                    added.IdCompany = APMCommon.Instance.LocationList[SelectedLocationIndex].IdCompany;
                    added.IdEmployee = (Int32)APMCommon.Instance.ResponsibleList[SelectedResponsibleIndex].IdEmployee;
                    added.IdLookupOrigin = APMCommon.Instance.OriginList[SelectedOriginIndex].IdLookupValue;
                    added.IdLookupBusinessUnit = APMCommon.Instance.BusinessUnitList[SelectedBusinessIndex].IdLookupValue;
                    added.IdDepartment = (Int32)APMCommon.Instance.DepartmentList[SelectedDepartmentIndex].IdDepartment;//[Sudhir.Jangra][GEOS2-6596]
                    added.OriginDescription = OriginDescription;//[shweta.thube][GEOS2-6794]
                    added.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    added.CreatedByName = GeosApplication.Instance.ActiveUser.FullName;//[shweta.thube][GEOS2-7218][23.07.2025]

                    //[shweta.thube][GEOS2-6911]
                    if (SelectedCustomer != null)
                    {
                        added.IdSite = SelectedCustomer.IdSite;
                        added.GroupName = SelectedCustomer.GroupName;
                    }

                    //[nsatpute][24-10-2024][GEOS2-6018]
                    added.CommentList = new List<ActionPlanComment>();
                    if (CommentsList != null && CommentsList.Count > 0)
                    {
                        foreach (ActionPlanComment comment in CommentsList)
                        {
                            comment.People.OwnerImage = null;
                            added.CommentList.Add(comment);
                        }
                    }
                    if (added.ActionPlanLogEntries == null)
                    {
                        added.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();
                    }
                    added.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                    {
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Datetime = DateTime.Now,
                        Comments = string.Format(System.Windows.Application.Current.FindResource("AddChangeLogForActionPlan").ToString(), added.Code)
                    });

                    //added = APMService.AddActionPlanDetails_V2580(added);
                    //added = APMService.AddActionPlanDetails_V2590(added);//[shweta.thube][GEOS2-6586] 
                    //added = APMService.AddActionPlanDetails_V2600(added);//[shweta.thube][GEOS2-6794] 
                   // APMService = new APMServiceController("localhost:6699");
                    added = APMService.AddActionPlanDetails_V2610(added);//[shweta.thube][GEOS2-6911] 
                    IsSave = true;
                    IsNewActionAction = true;
                    IsAddTask = true;

                    if (NewTempTaskList != null && NewTempTaskList.Count > 0)
                    {
                        foreach (var item in NewTempTaskList)
                        {
                            if (item.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                //APMService = new APMServiceController("localhost:6699");
                                IsSave = APMService.DeleteTaskForActionPlan_V2570((int)item.IdActionPlanTask);
                            }
                        }
                    }

                    //[Sudhir.Jangra][GEOS2-6019]

                    if (ActionPlanAttachmentList != null && ActionPlanAttachmentList.Count > 0)
                    {
                        ActionPlanAttachmentList.ToList().ForEach(x => x.AttachmentImage = null);
                        foreach (AttachmentsByActionPlan item in ActionPlanAttachmentList)
                        {
                            if (item.ChangeLogList == null)
                            {
                                item.ChangeLogList = new List<LogEntriesByActionPlan>();
                            }
                            item.IdActionPlan = added.IdActionPlan;
                            item.ChangeLogList.Add(new LogEntriesByActionPlan()
                            {
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = DateTime.Now,
                                IdActionPlan = added.IdActionPlan,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("AddAttachmentChangeLogForActionPlan").ToString(), item.OriginalFileName)
                            });
                        }
                        //APMService = new APMServiceController("localhost:6699");
                        IsSave = APMService.AddActionPlanAttachments_V2580(ActionPlanAttachmentList.ToList());
                    }
                    //[pallavi.kale][GEOS2-7003]
                    if (AddEditSubTaskList != null && AddEditSubTaskList.Count > 0)
                    {
                        foreach (var item in AddEditSubTaskList)
                        {
                            APMActionPlanSubTask temp = item;
                            item.IdActionPlan = added.IdActionPlan;
                            item.Code = Code;
                            item.CreatedBy = (UInt32)GeosApplication.Instance.ActiveUser.IdUser;
                            if (item.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                //APMService = new APMServiceController("localhost:6699");
                                temp = APMService.AddSubTaskForActionPlan_V2650(item);
                            }
                            else if (item.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                //APMService = new APMServiceController("localhost:6699");
                                IsSave = APMService.UpdateSubTaskForActionPlan_V2650(item);
                            }
                        }
                    }
                    //[shweta.thube][GEOS2-7218][28.08.2025]
                    EditInit(added);
                   // APMService = new APMServiceController("localhost:6699");
                    ActionPlanInfo = APMService.GetActionPlanDetailsByIdActionPlan_V2660(added.IdActionPlan);
                    AddedActionPlan = ActionPlanInfo;
                    CreatedBy = ActionPlanInfo.CreatedByName;
                    CreatedIn = ActionPlanInfo.CreatedIn;
                    APMCommon.Instance.ActionPlanList.Add(ActionPlanInfo);
                    IsNew = false;

                    if (IsSave)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionPlanAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                }
                else
                {
                    APMActionPlan updated = new APMActionPlan();
                    updated.IdActionPlan = IdActionPlan;
                    updated.CommentList = new List<ActionPlanComment>();

                    updated.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();

                    if (CommentsList != null && CommentsList.Count > 0)
                    {
                        foreach (ActionPlanComment comment in CommentsList)
                        {
                            if (comment.People != null)
                                comment.People.OwnerImage = null;

                            if (comment.TransactionOperation == ModelBase.TransactionOperations.Update && comment.Id == 0)
                                comment.TransactionOperation = ModelBase.TransactionOperations.Add;

                            if (comment.TransactionOperation == ModelBase.TransactionOperations.Delete && comment.Id == 0)
                                comment.TransactionOperation = ModelBase.TransactionOperations.Nothing;

                            if (comment.TransactionOperation == ModelBase.TransactionOperations.Add ||
                                comment.TransactionOperation == ModelBase.TransactionOperations.Update ||
                                comment.TransactionOperation == ModelBase.TransactionOperations.Delete)
                                updated.CommentList.Add(comment);
                        }
                    }

                    if (DeleteCommentsList?.Count > 0)
                        updated.CommentList.AddRange(DeleteCommentsList);

                    if (Description != PreviousDescription)
                    {
                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = updated.IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanNameChangeLog").ToString(), PreviousDescription, Description)
                        });
                        updated.Description = Description;
                    }
                    else
                    {
                        updated.Description = Description;
                    }

                    if (APMCommon.Instance.LocationList[SelectedLocationIndex].IdCompany != PreviousSelectedLocation.IdCompany)
                    {
                        //[shweta.thube][GEOS2-6020]
                        string OldLoaction = APMCommon.Instance.LocationList.FirstOrDefault(i => i.IdCompany == PreviousSelectedLocation.IdCompany).Alias;
                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = updated.IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,

                            Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanLocationChangeLog").ToString(), OldLoaction, APMCommon.Instance.LocationList[SelectedLocationIndex].Alias)
                        });
                        updated.IdCompany = APMCommon.Instance.LocationList[SelectedLocationIndex].IdCompany;
                    }
                    else
                    {
                        updated.IdCompany = APMCommon.Instance.LocationList[SelectedLocationIndex].IdCompany;
                    }

                    if (ActionPlanResponsibleList != null)
                    {
                        if (PreviousSelectedResponsible == null)
                        {
                            PreviousSelectedResponsible = new Responsible();
                        }
                        if (ActionPlanResponsibleList[SelectedResponsibleIndex]?.IdEmployee != PreviousSelectedResponsible?.IdEmployee)
                        {
                            //[shweta.thube][GEOS2-6020]
                            //string OldResponsible = ActionPlanResponsibleList.FirstOrDefault(i => i.IdEmployee == PreviousSelectedResponsible?.IdEmployee).FullName;
                            updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                            {
                                IdActionPlan = updated.IdActionPlan,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,

                                Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanResponsibleChangeLog").ToString(), PreviousSelectedResponsible?.FullName, ActionPlanResponsibleList[SelectedResponsibleIndex]?.FullName)
                            });

                            updated.IdEmployee = (Int32)ActionPlanResponsibleList[SelectedResponsibleIndex].IdEmployee;
                        }
                        else
                        {
                            updated.IdEmployee = (Int32)ActionPlanResponsibleList[SelectedResponsibleIndex].IdEmployee;
                        }
                    }
                    else
                    {
                        if (PreviousSelectedResponsible == null)
                        {
                            PreviousSelectedResponsible = new Responsible();
                        }
                        updated.IdEmployee = (Int32)PreviousSelectedResponsible?.IdEmployee;
                    }


                    if (APMCommon.Instance.OriginList[SelectedOriginIndex].IdLookupValue != PreviousSelectedOrigin.IdLookupValue)
                    {
                        //[shweta.thube][GEOS2-6020]
                        string OldOrigin = APMCommon.Instance.OriginList.FirstOrDefault(i => i.IdLookupValue == PreviousSelectedOrigin.IdLookupValue).Value;
                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = updated.IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,

                            Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanOriginChangeLog").ToString(), OldOrigin, APMCommon.Instance.OriginList[SelectedOriginIndex].Value)
                        });

                        updated.IdLookupOrigin = APMCommon.Instance.OriginList[SelectedOriginIndex].IdLookupValue;
                    }
                    else
                    {
                        updated.IdLookupOrigin = APMCommon.Instance.OriginList[SelectedOriginIndex].IdLookupValue;
                    }
                    if (PreviousSelectedDepartment != null)
                    {
                        if (APMCommon.Instance.DepartmentList[SelectedDepartmentIndex].IdDepartment != PreviousSelectedDepartment.IdDepartment)
                        {
                            //[shweta.thube][GEOS2-6020]
                            string OldDepartment = APMCommon.Instance.DepartmentList.FirstOrDefault(i => i.IdDepartment == PreviousSelectedDepartment.IdDepartment).DepartmentName;
                            updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                            {
                                IdActionPlan = updated.IdActionPlan,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,

                                Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanDepartmentChangeLog").ToString(), OldDepartment, APMCommon.Instance.DepartmentList[SelectedDepartmentIndex].DepartmentName)
                            });

                            updated.IdDepartment = (Int32)APMCommon.Instance.DepartmentList[SelectedDepartmentIndex].IdDepartment;
                        }
                        else
                        {
                            updated.IdDepartment = (Int32)APMCommon.Instance.DepartmentList[SelectedDepartmentIndex].IdDepartment;
                        }
                    }
                    else
                    {
                        updated.IdDepartment = (Int32)APMCommon.Instance.DepartmentList[SelectedDepartmentIndex].IdDepartment;
                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = updated.IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,

                            Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanDepartmentChangeLog").ToString(), "None", APMCommon.Instance.DepartmentList[SelectedDepartmentIndex].DepartmentName)
                        });
                    }



                    if (APMCommon.Instance.BusinessUnitList[SelectedBusinessIndex].IdLookupValue != PreviousSelectedBusiness.IdLookupValue)
                    {
                        //[shweta.thube][GEOS2-6020]
                        string OldBusinessUnit = APMCommon.Instance.BusinessUnitList.FirstOrDefault(i => i.IdLookupValue == PreviousSelectedBusiness.IdLookupValue).Value;
                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = updated.IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,

                            Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanBusinessUnitChangeLog").ToString(), OldBusinessUnit, APMCommon.Instance.BusinessUnitList[SelectedBusinessIndex].Value)
                        });

                        updated.IdLookupBusinessUnit = APMCommon.Instance.BusinessUnitList[SelectedBusinessIndex].IdLookupValue;
                    }
                    else
                    {
                        updated.IdLookupBusinessUnit = APMCommon.Instance.BusinessUnitList[SelectedBusinessIndex].IdLookupValue;
                    }

                    updated.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    //[shweta.thube][GEOS2-6794]
                    if (OriginDescription != PreviousOriginDescription)
                    {
                        updated.OriginDescription = OriginDescription;
                    }
                    else
                    {
                        updated.OriginDescription = OriginDescription;
                    }

                    //[shweta.thube][GEOS2-6911]
                    if (PreviousSelectedCustomer != null)
                    {
                        if (SelectedCustomer?.IdSite != PreviousSelectedCustomer?.IdSite)
                        {
                            string OldCustomer = ClonedCustomerList.FirstOrDefault(i => i.IdSite == PreviousSelectedCustomer.IdSite).GroupName;
                            updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                            {
                                IdActionPlan = updated.IdActionPlan,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,

                                Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanCustomerChangeLog").ToString(), OldCustomer, SelectedCustomer == null ? "None" : SelectedCustomer.GroupName)
                            });

                            updated.IdSite = SelectedCustomer == null ? 0 : SelectedCustomer.IdSite;
                        }
                        else
                        {
                            updated.IdSite = SelectedCustomer == null ? 0 : SelectedCustomer.IdSite;
                        }
                    }
                    else
                    {
                        if (SelectedCustomer != null)
                        {
                            updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                            {
                                IdActionPlan = updated.IdActionPlan,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,

                                Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanCustomerChangeLog").ToString(), PreviousSelectedCustomer == null ? "None" : PreviousSelectedCustomer.GroupName, SelectedCustomer.GroupName)
                            });

                            updated.IdSite = SelectedCustomer.IdSite;
                        }
                    }


                    if (AddEditTaskList != null && AddEditTaskList.Count > 0)
                    {
                        foreach (var item in AddEditTaskList)
                        {
                            APMActionPlanTask temp = item;
                            if (item.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                //IsSave = APMService.AddTaskForActionPlan_V2560(item);
                                //[shweta.thube][GEOS2-5981]
                                //temp = APMService.AddTaskForActionPlan_V2570(item);
                                item.CreatedBy = (UInt32)GeosApplication.Instance.ActiveUser.IdUser;
                                //[shweta.thube][GEOS2-6020]
                                item.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();
                                item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                {
                                    IdActionPlan = item.IdActionPlan,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,

                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanAddNewTaskChangeLog").ToString(), item.Title, item.TaskNumber)
                                });

                                //temp = APMService.AddTaskForActionPlan_V2580(item);
                                //  temp = APMService.AddTaskForActionPlan_V2590(item); //[shweta.thube][GEOS2-6585]

                                //[Sudhir.Jangra][GEOS2-6593]
                                //IAPMService APMService = new APMServiceController("localhost:6699");
                                //temp = APMService.AddTaskForActionPlan_V2620(item);
                                temp = APMService.AddTaskForActionPlan_V2690(item);//[shweta.thube] [GEOS2-9870][19-11-2025]
                                item.IsTaskAdded = true;//[shweta.thube] [GEOS2-8394]
                                item.IdActionPlanTask = temp.IdActionPlanTask;
                                IsSaveChanges = false;
                            }
                            else if (item.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                item.TaskTabBrush = null;
                                item.TaskTabColor = new System.Drawing.Color();
                                //IsSave = APMService.UpdateTaskForActionPlan_V2560(item);
                                //[shweta.thube][GEOS2-5981]
                                //IsSave = APMService.UpdateTaskForActionPlan_V2570(item);

                                var tempTask = TempTaskList.FirstOrDefault(i => i.IdActionPlanTask == item.IdActionPlanTask);
                                OldTaskList = APMCommon.Instance.ActionPlanList.FirstOrDefault(i => i.IdActionPlan == item.IdActionPlan).TaskList;
                                //[shweta.thube][GEOS2-6020]
                                if (tempTask != null)
                                {

                                    item.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();

                                    if (tempTask.IdActionPlan != item.IdActionPlan)
                                    {
                                        // updated.IdActionPlan = SelectedActionPlans.IdActionPlan;

                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = tempTask.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("OldActionPlanTaskNumberChangeLog").ToString(), tempTask.Code, item.Code, item.TaskNumber)
                                        });
                                        if (OldTaskList == null)
                                        {
                                            item.TaskNumber = 1;
                                        }
                                        else
                                        {
                                            item.TaskNumber = OldTaskList.OrderByDescending(x => x.TaskNumber).First().TaskNumber + 1;
                                        }

                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {

                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("NewActionPlanTaskNumberChangeLog").ToString(), item.Code, tempTask.Code, item.TaskNumber)
                                        });
                                    }

                                    if (item.Title != tempTask.Title)
                                    {

                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("TaskTitleChangeLog").ToString(), tempTask.Title, item.Title, item.TaskNumber)
                                        });
                                    }
                                    if (item.IdCompany != tempTask.IdCompany)
                                    {
                                        string OldLoaction = APMCommon.Instance.LocationList.FirstOrDefault(i => i.IdCompany == tempTask.IdCompany).Alias;
                                        string NewLocation = APMCommon.Instance.LocationList.FirstOrDefault(i => i.IdCompany == item.IdCompany).Alias;
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("TaskLocationChangeLog").ToString(), OldLoaction, NewLocation, item.TaskNumber)
                                        });
                                    }

                                    if (item.IdEmployee != tempTask.IdEmployee)
                                    {
                                        //string OldResponsible = APMCommon.Instance.ResponsibleList.FirstOrDefault(i => i.IdEmployee == tempTask.IdEmployee).FullName;
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("TaskResponsibleChangeLog").ToString(), tempTask.Responsible, item.Responsible, item.TaskNumber)
                                        });
                                    }
                                    if (item.IdLookupStatus != tempTask.IdLookupStatus)
                                    {
                                        string OldStatus = StatusList.FirstOrDefault(i => i.IdLookupValue == tempTask.IdLookupStatus).Value;
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("TaskLStatusChangeLog").ToString(), OldStatus, item.Status, item.TaskNumber)
                                        });
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("AddActionPlanTaskChangeLogComment").ToString(), item.TaskNumber, item.TaskStatusComment)
                                        });
                                        //[shweta.thube][GEOS2-7005][22.07.2025]
                                        if (item.TaskAttachmentList != null)
                                        {
                                            foreach (AttachmentsByTask attachment in item.TaskAttachmentList)
                                            {
                                                item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                                {
                                                    IdActionPlan = item.IdActionPlan,
                                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                                    Comments = string.Format(System.Windows.Application.Current.FindResource("AddActionPlanTaskChangeLogattachment").ToString(), item.TaskNumber, attachment.FileUploadName)
                                                });
                                            }
                                        }
                                        //[shweta.thube][GEOS2-7005][22.07.2025]
                                        if (!string.IsNullOrEmpty(item.TaskStatusDescription))
                                        {
                                            item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = item.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,
                                                Comments = string.Format(System.Windows.Application.Current.FindResource("AddActionPlanTaskChangeLogAttachmentDescription").ToString(), item.TaskNumber, item.TaskStatusDescription)
                                            });
                                        }
                                    }

                                    if (item.IdLookupPriority != tempTask.IdLookupPriority)
                                    {
                                        string OldPriority = APMCommon.Instance.PriorityList.FirstOrDefault(i => i.IdLookupValue == tempTask.IdLookupPriority).Value;
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("TaskLPriorityChangeLog").ToString(), OldPriority, item.Priority, item.TaskNumber)
                                        });
                                    }

                                    if (item.IdDelegated != tempTask.IdDelegated)
                                    {
                                        if (tempTask.IdDelegated == 0)
                                        {
                                            //string OldDelegatedto = APMCommon.Instance.DelegatedToList.FirstOrDefault(i => i.IdEmployee == tempTask.IdDelegated).FullName;
                                            item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = item.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("TaskDelegatedtoPreviousNoneChangeLog").ToString(), item.DelegatedTo, item.TaskNumber)
                                            });
                                        }
                                        else if (item.IdDelegated == 0)
                                        {
                                            string OldDelegatedto = APMCommon.Instance.DelegatedToList.FirstOrDefault(i => i.IdEmployee == tempTask.IdDelegated).FullName;
                                            item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = item.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("TaskDelegatedtoSelectedNoneChangeLog").ToString(), OldDelegatedto, item.TaskNumber)
                                            });
                                        }
                                        else
                                        {
                                            string OldDelegatedto = APMCommon.Instance.DelegatedToList.FirstOrDefault(i => i.IdEmployee == tempTask.IdDelegated).FullName;
                                            item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = item.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("TaskDelegatedtoSelectedChangeLog").ToString(), OldDelegatedto, item.DelegatedTo, item.TaskNumber)
                                            });
                                        }
                                    }
                                    if (item.DueDate != tempTask.DueDate)
                                    {
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("TaskDueDateChangeLog").ToString(), tempTask.DueDate.ToString("dd-MM-yyyy"), item.DueDate.ToString("dd-MM-yyyy"), item.TaskNumber)
                                        });
                                    }
                                    if (item.Progress != tempTask.Progress)
                                    {
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("TaskProgressChangeLog").ToString(), tempTask.Progress, item.Progress, item.TaskNumber)
                                        });
                                    }

                                    if (!string.IsNullOrEmpty(item.CodeNumber) && !string.IsNullOrEmpty(tempTask.CodeNumber))
                                    {
                                        if (item.CodeNumber != tempTask.CodeNumber)
                                        {
                                            item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = updated.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("TaskOtItemChangeLog").ToString(), tempTask.CodeNumber, item.CodeNumber, item.TaskNumber)
                                            });
                                        }
                                    }
                                    else if (string.IsNullOrEmpty(tempTask.CodeNumber) && !string.IsNullOrEmpty(item.CodeNumber))
                                    {
                                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = updated.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("TaskOtItemChangeLog").ToString(), "None", item.CodeNumber, item.TaskNumber)
                                        });
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(tempTask.CodeNumber) && string.IsNullOrEmpty(item.CodeNumber))
                                        {
                                            updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = updated.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("TaskOtItemChangeLog").ToString(), tempTask.CodeNumber, "None", item.TaskNumber)
                                            });
                                        }
                                    }
                                }
                                //[shweta.thube][GEOS2-9870][19-11-2025]
                                if (item.IdSite != tempTask.IdSite)
                                {
                                    string OldCustomer = string.Empty;
                                    if (tempTask.IdSite != 0)
                                    {
                                         OldCustomer = ClonedCustomerList.FirstOrDefault(i => i.IdSite == tempTask.IdSite).GroupName;
                                    }                                    
                                    item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                    {
                                        IdActionPlan = updated.IdActionPlan,
                                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                        Datetime = GeosApplication.Instance.ServerDateTime,

                                        Comments = string.Format(System.Windows.Application.Current.FindResource("TaskCustomerChangeLog").ToString(),string.IsNullOrEmpty(OldCustomer) ? "None" : OldCustomer,
                                           string.IsNullOrEmpty(item.GroupName) ? "None" : item.GroupName, item.TaskNumber)
                                    });                         
                                }
                                

                                // IsSave = APMService.UpdateTaskForActionPlan_V2580(item);//[shweta.thube][GEOS2-6020]
                                // IsSave = APMService.UpdateTaskForActionPlan_V2590(item); //[shweta.thube][GEOS2-6585]

                                //[Sudhir.jangra][GEOS2-6593]
                                //IsSave = APMService.UpdateTaskForActionPlan_V2590(item);
                                //[Sudhir.Jangra][GEOS2-7006]
                                //APMService = new APMServiceController("localhost:6699");
                                //IsSave = APMService.UpdateTaskForActionPlan_V2620(item);
                                IsSave = APMService.UpdateTaskForActionPlan_V2690(item);//[shweta.thube] [GEOS2-9870][19-11-2025]
                                IsSaveChanges = false;
                            }
                        }
                    }
                    if (NewTempTaskList != null && NewTempTaskList.Count > 0)
                    {
                        foreach (var item in NewTempTaskList)
                        {
                            if (item.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                //[shweta.thube][GEOS2-6020]
                                item.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();
                                item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                {
                                    IdActionPlan = item.IdActionPlan,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,

                                    Comments = string.Format(System.Windows.Application.Current.FindResource("TaskDeletedChangeLog").ToString(), item.TaskNumber)
                                });
                                //IsSave = APMService.DeleteTaskForActionPlan_V2570((int)item.IdActionPlanTask);
                                //APMService = new APMServiceController("localhost:6699");
                                IsSave = APMService.DeleteTaskForActionPlan_V2580((int)item.IdActionPlanTask, item.ActionPlanLogEntries);//[shweta.thube][GEOS2-6020]
                                IsSaveChanges = false;
                            }
                        }
                    }
                    if (NewTempSubTaskList != null && NewTempSubTaskList.Count > 0)
                    {
                        foreach (var item in NewTempSubTaskList)
                        {
                            if (item.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                item.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();
                                item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                {
                                    IdActionPlan = item.IdActionPlan,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,

                                    Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDeletedChangeLog").ToString(), item.SubTaskCode)
                                });
                                //APMService = new APMServiceController("localhost:6699");
                                IsSave = APMService.DeleteSubTaskForActionPlan_V2650((int)item.IdActionPlanTask, item.ActionPlanLogEntries);
                                IsSaveChanges = false;
                            }
                        }
                    }


                    //[Sudhir.Jangra][GEOS2-6020]
                    if (updated.CommentList != null && updated.CommentList.Count > 0)
                    {
                        if (updated.ActionPlanLogEntries == null)
                        {
                            updated.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();
                        }
                        foreach (var item in updated.CommentList)
                        {
                            if (item.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                {
                                    IdActionPlan = updated.IdActionPlan,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("AddCommentChangeLog").ToString(), item.Comment)
                                });
                            }
                            else if (item.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                ActionPlanComment original = ClonedCommentList.FirstOrDefault(x => x.Id == item.Id);

                                updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                {
                                    IdActionPlan = updated.IdActionPlan,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("UpdatedCommentChangeLog").ToString(), original.Comment, item.Comment)
                                });
                            }
                            else if (item.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                {
                                    IdActionPlan = updated.IdActionPlan,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("DeleteCommentChangeLog").ToString(), item.Comment)
                                });
                            }
                        }
                    }
                    //[pallavi.kale][GEOS2-7003]
                    if (AddEditSubTaskList != null && AddEditSubTaskList.Count > 0)
                    {
                        foreach (var item in AddEditSubTaskList)
                        {
                            APMActionPlanSubTask temp = item;
                            if (item.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                item.CreatedBy = (UInt32)GeosApplication.Instance.ActiveUser.IdUser;
                                //[shweta.thube][GEOS2-7005][08.07.2025]
                                item.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();
                                item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                {
                                    IdActionPlan = item.IdActionPlan,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,

                                    Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanAddNewSubTaskChangeLog").ToString(), item.Title, item.SubTaskCode)
                                });

                                //APMService = new APMServiceController("localhost:6699");
                                //temp = APMService.AddSubTaskForActionPlan_V2650(item);
                                temp = APMService.AddSubTaskForActionPlan_V2660(item); //[shweta.thube][GEOS2-7005][08.07.2025]
                                item.IsSubTaskAdded = true;
                                item.IdActionPlanTask = temp.IdActionPlanTask;
                                IsSaveChanges = false;
                            }
                            else if (item.TransactionOperation == ModelBase.TransactionOperations.Update)
                            { //[shweta.thube][GEOS2-7005][08.07.2025]
                                item.TaskTabBrush = null;
                                item.TaskTabColor = new System.Drawing.Color();

                                OldSubTaskList = APMCommon.Instance.TaskList.FirstOrDefault(i => i.IdActionPlanTask == PreviousIdParent).SubTaskList;
                                var tempTask = OldSubTaskList.FirstOrDefault(i => i.IdActionPlanTask == item.IdActionPlanTask);
                                if (tempTask != null)
                                {
                                    item.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();
                                    if (item.ParentTaskNumber != tempTask.ParentTaskNumber)
                                    {
                                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = tempTask.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("OldActionPlanSubTaskNumberChangeLog").ToString(), tempTask.ParentTaskNumber, item.ParentTaskNumber, tempTask.SubTaskCode)
                                        });
                                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("NewActionPlanSubTaskNumberChangeLog").ToString(), tempTask.ParentTaskNumber, item.ParentTaskNumber, item.SubTaskCode)
                                        });


                                    }
                                    if (item.Title != tempTask.Title)
                                    {

                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskTitleChangeLog").ToString(), tempTask.Title, item.Title, item.SubTaskCode)
                                        });
                                    }
                                    if (item.IdCompany != tempTask.IdCompany)
                                    {
                                        string OldLoaction = APMCommon.Instance.LocationList.FirstOrDefault(i => i.IdCompany == tempTask.IdCompany).Alias;
                                        string NewLocation = APMCommon.Instance.LocationList.FirstOrDefault(i => i.IdCompany == item.IdCompany).Alias;
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskLocationChangeLog").ToString(), OldLoaction, NewLocation, item.SubTaskCode)
                                        });
                                    }
                                    if (item.IdEmployee != tempTask.IdEmployee)
                                    {
                                        //string OldResponsible = APMCommon.Instance.ResponsibleList.FirstOrDefault(i => i.IdEmployee == tempTask.IdEmployee).FullName;
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskResponsibleChangeLog").ToString(), tempTask.Responsible, item.Responsible, item.SubTaskCode)
                                        });
                                    }
                                    if (item.IdLookupStatus != tempTask.IdLookupStatus)
                                    {
                                        string OldStatus = StatusList.FirstOrDefault(i => i.IdLookupValue == tempTask.IdLookupStatus).Value;
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskLStatusChangeLog").ToString(), OldStatus, item.Status, item.SubTaskCode)
                                        });
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("AddActionPlanSubTaskChangeLogComment").ToString(), item.SubTaskCode, item.TaskStatusComment)
                                        });
                                        //[shweta.thube][GEOS2-7005][22.07.2025]
                                        if (item.TaskAttachmentList != null)
                                        {
                                            foreach (AttachmentsByTask attachment in item.TaskAttachmentList)
                                            {
                                                item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                                {
                                                    IdActionPlan = item.IdActionPlan,
                                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                                    Comments = string.Format(System.Windows.Application.Current.FindResource("AddActionPlanSubTaskChangeLogattachment").ToString(), item.SubTaskCode, attachment.FileUploadName)
                                                });
                                            }
                                        }
                                        //[shweta.thube][GEOS2-7005][22.07.2025]
                                        if (!string.IsNullOrEmpty(item.TaskStatusDescription))
                                        {
                                            item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = item.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,
                                                Comments = string.Format(System.Windows.Application.Current.FindResource("AddActionPlanSubTaskChangeLogAttachmentDescription").ToString(), item.SubTaskCode, item.TaskStatusDescription)
                                            });
                                        }

                                    }
                                    if (item.IdLookupTheme != tempTask.IdLookupTheme)
                                    {
                                        string OldTheme = APMCommon.Instance.ThemeList.FirstOrDefault(i => i.IdLookupValue == tempTask.IdLookupTheme).Value;
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskThemeChangeLog").ToString(), OldTheme, item.Theme, item.SubTaskCode)
                                        });
                                    }

                                    if (item.IdLookupPriority != tempTask.IdLookupPriority)
                                    {
                                        string OldPriority = APMCommon.Instance.PriorityList.FirstOrDefault(i => i.IdLookupValue == tempTask.IdLookupPriority).Value;
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskLPriorityChangeLog").ToString(), OldPriority, item.Priority, item.SubTaskCode)
                                        });
                                    }

                                    if (item.IdDelegated != tempTask.IdDelegated)
                                    {
                                        if (tempTask.IdDelegated == 0)
                                        {
                                            //string OldDelegatedto = APMCommon.Instance.DelegatedToList.FirstOrDefault(i => i.IdEmployee == tempTask.IdDelegated).FullName;
                                            item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = item.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDelegatedtoPreviousNoneChangeLog").ToString(), item.DelegatedTo, item.TaskNumber)
                                            });
                                        }
                                        else if (item.IdDelegated == 0)
                                        {
                                            string OldDelegatedto = APMCommon.Instance.DelegatedToList.FirstOrDefault(i => i.IdEmployee == tempTask.IdDelegated).FullName;
                                            item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = item.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDelegatedtoSelectedNoneChangeLog").ToString(), OldDelegatedto, item.TaskNumber)
                                            });
                                        }
                                        else
                                        {
                                            string OldDelegatedto = APMCommon.Instance.DelegatedToList.FirstOrDefault(i => i.IdEmployee == tempTask.IdDelegated).FullName;
                                            item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = item.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDelegatedtoSelectedChangeLog").ToString(), OldDelegatedto, item.DelegatedTo, item.TaskNumber)
                                            });
                                        }
                                    }
                                    if (item.DueDate != tempTask.DueDate)
                                    {
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDueDateChangeLog").ToString(), tempTask.DueDate.ToString("dd-MM-yyyy"), item.DueDate.ToString("dd-MM-yyyy"), item.SubTaskCode)
                                        });
                                    }
                                    if (item.Progress != tempTask.Progress)
                                    {
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskProgressChangeLog").ToString(), tempTask.Progress, item.Progress, item.SubTaskCode)
                                        });
                                    }

                                    if (!string.IsNullOrEmpty(item.CodeNumber) && !string.IsNullOrEmpty(tempTask.CodeNumber))
                                    {
                                        if (item.CodeNumber != tempTask.CodeNumber)
                                        {
                                            item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = item.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskOtItemChangeLog").ToString(), tempTask.CodeNumber, item.CodeNumber, item.SubTaskCode)
                                            });
                                        }
                                    }
                                    else if (string.IsNullOrEmpty(tempTask.CodeNumber) && !string.IsNullOrEmpty(item.CodeNumber))
                                    {
                                        item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                        {
                                            IdActionPlan = item.IdActionPlan,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,

                                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskOtItemChangeLog").ToString(), "None", item.CodeNumber, item.SubTaskCode)
                                        });
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(tempTask.CodeNumber) && string.IsNullOrEmpty(item.CodeNumber))
                                        {
                                            item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = item.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskOtItemChangeLog").ToString(), tempTask.CodeNumber, "None", item.SubTaskCode)
                                            });
                                        }
                                    }

                                    if (item.Description != tempTask.Description)
                                    {
                                        if (string.IsNullOrEmpty(tempTask.Description))
                                        {
                                            item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = item.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDescriptionChangeLog").ToString(), "None", item.Description, item.SubTaskCode)
                                            });
                                        }
                                        else if (string.IsNullOrEmpty(Description))
                                        {
                                            item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = item.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDescriptionChangeLog").ToString(), tempTask.Description, "None", item.SubTaskCode)
                                            });
                                        }
                                        else
                                        {
                                            item.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                            {
                                                IdActionPlan = item.IdActionPlan,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Datetime = GeosApplication.Instance.ServerDateTime,

                                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDescriptionChangeLog").ToString(), tempTask.Description, item.Description, item.SubTaskCode)
                                            });
                                        }

                                    }

                                }
                                //APMService = new APMServiceController("localhost:6699");
                                //IsSave = APMService.UpdateSubTaskForActionPlan_V2650(item);
                                IsSave = APMService.UpdateSubTaskForActionPlan_V2660(item); //[shweta.thube][GEOS2-7005][08.07.2025]
                                IsSaveChanges = false;

                            }
                        }
                    }



                    //IsSave = APMService.UpdateActionPlan_V2580(updated);  //[nsatpute][24-10-2024][GEOS2-6018]
                    //IsSave = APMService.UpdateActionPlan_V2590(updated); //[shweta.thube][GEOS2-6585]
                    //IsSave = APMService.UpdateActionPlan_V2600(updated);//[shweta.thube][GEOS2-6794]
                    //APMService = new APMServiceController("localhost:6699");
                    IsSave = APMService.UpdateActionPlan_V2610(updated);//[shweta.thube][GEOS2-6911]
                    IsSaveChanges = false;

                    List<AttachmentsByActionPlan> attachmentList = new List<AttachmentsByActionPlan>();
                    #region GEOS2-6019
                    if (ClonedActionPlanAttachmentList == null)
                    {
                        ClonedActionPlanAttachmentList = new List<AttachmentsByActionPlan>();
                    }
                    //Delete
                    foreach (AttachmentsByActionPlan item in ClonedActionPlanAttachmentList)
                    {
                        if (!ActionPlanAttachmentList.Any(x => x.IdActionPlanAttachment == item.IdActionPlanAttachment))
                        {
                            AttachmentsByActionPlan attachmentsByActionPlan = (AttachmentsByActionPlan)item.Clone();
                            attachmentsByActionPlan.IdActionPlan = IdActionPlan;
                            attachmentsByActionPlan.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            if (attachmentsByActionPlan.ChangeLogList == null)
                            {
                                attachmentsByActionPlan.ChangeLogList = new List<LogEntriesByActionPlan>();
                            }
                            attachmentsByActionPlan.ChangeLogList.Add(new LogEntriesByActionPlan()
                            {
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdActionPlan = IdActionPlan,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanChangeLogFilesDelete").ToString(), item.OriginalFileName)
                            });
                            attachmentList.Add(attachmentsByActionPlan);
                        }
                    }
                    //Added
                    foreach (AttachmentsByActionPlan item in ActionPlanAttachmentList)
                    {
                        if (!ClonedActionPlanAttachmentList.Any(x => x.IdActionPlanAttachment == item.IdActionPlanAttachment))
                        {
                            AttachmentsByActionPlan attachmentsByActionPlan = (AttachmentsByActionPlan)item.Clone();
                            attachmentsByActionPlan.IdActionPlan = IdActionPlan;
                            attachmentsByActionPlan.TransactionOperation = ModelBase.TransactionOperations.Add;
                            if (attachmentsByActionPlan.ChangeLogList == null)
                            {
                                attachmentsByActionPlan.ChangeLogList = new List<LogEntriesByActionPlan>();
                            }
                            attachmentsByActionPlan.ChangeLogList.Add(new LogEntriesByActionPlan()
                            {
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdActionPlan = IdActionPlan,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanChangeLogFilesAdd").ToString(), item.OriginalFileName)
                            });
                            attachmentList.Add(attachmentsByActionPlan);
                        }
                    }
                    //Updated
                    foreach (AttachmentsByActionPlan original in ClonedActionPlanAttachmentList)
                    {
                        if (ActionPlanAttachmentList.Any(x => x.IdActionPlanAttachment == original.IdActionPlanAttachment))
                        {
                            AttachmentsByActionPlan docUpdated = ActionPlanAttachmentList.FirstOrDefault(x => x.IdActionPlanAttachment == original.IdActionPlanAttachment);
                            if (original.SavedFileName != docUpdated.SavedFileName || original.Description != docUpdated.Description || original.OriginalFileName != docUpdated.OriginalFileName)
                            {
                                AttachmentsByActionPlan attachDoc = (AttachmentsByActionPlan)docUpdated.Clone();
                                attachDoc.IdActionPlan = IdActionPlan;
                                attachDoc.PreviousFileName = original.SavedFileName;
                                attachDoc.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                attachDoc.TransactionOperation = ModelBase.TransactionOperations.Update;
                                if (attachDoc.ChangeLogList == null)
                                {
                                    attachDoc.ChangeLogList = new List<LogEntriesByActionPlan>();
                                }
                                if (original.Description != docUpdated.Description)
                                {
                                    if (string.IsNullOrEmpty(original.Description) && (!string.IsNullOrEmpty(docUpdated.Description)))
                                    {
                                        attachDoc.ChangeLogList.Add(new LogEntriesByActionPlan()
                                        {
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,
                                            IdActionPlan = IdActionPlan,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanChangeLogFilesFileDescriptionUpdated").ToString(), "None", docUpdated.Description)
                                        });
                                    }
                                    else if (string.IsNullOrEmpty(docUpdated.Description) && (!string.IsNullOrEmpty(original.Description)))
                                    {
                                        attachDoc.ChangeLogList.Add(new LogEntriesByActionPlan()
                                        {
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,
                                            IdActionPlan = IdActionPlan,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanChangeLogFilesFileDescriptionUpdated").ToString(), original.Description, "None")
                                        });
                                    }
                                    else if (!string.IsNullOrEmpty(docUpdated.Description) && (!string.IsNullOrEmpty(original.Description)))
                                    {
                                        attachDoc.ChangeLogList.Add(new LogEntriesByActionPlan()
                                        {
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            Datetime = GeosApplication.Instance.ServerDateTime,
                                            IdActionPlan = IdActionPlan,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanChangeLogFilesFileDescriptionUpdated").ToString(), original.Description, docUpdated.Description)
                                        });
                                    }

                                }
                                if (original.FileByte != docUpdated.FileByte)
                                {
                                    attachDoc.ChangeLogList.Add(new LogEntriesByActionPlan()
                                    {
                                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        IdActionPlan = IdActionPlan,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanChangeLogFilesFileUpdated").ToString(), original.SavedFileName, docUpdated.SavedFileName)
                                    });
                                }
                                //if (original.SavedFileName != docUpdated.SavedFileName)
                                //{
                                //    attachDoc.ChangeLogList.Add(new LogEntriesByActionPlan()
                                //    {
                                //        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                //        Datetime = GeosApplication.Instance.ServerDateTime,
                                //        IdActionPlan = IdActionPlan,
                                //        Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanChangeLogFilesFileNameUpdated").ToString(), original.SavedFileName, docUpdated.SavedFileName)
                                //    });
                                //}


                                attachmentList.Add(attachDoc);
                            }
                        }
                    }

                    attachmentList.ToList().ForEach(x => x.AttachmentImage = null);
                    if (attachmentList != null && attachmentList.Count > 0)
                    {
                        //APMService = new APMServiceController("localhost:6699");
                        IsSave = APMService.AddUpdateDeleteAttachmentsForActionPlan_V2580(attachmentList);
                        IsSaveChanges = false;
                    }
                    if (IsNewActionAction)
                    {
                       // APMService = new APMServiceController("localhost:6699");
                        ActionPlanInfo = APMService.GetActionPlanDetailsByIdActionPlan_V2660(AddedActionPlan.IdActionPlan);
                        APMCommon.Instance.ActionPlanList.Add(ActionPlanInfo);
                        AddEditTaskList = new List<APMActionPlanTask>();
                        ActionPlanAttachmentList = new ObservableCollection<AttachmentsByActionPlan>();
                        AddEditSubTaskList = new List<APMActionPlanSubTask>();
                        NewTempSubTaskList = new List<APMActionPlanSubTask>();
                        NewTempTaskList = new List<APMActionPlanTask>();
                        updated.CommentList = new List<ActionPlanComment>();
                        ClonedActionPlanAttachmentList = new List<AttachmentsByActionPlan>();
                        EditInit(ActionPlanInfo);
                        if (IsSave)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionPlanUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        if (IsSave)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionPlanUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                        RequestClose(null, null);
                    }
                    #endregion
                }

                GeosApplication.Instance.Logger.Log("Method AcceptButtonActionCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonActionCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonActionCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonActionCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


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
                    if (!string.IsNullOrEmpty(CustomFilterStringName))
                    {
                        customFilterEditorViewModel.FilterName = CustomFilterStringName;
                    }
                    else
                    {
                        customFilterEditorViewModel.FilterName = SelectedTileBarItem.Caption;
                    }
                    if (!string.IsNullOrEmpty(CustomFilterHTMLColor))
                    {
                        customFilterEditorViewModel.HTMLColor = CustomFilterHTMLColor;
                    }
                    else
                    {
                        customFilterEditorViewModel.HTMLColor = SelectedTileBarItem.BackColor;
                    }


                    customFilterEditorViewModel.IsSave = true;
                    customFilterEditorViewModel.IsNew = false;
                    IsEdit = false;
                }
                else
                    customFilterEditorViewModel.IsNew = true;
                if (ListOfFilterTile == null)
                {
                    ListOfFilterTile = new ObservableCollection<TileBarFilters>();
                }


                customFilterEditorViewModel.Init(e.FilterControl, ListOfFilterTile);


                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");

                customFilterEditorView.Grid.Children.Add(e.FilterControl);


                customFilterEditorView.ShowDialog();

                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    if (ListOfFilterTile == null)
                    {
                        ListOfFilterTile = new ObservableCollection<TileBarFilters>();
                    }
                    TileBarFilters tileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));

                    if (tileBarItem != null)
                    {
                        ListOfFilterTile.Remove(tileBarItem);
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
                        ListOfFilterTile = new ObservableCollection<TileBarFilters>();
                        FillLeftTileList();
                        AddCustomSetting();
                        if (ListOfFilterTile.Count == 1)
                        {
                            ListOfFilterTile = new ObservableCollection<TileBarFilters>();
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && !customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    if (ListOfFilterTile == null)
                    {
                        ListOfFilterTile = new ObservableCollection<TileBarFilters>();
                    }
                    TileBarFilters tileBarItem = ListOfFilterTile.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        CustomFilterHTMLColor = customFilterEditorViewModel.HTMLColor;
                        TableView table = (TableView)e.OriginalSource;
                        GridControl gridControl = (table).Grid;

                        if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterCriteria))
                        {
                            gridControl.FilterCriteria = CriteriaOperator.Parse(customFilterEditorViewModel.FilterCriteria);
                        }

                        VisibleRowCount = gridControl.VisibleRowCount;
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
                                ListOfFilterTile.Where(a => a.Caption == SelectedTileBarItem.Caption).ToList().ForEach(b => b.BackColor = CustomFilterHTMLColor);
                                PreviousSelectedTopTileBarItem = SelectedTileBarItem;
                                string name = userSettingsKeyForActionPlan + tileBarItem.Caption + "_" + CustomFilterHTMLColor;
                                lstUserConfiguration.Add(new Tuple<string, string>(name.ToString(), tileBarItem.FilterCriteria));
                                continue;
                            }


                            if (!key.Equals(filterCaption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            else
                                lstUserConfiguration.Add(new Tuple<string, string>((userSettingsKeyForActionPlan + tileBarItem.Caption), tileBarItem.FilterCriteria));


                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                        //  ListOfFilterTile = new ObservableCollection<TileBarFilters>(ListOfFilterTile);
                        ListOfFilterTile = new ObservableCollection<TileBarFilters>();

                        FillLeftTileList();
                        AddCustomSetting();
                    }
                }
                else if (customFilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    TableView table = (TableView)e.OriginalSource;
                    GridControl gridControl = (table).Grid;

                    if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterCriteria))
                    {
                        gridControl.FilterCriteria = CriteriaOperator.Parse(customFilterEditorViewModel.FilterCriteria);
                    }

                    VisibleRowCount = gridControl.VisibleRowCount;
                    if (ListOfFilterTile == null)
                    {
                        ListOfFilterTile = new ObservableCollection<TileBarFilters>();
                    }
                    ListOfFilterTile.Add(new TileBarFilters()
                    {
                        Caption = customFilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = customFilterEditorViewModel.HTMLColor,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = customFilterEditorViewModel.FilterCriteria,
                        Height = 80,
                        width = 200,
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

                    ListOfFilterTile = new ObservableCollection<TileBarFilters>();

                    FillLeftTileList();
                    AddCustomSetting();
                    SelectedTileBarItem = ListOfFilterTile.LastOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CommandTileBarDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarDoubleClickAction()...", category: Category.Info, priority: Priority.Low);

                if (CustomFilterStringName == "CUSTOM FILTERS" || CustomFilterStringName == "All")
                {
                    return;
                }

                TableView table = (TableView)obj;
                GridControl gridControl = table.Grid;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();

                string filterContent = SelectedTileBarItem.FilterCriteria;

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

        private void AddCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = new List<KeyValuePair<string, string>>();
                if (ListOfFilterTile == null)
                {
                    ListOfFilterTile = new ObservableCollection<TileBarFilters>();
                }

                tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKeyForActionPlan)).ToList();

                if (tempUserSettings != null && tempUserSettings.Count > 0)
                {
                    if (!ListOfFilterTile.Any(x => x.Caption == "All"))
                    {
                        ListOfFilterTile.Add(
                                      new TileBarFilters()
                                      {
                                          Caption = "All",
                                          Id = 0,
                                          BackColor = null,
                                          ForeColor = null,
                                          FilterCriteria = null,
                                          EntitiesCount = TaskList.Count(),
                                          EntitiesCountVisibility = Visibility.Visible,
                                          Height = 60,
                                          width = 220
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
                            string filter = item.Value;
                            var parts = item.Key.Split(new[] { "_#" }, StringSplitOptions.None);
                            string baseKey = parts[0].Replace(userSettingsKeyForActionPlan, "");

                            string backColor = colorMapping.ContainsKey(baseKey) ? colorMapping[baseKey] : null;

                            bool isDuplicate = ListOfFilterTile.Any(tile => tile.Caption == baseKey && tile.FilterCriteria == item.Value);

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

                                    count = TaskList.Count(ap =>
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

                                    count = TaskList.Count(ap =>
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

                                    count = TaskList.Count(ap =>
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

                                    count = TaskList.Count(ap =>
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

                                    count = TaskList.Count(ap =>
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

                                    count = TaskList.Count(ap =>
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

                                        count = TaskList.Count(ap =>
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

                                        count = TaskList.Count(ap =>
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

                                        count = TaskList.Count(ap =>
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

                                        count = TaskList.Count(ap =>
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

                                        count = TaskList.Count(ap =>
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

                                        count = TaskList.Count(ap =>
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

                                        count = TaskList.Count(ap =>
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
                                    count = TaskList.Count(ap =>
                                    {
                                        var property = ap.GetType().GetProperty(columnName);
                                        var value = property?.GetValue(ap)?.ToString();
                                        return !string.IsNullOrEmpty(value);
                                    });
                                }
                                else if (filter.Contains("IsNullOrEmpty"))
                                {
                                    string columnName = filter.Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(']')[0].Trim();
                                    count = TaskList.Count(ap =>
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

                                    count = TaskList.Count(ap =>
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

                                    count = TaskList.Count(ap =>
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

                                        count = TaskList.Count(ap =>
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

                                        count = TaskList.Count(ap =>
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

                                    count = TaskList.Count();
                                }


                                ListOfFilterTile.Add(
                                    new TileBarFilters()
                                    {
                                        Caption = baseKey,
                                        Id = 0,
                                        BackColor = !string.IsNullOrEmpty(backColor) ? backColor : null,
                                        ForeColor = null,
                                        FilterCriteria = item.Value,
                                        EntitiesCount = count,
                                        EntitiesCountVisibility = Visibility.Visible,
                                        Height = 60,
                                        width = 220
                                    });
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSetting() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }

                SelectedTileBarItem = ListOfFilterTile.FirstOrDefault();

                GeosApplication.Instance.Logger.Log("Method AddCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][24-10-2024][GEOS2-6018]
        private void FillComentsList()
        {
            try
            {
                //APMService = new APMServiceController("localhost:6699");
                GeosApplication.Instance.Logger.Log("Method FillComentsList()...", category: Category.Info, priority: Priority.Low);
                CommentsList = new ObservableCollection<ActionPlanComment>(APMService.GetActionPlanCommentsByIdActionPlan(IdActionPlan));
                CommentsList = new ObservableCollection<ActionPlanComment>(commentList.OrderByDescending(c => c.Datetime).ToList());
                SetUserProfileImageForIdUser(CommentsList);
                GeosApplication.Instance.Logger.Log("Method FillComentsList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillComentsList() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][07-11-2024][GEOS2-6018]
        private void SetUserProfileImageForIdUser(ObservableCollection<ActionPlanComment> TaskCommentsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImageForIdUser ...", category: Category.Info, priority: Priority.Low);

                foreach (var item in TaskCommentsList)
                {
                    UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(item.People.Login);

                    if (UserProfileImageByte != null)
                        item.People.OwnerImage = ByteArrayToBitmapImage(UserProfileImageByte);
                    else
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (item.People.IdPersonGender == 1)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/FemaleUser_White.png");
                            else if (item.People.IdPersonGender == 2)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/MaleUser_White.png");
                            else if (item.People.IdPersonGender == null)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/wUnknownGender.png");

                        }
                        else
                        {
                            if (item.People.IdPersonGender == 1)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/FemaleUser_Blue.png");
                            else if (item.People.IdPersonGender == 2)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/MaleUser_Blue.png");
                            else if (item.People.IdPersonGender == null)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.APM;component/Assets/Images/blueUnknownGender.png");
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method SetUserProfileImageForIdUser() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImageForIdUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImageForIdUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImageForIdUser() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][07-11-2024][GEOS2-6018]
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new MemoryStream(byteArrayIn))
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
                return image;

                //GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        private void AddActionPlanTaskViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddActionPlanTaskViewWindowShow()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                AddEditTaskViewModel addEditTaskViewModel = new AddEditTaskViewModel();
                AddEditTaskView addEditTaskView = new AddEditTaskView();
                EventHandler handle = delegate { addEditTaskView.Close(); };
                addEditTaskViewModel.RequestClose += handle;
                addEditTaskViewModel.IsNew = true;
                addEditTaskViewModel.IsAddEditActionPlan = true;
                addEditTaskViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddActionPlansHeader").ToString();
                List<APMActionPlan> temp = new List<APMActionPlan>();
                addEditTaskViewModel.Code = Code;
                //[shweta.thube][GEOS2-7218][23.07.2025]
                if (IsNewActionAction)
                {
                    addEditTaskViewModel.Init(APMCommon.Instance.ActionPlanList.FirstOrDefault(x => x.IdActionPlan == ActionPlanInfo.IdActionPlan));
                }
                else
                {
                    addEditTaskViewModel.Init(APMCommon.Instance.ActionPlanList.FirstOrDefault(x => x.Code == Code));
                }
                addEditTaskViewModel.OtItemVisibility = Visibility.Collapsed;  //[shweta.thube][GEOS2-6912]
                addEditTaskView.DataContext = addEditTaskViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addEditTaskView.ShowDialogWindow();

                if (addEditTaskViewModel.IsSave)
                {
                    if (TaskList == null)
                    {
                        TaskList = new ObservableCollection<APMActionPlanTask>();
                    }
                    List<APMActionPlanTask> taskList = new List<APMActionPlanTask>(TaskList);
                    APMActionPlanTask task = new APMActionPlanTask();
                    task.IdActionPlan = addEditTaskViewModel.TempTask.IdActionPlan;
                    task.IdActionPlanTask = addEditTaskViewModel.TempTask.IdActionPlanTask;
                    task.Code = addEditTaskViewModel.TempTask.Code;
                    task.Title = addEditTaskViewModel.TempTask.Title;
                    task.Description = addEditTaskViewModel.TempTask.Description;
                    task.IdCompany = addEditTaskViewModel.TempTask.IdCompany;
                    task.Location = addEditTaskViewModel.TempTask.Location;
                    task.IdEmployee = addEditTaskViewModel.TempTask.IdEmployee;
                    task.Responsible = addEditTaskViewModel.SelectedResponsible.FullName;
                    task.IdLookupStatus = addEditTaskViewModel.SelectedStatus.IdLookupValue;
                    task.Status = addEditTaskViewModel.SelectedStatus.Value;

                    task.IdLookupPriority = addEditTaskViewModel.SelectedPriority.IdLookupValue;
                    task.Priority = addEditTaskViewModel.SelectedPriority.Value;
                    //task.IdLookupBusiness = addEditTaskViewModel.SelectedBusinessUnit.IdLookupValue;
                    // task.BusinessUnit = addEditTaskViewModel.SelectedBusinessUnit.Value;
                    task.IdLookupTheme = addEditTaskViewModel.SelectedTheme.IdLookupValue;
                    task.Theme = addEditTaskViewModel.SelectedTheme.Value;
                    if (addEditTaskViewModel.TempTask.IdDelegated != 0)
                    {
                        task.IdDelegated = addEditTaskViewModel.TempTask.IdDelegated;
                        task.DelegatedTo = addEditTaskViewModel.SelectedDelegatedto.FullName;
                    }
                    task.DueDate = addEditTaskViewModel.TempTask.DueDate;
                    task.OriginalDueDate = addEditTaskViewModel.TempTask.OriginalDueDate;
                    task.CloseDate = addEditTaskViewModel.TempTask.CloseDate;
                    task.OpenDate = addEditTaskViewModel.TempTask.OpenDate;
                    task.LastUpdated = addEditTaskViewModel.TempTask.LastUpdated;
                    task.IsTaskDeleted = true;  //[shweta.thube][GEOS2-8880][23.07.2025]
                    task.ClosedBy = addEditTaskViewModel.TempTask.ClosedBy;


                    DateTime currentDate = DateTime.Now;
                    TimeSpan difference = currentDate.Date - task.DueDate.Date;
                    int dueDays = difference.Days;
                    task.DueDays = dueDays;
                    string dueColor = "";
                    if (currentDate > task.DueDate && currentDate <= task.DueDate.AddDays(2))
                    {
                        dueColor = "#008000";
                    }
                    else if (currentDate > task.DueDate.AddDays(2) && currentDate <= task.DueDate.AddDays(7))
                    {
                        dueColor = "#FFFF00";
                    }
                    else if (currentDate > task.DueDate.AddDays(7))
                    {
                        dueColor = "#FF0000";
                    }
                    task.DueColor = dueColor;
                    task.Progress = addEditTaskViewModel.TempTask.Progress;
                    task.TaskNumber = addEditTaskViewModel.TempTask.TaskNumber;//[shweta.thube][GEOS2-8985]
                    task.StatusHTMLColor = addEditTaskViewModel.SelectedStatus.HtmlColor;
                    task.IdOTItem = (Int32)addEditTaskViewModel.TempTask.IdOTItem;        //[shweta.thube][GEOS2-6912]
                    task.CodeNumber = addEditTaskViewModel.TempTask.CodeNumber;        //[shweta.thube][GEOS2-6912]
                    task.IdSite= addEditTaskViewModel.TempTask.IdSite;//[shweta.thube][GEOS2-9870][19-11-2025]
                    task.GroupName= addEditTaskViewModel.TempTask.GroupName;//[shweta.thube][GEOS2-9870][19-11-2025]
                    task.TransactionOperation = ModelBase.TransactionOperations.Add;
                    //[shweta.thube][GEOS2-8985]
                    if (AddEditTaskList != null && AddEditTaskList.Count > 0)
                    {
                        var addedTasks = AddEditTaskList
                            .Where(x => x.TransactionOperation == ModelBase.TransactionOperations.Add)
                            .ToList();

                        if (addedTasks.Count > 0)
                        {
                            int maxTaskNumber = addedTasks.Select(x => x.TaskNumber).DefaultIfEmpty(0).Max();
                            task.TaskNumber = maxTaskNumber + 1;
                        }
                    }
                    taskList.Add(task);
                    TaskList = new ObservableCollection<APMActionPlanTask>(taskList);
                    if (AddEditTaskList == null)
                    {
                        AddEditTaskList = new List<APMActionPlanTask>();
                    }
                    if (!AddEditTaskList.Any(x => x == task))
                    {
                        AddEditTaskList.Add(task);
                    }
                    IsSaveChanges = true;
                    ListOfFilterTile = new ObservableCollection<TileBarFilters>();
                    FillLeftTileList();
                    AddCustomSetting();

                    IsAddButtonEnabled = true;
                }
                GeosApplication.Instance.Logger.Log("Method AddActionPlanTaskViewWindowShow()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddActionPlanTaskViewWindowShow()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditTaskHyperLinkCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditTaskHyperLinkCommandAction....", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                AddEditTaskView addEditTaskView = new AddEditTaskView();
                AddEditTaskViewModel addEditTaskViewModel = new AddEditTaskViewModel();
                EventHandler handle = delegate { addEditTaskView.Close(); };
                addEditTaskViewModel.RequestClose += handle;
                addEditTaskViewModel.IsNew = false;
                addEditTaskViewModel.IsAddEditActionPlan = true;
                addEditTaskViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditActionPlansHeader").ToString();
                SelectedTask.Code = Code;
                SelectedTask.IdActionPlanResponsible = InactiveSelectedResponsibleToIdEmployee;
                SelectedTask.ActionPlanResponsibleIdUser = APMCommon.Instance.ResponsibleList[SelectedResponsibleIndex].IdUser;
                //SelectedTask.IdSite = IdSiteCustomer;
                //if (SelectedCustomer != null)
                //{
                //    SelectedTask.CustomerName = SelectedCustomer.GroupName;
                //}
                SelectedTask.IdActionPlanLocation = (Int32)IdResponsibleLocation;
                addEditTaskViewModel.OtItemVisibility = Visibility.Collapsed;
                addEditTaskViewModel.EditInit(SelectedTask);
                addEditTaskView.DataContext = addEditTaskViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addEditTaskView.Owner = Window.GetWindow(ownerInfo);

                addEditTaskView.ShowDialog();
                if (!addEditTaskViewModel.IsNew)
                {
                    if (addEditTaskViewModel.IsSave)
                    {
                        SelectedTask.IdActionPlan = addEditTaskViewModel.UpdatedTask.IdActionPlan;
                        SelectedTask.IdActionPlanTask = addEditTaskViewModel.UpdatedTask.IdActionPlanTask;
                        if (!string.IsNullOrEmpty(addEditTaskViewModel.UpdatedTask.Code))
                        {
                            SelectedTask.Code = addEditTaskViewModel.UpdatedTask.Code;
                        }
                        else
                        {
                            SelectedTask.Code = Code;
                        }
                        // SelectedTask.Code = addEditTaskViewModel.UpdatedTask.Code;
                        SelectedTask.Title = addEditTaskViewModel.UpdatedTask.Title;
                        SelectedTask.IdCompany = addEditTaskViewModel.UpdatedTask.IdCompany;
                        SelectedTask.IdEmployee = addEditTaskViewModel.UpdatedTask.IdEmployee;
                        SelectedTask.Responsible = addEditTaskViewModel.SelectedResponsible.FullName;

                        //[shweta.thube][GEOS2-7218][23.07.2025]
                        if (addEditTaskViewModel.UpdatedTask.Progress == 100)
                        {
                            SelectedTask.Status = "Done";
                            SelectedTask.StatusHTMLColor = "#92D050";
                            SelectedTask.IdLookupStatus = 1982;
                        }
                        else
                        {
                            SelectedTask.Status = addEditTaskViewModel.SelectedStatus.Value;
                            SelectedTask.StatusHTMLColor = addEditTaskViewModel.SelectedStatus.HtmlColor;
                            SelectedTask.IdLookupStatus = addEditTaskViewModel.SelectedStatus.IdLookupValue;
                        }

                        //SelectedTask.IdLookupStatus = addEditTaskViewModel.SelectedStatus.IdLookupValue;
                        //SelectedTask.Status = addEditTaskViewModel.SelectedStatus.Value;
                        SelectedTask.IdLookupPriority = addEditTaskViewModel.SelectedPriority.IdLookupValue;
                        SelectedTask.Priority = addEditTaskViewModel.SelectedPriority.Value;
                        //SelectedTask.IdLookupBusiness = addEditTaskViewModel.SelectedBusinessUnit.IdLookupValue;
                        //SelectedTask.BusinessUnit = addEditTaskViewModel.SelectedBusinessUnit.Value;
                        SelectedTask.IdLookupTheme = addEditTaskViewModel.SelectedTheme.IdLookupValue;
                        SelectedTask.Theme = addEditTaskViewModel.SelectedTheme.Value;
                        if (addEditTaskViewModel.SelectedDelegatedto != null)
                        {
                            SelectedTask.IdDelegated = addEditTaskViewModel.UpdatedTask.IdDelegated;
                            SelectedTask.DelegatedTo = addEditTaskViewModel.SelectedDelegatedto.FullName;
                        }

                        SelectedTask.DueDate = addEditTaskViewModel.UpdatedTask.DueDate;
                        //[shweta.thube][GEOS2-6589]
                        if (addEditTaskViewModel.PreviousDueDate != SelectedTask.DueDate)
                        {
                            SelectedTask.IsShowIcon = true;
                        }
                        else
                        {
                            SelectedTask.IsShowIcon = false;
                        }
                        SelectedTask.CloseDate = addEditTaskViewModel.UpdatedTask.CloseDate;
                        SelectedTask.ChangeCount = addEditTaskViewModel.UpdatedTask.ChangeCount;
                        SelectedTask.LastUpdated = addEditTaskViewModel.UpdatedTask.LastUpdated;
                        DateTime currentDate = DateTime.Now;
                        TimeSpan difference = currentDate.Date - SelectedTask.DueDate;
                        int dueDays = difference.Days;
                        SelectedTask.DueDays = dueDays;
                        string dueColor = "";
                        if (currentDate > SelectedTask.DueDate && currentDate <= SelectedTask.DueDate.AddDays(2))
                        {
                            dueColor = "#008000";
                        }
                        else if (currentDate > SelectedTask.DueDate.AddDays(2) && currentDate <= SelectedTask.DueDate.AddDays(7))
                        {
                            dueColor = "#FFFF00";
                        }
                        else if (currentDate > SelectedTask.DueDate.AddDays(7))
                        {
                            dueColor = "#FF0000";
                        }
                        SelectedTask.DueColor = dueColor;
                        SelectedTask.Progress = addEditTaskViewModel.UpdatedTask.Progress;
                        // SelectedTask.StatusHTMLColor = addEditTaskViewModel.SelectedStatus.HtmlColor;
                        SelectedTask.Description = addEditTaskViewModel.UpdatedTask.Description;
                        SelectedTask.ClosedBy = addEditTaskViewModel.UpdatedTask.ClosedBy;
                        SelectedTask.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        if (!string.IsNullOrEmpty(addEditTaskViewModel.UpdatedTask.TaskStatusComment))
                        {
                            SelectedTask.TaskStatusComment = addEditTaskViewModel.UpdatedTask.TaskStatusComment;
                            SelectedTask.CommentsCount += 1;
                            SelectedTask.TaskLastComment = addEditTaskViewModel.UpdatedTask.TaskStatusComment;
                        }
                        if (addEditTaskViewModel.UpdatedTask.TaskAttachmentList != null)
                        {
                            addEditTaskViewModel.UpdatedTask.TaskAttachmentList.ForEach(x => x.Description = addEditTaskViewModel.UpdatedTask.TaskStatusDescription);
                            if (SelectedTask.TaskAttachmentList == null)
                            {
                                SelectedTask.TaskAttachmentList = new List<AttachmentsByTask>();
                            }
                            SelectedTask.TaskAttachmentList.AddRange(addEditTaskViewModel.UpdatedTask.TaskAttachmentList);
                            SelectedTask.FileCount = SelectedTask.TaskAttachmentList.Count();
                            SelectedTask.TaskStatusDescription = addEditTaskViewModel.UpdatedTask.TaskStatusDescription;
                        }
                        SelectedTask.IdOTItem = addEditTaskViewModel.UpdatedTask.IdOTItem;        //[shweta.thube][GEOS2-6912]
                        SelectedTask.CodeNumber = addEditTaskViewModel.UpdatedTask.CodeNumber;        //[shweta.thube][GEOS2-6912]
                        SelectedTask.IdSite= addEditTaskViewModel.UpdatedTask.IdSite;//[shweta.thube][GEOS2-9870][19-11-2025]
                        SelectedTask.GroupName = addEditTaskViewModel.UpdatedTask.GroupName;//[shweta.thube][GEOS2-9870][19-11-2025]
                        //SelectedTask.TransactionOperation = ModelBase.TransactionOperations.Update;
                        // SelectedTask.TaskNumber = TaskList.Last().TaskNumber + 1;
                        // Init();

                        if (AddEditTaskList == null)
                        {
                            AddEditTaskList = new List<APMActionPlanTask>();
                        }
                        if (!AddEditTaskList.Any(x => x == SelectedTask))
                        {
                            AddEditTaskList.Add(SelectedTask);
                        }
                        else
                        {
                            AddEditTaskList.Remove(SelectedTask);
                            AddEditTaskList.Add(SelectedTask);
                        }
                        //  TaskList.Remove(SelectedTask);//[shweta.thube][GEOS2-6020]
                        IsSaveChanges = true;
                        ListOfFilterTile = new ObservableCollection<TileBarFilters>();
                        FillLeftTileList();
                        AddCustomSetting();

                        IsAddButtonEnabled = true;//[Sudhir.Jangra][GEOS2-6697]

                    }
                }
                else
                {
                    //[shweta.thube][GEOS2-8394]
                    if (addEditTaskViewModel.IsSave)
                    {
                        if (TaskList == null)
                        {
                            TaskList = new ObservableCollection<APMActionPlanTask>();
                        }
                        List<APMActionPlanTask> taskList = new List<APMActionPlanTask>(TaskList);
                        APMActionPlanTask task = new APMActionPlanTask();
                        task.IdActionPlan = addEditTaskViewModel.TempTask.IdActionPlan;
                        task.IdActionPlanTask = addEditTaskViewModel.TempTask.IdActionPlanTask;
                        task.Code = addEditTaskViewModel.TempTask.Code;
                        task.Title = addEditTaskViewModel.TempTask.Title;
                        task.Description = addEditTaskViewModel.TempTask.Description;
                        task.IdCompany = addEditTaskViewModel.TempTask.IdCompany;
                        task.Location = addEditTaskViewModel.TempTask.Location;
                        task.IdEmployee = addEditTaskViewModel.TempTask.IdEmployee;
                        task.Responsible = addEditTaskViewModel.SelectedResponsible.FullName;
                        task.IdLookupStatus = addEditTaskViewModel.SelectedStatus.IdLookupValue;
                        task.Status = addEditTaskViewModel.SelectedStatus.Value;

                        task.IdLookupPriority = addEditTaskViewModel.SelectedPriority.IdLookupValue;
                        task.Priority = addEditTaskViewModel.SelectedPriority.Value;
                        //task.IdLookupBusiness = addEditTaskViewModel.SelectedBusinessUnit.IdLookupValue;
                        // task.BusinessUnit = addEditTaskViewModel.SelectedBusinessUnit.Value;
                        task.IdLookupTheme = addEditTaskViewModel.SelectedTheme.IdLookupValue;
                        task.Theme = addEditTaskViewModel.SelectedTheme.Value;
                        if (addEditTaskViewModel.TempTask.IdDelegated != 0)
                        {
                            task.IdDelegated = addEditTaskViewModel.TempTask.IdDelegated;
                            task.DelegatedTo = addEditTaskViewModel.SelectedDelegatedto.FullName;
                        }
                        task.DueDate = addEditTaskViewModel.TempTask.DueDate;
                        task.OriginalDueDate = addEditTaskViewModel.TempTask.OriginalDueDate;
                        task.CloseDate = addEditTaskViewModel.TempTask.CloseDate;
                        task.OpenDate = addEditTaskViewModel.TempTask.OpenDate;
                        task.LastUpdated = addEditTaskViewModel.TempTask.LastUpdated;

                        task.ClosedBy = addEditTaskViewModel.TempTask.ClosedBy;


                        DateTime currentDate = DateTime.Now;
                        TimeSpan difference = currentDate.Date - task.DueDate.Date;
                        int dueDays = difference.Days;
                        task.DueDays = dueDays;
                        string dueColor = "";
                        if (currentDate > task.DueDate && currentDate <= task.DueDate.AddDays(2))
                        {
                            dueColor = "#008000";
                        }
                        else if (currentDate > task.DueDate.AddDays(2) && currentDate <= task.DueDate.AddDays(7))
                        {
                            dueColor = "#FFFF00";
                        }
                        else if (currentDate > task.DueDate.AddDays(7))
                        {
                            dueColor = "#FF0000";
                        }
                        task.DueColor = dueColor;
                        task.Progress = addEditTaskViewModel.TempTask.Progress;
                        if (TaskList != null && TaskList.Count > 0)
                        {

                            task.TaskNumber = TaskList.OrderByDescending(x => x.TaskNumber).First().TaskNumber + 1;
                        }
                        else
                        {
                            task.TaskNumber = 1;
                        }
                        if (TaskList.Any(x => x.TaskNumber == task.TaskNumber - 1))
                        {
                            var existingTask = TaskList.FirstOrDefault(x => x.TaskNumber == task.TaskNumber - 1);
                            TaskList.Remove(existingTask);
                            taskList.Remove(existingTask);
                            AddEditTaskList.Remove(existingTask);
                            if (TaskList != null && TaskList.Count > 0)
                            {
                                task.TaskNumber = TaskList.OrderByDescending(x => x.TaskNumber).First().TaskNumber + 1;
                            }
                            else
                            {
                                task.TaskNumber = 1;
                            }

                        }
                        task.StatusHTMLColor = addEditTaskViewModel.SelectedStatus.HtmlColor;
                        task.IdOTItem = (Int32)addEditTaskViewModel.TempTask.IdOTItem;        //[shweta.thube][GEOS2-6912]
                        task.CodeNumber = addEditTaskViewModel.TempTask.CodeNumber;        //[shweta.thube][GEOS2-6912]
                        task.TransactionOperation = ModelBase.TransactionOperations.Add;
                        taskList.Add(task);
                        TaskList = new ObservableCollection<APMActionPlanTask>(taskList);
                        if (AddEditTaskList == null)
                        {
                            AddEditTaskList = new List<APMActionPlanTask>();
                        }
                        if (!AddEditTaskList.Any(x => x == task))
                        {
                            AddEditTaskList.Add(task);
                        }
                        IsSaveChanges = true;
                        ListOfFilterTile = new ObservableCollection<TileBarFilters>();
                        FillLeftTileList();
                        AddCustomSetting();

                        IsAddButtonEnabled = true;
                    }
                }


                GeosApplication.Instance.Logger.Log("Method EditTaskHyperLinkCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditTaskHyperLinkCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void DeleteTaskCommandAction(object obj)
        {
            try
            {

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 122 && up.Permission.IdGeosModule == 15))// admin
                {

                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 134 && up.Permission.IdGeosModule == 15)) // Location Manager
                {
                    if (SelectedTask.IdLookupStatus != 1979)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("APMDeleteTaskValidationAsPerPermission").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 135 && up.Permission.IdGeosModule == 15))// Department Manager
                {
                    if (SelectedTask.IdLookupStatus != 1979)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("APMDeleteTaskValidationAsPerPermission").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 136 && up.Permission.IdGeosModule == 15))// Base User
                {
                    if (SelectedTask.IdLookupStatus != 1979)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("APMDeleteTaskValidationAsPerPermission").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 121 && up.Permission.IdGeosModule == 15))//View
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("APMDeleteTaskValidationAsPerPermission").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                APMActionPlanTask Temp = (APMActionPlanTask)obj;
                //SelectedTask = TaskList.FirstOrDefault(x => x.IdActionPlanTask == Temp.IdActionPlanTask);
                if (Temp.SubTaskList == null || Temp.SubTaskList.Count == 0)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteTasksDetailsMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (NewTempTaskList == null)
                        {
                            NewTempTaskList = new List<APMActionPlanTask>();
                        }
                        if (!NewTempTaskList.Contains(SelectedTask))
                        {
                            SelectedTask.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            NewTempTaskList.Add(SelectedTask);
                        }
                        TaskList.Remove(SelectedTask);

                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TaskDetailsDeletedsuccessfully").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        IsAddButtonEnabled = true;
                    }
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteActionplanTaskMessage").ToString(), SelectedTask.TaskNumber), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    IsDeleted = false;
                }


                GeosApplication.Instance.Logger.Log("Method DeleteTaskCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteTaskCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

                TableView detailView = (TableView)obj;
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
                    selectedTask.CommentsCount = taskCommentsViewModel.TaskCommentsList.Count();
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

        //[shweta.thube][GEOS2-6020]
        private void ExportToExcel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportToExcel()...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "APMChangeLogHistory_" + DateTime.Now.ToString("MMddyyyy_hhmm");
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (bool)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
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
                    ResultFileName = (saveFile.FileName);
                    TableView ChangeLogTableView = ((TableView)obj);
                    ChangeLogTableView.ShowTotalSummary = false;
                    ChangeLogTableView.ShowFixedTotalSummary = false;
                    ChangeLogTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    ChangeLogTableView.ShowTotalSummary = false;
                    ChangeLogTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method ExportToExcel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportToExcel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][24-10-2024][GEOS2-6018]
        private void AddCommentsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFile()...", category: Category.Info, priority: Priority.Low);
                GridControl gridControlView = (GridControl)obj;
                AddEditCommentsView addCommentsView = new AddEditCommentsView();
                AddEditCommentsViewModel addCommentsViewModel = new AddEditCommentsViewModel();
                EventHandler handle = delegate { addCommentsView.Close(); };
                addCommentsViewModel.RequestClose += handle;
                addCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCommentsHeader").ToString();
                var ownerInfo = (gridControlView as FrameworkElement);
                addCommentsView.Owner = Window.GetWindow(ownerInfo);

                addCommentsView.DataContext = addCommentsViewModel;
                addCommentsView.ShowDialog();
                if (addCommentsViewModel.NewComment != null)
                {

                    if (CommentsList == null)
                        CommentsList = new ObservableCollection<ActionPlanComment>();
                    addCommentsViewModel.NewComment.IdActionPlan = IdActionPlan;
                    CommentsList.Insert(0, addCommentsViewModel.NewComment);
                    SelectedComment = addCommentsViewModel.NewComment;
                    CommentText = SelectedComment.Comment;
                    CommentDateTimeText = DateTime.Now;
                    CommentFullNameText = GeosApplication.Instance.ActiveUser.FullName;
                    IsAddButtonEnabled = true;
                    SelectedComment.IsDeleteButtonEnabled = true;
                }
                GeosApplication.Instance.Logger.Log("Method AddFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][24-10-2024][GEOS2-6018]
        public void DeleteCommentRowCommandAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert DeleteCommentCommandAction ...", category: Category.Info, priority: Priority.Low);
                ActionPlanComment commentObject = (ActionPlanComment)parameter;
                if (commentObject.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser || GeosApplication.Instance.IsAPMActionPlanPermission)
                {
                    bool result = false;

                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteProductTypeComment"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (CommentsList != null && CommentsList?.Count > 0)
                        {
                            ActionPlanComment Comment = (ActionPlanComment)commentObject;

                            CommentsList.Remove(Comment);

                            if (DeleteCommentsList == null)
                                DeleteCommentsList = new ObservableCollection<ActionPlanComment>();

                            DeleteCommentsList.Add(new ActionPlanComment()
                            {
                                IdUser = Comment.IdUser,
                                Comment = Comment.Comment,
                                Id = Comment.Id,
                                TransactionOperation = ModelBase.TransactionOperations.Delete
                            });
                            CommentsList = new ObservableCollection<ActionPlanComment>(CommentsList);

                            if (CommentsList?.Count > 0)
                            {
                                CommentText = CommentsList[CommentsList.Count - 1].Comment;
                                CommentDateTimeText = CommentsList[CommentsList.Count - 1].Datetime;
                                CommentFullNameText = CommentsList[CommentsList.Count - 1].People.FullName;
                            }
                            else
                            {
                                CommentText = string.Empty;
                                CommentDateTimeText = null;
                                CommentFullNameText = string.Empty;
                            }
                        }
                        IsAddButtonEnabled = true;
                    }
                }
                else
                {
                    CustomMessageBox.Show(Application.Current.Resources["PCMDeleteProductTypeCommentNotAllowed"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteCommentRowCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][24-10-2024][GEOS2-6018]
        private void CommentDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CommentDoubleClickCommandAction()...", category: Category.Info, priority: Priority.Low);
            ActionPlanComment logcomments = SelectedComment;
            if (logcomments.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser || GeosApplication.Instance.IsAPMActionPlanPermission)
            {
                AddEditCommentsView editCommentsView = new AddEditCommentsView();
                AddEditCommentsViewModel editCommentsViewModel = new AddEditCommentsViewModel();
                EventHandler handle = delegate { editCommentsView.Close(); };
                editCommentsViewModel.RequestClose += handle;
                editCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditCommentsHeader").ToString();
                editCommentsViewModel.EditedComment = logcomments;
                editCommentsViewModel.IsEditingMode = true;
                editCommentsViewModel.EditInit();
                editCommentsView.DataContext = editCommentsViewModel;
                editCommentsView.ShowDialog();
                if (editCommentsViewModel.IsSaved)
                {
                    logcomments.Datetime = GeosApplication.Instance.ServerDateTime;
                    logcomments.IsDeleteButtonEnabled = true;
                    if (UpdatedCommentsList == null)
                        UpdatedCommentsList = new List<ActionPlanComment>();
                    UpdatedCommentsList.Add(logcomments);
                    //UpdatedCommentsList.Insert(0, logcomments);
                    if (CommentsList.Contains(logcomments))
                    {
                        CommentsList.Remove(logcomments);
                    }

                    CommentsList.Insert(0, logcomments);

                    IsAddButtonEnabled = true;
                }
            }
            else
            {
                CustomMessageBox.Show(Application.Current.Resources["EditProductTypeCommentNotAllowed"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }

        }
        //[nsatpute][07-11-2024][GEOS2-6018]
        private BitmapImage GetImage(string path)
        {
            //return new BitmapImage(new Uri(path, UriKind.Relative));
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
        }
        //[Sudhir.Jangra][GEOS2-6016]
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
                TableView detailView = (TableView)obj;
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
                    List<APMActionPlanTask> tempList = new List<APMActionPlanTask>(TaskList);
                    tempList.FirstOrDefault(x => x.IdActionPlanTask == selectedTask.IdActionPlanTask).FileCount = taskAttachmentsViewModel.ListAttachment.Count();
                    TaskList = new ObservableCollection<APMActionPlanTask>(tempList);
                    //selectedTask.FileCount = taskAttachmentsViewModel.ListAttachment.Count();
                    IsSave = true;
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

        //[Sudhir.Jangra][GEOS2-6016]
        private void FillTaskList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTaskist ...", category: Category.Info, priority: Priority.Low);
                string idPeriods = string.Empty;
                if (APMCommon.Instance.SelectedPeriod != null)
                {
                    List<long> selectedPeriod = APMCommon.Instance.SelectedPeriod.Cast<long>().ToList();
                    idPeriods = string.Join(",", selectedPeriod);
                }

                //TaskList = new ObservableCollection<APMActionPlanTask>(APMService.GetTaskListByIdActionPlan_V2580(IdActionPlan, idPeriods));
                // TaskList = new ObservableCollection<APMActionPlanTask>(APMService.GetTaskListByIdActionPlan_V2590(IdActionPlan, idPeriods, GeosApplication.Instance.ActiveUser.IdUser));//[shweta.thube][GEOS2-6589]
                //[Sudhir.Jangra][GEOS2-6616]
                //TaskList = new ObservableCollection<APMActionPlanTask>(APMService.GetTaskListByIdActionPlan_V2610(IdActionPlan, idPeriods, GeosApplication.Instance.ActiveUser.IdUser));//[shweta.thube][GEOS2-6589]

                //[shweta.thube][GEOS2-6912]
                //TaskList = new ObservableCollection<APMActionPlanTask>(APMService.GetTaskListByIdActionPlan_V2620(IdActionPlan, idPeriods, GeosApplication.Instance.ActiveUser.IdUser));//[shweta.thube][GEOS2-6589]
                //[Sudhir.Jangra][GEOS2-7209]
                //TaskList = new ObservableCollection<APMActionPlanTask>(APMService.GetTaskListByIdActionPlan_V2630(IdActionPlan, idPeriods, GeosApplication.Instance.ActiveUser.IdUser));//[shweta.thube][GEOS2-6589]

                //[shweta.thube][GEOS2-7212][23.04.2025]
                //TaskList = new ObservableCollection<APMActionPlanTask>(APMService.GetTaskListByIdActionPlan_V2640(IdActionPlan, idPeriods, GeosApplication.Instance.ActiveUser.IdUser));//[shweta.thube][GEOS2-6589]
                // APMService = new APMServiceController("localhost:6699");

                //[pallavi.kale][GEOS2-7002][19.06.2025]
                //APMService = new APMServiceController("localhost:6699");
                //TaskList = new ObservableCollection<APMActionPlanTask>(APMService.GetTaskListByIdActionPlan_V2650(IdActionPlan, idPeriods, GeosApplication.Instance.ActiveUser.IdUser));
                //TaskList = new ObservableCollection<APMActionPlanTask>(APMService.GetTaskListByIdActionPlan_V2670(IdActionPlan, idPeriods, GeosApplication.Instance.ActiveUser.IdUser));//[shweta.thube][GEOS2-8695][22.09.2025]
                //TaskList = new ObservableCollection<APMActionPlanTask>(APMService.GetTaskListByIdActionPlan_V2680(IdActionPlan, idPeriods, GeosApplication.Instance.ActiveUser.IdUser));//[pallavi.kale][GEOS2-8997][28.10.2025]
                TaskList = new ObservableCollection<APMActionPlanTask>(APMService.GetTaskListByIdActionPlan_V2690(IdActionPlan, idPeriods, GeosApplication.Instance.ActiveUser.IdUser)); //[shweta.thube] [GEOS2-9868][07-11-2025]
                TaskList.Where(t => !t.IsTaskAdded).ToList().ForEach(t => t.IsTaskAdded = true); //[shweta.thube][GEOS2-8394]
                                                                                                 //[pallavi.kale][GEOS2-7003]
                foreach (var t in TaskList)
                {
                    if (t.SubTaskList != null)
                    {
                        foreach (var st in t.SubTaskList.Where(st => !st.IsSubTaskAdded))
                        {
                            st.IsSubTaskAdded = true;
                        }
                    }
                }

                //[shweta.thube][GEOS2-9910][16.10.2025]
                foreach (APMActionPlanTask item in TaskList)
                {
                    if (TaskList != null && TaskList.Count > 0)
                    {
                        foreach (APMActionPlanTask task in TaskList)
                        {
                            if (task.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser ||
                                item.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser ||
                                GeosApplication.Instance.IsAPMActionPlanPermission)
                            {
                                task.IsTaskDeleted = true;
                            }
                            else
                            {
                                task.IsTaskDeleted = false;
                            }

                            if (task.SubTaskList != null && task.SubTaskList.Count > 0)
                            {
                                foreach (APMActionPlanSubTask subtask in task.SubTaskList)
                                {
                                    subtask.IsSubTaskDeleted = (
                                        subtask.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser ||
                                        task.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser ||
                                        item.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser ||
                                        GeosApplication.Instance.IsAPMActionPlanPermission
                                    );
                                }
                            }

                        }
                    }

                }
                TaskList = new ObservableCollection<APMActionPlanTask>(TaskList);
                GeosApplication.Instance.Logger.Log("Method FillTaskist() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTaskist() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTaskist() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTaskist() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[shweta.thube][GEOS2-6020]
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList ...", category: Category.Info, priority: Priority.Low);
                // if (APMCommon.Instance.StatusList == null)
                //{
                //APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                var temp = new List<LookupValue>(APMService.GetLookupValues_V2550(152));//TaskStatus
                StatusList = new ObservableCollection<LookupValue>(temp);
                //APMCommon.Instance.StatusList = new List<LookupValue>(temp);
                // }
                GeosApplication.Instance.Logger.Log("Method FillStatusList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-6019]

        private void AddAttachmentFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddAttachmentFileCommandAction ...", category: Category.Info, priority: Priority.Low);
                AddEditActionPlanAttachmentsView addEditActionPlanAttachmentsView = new AddEditActionPlanAttachmentsView();
                AddEditActionPlanAttachmentsViewModel addEditActionPlanAttachmentsViewModel = new AddEditActionPlanAttachmentsViewModel();
                addEditActionPlanAttachmentsView.DataContext = addEditActionPlanAttachmentsViewModel;
                EventHandler handle = delegate { addEditActionPlanAttachmentsView.Close(); };
                addEditActionPlanAttachmentsViewModel.RequestClose += handle;
                addEditActionPlanAttachmentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddActionPlanAttachmentHeader").ToString();
                addEditActionPlanAttachmentsViewModel.Init();
                addEditActionPlanAttachmentsViewModel.IsNew = true;
                addEditActionPlanAttachmentsView.ShowDialog();
                if (addEditActionPlanAttachmentsViewModel.IsSave)
                {
                    if (ActionPlanAttachmentList == null)
                    {
                        ActionPlanAttachmentList = new ObservableCollection<AttachmentsByActionPlan>();
                    }
                    if (addEditActionPlanAttachmentsViewModel.AttachmentObjectList != null)
                    {
                        foreach (var item in addEditActionPlanAttachmentsViewModel.AttachmentObjectList)
                        {
                            item.IsDeleteButtonEnabled = true;
                            //ActionPlanAttachmentList.Add(item);
                            ActionPlanAttachmentList.Insert(0, item);//[shweta.thube][GEOS2-6019]
                        }
                    }
                    IsAddButtonEnabled = true;
                }
                GeosApplication.Instance.Logger.Log("Method AddAttachmentFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddAttachmentFileCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditActionPlanFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditActionPlanFileCommandAction ...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                AttachmentsByActionPlan attachment = (AttachmentsByActionPlan)detailView.DataControl.CurrentItem;
                if (attachment.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser || GeosApplication.Instance.IsAPMActionPlanPermission)
                {
                    Int64 idActionPlanAttachment = attachment.IdActionPlanAttachment;
                    int selectedIndex = ActionPlanAttachmentList.IndexOf(attachment);
                    AddEditActionPlanAttachmentsView addEditActionPlanAttachmentsView = new AddEditActionPlanAttachmentsView();
                    AddEditActionPlanAttachmentsViewModel addEditActionPlanAttachmentsViewModel = new AddEditActionPlanAttachmentsViewModel();
                    addEditActionPlanAttachmentsView.DataContext = addEditActionPlanAttachmentsViewModel;
                    EventHandler handle = delegate { addEditActionPlanAttachmentsView.Close(); };
                    addEditActionPlanAttachmentsViewModel.RequestClose += handle;
                    addEditActionPlanAttachmentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditActionPlanAttachmentHeader").ToString();
                    addEditActionPlanAttachmentsViewModel.EditInit(attachment);
                    addEditActionPlanAttachmentsViewModel.IsNew = false;
                    var ownerInfo = (detailView as FrameworkElement);
                    addEditActionPlanAttachmentsView.Owner = Window.GetWindow(ownerInfo);
                    addEditActionPlanAttachmentsView.ShowDialog();
                    if (addEditActionPlanAttachmentsViewModel.IsSave)
                    {
                        attachment = addEditActionPlanAttachmentsViewModel.ActionPlanAttachmentFile;
                        attachment.AttachmentImage = addEditActionPlanAttachmentsViewModel.ActionPlanAttachmentFile.AttachmentImage;
                        //attachment.SavedFileName = addEditActionPlanAttachmentsViewModel.FileNameString;
                        //     attachment.OriginalFileName= addEditActionPlanAttachmentsViewModel.FileNameString;
                        attachment.CreatedIn = DateTime.Now;
                        attachment.Description = addEditActionPlanAttachmentsViewModel.Description;
                        attachment.IdActionPlanAttachment = idActionPlanAttachment;
                        attachment.IsDeleteButtonEnabled = true;
                        //SelectedActionPlanAttachment = ActionPlanAttachmentList[selectedIndex] = attachment;
                        if (selectedIndex >= 0 && selectedIndex < ActionPlanAttachmentList.Count)
                        {
                            ActionPlanAttachmentList.RemoveAt(selectedIndex);
                        }
                        ActionPlanAttachmentList.Insert(0, attachment);
                        SelectedActionPlanAttachment = attachment;
                        IsAddButtonEnabled = true;
                    }
                }
                else
                {
                    CustomMessageBox.Show(Application.Current.Resources["EditActionPlanAttachmentNotAllowed"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method EditActionPlanFileCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditActionPlanFileCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillActionPlanAttachmentList(Int64 idActionPlan)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTaskist ...", category: Category.Info, priority: Priority.Low);
                ActionPlanAttachmentList = new ObservableCollection<AttachmentsByActionPlan>(APMService.GetActionPlanAttachmentByIdActionPlan_V2580(idActionPlan));
                if (ClonedActionPlanAttachmentList == null)
                {
                    ClonedActionPlanAttachmentList = new List<AttachmentsByActionPlan>();
                }
                foreach (AttachmentsByActionPlan item in ActionPlanAttachmentList)
                {
                    ImageSource objImage = FileExtensionToFileIcon.FindIconForFilename(item.SavedFileName, true);
                    item.AttachmentImage = objImage;
                    item.OriginalFileName = Path.GetFileNameWithoutExtension(item.SavedFileName);
                    ClonedActionPlanAttachmentList.Add((AttachmentsByActionPlan)item.Clone());
                }
                GeosApplication.Instance.Logger.Log("Method FillTaskist() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTaskist() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTaskist() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTaskist() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        protected ISaveFileDialogService SaveFileDialogService
        {
            get
            {
                return this.GetService<ISaveFileDialogService>();
            }
        }

        public void DownloadFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
                bool isDownload = false;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ActionPlanTaskFileDownloadMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    AttachmentsByActionPlan attachmentObject = (AttachmentsByActionPlan)obj;
                    FileInfo file = new FileInfo(attachmentObject.SavedFileName);
                    SaveFileDialogService.DefaultExt = file.Extension;
                    SaveFileDialogService.DefaultFileName = attachmentObject.OriginalFileName;
                    SaveFileDialogService.Filter = "All Files|*.*";
                    SaveFileDialogService.FilterIndex = 1;
                    DialogResult = SaveFileDialogService.ShowDialog();
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
                                //WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                                win.Topmost = false;
                                return win;
                            }, x =>
                            {
                                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                            }, null, null);
                        }
                        attachmentObject.FileUploadName = attachmentObject.SavedFileName;
                        attachmentObject.AttachmentImage = null;
                        //APMService = new APMServiceController("localhost:6699");
                        attachmentObject = APMService.DownloadActionPlanAttachment_V2580(attachmentObject);
                        ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;
                        isDownload = SaveData(ResultFileName, attachmentObject.FileByte);
                    }
                    if (isDownload)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileDownloadSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                        IsBusy = false;
                    }
                    else
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileDownloadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        IsBusy = false;
                    }
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    IsBusy = false;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        protected bool SaveData(string FileName, byte[] Data)
        {
            BinaryWriter Writer = null;
            string Name = FileName;
            try
            {
                // Create a new stream to write to the file
                Writer = new BinaryWriter(File.OpenWrite(Name));
                // Writer raw data
                Writer.Write(Data);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                //...
                return false;
            }
            return true;
        }

        //[Sudhir.Jangra][GEOS2-6019]
        public void DeleteAttachmentRowCommandAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteAttachmentRowCommandAction() ...", category: Category.Info, priority: Priority.Low);
                // DXSplashScreen.Show<SplashScreenView>();
                // if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
                bool isDelete = false;
                AttachmentsByActionPlan attachmentObject = (AttachmentsByActionPlan)parameter;
                if (attachmentObject.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser || GeosApplication.Instance.IsAPMActionPlanPermission)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ActionPlanTaskFileDeleteMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (ActionPlanAttachmentList != null && ActionPlanAttachmentList.Count > 0)
                        {
                            List<AttachmentsByActionPlan> tempData = new List<AttachmentsByActionPlan>(ActionPlanAttachmentList);
                            tempData.Remove((AttachmentsByActionPlan)attachmentObject);
                            ActionPlanAttachmentList = new ObservableCollection<AttachmentsByActionPlan>(tempData);
                            isDelete = true;
                            IsAddButtonEnabled = true;
                        }
                        if (isDelete)
                        {
                            IsBusy = false;
                            GeosApplication.Instance.Logger.Log("Method DeleteAttachmentRowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionPlanTaskFileDeleteFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        IsBusy = false;
                    }
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionPlanTaskFileDeleteNotAllowed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Shweta.thube][GEOS2-6591]
        private void LocationListClosedCommandAction(object obj)
        {
            try
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

                var selectedLocationItem = APMCommon.Instance.LocationList[SelectedLocationIndex];
                SelectedLocation = selectedLocationItem;

                string idCompanyString = Convert.ToString(SelectedLocation.IdCompany);
                if (PreviousSelectedLocation != null)
                {
                    if (APMCommon.Instance.LocationList[SelectedLocationIndex].IdCompany != PreviousSelectedLocation.IdCompany)
                    {
                        FillResponsibleList(idCompanyString);
                    }
                }
                else
                {
                    FillResponsibleList(idCompanyString);
                }
                //SelectedLocation = APMCommon.Instance.LocationList.FirstOrDefault(x => x.IdCompany == SelectedActionPlan.IdCompany);
                //ResponsibleList = new ObservableCollection<Responsible>(APMCommon.Instance.ResponsibleList.Where(x => x.IdOrganizationCode == SelectedLocation.IdCompany));

                if (IsNew != true)
                {
                    if (APMCommon.Instance.LocationList[SelectedLocationIndex].IdCompany == PreviousSelectedLocation.IdCompany)
                    {
                        if (!APMCommon.Instance.ResponsibleList.Any(x => x.IdEmployee == InactiveSelectedResponsibleToIdEmployee) && InactiveSelectedResponsibleToIdEmployee != 0)
                        {
                            //APMService = new APMServiceController("localhost:6699");
                            Responsible temp = APMService.GetInactiveResponsibleForActionPlan_V2560(InactiveSelectedResponsibleToIdEmployee);
                            temp.IsInActive = true;
                            temp.EmployeeCodeWithIdGender = temp.EmployeeCode + "_" + temp.IdGender;
                            if (ActionPlanResponsibleList != null)
                            {
                                ActionPlanResponsibleList.Add(temp);
                            }
                            APMCommon.Instance.ResponsibleList.Add(temp);

                            SelectedResponsibleIndex = APMCommon.Instance.ResponsibleList.IndexOf(temp);
                        }
                        else
                        {
                            SelectedResponsibleIndex = APMCommon.Instance.ResponsibleList.ToList().FindIndex(x => x.IdEmployee == InactiveSelectedResponsibleToIdEmployee);
                        }
                    }
                    else
                    {
                        SelectedResponsibleIndex = -1;
                    }

                }
                else
                {
                    SelectedResponsibleIndex = -1;
                }



                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LocationListClosedCommandAction() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6591]
        private void FillResponsibleList(string idCompanies)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillResponsibleList ...", category: Category.Info, priority: Priority.Low);
                //if (APMCommon.Instance.ResponsibleList == null && ActionPlanResponsibleList == null)
                //{
                APMCommon.Instance.ResponsibleList = new List<Responsible>();
                // string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                // Company usrDefault = GeosApplication.Instance.PlantOwnerUsersList.FirstOrDefault(x => x.ShortName == serviceurl);
                //var temp = APMService.GetActiveInactiveResponsibleForActionPlan_V2560(usrDefault.IdCompany, APMCommon.Instance.SelectedPeriod,
                //    APMCommon.Instance.ActiveEmployee.Organization, APMCommon.Instance.ActiveEmployee.EmployeeDepartments, APMCommon.Instance.IdUserPermission);
                string idPeriods = string.Empty;
                if (APMCommon.Instance.SelectedPeriod != null)
                {
                    List<long> selectedPeriod = APMCommon.Instance.SelectedPeriod.Cast<long>().ToList();
                    idPeriods = string.Join(",", selectedPeriod);
                }


                #region
                //[Sudhir.Jangra][GEOS2-6017]
                //var temp = APMService.GetActiveInactiveResponsibleForActionPlan_V2570(usrDefault.IdCompany, APMCommon.Instance.SelectedPeriod,
                //  APMCommon.Instance.ActiveEmployee.Organization, APMCommon.Instance.ActiveEmployee.EmployeeDepartments, APMCommon.Instance.IdUserPermission);

                //[Sudhir.Jangra][GEOS2-5976]
                // var temp = APMService.GetActiveInactiveResponsibleForActionPlan_V2570(usrDefault.IdCompany, idPeriods,
                //APMCommon.Instance.ActiveEmployee.Organization, APMCommon.Instance.ActiveEmployee.EmployeeDepartments, APMCommon.Instance.IdUserPermission);

                //APMService = new APMServiceController("localhost:6699");
                // var temp = APMService.GetResponsibleListAsPerLocation_V2580(idCompanies);//[shweta.thube][GEOS2-6591]

                //[Sudhir.Jangra][GEOS2-6787]
                //APMService = new APMServiceController("localhost:6699");
                //Service GetResponsibleListAsPerLocation_V2600 changed to GetResponsibleListAsPerLocation_V2670 [rdixit][GEOS2-9354][01.09.2025]
                #endregion
                var temp = APMService.GetResponsibleListAsPerLocation_V2670(idCompanies);


                ActionPlanResponsibleList = new List<Responsible>(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));

                APMCommon.Instance.ResponsibleList.AddRange(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                TempResponsibleList = new List<Responsible>(temp);
                APMCommon.Instance.ResponsibleList = new List<Responsible>(APMCommon.Instance.ResponsibleList);

                //}

                if (ActionPlanResponsibleList == null)
                {
                    ActionPlanResponsibleList = new List<Responsible>(APMCommon.Instance.ResponsibleList);
                }


                if (isNew)
                {
                    SelectedResponsibleIndex = 0;
                }


                GeosApplication.Instance.Logger.Log("Method FillResponsibleList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //[Sudhir.Jangra][GEOS2-6697]
        private void SetUserPermission()
        {
            if (IsNew)
            {
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 122 && up.Permission.IdGeosModule == 15))// admin
                {
                    IsAddButtonEnabled = true;
                    DeleteCommentButtonVisible = true;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 134 && up.Permission.IdGeosModule == 15)) // Location Manager
                {
                    IsAddButtonEnabled = true;
                    DeleteCommentButtonVisible = true;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 135 && up.Permission.IdGeosModule == 15))// Department Manager
                {
                    IsAddButtonEnabled = true;
                    DeleteCommentButtonVisible = true;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 136 && up.Permission.IdGeosModule == 15))// Base User
                {
                    IsAddButtonEnabled = true;
                    DeleteCommentButtonVisible = true;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 121 && up.Permission.IdGeosModule == 15))//View
                {
                    IsAddButtonEnabled = false;
                    DeleteCommentButtonVisible = false;
                }
            }
            else
            {
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 122 && up.Permission.IdGeosModule == 15))// admin
                {

                    IsAddButtonEnabled = true;
                    DeleteCommentButtonVisible = true;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 134 && up.Permission.IdGeosModule == 15)) // Location Manager
                {
                    IsAddButtonEnabled = true;
                    DeleteCommentButtonVisible = true;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 135 && up.Permission.IdGeosModule == 15))// Department Manager
                {
                    IsAddButtonEnabled = true;
                    DeleteCommentButtonVisible = true;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 136 && up.Permission.IdGeosModule == 15))// Base User
                {
                    IsAddButtonEnabled = true;
                    DeleteCommentButtonVisible = true;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 121 && up.Permission.IdGeosModule == 15))//View
                {
                    IsAddButtonEnabled = false;
                    DeleteCommentButtonVisible = false;
                }
            }
        }

        //[shweta.thube][GEOS2-6911]
        private void FillCustomerList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCustomerList ...", category: Category.Info, priority: Priority.Low);
                //APMService = new APMServiceController("localhost:6699");
                CustomerList = new ObservableCollection<APMCustomer>(APMService.GetCustomersWithSite_V2610());


                ClonedCustomerList = new List<APMCustomer>();
                foreach (APMCustomer item in CustomerList)
                {
                    ClonedCustomerList.Add((APMCustomer)item.Clone());
                }
                //APMService = new APMServiceController("localhost:6699");
                ActiveUserList = new ObservableCollection<APMCustomer>(APMService.GetActiveUserIdRegion_V2610((int)GeosApplication.Instance.ActiveUser.IdCompany));


                CustomerList = new ObservableCollection<APMCustomer>(ClonedCustomerList.Where(c => c.Region == ActiveUserList.FirstOrDefault().Region));
                //SelectedRegion = RegionList.FirstOrDefault(x => x.Caption == ActiveUserList.FirstOrDefault().Region);


                GeosApplication.Instance.Logger.Log("Method FillCustomerList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTFillCustomerListaskist() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomerList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void SelectedRegionCommandAction(object obj)
        {
            try
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
                //[shweta.thube][GEOS2-7218][23.07.2025]
                //SelectedRegion = obj as TileBarFilters;
                if (SelectedRegion != null)
                {
                    if (!string.IsNullOrEmpty(SelectedRegion.Caption))
                    {
                        TempCustomerList = ClonedCustomerList
                                  .Where(c => c.Region == SelectedRegion.Caption)
                                  .ToList();
                        CustomerList.Clear();

                        foreach (var customer in TempCustomerList)
                        {
                            CustomerList.Add(customer);
                        }
                        CustomerList = new ObservableCollection<APMCustomer>(CustomerList.OrderBy(ap => ap.Group));
                        TempCustomerList = null;
                    }
                    SelectedRegion = RegionList.FirstOrDefault(x => x.Caption == SelectedRegion.Caption);
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LocationListClosedCommandAction() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillRegionList()
        {
            try
            {
                RegionList = new List<TileBarFilters>();

                if (ClonedCustomerList != null && ClonedCustomerList.Count > 0)
                {
                    foreach (var item in ClonedCustomerList)
                    {
                        if (!RegionList.Any(Caption => Caption.Caption == item.Region) && item.Region != null)
                        {
                            RegionList.Add(new TileBarFilters()
                            {
                                Caption = item.Region,
                                Height = 25,
                                width = 60,
                                Id = item.IdRegion
                            });
                        }
                    }

                }

                RegionList = new List<TileBarFilters>(RegionList.OrderBy(item => item.Id));
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in FillRegionList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

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
                TableView detailView = (TableView)obj;
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
                    List<APMActionPlanTask> tempList = new List<APMActionPlanTask>(TaskList);
                    tempList.FirstOrDefault(x => x.IdActionPlanTask == selectedTask.IdActionPlanTask).ParticipantCount = taskParticipantsViewModel.ListParticipants.Count();
                    TaskList = new ObservableCollection<APMActionPlanTask>(tempList);
                    IsSave = true;

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

        //[shweta.thube][GEOS2-8063][27/05/2025]
        private void SendNotificationCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendNotificationCommandAction....", category: Category.Info, priority: Priority.Low);
                if (IsEmailIcon == true)
                {
                    SendEmailNotificationView SendEmailNotificationView = new SendEmailNotificationView();
                    SendEmailNotificationViewModel sendEmailNotificationViewModel = new SendEmailNotificationViewModel();
                    EventHandler handle = delegate { SendEmailNotificationView.Close(); };
                    sendEmailNotificationViewModel.RequestClose += handle;
                    sendEmailNotificationViewModel.Init(IdActionPlan, IdActionPlanResponsible);
                    SendEmailNotificationView.DataContext = sendEmailNotificationViewModel;
                    SendEmailNotificationView.ShowDialog();
                }
                else
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["EmailWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method SendNotificationCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method SendNotificationCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-7004]
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

                GridControl detailView1 = (GridControl)obj;
                TableView detailView = (TableView)detailView1.View;
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
                    //selectedSubTask.CommentsCount = subTaskCommentsViewModel.SubTaskCommentsList.Count();
                    //selectedSubTask.TaskLastComment = subTaskCommentsViewModel.SubTaskCommentsList.OrderByDescending(a => a.CreatedIn).FirstOrDefault().Comments;

                    SelectedTask = TaskList.FirstOrDefault(x => x.IdActionPlanTask == selectedSubTask.IdParent);
                    //var Subtask = SelectedActionPlanTask?.SubTaskList?.FirstOrDefault(t => t.IdActionPlanTask == selectedSubTask.IdActionPlanTask);
                    var index = SelectedTask.SubTaskList.FindIndex(x => x.IdActionPlanTask == selectedSubTask.IdActionPlanTask);
                    if (index >= 0)
                    {
                        var subtask = SelectedTask.SubTaskList[index];
                        subtask.CommentsCount = subTaskCommentsViewModel.SubTaskCommentsList.Count;
                        subtask.TaskLastComment = subTaskCommentsViewModel.SubTaskCommentsList.OrderByDescending(a => a.CreatedIn).FirstOrDefault().Comments;

                        SelectedTask.SubTaskList = new ObservableCollection<APMActionPlanSubTask>(SelectedTask.SubTaskList).ToList();
                    }
                    TaskList = new ObservableCollection<APMActionPlanTask>(TaskList);
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
        //[shweta.thube][GEOS2-7004]
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
                GridControl detailView1 = (GridControl)obj;
                TableView detailView = (TableView)detailView1.View;
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
                    SelectedTask = TaskList.FirstOrDefault(x => x.IdActionPlanTask == selectedSubTask.IdParent);
                    //var Subtask = SelectedActionPlanTask?.SubTaskList?.FirstOrDefault(t => t.IdActionPlanTask == selectedSubTask.IdActionPlanTask);
                    var index = SelectedTask.SubTaskList.FindIndex(x => x.IdActionPlanTask == selectedSubTask.IdActionPlanTask);
                    if (index >= 0)
                    {
                        var subtask = SelectedTask.SubTaskList[index];
                        subtask.FileCount = subTaskAttachmentsViewModel.ListAttachment.Count;
                        SelectedTask.SubTaskList = new ObservableCollection<APMActionPlanSubTask>(SelectedTask.SubTaskList).ToList();
                    }
                    //SelectedActionPlanSubTask = SelectedTask.SubTaskList.FirstOrDefault();
                    TaskList = new ObservableCollection<APMActionPlanTask>(TaskList);
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
        //[shweta.thube][GEOS2-7004]
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
                GridControl detailView1 = (GridControl)obj;
                TableView detailView = (TableView)detailView1.View;
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
                    SelectedTask = TaskList.FirstOrDefault(x => x.IdActionPlanTask == selectedSubTask.IdParent);
                    //var Subtask = SelectedActionPlanTask?.SubTaskList?.FirstOrDefault(t => t.IdActionPlanTask == selectedSubTask.IdActionPlanTask);
                    var index = SelectedTask.SubTaskList.FindIndex(x => x.IdActionPlanTask == selectedSubTask.IdActionPlanTask);
                    if (index >= 0)
                    {
                        var subtask = SelectedTask.SubTaskList[index];
                        subtask.ParticipantCount = subTaskParticipantsViewModel.ListParticipants.Count;
                        SelectedTask.SubTaskList = new ObservableCollection<APMActionPlanSubTask>(SelectedTask.SubTaskList).ToList();
                    }
                    //SelectedActionPlanSubTask = SelectedTask.SubTaskList.FirstOrDefault();
                    TaskList = new ObservableCollection<APMActionPlanTask>(TaskList);
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
        //[pallavi.kale][GEOS2-7003]
        private void AddActionPlanSubTaskViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddActionPlanSubTaskViewWindowShow()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                AddEditSubTaskViewModel addEditSubTaskViewModel = new AddEditSubTaskViewModel();
                AddEditSubTaskView addEditSubTaskView = new AddEditSubTaskView();
                EventHandler handle = delegate { addEditSubTaskView.Close(); };
                addEditSubTaskViewModel.RequestClose += handle;
                addEditSubTaskViewModel.IsNew = true;
                addEditSubTaskViewModel.IsAddEditActionPlan = true;
                addEditSubTaskViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddSubTaskHeader").ToString();
                List<APMActionPlan> temp = new List<APMActionPlan>();
                addEditSubTaskViewModel.Code = Code;
                SelectedTask.ActionPlanCode = Code;
                SelectedTask.IdSite = IdSiteCustomer;
                addEditSubTaskViewModel.Init(SelectedTask);
                addEditSubTaskViewModel.OtItemVisibility = Visibility.Collapsed;
                addEditSubTaskView.DataContext = addEditSubTaskViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addEditSubTaskView.ShowDialogWindow();

                if (addEditSubTaskViewModel.IsSave)
                {
                    if (SelectedTask.SubTaskList == null)
                    {
                        SelectedTask.SubTaskList = new List<APMActionPlanSubTask>();
                    }
                    List<APMActionPlanSubTask> taskList = new List<APMActionPlanSubTask>(SelectedTask.SubTaskList);
                    APMActionPlanSubTask task = new APMActionPlanSubTask();
                    task.IdActionPlan = addEditSubTaskViewModel.TempSubTask.IdActionPlan;
                    task.IdParent = addEditSubTaskViewModel.TempSubTask.IdParent;
                    task.IdActionPlanTask = addEditSubTaskViewModel.TempSubTask.IdActionPlanTask;
                    task.Code = addEditSubTaskViewModel.TempSubTask.Code;
                    task.Title = addEditSubTaskViewModel.TempSubTask.Title;
                    task.Description = addEditSubTaskViewModel.TempSubTask.Description;
                    task.IdCompany = addEditSubTaskViewModel.TempSubTask.IdCompany;
                    task.Location = addEditSubTaskViewModel.TempSubTask.Location;
                    task.IdEmployee = addEditSubTaskViewModel.TempSubTask.IdEmployee;
                    task.Responsible = addEditSubTaskViewModel.SelectedResponsible.FullName;
                    task.IdLookupStatus = addEditSubTaskViewModel.SelectedStatus.IdLookupValue;
                    task.Status = addEditSubTaskViewModel.SelectedStatus.Value;

                    task.IdLookupPriority = addEditSubTaskViewModel.SelectedPriority.IdLookupValue;
                    task.Priority = addEditSubTaskViewModel.SelectedPriority.Value;
                    task.IdLookupTheme = addEditSubTaskViewModel.SelectedTheme.IdLookupValue;
                    task.Theme = addEditSubTaskViewModel.SelectedTheme.Value;
                    if (addEditSubTaskViewModel.TempSubTask.IdDelegated != 0)
                    {
                        task.IdDelegated = addEditSubTaskViewModel.TempSubTask.IdDelegated;
                        task.DelegatedTo = addEditSubTaskViewModel.SelectedDelegatedto.FullName;
                    }
                    task.DueDate = addEditSubTaskViewModel.TempSubTask.DueDate;
                    task.OriginalDueDate = addEditSubTaskViewModel.TempSubTask.OriginalDueDate;
                    task.CloseDate = addEditSubTaskViewModel.TempSubTask.CloseDate;
                    task.OpenDate = addEditSubTaskViewModel.TempSubTask.OpenDate;
                    task.LastUpdated = addEditSubTaskViewModel.TempSubTask.LastUpdated;
                    task.IsSubTaskDeleted = true; //[shweta.thube][GEOS2-9910][16.10.2025]
                    task.ClosedBy = addEditSubTaskViewModel.TempSubTask.ClosedBy;


                    DateTime currentDate = DateTime.Now;
                    TimeSpan difference = currentDate.Date - task.DueDate.Date;
                    int dueDays = difference.Days;
                    task.DueDays = dueDays;
                    string dueColor = "";
                    if (currentDate > task.DueDate && currentDate <= task.DueDate.AddDays(2))
                    {
                        dueColor = "#008000";
                    }
                    else if (currentDate > task.DueDate.AddDays(2) && currentDate <= task.DueDate.AddDays(7))
                    {
                        dueColor = "#FFFF00";
                    }
                    else if (currentDate > task.DueDate.AddDays(7))
                    {
                        dueColor = "#FF0000";
                    }
                    task.DueColor = dueColor;
                    task.Progress = addEditSubTaskViewModel.TempSubTask.Progress;
                    task.SubTaskCode = addEditSubTaskViewModel.TempSubTask.SubTaskCode;//[shweta.thube][GEOS2-8985]
                    task.StatusHTMLColor = addEditSubTaskViewModel.SelectedStatus.HtmlColor;
                    task.IdOTItem = (Int32)addEditSubTaskViewModel.TempSubTask.IdOTItem;
                    task.CodeNumber = addEditSubTaskViewModel.TempSubTask.CodeNumber;
                    task.TransactionOperation = ModelBase.TransactionOperations.Add;
                    //[shweta.thube][GEOS2-8985]
                    if (AddEditSubTaskList != null && AddEditSubTaskList.Count > 0)
                    {
                        var addedTasks = AddEditSubTaskList
                            .Where(x => x.TransactionOperation == ModelBase.TransactionOperations.Add)
                            .ToList();

                        if (addedTasks != null && addedTasks.Count > 0)
                        {
                            int maxSubNumber = addedTasks
                                .Select(x =>
                                {
                                    if (!string.IsNullOrEmpty(x.SubTaskCode) && x.SubTaskCode.Contains("."))
                                    {
                                        var parts = x.SubTaskCode.Split('.');
                                        if (parts.Length == 2 && int.TryParse(parts[1], out int subNum))
                                            return subNum;
                                    }
                                    return 0;
                                })
                                .DefaultIfEmpty(0)
                                .Max();
                            string parentTaskNumber = SelectedTask.TaskNumber.ToString();
                            task.SubTaskCode = $"{parentTaskNumber}.{maxSubNumber + 1}";
                        }

                    }
                    taskList.Add(task);

                    SelectedTask.SubTaskList.Add(task);
                    TaskList = new ObservableCollection<APMActionPlanTask>(TaskList);
                    SelectedTask.SubTaskList = new List<APMActionPlanSubTask>(SelectedTask.SubTaskList);
                    if (AddEditSubTaskList == null)
                    {
                        AddEditSubTaskList = new List<APMActionPlanSubTask>();
                    }
                    if (!AddEditSubTaskList.Any(x => x == task))
                    {
                        AddEditSubTaskList.Add(task);
                    }
                    IsSaveChanges = true;

                    IsAddButtonEnabled = true;
                }
                GeosApplication.Instance.Logger.Log("Method AddActionPlanSubTaskViewWindowShow()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddActionPlanSubTaskViewWindowShow()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-8683]
        public void DeleteSubTaskCommandAction(object obj)
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteSubTasksDetailsMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    GridControl detailView1 = (GridControl)obj;
                    TableView detailView = (TableView)detailView1.View;
                    APMActionPlanSubTask selectedSubTask = (APMActionPlanSubTask)detailView.DataControl.CurrentItem;  //[shweta.thube][GEOS2-8985]
                    APMActionPlanSubTask Temp = selectedSubTask;
                    SelectedTask = TaskList.FirstOrDefault(x => x.IdActionPlanTask == Temp.IdParent);
                    if (NewTempSubTaskList == null)
                    {
                        NewTempSubTaskList = new List<APMActionPlanSubTask>();
                    }
                    if (!NewTempSubTaskList.Contains(SelectedActionPlanSubTask))
                    {
                        SelectedActionPlanSubTask.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        NewTempSubTaskList.Add(SelectedActionPlanSubTask);
                    }
                    SelectedTask.SubTaskList.Remove(SelectedActionPlanSubTask);
                    //[shweta.thube][GEOS2-8985]
                    if (SelectedTask.SubTaskList.Count == 0)
                    {
                        SelectedTask.SubTaskList = null;
                    }
                    TaskList = new ObservableCollection<APMActionPlanTask>(TaskList);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SubTaskDetailsDeletedsuccessfully").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
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
        //[pallavi.kale][GEOS2-7003]
        private void EditSubTaskHyperLinkCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditSubTaskHyperLinkCommandAction....", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = (GridControl)obj;
                TableView detailView = (TableView)gridControl.View;
                APMActionPlanSubTask SelectedActionPlanSubTask = (APMActionPlanSubTask)detailView.DataControl.CurrentItem;
                AddEditSubTaskView addEditSubTaskView = new AddEditSubTaskView();
                AddEditSubTaskViewModel addEditSubTaskViewModel = new AddEditSubTaskViewModel();
                EventHandler handle = delegate { addEditSubTaskView.Close(); };
                addEditSubTaskViewModel.RequestClose += handle;
                addEditSubTaskViewModel.IsNew = false;
                addEditSubTaskViewModel.IsAddEditActionPlan = true;
                addEditSubTaskViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditSubTaskHeader").ToString();
                SelectedActionPlanSubTask.Code = Code;
                SelectedActionPlanSubTask.IdActionPlanResponsible = InactiveSelectedResponsibleToIdEmployee;
                SelectedActionPlanSubTask.ActionPlanResponsibleIdUser = APMCommon.Instance.ResponsibleList[SelectedResponsibleIndex].IdUser;
                SelectedActionPlanSubTask.IdSite = IdSiteCustomer;
                if (SelectedCustomer != null)
                {
                    SelectedActionPlanSubTask.CustomerName = SelectedCustomer.GroupName;
                }
                SelectedActionPlanSubTask.IdActionPlanLocation = (Int32)IdResponsibleLocation;
                addEditSubTaskViewModel.OtItemVisibility = Visibility.Collapsed;
                PreviousIdParent = SelectedActionPlanSubTask.IdParent;
                addEditSubTaskViewModel.EditInit(SelectedActionPlanSubTask);
                addEditSubTaskView.DataContext = addEditSubTaskViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addEditSubTaskView.Owner = Window.GetWindow(ownerInfo);

                addEditSubTaskView.ShowDialog();
                if (!addEditSubTaskViewModel.IsNew)
                {
                    if (addEditSubTaskViewModel.IsSave)
                    {
                        var oldParentId = SelectedActionPlanSubTask.IdParent;
                        SelectedActionPlanSubTask.IdActionPlan = addEditSubTaskViewModel.UpdatedSubTask.IdActionPlan;
                        SelectedActionPlanSubTask.IdActionPlanTask = addEditSubTaskViewModel.UpdatedSubTask.IdActionPlanTask;
                        SelectedActionPlanSubTask.IdParent = addEditSubTaskViewModel.UpdatedSubTask.IdParent;
                        if (!string.IsNullOrEmpty(addEditSubTaskViewModel.UpdatedSubTask.Code))
                        {
                            SelectedActionPlanSubTask.Code = addEditSubTaskViewModel.UpdatedSubTask.Code;
                        }
                        else
                        {
                            SelectedActionPlanSubTask.Code = Code;
                        }

                        SelectedActionPlanSubTask.Title = addEditSubTaskViewModel.UpdatedSubTask.Title;
                        SelectedActionPlanSubTask.IdCompany = addEditSubTaskViewModel.UpdatedSubTask.IdCompany;
                        SelectedActionPlanSubTask.IdEmployee = addEditSubTaskViewModel.UpdatedSubTask.IdEmployee;
                        SelectedActionPlanSubTask.Responsible = addEditSubTaskViewModel.SelectedResponsible.FullName;

                        if (addEditSubTaskViewModel.UpdatedSubTask.Progress == 100)
                        {
                            SelectedActionPlanSubTask.Status = "Done";
                            SelectedActionPlanSubTask.StatusHTMLColor = "#92D050";
                            SelectedActionPlanSubTask.IdLookupStatus = 1982;
                        }
                        else
                        {
                            SelectedActionPlanSubTask.Status = addEditSubTaskViewModel.SelectedStatus.Value;
                            SelectedActionPlanSubTask.StatusHTMLColor = addEditSubTaskViewModel.SelectedStatus.HtmlColor;
                            SelectedActionPlanSubTask.IdLookupStatus = addEditSubTaskViewModel.SelectedStatus.IdLookupValue;
                        }

                        SelectedActionPlanSubTask.IdLookupPriority = addEditSubTaskViewModel.SelectedPriority.IdLookupValue;
                        SelectedActionPlanSubTask.Priority = addEditSubTaskViewModel.SelectedPriority.Value;

                        SelectedActionPlanSubTask.IdLookupTheme = addEditSubTaskViewModel.SelectedTheme.IdLookupValue;
                        SelectedActionPlanSubTask.Theme = addEditSubTaskViewModel.SelectedTheme.Value;
                        if (addEditSubTaskViewModel.SelectedDelegatedto != null)
                        {
                            SelectedActionPlanSubTask.IdDelegated = addEditSubTaskViewModel.UpdatedSubTask.IdDelegated;
                            SelectedActionPlanSubTask.DelegatedTo = addEditSubTaskViewModel.SelectedDelegatedto.FullName;
                        }

                        SelectedActionPlanSubTask.DueDate = addEditSubTaskViewModel.UpdatedSubTask.DueDate;

                        if (addEditSubTaskViewModel.PreviousDueDate != SelectedActionPlanSubTask.DueDate)
                        {
                            SelectedActionPlanSubTask.IsShowIcon = true;
                        }
                        else
                        {
                            SelectedActionPlanSubTask.IsShowIcon = false;
                        }
                        SelectedActionPlanSubTask.CloseDate = addEditSubTaskViewModel.UpdatedSubTask.CloseDate;
                        SelectedActionPlanSubTask.ChangeCount = addEditSubTaskViewModel.UpdatedSubTask.ChangeCount;
                        SelectedActionPlanSubTask.LastUpdated = addEditSubTaskViewModel.UpdatedSubTask.LastUpdated;
                        DateTime currentDate = DateTime.Now;
                        TimeSpan difference = currentDate.Date - SelectedActionPlanSubTask.DueDate;
                        int dueDays = difference.Days;
                        SelectedActionPlanSubTask.DueDays = dueDays;
                        string dueColor = "";
                        if (currentDate > SelectedActionPlanSubTask.DueDate && currentDate <= SelectedActionPlanSubTask.DueDate.AddDays(2))
                        {
                            dueColor = "#008000";
                        }
                        else if (currentDate > SelectedActionPlanSubTask.DueDate.AddDays(2) && currentDate <= SelectedActionPlanSubTask.DueDate.AddDays(7))
                        {
                            dueColor = "#FFFF00";
                        }
                        else if (currentDate > SelectedActionPlanSubTask.DueDate.AddDays(7))
                        {
                            dueColor = "#FF0000";
                        }
                        SelectedActionPlanSubTask.DueColor = dueColor;
                        SelectedActionPlanSubTask.Progress = addEditSubTaskViewModel.UpdatedSubTask.Progress;

                        SelectedActionPlanSubTask.Description = addEditSubTaskViewModel.UpdatedSubTask.Description;
                        SelectedActionPlanSubTask.ClosedBy = addEditSubTaskViewModel.UpdatedSubTask.ClosedBy;
                        SelectedActionPlanSubTask.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        SelectedActionPlanSubTask.CodeNumber = addEditSubTaskViewModel.UpdatedSubTask.CodeNumber;
                        if (!string.IsNullOrEmpty(addEditSubTaskViewModel.UpdatedSubTask.TaskStatusComment))
                        {
                            SelectedActionPlanSubTask.TaskStatusComment = addEditSubTaskViewModel.UpdatedSubTask.TaskStatusComment;
                            SelectedActionPlanSubTask.CommentsCount += 1;
                            SelectedActionPlanSubTask.TaskLastComment = addEditSubTaskViewModel.UpdatedSubTask.TaskStatusComment;
                        }
                        if (addEditSubTaskViewModel.UpdatedSubTask.TaskAttachmentList != null)
                        {
                            addEditSubTaskViewModel.UpdatedSubTask.TaskAttachmentList.ForEach(x => x.Description = addEditSubTaskViewModel.UpdatedSubTask.TaskStatusDescription);
                            if (SelectedActionPlanSubTask.TaskAttachmentList == null)
                            {
                                SelectedActionPlanSubTask.TaskAttachmentList = new List<AttachmentsByTask>();
                            }
                            SelectedActionPlanSubTask.TaskAttachmentList.AddRange(addEditSubTaskViewModel.UpdatedSubTask.TaskAttachmentList);
                            SelectedActionPlanSubTask.FileCount = SelectedActionPlanSubTask.TaskAttachmentList.Count();
                            SelectedActionPlanSubTask.TaskStatusDescription = addEditSubTaskViewModel.UpdatedSubTask.TaskStatusDescription;
                        }
                        SelectedActionPlanSubTask.IdOTItem = addEditSubTaskViewModel.UpdatedSubTask.IdOTItem;

                        if (AddEditSubTaskList == null)
                        {
                            AddEditSubTaskList = new List<APMActionPlanSubTask>();
                        }
                        if (!AddEditSubTaskList.Any(x => x == SelectedActionPlanSubTask))
                        {
                            AddEditSubTaskList.Add(SelectedActionPlanSubTask);
                        }
                        else
                        {
                            AddEditSubTaskList.Remove(SelectedActionPlanSubTask);
                            AddEditSubTaskList.Add(SelectedActionPlanSubTask);
                        }

                        var previousParentTask = TaskList.FirstOrDefault(t => t.IdActionPlanTask == oldParentId);
                        if (previousParentTask?.SubTaskList != null)
                        {
                            var existingTask = previousParentTask.SubTaskList.FirstOrDefault(x => x.IdActionPlanTask == SelectedActionPlanSubTask.IdActionPlanTask);
                            if (existingTask != null)
                            {
                                previousParentTask.SubTaskList.Remove(existingTask);
                                previousParentTask.SubTaskList = new List<APMActionPlanSubTask>(previousParentTask.SubTaskList);
                                AddEditSubTaskList.Remove(existingTask);
                            }
                        }

                        var parentTask = TaskList.FirstOrDefault(t => t.IdActionPlanTask == SelectedActionPlanSubTask.IdParent);
                        if (parentTask != null)
                        {
                            if (parentTask.SubTaskList == null)
                                parentTask.SubTaskList = new List<APMActionPlanSubTask>();

                            if (parentTask.SubTaskList.Any(x => x.TaskNumber == SelectedActionPlanSubTask.TaskNumber))
                            {
                                SelectedActionPlanSubTask.TaskNumber = parentTask.SubTaskList.Max(x => x.TaskNumber) + 1;
                            }
                            else if (parentTask.SubTaskList.Count == 0)
                            {
                                SelectedActionPlanSubTask.TaskNumber = 1;
                            }
                            SelectedActionPlanSubTask.SubTaskCode = $"{parentTask.TaskNumber}.{SelectedActionPlanSubTask.TaskNumber}";
                            SelectedActionPlanSubTask.ParentTaskNumber = parentTask.TaskNumber;
                            parentTask.SubTaskList.Add(SelectedActionPlanSubTask);
                            parentTask.SubTaskList = parentTask.SubTaskList.OrderBy(x => x.TaskNumber).ToList();
                            parentTask.SubTaskList = new List<APMActionPlanSubTask>(parentTask.SubTaskList);
                        }
                        SelectedActionPlanSubTask.TransactionOperation = ModelBase.TransactionOperations.Update;

                        if (!AddEditSubTaskList.Any(x => x == SelectedActionPlanSubTask))
                        {
                            AddEditSubTaskList.Add(SelectedActionPlanSubTask);
                        }
                        else
                        {
                            AddEditSubTaskList.Remove(SelectedActionPlanSubTask);
                            AddEditSubTaskList.Add(SelectedActionPlanSubTask);
                        }

                        TaskList = new ObservableCollection<APMActionPlanTask>(TaskList);
                        IsSaveChanges = true;

                    }
                }
                else
                {
                    if (addEditSubTaskViewModel.IsSave)
                    {
                        var idTask = TaskList.FirstOrDefault(t => t.IdActionPlanTask == SelectedActionPlanSubTask.IdParent);
                        if (idTask?.SubTaskList == null)
                        {
                            idTask.SubTaskList = new List<APMActionPlanSubTask>();
                        }

                        List<APMActionPlanSubTask> subTaskList = new List<APMActionPlanSubTask>(idTask.SubTaskList);
                        APMActionPlanSubTask subTask = new APMActionPlanSubTask();
                        subTask.IdActionPlan = addEditSubTaskViewModel.TempSubTask.IdActionPlan;
                        subTask.IdParent = addEditSubTaskViewModel.TempSubTask.IdParent;
                        subTask.IdActionPlanTask = addEditSubTaskViewModel.TempSubTask.IdActionPlanTask;
                        subTask.Code = addEditSubTaskViewModel.TempSubTask.Code;
                        subTask.Title = addEditSubTaskViewModel.TempSubTask.Title;
                        subTask.Description = addEditSubTaskViewModel.TempSubTask.Description;
                        subTask.IdCompany = addEditSubTaskViewModel.TempSubTask.IdCompany;
                        subTask.Location = addEditSubTaskViewModel.TempSubTask.Location;
                        subTask.IdEmployee = addEditSubTaskViewModel.TempSubTask.IdEmployee;
                        subTask.Responsible = addEditSubTaskViewModel.SelectedResponsible.FullName;
                        subTask.IdLookupStatus = addEditSubTaskViewModel.SelectedStatus.IdLookupValue;
                        subTask.Status = addEditSubTaskViewModel.SelectedStatus.Value;

                        subTask.IdLookupPriority = addEditSubTaskViewModel.SelectedPriority.IdLookupValue;
                        subTask.Priority = addEditSubTaskViewModel.SelectedPriority.Value;

                        subTask.IdLookupTheme = addEditSubTaskViewModel.SelectedTheme.IdLookupValue;
                        subTask.Theme = addEditSubTaskViewModel.SelectedTheme.Value;
                        if (addEditSubTaskViewModel.TempSubTask.IdDelegated != 0)
                        {
                            subTask.IdDelegated = addEditSubTaskViewModel.TempSubTask.IdDelegated;
                            subTask.DelegatedTo = addEditSubTaskViewModel.SelectedDelegatedto.FullName;
                        }
                        subTask.DueDate = addEditSubTaskViewModel.TempSubTask.DueDate;
                        subTask.CloseDate = addEditSubTaskViewModel.TempSubTask.CloseDate;
                        subTask.OpenDate = addEditSubTaskViewModel.TempSubTask.OpenDate;
                        subTask.LastUpdated = addEditSubTaskViewModel.TempSubTask.LastUpdated;

                        subTask.ClosedBy = addEditSubTaskViewModel.TempSubTask.ClosedBy;


                        DateTime currentDate = DateTime.Now;
                        TimeSpan difference = currentDate.Date - subTask.DueDate.Date;
                        int dueDays = difference.Days;
                        subTask.DueDays = dueDays;
                        string dueColor = "";
                        if (currentDate > subTask.DueDate && currentDate <= subTask.DueDate.AddDays(2))
                        {
                            dueColor = "#008000";
                        }
                        else if (currentDate > subTask.DueDate.AddDays(2) && currentDate <= subTask.DueDate.AddDays(7))
                        {
                            dueColor = "#FFFF00";
                        }
                        else if (currentDate > subTask.DueDate.AddDays(7))
                        {
                            dueColor = "#FF0000";
                        }
                        subTask.DueColor = dueColor;
                        subTask.StatusHTMLColor = addEditSubTaskViewModel.SelectedStatus.HtmlColor;
                        subTask.IdOTItem = (Int32)addEditSubTaskViewModel.TempSubTask.IdOTItem;
                        subTask.CodeNumber = addEditSubTaskViewModel.TempSubTask.CodeNumber;
                        subTask.TransactionOperation = ModelBase.TransactionOperations.Add;

                        if (idTask?.SubTaskList != null)
                        {
                            var existing = idTask.SubTaskList.FirstOrDefault(x => x.IdActionPlanTask == subTask.IdActionPlanTask);
                            if (existing != null)
                                subTask.TaskNumber = existing.TaskNumber;
                            else
                                subTask.TaskNumber = idTask.SubTaskList.Any() ? idTask.SubTaskList.Max(x => x.TaskNumber) + 1 : 1;
                        }
                        else
                        {
                            subTask.TaskNumber = 1;
                        }

                        subTask.SubTaskCode = $"{idTask?.TaskNumber}.{subTask.TaskNumber}";
                        SelectedActionPlanSubTask = subTask;
                        foreach (var t in TaskList)
                        {
                            if (t.SubTaskList != null)
                            {
                                var existing = t.SubTaskList.FirstOrDefault(x => x.IdActionPlanTask == subTask.IdActionPlanTask);
                                if (existing != null)
                                    t.SubTaskList.Remove(existing);
                            }
                        }

                        if (idTask != null)
                        {
                            if (idTask.SubTaskList == null)
                                idTask.SubTaskList = new List<APMActionPlanSubTask>();
                            idTask.SubTaskList.Add(subTask);
                            idTask.SubTaskList = idTask.SubTaskList.OrderBy(x => x.TaskNumber).ToList();
                        }

                        var existingInSubTaskList = subTaskList.FirstOrDefault(x => x.IdActionPlanTask == subTask.IdActionPlanTask);
                        if (existingInSubTaskList != null)
                            subTaskList.Remove(existingInSubTaskList);
                        subTaskList.Add(subTask);
                        idTask.SubTaskList = subTaskList.OrderBy(x => x.TaskNumber).ToList();

                        if (AddEditSubTaskList == null)
                            AddEditSubTaskList = new List<APMActionPlanSubTask>();
                        var existingInAddEdit = AddEditSubTaskList.FirstOrDefault(x => x.IdActionPlanTask == subTask.IdActionPlanTask);
                        if (existingInAddEdit != null)
                            AddEditSubTaskList.Remove(existingInAddEdit);
                        AddEditSubTaskList.Add(subTask);

                        TaskList = new ObservableCollection<APMActionPlanTask>(TaskList);
                        IsSaveChanges = true;

                        IsAddButtonEnabled = true;
                    }
                }


                GeosApplication.Instance.Logger.Log("Method EditSubTaskHyperLinkCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditSubTaskHyperLinkCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[pallavi.kale][GEOS2-8084][29.08.2025]
        private void ExportCustomerButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportCustomerButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                var currentActionPlan = APMCommon.Instance.ActionPlanList.FirstOrDefault(a => a.IdActionPlan == IdActionPlan);

                if (currentActionPlan == null)
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
                    FileName = "ETM_" + currentActionPlan.Code,
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
                        }, x => new SplashScreenView() { DataContext = new SplashScreenViewModel() }, null, null);
                    }
                    ResultFileName = saveFile.FileName;
                    GridExportService.Export(ResultFileName);//[rdixit][GEOS2-9316][26.08.2025]

                    Workbook mainWorkbook = new Workbook();
                    mainWorkbook.LoadDocument(ResultFileName);

                    byte[] templateBytes = APMService.GetActionPlanExcel();
                    if (templateBytes == null || templateBytes.Length == 0)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                            DXSplashScreen.Close();
                        string templatePath = APMService.GetAPMTemplateFilePath("APMExcelTemplate.xlsx");
                        CustomMessageBox.Show(string.Format(Application.Current.Resources["APMActionPlanTemplateError"].ToString()) + " : " + templatePath, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                        IsBusy = false;
                        return;

                    }

                    Workbook templateWorkbook = new Workbook();
                    using (MemoryStream ms = new MemoryStream(templateBytes))
                    {
                        templateWorkbook.LoadDocument(ms, DocumentFormat.OpenXml);
                    }

                    Worksheet templateSheet = templateWorkbook.Worksheets[0];
                    Worksheet sheet = mainWorkbook.Worksheets[0];
                    sheet.Rows.Insert(0, 3);
                    sheet.CopyFrom(templateSheet);

                    CultureInfo cultureInfo = CultureInfo.CurrentCulture;
                    CalendarWeekRule weekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
                    DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
                    int weekNumber = cultureInfo.Calendar.GetWeekOfYear(DateTime.Now, weekRule, firstDayOfWeek);
                    string YearWeek = DateTime.Now.Year + "CW" + weekNumber;

                    sheet.Cells["B7"].Value = currentActionPlan.Code;
                    sheet.Cells["C7"].Value = currentActionPlan.Location;
                    sheet.Cells["D7"].Value = currentActionPlan.Description;
                    sheet.Cells["E7"].Value = currentActionPlan.Origin;
                    sheet.Cells["F7"].Value = currentActionPlan.BusinessUnit;
                    sheet.Cells["G7"].Value = currentActionPlan.Department;
                    sheet.Cells["I7"].Value = currentActionPlan.FullName;
                    sheet.Cells["K7"].Value = currentActionPlan.GroupName;
                    sheet.Cells["L7"].Value = currentActionPlan.Site;
                    sheet.Cells["N7"].Value = currentActionPlan.CreatedIn?.ToShortDateString();
                    sheet.Cells["N3"].Value = YearWeek;
                    sheet.Cells["N2"].Value = DateTime.Now;

                    var concreteService = GridExportService as GridExportService;
                    var viewToExport = concreteService?.GetView();
                    var gridTasks = (viewToExport?.DataControl.ItemsSource as IEnumerable<APMActionPlanTask>)?.ToList();

                    var allTasks = gridTasks?.Where(t => IncludeTaskForExport(t)).ToList()
                                   ?? currentActionPlan.TaskList?.Where(t => IncludeTaskForExport(t)).ToList()
                                   ?? new List<APMActionPlanTask>();



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
                        var taskRange = sheet.Range.FromLTRB(1, 9, 13, startRow - 1);
                        taskRange.Alignment.WrapText = true;
                    }

                    mainWorkbook.SaveDocument(ResultFileName, DocumentFormat.OpenXml);
                    System.Diagnostics.Process.Start(ResultFileName);

                    IsBusy = false;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                        DXSplashScreen.Close();

                    GeosApplication.Instance.Logger.Log("ExportCustomerButtonCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    DXSplashScreen.Close();

                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Error in ExportCustomerButtonCommandAction(): " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        private bool IncludeTaskForExport(APMActionPlanTask task)
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
        //[pallavi.kale][GEOS2-8084][29.08.2025]
        private void ExportButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                var currentActionPlan = APMCommon.Instance.ActionPlanList.FirstOrDefault(a => a.IdActionPlan == IdActionPlan);
                if (currentActionPlan == null)
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
                    FileName = "ETM_" + currentActionPlan.Code,
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

                    GridExportService.Export(ResultFileName);
                    var workbook = new DevExpress.Spreadsheet.Workbook();
                    workbook.LoadDocument(ResultFileName);
                    var sheet = workbook.Worksheets[0];
                    sheet.Rows.Insert(0, 3); 

                    string[,] comboHeadersAndValues = new string[,]
                    {
            { (string)Application.Current.FindResource("APMActionPlansCodeHeader"), currentActionPlan.Code },
            { (string)Application.Current.FindResource("APMActionPlansTitleHeader"), currentActionPlan.Description },
            { (string)Application.Current.FindResource("APMActionPlansLocationHeader"), currentActionPlan.Location },
            { (string)Application.Current.FindResource("APMActionPlansPersonHeader"), currentActionPlan.FullName },
            { (string)Application.Current.FindResource("APMActionPlansBusinessUnitHeader"), currentActionPlan.BusinessUnit },
            { (string)Application.Current.FindResource("APMActionPlansCustomerHeader"), currentActionPlan.GroupName },
            { (string)Application.Current.FindResource("ActionPlanChildCreatedByHeader"), currentActionPlan.CreatedByName },
            { (string)Application.Current.FindResource("ActionPlanChildCreationDateHeader"), currentActionPlan.CreatedIn?.ToString("dd/MM/yyyy") },
            { (string)Application.Current.FindResource("APMActionPlansDepartmentHeader"), currentActionPlan.Department },
            { (string)Application.Current.FindResource("APMActionPlansOriginHeader"), currentActionPlan.Origin },
            { (string)Application.Current.FindResource("APMOriginDescriptionHeader"), currentActionPlan.OriginDescription }
                    };

                    int cols = comboHeadersAndValues.GetLength(0);

                    for (int c = 0; c < cols; c++)
                    {
                        var cell = sheet.Cells[0, c];
                        cell.Value = comboHeadersAndValues[c, 0];
                        cell.Alignment.Horizontal = DevExpress.Spreadsheet.SpreadsheetHorizontalAlignment.Left; 
                    }

                    for (int c = 0; c < cols; c++)
                    {
                        var cell = sheet.Cells[1, c];
                        cell.Value = comboHeadersAndValues[c, 1];
                        cell.Alignment.Horizontal = DevExpress.Spreadsheet.SpreadsheetHorizontalAlignment.Left; 
                    }

                    var comboHeaderRange = sheet.Range.FromLTRB(0, 0, cols - 1, 0);
                    comboHeaderRange.Fill.BackgroundColor = System.Drawing.Color.LightGray;
                    comboHeaderRange.Font.Bold = true;
                    comboHeaderRange.Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    var comboValueRange = sheet.Range.FromLTRB(0, 1, cols - 1, 1);
                    comboValueRange.Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                    for (int c = 0; c < cols; c++)
                        sheet.Columns[c].AutoFit();

                    var headerRowIndex = 3;
                    var taskHeaderRange = sheet.Range.FromLTRB(0, headerRowIndex, sheet.GetUsedRange().RightColumnIndex, headerRowIndex);
                    taskHeaderRange.Fill.BackgroundColor = System.Drawing.Color.LightGray;
                    taskHeaderRange.Font.Bold = true;

                    workbook.SaveDocument(ResultFileName);
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }

                    System.Diagnostics.Process.Start(ResultFileName);
                    IsBusy = false;

                    GeosApplication.Instance.Logger.Log("ExportButtonCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }

                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Error in ExportButtonCommandAction(): " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[pallavi.kale][GEOS2-8084][29.08.2025]
        private void PrintButtonCommandAction(object obj)
        {
            try
            {
                IsBusy = true;
                GeosApplication.Instance.Logger.Log("PrintButtonCommandAction() started...", Category.Info, Priority.Low);

                var currentActionPlan = APMCommon.Instance.ActionPlanList.FirstOrDefault(a => a.IdActionPlan == IdActionPlan);
                if (currentActionPlan == null)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                        DXSplashScreen.Close();

                    CustomMessageBox.Show(
                            string.Format(Application.Current.Resources["APMActionPlanViewEmptyError"].ToString()),
                            "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    IsBusy = false;
                    return;

                }
                //APMService = new APMServiceController("localhost:6699");
                byte[] templateBytes = APMService.GetEditActionPlanExcel();
                if (templateBytes == null || templateBytes.Length == 0)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                        DXSplashScreen.Close();
                    string templatePath = APMService.GetAPMTemplateFilePath("APMStandardPrintTemplate.xlsx");
                    CustomMessageBox.Show(string.Format(Application.Current.Resources["APMActionPlanTemplateError"].ToString()) + " : " + templatePath, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                    IsBusy = false;
                    return;

                }
                Workbook templateWorkbook = new Workbook();
                using (MemoryStream ms = new MemoryStream(templateBytes))
                {
                    templateWorkbook.LoadDocument(ms, DocumentFormat.OpenXml);
                }

                Worksheet sheet = templateWorkbook.Worksheets[0];

                sheet.Cells["A5"].Value = currentActionPlan.Code;              
                sheet.Cells["D5"].Value = currentActionPlan.Description;       
                sheet.Cells["F5"].Value = currentActionPlan.Location;         
                sheet.Cells["G5"].Value = currentActionPlan.ActionPlanResponsibleDisplayName; 
                sheet.Cells["I5"].Value = currentActionPlan.Origin;           
                sheet.Cells["M5"].Value = currentActionPlan.BusinessUnit;      
                sheet.Cells["P5"].Value = currentActionPlan.Department;        
                sheet.Cells["U5"].Value = currentActionPlan.GroupName;            
                sheet.Cells["Y5"].Value = currentActionPlan.TotalActionItems; 
                sheet.Cells["Z5"].Value = currentActionPlan.TotalOpenItems;    
                sheet.Cells["AC5"].Value = currentActionPlan.TotalClosedItems; 
                sheet.Cells["AF5"].Value = currentActionPlan.Percentage + "%"; 

                var concreteService = GridExportService as GridExportService;
                var viewToExport = concreteService?.GetView();
                var gridTasks = (viewToExport?.DataControl.ItemsSource as IEnumerable<APMActionPlanTask>)?.ToList();

                var allTasks = gridTasks?.ToList()?? currentActionPlan.TaskList?.ToList() ?? new List<APMActionPlanTask>();

                int startRow = 8;
                
                string[] numericColumns = { "S", "U", "Y" }; 
                string[] dateColumns = { "N", "O", "Q", "W", "AA" }; 

                foreach (var col in numericColumns)
                {
                    sheet.Columns[col].NumberFormat = "0"; 
                }

                foreach (var col in dateColumns)
                {
                    sheet.Columns[col].NumberFormat = "dd-MM-yyyy";
                }

                foreach (var task in allTasks)
                {
                    sheet.Cells["B" + startRow].Value = task.TaskNumber;            
                    sheet.Cells["C" + startRow].Value = task.Title;                 
                    sheet.Cells["E" + startRow].Value = task.Description;            
                    sheet.Cells["H" + startRow].Value = task.Responsible;            
                    sheet.Cells["J" + startRow].Value = task.Status;         
                    sheet.Cells["K" + startRow].Value = task.Priority;               
                    sheet.Cells["L" + startRow].Value = task.Theme;                  
                    sheet.Cells["N" + startRow].Value = task.OpenDate?.ToShortDateString();
                    sheet.Cells["O" + startRow].Value = task.OriginalDueDate?.ToShortDateString(); 
                    sheet.Cells["Q" + startRow].Value = task.DueDate.ToShortDateString();       
                    sheet.Cells["S" + startRow].Value = task.Duration;             
                    sheet.Cells["U" + startRow].Value = task.DueDays;              
                    sheet.Cells["W" + startRow].Value = task.LastUpdated?.ToShortDateString(); 
                    sheet.Cells["Y" + startRow].Value = task.ChangeCount;           
                    sheet.Cells["AA" + startRow].Value = task.CreatedIn?.ToShortDateString(); 
                    sheet.Cells["AE" + startRow].Value = task.DelegatedTo;          
                    sheet.Cells["AI" + startRow].Value = task.Progress + "%";       

                    startRow++;
                }

                if (allTasks.Any())
                {
                    for (int row = 8; row < startRow; row++)
                    {
                        var rowRange = sheet.Rows[row];
                        rowRange.Alignment.WrapText = true;                  
                        rowRange.Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                        rowRange.AutoFit();                                

                      
                    }
                }

                string pdfFile = Path.Combine(Path.GetTempPath(), "ETM_" + currentActionPlan.Code + ".pdf");
                templateWorkbook.ExportToPdf(pdfFile);
                System.Diagnostics.Process.Start(new ProcessStartInfo(pdfFile) { UseShellExecute = true });
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("PrintCustomerButtonCommandAction() completed successfully", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Error in PrintButtonCommandAction(): " + ex.Message, Category.Exception, Priority.Low);
            }

        }

        //[pallavi.kale][GEOS2-8084][29.08.2025]
        private void PrintCustomerButtonCommandAction(object obj)
        {
            try
            {
                IsBusy = true;
                GeosApplication.Instance.Logger.Log("PrintCustomerButtonCommandAction() started...", Category.Info, Priority.Low);

                var currentActionPlan = APMCommon.Instance.ActionPlanList
                    .FirstOrDefault(a => a.IdActionPlan == IdActionPlan);
                if (currentActionPlan == null)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                        DXSplashScreen.Close();

                    CustomMessageBox.Show(
                            string.Format(Application.Current.Resources["APMActionPlanViewEmptyError"].ToString()),
                            "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                    IsBusy = false;
                    return;

                }

                byte[] templateBytes = APMService.GetActionPlanExcel();
                if (templateBytes == null || templateBytes.Length == 0)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                        DXSplashScreen.Close();
                    string templatePath = APMService.GetAPMTemplateFilePath("APMExcelTemplate.xlsx");
                    CustomMessageBox.Show(string.Format(Application.Current.Resources["APMActionPlanTemplateError"].ToString()) + " : " + templatePath, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                    IsBusy = false;
                    return;

                }

                Workbook templateWorkbook = new Workbook();
                using (MemoryStream ms = new MemoryStream(templateBytes))
                {
                    templateWorkbook.LoadDocument(ms, DocumentFormat.OpenXml);
                }
                Worksheet templateSheet = templateWorkbook.Worksheets[0];

                Workbook mainWorkbook = new Workbook();
                mainWorkbook.CreateNewDocument();
                Worksheet sheet = mainWorkbook.Worksheets[0];
                sheet.CopyFrom(templateSheet);

                var cultureInfo = CultureInfo.CurrentCulture;
                var calendar = cultureInfo.Calendar;
                int weekNumber = calendar.GetWeekOfYear(DateTime.Now, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek);
                string YearWeek = DateTime.Now.Year + "CW" + weekNumber;

                sheet.Cells["B7"].Value = currentActionPlan.Code;
                sheet.Cells["C7"].Value = currentActionPlan.Location;
                sheet.Cells["D7"].Value = currentActionPlan.Description;
                sheet.Cells["E7"].Value = currentActionPlan.Origin;
                sheet.Cells["F7"].Value = currentActionPlan.BusinessUnit;
                sheet.Cells["G7"].Value = currentActionPlan.Department;
                sheet.Cells["I7"].Value = currentActionPlan.FullName;
                sheet.Cells["K7"].Value = currentActionPlan.GroupName;
                sheet.Cells["L7"].Value = currentActionPlan.Site;
                sheet.Cells["N7"].Value = currentActionPlan.CreatedIn?.ToShortDateString();
                sheet.Cells["N3"].Value = YearWeek;
                sheet.Cells["N2"].Value = DateTime.Now;

                var concreteService = GridExportService as GridExportService;
                var viewToExport = concreteService?.GetView();
                var gridTasks = (viewToExport?.DataControl.ItemsSource as IEnumerable<APMActionPlanTask>)?.ToList();

                var allTasks = gridTasks?.Where(t => IncludeTaskForExport(t)).ToList()
                               ?? currentActionPlan.TaskList?.Where(t => IncludeTaskForExport(t)).ToList()
                               ?? new List<APMActionPlanTask>();

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
                    var taskRange = sheet.Range.FromLTRB(1, 9, 13, startRow - 1);
                    taskRange.Alignment.WrapText = true;
                }

                string pdfFile = Path.Combine(Path.GetTempPath(), "ETM_" + currentActionPlan.Code + ".pdf");
                mainWorkbook.ExportToPdf(pdfFile);

                System.Diagnostics.Process.Start(new ProcessStartInfo(pdfFile) { UseShellExecute = true });

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("PrintCustomerButtonCommandAction() completed successfully", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Error in PrintCustomerButtonCommandAction(): " + ex.Message, Category.Exception, Priority.Low);
            }
        }


        #endregion

        #region Validation
        bool allowValidation = false;
        private string parameter;

        string EnableValidationAndGetError()
        {
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
                return error;
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                                me[BindableBase.GetPropertyName(() => SelectedLocationIndex)] +
                                me[BindableBase.GetPropertyName(() => SelectedResponsibleIndex)] +
                                 me[BindableBase.GetPropertyName(() => SelectedOriginIndex)] +
                                 me[BindableBase.GetPropertyName(() => SelectedBusinessIndex)] +
                                 me[BindableBase.GetPropertyName(() => SelectedDepartmentIndex)] +
                                 me[BindableBase.GetPropertyName(() => Description)];//[pallavi.kale][GEOS2-8218]

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;

                string selectedLocationIndex = BindableBase.GetPropertyName(() => SelectedLocationIndex);
                string selectedResponsibleIndex = BindableBase.GetPropertyName(() => SelectedResponsibleIndex);
                string selectedOriginIndex = BindableBase.GetPropertyName(() => SelectedOriginIndex);
                string selectedBusinessIndex = BindableBase.GetPropertyName(() => SelectedBusinessIndex);
                string selectedDepartmentIndex = BindableBase.GetPropertyName(() => SelectedDepartmentIndex);
                string description = BindableBase.GetPropertyName(() => Description);//[pallavi.kale][GEOS2-8218]

                if (columnName == selectedLocationIndex)
                {
                    return AddEditActionPlanValidation.GetErrorMessage(selectedLocationIndex, SelectedLocationIndex);
                }
                if (columnName == selectedResponsibleIndex)
                {
                    return AddEditActionPlanValidation.GetErrorMessage(selectedResponsibleIndex, SelectedResponsibleIndex);
                }
                if (columnName == selectedOriginIndex)
                {
                    return AddEditActionPlanValidation.GetErrorMessage(selectedOriginIndex, SelectedOriginIndex);
                }
                //[shweta.thube][GEOS2-6586]
                if (columnName == selectedBusinessIndex)
                {
                    return AddEditActionPlanValidation.GetErrorMessage(selectedBusinessIndex, SelectedBusinessIndex);
                }
                if (columnName == selectedDepartmentIndex)
                {
                    return AddEditActionPlanValidation.GetErrorMessage(selectedDepartmentIndex, SelectedDepartmentIndex);
                }
                //[pallavi.kale][GEOS2-8218]
                if (columnName == description)
                {
                    return AddEditActionPlanValidation.GetErrorMessage(description, Description);
                }
                return null;
            }
        }
        #endregion

       
    }
}
