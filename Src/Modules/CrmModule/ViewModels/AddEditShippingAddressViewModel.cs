using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.PivotGrid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SCM;
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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    class AddEditShippingAddressViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        // ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());


        #endregion

        #region Events
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

        #region Declaration
        string informationError;
        private string windowHeader;
        private bool isSave;
        private Country selectedCountry;
        private bool isInUseVisible;
        private bool isLongTermAbsent;
        private bool isNew;
        private bool isPrimaryAddress;
        private string name;
        private string address;
        private string zipcode;
        private string city;
        private string region;
        private Int32 idcountry;
        private Int64 idShippingAddress;
        private string countryName;
        private string remark;
        private bool isInUse;
        private Emdep.Geos.Data.Common.Crm.ShippingAddress newAddress;
        int selectedIndexCountry;
        List<Country> countryList;
        #endregion

        #region Properties

        public int SelectedIndexCountry
        {
            get { return selectedIndexCountry; }
            set
            {
                selectedIndexCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCountry"));
            }
        }

        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }

        public List<Country> CountryList 
        {
            get { return countryList; }
            set
            {
                countryList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }

        public bool IsInUseVisible
        {
            get { return isInUseVisible; }
            set
            {
                isInUseVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInUseVisible"));
            }
        }

        public bool IsInUse
        {
            get { return isInUse; }
            set
            {
                isInUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("isInUse"));
            }
        }

        public Country SelectedCountry
        {
            get
            {
                return selectedCountry;
            }

            set
            {
                selectedCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCountry"));
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

        public bool IsLongTermAbsent
        {
            get
            {
                return isLongTermAbsent;
            }
            set
            {
                isLongTermAbsent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLongTermAbsent"));

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

        public bool IsPrimaryAddress
        {
            get
            {
                return isPrimaryAddress;
            }

            set
            {
                isPrimaryAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPrimaryAddress"));
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

        public string Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Address"));
            }
        }

        public string Zipcode
        {
            get
            {
                return zipcode;
            }

            set
            {
                zipcode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Zipcode"));
            }
        }

        public string City
        {
            get
            {
                return city;
            }

            set
            {
                city = value;
                OnPropertyChanged(new PropertyChangedEventArgs("City"));
            }
        }


        public string Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Region"));
            }
        }

        public Int32 IdCountry
        {
            get
            {
                return idcountry;
            }

            set
            {
                idcountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCountry"));
            }
        }


        public Int64 IdShippingAddress
        {
            get
            {
                return idShippingAddress;
            }

            set
            {
                idShippingAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdShippingAddress"));
            }
        }

        public string CountryName
        {
            get
            {
                return countryName;
            }

            set
            {
                countryName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountryName"));
            }
        }


        public string Remark
        {
            get
            {
                return remark;
            }

            set
            {
                remark = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remark"));
            }
        }


        public Emdep.Geos.Data.Common.Crm.ShippingAddress NewAddress
        {
            get
            {
                return newAddress;
            }
            set
            {
                newAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewAddress"));
            }
        }

        #endregion

        #region ICommand
        public ICommand CancelButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand AcceptFileActionCommand { get; set; }
        #endregion

        #region Constructor

        public AddEditShippingAddressViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEditShippingAddressViewModel ...", category: Category.Info, priority: Priority.Low);
                AcceptFileActionCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                FillCountry();
                GeosApplication.Instance.Logger.Log("Method AddEditShippingAddressViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCountry() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region  Methods

        private void FillCountry()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCountry ...", category: Category.Info, priority: Priority.Low);

                IList<Country> tempCountryList = CrmStartUp.GetCountries_V2570();
                CountryList = new List<Country>(tempCountryList);
                CountryList.Insert(0, new Country() { Name = "---" });
                SelectedCountry = CountryList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method FillCountry() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCountry() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCountry() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCountry() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

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

        public void EditInit(Data.Common.Crm.ShippingAddress SelectedLinkedAddress)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                if (SelectedLinkedAddress != null)
                {
                    IdShippingAddress = SelectedLinkedAddress.IdShippingAddress;
                    Name = SelectedLinkedAddress.Name;
                    Address = SelectedLinkedAddress.Address;
                    Zipcode = SelectedLinkedAddress.ZipCode;
                    City = SelectedLinkedAddress.City;
                    Region = SelectedLinkedAddress.Region;
                    IdCountry = Convert.ToInt32(SelectedLinkedAddress.IdCountry);
                    SelectedCountry = CountryList.FirstOrDefault(x => x.IdCountry == IdCountry);
                    SelectedIndexCountry = CountryList.FindIndex(x => x.IdCountry == IdCountry);
                    CountryName = SelectedLinkedAddress.CountryName;
                    Remark = SelectedLinkedAddress.Remark;
                    IsInUseVisible = true;
                    isPrimaryAddress = SelectedLinkedAddress.IsDefault;
                    IsInUse = true;
                }
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                #region Validation [rdixit][GEOS2-6462][25.11.2024]
                InformationError = null;
                string error = EnableValidationAndGetError();
              
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                PropertyChanged(this, new PropertyChangedEventArgs("Address"));
                PropertyChanged(this, new PropertyChangedEventArgs("ZipCode"));
                PropertyChanged(this, new PropertyChangedEventArgs("City"));
                PropertyChanged(this, new PropertyChangedEventArgs("ZipCode"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCountry"));
                
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";
                if (error != null)
                {
                    return;
                }
                #endregion

                if (IsNew)
                {
                    if (NewAddress == null)
                        NewAddress = new Data.Common.Crm.ShippingAddress();
                    NewAddress.IsDefault = IsPrimaryAddress;
                    NewAddress.IsDisabled = isInUse;
                    NewAddress.Name = Name;
                    NewAddress.Address = Address;
                    NewAddress.ZipCode = Zipcode;
                    NewAddress.City = City;
                    NewAddress.Region = Region;
                    NewAddress.Zipcityregion = Zipcode + " - " + City + " - " + Region; ;
                    NewAddress.Remark = Remark;
                    NewAddress.IsInUse = IsInUse;
                    NewAddress.IdCountry = SelectedCountry.IdCountry;
                    NewAddress.CountryName = SelectedCountry.Name;
                    NewAddress.Country = SelectedCountry;
                    if (NewAddress.TransactionOperation != ModelBase.TransactionOperations.Add)
                        NewAddress.TransactionOperation = ModelBase.TransactionOperations.Update;
                }
                IsSave = true;
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Validation [rdixit][GEOS2-6462][25.11.2024]

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
                    me[GetPropertyName(() => Name)] +
                    me[GetPropertyName(() => Address)] +
                    me[GetPropertyName(() => Zipcode)] +
                    me[GetPropertyName(() => City)] +
                    me[GetPropertyName(() => SelectedIndexCountry)];

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
                string name = GetPropertyName(() => Name);
                string address = GetPropertyName(() => Address);  
                string zipcode = GetPropertyName(() => Zipcode);
                string city = GetPropertyName(() => City);
                string region = GetPropertyName(() => Region);
                string selectedIndexCountry = GetPropertyName(() => SelectedIndexCountry);

                if (columnName == name)
                    return AddEditShippingAddressValidation.GetErrorMessage(name,Name);

                if (columnName == address)
                    return AddEditShippingAddressValidation.GetErrorMessage(address, Address);

                if (columnName == zipcode)
                    return AddEditShippingAddressValidation.GetErrorMessage(zipcode, Zipcode);

                if (columnName == city)
                    return AddEditShippingAddressValidation.GetErrorMessage(city, City);

                if (columnName == selectedIndexCountry)
                    return AddEditShippingAddressValidation.GetErrorMessage(selectedIndexCountry, SelectedIndexCountry);

                return null;
            }
        }

        #endregion
    }
}
