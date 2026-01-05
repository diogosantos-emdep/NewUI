using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddNewEmployeeAuthorizedLeaveViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private bool isNew;
        private string windowHeader;
        private int regularDays;
        private decimal regularHours;
        private int additionalDays;
        private decimal additionalHours;
        private int selectedLeaveType;
        private ObservableCollection<EmployeeAnnualLeave> existEmployeeAnnualLeaveList;
        private int selectedYear;
        public EmployeeAnnualLeave ExistEmployeeAnnualLeave;
        private int idCompany;
        private int idCompanyShift;
        private bool isSave;
        private string error = string.Empty;
        private decimal enjoyedHours;
        private ObservableCollection<LookupValue> annualLeaveList;
        private bool isAdd;
        private bool isEdit;

        #endregion

        #region Properties

        public EmployeeAnnualLeave NewEmployeeAnnualLeave { get; set; }
        public EmployeeAnnualLeave UpdateEmployeeAnnualLeave { get; set; }
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

        public int RegularDays
        {
            get
            {
                return regularDays;
            }

            set
            {
                regularDays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegularDays"));
            }
        }

        public decimal RegularHours
        {
            get
            {
                return regularHours;
            }

            set
            {
                regularHours = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegularHours"));
            }
        }

        public int AdditionalDays
        {
            get
            {
                return additionalDays;
            }

            set
            {
                additionalDays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AdditionalDays"));
            }
        }

        public decimal AdditionalHours
        {
            get
            {
                return additionalHours;
            }

            set
            {
                additionalHours = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AdditionalHours"));
            }
        }

        public int SelectedLeaveType
        {
            get
            {
                return selectedLeaveType;
            }

            set
            {
                selectedLeaveType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLeaveType"));
            }
        }
        public ObservableCollection<EmployeeAnnualLeave> ExistEmployeeAnnualLeaveList
        {
            get
            {
                return existEmployeeAnnualLeaveList;
            }

            set
            {
                existEmployeeAnnualLeaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeAnnualLeaveList"));
            }
        }

        public int SelectedYear
        {
            get
            {
                return selectedYear;
            }

            set
            {
                selectedYear = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedYear"));
            }
        }

        public int IdCompany
        {
            get
            {
                return idCompany;
            }

            set
            {
                idCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCompany"));
            }
        }
        public int IdCompanyShift
        {
            get
            {
                return idCompanyShift;
            }

            set
            {
                idCompanyShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCompanyShift"));
            }
        }

        public decimal EnjoyedHours
        {
            get
            {
                return enjoyedHours;
            }

            set
            {
                enjoyedHours = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EnjoyedHours"));
            }
        }

        public ObservableCollection<LookupValue> AnnualLeaveList
        {
            get
            {
                return annualLeaveList;
            }

            set
            {
                annualLeaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AnnualLeaveList"));
            }
        }

        public bool IsAdd
        {
            get
            {
                return isAdd;
            }

            set
            {
                isAdd = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAdd"));
            }
        }

        public bool IsEdit
        {
            get
            {
                return isEdit;
            }

            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
            }
        }

        public decimal DailyHoursCount { get; set; }

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

        #region Public Commands
        public ICommand AddAuthorizedLeaveViewCancelButtonCommand { get; set; }
        public ICommand AddAuthorizedLeaveViewAcceptButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Constructor

        public AddNewEmployeeAuthorizedLeaveViewModel()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddNewEmployeeAuthorizedLeaveViewModel()...", category: Category.Info, priority: Priority.Low);
                AddAuthorizedLeaveViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddAuthorizedLeaveViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddNewEmployeeAuthorizedLeave));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                FillEmployeeLeaveType();
                GeosApplication.Instance.Logger.Log("Constructor AddNewEmployeeAuthorizedLeaveViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddNewEmployeeAuthorizedLeaveViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    me[BindableBase.GetPropertyName(() => SelectedLeaveType)];

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
                string _selectedType = BindableBase.GetPropertyName(() => SelectedLeaveType);

                if (columnName == _selectedType)
                {
                    return AuthorizedLeaveValidation.GetErrorMessage(_selectedType, SelectedLeaveType);
                }

                return null;
            }
        }






        #endregion


        #region Method

        public void Init(ObservableCollection<EmployeeAnnualLeave> AnnualLeaveList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("AddAuthorizedLeave").ToString();
                AdditionalDays = 0;
                AdditionalHours = 0;
                RegularDays = 0;
                RegularHours = 0;
                ExistEmployeeAnnualLeaveList = new ObservableCollection<EmployeeAnnualLeave>(AnnualLeaveList);
                FillEmployeeLeaveList();
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(EmployeeAnnualLeave selectedEmployeeAnnualLeave, ObservableCollection<EmployeeAnnualLeave> LeaveList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                WindowHeader = Application.Current.FindResource("EditAuthorizedLeave").ToString();

                ExistEmployeeAnnualLeaveList = new ObservableCollection<EmployeeAnnualLeave>(LeaveList);
                ExistEmployeeAnnualLeave = selectedEmployeeAnnualLeave;
                FillEmployeeLeaveList();
                RegularDays = (int)(ExistEmployeeAnnualLeave.RegularHoursCount / DailyHoursCount);
                RegularHours = (ExistEmployeeAnnualLeave.RegularHoursCount % DailyHoursCount);
                AdditionalDays = (int)(ExistEmployeeAnnualLeave.AdditionalHoursCount / DailyHoursCount);
                AdditionalHours = (ExistEmployeeAnnualLeave.AdditionalHoursCount % DailyHoursCount);

                SelectedLeaveType = AnnualLeaveList.IndexOf(AnnualLeaveList.FirstOrDefault(x => x.IdLookupValue == Convert.ToUInt16(ExistEmployeeAnnualLeave.IdLeave)));

                EnjoyedHours = ExistEmployeeAnnualLeave.Enjoyed;

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }


            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddNewEmployeeAuthorizedLeave(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddAuthorizedLeave()...", category: Category.Info, priority: Priority.Low);
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedLeaveType"));
                if (error != null)
                {
                    return;
                }

                CompanyLeave companyLeave = new CompanyLeave();

                if (IsNew)
                {

                    NewEmployeeAnnualLeave = new EmployeeAnnualLeave();
                    NewEmployeeAnnualLeave.TransactionOperation = ModelBase.TransactionOperations.Add;
                    NewEmployeeAnnualLeave.IdLeave = AnnualLeaveList[SelectedLeaveType].IdLookupValue;
                    NewEmployeeAnnualLeave.RegularHoursCount = (RegularDays * DailyHoursCount) + RegularHours;
                    NewEmployeeAnnualLeave.AdditionalHoursCount = (AdditionalDays * DailyHoursCount) + AdditionalHours;
                    NewEmployeeAnnualLeave.Enjoyed = 0;
                    NewEmployeeAnnualLeave.Remaining = (NewEmployeeAnnualLeave.RegularHoursCount + NewEmployeeAnnualLeave.AdditionalHoursCount) - NewEmployeeAnnualLeave.Enjoyed;
                    companyLeave.IdCompanyLeave = (ulong)AnnualLeaveList[SelectedLeaveType].IdLookupValue;
                    companyLeave.Name = AnnualLeaveList[SelectedLeaveType].Value;
                    companyLeave.HtmlColor = AnnualLeaveList[SelectedLeaveType].HtmlColor;
                    NewEmployeeAnnualLeave.CompanyLeave = companyLeave;
                    NewEmployeeAnnualLeave.Year = SelectedYear;
                    IsSave = true;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddAuthorizedLeaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else
                {

                    UpdateEmployeeAnnualLeave = new EmployeeAnnualLeave();
                    UpdateEmployeeAnnualLeave.IdEmployeeAnnualLeave = ExistEmployeeAnnualLeave.IdEmployeeAnnualLeave;
                    UpdateEmployeeAnnualLeave.IdLeave = AnnualLeaveList[SelectedLeaveType].IdLookupValue;
                    UpdateEmployeeAnnualLeave.RegularHoursCount = (RegularDays * DailyHoursCount) + RegularHours;
                    UpdateEmployeeAnnualLeave.AdditionalHoursCount = (AdditionalDays * DailyHoursCount) + AdditionalHours;
                    UpdateEmployeeAnnualLeave.Year = SelectedYear;
                    UpdateEmployeeAnnualLeave.Enjoyed = 0;
                    UpdateEmployeeAnnualLeave.Remaining = (UpdateEmployeeAnnualLeave.RegularHoursCount + UpdateEmployeeAnnualLeave.AdditionalHoursCount) - UpdateEmployeeAnnualLeave.Enjoyed;
                    companyLeave.IdCompanyLeave = (ulong)AnnualLeaveList[SelectedLeaveType].IdLookupValue;
                    companyLeave.Name = AnnualLeaveList[SelectedLeaveType].Value;
                    companyLeave.HtmlColor = AnnualLeaveList[SelectedLeaveType].HtmlColor;
                    UpdateEmployeeAnnualLeave.CompanyLeave = companyLeave;
                    IsSave = true;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateAuthorizedLeaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddAuthorizedLeave()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            //catch (FaultException<ServiceException> ex)
            //{
            //    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            //    GeosApplication.Instance.Logger.Log("Get an error in AddAuthorizedLeave() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
            //    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            //}
            //catch (ServiceUnexceptedException ex)
            //{
            //    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            //    GeosApplication.Instance.Logger.Log("Get an error in AddAuthorizedLeave() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            //    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            //}
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddAuthorizedLeave()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void FillEmployeeLeaveType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeaveType()...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.EmployeeLeaveList == null)
                {

                    GeosApplication.Instance.EmployeeLeaveList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(32));
                    GeosApplication.Instance.EmployeeLeaveList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0 });
                }
                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeaveType()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeaveType() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeaveType() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeLeaveType()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        public void FillEmployeeLeaveList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeaveList()...", category: Category.Info, priority: Priority.Low);
                AnnualLeaveList = new ObservableCollection<LookupValue>(GeosApplication.Instance.EmployeeLeaveList);

                if (IsAdd)
                {
                    for (int i = 0; i < ExistEmployeeAnnualLeaveList.Count; i++)
                    {
                        LookupValue AnnualLeave = new LookupValue();
                        AnnualLeave = GeosApplication.Instance.EmployeeLeaveList.Where(x => x.IdLookupValue == Convert.ToInt32(ExistEmployeeAnnualLeaveList[i].CompanyLeave.IdCompanyLeave)).FirstOrDefault();
                        AnnualLeaveList.Remove(AnnualLeave);

                    }
                }
                else if (IsEdit)
                {
                    var TempExistEmployeeAnnualLeaveList = ExistEmployeeAnnualLeaveList.Where(x => x.CompanyLeave.IdCompanyLeave != ExistEmployeeAnnualLeave.CompanyLeave.IdCompanyLeave).ToList();
                    for (int i = 0; i < TempExistEmployeeAnnualLeaveList.Count; i++)
                    {
                        LookupValue AnnualLeave = new LookupValue();
                        AnnualLeave = GeosApplication.Instance.EmployeeLeaveList.Where(x => x.IdLookupValue == Convert.ToInt32(TempExistEmployeeAnnualLeaveList[i].CompanyLeave.IdCompanyLeave)).FirstOrDefault();
                        AnnualLeaveList.Remove(AnnualLeave);

                    }
                }



                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeaveList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeLeaveList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

    }
}
