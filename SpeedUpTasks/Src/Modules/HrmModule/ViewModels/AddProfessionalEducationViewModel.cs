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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddProfessionalEducationViewModel : INotifyPropertyChanged, IDataErrorInfo
    {

        #region Services       
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public ICommands
        public ICommand AddProfessionalEducationViewCancelButtonCommand { get; set; }
        public ICommand AddProfessionalEducationViewAcceptButtonCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        public ICommand OnDurationValueEditValueChangingCommand { get; set; }

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

        #region Declaration
        private string windowHeader;
        private bool isSave;
        private bool isNew;
        private bool IsBusy;
        private DateTime? startDate;
        private DateTime? endDate;
        private string description;
        private string entity;
        private string remarks;
        private byte[] educationProfessionalEducationFileInBytes;
        private string startDateErrorMessage = string.Empty;
        private string endDateErrorMessage = string.Empty;
        private string error = string.Empty;
        private List<object> attachmentList;
        private string professionalEducationFileName;
        private int selectedIndexEducationType;
        private int selectedIndexTimeDuration;
        private string durationValue;
        private ObservableCollection<EmployeeProfessionalEducation> existEmployeeProfessionalEducation;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        #endregion

        #region Properties
        public List<LookupValue> EducationTypeList { get; set; }
        public List<LookupValue> DurationList { get; set; }
        public EmployeeProfessionalEducation NewProfessionalEducation { get; set; }
        public EmployeeProfessionalEducation EditProfessionalEducation { get; set; }
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
        public ObservableCollection<EmployeeProfessionalEducation> ExistEmployeeProfessionalEducationList
        {
            get
            {
                return existEmployeeProfessionalEducation;
            }
            set
            {
                existEmployeeProfessionalEducation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeProfessionalEducationList"));
            }
        }

        public byte[] EducationProfessionalEducationFileInBytes
        {
            get
            {
                return educationProfessionalEducationFileInBytes;
            }

            set
            {
                educationProfessionalEducationFileInBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EducationProfessionalEducationFileInBytes"));
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

        public string ProfessionalEducationFileName
        {
            get
            {
                return professionalEducationFileName;
            }

            set
            {
                professionalEducationFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfessionalEducationFileName"));
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

        public int SelectedIndexEducationType
        {
            get
            {
                return selectedIndexEducationType;
            }

            set
            {
                selectedIndexEducationType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexEducationType"));
            }
        }

        public int SelectedIndexTimeDuration
        {
            get
            {
                return selectedIndexTimeDuration;
            }

            set
            {
                selectedIndexTimeDuration = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexTimeDuration"));
            }
        }

        public string DurationValue
        {
            get
            {
                return durationValue;
            }

            set
            {
                durationValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DurationValue"));
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

        #region Constructor
        public AddProfessionalEducationViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor AddProfessionalEducationViewModel()...", category: Category.Info, priority: Priority.Low);

                AddProfessionalEducationViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddProfessionalEducationViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddProfessionalEducationInformation));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFileCommandAction));
                OnDurationValueEditValueChangingCommand = new DelegateCommand<KeyEventArgs>(OnDurationValueEditValueChanging);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                GeosApplication.Instance.Logger.Log("Constructor AddProfessionalEducationViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddProfessionalEducationViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods     
        public void Init(ObservableCollection<EmployeeProfessionalEducation> EmployeeProfessionalEducationList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("AddProfessionalEducation").ToString();
                ExistEmployeeProfessionalEducationList = EmployeeProfessionalEducationList;
                FillEducationTypeList();
                FillTimeDurationList();

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(ObservableCollection<EmployeeProfessionalEducation> EmployeeProfessionalEducationList, EmployeeProfessionalEducation empEmployeeProfessionalEducation)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                WindowHeader = Application.Current.FindResource("EditProfessionalEducation").ToString();
                ExistEmployeeProfessionalEducationList = new ObservableCollection<EmployeeProfessionalEducation>(EmployeeProfessionalEducationList.ToList());
                ExistEmployeeProfessionalEducationList.Remove(empEmployeeProfessionalEducation);

                FillEducationTypeList();
                FillTimeDurationList();
                SelectedIndexEducationType = EducationTypeList.FindIndex(x => x.IdLookupValue == empEmployeeProfessionalEducation.IdType);
                SelectedIndexTimeDuration = DurationList.FindIndex(x => x.IdLookupValue == empEmployeeProfessionalEducation.IdDurationUnit);
                Description = empEmployeeProfessionalEducation.Name;
                Entity = empEmployeeProfessionalEducation.Entity;
                DurationValue = empEmployeeProfessionalEducation.DurationValue.ToString();
                ProfessionalEducationFileName = empEmployeeProfessionalEducation.FileName;
                EducationProfessionalEducationFileInBytes = empEmployeeProfessionalEducation.ProfessionalFileInBytes;
                StartDate = empEmployeeProfessionalEducation.StartDate;
                EndDate = empEmployeeProfessionalEducation.EndDate;
                Remarks = empEmployeeProfessionalEducation.Remarks;

                AttachmentList = new List<object>();
                if (empEmployeeProfessionalEducation.Attachment != null)
                {
                    AttachmentList.Add(empEmployeeProfessionalEducation.Attachment);
                }
                else if (!string.IsNullOrEmpty(ProfessionalEducationFileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = empEmployeeProfessionalEducation.FileName;
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
        public void InitReadOnly(EmployeeProfessionalEducation empEmployeeProfessionalEducation)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);

                WindowHeader = Application.Current.FindResource("EditProfessionalEducation").ToString();

                EducationTypeList = new List<LookupValue>();
               
                EducationTypeList.Insert(0, new LookupValue() { Value = empEmployeeProfessionalEducation.Name, InUse = true });
                DurationList = new List<LookupValue>();
                DurationList.Add(empEmployeeProfessionalEducation.DurationUnit);
                SelectedIndexEducationType = 0;
                SelectedIndexTimeDuration = 0;

                Description = empEmployeeProfessionalEducation.Name;
                Entity = empEmployeeProfessionalEducation.Entity;
                DurationValue = empEmployeeProfessionalEducation.DurationValue.ToString();
                ProfessionalEducationFileName = empEmployeeProfessionalEducation.FileName;
                EducationProfessionalEducationFileInBytes = empEmployeeProfessionalEducation.ProfessionalFileInBytes;
                StartDate = empEmployeeProfessionalEducation.StartDate;
                EndDate = empEmployeeProfessionalEducation.EndDate;
                Remarks = empEmployeeProfessionalEducation.Remarks;

                AttachmentList = new List<object>();
                if (empEmployeeProfessionalEducation.Attachment != null)
                {
                    AttachmentList.Add(empEmployeeProfessionalEducation.Attachment);
                }
                else if (!string.IsNullOrEmpty(ProfessionalEducationFileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = empEmployeeProfessionalEducation.FileName;
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

        public void OnDurationValueEditValueChanging(KeyEventArgs e)
        {
            if (DurationValue != null)
            {
              
                if (DurationValue.Length >= 4)
                    e.Handled = true;
            }
        }
        //void NumberValidation(object sender, KeyPressEventArgs e)
        //{
        //    if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 46)
        //    {
        //        e.Handled = false;
        //    }
        //    else
        //    {
        //        e.Handled = true;
        //    }
        //}
        private void FillEducationTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEducationTypeList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempList = CrmStartUp.GetLookupValues(25);
                EducationTypeList = new List<LookupValue>();
                EducationTypeList = new List<LookupValue>(tempList);
                EducationTypeList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                GeosApplication.Instance.Logger.Log("Method FillEducationTypeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEducationTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillTimeDurationList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTimeDurationList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempList = CrmStartUp.GetLookupValues(26);
                DurationList = new List<LookupValue>();
                DurationList = new List<LookupValue>(tempList);
                DurationList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                GeosApplication.Instance.Logger.Log("Method FillTimeDurationList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillTimeDurationList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void OnDateEditValueChanging(EditValueChangingEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);
            if (StartDate != null && EndDate != null)
            {
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
            }
            GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
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
                    EducationProfessionalEducationFileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    AttachmentList = new List<object>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    ProfessionalEducationFileName = file.Name;

                    List<object> newAttachmentList = new List<object>();

                    Attachment attachment = new Attachment();
                    attachment.FilePath = file.FullName;
                    attachment.OriginalFileName = file.Name;
                    attachment.IsDeleted = false;
                    attachment.FileByte = EducationProfessionalEducationFileInBytes;

                    newAttachmentList.Add(attachment);
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
                    me[BindableBase.GetPropertyName(() => Description)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexEducationType)] +
                     me[BindableBase.GetPropertyName(() => SelectedIndexTimeDuration)] +
                     me[BindableBase.GetPropertyName(() => StartDate)] +
                      me[BindableBase.GetPropertyName(() => EndDate)] +
                       me[BindableBase.GetPropertyName(() => DurationValue)];


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
                string eduEntity = BindableBase.GetPropertyName(() => Entity);
                string eduDescription = BindableBase.GetPropertyName(() => Description);
                string selectedEducationType = BindableBase.GetPropertyName(() => SelectedIndexEducationType);
                string selectedTimeDuration = BindableBase.GetPropertyName(() => SelectedIndexTimeDuration);
                string eduStartDate = BindableBase.GetPropertyName(() => StartDate);
                string eduEndDate = BindableBase.GetPropertyName(() => EndDate);
                string eduDurationValue = BindableBase.GetPropertyName(() => DurationValue);

                if (columnName == eduEntity)
                {
                    return EmployeeProfileValidation.GetErrorMessage(eduEntity, Entity);
                }
                if (columnName == eduDescription)
                {
                    return EmployeeProfileValidation.GetErrorMessage(eduDescription, Description);
                }

                if (columnName == selectedEducationType)
                {
                    return EmployeeProfileValidation.GetErrorMessage(selectedEducationType, SelectedIndexEducationType);
                }
                if (columnName == selectedTimeDuration)
                {
                    return EmployeeProfileValidation.GetErrorMessage(selectedTimeDuration, SelectedIndexTimeDuration);
                }
                if (columnName == eduStartDate)
                {
                    if (!string.IsNullOrEmpty(startDateErrorMessage))
                    {
                        return startDateErrorMessage;
                    }
                    else
                    {
                        return EmployeeProfileValidation.GetErrorMessage(eduStartDate, StartDate);
                    }
                }
                if (columnName == eduEndDate)
                {
                    if (!string.IsNullOrEmpty(endDateErrorMessage))
                    {
                        return endDateErrorMessage;
                    }
                    else
                    {
                        return EmployeeProfileValidation.GetErrorMessage(eduEndDate, EndDate);
                    }
                }
                if (columnName == eduDurationValue)
                {
                    return EmployeeProfileValidation.GetErrorMessage(eduDurationValue, DurationValue);
                }
                return null;
            }
        }

        private void AddProfessionalEducationInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddProfessionalEducationInformation()...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("Entity"));
                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexEducationType"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexTimeDuration"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("DurationValue"));

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

                if (AttachmentList != null && AttachmentList.Count == 0)
                {
                    ProfessionalEducationFileName = null;
                    EducationProfessionalEducationFileInBytes = null;                   
                }
                EmployeeProfessionalEducation TempNewEmployeeProfessionalEducation = new EmployeeProfessionalEducation();
                TempNewEmployeeProfessionalEducation = ExistEmployeeProfessionalEducationList.FirstOrDefault(x => x.IdType == EducationTypeList[SelectedIndexEducationType].IdLookupValue && x.Name == Description);
                if (TempNewEmployeeProfessionalEducation == null)
                {
                    EmployeeProfessionalEducation TempNewProfessionalEducationForFile = new EmployeeProfessionalEducation();
                    var ExistFilesRecord = ExistEmployeeProfessionalEducationList.Where(x => x.FileName != null);
                    TempNewProfessionalEducationForFile = ExistFilesRecord.FirstOrDefault(x => x.FileName == ProfessionalEducationFileName);
                    if (TempNewProfessionalEducationForFile == null)
                    {
                        if (IsNew == true)
                        {
                            NewProfessionalEducation = new EmployeeProfessionalEducation()
                            {
                                Type = EducationTypeList[SelectedIndexEducationType],
                                IdDurationUnit = DurationList[SelectedIndexTimeDuration].IdLookupValue,
                                IdType = EducationTypeList[SelectedIndexEducationType].IdLookupValue,
                                Name = Description,
                                Entity = Entity,
                                DurationValue = (ushort)Convert.ToInt32(DurationValue),
                                DurationUnit = DurationList[SelectedIndexTimeDuration],
                                StartDate = (DateTime)StartDate,
                                EndDate = (DateTime)EndDate,
                                FileName = ProfessionalEducationFileName,
                                ProfessionalFileInBytes = EducationProfessionalEducationFileInBytes,
                                Remarks = Remarks,
                                IsProfessionalFileDeleted = IsDeleteFile,                                             
                                TransactionOperation = ModelBase.TransactionOperations.Add
                            };

                            IsSave = true;
                            RequestClose(null, null);
                        }
                        else
                        {

                            EditProfessionalEducation = new EmployeeProfessionalEducation()
                            {
                                Type = EducationTypeList[SelectedIndexEducationType],
                                IdDurationUnit = DurationList[SelectedIndexTimeDuration].IdLookupValue,
                                IdType = EducationTypeList[SelectedIndexEducationType].IdLookupValue,
                                Name = Description,
                                Entity = Entity,
                                DurationValue = (ushort)Convert.ToInt32(DurationValue),
                                DurationUnit = DurationList[SelectedIndexTimeDuration],
                                StartDate = (DateTime)StartDate,
                                EndDate = (DateTime)EndDate,
                                FileName = ProfessionalEducationFileName,
                                ProfessionalFileInBytes = EducationProfessionalEducationFileInBytes,
                                Remarks = Remarks,
                                IsProfessionalFileDeleted = IsDeleteFile,                               
                                TransactionOperation = ModelBase.TransactionOperations.Update
                            };

                            IsSave = true;

                            RequestClose(null, null);
                        }
                    }
                    else
                    {
                        IsSave = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddProfessionalEducationFileExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    IsSave = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddProfessionalEducationExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method AddProfessionalEducationInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddProfessionalEducationInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
