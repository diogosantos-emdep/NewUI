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

/// <summary>
/// Sprint 41--HRM	M041-08	New configuration section Job Descriptions--To add new Job Descriptions ---sdesai
/// </summary>
namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddNewJobDescriptionsViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        #endregion

        #region Properties

        public string OldJobDescriptionCode { get; set; }
        public string OldJobDescriptionFileName { get; set; }
        public bool IsJobDescriptionFileUpdated { get; set; }
        public List<Department> DepartmentList { get; set; }
        public List<JobDescription> JobDescriptionListParent { get; set; }
        public JobDescription NewJobDescription { get; set; }
        public JobDescription UpdateJobDescription { get; set; }

        public List<LookupValue> JDLevelList { get; set; }
        public List<LookupValue> JDScopeList { get; set; }
        //public List<String> JDScopeList { get; set; }

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
                if (value != null)
                {
                    jobDescriptionCode = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionCode"));
                }
            }
        }

        public string JobDescriptionTitle
        {
            get { return jobDescriptionTitle; }
            set
            {
                if (value != null)
                {
                    jobDescriptionTitle = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionTitle"));
                }
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
                if (value != null)
                {
                    jobDescriptionAbbreviation = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionAbbreviation"));
                }
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

        #endregion

        #region Public ICommand

        public ICommand AddJobDescriptionsViewCancelButtonCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        public ICommand AddJobDescriptionButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }

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
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexDepartment"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexLevel"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexScope"));

                if (error != null)
                {
                    return;
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
                            // Update IsMandatory="Yes" In Job Description Parent if IsMandatory="No"
                            UpdateIsMandatoryInJobDescriptionParent = HrmService.UpdateIsMandatoryInJobDescriptionParent(Convert.ToInt32(IdJobDescription));
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

                if (!string.IsNullOrEmpty(JobDescriptionCode))
                    JobDescriptionCode = JobDescriptionCode.Trim();

                if (!string.IsNullOrEmpty(JobDescriptionTitle))
                    JobDescriptionTitle = JobDescriptionTitle.Trim();

                if (!string.IsNullOrEmpty(JobDescriptionAbbreviation))
                    JobDescriptionAbbreviation = JobDescriptionAbbreviation.Trim();

                JobDescription TempJobDescription = new JobDescription();
                var TempExistJobDescription = ExistJobDescription.Where(x => x.JobDescriptionCode == JobDescriptionCode);
                TempJobDescription = TempExistJobDescription.FirstOrDefault(x => x.JobDescriptionCode == JobDescriptionCode);

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

                        NewJobDescription = HrmService.AddJobDescription_V2046(NewJobDescription);
                        IsSave = true;
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
                            IdJdScope= JDScopeList[SelectedIndexScope].IdLookupValue == 0 ? default(int) : JDScopeList[SelectedIndexScope].IdLookupValue,
                            JDScope= JDScopeList[SelectedIndexScope],
                        };

                        UpdateJobDescription.IdJobDescription = _JobDescription.IdJobDescription;
                        //UpdateJobDescription = HrmService.UpdateJobDescription_V2040(UpdateJobDescription);
                        UpdateJobDescription = HrmService.UpdateJobDescription_V2046(UpdateJobDescription);
                        IsSave = true;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("JobDescriptionInformationUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                    else
                    {
                        IsSave = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("JobDescriptionCodeExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddHoliday()....executed successfully", category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddHoliday() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                DepartmentList = new List<Department>(tempList);
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
                JDScopeList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0});
                
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
                JobDescriptionListParent.Insert(0, new JobDescription() { JobDescriptionTitle = "---", IdJobDescription = 0 });
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
                ExistJobDescription = new ObservableCollection<JobDescription>(JobDescriptionListDetails);
                _JobDescription = jobDescription;
                JobDescriptionCode = _JobDescription.JobDescriptionCode;
                OldJobDescriptionCode = _JobDescription.JobDescriptionCode;
                JobDescriptionTitle = _JobDescription.JobDescriptionTitle;
                JobDescriptionAbbreviation = _JobDescription.Abbreviation;
                SelectedIndexDepartment = DepartmentList.FindIndex(x => x.IdDepartment == _JobDescription.IdDepartment);
                JobDescriptionListParent = ExistJobDescription.Select(x => (JobDescription)x.Clone()).ToList();
                JobDescriptionListParent.Remove(JobDescriptionListParent.FirstOrDefault(x => x.IdJobDescription == jobDescription.IdJobDescription));
                JobDescriptionListParent.Insert(0, new JobDescription() { JobDescriptionTitle = "", IdJobDescription = 0 });
                SelectedParentIndex = JobDescriptionListParent.FindIndex(x => x.IdJobDescription == _JobDescription.IdParent);
                ChildOrientation = jobDescription.ChildOrientation;

                if (JDLevelList.FindIndex(x => x.IdLookupValue == _JobDescription.IdJDLevel) != -1)
                    SelectedIndexLevel = JDLevelList.FindIndex(x => x.IdLookupValue == _JobDescription.IdJDLevel);
                //sjadhav
                if (JDScopeList.FindIndex(x => x.IdLookupValue == _JobDescription.IdJdScope) != -1)
                    SelectedIndexScope= JDScopeList.FindIndex(x => x.IdLookupValue == _JobDescription.IdJdScope);

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
                     me[BindableBase.GetPropertyName(() => JobDescriptionTitle)] +
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
