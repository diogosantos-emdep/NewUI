using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Spreadsheet;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.UI.Validations;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class PurchasingReportViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {

        #region TaskLog
        // [nsatpute][21-01-2025][GEOS2-5725]
        #endregion


        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SrmService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SrmService = new SRMServiceController("localhost:6699");
        #endregion // Services

        #region ICommands
        public ICommand SalesReportAcceptButtonCommand { get; set; }
        public ICommand SalesReportCancelButtonCommand { get; set; }
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand SelectedItemTemplateChangedCommand { get; set; }
        public ICommand OptionPopupClosedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion // Commands

        #region Declaration
        ModuleFamily selectedFamily;
        ModuleSubFamily selectedSubFamily;
        ObservableCollection<Customer> groupList;
        List<Company> plantList;


        Int16 selectedIndexGroup;
        Int16 selectedIndexPlant;

        ObservableCollection<ModuleFamily> familyList;
        ObservableCollection<ModuleSubFamily> subFamilyList;
        Int16 selectedIndexTemplate;
        Int16 selectedIndexCpType;
        Int16 selectedIndexOption;
        private DateTime fromDate;
        private DateTime toDate;
        private ObservableCollection<Company> entireCompanyPlantList;
        private ObservableCollection<Company> companyList;
        private Company selectedCompanyIndex;
        private ObservableCollection<Article> articleList;
        #endregion // Declaration

        #region Properties



        public virtual List<object> SelectedItems { get; set; }
        public ObservableCollection<Company> CompanyList
        {
            get
            {
                return companyList;
            }

            set
            {
                companyList = value;
                OnPropertyChanged("CompanyList");
            }
        }

        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set { entireCompanyPlantList = value; }
        }
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }

        public ObservableCollection<Customer> GroupList
        {
            get { return groupList; }
            set
            {
                groupList = value;
                OnPropertyChanged("GroupList");

            }
        }

        public List<Company> PlantList
        {
            get { return plantList; }
            set
            {
                plantList = value;
                OnPropertyChanged("PlantList");
            }
        }


        public short SelectedIndexGroup
        {
            get { return selectedIndexGroup; }
            set
            {
                selectedIndexGroup = value;
                OnPropertyChanged("SelectedIndexGroup");
            }
        }

        public short SelectedIndexPlant
        {
            get { return selectedIndexPlant; }
            set
            {
                selectedIndexPlant = value;
                OnPropertyChanged("SelectedIndexPlant");
            }
        }

        public ObservableCollection<Article> ArticleList
        {
            get { return articleList; }
            set
            {
                articleList = value;
                OnPropertyChanged("ArticleList");

            }
        }

        #endregion // Properties

        #region Constructor
        public PurchasingReportViewModel()
        {
            SalesReportAcceptButtonCommand = new RelayCommand(new Action<object>(SalesReportAcceptAction));
            SalesReportCancelButtonCommand = new RelayCommand(new Action<object>(SalesReportCancelButtonCommandAction));
            SelectedItemChangedCommand = new RelayCommand(new Action<object>(SelectedItemChangedAction));
            OptionPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(OptionPopupClosedCommandAction);
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
            FromDate = new DateTime(DateTime.Now.Year, 1, 1);
            ToDate = DateTime.Now.Date;
            FillCompanyPlantList();
            FillGroupList();
            FillArticleList();
            CompanyList = new ObservableCollection<Company>(CrmStartUp.GetAllCompaniesDetails_V2500(GeosApplication.Instance.ActiveUser.IdUser));

            SelectedItems = new List<object>();
            SelectedItems.Clear();
            SelectedItems = new List<object>(CompanyList);

        }

        #endregion // Constructor

        #region public Events

        public event EventHandler RequestClose;
        // Property Change Logic 
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // Public Events

        #region validation

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
                    me[BindableBase.GetPropertyName(() => FromDate)] +
                    me[BindableBase.GetPropertyName(() => ToDate)];

                //+
                //me[BindableBase.GetPropertyName(() => SelectedIndexPlant)] +
                //me[BindableBase.GetPropertyName(() => SelectedIndexGroup)] +
                //me[BindableBase.GetPropertyName(() => SelectedIndexTemplate)] +
                //me[BindableBase.GetPropertyName(() => SelectedIndexCpType)] +
                //me[BindableBase.GetPropertyName(() => SelectedIndexOption)];

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
                string fromDateProp = BindableBase.GetPropertyName(() => FromDate);
                string toDateProp = BindableBase.GetPropertyName(() => ToDate);

                //string selectedIndexGroupProp = BindableBase.GetPropertyName(() => SelectedIndexGroup);
                //string selectedIndexPlantProp = BindableBase.GetPropertyName(() => SelectedIndexPlant);
                //string selectedIndexTemplateProp = BindableBase.GetPropertyName(() => SelectedIndexTemplate);
                //string selectedIndexCpTypeProp = BindableBase.GetPropertyName(() => SelectedIndexCpType);
                //string selectedIndexOption = BindableBase.GetPropertyName(() => SelectedIndexOption);

                if (columnName == fromDateProp)
                    return ReportValidation.GetErrorMessage(fromDateProp, FromDate);
                else if (columnName == toDateProp)
                    return ReportValidation.GetErrorMessage(toDateProp, ToDate);

                //else if (columnName == selectedIndexGroupProp)
                //    return ReportValidation.GetErrorMessage(selectedIndexGroupProp, SelectedIndexGroup);
                //else if (columnName == selectedIndexPlantProp)
                //    return ReportValidation.GetErrorMessage(selectedIndexPlantProp, SelectedIndexPlant);
                //else if (columnName == selectedIndexTemplateProp)
                //    return ReportValidation.GetErrorMessage(selectedIndexTemplateProp, SelectedIndexTemplate);
                //else if (columnName == selectedIndexCpTypeProp)
                //    return ReportValidation.GetErrorMessage(selectedIndexCpTypeProp, SelectedIndexCpType);
                //else if (columnName == selectedIndexOption)
                //    return ReportValidation.GetErrorMessage(selectedIndexOption, SelectedIndexOption);

                return null;
            }
        }

        #endregion

        #region Methods

        private void OptionPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }
        }

        private void FillGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);

                IList<Customer> TempCompanyGroupList = null;
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    string salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP21"))
                    {
                        GroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP21"];
                        //SelectedIndexCompanyGroup = 0;
                    }
                    else
                    {
                        GroupList = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup_V2420(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", GroupList);
                        //SelectedIndexCompanyGroup = 0;
                    }
                }
                else
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP"))
                    {
                        GroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP"];
                        //SelectedIndexCompanyGroup = 0;
                    }
                    else
                    {
                        GroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup_V2450(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", GroupList);
                        //SelectedIndexCompanyGroup = 0;
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

        private void FillArticleList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillArticleList() ...", category: Category.Info, priority: Priority.Low);

                ArticleList = new ObservableCollection<Article>(SrmService.GetAllArticles());;

                GeosApplication.Instance.Logger.Log("Method FillArticleList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillArticleList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SelectedItemChangedAction(object obj)
        {
            if (SelectedIndexGroup > 0)
            {
                PlantList = new List<Company>();
                PlantList = EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == GroupList[SelectedIndexGroup].IdCustomer || cpl.Name == "---").ToList();
                SelectedIndexPlant = 0;
            }
            else
            {
                PlantList = new List<Company>();
                PlantList = EntireCompanyPlantList.Where(cpl => cpl.Name == "---").ToList();
                SelectedIndexPlant = 0;
            }
        }

        private void FillCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList ...", category: Category.Info, priority: Priority.Low);
                List<Company> TempPlantList = new List<Company>();
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    string salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
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
                        EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT"];
                    else
                    {
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId_V2420(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);

                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SalesReportAcceptAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ModulesReportAcceptAction ...", category: Category.Info, priority: Priority.Low);

                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("FromDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("ToDate"));

                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexGroup"));
                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexPlant"));

                if (error != null)
                    return;
                else
                {
                    GenerateSalesReport(obj);
                }

                GeosApplication.Instance.Logger.Log("Method ModulesReportAcceptAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ModulesReportAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        public void SalesReportCancelButtonCommandAction(object obj)
        {
            RequestClose(null, null);
        }       
        public void GenerateSalesReport(object obj)
        {
            try
            {


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

                try
                {


                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    GeosApplication.Instance.Logger.Log(string.Format("Get an error in GenerateModuleReport() method ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    GeosApplication.Instance.Logger.Log(string.Format("Get an error in GenerateModuleReport() Method - ServiceUnexceptedException ", ex.Message), category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    GeosApplication.Instance.Logger.Log(String.Format("Get an error in GenerateModuleReport() method {0} ", ex.Message), category: Category.Exception, priority: Priority.Low);
                }


            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        public void Dispose()
        {
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (GeosApplication.Instance.IsPermissionReadOnly)
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
