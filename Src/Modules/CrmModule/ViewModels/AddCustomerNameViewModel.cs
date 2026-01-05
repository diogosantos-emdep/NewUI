using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
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

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AddCustomerNameViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Service
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        public bool IsSave { get; set; }
        private string customerName;
        private bool isBusy;
        private List<Customer> customerGroupNameList;
        public List<Customer> CustomerGroupList = new List<Customer>();
        private List<string> customerNameStrList;

        private Visibility alertVisibility;

        private string visible;
        #endregion

        #region Properties

        public string CustomerName
        {
            get { return customerName; }
            set
            {
                customerName = value.TrimStart();
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerName"));
                ShowPopupAsPerCustomerName(customerName);
            }
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

        public List<Customer> CustomerGroupNameList
        {
            get
            {
                return customerGroupNameList;
            }

            set
            {
                customerGroupNameList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerGroupNameList"));
            }
        }

        public Visibility AlertVisibility
        {
            get
            {
                return alertVisibility;
            }

            set
            {
                alertVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AlertVisibility"));
            }
        }

        public List<string> CustomerNameStrList
        {
            get
            {
                return customerNameStrList;
            }

            set
            {
                customerNameStrList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerNameStrList"));
            }
        }
        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
        }

        #endregion

        #region ICommands

        public ICommand AddCustomerNameViewCancelButtonCommand { get; set; }
        public ICommand AddCustomerNameViewAcceptButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
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
                    me[BindableBase.GetPropertyName(() => CustomerName)];

                return CustomerAddRequiredFieldValidation.GetErrorMessage(error, CustomerName);


                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string CustomerNameProp = BindableBase.GetPropertyName(() => CustomerName);

                if (columnName == CustomerNameProp)
                    return CustomerAddRequiredFieldValidation.GetErrorMessage(CustomerNameProp, CustomerName);

                return null;
            }
        }

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

        #region Constructor

        public AddCustomerNameViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddCustomerNameViewModel ...", category: Category.Info, priority: Priority.Low);

                AddCustomerNameViewAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(AddCustomerNameAccept);
                AddCustomerNameViewCancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                AlertVisibility = Visibility.Hidden;

                //CustomerGroupList = CrmStartUp.GetAllCustomer();
                CustomerGroupList = CrmStartUp.GetAllCustomer_V2630();//chitra.girigosavi[GEOS2-7207][25/03/2025]
                CustomerGroupNameList = CustomerGroupList.ToList();

                string error = EnableValidationAndGetError();
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerName"));
                //set hide/show shortcuts on permissions
                Visible = Visibility.Visible.ToString();
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    Visible = Visibility.Hidden.ToString();
                }
                else
                {
                    Visible = Visibility.Visible.ToString();
                }
                GeosApplication.Instance.Logger.Log("Constructor AddCustomerNameViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddCustomerNameViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion

        #region Methods

        /// <summary>
        /// Method for search similar word.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        double StringSimilarityScore(string name, string searchString)
        {
            if (name.Contains(searchString))
            {
                return (double)searchString.Length / (double)name.Length;
            }

            return 0;
        }

        /// <summary>
        /// Method for search similar customer name.
        /// </summary>
        /// <param name="Name"></param>
        private void ShowPopupAsPerCustomerName(string Name)
        {
            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerCustomerName ...", category: Category.Info, priority: Priority.Low);

            CustomerGroupNameList = CustomerGroupList.ToList();

            if (CustomerGroupNameList != null && !string.IsNullOrEmpty(Name))
            {
                if (Name.Length > 1)
                {
                    CustomerGroupNameList = CustomerGroupNameList.Where(h => h.CustomerName.ToUpper().Contains(Name.ToUpper()) || h.CustomerName.ToUpper().StartsWith(Name.Substring(0, 2).ToUpper())
                                                            || h.CustomerName.ToUpper().EndsWith(Name.Substring(Name.Length - 2).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.CustomerName, Name)).ToList();
                    CustomerNameStrList = CustomerGroupNameList.Select(pn => pn.CustomerName).ToList();
                }
                else
                {
                    CustomerGroupNameList = CustomerGroupNameList.Where(h => h.CustomerName.ToUpper().Contains(Name.ToUpper()) || h.CustomerName.ToUpper().StartsWith(Name.Substring(0, 1).ToUpper())
                                                            || h.CustomerName.ToUpper().EndsWith(Name.ToUpper().Substring(Name.Length - 1).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.CustomerName, Name)).ToList();
                    CustomerNameStrList = CustomerGroupNameList.Select(pn => pn.CustomerName).ToList();
                }
            }
            else
            {
                CustomerGroupNameList = new List<Customer>();
                CustomerNameStrList = new List<string>();
            }

            if (CustomerGroupNameList.Count > 0)
            {
                AlertVisibility = Visibility.Visible;
            }
            else
            {
                AlertVisibility = Visibility.Hidden;
            }

            GeosApplication.Instance.Logger.Log("Method SalesOwnerFill() executed successfully", category: Category.Info, priority: Priority.Low);

        }

        /// <summary>
        /// Method for add customer name. 
        /// [001][cpatil][10-02-2019][GEOS2-1956]New Group FERRARI
        /// </summary>
        /// <param name="obj"></param>
        public void AddCustomerNameAccept(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" AddCustomerNameAccept() Method ...", category: Category.Info, priority: Priority.Low);

                if (!string.IsNullOrEmpty(CustomerName.Trim()))
                {
                    // [001] Changed Service method this IsExistCustomer to IsExistCustomer_V2040
                    //Service IsExistCustomer_V2040() Changed with IsExistCustomer_V2350() by [GEOS2-4120][rdixit][10.01.2022]
                    bool isExist = CrmStartUp.IsExistCustomer_V2350(CustomerName.Trim(),1);
                    if (!isExist)
                    {
                        if (!string.IsNullOrEmpty(CustomerName))
                            CustomerName = CustomerName.Trim();

                        IsSave = true;
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddCustomerNameViewCompanyExist").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }

                    RequestClose(null, null);

                }

                GeosApplication.Instance.Logger.Log("Method AddCustomerNameAccept() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCustomerNameAccept() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCustomerNameAccept() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddCustomerNameAccept() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for close window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            IsSave = false;
            CustomerName = string.Empty;
            RequestClose(null, null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
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
    }
}
