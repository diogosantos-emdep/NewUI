using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Spreadsheet;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class OrderViewModule : NavigationViewModelBase, INotifyPropertyChanged
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        private DataTable dttable;
        private DataTable dttableCopy;
        private DataTable dataTable;
        private float probabilityOfSuccess = .0F;
        private IList<Offer> orderList;
        private List<OptionsByOffer> orderOptionsByOfferList;
        private IList<Template> templates;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        string myFilterString;
        private DateTime startSelectionDate;
        private DateTime finishSelectionDate;

        private bool isBusy;

        private Visibility cmbSalesOwnerUsers;
        private Visibility cmbPlantOwnerUsers;
        private bool isViewRangeControl;
        private string gridRowHeight;
        private bool isOrderColumnChooserVisible;


        #endregion // Declaration

        #region Public Properties

        public IList<Offer> OrderList
        {
            get { return orderList; }
            set { orderList = value; OnPropertyChanged(new PropertyChangedEventArgs("OrderList")); }
        }

        public List<OptionsByOffer> OrderOptionsByOfferList
        {
            get { return orderOptionsByOfferList; }
            set { orderOptionsByOfferList = value; }
        }

        public DataTable DataTable
        {
            get { return dataTable; }
            set { dataTable = value; }
        }

        public float ProbabilityOfSuccess
        {
            get { return probabilityOfSuccess; }
            set { probabilityOfSuccess = value; }
        }

        public DataTable Dttable
        {
            get { return dttable; }
            set
            {
                dttable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Dttable"));
            }
        }

        public DataTable DttableCopy
        {
            get { return dttableCopy; }
            set
            {
                dttableCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DttableCopy"));
            }
        }

        public IList<Template> Templates
        {
            get { return templates; }
            set { templates = value; }
        }

        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
                //RaisePropertyChanged("MyFilterString");
            }
        }

        public DateTime FinishSelectionDate
        {
            get { return finishSelectionDate; }
            set
            {
                finishSelectionDate = value;
                UpdateFilterString();
                OnPropertyChanged(new PropertyChangedEventArgs("FinishSelectionDate"));
                //RaisePropertyChanged("FinishSelectionDate");
            }
        }

        public DateTime StartSelectionDate
        {
            get { return startSelectionDate; }
            set
            {
                startSelectionDate = value;
                UpdateFilterString();
                OnPropertyChanged(new PropertyChangedEventArgs("StartSelectionDate"));
                //RaisePropertyChanged("StartSelectionDate");
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
                //RaisePropertyChanged("IsBusy");
            }
        }

        public Visibility CmbPlantOwnerUsers
        {
            get { return cmbPlantOwnerUsers; }
            set { cmbPlantOwnerUsers = value; }
        }

        public Visibility CmbSalesOwnerUsers
        {
            get { return cmbSalesOwnerUsers; }
            set { cmbSalesOwnerUsers = value; }
        }


        public bool IsViewRangeControl
        {
            get { return isViewRangeControl; }
            set
            {
                isViewRangeControl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsViewRangeControl"));
            }
        }

        public string GridRowHeight
        {
            get { return gridRowHeight; }
            set
            {
                gridRowHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridRowHeight"));
            }
        }

        List<OfferOption> offerOptions;

        public List<OfferOption> OfferOptions
        {
            get { return offerOptions; }
            set { offerOptions = value; }
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

        public bool IsOrderColumnChooserVisible
        {
            get
            {
                return isOrderColumnChooserVisible;
            }

            set
            {
                isOrderColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOrderColumnChooserVisible"));
            }
        }

        // Export Excel .xlsx
        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }

        #endregion // Properties

        #region Events

        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ContentChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Commands

        public ICommand PrintButtonCommand { get; set; }
        public ICommand CustomCellAppearanceCommand { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand ViewHideRangeControlCommand { get; set; }
        public ICommand RefreshOrderViewCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }

        

        #endregion // Commands

        #region Constructor

        public OrderViewModule()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OrderViewModule ...", category: Category.Info, priority: Priority.Low);

                CustomCellAppearanceCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(CustomCellAppearanceGridControl);
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintLeadsGrid));
                CommandGridDoubleClick = new DelegateCommand<object>(LeadsEditViewWindowShow);
                ViewHideRangeControlCommand = new DelegateCommand<object>(ViewHideRangeControl);
                SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                RefreshOrderViewCommand = new RelayCommand(new Action<object>(RefreshOrderDetails));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportLeadsGridButtonCommandAction));

                IsViewRangeControl = true;
                GridRowHeight = "100";

                //Fillgrid();

                FillCmbSalesOwner();

                GeosApplication.Instance.Logger.Log("Method OrderViewModule() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OrderViewModule() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        #endregion // Constructor

        #region Methods

        private void FillCmbSalesOwner()
        {
            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                CmbSalesOwnerUsers = Visibility.Visible;
                CmbPlantOwnerUsers = Visibility.Hidden;
                // Called for 1st Time
                FillOrderGridDetailsByUsers();
                IsOrderColumnChooserVisible = false;
            }
            else if (GeosApplication.Instance.IdUserPermission == 22)
            {
                CmbSalesOwnerUsers = Visibility.Hidden;
                CmbPlantOwnerUsers = Visibility.Visible;
                // Called for 1st Time
                FillOrderGridDetailsByPlant();

                if(GeosApplication.Instance.CrmOfferYear >= 2017)
                IsOrderColumnChooserVisible = true;
            }
            else
            {
                CmbSalesOwnerUsers = Visibility.Hidden;
                CmbPlantOwnerUsers = Visibility.Hidden;
                FillOrderGridDetails();
                IsOrderColumnChooserVisible = false;
            }
        }

        /// <summary>
        /// Method for refresh Order Grid details.
        /// </summary>
        private void FillOrderGridDetailsByPlant()
        {
            GeosApplication.Instance.Logger.Log("Method FillOrderGrid ...", category: Category.Info, priority: Priority.Low);

            GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
            GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

            try
            {
                Fillgrid();
                FillOrderPlants();
                FillDataTable();
                //Fillgrid();
                GeosApplication.Instance.Logger.Log("Method FillOrderGrid() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOrderGrid() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for refresh Order Grid details.
        /// </summary>
        private void FillOrderGridDetails()
        {
            GeosApplication.Instance.Logger.Log("Method FillOrderGrid ...", category: Category.Info, priority: Priority.Low);

            GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
            GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

            try
            {
                Fillgrid();
                FillOrder();
                FillDataTable();
                //Fillgrid();
                GeosApplication.Instance.Logger.Log("Method FillOrderGrid() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillOrderGrid() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void FillOrderGridDetailsByUsers()
        {
            GeosApplication.Instance.Logger.Log("Method FillOrderGridDetailsByUsers ...", category: Category.Info, priority: Priority.Low);

            GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
            GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

            try
            {
                if (!DXSplashScreen.IsActive)
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
                Fillgrid();
                FillOrderByUsers();
                FillDataTable();

                //Fillgrid();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillOrderGridDetailsByUsers() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOrderGridDetailsByUsers() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method FillOrderGridDetailsByUsers() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void FillOrderByUsers()
        {
            GeosApplication.Instance.Logger.Log("Method FillOrderByUsers ...", category: Category.Info, priority: Priority.Low);

            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
            {
                List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                OrderList = new List<Offer>();
                OrderOptionsByOfferList = new List<OptionsByOffer>();
                List<Offer> offerOrderList = new List<Offer>();
                List<OptionsByOffer> OptionsByOfferOrder = new List<OptionsByOffer>();

                OffersOptionsList offersOptionsLst = new OffersOptionsList();

                // Continue loop although some plant is not available and Show error message.
                foreach (var item in GeosApplication.Instance.CompanyList)
                {
                    try
                    {
                        GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.Alias;
                        //offerOrderList.
                        //OptionsByOfferOrder.

                        offersOptionsLst = CrmStartUp.GetSelectedUsersOrdersDatatable(
                                                   GeosApplication.Instance.IdCurrencyByRegion,
                                                   salesOwnersIds,
                                                   GeosApplication.Instance.CrmOfferYear,
                                                   item);

                        offerOrderList.AddRange(offersOptionsLst.Offers);
                        OptionsByOfferOrder.AddRange(offersOptionsLst.OptionsByOffers);

                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Info, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
                    }
                }

                GeosApplication.Instance.SplashScreenMessage = "";
                OrderList = offerOrderList;
                OrderOptionsByOfferList = OptionsByOfferOrder;
                //var kksdj = OrderOptionsByOfferList.Where(oob => oob.IdOffer == 13034).FirstOrDefault();
            }
        }

        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }
            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
            {
                if (!DXSplashScreen.IsActive)
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

                FillOrderGridDetailsByPlant();

                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
            }
            else
            {
                OrderList = new List<Offer>();
                OrderOptionsByOfferList = new List<OptionsByOffer>();
                Dttable.Rows.Clear();
            }
        }

        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void SalesOwnerPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }
            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
            {
                FillOrderGridDetailsByUsers();
            }
            else
            {
                OrderList = new List<Offer>();
                OrderOptionsByOfferList = new List<OptionsByOffer>();
                Dttable.Rows.Clear();
            }
        }

        /// <summary>
        /// Method for refresh Order Grid From database by refresh Button.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshOrderDetails(object obj)
        {
            if (!DXSplashScreen.IsActive)
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
            MyFilterString = string.Empty;
            FillCmbSalesOwner();

            if (DXSplashScreen.IsActive)
            {
                DXSplashScreen.Close();
            }
        }

        private void LeadsEditViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LeadsEditViewWindowShow...", category: Category.Info, priority: Priority.Low);

                if ((System.Data.DataRowView)obj != null)
                {
                    if (!DXSplashScreen.IsActive)
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

                    long idOffer = Convert.ToInt64(((System.Data.DataRowView)obj).Row.ItemArray[1].ToString());
                    Int32 ConnectPlantId = Convert.ToInt32(((System.Data.DataRowView)obj).Row.ItemArray[0].ToString());
                    IList<Offer> TempLeadsList = new List<Offer>();

                    TempLeadsList.Add(CrmStartUp.GetOfferDetailsById(idOffer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.CrmOfferYear, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ConnectPlantId.ToString()).FirstOrDefault()));
                    // TempLeadsList[0].DeliveryDate = Convert.ToDateTime(((System.Data.DataRowView)obj).Row.ItemArray[11].ToString());
                    LostReasonsByOffer lostReasonsByOffer = CrmStartUp.GetLostReasonsByOffer(Convert.ToInt64(TempLeadsList[0].IdOffer), GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ConnectPlantId.ToString()).Select(x => x.ConnectPlantConstr).FirstOrDefault());

                    if (lostReasonsByOffer != null)
                    {
                        TempLeadsList[0].LostReasonsByOffer = lostReasonsByOffer;
                    }

                    LeadsEditViewModel leadsEditViewModel = new LeadsEditViewModel();
                    LeadsEditView leadsEditView = new LeadsEditView();
                    leadsEditViewModel.IsControlEnable = true;
                    leadsEditViewModel.IsControlEnableorder = false;
                    leadsEditViewModel.IsStatusChangeAction = true;
                    leadsEditViewModel.IsAcceptControlEnableorder = false;

                    leadsEditViewModel.InItLeadsEditReadonly(TempLeadsList);
                    //leadsEditViewModel.InIt(TempLeadsList);

                    EventHandler handle = delegate { leadsEditView.Close(); };
                    leadsEditViewModel.RequestClose += handle;
                    leadsEditView.DataContext = leadsEditViewModel;

                    if (DXSplashScreen.IsActive)
                    {
                        DXSplashScreen.Close();
                    }

                    IsOrderColumnChooserVisible = false;

                    leadsEditView.ShowDialogWindow();

                    if (GeosApplication.Instance.IdUserPermission == 22 && GeosApplication.Instance.CrmOfferYear >= 2017)
                    {
                        IsOrderColumnChooserVisible = true;
                    }
                    else
                    {
                        IsOrderColumnChooserVisible = false;
                    }
                    
                }

                GeosApplication.Instance.Logger.Log("Method LeadsEditViewWindowShow executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in LeadsEditViewWindowShow() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void CustomCellAppearanceGridControl(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomCellAppearancePivotgrid ...", category: Category.Info, priority: Priority.Low);

                TableView detailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                detailView.BestFitColumns();

                GeosApplication.Instance.Logger.Log("Method CustomCellAppearancePivotgrid() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomCellAppearancePivotgrid() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        public void UpdateFilterString()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateFilterString ...", category: Category.Info, priority: Priority.Low);

                if (StartSelectionDate > new DateTime(2000, 1, 1) && FinishSelectionDate > new DateTime(2000, 1, 1))
                {
                    string st = string.Format("[PODate] >= #{0}# And [PODate] < #{1}#", StartSelectionDate.ToString("MM-dd-yyyy"), FinishSelectionDate.ToString("MM-dd-yyyy"));
                    MyFilterString = st;
                }

                GeosApplication.Instance.Logger.Log("Method UpdateFilterString() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in UpdateFilterString() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void FillOrder()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOrder ...", category: Category.Info, priority: Priority.Low);

                OrderList = new List<Offer>();
                OrderOptionsByOfferList = new List<OptionsByOffer>();
                List<Offer> offerOrderList = new List<Offer>();
                List<OptionsByOffer> OptionsByOfferOrder = new List<OptionsByOffer>();

                OffersOptionsList offersOptionsLst = new OffersOptionsList();

                // Continue loop although some plant is not available and Show error message.
                foreach (var item in GeosApplication.Instance.CompanyList)
                {
                    try
                    {
                        GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.Alias;

                        offersOptionsLst = CrmStartUp.GetOrdersDatatable(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.CrmOfferYear, item, GeosApplication.Instance.IdUserPermission);

                        offerOrderList.AddRange(offersOptionsLst.Offers);
                        OptionsByOfferOrder.AddRange(offersOptionsLst.OptionsByOffers);

                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Info, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
                    }
                }

                OrderList = offerOrderList;
                OrderOptionsByOfferList = OptionsByOfferOrder;

                GeosApplication.Instance.Logger.Log("Method FillOrder() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                    GeosApplication.Instance.SplashScreenMessage = "";
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillOrder() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                    GeosApplication.Instance.SplashScreenMessage = "";
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillOrder() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            GeosApplication.Instance.SplashScreenMessage = "";
        }

        private void FillOrderPlants()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOrder ...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ShortName));


                    OrderList = new List<Offer>();
                    OrderOptionsByOfferList = new List<OptionsByOffer>();
                    List<Offer> offerOrderList = new List<Offer>();
                    List<OptionsByOffer> OptionsByOfferOrder = new List<OptionsByOffer>();

                    OffersOptionsList offersOptionsLst = new OffersOptionsList();

                    // Continue loop although some plant is not available and Show error message.
                    foreach (var item in plantOwners)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.ShortName;

                            offersOptionsLst = CrmStartUp.GetOrdersDatatable(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.CrmOfferYear, item, GeosApplication.Instance.IdUserPermission);

                            offerOrderList.AddRange(offersOptionsLst.Offers);
                            OptionsByOfferOrder.AddRange(offersOptionsLst.OptionsByOffers);

                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.ShortName, " Failed");
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ShortName, " Failed ", ex.Detail.ErrorMessage), category: Category.Info, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.ShortName, " Failed");
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ShortName, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.ShortName, " Failed");
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ShortName, " Failed ", ex.Message), category: Category.Info, priority: Priority.Low);
                        }
                    }

                    OrderList = offerOrderList;
                    OrderOptionsByOfferList = OptionsByOfferOrder;

                    GeosApplication.Instance.Logger.Log("Method FillOrder() executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                    GeosApplication.Instance.SplashScreenMessage = "";
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillOrder() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                    GeosApplication.Instance.SplashScreenMessage = "";
                }
                GeosApplication.Instance.Logger.Log("Get an error in FillOrder() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            GeosApplication.Instance.SplashScreenMessage = "";
        }

        private void Fillgrid()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Fillgrid ...", category: Category.Info, priority: Priority.Low);

                Columns = new ObservableCollection<Emdep.Geos.UI.Helper.Column>() {
                    
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "IdOffer", HeaderText = "IdOffer", Settings = SettingsType.Default, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = false, IsVertical = false,FixedWidth=true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "ConnectPlantId", HeaderText = "ConnectPlantId", Settings = SettingsType.Default, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = false, IsVertical = false ,FixedWidth=true },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Code", HeaderText = "Code", Settings = SettingsType.Default, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = true, IsVertical = false ,FixedWidth=true },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Group", HeaderText = "Group", Settings = SettingsType.Default, AllowCellMerge = false, Width = 100, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=true },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Plant", HeaderText = "Plant", Settings = SettingsType.Default, AllowCellMerge = false, Width = 150, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=true },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Project",HeaderText="Project", Settings = SettingsType.OthersFields, AllowCellMerge=false, Width=120,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Country", HeaderText = "Country", Settings = SettingsType.Default, AllowCellMerge = false, Width = 100, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=true },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Region", HeaderText = "Region", Settings = SettingsType.Default, AllowCellMerge = false, Width = 100, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=true },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Description", HeaderText = "Description", Settings = SettingsType.Default, AllowCellMerge = false, Width = 250, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=true },

                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Status", HeaderText = "Status", Settings = SettingsType.Status, AllowCellMerge = false, Width = 200, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=true },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "HtmlColor", HeaderText = "HtmlColor", Settings = SettingsType.Default, AllowCellMerge = false, Width = 200, AllowEditing = false, Visible = false, IsVertical = false,FixedWidth=true },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "Amount", HeaderText = "Amount", Settings = SettingsType.Amount, AllowCellMerge = false, Width = 170, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=true },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "InvoiceAmount", HeaderText = "Invoice Amount", Settings = SettingsType.Amount, AllowCellMerge = false, Width = 170, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=true },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "DeliveryDate", HeaderText = "DeliveryDate", Settings = SettingsType.Default, AllowCellMerge = false, Width = 90, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=true },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "PODate", HeaderText = "PODate", Settings = SettingsType.Default, AllowCellMerge = false, Width = 90, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=true },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "SalesOwner", HeaderText = "Sales Owner", Settings = SettingsType.SalesOwner, AllowCellMerge = false, Width = 150, AllowEditing = false, Visible = true, IsVertical = false,FixedWidth=true },
                    new Emdep.Geos.UI.Helper.Column() { FieldName = "IsSaleOwnerNull", HeaderText = "IsSaleOwnerNull", Settings = SettingsType.Default, AllowCellMerge=false,Width=10,AllowEditing=false,Visible=false,IsVertical= false,FixedWidth=true },
                    

                };

                if (GeosApplication.Instance.IdUserPermission == 22 && GeosApplication.Instance.CrmOfferYear >= 2017 )
                {
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "OEM", HeaderText = "OEM", Settings = SettingsType.OthersFields, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = true });
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "Source", HeaderText = "Source", Settings = SettingsType.OthersFields, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = true });
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "BusinessUnit", HeaderText = "Business Unit", Settings = SettingsType.OthersFields, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = true });
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "RFQReceptionDate", HeaderText = "RFQ Reception Date", Settings = SettingsType.RFQDate, AllowCellMerge = false, Width = 90, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = true });
                    Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "QuoteSentDate", HeaderText = "Quote Sent Date", Settings = SettingsType.QuoteSentDate, AllowCellMerge = false, Width = 90, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = true });
                }

                //new Column() { FieldName = "Site", HeaderText = "Site", Settings = SettingsType.Default, AllowCellMerge = true, Width = 50, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = true },

                Dttable = new DataTable();
                Dttable.Columns.Add("ConnectPlantId", typeof(Int32));
                Dttable.Columns.Add("IdOffer", typeof(long));
                Dttable.Columns.Add("Code", typeof(string));
                Dttable.Columns.Add("Country", typeof(string));
                Dttable.Columns.Add("Region", typeof(string));
                Dttable.Columns.Add("Group", typeof(string));
                Dttable.Columns.Add("Plant", typeof(string));
                Dttable.Columns.Add("Site", typeof(string));
                Dttable.Columns.Add("Description", typeof(string));
                Dttable.Columns.Add("Status", typeof(string));
                Dttable.Columns.Add("HtmlColor", typeof(string));
                Dttable.Columns.Add("Amount", typeof(double));
                Dttable.Columns.Add("InvoiceAmount", typeof(double));
                Dttable.Columns.Add("DeliveryDate", typeof(DateTime));
                Dttable.Columns.Add("PODate", typeof(DateTime));
                Dttable.Columns.Add("SalesOwner", typeof(string));
                Dttable.Columns.Add("IsSaleOwnerNull", typeof(bool));
                
                Dttable.Columns.Add("Project", typeof(string));

                if (GeosApplication.Instance.IdUserPermission == 22 && GeosApplication.Instance.CrmOfferYear >= 2017)
                {

                    Dttable.Columns.Add("OEM", typeof(string));
                    Dttable.Columns.Add("Source", typeof(string));
                    Dttable.Columns.Add("BusinessUnit", typeof(string));
                    Dttable.Columns.Add("RFQReceptionDate", typeof(DateTime));
                    Dttable.Columns.Add("QuoteSentDate", typeof(DateTime));
                }

                Templates = new List<Template>();

                // Total Summary
                TotalSummary = new ObservableCollection<Summary>();
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = "Amount", DisplayFormat = " {0:C}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = "InvoiceAmount", DisplayFormat = " {0:C}" });
                TotalSummary.Add(new Summary() { Type = SummaryItemType.Count, FieldName = "Code", DisplayFormat = "Total: {0}" });
                OfferOptions = CrmStartUp.GetAllOfferOptions();

                for (int i = 0; i < OfferOptions.Count; i++)
                {
                    if (!Dttable.Columns.Contains(OfferOptions[i].Name))
                    {
                        Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = OfferOptions[i].Name.ToString(), HeaderText = OfferOptions[i].Name.ToString(), Settings = SettingsType.Array, AllowCellMerge = false, Width = 33, AllowEditing = false, Visible = false, IsVertical = true, FixedWidth = true });
                        TotalSummary.Add(new Summary() { Type = SummaryItemType.Sum, FieldName = OfferOptions[i].Name.ToString(), DisplayFormat = " {0}" });
                        Dttable.Columns.Add(OfferOptions[i].Name.ToString(), typeof(string));
                    }
                }

                //added site column in last on the grid.
                Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "Site", HeaderText = "Site", Settings = SettingsType.Default, AllowCellMerge = false, Width = 50, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = true });

                

                //FillDataTable();

                GeosApplication.Instance.Logger.Log("Method Fillgrid() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in Fillgrid() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in Fillgrid() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Fillgrid() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        public void FillDataTable()
        {
            Dttable.Rows.Clear();

            DttableCopy = Dttable.Copy();


            for (int i = 0; i < OrderList.Count; i++)
            {
                var converter = new System.Windows.Media.BrushConverter();
                System.Windows.Media.Brush brush = (System.Windows.Media.Brush)converter.ConvertFromString(OrderList[i].GeosStatus.HtmlColor.ToString());
                OrderList[i].GeosStatus.HtmlBrushColor = brush;

                DataRow dr = DttableCopy.NewRow();
                dr["IdOffer"] = OrderList[i].IdOffer.ToString();
                dr["Code"] = OrderList[i].Code.ToString();
                dr["Country"] = OrderList[i].Site.Country.Name.ToString();
                dr["Region"] = OrderList[i].Site.Country.Zone.Name.ToString();
                dr["Group"] = OrderList[i].Site.Customers[0].CustomerName.ToString();
                dr["Plant"] = OrderList[i].Site.Name.ToString();
                dr["Description"] = OrderList[i].Description.ToString();
                dr["Status"] = OrderList[i].GeosStatus.Name.ToString();
                dr["HtmlColor"] = OrderList[i].GeosStatus.HtmlColor.ToString();
                dr["Amount"] = OrderList[i].Value;
                dr["InvoiceAmount"] = OrderList[i].InvoiceAmount;
                if (OrderList[i].DeliveryDate != null)
                {


                    dr["DeliveryDate"] = OrderList[i].DeliveryDate.Value.ToShortDateString();
                }
                dr["PODate"] = OrderList[i].CustomerPurchaseOrdersByOffers[0].CustomerPurchaseOrder.ReceivedIn.ToShortDateString();

                if (OrderList[i].Site != null)
                {
                    dr["ConnectPlantId"] = OrderList[i].Site.ConnectPlantId.ToString();

                    OrderList[i].Site.Alias = GeosApplication.Instance.CompanyList.Where(cmplst => cmplst.ConnectPlantId == OrderList[i].Site.ConnectPlantId).Select(slt => slt.Alias).FirstOrDefault();
                    dr["Site"] = OrderList[i].Site.Alias;
                }


                if (OrderList[i].CarProject != null)
                    dr["Project"] = OrderList[i].CarProject.Name;

                if (GeosApplication.Instance.IdUserPermission == 22 && GeosApplication.Instance.CrmOfferYear >= 2017)
                {
                    if (OrderList[i].CarOEM != null)
                        dr["OEM"] = OrderList[i].CarOEM.Name;
                    if (OrderList[i].Source != null)
                        dr["Source"] = OrderList[i].Source.Value;
                    if (OrderList[i].BusinessUnit != null)
                        dr["BusinessUnit"] = OrderList[i].BusinessUnit.Value;

                    if (OrderList[i].RFQReception != null)
                    {
                        if (OrderList[i].RFQReception == DateTime.MinValue)
                            dr["RFQReceptionDate"] = DBNull.Value;
                        else
                            dr["RFQReceptionDate"] = OrderList[i].RFQReception.Value;
                    }
                    else
                    {
                        dr["RFQReceptionDate"] = DBNull.Value;
                    }

                    if (OrderList[i].SendIn != null)
                    {
                        if (OrderList[i].SendIn == DateTime.MinValue)
                            dr["QuoteSentDate"] = DBNull.Value;
                        else
                            dr["QuoteSentDate"] = OrderList[i].SendIn.Value;
                    }
                    else
                    {
                        dr["QuoteSentDate"] = DBNull.Value;
                    }

                }
                // *Below code for fill SalesOwner --Start

                if (orderList[i].IdSalesOwner != null)
                {
                    dr["SalesOwner"] = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == orderList[i].IdSalesOwner).Select(slt => slt.FullName).FirstOrDefault();//OrderList[i].SalesOwner.FullName;
                    dr["IsSaleOwnerNull"] = false;
                }
                else if (orderList[i].Site.IdSalesResponsible != null && orderList[i].Site.IdSalesResponsibleAssemblyBU != null)
                {
                    dr["SalesOwner"] = "";
                    dr["IsSaleOwnerNull"] = false;
                }
                else if (orderList[i].Site.IdSalesResponsible != null && orderList[i].Site.IdSalesResponsibleAssemblyBU == null)
                {
                    dr["SalesOwner"] = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == orderList[i].Site.IdSalesResponsible).Select(slt => slt.FullName).FirstOrDefault();
                    dr["IsSaleOwnerNull"] = true;
                }
                else if (orderList[i].Site.IdSalesResponsible == null && orderList[i].Site.IdSalesResponsibleAssemblyBU != null)
                {
                    dr["SalesOwner"] = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == orderList[i].Site.IdSalesResponsibleAssemblyBU).Select(slt => slt.FullName).FirstOrDefault();
                    dr["IsSaleOwnerNull"] = true;
                }

                try
                {
                    // *Below code for fill SalesOwner --End
                    OrderList[i].OptionsByOffers = OrderOptionsByOfferList.Where(option => option.IdOffer == OrderList[i].IdOffer && option.IdOfferPlant == Convert.ToInt32(OrderList[i].Site.ConnectPlantId)).ToList();
                    //LeadsList[i].OptionsByOffers = OptionsByOfferLeadsList.Where(oboll => oboll.IdOffer == LeadsList[i].IdOffer && oboll.IdOfferPlant == Convert.ToInt32(LeadsList[i].Site.ConnectPlantId)).ToList();
                    foreach (OptionsByOffer item in OrderList[i].OptionsByOffers)
                    {
                        if (DttableCopy.Columns[item.OfferOption.Name.ToString()].ToString() == item.OfferOption.Name.ToString())
                        {

                            var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == item.OfferOption.Name.ToString().Trim().ToUpper());
                            int indexc = Columns.IndexOf(column);
                            Columns[indexc].Visible = true;
                            dr[item.OfferOption.Name] = item.Quantity;
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                DttableCopy.Rows.Add(dr);

            }

            Dttable = DttableCopy;
        }

        /// <summary>
        /// Method for resize Grid Row on design.
        /// </summary>
        /// <param name="obj"></param>
        private void ViewHideRangeControl(object obj)
        {
            if (IsViewRangeControl)
            {
                GridRowHeight = "0";
                IsViewRangeControl = false;
            }
            else
            {
                GridRowHeight = "100";
                IsViewRangeControl = true;
            }
        }

        private void PrintLeadsGrid(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintLeadsGrid ...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["PageHeader"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["PageFooter"];
                pcl.Landscape = true;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A3;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                GeosApplication.Instance.Logger.Log("Method PrintLeadsGrid() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in PrintLeadsGrid() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void ExportLeadsGridButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportLeadsGridButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialogService.DefaultExt = "xlsx";
                SaveFileDialogService.DefaultFileName = "Orders";
                SaveFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                SaveFileDialogService.FilterIndex = 1;
                DialogResult = SaveFileDialogService.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    IsBusy = true;
                    if (!DXSplashScreen.IsActive)
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

                    ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;

                    TableView _OrdersTableView = ((TableView)obj);
                    _OrdersTableView.ShowTotalSummary = false;
                    _OrdersTableView.ExportToXlsx(ResultFileName);

                    SpreadsheetControl control = new SpreadsheetControl();
                    control.LoadDocument(ResultFileName);
                    Worksheet worksheet = control.ActiveWorksheet;

                    // Split all merged cells in the worksheet. 
                    foreach (var item in worksheet.Cells.GetMergedRanges())
                    {
                        item.UnMerge();
                    }

                    // Rotate only the OfferOptions.
                    DevExpress.Spreadsheet.ColumnCollection worksheetColumns = worksheet.Columns;

                    int countTechCol = 0;
                    foreach (var item in _OrdersTableView.AutoFilterRowData.FixedNoneCellData)
                    {
                        if (item.Column !=null && item.Column.FieldName == offerOptions[0].Name)
                        {
                            break;
                        }
                        if (item.Column != null && item.Column.Visible)
                            countTechCol++;
                    }
                    int i = countTechCol; // means Column Rotate 

                    foreach (var item in OfferOptions)
                    {
                        Emdep.Geos.UI.Helper.Column col = Columns.FirstOrDefault(x => x.FieldName == item.Name);

                        if (col.Visible)
                        {
                            string text = worksheetColumns[i].Heading + 1;

                            Cell cell = worksheet.Cells[text];
                            cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                            cell.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                            cell.Alignment.RotationAngle = 90;

                            i++;
                        }
                    }
                    DevExpress.Spreadsheet.Range supplierNamesRange = worksheet.Range["A1:AZ1"];
                    supplierNamesRange.Font.Bold = true;
                    control.SaveDocument();

                    IsBusy = false;
                    _OrdersTableView.ShowTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    Process.Start(ResultFileName);
                }

                GeosApplication.Instance.Logger.Log("Method ExportLeadsGridButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportLeadsGridButtonCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        #endregion
    }
}
