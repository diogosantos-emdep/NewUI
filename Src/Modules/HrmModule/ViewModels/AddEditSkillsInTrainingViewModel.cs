using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddEditSkillsInTrainingViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        private string code;
        private string description;
        private bool isNew;
        private bool isSave;
        private string error = string.Empty;
        private string name;
        private bool inUse;
        private string windowHeader;
        public int IdProfessionalSkills;
        private List<ProfessionalSkill> profSkillsList { get; set; }
        private string type { get; set; }
        private List<LookupValue> skillsTypeList { get; set; }
        private LookupValue selectedType { get; set; }
        private double dialogHeight;
        private double dialogWidth;
        private ObservableCollection<ProfessionalSkill> addSkillsList;
        private ProfessionalSkill selectedAddSkills;
        private ObservableCollection<ProfessionalSkill> professionalSkillGridList;
        private ProfessionalSkill selectedSkill;
        private float? duration;
        private int idProfessionalSkill;
        private int idProfessionalTrainingSkill;

        #endregion

        #region Properties
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

        public bool InUse
        {
            get { return inUse; }
            set
            {
                inUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InUse"));
            }
        }

        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Type"));
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

        public List<ProfessionalSkill> ProfSkillsList
        {
            get
            {
                return profSkillsList;
            }
            set
            {
                profSkillsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfSkillsList"));
            }
        }
        public ProfessionalSkill NewProfessionalSkill { get; set; }
        public ProfessionalSkill EditProfessionalSkill { get; set; }
        public List<LookupValue> SkillsTypeList
        {
            get
            {
                return skillsTypeList;
            }
            set
            {
                skillsTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillsTypeList"));
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

        public ObservableCollection<ProfessionalSkill> AddSkillsList
        {
            get { return addSkillsList; }
            set
            {
                addSkillsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddSkillsList"));
            }
        }

        public ProfessionalSkill SelectedAddSkills
        {
            get { return selectedAddSkills; }
            set
            {
                selectedAddSkills = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAddSkills"));

                if (SelectedAddSkills != null)
                {
                    Name = SelectedAddSkills.Name;
                }
                else
                {
                    Name = string.Empty;
                }
                
            }
        }

        public ObservableCollection<ProfessionalSkill> ProfessionalSkillGridList
        {
            get { return professionalSkillGridList; }
            set
            {
                professionalSkillGridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfessionalSkillGridList"));
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

        public float? Duration
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

        public int IdProfessionalTrainingSkill
        {
            get
            {
                return idProfessionalTrainingSkill;
            }

            set
            {
                idProfessionalTrainingSkill = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdProfessionalTrainingSkill"));
            }
        }

        public int IdProfessionalTraining
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

        #endregion

        #region Public ICommands
        public ICommand AddEditSkillsAcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        #endregion

        #region Constructor
        public AddEditSkillsInTrainingViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditSkillsInTrainingViewModel ...", category: Category.Info, priority: Priority.Low);

                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 90;

                AddEditSkillsAcceptButtonCommand = new DelegateCommand<object>(AddEditSkillsAcceptButtonCommandAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                //FillAddSkillsList();
                GeosApplication.Instance.Logger.Log("Constructor AddEditSkillsInTrainingViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditSkillsInTrainingViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                FillAddSkillsList();
                Duration = new float();
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


        public void EditInit(ProfessionalSkill selectedGridRow)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                FillAddSkillsList();
                if(AddSkillsList != null)
                {
                    if (!AddSkillsList.Any(x=> x.IdProfessionalSkill == selectedGridRow.IdProfessionalSkill))
                    {
                        AddSkillsList.Add(selectedGridRow);
                        selectedGridRow.IsEnabled = false;
                    }
                }
                //if (selectedGridRow.InUse == false)
                //{
                //    AddSkillsList.Add(selectedGridRow);
                //}
                IdProfessionalTrainingSkill = selectedGridRow.IdProfessionalTrainingSkill;
                IdProfessionalTraining = selectedGridRow.IdProfessionalTraining;

                //if (selectedGridRow.InUse == false)
                //{
                //    SelectedAddSkills = selectedGridRow;
                //}
                //else
                //{
            //}
                SelectedAddSkills = AddSkillsList.FirstOrDefault(x => x.Code == selectedGridRow.Code);
                
                 //selectedGridRow.IdProfessionalSkill;
                Code = selectedGridRow.Code;
                Name = selectedGridRow.Name;
                Description = selectedGridRow.Description;
                Type = selectedGridRow.Type;

                if (selectedGridRow.Duration != 0)
                {
                    Duration = selectedGridRow.Duration;
                }
                else
                {
                    Duration = 0;
                }

                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        } 

        /// <summary>
        /// SkillsAction Method is used for Accept Button for both Add and Edit Skills
        /// </summary>
        /// <param name="obj"></param>
        private void AddEditSkillsAcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SkillsAction()...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                PropertyChanged(this, new PropertyChangedEventArgs("Duration"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedAddSkills"));

                if (error != null)
                {
                    return;
                }
                #region Add New Skills
                if (IsNew)
                {
                    SelectedSkill = new ProfessionalSkill();
                    SelectedSkill.IdProfessionalSkill = SelectedAddSkills.IdProfessionalSkill;
                    SelectedSkill.Code = SelectedAddSkills.Code;
                    SelectedSkill.Name = SelectedAddSkills.Name;
                    SelectedSkill.Description = SelectedAddSkills.Description;
                    SelectedSkill.IdSkillType = SelectedAddSkills.IdSkillType; //SelectedType.IdLookupValue;// SkillsTypeList[selectedIndexSkillType].IdLookupValue;
                    SelectedSkill.Type = SelectedAddSkills.Type;
                    SelectedSkill.InUse = SelectedAddSkills.InUse;
                    SelectedSkill.CreatedBy = 0;

                    if (Duration != 0)
                    {
                        SelectedSkill.Duration = Duration;
                    }
                    else
                    {
                        SelectedSkill.Duration = 0;
                    }

                    IsSave = true;
                    RequestClose(null, null);
                }
                #endregion

                #region Edit Skills
                else
                {
                    SelectedSkill = new ProfessionalSkill();
                    //SelectedSkill.IdProfessionalTrainingSkill = 
                    SelectedSkill.IdProfessionalSkill = SelectedAddSkills.IdProfessionalSkill;
                    SelectedSkill.IdProfessionalTrainingSkill = IdProfessionalTrainingSkill;
                    SelectedSkill.IdProfessionalTraining = IdProfessionalTraining;
                    SelectedSkill.Code = SelectedAddSkills.Code;
                    SelectedSkill.Name = SelectedAddSkills.Name;
                    SelectedSkill.Description = SelectedAddSkills.Description;
                    SelectedSkill.IdSkillType = SelectedAddSkills.IdSkillType;
                    SelectedSkill.Type = SelectedAddSkills.Type;
                    SelectedSkill.InUse = InUse;
                    SelectedSkill.CreatedBy = 0;

                    if (Duration != 0)
                    {
                        SelectedSkill.Duration = Duration;
                    }
                    else
                    {
                        SelectedSkill.Duration = 0;
                    }

                    IsSave = true;
                    RequestClose(null, null);
                }
                #endregion

                GeosApplication.Instance.Logger.Log("Method SkillsAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Error in Method SkillsAction()...." + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method SkillsAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SkillsAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// CloseWindow Method is used for Cancel Button for Both Add and Edit Skills
        /// </summary>
        /// <param name="obj"></param>
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
        /// EditInit Method is used for while Edit the Skills
        /// </summary>
        public void EditInit(List<ProfessionalSkill> Skillslist , ProfessionalSkill Selectedskill)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                IdProfessionalSkills = Selectedskill.IdProfessionalSkill;
                Code = Selectedskill.Code;
                Name = Selectedskill.Name;
                Description = Selectedskill.Description;
                InUse = Selectedskill.InUse;
                FillSkillLookupValueList();
                SelectedType = SkillsTypeList.FirstOrDefault(x => x.IdLookupValue == Selectedskill.SkillType.IdLookupValue);

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// FillSkillLookupValueList is used for Fill the Type List
        /// </summary>
        public void FillSkillLookupValueList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSkillLookupValueList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempList = CrmService.GetLookupValues(69);
                SkillsTypeList = new List<LookupValue>();
                SkillsTypeList = new List<LookupValue>(tempList);
                SkillsTypeList.Insert(0, new LookupValue() { Value = " ---", InUse = true });
                GeosApplication.Instance.Logger.Log("Method FillSkillLookupValueList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillSkillLookupValueList()...." + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillSkillLookupValueList()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillSkillLookupValueList()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }

        private void FillAddSkillsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAddSkillsList ...", category: Category.Info, priority: Priority.Low);
                if (AddSkillsList == null)
                {
                    AddSkillsList = new ObservableCollection<ProfessionalSkill>(HrmService.GetAllProfessionalSkillsForTraining());
                }
                GeosApplication.Instance.Logger.Log("Method FillAddSkillsList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillAddSkillsList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillAddSkillsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAddSkillsList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Validation
        bool allowValidation = false;
        private int idProfessionalTraining;

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
                    me[BindableBase.GetPropertyName(() => Name)] +
                    me[BindableBase.GetPropertyName(() => Duration)] +
                    me[BindableBase.GetPropertyName(() => SelectedAddSkills)];


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
                string skillName = BindableBase.GetPropertyName(() => Name);
                string skillDuration = BindableBase.GetPropertyName(() => Duration);
                string _selectedAddSkills = BindableBase.GetPropertyName(() => SelectedAddSkills);

                if (columnName == _selectedAddSkills)
                {
                    return SkillInTrainingValidation.GetErrorMessage(_selectedAddSkills, SelectedAddSkills);
                }

                if (columnName == skillName)
                {
                    return SkillInTrainingValidation.GetErrorMessage(skillName, Name);
                }
                if (columnName == skillDuration)
                {
                    return SkillInTrainingValidation.GetErrorMessage(skillDuration, Duration);
                }

                return null;
            }
        }
        #endregion
    }
}
