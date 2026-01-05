using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Crm;
using Emdep.Geos.Modules.Crm.ViewModels;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.Data.Common.PLM;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    // [nsatpute] [GEOS2-5702][28-06-2024] Add new import accounts/contacts option (2/2)
    public class ReadImportedCustomerContactViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        #endregion


        #region Fields
        private bool isBusy;
        private ObservableCollection<People> importedContacts;
        private bool isSave;
        private List<IDictionary<string, object>> reportData;
        private IList<LookupValue> departmentList;
        private IList<LookupValue> influenceLevelList;
        private IList<LookupValue> productInvolvedList;
        private IList<LookupValue> emdepAffinityList;
        private IList<LookupValue> genderList;
        private IList<Company> plantList;
        private bool isAcceptEnable;
        private ObservableCollection<Customer> listGroup;
        private string salesOwnersIds = "";
        private ObservableCollection<Company> entireCompanyPlantList;
        #endregion


        #region Properties

        public ObservableCollection<People> ImportedContacts
        {
            get { return importedContacts; }
            set { importedContacts = value; OnPropertyChanged(new PropertyChangedEventArgs("ImportedContacts")); }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
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
        public IList<LookupValue> DepartmentList
        {
            get { return departmentList; }
            set
            {
                departmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DepartmentList"));
            }
        }
        public IList<LookupValue> InfluenceLevelList
        {
            get { return influenceLevelList; }
            set { influenceLevelList = value; OnPropertyChanged(new PropertyChangedEventArgs("InfluenceLevelList")); }
        }

        public IList<LookupValue> ProductInvolvedList
        {
            get { return productInvolvedList; }
            set { productInvolvedList = value; OnPropertyChanged(new PropertyChangedEventArgs("ProductInvolvedList")); }
        }
        public IList<LookupValue> EmdepAffinityList
        {
            get { return emdepAffinityList; }
            set { emdepAffinityList = value; OnPropertyChanged(new PropertyChangedEventArgs("EmdepAffinityList")); }
        }
        public IList<LookupValue> GenderList
        {
            get { return genderList; }
            set { genderList = value; OnPropertyChanged(new PropertyChangedEventArgs("GenderList")); }
        }
        public IList<Company> PlantList
        {
            get { return plantList; }
            set { plantList = value; OnPropertyChanged(new PropertyChangedEventArgs("PlantList")); }
        }
        public bool IsAcceptEnable
        {
            get { return isAcceptEnable; }
            set { isAcceptEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnable")); }
        }
        public ObservableCollection<Customer> ListGroup
        {
            get { return listGroup; }
            set
            {
                listGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListGroup"));
            }
        }
        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set
            {
                entireCompanyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
            }
        }
        #endregion

        #region Events
        public event EventHandler RequestClose;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public commands
        public ICommand EmdepAffinityEditorValidateCommand { get; set; }
        public ICommand InfluenceLevelEditorValidateCommand { get; set; }
        public ICommand ProductInvolvedEditorValidateCommand { get; set; }
        public ICommand CompanyDepartmentEditorValidateCommand { get; set; }
        public ICommand FirstNameFieldValidateCommand { get; set; }
        public ICommand LastNameFieldValidateCommand { get; set; }
        public ICommand JobTitleFieldValidateCommand { get; set; }
        public ICommand EmailFieldValidateCommand { get; set; }

        public ICommand GroupNameValidateCommand { get; set; }
        public ICommand PlantValidateCommand { get; set; }
        public ICommand GenderValidateCommand { get; set; }

        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand UpdateSelectedFieldsCommand { get; set; }

        #endregion

        #region Constructor 
        public ReadImportedCustomerContactViewModel(List<IDictionary<string, object>> reportData)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor  ReadImportedCustomerContactViewModel()...", category: Category.Info, priority: Priority.Low);
                InitComamnds();
                importedContacts = new ObservableCollection<People>();
                this.reportData = reportData;
                FillGroupList();
                FillCompanyPlantList();
                GenderList = CrmStartUp.GetLookupValues(1).Where(x => x.InUse).ToList();
                DepartmentList = CrmStartUp.GetLookupValues(21).Where(x => x.InUse).ToList();
                InfluenceLevelList = CrmStartUp.GetLookupValues(22).Where(x => x.InUse).ToList();
                EmdepAffinityList = CrmStartUp.GetLookupValues(23).Where(x => x.InUse).ToList();
                ProductInvolvedList = CrmStartUp.GetLookupValues(24).Where(x => x.InUse).ToList();
                ProcessReportData();
                IsAcceptEnable = ImportedContacts.Count > 0;
                IsBusy = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor ReadImportedCustomerContactViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedCustomerContactViewModel Constructor ReadImportedCustomerContactViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        private void FillGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP21"))
                        ListGroup = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP21"];
                    else
                    {
                        //[rdixit][GEOS2-4682][08-08-2023] Service GetSelectedUserCompanyGroup updated with GetSelectedUserCompanyGroup_V2420
                        ListGroup = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup_V2420(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", ListGroup);
                    }
                }
                else
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP"))
                        ListGroup = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP"];
                    else
                    {

                        //ListGroup = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));

                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        //[19.10.2023][GEOS2-4903][rdixit] Service GetCompanyGroup_V2420 updated with GetCompanyGroup_V2450
                        ListGroup = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup_V2450(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", ListGroup);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillGroupList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList ...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT21"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT21"];
                    else
                    {
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser_V2420(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", EntireCompanyPlantList);
                    }
                }
                else
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)(GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT"]);
                    else
                    {
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId_V2420(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);
                    }
                }

                if (EntireCompanyPlantList != null)
                {
                    ImportedContacts.ToList().ForEach(x => { x.AllCompanies = EntireCompanyPlantList.ToList(); });
                }
                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void AcceptButtonCommandAction(object obj)
        {
            try
            {
                if (Validation() && EmailValidation())
                {
                    GeosApplication.Instance.Logger.Log("AcceptButtonCommandAction Method  AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                    OnPropertyChanged(new PropertyChangedEventArgs("BusinessField.Value"));
                    OnPropertyChanged(new PropertyChangedEventArgs("BusinessCenter.Value"));

                    DateTime createdIn = DateTime.Now;
                    ImportedContacts.Where(x => x.IsSelected).ToList().ForEach(x =>
                     {
                         x.CreatedIn = createdIn; Customer cust = new Customer();
                         cust.CustomerName = x.Customer.CustomerName;
                         cust.IdCustomer = x.Customer.IdCustomer;
                         x.Creator = new People(); x.Creator.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                         x.ImportContactCompany.Customers.Add(cust);
                         x.Company = x.ImportContactCompany;
                         AddChangeLog(x);
                     });
                    List<People> finalList = ImportedContacts.Where(x => x.IsSelected).ToList();
                    IsSave = CrmStartUp.InsertImportedContacts(finalList);
                    if (IsSave)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ReadImportedCotactView_ContactSaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("AcceptButtonCommandAction Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                IsSave = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private bool Validation()
        {
            if (ImportedContacts.Where(x => x.IsSelected).Any(x => x.Customer == null))
                return false;
            if (ImportedContacts.Where(x => x.IsSelected).Any(x => x.ImportContactCompany.Country == null))
                return false;
            if (ImportedContacts.Where(x => x.IsSelected).Any(x => x.Gender == null))
                return false;
            if (ImportedContacts.Where(x => x.IsSelected).Any(x => x.ProductInvolved == null))
                return false;
            if (ImportedContacts.Where(x => x.IsSelected).Any(x => x.InfluenceLevel == null))
                return false;
            if (ImportedContacts.Where(x => x.IsSelected).Any(x => x.EmdepAffinity == null))
                return false;
            if (ImportedContacts.Where(x => x.IsSelected).Any(x => x.CompanyDepartment == null))
                return false;
            if (ImportedContacts.Where(x => x.IsSelected).Any(x => x.Name == null) ||
                ImportedContacts.Where(x => x.IsSelected).Any(x => x.Name.Trim() == string.Empty))
                return false;
            if (ImportedContacts.Where(x => x.IsSelected).Any(x => x.Surname == null) ||
               ImportedContacts.Where(x => x.IsSelected).Any(x => x.Surname.Trim() == string.Empty))
                return false;
            if (ImportedContacts.Where(x => x.IsSelected).Any(x => x.JobTitle == null) ||
               ImportedContacts.Where(x => x.IsSelected).Any(x => x.JobTitle.Trim() == string.Empty))
                return false;
            if (ImportedContacts.Where(x => x.IsSelected).Any(x => x.Email == null) ||
               ImportedContacts.Where(x => x.IsSelected).Any(x => x.Email.Trim() == string.Empty))
                return false;
            return true;
        }
        private bool EmailValidation()
        {
            bool result = true;
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            StringBuilder sbEmails = new StringBuilder();
            sbEmails.Append(Environment.NewLine);
            foreach (People people in ImportedContacts.Where(x => x.IsSelected))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(people.Email, emailPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    result = false;
                    sbEmails.Append(people.FullName);
                    sbEmails.Append(Environment.NewLine);
                }
            }
            if (!result)
            {
                CustomMessageBox.Show(string.Format(Application.Current.Resources["ReadImportedCotactView_EmailValidation"].ToString(), sbEmails.ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }

            return result;
        }
        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }
        public void InitComamnds()
        {
            EmdepAffinityEditorValidateCommand = new DelegateCommand<object>(EmdepAffinityEditorValidateCommandAction);
            InfluenceLevelEditorValidateCommand = new DelegateCommand<object>(InfluenceLevelEditorValidateCommandAction);
            ProductInvolvedEditorValidateCommand = new DelegateCommand<object>(ProductInvolvedEditorValidateCommandAction);
            CompanyDepartmentEditorValidateCommand = new DelegateCommand<object>(CompanyDepartmentEditorValidateCommandAction);
            FirstNameFieldValidateCommand = new DelegateCommand<object>(FirstNameFieldValidateCommandAction);
            LastNameFieldValidateCommand = new DelegateCommand<object>(LastNameFieldValidateCommandAction);
            JobTitleFieldValidateCommand = new DelegateCommand<object>(JobTitleFieldValidateCommandAction);
            EmailFieldValidateCommand = new DelegateCommand<object>(EmailFieldValidateCommandAction);
            UpdateSelectedFieldsCommand = new DelegateCommand<object>(UpdateSelectedFieldsCommandAction);
            GroupNameValidateCommand = new DelegateCommand<object>(GroupNameValidateCommandAction);
            PlantValidateCommand = new DelegateCommand<object>(PlantValidateCommandAction);
            GenderValidateCommand = new DelegateCommand<object>(GenderValidateCommandAction);
            AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
            CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
        }

        public void EmdepAffinityEditorValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Emdep Affinity");
            }
        }
        public void InfluenceLevelEditorValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Influence Level");
            }
        }
        public void ProductInvolvedEditorValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Product Involved");
            }
        }
        public void CompanyDepartmentEditorValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Department");
            }
        }
        public void GroupNameValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Group");
            }
        }
        #region Simultaneous modifications to the fields.

        public void UpdateSelectedFieldsCommandAction(object obj)
        {
            if (Key.Enter == ((System.Windows.Input.KeyEventArgs)obj).Key)
            {
                string fieldName = string.Empty;
                string fieldValue = string.Empty;
                System.Windows.RoutedEventArgs routedEvent = (System.Windows.RoutedEventArgs)obj;
                if (routedEvent.OriginalSource is System.Windows.Controls.TextBox)
                {
                    fieldName = ((DevExpress.Xpf.Grid.GridViewBase)routedEvent.Source).Grid.CurrentColumn.FieldName;
                    fieldValue = ((System.Windows.Controls.TextBox)routedEvent.OriginalSource).Text;
                    UpdateDataSwitchCase(fieldName, fieldValue);
                }
                else if (routedEvent.OriginalSource is System.Windows.Controls.TextBlock)
                {
                    fieldName = ((DevExpress.Xpf.Grid.GridViewBase)routedEvent.Source).Grid.CurrentColumn.FieldName;
                    fieldValue = ((System.Windows.Controls.TextBlock)routedEvent.OriginalSource).Text;
                    UpdateDataSwitchCase(fieldName, fieldValue);
                }
            }
        }
        public void UpdateDataSwitchCase(string fieldName, string fieldValue)
        {
            switch (fieldName)
            {
                case "Customer.CustomerName":
                    UpdateGroupData(fieldValue);
                    break;
                case "ImportContactCompany.SiteNameWithoutCountry":
                    UpdatePlantData(fieldValue);
                    break;
                case "Name":
                    UpdateFirstNameData(fieldValue);
                    break;
                case "Surname":
                    UpdateLastNameData(fieldValue);
                    break;
                case "Gender.Value":
                    UpdateGenderData(fieldValue);
                    break;
                case "Phone":
                    UpdatePhoneData(fieldValue);
                    break;
                case "CompanyDepartment.Value":
                    UpdateDepartmentData(fieldValue);
                    break;
                case "JobTitle":
                    UpdateJobTitleData(fieldValue);
                    break;
                case "Email":
                    UpdateEmailData(fieldValue);
                    break;
                case "ProductInvolved.Value":
                    UpdateProductInvolvedData(fieldValue);
                    break;
                case "InfluenceLevel.Value":
                    UpdateInfluenceLevelData(fieldValue);
                    break;
                case "EmdepAffinity.Value":
                    UpdateEmdepAffinityData(fieldValue);
                    break;
            }
        }
        public void UpdatePlantData(string value)
        {
            if (EntireCompanyPlantList.Any(x => x.SiteNameWithoutCountry == value))
            {
                Company company = EntireCompanyPlantList.FirstOrDefault(x => x.SiteNameWithoutCountry == value);
                foreach (People contact in ImportedContacts.Where(x => x.IsSelected))
                {
                    contact.ImportContactCompany = company;
                }
            }
        }
        public void UpdateGroupData(string value)
        {
            if (ListGroup.Any(x => x.CustomerName == value))
            {
                Customer group = ListGroup.FirstOrDefault(x => x.CustomerName == value);
                foreach (People contact in importedContacts.Where(x => x.IsSelected))
                {
                    contact.Customer = group;
                }
            }
        }
        public void UpdateFirstNameData(string value)
        {
            foreach (People contact in importedContacts.Where(x => x.IsSelected))
            {
                contact.Name = value;
            }
        }
        public void UpdateLastNameData(string value)
        {
            foreach (People contact in importedContacts.Where(x => x.IsSelected))
            {
                contact.Surname = value;
            }
        }
        public void UpdateGenderData(string value)
        {
            if (GenderList.Any(x => x.Value == value))
            {
                LookupValue gender = GenderList.FirstOrDefault(x => x.Value == value);
                foreach (People contact in importedContacts.Where(x => x.IsSelected))
                {
                    contact.Gender = gender;
                }
            }
        }
        public void UpdatePhoneData(string value)
        {
            foreach (People contact in importedContacts.Where(x => x.IsSelected))
            {
                contact.Phone = value;
            }
        }
        public void UpdateDepartmentData(string value)
        {
            if (DepartmentList.Any(x => x.Value == value))
            {
                LookupValue department = DepartmentList.FirstOrDefault(x => x.Value == value);
                foreach (People contact in importedContacts.Where(x => x.IsSelected))
                {
                    contact.CompanyDepartment = department;
                }
            }
        }
        public void UpdateJobTitleData(string value)
        {
            foreach (People contact in importedContacts.Where(x => x.IsSelected))
            {
                contact.JobTitle = value;
            }
        }
        public void UpdateEmailData(string value)
        {
            foreach (People contact in importedContacts.Where(x => x.IsSelected))
            {
                contact.Email = value;
            }
        }
        public void UpdateProductInvolvedData(string value)
        {
            if (ProductInvolvedList.Any(x => x.Value == value))
            {
                LookupValue productInvolved = ProductInvolvedList.FirstOrDefault(x => x.Value == value);
                foreach (People contact in importedContacts.Where(x => x.IsSelected))
                {
                    contact.ProductInvolved = productInvolved;
                }
            }
        }
        public void UpdateInfluenceLevelData(string value)
        {
            if (InfluenceLevelList.Any(x => x.Value == value))
            {
                LookupValue influenceLevel = InfluenceLevelList.FirstOrDefault(x => x.Value == value);
                foreach (People contact in importedContacts.Where(x => x.IsSelected))
                {
                    contact.InfluenceLevel = influenceLevel;
                }
            }
        }
        public void UpdateEmdepAffinityData(string value)
        {
            if (EmdepAffinityList.Any(x => x.Value == value))
            {
                LookupValue emdepAffinity = EmdepAffinityList.FirstOrDefault(x => x.Value == value);
                foreach (People contact in importedContacts.Where(x => x.IsSelected))
                {
                    contact.EmdepAffinity = emdepAffinity;
                }
            }
        }

        #endregion

        public void FirstNameFieldValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "First Name");
            }
        }
        public void LastNameFieldValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Last Name");
            }
        }
        public void JobTitleFieldValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Job Title");
            }
        }
        public void EmailFieldValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Email");
            }
        }

        public void PlantValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Plant");
            }
        }
        public void GenderValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Gender");
            }
        }
        public void ProcessReportData()
        {

            foreach (IDictionary<string, object> dict in reportData)
            {
                People contact = new People();

                foreach (KeyValuePair<string, object> key in dict)
                {
                    if (!string.IsNullOrEmpty(key.Value?.ToString()))
                    {
                        if (contact.ImportContactCompany == null)
                            contact.ImportContactCompany = new Company();
                        if (contact.CompanyDepartment == null)
                            contact.CompanyDepartment = new LookupValue();
                        if (contact.ProductInvolved == null)
                            contact.ProductInvolved = new LookupValue();
                        if (contact.InfluenceLevel == null)
                            contact.InfluenceLevel = new LookupValue();
                        if (contact.EmdepAffinity == null)
                            contact.EmdepAffinity = new LookupValue();
                        if (contact.Country == null)
                            contact.Country = new Country();
                        if (contact.AllCompanies == null)
                            contact.AllCompanies = EntireCompanyPlantList.ToList();
                        contact.IsSelected = true;
                        switch (key.Key)
                        {
                            case "Group":
                                Customer group = ListGroup.FirstOrDefault(x => x.CustomerName.ToUpper() == key.Value.ToString().ToUpper());
                                if (group != null)
                                    contact.Customer = group;
                                break;
                            case "Plant":
                                Company company = new Company();
                                if (contact.FilteredCompanies == null)
                                    company = new Company();
                                else
                                    company = contact.FilteredCompanies.FirstOrDefault(x => x.SiteNameWithoutCountry == key.Value.ToString());
                                if (company == null)
                                    company = new Company();
                                contact.ImportContactCompany = company;
                                break;
                            case "Country":
                                contact.Country.Name = key.Value.ToString();
                                break;
                            case "FirstName":
                                contact.Name = key.Value.ToString();
                                break;
                            case "LastName":
                                contact.Surname = key.Value.ToString();
                                break;
                            case "Gender":
                                LookupValue gender = GenderList.FirstOrDefault(x => x.Value.ToUpper() == key.Value.ToString().ToUpper());
                                if (gender == null)
                                    gender = GenderList.FirstOrDefault(x => x.Abbreviation.ToUpper() == key.Value.ToString().ToUpper().Trim());
                                if (gender == null)
                                    gender = new LookupValue();
                                contact.Gender = gender;
                                break;
                            case "Phone":
                                contact.Phone = key.Value.ToString();
                                break;
                            case "Department":
                                LookupValue department = DepartmentList.FirstOrDefault(x => x.Value.ToUpper() == key.Value.ToString().ToUpper());
                                if (department == null)
                                    department = new LookupValue();
                                contact.CompanyDepartment = department;
                                break;
                            case "JobTitle":
                                contact.JobTitle = key.Value.ToString();
                                break;
                            case "Email":
                                contact.Email = key.Value.ToString();
                                break;
                            case "ProductInvolved":
                                LookupValue productInvolved = ProductInvolvedList.FirstOrDefault(x => x.Value.ToUpper() == key.Value.ToString().ToUpper());
                                if (productInvolved == null)
                                    productInvolved = new LookupValue();
                                contact.ProductInvolved = productInvolved;
                                break;
                            case "InfluenceLevel":
                                LookupValue influenceLevel = InfluenceLevelList.FirstOrDefault(x => x.Value.ToUpper() == key.Value.ToString().ToUpper());
                                if (influenceLevel == null)
                                    influenceLevel = new LookupValue();
                                contact.InfluenceLevel = influenceLevel;
                                break;
                            case "EmdepAffinity":
                                LookupValue emdepAffinity = EmdepAffinityList.FirstOrDefault(x => x.Value.ToUpper() == key.Value.ToString().ToUpper());
                                if (emdepAffinity == null)
                                    emdepAffinity = new LookupValue();
                                contact.EmdepAffinity = emdepAffinity;
                                break;
                        }
                    }
                }
                ImportedContacts.Add(contact);
            }
        }
        private void AddChangeLog(People contact)
        {
            GeosApplication.Instance.Logger.Log("Method AddChangeLog ...", category: Category.Info, priority: Priority.Low);

            if (contact != null)
            {
                string ContactName = string.Empty;
                string CustomerName = string.Empty;
                ContactName = "'" + contact.FullName + "'";
                CustomerName = "'" + contact.ImportContactCompany.Customers[0].CustomerName + "-" + contact.ImportContactCompany.Country.Name + "'";
                contact.LogEntriesByContact = new List<LogEntriesByContact>();
                contact.LogEntriesByContact.Add(new LogEntriesByContact() { IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("AddedContactChangeLog").ToString(), ContactName, CustomerName), IdLogEntryType = 2 });
            }
            GeosApplication.Instance.Logger.Log("Method AddChangeLog() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
