using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Spreadsheet;
using Microsoft.Win32;
using System.Data;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AccountViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        ObservableCollection<Company> companies;
        public string AccountGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "AccountGridSetting.Xml";
        bool isAccountColumnChooserVisible;
        bool isBusy;

        private string myFilterString;
        private Company selectedObject;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        private List<LookupValue> BusinessProductList { get; set; }
        private ObservableCollection<Company> entireCompanyPlantList;
        private ObservableCollection<Customer> companyGroupList;

        #endregion // Declaration

        #region  public Properties

        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set
            {
                entireCompanyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
            }
        }
        public ObservableCollection<Customer> CompanyGroupList
        {
            get { return companyGroupList; }
            set
            {
                companyGroupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyGroupList"));
            }
        }
        public List<LogEntryBySite> ChangedLogsEntries { get; set; }
        public string PreviouslySelectedSalesOwners { get; set; }
        public string PreviouslySelectedPlantOwners { get; set; }
        public IList<LookupValue> BusinessProduct { get; set; }
        public Company SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObject"));
            }
        }
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        public ObservableCollection<Company> Companies
        {
            get { return companies; }
            set
            {
                companies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Companies"));
            }
        }

        public bool IsAccountColumnChooserVisible
        {
            get { return isAccountColumnChooserVisible; }
            set
            {
                isAccountColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAccountColumnChooserVisible"));
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

        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }

        // Export Excel .xlsx
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        //public ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }

        #endregion // Properties.

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Public Commands

        public ICommand CustomCellAppearanceCommand { get; private set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand HyperlinkForWebsite { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand CommandNewAccountClick { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand SalesOwnerUnboundDataCommand { get; set; }
        public ICommand SalesOwnerCustomShowFilterPopup { get; set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand RefreshAccountViewCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand DisableAccountCommand { get; set; }

        #endregion // Commands

        #region Constructor

        public AccountViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AccountViewModel ...", category: Category.Info, priority: Priority.Low);

                CustomCellAppearanceCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(CustomCellAppearanceGridControl);

                CommandGridDoubleClick = new DelegateCommand<object>(EditAccountViewWindowShow);
                PrintButtonCommand = new Prism.Commands.DelegateCommand<object>(PrintAction);
                CommandNewAccountClick = new RelayCommand(new Action<object>(AddCustomerViewWindowShow));
                RefreshAccountViewCommand = new RelayCommand(new Action<object>(RefreshAccountDetails));
                HyperlinkForWebsite = new DelegateCommand<object>(new Action<object>((obj) => { OpenWebsite(obj); }));
                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) => { SendMailtoPerson(obj); }));
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                SalesOwnerCustomShowFilterPopup = new DelegateCommand<FilterPopupEventArgs>(SalesOwnerCustomShowFilterPopupAction);
                SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportAccountsGridButtonCommandAction));
                DisableAccountCommand = new DelegateCommand<object>(DisableAccountRowCommandAction);
                FillCmbSalesOwner();
                MyFilterString = string.Empty;

                GeosApplication.Instance.Logger.Log("Constructor AccountViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AccountViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // Constructor.

        #region Methods

        /// <summary>
        /// Method for show data as per user permission.
        /// </summary>
        private void FillCmbSalesOwner()
        {
            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                FillAccountListByUser();
            }
            else if (GeosApplication.Instance.IdUserPermission == 22)
            {
                FillAccountListByPlant();
            }
            else
            {
                FillAccountList();
            }

            BusinessProductList = CrmStartUp.GetLookupValues(7).ToList();
        }

        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        /// 
        private void SalesOwnerPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
            {
                FillAccountListByUser();
            }
            else
            {
                Companies = new ObservableCollection<Company>();
            }

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }

        /// <summary>
        /// Method for action after close plant owner popup.
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
            {
                FillAccountListByPlant();
            }
            else
            {
                Companies = new ObservableCollection<Company>();
            }

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Methdo for fill account details from database.
        /// </summary>
        private void FillAccountListByUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAccountListByUser ...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    PreviouslySelectedSalesOwners = salesOwnersIds;
                    Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersBySalesOwnerId(salesOwnersIds));
                    SalesOwnerFill();
                    FillAccountAge();

                    //fill first customer from customers list.
                    foreach (var item in Companies)
                    {
                        item.Customer = item.Customers[0];
                    }
                }
                else
                {
                    Companies = new ObservableCollection<Company>();
                }

                GeosApplication.Instance.Logger.Log("Method FillAccountListByUser executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountListByUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountListByUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
        }

        /// <summary>
        /// Method for fill account details from database by plant.
        /// </summary>
        private void FillAccountListByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAccountListByPlant ...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    //Companies = CrmStartUp.GetCustomersBySalesOwnerId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission);
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
                    Companies = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCustomersByPlant(plantOwnersIds));

                    SalesOwnerFill();
                    FillAccountAge();

                    //fill first customer from customers list.
                    foreach (var item in Companies)
                    {
                        item.Customer = item.Customers[0];
                    }
                }
                else
                {
                    Companies = new ObservableCollection<Company>();
                }
                GeosApplication.Instance.Logger.Log("Method FillAccountListByPlant executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountListByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
        }



        /// <summary>
        /// Method for fill Account Age.
        /// </summary>
        private void FillAccountAge()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAccountAge ...", category: Category.Info, priority: Priority.Low);
                int i = 0;
                if (Companies != null)
                {
                    foreach (Company age in Companies)
                    {
                        Companies[i].Age = Math.Round((double)((GeosApplication.Instance.ServerDateTime.Month - Companies[i].CreatedIn.Value.Month) + 12 * (GeosApplication.Instance.ServerDateTime.Year - Companies[i].CreatedIn.Value.Year)) / 12, 1);
                        i++;
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountAge() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountAge() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountAge() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Methdo for fill account details from database.
        /// </summary>
        private void FillAccountList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAccountList ...", category: Category.Info, priority: Priority.Low);

                Companies = new ObservableCollection<Company>(CrmStartUp.GetCustomersBySalesOwnerId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission));
                SalesOwnerFill();
                FillAccountAge();

                //fill first customer from customers list.
                foreach (var item in Companies)
                {
                    item.Customer = item.Customers[0];
                }

                GeosApplication.Instance.Logger.Log("Method FillAccountList executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAccountList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
        }

        /// <summary>
        /// Method for fill sales owner .
        /// </summary>
        private void SalesOwnerFill()
        {
            try
            {


                GeosApplication.Instance.Logger.Log("Method SalesOwnerFill ...", category: Category.Info, priority: Priority.Low);
                if (Companies != null)
                {
                    foreach (var item in Companies)
                    {
                        if (item.IdSalesResponsible != null)
                        {
                            item.People = new People();
                            PeopleDetails peopleDetails = new PeopleDetails();
                            peopleDetails = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == item.IdSalesResponsible).FirstOrDefault();
                            if(peopleDetails!=null)
                            {
                                item.People.IdPerson = peopleDetails.IdPerson;
                                item.People.Name = peopleDetails.Name;
                                item.People.Surname = peopleDetails.Surname;
                                item.People.FullName = peopleDetails.FullName;
                                item.People.Email = peopleDetails.Email;
                            }
                            
                        }

                        if (item.IdSalesResponsibleAssemblyBU != null)
                        {
                            PeopleDetails peopleDetails = new PeopleDetails();
                            item.PeopleSalesResponsibleAssemblyBU = new People();
                            peopleDetails = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == item.IdSalesResponsibleAssemblyBU).FirstOrDefault();
                            if(peopleDetails!=null)
                            {
                              
                                item.PeopleSalesResponsibleAssemblyBU.IdPerson = peopleDetails.IdPerson;
                                item.PeopleSalesResponsibleAssemblyBU.Name = peopleDetails.Name;
                                item.PeopleSalesResponsibleAssemblyBU.Surname = peopleDetails.Surname;
                                item.PeopleSalesResponsibleAssemblyBU.FullName = peopleDetails.FullName;
                                item.PeopleSalesResponsibleAssemblyBU.Email = peopleDetails.Email;
                            }
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            GeosApplication.Instance.Logger.Log("Method SalesOwnerFill() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        /// <summary>
        /// Method for open website on browser . 
        /// </summary>
        /// <param name="obj"></param>
        public void OpenWebsite(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenWebsite ...", category: Category.Info, priority: Priority.Low);

                string website = Convert.ToString(obj);
                if (!string.IsNullOrEmpty(website) && website != "-" && !website.Contains("@"))
                {
                    string[] websiteArray = website.Split(' ');
                    System.Diagnostics.Process.Start(websiteArray[0]);
                }

                GeosApplication.Instance.Logger.Log("Method OpenWebsite() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OpenWebsite() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is used to Display the Custom Filter for Sales Owner as 
        /// to display values of multiple properties in a single column.
        /// [001][2018-08-22][skhade] Solved bug in filter when testing.
        /// </summary>
        /// <param name="e">The FilterPopupEventArgs</param>
        private void SalesOwnerCustomShowFilterPopupAction(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method SalesOwnerCustomShowFilterPopupAction ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "SalesOwnerUnbound" && e.Column.FieldName != "BusinessProductString")
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();

                if (e.Column.FieldName == "SalesOwnerUnbound")
                {
                    //filterItems.Add(new CustomComboBoxItem()
                    //{
                    //    DisplayValue = "(All)",
                    //    EditValue = new CustomComboBoxItem()
                    //});
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("SalesOwnerUnbound = ' '")   // SalesOwner is equal to ' '
                    });
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("SalesOwnerUnbound <> ' '")  // SalesOwner does not equal to ' '
                    });

                    foreach (Company dataObject in Companies)
                    {
                        if (dataObject.IdSalesResponsible == null && dataObject.IdSalesResponsibleAssemblyBU == null)
                        {
                            continue;
                        }
                        else if (dataObject.IdSalesResponsible != null && dataObject.IdSalesResponsible > 0 && dataObject.IdSalesResponsibleAssemblyBU == null)
                        {
                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == dataObject.IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault().Trim()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == dataObject.IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault().Trim();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("SalesOwnerUnbound Like '%{0}%'", GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == dataObject.IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault().Trim()));
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                        else if (dataObject.IdSalesResponsible == null && dataObject.IdSalesResponsibleAssemblyBU != null && dataObject.IdSalesResponsibleAssemblyBU > 0)
                        {
                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == dataObject.IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault().Trim()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == dataObject.IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault().Trim();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("SalesOwnerUnbound Like '%{0}%'", GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == dataObject.IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault().Trim()));
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                        else if (dataObject.IdSalesResponsible != null && dataObject.IdSalesResponsibleAssemblyBU != null)
                        {
                            if (dataObject.IdSalesResponsible > 0 && !filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == dataObject.IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault().Trim()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == dataObject.IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault().Trim();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("SalesOwnerUnbound Like '%{0}%'", GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == dataObject.IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault().Trim()));
                                filterItems.Add(customComboBoxItem);
                            }
                            if (dataObject.IdSalesResponsibleAssemblyBU > 0 && !filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == dataObject.IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault().Trim()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == dataObject.IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault().Trim();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("SalesOwnerUnbound Like '%{0}%'", GeosApplication.Instance.PeopleList.Where(y => y.IdPerson == dataObject.IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault().Trim()));
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "BusinessProductString")
                {
                    //filterItems.Add(new CustomComboBoxItem()
                    //{
                    //    DisplayValue = "(All)",
                    //    EditValue = new CustomComboBoxItem()
                    //});
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("BusinessProductString = ''")   // SalesOwner is equal to ' '
                    });
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("BusinessProductString <> ''")  // SalesOwner does not equal to ' '
                    });

                    foreach (LookupValue item in BusinessProductList)
                    {
                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                        customComboBoxItem.DisplayValue = item.Value.Trim();
                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("BusinessProductString Like '%{0}%'", item.Value.Trim()));
                        filterItems.Add(customComboBoxItem);
                    }
                }

                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();

                GeosApplication.Instance.Logger.Log("Method SalesOwnerCustomShowFilterPopupAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SalesOwnerCustomShowFilterPopupAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditAccountViewWindowShow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method LeadsEditViewWindowShow...", category: Category.Info, priority: Priority.Low);
            try
            {
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

                if ((Company)detailView.FocusedRow != null)
                {
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                    //List<Company> TempCompany =  Companies.Where(cpm => cpm.IdCompany == ((Company)obj).IdCompany).ToList();
                    List<Company> TempCompany = new List<Company>();
                    int companyId = Convert.ToInt32(((Company)detailView.FocusedRow).IdCompany);
                    TempCompany.Add(CrmStartUp.GetCompanyDetailsById(companyId));

                    EditCustomerViewModel editCustomerViewModel = new EditCustomerViewModel();
                    EditCustomerView editCustomerView = new EditCustomerView();
                    editCustomerViewModel.InIt(TempCompany);
                    EventHandler handle = delegate { editCustomerView.Close(); };
                    editCustomerViewModel.RequestClose += handle;
                    editCustomerView.DataContext = editCustomerViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }

                    editCustomerView.ShowDialog();

                    if (editCustomerViewModel.CustomerData != null)
                    {
                        Company company = Companies.FirstOrDefault(cmpny => cmpny.IdCompany == companyId);

                        company.RegisteredName = editCustomerViewModel.CustomerData.RegisteredName;
                        company.Size = editCustomerViewModel.CustomerData.Size;
                        company.NumberOfEmployees = editCustomerViewModel.CustomerData.NumberOfEmployees;
                        company.Customer = editCustomerViewModel.CustomerData.Customers[0];
                        company.Customers = new List<Customer>(editCustomerViewModel.CustomerData.Customers);
                        company.IdCustomer = editCustomerViewModel.CustomerData.IdCustomer;
                        company.Name = editCustomerViewModel.CustomerData.Name;
                        company.IdBusinessCenter = editCustomerViewModel.CustomerData.IdBusinessCenter;
                        company.IdBusinessProduct = editCustomerViewModel.CustomerData.IdBusinessProduct;
                        company.IdBusinessField = editCustomerViewModel.CustomerData.IdBusinessField;
                        company.IdCountry = editCustomerViewModel.CustomerData.IdCountry;
                        company.Region = editCustomerViewModel.CustomerData.Region;
                        company.Website = editCustomerViewModel.CustomerData.Website;
                        company.Line = editCustomerViewModel.CustomerData.Line;
                        company.CuttingMachines = editCustomerViewModel.CustomerData.CuttingMachines;
                        company.Address = editCustomerViewModel.CustomerData.Address;
                        company.City = editCustomerViewModel.CustomerData.City;
                        company.ZipCode = editCustomerViewModel.CustomerData.ZipCode;
                        company.Telephone = editCustomerViewModel.CustomerData.Telephone;
                        company.Email = editCustomerViewModel.CustomerData.Email;
                        company.Fax = editCustomerViewModel.CustomerData.Fax;
                        company.BusinessField = editCustomerViewModel.CustomerData.BusinessField;
                        company.BusinessProductList = editCustomerViewModel.CustomerData.BusinessProductList;
                        company.BusinessProductString = editCustomerViewModel.CustomerData.BusinessProductString;
                        company.Latitude = editCustomerViewModel.CustomerData.Latitude;
                        company.Longitude = editCustomerViewModel.CustomerData.Longitude;
                        company.Country = editCustomerViewModel.CustomerData.Country;
                        company.BusinessCenter = editCustomerViewModel.CustomerData.BusinessCenter;
                        company.ModifiedIn = editCustomerViewModel.CustomerData.ModifiedIn;

                        //if (editCustomerViewModel.CustomerData.People != null)
                        company.People = editCustomerViewModel.CustomerData.People;

                        //if (editCustomerViewModel.CustomerData.PeopleSalesResponsibleAssemblyBU != null)
                        company.PeopleSalesResponsibleAssemblyBU = editCustomerViewModel.CustomerData.PeopleSalesResponsibleAssemblyBU;

                        // Companies.Add(company);
                        SelectedObject = company;
                        gridControl.RefreshData();

                        try
                        {
                            IList<Customer> TempCompanyGroupList = null;
                            if (GeosApplication.Instance.IdUserPermission == 21)
                            {
                                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                                {
                                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                                    CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds, true));
                                    GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYGROUP21");
                                    GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", CompanyGroupList);
                                    EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                                    GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYPLANT21");
                                    GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", EntireCompanyPlantList);
                                }
                            }
                            else
                            {
                                CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                                GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYGROUP");
                                GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", CompanyGroupList);
                                EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                                GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYPLANT");
                                GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow()" + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }

                    }

                    // code for hide column chooser if empty

                    int visibleFalseCoulumn = 0;
                    foreach (GridColumn column in gridControl.Columns)
                    {
                        if (column.Visible == false)
                        {
                            visibleFalseCoulumn++;
                        }
                    }

                    if (visibleFalseCoulumn > 0)
                    {
                        IsAccountColumnChooserVisible = true;
                    }
                    else
                    {
                        IsAccountColumnChooserVisible = false;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method EditAccountViewWindowShow() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditAccountViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditAccountViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditAccountViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for BestFit the grid and save and load Grid as per user.
        /// </summary>
        /// <param name="obj"></param>
        private void CustomCellAppearanceGridControl(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseCoulumn = 0;

                if (File.Exists(AccountGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(AccountGridSettingFilePath);
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout...
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(AccountGridSettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    }

                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }

                if (visibleFalseCoulumn > 0)
                {
                    IsAccountColumnChooserVisible = true;
                }
                else
                {
                    IsAccountColumnChooserVisible = false;
                }

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                datailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method CustomCellAppearanceGridControl() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomCellAppearanceGridControl() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for remove filter save on grid layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            if (e.DependencyProperty == GridControl.FilterStringProperty)
                e.Allow = false;

            if (e.Property.Name == "GroupCount")
                e.Allow = false;

            if (e.DependencyProperty == TableView.SearchStringProperty)
                e.Allow = false;
        }

        /// <summary>
        /// Method for save Grid as per user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(AccountGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsAccountColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for save Grid as per user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(AccountGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for open MailTo in Outlook for send Email. 
        /// </summary>
        /// <param name="obj"></param>
        public void SendMailtoPerson(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);

                string emailAddess = Convert.ToString(obj);

                string command = "mailto:" + emailAddess;
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo.FileName = command;
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();

                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for print Contact grid.
        /// </summary>
        /// <param name="obj"></param>
        public void PrintAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintAction ...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["AccountViewCustomPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["AccountViewCustomPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                GeosApplication.Instance.Logger.Log("Method PrintAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for add new customer.
        /// </summary>
        /// <param name="obj"></param>
        private void AddCustomerViewWindowShow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddCustomerViewWindowShow...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    // DXSplashScreen.Show<SplashScreenView>(); 
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


                AddCustomerViewModel addCustomerViewModel = new AddCustomerViewModel();
                AddCustomerView addCustomerView = new AddCustomerView();
                EventHandler handle = delegate { addCustomerView.Close(); };
                addCustomerViewModel.RequestClose += handle;
                addCustomerView.DataContext = addCustomerViewModel;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                //IsAccountColumnChooserVisible = false;
                addCustomerView.ShowDialog();

                if (addCustomerViewModel.CustomerData != null)
                {
                    Company company = new Company();
                    company.IdCompany = addCustomerViewModel.CustomerData.IdCompany;
                    company.Customer = addCustomerViewModel.CustomerData.Customers[0];
                    company.Customers = new List<Customer>(addCustomerViewModel.CustomerData.Customers);
                    company.Country = new Country();
                    company.Country = addCustomerViewModel.CustomerData.Country;


                    company.CIF = addCustomerViewModel.CustomerData.CIF;

                    company.RegisteredName = addCustomerViewModel.CustomerData.RegisteredName;
                    company.Size = addCustomerViewModel.CustomerData.Size;
                    company.NumberOfEmployees = addCustomerViewModel.CustomerData.NumberOfEmployees;

                    company.IdCustomer = addCustomerViewModel.CustomerData.IdCustomer;
                    //CustomerData.IdCompany = Convert.ToInt32(CompanyPlantList[SelectedIndexCompanyPlant].IdCompany);
                    company.Name = addCustomerViewModel.CustomerData.Name;
                    company.IdBusinessCenter = addCustomerViewModel.CustomerData.IdBusinessCenter;
                    company.IdBusinessProduct = addCustomerViewModel.CustomerData.IdBusinessProduct;
                    company.IdBusinessField = addCustomerViewModel.CustomerData.IdBusinessField;
                    company.IdCountry = addCustomerViewModel.CustomerData.IdCountry;
                    company.Region = addCustomerViewModel.CustomerData.Region;
                    company.Website = addCustomerViewModel.CustomerData.Website;
                    company.Line = addCustomerViewModel.CustomerData.Line;
                    company.CuttingMachines = addCustomerViewModel.CustomerData.CuttingMachines;
                    company.Address = addCustomerViewModel.CustomerData.Address;
                    company.City = addCustomerViewModel.CustomerData.City;
                    company.ZipCode = addCustomerViewModel.CustomerData.ZipCode;
                    company.Telephone = addCustomerViewModel.CustomerData.Telephone;
                    company.Email = addCustomerViewModel.CustomerData.Email;
                    company.Fax = addCustomerViewModel.CustomerData.Fax;
                    company.BusinessProductString = addCustomerViewModel.CustomerData.BusinessProductString;
                    company.BusinessProductList = addCustomerViewModel.CustomerData.BusinessProductList;
                    company.BusinessField = addCustomerViewModel.CustomerData.BusinessField;
                    company.Latitude = addCustomerViewModel.CustomerData.Latitude;
                    company.Longitude = addCustomerViewModel.CustomerData.Longitude;
                    company.BusinessCenter = addCustomerViewModel.CustomerData.BusinessCenter;
                    company.CreatedIn = addCustomerViewModel.CustomerData.CreatedIn;
                    company.PeopleCreatedBy = new People();
                    company.PeopleCreatedBy.Name = addCustomerViewModel.CustomerData.PeopleCreatedBy.Name;
                    company.PeopleCreatedBy.Surname = addCustomerViewModel.CustomerData.PeopleCreatedBy.Surname;
                    if (addCustomerViewModel.CustomerData.People != null)
                        company.People = addCustomerViewModel.CustomerData.People;

                    Companies.Add(company);
                    SelectedObject = company;

                    // code for hide column chooser if empty
                    TableView detailView = ((TableView)obj);
                    GridControl gridControl = (detailView).Grid;
                    int visibleFalseCoulumn = 0;

                    foreach (GridColumn column in gridControl.Columns)
                    {
                        if (column.Visible == false)
                        {
                            visibleFalseCoulumn++;
                        }
                    }

                    if (visibleFalseCoulumn > 0)
                    {
                        IsAccountColumnChooserVisible = true;
                    }
                    else
                    {
                        IsAccountColumnChooserVisible = false;
                    }

                    detailView.Focus();

                    try
                    {
                        IList<Customer> TempCompanyGroupList = null;
                        if (GeosApplication.Instance.IdUserPermission == 21)
                        {
                            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                            {
                                List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                                var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));


                                CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds, true));
                                GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYGROUP21");
                                GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", CompanyGroupList);
                                EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                                GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYPLANT21");
                                GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", EntireCompanyPlantList);
                            }
                        }
                        else
                        {

                            CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                            GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYGROUP");
                            GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", CompanyGroupList);
                            EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                            GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYPLANT");
                            GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Method AddCustomerViewWindowShow() executed successfully" + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }

                }

                GeosApplication.Instance.Logger.Log("Method AddCustomerViewWindowShow() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCustomerViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCustomerViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCustomerViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        private void DisableAccountRowCommandAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DisableAccountRowCommandAction() ...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
                bool result = false;
                Company ObjCustomer = (Company)parameter;

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["AccountDisableMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (Companies != null && Companies.Count > 0)
                    {
                        ObjCustomer.IsStillActive = 0;

                        if (ObjCustomer.People != null)
                        {
                            ObjCustomer.People.OwnerImage = null;
                        }

                        if (ObjCustomer.PeopleSalesResponsibleAssemblyBU != null)
                        {
                            ObjCustomer.PeopleSalesResponsibleAssemblyBU.OwnerImage = null;
                        }

                        ChangedLogsEntries = new List<LogEntryBySite>();
                        ChangedLogsEntries.Add(new LogEntryBySite() { IdSite = ObjCustomer.IdCompany, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("AccountDisableChangeLog").ToString(), ObjCustomer.GroupPlantName), IdLogEntryBySite = 2 });
                        ObjCustomer.LogEntryBySites = ChangedLogsEntries;
                        result = CrmStartUp.DisableAccount(ObjCustomer);
                        Companies.Remove((Company)ObjCustomer);
                    }

                    if (result)
                    {
                        try
                        {
                            IList<Customer> TempCompanyGroupList = null;
                            if (GeosApplication.Instance.IdUserPermission == 21)
                            {
                                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                                {
                                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));


                                    CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds, true));
                                    GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYGROUP21");
                                    GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", CompanyGroupList);
                                    EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                                    GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYPLANT21");
                                    GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", EntireCompanyPlantList);
                                }
                            }
                            else
                            {

                                CompanyGroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                                GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYGROUP");
                                GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", CompanyGroupList);
                                EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                                GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYPLANT");
                                GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        IsBusy = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AccountDisableSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        GeosApplication.Instance.Logger.Log("Method DisableAccountRowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                    else
                    {
                        IsBusy = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AccountDisableFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    IsBusy = false;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DisableAccountRowCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DisableAccountRowCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DisableAccountRowCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for refresh Account details.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshAccountDetails(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RefreshAccountDetails...", category: Category.Info, priority: Priority.Low);

            TableView detailView = (TableView)obj;
            GridControl gridControl = (detailView).Grid;

            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

            MyFilterString = string.Empty;
            FillCmbSalesOwner();

            // code for hide column chooser if empty

            int visibleFalseCoulumn = 0;
            foreach (GridColumn column in gridControl.Columns)
            {
                if (column.Visible == false)
                {
                    visibleFalseCoulumn++;
                }
            }

            if (visibleFalseCoulumn > 0)
            {
                IsAccountColumnChooserVisible = true;
            }
            else
            {
                IsAccountColumnChooserVisible = false;
            }

            detailView.SearchString = null;
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method RefreshAccountDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method convert BitmapImage to Image Source
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        private void ExportAccountsGridButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportAccountsGridButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Accounts";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (Boolean)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    IsBusy = true;
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                    ResultFileName = (saveFile.FileName);

                    TableView accountTableView = ((TableView)obj);
                    accountTableView.ShowTotalSummary = false;
                    accountTableView.ShowFixedTotalSummary = false;

                    accountTableView.ExportToXlsx(ResultFileName);

                    IsBusy = false;
                    accountTableView.ShowTotalSummary = true;
                    accountTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportAccountsGridButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportAccountsGridButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion // Methods.

    }

}
