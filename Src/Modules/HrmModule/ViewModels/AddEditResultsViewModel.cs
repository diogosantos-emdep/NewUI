using DevExpress.DataAccess.Wizard.Presenters;
using DevExpress.Mvvm;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.PCM;
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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddEditResultsViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {

        #region Service
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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

        #region Declaration
        private string windowHeader;
        private ObservableCollection<Employee> employeeListForResult;
        private bool isSave;
        private bool isNew;
        private bool isNewResult;
        private int selectedIndexForTrainee;
        private string name;
        private string code;
        private bool isBusy;
        private UInt32 idClassification;
        private string classification;
        private List<LookupValue> classificationList;
        private LookupValue selectedclassification;
        private string attachment;
        private int idEmployee;
        Employee employee;
        private string resultTitle;
        private Visibility isVisible;
        private List<object> selectedTraineeList;
        private ObservableCollection<ProfessionalSkill> professionalTrainingSkillForResult;
        private ObservableCollection<ProfessionalTrainingResults> professionalTraineeListForResult;
        private UInt64 idProfessionalTraining;
        private byte[]professionalTrainingResultFileInBytes;
        private List<object> attachmentList;
        private string professionalTrainingResultFileName;
        private float? resultDuration;
        private float? averageOfResults;
        private Int32 idProfessionalSkill;
        private UInt64 professionalTrainingId;
        private UInt64 idProfessionalTrainingResult;
        private UInt64 idProfessionalTrainingSkillResult;
        private ProfessionalTrainingResults updateTraineeResult;
        private ProfessionalTrainingResults oldTraineeResultDetatils;
        private string error = string.Empty;
        private double? acceptanceValue;
        float? skillDuration1;
        float? skillDuration2;
        float? skillDuration3;
        float? skillDuration4;
        float? skillDuration5;
        private string traineeResultFileName;
        #endregion

        #region properties
        public string TraineeResultFileName
        {
            get
            {
                return traineeResultFileName;
            }

            set
            {
                traineeResultFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TraineeResultFileName"));
            }
        }
        public float? SkillDuration1
        {
            get { return skillDuration1; }
            set
            {
                skillDuration1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillDuration1"));
            }
        }
        public float? SkillDuration2
        {
            get { return skillDuration2; }
            set
            {
                skillDuration2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillDuration2"));
            }
        }
        public float? SkillDuration3
        {
            get { return skillDuration3; }
            set
            {
                skillDuration3 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillDuration3"));
            }
        }
        public float? SkillDuration4
        {
            get { return skillDuration4; }
            set
            {
                skillDuration4 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillDuration4"));
            }
        }
        public float? SkillDuration5
        {
            get { return skillDuration5; }
            set
            {
                skillDuration5 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("skillDuration5"));
            }
        }
        public ProfessionalTrainingResults UpdateTraineeResult
        {
            get { return updateTraineeResult; }
            set
            {
                updateTraineeResult = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateTraineeResult"));
            }
        }
        public ProfessionalTrainingResults OldTraineeResultDetatils
        {
            get { return oldTraineeResultDetatils; }
            set
            {
                oldTraineeResultDetatils = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldTraineeResultDetatils"));
            }
        }
        public List<ProfessionalTrainingResults> NewResultList { get; set; }
        public List<ProfessionalTrainingResults> UpdateResultList { get; set; }
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
        public string ResultFileName
        {
            get
            {
                return professionalTrainingResultFileName;
            }

            set
            {
                professionalTrainingResultFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("professionalTrainingResultFileName"));
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
        public byte[] ProfessionalTrainingResultFileInBytes
        {
            get
            {
                return professionalTrainingResultFileInBytes;
            }

            set
            {
                professionalTrainingResultFileInBytes = value;
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
        public ObservableCollection<Employee> EmployeeListForResult
        {
            get { return employeeListForResult; }
            set
            {
                employeeListForResult = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeListForResult"));
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
        public UInt64 IdProfessionalTraining
        {
            get { return idProfessionalTraining; }
            set
            {
                idProfessionalTraining = value;
                OnPropertyChanged(new PropertyChangedEventArgs("idProfessionalTraining"));
            }
        }
        public UInt64 IdProfessionalTrainingResult
        {
            get { return idProfessionalTrainingResult; }
            set
            {
                idProfessionalTrainingResult = value;
                OnPropertyChanged(new PropertyChangedEventArgs("idProfessionalTrainingResult"));
            }
        }
        public UInt64 IdProfessionalTrainingSkillResult
        {
            get { return idProfessionalTrainingSkillResult; }
            set
            {
                idProfessionalTrainingSkillResult = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdProfessionalTrainingSkillResult"));
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
        private bool IsNewResult
        {
            get { return isNewResult; }
            set
            {
                isNewResult = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNewResult"));
            }
        }
        public int SelectedIndexForTrainee
        {
            get
            {
                return selectedIndexForTrainee;
            }

            set
            {
                selectedIndexForTrainee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForTrainee"));
            }
        }
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
            }
        }
        public UInt32 IdClassification
        {
            get
            {
                return idClassification;
            }
            set
            {
                idClassification = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdClassification"));
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
        public List<LookupValue> ClassificationList { get; set; }
        public LookupValue SelectedClassification
        {
            get
            {
                return selectedclassification;
            }
            set
            {
                selectedclassification = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedClassification"));
            }
        }
      
        public string Attachment
        {
            get
            {
                return attachment;
            }
            set
            {
               attachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Attachment"));
            }
        }
        public int IdEmployee
        {
            get
            {
                return idEmployee;
            }
            set
            {
                idEmployee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdEmployee"));
            }
        }
        public string ResultTitle
        {
            get { return resultTitle; }
            set
            {
                resultTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResultTitle"));
            }
        }
        public List<object> SelectedTraineeList
        {
            get { return selectedTraineeList; }
            set
            {
                selectedTraineeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTraineeList"));
            }
        }
        public ObservableCollection<ProfessionalTrainingResults> ProfessionalTraineeListForResult
        {
            get { return professionalTraineeListForResult; }
            set
            {
                professionalTraineeListForResult = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfessionalTraineeListForResult"));
            }
        }
        public ObservableCollection<ProfessionalSkill> ProfessionalTrainingSkillForResult
        {
            get { return professionalTrainingSkillForResult; }
            set
            {
                professionalTrainingSkillForResult = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfessionalTrainingSkillForResult"));
            }
        }
        public ProfessionalTrainingResults NewProfessionalResult { get; set; }
        public ProfessionalTrainingResults UpdateProfessionalResult { get; set; }

        public Visibility IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsVisible"));
            }
        }
        public float? ResultDuration
        {
            get { return resultDuration; }
            set
            {
                resultDuration = value;
                OnPropertyChanged(new PropertyChangedEventArgs(" ResultDuration"));
            }
        }
        public Int32 IdProfessionalSkill
        {
            get { return idProfessionalSkill; }
            set
            {
                idProfessionalSkill = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdProfessionalSkill"));
            }
        }
        public UInt64 ProfessionalTrainingId
        {
            get { return professionalTrainingId; }
            set
            {
                professionalTrainingId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfessionalTrainingId"));
            }
        }
        public double? AcceptanceValue
        {
            get
            {
                return acceptanceValue;
            }

            set
            {
                acceptanceValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AcceptanceValue"));
            }
        }
        public bool IsAdd { get; set; }
        #endregion


        #region Icommands
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        #endregion


        #region Constructor
        public AddEditResultsViewModel()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Constructor AddEditResultsViewModel ...", category: Category.Info, priority: Priority.Low);

                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFileCommandAction));
                GeosApplication.Instance.Logger.Log("Constructor AddEditResultsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddEditResultsViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        #region Methods
        public void Init(UInt64 IdProfessionalTraining, string AcceptanceValues, ObservableCollection<ProfessionalSkill> SkillsList, ObservableCollection<Employee>TraineeList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
               
                this.IsNewResult = true;
                this.IsNew = true;
                ProfessionalTrainingId = IdProfessionalTraining;

                EmployeeListForResult = new ObservableCollection<Employee>();
                EmployeeListForResult.AddRange(TraineeList);

                ProfessionalTrainingSkillForResult = new ObservableCollection<ProfessionalSkill>();
                ProfessionalTrainingSkillForResult.AddRange(SkillsList);

                ClassificationList = new List<LookupValue>();
                ClassificationList.AddRange(CrmStartUp.GetLookupValues(89));
                
                this.EmployeeListForResult = new ObservableCollection<Employee>(EmployeeListForResult.GroupBy(p => p.IdEmployee).Select(g => g.First()).ToList());
                Employee obj = new Employee();
                if (obj != null)
                {
                    SelectedIndexForTrainee = EmployeeListForResult.IndexOf(EmployeeListForResult.FirstOrDefault(x => x.IdEmployee == obj.IdEmployee));
                }

                IsVisible = Visibility.Hidden;
                IsAdd = true;
                AcceptanceValue = Convert.ToDouble(AcceptanceValues);
                SelectedTraineeList = new List<object>();
                if (obj != null)
                {
                    SelectedTraineeList.Add(obj.IdEmployee);
                }
                else if (SelectedIndexForTrainee > -1)
                {
                    SelectedTraineeList.Add(EmployeeListForResult[SelectedIndexForTrainee].IdEmployee);
                }

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
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(ProfessionalTrainingResults selectedGridRow, ObservableCollection<ProfessionalSkill> SkillList, string AcceptanceValues, ObservableCollection<Employee> TraineeList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                this.IsNew = false;
                this.IsNewResult = false;

                OldTraineeResultDetatils = new ProfessionalTrainingResults();
                OldTraineeResultDetatils = (ProfessionalTrainingResults)selectedGridRow.Clone();

                EmployeeListForResult = new ObservableCollection<Employee>();
                Employee employee = new Employee();
                employee.IdEmployee = selectedGridRow.IdEmployee;
                employee.EmployeeCode = selectedGridRow.EmployeeCode;
                employee.FirstName = selectedGridRow.FirstName;
                employee.LastName = selectedGridRow.LastName;
                EmployeeListForResult.Add(employee);

                IdProfessionalTrainingSkillResult = selectedGridRow.IdProfessionalTrainingSkillResult;
                IdProfessionalTrainingResult = selectedGridRow.IdProfessionalTrainingResult;
                IdProfessionalTraining = selectedGridRow.IdProfessionalTraining;
                TraineeResultFileName= selectedGridRow.ResultFileName;
                ProfessionalTrainingResultFileInBytes = selectedGridRow.TraineeDocumentFileInBytes;
                IdClassification = selectedGridRow.IdClassification;


                AttachmentList = new List<object>();
                if (selectedGridRow.Attachment != null)
                {
                    AttachmentList.Add(selectedGridRow.Attachment);
                }
                else if (!string.IsNullOrEmpty(TraineeResultFileName))
                {
                    Attachment attachment = new Attachment();
                    attachment.FilePath = null;
                    attachment.OriginalFileName = selectedGridRow.ResultFileName;
                    attachment.FileByte = ProfessionalTrainingResultFileInBytes;
                    attachment.IsDeleted = false;
                    AttachmentList.Add(attachment);
                }

                ClassificationList = new List<LookupValue>();
                ClassificationList.AddRange(CrmStartUp.GetLookupValues(89));

                ProfessionalTrainingSkillForResult = new ObservableCollection<ProfessionalSkill>();
                foreach (var item in SkillList)
                {
                    var clone = (ProfessionalSkill)item.Clone();
                    ProfessionalTrainingSkillForResult.Add(clone);
                }
                //ProfessionalTrainingSkillForResult.AddRange(SkillList);
                int cnt = 1;
                foreach (ProfessionalSkill item in ProfessionalTrainingSkillForResult)
                {
                    if (cnt == 1)
                    {
                        item.ResultDuration = selectedGridRow.SkillDuration1;

                    }
                    else if (cnt == 2)
                    {
                        item.ResultDuration = selectedGridRow.SkillDuration2;
                    }
                    else if (cnt == 3)
                    {
                        item.ResultDuration = selectedGridRow.SkillDuration3;
                    }
                    else if (cnt == 4)
                    {
                        item.ResultDuration = selectedGridRow.SkillDuration4;
                    }
                    else if (cnt == 5)
                    {
                        item.ResultDuration = selectedGridRow.SkillDuration5;
                    }
                    cnt = cnt + 1;
                }
                AcceptanceValue = 0;
                AcceptanceValue = Convert.ToDouble(AcceptanceValues);
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// AcceptButtonCommandAction Method is used for Accept Button for both Add and Edit Result
        /// </summary>
        /// <param name="obj"></param>
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForTrainee"));
                PropertyChanged(this, new PropertyChangedEventArgs("Duration"));

                if (error != null)
                {
                    return;
                }

                var SelectedTraineesListJoined = string.Join(",", SelectedTraineeList);
                // error = EnableValidationAndGetError();
                bool IsDeleteFile = false;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexForTrainee"));
                List<Attachment> temp = new List<Attachment>();
                if (AttachmentList != null && AttachmentList.Count == 0)
                {
                    traineeResultFileName = null;
                    ProfessionalTrainingResultFileInBytes = null;
                    temp = AttachmentList.Cast<Attachment>().ToList();
                }


                NewResultList = new List<ProfessionalTrainingResults>();
                List<int> SelectedTraineesList = SelectedTraineeList.Cast<int>().ToList();
                var EmployeeIds = string.Join(",", SelectedTraineesList.Select(i => i));
                #region Add New Result
                if (IsNew)
                {
                    float? sum = 0;
                    int cnt = 0;
                    for (int j = 0; j < SelectedTraineeList.Count; j++)
                    {
                        Employee emp = new Employee();
                        if (EmployeeListForResult.Any(k => k.IdEmployee == SelectedTraineesList[j]))
                        {
                            emp = EmployeeListForResult.Where(k => k.IdEmployee == SelectedTraineesList[j]).FirstOrDefault();
                        }
                       
                        NewProfessionalResult = new ProfessionalTrainingResults()
                        {
                            EmployeeCode = emp.EmployeeCode,
                            FirstName = emp.FirstName,
                            LastName=emp.LastName,
                            IdEmployee = Convert.ToInt16(SelectedTraineesList[j]),
                            IdProfessionalTraining = ProfessionalTrainingId,
                            ResultFileName = ResultFileName,
                            IdCreator = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser),
                            CreationDate = GeosApplication.Instance.ServerDateTime,
                            IsResultFileDeleted = IsDeleteFile,
                            Attachment = temp != null && temp.Count > 0 ? temp[0] : null,
                            TraineeDocumentFileInBytes = ProfessionalTrainingResultFileInBytes

                        };
                        foreach (ProfessionalSkill temp1 in ProfessionalTrainingSkillForResult)
                        {
                            ResultDuration = temp1.ResultDuration;
                            IdProfessionalSkill = temp1.IdProfessionalSkill;
                            sum =sum+ temp1.ResultDuration;
                            cnt++;
                        }
                        decimal decimalValue = Math.Round((decimal)(sum / cnt), 2);
                        NewProfessionalResult.Results = (float?)decimalValue;

                        if (NewProfessionalResult.Results >= AcceptanceValue)
                            NewProfessionalResult.IdClassification = 1573;
                        else
                            NewProfessionalResult.IdClassification = 1574;
                        NewProfessionalResult.SelectedTrainingResultSkillList = new List<ProfessionalSkill>(ProfessionalTrainingSkillForResult);
                        int cnt1 = 1;
                        foreach (ProfessionalSkill item in NewProfessionalResult.SelectedTrainingResultSkillList)
                        {
                            if (cnt1 == 1)
                            {
                                NewProfessionalResult.SkillDuration1 = item.ResultDuration;

                            }
                            else if (cnt1 == 2)
                            {
                                NewProfessionalResult.SkillDuration2 = item.ResultDuration;
                            }
                            else if (cnt1 == 3)
                            {
                                NewProfessionalResult.SkillDuration3 = item.ResultDuration;
                            }
                            else if (cnt1 == 4)
                            {
                                NewProfessionalResult.SkillDuration4 = item.ResultDuration;
                            }
                            else if (cnt1 == 5)
                            {
                                NewProfessionalResult.SkillDuration5 = item.ResultDuration;
                            }
                            cnt1 = cnt1 + 1;
                        }

                        SelectedClassification = ClassificationList.FirstOrDefault(x => x.IdLookupValue == NewProfessionalResult.IdClassification);
                        NewProfessionalResult.IdClassification = (uint)SelectedClassification.IdLookupValue;
                        NewProfessionalResult.Classification = SelectedClassification;

                        NewResultList.Add(NewProfessionalResult);
                    }
                   // NewResultList = HrmService.AddProfessionalTrainingResult(NewResultList);
                    IsSave = true;

                   // CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddTrainingResultSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    
                    RequestClose(null, null);
                }
                #endregion

                #region Edit Results
                else
                {
                    UpdateResultList = new List<ProfessionalTrainingResults>();
                    float? sum = 0;
                    int cnt = 0;

                    
                    for (int j = 0; j < SelectedTraineeList.Count; j++)
                    {
                        Employee emp = new Employee();
                        if (EmployeeListForResult.Any(k => k.IdEmployee == SelectedTraineesList[j]))
                        {
                            emp = EmployeeListForResult.Where(k => k.IdEmployee == SelectedTraineesList[j]).FirstOrDefault();
                        }
                        UpdateProfessionalResult = new ProfessionalTrainingResults()
                        {

                            EmployeeCode = emp.EmployeeCode,
                            FirstName = emp.FirstName,
                            LastName = emp.LastName,
                            IdEmployee = Convert.ToInt16(SelectedTraineesList[j]),
                            IdProfessionalTrainingResult = IdProfessionalTrainingResult,
                            IdProfessionalTraining = IdProfessionalTraining,
                            ResultFileName=TraineeResultFileName,
                            IdModifier = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser),
                            ModificationDate = GeosApplication.Instance.ServerDateTime,
                            IsResultFileDeleted = IsDeleteFile,
                            Attachment = temp != null && temp.Count > 0 ? temp[0] : null,
                            TraineeDocumentFileInBytes = ProfessionalTrainingResultFileInBytes
                        };
                        UpdateProfessionalResult.SelectedTrainingResultSkillList = new List<ProfessionalSkill>();
                        foreach (ProfessionalSkill temp2 in ProfessionalTrainingSkillForResult)
                        {


                            IdProfessionalTrainingSkillResult = temp2.IdProfessionalTrainingSkillResult;
                            ResultDuration = temp2.ResultDuration;
                            IdProfessionalSkill = temp2.IdProfessionalSkill;
                            sum = sum + temp2.ResultDuration;
                            cnt++;

                        }
                        decimal decimalValue = Math.Round((decimal)(sum / cnt), 2);
                        UpdateProfessionalResult.Results = (float?)decimalValue;
                       // UpdateProfessionalResult.Results = sum / cnt;

                        if (UpdateProfessionalResult.Results >= AcceptanceValue)
                            UpdateProfessionalResult.IdClassification = 1573;
                        else
                            UpdateProfessionalResult.IdClassification = 1574;
                        UpdateProfessionalResult.SelectedTrainingResultSkillList = new List<ProfessionalSkill>(ProfessionalTrainingSkillForResult);

                        int cnt1 = 1;
                        foreach (ProfessionalSkill item in UpdateProfessionalResult.SelectedTrainingResultSkillList)
                        {
                            if (cnt1 == 1)
                            {
                                UpdateProfessionalResult.SkillDuration1 = item.ResultDuration;

                            }
                            else if (cnt1 == 2)
                            {
                                UpdateProfessionalResult.SkillDuration2 = item.ResultDuration;
                            }
                            else if (cnt1 == 3)
                            {
                                UpdateProfessionalResult.SkillDuration3 = item.ResultDuration;
                            }
                            else if (cnt1 == 4)
                            {
                                UpdateProfessionalResult.SkillDuration4 = item.ResultDuration;
                            }
                            else if (cnt1 == 5)
                            {
                                UpdateProfessionalResult.SkillDuration5 = item.ResultDuration;
                            }
                            cnt1 = cnt1 + 1;
                        }
                        if(UpdateProfessionalResult.IdClassification != 0)
                        {
                            SelectedClassification = ClassificationList.FirstOrDefault(x => x.IdLookupValue == UpdateProfessionalResult.IdClassification);
                            UpdateProfessionalResult.IdClassification = (uint)SelectedClassification.IdLookupValue;
                            UpdateProfessionalResult.Classification = SelectedClassification;
                        }
                        UpdateResultList.Add(UpdateProfessionalResult);
                    }

                   
                    IsSave = true;
                    RequestClose(null, null);
                 
                }
                #endregion

                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Error in Method AcceptButtonCommandAction()...." + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method AcceptButtonCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SkillsAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// CloseWindow Method is used for Cancel Button
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                // IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void BrowseFileCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileCommandAction() ...", category: Category.Info, priority: Priority.Low);

            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = "Pdf Files|*.pdf";
                dlg.Filter = "Pdf Files|*.pdf";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    ProfessionalTrainingResultFileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                   
                    AttachmentList = new List<object>();

                    System.IO.FileInfo file = new System.IO.FileInfo(dlg.FileName);
                    ResultFileName = file.Name;
                    TraineeResultFileName = file.Name;
                    List<object> newAttachmentList = new List<object>();

                    Attachment attachment = new Attachment();
                    attachment.FilePath = file.FullName;
                    attachment.OriginalFileName = file.Name;
                    attachment.IsDeleted = false;
                    attachment.FileByte = ProfessionalTrainingResultFileInBytes;

                    newAttachmentList.Add(attachment);

                    AttachmentList = newAttachmentList;
                }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method BrowseFileCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method BrowseFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        #endregion

        #region Validation
        bool allowValidation = false;

        string EnableValidationAndGetError()
        {
            allowValidation = true;
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
                    me[BindableBase.GetPropertyName(() => SelectedIndexForTrainee)] +
                    me[BindableBase.GetPropertyName(() => ResultDuration)];


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
                string traineeName = BindableBase.GetPropertyName(() => SelectedIndexForTrainee);
                string duration = BindableBase.GetPropertyName(() => ResultDuration);


                if (columnName == traineeName)
                {
                    return ResultInTrainingValidation.GetErrorMessage(traineeName, SelectedIndexForTrainee);
                }
                if (columnName == duration)
                {
                    return ResultInTrainingValidation.GetErrorMessage(duration, ResultDuration);
                }

                return null;
            }
        }
        #endregion
    }
}
