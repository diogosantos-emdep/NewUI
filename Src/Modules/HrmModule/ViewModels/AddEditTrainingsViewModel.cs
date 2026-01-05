using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.UI.Validations;
using System.ServiceModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.Accordion;
using DevExpress.Data.Filtering;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddEditTrainingsViewModel : NavigationViewModelBase, IDisposable, INotifyPropertyChanged, IDataErrorInfo
    {
        public void Dispose()
        {
        }

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

        #endregion // End Of Events 

        #region Services
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region ICommands

        public ICommand EscapeButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AddNewSkillCommand { get; set; }
        public ICommand GenearateCertificateCommand { get; set; }//rajashri GEOS2-4911
        public ICommand EditSkillCommand { get; set; }
        public ICommand AcceptTrainingActionCommand { get; set; }

        public ICommand DeleteSkillCommand { get; set; }

        public ICommand AddNewTraineesCommand { get; set; }
        public ICommand DeleteTraineesCommand { get; set; }
        public ICommand AddNewResultsCommand { get; set; }
        public ICommand EditResultCommand { get; set; }
        public ICommand TraineeResultDocumentViewCommand { get; set; }

        public ICommand AddFileCommand { get; set; }
        public ICommand OpenPDFDocumentCommand { get; set; }
        public ICommand EditFileCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }
        public ICommand CommandShowFilterPopupForTranieeClick { get; set; }
        #endregion

        #region Declaration

        private string windowHeader;
        private bool isNew;
        private bool isSave;

        private double dialogHeight;
        private double dialogWidth;
        private int selectedBoxIndex;
        private string name;
        private string error = string.Empty;
        private string informationError;

        private string selectedProfessionalTrainingCode;
        private string description;
        private DateTime expectedDate;
        private DateTime? finalizationDate;
        private float duration;
        private float Result;
        private LookupValue selectedStatus;
        private LookupValue selectedType;
        private LookupValue selectedAcceptance;
        private string acceptanceValue;
        private Employee responsible;
        private Employee trainer;
        private string externalTrainer;
        private string externalEntity;
        private List<LookupValue> statusList;
        private List<LookupValue> typeList;
        private List<LookupValue> acceptanceList;

        private List<Employee> responsibleList;
        private Employee selectedResponsible;

        public List<Employee> trainerList { get; set; }
        public Employee selectedTrainer { get; set; }

        public ProfessionalTraining NewProfessionalTraining { get; set; }
        public ProfessionalTraining UpdateProfessionalTraining { get; set; }
        public bool isEntityExtTraineeEnable { get; set; }

        public bool isAcceptanceValueEnable { get; set; }
        public UInt64 idProfessionalTraining { get; set; }
        private ProfessionalTraining objProfessionalTrainingClone;

        private ObservableCollection<ProfessionalSkill> skillList;
        private ProfessionalSkill selectedSkill;
        private bool isDeleteColumnVisible;

        private ObservableCollection<Employee> traineesList;
        private Employee selectedTrainees;
        private Visibility isVisible;
        private ObservableCollection<ProfessionalTrainingResults> resultList;
        private float? averageOfResult;
        private string resultSkill1;
        private string resultSkill2;
        private string resultSkill3;
        private string resultSkill4;
        private string resultSkill5;
        private bool isVisibleResultSkill1;
        private bool isVisibleResultSkill2;
        private bool isVisibleResultSkill3;
        private bool isVisibleResultSkill4;
        private bool isVisibleResultSkill5;
        string skillName1;
        string skillName2;
        string skillName3;
        string skillName4;
        string skillName5;
        string resultFileName;
        ProfessionalTrainingResults selectedTraineeResultRow;
        private string remarkForProfEdu;
        private ProfessionalTraining trainingDetail;
        private ProfessionalTraining trainingUpdatedDetail;
        private ObservableCollection<EmployeeChangelog> employeeChangeLogList;
        private ProfessionalTraining trainingExistDetail;
        string S1 = "";
        string S2 = "";
        string S3 = "";
        string S4 = "";
        string S5 = "";
        private List<LookupValue> classificationList;
        private LookupValue selectedclassification;
        private float? skillDuration1;
        private float? skillDuration2;
        private float? skillDuration3;
        private float? skillDuration4;
        private float? skillDuration5;
        private float? skillResult1=0;
        private float? skillResult2=0;
        private float? skillResult3=0;
        private float? skillResult4=0;
        private float? skillResult5=0;
        private ObservableCollection<ProfessionalTrainingAttachments> attachmentsList;
        private ProfessionalTrainingAttachments selectedProfTrainingFile;
        private ObservableCollection<ProfessionalTrainingAttachments> fourRecordsTrainingFilesList;
        private string attachedFileName;
        private ObservableCollection<TrainingChangeLog> trainingAllChangeLogList;
        private ObservableCollection<TrainingChangeLog> trainingChangeLogList;
        private ObservableCollection<TrainingChangeLog> trainingNewChangeLogList;
        #endregion

        #region Properties
        public ObservableCollection<ProfessionalTrainingAttachments> FourRecordsTrainingFilesList
        {
            get
            {
                return fourRecordsTrainingFilesList;
            }

            set
            {
                fourRecordsTrainingFilesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FourRecordsTrainingFilesList"));
            }
        }
        public ProfessionalTrainingAttachments SelectedProfTrainingFile
        {
            get
            {
                return selectedProfTrainingFile;
            }
            set
            {
                selectedProfTrainingFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProfTrainingFile"));
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
                skillDuration2= value;
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
                OnPropertyChanged(new PropertyChangedEventArgs("SkillDuration5"));
            }
        }

        public float? SkillResult1
        {
            get { return skillResult1; }
            set
            {
                skillResult1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillResult1"));
            }

        }
        public float? SkillResult2
        {
            get { return skillResult2; }
            set
            {
                skillResult2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillResult2"));
            }

        }
        public float? SkillResult3
        {
            get { return skillResult3; }
            set
            {
                skillResult3 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillResult3"));
            }

        }
        public float? SkillResult4
        {
            get { return skillResult4; }
            set
            {
                skillResult4 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillResult4"));
            }

        }
        public float? SkillResult5
        {
            get { return skillResult5; }
            set
            {
                skillResult5 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillResult5"));
            }

        }

        public List<LookupValue> ClassificationList { get; set; }
        public ProfessionalTraining TrainingExistDetail
        {
            get { return trainingExistDetail; }
            set
            {
                trainingExistDetail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TrainingExistDetail"));
            }
        }
        public ObservableCollection<EmployeeChangelog> EmployeeChangeLogList
        {
            get { return employeeChangeLogList; }
            set
            {
                employeeChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeChangeLogList"));
            }
        }

        public ProfessionalTraining TrainingUpdatedDetail
        {
            get { return trainingUpdatedDetail; }
            set
            {
                trainingUpdatedDetail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TrainingUpdatedDetail"));
            }
        }
        public ProfessionalTrainingResults SelectedTraineeResultRow
        {
            get { return selectedTraineeResultRow; }
            set
            {
                selectedTraineeResultRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTraineeResultRow"));
            }
        }
        public string RemarkForProfEdu
        {
            get { return remarkForProfEdu; }
            set
            {
                remarkForProfEdu = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RemarkForProfEdu"));
            }
        }
        public string ResultFileName
        {
            get { return resultFileName; }
            set
            {
                resultFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResultFileName"));
            }
        }
        public string AttachedFileName
        {
            get { return attachedFileName; }
            set
            {
                attachedFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachedFileName"));
            }
        }
        public string SkillName1
        {
            get { return skillName1; }
            set
            {
                skillName1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillName1"));
            }
        }
        public string SkillName2
        {
            get { return skillName2; }
            set
            {
                skillName2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillName2"));
            }
        }
        public string SkillName3
        {
            get { return skillName3; }
            set
            {
                skillName3 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillName3"));
            }
        }
        public string SkillName4
        {
            get { return skillName4; }
            set
            {
                skillName4 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillName4"));
            }
        }
        public string SkillName5
        {
            get { return skillName5; }
            set
            {
                skillName5 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillName5"));
            }
        }
        public string ResultSkill1
        {
            get
            {
                return resultSkill1;
            }

            set
            {
                resultSkill1 = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ResultSkill1"));

            }
        }


        public string ResultSkill2
        {
            get
            {
                return resultSkill2;
            }

            set
            {
                resultSkill2 = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ResultSkill2"));

            }
        }

        public string ResultSkill3
        {
            get
            {
                return resultSkill3;
            }

            set
            {
                resultSkill3 = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ResultSkill3"));

            }
        }

        public string ResultSkill4
        {
            get
            {
                return resultSkill4;
            }

            set
            {
                resultSkill4 = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ResultSkill4"));

            }
        }

        public string ResultSkill5
        {
            get
            {
                return resultSkill5;
            }

            set
            {
                resultSkill5 = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ResultSkill5"));

            }
        }

        public bool IsVisibleResultSkill1
        {
            get
            {
                return isVisibleResultSkill1;
            }

            set
            {
                isVisibleResultSkill1 = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IsVisibleResultSkill1"));

            }
        }


        public bool IsVisibleResultSkill2
        {
            get
            {
                return isVisibleResultSkill2;
            }

            set
            {
                isVisibleResultSkill2 = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IsVisibleResultSkill2"));

            }
        }

        public bool IsVisibleResultSkill3
        {
            get
            {
                return isVisibleResultSkill3;
            }

            set
            {
                isVisibleResultSkill3 = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IsVisibleResultSkill3"));

            }
        }

        public bool IsVisibleResultSkill4
        {
            get
            {
                return isVisibleResultSkill4;
            }

            set
            {
                isVisibleResultSkill4 = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IsVisibleResultSkill4"));

            }
        }

        public bool IsVisibleResultSkill5
        {
            get
            {
                return isVisibleResultSkill5;
            }

            set
            {
                isVisibleResultSkill5 = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IsVisibleResultSkill5"));

            }
        }
        public float? AverageOfResult
        {
            get { return averageOfResult; }
            set
            {
                averageOfResult = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AverageOfResult"));
            }
        }
        public bool IsAcceptanceValueEnable
        {
            get
            {
                return isAcceptanceValueEnable;
            }
            set
            {
                isAcceptanceValueEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptanceValueEnable"));
            }
        }
        public bool IsEntityExtTraineeEnable
        {
            get
            {
                return isEntityExtTraineeEnable;
            }
            set
            {
                isEntityExtTraineeEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEntityExtTraineeEnable"));
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

                //if (WindowHeader == System.Windows.Application.Current.FindResource("AddNewTrainings").ToString())
                //{
                //    IsVisible = Visibility.Visible;
                //}
                //else
                //{
                //    IsVisible = Visibility.Collapsed;
                //}
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

        public ProfessionalTraining ObjProfessionalTrainingClone
        {
            get
            {
                return objProfessionalTrainingClone;
            }

            set
            {
                objProfessionalTrainingClone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjProfessionalTrainingClone"));
            }
        }

        //public string Name
        //{
        //    get
        //    {
        //        return name;
        //    }

        //    set
        //    {
        //        name = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("Name"));
        //    }
        //}

        public UInt64 IdProfessionalTraining
        {
            get
            {
                return idProfessionalTraining;
            }

            set
            {
                idProfessionalTraining = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdProfessionalTraining"));
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
                if (!string.IsNullOrEmpty(value))
                {
                    InformationError = null;
                }
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

        public string SelectedProfessionalTrainingCode
        {
            get
            {
                return selectedProfessionalTrainingCode;
            }

            set
            {
                selectedProfessionalTrainingCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProfessionalTrainingCode"));
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

        public DateTime ExpectedDate
        {
            get
            {
                return expectedDate;
            }

            set
            {
                expectedDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExpectedDate"));
            }
        }

        public DateTime? FinalizationDate
        {
            get
            {
                return finalizationDate;
            }

            set
            {
                finalizationDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FinalizationDate"));
            }
        }

        public float Duration
        {
            get
            {
                return duration;
            }

            set
            {
                duration = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Duration"));
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

                if(SelectedStatus.IdLookupValue == 1485)
                {
                    IsDeleteColumnVisible = true;
                }
                else
                {
                    IsDeleteColumnVisible = true;
                }
            }
        }

        public LookupValue SelectedType
        {
            get
            {
                return selectedType;
            }

            set
            {
                selectedType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedType"));
            }
        }

        public LookupValue SelectedAcceptance
        {
            get
            {
                return selectedAcceptance;
            }

            set
            {
                selectedAcceptance = value;
                if (value == AcceptanceList[0])
                {
                    IsAcceptanceValueEnable = true;
                }
                else
                {
                    IsAcceptanceValueEnable = false;
                    AcceptanceValue = null;
                    // error = string.Empty;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAcceptance"));
            }
        }
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

        public string AcceptanceValue
        {
            get
            {
                return acceptanceValue;
            }

            set
            {
                acceptanceValue = value;

                if (ResultList != null)
                {
                    foreach (ProfessionalTrainingResults item in ResultList)
                    {
                        if (ClassificationList == null)
                        {
                            ClassificationList = new List<LookupValue>();
                            ClassificationList.AddRange(CrmService.GetLookupValues(89));
                        }
                        if (item.Results >= Convert.ToDouble(acceptanceValue))
                        {
                            item.IdClassification = 1573;
                            SelectedClassification = ClassificationList.FirstOrDefault(x => x.IdLookupValue == item.IdClassification);
                            item.IdClassification = (uint)SelectedClassification.IdLookupValue;
                            item.Classification = SelectedClassification;
                        }
                            
                        else
                        {
                            item.IdClassification = 1574;
                            SelectedClassification = ClassificationList.FirstOrDefault(x => x.IdLookupValue == item.IdClassification);
                            item.IdClassification = (uint)SelectedClassification.IdLookupValue;
                            item.Classification = SelectedClassification;
                        }
                            
                    }
                }
               

                OnPropertyChanged(new PropertyChangedEventArgs("AcceptanceValue"));
            }
        }

        public Employee Responsible
        {
            get
            {
                return responsible;
            }

            set
            {
                responsible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Responsible"));

            }
        }

        public Employee Trainer
        {
            get
            {
                return trainer;
            }

            set
            {
                trainer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Trainer"));

            }
        }

        public string ExternalTrainer
        {
            get
            {
                return externalTrainer;
            }

            set
            {
                externalTrainer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExternalTrainer"));
                if (!string.IsNullOrEmpty(value))
                {
                    InformationError = null;
                }
            }
        }

        public string ExternalEntity
        {
            get
            {
                return externalEntity;
            }

            set
            {
                externalEntity = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ExternalEntity"));
                if (!string.IsNullOrEmpty(value))
                {
                    InformationError = null;
                }
            }
        }

        public List<LookupValue> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }

        public List<LookupValue> TypeList
        {
            get { return typeList; }
            set
            {
                typeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TypeList"));
            }
        }

        public List<LookupValue> AcceptanceList
        {
            get
            {
                return acceptanceList;
            }

            set
            {
                acceptanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AcceptanceList"));
            }
        }

        public List<Employee> ResponsibleList
        {
            get
            {
                return responsibleList;
            }

            set
            {
                responsibleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AcceptanceList"));
            }
        }

        public Employee SelectedResponsible
        {
            get
            {
                return selectedResponsible;
            }

            set
            {
                selectedResponsible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedResponsible"));
                if (!string.IsNullOrEmpty(SelectedResponsible.FullName))
                {
                    InformationError = null;
                }
            }
        }

        public Employee SelectedTrainer
        {
            get
            {
                return selectedTrainer;
            }

            set
            {
                selectedTrainer = value;
                if (value == TrainerList[0])
                {
                    IsEntityExtTraineeEnable = true;
                    ExternalEntity = string.Empty;
                    ExternalTrainer = string.Empty;
                }
                else
                {
                    IsEntityExtTraineeEnable = false;
                    ExternalEntity = "EMDEP";
                    ExternalTrainer = string.Empty;

                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTrainer"));
            }
        }

        public List<Employee> TrainerList
        {
            get
            {
                return trainerList;
            }

            set
            {
                trainerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TrainerList"));
            }
        }

        public ObservableCollection<ProfessionalSkill> SkillList
        {
            get
            {
                return skillList;
            }

            set
            {
                skillList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillList"));
            }
        }

        public ProfessionalSkill SelectedSkill
        {
            get
            {
                return selectedSkill;
            }

            set
            {
                selectedSkill = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSkill"));
            }
        }

        public bool IsDeleteColumnVisible
        {
            get
            {
                return isDeleteColumnVisible;
            }

            set
            {
                isDeleteColumnVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleteColumnVisible"));
            }
        }

        public int SelectedBoxIndex
        {
            get
            {
                return selectedBoxIndex;
            }

            set
            {
                selectedBoxIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBoxIndex"));
            }
        }


        public ObservableCollection<Employee> TraineesList
        {
            get
            {
                return traineesList;
            }

            set
            {
                traineesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TraineesList"));
            }
        }

        public Employee SelectedTrainees
        {
            get
            {
                return selectedTrainees;
            }

            set
            {
                selectedTrainees = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTrainees"));
            }
        }


        public Visibility IsVisible
        {
            get
            {
                return isVisible;
            }

            set
            {
                isVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsVisible"));
            }
        }
        public ObservableCollection<ProfessionalTrainingResults> ResultList
        {
            get
            {
                return resultList;
            }

            set
            {
               resultList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResultList"));
            }
        }
        public ProfessionalTraining TrainingDetail
        {
            get { return trainingDetail; }
            set
            {
                trainingDetail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TrainingDetail"));
            }
        }
        public ObservableCollection<ProfessionalTrainingAttachments> AttachmentsList
        {
            get
            {
                return attachmentsList;
            }

            set
            {
                attachmentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentsList"));
            }
        }
        public ObservableCollection<TrainingChangeLog> TrainingAllChangeLogList
        {
            get { return trainingAllChangeLogList; }
            set
            {
                trainingAllChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TrainingChangeLogList"));
            }
        }

        public ObservableCollection<TrainingChangeLog> TrainingChangeLogList
        {
            get { return trainingChangeLogList; }
            set
            {
                trainingChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TrainingChangeLogList"));
            }
        }

        public ObservableCollection<TrainingChangeLog> TrainingNewChangeLogList
        {
            get { return trainingNewChangeLogList; }
            set
            {
                trainingNewChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TrainingChangeLogList"));
            }
        }

        bool isCertificateGenerationEnabled = false;
        public bool IsCertificateGenerationEnabled
        {
            get
            {
                return isCertificateGenerationEnabled;
            }

            set
            {
                isCertificateGenerationEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCertificateGenerationEnabled"));
            }
        }
        #endregion

        #region Constructor

        public AddEditTrainingsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditTrainingsViewModel ...", category: Category.Info, priority: Priority.Low);

                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 90;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                AcceptTrainingActionCommand = new DelegateCommand<object>(AcceptTrainingActionCommandAction);

                AddNewSkillCommand = new DelegateCommand<object>(AddNewSkillCommandAction);
                EditSkillCommand = new DelegateCommand<object>(EditSkillCommandAction);
                DeleteSkillCommand = new DelegateCommand<object>(DeleteSkillCommandAction);
                GenearateCertificateCommand = new DelegateCommand<object>(GenearateCertificateCommandAction);//rajashri GEOS2-4911
                AddNewTraineesCommand = new DelegateCommand<object>(AddNewTraineesCommandAction);
                DeleteTraineesCommand = new DelegateCommand<object>(DeleteTraineesCommandAction);

                AddNewResultsCommand = new DelegateCommand<object>(AddNewResultsCommandAction);
                EditResultCommand = new DelegateCommand<object>(EditResultCommandAction);
                TraineeResultDocumentViewCommand = new RelayCommand(new Action<object>(OpenTraineeResultDocument));
                //CustomUnboundColumnDataCommand = new DelegateCommand<object>(CustomUnboundColumnDataAction);

                AddFileCommand = new DelegateCommand<object>(AddFile);
                OpenPDFDocumentCommand = new RelayCommand(new Action<object>(OpenPDFDocument));
                DeleteFileCommand = new DelegateCommand<object>(DeleteFile);
                EditFileCommand = new DelegateCommand<object>(EditFile);
                CommandShowFilterPopupForTranieeClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupForTrainee);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor AddEditTrainingsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditTrainingsViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion

        #region Methods

        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [001][avpawar][GEOS2-3317][Sr No.2 HRM - Trainings 5 of 8 [#TRN05] ( Add Trainees)]
        /// </summary>
        /// <param name="obj"></param>
        private void AcceptTrainingActionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptTrainingActionCommandAction()...", category: Category.Info, priority: Priority.Low);
                InformationError = null;
                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedResponsible"));
                PropertyChanged(this, new PropertyChangedEventArgs("ExternalEntity"));
                PropertyChanged(this, new PropertyChangedEventArgs("ExternalTrainer"));
                PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = " ";
                if (error != null)
                {
                    return;
                }

                if(SkillList == null || SkillList.Count == 0)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingZeroSkillWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
               
                if (IsNew)
                {
                    #region New
                    NewProfessionalTraining = new ProfessionalTraining();

                    NewProfessionalTraining.Code = SelectedProfessionalTrainingCode;

                    if(!string.IsNullOrEmpty(Name))
                        NewProfessionalTraining.Name = Name.Trim();
                    else
                        NewProfessionalTraining.Name = Name;

                    if(!string.IsNullOrEmpty(Description))
                        NewProfessionalTraining.Description = Description.Trim();
                    else
                        NewProfessionalTraining.Description = Description;

                    if ((uint)SelectedStatus.IdLookupValue == 1487)
                    {
                        if (FinalizationDate != null && FinalizationDate >= ExpectedDate)
                        {
                            NewProfessionalTraining.IdStatus = (uint)SelectedStatus.IdLookupValue;
                        }
                    }
                    else
                    {
                        NewProfessionalTraining.IdStatus = (uint)SelectedStatus.IdLookupValue;
                    }
                    NewProfessionalTraining.ExpectedDate = ExpectedDate;
                   // NewProfessionalTraining.FinalizationDate = FinalizationDate;
                    NewProfessionalTraining.Duration = Duration;
                    NewProfessionalTraining.AcceptanceValue = AcceptanceValue;
                    NewProfessionalTraining.IdStatus = (uint)SelectedStatus.IdLookupValue;
                    NewProfessionalTraining.IdType = (uint)SelectedType.IdLookupValue;
                    NewProfessionalTraining.IdAcceptance = (uint)SelectedAcceptance.IdLookupValue;
                    NewProfessionalTraining.IdResponsible = (uint)SelectedResponsible.IdEmployee;
                    if ((uint?)SelectedTrainer.IdEmployee == 0)
                    {
                        NewProfessionalTraining.IdTrainer = null;
                    }
                    else
                    {
                        NewProfessionalTraining.IdTrainer = (uint?)SelectedTrainer.IdEmployee;
                    }

                    if(!string.IsNullOrEmpty(ExternalTrainer))
                        NewProfessionalTraining.ExternalTrainer = ExternalTrainer.Trim();
                    else
                        NewProfessionalTraining.ExternalTrainer = ExternalTrainer;

                    if(!string.IsNullOrEmpty(ExternalEntity))
                        NewProfessionalTraining.ExternalEntity = ExternalEntity.Trim();
                    else
                        NewProfessionalTraining.ExternalEntity = ExternalEntity;

                    if (FinalizationDate != null && FinalizationDate < ExpectedDate.Date)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingFinalizationDateWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                    if (FinalizationDate != null && ResultList.Count == 0)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingResultCountWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                    if (ResultList != null && ResultList.Count > 0)
                    {
                        if (FinalizationDate != null)
                        {
                            foreach (var item in ResultList)
                            {
                                int cnt = 1;
                                foreach (ProfessionalSkill itemProfessionalSkill in SkillList)
                                {
                                    if (cnt == 1)
                                    {
                                        SkillName1 = itemProfessionalSkill.Name;
                                    }
                                    if (cnt == 2)
                                    {
                                        SkillName2 = itemProfessionalSkill.Name;
                                    }
                                    if (cnt == 3)
                                    {
                                        SkillName3 = itemProfessionalSkill.Name;
                                    }
                                    if (cnt == 4)
                                    {
                                        SkillName4 = itemProfessionalSkill.Name;
                                    }
                                    if (cnt == 5)
                                    {
                                        SkillName5 = itemProfessionalSkill.Name;
                                    }

                                    cnt = cnt + 1;
                                }

                                if (SkillName1 != null && SkillName1 != "")
                                {
                                    SkillResult1 = item.SkillDuration1;
                                }
                                if (SkillName2 != null && SkillName2 != "")
                                {
                                    SkillResult2 = item.SkillDuration2;
                                }
                                if (SkillName3 != null && SkillName3 != "")
                                {
                                    SkillResult3 = item.SkillDuration3;
                                }
                                if (SkillName4 != null && SkillName4 != "")
                                {
                                    SkillResult4 = item.SkillDuration4;
                                }
                                if (SkillName5 != null && SkillName5 != "")
                                {
                                    SkillResult5 = item.SkillDuration5;
                                }

                                if (SkillResult1 == null || SkillResult2 == null || SkillResult3 == null || SkillResult4 == null || SkillResult5 == null)
                                {
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingResultValueWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return;
                                }
                            }
                        }
                          if (!(SkillResult1 == null || SkillResult2 == null || SkillResult3 == null || SkillResult4 == null || SkillResult5 == null) && FinalizationDate >= ExpectedDate.Date)
                            {
                                NewProfessionalTraining.FinalizationDate = FinalizationDate;
                            }
                            if (!ResultList.Any(x=>x.Results==0) && NewProfessionalTraining.FinalizationDate == null)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingFinalizationDateMandatoryWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                    }
                    if (NewProfessionalTraining.FinalizationDate != null)
                        NewProfessionalTraining.IdStatus = 1487;
                    else
                    {
                        if (NewProfessionalTraining.IdStatus == 1487)
                            NewProfessionalTraining.IdStatus = 1485;
                    }


                    NewProfessionalTraining.RemarkForProfEdu = RemarkForProfEdu;
                    NewProfessionalTraining.ResultFileName = ResultFileName;

                    if (NewProfessionalTraining.ProfessionalSkillList == null)
                    {
                        NewProfessionalTraining.ProfessionalSkillList = new List<ProfessionalSkill>();
                    }
                    
                    foreach(ProfessionalSkill temp in SkillList)
                    {
                        temp.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        temp.CreatedIn = GeosApplication.Instance.ServerDateTime;
                    }

                    NewProfessionalTraining.ProfessionalSkillList = SkillList.ToList();


                    // [001] Trainee start
                    if (NewProfessionalTraining.TraineeList == null)
                    {
                        NewProfessionalTraining.TraineeList = new List<Employee>();
                    }

                    if(TraineesList != null)
                    {
                        foreach (Employee temp in TraineesList)
                        {
                            temp.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            temp.CreatedIn = GeosApplication.Instance.ServerDateTime;
                        }

                        NewProfessionalTraining.TraineeList = TraineesList.ToList();
                    }
                   

                    // [001] Result start
                    if (NewProfessionalTraining.ProfessionalTrainingResultList == null)
                    {
                        NewProfessionalTraining.ProfessionalTrainingResultList = new List<ProfessionalTrainingResults>();
                    }
                    if (ResultList != null)
                    {
                        foreach (ProfessionalTrainingResults temp in ResultList)
                        {
                            temp.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                            temp.CreationDate = GeosApplication.Instance.ServerDateTime;
                        }
                        NewProfessionalTraining.ProfessionalTrainingResultList = ResultList.ToList();
                    }

                    foreach (ProfessionalTrainingResults item in NewProfessionalTraining.ProfessionalTrainingResultList)
                    {
                        int cnt = 1;
                        foreach (ProfessionalSkill itemProfessionalSkill in NewProfessionalTraining.ProfessionalSkillList)
                        {
                            if (cnt == 1)
                            {
                                SkillName1 = itemProfessionalSkill.Name;
                            }
                            if (cnt == 2)
                            {
                                SkillName2 = itemProfessionalSkill.Name;
                            }
                            if (cnt == 3)
                            {
                                SkillName3 = itemProfessionalSkill.Name;
                            }
                            if (cnt == 4)
                            {
                                SkillName4 = itemProfessionalSkill.Name;
                            }
                            if (cnt == 5)
                            {
                                SkillName5 = itemProfessionalSkill.Name;
                            }

                            cnt = cnt + 1;
                        }

                        if (SkillName1 != null && SkillName1 != "")
                        {
                            S1 = SkillName1 + " :" + item.SkillDuration1 + " \n ";
                        }
                        if (SkillName2 != null && SkillName2 != "")
                        {
                            S2 = SkillName2 + " :" + item.SkillDuration2 + " \n ";
                        }
                        if (SkillName3 != null && SkillName3 != "")
                        {
                            S3 = SkillName3 + " :" + item.SkillDuration3 + " \n ";
                        }
                        if (SkillName4 != null && SkillName4 != "")
                        {
                            S4 = SkillName4 + " :" + item.SkillDuration4 + " \n ";
                        }
                        if (SkillName5 != null && SkillName5 != "")
                        {
                            S5 = SkillName5 + " :" + item.SkillDuration5 + " \n ";
                        }
                        item.ResultRemark = item.Classification.Value + " \n " + S1 + S2 + S3 + S4 + S5;
                    }
                    if (NewProfessionalTraining.IdStatus== 1487)
                    {
                        AddChangedEmployeeLogDetails();
                        NewProfessionalTraining.EmployeeProfessionalTrainingChangeLog = new List<EmployeeChangelog>(EmployeeChangeLogList);
                    }

                    // [001] Attachment start
                    if (NewProfessionalTraining.ProfessionalTrainingAttachmentList == null)
                    {
                        NewProfessionalTraining.ProfessionalTrainingAttachmentList = new List<ProfessionalTrainingAttachments>();
                    }
                    if (AttachmentsList != null)
                    {
                        foreach (ProfessionalTrainingAttachments temp in AttachmentsList)
                        {
                            temp.IdCreator = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                            temp.CreatedIn = GeosApplication.Instance.ServerDateTime;
                        }
                        NewProfessionalTraining.ProfessionalTrainingAttachmentList = AttachmentsList.ToList();
                    }
                    TrainingNewChangeLogList.Add(new TrainingChangeLog() { ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogAdd").ToString(), string.Format("{0} ", NewProfessionalTraining.Name)) });
                    NewProfessionalTraining.TrainingAllChangeLog = new List<TrainingChangeLog>(TrainingNewChangeLogList).ToList();
                    //NewProfessionalTraining.TrainingAllChangeLog = new List<TrainingChangeLog>(TrainingChangeLogList);
                    //NewProfessionalTraining = HrmService.AddProfessionalTraining(NewProfessionalTraining);

                    //NewProfessionalTraining = HrmService.AddProfessionalTraining_V2180(NewProfessionalTraining);
                    // NewProfessionalTraining = HrmService.AddProfessionalTraining_V2200(NewProfessionalTraining);

                    //NewProfessionalTraining = HrmService.AddProfessionalTraining_V2220(NewProfessionalTraining);

                    //NewProfessionalTraining = HrmService.AddProfessionalTraining_V2240(NewProfessionalTraining);

                    //[pramod.misal][GEOS2 - 5400][14.03.2024]
                    NewProfessionalTraining = HrmService.AddProfessionalTraining_V2500(NewProfessionalTraining);
                    //[001] End

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    IsSave = true;

                    RequestClose(null, null);
                    #endregion
                }
                else
                {
                    #region update
                    #region Information
                    // if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                    UpdateProfessionalTraining = new ProfessionalTraining();
                    UpdateProfessionalTraining.IdProfessionalTraining = IdProfessionalTraining;
                    UpdateProfessionalTraining.Code = SelectedProfessionalTrainingCode;
                    if(!string.IsNullOrEmpty(Name))
                        UpdateProfessionalTraining.Name = Name.Trim();
                    else
                        UpdateProfessionalTraining.Name = Name;

                    if(!string.IsNullOrEmpty(Description))
                        UpdateProfessionalTraining.Description = Description.Trim();
                    else
                        UpdateProfessionalTraining.Description = Description;

                    UpdateProfessionalTraining.ExpectedDate = ExpectedDate;
                   // UpdateProfessionalTraining.FinalizationDate = FinalizationDate;
                   if((uint)SelectedStatus.IdLookupValue== 1487)
                    {
                        if (FinalizationDate != null && FinalizationDate >= ExpectedDate)
                        {
                            UpdateProfessionalTraining.IdStatus = (uint)SelectedStatus.IdLookupValue;
                        }
                    }
                   else
                    {
                        UpdateProfessionalTraining.IdStatus = (uint)SelectedStatus.IdLookupValue;
                    }
                    UpdateProfessionalTraining.Duration = Duration;
                    UpdateProfessionalTraining.IdType = (uint)SelectedType.IdLookupValue;
                    UpdateProfessionalTraining.IdAcceptance = (uint)SelectedAcceptance.IdLookupValue;
                    UpdateProfessionalTraining.IdResponsible = (uint)SelectedResponsible.IdEmployee;

                    if (UpdateProfessionalTraining.IdAcceptance==1516)
                    {
                        UpdateProfessionalTraining.AcceptanceValue = "";
                    }
                    else
                    {
                        UpdateProfessionalTraining.AcceptanceValue = AcceptanceValue;
                    }
                    if ((uint?)SelectedTrainer.IdEmployee == 0)
                    {
                        UpdateProfessionalTraining.IdTrainer = null;
                    }
                    else
                    {
                        UpdateProfessionalTraining.IdTrainer = (uint?)SelectedTrainer.IdEmployee;
                    }
                    if(!string.IsNullOrEmpty(ExternalTrainer))
                        UpdateProfessionalTraining.ExternalTrainer = ExternalTrainer.Trim();
                    else
                        UpdateProfessionalTraining.ExternalTrainer = ExternalTrainer;

                    if(!string.IsNullOrEmpty(ExternalEntity))
                        UpdateProfessionalTraining.ExternalEntity = ExternalEntity.Trim();
                    else
                        UpdateProfessionalTraining.ExternalEntity = ExternalEntity;

                    if (FinalizationDate != null && FinalizationDate < ExpectedDate.Date)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingFinalizationDateWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                    if (FinalizationDate != null && ResultList.Count ==0)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingResultCountWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }

                    if (ResultList != null && ResultList.Count >0)
                    {
                        if (FinalizationDate != null)
                        {
                            foreach (var item in ResultList)
                            {
                                int cnt = 1;
                                foreach (ProfessionalSkill itemProfessionalSkill in SkillList)
                                {
                                    if (cnt == 1)
                                    {
                                        SkillName1 = itemProfessionalSkill.Name;
                                    }
                                    if (cnt == 2)
                                    {
                                        SkillName2 = itemProfessionalSkill.Name;
                                    }
                                    if (cnt == 3)
                                    {
                                        SkillName3 = itemProfessionalSkill.Name;
                                    }
                                    if (cnt == 4)
                                    {
                                        SkillName4 = itemProfessionalSkill.Name;
                                    }
                                    if (cnt == 5)
                                    {
                                        SkillName5 = itemProfessionalSkill.Name;
                                    }

                                    cnt = cnt + 1;
                                }

                                if (SkillName1 != null && SkillName1 != "")
                                {
                                    SkillResult1 = item.SkillDuration1;
                                }
                                if (SkillName2 != null && SkillName2 != "")
                                {
                                    SkillResult2 = item.SkillDuration2;
                                }
                                if (SkillName3 != null && SkillName3 != "")
                                {
                                    SkillResult3 = item.SkillDuration3;
                                }
                                if (SkillName4 != null && SkillName4 != "")
                                {
                                    SkillResult4 = item.SkillDuration4;
                                }
                                if (SkillName5 != null && SkillName5 != "")
                                {
                                    SkillResult5 = item.SkillDuration5;
                                }

                                if (SkillResult1 == null || SkillResult2 == null || SkillResult3 == null || SkillResult4 == null || SkillResult5 == null)
                                {
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingResultValueWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return;
                                }
                            }
                        }
                        if (!(SkillResult1 == null || SkillResult2 == null || SkillResult3 == null || SkillResult4 == null || SkillResult5 == null) && FinalizationDate >= ExpectedDate.Date)
                        {
                            UpdateProfessionalTraining.FinalizationDate = FinalizationDate;
                        }

                        if (!ResultList.Any(x => x.Results == 0) && UpdateProfessionalTraining.FinalizationDate == null)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingFinalizationDateMandatoryWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }
                        
                    }
                    
                    if (UpdateProfessionalTraining.FinalizationDate !=null)
                        UpdateProfessionalTraining.IdStatus = 1487;
                    else
                    {
                        if(UpdateProfessionalTraining.IdStatus == 1487)
                         UpdateProfessionalTraining.IdStatus = 1485;
                    }
                     
                    UpdateProfessionalTraining.RemarkForProfEdu = RemarkForProfEdu;
                    UpdateProfessionalTraining.ResultFileName = ResultFileName;
                    //Professional Skills
                    UpdateProfessionalTraining.ProfessionalSkillList = new List<ProfessionalSkill>();

                    // Delete ProductType link
                    foreach (ProfessionalSkill item in ObjProfessionalTrainingClone.ProfessionalSkillList)
                    {
                        if (SkillList != null && !SkillList.Any(x => x.IdProfessionalTrainingSkill == item.IdProfessionalTrainingSkill))
                        {
                            ProfessionalSkill professionalSkill = (ProfessionalSkill)item.Clone();
                            professionalSkill.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProfessionalTraining.ProfessionalSkillList.Add(professionalSkill);
                        }
                    }

                    //Insert
                    foreach (ProfessionalSkill item in SkillList)
                    {
                        if (!ObjProfessionalTrainingClone.ProfessionalSkillList.Any(x => x.IdProfessionalTrainingSkill == item.IdProfessionalTrainingSkill))
                        {
                            item.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            item.CreatedIn = GeosApplication.Instance.ServerDateTime;
                            ProfessionalSkill professionalSkill = (ProfessionalSkill)item.Clone();
                            professionalSkill.TransactionOperation = ModelBase.TransactionOperations.Add;
                            UpdateProfessionalTraining.ProfessionalSkillList.Add(professionalSkill);
                        }
                    }

                    //Update
                    foreach (ProfessionalSkill originalProfessionalSkill in ObjProfessionalTrainingClone.ProfessionalSkillList)
                    {
                        if (SkillList != null && SkillList.Any(x => x.IdProfessionalTrainingSkill == originalProfessionalSkill.IdProfessionalTrainingSkill))
                        {
                            ProfessionalSkill professionalSkillUpdated = SkillList.FirstOrDefault(x => x.IdProfessionalTrainingSkill == originalProfessionalSkill.IdProfessionalTrainingSkill);
                            if((professionalSkillUpdated.Duration != originalProfessionalSkill.Duration) || (professionalSkillUpdated.Code != originalProfessionalSkill.Code))
                            {
                                ProfessionalSkill professionalSkill = (ProfessionalSkill)professionalSkillUpdated.Clone();
                                professionalSkill.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                professionalSkill.ModifiedIn = GeosApplication.Instance.ServerDateTime;
                                professionalSkill.TransactionOperation = ModelBase.TransactionOperations.Update;
                                UpdateProfessionalTraining.ProfessionalSkillList.Add(professionalSkill);
                            }
                        }
                    }


                    //Trainees
                    // [001] Trainee start
                    UpdateProfessionalTraining.TraineeList = new List<Employee>();

                    // Delete ProductType link
                    foreach (Employee item in ObjProfessionalTrainingClone.TraineeList)
                    {
                        if (TraineesList != null && !TraineesList.Any(x => x.IdProfessionalTrainingTrainee == item.IdProfessionalTrainingTrainee))
                        {
                            Employee traineeList = (Employee)item.Clone();
                            traineeList.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProfessionalTraining.TraineeList.Add(traineeList);
                        }
                    }

                    //Insert
                    foreach (Employee item in TraineesList)
                    {
                        if (!ObjProfessionalTrainingClone.TraineeList.Any(x => x.IdProfessionalTrainingTrainee == item.IdProfessionalTrainingTrainee))
                        {
                            item.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            item.CreatedIn = GeosApplication.Instance.ServerDateTime;
                            Employee traineeList = (Employee)item.Clone();
                            traineeList.TransactionOperation = ModelBase.TransactionOperations.Add;
                            UpdateProfessionalTraining.TraineeList.Add(traineeList);
                        }
                    }
                    //Professional Results
                    UpdateProfessionalTraining.ProfessionalTrainingResultList = new List<ProfessionalTrainingResults>();
                    //Insert Result
                    foreach (ProfessionalTrainingResults item in ResultList)
                    {
                        if (!ObjProfessionalTrainingClone.ProfessionalTrainingResultList.Any(x => x.IdProfessionalTrainingResult == item.IdProfessionalTrainingResult))
                        {
                            item.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                            item.CreationDate = GeosApplication.Instance.ServerDateTime;
                            ProfessionalTrainingResults professionalResult = (ProfessionalTrainingResults)item.Clone();
                            professionalResult.TransactionOperation = ModelBase.TransactionOperations.Add;
                            UpdateProfessionalTraining.ProfessionalTrainingResultList.Add(professionalResult);
                        }
                    }
                    //Update Result
                    foreach (ProfessionalTrainingResults originalProfessionalResult in ObjProfessionalTrainingClone.ProfessionalTrainingResultList)
                    {
                        if (ResultList != null && ResultList.Any(x => x.IdProfessionalTrainingResult == originalProfessionalResult.IdProfessionalTrainingResult))
                        {
                            ProfessionalTrainingResults professionalResultUpdated = ResultList.FirstOrDefault(x => x.IdProfessionalTrainingResult == originalProfessionalResult.IdProfessionalTrainingResult);
                         
                            if ((professionalResultUpdated.ResultFileName != originalProfessionalResult.ResultFileName) || (professionalResultUpdated.SkillDuration1 != originalProfessionalResult.SkillDuration1) || (professionalResultUpdated.SkillDuration2 != originalProfessionalResult.SkillDuration2) || (professionalResultUpdated.SkillDuration3 != originalProfessionalResult.SkillDuration3) || (professionalResultUpdated.SkillDuration4 != originalProfessionalResult.SkillDuration4) || (professionalResultUpdated.SkillDuration5 != originalProfessionalResult.SkillDuration5))
                            {
                                ProfessionalTrainingResults professionalResult = (ProfessionalTrainingResults)professionalResultUpdated.Clone();
                                professionalResult.IdModifier = GeosApplication.Instance.ActiveUser.IdUser;
                                professionalResult.ModificationDate = GeosApplication.Instance.ServerDateTime;
                                professionalResult.TransactionOperation = ModelBase.TransactionOperations.Update;
                                UpdateProfessionalTraining.ProfessionalTrainingResultList.Add(professionalResult);
                            }
                        }
                    }
                    foreach (ProfessionalTrainingResults item in UpdateProfessionalTraining.ProfessionalTrainingResultList)
                    {
                        int cnt = 1;
                        foreach (ProfessionalSkill itemProfessionalSkill in UpdateProfessionalTraining.ProfessionalSkillList)
                        {
                            if (cnt == 1)
                            {
                                SkillName1 = itemProfessionalSkill.Name;
                            }
                            if (cnt == 2)
                            {
                                SkillName2 = itemProfessionalSkill.Name;
                            }
                            if (cnt == 3)
                            {
                                SkillName3 = itemProfessionalSkill.Name;
                            }
                            if (cnt == 4)
                            {
                                SkillName4 = itemProfessionalSkill.Name;
                            }
                            if (cnt == 5)
                            {
                                SkillName5 = itemProfessionalSkill.Name;
                            }

                            cnt = cnt + 1;
                        }

                        if (SkillName1 != null && SkillName1 != "")
                        {
                            S1 = SkillName1 + ":" + item.SkillDuration1 + "\n";
                        }
                        if (SkillName2 != null && SkillName2 != "")
                        {
                            S2 = SkillName2 + ":" + item.SkillDuration2 + "\n";
                        }
                        if (SkillName3 != null && SkillName3 != "")
                        {
                            S3 = SkillName3 + ":" + item.SkillDuration3 + "\n";
                        }
                        if (SkillName4 != null && SkillName4 != "")
                        {
                            S4 = SkillName4 + ":" + item.SkillDuration4 + "\n";
                        }
                        if (SkillName5 != null && SkillName5 != "")
                        {
                            S5 = SkillName5 + ":" + item.SkillDuration5 + "\n";
                        }
                        item.ResultRemark = "Classification:"+ item.Classification.Value + "\n" + S1 + S2 + S3 + S4 + S5;
                    }
                    if (UpdateProfessionalTraining.IdStatus ==1487)
                    {
                        AddChangedEmployeeLogDetails();
                        UpdateProfessionalTraining.EmployeeProfessionalTrainingChangeLog = new List<EmployeeChangelog>(EmployeeChangeLogList);
                    }

                    //Professional Training Attachments
                    UpdateProfessionalTraining.ProfessionalTrainingAttachmentList = new List<ProfessionalTrainingAttachments>();

                    // Delete Attachment
                    foreach (ProfessionalTrainingAttachments item in ObjProfessionalTrainingClone.ProfessionalTrainingAttachmentList)
                    {
                        if (AttachmentsList != null && !AttachmentsList.Any(x => x.IdProfessionalTrainingAttachment == item.IdProfessionalTrainingAttachment))
                        {
                            ProfessionalTrainingAttachments professionalTrainingAttachments = (ProfessionalTrainingAttachments)item.Clone();
                            professionalTrainingAttachments.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            UpdateProfessionalTraining.ProfessionalTrainingAttachmentList.Add(professionalTrainingAttachments);
                        }
                    }

                    //Insert Attachment
                    foreach (ProfessionalTrainingAttachments item in AttachmentsList)
                    {
                        if (!ObjProfessionalTrainingClone.ProfessionalTrainingAttachmentList.Any(x => x.IdProfessionalTrainingAttachment == item.IdProfessionalTrainingAttachment))
                        {
                            item.IdCreator = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                            item.CreatedIn = GeosApplication.Instance.ServerDateTime;
                            ProfessionalTrainingAttachments professionalTrainingAttachments = (ProfessionalTrainingAttachments)item.Clone();
                            professionalTrainingAttachments.TransactionOperation = ModelBase.TransactionOperations.Add;
                            UpdateProfessionalTraining.ProfessionalTrainingAttachmentList.Add(professionalTrainingAttachments);
                        }
                    }

                    //Update Attachment
                    foreach (ProfessionalTrainingAttachments originalProfessionalTrainingAttachments in ObjProfessionalTrainingClone.ProfessionalTrainingAttachmentList)
                    {
                        if (AttachmentsList != null && AttachmentsList.Any(x => x.IdProfessionalTrainingAttachment == originalProfessionalTrainingAttachments.IdProfessionalTrainingAttachment))
                        {
                            ProfessionalTrainingAttachments professionalTrainingAttachmentsUpdated = AttachmentsList.FirstOrDefault(x => x.IdProfessionalTrainingAttachment == originalProfessionalTrainingAttachments.IdProfessionalTrainingAttachment);
                           
                            if ((professionalTrainingAttachmentsUpdated.Description != originalProfessionalTrainingAttachments.Description) || (professionalTrainingAttachmentsUpdated.OriginalFileName != originalProfessionalTrainingAttachments.OriginalFileName) || (professionalTrainingAttachmentsUpdated.ProfTrainigAttachedDocInBytes != originalProfessionalTrainingAttachments.ProfTrainigAttachedDocInBytes) || (professionalTrainingAttachmentsUpdated.SavedFileName != originalProfessionalTrainingAttachments.SavedFileName))
                            {
                                ProfessionalTrainingAttachments professionalTrainingAttachments = (ProfessionalTrainingAttachments)professionalTrainingAttachmentsUpdated.Clone();
                                professionalTrainingAttachments.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                professionalTrainingAttachments.ModifiedIn = GeosApplication.Instance.ServerDateTime;
                                professionalTrainingAttachments.TransactionOperation = ModelBase.TransactionOperations.Update;
                                UpdateProfessionalTraining.ProfessionalTrainingAttachmentList.Add(professionalTrainingAttachments);
                            }

                        }
                    }
                    AddChangedTrainingLogDetails();
                    UpdateProfessionalTraining.TrainingAllChangeLog = new List<TrainingChangeLog>(TrainingChangeLogList);

                    // bool IsUpdate = HrmService.UpdateProfessionalTraining(UpdateProfessionalTraining);
                    //bool IsUpdate = HrmService.UpdateProfessionalTraining_V2180(UpdateProfessionalTraining);

                    //bool IsUpdate = HrmService.UpdateProfessionalTraining_V2190(UpdateProfessionalTraining);
                    //bool IsUpdate = HrmService.UpdateProfessionalTraining_V2200(UpdateProfessionalTraining);
                    //bool IsUpdate = HrmService.UpdateProfessionalTraining_V2220(UpdateProfessionalTraining);

                    //bool IsUpdate = HrmService.UpdateProfessionalTraining_V2240(UpdateProfessionalTraining);

                    //[pramod.misal][GEOS2 - 5400][14.03.2024]
                    //bool IsUpdate = HrmService.UpdateProfessionalTraining_V2500(UpdateProfessionalTraining);

                    bool IsUpdate = HrmService.UpdateProfessionalTraining_V2540(UpdateProfessionalTraining); //chitra.girigosavi [17/07/2024] GEOS2-5955 Registro de Capacitación 2

                    // [001] Trainee End

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    if (IsUpdate) { IsSave = true; }
                    else { IsSave = false; }
                    RequestClose(null, null);
                    #endregion

                    #region Skills

                    #endregion


                    #endregion
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptTrainingActionCommandAction() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptTrainingActionCommandAction() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptTrainingActionCommandAction() Method " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// [001][cpatil][28-03-2022][GEOS2-3567]HRM - Allow add future Job descriptions [#ERF97] - 6
        private void CustomShowFilterPopupForTrainee(FilterPopupEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopupForTrainee ...", category: Category.Info, priority: Priority.Low);

                if (e.Column.FieldName == "EmployeeDepartments")
                {
                    if (e.Column.FieldName != "EmployeeDepartments")
                    {
                        return;
                    }

                    try
                    {
                        List<object> filterItems = new List<object>();

                        if (e.Column.FieldName == "EmployeeDepartments")
                        {
                            filterItems.Add(new CustomComboBoxItem()
                            {
                                DisplayValue = "(Blanks)",
                                EditValue = CriteriaOperator.Parse("IsNull([EmployeeDepartments])")//[002] added
                            });

                            filterItems.Add(new CustomComboBoxItem()
                            {
                                DisplayValue = "(Non blanks)",
                                EditValue = CriteriaOperator.Parse("!IsNull([EmployeeDepartments])")
                            });

                            foreach (var dataObject in TraineesList)
                            {
                                if (dataObject.EmployeeDepartments == null)
                                {
                                    continue;
                                }
                                else if (dataObject.EmployeeDepartments != null)
                                {
                                    if (dataObject.EmployeeDepartments.Contains("\n"))
                                    {
                                        string tempDepartments = dataObject.EmployeeDepartments;
                                        for (int index = 0; index < tempDepartments.Length; index++)
                                        {
                                            string empDepartments = tempDepartments.Split('\n').First();

                                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empDepartments))
                                            {
                                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                                customComboBoxItem.DisplayValue = empDepartments;
                                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDepartments Like '%{0}%'", empDepartments));
                                                filterItems.Add(customComboBoxItem);
                                            }
                                            if (tempDepartments.Contains("\n"))
                                                tempDepartments = tempDepartments.Remove(0, empDepartments.Length + 1);
                                            else
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == TraineesList.Where(y => y.EmployeeDepartments == dataObject.EmployeeDepartments).Select(slt => slt.EmployeeDepartments).FirstOrDefault().Trim()))
                                        {
                                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                            customComboBoxItem.DisplayValue = TraineesList.Where(y => y.EmployeeDepartments == dataObject.EmployeeDepartments).Select(slt => slt.EmployeeDepartments).FirstOrDefault().Trim();
                                            customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDepartments Like '%{0}%'", TraineesList.Where(y => y.EmployeeDepartments == dataObject.EmployeeDepartments).Select(slt => slt.EmployeeDepartments).FirstOrDefault().Trim()));
                                            filterItems.Add(customComboBoxItem);
                                        }
                                    }
                                }
                            }
                        }
                        e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
                        GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopupForTrainee() executed successfully", category: Category.Info, priority: Priority.Low);

                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupForTrainee() method " + ex.Message, category: Category.Info, priority: Priority.Low);
                    }
                }

                else if (e.Column.FieldName == "Organization")
                {
                    List<object> filterItems = new List<object>();
                    try
                    {
                        filterItems.Add(new CustomComboBoxItem()
                        {
                            DisplayValue = "(Blanks)",
                            EditValue = CriteriaOperator.Parse("IsNull([Organization])")//[002] added
                        });

                        filterItems.Add(new CustomComboBoxItem()
                        {
                            DisplayValue = "(Non blanks)",
                            EditValue = CriteriaOperator.Parse("!IsNull([Organization])")
                        });

                        foreach (var dataObject in TraineesList)
                        {
                            if (dataObject.Organization == null)
                            {
                                continue;
                            }
                            else if (dataObject.Organization != null)
                            {
                                if (dataObject.Organization.Contains("\n"))
                                {
                                    string tempOrganization = dataObject.Organization;
                                    for (int index = 0; index < tempOrganization.Length; index++)
                                    {
                                        string empOrganization = tempOrganization.Split('\n').First();

                                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empOrganization))
                                        {
                                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                            customComboBoxItem.DisplayValue = empOrganization;
                                            customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Organization Like '%{0}%'", empOrganization));
                                            filterItems.Add(customComboBoxItem);
                                        }
                                        if (tempOrganization.Contains("\n"))
                                            tempOrganization = tempOrganization.Remove(0, empOrganization.Length + 1);
                                        else
                                            break;
                                    }
                                }
                                else
                                {
                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == TraineesList.Where(y => y.Organization == dataObject.Organization).Select(slt => slt.Organization).FirstOrDefault().Trim()))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = TraineesList.Where(y => y.Organization == dataObject.Organization).Select(slt => slt.Organization).FirstOrDefault().Trim();
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Organization Like '%{0}%'", TraineesList.Where(y => y.Organization == dataObject.Organization).Select(slt => slt.Organization).FirstOrDefault().Trim()));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                }
                            }
                        }

                        e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
                        GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopupForTrainee() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupForTrainee() method " + ex.Message, category: Category.Info, priority: Priority.Low);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopupForTrainee() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method CustomShowFilterPopupForTrainee()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //rajashri
        private void GenearateCertificateCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GenearateCertificateCommandAction()...", category: Category.Info, priority: Priority.Low);

                AddGenerateCertificateView addGenerateCertificateView = new AddGenerateCertificateView();
                AddGenerateCertificateViewModel addGenerateCertificateViewModel = new AddGenerateCertificateViewModel();
                EventHandler handle = delegate { addGenerateCertificateView.Close(); };
                addGenerateCertificateViewModel.RequestClose += handle;
                NewProfessionalTraining = new ProfessionalTraining();
                NewProfessionalTraining.TraineeList = TraineesList.ToList();
                NewProfessionalTraining.ProfessionalTrainingResultList = ResultList.ToList();
                NewProfessionalTraining.IdProfessionalTraining = TrainingDetail.IdProfessionalTraining;
                NewProfessionalTraining.ExternalEntity = TrainingDetail.ExternalEntity;
                NewProfessionalTraining.ExternalTrainer = TrainingDetail.ExternalTrainer;
                addGenerateCertificateViewModel.NewProfessionalTraining = NewProfessionalTraining;
              
                addGenerateCertificateViewModel.SkillList = SkillList;
                addGenerateCertificateView.DataContext = addGenerateCertificateViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addGenerateCertificateView.Owner = Window.GetWindow(ownerInfo);
                addGenerateCertificateView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method GenearateCertificateCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method GenearateCertificateCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddNewSkillCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewSkillCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (SkillList != null && SkillList.Count == 5)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingFiveSkillWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                TableView detailView = (TableView)obj;

                AddEditSkillsInTrainingView addEditSkillsInTrainingView = new AddEditSkillsInTrainingView();
                AddEditSkillsInTrainingViewModel addEditSkillsInTrainingViewModel = new AddEditSkillsInTrainingViewModel();
                EventHandler handle = delegate { addEditSkillsInTrainingView.Close(); };
                addEditSkillsInTrainingViewModel.RequestClose += handle;
                addEditSkillsInTrainingViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddNewSkills").ToString();
                addEditSkillsInTrainingViewModel.IsNew = true;
                addEditSkillsInTrainingViewModel.Init();

                addEditSkillsInTrainingView.DataContext = addEditSkillsInTrainingViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditSkillsInTrainingView.Owner = Window.GetWindow(ownerInfo);
                addEditSkillsInTrainingView.ShowDialog();

                if (addEditSkillsInTrainingViewModel.IsSave)
                {
                    if (SkillList == null)
                        SkillList = new ObservableCollection<ProfessionalSkill>();

                    if (SkillList.Count > 0)
                    {
                        if(SkillList.Any(y => y.IdProfessionalSkill == addEditSkillsInTrainingViewModel.SelectedSkill.IdProfessionalSkill))
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingRepeatedSkillWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }                           
                    }
                    SkillList.Add(addEditSkillsInTrainingViewModel.SelectedSkill);
                    CalculateDuration();
                    IsVisibleResultSkill1 = false;
                    IsVisibleResultSkill2 = false;
                    IsVisibleResultSkill3 = false;
                    IsVisibleResultSkill4 = false;
                    IsVisibleResultSkill5 = false;
                    int cnt = 1;
                    foreach (ProfessionalSkill itemProfessionalSkill in SkillList)
                    {
                        if (cnt == 1)
                        {
                           ResultSkill1 = itemProfessionalSkill.Code;
                           SkillName1 = itemProfessionalSkill.Name;
                           IsVisibleResultSkill1 = true;
                        }
                        else if (cnt == 2)
                        {
                            ResultSkill2 = itemProfessionalSkill.Code;
                            SkillName2 = itemProfessionalSkill.Name;
                           IsVisibleResultSkill2 = true;
                        }
                        else if (cnt == 3)
                        {
                           ResultSkill3 = itemProfessionalSkill.Code;
                           SkillName3 = itemProfessionalSkill.Name;
                           IsVisibleResultSkill3 = true;
                        }
                        else if (cnt == 4)
                        {
                            ResultSkill4 = itemProfessionalSkill.Code;
                            SkillName4 = itemProfessionalSkill.Name;
                            IsVisibleResultSkill4 = true;
                        }
                        else if (cnt == 5)
                        {
                            ResultSkill5 = itemProfessionalSkill.Code;
                           SkillName5 = itemProfessionalSkill.Name;
                           IsVisibleResultSkill5 = true;
                        }
                        cnt = cnt + 1;
                    }
                    foreach (ProfessionalTrainingResults result in ResultList)
                    {
                        float? Avg = 0;

                        if (IsVisibleResultSkill1)
                            Avg = Avg + result.SkillDuration1;

                        if (IsVisibleResultSkill2)
                            Avg = Avg + result.SkillDuration2;

                        if (IsVisibleResultSkill3)
                            Avg = Avg + result.SkillDuration3;

                        if (IsVisibleResultSkill4)
                            Avg = Avg + result.SkillDuration4;

                        if (IsVisibleResultSkill5)
                            Avg = Avg + result.SkillDuration5;

                        //result.Results = Avg / cnt;

                        decimal decimalValue = Math.Round((decimal)(Avg / cnt), 2);
                        result.Results = (float?)decimalValue;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddNewSkillCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewSkillCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditSkillCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditSkillCommandAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;

                ProfessionalSkill SelectedGridRow = (ProfessionalSkill)detailView.DataControl.CurrentItem;
                //SelectedGridRow.IdProfessionalTrainingSkill = Convert.ToInt32(IdProfessionalTraining);
                AddEditSkillsInTrainingView addEditSkillsInTrainingView = new AddEditSkillsInTrainingView();
                AddEditSkillsInTrainingViewModel addEditSkillsInTrainingViewModel = new AddEditSkillsInTrainingViewModel();
                EventHandler handle = delegate { addEditSkillsInTrainingView.Close(); };
                addEditSkillsInTrainingViewModel.RequestClose += handle;
                addEditSkillsInTrainingViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditSkills").ToString();
                addEditSkillsInTrainingViewModel.IsNew = false;
                addEditSkillsInTrainingViewModel.EditInit(SelectedGridRow);

                addEditSkillsInTrainingView.DataContext = addEditSkillsInTrainingViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditSkillsInTrainingView.Owner = Window.GetWindow(ownerInfo);
                addEditSkillsInTrainingView.ShowDialog();

                if (addEditSkillsInTrainingViewModel.IsSave)
                {
                    if ((SelectedSkill.IdProfessionalSkill!= addEditSkillsInTrainingViewModel.SelectedSkill.IdProfessionalSkill) && (SkillList.Any(y => y.IdProfessionalSkill == addEditSkillsInTrainingViewModel.SelectedSkill.IdProfessionalSkill)))
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingRepeatedSkillWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                    SelectedSkill.IdProfessionalTrainingSkill = addEditSkillsInTrainingViewModel.SelectedSkill.IdProfessionalTrainingSkill;
                    SelectedSkill.Code = addEditSkillsInTrainingViewModel.SelectedSkill.Code;
                    SelectedSkill.Name = addEditSkillsInTrainingViewModel.SelectedSkill.Name;
                    SelectedSkill.Description = addEditSkillsInTrainingViewModel.SelectedSkill.Description;
                    SelectedSkill.Type = addEditSkillsInTrainingViewModel.SelectedSkill.Type;
                    SelectedSkill.IdProfessionalSkill = addEditSkillsInTrainingViewModel.SelectedSkill.IdProfessionalSkill;

                    if (addEditSkillsInTrainingViewModel.SelectedSkill.Duration != 0)
                    {
                        SelectedSkill.Duration = addEditSkillsInTrainingViewModel.SelectedSkill.Duration;
                    }
                    else
                    {
                        SelectedSkill.Duration = 0;
                    }
                    IsVisibleResultSkill1 = false;
                    IsVisibleResultSkill2 = false;
                    IsVisibleResultSkill3 = false;
                    IsVisibleResultSkill4 = false;
                    IsVisibleResultSkill5 = false;
                    int cnt = 1;
                    foreach (ProfessionalSkill itemProfessionalSkill in SkillList)
                    {
                        if (cnt == 1)
                        {
                            ResultSkill1 = itemProfessionalSkill.Code;
                            SkillName1 = itemProfessionalSkill.Name;
                            IsVisibleResultSkill1 = true;
                        }
                        else if (cnt == 2)
                        {
                            ResultSkill2 = itemProfessionalSkill.Code;
                            SkillName2 = itemProfessionalSkill.Name;
                            IsVisibleResultSkill2 = true;
                        }
                        else if (cnt == 3)
                        {
                            ResultSkill3 = itemProfessionalSkill.Code;
                            SkillName3 = itemProfessionalSkill.Name;
                            IsVisibleResultSkill3 = true;
                        }
                        else if (cnt == 4)
                        {
                            ResultSkill4 = itemProfessionalSkill.Code;
                            SkillName4 = itemProfessionalSkill.Name;
                            IsVisibleResultSkill4 = true;
                        }
                        else if (cnt == 5)
                        {
                            ResultSkill5 = itemProfessionalSkill.Code;
                            SkillName5 = itemProfessionalSkill.Name;
                            IsVisibleResultSkill5 = true;
                        }
                        cnt = cnt + 1;
                    }
                    foreach (ProfessionalTrainingResults result in ResultList)
                    {
                        float? Avg = 0;

                        if (IsVisibleResultSkill1)
                            Avg = Avg + result.SkillDuration1;

                        if (IsVisibleResultSkill2)
                            Avg = Avg + result.SkillDuration2;

                        if (IsVisibleResultSkill3)
                            Avg = Avg + result.SkillDuration3;

                        if (IsVisibleResultSkill4)
                            Avg = Avg + result.SkillDuration4;

                        if (IsVisibleResultSkill5)
                            Avg = Avg + result.SkillDuration5;

                        decimal decimalValue = Math.Round((decimal)(Avg / cnt), 2);
                        result.Results = (float?)decimalValue;
                    }

                    CalculateDuration();
                }

                GeosApplication.Instance.Logger.Log("Method EditSkillCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditSkillCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                ExpectedDate = DateTime.Now;
                //FinalizationDate = DateTime.Now;

                FillProfessionalTrainingCode();
                FillStatusList();
                SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == 1485);
                FillTypeList();
                SelectedType = TypeList.FirstOrDefault(x => x.IdLookupValue == 1489);
                FillAcceptanceList();
                SelectedAcceptance = AcceptanceList.FirstOrDefault();
                FillResponsibleList();
                SelectedResponsible = ResponsibleList.FirstOrDefault();
                FillTrainerList();
                SelectedTrainer = TrainerList.FirstOrDefault();
                AddFiles();
                ClassificationList = new List<LookupValue>();
                ClassificationList.AddRange(CrmService.GetLookupValues(89));
                TrainingAllChangeLogList = new ObservableCollection<TrainingChangeLog>();
                TrainingNewChangeLogList = new ObservableCollection<TrainingChangeLog>();
                TrainingExistDetail = new ProfessionalTraining();
                TrainingExistDetail = (ProfessionalTraining)TrainingDetail.Clone();
                


                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method Init()...." + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method Init()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// [001][cpatil][24-03-2022][GEOS2-3637]HRM - Allow add future Job descriptions [#ERF97] - 6
        public void EditInit(ProfessionalTraining selectedSkill)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit..."), category: Category.Info, priority: Priority.Low);
                #region Service Comments
                //ProfessionalTraining obj = HrmService.GetProfessionalTrainingDetailsById(selectedSkill.IdProfessionalTraining);
                //ProfessionalTraining obj = HrmService.GetProfessionalTrainingDetailsById_V2180(selectedSkill.IdProfessionalTraining);
                //ProfessionalTraining obj = HrmService.GetProfessionalTrainingDetailsById_V2190(selectedSkill.IdProfessionalTraining);
                //  ProfessionalTraining obj = HrmService.GetProfessionalTrainingDetailsById_V2210(selectedSkill.IdProfessionalTraining);
                //Service GetProfessionalTrainingDetailsById_V2250 changed with GetProfessionalTrainingDetailsById_V2390 [rdixit][GEOS2-4476][22.05.2023]
                #endregion
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    //[001]
                    ProfessionalTraining obj = HrmService.GetProfessionalTrainingDetailsById_V2390(selectedSkill.IdProfessionalTraining, plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission);
                    TrainingDetail = obj;

                    //TrainingExistDetail = new ProfessionalTraining();
                    // TrainingExistDetail = (ProfessionalTraining)obj.Clone();

                    ObjProfessionalTrainingClone = (ProfessionalTraining)obj.Clone();

                    if (ObjProfessionalTrainingClone.ProfessionalSkillList == null)
                    {
                        ObjProfessionalTrainingClone.ProfessionalSkillList = new List<ProfessionalSkill>();
                    }
                    SelectedBoxIndex = 0;
                    SelectedProfessionalTrainingCode = obj.Code;
                    Name = obj.Name;
                    Description = obj.Description;
                    ExpectedDate = Convert.ToDateTime(obj.ExpectedDate);
                    if (obj.FinalizationDate == null)
                        FinalizationDate = null;
                    else
                        FinalizationDate = Convert.ToDateTime(obj.FinalizationDate);

                    Duration = obj.Duration;
                    ResultSkill1 = obj.ResultSkill1;
                    ResultSkill2 = obj.ResultSkill2;
                    ResultSkill3 = obj.ResultSkill3;
                    ResultSkill4 = obj.ResultSkill4;
                    ResultSkill5 = obj.ResultSkill5;

                    IsVisibleResultSkill1 = obj.VisibleResultSkill1;
                    IsVisibleResultSkill2 = obj.VisibleResultSkill2;
                    IsVisibleResultSkill3 = obj.VisibleResultSkill3;
                    IsVisibleResultSkill4 = obj.VisibleResultSkill4;
                    IsVisibleResultSkill5 = obj.VisibleResultSkill5;

                    SkillName1 = obj.SkillName1;
                    SkillName2 = obj.SkillName2;
                    SkillName3 = obj.SkillName3;
                    SkillName4 = obj.SkillName4;
                    SkillName5 = obj.SkillName5;

                    IdProfessionalTraining = obj.IdProfessionalTraining;
                    FillStatusList();
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == obj.IdStatus);
                    FillTypeList();
                    SelectedType = TypeList.FirstOrDefault(x => x.IdLookupValue == obj.IdType);

                    FillAcceptanceList();
                    SelectedAcceptance = AcceptanceList.FirstOrDefault(x => x.IdLookupValue == obj.IdAcceptance);

                    FillResponsibleList();
                    if (ResponsibleList != null)
                    {
                        if (!ResponsibleList.Any(x => x.IdEmployee == obj.Responsible.IdEmployee))
                        {
                            ResponsibleList.Add(obj.Responsible);
                            obj.Responsible.IsNoLongerResponsible = false;
                        }
                    }
                    SelectedResponsible = ResponsibleList.FirstOrDefault(x => x.IdEmployee == obj.IdResponsible);

                    FillTrainerList();
                    if (obj.IdTrainer == null)
                    {
                        SelectedTrainer = TrainerList.FirstOrDefault();
                    }
                    else
                    {
                        if (TrainerList != null)
                        {
                            SelectedTrainer = HrmService.GetNoLongerTrainer(obj.Trainer.IdEmployee);
                            if (!(TrainerList.Any(x => x.IdEmployee == SelectedTrainer.IdEmployee)))
                            {
                                Employee emp = new Employee();
                                emp.IdEmployee = SelectedTrainer.IdEmployee;
                                emp.FirstName = SelectedTrainer.FirstName;
                                emp.LastName = SelectedTrainer.LastName;
                                emp.FullName = SelectedTrainer.FullName;
                                emp.IsnolongerTrainer = false;
                                emp.IsTrainer = 0;
                                TrainerList.Add(emp);
                            }
                        }
                        SelectedTrainer = TrainerList.FirstOrDefault(x => x.IdEmployee == obj.IdTrainer);
                    }

                    AcceptanceValue = obj.AcceptanceValue;
                    ExternalEntity = Convert.ToString(obj.ExternalEntity);
                    ExternalTrainer = Convert.ToString(obj.ExternalTrainer);

                    if (obj.ProfessionalSkillList != null)
                    {
                        SkillList = new ObservableCollection<ProfessionalSkill>(obj.ProfessionalSkillList.ToList());
                    }
                    else
                    {
                        SkillList = new ObservableCollection<ProfessionalSkill>();
                    }

                    if (obj.TraineeList != null)
                    {
                        TraineesList = new ObservableCollection<Employee>(obj.TraineeList.ToList());
                        for (int i = 0; i < TraineesList.Count; i++)
                        {
                            TraineesList[i].SrNo = i + 1;
                        }
                    }
                    else
                    {
                        TraineesList = new ObservableCollection<Employee>();
                    }
                    if (obj.ProfessionalTrainingResultList != null)
                    {
                        ResultList = new ObservableCollection<ProfessionalTrainingResults>(obj.ProfessionalTrainingResultList.ToList());
                        foreach (ProfessionalTrainingResults item in ResultList)
                        {
                            ResultFileName = item.ResultFileName;
                        }
                        for (int i = 0; i < ResultList.Count; i++)
                        {

                            ResultList[i].SrNo = i + 1;
                            if (string.IsNullOrEmpty(AcceptanceValue))
                                AcceptanceValue = "0.01";
                            if (ResultList[i].Results >= Convert.ToDouble(AcceptanceValue))
                                ResultList[i].IdClassification = 1573;
                            else
                                ResultList[i].IdClassification = 1574;

                        }
                    }
                    else
                    {
                        ResultList = new ObservableCollection<ProfessionalTrainingResults>();
                    }
                    foreach (ProfessionalTrainingResults item in TrainingDetail.ProfessionalTrainingResultList)
                    {
                        int cnt = 1;
                        foreach (ProfessionalSkill itemProfessionalSkill in TrainingDetail.ProfessionalSkillList)
                        {
                            if (cnt == 1)
                            {
                                SkillName1 = itemProfessionalSkill.Name;
                            }
                            if (cnt == 2)
                            {
                                SkillName2 = itemProfessionalSkill.Name;
                            }
                            if (cnt == 3)
                            {
                                SkillName3 = itemProfessionalSkill.Name;
                            }
                            if (cnt == 4)
                            {
                                SkillName4 = itemProfessionalSkill.Name;
                            }
                            if (cnt == 5)
                            {
                                SkillName5 = itemProfessionalSkill.Name;
                            }

                            cnt = cnt + 1;
                        }

                        if (SkillName1 != null && SkillName1 != "")
                        {
                            S1 = SkillName1 + " :" + item.SkillDuration1 + " \n ";
                        }
                        if (SkillName2 != null && SkillName2 != "")
                        {
                            S2 = SkillName2 + " :" + item.SkillDuration2 + " \n ";
                        }
                        if (SkillName3 != null && SkillName3 != "")
                        {
                            S3 = SkillName3 + " :" + item.SkillDuration3 + " \n ";
                        }
                        if (SkillName4 != null && SkillName4 != "")
                        {
                            S4 = SkillName4 + " :" + item.SkillDuration4 + " \n ";
                        }
                        if (SkillName5 != null && SkillName5 != "")
                        {
                            S5 = SkillName5 + " :" + item.SkillDuration5 + " \n ";
                        }
                        item.ResultRemark = item.Classification.Value + " \n " + S1 + S2 + S3 + S4 + S5;
                    }
                    if (obj.ProfessionalTrainingAttachmentList != null)
                    {
                        AttachmentsList = new ObservableCollection<ProfessionalTrainingAttachments>(obj.ProfessionalTrainingAttachmentList.ToList());
                        if (AttachmentsList.Count > 0)
                            SelectedProfTrainingFile = AttachmentsList.FirstOrDefault();
                    }
                    else
                    {
                        AttachmentsList = new ObservableCollection<ProfessionalTrainingAttachments>();
                    }
                    FourRecordsTrainingFilesList = new ObservableCollection<ProfessionalTrainingAttachments>(AttachmentsList.OrderBy(x => x.IdProfessionalTraining).Take(4).ToList());
                    TrainingChangeLogList = new ObservableCollection<TrainingChangeLog>();
                    TrainingAllChangeLogList = new ObservableCollection<TrainingChangeLog>(obj.TrainingAllChangeLog.OrderByDescending(x => x.IdTrainingChangeLog).ToList());
                    #region IsCertificateGenerationEnabled
                    //Shubham[skadam] GEOS2-4911 Download of the Training Report 14 02 2024
                    try
                    {
                        //obj.IsCertificateGenerationEnabled = false;
                        if (obj.IdStatus == 1485)
                        {
                            IsCertificateGenerationEnabled = false;
                        }
                        else
                        {
                            IsCertificateGenerationEnabled = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    }
                    #endregion
                    GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillProfessionalTrainingCode()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillProfessionalTrainingCode()...", category: Category.Info, priority: Priority.Low);

                SelectedProfessionalTrainingCode = HrmService.GetLatestProfessionalTrainingCode();

                GeosApplication.Instance.Logger.Log("Method FillProfessionalTrainingCode()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillProfessionalTrainingCode() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillProfessionalTrainingCode() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillProfessionalTrainingCode() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempStatusList = CrmService.GetLookupValues(72);
                StatusList = new List<LookupValue>();
                StatusList = new List<LookupValue>(tempStatusList);

                GeosApplication.Instance.Logger.Log("Method FillStatusList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTypeList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempTypeList = CrmService.GetLookupValues(73);
                TypeList = new List<LookupValue>();
                TypeList = new List<LookupValue>(tempTypeList);

                GeosApplication.Instance.Logger.Log("Method FillTypeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTypeList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTypeList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTypeList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillAcceptanceList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAcceptanceList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempAcceptanceList = CrmService.GetLookupValues(77);
                AcceptanceList = new List<LookupValue>();
                AcceptanceList = new List<LookupValue>(tempAcceptanceList);
                GeosApplication.Instance.Logger.Log("Method FillAcceptanceList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAcceptanceList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAcceptanceList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillAcceptanceList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][avpawar][GEOS2-3317][Sr No.2 HRM - Trainings 5 of 8 [#TRN05] ( Add Trainees)]
        /// </summary>
        private void FillResponsibleList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillResponsibleList()...", category: Category.Info, priority: Priority.Low);
                //IList<Employee> tempResponsibleList = HrmService.GetAllResponsibles();
                IList<Employee> tempResponsibleList = HrmService.GetAllResponsibles_V2200(); //[001][changed the service method]
                ResponsibleList = new List<Employee>();
                ResponsibleList.Insert(0, new Employee() { FirstName = "---" });
                ResponsibleList.AddRange(tempResponsibleList);
                ResponsibleList.ToList().ForEach(rs => rs.IsNoLongerResponsible = true);


                GeosApplication.Instance.Logger.Log("Method FillResponsibleList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillResponsibleList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [001][avpawar][GEOS2-3317][Sr No.2 HRM - Trainings 5 of 8 [#TRN05] ( Add Trainees)]
        /// </summary>
        private void FillTrainerList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTrainerList()...", category: Category.Info, priority: Priority.Low);

                //IList<Employee> temptrainerList = HrmService.GetAllTrainers();
                IList<Employee> temptrainerList = HrmService.GetAllTrainers_V2200(); ////[001][changed the service method]
                TrainerList = new List<Employee>();
                TrainerList.Insert(0, new Employee() { FirstName = "---" });
                TrainerList.AddRange(temptrainerList);
                TrainerList.ToList().ForEach(esl => esl.IsnolongerTrainer = true);

                GeosApplication.Instance.Logger.Log("Method FillTrainerList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTrainerList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTrainerList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTrainerList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void DeleteSkillCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteSkillCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (ObjProfessionalTrainingClone == null)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ProfessionalTrainingDeleteSkillMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    ProfessionalSkill professionalSkill = (ProfessionalSkill)obj;
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        SkillList.Remove(SelectedSkill);
                        SkillList = new ObservableCollection<ProfessionalSkill>(SkillList);
                        SelectedSkill = SkillList.FirstOrDefault();
                        CalculateDuration();
                        IsVisibleResultSkill1 = false;
                        IsVisibleResultSkill2 = false;
                        IsVisibleResultSkill3 = false;
                        IsVisibleResultSkill4 = false;
                        IsVisibleResultSkill5 = false;
                        int cnt = 1;
                        foreach (ProfessionalSkill itemProfessionalSkill in SkillList)
                        {
                            if (cnt == 1)
                            {
                                ResultSkill1 = itemProfessionalSkill.Code;
                                SkillName1 = itemProfessionalSkill.Name;
                                IsVisibleResultSkill1 = true;
                            }
                            else if (cnt == 2)
                            {
                                ResultSkill2 = itemProfessionalSkill.Code;
                                SkillName2 = itemProfessionalSkill.Name;
                                IsVisibleResultSkill2 = true;
                            }
                            else if (cnt == 3)
                            {
                                ResultSkill3 = itemProfessionalSkill.Code;
                                SkillName3 = itemProfessionalSkill.Name;
                                IsVisibleResultSkill3 = true;
                            }
                            else if (cnt == 4)
                            {
                                ResultSkill4 = itemProfessionalSkill.Code;
                                SkillName4 = itemProfessionalSkill.Name;
                                IsVisibleResultSkill4 = true;
                            }
                            else if (cnt == 5)
                            {
                                ResultSkill5 = itemProfessionalSkill.Code;
                                SkillName5 = itemProfessionalSkill.Name;
                                IsVisibleResultSkill5 = true;
                            }
                            cnt = cnt + 1;
                        }
                        foreach (ProfessionalTrainingResults result in ResultList)
                        {
                            float? Avg = 0;

                            if (IsVisibleResultSkill1)
                                Avg = Avg + result.SkillDuration1;

                            if (IsVisibleResultSkill2)
                                Avg = Avg + result.SkillDuration2;

                            if (IsVisibleResultSkill3)
                                Avg = Avg + result.SkillDuration3;

                            if (IsVisibleResultSkill4)
                                Avg = Avg + result.SkillDuration4;

                            if (IsVisibleResultSkill5)
                                Avg = Avg + result.SkillDuration5;

                            decimal decimalValue = Math.Round((decimal)(Avg / cnt), 2);
                            result.Results = (float?)decimalValue;
                        }
                    }
                }
                else if (ObjProfessionalTrainingClone!=null && ObjProfessionalTrainingClone.IdStatus == 1485)
                {

                   
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ProfessionalTrainingDeleteSkillMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    ProfessionalSkill professionalSkill = (ProfessionalSkill)obj;
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        SkillList.Remove(SelectedSkill);
                        SkillList = new ObservableCollection<ProfessionalSkill>(SkillList);
                        SelectedSkill = SkillList.FirstOrDefault();
                        CalculateDuration();
                        IsVisibleResultSkill1 = false;
                        IsVisibleResultSkill2 = false;
                        IsVisibleResultSkill3 = false;
                        IsVisibleResultSkill4 = false;
                        IsVisibleResultSkill5 = false;
                        int cnt = 1;
                        foreach (ProfessionalSkill itemProfessionalSkill in SkillList)
                        {
                            if (cnt == 1)
                            {
                                ResultSkill1 = itemProfessionalSkill.Code;
                                SkillName1 = itemProfessionalSkill.Name;
                                IsVisibleResultSkill1 = true;
                            }
                            else if (cnt == 2)
                            {
                                ResultSkill2 = itemProfessionalSkill.Code;
                                SkillName2 = itemProfessionalSkill.Name;
                                IsVisibleResultSkill2 = true;
                            }
                            else if (cnt == 3)
                            {
                                ResultSkill3 = itemProfessionalSkill.Code;
                                SkillName3 = itemProfessionalSkill.Name;
                                IsVisibleResultSkill3 = true;
                            }
                            else if (cnt == 4)
                            {
                                ResultSkill4 = itemProfessionalSkill.Code;
                                SkillName4 = itemProfessionalSkill.Name;
                                IsVisibleResultSkill4 = true;
                            }
                            else if (cnt == 5)
                            {
                                ResultSkill5 = itemProfessionalSkill.Code;
                                SkillName5 = itemProfessionalSkill.Name;
                                IsVisibleResultSkill5 = true;
                            }
                            cnt = cnt + 1;
                        }
                       
                       foreach(ProfessionalTrainingResults result in ResultList)
                        {
                            float? Avg = 0;

                            if (IsVisibleResultSkill1)
                                Avg = Avg + result.SkillDuration1;
                           
                                if (IsVisibleResultSkill2)
                                    Avg = Avg + result.SkillDuration2;
                           
                                if (IsVisibleResultSkill3)
                                    Avg = Avg + result.SkillDuration3;
                           
                                if (IsVisibleResultSkill4)
                                    Avg = Avg + result.SkillDuration4;
                           
                                if (IsVisibleResultSkill5)
                                    Avg = Avg + result.SkillDuration5;

                            decimal decimalValue = Math.Round((decimal)(Avg / cnt), 2);
                            result.Results = (float?)decimalValue;
                        }


                    }
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingSkillDeleteWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                GeosApplication.Instance.Logger.Log("Method DeleteSkillCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteSkillCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void CalculateDuration()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateDuration()...", category: Category.Info, priority: Priority.Low);
                if (SkillList != null && SkillList.Count > 0)
                    Duration = float.Parse(SkillList.Select(a=>a.Duration).Sum().ToString());
                GeosApplication.Instance.Logger.Log("Method CalculateDuration()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CalculateDuration()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void AddNewTraineesCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewTraineesCommandAction()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;

                AddEditTraineeView addEditTraineeView = new AddEditTraineeView();
                AddEditTraineeViewModel addEditTraineeViewModel = new AddEditTraineeViewModel();

                EventHandler handle = delegate { addEditTraineeView.Close(); };
                addEditTraineeViewModel.RequestClose += handle;
                addEditTraineeViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddTrainee").ToString();
                addEditTraineeView.DataContext = addEditTraineeViewModel;
                addEditTraineeViewModel.IsNew = true;
                addEditTraineeViewModel.Init();

                var ownerInfo = (obj as FrameworkElement);
                addEditTraineeView.Owner = Window.GetWindow(ownerInfo);
                addEditTraineeView.ShowDialog();

                if(addEditTraineeViewModel.IsSave == true)
                {
                    if(TraineesList == null)
                    {
                        TraineesList = new ObservableCollection<Employee>();
                    }

                    if (TraineesList.Count > 0)
                    {
                        foreach(Employee temp in addEditTraineeViewModel.TraineesListForMainGrid)
                        {
                            for (int i = 0; i < TraineesList.Count; i++)
                            {
                                if(TraineesList[i].IdEmployee == temp.IdEmployee)
                                {
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingRepeatedTraineeWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return;
                                }
                            }
                        }
                    }

                    TraineesList.AddRange(addEditTraineeViewModel.TraineesListForMainGrid).ToList();
                    for (int i = 0; i < TraineesList.Count; i++)
                    {
                        TraineesList[i].SrNo = i + 1;
                    }

                    if(TraineesList.Count > 20)
                    {
                        MessageBoxResult MessageBoxResult1 = CustomMessageBox.Show(Application.Current.Resources["ProfessionalTrainingTraineeCountInformationMessage"].ToString(), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                    }
                    addEditTraineeViewModel.IsSave = false;
                }



                GeosApplication.Instance.Logger.Log("Method AddNewTraineesCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewTraineesCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }



        private void DeleteTraineesCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteTraineesCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (ObjProfessionalTrainingClone == null)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ProfessionalTrainingDeleteTraineeMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    Employee trainee = (Employee)obj;
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        TraineesList.Remove(SelectedTrainees);
                        TraineesList = new ObservableCollection<Employee>(TraineesList);
                        SelectedTrainees = TraineesList.FirstOrDefault();
                        if (TraineesList.Count > 0)
                        {
                            for (int i = 0; i < TraineesList.Count; i++)
                            {
                                TraineesList[i].SrNo = i + 1;
                            }
                        }
                    }
                }

                else if (ObjProfessionalTrainingClone != null && ObjProfessionalTrainingClone.IdStatus == 1485)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ProfessionalTrainingDeleteTraineeMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    Employee trainee = (Employee)obj;
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        TraineesList.Remove(SelectedTrainees);
                        TraineesList = new ObservableCollection<Employee>(TraineesList);
                        SelectedTrainees = TraineesList.FirstOrDefault();
                        if (TraineesList.Count > 0)
                        {
                            for (int i = 0; i < TraineesList.Count; i++)
                            {
                                TraineesList[i].SrNo = i + 1;
                            }
                        }
                    }
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingSkillDeleteWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                GeosApplication.Instance.Logger.Log("Method DeleteTraineesCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteTraineesCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }



        //private void CustomUnboundColumnDataAction(object e) //GridColumnDataEventArgs
        //{
        //    //try
        //    //{
        //    //    //GridControl gc = new GridControl();
        //    //    // GridColumn colCounter = gridView1.Columns.AddVisible("RowHandle");

        //    //    if (e == null) return;

        //    //    //var nd = e.ListSourceRowIndex;
        //    //    //var rh = nd.RowHandle;

        //    //    GridColumnDataEventArgs obj = e as GridColumnDataEventArgs;
        //    //    //GridControl gc = new GridControl();         //(GridControl) e as GridColumnDataEventArgs
        //    //    if (obj.Column.Name == "SrNo")              //Name == "SalesOwner")
        //    //    {
        //    //        //obj.Value = gc.GetRowHandleByListIndex( obj.ListSourceRowIndex + 1);
        //    //        obj.Value = obj.ListSourceRowIndex + 1;//view.GetRowHandle(e.ListSourceRowIndex) + 1;

        //    //    }
        //    //}
        //    //catch(Exception ex)
        //    //{

        //    //}  
        //}

        private void AddNewResultsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewResultsCommandAction()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;

                AddEditResultsView addeditresultsview = new AddEditResultsView();
                AddEditResultsViewModel addeditresultsviewmodel = new AddEditResultsViewModel();

                EventHandler handle = delegate { addeditresultsview.Close(); };
                addeditresultsviewmodel.RequestClose += handle;
                addeditresultsviewmodel.WindowHeader = System.Windows.Application.Current.FindResource("AddNewResults").ToString();
                addeditresultsview.DataContext = addeditresultsviewmodel;
                addeditresultsviewmodel.IsNew = true;
                addeditresultsviewmodel.Init(IdProfessionalTraining, AcceptanceValue,SkillList,TraineesList);

                var ownerInfo = (obj as FrameworkElement);
                addeditresultsview.Owner = Window.GetWindow(ownerInfo);
                addeditresultsview.ShowDialog();

                if (addeditresultsviewmodel.IsSave)
                {
                    if (ResultList == null)
                        ResultList = new ObservableCollection<ProfessionalTrainingResults>();

                    if (ResultList.Count > 0)
                    {

                        //if (ResultList.Any(y => y.IdEmployee == addeditresultsviewmodel.NewProfessionalResult.IdEmployee))
                        //{
                        //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingRepeatedSkillWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        //    return;
                        //}

                        foreach (ProfessionalTrainingResults temp in addeditresultsviewmodel.NewResultList)
                        {
                            for (int i = 0; i < ResultList.Count; i++)
                            {
                                if (ResultList[i].IdEmployee == temp.IdEmployee)
                                {
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProfessionalTrainingRepeatedTraineeResultWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return;
                                }
                            }
                        }
                    }
                    ResultList.AddRange(addeditresultsviewmodel.NewResultList);
                    for (int i = 0; i < ResultList.Count; i++)
                    {
                        ResultList[i].SrNo = i + 1;
                    }
                    selectedTraineeResultRow = addeditresultsviewmodel.NewProfessionalResult;
                }

                GeosApplication.Instance.Logger.Log("Method AddNewResultsCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewResultsCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditResultCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditResultCommandAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                string ExistFileName = null;
                ProfessionalTrainingResults SelectedGridRow = (ProfessionalTrainingResults)detailView.DataControl.CurrentItem;
                if (SelectedGridRow != null)
                {
                    SelectedTraineeResultRow = SelectedGridRow;

                    AddEditResultsView addEditResultsView = new AddEditResultsView();
                    AddEditResultsViewModel addEditResultsViewModel = new AddEditResultsViewModel();
                    EventHandler handle = delegate { addEditResultsView.Close(); };
                    addEditResultsViewModel.RequestClose += handle;
                    addEditResultsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditResults").ToString();
                    addEditResultsViewModel.IsNew = false;
                    addEditResultsViewModel.EditInit(SelectedGridRow, SkillList, AcceptanceValue, TraineesList);

                    addEditResultsView.DataContext = addEditResultsViewModel;
                    var ownerInfo = (obj as FrameworkElement);
                    addEditResultsView.Owner = Window.GetWindow(ownerInfo);
                    addEditResultsView.ShowDialog();


                    if (addEditResultsViewModel.IsSave == true)
                    {
                        if (ResultList.Count != 0)
                        {
                            ProfessionalTrainingResults existTraineeResult = ResultList.Where(x => x.EmployeeCode == SelectedGridRow.EmployeeCode).FirstOrDefault();
                            ResultList.Remove(existTraineeResult);
                        }

                       
                        ProfessionalTrainingResults professionalTrainingResultUpdated = new ProfessionalTrainingResults();
                        professionalTrainingResultUpdated = addEditResultsViewModel.UpdateResultList.FirstOrDefault();
                        ResultList.Add(professionalTrainingResultUpdated);
                        ResultList = new ObservableCollection<ProfessionalTrainingResults>( ResultList.OrderBy(o => o.SrNo).ToList());

                        for (int i = 0; i < ResultList.Count; i++)
                        {
                            ResultList[i].SrNo = i + 1;
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditResultCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditResultCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenTraineeResultDocument(object obj)
        {
            try
            {
               // AccordionControl a = (AccordionControl)obj;
                //Employee traineeDocument = (Employee)a.SelectedItem;

                GeosApplication.Instance.Logger.Log("Method OpenEmployeeEducationDocument()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                EmployeeDocumentView traineeResultDocumentView = new EmployeeDocumentView();
                EmployeeDocumentViewModel traineeResultDocumentViewModel = new EmployeeDocumentViewModel();
                traineeResultDocumentViewModel.OpenPdfFileTraineeResultByEmployeeCode(null, obj);
                traineeResultDocumentView.DataContext = traineeResultDocumentViewModel;
                traineeResultDocumentView.Show();
                GeosApplication.Instance.Logger.Log("Method OpenTraineeResultDocument()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenTraineeResultDocument() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                //employeeEducationQualification.PdfFilePath
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenTraineeResultDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                //CustomMessageBox.Show(string.Format("Could not find file '{0}'.", employeeEducationQualification.QualificationFileName), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenTraineeResultDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddChangedEmployeeLogDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddChangedEmployeeLogDetails()...", category: Category.Info, priority: Priority.Low);

                EmployeeChangeLogList = new ObservableCollection<EmployeeChangelog>();
                ProfessionalTraining prof_Training = UpdateProfessionalTraining;
                ProfessionalTraining New_prof_Training = NewProfessionalTraining;
                //Update Training
                if (IsNew == false)
                {
                    foreach (var item in UpdateProfessionalTraining.ProfessionalTrainingResultList)
                    {

                        //Training Type
                        if (TrainingDetail.Name != prof_Training.Name)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = item.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalTrainingName").ToString(), TrainingDetail.Name, prof_Training.Name) });
                        }
                        //ExpectedDate
                        if (TrainingDetail.ExpectedDate.Date != prof_Training.ExpectedDate.Date)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = item.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalTrainingExcepctedDate").ToString(), prof_Training.Name, TrainingDetail.ExpectedDate!=null? TrainingDetail.ExpectedDate.ToString("dd/MM/yyyy"):"None", prof_Training.ExpectedDate!=null? prof_Training.ExpectedDate.ToString("dd/MM/yyyy"):"None") });
                        }
                        //FinalizationDate
                        if (TrainingDetail.FinalizationDate != prof_Training.FinalizationDate)
                        {
                            if (TrainingDetail.FinalizationDate == null)
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = item.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalTrainingFinalizationDate").ToString(), prof_Training.Name, "None", prof_Training.FinalizationDate.Value.ToString("dd/MM/yyyy")) });
                            else
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = item.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalTrainingFinalizationDate").ToString(), prof_Training.Name, TrainingDetail.FinalizationDate.Value.ToString("dd/MM/yyyy"), prof_Training.FinalizationDate!=null ? prof_Training.FinalizationDate.Value.ToString("dd/MM/yyyy"):"None") });
                        }
                        //Entity  

                        if (TrainingDetail.ExternalEntity != prof_Training.ExternalEntity)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = item.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalTrainingEntity").ToString(), prof_Training.Name, TrainingDetail.ExternalEntity, prof_Training.ExternalEntity) });
                        }

                        //Duration
                        if (TrainingDetail.Duration != prof_Training.Duration)
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = item.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalTrainingDuration").ToString(), prof_Training.Name, TrainingDetail.Duration, prof_Training.Duration) });
                        }

                        if (TrainingDetail.ProfessionalTrainingResultList.Any(ptfr => ptfr.IdEmployee == item.IdEmployee))
                        {
                            if (!string.IsNullOrEmpty(TrainingDetail.ProfessionalTrainingResultList.Where(ptfr => ptfr.IdEmployee == item.IdEmployee).FirstOrDefault().ResultRemark))
                            {
                                //Remark
                                if (TrainingDetail.ProfessionalTrainingResultList.Where(ptfr => ptfr.IdEmployee == item.IdEmployee).FirstOrDefault().ResultRemark != item.ResultRemark)
                                {
                                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = item.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalTrainingRemark").ToString(), prof_Training.Name, TrainingDetail.ProfessionalTrainingResultList.Where(ptfr => ptfr.IdEmployee == item.IdEmployee).FirstOrDefault().ResultRemark, item.ResultRemark) });
                                }
                            }
                            else
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = item.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalTrainingRemark").ToString(), prof_Training.Name, "None", item.ResultRemark) });
                            }
                        }
                        else if (!string.IsNullOrEmpty(item.ResultRemark))
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = item.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalTrainingRemark").ToString(), prof_Training.Name, "None", item.ResultRemark) });
                        }

                        if (TrainingDetail.ProfessionalTrainingResultList.Any(ptfr => ptfr.IdEmployee == item.IdEmployee))
                        {
                            if (!string.IsNullOrEmpty(TrainingDetail.ProfessionalTrainingResultList.Where(ptfr => ptfr.IdEmployee == item.IdEmployee).FirstOrDefault().ResultFileName))
                            {
                                //Remark
                                if (TrainingDetail.ProfessionalTrainingResultList.Where(ptfr => ptfr.IdEmployee == item.IdEmployee).FirstOrDefault().ResultFileName != item.ResultFileName)
                                {
                                    EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = item.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalTrainingFile").ToString(), prof_Training.Name, TrainingDetail.ProfessionalTrainingResultList.Where(ptfr => ptfr.IdEmployee == item.IdEmployee).FirstOrDefault().ResultFileName, item.ResultFileName) });
                                }
                            }
                            else
                            {
                                EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = item.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalTrainingFile").ToString(), prof_Training.Name, "None", item.ResultFileName) });
                            }
                        }
                        else if (!string.IsNullOrEmpty(item.ResultFileName))
                        {
                            EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = item.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalTrainingFile").ToString(), prof_Training.Name, "None", item.ResultFileName) });
                        }

                    }
                }
                //Training Created
                if (IsNew == true)
                {
                    foreach (var item1 in NewProfessionalTraining.ProfessionalTrainingResultList)
                    {
                        EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = item1.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeProfessionalTrainingAdd").ToString(), New_prof_Training.Name) });
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddChangedEmployeeLogDetails()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an Error in Method AddChangedEmployeeLogDetails()........" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //GEOS2-3502
        public void AddFiles()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFiles()...", category: Category.Info, priority: Priority.Low);

                AttachmentsList = new ObservableCollection<ProfessionalTrainingAttachments>();

                GeosApplication.Instance.Logger.Log("Method AddFiles()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddFiles() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFile()...", category: Category.Info, priority: Priority.Low);

                AddEditAttachmentView addEditAttachmentView = new AddEditAttachmentView();
                AddEditAttachmentViewModel addEditAttachmentViewModel = new AddEditAttachmentViewModel();
                EventHandler handle = delegate { addEditAttachmentView.Close(); };
                addEditAttachmentViewModel.RequestClose += handle;
                addEditAttachmentViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddFileHeader").ToString();
                addEditAttachmentViewModel.IsNew = true;
                addEditAttachmentView.DataContext = addEditAttachmentViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditAttachmentView.Owner = Window.GetWindow(ownerInfo);
                addEditAttachmentView.ShowDialog();

                if (addEditAttachmentViewModel.IsSave)
                {
                    AttachmentsList.Add(addEditAttachmentViewModel.SelectedProfTrainingFile);
                    SelectedProfTrainingFile = addEditAttachmentViewModel.SelectedProfTrainingFile;
                    FourRecordsTrainingFilesList = new ObservableCollection<ProfessionalTrainingAttachments>(AttachmentsList.OrderBy(x => x.IdProfessionalTraining).Take(4).ToList());
                }
                GeosApplication.Instance.Logger.Log("Method AddFile()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //GEOS2-3501
        private void EditFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditFile()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                ProfessionalTrainingAttachments professionalTrainingAttachments = (ProfessionalTrainingAttachments)detailView.DataControl.CurrentItem;
                AddEditAttachmentView addEditAttachmentView = new AddEditAttachmentView();
                AddEditAttachmentViewModel addEditAttachmentViewModel = new AddEditAttachmentViewModel();
                EventHandler handle = delegate { addEditAttachmentView.Close(); };
                addEditAttachmentViewModel.RequestClose += handle;
                addEditAttachmentViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditFileHeader").ToString();
                addEditAttachmentViewModel.IsNew = false;
                addEditAttachmentViewModel.EditInit(IdProfessionalTraining,professionalTrainingAttachments);
                addEditAttachmentView.DataContext = addEditAttachmentViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addEditAttachmentView.Owner = Window.GetWindow(ownerInfo);
                addEditAttachmentView.ShowDialog();

                if (addEditAttachmentViewModel.IsSave == true)
                {
                    SelectedProfTrainingFile.IdProfessionalTrainingAttachment = addEditAttachmentViewModel.IdProfessionalTrainingAttachment;
                    SelectedProfTrainingFile.IdProfessionalTraining = addEditAttachmentViewModel.IdProfessionalTraining;
                    SelectedProfTrainingFile.OriginalFileName = addEditAttachmentViewModel.FileName;
                    SelectedProfTrainingFile.Description = addEditAttachmentViewModel.Description;
                    SelectedProfTrainingFile.ProfTrainigAttachedDocInBytes = addEditAttachmentViewModel.FileInBytes;
                    SelectedProfTrainingFile.SavedFileName = addEditAttachmentViewModel.ProfTrainingSavedFileName;
                    SelectedProfTrainingFile.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                }
                GeosApplication.Instance.Logger.Log("Method EditFile()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //GEOS2-2848
        private void DeleteFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteFile()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteProfDocsMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                ProfessionalTrainingAttachments productTypeAttachedDoc = (ProfessionalTrainingAttachments)obj;
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    AttachmentsList.Remove(SelectedProfTrainingFile);
                    AttachmentsList = new ObservableCollection<ProfessionalTrainingAttachments>(AttachmentsList);
                    SelectedProfTrainingFile = AttachmentsList.FirstOrDefault();
                    FourRecordsTrainingFilesList = new ObservableCollection<ProfessionalTrainingAttachments>(AttachmentsList.OrderBy(x => x.IdProfessionalTraining).Take(4).ToList());
                }

                GeosApplication.Instance.Logger.Log("Method DeleteFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteFile()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OpenPDFDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPDFDocument()...", category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                EmployeeDocumentView trainingDocumentView = new EmployeeDocumentView();
                EmployeeDocumentViewModel TrainingDocumentViewModel = new EmployeeDocumentViewModel();
                TrainingDocumentViewModel.OpenPdf(SelectedProfTrainingFile, obj);
                //if (TrainingDocumentViewModel.IsPresent)
                //{
                    trainingDocumentView.DataContext = TrainingDocumentViewModel;
                    trainingDocumentView.Show();
               // }
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
        #endregion

        #region ChangeLog
        public void AddChangedTrainingLogDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddChangedTrainingLogDetails()...", category: Category.Info, priority: Priority.Low);
                ProfessionalTraining prof_Training = UpdateProfessionalTraining;
                ProfessionalTraining New_prof_Training = NewProfessionalTraining;
                //Update Training
                if (IsNew == false)
                {
                    //Name
                    if (TrainingDetail.Name != null && TrainingDetail.Name != prof_Training.Name)
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogName").ToString(), TrainingDetail.Name, prof_Training.Name.Trim()) });

                     //   TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = Convert.ToInt32(TrainingDetail.IdProfessionalTraining), ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogName").ToString(), TrainingDetail.Name, Name.Trim()) });
                    }
                    //Description
                    if (TrainingDetail.Description != null && TrainingDetail.Description != prof_Training.Description)
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogDescription").ToString(), TrainingDetail.Description, Description.Trim()) });
                    }
                    //Description null
                    if (TrainingDetail.Description == null && TrainingDetail.Description != prof_Training.Description)
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogDescription").ToString(),"None", Description.Trim()) });
                    }
                    //Description null
                    if (prof_Training.Description == null && TrainingDetail.Description !=prof_Training.Description)
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogDescription").ToString(), "None", Description.Trim()) });
                    }

                    //ExpectedDate
                    if (TrainingDetail.ExpectedDate != null && !TrainingDetail.ExpectedDate.Equals(prof_Training.ExpectedDate))
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining =TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogExcepctedDate").ToString(),TrainingDetail.ExpectedDate.ToString("dd/MM/yyyy"), prof_Training.ExpectedDate.ToString("dd/MM/yyyy")) });
                    }
                    //FinalizationDate
                    if (!TrainingDetail.FinalizationDate.Equals(prof_Training.FinalizationDate))
                    {
                        if (TrainingDetail.FinalizationDate == null)
                            TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining =TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogFinalizationDate").ToString(), "None", prof_Training.FinalizationDate.Value.ToString("dd/MM/yyyy")) });
                        else
                            TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogFinalizationDate").ToString(), TrainingDetail.FinalizationDate.Value.ToString("dd/MM/yyyy"), prof_Training.FinalizationDate.Value.ToString("dd/MM/yyyy")) });

                        //Status
                        if (prof_Training.IdStatus!= TrainingDetail.IdStatus && prof_Training.FinalizationDate !=null)
                        {
                            TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogStatus").ToString(), TrainingDetail.Status.Value, "Done") });
                        }
                    }
                    //Entity  

                    if (TrainingDetail.ExternalEntity != null && !TrainingDetail.ExternalEntity.Equals(ExternalEntity))
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining =TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogEntity").ToString(),TrainingDetail.ExternalEntity, prof_Training.ExternalEntity) });
                    }
                    
                    //Duration
                    if (TrainingDetail.Duration != prof_Training.Duration)
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining =TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogDuration").ToString(), TrainingDetail.Duration, prof_Training.Duration) });
                    }
                    if (TrainingDetail.AcceptanceValue != prof_Training.AcceptanceValue)
                    {
                        //AcceptanceValue
                        if ((TrainingDetail.AcceptanceValue == "" || TrainingDetail.AcceptanceValue == null) && TrainingDetail.AcceptanceValue != prof_Training.AcceptanceValue && (prof_Training.AcceptanceValue != "" || prof_Training.AcceptanceValue != null))
                        {
                            TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogAcceptanceValue").ToString(), "None", prof_Training.AcceptanceValue) });
                        }
                    }
                    //AcceptanceValue
                    if ((prof_Training.AcceptanceValue == "" || prof_Training.AcceptanceValue == null) && TrainingDetail.AcceptanceValue != prof_Training.AcceptanceValue && (TrainingDetail.AcceptanceValue != "" || TrainingDetail.AcceptanceValue != null))
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogAcceptanceValue").ToString(), TrainingDetail.AcceptanceValue, "None") });
                    }
                    //AcceptanceValue null
                    if ((prof_Training.AcceptanceValue != "" && prof_Training.AcceptanceValue != null) &&( TrainingDetail.AcceptanceValue !="" && TrainingDetail.AcceptanceValue != null) && TrainingDetail.AcceptanceValue != prof_Training.AcceptanceValue)
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogAcceptanceValue").ToString(), TrainingDetail.AcceptanceValue, prof_Training.AcceptanceValue) });
                    }
                    ////AcceptanceValue null
                    //if (prof_Training.AcceptanceValue != "" && TrainingDetail.AcceptanceValue == "")
                    //{
                    //    TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogAcceptanceValue").ToString(),"None", prof_Training.AcceptanceValue) });
                    //}
                    //ExternalTrainer
                    if ((TrainingDetail.ExternalTrainer !=null && TrainingDetail.ExternalTrainer != "") && (prof_Training.ExternalTrainer != null && prof_Training.ExternalTrainer != "") && !TrainingDetail.ExternalTrainer.Equals(prof_Training.ExternalTrainer))
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogExternalTrainer").ToString(),  TrainingDetail.ExternalTrainer, prof_Training.ExternalTrainer) });
                    }
                    //ExternalTrainer null
                    if ((prof_Training.ExternalTrainer == null || prof_Training.ExternalTrainer == "") && !TrainingDetail.ExternalTrainer.Equals(prof_Training.ExternalTrainer))
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogExternalTrainer").ToString(), TrainingDetail.ExternalTrainer, "None") });
                    }
                    //ExternalTrainer null
                    if ((TrainingDetail.ExternalTrainer == null || TrainingDetail.ExternalTrainer == "") && !TrainingDetail.ExternalTrainer.Equals(prof_Training.ExternalTrainer))
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogExternalTrainer").ToString(), "None", prof_Training.ExternalTrainer) });
                    }
                    //Acceptance
                    if (TrainingDetail.IdAcceptance !=prof_Training.IdAcceptance && SelectedAcceptance.IdLookupValue !=-1)
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogAcceptance").ToString(), TrainingDetail.Acceptance.Value, SelectedAcceptance.Value) });
                    }

                    //Status
                    if (SelectedStatus.IdLookupValue != TrainingDetail.IdStatus && SelectedStatus.IdLookupValue != -1 && !(TrainingDetail.IdStatus==1487 && TrainingDetail.FinalizationDate!=null))
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogStatus").ToString(), TrainingDetail.Status.Value, SelectedStatus.Value) });
                    }
                    //Type
                    if (TrainingDetail.IdType != prof_Training.IdType && SelectedType.IdLookupValue !=-1)
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogType").ToString(), TrainingDetail.Type.Value, SelectedType.Value) });
                    }
                    //Responsible
                    if (TrainingDetail.IdResponsible != prof_Training.IdResponsible && SelectedResponsible.IdEmployee != TrainingDetail.IdResponsible)
                    {
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogResponsible").ToString(), TrainingDetail.Responsible.FullName, SelectedResponsible.FullName) });
                    }
                    ////Responsible Null
                    //if (TrainingDetail.IdResponsible != prof_Training.IdResponsible && SelectedResponsible.IdEmployee != TrainingDetail.IdResponsible)
                    //{
                    //    TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogResponsible").ToString(), TrainingDetail.Responsible.FullName, SelectedResponsible.FullName) });
                    //}

                    if (TrainingDetail.IdTrainer != null && prof_Training.IdTrainer != null && TrainingDetail.IdTrainer != prof_Training.IdTrainer && SelectedTrainer.IdEmployee != -1)
                    {
                        Employee Old_Trainer = TrainerList.FirstOrDefault(x => x.IdEmployee == TrainingDetail.IdTrainer);
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogTrainer").ToString(), Old_Trainer.FullName, SelectedTrainer.FullName) });
                        //  EmployeeChangeLogList.Add(new EmployeeChangelog() { IdEmployee = TrainingDetail.IdEmployee, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("EmployeeChangeLogNationality").ToString(), Old_Nationality.Name, NationalityList[SelectedNationalityIndex].Name) });
                    }
                    ////Trainer
                    //if (TrainingDetail.IdTrainer != null && prof_Training.IdTrainer != null  && TrainingDetail.IdTrainer != prof_Training.IdTrainer && SelectedTrainer.IdEmployee !=-1)
                    //{

                    //    TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogTrainer").ToString(), TrainingDetail.IdTrainer, SelectedTrainer.FullName) });
                    //}
                    //Trainer null
                    if ((prof_Training.IdTrainer == null || prof_Training.IdTrainer == 0) && SelectedTrainer.IdEmployee != -1 && prof_Training.IdTrainer != TrainingDetail.IdTrainer)
                    {
                        Employee Old_Trainer = TrainerList.FirstOrDefault(x => x.IdEmployee == TrainingDetail.IdTrainer);
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogTrainer").ToString(), Old_Trainer.FullName, "None") });
                    }
                    //Trainer null
                    if ((TrainingDetail.IdTrainer == null || TrainingDetail.IdTrainer == 0) && SelectedTrainer.IdEmployee != -1 && prof_Training.IdTrainer != TrainingDetail.IdTrainer)
                    {
                        Employee Old_Trainer = TrainerList.FirstOrDefault(x => x.IdEmployee == TrainingDetail.IdTrainer);
                        TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogTrainer").ToString(), "None", SelectedTrainer.FullName) });
                    }

                    //Professional Training Skill


                    if (prof_Training.ProfessionalSkillList != ObjProfessionalTrainingClone.ProfessionalSkillList)
                    {
                        if (UpdateProfessionalTraining.ProfessionalSkillList != null)
                        {
                            foreach (ProfessionalSkill professionalSkill in UpdateProfessionalTraining.ProfessionalSkillList)
                            {
                                if (professionalSkill.TransactionOperation == ModelBase.TransactionOperations.Update)
                                {
                                    if (SkillList != null && SkillList.Any(x => x.IdProfessionalTrainingSkill == professionalSkill.IdProfessionalTrainingSkill))
                                    {
                                        ProfessionalSkill professionalSkillUpdated = ObjProfessionalTrainingClone.ProfessionalSkillList.FirstOrDefault(x => x.IdProfessionalTrainingSkill == professionalSkill.IdProfessionalTrainingSkill);
                                        if ((professionalSkillUpdated.Duration != professionalSkill.Duration) || (professionalSkillUpdated.Code != professionalSkill.Code))
                                        {
                                            //Name
                                            if (professionalSkillUpdated.Code != professionalSkill.Code)
                                            {
                                                TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogSkillName").ToString(), professionalSkillUpdated.Name, professionalSkill.Name) });
                                            }
                                            //Duration
                                            if (professionalSkillUpdated.Duration != professionalSkill.Duration)
                                            {
                                                TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogSkillDuration").ToString(), professionalSkill.Name, professionalSkillUpdated.Duration, professionalSkill.Duration) });
                                            }
                                        }
                                    }
                                }
                                //professionalSkill  Deleted
                                if (professionalSkill.TransactionOperation == ModelBase.TransactionOperations.Delete)
                                {
                                    TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogSkillDeleted").ToString(), professionalSkill.Name) });
                                }
                                //professionalSkill  Created
                                if (professionalSkill.TransactionOperation == ModelBase.TransactionOperations.Add)
                                {
                                    TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogSkillAdd").ToString(), professionalSkill.Name) });
                                }
                            }
                        }
                    }

                    //Professional Training Trainee

                    if (prof_Training.TraineeList != TrainingDetail.TraineeList)
                    {
                        foreach (Employee professionalTrainee in UpdateProfessionalTraining.TraineeList)
                        {
                            //professionalTrainee  Deleted
                            if (professionalTrainee.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogTraineeDelete").ToString(), professionalTrainee.FirstName +" "+ professionalTrainee.LastName) });
                            }
                            //professionalTrainee  Created
                            if (professionalTrainee.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogTraineeAdd").ToString(), professionalTrainee.FirstName + " " + professionalTrainee.LastName) });
                            }
                        }
                    }

                    //Professional Training Result

                    if (prof_Training.ProfessionalTrainingResultList != TrainingDetail.ProfessionalTrainingResultList)
                    {
                        foreach (ProfessionalTrainingResults professionalTrainingResults in UpdateProfessionalTraining.ProfessionalTrainingResultList)
                        {
                            if (professionalTrainingResults.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                ProfessionalTrainingResults oldProfessionalTrainingResults = ObjProfessionalTrainingClone.ProfessionalTrainingResultList.FirstOrDefault(x => x.IdProfessionalTrainingResult == professionalTrainingResults.IdProfessionalTrainingResult);

                                //Duration 1
                                if (professionalTrainingResults.SkillDuration1!=null && oldProfessionalTrainingResults.SkillDuration1 != null && oldProfessionalTrainingResults.SkillDuration1 != professionalTrainingResults.SkillDuration1)
                                {
                                    TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogResultSkillDuration1").ToString(), TrainingDetail.SkillName1, oldProfessionalTrainingResults.SkillDuration1, professionalTrainingResults.SkillDuration1) });
                                }
                                //Duration 2
                                if (professionalTrainingResults.SkillDuration2 != null &&  oldProfessionalTrainingResults.SkillDuration2 != null && oldProfessionalTrainingResults.SkillDuration2 != professionalTrainingResults.SkillDuration2)
                                {
                                    TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogResultSkillDuration2").ToString(), TrainingDetail.SkillName2, oldProfessionalTrainingResults.SkillDuration2, professionalTrainingResults.SkillDuration2) });
                                }
                                //Duration 3
                                if (professionalTrainingResults.SkillDuration3 != null && oldProfessionalTrainingResults.SkillDuration3 != null && oldProfessionalTrainingResults.SkillDuration3 != professionalTrainingResults.SkillDuration3)
                                {
                                    TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogResultSkillDuration3").ToString(), TrainingDetail.SkillName3, oldProfessionalTrainingResults.SkillDuration3, professionalTrainingResults.SkillDuration3) });
                                }
                                //Duration 4
                                if (professionalTrainingResults.SkillDuration4 != null && oldProfessionalTrainingResults.SkillDuration4 != null &&   oldProfessionalTrainingResults.SkillDuration4 != professionalTrainingResults.SkillDuration4)
                                {
                                    TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogResultSkillDuration4").ToString(), TrainingDetail.SkillName4, oldProfessionalTrainingResults.SkillDuration4, professionalTrainingResults.SkillDuration4) });
                                }
                                //Duration 5
                                if (professionalTrainingResults.SkillDuration5 != null && oldProfessionalTrainingResults.SkillDuration5 != null && oldProfessionalTrainingResults.SkillDuration5 != professionalTrainingResults.SkillDuration5)
                                {
                                    TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogResultSkillDuration5").ToString(), TrainingDetail.SkillName5, oldProfessionalTrainingResults.SkillDuration5, professionalTrainingResults.SkillDuration5) });
                                }
                                
                                //File
                                if (oldProfessionalTrainingResults.TraineeDocumentFileInBytes != professionalTrainingResults.TraineeDocumentFileInBytes)
                                {
                                    TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogResultFileName").ToString(), oldProfessionalTrainingResults.ResultFileName, professionalTrainingResults.ResultFileName) });
                                }
                            }
                            
                            //professionalSkill  Created
                            if (professionalTrainingResults.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogResultAdd").ToString(), professionalTrainingResults.FirstName +" "+ professionalTrainingResults.LastName) });
                            }
                        }
                    }
                    //Professional Training Attachment

                    if (prof_Training.ProfessionalTrainingAttachmentList != TrainingDetail.ProfessionalTrainingAttachmentList)
                    {
                        foreach (ProfessionalTrainingAttachments professionalTrainingAttachments in UpdateProfessionalTraining.ProfessionalTrainingAttachmentList)
                        {
                            if (professionalTrainingAttachments.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                ProfessionalTrainingAttachments oldProfessionalTrainingAttachments = ObjProfessionalTrainingClone.ProfessionalTrainingAttachmentList.FirstOrDefault(x => x.IdProfessionalTrainingAttachment == professionalTrainingAttachments.IdProfessionalTrainingAttachment);

                                //Name
                                if (oldProfessionalTrainingAttachments.SavedFileName != professionalTrainingAttachments.SavedFileName)
                                {
                                    TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogAttachmentOriginalFileName").ToString(), oldProfessionalTrainingAttachments.SavedFileName, professionalTrainingAttachments.SavedFileName) });
                                }
                                //Description
                                if (oldProfessionalTrainingAttachments.Description != professionalTrainingAttachments.Description)
                                {
                                    TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogAttachmentDescription").ToString(), oldProfessionalTrainingAttachments.Description, professionalTrainingAttachments.Description) });
                                }
                                //Duration
                                if (oldProfessionalTrainingAttachments.ProfTrainigAttachedDocInBytes != professionalTrainingAttachments.ProfTrainigAttachedDocInBytes)
                                {
                                    TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogAttachmentFile").ToString(), oldProfessionalTrainingAttachments.SavedFileName, professionalTrainingAttachments.SavedFileName) });
                                }
                            }
                            //professionalTrainingAttachments  Delete
                            if (professionalTrainingAttachments.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogAttachmentDelete").ToString(), professionalTrainingAttachments.SavedFileName) });
                            }
                            //professionalTrainingAttachments  Created
                            if (professionalTrainingAttachments.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                TrainingChangeLogList.Add(new TrainingChangeLog() { IdProfessionalTraining = TrainingDetail.IdProfessionalTraining, ChangeLogDatetime = GeosApplication.Instance.ServerDateTime, ChangeLogIdUser = GeosApplication.Instance.ActiveUser.IdUser, ChangeLogChange = string.Format(System.Windows.Application.Current.FindResource("TrainingChangeLogAttachmentAdd").ToString(), professionalTrainingAttachments.SavedFileName) });
                            }
                        }
                    }
                }
               

                GeosApplication.Instance.Logger.Log("Method AddChangedTrainingLogDetails()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an Error in Method AddChangedTrainingLogDetails()........" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion ChangeLog

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
                       me[BindableBase.GetPropertyName(() => Name)] +
                       me[BindableBase.GetPropertyName(() => InformationError)] +
                       me[BindableBase.GetPropertyName(() => SelectedResponsible)] +
                       me[BindableBase.GetPropertyName(() => ExternalEntity)] +
                       me[BindableBase.GetPropertyName(() => ExternalTrainer)];


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


                string name = BindableBase.GetPropertyName(() => Name);
                string headerInformtionError = BindableBase.GetPropertyName(() => InformationError);
                string selectedResponsible = BindableBase.GetPropertyName(() => SelectedResponsible);
                string selectedEntity = BindableBase.GetPropertyName(() => ExternalEntity);
                string selectedExternalTrainer = BindableBase.GetPropertyName(() => ExternalTrainer);

                if (columnName == name)
                {
                    return AddEditTrainingValidation.GetErrorMessage(name, Name);
                }

                if (columnName == headerInformtionError)
                {
                    return AddEditTrainingValidation.GetErrorMessage(headerInformtionError, InformationError);
                }

                if (columnName == selectedResponsible)
                {
                    return AddEditTrainingValidation.GetErrorMessage(selectedResponsible, SelectedResponsible);
                }
                if (columnName == selectedEntity)
                {
                    if (IsEntityExtTraineeEnable)
                    {
                        return AddEditTrainingValidation.GetErrorMessage(selectedEntity, ExternalEntity);
                    }
                }
                if (columnName == selectedExternalTrainer)
                {
                    if (IsEntityExtTraineeEnable)
                    {
                        return AddEditTrainingValidation.GetErrorMessage(selectedExternalTrainer, ExternalTrainer);
                    }
                }

                return null;
            }
        }

        public string CheckInformationError(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
            }

            return error;
        }

        #endregion

    }
}
