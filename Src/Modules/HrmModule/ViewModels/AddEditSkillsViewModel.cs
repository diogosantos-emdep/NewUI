using DevExpress.Mvvm;
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
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddEditSkillsViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
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

        
        #endregion

        #region Public ICommands
        public ICommand AcceptFileActionCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        #endregion

        #region Constructor
        public AddEditSkillsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditSkillsViewModel ...", category: Category.Info, priority: Priority.Low);

                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 90;

                AcceptFileActionCommand = new DelegateCommand<object>(SkillsAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);

                GeosApplication.Instance.Logger.Log("Constructor AddEditSkillsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditSkillsViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                Code = HrmService.GetLatestProfessionalSkillCode();
                Name = string.Empty;
                Description = string.Empty;
                InUse = true;
                FillSkillLookupValueList();
                SelectedType = SkillsTypeList.FirstOrDefault();

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
            

        /// <summary>
        /// SkillsAction Method is used for Accept Button for both Add and Edit Skills
        /// </summary>
        /// <param name="obj"></param>
        private void SkillsAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SkillsAction()...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedType"));

                if (error != null)
                {
                    return;
                }

                char[] trimChars = { '\r', '\n' };
                Description = Description == null ? "" : Description;
                Name = name == null ? "" : Name;
                if (Description != null || Name != null)
                {
                    if (Description.Contains("\r\n") || Name.Contains("\r\n"))
                    {
                        Description = Description.TrimEnd(trimChars);
                        Description = Description.TrimStart(trimChars);
                        Name = name.TrimStart(trimChars);
                        Name = name.TrimEnd(trimChars);
                    }
                }

                #region Add New Skills
                if (IsNew)
                {
                    NewProfessionalSkill = new ProfessionalSkill();
                    NewProfessionalSkill.Code = Code;
                    NewProfessionalSkill.Name = Name;
                    NewProfessionalSkill.Description = Description;
                    //if (SelectedType.Value.Equals("---"))
                    //    NewProfessionalSkill.SkillType = null;
                    //else
                    NewProfessionalSkill.IdSkillType = SelectedType.IdLookupValue;// SkillsTypeList[selectedIndexSkillType].IdLookupValue;
                    //NewProfessionalSkill.SkillType = SkillsTypeList[SelectedIndexSkillType];
                    NewProfessionalSkill.InUse = InUse;
                    NewProfessionalSkill.CreatedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    NewProfessionalSkill.IdProfessionalSkill = Convert.ToInt32(HrmService.AddProfessionalSkill(NewProfessionalSkill));

                    if (NewProfessionalSkill.IdProfessionalSkill != null)
                        IsSave = true;
                    else
                        IsSave = false;

                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SkillAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }
                #endregion

                #region Edit Skills
                else
                {
                    EditProfessionalSkill = new ProfessionalSkill();
                    EditProfessionalSkill.Code = Code;
                    EditProfessionalSkill.Name = Name;
                    EditProfessionalSkill.Description = Description;
                    EditProfessionalSkill.InUse = InUse;
                    EditProfessionalSkill.IdSkillType = SelectedType.IdLookupValue; //SkillsTypeList[selectedIndexSkillType].IdLookupValue; //SelectedType.IdLookupValue;
                    //NewProfessionalSkill.SkillType = SkillsTypeList[SelectedIndexSkillType];
                    EditProfessionalSkill.IdProfessionalSkill = IdProfessionalSkills;
                    EditProfessionalSkill.ModifiedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);

                    bool result = HrmService.UpdateProfessionalSkill(EditProfessionalSkill);

                    if (result == true)
                        IsSave = true;
                    else
                        IsSave = false;

                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SkillUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
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
                //Type = Selectedskill.SkillType.Value;
                InUse = Selectedskill.InUse;
                FillSkillLookupValueList();
                SelectedType = SkillsTypeList.FirstOrDefault(x => x.IdLookupValue == Selectedskill.SkillType.IdLookupValue);
                //SelectedIndexSkillType = SkillsTypeList.FindIndex(x => x.IdLookupValue == Selectedskill.SkillType.IdLookupValue);

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
                    me[BindableBase.GetPropertyName(() => Name)] +
                    me[BindableBase.GetPropertyName(() => Description)]+
                    me[BindableBase.GetPropertyName(() => SelectedType)];


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
                string skillDescripton = BindableBase.GetPropertyName(() => Description);
                //string _selectedIndexSkillType = BindableBase.GetPropertyName(() => SelectedIndexSkillType);
                string _selectedType = BindableBase.GetPropertyName(() => SelectedType);

                if (columnName == skillName)
                {
                    return SkillValidation.GetErrorMessage(skillName, Name);
                }
                if (columnName == skillDescripton)
                {
                    return SkillValidation.GetErrorMessage(skillDescripton, Description);
                }
                if(columnName == _selectedType)
                {
                    return SkillValidation.GetErrorMessage(_selectedType, SelectedType);
                }

                return null;
            }
        }
        #endregion
    }
}
