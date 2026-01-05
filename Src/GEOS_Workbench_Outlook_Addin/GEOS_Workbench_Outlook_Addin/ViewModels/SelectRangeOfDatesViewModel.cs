using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class SelectRangeOfDatesViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region TaskLog
        //GEOS2-224 sprint-60 (#64346) Create recurrent activities only for visible rows [adadibathina]
        #endregion

        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Declaration
        private DateTime? startDate;
        private DateTime? endDate;
        private string windowHeader;
        private bool isNew;
        private bool isResult;
        private bool isSave;
        private string startDateErrorMessage = string.Empty;
        private string endDateErrorMessage = string.Empty;
        #endregion

        #region Public ICommand
        public ICommand SelectRangeOfDatesViewCancelButtonCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand SelectRangeOfDatesViewAcceptButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Properties    
        List<ActivitiesRecurrence> LstActivitiesRecurrence { get; set; }

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

        public bool IsResult
        {
            get
            {
                return isResult;
            }

            set
            {
                isResult = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsResult"));
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
        public SelectRangeOfDatesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SelectRangeOfDatesViewModel()...", category: Category.Info, priority: Priority.Low);
                SelectRangeOfDatesViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                SelectRangeOfDatesViewAcceptButtonCommand = new RelayCommand(new Action<object>(CreatePlannedActivityAction));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                GeosApplication.Instance.Logger.Log("Constructor SelectRangeOfDatesViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor SelectRangeOfDatesViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion



        #region Methods
        /// <summary>
        /// Method to create activity
        /// </summary>
        /// <param name="obj"></param>
        private void CreatePlannedActivityAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreatePlannedActivityAction()...", category: Category.Info, priority: Priority.Low);

                string error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
                if (error != null)
                {
                    return;
                }

                IsResult = CrmStartUp.CreateAutomaticPlannedActivity_V2035((DateTime)StartDate, (DateTime)EndDate, GeosApplication.Instance.ActiveUser.IdUser, LstActivitiesRecurrence);
                IsSave = true;
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivitySaveSuccessfull").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CreatePlannedActivityAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CreatePlannedActivityAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CreatePlannedActivityAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CreatePlannedActivityAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to close window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        public void Init(IList LstActivitiesRecurrence)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                WindowHeader = System.Windows.Application.Current.FindResource("SelectRangeOfDates").ToString();
                this.LstActivitiesRecurrence = LstActivitiesRecurrence.Cast<ActivitiesRecurrence>().ToList();
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// method to check validation On Date Edit Value Changing
        /// </summary>
        /// <param name="e"></param>
        public void OnDateEditValueChanging(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);
                if (StartDate != null && EndDate != null)
                {
                    if (StartDate > EndDate)
                    {
                        startDateErrorMessage = System.Windows.Application.Current.FindResource("SelectRangeOfDatesStartDateError").ToString();
                        endDateErrorMessage = System.Windows.Application.Current.FindResource("SelectRangeOfDatesEndDateError").ToString();
                    }
                    else
                    {
                        startDateErrorMessage = string.Empty;
                        endDateErrorMessage = string.Empty;
                    }
                }
                else
                {
                    startDateErrorMessage = string.Empty;
                    endDateErrorMessage = string.Empty;
                }


                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));

                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnDateEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    return;
                }
                CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

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
                string startdate = BindableBase.GetPropertyName(() => StartDate);
                string enddate = BindableBase.GetPropertyName(() => EndDate);

                if (columnName == startdate)
                {
                    if (!string.IsNullOrEmpty(startDateErrorMessage))
                    {
                        return startDateErrorMessage;
                    }
                    else
                    {
                        return EmployeeProfileValidation.GetErrorMessage(startdate, StartDate);
                    }
                }

                if (columnName == enddate)
                {
                    if (!string.IsNullOrEmpty(endDateErrorMessage))
                    {
                        return endDateErrorMessage;
                    }
                    else
                    {
                        return EmployeeProfileValidation.GetErrorMessage(enddate, EndDate);
                    }
                }

                return null;
            }
        }

        #endregion

    }
}
