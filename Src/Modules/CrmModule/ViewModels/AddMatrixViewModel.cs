using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AddMatrixViewModel : NavigationViewModelBase, IDataErrorInfo,
        INotifyPropertyChanged, IDisposable
    {
        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion Services

        #region Commands
        public ICommand AddMatrixCancelButtonCommand { get; set; }
        public ICommand AddMatrixAcceptButtonCommand { get; set; }
        #endregion Commands

        #region Declaration

        private QuotationMatrixTemplate existingMatrixOpenedInEditScreen;
        private QuotationMatrixTemplate selectedMatrix;
        string name;
        string description;
        LookupValue regionLookupValue;
        Customer customer;
        ProductCategory productCategory;
        string url;
        bool inUse;
        private bool isSave;
        private bool isAddNewRecord;
        bool isBusy;

        private ObservableCollection<LookupValue> regionLookupValuesCollection;
        private ObservableCollection<Customer> customersCollection;
        private ObservableCollection<ProductCategory> productCategoriesCollection;
        private ObservableCollection<QuotationMatrixTemplate> allMatricesCollection;

        int selectedIndexRegionLookupValue;
        int selectedIndexCustomer;
        int selectedIndexProductCategory;

        string windowTitle;

        #endregion // Declaration

        #region Properties

        public QuotationMatrixTemplate SelectedMatrix
        {
            get { return selectedMatrix; }
            set
            {
                selectedMatrix = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedMatrix)));
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Description)));
            }
        }

        public LookupValue RegionLookupValue
        {
            get
            {
                return regionLookupValue;
            }
            set
            {
                regionLookupValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RegionLookupValue)));
            }
        }

        public Customer Customer
        {
            get
            {
                return customer;
            }
            set
            {
                customer = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Customer)));
            }
        }


        public ProductCategory ProductCategory
        {
            get
            {
                return productCategory;
            }
            set
            {
                productCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ProductCategory)));
            }
        }

        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Url)));
            }
        }

        public bool InUse
        {
            get
            {
                return inUse;
            }
            set
            {
                inUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(InUse)));
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
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSave)));
            }
        }

        public bool IsAddNewRecord
        {
            get
            {
                return isAddNewRecord;
            }
            set
            {
                isAddNewRecord = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAddNewRecord)));
            }
        }

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsBusy)));
            }
        }


        public ObservableCollection<LookupValue> RegionLookupValuesCollection
        {
            get
            {
                return regionLookupValuesCollection;
            }

            set
            {
                this.regionLookupValuesCollection = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RegionLookupValuesCollection)));
            }
        }

        public ObservableCollection<Customer> CustomersCollection
        {
            get
            {
                return customersCollection;
            }

            set
            {
                this.customersCollection = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(CustomersCollection)));
            }
        }

        public ObservableCollection<ProductCategory> ProductCategoriesCollection
        {
            get
            {
                return productCategoriesCollection;
            }

            set
            {
                this.productCategoriesCollection = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ProductCategoriesCollection)));
            }
        }

        public ObservableCollection<QuotationMatrixTemplate> AllMatricesCollection
        {
            get
            {
                return allMatricesCollection;
            }

            set
            {
                this.allMatricesCollection = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(AllMatricesCollection)));
            }
        }

        public int SelectedIndexRegionLookupValue
        {
            get
            {
                return selectedIndexRegionLookupValue;
            }

            set
            {
                this.selectedIndexRegionLookupValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedIndexRegionLookupValue)));

            }
        }

        public int SelectedIndexCustomer
        {
            get
            {
                return selectedIndexCustomer;
            }

            set
            {
                this.selectedIndexCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedIndexCustomer)));

            }
        }

        public int SelectedIndexProductCategory
        {
            get
            {
                return selectedIndexProductCategory;
            }

            set
            {
                this.selectedIndexProductCategory = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedIndexProductCategory)));

            }
        }

        public string WindowTitle
        {
            get
            {
                return windowTitle;
            }

            set
            {
                this.windowTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(WindowTitle)));
            }
        }
        #endregion // Properties

        #region Constructor
        public AddMatrixViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddMatrixViewModel...", category: Category.Info, priority: Priority.Low);

                var allCustomersCollection = new ObservableCollection<Customer>(CrmStartUp.GetAllCustomersDetails_V2160());
                allCustomersCollection = new ObservableCollection<Customer>(
                    allCustomersCollection.OrderBy(x => x.CustomerName));
                this.CustomersCollection = new ObservableCollection<Customer>();

                foreach (var item in allCustomersCollection)
                {
                    if (item.CustomerName!="< DEFAULT >" && item.IsStillActive>0)
                    {
                        this.CustomersCollection.Add(item);
                    }
                    //else
                    //{ }
                }
                
                CustomersCollection.Insert(0, new Customer { CustomerName = "---" });

                var allRegionLookupValuesCollection = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(8));
                this.RegionLookupValuesCollection = new ObservableCollection<LookupValue>(
                    allRegionLookupValuesCollection.OrderBy(x=>x.Value));

                using (var lookupValue = new LookupValue { Value = "---", InUse = true })
                {
                    RegionLookupValuesCollection.Insert(0, lookupValue);
                }

                 var allCategoriesCollection = new ObservableCollection<ProductCategory>(
                    CrmStartUp.GetAllCategory());
                allCategoriesCollection = new ObservableCollection<ProductCategory>(
                    allCategoriesCollection.Where(x => x.Level == 1).ToList());
                allCategoriesCollection = new ObservableCollection<ProductCategory>(allCategoriesCollection.OrderBy(x => x.Name));
                this.ProductCategoriesCollection = new ObservableCollection<ProductCategory>();

                foreach (var newItem in allCategoriesCollection)
                {
                    var newItemIsDuplicateItem = false;
                    foreach (var existingItem in this.ProductCategoriesCollection)
                    {
                        if (existingItem.Name == newItem.Name)
                        {
                            newItemIsDuplicateItem = true;
                            break;
                        }
                    }
                    if (!newItemIsDuplicateItem)
                    {
                        this.ProductCategoriesCollection.Add(newItem);
                    }
                }

                using (var productCategory1 = new ProductCategory { Name = "---" })
                {
                    this.ProductCategoriesCollection.Insert(0, productCategory1);
                }

                this.AllMatricesCollection = new ObservableCollection<QuotationMatrixTemplate>(
                    CrmStartUp.GetAllQuotationMatrixTemplates_V2160());

                AddMatrixAcceptButtonCommand = new RelayCommand(new Action<object>(AddMatrixAcceptAction));
                AddMatrixCancelButtonCommand = new DelegateCommand<object>(CloseWindow);

                GeosApplication.Instance.Logger.Log("Constructor AddMatrixViewModel() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddMatrixViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddMatrixViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Get an error in AddMatrixViewModel() Constructor...{GeosApplication.createExceptionDetailsMsg(ex)}", category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Init()...", category: Category.Info, priority: Priority.Low);

                this.SelectedMatrix = new QuotationMatrixTemplate();
                this.Name = string.Empty;
                this.Description = string.Empty;
                this.RegionLookupValue = new LookupValue();
                this.Customer = new Customer();
                this.ProductCategory = new ProductCategory();
                this.Url = string.Empty;
                this.InUse = true;
                this.IsSave = false;
                this.IsAddNewRecord = true;
                this.IsBusy = false;
                this.WindowTitle = Application.Current.FindResource("AddMatrixViewHeader").ToString();
                GeosApplication.Instance.Logger.Log("Init() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Get an error in Init() Method...{GeosApplication.createExceptionDetailsMsg(ex)}", category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(QuotationMatrixTemplate quotationMatrixTemplate)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("EditInit()...", category: Category.Info, priority: Priority.Low);
                this.existingMatrixOpenedInEditScreen = (QuotationMatrixTemplate)quotationMatrixTemplate.Clone();
                this.SelectedMatrix = quotationMatrixTemplate;
                this.Name = quotationMatrixTemplate.Name;
                this.Description = quotationMatrixTemplate.Description;

                for (int i = 0; i < RegionLookupValuesCollection.Count; i++)
                {
                    if (RegionLookupValuesCollection[i].IdLookupValue ==
                        quotationMatrixTemplate.RegionLookupValue.IdRegion)
                    {
                        SelectedIndexRegionLookupValue = i;
                    }
                }

                for (int i = 0; i < CustomersCollection.Count; i++)
                {
                    if (CustomersCollection[i].IdCustomer ==
                        quotationMatrixTemplate.Customer.IdCustomer)
                    {
                        SelectedIndexCustomer = i;
                    }
                }

                for (int i = 0; i < ProductCategoriesCollection.Count; i++)
                {
                    if (ProductCategoriesCollection[i].IdProductCategory ==
                        quotationMatrixTemplate.ProductCategory.IdProductCategory)
                    {
                        SelectedIndexProductCategory = i;
                    }
                }
                this.Url = quotationMatrixTemplate.Url;
                this.InUse = quotationMatrixTemplate.InUse;
                this.IsSave = false;
                this.IsAddNewRecord = false;
                this.IsBusy = false;
                this.WindowTitle = Application.Current.FindResource("EditMatrixViewHeader").ToString();
                GeosApplication.Instance.Logger.Log("EditInit() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Get an error in EditInit() Method...{GeosApplication.createExceptionDetailsMsg(ex)}", category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Events

        public event EventHandler RequestClose;

        public new event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method for add new Matrix.
        /// </summary>
        /// <param name="obj"></param>
        public void AddMatrixAcceptAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddMatrixAcceptAction() ...", category: Category.Info, priority: Priority.Low);
            IsBusy = true;
            var error = EnableValidationAndGetError();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Url)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndexRegionLookupValue)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndexCustomer)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndexProductCategory)));

            if (error != null)
            {
                IsBusy = false;
                return;
            }

            try
            {
             //   GeosApplication.Instance.Logger.Log("Method AddMatrixAcceptAction()...", category: Category.Info, priority: Priority.Low);

                var matrixData = new QuotationMatrixTemplate
                {
                    Customer = this.CustomersCollection[this.SelectedIndexCustomer],
                    Description = this.Description,
                    InUse = this.InUse,
                    Name = this.Name,
                    ProductCategory = this.ProductCategoriesCollection[this.selectedIndexProductCategory],
                    RegionLookupValue = this.RegionLookupValuesCollection[this.selectedIndexRegionLookupValue],
                    Url = this.Url,

                };

                if (this.SelectedMatrix != null)
                {
                    matrixData.IdQuotationMatrixTemplate = this.SelectedMatrix.IdQuotationMatrixTemplate;
                }

                matrixData.RegionLookupValue.IdRegion = matrixData.RegionLookupValue.IdLookupValue;
                matrixData.RegionLookupValue.Region = matrixData.RegionLookupValue.Value;

                if (this.IsAddNewRecord)
                {
                    var addedQuotationMatrix = CrmStartUp.AddQuotationMatrix_V2160(matrixData);

                    if (addedQuotationMatrix != null)
                    {
                        SelectedMatrix = addedQuotationMatrix;
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("DataSavedSuccessfullyMessage").ToString()),
                            "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        IsBusy = false;
                        this.IsSave = true;
                        RequestClose?.Invoke(null, null);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("FailedToSaveDataMessage").ToString()),
                            "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    var updatedQuotationMatrix = CrmStartUp.UpdateQuotationMatrix_V2160(matrixData);
                    if (updatedQuotationMatrix)
                    {
                        SelectedMatrix = matrixData;
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("DataUpdatedSuccessfullyMessage").ToString()),
                            "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        IsBusy = false;
                        this.IsSave = true;
                        RequestClose?.Invoke(null, null);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("FailedToSaveDataMessage").ToString()),
                            "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }


                GeosApplication.Instance.Logger.Log("Method AddMatrixAcceptAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddMatrixAcceptAction() Method " +
                    ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);

                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddMatrixAcceptAction() Method - ServiceUnexceptedException " +
                    ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);

                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddMatrixAcceptAction() Method "
                    + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);

                CustomMessageBox.Show(string.Format(Application.Current.FindResource("FailedToSaveDataMessage").ToString()),
                        "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }

        private void CloseWindow(object obj)
        {
            RequestClose?.Invoke(null, null);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            var error = ((IDataErrorInfo)this).Error;
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
                var me = (IDataErrorInfo)this;
                var error =
                     me[BindableBase.GetPropertyName(() => Name)] +
                    me[BindableBase.GetPropertyName(() => Description)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexRegionLookupValue)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexCustomer)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexProductCategory)] +
                    me[BindableBase.GetPropertyName(() => Url)] +
                    me[BindableBase.GetPropertyName(() => InUse)];

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
                var NameProp = BindableBase.GetPropertyName(() => Name);
                var DescriptionProp = BindableBase.GetPropertyName(() => Description);
                var SelectedIndexRegionLookupValueProp = BindableBase.GetPropertyName(() => SelectedIndexRegionLookupValue);
                var SelectedIndexCustomerProp = BindableBase.GetPropertyName(() => SelectedIndexCustomer);
                var SelectedIndexProductCategoryProp = BindableBase.GetPropertyName(() => SelectedIndexProductCategory);
                var UrlProp = BindableBase.GetPropertyName(() => Url);
                var InUseProp = BindableBase.GetPropertyName(() => InUse);

                if (columnName == NameProp)
                {
                    var sameNameMatrix = AllMatricesCollection.FirstOrDefault(
                        x => x.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase));

                    var matrixNameAlreadyExists = 
                        (sameNameMatrix != null);

                    var theUserIsOnEditScreen = (this.existingMatrixOpenedInEditScreen != null);
                    if (matrixNameAlreadyExists)
                    {
                        //Ensure the user is not editing the same matrix
                        if (theUserIsOnEditScreen)
                        {
                            var editingCurrentName = this.existingMatrixOpenedInEditScreen.Name.Equals(
                                Name, StringComparison.InvariantCultureIgnoreCase);
                            // The user is on edit screen
                            if (!editingCurrentName)
                            {
                                return Application.Current.FindResource("TheMatrixNameAlreadyExists").ToString();
                                //"The Matrix name already exists.";
                            }
                        }
                        else
                        {
                            return Application.Current.FindResource("TheMatrixNameAlreadyExists").ToString();
                            //"The Matrix name already exists.";
                        }
                    }
                    //Do other validations like "Field is mandatory"
                    return MatrixValidation.GetErrorMessage(NameProp, Name, "Name");
                }
                    
                if (columnName == DescriptionProp)
                    return MatrixValidation.GetErrorMessage(DescriptionProp, Description, "Description");

                if (columnName == SelectedIndexRegionLookupValueProp)
                    return MatrixValidation.GetErrorMessage(
                        SelectedIndexRegionLookupValueProp,
                        SelectedIndexRegionLookupValue,
                        "Region");

                if (columnName == SelectedIndexCustomerProp)
                    return MatrixValidation.GetErrorMessage(
                        SelectedIndexCustomerProp,
                        SelectedIndexCustomer,
                        "Group");

                if (columnName == SelectedIndexProductCategoryProp)
                    return MatrixValidation.GetErrorMessage(
                        SelectedIndexProductCategoryProp,
                        SelectedIndexProductCategory,
                        "Category1");

                if (columnName == UrlProp)
                    return MatrixValidation.GetErrorMessage(UrlProp, Url, "URL");

                return null;
            }
        }

        #endregion

    }
}
