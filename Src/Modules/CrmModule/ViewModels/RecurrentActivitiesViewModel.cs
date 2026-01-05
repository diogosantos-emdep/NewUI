using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    // Sprint 46 CRM M046-02	Add new section Recurrent Activities in Configuration-Sdesai
    public class RecurrentActivitiesViewModel : INotifyPropertyChanged
    {
        #region TaskLog
        //GEOS2-224 sprint-60 (#64346) Create recurrent activities only for visible rows [adadibathina]
        #endregion

        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        #endregion // Services

        #region Declaration
        private ObservableCollection<ActivitiesRecurrence> recurrentActivityList;
        private ActivitiesRecurrence selectedRecurranceActivity;

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
        #endregion

        #region Public ICommand
        public ICommand PrintButtonCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand RecurrentActivitiesGridDoubleClickCommand { get; set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand SelectRangeofDatesCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        #endregion

        #region Properties   
        public ObservableCollection<ActivitiesRecurrence> RecurrentActivityList
        {
            get { return recurrentActivityList; }
            set
            {
                recurrentActivityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RecurrentActivityList"));
            }
        }
        public ActivitiesRecurrence SelectedRecurranceActivity
        {
            get
            {
                return selectedRecurranceActivity;
            }

            set
            {
                selectedRecurranceActivity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRecurranceActivity"));
            }
        }

        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

        #endregion

        public RecurrentActivitiesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor RecurrentActivitiesViewModel()...", category: Category.Info, priority: Priority.Low);
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintRecurrentActivitiesList));
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshRecurrentActivities));
                RecurrentActivitiesGridDoubleClickCommand = new DelegateCommand<object>(EditRecurrentActivityViewWindowShow);
                PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
                SelectRangeofDatesCommand = new RelayCommand(new Action<object>(SelectRangeofDateAction));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportRecurrentActivitiesButtonCommandAction));
                GeosApplication.Instance.Logger.Log("Constructor RecurrentActivitiesViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor RecurrentActivitiesViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region Methods


        /// <summary>
        /// Method to get Recurrent Activity List
        /// </summary>
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
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
                RecurrentActivitiesDetails();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to get data as per User Permission
        /// </summary>
        public void RecurrentActivitiesDetails()
        {
            if (GeosApplication.Instance.IdUserPermission == 21)
            {
                RecurrentActivityDetailsByUser();
            }
            else if (GeosApplication.Instance.IdUserPermission == 22)
            {
                RecurrentActivityDetailsByPlant();
            }
            else
            {
                RecurrentActivityDetailsByActiveUser();
            }
        }
        /// <summary>
        /// Method to get Recurrent Activity Details By User
        /// </summary>
        private void RecurrentActivityDetailsByUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RecurrentActivityDetailsByUser ...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    string salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    #region Sevice Comments
                    //RecurrentActivityList = new ObservableCollection<ActivitiesRecurrence>(CrmStartUp.GetActivitiesRecurrence_V2031(salesOwnersIds, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, "0", GeosApplication.Instance.IdUserPermission));
                    //[GEOS2-4327][rdixit][18.04.2023]
                    //Service Updated from GetActivitiesRecurrence_V2380 with GetActivitiesRecurrence_V2390 by [rdixit][GEOS2-4283][11.05.2023]
                    //Service Updated from GetActivitiesRecurrence_V2390 with GetActivitiesRecurrence_V2420 by [rdixit][GEOS2-4712][01.08.2023]     
                    #endregion
                    RecurrentActivityList = new ObservableCollection<ActivitiesRecurrence>(CrmStartUp.GetActivitiesRecurrence_V2420(salesOwnersIds, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, "0", GeosApplication.Instance.IdUserPermission));
                    if (RecurrentActivityList.Count > 0)
                        SelectedRecurranceActivity = RecurrentActivityList[0];
                }
                else
                {
                    RecurrentActivityList = new ObservableCollection<ActivitiesRecurrence>();
                }

                GeosApplication.Instance.Logger.Log("Method RecurrentActivityDetailsByUser() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RecurrentActivityDetailsByUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RecurrentActivityDetailsByUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RecurrentActivityDetailsByUser()", category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to get Recurrent Activity Details By Plant
        /// </summary>
        private void RecurrentActivityDetailsByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RecurrentActivityDetailsByPlant ...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                    #region Service Comments
                    //RecurrentActivityList = new ObservableCollection<ActivitiesRecurrence>(CrmStartUp.GetActivitiesRecurrence_V2031(GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, plantOwnersIds, GeosApplication.Instance.IdUserPermission));
                    //[GEOS2-4327][rdixit][18.04.2023]
                    //Service Updated from GetActivitiesRecurrence_V2380 with GetActivitiesRecurrence_V2390 by [rdixit][GEOS2-4283][11.05.2023]
                    //Service Updated from GetActivitiesRecurrence_V2390 with GetActivitiesRecurrence_V2420 by [rdixit][GEOS2-4712][01.08.2023]                    
                    #endregion
                    RecurrentActivityList = new ObservableCollection<ActivitiesRecurrence>(CrmStartUp.GetActivitiesRecurrence_V2420(GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, plantOwnersIds, GeosApplication.Instance.IdUserPermission));                    
                    if (RecurrentActivityList.Count > 0)
                        SelectedRecurranceActivity = RecurrentActivityList[0];
                }
                else
                {
                    RecurrentActivityList = new ObservableCollection<ActivitiesRecurrence>();

                }
                GeosApplication.Instance.Logger.Log("Method RecurrentActivityDetailsByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RecurrentActivityDetailsByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RecurrentActivityDetailsByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RecurrentActivityDetailsByPlant()", category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to get Recurrent Activity Details By Active User
        /// </summary>
        private void RecurrentActivityDetailsByActiveUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RecurrentActivityDetailsByActiveUser() ...", category: Category.Info, priority: Priority.Low);
                #region Service Comments
                //RecurrentActivityList = new ObservableCollection<ActivitiesRecurrence>(CrmStartUp.GetActivitiesRecurrence_V2031(GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, "0", GeosApplication.Instance.IdUserPermission));
                //[GEOS2-4327][rdixit][18.04.2023]
                //Service Updated from GetActivitiesRecurrence_V2380 with GetActivitiesRecurrence_V2390 by [rdixit][GEOS2-4283][11.05.2023]
                //Service Updated from GetActivitiesRecurrence_V2390 with GetActivitiesRecurrence_V2420 by [rdixit][GEOS2-4712][01.08.2023]
                #endregion
                RecurrentActivityList = new ObservableCollection<ActivitiesRecurrence>(CrmStartUp.GetActivitiesRecurrence_V2420(GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, "0", GeosApplication.Instance.IdUserPermission));
                GeosApplication.Instance.Logger.Log("Method RecurrentActivityDetailsByActiveUser() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RecurrentActivityDetailsByActiveUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RecurrentActivityDetailsByActiveUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RecurrentActivityDetailsByActiveUser()", category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to print Recurrent Activities List
        /// </summary>
        /// <param name="obj"></param>
        private void PrintRecurrentActivitiesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintRecurrentActivitiesList()...", category: Category.Info, priority: Priority.Low);

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
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["RecurrentActivitiesReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["RecurrentActivitiesReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintRecurrentActivitiesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintRecurrentActivitiesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Refresh Recurrent Activities List
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshRecurrentActivities(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method RefreshRecurrentActivities()...", category: Category.Info, priority: Priority.Low);

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
                detailView.SearchString = string.Empty;
                RecurrentActivitiesDetails();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshRecurrentActivities()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshRecurrentActivities() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshRecurrentActivities() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefreshRecurrentActivities() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditRecurrentActivityViewWindowShow(object obj)
        {
            try
            {
                // [001] added
                if (GeosApplication.Instance.IsPermissionAuditor)
                {
                    GeosApplication.Instance.Logger.Log("Method EditRecurrentActivityViewWindowShow()...", category: Category.Info, priority: Priority.Low);
                    TableView detailView = (TableView)obj;
                    ActivitiesRecurrence RecurrentActivity = (ActivitiesRecurrence)detailView.FocusedRow;
                    SelectedRecurranceActivity = RecurrentActivity;

                    if (RecurrentActivity != null)
                    {
                        AddRecurrentActivityView addRecurrentActivityView = new AddRecurrentActivityView();
                        AddRecurrentActivityViewModel addRecurrentActivityViewModel = new AddRecurrentActivityViewModel();
                        EventHandler handle = delegate { addRecurrentActivityView.Close(); };
                        addRecurrentActivityViewModel.RequestClose += handle;
                        addRecurrentActivityViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditRecurrentActivity").ToString();
                        addRecurrentActivityViewModel.IsNew = false;
                        addRecurrentActivityView.DataContext = addRecurrentActivityViewModel;
                        addRecurrentActivityViewModel.EditInit(SelectedRecurranceActivity);
                        var ownerInfo = (detailView as FrameworkElement);
                        addRecurrentActivityView.Owner = Window.GetWindow(ownerInfo);
                        addRecurrentActivityView.ShowDialog();
                        if (addRecurrentActivityViewModel.IsSave)
                        {

                            RecurrentActivity.Group = addRecurrentActivityViewModel.UpdateRecurrentActivity.Group;
                            RecurrentActivity.SiteNameWithoutCountry = addRecurrentActivityViewModel.UpdateRecurrentActivity.SiteNameWithoutCountry;
                            RecurrentActivity.Country = addRecurrentActivityViewModel.UpdateRecurrentActivity.Country;
                            RecurrentActivity.Region = addRecurrentActivityViewModel.UpdateRecurrentActivity.Region;
                            RecurrentActivity.SalesOwner = addRecurrentActivityViewModel.UpdateRecurrentActivity.SalesOwner;
                            RecurrentActivity.WeekDay = addRecurrentActivityViewModel.UpdateRecurrentActivity.WeekDay;
                            RecurrentActivity.Periodicity = addRecurrentActivityViewModel.UpdateRecurrentActivity.Periodicity;
                            RecurrentActivity.IsSalesResponsible = addRecurrentActivityViewModel.UpdateRecurrentActivity.IsSalesResponsible;
                            SelectedRecurranceActivity = RecurrentActivity;
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditRecurrentActivityViewWindowShow()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditRecurrentActivityViewWindowShow()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

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
                // FillRecurrentActivitiesListByPlant();
                RecurrentActivityDetailsByPlant();
            }
            else
            {
                RecurrentActivityList = new ObservableCollection<ActivitiesRecurrence>();
            }

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }


        /// <summary>
        /// Method to fill Recurrent Activities by plant
        /// </summary>
        //private void FillRecurrentActivitiesListByPlant()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillRecurrentActivitiesListByPlant ...", category: Category.Info, priority: Priority.Low);
        //        if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
        //        {
        //            List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
        //            var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));
        //            RecurrentActivityList = new ObservableCollection<ActivitiesRecurrence>(CrmStartUp.GetActivitiesRecurrence(GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, plantOwnersIds, GeosApplication.Instance.IdUserPermission));
        //        }

        //        GeosApplication.Instance.Logger.Log("Method FillRecurrentActivitiesListByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillRecurrentActivitiesListByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillRecurrentActivitiesListByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //}

        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void SalesOwnerPopupClosedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SalesOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);

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
                    //List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    //string salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    //RecurrentActivityList = new ObservableCollection<ActivitiesRecurrence>(CrmStartUp.GetActivitiesRecurrence(salesOwnersIds, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, "0", GeosApplication.Instance.IdUserPermission));

                    RecurrentActivityDetailsByUser();

                    //if (RecurrentActivityList.Count > 0)
                    //    SelectedRecurranceActivity = RecurrentActivityList[0];
                }
                else
                {
                    RecurrentActivityList = new ObservableCollection<ActivitiesRecurrence>();
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method SalesOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SalesOwnerPopupClosedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SalesOwnerPopupClosedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SalesOwnerPopupClosedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Select Range of Date
        /// </summary>
        /// <param name="obj"></param>
        private void SelectRangeofDateAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectRangeofDateAction()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                SelectRangeOfDatesView selectRangeOfDatesView = new SelectRangeOfDatesView();
                SelectRangeOfDatesViewModel selectRangeOfDatesViewModel = new SelectRangeOfDatesViewModel();
                EventHandler handle = delegate { selectRangeOfDatesView.Close(); };
                selectRangeOfDatesViewModel.RequestClose += handle;

                selectRangeOfDatesViewModel.IsNew = true;
                selectRangeOfDatesView.DataContext = selectRangeOfDatesViewModel;
                var FilterdView = (DevExpress.Xpf.Grid.TableView)obj;
                IList Rows = FilterdView.Grid.DataController.GetAllFilteredAndSortedRows();
                selectRangeOfDatesViewModel.Init(Rows);
                var ownerInfo = (detailView as FrameworkElement);
                selectRangeOfDatesView.Owner = Window.GetWindow(ownerInfo);
                selectRangeOfDatesView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method SelectRangeofDateAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectRangeofDateAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Export to excel Recurrent Activities
        /// </summary>
        /// <param name="obj"></param>
        private void ExportRecurrentActivitiesButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportRecurrentActivitiesButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Recurrent Activities";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (bool)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
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
                    ResultFileName = (saveFile.FileName);
                    TableView recurrentActivitiesTableView = ((TableView)obj);
                    recurrentActivitiesTableView.ShowTotalSummary = false;
                    recurrentActivitiesTableView.ShowFixedTotalSummary = false;
                    recurrentActivitiesTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    Process.Start(ResultFileName);
                    recurrentActivitiesTableView.ShowTotalSummary = false;
                    recurrentActivitiesTableView.ShowFixedTotalSummary = true;
                }

                GeosApplication.Instance.Logger.Log("Method ExportRecurrentActivitiesButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportRecurrentActivitiesButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
