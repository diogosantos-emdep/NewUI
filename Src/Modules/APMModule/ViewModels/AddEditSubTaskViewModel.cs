using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.Modules.APM.CommonClasses;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.APM.Views;
using Emdep.Geos.UI.Validations;
using System.Net;
using System.Windows.Shell;
using System.Net.Mail;
using System.Drawing;
using DevExpress.Xpo.DB;
using System.Windows.Media.Media3D;

namespace Emdep.Geos.Modules.APM.ViewModels
{
    public class AddEditSubTaskViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    { 
        //[pallavi.kale][GEOS2-7003][19.06.2025]
        #region Services
        IAPMService APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IAPMService APMService = new APMServiceController("localhost:6699");
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region public Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RequestClose;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region Declaration
        private bool isNew;
        private string windowHeader;
        private LookupValue selectedBusinessUnit;
        private LookupValue selectedStatus;
        private LookupValue selectedTheme;
        private LookupValue selectedPriority;
        private Company selectedLocation;
        private ObservableCollection<APMActionPlan> actionPlanList;
        private APMActionPlan selectedActionPlans;
        private Responsible selectedResponsible;
        private List<Responsible> tempResponsibleList;
        private Responsible selectedDelegatedto;
        private APMActionPlanTask tempTask;
        private string title;
        private bool isSave;
        private string code;
        private DateTime dueDate;
        private int progress;
        private int previousProgress;
        private string informationError;
        private Responsible previousSelectedResponsible;
        private Responsible previousSelectedDelegatedto;
        private Int64 idActionPlanTask;
        private string previousTitle;
        private Company previousSelectedLocation;
        private LookupValue previousSelectedStatus;
        private LookupValue previousSelectedTheme;
        private LookupValue previousSelectedBusinessUnit;
        private LookupValue previousSelectedPriority;
        private Int32 taskNumber;
        private Int64 idActionPlan;
        private bool isAddEditActionPlan;
        private APMActionPlanTask updatedTask;
        private bool isAddButtonEnabled;
        private DateTime previousDueDate;
        private Int32 changeCount;
        private ObservableCollection<Responsible> delegatedToList;
        public Int32 InactiveSelectedDelegatedToIdEmployee { get; set; }
        public int PreviousIdDelegatedTo { get; set; }
        private bool isSaveChanges;
        private ObservableCollection<LookupValue> statusList;
        private Int32 idActionPlanResponsible;

        private string previousCode;
        private ObservableCollection<APMActionPlanTask> taskList;
        private List<APMActionPlanTask> tempTaskList;
        private List<APMActionPlanSubTask> oldSubTaskList;
        public Int32 InactiveSelectedResponsibleToIdEmployee { get; set; }
        private bool isEditable; 
        private List<Responsible> responsibleList;
        private string description;
        private string previousDescription;
        private Int32 actionPlanResponsibleIdUser;
        private bool isTaskProgressValidation;

        private LookupValue previousStatus;
        private Visibility isStatusFieldsVisible;
        private string taskStatusComment;
        private ObservableCollection<AttachmentsByTask> attachmentObjectList;
        private AttachmentsByTask actionPlanAttachmentFile;
        private string taskStatusDescription;
        private string fileName;
        private string uniqueFileName;
        private string fileNameString;
        private bool isBusy;
        private AttachmentsByTask attachment;
        private Visibility otItemVisibility;
        private DateTime fromDate;
        private DateTime toDate;
        private List<UserSiteGeosServiceProvider> allPlantsList;
        private ObservableCollection<OTs> oTsList;
        private Company tempPlantDetail;
        private OTs selectedOTCode;
        private ObservableCollection<OTs> oTItemsList;
        private string oTItem;
        private OTs selectedOTItem;
        private Int64 idSite;
        private Visibility otItemFind;
        private Int64 idResponsibleLocation;
        public int previousSelectedOTItem;
        private string findOTItemHeader;
        private string customerName;
        private Int64 height;
        private string previousOTItem;
        private bool isDescriptionEnabled;
        private bool isSelectButtonEnabled;
        private APMActionPlanTask selectedTask;
        private ObservableCollection<Responsible> responsibleObsList;
        private Int64 idActionPlanTaskResponsible;
        private Int64 idDelegatedTo;
        private APMActionPlanSubTask tempSubTask;
        private APMActionPlanSubTask updatedSubTask;
        private Int64 idParent;//[pallavi.kale][GEOS2-7003]
        private APMActionPlanSubTask selectedEditSubTask;//[pallavi.kale][GEOS2-7003]
        private Int64 previousParentTaskNumber; //[shweta.thube][GEOS2-7005][08.07.2025]
        private string previousSubTaskCode; //[shweta.thube][GEOS2-7005][08.07.2025]
        #endregion

        #region  public Properties;
        public bool IsNew
        {
            get
            {
                return isNew;
            }

            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }
        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public LookupValue SelectedBusinessUnit
        {
            get
            {
                return selectedBusinessUnit;
            }

            set
            {
                selectedBusinessUnit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBusinessUnit"));
            }
        }
        public LookupValue SelectedStatus
        {
            get
            {
                return selectedStatus;
            }

            set
            {
                selectedStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatus"));
         
                if (Progress != 100 && SelectedStatus?.IdLookupValue == 1982)
                    Progress = 100;

                if (PreviousStatus != null && PreviousStatus != SelectedStatus && !IsNew)
                {
                    IsStatusFieldsVisible = Visibility.Visible;
                    Height = 800;
                }
                else
                {
                    IsStatusFieldsVisible = Visibility.Collapsed;
                    Height = 500;
                }
            }
        }
        public LookupValue SelectedTheme
        {
            get
            {
                return selectedTheme;
            }

            set
            {
                selectedTheme = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTheme"));
            }
        }

        public LookupValue SelectedPriority
        {
            get
            {
                return selectedPriority;
            }

            set
            {
                selectedPriority = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPriority"));
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
   
        public APMActionPlan SelectedActionPlans
        {
            get { return selectedActionPlans; }
            set
            {
                selectedActionPlans = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActionPlans"));
                if (SelectedActionPlans != null)
                {

                    APMCommon.Instance.TaskList = selectedActionPlans.TaskList .OrderBy(t => t.TaskNumber).ToList() ?? new List<APMActionPlanTask>();
                }
            }
        }
        public Responsible SelectedResponsible
        {
            get { return selectedResponsible; }
            set
            {
                if (value != null)
                    selectedResponsible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedResponsible"));
                if (SelectedResponsible != null)
                {
                    if ((SelectedResponsible != null && SelectedResponsible.IdEmployee == IdActionPlanResponsible) || (GeosApplication.Instance.ActiveUser.IdUser == SelectedResponsible.IdUser) || (GeosApplication.Instance.ActiveUser.IdUser == ActionPlanResponsibleIdUser))
                    {
                        StatusList.FirstOrDefault(x => x.Value == "Done").InUse = true;
                        IsTaskProgressValidation = false;
                    }
                    else
                    {
                        StatusList.FirstOrDefault(x => x.Value == "Done").InUse = false;
                        IsTaskProgressValidation = true;
                    }
                }
                else
                {
                    if ((SelectedResponsible != null && SelectedResponsible.IdEmployee == IdActionPlanResponsible) || (GeosApplication.Instance.ActiveUser.IdUser == ActionPlanResponsibleIdUser))
                    {
                        StatusList.FirstOrDefault(x => x.Value == "Done").InUse = true;
                        IsTaskProgressValidation = false;
                    }
                    else
                    {
                        StatusList.FirstOrDefault(x => x.Value == "Done").InUse = false;
                        IsTaskProgressValidation = true;
                    }
                }

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
        public Responsible SelectedDelegatedto
        {
            get { return selectedDelegatedto; }
            set
            {
                if (value != null)
                {
                    selectedDelegatedto = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedDelegatedto"));
                }
            }
        }
        public APMActionPlanTask TempTask
        {
            get { return tempTask; }
            set
            {
                tempTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempTask"));
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
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
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
            }
        }
        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                dueDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DueDate"));
            }
        }
        public int Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Progress"));
              
                if (Progress == 100 && SelectedStatus?.IdLookupValue != 1982 && StatusList?.FirstOrDefault(status => status.Value == "Done").InUse != false)
                {
                    var temp = StatusList?.FirstOrDefault(status => status.Value == "Done");
                    SelectedStatus = temp;
                }
            }
        }
    
        public int PreviousProgress
        {
            get { return previousProgress; }
            set
            {
                previousProgress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousProgress"));
            }
        }
        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
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
        public Responsible PreviousSelectedDelegatedto
        {
            get { return previousSelectedDelegatedto; }
            set
            {
                previousSelectedDelegatedto = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedDelegatedto"));
            }
        }
        public Int64 IdActionPlanTask
        {
            get { return idActionPlanTask; }
            set
            {
                idActionPlanTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdActionPlanTask"));
            }
        }
        public string PreviousTitle
        {
            get { return previousTitle; }
            set
            {
                previousTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousTitle"));
            }
        }
        public Company PreviousSelectedLocation
        {
            get { return previousSelectedLocation; }
            set
            {
                if (value != null)
                {
                    previousSelectedLocation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedLocation"));
                }
            }
        }
        public LookupValue PreviousSelectedStatus
        {
            get
            {
                return previousSelectedStatus;
            }

            set
            {
                if (value != null)
                {
                    previousSelectedStatus = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedStatus"));
                }
            }
        }
        public LookupValue PreviousSelectedTheme
        {
            get
            {
                return previousSelectedTheme;
            }

            set
            {
                if (value != null)
                {
                    previousSelectedTheme = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedTheme"));
                }
            }
        }
        public LookupValue PreviousSelectedBusinessUnit
        {
            get
            {
                return previousSelectedBusinessUnit;
            }

            set
            {
                if (value != null)
                {
                    previousSelectedBusinessUnit = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedBusinessUnit"));
                }
            }
        }
        public LookupValue PreviousSelectedPriority
        {
            get
            {
                return previousSelectedPriority;
            }

            set
            {
                if (value != null)
                {
                    previousSelectedPriority = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedPriority"));
                }
            }
        }

        public Int32 TaskNumber
        {
            get { return taskNumber; }
            set
            {
                taskNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskNumber"));
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

        public APMActionPlanTask UpdatedTask
        {
            get { return updatedTask; }
            set
            {
                updatedTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedTask"));
            }
        }
        public bool IsAddEditActionPlan
        {
            get { return isAddEditActionPlan; }
            set
            {
                isAddEditActionPlan = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddEditActionPlan"));
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
        public DateTime PreviousDueDate
        {
            get { return previousDueDate; }
            set
            {
                previousDueDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousDueDate"));
            }
        }
        public Int32 ChangeCount
        {
            get { return changeCount; }
            set
            {
                changeCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ChangeCount"));
            }
        }

        public ObservableCollection<Responsible> DelegatedToList
        {
            get { return delegatedToList; }
            set
            {
                delegatedToList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DelegatedToList"));
                if (IdDelegatedTo > 0)
                {
                    SelectedDelegatedto = DelegatedToList.FirstOrDefault(x => x.IdEmployee == IdDelegatedTo);
                }
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

        public ObservableCollection<LookupValue> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }

        public Int32 IdActionPlanResponsible
        {
            get { return idActionPlanResponsible; }
            set
            {
                idActionPlanResponsible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdActionPlanResponsible"));
            }
        }
        public string PreviousCode
        {
            get { return previousCode; }
            set
            {
                previousCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousCode"));
            }
        }
        public ObservableCollection<APMActionPlanTask> TaskList
        {
            get { return taskList; }
            set
            {
                taskList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskList"));
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
        public List<APMActionPlanSubTask> OldSubTaskList
        {
            get { return oldSubTaskList; }
            set
            {
                oldSubTaskList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldSubTaskList"));
            }
        }
        public bool IsEditable
        {
            get
            {
                return isEditable;
            }

            set
            {
                isEditable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditable"));
            }
        }

        public List<Responsible> ResponsibleList
        {
            get { return responsibleList; }
            set
            {
                responsibleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResponsibleList"));
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
        public string PreviousDescription
        {
            get { return previousDescription; }
            set
            {
                previousDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousDescription"));
            }
        }

        public Int32 ActionPlanResponsibleIdUser
        {
            get { return actionPlanResponsibleIdUser; }
            set
            {
                actionPlanResponsibleIdUser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanResponsibleIdUser"));
            }
        }

        public bool IsTaskProgressValidation
        {
            get { return isTaskProgressValidation; }
            set
            {
                isTaskProgressValidation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTaskProgressValidation"));
            }
        }

        public LookupValue PreviousStatus
        {
            get { return previousStatus; }
            set
            {
                previousStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousStatus"));
            }
        }

        public Visibility IsStatusFieldsVisible
        {
            get { return isStatusFieldsVisible; }
            set
            {
                isStatusFieldsVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsStatusFieldsVisible"));
            }
        }

        public string TaskStatusComment
        {
            get { return taskStatusComment; }
            set
            {
                taskStatusComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskStatusComment"));
            }
        }
        public ObservableCollection<AttachmentsByTask> AttachmentObjectList
        {
            get { return attachmentObjectList; }
            set
            {
                attachmentObjectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentObjectList"));
                if (AttachmentObjectList != null && AttachmentObjectList.Count > 0)
                {
                    IsDescriptionEnabled = true;
                }
                else
                {
                    IsDescriptionEnabled = false;
                }
            }
        }

        public AttachmentsByTask ActionPlanAttachmentFile
        {
            get { return actionPlanAttachmentFile; }
            set
            {
                actionPlanAttachmentFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanAttachmentFile"));
            }
        }
        public string TaskStatusDescription
        {
            get { return taskStatusDescription; }
            set
            {
                taskStatusDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskStatusDescription"));
            }
        }
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileName"));
            }
        }

        public string UniqueFileName
        {
            get { return uniqueFileName; }
            set
            {
                uniqueFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UniqueFileName"));
            }
        }

        public string FileNameString
        {
            get { return fileNameString; }
            set
            {
                fileNameString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileNameString"));
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
        public AttachmentsByTask Attachment
        {
            get { return attachment; }
            set
            {
                attachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Attachment"));
            }
        }
        public Visibility OtItemVisibility
        {
            get { return otItemVisibility; }
            set
            {
                otItemVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtItemVisibility"));
            }
        }
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));
            }
        }  
        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
            }
        }
        public Company TempPlantDetail
        {
            get { return tempPlantDetail; }
            set
            {
                tempPlantDetail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempPlantDetail"));
            }
        }
        public List<UserSiteGeosServiceProvider> AllPlantsList
        {
            get { return allPlantsList; }
            set
            {
                allPlantsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllPlantsList"));
            }


        }
        public ObservableCollection<OTs> OTsList
        {
            get { return oTsList; }
            set
            {
                oTsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTsList"));
            }
        }
        public ObservableCollection<OTs> OTItemsList
        {
            get { return oTItemsList; }
            set
            {
                oTItemsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTItemsList"));
            }
        }
        public OTs SelectedOTCode
        {
            get { return selectedOTCode; }
            set
            {
                selectedOTCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOTCode"));
            }
        }
        public OTs SelectedOTItem
        {
            get { return selectedOTItem; }
            set
            {
                selectedOTItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOTItem"));
                if (SelectedOTItem != null)
                {
                    IsSelectButtonEnabled = true;
                }
                else
                {
                    IsSelectButtonEnabled = false;
                }
            }
        }
        public string OTItem
        {
            get { return oTItem; }
            set
            {
                oTItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTItem"));
            }
        }
        public Int64 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdSite"));
            }
        }
        public Visibility OtItemFind
        {
            get { return otItemFind; }
            set
            {
                otItemFind = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtItemFind"));
            }
        }
        public Int64 IdResponsibleLocation
        {
            get { return idResponsibleLocation; }
            set
            {
                idResponsibleLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdResponsibleLocation"));
            }
        }
        public int PreviousSelectedOTItem
        {
            get
            {
                return previousSelectedOTItem;
            }

            set
            {
                if (value != 0)
                {
                    previousSelectedOTItem = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PreviousSelectedOTItem"));
                }
            }
        }
        public string FindOTItemHeader
        {
            get
            {
                return findOTItemHeader;
            }

            set
            {
                findOTItemHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FindOTItemHeader"));
            }
        }
        public string PreviousOTItem
        {
            get { return previousOTItem; }
            set
            {
                previousOTItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousOTItem"));
            }
        }
        public string CustomerName
        {
            get { return customerName; }
            set
            {
                customerName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerName"));
            }
        }
        public Int64 Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Height"));
            }
        }
        public bool IsDescriptionEnabled
        {
            get { return isDescriptionEnabled; }
            set
            {
                isDescriptionEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDescriptionEnabled"));
            }
        }
        public bool IsSelectButtonEnabled
        {
            get { return isSelectButtonEnabled; }
            set
            {
                isSelectButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectButtonEnabled"));
            }
        }
        public APMActionPlanTask SelectedTask
        {
            get { return selectedTask; }
            set
            {
                selectedTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTask"));
            }
        }
        public ObservableCollection<Responsible> ResponsibleObsList
        {
            get { return responsibleObsList; }
            set
            {
                responsibleObsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResponsibleObsList"));
            }
        }
        public Int64 IdActionPlanTaskResponsible
        {
            get { return idActionPlanTaskResponsible; }
            set
            {
                idActionPlanTaskResponsible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdActionPlanTaskResponsible"));
            }
        }
        public Int64 IdDelegatedTo
        {
            get { return idDelegatedTo; }
            set
            {
                idDelegatedTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDelegatedTo"));
            }
        }
        public APMActionPlanSubTask TempSubTask
        {
            get { return tempSubTask; }
            set
            {
                tempSubTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempSubTask"));
            }
        }
        public APMActionPlanSubTask UpdatedSubTask
        {
            get { return updatedSubTask; }
            set
            {
                updatedSubTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedSubTask"));
            }
        }
      //[pallavi.kale][GEOS2-7003]
        public Int64 IdParent
        {
            get { return idParent; }
            set
            {
                idParent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdParent"));
            }
        }
       //[pallavi.kale][GEOS2-7003]
        public APMActionPlanSubTask SelectedEditSubTask
        {
            get { return selectedEditSubTask; }
            set
            {
                selectedEditSubTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEditSubTask"));
            }
        }
        //[shweta.thube][GEOS2-7005][08.07.2025]
        public Int64 PreviousParentTaskNumber
        {
            get { return previousParentTaskNumber; }
            set
            {
                previousParentTaskNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousParentTaskNumber"));
            }
        }
        //[shweta.thube][GEOS2-7005][08.07.2025]
        public string PreviousSubTaskCode
        {
            get { return previousSubTaskCode; }
            set
            {
                previousSubTaskCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousSubTaskCode"));
            }
        }
        #endregion

        #region Public Commands
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand LocationListClosedCommand { get; set; }

        public ICommand ChooseFileActionCommand { get; set; }
        public ICommand FindOTItemCommand { get; set; }
        public ICommand OtItemCancelButtonCommand { get; set; }
        public ICommand OTsLoadCommand { get; set; }

        public ICommand OTCodeListClosedCommand { get; set; }
        public ICommand SelectButtonCommand { get; set; }
        #endregion

        #region Constructor        
        public AddEditSubTaskViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEditTaskViewModel ...", category: Category.Info, priority: Priority.Low);
                AcceptButtonCommand = new RelayCommand(new Action<object>(AddSubTaskForActionPlan));
                CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
                LocationListClosedCommand = new RelayCommand(new Action<object>(LocationListClosedCommandAction));
                ChooseFileActionCommand = new RelayCommand(new Action<object>(ChooseFileActionCommandAction));
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(p => p.IdPermission == 122))
                {
                    IsAddButtonEnabled = true;
                }
                else
                {
                    IsAddButtonEnabled = false;
                }
                FindOTItemCommand = new RelayCommand(new Action<object>(FindOTItemCommandAction));
                OtItemCancelButtonCommand = new RelayCommand(new Action<object>(OtItemCancelButtonCommandAction));
                OTsLoadCommand = new RelayCommand(new Action<object>(OTsLoadCommandAction));
                //OTCodeListClosedCommand= new RelayCommand(new Action<object>(OTCodeListClosedCommandAction));
                SelectButtonCommand = new RelayCommand(new Action<object>(SelectButtonCommandAction));
                GeosApplication.Instance.Logger.Log("Constructor AddEditSubTaskViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddEditSubTaskViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
       
        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                if (!IsNew)
                {
                    IsSaveChanges = false;
                    if (IdActionPlan != SelectedActionPlans.IdActionPlan)
                    {
                        IsSaveChanges = true;
                    }

                    if (PreviousTitle != Title)
                    {
                        IsSaveChanges = true;
                    }

                    if (PreviousSelectedLocation == null)
                    {
                        if (SelectedLocation != null)
                        {
                            IsSaveChanges = true;
                        }
                    }
                    else
                    {
                        if (PreviousSelectedLocation.IdCompany != SelectedLocation.IdCompany)
                        {
                            IsSaveChanges = true;
                        }
                    }

                    if (PreviousSelectedResponsible == null)
                    {
                        if (SelectedResponsible != null)
                        {
                            IsSaveChanges = true;
                        }
                    }
                    else
                    {
                        if (SelectedResponsible != null && PreviousSelectedResponsible.IdEmployee != SelectedResponsible.IdEmployee)
                        {
                            IsSaveChanges = true;
                        }
                    }

                    if (PreviousSelectedStatus == null)
                    {
                        if (SelectedStatus != null)
                        {
                            IsSaveChanges = true;
                        }
                    }
                    else
                    {
                        if (PreviousSelectedStatus.IdLookupValue != SelectedStatus.IdLookupValue)
                        {
                            IsSaveChanges = true;
                        }
                    }

                    if (PreviousSelectedPriority == null)
                    {
                        if (SelectedPriority != null)
                        {
                            IsSaveChanges = true;
                        }
                    }
                    else
                    {
                        if (PreviousSelectedPriority.IdLookupValue != SelectedPriority.IdLookupValue)
                        {
                            IsSaveChanges = true;
                        }
                    }

                    //if (PreviousSelectedBusinessUnit == null)
                    //{
                    //    if (SelectedBusinessUnit != null)
                    //    {
                    //        IsSaveChanges = true;
                    //    }
                    //}
                    //else
                    //{
                    //    if (PreviousSelectedBusinessUnit.IdLookupValue != SelectedBusinessUnit.IdLookupValue)
                    //    {
                    //        IsSaveChanges = true;
                    //    }
                    //}

                    if (PreviousSelectedTheme == null)
                    {
                        if (SelectedTheme != null)
                        {
                            IsSaveChanges = true;
                        }
                    }
                    else
                    {
                        if (PreviousSelectedTheme.IdLookupValue != SelectedTheme.IdLookupValue)
                        {
                            IsSaveChanges = true;
                        }
                    }

                    if (PreviousDueDate != DueDate)
                    {
                        IsSaveChanges = true;
                    }
                    if (PreviousSelectedDelegatedto == null)
                    {
                        if (SelectedDelegatedto != null)
                        {
                            IsSaveChanges = true;
                        }
                    }
                    else
                    {
                        if (PreviousSelectedDelegatedto.IdEmployee != SelectedDelegatedto.IdEmployee)
                        {
                            IsSaveChanges = true;
                        }
                    }

                    if (PreviousProgress != Progress)
                    {
                        IsSaveChanges = true;
                    }
                    if (IsSaveChanges)
                    {
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ActionPlanUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                        if (MessageBoxResult == MessageBoxResult.Yes)
                        {
                            AddSubTaskForActionPlan(null);
                        }
                    }

                }

                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList ...", category: Category.Info, priority: Priority.Low);
                //APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                var temp = new List<LookupValue>(APMService.GetLookupValues_V2550(152));//TaskStatus
                StatusList = new ObservableCollection<LookupValue>(temp);
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
        private void FillThemeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillThemeList ...", category: Category.Info, priority: Priority.Low);
                if (APMCommon.Instance.ThemeList == null)
                {
                    List<LookupValue> temp = new List<LookupValue>();
                    //APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    temp = new List<LookupValue>(APMService.GetLookupValues_V2550(155));//TaskThemes
                    APMCommon.Instance.ThemeList = new List<LookupValue>(temp);
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
        private void FillPriorityList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPriorityList ...", category: Category.Info, priority: Priority.Low);
                if (APMCommon.Instance.PriorityList == null)
                {
                    List<LookupValue> temp = new List<LookupValue>();
                    //APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    temp = new List<LookupValue>(APMService.GetLookupValues_V2550(153));//TaskPriority                    
                    APMCommon.Instance.PriorityList = new List<LookupValue>(temp);
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

        private void FillLocationList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLocationList ...", category: Category.Info, priority: Priority.Low);
               
                if (APMCommon.Instance.LocationList == null)
                {
                    APMCommon.Instance.LocationList = new List<Company>(APMService.GetAuthorizedLocationListByIdUser_V2570(GeosApplication.Instance.ActiveUser.IdUser));

                }
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
        public void Init(APMActionPlanTask selectedActionPlan)
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
              
                if (selectedActionPlan.IdSite != 0)
                {
                    OtItemFind = Visibility.Visible;
                }
                else
                {
                    OtItemFind = Visibility.Collapsed;
                }
              
                IsEditable = true;
                IdActionPlan = selectedActionPlan.IdActionPlan;
                FillTaskList();
                //[shweta.thube][GEOS2-7218][23.07.2025]
                if (APMCommon.Instance.ActionPlanList != null)
                {
                    var targetActionPlan = APMCommon.Instance.ActionPlanList.FirstOrDefault(x => x.IdActionPlan == IdActionPlan);
                    if (targetActionPlan != null)
                    {
                        targetActionPlan.TaskList = TaskList.ToList(); // Replace with your actual list
                    }
                    APMCommon.Instance.ActionPlanList = new List<APMActionPlan>(APMCommon.Instance.ActionPlanList);
                    if (selectedActionPlan != null)
                    {
                        //selectedActionPlan.TaskList = new List<APMActionPlanTask>(TaskList);
                        SelectedActionPlans = APMCommon.Instance.ActionPlanList.FirstOrDefault(x => x.IdActionPlan == selectedActionPlan.IdActionPlan);
                    }
                }
                if (APMCommon.Instance.TaskList != null)
                {
                    if (selectedActionPlan != null)
                    {
                        SelectedTask = APMCommon.Instance.TaskList.FirstOrDefault(x => x.IdActionPlanTask == selectedActionPlan.IdActionPlanTask);
                    }
                }
                FillStatusList();

                SelectedStatus = StatusList.FirstOrDefault(status => status.Value == "To Do");
                IdActionPlanResponsible = selectedActionPlan.IdEmployee;
                ActionPlanResponsibleIdUser = selectedActionPlan.ResponsibleIdUser;
                FillPriorityList();
                FillThemeList();
                FillLocationList();
                var userCompanyId = selectedActionPlan.IdCompany;

                SelectedLocation = APMCommon.Instance.LocationList.FirstOrDefault(loc => loc.IdCompany == userCompanyId);
               
                if (!IsNew)
                {
                    SelectedResponsible = ResponsibleList.FirstOrDefault();
                    SelectedDelegatedto = APMCommon.Instance.DelegatedToList.FirstOrDefault();
                    if (APMCommon.Instance.ThemeList != null)
                    {
                        SelectedTheme = APMCommon.Instance.ThemeList.FirstOrDefault();
                    }
                    if (APMCommon.Instance.PriorityList != null)
                    {
                        SelectedPriority = APMCommon.Instance.PriorityList.FirstOrDefault();
                    }
                    SelectedBusinessUnit = APMCommon.Instance.BusinessUnitList.FirstOrDefault();
                }
                DueDate = DateTime.Now.AddDays(7);
             
                if (TempTaskList == null)
                {
                    TempTaskList = new List<APMActionPlanTask>();
                }
                if (TaskList != null)
                {
                    foreach (var item in TaskList)
                    {
                        item.Code = Code;
                        TempTaskList.Add((APMActionPlanTask)item.Clone());
                    }
                }

                IsStatusFieldsVisible = Visibility.Collapsed;
                IdSite = selectedActionPlan.IdSite;
                IdResponsibleLocation = userCompanyId;
                CustomerName = selectedActionPlan.GroupName;
                Height = 500;
                FillEmdepSitesDetail();
                Code = selectedActionPlan.ActionPlanCode;
                SetUserPermission();      //[shweta.thube][GEOS2-9910][16.10.2025]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddSubTaskForActionPlan(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method AddTaskForActionPlan ()...", category: Category.Info, priority: Priority.Low);


                InformationError = null;
                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";
                PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedLocation"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedResponsible"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedStatus"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedPriority"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedTheme"));
                if (IsStatusFieldsVisible == Visibility.Visible)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TaskStatusComment"));
                }                
                if (SelectedActionPlans.IdEmployee != SelectedResponsible?.IdEmployee)
                {
                    if (Progress == 100)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Progress"));
                    }
                }
                else if (Title != null && SelectedLocation != null && SelectedResponsible != null && SelectedStatus != null && SelectedPriority != null && SelectedBusinessUnit != null && SelectedTheme != null)
                {
                    error = null;
                }
                if (error != null)
                {
                    //IsBusy = false;
                    return;
                }

                if (SelectedLocation.IsStillActive == 0)
                {
                    string Comment = string.Format(System.Windows.Application.Current.FindResource("SelectedLocationNotAllowed").ToString(), SelectedLocation.Alias);
                    CustomMessageBox.Show(Comment.ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }


                if (IsNew)
                {
                    TempSubTask = new APMActionPlanSubTask();
                    if (SelectedActionPlans != null)
                    {
                        TempSubTask.IdActionPlan = SelectedActionPlans.IdActionPlan;
                    }
                    if (SelectedTask != null)
                    {
                        TempSubTask.IdParent = SelectedTask.IdActionPlanTask; 
                    }
                   
                    TempSubTask.Code = SelectedActionPlans.Code;
                    TempSubTask.Title = Title;
                    TempSubTask.Description = Description;
                    TempSubTask.IdCompany = SelectedLocation.IdCompany;
                    TempSubTask.Location = SelectedLocation.Alias;
                    TempSubTask.IdEmployee = (int)SelectedResponsible.IdEmployee;
                    TempSubTask.Responsible = SelectedResponsible.FullName;
                    TempSubTask.TaskResponsibleDisplayName = SelectedResponsible.ResponsibleDisplayName;
                    TempSubTask.IdLookupStatus = SelectedStatus.IdLookupValue;
                    TempSubTask.Status = SelectedStatus.Value;
                    TempSubTask.StatusHTMLColor = SelectedStatus.HtmlColor;
                    if (TempSubTask.Status == "Done")
                    {
                        TempSubTask.CloseDate = DateTime.Now;
                        TempSubTask.ClosedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        TempSubTask.ClosedByName = GeosApplication.Instance.ActiveUser.FullName;
                    }
                    TempSubTask.IdLookupTheme = SelectedTheme.IdLookupValue;
                    TempSubTask.Theme = SelectedTheme.Value;
                    TempSubTask.IdLookupPriority = SelectedPriority.IdLookupValue;
                    TempSubTask.Priority = SelectedPriority.Value;
                    TempSubTask.DueDate = DueDate;
                    TempSubTask.OriginalDueDate = DueDate;
                    if (SelectedDelegatedto != null)
                    {
                        TempSubTask.IdDelegated = (int)SelectedDelegatedto.IdEmployee;
                        TempSubTask.DelegatedTo = SelectedDelegatedto.FullName;
                        TempSubTask.DelegatedDisplayName = SelectedDelegatedto.ResponsibleDisplayName;
                    }
                    TempSubTask.OpenDate = DateTime.Now;
                    TempSubTask.Progress = Progress;
                    TempSubTask.CreatedBy = (UInt32)GeosApplication.Instance.ActiveUser.IdUser;
                    TempSubTask.CreatedByName = GeosApplication.Instance.ActiveUser.FullName;

                    TempSubTask.CreatedIn = DateTime.Now;
                 
                    OldSubTaskList = APMCommon.Instance.TaskList.FirstOrDefault(i => i.IdActionPlanTask == TempSubTask.IdParent).SubTaskList;


                    if (OldSubTaskList == null || OldSubTaskList.Count == 0)
                    {
                        TempSubTask.TaskNumber = 1;
                        TempSubTask.SubTaskNumber = 1;
                     
                    }
                    else
                    {
                        TempSubTask.TaskNumber = (Int32)OldSubTaskList.OrderByDescending(x => x.SubTaskNumber).First().SubTaskNumber + 1;
                        TempSubTask.SubTaskNumber= (Int32)OldSubTaskList.OrderByDescending(x => x.SubTaskNumber).First().SubTaskNumber + 1;


                    }
                    //APMService = new APMServiceController("localhost:6699");
                    TempSubTask.SubTaskCode=APMService.GetMaxSubTaskCodeByIdTask_V2660((int)TempSubTask.IdParent);//[shweta.thube][GEOS2-8985]
                    if (SelectedOTItem != null)
                    {
                        TempSubTask.IdOTItem = (Int32)SelectedOTItem.IdOTItem;
                    }
                    TempSubTask.CodeNumber = OTItem;
                    SelectedEditSubTask = TempSubTask;
                    //[shweta.thube][GEOS2-7005][08.07.2025]
                    SelectedEditSubTask.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();
                    TempSubTask.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                    {
                        IdActionPlan = TempSubTask.IdActionPlan,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Datetime = GeosApplication.Instance.ServerDateTime,

                        Comments = string.Format(System.Windows.Application.Current.FindResource("ActionPlanAddNewSubTaskChangeLog").ToString(), TempSubTask.Title, TempSubTask.SubTaskCode)
                    });
                    if (!IsAddEditActionPlan)
                    {
                        //APMService = new APMServiceController("localhost:6699");
                        //TempSubTask = APMService.AddSubTaskForActionPlan_V2650(TempSubTask);
                        TempSubTask = APMService.AddSubTaskForActionPlan_V2660(TempSubTask);  //[shweta.thube][GEOS2-7005]

                        IsSave = true;
                        if (IsSave)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionPlanSubTaskAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        IsSave = true;
                    }
                }
                else
                {
                    APMActionPlanSubTask updated = new APMActionPlanSubTask();
                    //[shweta.thube][GEOS2-7005][08.07.2025]
                    updated.ActionPlanLogEntries = new List<LogEntriesByActionPlan>();
                    if (SelectedActionPlans.IdActionPlan != IdActionPlan)
                    {
                        updated.IdActionPlan = SelectedActionPlans.IdActionPlan;
                        updated.Code = SelectedActionPlans.Code;
                        
                    }
                    else
                    {
                        updated.IdActionPlan = IdActionPlan;
                        
                    }
                    if (SelectedTask.IdActionPlanTask != IdParent)
                    {

                        if (SelectedTask.SubTaskList != null && SelectedTask.SubTaskList.Count > 0)
                        {
                              updated.TaskNumber = (Int32)(SelectedTask.SubTaskList.OrderByDescending(x => x.SubTaskNumber).First().SubTaskNumber + 1);
                        }
                        else
                        {
                            updated.TaskNumber = 1;
                        }
                            updated.IdParent = SelectedTask.IdActionPlanTask;
                    }
                    else
                    {
                        updated.TaskNumber = TaskNumber;
                        updated.IdParent = IdParent;
                    }
                    updated.IdActionPlanTask = IdActionPlanTask;
                    updated.SubTaskCode = $"{SelectedTask.TaskNumber}.{updated.TaskNumber}";
                    if (PreviousParentTaskNumber != SelectedTask.TaskNumber)
                    {
                        updated.ParentTaskNumber = SelectedTask.TaskNumber;
                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,

                            Comments = string.Format(System.Windows.Application.Current.FindResource("OldActionPlanSubTaskNumberChangeLog").ToString(), PreviousParentTaskNumber, SelectedTask.TaskNumber, PreviousSubTaskCode)
                        });
                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = updated.IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,

                            Comments = string.Format(System.Windows.Application.Current.FindResource("NewActionPlanSubTaskNumberChangeLog").ToString(), SelectedTask.TaskNumber, PreviousParentTaskNumber, updated.SubTaskCode)
                        });


                    }
                    if (Title != PreviousTitle)
                    {
                        updated.Title = Title;
                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = updated.IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,

                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskTitleChangeLog").ToString(), PreviousTitle, Title, updated.SubTaskCode)
                        });
                    }
                    else
                    {
                        updated.Title = Title;
                    }
                    if (SelectedLocation.IdCompany != PreviousSelectedLocation.IdCompany)
                    {
                        updated.IdCompany = SelectedLocation.IdCompany;

                        string OldLoaction = APMCommon.Instance.LocationList.FirstOrDefault(i => i.IdCompany == PreviousSelectedLocation.IdCompany).Alias;
                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = updated.IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,

                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskLocationChangeLog").ToString(), OldLoaction, SelectedLocation.Alias, updated.SubTaskCode)
                        });

                    }
                    else
                    {
                        updated.IdCompany = SelectedLocation.IdCompany;
                    }
                    if (PreviousSelectedResponsible != null)
                    {
                        if (SelectedResponsible.IdEmployee != PreviousSelectedResponsible.IdEmployee)
                        {
                            updated.IdEmployee = (int)SelectedResponsible.IdEmployee;
                            updated.Responsible = SelectedResponsible.FullName;
                            updated.TaskResponsibleDisplayName = SelectedResponsible.ResponsibleDisplayName;

                            updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                            {
                                IdActionPlan = updated.IdActionPlan,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,

                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskResponsibleChangeLog").ToString(), PreviousSelectedResponsible.FullName, SelectedResponsible.FullName, updated.SubTaskCode)
                            });
                        }
                        else
                        {
                            updated.IdEmployee = (int)SelectedResponsible.IdEmployee;
                        }
                    }
                    else
                    {
                        updated.IdEmployee = (int)SelectedResponsible.IdEmployee;
                    }
                    updated.Progress = Progress;
                    updated.IdLookupStatus = SelectedStatus.IdLookupValue;
                    updated.Status = SelectedStatus.Value;

                    if (updated.Progress == 100)
                    {
                        updated.CloseDate = DateTime.Now;
                        updated.ClosedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        updated.IdLookupStatus = 1982;
                        updated.Status = "Done";
                        updated.StatusHTMLColor = "#92D050";
                    }

                    if (PreviousSelectedStatus != null)
                    {
                        if (updated.IdLookupStatus != PreviousSelectedStatus.IdLookupValue)
                        {
                            string OldStatus = StatusList.FirstOrDefault(i => i.IdLookupValue == PreviousSelectedStatus.IdLookupValue).Value;
                            updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                            {
                                IdActionPlan = updated.IdActionPlan,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,

                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskLStatusChangeLog").ToString(), OldStatus, SelectedStatus.Value, updated.SubTaskCode)
                            });
                        }

                    }


                    if (updated.Status == "Done")
                    {
                        updated.CloseDate = DateTime.Now;
                        updated.ClosedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        Progress = 100;
                    }

                    if (SelectedPriority.IdLookupValue != PreviousSelectedPriority.IdLookupValue)
                    {
                        updated.IdLookupPriority = SelectedPriority.IdLookupValue;

                        string OldPriority = APMCommon.Instance.PriorityList.FirstOrDefault(i => i.IdLookupValue == PreviousSelectedPriority.IdLookupValue).Value;
                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = updated.IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,

                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskLPriorityChangeLog").ToString(), OldPriority, SelectedPriority.Value, updated.SubTaskCode)
                        });

                    }
                    else
                    {
                        updated.IdLookupPriority = SelectedPriority.IdLookupValue;
                    }

                    if (SelectedTheme.IdLookupValue != PreviousSelectedTheme.IdLookupValue)
                    {
                        updated.IdLookupTheme = SelectedTheme.IdLookupValue;

                        string OldTheme = APMCommon.Instance.ThemeList.FirstOrDefault(i => i.IdLookupValue == PreviousSelectedTheme.IdLookupValue).Value;
                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = updated.IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,

                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskThemeChangeLog").ToString(), OldTheme, SelectedTheme.Value, updated.SubTaskCode)
                        });
                    }
                    else
                    {
                        updated.IdLookupTheme = SelectedTheme.IdLookupValue;
                    }
                    if (SelectedDelegatedto != null)
                    {
                        updated.IdDelegated = (Int32)SelectedDelegatedto.IdEmployee;
                        updated.DelegatedTo = SelectedDelegatedto.FullName;
                        updated.DelegatedDisplayName = SelectedDelegatedto.ResponsibleDisplayName;

                        if (SelectedDelegatedto.IdEmployee != PreviousIdDelegatedTo)
                        {
                            if (PreviousIdDelegatedTo == 0)
                            {
                                // string OldDelegatedto = APMCommon.Instance.DelegatedToList.FirstOrDefault(i => i.IdEmployee == PreviousIdDelegatedTo).FullName;
                                updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                {
                                    IdActionPlan = updated.IdActionPlan,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,

                                    Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDelegatedtoPreviousNoneChangeLog").ToString(), SelectedDelegatedto.FullName, updated.SubTaskCode)
                                });
                            }
                            else if (SelectedDelegatedto.IdEmployee == 0)
                            {
                                string OldDelegatedto = APMCommon.Instance.DelegatedToList.FirstOrDefault(i => i.IdEmployee == PreviousIdDelegatedTo).FullName;
                                updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                {
                                    IdActionPlan = updated.IdActionPlan,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,

                                    Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDelegatedtoSelectedNoneChangeLog").ToString(), OldDelegatedto, updated.SubTaskCode)
                                });
                            }
                            else
                            {
                                string OldDelegatedto = APMCommon.Instance.DelegatedToList.FirstOrDefault(i => i.IdEmployee == PreviousIdDelegatedTo).FullName;
                                updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                                {
                                    IdActionPlan = updated.IdActionPlan,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Datetime = GeosApplication.Instance.ServerDateTime,

                                    Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDelegatedtoChangeLog").ToString(), OldDelegatedto, SelectedDelegatedto.FullName, updated.SubTaskCode)
                                });
                            }
                        }
                    }
                    updated.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    if (DueDate != PreviousDueDate)
                    {
                        updated.DueDate = DueDate;
                        updated.ChangeCount = ChangeCount + 1;


                    }
                    else
                    {
                        updated.ChangeCount = ChangeCount;
                        updated.DueDate = DueDate;
                    }

                    updated.LastUpdated = DateTime.Now;
                    updated.Progress = Progress;
                    if (Progress != PreviousProgress)
                    {
                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = updated.IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,

                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskProgressChangeLog").ToString(), PreviousProgress, Progress, updated.SubTaskCode)
                        });
                    }

                    updated.Description = Description;
                    if (Description != PreviousDescription)
                    {
                        if (string.IsNullOrEmpty(PreviousDescription))
                        {
                            updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                            {
                                IdActionPlan = updated.IdActionPlan,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,

                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDescriptionChangeLog").ToString(), "None", Description, updated.SubTaskCode)
                            });
                        }
                        else if (string.IsNullOrEmpty(Description))
                        {
                            updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                            {
                                IdActionPlan = updated.IdActionPlan,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,

                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDescriptionChangeLog").ToString(), PreviousDescription, "None", updated.SubTaskCode)
                            });
                        }
                        else
                        {
                            updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                            {
                                IdActionPlan = updated.IdActionPlan,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,

                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskDescriptionChangeLog").ToString(), PreviousDescription, Description, updated.SubTaskCode)
                            });
                        }

                    }
                    if (PreviousStatus.IdLookupValue != updated.IdLookupStatus)
                    {

                        updated.TaskStatusComment = TaskStatusComment;

                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = updated.IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("AddActionPlanSubTaskChangeLogComment").ToString(), updated.SubTaskCode, TaskStatusComment)
                        });

                        updated.TaskStatusDescription = TaskStatusDescription;
                        if (AttachmentObjectList == null)
                        {
                            AttachmentObjectList = new ObservableCollection<AttachmentsByTask>();
                        }
                        updated.TaskAttachmentList = new List<AttachmentsByTask>(AttachmentObjectList);
                        updated.TaskAttachmentList.ForEach(x => x.AttachmentImage = null);
                        //[shweta.thube][GEOS2-7005][22.07.2025]
                        foreach (AttachmentsByTask item in updated.TaskAttachmentList)
                        {
                            updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                            {
                                IdActionPlan = updated.IdActionPlan,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("AddActionPlanSubTaskChangeLogattachment").ToString(), updated.SubTaskCode, item.FileUploadName)
                            });
                        }
                        //[shweta.thube][GEOS2-7005][22.07.2025]
                        if (!string.IsNullOrEmpty(updated.TaskStatusDescription))
                        {
                            updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                            {
                                IdActionPlan = updated.IdActionPlan,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("AddActionPlanSubTaskChangeLogAttachmentDescription").ToString(), updated.SubTaskCode, TaskStatusDescription)
                            });
                        }
                    }
                    if (!string.IsNullOrEmpty(OTItem) && !string.IsNullOrEmpty(PreviousOTItem))
                    {
                        if (OTItem == PreviousOTItem)
                        {
                            updated.IdOTItem = PreviousSelectedOTItem;
                            updated.CodeNumber = PreviousOTItem;
                        }
                        else
                        {
                            updated.IdOTItem = (Int32)SelectedOTItem.IdOTItem;
                            updated.CodeNumber = SelectedOTItem.CodeNumber;
                            updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                            {
                                IdActionPlan = updated.IdActionPlan,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,

                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskOtItemChangeLog").ToString(), PreviousOTItem, OTItem, updated.SubTaskCode)
                            });
                        }
                    }
                    else if (string.IsNullOrEmpty(OTItem) && string.IsNullOrEmpty(PreviousOTItem))
                    {
                        updated.IdOTItem = 0;
                    }
                    else if (string.IsNullOrEmpty(PreviousOTItem))
                    {
                        updated.IdOTItem = (Int32)SelectedOTItem.IdOTItem;
                        updated.CodeNumber = SelectedOTItem.CodeNumber;
                        updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                        {
                            IdActionPlan = updated.IdActionPlan,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,

                            Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskOtItemChangeLog").ToString(), "None", OTItem, updated.SubTaskCode)
                        });

                    }
                    else //if (string.IsNullOrEmpty(OTItem))
                    {
                        updated.IdOTItem = 0;
                        if (!string.IsNullOrEmpty(PreviousOTItem))
                        {
                            updated.ActionPlanLogEntries.Add(new LogEntriesByActionPlan()
                            {
                                IdActionPlan = updated.IdActionPlan,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,

                                Comments = string.Format(System.Windows.Application.Current.FindResource("SubTaskOtItemChangeLog").ToString(), PreviousOTItem, "None", updated.SubTaskCode)
                            });
                        }
                    }



                    UpdatedSubTask = updated;
                    if (!IsAddEditActionPlan)
                    {
                        //APMService = new APMServiceController("localhost:6699");
                        //IsSave = APMService.UpdateSubTaskForActionPlan_V2650(updated);
                        IsSave = APMService.UpdateSubTaskForActionPlan_V2660(updated); //[shweta.thube][GEOS2-7005][08.07.2025]


                        if (IsSave)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionPlanSubTaskUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        IsSave = true;
                    }
                }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddSubTaskForActionPlan....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddSubTaskForActionPlan() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddSubTaskForActionPlan() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddSubTaskForActionPlan() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        public void EditInit(APMActionPlanSubTask selectedActionPlanTask)
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
                if (!selectedActionPlanTask.IsSubTaskAdded)
                {
                    IsNew = true;
                    selectedActionPlanTask.TransactionOperation = ModelBase.TransactionOperations.Add;

                }
                else
                {
                    IsNew = false;
                    selectedActionPlanTask.TransactionOperation = ModelBase.TransactionOperations.Update;
                }
                if(SelectedEditSubTask !=null)
                {
                    selectedActionPlanTask = SelectedEditSubTask;

                }
                IdActionPlanTask = selectedActionPlanTask.IdActionPlanTask;
                IdParent= selectedActionPlanTask.IdParent;
                IdActionPlan = selectedActionPlanTask.IdActionPlan;
                IdActionPlanResponsible = selectedActionPlanTask.IdActionPlanResponsible;
                ActionPlanResponsibleIdUser = selectedActionPlanTask.ActionPlanResponsibleIdUser;
                SelectedActionPlans = APMCommon.Instance.ActionPlanList.FirstOrDefault(x => x.IdActionPlan == selectedActionPlanTask.IdActionPlan);
                Title = selectedActionPlanTask.Title;
                PreviousTitle = selectedActionPlanTask.Title;
                TaskNumber = (int)selectedActionPlanTask.SubTaskNumber;
                Code = selectedActionPlanTask.Code;
                Description = selectedActionPlanTask.Description;
                PreviousDescription = selectedActionPlanTask.Description;
                IdActionPlanTaskResponsible = selectedActionPlanTask.IdEmployee;
                IdDelegatedTo = selectedActionPlanTask.IdDelegated;
               
                FillStatusList();

                SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == selectedActionPlanTask.IdLookupStatus);
                PreviousSelectedStatus = SelectedStatus;
                PreviousStatus = SelectedStatus;


                FillPriorityList();
                if (APMCommon.Instance.PriorityList != null)
                {
                    SelectedPriority = APMCommon.Instance.PriorityList.FirstOrDefault(x => x.IdLookupValue == selectedActionPlanTask.IdLookupPriority);
                    PreviousSelectedPriority = SelectedPriority;
                }

                FillThemeList();
                if (APMCommon.Instance.ThemeList != null)
                {
                    SelectedTheme = APMCommon.Instance.ThemeList.FirstOrDefault(x => x.IdLookupValue == selectedActionPlanTask.IdLookupTheme);
                    PreviousSelectedTheme = SelectedTheme;
                }

                FillLocationList();

                SelectedLocation = APMCommon.Instance.LocationList.FirstOrDefault(x => x.IdCompany == selectedActionPlanTask.IdCompany);
                PreviousSelectedLocation = APMCommon.Instance.LocationList.FirstOrDefault(x => x.IdCompany == selectedActionPlanTask.IdCompany); ;

                //FillResponsibleList();
                string idCompanyString = Convert.ToString(SelectedLocation.IdCompany);
                FillResponsibleList(idCompanyString);
                if (ResponsibleList != null)
                {
                    if (ResponsibleList.Any(x => x.IdEmployee == selectedActionPlanTask.IdEmployee))
                    {
                        if (ResponsibleList.Any(x => x.IsInActive == true))
                        {
                            List<Responsible> isInactiveResponsible = ResponsibleList.Where(x => x.IsInActive == true).ToList();
                            foreach (var item in isInactiveResponsible)
                            {
                                ResponsibleList.Remove(item);
                            }
                        }
                    }
                    else
                    {
                        Responsible temp = APMService.GetInactiveResponsibleForActionPlan_V2560(selectedActionPlanTask.IdEmployee);
                        temp.IsInActive = true;
                        temp.EmployeeCodeWithIdGender = temp.EmployeeCode + "_" + temp.IdGender;
                        if (ResponsibleList == null)
                        {
                            ResponsibleList = new List<Responsible>();
                        }
                        ResponsibleList.Add(temp);
                        ResponsibleList = new List<Responsible>(ResponsibleList);
                    }
                }
                else
                {
                    Responsible temp = APMService.GetInactiveResponsibleForActionPlan_V2560(selectedActionPlanTask.IdEmployee);
                    temp.IsInActive = true;
                    temp.EmployeeCodeWithIdGender = temp.EmployeeCode + "_" + temp.IdGender;
                    if (ResponsibleList == null)
                    {
                        ResponsibleList = new List<Responsible>();
                    }
                    ResponsibleList.Add(temp);
                    ResponsibleList = new List<Responsible>(ResponsibleList);
                }

                InactiveSelectedDelegatedToIdEmployee = selectedActionPlanTask.IdDelegated;
                InactiveSelectedResponsibleToIdEmployee = selectedActionPlanTask.IdEmployee;


                if (ResponsibleList != null)
                {
                    SelectedResponsible = ResponsibleList.FirstOrDefault(x => x.FullName == selectedActionPlanTask.Responsible);
                    if (SelectedResponsible == null)
                    {
                        SelectedResponsible = new Responsible();

                        SelectedResponsible.IdEmployee = (uint)selectedActionPlanTask.IdEmployee;
                        SelectedResponsible.FullName = selectedActionPlanTask.Responsible;
                    }
                    PreviousSelectedResponsible = SelectedResponsible;

                }

                PreviousIdDelegatedTo = selectedActionPlanTask.IdDelegated;

                DueDate = selectedActionPlanTask.DueDate;
                PreviousDueDate = selectedActionPlanTask.DueDate;
                ChangeCount = selectedActionPlanTask.ChangeCount;
                Progress = selectedActionPlanTask.Progress;
                PreviousProgress = selectedActionPlanTask.Progress;
                PreviousCode = selectedActionPlanTask.Code;
  
                if (selectedActionPlanTask.IdLookupPriority == 1983)
                {
                    IsEditable = false;
                }
                else
                {
                    IsEditable = true;
                }

                IsStatusFieldsVisible = Visibility.Collapsed; 
                Height = 500;

                if (selectedActionPlanTask.IdSite > 0)
                {
                    OtItemFind = Visibility.Visible;
                }
                else
                {
                    OtItemFind = Visibility.Collapsed;
                }
                IdSite = selectedActionPlanTask.IdSite;
                IdResponsibleLocation = selectedActionPlanTask.IdActionPlanLocation;
                PreviousOTItem = selectedActionPlanTask.CodeNumber;
                OTItem = selectedActionPlanTask.CodeNumber;
                PreviousSelectedOTItem = selectedActionPlanTask.IdOTItem;
                CustomerName = selectedActionPlanTask.CustomerName;
                FillEmdepSitesDetail();
                PreviousParentTaskNumber = selectedActionPlanTask.ParentTaskNumber;
                PreviousSubTaskCode = selectedActionPlanTask.SubTaskCode;
                if (APMCommon.Instance.TaskList != null)
                {
                    if (selectedActionPlanTask != null)
                    {
                        SelectedTask = APMCommon.Instance.TaskList.FirstOrDefault(x => x.IdActionPlanTask == selectedActionPlanTask.IdParent);
                    }
                }
                SetUserPermission();       //[shweta.thube][GEOS2-9910][16.10.2025]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDelegatedToList(string idCompanies)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDelegatedToList ...", category: Category.Info, priority: Priority.Low);

                APMCommon.Instance.DelegatedToList = new List<Responsible>(APMService.GetEmployeeListAsPerLocation_V2600(idCompanies));

                APMCommon.Instance.DelegatedToList = new List<Responsible>(APMCommon.Instance.DelegatedToList);

                GeosApplication.Instance.Logger.Log("Method FillDelegatedToList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDelegatedToList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDelegatedToList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDelegatedToList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
       
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

                List<int> idCompanies = APMCommon.Instance.LocationList.Select(x => x.IdCompany).ToList();

                // Convert the list of integers into a comma-separated string
                string idCompanyString = string.Join(",", idCompanies);

                if (APMCommon.Instance.DelegatedToList == null)
                {
                    FillDelegatedToList(idCompanyString);
                }

                DelegatedToList = new ObservableCollection<Responsible>(APMCommon.Instance.DelegatedToList.Where(x => x.IdOrganizationCode == SelectedLocation.IdCompany));
              
                string idCompany = Convert.ToString(SelectedLocation.IdCompany);
                if (PreviousSelectedLocation != null)
                {
                    if (SelectedLocation.IdCompany != PreviousSelectedLocation.IdCompany)
                    {
                        FillResponsibleList(idCompany);
                    }
                }
                else
                {
                    FillResponsibleList(idCompany);
                }

                if (!IsNew)
                {
                    if (PreviousSelectedLocation.IdCompany == SelectedLocation.IdCompany)
                    {
                        if (!DelegatedToList.Any(x => x.IdEmployee == InactiveSelectedDelegatedToIdEmployee) && InactiveSelectedDelegatedToIdEmployee != 0)
                        {
                            Responsible temp = APMService.GetInactiveDelegatedToForActionPlanTask_V2570(InactiveSelectedDelegatedToIdEmployee);
                            temp.IsInActive = true;
                            temp.EmployeeCodeWithIdGender = temp.EmployeeCode + "_" + temp.IdGender;
                            DelegatedToList.Add(temp);
                            SelectedDelegatedto = temp;
                        }
                        if (DelegatedToList != null)
                        {
                            SelectedDelegatedto = DelegatedToList.FirstOrDefault(x => x.IdEmployee == PreviousIdDelegatedTo);
                            PreviousSelectedDelegatedto = SelectedDelegatedto;
                        }
                        if (!ResponsibleList.Any(x => x.IdEmployee == InactiveSelectedResponsibleToIdEmployee) && InactiveSelectedResponsibleToIdEmployee != 0 && PreviousSelectedResponsible != null)  //[GEOS2-6021][rdixit][17.02.2025]
                        {
                            Responsible temp = APMService.GetInactiveResponsibleForActionPlan_V2560(InactiveSelectedResponsibleToIdEmployee);
                            temp.IsInActive = true;
                            temp.EmployeeCodeWithIdGender = temp.EmployeeCode + "_" + temp.IdGender;
                            ResponsibleList.Add(temp);

                            SelectedResponsible = ResponsibleList.FirstOrDefault(x => x.IdEmployee == PreviousSelectedResponsible.IdEmployee);
                        }
                        else
                        {
                            SelectedResponsible = ResponsibleList.FirstOrDefault(x => x.IdEmployee == InactiveSelectedResponsibleToIdEmployee);
                        }
                    }
                    else
                    {
                        SelectedDelegatedto = null;
                        SelectedResponsible = null;
                    }
                }


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in LocationListClosedCommandAction() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
       
        private void FillTaskList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTaskList ...", category: Category.Info, priority: Priority.Low);
                string idPeriods = string.Empty;
                if (APMCommon.Instance.SelectedPeriod != null)
                {
                    List<long> selectedPeriod = APMCommon.Instance.SelectedPeriod.Cast<long>().ToList();
                    idPeriods = string.Join(",", selectedPeriod);
                }
                //APMService = new APMServiceController("localhost:6699");
                TaskList = new ObservableCollection<APMActionPlanTask>(APMService.GetTaskListByIdActionPlan_V2650(IdActionPlan, idPeriods, GeosApplication.Instance.ActiveUser.IdUser));

                GeosApplication.Instance.Logger.Log("Method FillTaskList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTaskList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTaskList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTaskList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillResponsibleList(string idCompanies)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillResponsibleList ...", category: Category.Info, priority: Priority.Low);
                
                string idPeriods = string.Empty;
                if (APMCommon.Instance.SelectedPeriod != null)
                {
                    List<long> selectedPeriod = APMCommon.Instance.SelectedPeriod.Cast<long>().ToList();
                    idPeriods = string.Join(",", selectedPeriod);
                }
                //Service GetResponsibleListAsPerLocation_V2600 changed to GetResponsibleListAsPerLocation_V2670 [rdixit][GEOS2-9354][01.09.2025]
                var temp = APMService.GetResponsibleListAsPerLocation_V2670(idCompanies);

                ResponsibleList = new List<Responsible>();
                ResponsibleList.AddRange(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                TempResponsibleList = new List<Responsible>(temp);
                ResponsibleList = new List<Responsible>(ResponsibleList);

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

        public void ChooseFileActionCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFile() ...", category: Category.Info, priority: Priority.Low);
           
            IsBusy = true;
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".*";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    FileInfo file = new FileInfo(dlg.FileName);
                    FileName = file.FullName;
                    var newFileList = AttachmentObjectList != null ? new ObservableCollection<AttachmentsByTask>(AttachmentObjectList) : new ObservableCollection<AttachmentsByTask>();
                    UniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    Attachment = new AttachmentsByTask();
                    Attachment.FileByte = System.IO.File.ReadAllBytes(dlg.FileName);
                    Attachment.FileType = file.Extension;
                    if (file.Name.Contains("."))
                    {
                        string[] a = file.Name.Split('.');
                        FileNameString = a[0];
                    }
                    else
                    {
                        FileNameString = file.Name;
                    }
                    Attachment.FilePath = file.FullName;
                    Attachment.OriginalFileName = FileNameString;
                    Attachment.SavedFileName = file.Name;
                    Attachment.CreatedIn = GeosApplication.Instance.ServerDateTime;
                    Attachment.FileSize = file.Length;
                    Attachment.FileType = file.Extension;
                    Attachment.FileUploadName = file.Name;
                    Attachment.IsUploaded = true;

                    Attachment.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    Attachment.CreatedByName = GeosApplication.Instance.ActiveUser.FullName;
                    Attachment.CreatedIn = DateTime.Now;

                    var theIcon = IconFromFilePath(FileName);
                    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\Images\";
                    if (theIcon != null)
                    {
                        // Save it to disk, or do whatever you want with it.
                        if (!Directory.Exists(tempPath))
                        {
                            System.IO.Directory.CreateDirectory(tempPath);
                        }

                        if (!File.Exists(tempPath + UniqueFileName + file.Extension + ".ico"))
                        {
                            using (var stream = new System.IO.FileStream(tempPath + UniqueFileName + file.Extension + ".ico", System.IO.FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                theIcon.Save(stream);
                                stream.Close();
                                stream.Dispose();
                            }
                        }
                        theIcon.Dispose();
                    }

                    // useful to get icon end process of temp. used imgage 
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(tempPath + UniqueFileName + file.Extension + ".ico", UriKind.RelativeOrAbsolute);
                    image.EndInit();
                    Attachment.AttachmentImage = image;

                    // not allow to add same files
                    List<AttachmentsByTask> fooList = newFileList.OfType<AttachmentsByTask>().ToList();
                    if (!fooList.Any(x => x.OriginalFileName == Attachment.OriginalFileName))
                    {
                        newFileList.Add(Attachment);
                    }
                    AttachmentObjectList = newFileList;
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            GeosApplication.Instance.Logger.Log("Method BrowseFile() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public static Icon IconFromFilePath(string filePath)
        {
            var result = (Icon)null;

            try
            {
                result = Icon.ExtractAssociatedIcon(filePath);
            }
            catch (System.Exception)
            {
                // swallow and return nothing. You could supply a default Icon here as well
            }

            return result;
        }
        public void FindOTItemCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FindOTItemCommandAction ...", category: Category.Info, priority: Priority.Low);
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
                IsSelectButtonEnabled = false;
                FindOTItemHeader = string.Format(System.Windows.Application.Current.FindResource("LabelFindOtItem").ToString(), CustomerName);
                OTsList = new ObservableCollection<OTs>();
                FromDate = DateTime.Now.AddYears(-1);
                ToDate = DateTime.Now.Date;
                OtItemVisibility = Visibility.Visible;


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FindOTItemCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FindOTItemCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FindOTItemCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FindOTItemCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
       
        private void OtItemCancelButtonCommandAction(object obj)
        {
            try
            {
                OtItemVisibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OtItemCancelButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
      
        private void FillEmdepSitesDetail()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmdepSitesDetail()...", category: Category.Info, priority: Priority.Low);

                if (APMCommon.Instance.CommonAllPlantList == null || APMCommon.Instance.CommonAllPlantList.Count == 0)
                {
                    APMCommon.Instance.CommonAllPlantList = new List<UserSiteGeosServiceProvider>(CrmStartUp.GetAllCompaniesWithServiceProvider(GeosApplication.Instance.ActiveUser.IdUser));
                }
                if (AllPlantsList == null || AllPlantsList.Count == 0)
                {
                    AllPlantsList = new List<UserSiteGeosServiceProvider>();
                }
                AllPlantsList = APMCommon.Instance.CommonAllPlantList;


                GeosApplication.Instance.Logger.Log("Method FillEmdepSitesDetail() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmdepSitesDetail() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmdepSitesDetail() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillEmdepSitesDetail() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
    
        private void FillOTAsPerTaskLocation()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOTAsPerTaskLocation()...", category: Category.Info, priority: Priority.Low);


                if (AllPlantsList != null && AllPlantsList.Count > 0)
                {
                    var selected = AllPlantsList.FirstOrDefault(x => x.ShortName == SelectedLocation.Alias);


                    IAPMService APMServiceToGetOts = new APMServiceController(selected.ServiceProviderUrl);
                   
                    OTsList = new ObservableCollection<OTs>(APMServiceToGetOts.GetOTsAsPerTaskLocation_V2620((Int32)IdSite, FromDate, ToDate));

                    if (OTsList.Count <= 0)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NoOTItems").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NoOTItems").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                }

                GeosApplication.Instance.Logger.Log("Method FillOTAsPerTaskLocation() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOTAsPerTaskLocation() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOTAsPerTaskLocation() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOTAsPerTaskLocation() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

       
        private void OTsLoadCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method OTsLoadCommandAction ()...", category: Category.Info, priority: Priority.Low);
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
                IsSelectButtonEnabled = false;

                FillOTAsPerTaskLocation();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method OTsLoadCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OTsLoadCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OTsLoadCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OTsLoadCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
       
        public void SelectButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectButtonCommandAction ...", category: Category.Info, priority: Priority.Low);
                if (SelectedOTItem != null)
                {
                    OTItem = SelectedOTItem.CodeNumber;
                }
                OtItemVisibility = Visibility.Collapsed;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SelectButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SelectButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FindOTItemCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FindOTItemCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[shweta.thube][GEOS2-9910][16.10.2025]
        private void SetUserPermission()
        {
            if (IsNew)
            {
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 122 && up.Permission.IdGeosModule == 15))// admin
                {
                    IsAddButtonEnabled = true;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 134 && up.Permission.IdGeosModule == 15)) // Location Manager
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
            }
            else
            {
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 122 && up.Permission.IdGeosModule == 15))// admin
                {
                    IsAddButtonEnabled = true;
                    StatusList.FirstOrDefault(x => x.Value == "Done").InUse = true;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 134 && up.Permission.IdGeosModule == 15)) // Location Manager
                {
                    IsAddButtonEnabled = true;
                    StatusList.FirstOrDefault(x => x.Value == "Done").InUse = false;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 135 && up.Permission.IdGeosModule == 15))// Department Manager
                {
                    IsAddButtonEnabled = true;
                    StatusList.FirstOrDefault(x => x.Value == "Done").InUse = false;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 136 && up.Permission.IdGeosModule == 15))// Base User
                {
                    IsAddButtonEnabled = true;
                    StatusList.FirstOrDefault(x => x.Value == "Done").InUse = false;
                }
                else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 121 && up.Permission.IdGeosModule == 15))//View
                {
                    IsAddButtonEnabled = false;
                    StatusList.FirstOrDefault(x => x.Value == "Done").InUse = false;
                }
            }
        }
        #endregion

        #region Validation
        #region 
        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }
        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error = me[BindableBase.GetPropertyName(() => Title)] +
                    me[BindableBase.GetPropertyName(() => SelectedLocation)] +
                    me[BindableBase.GetPropertyName(() => SelectedResponsible)] +
                    me[BindableBase.GetPropertyName(() => SelectedStatus)] +
                     me[BindableBase.GetPropertyName(() => SelectedPriority)] +
                    //me[BindableBase.GetPropertyName(() => SelectedBusinessUnit)] +
                    me[BindableBase.GetPropertyName(() => SelectedTheme)] +
                    me[BindableBase.GetPropertyName(() => Progress)] +
                    me[BindableBase.GetPropertyName(() => TaskStatusComment)];


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
                string TitleProp = BindableBase.GetPropertyName(() => Title);
                string SelectedIndexLocationProp = BindableBase.GetPropertyName(() => SelectedLocation);
                string SelectedIndexResponsibleProp = BindableBase.GetPropertyName(() => SelectedResponsible);
                string SelectedIndexStatusProp = BindableBase.GetPropertyName(() => SelectedStatus);
                string SelectedIndexPriorityProp = BindableBase.GetPropertyName(() => SelectedPriority);
                //string SelectedIndexBusinessUnitProp = BindableBase.GetPropertyName(() => SelectedBusinessUnit);
                string SelectedIndexThemeProp = BindableBase.GetPropertyName(() => SelectedTheme);

                string ProgressProp = BindableBase.GetPropertyName(() => Progress);
                string taskStatusComment = BindableBase.GetPropertyName(() => TaskStatusComment);

                if (columnName == TitleProp)
                {
                    return AddEditTaskValidation.GetErrorMessage(TitleProp, Title);
                }
                if (columnName == SelectedIndexLocationProp)
                {
                    return AddEditTaskValidation.GetErrorMessage(SelectedIndexLocationProp, SelectedLocation);
                }

                if (columnName == SelectedIndexResponsibleProp)
                {
                    return AddEditTaskValidation.GetErrorMessage(SelectedIndexResponsibleProp, SelectedResponsible);
                }
                if (columnName == SelectedIndexStatusProp)
                {
                    return AddEditTaskValidation.GetErrorMessage(SelectedIndexStatusProp, SelectedStatus);
                }
                if (columnName == SelectedIndexPriorityProp)
                {
                    return AddEditTaskValidation.GetErrorMessage(SelectedIndexPriorityProp, SelectedPriority);
                }
                
                if (columnName == SelectedIndexThemeProp)
                {
                    return AddEditTaskValidation.GetErrorMessage(SelectedIndexThemeProp, SelectedTheme);
                }
                if (!GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 122 && up.Permission.IdGeosModule == 15)
                    && SelectedActionPlans.IdEmployee != SelectedResponsible?.IdEmployee
                    && columnName == ProgressProp && IsTaskProgressValidation)
                {
                    return AddEditTaskValidation.GetErrorMessage(ProgressProp, Progress);
                }

                if (columnName == taskStatusComment && IsStatusFieldsVisible == Visibility.Visible) 
                {
                    return AddEditTaskValidation.GetErrorMessage(taskStatusComment, TaskStatusComment);
                }


                return null;
            }
        }
        #endregion
        #endregion
    }
}


