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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddLanguagesViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services       
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public Icommands
        public ICommand AddLanguagesViewCancelButtonCommand { get; set; }
        public ICommand AddLanguagesViewAcceptButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Declaration
        private bool isSave;
        private string remarks;
        private int selectedIndexLanguage;
        private int selectedIndexUnderstandingLevel;
        private int selectedIndexSpeakingLevel;
        private int selectedIndexWritingLevel;
        private bool isNew;
        private string windowHeader;
        private ObservableCollection<EmployeeLanguage> existEmployeeLanguageList;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;
        #endregion

        #region Properties

        public List<LookupValue> LanguageList { get; set; }
        public List<LookupValue> UnderstandingLevelList { get; set; }
        public List<LookupValue> SpeakingLevelList { get; set; }
        public List<LookupValue> WritingLevelList { get; set; }
        public EmployeeLanguage NewLanguage { get; set; }
        public EmployeeLanguage EditLanguage { get; set; }
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

        public int SelectedIndexLanguage
        {
            get
            {
                return selectedIndexLanguage;
            }

            set
            {
                selectedIndexLanguage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexLanguage"));
            }
        }

        public int SelectedIndexUnderstandingLevel
        {
            get
            {
                return selectedIndexUnderstandingLevel;
            }

            set
            {
                selectedIndexUnderstandingLevel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexUnderstandingLevel"));
            }
        }

        public int SelectedIndexSpeakingLevel
        {
            get
            {
                return selectedIndexSpeakingLevel;
            }

            set
            {
                selectedIndexSpeakingLevel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexSpeakingLevel"));
            }
        }

        public int SelectedIndexWritingLevel
        {
            get
            {
                return selectedIndexWritingLevel;
            }

            set
            {
                selectedIndexWritingLevel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexWritingLevel"));
            }
        }

        public ObservableCollection<EmployeeLanguage> ExistEmployeeLanguageList
        {
            get
            {
                return existEmployeeLanguageList;
            }

            set
            {
                existEmployeeLanguageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeLanguageList"));
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
        public AddLanguagesViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor AddLanguagesViewModel()...", category: Category.Info, priority: Priority.Low);

                AddLanguagesViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddLanguagesViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddLanguageInformation));
                ExistEmployeeLanguageList = new ObservableCollection<EmployeeLanguage>();
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                GeosApplication.Instance.Logger.Log("Constructor AddLanguagesViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddLanguagesViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
        private void FillLanguageList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLanguageList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempCountryList = CrmStartUp.GetLookupValues(16);
                LanguageList = new List<LookupValue>();
                LanguageList = new List<LookupValue>(tempCountryList);
                LanguageList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                GeosApplication.Instance.Logger.Log("Method FillLanguageList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillLanguageList()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }

        private void FillUnderstandingLevelList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLanguageList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempCountryList = CrmStartUp.GetLookupValues(17);
                UnderstandingLevelList = new List<LookupValue>();
                UnderstandingLevelList = new List<LookupValue>(tempCountryList);
                UnderstandingLevelList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                GeosApplication.Instance.Logger.Log("Method FillLanguageList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillLanguageList()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }

        private void FillSpeakingLevelList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSpeakingLevelList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempCountryList = CrmStartUp.GetLookupValues(17);
                SpeakingLevelList = new List<LookupValue>();
                SpeakingLevelList = new List<LookupValue>(tempCountryList);
                SpeakingLevelList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                GeosApplication.Instance.Logger.Log("Method FillSpeakingLevelList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillSpeakingLevelList()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }

        private void FillWritingLevelList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWritingLevelList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempCountryList = CrmStartUp.GetLookupValues(17);
                WritingLevelList = new List<LookupValue>();
                WritingLevelList = new List<LookupValue>(tempCountryList);
                WritingLevelList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                GeosApplication.Instance.Logger.Log("Method FillWritingLevelList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillWritingLevelList()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }
        public void Init(ObservableCollection<EmployeeLanguage> EmployeeLanguageList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("AddLanguageInformation").ToString();
                ExistEmployeeLanguageList = EmployeeLanguageList;
                FillLanguageList();
                FillUnderstandingLevelList();
                FillSpeakingLevelList();
                FillWritingLevelList();

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method Init()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }

        public void EditInit(ObservableCollection<EmployeeLanguage> EmployeeLanguageList, EmployeeLanguage empLanguage)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("EditLanguageInformation").ToString();
                ExistEmployeeLanguageList = new ObservableCollection<EmployeeLanguage>(EmployeeLanguageList.ToList());
                ExistEmployeeLanguageList.Remove(empLanguage);

                FillLanguageList();
                FillUnderstandingLevelList();
                FillSpeakingLevelList();
                FillWritingLevelList();
                SelectedIndexLanguage = LanguageList.FindIndex(x => x.IdLookupValue == empLanguage.Language.IdLookupValue);
                SelectedIndexUnderstandingLevel = UnderstandingLevelList.FindIndex(x => x.IdLookupValue == empLanguage.UnderstandingLevel.IdLookupValue);
                SelectedIndexSpeakingLevel = SpeakingLevelList.FindIndex(x => x.IdLookupValue == empLanguage.SpeakingLevel.IdLookupValue);
                selectedIndexWritingLevel = WritingLevelList.FindIndex(x => x.IdLookupValue == empLanguage.WritingLevel.IdLookupValue);
                Remarks = empLanguage.LanguageRemarks;

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }

        public void InitReadOnly(EmployeeLanguage empLanguage)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("EditLanguageInformation").ToString();


                LanguageList = new List<LookupValue>();
                LanguageList.Add(empLanguage.Language);
                UnderstandingLevelList = new List<LookupValue>();
                UnderstandingLevelList.Add(empLanguage.UnderstandingLevel);
                SpeakingLevelList = new List<LookupValue>();
                SpeakingLevelList.Add(empLanguage.SpeakingLevel);
                WritingLevelList = new List<LookupValue>();
                WritingLevelList.Add(empLanguage.WritingLevel);

                SelectedIndexLanguage = 0;
                SelectedIndexUnderstandingLevel = 0;
                SelectedIndexSpeakingLevel = 0;
                selectedIndexWritingLevel = 0;
                Remarks = empLanguage.LanguageRemarks;

                GeosApplication.Instance.Logger.Log("Method InitReadOnly()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
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
                    me[BindableBase.GetPropertyName(() => SelectedIndexLanguage)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexUnderstandingLevel)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexSpeakingLevel)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexWritingLevel)];



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
                string empselectedIndexLanguage = BindableBase.GetPropertyName(() => SelectedIndexLanguage);
                string empselectedIndexUnderstandingLevel = BindableBase.GetPropertyName(() => SelectedIndexUnderstandingLevel);
                string empselectedIndexSpeakingLevel = BindableBase.GetPropertyName(() => SelectedIndexSpeakingLevel);
                string empselectedIndexWritingLevel = BindableBase.GetPropertyName(() => SelectedIndexWritingLevel);

                if (columnName == empselectedIndexLanguage)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedIndexLanguage, SelectedIndexLanguage);
                }
                if (columnName == empselectedIndexUnderstandingLevel)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedIndexUnderstandingLevel, SelectedIndexUnderstandingLevel);
                }
                if (columnName == empselectedIndexSpeakingLevel)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedIndexSpeakingLevel, SelectedIndexSpeakingLevel);
                }
                if (columnName == empselectedIndexWritingLevel)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empselectedIndexWritingLevel, SelectedIndexWritingLevel);
                }
                return null;
            }
        }


        private void AddLanguageInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLanguageInformation()...", category: Category.Info, priority: Priority.Low);

                string error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexLanguage"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexUnderstandingLevel"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexSpeakingLevel"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexWritingLevel"));

                if (error != null)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(Remarks))
                    Remarks = Remarks.Trim();

                EmployeeLanguage TempNewLanguage = new EmployeeLanguage();
                TempNewLanguage = ExistEmployeeLanguageList.FirstOrDefault(x => x.IdLanguage == LanguageList[SelectedIndexLanguage].IdLookupValue);
                if (TempNewLanguage == null)
                {
                    if (IsNew == true)
                    {
                        NewLanguage = new EmployeeLanguage()
                        {
                            IdLanguage = LanguageList[SelectedIndexLanguage].IdLookupValue,
                            Language = LanguageList[SelectedIndexLanguage],
                            LanguageRemarks = Remarks,
                            SpeakingIdLanguageLevel = SpeakingLevelList[SelectedIndexSpeakingLevel].IdLookupValue,
                            SpeakingLevel = SpeakingLevelList[SelectedIndexSpeakingLevel],
                            UnderstandingIdLanguageLevel = UnderstandingLevelList[SelectedIndexUnderstandingLevel].IdLookupValue,
                            UnderstandingLevel = UnderstandingLevelList[SelectedIndexUnderstandingLevel],
                            WritingIdLanguageLevel = WritingLevelList[SelectedIndexWritingLevel].IdLookupValue,
                            WritingLevel = WritingLevelList[SelectedIndexWritingLevel],
                            TransactionOperation = ModelBase.TransactionOperations.Add
                        };

                        IsSave = true;
                        RequestClose(null, null);

                    }
                    else
                    {
                        EditLanguage = new EmployeeLanguage() { IdLanguage = LanguageList[SelectedIndexLanguage].IdLookupValue, Language = LanguageList[SelectedIndexLanguage], LanguageRemarks = Remarks, SpeakingIdLanguageLevel = SpeakingLevelList[SelectedIndexSpeakingLevel].IdLookupValue, SpeakingLevel = SpeakingLevelList[SelectedIndexSpeakingLevel], UnderstandingIdLanguageLevel = UnderstandingLevelList[SelectedIndexUnderstandingLevel].IdLookupValue, UnderstandingLevel = UnderstandingLevelList[SelectedIndexUnderstandingLevel], WritingIdLanguageLevel = WritingLevelList[SelectedIndexWritingLevel].IdLookupValue, WritingLevel = WritingLevelList[SelectedIndexWritingLevel], TransactionOperation = ModelBase.TransactionOperations.Update };
                        IsSave = true;
                        RequestClose(null, null);
                    }
                }
                else
                {
                    IsSave = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddLanguageInformationExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method AddLanguageInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method AddLanguageInformation()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
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
