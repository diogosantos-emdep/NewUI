using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System.Windows.Input;
using DevExpress.Mvvm.POCO;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Printing;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using DevExpress.XtraReports.UI;
using Emdep.Geos.UI.Commands;
using Microsoft.Win32;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using DevExpress.Xpf.LayoutControl;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.Helper;
using System.Text.RegularExpressions;
using Emdep.Geos.Data.Common.Hrm;
using System.Globalization;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Validations;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EditMealAllowanceViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {
        #region TaskComments
        //[GEOS2-4181][20.03.2023][rdixit]
        #endregion

        #region Services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return GetService<INavigationService>(); } }
        #endregion // End Services

        #region Command
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptActionCommand { get; set; }

        #endregion

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public event EventHandler RequestClose;
        #endregion // Events

        #region Declaration
        bool isUpdated;
        string windowHeader;
        ObservableCollection<Currency> currencyList;
        Currency selectedCurrency;
        string selectedCompany;
        EmployeeMealBudget selectedGridRow;
        ObservableCollection<EmployeeMealBudget> mealAllowanceList;
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
        public EmployeeMealBudget SelectedGridRow
        {
            get
            {
                return selectedGridRow;
            }

            set
            {
                selectedGridRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGridRow"));
            }
        }
        
         public ObservableCollection<EmployeeMealBudget> MealAllowanceList
        {
            get
            {
                return mealAllowanceList;
            }

            set
            {
                mealAllowanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MealAllowanceList"));
            }
        }
        public ObservableCollection<Currency> CurrencyList
        {
            get
            {
                return currencyList;
            }

            set
            {
                currencyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyList"));
            }
        }
        public Currency SelectedCurrency
        {
            get
            {
                return selectedCurrency;
            }

            set
            {
                selectedCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrency"));
            }
        }
        public string SelectedCompany
        {
            get
            {
                return selectedCompany;
            }

            set
            {
                selectedCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompany"));
            }
        }
        public bool IsUpdated
        {
            get
            {
                return isUpdated;
            }

            set
            {
                isUpdated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUpdated"));
            }
        }
        #endregion

        #region Constructor
        public EditMealAllowanceViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor EditMealAllowanceViewModel()...", category: Category.Info, priority: Priority.Low);
            CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
            AcceptActionCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
            GeosApplication.Instance.Logger.Log("Constructor EditMealAllowanceViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        }
        #endregion

        #region Methods
        public void EditInit(MealAllowance selectedCompanyMeal)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
        
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GetCurrencies();
                //[pramod.misal][GEOS2-5365][12.03.2024]
                MealAllowanceList = new ObservableCollection<EmployeeMealBudget>();
                if (selectedCompanyMeal.GlobalEmp == null)
                {
                    selectedCompanyMeal.GlobalEmp = new EmployeeMealBudget
                    {
                        EmployeeProfile = "Global",
                        IdEmployeeProfile = 1626,
                        IdCompany = selectedCompanyMeal.IdCompany,
                        //IdCompanyEmployeeMealBudget= selectedCompanyMeal.IdCompanyEmployeeMealBudget
                        IdCurrency= selectedCompanyMeal.IdOfficialCurrency

                    };
                }

                if (selectedCompanyMeal.RegularEmp == null)
                {
                    selectedCompanyMeal.RegularEmp = new EmployeeMealBudget
                    {
                        EmployeeProfile = "Regular",
                        IdEmployeeProfile = 1625,
                        IdCompany = selectedCompanyMeal.IdCompany,
                        //IdCompanyEmployeeMealBudget = selectedCompanyMeal.IdCompanyEmployeeMealBudget
                        IdCurrency = selectedCompanyMeal.IdOfficialCurrency
                    };
                }

                MealAllowanceList.Add(selectedCompanyMeal.GlobalEmp);
                MealAllowanceList.Add(selectedCompanyMeal.RegularEmp);
                SelectedCurrency = CurrencyList.FirstOrDefault(i=>i.IdCurrency == MealAllowanceList.FirstOrDefault().IdCurrency);
                SelectedCompany = selectedCompanyMeal.CompanyAlias;
                
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditInit() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CancelButtonCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
            RequestClose(null, null);
            
        }
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                MealAllowanceList.ToList().ForEach(i => i.IdCurrency = SelectedCurrency.IdCurrency);
                MealAllowanceList.ToList().ForEach(i => i.CurrencySymbol = SelectedCurrency.Symbol);
                
                #region Service commented for GEOS2-5365
                //IsUpdated = HrmService.UpdateMealAllowance(MealAllowanceList.ToList());
                #endregion

                #region updated Service for GEOS2-5365
                //[pramod.misal][GEOS2-5365][12.03.2024]
                //HrmService = new HrmServiceController("localhost:6699");
                IsUpdated = HrmService.UpdateMealAllowance_V2500(MealAllowanceList.ToList());
                #endregion
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetCurrencies()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetCurrencies()...", category: Category.Info, priority: Priority.Low);
               // HrmService = new HrmServiceController("localhost:6699");
                CurrencyList = new ObservableCollection<Currency>(HrmService.GetCurrencies_V2360());                                               
                GeosApplication.Instance.Logger.Log("Method GetCurrencies()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetCurrencies() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetCurrencies() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetCurrencies() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
