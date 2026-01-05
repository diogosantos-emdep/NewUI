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

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    // [nsatpute][25-06-2024][GEOS2-5701] Add new import accounts/contacts option (1/2) 
    public class ReadImportedCustomerAccountViewModel : INotifyPropertyChanged
    {
        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        #endregion


        #region Fields
        private bool isBusy;
        private ObservableCollection<Company> importedAccounts;
        private bool isSave;
        private List<IDictionary<string, object>> reportData;
        private IList<LookupValue> businessCenters;
        private IList<LookupValue> businessFields;
        private IList<LookupValue> sourceList;
        private IList<Country> countryList;
        private List<People> salesOwnerList;
        private bool isAcceptEnable;
        #endregion


        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public ObservableCollection<Company> ImportedAccounts
        {
            get { return importedAccounts; }
            set
            {
                importedAccounts = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImportedAccounts"));
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

        public IList<LookupValue> BusinessCenters
        {
            get { return businessCenters; }
            set { businessCenters = value; OnPropertyChanged(new PropertyChangedEventArgs("BusinessCenter")); }
        }

        public IList<LookupValue> BusinessFields
        {
            get { return businessFields; }
            set { businessFields = value; OnPropertyChanged(new PropertyChangedEventArgs("BusinessFields")); }
        }
        public IList<LookupValue> SourceList
        {
            get { return sourceList; }
            set { sourceList = value; OnPropertyChanged(new PropertyChangedEventArgs("SourceList")); }
        }

        public IList<Country> CountryList
        {
            get { return countryList; }
            set { countryList = value; OnPropertyChanged(new PropertyChangedEventArgs("CountryList")); }
        }
        public List<People> SalesOwnerList
        {
            get { return salesOwnerList; }
            set
            {
                salesOwnerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesOwnerList"));
            }
        }
        public bool IsAcceptEnable
        {
            get { return isAcceptEnable; }
            set { isAcceptEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnable")); }
        }
        #endregion

        #region Events
        public event EventHandler RequestClose;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public commands
        public ICommand BusinessCenterEditorValidateCommand { get; set; }
        public ICommand BusinessFieldEditorValidateCommand { get; set; }
        public ICommand SalesOwnerValidateCommand { get; set; }
        public ICommand SourceNameValidateCommand { get; set; }
        public ICommand GroupNameValidateCommand { get; set; }
        public ICommand PlantFieldValidateCommand { get; set; }
        public ICommand CityFieldValidateCommand { get; set; }
        public ICommand AddressFieldValidateCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand UpdateSelectedFieldsCommand { get; set; }
        #endregion

        #region Constructor 
        public ReadImportedCustomerAccountViewModel(List<IDictionary<string, object>> reportData)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor  ReadImportedCustomerAccountViewModel()...", category: Category.Info, priority: Priority.Low);


                InitComamnds();
                importedAccounts = new ObservableCollection<Company>();
                this.reportData = reportData;
                BusinessCenters = CrmStartUp.GetLookupValues(6).Where(x => x.InUse).ToList();
                BusinessFields = CrmStartUp.GetLookupValues(5).Where(x => x.InUse).ToList();
                SourceList = CrmStartUp.GetLookupValues(126).Where(x => x.InUse).ToList();
                CountryList = CrmStartUp.GetAllCountriesDetails();
                SalesOwnerList = CrmStartUp.GetAllActivePeoples_V2530();
                ProcessReportData();
                IsAcceptEnable = ImportedAccounts.Count > 0;
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor ReadImportedCustomerAccountViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsAcceptEnable = false;
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ReadImportedCustomerAccountViewModel Constructor ReadImportedCustomerAccountViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        public void AcceptButtonCommandAction(object obj)
        {
            try
            {
                if (Validation())
                {
                    var finalList = ImportedAccounts?.Where(i => i.IsSelected).ToList();
                    if (finalList?.Count > 0)
                    {
                        GeosApplication.Instance.Logger.Log("AcceptButtonCommandAction Method  AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                        finalList.ToList().ForEach(i => i.CIF = i.RegisteredNumber);
                        Company emdepSite = null;

                        if (GeosApplication.Instance.EmdepSiteList != null)
                        {
                            Company temp = GeosApplication.Instance.EmdepSiteList.FirstOrDefault(x => x.Alias == GeosApplication.Instance.SiteName);

                            if (temp != null)   
                            {
                                emdepSite = new Company();
                                emdepSite.IdCompany = temp.IdCompany;
                            }
                        }                        
                        IsSave = CrmStartUp.InsertImportedAccounts_V2690(finalList.ToList(), emdepSite); //[nsatpute][03.12.2025][GEOS2-10361]
                        if (IsSave)
                        {
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AccountSaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                            RequestClose(null, null);
                        }
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("AcceptButtonCommandAction Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                    }
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
            foreach(Company com in ImportedAccounts)
            {
                string validate = "!@#$";
                com.Address = com.Address + validate;
                com.Name = com.Name + validate;
                com.Address = com.Address.Replace(validate, string.Empty);
                com.Name = com.Name.Replace(validate, string.Empty);
            }

            if (ImportedAccounts.Where(x => x.IsSelected).Any(x => x.BusinessField == null))
                return false;

            if (ImportedAccounts.Where(x => x.IsSelected).Any(x => x.BusinessCenter == null))
                return false;

            if (ImportedAccounts.Where(x => x.IsSelected).Any(x => x.Source == null))
                return false;

            if (ImportedAccounts.Where(x => x.IsSelected).Any(x => x.PeopleSalesResponsibleAssemblyBU == null))
                return false;

            if (ImportedAccounts.Where(x => x.IsSelected).Any(x => x.Customer.CustomerName == null) 
                || ImportedAccounts.Where(x => x.IsSelected).Any(x => x.Customer.CustomerName.Trim().ToString() == string.Empty))
                return false;

            if (ImportedAccounts.Where(x => x.IsSelected).Any(x => x.Name == null)
               || ImportedAccounts.Where(x => x.IsSelected).Any(x => x.Name.ToString() == string.Empty) || PlantValidator())
                return false;

            if (ImportedAccounts.Where(x => x.IsSelected).Any(x => x.City == null)
            || ImportedAccounts.Where(x => x.IsSelected).Any(x => x.City == string.Empty))
                return false;

            if (AddressValidator())
                return false;

            return true;
        }
        private bool PlantValidator()
        {
            bool result = false;
            foreach(Company company in ImportedAccounts.Where(x => x.IsSelected))
            {
                if(company.Name.ToUpper() != company.City.ToUpper())
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        private bool AddressValidator()
        {
            bool result = false;
            foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
            {
                if (company.Address != null && company.Address.Trim() != string.Empty)
                {
                    if (company.Address.ToUpper().Contains(company.City.ToUpper()) || (company.Region != null && company.Address.ToUpper().Contains(company.Region.ToUpper()))
                            || (company.GroupCountry != null && company.Address.ToUpper().Contains(company.GroupCountry.Name.ToUpper())) ||
                            (company.RegisteredName != null && company.Address.ToString().ToUpper().Contains(company.RegisteredName.ToUpper())) ||
                            (company.ZipCode != null && company.Address.ToString().ToUpper().Contains(company.ZipCode.ToUpper())) || company.Address == string.Empty)
                    {
                        result = true;
                        break;
                    }
                }
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
            BusinessCenterEditorValidateCommand = new DelegateCommand<object>(BusinessCenterEditorValidateCommandAction);
            BusinessFieldEditorValidateCommand = new DelegateCommand<object>(BusinessFieldEditorValidateCommandAction);
            SalesOwnerValidateCommand = new DelegateCommand<object>(SalesOwnerValidateCommandAction);
            SourceNameValidateCommand = new DelegateCommand<object>(SourceNameValidateCommandAction);
            GroupNameValidateCommand = new DelegateCommand<object>(GroupNameValidateCommandAction);
            PlantFieldValidateCommand = new DelegateCommand<object>(PlantFieldValidateCommandAction);
            CityFieldValidateCommand = new DelegateCommand<object>(CityFieldValidateCommandAction);
            AddressFieldValidateCommand = new DelegateCommand<object>(AddressFieldValidateCommandAction);
            AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
            CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            UpdateSelectedFieldsCommand = new DelegateCommand<object>(UpdateSelectedFieldsCommandAction);
        }

        public void BusinessCenterEditorValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Business Center");
            }
        }
        public void BusinessFieldEditorValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Business Field");
            }
        }
        public void SalesOwnerValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Sales Owner");
            }
        }
        public void SourceNameValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Source Name");
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


        public void PlantFieldValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (((System.Windows.RoutedEventArgs)obj).Source is System.Windows.FrameworkElement)
            {
                Company company = ((Emdep.Geos.Data.Common.Company)((DevExpress.Xpf.Grid.GridCellData)((System.Windows.FrameworkElement)((System.Windows.RoutedEventArgs)obj).Source).DataContext).RowData.Row);
                if (company != null && Convert.ToString(e.Value).Trim().ToUpper() != company.City.Trim().ToUpper())
                {
                    e.IsValid = false;
                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                    e.ErrorContent = string.Format("Please add City name in Plant name");
                }
                if (e.Value == null)
                {
                    e.IsValid = false;
                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                    e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Plant");
                }
            }
        }

        public void CityFieldValidateCommandAction(object obj)
        {
            try
            {
                if (obj is Company)
                {
                    DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
                    if (e.Value == null)
                    {
                        e.IsValid = false;
                        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "City");
                    }
                }
            }
            catch { }
        }
		//[nsatpute][03.12.2025][GEOS2-10361]
        public void AddressFieldValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (((System.Windows.RoutedEventArgs)obj).Source is System.Windows.FrameworkElement)
            {
                Company company = ((Emdep.Geos.Data.Common.Company)((DevExpress.Xpf.Grid.GridCellData)((System.Windows.FrameworkElement)((System.Windows.RoutedEventArgs)obj).Source).DataContext).RowData.Row);
                if (e.Value != null && !string.IsNullOrEmpty(e.Value.ToString()))
                {
                    string valueString = e.Value.ToString().ToUpper();

                    if ((company?.City != null && valueString.Contains(company.City.ToUpper())) ||
                        (company?.Region != null && valueString.Contains(company.Region.ToUpper())) ||
                        (company?.GroupCountry?.Name != null && valueString.Contains(company.GroupCountry.Name.ToUpper())) ||
                        (company?.RegisteredName != null && valueString.Contains(company.RegisteredName.ToUpper())) ||
                        (company?.ZipCode != null && valueString.Contains(company.ZipCode.ToUpper())))
                    {
                        e.IsValid = false;
                        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        e.ErrorContent = "Address must not contain City/State/Country/Registered Name/Zip code";
                    }
                }
            }
        }

        public void ProcessReportData()
        {
            foreach (IDictionary<string, object> dict in reportData)
            {
                Company company = new Company();
                company.Customer = new Customer();
                company.Country = new Country();
                company.BusinessField = new LookupValue();
                company.BusinessType = new LookupValue();
                company.BusinessCenter = new LookupValue();

                foreach (KeyValuePair<string, object> key in dict)
                {
                    if (!string.IsNullOrEmpty(key.Value?.ToString()))
                    {
                        switch (key.Key)
                        {
                            case "Group":
                                company.Customer.CustomerName = key.Value.ToString();
                                break;
                            case "Plant":
                                company.Name = key.Value.ToString();
                                break;
                            case "Country":
                                Country country = CountryList.FirstOrDefault(x => x.Name.ToUpper() == key.Value.ToString().ToUpper());
                                if (country == null)
                                    country = CountryList.FirstOrDefault(x => x.Iso.ToUpper() == key.Value.ToString().ToUpper());
                                company.GroupCountry = country;
                                break;
                            case "City":
                                company.City = key.Value.ToString();
                                break;
                            case "ZipCode":
                                company.ZipCode = key.Value.ToString();
                                break;
                            case "Address":
                                company.Address = key.Value.ToString();
                                break;
                            case "State":
                                company.Region = key.Value.ToString();
                                break;
                            case "RegisteredName":
                                company.RegisteredName = key.Value.ToString();
                                break;
                            case "RegistrationNumber":
                                company.RegisteredNumber = key.Value.ToString();
                                break;
                            case "BusinessField":
                                LookupValue businessField = BusinessFields.FirstOrDefault(x => x.Value.ToUpper() == key.Value.ToString().ToUpper());
                                company.BusinessField = businessField;
                                break;
                            case "BusinessType":
                                LookupValue businessCenter = BusinessCenters.FirstOrDefault(x => x.Value.ToUpper() == key.Value.ToString().ToUpper());
                                company.BusinessCenter = businessCenter;
                                company.BusinessType = businessCenter;
                                break;
                            case "Website":
                                company.Website = key.Value.ToString();
                                break;
                            case "SalesOwner":
                                People salesOwner = SalesOwnerList.FirstOrDefault(x => x.Login.ToUpper() == key.Value.ToString().ToUpper());
                                if (salesOwner == null)
                                    salesOwner = SalesOwnerList.FirstOrDefault(x => x.Email.ToUpper() == key.Value.ToString().ToUpper());
                                if (salesOwner == null)
                                    salesOwner = SalesOwnerList.FirstOrDefault(x => x.FullName.ToUpper() == key.Value.ToString().ToUpper());
                                company.PeopleSalesResponsibleAssemblyBU = salesOwner;
                                break;
                            case "Source":
                                company.SourceName = key.Value.ToString();
                                if (SourceList.FirstOrDefault(i => i.Value.Trim() == company.SourceName.Trim()) != null)
                                    company.IdSource = SourceList.FirstOrDefault(i => i.Value.Trim() == company.SourceName.Trim()).IdLookupValue;
                                company.Source = SourceList.FirstOrDefault(i => i.Value.Trim() == company.SourceName.Trim());
                                break;
                        }
                    }
                }
                company.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                company.CreatedIn = DateTime.Now;
                company.IsSelected = true;
                importedAccounts.Add(company);
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
                    UpdateCustomerData(fieldValue);
                    break;
                case "Name":
                    UpdateNameData(fieldValue);
                    break;
                case "GroupCountry.Name":
                    UpdateCountryData(fieldValue);
                    break;
                case "City":
                    UpdateCityData(fieldValue);
                    break;
                case "BusinessField.Value":
                    UpdateBusinessFieldData(fieldValue);
                    break;
                case "BusinessCenter.Value":
                    UpdateBusinessCenterData(fieldValue);
                    break;
                case "ZipCode":
                    UpdateZipCodeData(fieldValue);
                    break;
                case "Address":
                    UpdateAddressData(fieldValue);
                    break;
                case "Region":
                    UpdateRegionData(fieldValue);
                    break;
                case "RegisteredName":
                    UpdateRegisteredNameData(fieldValue);
                    break;
                case "RegisteredNumber":
                    UpdateRegisteredNumberData(fieldValue);
                    break;
                case "Website":
                    UpdateWebsiteData(fieldValue);
                    break;
                case "PeopleSalesResponsibleAssemblyBU.FullName":
                    UpdateSalesOwnerUnboundData(fieldValue);
                    break;
                case "Source.Value":
                    UpdateSourceData(fieldValue);
                    break;
            }
        }
        public void UpdateCustomerData(string value)
        {
            foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
            {
                company.Customer = new Customer() { CustomerName = value };
            }
        }
        public void UpdateNameData(string value)
        {
            foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
            {
                company.Name = value;
            }
        }
        public void UpdateCountryData(string value)
        {
            if (CountryList.Any(x => x.Name == value))
            {
                Country country = CountryList.FirstOrDefault(x => x.Name == value);
                foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
                {
                    company.GroupCountry = country;
                }
            }
        }
        public void UpdateCityData(string value)
        {
            foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
            {
                company.City = value;
            }
        }
        public void UpdateBusinessFieldData(string value)
        {
            if (BusinessFields.Any(x => x.Value == value))
            {
                LookupValue businessField = BusinessFields.FirstOrDefault(x => x.Value == value);
                foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
                {
                    company.BusinessField = businessField;
                }
            }
        }
        public void UpdateBusinessCenterData(string value)
        {
            if (BusinessCenters.Any(x => x.Value == value))
            {
                LookupValue businessCenters = BusinessCenters.FirstOrDefault(x => x.Value == value);
                foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
                {
                    company.BusinessCenter = businessCenters;
                }
            }
        }
        public void UpdateZipCodeData(string value)
        {
            foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
            {
                company.ZipCode = value;
            }
        }
        public void UpdateAddressData(string value)
        {
            foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
            {
                company.Address = value;
            }
        }
        public void UpdateRegionData(string value)
        {
            foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
            {
                company.Region = value;
            }
        }
        public void UpdateRegisteredNameData(string value)
        {
            foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
            {
                company.RegisteredName = value;
            }
        }
        public void UpdateRegisteredNumberData(string value)
        {
            foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
            {
                company.RegisteredNumber = value;
            }
        }
        public void UpdateWebsiteData(string value)
        {
            foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
            {
                company.Website = value;
            }
        }

        public void UpdateSalesOwnerUnboundData(string value)
        {
            if (SalesOwnerList.Any(x => x.FullName == value))
            {
                People people = SalesOwnerList.FirstOrDefault(x => x.FullName == value);
                foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
                {
                    company.SalesOwnerUnbound = people.FullName;
                    company.PeopleSalesResponsibleAssemblyBU = people;
                }
            }
        }

        public void UpdateSourceData(string value)
        {
            if (SourceList.Any(x => x.Value == value))
            {
                LookupValue source = SourceList.FirstOrDefault(x => x.Value == value);
                foreach (Company company in ImportedAccounts.Where(x => x.IsSelected))
                {
                    company.Source = source;
                }
            }
        }

        #endregion



        #endregion
    }
}
