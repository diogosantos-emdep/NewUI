using DevExpress.Mvvm;
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
    public class AddEditObjectivesViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
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

        #region Declarations
        private string code;
        private string description;
        private bool isNew;
        private bool isSave;
        private string error = string.Empty;
        private bool inUse;
        private JobDescription selectedJobDescription;
        private string windowHeader;
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

        public JobDescription SelectedJobDescription
        {
            get { return selectedJobDescription; }
            set
            {
                selectedJobDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedJobDescription"));
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

        public ProfessionalObjective NewProfessionalObjective { get; set; }
        public ProfessionalObjective EditProfessionalObjective { get; set; }
        
        public ulong IdProfessionalObjective;
        public List<JobDescription> JDCodeList { get; set; }


        #endregion

        #region Public ICommands
        public ICommand AcceptObjectiveActionCommand { get; set; }
        public ICommand CancelObjectiveButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        #endregion

        #region Constructor
        public AddEditObjectivesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditObjectivesViewModel ...", category: Category.Info, priority: Priority.Low);

                AcceptObjectiveActionCommand = new DelegateCommand<object>(ObjectivesAction);
                CancelObjectiveButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);

                GeosApplication.Instance.Logger.Log("Constructor AddEditObjectivesViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditObjectivesViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method for Intialize....
        /// </summary>
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                Code = HrmService.GetLatestProfessionalObjectiveCode();
                Description = string.Empty;
                FillJobDescriptionList();
                SelectedJobDescription = JDCodeList.FirstOrDefault();
                InUse = true;
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
        /// ObjectivesAction Method is used for Accept Button for both Add and Edit Objectives
        /// </summary>
        /// <param name="obj"></param>
        private void ObjectivesAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ObjectivesAction()...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedJobDescription"));

                if (error != null)
                {
                    return;
                }

                char[] trimChars = { '\r', '\n' };
                Description = Description == null ? "" : Description;
                if (Description != null)
                {
                    if (Description.Contains("\r\n"))
                    {
                        Description = Description.TrimEnd(trimChars);
                        Description = Description.TrimStart(trimChars);
                    }
                }

                #region Add New Skills
                if (IsNew)
                {
                    NewProfessionalObjective = new ProfessionalObjective();
                    NewProfessionalObjective.Code = Code;
                    NewProfessionalObjective.Description = Description;
                    NewProfessionalObjective.IdJobDescription = SelectedJobDescription.IdJobDescription;
                    NewProfessionalObjective.InUse = InUse;
                    NewProfessionalObjective.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    NewProfessionalObjective = HrmService.AddProfessionalObjective(NewProfessionalObjective);

                    if (NewProfessionalObjective != null)
                        IsSave = true;
                    else
                        IsSave = false;

                    if (IsSave)
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ObjectiveAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    RequestClose(null, null);
                }
                #endregion

                #region Edit Skills
                else
                {
                    IsSave = false;

                    EditProfessionalObjective = new ProfessionalObjective();
                    EditProfessionalObjective.Code = Code;
                    EditProfessionalObjective.Description = Description;
                    EditProfessionalObjective.InUse = InUse;
                    EditProfessionalObjective.IdJobDescription = SelectedJobDescription.IdJobDescription;
                    EditProfessionalObjective.IdProfessionalObjective = IdProfessionalObjective;
                    EditProfessionalObjective.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                    IsSave = HrmService.UpdateProfessionalObjective(EditProfessionalObjective.IdProfessionalObjective, EditProfessionalObjective);

                    if(IsSave)
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ObjectiveUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }
                #endregion

                GeosApplication.Instance.Logger.Log("Method ObjectivesAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Error in Method ObjectivesAction()...." + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ObjectivesAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ObjectivesAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// CloseWindow Method is used for Cancel Button for Both Add and Edit Objectives
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
        /// EditInit Method is used for while Edit the Objectives
        /// </summary>
        public void EditInit(List<ProfessionalObjective> objectivesList, ProfessionalObjective Selectedobjective)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                IdProfessionalObjective = Selectedobjective.IdProfessionalObjective;
                Code = Selectedobjective.Code;
                Description = Selectedobjective.Description;
                InUse = Selectedobjective.InUse;
                FillJobDescriptionList();
                SelectedJobDescription = JDCodeList.FirstOrDefault(x => x.IdJobDescription == Selectedobjective.IdJobDescription);

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillJobDescriptionList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillJobDescriptionList()...", category: Category.Info, priority: Priority.Low);

                IList<JobDescription> tempCountryList = HrmService.GetAllJobDescriptions_V2046().OrderBy(a => a.JobDescriptionTitleAndCode).ToList();
                JDCodeList = new List<JobDescription>();
                JDCodeList.Insert(0, new JobDescription() { JobDescriptionTitleAndCode = "---" });
                JDCodeList.AddRange(tempCountryList);
                //List<string> abc = JDCodeList.Where(x => x.JobDescriptionInUse == 0).Select(x => x.JobDescriptionCode).ToList();

                GeosApplication.Instance.Logger.Log("Method FillJobDescriptionList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillJobDescriptionList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    me[BindableBase.GetPropertyName(() => Description)] +
                    me[BindableBase.GetPropertyName(() => SelectedJobDescription)];


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
                string objectiveDescripton = BindableBase.GetPropertyName(() => Description);
                string _selectedJDCode = BindableBase.GetPropertyName(() => SelectedJobDescription);

                if (columnName == objectiveDescripton)
                {
                    return ObjectiveValidation.GetErrorMessage(objectiveDescripton, Description);
                }
                if (columnName == _selectedJDCode)
                {
                    return ObjectiveValidation.GetErrorMessage(_selectedJDCode, SelectedJobDescription.JobDescriptionTitleAndCode);
                }

                return null;
            }
        }
        #endregion
    }
}
