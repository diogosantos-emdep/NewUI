using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using System.ServiceModel;
using Emdep.Geos.Data.Common.Epc;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Grid;
using System.Globalization;

/// <summary>
/// Sprint 41--HRM	M041-08	New configuration section Job Descriptions--To add new Job Descriptions ---sdesai
/// </summary>
namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddNewJobDescriptionsViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // IHrmService HrmService = new HrmServiceController("localhost:6699");
        //ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration

        private string windowHeader;
        private bool isNew;
        private bool IsBusy;
        private byte[] jobDescriptionFileInBytes;
        private List<object> attachmentList;
        private string jobDescriptionFileName;
        private bool isSave;
        private string jobDescriptionCode;
        private string jobDescriptionTitle;
        private bool inUse;
        private int selectedIndexDepartment;
        private int selectedIndexScope;
        private ObservableCollection<JobDescription> existJobDescription;
        private int Use;
        private string childOrientation;
        JobDescription _JobDescription;
        private string jobDescriptionAbbreviation;
        private int selectedParentIndex;
        private int selectedIndexLevel;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        private bool isMandatory;
        private int Mandatory;
        private List<JobDescription> isMandatoryNotExistInJobDescriptionParent;
        private uint idJobDescription;
        private bool updateIsMandatoryInJobDescriptionParent;
        private bool isRemote;
        private int Remote;

        public List<Department> departmentList;
        public ObservableCollection<LogNewJobDescription> tempJobChangeLog;
        public GeosModule geosModule;
        public LookupValue lookup;
        public List<JobDescription> listofChangelog;
        public List<LogNewJobDescription> listJobChangeLog;
        public int lookupValueId;
        public ObservableCollection<LookupValue> lookuplist;

        public string HealthAndSafetyHeader { get; set; }//[Sudhir.Jangra][GEOS2-5549]
        public string EquipmentAndToolsHeader { get; set; }//[Sudhir.Jangra][GEOS2-5549]
        private Visibility isHealthAndSafetyVisible;//[Sudhir.Jangra][GEOS2-5549]
        private Visibility isEquipmentAndToolsVisible;//[Sudhir.Jangra][GEOS2-5549]
        private ObservableCollection<HealthAndSafetyAttachedDoc> healthAndSafetyFilesList;//[Sudhir.jangra][GEOS2-5549]
        private HealthAndSafetyAttachedDoc selectedHealthAndSafetyFile;//[Sudhir.Jangra][GEOS2-5549]
        private ObservableCollection<EquipmentAndToolsAttachedDoc> equipmentAndToolsFilesList;//[Sudhir.jangra][GEOS2-5549]
        private EquipmentAndToolsAttachedDoc selectedEquipmentAndToolsFile;//[Sudhir.Jangra][GEOS2-5549]
        private ObservableCollection<HealthAndSafetyAttachedDoc> clonedHealthAndSafetyAttachment;//pramod.misal 17.06.2024
        public bool isEnabledCancelButton = false;//pramod.misal 17.06.2024
        private List<EquipmentAndToolsAttachedDoc> clonedEquipmentAndToolsList;
        private string shortName;
        #endregion

        #region Properties

        public string OldJobDescriptionCode { get; set; }
        public string OldJobDescriptionFileName { get; set; }
        public bool IsJobDescriptionFileUpdated { get; set; }

        public List<JobDescription> JobDescriptionListParent { get; set; }
        public JobDescription NewJobDescription { get; set; }
        public JobDescription UpdateJobDescription { get; set; }

        public List<LookupValue> JDLevelList { get; set; }
        public List<LookupValue> JDScopeList { get; set; }
        //public List<String> JDScopeList { get; set; }

        //rajashri.telvekar[GEOS-3693][10-11-2023]
        public int LookupValueId
        {
            get { return lookupValueId; }
            set
            {
                lookupValueId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LookupValueId"));
            }
        }
        public ObservableCollection<LogNewJobDescription> TempJobChangeLog
        {
            get { return tempJobChangeLog; }
            set
            {
                tempJobChangeLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempJobChangeLog"));
            }
        }
        public ObservableCollection<LookupValue> Lookuplist
        {
            get { return lookuplist; }
            set
            {
                lookuplist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Lookuplist"));
            }
        }
        public List<LogNewJobDescription> ListJobChangeLog
        {
            get { return listJobChangeLog; }
            set
            {
                listJobChangeLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListJobChangeLog"));
            }
        }
        public List<JobDescription> ListofChangelog
        {
            get { return listofChangelog; }
            set
            {
                listofChangelog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListofChangelog"));
            }
        }
        public JobDescription Jobdata { get; set; }
        public JobDescription initialJobData { get; set; }
        public GeosModule GeosModuleData
        {
            get
            {
                return geosModule;
            }
            set
            {
                geosModule = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosModuleData"));
            }
        }
        public LookupValue LookupValueData
        {
            get
            {
                return lookup;
            }
            set
            {
                lookup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LookupValueData"));
            }
        }
        public List<Department> DepartmentList
        {
            get
            {
                return departmentList;
            }
            set
            {
                departmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DepartmentList"));
            }

        }

        public ObservableCollection<JobDescription> ExistJobDescription
        {
            get { return existJobDescription; }
            set
            {
                existJobDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistJobDescription"));
            }
        }

        public bool InUse
        {
            get { return inUse; }
            set
            {
                inUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InUse"));
            }
        }

        public string JobDescriptionCode
        {
            get { return jobDescriptionCode; }
            set
            {
                //if (value != null)
                //{
                jobDescriptionCode = value;//.TrimStart();
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionCode"));
                //}
            }
        }

        public string JobDescriptionTitle
        {
            get { return jobDescriptionTitle; }
            set
            {
                // if (value != null)
                // {
                jobDescriptionTitle = value;//.TrimStart();
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionTitle"));
                // }
            }
        }

        public byte[] JobDescriptionFileInBytes
        {
            get { return jobDescriptionFileInBytes; }
            set
            {
                jobDescriptionFileInBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionFileInBytes"));
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

        public string WorkingPlantId { get; set; }

        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public List<object> AttachmentList
        {
            get { return attachmentList; }
            set
            {
                attachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentList"));
            }
        }

        public string JobDescriptionFileName
        {
            get { return jobDescriptionFileName; }
            set
            {
                jobDescriptionFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionFileName"));
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

        public int SelectedIndexDepartment
        {
            get { return selectedIndexDepartment; }
            set
            {
                selectedIndexDepartment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexDepartment"));
            }
        }

        public int SelectedIndexScope
        {
            get { return selectedIndexScope; }
            set
            {
                selectedIndexScope = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexScope"));
            }
        }

        public string JobDescriptionAbbreviation
        {
            get { return jobDescriptionAbbreviation; }
            set
            {
                // if (value != null)
                //{
                jobDescriptionAbbreviation = value;//.TrimStart();
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionAbbreviation"));
                // }
            }
        }

        public int SelectedParentIndex
        {
            get { return selectedParentIndex; }
            set
            {
                selectedParentIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedParentIndex"));
            }
        }

        public string ChildOrientation
        {
            get { return childOrientation; }
            set
            {
                childOrientation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ChildOrientation"));
            }
        }

        public int SelectedIndexLevel
        {
            get { return selectedIndexLevel; }
            set
            {
                selectedIndexLevel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexLevel"));
            }
        }
        public bool IsReadOnlyField
        {
            get { return isReadOnlyField; }
            set
            {
                isReadOnlyField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnlyField"));
            }
        }

        public bool IsAcceptEnabled
        {
            get { return isAcceptEnabled; }
            set
            {
                isAcceptEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnabled"));
            }
        }

        public bool IsMandatory
        {
            get { return isMandatory; }
            set
            {
                isMandatory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMandatory"));
            }
        }
        public List<JobDescription> IsMandatoryNotExistInJobDescriptionParent
        {
            get { return isMandatoryNotExistInJobDescriptionParent; }
            set
            {
                isMandatoryNotExistInJobDescriptionParent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMandatoryNotExistInJobDescriptionParent"));
            }
        }
        public uint IdJobDescription
        {
            get { return idJobDescription; }
            set
            {
                idJobDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdJobDescription"));
            }
        }
        public bool UpdateIsMandatoryInJobDescriptionParent
        {
            get { return updateIsMandatoryInJobDescriptionParent; }
            set
            {
                updateIsMandatoryInJobDescriptionParent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateIsMandatoryInJobDescriptionParent"));
            }
        }

        public bool IsRemote
        {
            get { return isRemote; }
            set
            {
                isRemote = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRemote"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5549]
        public Visibility IsHealthAndSafetyVisible
        {
            get { return isHealthAndSafetyVisible; }
            set
            {
                isHealthAndSafetyVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsHealthAndSafetyVisible"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5549]
        public Visibility IsEquipmentAndToolsVisible
        {
            get { return isEquipmentAndToolsVisible; }
            set
            {
                isEquipmentAndToolsVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEquipmentAndToolsVisible"));
            }
        }

        //[Sudhir.jangra][GEOS2-5549]
        public ObservableCollection<HealthAndSafetyAttachedDoc> HealthAndSafetyFilesList
        {
            get { return healthAndSafetyFilesList; }
            set
            {
                healthAndSafetyFilesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HealthAndSafetyFilesList"));
            }
        }

        //[Sudhir.Jangra][GEOS2-5549]
        public HealthAndSafetyAttachedDoc SelectedHealthAndSafetyFile
        {
            get { return selectedHealthAndSafetyFile; }
            set
            {
                selectedHealthAndSafetyFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedHealthAndSafetyFile"));
            }
        }

        //[Sudhir.jangra][GEOS2-5549]
        public ObservableCollection<EquipmentAndToolsAttachedDoc> EquipmentAndToolsFilesList
        {
            get { return equipmentAndToolsFilesList; }
            set
            {
                equipmentAndToolsFilesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EquipmentAndToolsFilesList"));
            }
        }
        //[Sudhir.jangra][GEOS2-5549]
        public EquipmentAndToolsAttachedDoc SelectedEquipmentAndToolsFile
        {
            get { return selectedEquipmentAndToolsFile; }
            set
            {
                selectedEquipmentAndToolsFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEquipmentAndToolsFile"));
            }
        }

        //pramod.misal 17.06.2024
        public ObservableCollection<HealthAndSafetyAttachedDoc> ClonedHealthAndSafetyAttachment
        {
            get
            {
                return clonedHealthAndSafetyAttachment;
            }

            set
            {
                clonedHealthAndSafetyAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedHealthAndSafetyAttachment"));
            }
        }

        public bool IsEnabledCancelButton
        {
            get { return isEnabledCancelButton; }
            set
            {
                isEnabledCancelButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledCancelButton"));
            }
        }



        public List<EquipmentAndToolsAttachedDoc> ClonedEquipmentAndToolsList

        {

            get { return clonedEquipmentAndToolsList; }

            set

            {

                clonedEquipmentAndToolsList = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ClonedEquipmentAndToolsList"));

            }

        }

        public string ShortName
        {
            get { return shortName; }
            set
            {
                shortName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShortName"));
            }
        }
        #endregion

        #region Public ICommand

        public ICommand AddJobDescriptionsViewCancelButtonCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        public ICommand AddJobDescriptionButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }

        public ICommand AddHealthAndSafetyFileCommand { get; set; }//[Sudhir.Jangra][GEOS2-5549]
        public ICommand AddEquipmentAndToolsFileCommand { get; set; }//[Sudhir.Jangra][GEOS2-5549]

        public ICommand OpenPDFDocumentCommand { get; set; }//[Sudhir.Jangra][GEOS2-5549]

        public ICommand EditFileCommand { get; set; }//[Sudhir.Jangra][GEOS2-5549]

        public ICommand OpenEquipmentPDFDocumentCommand { get; set; }

        public ICommand EditEquipmentFileCommand { get; set; }

        public ICommand DeleteEquipmentFileCommand { get; set; }

        public ICommand DeleteHealthFileCommand { get; set; }
        #endregion

        #region public Events

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

        #region Constructor
        public AddNewJobDescriptionsViewModel()
        {
            try
            {

                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor AddNewJobDescriptionsViewModel()...", category: Category.Info, priority: Priority.Low);
                AddJobDescriptionsViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFileAction));
                AddJobDescriptionButtonCommand = new RelayCommand(new Action<object>(AddJobDescription));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                AddHealthAndSafetyFileCommand = new RelayCommand(new Action<object>(AddHealthAndSafetyFileCommandAction));
                AddEquipmentAndToolsFileCommand = new RelayCommand(new Action<object>(AddEquipmentAndToolsFileCommandAction));
                OpenPDFDocumentCommand = new RelayCommand(new Action<object>(OpenPDFDocument));
                EditFileCommand = new RelayCommand(new Action<object>(EditFile));
                OpenEquipmentPDFDocumentCommand = new RelayCommand(new Action<object>(OpenEquipmentPDFDocument));
                EditEquipmentFileCommand = new RelayCommand(new Action<object>(EditEquipmentFile));
                DeleteEquipmentFileCommand = new RelayCommand(new Action<object>(DeleteEquipmentFile));
                DeleteHealthFileCommand = new RelayCommand(new Action<object>(DeleteHealthFile));

                HealthAndSafetyHeader = string.Format(System.Windows.Application.Current.FindResource("HealthAndSafetyHeader").ToString(), "&");
                EquipmentAndToolsHeader = string.Format(System.Windows.Application.Current.FindResource("EquipmentAndToolsHeader").ToString(), "&");



                GeosApplication.Instance.Logger.Log("Constructor AddNewJobDescriptionsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddNewJobDescriptionsViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method to Close Window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        /// <summary>in
        /// Method to Browse File Action
        /// </summary>
        /// <param name="obj"></param>
        public void BrowseFileAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFile() ...", category: Category.Info, priority: Priority.Low);
            if (obj != null)
            {
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                IsBusy = true;
            }
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = "Pdf Files|*.pdf";
                dlg.Filter = "Pdf Files|*.pdf";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    JobDescriptionFileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    AttachmentList = new List<object>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    JobDescriptionFileName = file.Name;

                    List<object> newAttachmentList = new List<object>();

                    Attachment attachment = new Attachment();
                    attachment.FilePath = file.FullName;
                    attachment.OriginalFileName = file.Name;
                    attachment.IsDeleted = false;
                    attachment.FileByte = JobDescriptionFileInBytes;

                    newAttachmentList.Add(attachment);
                    AttachmentList = newAttachmentList;
                }
                if (obj != null)
                {
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                if (obj != null)
                {
                    IsBusy = false;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method BrowseFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method BrowseFile() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method to save Job Description
        /// [001][spawar][04-03-2020][GEOS2-1918] Job Description Configuration
        /// [002][avpawar][GEOS2-2438][Identify the Job descriptions working remotely]
        /// [003][cpatil][GEOS2-4043][When create New JD from configuration menu its show error]
        /// </summary>
        /// <param name="obj"></param>
        private void AddJobDescription(object obj)
        {
            try
            {
                
                GeosApplication.Instance.Logger.Log("Method AddJobDescription()...", category: Category.Info, priority: Priority.Low);
                string error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("JobDescriptionCode"));
                PropertyChanged(this, new PropertyChangedEventArgs("JobDescriptionTitle"));
                PropertyChanged(this, new PropertyChangedEventArgs("JobDescriptionAbbreviation"));//rajashri
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexDepartment"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexLevel"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexScope"));

                if (error != null)
                {
                    return;
                }
                //[rahul.gadhave][GEOS2-6449][Date:11 - 10 - 2024]
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }



                if (IsMandatory && (IdJobDescription > 0 || JobDescriptionListParent[SelectedParentIndex].IdJobDescription > 0))
                {
                    if (IdJobDescription > 0)
                    {
                        IsMandatoryNotExistInJobDescriptionParent = HrmService.GetIsMandatoryNotExistInJobDescriptionParent(Convert.ToInt32(IdJobDescription));
                    }
                    else if (JobDescriptionListParent[SelectedParentIndex].IdJobDescription > 0)
                    {
                        IsMandatoryNotExistInJobDescriptionParent = HrmService.GetIsMandatoryNotExistInJobDescriptionParent(Convert.ToInt32(JobDescriptionListParent[SelectedParentIndex].IdJobDescription));
                    }

                    if (IsMandatoryNotExistInJobDescriptionParent.Count() > 0)
                    {
                        List<string> isMandatoryNotExistInJobDescriptionParent = new List<string>();
                        foreach (JobDescription JobDescription in IsMandatoryNotExistInJobDescriptionParent)
                        {
                            isMandatoryNotExistInJobDescriptionParent.Add(JobDescription.JobDescriptionTitleAndCode);
                        }

                        MessageBoxResult result = CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("JobDescriptionIsMandatoryNotExistInParent").ToString(), string.Join(", ", isMandatoryNotExistInJobDescriptionParent.ToList())), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.Yes)
                        {
                            if (SelectedParentIndex > 0 && JobDescriptionListParent[SelectedParentIndex].IdJobDescription > 0)
                            {
                                //[003] changes service method
                                // Update IsMandatory="Yes" In Job Description Parent if IsMandatory="No"
                                UpdateIsMandatoryInJobDescriptionParent = HrmService.UpdateIsMandatoryInJobDescriptionParentByIdParent(Convert.ToInt32(JobDescriptionListParent[SelectedParentIndex].IdJobDescription));
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                if (InUse)
                {
                    Use = 1;
                }
                else
                {
                    Use = 0;
                }

                //[001]
                if (IsMandatory)
                {
                    Mandatory = 1;
                }
                else
                {
                    Mandatory = 0;
                }
                //

                //[002] Added
                if (IsRemote)
                {
                    Remote = 1;
                }
                else
                {
                    Remote = 0;
                }
                //[002] End

                List<Attachment> temp = new List<Attachment>();

                if (AttachmentList != null && AttachmentList.Count == 0)
                {
                    JobDescriptionFileName = null;
                    JobDescriptionFileInBytes = null;
                }

                //if (!string.IsNullOrEmpty(JobDescriptionCode))
                //    JobDescriptionCode = JobDescriptionCode.Trim();

                //if (!string.IsNullOrEmpty(JobDescriptionTitle))
                //    JobDescriptionTitle = JobDescriptionTitle.Trim();

                //if (!string.IsNullOrEmpty(JobDescriptionAbbreviation))
                //    JobDescriptionAbbreviation = JobDescriptionAbbreviation.Trim();

                JobDescription TempJobDescription = new JobDescription();
                var TempExistJobDescription = ExistJobDescription.Where(x => x.JobDescriptionCode == JobDescriptionCode);
                TempJobDescription = TempExistJobDescription.FirstOrDefault(x => x.JobDescriptionCode == JobDescriptionCode);


                // FillLogData(_JobDescription.IdJobDescription);
                if (isNew)
                {

                    if (TempJobDescription == null)
                    {

                        NewJobDescription = new JobDescription()
                        {
                            JobDescriptionCode = JobDescriptionCode,
                            JobDescriptionTitle = JobDescriptionTitle,
                            IdDepartment = DepartmentList[SelectedIndexDepartment].IdDepartment,
                            Department = DepartmentList[SelectedIndexDepartment],
                            Abbreviation = JobDescriptionAbbreviation,
                            ParentJobDescription = JobDescriptionListParent[SelectedParentIndex],
                            IdParent = JobDescriptionListParent[SelectedParentIndex].IdJobDescription,
                            JobDescriptionFileName = JobDescriptionFileName,
                            JobDescriptionFileInBytes = JobDescriptionFileInBytes,
                            ChildOrientation = ChildOrientation,
                            JobDescriptionInUse = (byte)Use,
                            //[001]
                            JobDescriptionIsMandatory = (byte)Mandatory,
                            //
                            JobDescriptionIsRemote = (byte)Remote, //[002] Added
                            IdJDLevel = JDLevelList[SelectedIndexLevel].IdLookupValue == 0 ? default(int?) : JDLevelList[SelectedIndexLevel].IdLookupValue,
                            JDLevel = JDLevelList[SelectedIndexLevel],
                            //sjadhav
                            IdJdScope = JDScopeList[SelectedIndexScope].IdLookupValue == 0 ? default(int) : JDScopeList[SelectedIndexScope].IdLookupValue,
                            JDScope = JDScopeList[SelectedIndexScope],

                        };
                        AddJobChangeLogForAdd(NewJobDescription);

                        // NewJobDescription = HrmService.AddJobDescription_V2046(NewJobDescription);
                        NewJobDescription = HrmService.AddJobDescription_V2470(NewJobDescription);//rajashri GEOS2-3692
                        IsSave = true;
                        if (IsSave)
                        {
                            Notification notification = new Notification();
                            notification.FromUser = GeosApplication.Instance.ActiveUser.IdUser;
                            notification.Title = System.Windows.Application.Current.FindResource("AddNewJobDescriptionViewTitle").ToString(); // "New Job Description";
                            notification.Message = string.Format(System.Windows.Application.Current.FindResource("AddNewJobDescriptionNotification").ToString(), NewJobDescription.JobDescriptionCode, NewJobDescription.ParentJobDescription.JobDescriptionTitle, GeosApplication.Instance.ActiveUser.FullName);
                            notification.IdModule = 7;
                            notification.Status = "Unread";
                            notification.IsNew = 1;

                            MailTemplateFormat mailTemplateFormat = new MailTemplateFormat();
                            if (NewJobDescription.ParentJobDescription.JobDescriptionTitle == "---")
                            {
                                mailTemplateFormat.ParentName = "None";
                            }
                            else
                            {
                                mailTemplateFormat.ParentName = NewJobDescription.ParentJobDescription.JobDescriptionTitle;
                            }

                            mailTemplateFormat.CreatedOrUpdated = "Created";
                            mailTemplateFormat.CreatedUser = GeosApplication.Instance.ActiveUser.FullName;
                            mailTemplateFormat.CreatedByEmail = GeosApplication.Instance.ActiveUser.CompanyEmail;
                            notification.MailTemplateFormat = mailTemplateFormat;
                            notification = HrmService.AddCommonNotification(notification);
                        }

                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("JobDescriptionInformationAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                    else
                    {
                        IsSave = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("JobDescriptionCodeExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {

                    if (TempJobDescription == null || TempJobDescription.IdJobDescription == _JobDescription.IdJobDescription)
                    {
                        if (OldJobDescriptionFileName == JobDescriptionFileName)
                        {
                            IsJobDescriptionFileUpdated = false;
                        }
                        else
                        {
                            IsJobDescriptionFileUpdated = true;
                        }

                        UpdateJobDescription = new JobDescription()
                        {
                            OldJobDescriptionCode = OldJobDescriptionCode,
                            JobDescriptionCode = JobDescriptionCode,
                            JobDescriptionTitle = JobDescriptionTitle,
                            Abbreviation = JobDescriptionAbbreviation,
                            ParentJobDescription = JobDescriptionListParent[SelectedParentIndex],
                            IdParent = JobDescriptionListParent[SelectedParentIndex].IdJobDescription,
                            IdDepartment = DepartmentList[SelectedIndexDepartment].IdDepartment,
                            Department = DepartmentList[SelectedIndexDepartment],
                            JobDescriptionFileName = JobDescriptionFileName,
                            IsJobDescriptionFileUpdated = IsJobDescriptionFileUpdated,
                            JobDescriptionFileInBytes = JobDescriptionFileInBytes,
                            ChildOrientation = ChildOrientation,
                            JobDescriptionInUse = (byte)Use,

                            //[001]
                            JobDescriptionIsMandatory = (byte)Mandatory,
                            //
                            JobDescriptionIsRemote = (byte)Remote, //[002] Added
                            IdJDLevel = JDLevelList[SelectedIndexLevel].IdLookupValue == 0 ? default(int?) : JDLevelList[SelectedIndexLevel].IdLookupValue,
                            JDLevel = JDLevelList[selectedIndexLevel],
                            //sjadhav
                            IdJdScope = JDScopeList[SelectedIndexScope].IdLookupValue == 0 ? default(int) : JDScopeList[SelectedIndexScope].IdLookupValue,
                            JDScope = JDScopeList[SelectedIndexScope],
                        };

                        UpdateJobDescription.IdJobDescription = _JobDescription.IdJobDescription;


                        AddJobChangeLog(_JobDescription, UpdateJobDescription);
                        //pramod.misal 17.06.2024

                        //Files
                        UpdateJobDescription.HealthAndSafetyAttachedDocList = new List<HealthAndSafetyAttachedDoc>();

                        //Add files 
                        foreach (HealthAndSafetyAttachedDoc item in HealthAndSafetyFilesList)
                        {
                            if (ClonedHealthAndSafetyAttachment == null)
                            {
                                ClonedHealthAndSafetyAttachment = new ObservableCollection<HealthAndSafetyAttachedDoc>();
                            }
                            if (!ClonedHealthAndSafetyAttachment.Any(x => x.IdJobDescriptionHealthAndSafetyAttachedDoc == item.IdJobDescriptionHealthAndSafetyAttachedDoc))
                            {
                                HealthAndSafetyAttachedDoc healthAndSafetyAttachedDoc = (HealthAndSafetyAttachedDoc)item.Clone();
                                healthAndSafetyAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdateJobDescription.HealthAndSafetyAttachedDocList.Add(healthAndSafetyAttachedDoc);
                                UpdateJobDescription.LogEntries.Add(
                                    new LogNewJobDescription()
                                    {
                                        IdGeosModule=7,
                                        IdObject=LookupValueId,
                                        Datetime=GeosApplication.Instance.ServerDateTime,
                                        IdUser=GeosApplication.Instance.ActiveUser.IdUser,
                                        Change=string.Format(System.Windows.Application.Current.FindResource("AddHealthAndSafetyChangeLog").ToString(), healthAndSafetyAttachedDoc.Title)
                                    });
                            }

                        }
                        //Deleted files
                        foreach (HealthAndSafetyAttachedDoc item in ClonedHealthAndSafetyAttachment)
                        {
                            if (HealthAndSafetyFilesList != null && !HealthAndSafetyFilesList.Any(x => x.IdJobDescriptionHealthAndSafetyAttachedDoc == item.IdJobDescriptionHealthAndSafetyAttachedDoc))
                            {
                                HealthAndSafetyAttachedDoc healthAndSafetyAttachedDoc = (HealthAndSafetyAttachedDoc)item.Clone();
                                healthAndSafetyAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdateJobDescription.HealthAndSafetyAttachedDocList.Add(healthAndSafetyAttachedDoc);
                                UpdateJobDescription.LogEntries.Add(
                                    new LogNewJobDescription()
                                    {
                                        IdGeosModule = 7,
                                        IdObject = LookupValueId,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                        Change = string.Format(System.Windows.Application.Current.FindResource("DeleteHealthAndSafetyChangeLog").ToString(), healthAndSafetyAttachedDoc.Title)
                                    });
                            }

                        }

                        //Update files

                        foreach (HealthAndSafetyAttachedDoc originalAttachedDoc in ClonedHealthAndSafetyAttachment)
                        {
                            if (HealthAndSafetyFilesList.Any(x => x.IdJobDescriptionHealthAndSafetyAttachedDoc == originalAttachedDoc.IdJobDescriptionHealthAndSafetyAttachedDoc))
                            {
                                HealthAndSafetyAttachedDoc updatedDoc = HealthAndSafetyFilesList.FirstOrDefault(x => x.IdJobDescriptionHealthAndSafetyAttachedDoc == originalAttachedDoc.IdJobDescriptionHealthAndSafetyAttachedDoc);
                                if ((updatedDoc.SavedFileName != originalAttachedDoc.SavedFileName)
                                    || (updatedDoc.OriginalFileName != originalAttachedDoc.OriginalFileName)
                                    || (updatedDoc.Remarks != originalAttachedDoc.Remarks) || (updatedDoc.Plant != originalAttachedDoc.Plant))
                                {
                                    HealthAndSafetyAttachedDoc HealthAndSafetyAttachedDoc = (HealthAndSafetyAttachedDoc)updatedDoc.Clone();
                                    HealthAndSafetyAttachedDoc.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    HealthAndSafetyAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdateJobDescription.HealthAndSafetyAttachedDocList.Add(HealthAndSafetyAttachedDoc);
                                    UpdateHealthAndSafetyChangeLog(originalAttachedDoc, HealthAndSafetyAttachedDoc, UpdateJobDescription);
                                }

                            }
                        }



                        //[Sudhir.Jangra][GEOS2-5549]

                        UpdateJobDescription.EquipmentAndToolsList = new List<EquipmentAndToolsAttachedDoc>();
                        if (EquipmentAndToolsFilesList == null)
                        {
                            EquipmentAndToolsFilesList = new ObservableCollection<EquipmentAndToolsAttachedDoc>();
                        }

                        //Delete Equipment

                        foreach (EquipmentAndToolsAttachedDoc item in ClonedEquipmentAndToolsList)
                        {
                            if (EquipmentAndToolsFilesList != null && !EquipmentAndToolsFilesList.Any(x => x.IdJobDescriptionEquipment == item.IdJobDescriptionEquipment))
                            {
                                EquipmentAndToolsAttachedDoc equipmentAndToolsAttachedDoc = (EquipmentAndToolsAttachedDoc)item.Clone();
                                equipmentAndToolsAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                UpdateJobDescription.EquipmentAndToolsList.Add(equipmentAndToolsAttachedDoc);

                                UpdateJobDescription.LogEntries.Add(
                                   new LogNewJobDescription()
                                   {
                                       IdGeosModule = 7,
                                       IdObject = LookupValueId,
                                       Datetime = GeosApplication.Instance.ServerDateTime,
                                       IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                       Change = string.Format(System.Windows.Application.Current.FindResource("DeleteEquipmentAndToolsChangeLog").ToString(), equipmentAndToolsAttachedDoc.EquipmentType)
                                   });
                            }
                        }
                        //Added
                        if (EquipmentAndToolsFilesList != null)
                        {
                            foreach (EquipmentAndToolsAttachedDoc item in EquipmentAndToolsFilesList)
                            {
                                if (!ClonedEquipmentAndToolsList.Any(x => x.IdJobDescriptionEquipment == item.IdJobDescriptionEquipment))
                                {
                                    EquipmentAndToolsAttachedDoc equipmentAndToolsAttachedDoc = (EquipmentAndToolsAttachedDoc)item.Clone();
                                    equipmentAndToolsAttachedDoc.TransactionOperation = ModelBase.TransactionOperations.Add;
                                    UpdateJobDescription.EquipmentAndToolsList.Add(equipmentAndToolsAttachedDoc);
                                    UpdateJobDescription.LogEntries.Add(
                                  new LogNewJobDescription()
                                  {
                                      IdGeosModule = 7,
                                      IdObject = LookupValueId,
                                      Datetime = GeosApplication.Instance.ServerDateTime,
                                      IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                      Change = string.Format(System.Windows.Application.Current.FindResource("AddEquipmentAndToolsChangeLog").ToString(), equipmentAndToolsAttachedDoc.EquipmentType)
                                  });
                                }
                            }
                        }
                        //Updated
                        foreach (EquipmentAndToolsAttachedDoc original in ClonedEquipmentAndToolsList)
                        {
                            if (EquipmentAndToolsFilesList != null && EquipmentAndToolsFilesList.Any(x => x.IdJobDescriptionEquipment == original.IdJobDescriptionEquipment))
                            {
                                EquipmentAndToolsAttachedDoc equipmentAndToolsAttachedDoc = EquipmentAndToolsFilesList.FirstOrDefault(x => x.IdJobDescriptionEquipment == original.IdJobDescriptionEquipment);
                                if ((equipmentAndToolsAttachedDoc.SavedFileName != original.SavedFileName) || (equipmentAndToolsAttachedDoc.OriginalFileName != original.OriginalFileName) || (equipmentAndToolsAttachedDoc.Remarks != original.Remarks) || (equipmentAndToolsAttachedDoc.IdLookupValue != original.IdLookupValue) || (equipmentAndToolsAttachedDoc.IsMandatory != original.IsMandatory) || (equipmentAndToolsAttachedDoc.StartDate != original.StartDate) || (equipmentAndToolsAttachedDoc.EndDate != original.EndDate))
                                {
                                    EquipmentAndToolsAttachedDoc equipmentAndToolsAttached = (EquipmentAndToolsAttachedDoc)equipmentAndToolsAttachedDoc.Clone();
                                    equipmentAndToolsAttached.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                    equipmentAndToolsAttached.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    UpdateJobDescription.EquipmentAndToolsList.Add(equipmentAndToolsAttached);
                                    UpdateEquipmentAndToolsChangeLog(original, equipmentAndToolsAttached, UpdateJobDescription);
                                }
                            }
                        }





                        //UpdateJobDescription = HrmService.UpdateJobDescription_V2040(UpdateJobDescription);



                        //UpdateJobDescription = HrmService.UpdateJobDescription_V2046(UpdateJobDescription);
                        //UpdateJobDescription = HrmService.UpdateJobDescription_V2470(UpdateJobDescription);//rajashri GEOS2-3692
                        //pramod.misal 17.06.2024
                       UpdateJobDescription = HrmService.UpdateJobDescription_V2530(UpdateJobDescription);

                        IsSave = true;
                        if (IsSave)
                        {
                            Notification notification = new Notification();
                            notification.FromUser = GeosApplication.Instance.ActiveUser.IdUser;
                            notification.Title = System.Windows.Application.Current.FindResource("AddEditJobDescriptionViewTitle").ToString(); // "New Job Description";
                            notification.Message = string.Format(System.Windows.Application.Current.FindResource("AddEditJobDescriptionNotification").ToString(), UpdateJobDescription.JobDescriptionCode, UpdateJobDescription.ParentJobDescription.JobDescriptionTitle, GeosApplication.Instance.ActiveUser.FullName);
                            notification.IdModule = 7;
                            notification.Status = "Unread";
                            notification.IsNew = 0;

                            MailTemplateFormat mailTemplateFormat = new MailTemplateFormat();
                            if (UpdateJobDescription.ParentJobDescription.JobDescriptionTitle == "---")
                            {
                                mailTemplateFormat.ParentName = UpdateJobDescription.JobDescriptionCode + " - " + "None";
                            }
                            else
                            {
                                mailTemplateFormat.ParentName = UpdateJobDescription.JobDescriptionCode + " - " + UpdateJobDescription.ParentJobDescription.JobDescriptionTitle;
                            }
                            mailTemplateFormat.CreatedOrUpdated = "Updated";
                            mailTemplateFormat.CreatedUser = GeosApplication.Instance.ActiveUser.FullName;
                            mailTemplateFormat.CreatedByEmail = GeosApplication.Instance.ActiveUser.CompanyEmail;
                            notification.MailTemplateFormat = mailTemplateFormat;
                            notification = HrmService.AddCommonNotification(notification);
                        }
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("JobDescriptionInformationUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                    else
                    {
                        IsSave = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("JobDescriptionCodeExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddJobDescription()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddJobDescription() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddJobDescription() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddJobDescription() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method to Fill Department List
        /// </summary>
        private void FillDepartmentList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDepartmentList()...", category: Category.Info, priority: Priority.Low);

                IList<Department> tempList = HrmService.GetAllDepartments();
                DepartmentList = new List<Department>();
                // [sshegaonkar][Geos-2733][13-o1-23]
                DepartmentList = new List<Department>(tempList.OrderBy(x => x.DepartmentName));
                DepartmentList.Insert(0, new Department() { DepartmentName = "---", IdDepartment = 0, DepartmentInUse = 1 });

                GeosApplication.Instance.Logger.Log("Method FillDepartmentList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantOpportunityList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillDepartmentList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        //sjadhav
        private void FillScopeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillScopeList()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempList = CrmService.GetLookupValues(59);
                JDScopeList = new List<LookupValue>(tempList);
                JDScopeList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });

                GeosApplication.Instance.Logger.Log("Method FillScopeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillScopeList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillScopeList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillScopeList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [Sprint-63][24/05/2019][psutar][GEOS2-1476] -Add new field Level in JD
        /// Method to Fill Level List
        /// </summary>
        private void FillLevelList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLevelList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempList = CrmService.GetLookupValues(42);
                JDLevelList = new List<LookupValue>();
                JDLevelList = new List<LookupValue>(tempList);
                JDLevelList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0, IdImage = 0 });

                GeosApplication.Instance.Logger.Log("Method FillLevelList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLevelList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLevelList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillLevelList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][spawar][04-03-2020][GEOS2-1918] Job Description Configuration</para>
        /// [002][avpawar][GEOS2-2438][Identify the Job descriptions working remotely]
        /// </summary>
        /// <param name="JobDescriptionListDetails"></param>
        public void Init(ObservableCollection<JobDescription> JobDescriptionListDetails)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                FillDepartmentList();
                FillLevelList();
                FillScopeList();
                ExistJobDescription = new ObservableCollection<JobDescription>(JobDescriptionListDetails);
                InUse = true;
                //[001]
                IsMandatory = true;
                //
                IsRemote = false; //[002] Added
                JobDescriptionListParent = ExistJobDescription.Select(x => (JobDescription)x.Clone()).ToList();
                JobDescriptionListParent.Insert(0, new JobDescription() { JobDescriptionTitle = "---", IdJobDescription = 0, JobDescriptionInUse = 1 });
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Edit job Description
        /// </summary>
        /// [001][spawar][04-03-2020][GEOS2-1918] Job Description Configuration</para>
        /// [002][avpawar][GEOS2-2438][Identify the Job descriptions working remotely]
        /// <param name="JobDescriptionListDetails"></param>
        /// <param name="jobDescription"></param>
        public void EditInit(ObservableCollection<JobDescription> JobDescriptionListDetails, JobDescription jobDescription)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                FillDepartmentList();
                FillLevelList();
                FillScopeList();
                FillLogData(jobDescription.IdJobDescription);
                ExistJobDescription = new ObservableCollection<JobDescription>(JobDescriptionListDetails);
                //pramod.misal 13.06.2024
                FillHealthAndSafetyFilesList(jobDescription.IdJobDescription);




                //[Sudhir.Jangra][GEOS2-5549]

                FillEquipmentAndToolsList(jobDescription.IdJobDescription);

                _JobDescription = jobDescription;
                JobDescriptionCode = _JobDescription.JobDescriptionCode;
                OldJobDescriptionCode = _JobDescription.JobDescriptionCode;
                JobDescriptionTitle = _JobDescription.JobDescriptionTitle;
                JobDescriptionAbbreviation = _JobDescription.Abbreviation;
                SelectedIndexDepartment = DepartmentList.FindIndex(x => x.IdDepartment == _JobDescription.IdDepartment);
                JobDescriptionListParent = ExistJobDescription.Select(x => (JobDescription)x.Clone()).ToList();
                JobDescriptionListParent.Remove(JobDescriptionListParent.FirstOrDefault(x => x.IdJobDescription == jobDescription.IdJobDescription));
                JobDescriptionListParent.Insert(0, new JobDescription() { JobDescriptionTitle = "---", IdJobDescription = 0, JobDescriptionInUse = 1 });
                SelectedParentIndex = JobDescriptionListParent.FindIndex(x => x.IdJobDescription == _JobDescription.IdParent);
                ChildOrientation = jobDescription.ChildOrientation;

                if (JDLevelList.FindIndex(x => x.IdLookupValue == _JobDescription.IdJDLevel) != -1)
                    SelectedIndexLevel = JDLevelList.FindIndex(x => x.IdLookupValue == _JobDescription.IdJDLevel);
                //sjadhav
                if (JDScopeList.FindIndex(x => x.IdLookupValue == _JobDescription.IdJdScope) != -1)
                    SelectedIndexScope = JDScopeList.FindIndex(x => x.IdLookupValue == _JobDescription.IdJdScope);

                Use = _JobDescription.JobDescriptionInUse;
                if (Use == 1)
                {
                    InUse = true;
                }
                else
                {
                    InUse = false;
                }

                JobDescriptionFileName = _JobDescription.JobDescriptionFileName;
                OldJobDescriptionFileName = _JobDescription.JobDescriptionFileName;
                AttachmentList = new List<object>();

                if (!string.IsNullOrEmpty(_JobDescription.JobDescriptionFileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = _JobDescription.JobDescriptionFileName;
                    attachment.IsDeleted = false;
                    AttachmentList.Add(attachment);
                }
                //[001]
                Mandatory = _JobDescription.JobDescriptionIsMandatory;

                if (Mandatory == 1)
                {
                    IsMandatory = true;
                }
                else
                {
                    IsMandatory = false;
                }
                IdJobDescription = _JobDescription.IdJobDescription;

                //[002] Added
                Remote = _JobDescription.JobDescriptionIsRemote;

                if (Remote == 1)
                {
                    IsRemote = true;
                }
                else
                {
                    IsRemote = false;
                }
                //[002] End

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method InitReadOnly to Readonly users
        /// [001][HRM-M046-07] Add new permission ReadOnly--By Amit
        /// [002][spawar][04-03-2020][GEOS2-1918] Job Description Configuration
        /// [003][avpawar][GEOS2-2438][Identify the Job descriptions working remotely]
        /// Job Description Configuration
        /// </summary>
        public void InitReadOnly(JobDescription jobDescription)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);

                _JobDescription = jobDescription;
                JobDescriptionCode = _JobDescription.JobDescriptionCode;
                OldJobDescriptionCode = _JobDescription.JobDescriptionCode;
                JobDescriptionTitle = _JobDescription.JobDescriptionTitle;
                JobDescriptionAbbreviation = _JobDescription.Abbreviation;
                DepartmentList = new List<Department>();
                DepartmentList.Add(jobDescription.Department);
                SelectedIndexDepartment = 0;
                JobDescriptionListParent = new List<JobDescription>();
                JobDescriptionListParent.Add(jobDescription.ParentJobDescription);
                SelectedParentIndex = 0;
                Use = _JobDescription.JobDescriptionInUse;
                JDLevelList = new List<LookupValue>();
                JDLevelList.Add(jobDescription.JDLevel);
                //sjadhav
                JDScopeList = new List<LookupValue>();
                JDScopeList.Add(jobDescription.JDScope);
                SelectedIndexLevel = SelectedIndexScope = 0;

                if (Use == 1)
                {
                    InUse = true;
                }
                else
                {
                    InUse = false;
                }

                JobDescriptionFileName = _JobDescription.JobDescriptionFileName;
                OldJobDescriptionFileName = _JobDescription.JobDescriptionFileName;
                AttachmentList = new List<object>();

                if (!string.IsNullOrEmpty(_JobDescription.JobDescriptionFileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = _JobDescription.JobDescriptionFileName;
                    attachment.IsDeleted = false;
                    AttachmentList.Add(attachment);
                }
                //[002]
                Mandatory = _JobDescription.JobDescriptionIsMandatory;

                if (Mandatory == 1)
                {
                    IsMandatory = true;
                }
                else
                {
                    IsMandatory = false;
                }
                //
                IdJobDescription = _JobDescription.IdJobDescription;

                //[003] Added
                Remote = _JobDescription.JobDescriptionIsRemote;

                if (Remote == 1)
                {
                    IsRemote = true;
                }
                else
                {
                    IsRemote = false;
                }
                //[003] End

                GeosApplication.Instance.Logger.Log("Method InitReadOnly()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method InitReadOnly()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {

                HrmCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //rajashri.telvekar[GEOS-3693][10-11-2023]
        public void AddJobChangeLog(JobDescription oldjob, JobDescription newJob)
        {

            GeosApplication.Instance.Logger.Log("Method AddJobChangeLog ...", category: Category.Info, priority: Priority.Low);
            if (oldjob != null && newJob != null)
            {
                //ListofChangelog.Add(newJob);
                if (Lookuplist == null)
                {
                    Lookuplist = new ObservableCollection<LookupValue>(CrmService.GetLookupValues(136));
                }
                LookupValueId = Lookuplist.Select(i => i.IdLookupValue).FirstOrDefault();
                Lookuplist.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0, IdLookupKey = 0 });

                if (newJob.LogEntries == null)
                {
                    newJob.LogEntries = new List<LogNewJobDescription>();
                }
                // UpdateJobDescription.LogEntries = ListofChangelog.ToList();
                //UpdateJobDescription.LogEntries = newJob.LogEntries;

                if (oldjob.JobDescriptionCode != newJob.JobDescriptionCode)
                {
                    newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_Code").ToString(), oldjob.JobDescriptionCode,
                        newJob.JobDescriptionCode) });
                }
                if (oldjob.JobDescriptionTitle != newJob.JobDescriptionTitle)
                {
                    newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_title").ToString(), oldjob.JobDescriptionTitle, newJob.JobDescriptionTitle) });
                }
                if (oldjob.Abbreviation != newJob.Abbreviation)
                {
                    newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_JobCode").ToString(), oldjob.Abbreviation, newJob.Abbreviation) });
                }
                if (oldjob.IdParent != newJob.IdParent)
                {
                    if (newJob.ParentJobDescription.JobDescriptionTitle == "---")
                    {
                        newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_Parent").ToString(), oldjob.ParentJobDescription.JobDescriptionTitle, "None") });
                    }
                    else
                    {
                        if (oldjob.IdParent == 0)
                        {
                            newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_Parent").ToString(), "None", newJob.ParentJobDescription.JobDescriptionTitle) });
                        }
                        else
                        {
                            newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_Parent").ToString(), oldjob.ParentJobDescription.JobDescriptionTitle, newJob.ParentJobDescription.JobDescriptionTitle) });
                        }
                    }
                }
                if (oldjob.IdDepartment != newJob.IdDepartment)
                {
                    newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_Department").ToString(), oldjob.Department, newJob.Department) });
                }
                if (oldjob.IdJDLevel != newJob.IdJDLevel)
                {
                    if (newJob.JDLevel.Value == "---")
                    {
                        newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_Level").ToString(), oldjob.JDLevel.Value, "None") });
                    }
                    else
                    {
                        if (oldjob.IdJDLevel == null)
                        {
                            newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_Level").ToString(), "None", newJob.JDLevel.Value) });
                        }
                        else
                        {
                            newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_Level").ToString(), oldjob.JDLevel.Value, newJob.JDLevel.Value) });
                        }

                    }

                }
                if (oldjob.IdJdScope != newJob.IdJdScope)
                {
                    newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_Scope").ToString(), oldjob.JDScope.Value, newJob.JDScope.Value) });
                }
                if (oldjob.JobDescriptionFileName != newJob.JobDescriptionFileName)
                {
                    if (oldjob.JobDescriptionFileName == null)
                    {
                        // Log when the old value is null
                        newJob.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_File").ToString(), "None", newJob.JobDescriptionFileName)
                        });
                    }
                    else
                    {


                        // Log when the new value is null
                        if (newJob.JobDescriptionFileName == null)
                        {
                            newJob.LogEntries.Add(new LogNewJobDescription()
                            {
                                IdGeosModule = 7,
                                IdObject = LookupValueId,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_File").ToString(), oldjob.JobDescriptionFileName, "None")
                            });
                        }
                        else
                        {
                            // Log when the old value is not null
                            newJob.LogEntries.Add(new LogNewJobDescription()
                            {
                                IdGeosModule = 7,
                                IdObject = LookupValueId,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_File").ToString(), oldjob.JobDescriptionFileName, newJob.JobDescriptionFileName)
                            });
                        }
                    }



                }
                if (oldjob.JobDescriptionInUse != newJob.JobDescriptionInUse)
                {
                    if (oldjob.JobDescriptionInUse == 1)
                    {
                        newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_InUse").ToString(), "Yes", "No") });
                    }
                    else
                    {
                        newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_InUse").ToString(), "No", "Yes") });
                    }
                }
                if (oldjob.JobDescriptionIsMandatory != newJob.JobDescriptionIsMandatory)
                {
                    if (oldjob.JobDescriptionIsMandatory == 1)
                    {
                        newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_IsMandatory").ToString(), "Yes", "No") });
                    }
                    else
                    {
                        newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_IsMandatory").ToString(), "No", "Yes") });
                    }

                }
                if (oldjob.JobDescriptionIsRemote != newJob.JobDescriptionIsRemote)
                {
                    if (oldjob.JobDescriptionIsMandatory == 1)
                    {
                        newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_Isremote").ToString(), "Yes", "No") });
                    }
                    else
                    {
                        newJob.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobDescription_Isremote").ToString(), "No", "Yes") });
                    }
                }

            }
            GeosApplication.Instance.Logger.Log("Method AddJobChangeLog() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public void AddJobChangeLogForAdd(JobDescription NewJobData)
        {

            GeosApplication.Instance.Logger.Log("Method AddJobChangeLog ...", category: Category.Info, priority: Priority.Low);
            if (NewJobData != null)
            {
                //ListofChangelog.Add(newJob);
                if (Lookuplist == null)
                {
                    Lookuplist = new ObservableCollection<LookupValue>(CrmService.GetLookupValues(136));
                }
                LookupValueId = Lookuplist.Select(i => i.IdLookupValue).FirstOrDefault();
                Lookuplist.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0, IdLookupKey = 0 });

                if (NewJobData.LogEntries == null)
                {
                    NewJobData.LogEntries = new List<LogNewJobDescription>();
                }
                // UpdateJobDescription.LogEntries = ListofChangelog.ToList();
                //UpdateJobDescription.LogEntries = newJob.LogEntries;

                NewJobData.LogEntries.Add(new LogNewJobDescription() { IdGeosModule = 7, IdObject = LookupValueId, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Change = string.Format(System.Windows.Application.Current.FindResource("JobAdded").ToString(), NewJobData.JobDescriptionTitle) });

            }
            GeosApplication.Instance.Logger.Log("Method AddJobChangeLog() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        public void FillLogData(UInt32 jobid)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLogData ...", category: Category.Info, priority: Priority.Low);


                ListJobChangeLog = new List<LogNewJobDescription>(HrmService.GetLogEntriesForJob_V2440(jobid));
                ListJobChangeLog = ListJobChangeLog.OrderByDescending(entry => entry.Datetime).ToList();
                GeosApplication.Instance.Logger.Log("Method FillLogData() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLogData() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLogData() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLogData() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5549]
        private void AddHealthAndSafetyFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddHealthAndSafetyFileCommandAction()...", category: Category.Info, priority: Priority.Low);
                AddHealthAndSafetyFileView addHealthAndSafetyFileView = new AddHealthAndSafetyFileView();
                AddHealthAndSafetyFileViewModel addHealthAndSafetyFileViewModel = new AddHealthAndSafetyFileViewModel();
                EventHandler handle = delegate { addHealthAndSafetyFileView.Close(); };
                addHealthAndSafetyFileViewModel.RequestClose += handle;
                addHealthAndSafetyFileViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddHealthAndSafetyDocumentHeader").ToString();
                addHealthAndSafetyFileViewModel.IsNew = true;
                addHealthAndSafetyFileViewModel.Init();
                addHealthAndSafetyFileView.DataContext = addHealthAndSafetyFileViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addHealthAndSafetyFileView.Owner = Window.GetWindow(ownerInfo);
                addHealthAndSafetyFileView.ShowDialog();

                if (addHealthAndSafetyFileViewModel.IsSave)
                {
                    if (HealthAndSafetyFilesList == null)
                    {
                        HealthAndSafetyFilesList = new ObservableCollection<HealthAndSafetyAttachedDoc>();
                    }
                    HealthAndSafetyFilesList.Add(addHealthAndSafetyFileViewModel.SelectedHealthAndSafetyFile);
                    if (!string.IsNullOrEmpty(ShortName))
                    {
                        string[] shortNameValues = ShortName.Split(',').Select(s => s.Trim()).Distinct().ToArray();
                        HealthAndSafetyFilesList = new ObservableCollection<HealthAndSafetyAttachedDoc>(
                        HealthAndSafetyFilesList.Where(doc =>
                        shortNameValues.Any(sn => doc.Plant.Contains(sn.Trim())) || doc.Plant.Equals("All", StringComparison.OrdinalIgnoreCase)
                         ));
                    }
                    SelectedHealthAndSafetyFile = HealthAndSafetyFilesList.FirstOrDefault(x=>x.IdJobDescriptionHealthAndSafetyAttachedDoc== addHealthAndSafetyFileViewModel.SelectedHealthAndSafetyFile.IdJobDescriptionHealthAndSafetyAttachedDoc);
                }
                GeosApplication.Instance.Logger.Log("Method AddHealthAndSafetyFileCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddHealthAndSafetyFileCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5549]
        private void AddEquipmentAndToolsFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEquipmentAndToolsFileCommandAction()...", category: Category.Info, priority: Priority.Low);
                AddEquimentAndToolsView addEquimentAndToolsView = new AddEquimentAndToolsView();
                AddEquimentAndToolsViewModel addEquimentAndToolsViewModel = new AddEquimentAndToolsViewModel();
                EventHandler handle = delegate { addEquimentAndToolsView.Close(); };
                addEquimentAndToolsViewModel.RequestClose += handle;
                addEquimentAndToolsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddEquipmentAndToolsDocumentHeader").ToString();
                addEquimentAndToolsViewModel.IsNew = true;
                addEquimentAndToolsViewModel.Init();
                addEquimentAndToolsView.DataContext = addEquimentAndToolsViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEquimentAndToolsView.Owner = Window.GetWindow(ownerInfo);
                addEquimentAndToolsView.ShowDialog();

                if (addEquimentAndToolsViewModel.IsSave)
                {
                    if (EquipmentAndToolsFilesList == null)
                    {
                        EquipmentAndToolsFilesList = new ObservableCollection<EquipmentAndToolsAttachedDoc>();
                    }
                    EquipmentAndToolsFilesList.Add(addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile);
                    SelectedEquipmentAndToolsFile = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile;
                }
                GeosApplication.Instance.Logger.Log("Method AddEquipmentAndToolsFileCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddEquipmentAndToolsFileCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenPDFDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();
                documentViewModel.OpenHealthAndSafetyPdf(SelectedHealthAndSafetyFile, obj);
                if (documentViewModel.IsPresent)
                {
                    documentView.DataContext = documentViewModel;
                    documentView.Show();
                }
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPDFDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditFile()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                HealthAndSafetyAttachedDoc healthAndSafetyAttachedDoc = (HealthAndSafetyAttachedDoc)detailView.DataControl.CurrentItem;
                AddHealthAndSafetyFileView addHealthAndSafetyFileView = new AddHealthAndSafetyFileView();
                AddHealthAndSafetyFileViewModel addHealthAndSafetyFileViewModel = new AddHealthAndSafetyFileViewModel();
                EventHandler handle = delegate { addHealthAndSafetyFileView.Close(); };
                addHealthAndSafetyFileViewModel.RequestClose += handle;
                addHealthAndSafetyFileViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditHealthAndSafetyDocumentHeader").ToString();
                addHealthAndSafetyFileViewModel.IsNew = false;
                addHealthAndSafetyFileViewModel.EditInit(healthAndSafetyAttachedDoc);
                addHealthAndSafetyFileView.DataContext = addHealthAndSafetyFileViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addHealthAndSafetyFileView.Owner = Window.GetWindow(ownerInfo);
                addHealthAndSafetyFileView.ShowDialog();

                if (addHealthAndSafetyFileViewModel.IsSave)
                {
                    // SelectedProductTypeFile.IdCPTypeAttachedDoc = addFileInProductTypeViewModel.IdCPTypeAttachedDoc;

                    SelectedHealthAndSafetyFile.Title = addHealthAndSafetyFileViewModel.HealthAndSafetyTitle;
                    SelectedHealthAndSafetyFile.Remarks = addHealthAndSafetyFileViewModel.HealthAndSafetyRemarks;
                    SelectedHealthAndSafetyFile.OriginalFileName = addHealthAndSafetyFileViewModel.SelectedHealthAndSafetyFile.OriginalFileName;
                    SelectedHealthAndSafetyFile.HealthAndSafetyAttachedDocInBytes = addHealthAndSafetyFileViewModel.FileInBytes;
                    SelectedHealthAndSafetyFile.SavedFileName = addHealthAndSafetyFileViewModel.SelectedHealthAndSafetyFile.SavedFileName;
                    SelectedHealthAndSafetyFile.Plant = addHealthAndSafetyFileViewModel.EditPlantName;
                    SelectedHealthAndSafetyFile.IdPlants = addHealthAndSafetyFileViewModel.EditPlantIds;
                }

                GeosApplication.Instance.Logger.Log("Method EditFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-5549][13.06.2024]
        private void FillHealthAndSafetyFilesList(UInt32 jobid)
        {

            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillHealthAndSafetyFilesList().... "), category: Category.Info, priority: Priority.Low);


                ObservableCollection<HealthAndSafetyAttachedDoc> temp = new ObservableCollection<HealthAndSafetyAttachedDoc>(HrmService.GetAllHealthAndSafetyFilesByIdJobDescription(jobid));

                ShortName = HrmService.GetUserAllowSites_V2530(GeosApplication.Instance.ActiveUser.IdUser);

                foreach (var item in temp)
                {
                    
                    if (item.IdPlants==null ||string.IsNullOrEmpty(item.IdPlants)|| item.IdPlants=="")
                    {
                        item.Plant = "All";
                    }
                    else
                    {
                        var plantIds = item.IdPlants.Split(',').Select(id => int.Parse(id.Trim())).ToList();
                        var plantNames = HrmCommon.Instance.UserAuthorizedPlantsList
                                      .Where(x => plantIds.Contains(x.IdCompany))
                                      .Select(x => x.ShortName)
                                      .ToList();
                        item.Plant = string.Join(",", plantNames);
                    }
                }

                HealthAndSafetyFilesList = new ObservableCollection<HealthAndSafetyAttachedDoc>(temp);

                if (!string.IsNullOrEmpty(ShortName))
                {
                    string[] shortNameValues = ShortName.Split(',').Select(s => s.Trim()).Distinct().ToArray();

                    HealthAndSafetyFilesList = new ObservableCollection<HealthAndSafetyAttachedDoc>(
                    HealthAndSafetyFilesList.Where(doc =>
                    shortNameValues.Any(sn => doc.Plant.Contains(sn.Trim())) || doc.Plant.Equals("All", StringComparison.OrdinalIgnoreCase)
                     ));
                }


                if (ClonedHealthAndSafetyAttachment == null)
                {
                    ClonedHealthAndSafetyAttachment = new ObservableCollection<HealthAndSafetyAttachedDoc>();
                }

                foreach (HealthAndSafetyAttachedDoc item in HealthAndSafetyFilesList)
                {
                    ClonedHealthAndSafetyAttachment.Add((HealthAndSafetyAttachedDoc)item.Clone());
                }



                GeosApplication.Instance.Logger.Log(string.Format("Method FillHealthAndSafetyFilesList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillHealthAndSafetyFilesList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        private void OpenEquipmentPDFDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();
                documentViewModel.OpenEquipmentAndToolsPdf(SelectedEquipmentAndToolsFile, obj);
                if (documentViewModel.IsPresent)
                {
                    documentView.DataContext = documentViewModel;
                    documentView.Show();
                }
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPDFDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPDFDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditEquipmentFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditFile()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                EquipmentAndToolsAttachedDoc equipmentAndToolsAttachedDoc = (EquipmentAndToolsAttachedDoc)detailView.DataControl.CurrentItem;
                AddEquimentAndToolsView addEquimentAndToolsView = new AddEquimentAndToolsView();
                AddEquimentAndToolsViewModel addEquimentAndToolsViewModel = new AddEquimentAndToolsViewModel();
                EventHandler handle = delegate { addEquimentAndToolsView.Close(); };
                addEquimentAndToolsViewModel.RequestClose += handle;
                addEquimentAndToolsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditEquipmentAndToolsHeader").ToString();
                addEquimentAndToolsViewModel.IsNew = false;
                addEquimentAndToolsViewModel.EditInit(equipmentAndToolsAttachedDoc);
                addEquimentAndToolsView.DataContext = addEquimentAndToolsViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addEquimentAndToolsView.Owner = Window.GetWindow(ownerInfo);
                addEquimentAndToolsView.ShowDialog();

                if (addEquimentAndToolsViewModel.IsSave)
                {
                    // SelectedProductTypeFile.IdCPTypeAttachedDoc = addFileInProductTypeViewModel.IdCPTypeAttachedDoc;
                    SelectedEquipmentAndToolsFile.IdLookupValue = addEquimentAndToolsViewModel.SelectedEquipment.IdLookupValue;
                    SelectedEquipmentAndToolsFile.EquipmentType = addEquimentAndToolsViewModel.SelectedEquipment.EquipmentType;
                    SelectedEquipmentAndToolsFile.IdParent = addEquimentAndToolsViewModel.SelectedEquipment.IdParent ?? 0;
                    SelectedEquipmentAndToolsFile.CategoryType = addEquimentAndToolsViewModel.SelectedEquipment.CategoryType;
                    SelectedEquipmentAndToolsFile.IsMandatory = addEquimentAndToolsViewModel.IsMandatory;
                    SelectedEquipmentAndToolsFile.StartDate = addEquimentAndToolsViewModel.StartDate;
                    SelectedEquipmentAndToolsFile.EndDate = addEquimentAndToolsViewModel.EndDate;
                    SelectedEquipmentAndToolsFile.Remarks = addEquimentAndToolsViewModel.EquipmentAndToolsRemarks;
                    SelectedEquipmentAndToolsFile.OriginalFileName = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.OriginalFileName;
                    SelectedEquipmentAndToolsFile.EquipmentAndToolsAttachedDocInBytes = addEquimentAndToolsViewModel.FileInBytes;
                    SelectedEquipmentAndToolsFile.SavedFileName = addEquimentAndToolsViewModel.SelectedEquipmentAndToolsFile.SavedFileName;

                }

                GeosApplication.Instance.Logger.Log("Method EditFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.jangra][GEOS2-5549]
        private void DeleteEquipmentFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteEquipmentFile()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["JobDescriptionDeleteDocumentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    EquipmentAndToolsFilesList.Remove(SelectedEquipmentAndToolsFile);
                }

                GeosApplication.Instance.Logger.Log("Method DeleteEquipmentFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteEquipmentFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.jangra][GEOS2-5549]
        private void DeleteHealthFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteHealthFile()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["JobDescriptionDeleteDocumentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                //pramod.misal 18.06.2024
                HealthAndSafetyAttachedDoc healthAndSafetyAttachedDoc = (HealthAndSafetyAttachedDoc)obj;

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsEnabledCancelButton = true;
                    HealthAndSafetyFilesList.Remove(SelectedHealthAndSafetyFile);
                    HealthAndSafetyFilesList = new ObservableCollection<HealthAndSafetyAttachedDoc>(HealthAndSafetyFilesList);
                    SelectedHealthAndSafetyFile = HealthAndSafetyFilesList.FirstOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method DeleteHealthFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteHealthFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5549]

        private void FillEquipmentAndToolsList(UInt32 idJobDescription)
        {
            try
            {
                EquipmentAndToolsFilesList = new ObservableCollection<EquipmentAndToolsAttachedDoc>(HrmService.GetEquipmentAndToolsForJobDescription_V2530(idJobDescription));
                if (ClonedEquipmentAndToolsList == null)
                {
                    ClonedEquipmentAndToolsList = new List<EquipmentAndToolsAttachedDoc>();
                }
                foreach (var item in EquipmentAndToolsFilesList)
                {
                    ClonedEquipmentAndToolsList.Add((EquipmentAndToolsAttachedDoc)item.Clone());
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillEquipmentAndToolsList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillEquipmentAndToolsList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillEquipmentAndToolsList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UpdateHealthAndSafetyChangeLog(HealthAndSafetyAttachedDoc old,HealthAndSafetyAttachedDoc newData,JobDescription jobdescription)
        {
            if (old!=null&& newData!=null)
            {
                if (old.Plant!=newData.Plant)
                {
                    string[] newdataPlant = newData.Plant.Split(',');
                    string[] olddataPlant = old.Plant.Split(',');

                    if (HrmCommon.Instance.UserAuthorizedPlantsList.Count()== newdataPlant.Count())
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdatePlantForHealthAndSafetyChangeLog").ToString(), newData.Title, old.Plant, "All")
                        });
                    }
                    else if (HrmCommon.Instance.UserAuthorizedPlantsList.Count() == olddataPlant.Count())
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdatePlantForHealthAndSafetyChangeLog").ToString(), newData.Title, "All", newData.Plant)
                        });
                    }
                    else
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdatePlantForHealthAndSafetyChangeLog").ToString(), newData.Title, old.Plant, newData.Plant)
                        });
                    }

                    
                }

                if (old.Title!=newData.Title)
                {
                    jobdescription.LogEntries.Add(new LogNewJobDescription()
                    {
                        IdGeosModule = 7,
                        IdObject = LookupValueId,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Change = string.Format(System.Windows.Application.Current.FindResource("UpdateTitleForHealthAndSafetyChangeLog").ToString(), old.Title, newData.Title)
                    });
                }

                if (old.SavedFileName != newData.SavedFileName)
                {
                    if ((old.SavedFileName==null||string.IsNullOrEmpty(old.SavedFileName)|| old.SavedFileName=="") && (newData.SavedFileName==null||string.IsNullOrEmpty(newData.SavedFileName)|| newData.SavedFileName==""))
                    {
                        
                    }
                    else if (old.SavedFileName==null||string.IsNullOrEmpty(old.SavedFileName))
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdateAttachmentForHealthAndSafetyChangeLog").ToString(), newData.Title, "None", newData.SavedFileName)
                        });
                    }
                    else if (newData.SavedFileName == null || string.IsNullOrEmpty(newData.SavedFileName))
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdateAttachmentForHealthAndSafetyChangeLog").ToString(), newData.Title, old.SavedFileName, "None")
                        });
                    }
                    else
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdateAttachmentForHealthAndSafetyChangeLog").ToString(), newData.Title, old.SavedFileName, newData.SavedFileName)
                        });
                    }
                    
                }

                if (old.Remarks != newData.Remarks)
                {
                    if (old.Remarks==null||string.IsNullOrEmpty(old.Remarks))
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdateRemarksForHealthAndSafetyChangeLog").ToString(),newData.Title, "None", newData.Remarks)
                        });
                    }
                    else if (newData.Remarks==null ||string.IsNullOrEmpty(newData.Remarks))
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdateRemarksForHealthAndSafetyChangeLog").ToString(), newData.Title, old.Remarks, "None")
                        });
                    }
                    else
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdateRemarksForHealthAndSafetyChangeLog").ToString(), newData.Title, old.Remarks, newData.Remarks)
                        });
                    }
                    
                }


            }
        }

        private void UpdateEquipmentAndToolsChangeLog(EquipmentAndToolsAttachedDoc old, EquipmentAndToolsAttachedDoc newData, JobDescription jobdescription)
        {
            if (old != null && newData != null)
            {
                if (old.EquipmentType!=newData.EquipmentType)
                {
                    jobdescription.LogEntries.Add(new LogNewJobDescription()
                    {
                        IdGeosModule = 7,
                        IdObject = LookupValueId,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Change = string.Format(System.Windows.Application.Current.FindResource("UpdateEquipmentTypeForEquipmentAndToolsChangeLog").ToString(), old.EquipmentType, newData.EquipmentType)
                    });
                }

                if (old.IsMandatory != newData.IsMandatory)
                {
                    string oldString = string.Empty;
                    string newString = string.Empty;
                    if (old.IsMandatory==true)
                    {
                        oldString = "Yes";
                    }
                    else
                    {
                        oldString = "No";
                    }
                    if (newData.IsMandatory==true)
                    {
                        newString = "Yes";
                    }
                    else
                    {
                        newString = "No";
                    }
                    jobdescription.LogEntries.Add(new LogNewJobDescription()
                    {
                        IdGeosModule = 7,
                        IdObject = LookupValueId,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Change = string.Format(System.Windows.Application.Current.FindResource("UpdateIsMandatoryForEquipmentAndToolsChangeLog").ToString(), newData.EquipmentType, oldString, newString)
                    });
                }

                if (old.StartDate != newData.StartDate)
                {
                    jobdescription.LogEntries.Add(new LogNewJobDescription()
                    {
                        IdGeosModule = 7,
                        IdObject = LookupValueId,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Change = string.Format(System.Windows.Application.Current.FindResource("UpdateStartDateForEquipmentAndToolsChangeLog").ToString(), newData.EquipmentType, old.StartDate?.ToString("d", CultureInfo.CurrentCulture), newData.StartDate?.ToString("d", CultureInfo.CurrentCulture))
                    });
                }

                if (old.EndDate != newData.EndDate)
                {
                    jobdescription.LogEntries.Add(new LogNewJobDescription()
                    {
                        IdGeosModule = 7,
                        IdObject = LookupValueId,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                        Change = string.Format(System.Windows.Application.Current.FindResource("UpdateEndDateForEquipmentAndToolsChangeLog").ToString(), newData.EquipmentType, old.EndDate?.ToString("d", CultureInfo.CurrentCulture), newData.EndDate?.ToString("d", CultureInfo.CurrentCulture))
                    });
                }


                if (old.SavedFileName != newData.SavedFileName)
                {
                    if ((old.SavedFileName == null || string.IsNullOrEmpty(old.SavedFileName) || old.SavedFileName == "") && (newData.SavedFileName == null || string.IsNullOrEmpty(newData.SavedFileName) || newData.SavedFileName == ""))
                    {

                    }
                    else if (old.SavedFileName==null||string.IsNullOrEmpty(old.SavedFileName))
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdateAttachmentForEquipmentAndToolsChangeLog").ToString(), newData.EquipmentType, "None", newData.SavedFileName)
                        });
                    }
                    else if (newData.SavedFileName == null || string.IsNullOrEmpty(newData.SavedFileName))
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdateAttachmentForEquipmentAndToolsChangeLog").ToString(), newData.EquipmentType, old.SavedFileName, "None")
                        });
                    }
                    else
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdateAttachmentForEquipmentAndToolsChangeLog").ToString(), newData.EquipmentType, old.SavedFileName, newData.SavedFileName)
                        });
                    }
                   
                }

                if (old.Remarks != newData.Remarks)
                {
                    if (old.Remarks==null||string.IsNullOrEmpty(old.Remarks))
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdateRemarksForEquipmentAndToolsChangeLog").ToString(), newData.EquipmentType, "None", newData.Remarks)
                        });
                    }
                    else if (newData.Remarks == null || string.IsNullOrEmpty(newData.Remarks))
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdateRemarksForEquipmentAndToolsChangeLog").ToString(), newData.EquipmentType, old.Remarks, "None")
                        });
                    }
                    else
                    {
                        jobdescription.LogEntries.Add(new LogNewJobDescription()
                        {
                            IdGeosModule = 7,
                            IdObject = LookupValueId,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Change = string.Format(System.Windows.Application.Current.FindResource("UpdateRemarksForEquipmentAndToolsChangeLog").ToString(), newData.EquipmentType, old.Remarks, newData.Remarks)
                        });
                    }
                   
                }

            }
        }
        #endregion

        #region Validation

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
                string error =
                    me[BindableBase.GetPropertyName(() => JobDescriptionCode)] +
                     me[BindableBase.GetPropertyName(() => JobDescriptionTitle)] + me[BindableBase.GetPropertyName(() => JobDescriptionAbbreviation)] +
                     me[BindableBase.GetPropertyName(() => SelectedIndexScope)] +
                       me[BindableBase.GetPropertyName(() => SelectedIndexDepartment)];

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
                string Code = BindableBase.GetPropertyName(() => JobDescriptionCode);
                string Title = BindableBase.GetPropertyName(() => JobDescriptionTitle);
                string JdAbbreviation = BindableBase.GetPropertyName(() => JobDescriptionAbbreviation);
                string department = BindableBase.GetPropertyName(() => SelectedIndexDepartment);
                string scope = BindableBase.GetPropertyName(() => SelectedIndexScope);

                if (columnName == Code)
                {
                    return JobDescriptionValidation.GetErrorMessage(Code, JobDescriptionCode);
                }

                if (columnName == Title)
                {
                    return JobDescriptionValidation.GetErrorMessage(Title, JobDescriptionTitle);
                }
                if (columnName == JdAbbreviation)
                {
                    return JobDescriptionValidation.GetErrorMessage(JdAbbreviation, JobDescriptionAbbreviation);
                }
                if (columnName == department)
                {
                    return JobDescriptionValidation.GetErrorMessage(department, SelectedIndexDepartment);
                }

                if (columnName == scope)
                {
                    return JobDescriptionValidation.GetErrorMessage(scope, SelectedIndexScope);
                }

                return null;
            }
        }


        #endregion

        private void SetUserPermission()
        {
            //HrmCommon.Instance.UserPermission = PermissionManagement.PlantViewer;

            switch (HrmCommon.Instance.UserPermission)
            {
                case PermissionManagement.SuperAdmin:
                    IsAcceptEnabled = true;
                    IsReadOnlyField = false;
                    break;

                case PermissionManagement.Admin:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                case PermissionManagement.PlantViewer:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                case PermissionManagement.GlobalViewer:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                default:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;
            }
        }

    }
}
