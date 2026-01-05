using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
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

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddEducationViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services       
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public Icommands
        public ICommand AddEducationViewCancelButtonCommand { get; set; }
        public ICommand AddEducationViewAcceptButtonCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Declaration
        private string windowHeader;
        private bool isSave;
        private string remarks;
        private int selectedIndexQualification;
        private DateTime? startDate;
        private DateTime? endDate;
        private string description;
        private string entity;
        public string EmployeeCode;
        private string classification;
        private string qualificationFileName;
        private string ExistqualificationFileName;
        private bool isNew;
        private bool isBusy;
        private byte[] educationQualificationFileInBytes;
        private string startDateErrorMessage = string.Empty;
        private string endDateErrorMessage = string.Empty;
        private string error = string.Empty;
        private List<object> attachmentList;
        private ObservableCollection<EmployeeEducationQualification> existEmployeeEducationQualificationList;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;

        #endregion

        #region Properties
        public List<EducationQualification> QualificationList { get; set; }
        public EmployeeEducationQualification NewEducationQualification { get; set; }
        public EmployeeEducationQualification EditEducationQualification { get; set; }
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
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        public bool IsSave
        {
            get
            {
                return isSave;
            }

            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }

            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public string Classification
        {
            get
            {
                return classification;
            }

            set
            {
                classification = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Classification"));
            }
        }
        public byte[] EducationQualificationFileInBytes
        {
            get
            {
                return educationQualificationFileInBytes;
            }

            set
            {
                educationQualificationFileInBytes = value;
            }
        }

        public ObservableCollection<EmployeeEducationQualification> ExistEmployeeEducationQualificationList
        {
            get
            {
                return existEmployeeEducationQualificationList;
            }

            set
            {
                existEmployeeEducationQualificationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeEducationQualificationList"));
            }
        }

        public int SelectedIndexQualification
        {
            get
            {
                return selectedIndexQualification;
            }

            set
            {
                selectedIndexQualification = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexQualification"));
            }
        }

        public string Remarks
        {
            get
            {
                return remarks;
            }

            set
            {
                remarks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remarks"));
            }
        }

        public DateTime? StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }

        public DateTime? EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }

        public string Entity
        {
            get
            {
                return entity;
            }

            set
            {
                entity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Entity"));
            }
        }
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
        public string QualificationFileName
        {
            get
            {
                return qualificationFileName;
            }

            set
            {
                qualificationFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("QualificationFileName"));
            }
        }

        public List<object> AttachmentList
        {
            get
            {
                return attachmentList;
            }

            set
            {

                attachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentList"));

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
        public AddEducationViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor AddEducationViewModel()...", category: Category.Info, priority: Priority.Low);

                AddEducationViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddEducationViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddEducationQualificationInformation));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFileCommandAction));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                GeosApplication.Instance.Logger.Log("Constructor AddEducationViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddEducationViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void OnDateEditValueChanging(EditValueChangingEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);
            if (StartDate > EndDate)
            {
                startDateErrorMessage = System.Windows.Application.Current.FindResource("EmployeeContractSituationStartDateError").ToString();
                endDateErrorMessage = System.Windows.Application.Current.FindResource("EmployeeContractSituationEndDateError").ToString();
            }
            else
            {
                startDateErrorMessage = string.Empty;
                endDateErrorMessage = string.Empty;
            }
            error = EnableValidationAndGetError();
            PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
            PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));

            GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        public void Init(ObservableCollection<EmployeeEducationQualification> EmployeeEducationQualificationList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("AddEducationQualification").ToString();
                ExistEmployeeEducationQualificationList = EmployeeEducationQualificationList;
                FillQualificationList();


                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(ObservableCollection<EmployeeEducationQualification> EmployeeEducationQualificationList, EmployeeEducationQualification empEducationQualification)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("EditEducationQualification").ToString();
                ExistEmployeeEducationQualificationList = new ObservableCollection<EmployeeEducationQualification>(EmployeeEducationQualificationList.ToList());
                ExistEmployeeEducationQualificationList.Remove(empEducationQualification);

                FillQualificationList();

                SelectedIndexQualification = QualificationList.FindIndex(x => x.IdEducationQualification == empEducationQualification.IdEducationQualification);
                StartDate = empEducationQualification.QualificationStartDate;
                EndDate = empEducationQualification.QualificationEndDate;
                Description = empEducationQualification.QualificationName;
                Entity = empEducationQualification.QualificationEntity;
                Classification = empEducationQualification.QualificationClassification;
                QualificationFileName = empEducationQualification.QualificationFileName;
                Remarks = empEducationQualification.QualificationRemarks;
                EducationQualificationFileInBytes = empEducationQualification.QualificationFileInBytes;
                ExistqualificationFileName = QualificationFileName;

                AttachmentList = new List<object>();
                if (empEducationQualification.Attachment != null)
                {
                    AttachmentList.Add(empEducationQualification.Attachment);
                }
                else if (!string.IsNullOrEmpty(QualificationFileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = empEducationQualification.QualificationFileName;
                    attachment.IsDeleted = false;
                    //attachment.FileByte = EducationQualificationFileInBytes;
                    AttachmentList.Add(attachment);
                }

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method InitReadOnly to Readonly users
        /// [HRM-M046-07] Add new permission ReadOnly--By Amit
        /// </summary>

        public void InitReadOnly(EmployeeEducationQualification empEducationQualification)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("EditEducationQualification").ToString();
      

                QualificationList = new List<EducationQualification>();
                QualificationList.Add(empEducationQualification.EducationQualification);

                SelectedIndexQualification = 0;

                StartDate = empEducationQualification.QualificationStartDate;
                EndDate = empEducationQualification.QualificationEndDate;
                Description = empEducationQualification.QualificationName;
                Entity = empEducationQualification.QualificationEntity;
                Classification = empEducationQualification.QualificationClassification;
                QualificationFileName = empEducationQualification.QualificationFileName;
                Remarks = empEducationQualification.QualificationRemarks;
                EducationQualificationFileInBytes = empEducationQualification.QualificationFileInBytes;
                ExistqualificationFileName = QualificationFileName;

                AttachmentList = new List<object>();
                if (empEducationQualification.Attachment != null)
                {
                    AttachmentList.Add(empEducationQualification.Attachment);
                }
                else if (!string.IsNullOrEmpty(QualificationFileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = empEducationQualification.QualificationFileName;
                    attachment.IsDeleted = false;
                    //attachment.FileByte = EducationQualificationFileInBytes;
                    AttachmentList.Add(attachment);
                }

                GeosApplication.Instance.Logger.Log("Method InitReadOnly()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InitReadOnly()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void BrowseFileCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFile() ...", category: Category.Info, priority: Priority.Low);

            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = "Pdf Files|*.pdf";
                dlg.Filter = "Pdf Files|*.pdf";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    //QualificationFileName = dlg.SafeFileName;
                    EducationQualificationFileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    AttachmentList = new List<object>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    QualificationFileName = file.Name;

                    List<object> newAttachmentList = new List<object>();

                    Attachment attachment = new Attachment();
                    attachment.FilePath = file.FullName;
                    attachment.OriginalFileName = file.Name;
                    attachment.IsDeleted = false;
                    attachment.FileByte = EducationQualificationFileInBytes;

                    newAttachmentList.Add(attachment);

                    //newAttachmentList.Add(file);

                    AttachmentList = newAttachmentList;
                }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            GeosApplication.Instance.Logger.Log("Method BrowseFile() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void FillQualificationList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLanguageList()...", category: Category.Info, priority: Priority.Low);

                IList<EducationQualification> tempCountryList = HrmService.GetAllEducationQualifications(); ;
                QualificationList = new List<EducationQualification>();
                QualificationList.Insert(0, new EducationQualification() { Name = "---" });
                QualificationList.AddRange(tempCountryList);

                GeosApplication.Instance.Logger.Log("Method FillLanguageList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLanguageList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLanguageList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLanguageList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

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
                    me[BindableBase.GetPropertyName(() => Entity)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexQualification)] +
                    me[BindableBase.GetPropertyName(() => Description)] +
                     me[BindableBase.GetPropertyName(() => StartDate)] +
                      me[BindableBase.GetPropertyName(() => EndDate)];


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
                string empEntity = BindableBase.GetPropertyName(() => Entity);
                string empselectedIndexQualification = BindableBase.GetPropertyName(() => SelectedIndexQualification);
                string empcontractStartDate = BindableBase.GetPropertyName(() => StartDate);
                string empcontractEndDate = BindableBase.GetPropertyName(() => EndDate);
                string empeducationDesc = BindableBase.GetPropertyName(() => Description);

                if (columnName == empEntity)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empEntity, Entity);
                }

                if (columnName == empselectedIndexQualification)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedIndexQualification, SelectedIndexQualification);
                }
                if (columnName == empeducationDesc)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empeducationDesc, Description);
                }
                if (columnName == empcontractStartDate)
                {
                    if (!string.IsNullOrEmpty(startDateErrorMessage))
                    {
                        return startDateErrorMessage;
                    }
                }
                if (columnName == empcontractEndDate)
                {
                    if (!string.IsNullOrEmpty(endDateErrorMessage))
                    {
                        return endDateErrorMessage;
                    }
                }
                return null;
            }
        }

        private void AddEducationQualificationInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEducationQualificationInformation()...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexQualification"));
                PropertyChanged(this, new PropertyChangedEventArgs("Entity"));
                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                List<Attachment> temp = new List<Attachment>();
                if (error != null)
                {
                    return;
                }
                bool IsDeleteFile = false;
                if (!string.IsNullOrEmpty(Remarks))
                    Remarks = Remarks.Trim();
                if (!string.IsNullOrEmpty(Entity))
                    Entity = Entity.Trim();
                if (!string.IsNullOrEmpty(Description))
                    Description = Description.Trim();
                if (!string.IsNullOrEmpty(Classification))
                    Classification = Classification.Trim();
                if (AttachmentList != null && AttachmentList.Count == 0)
                {
                    QualificationFileName = null;
                    EducationQualificationFileInBytes = null;
                    temp = AttachmentList.Cast<Attachment>().ToList();
                }
                EmployeeEducationQualification TempNewEducationQualification = new EmployeeEducationQualification();
                TempNewEducationQualification = ExistEmployeeEducationQualificationList.FirstOrDefault(x => x.IdEducationQualification == QualificationList[SelectedIndexQualification].IdEducationQualification && x.QualificationName == Description);

                if (TempNewEducationQualification == null)
                {
                    EmployeeEducationQualification TempNewEducationQualificationForFile = new EmployeeEducationQualification();
                    var ExistFilesRecord = ExistEmployeeEducationQualificationList.Where(x => x.QualificationFileName != null);
                    TempNewEducationQualificationForFile = ExistFilesRecord.FirstOrDefault(x => x.QualificationFileName == QualificationFileName);

                    if (TempNewEducationQualificationForFile == null)
                    {
                        if (IsNew == true)
                        {
                            NewEducationQualification = new EmployeeEducationQualification()
                            {
                                EducationQualification = QualificationList[SelectedIndexQualification],
                                IdEducationQualification = QualificationList[SelectedIndexQualification].IdEducationQualification,
                                QualificationStartDate = StartDate,
                                QualificationEndDate = EndDate,
                                QualificationEntity = Entity,
                                QualificationFileName = QualificationFileName,
                                QualificationName = Description,
                                QualificationClassification = Classification,
                                QualificationFileInBytes = EducationQualificationFileInBytes,
                                QualificationRemarks = Remarks,
                                IsQualificationFileDeleted = IsDeleteFile,
                                Attachment = temp != null && temp.Count > 0 ? temp[0] : null,
                                TransactionOperation = ModelBase.TransactionOperations.Add
                            };

                            IsSave = true;
                            RequestClose(null, null);
                        }
                        else
                        {
                            EditEducationQualification = new EmployeeEducationQualification()
                            {
                                EducationQualification = QualificationList[SelectedIndexQualification],
                                IdEducationQualification = QualificationList[SelectedIndexQualification].IdEducationQualification,
                                QualificationStartDate = StartDate,
                                QualificationEndDate = EndDate,
                                QualificationEntity = Entity,
                                QualificationFileName = QualificationFileName,
                                QualificationName = Description,
                                QualificationClassification = Classification,
                                QualificationFileInBytes = EducationQualificationFileInBytes,
                                QualificationRemarks = Remarks,
                                IsQualificationFileDeleted = IsDeleteFile,
                                Attachment = temp != null && temp.Count > 0 ? temp[0] : null,
                                TransactionOperation = ModelBase.TransactionOperations.Update
                            };
                            IsSave = true;
                            RequestClose(null, null);
                        }
                    }
                    else
                    {
                        IsSave = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEducationQualificationFileExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    IsSave = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEducationQualificationExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method AddEducationQualificationInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddEducationQualificationInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
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
                    IsAcceptEnabled = true;
                    IsReadOnlyField = false;
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
