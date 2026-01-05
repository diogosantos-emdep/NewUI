using DevExpress.Mvvm;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddFamilyMembersViewModel : INotifyPropertyChanged, IDataErrorInfo
    {

        #region Services       
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public Icommands
        public ICommand AddFamilyMemberViewCancelButtonCommand { get; set; }
        public ICommand AddFamilyMemberViewAcceptButtonCommand { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }
        public ICommand OnDateEditValueChangingCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Declaration
        private string windowHeader;
        private bool isSave;
        private string firstName;
        private string lastName;
        private string nativeName;
        private int selectedIndexGender = -1;
        private List<int> disabilityLevelList;
        private int selectedNationalityIndex;
        private int selectedRelationshipIndex;
        private string remarks;
        private bool isDependent;
        private int Dependent;
        private int selectedDisability;
        private bool isNew;
        private Regex regex;
        private string error = string.Empty;
        private string firstNameErrorMsg = string.Empty;
        private string lastNameErrorMsg = string.Empty;
        private string nativeNameErrorMsg = string.Empty;
        private string birthDateErrorMessage = string.Empty;
        private ObservableCollection<EmployeeFamilyMember> existEmployeeFamilyMember;
        private DateTime? birthDate;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;




        #endregion

        #region Properties
        public IList<LookupValue> GenderList { get; set; }
        public List<Country> NationalityList { get; set; }
        public List<LookupValue> RelationShipTypeList { get; set; }
        public EmployeeFamilyMember NewFamilyMember { get; set; }
        public EmployeeFamilyMember EditFamilyMember { get; set; }
        public bool IsFromFirstName { get; set; }
        public bool IsFromLastName { get; set; }
        public bool IsFromNativeName { get; set; }
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

        public ObservableCollection<EmployeeFamilyMember> ExistEmployeeFamilyMember
        {
            get
            {
                return existEmployeeFamilyMember;
            }

            set
            {
                existEmployeeFamilyMember = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeFamilyMember"));
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }

            set
            {
                firstName = value;
                IsFromFirstName = true;
                IsFromLastName = false;
                IsFromNativeName = false;
                OnPropertyChanged(new PropertyChangedEventArgs("FirstName"));
            }
        }

        public string LastName
        {
            get
            {

                return lastName;
            }

            set
            {
                lastName = value;
                IsFromLastName = true;
                IsFromFirstName = false;
                IsFromNativeName = false;
                OnPropertyChanged(new PropertyChangedEventArgs("LastName"));

            }
        }

        public string NativeName
        {
            get
            {
                return nativeName;
            }

            set
            {
                nativeName = value;
                IsFromLastName = false;
                IsFromFirstName = false;
                IsFromNativeName = true;
                OnPropertyChanged(new PropertyChangedEventArgs("NativeName"));
            }
        }

        public int SelectedIndexGender
        {
            get
            {
                return selectedIndexGender;
            }

            set
            {
                selectedIndexGender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexGender"));
            }
        }

        public List<int> DisabilityLevelList
        {
            get
            {
                return disabilityLevelList;
            }

            set
            {
                disabilityLevelList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DisabilityLevelList"));
            }
        }
        public DateTime? BirthDate
        {
            get
            {
                return birthDate;
            }

            set
            {
                birthDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BirthDate"));
            }
        }
        public int SelectedNationalityIndex
        {
            get
            {
                return selectedNationalityIndex;
            }

            set
            {
                selectedNationalityIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedNationalityIndex"));
            }
        }

        public int SelectedRelationshipIndex
        {
            get
            {
                return selectedRelationshipIndex;
            }

            set
            {
                selectedRelationshipIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRelationshipIndex"));
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

        public bool IsDependent
        {
            get
            {
                return isDependent;
            }

            set
            {
                isDependent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDependent"));
            }
        }

        public int SelectedDisability
        {
            get
            {
                return selectedDisability;
            }

            set
            {
                selectedDisability = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDisability"));
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
        public AddFamilyMembersViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor AddFamilyMembersViewModel()...", category: Category.Info, priority: Priority.Low);

                regex = new Regex(@"[~`!@#$%^&*()-_+=|\{}':;.,<>/?" + Convert.ToChar(34) + "]");
                SelectedDisability = 0;
                AddFamilyMemberViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddFamilyMemberViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddFamilyMemberInformation));
                OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnTextEditValueChanging);
                OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                GeosApplication.Instance.Logger.Log("Constructor AddFamilyMembersViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddFamilyMembersViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods

        public void OnTextEditValueChanging(EditValueChangingEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method OnTextEditValueChanging ...", category: Category.Info, priority: Priority.Low);
            if (IsFromFirstName)
                firstNameErrorMsg = string.Empty;
            if (IsFromLastName)
                lastNameErrorMsg = string.Empty;
            if (IsFromNativeName)
                nativeNameErrorMsg = string.Empty;
            var newInput = (string)e.NewValue;

            if (!string.IsNullOrEmpty(newInput))
            {
                MatchCollection matches = regex.Matches(newInput.ToLower().ToString());
                if (newInput.Count(char.IsDigit) > 0 || matches.Count > 0)
                {
                    if (IsFromFirstName)
                    {
                        firstNameErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                    }

                    if (IsFromLastName)
                    {
                        lastNameErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                    }

                    if (IsFromNativeName)
                    {
                        nativeNameErrorMsg = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberFirstNameError").ToString();
                    }

                }
                error = EnableValidationAndGetError();

                if (IsFromFirstName)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                }
                if (IsFromLastName)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                }
                if (IsFromNativeName)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("NativeName"));
                }
                IsFromFirstName = false;
                IsFromLastName = false;
                IsFromNativeName = false;
            }


            GeosApplication.Instance.Logger.Log("Method OnTextEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public void OnDateEditValueChanging(EditValueChangingEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);

            if (BirthDate >= DateTime.Now)
            {
                birthDateErrorMessage = System.Windows.Application.Current.FindResource("EmployeeFamilyMemberBirthDateError").ToString();
            }
            else
            {
                birthDateErrorMessage = string.Empty;
            }
            error = EnableValidationAndGetError();
            PropertyChanged(this, new PropertyChangedEventArgs("BirthDate"));

            GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public void Init(ObservableCollection<EmployeeFamilyMember> EmployeeFamilyMemberList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("AddFamilyMemberInformation").ToString();
                ExistEmployeeFamilyMember = EmployeeFamilyMemberList;
                FillGenderList();
                FillRelationShipTypeList();
                FillNationalityList();

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
        }

        public void EditInit(ObservableCollection<EmployeeFamilyMember> EmployeeFamilyMemberList, EmployeeFamilyMember empFamilyMember)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("EditFamilyMemberInformation").ToString();
                ExistEmployeeFamilyMember = new ObservableCollection<EmployeeFamilyMember>(EmployeeFamilyMemberList.ToList());
                ExistEmployeeFamilyMember.Remove(empFamilyMember);

                FillGenderList();
                FillRelationShipTypeList();
                FillNationalityList();
                int IdGender = empFamilyMember.FamilyMemberGender.IdLookupValue;

                if (IdGender == 1)
                {
                    SelectedIndexGender = 0;
                }
                else
                {
                    SelectedIndexGender = 1;
                }
                SelectedNationalityIndex = NationalityList.FindIndex(x => x.IdCountry == empFamilyMember.FamilyMemberIdNationality);
                SelectedRelationshipIndex = RelationShipTypeList.FindIndex(x => x.IdLookupValue == empFamilyMember.FamilyMemberIdRelationshipType);
                SelectedDisability = empFamilyMember.FamilyMemberDisability;
                FirstName = empFamilyMember.FamilyMemberFirstName;
                LastName = empFamilyMember.FamilyMemberLastName;
                NativeName = empFamilyMember.FamilyMemberNativeName;
                Dependent = empFamilyMember.FamilyMemberIsDependent;
                BirthDate = empFamilyMember.FamilyMemberBirthDate;
                if (Dependent == 1)
                {
                    IsDependent = true;
                }
                else
                {
                    IsDependent = false;
                }
                Remarks = empFamilyMember.FamilyMemberRemarks;
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method InitReadOnly to Readonly users
        /// [HRM-M046-07] Add new permission ReadOnly--By Amit
        /// </summary>
        public void InitReadOnly(EmployeeFamilyMember empFamilyMember)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = Application.Current.FindResource("EditFamilyMemberInformation").ToString();


                GenderList = new List<LookupValue>();
                GenderList.Add(empFamilyMember.FamilyMemberGender);
                NationalityList = new List<Country>();
                NationalityList.Add(empFamilyMember.FamilyMemberNationality);
                RelationShipTypeList = new List<LookupValue>();
                RelationShipTypeList.Add(empFamilyMember.FamilyMemberRelationshipType);

                int IdGender = empFamilyMember.FamilyMemberGender.IdLookupValue;

                SelectedIndexGender = 0;

                SelectedNationalityIndex = 0;
                SelectedRelationshipIndex = 0;
                SelectedDisability = empFamilyMember.FamilyMemberDisability;
                FirstName = empFamilyMember.FamilyMemberFirstName;
                LastName = empFamilyMember.FamilyMemberLastName;
                NativeName = empFamilyMember.FamilyMemberNativeName;
                Dependent = empFamilyMember.FamilyMemberIsDependent;
                BirthDate = empFamilyMember.FamilyMemberBirthDate;
                if (Dependent == 1)
                {
                    IsDependent = true;
                }
                else
                {
                    IsDependent = false;
                }
                Remarks = empFamilyMember.FamilyMemberRemarks;
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method InitReadOnly()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
        }

        private void FillGenderList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGenderList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> temptypeList = CrmStartUp.GetLookupValues(1);
                GenderList = new List<LookupValue>();
                //GenderList.Insert(0, new LookupValue() { Value = "---" });
                GenderList.AddRange(temptypeList);

                GeosApplication.Instance.Logger.Log("Method FillGenderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillGenderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillGenderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillGenderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillRelationShipTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMaritalStatusList()...", category: Category.Info, priority: Priority.Low);


                IList<LookupValue> tempCountryList = CrmStartUp.GetLookupValues(14);
                RelationShipTypeList = new List<LookupValue>();
                RelationShipTypeList = new List<LookupValue>(tempCountryList);
                RelationShipTypeList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                GeosApplication.Instance.Logger.Log("Method FillMaritalStatusList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillMaritalStatusList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillMaritalStatusList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillMaritalStatusList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillNationalityList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillNationalityList()...", category: Category.Info, priority: Priority.Low);

                IList<Country> temptypeList = HrmService.GetAllCountries();
                NationalityList = new List<Country>();
                NationalityList.Insert(0, new Country() { Name = "---" });
                NationalityList.AddRange(temptypeList);

                GeosApplication.Instance.Logger.Log("Method FillNationalityList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillNationalityList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillNationalityList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillNationalityList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    me[BindableBase.GetPropertyName(() => FirstName)] +
                    me[BindableBase.GetPropertyName(() => LastName)] +
                    me[BindableBase.GetPropertyName(() => SelectedNationalityIndex)] +
                    me[BindableBase.GetPropertyName(() => SelectedRelationshipIndex)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexGender)] +
                     me[BindableBase.GetPropertyName(() => BirthDate)];



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
                string empFirstName = BindableBase.GetPropertyName(() => FirstName);
                string empLastName = BindableBase.GetPropertyName(() => LastName);
                string empNativeName = BindableBase.GetPropertyName(() => NativeName);
                string empSelectedNationalityIndex = BindableBase.GetPropertyName(() => SelectedNationalityIndex);
                string empSelectedRelationshipIndex = BindableBase.GetPropertyName(() => SelectedRelationshipIndex);
                string empSelectedIndexGender = BindableBase.GetPropertyName(() => SelectedIndexGender);
                string empBirthDate = BindableBase.GetPropertyName(() => BirthDate);

                if (columnName == empFirstName)
                {
                    if (!string.IsNullOrEmpty(firstNameErrorMsg))
                    {
                        return firstNameErrorMsg;
                    }
                    else
                    {
                        return EmployeeProfileValidation.GetErrorMessage(empFirstName, FirstName);
                    }
                }

                if (columnName == empLastName)
                {
                    if (!string.IsNullOrEmpty(lastNameErrorMsg))
                    {
                        return lastNameErrorMsg;
                    }
                    else
                    {
                        return EmployeeProfileValidation.GetErrorMessage(empLastName, LastName);
                    }
                }

                if (columnName == empNativeName)
                {
                    if (!string.IsNullOrEmpty(nativeNameErrorMsg))
                    {
                        return nativeNameErrorMsg;
                    }

                }
                if (columnName == empBirthDate)
                {
                    if (!string.IsNullOrEmpty(birthDateErrorMessage))
                    {
                        return birthDateErrorMessage;
                    }
                }

                if (columnName == empSelectedNationalityIndex)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empSelectedNationalityIndex, SelectedNationalityIndex);
                }

                if (columnName == empSelectedRelationshipIndex)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empSelectedRelationshipIndex, SelectedRelationshipIndex);
                }

                if (columnName == empSelectedIndexGender)
                {
                    return EmployeeProfileValidation.GetErrorMessage(empSelectedIndexGender, SelectedIndexGender);
                }
                return null;
            }
        }

        private void AddFamilyMemberInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFamilyMemberInformation()...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedNationalityIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedRelationshipIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexGender"));


                if (error != null)
                {
                    return;
                }
                if (IsDependent == true)
                {
                    Dependent = 1;
                }
                else
                {
                    Dependent = 0;
                }

                if (!string.IsNullOrEmpty(Remarks))
                    Remarks = Remarks.Trim();
                if (!string.IsNullOrEmpty(NativeName))
                    NativeName = NativeName.Trim();

                EmployeeFamilyMember TempEmployeeFamilyMember = new EmployeeFamilyMember();
                TempEmployeeFamilyMember = ExistEmployeeFamilyMember.FirstOrDefault(x => x.FamilyMemberFirstName == FirstName.Trim() && x.FamilyMemberLastName == LastName.Trim());
                if (TempEmployeeFamilyMember == null)
                {
                    if (IsNew == true)
                    {
                        NewFamilyMember = new EmployeeFamilyMember() { FamilyMemberFirstName = FirstName.Trim(), FamilyMemberLastName = LastName.Trim(), FamilyMemberNativeName = NativeName, FamilyMemberNationality = NationalityList[SelectedNationalityIndex], FamilyMemberIdNationality = NationalityList[SelectedNationalityIndex].IdCountry, FamilyMemberRelationshipType = RelationShipTypeList[SelectedRelationshipIndex], FamilyMemberIdRelationshipType = (uint)RelationShipTypeList[SelectedRelationshipIndex].IdLookupValue, FamilyMemberGender = GenderList[SelectedIndexGender], FamilyMemberIdGender = (uint)GenderList[SelectedIndexGender].IdLookupValue, FamilyMemberDisability = (ushort)SelectedDisability, FamilyMemberIsDependent = (byte)Dependent, FamilyMemberRemarks = Remarks, FamilyMemberBirthDate = BirthDate, TransactionOperation = ModelBase.TransactionOperations.Add };

                        IsSave = true;
                        RequestClose(null, null);

                    }
                    else
                    {
                        EditFamilyMember = new EmployeeFamilyMember() { FamilyMemberFirstName = FirstName.Trim(), FamilyMemberLastName = LastName.Trim(), FamilyMemberNativeName = NativeName, FamilyMemberNationality = NationalityList[SelectedNationalityIndex], FamilyMemberIdNationality = NationalityList[SelectedNationalityIndex].IdCountry, FamilyMemberRelationshipType = RelationShipTypeList[SelectedRelationshipIndex], FamilyMemberIdRelationshipType = (uint)RelationShipTypeList[SelectedRelationshipIndex].IdLookupValue, FamilyMemberGender = GenderList[SelectedIndexGender], FamilyMemberIdGender = (uint)GenderList[SelectedIndexGender].IdLookupValue, FamilyMemberDisability = (ushort)SelectedDisability, FamilyMemberIsDependent = (byte)Dependent, FamilyMemberRemarks = Remarks, FamilyMemberBirthDate = BirthDate, TransactionOperation = ModelBase.TransactionOperations.Update };
                        IsSave = true;
                        RequestClose(null, null);
                    }
                }
                else
                {
                    IsSave = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddContractSituationInformationExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                GeosApplication.Instance.Logger.Log("Method AddFamilyMemberInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Method AddFamilyMemberInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
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
