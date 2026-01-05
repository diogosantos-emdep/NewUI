using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
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
    public class EmployeeIdBadgeSelectionViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services       
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private string windowHeader;
        private bool isPrint;
        private List<JobDescription> employeeJobDescriptionList;
        private int selectedJobDescription;
        private string error = string.Empty;
        public JobDescription EmployeeJobDescription { get; set; }
        #endregion

        #region Public Icommands
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Properties
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
        public bool IsPrint
        {
            get
            {
                return isPrint;
            }

            set
            {
                isPrint = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        public List<JobDescription> EmployeeJobDescriptionList
        {
            get { return employeeJobDescriptionList; }
            set
            {
                employeeJobDescriptionList = value;
            }
        }
        public int SelectedJobDescription
        {
            get
            {
                return selectedJobDescription;
            }

            set
            {
                selectedJobDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedJobDescription"));
            }
        }

        #endregion

        #region Pubilc Event
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
        /// [001][cpatil][26-03-2020][GEOS2-1974] Add improvements in PrintIdCard [#POC21] (linked #POC20)
        #region Methods
        public void Init(Employee employeeDetail, long selectedPeriod)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("SelectjobTitle").ToString();

                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                // [001] Changed service method
                List<JobDescription> tempJobDescriptionList = HrmService.GetEmployeeLatestJobDescriptionsByIdEmployee_V2260(employeeDetail.IdEmployee, selectedPeriod);
                EmployeeJobDescriptionList = new List<JobDescription>();
                EmployeeJobDescriptionList.Insert(0, new JobDescription() { JobDescriptionTitleAndCode = "---" });
                EmployeeJobDescriptionList.AddRange(tempJobDescriptionList);
                // [001]
                if (tempJobDescriptionList.Any(i => i.EmployeeJobDescriptions[0].IsMainJobDescription == 1))
                {
                    SelectedJobDescription = EmployeeJobDescriptionList.IndexOf(tempJobDescriptionList.Where(tjd => tjd.EmployeeJobDescriptions[0].IsMainJobDescription == 1).FirstOrDefault());
                }
                else
                {
                    if (tempJobDescriptionList.Any(i => i.EmployeeJobDescriptions.Any(x => x.JobDescriptionUsage != tempJobDescriptionList[0].EmployeeJobDescriptions[0].JobDescriptionUsage)))
                    {
                        SelectedJobDescription = EmployeeJobDescriptionList.IndexOf(tempJobDescriptionList[0]);
                    }
                    else
                    {
                        var findFirstvalue = EmployeeJobDescriptionList.FirstOrDefault();

                        if (findFirstvalue != null)
                            SelectedJobDescription = EmployeeJobDescriptionList.IndexOf(findFirstvalue);
                    }
                }


                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedJobDescription"));
                if (error != null)
                {
                    return;
                }
                isPrint = true;
                EmployeeJobDescription = new JobDescription() { JobDescriptionTitle = EmployeeJobDescriptionList[SelectedJobDescription].JobDescriptionTitle, Department = EmployeeJobDescriptionList[SelectedJobDescription].Department, JDLevel = EmployeeJobDescriptionList[SelectedJobDescription].JDLevel, EmployeeJobDescriptions= EmployeeJobDescriptionList[SelectedJobDescription].EmployeeJobDescriptions };
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method  AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {
            IsPrint = false;
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


        #region  Validation

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
                string empselectedJobDescription = BindableBase.GetPropertyName(() => SelectedJobDescription);

                if (columnName == empselectedJobDescription)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedJobDescription, SelectedJobDescription);
                }

                return null;
            }
        }
        #endregion

    }
}
