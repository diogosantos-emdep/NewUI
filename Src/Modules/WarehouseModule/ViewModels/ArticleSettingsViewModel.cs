using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Data.Async;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{

	//[nsatpute][29.08.2025][GEOS2-6505]
    internal class ArticleSettingsViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        // [nsatpute][14-11-2024][GEOS2-5747]
        #region Services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        #endregion

        #region Fields
        public string Error => throw new NotImplementedException();
        public string this[string columnName] => throw new NotImplementedException();
        private bool showBacklogDays;
        private Visibility showNextButton;
        private Visibility showSaveButton;

        private bool isBusy;
        private bool enableFilters;
        private bool regionChanged;
        private bool initComplete;
        #endregion

        #region Constructors

        public ArticleSettingsViewModel()
        {
            try
            {
                initComplete = false;
                GeosApplication.Instance.Logger.Log("Constructor EmployeeBacklogHolidaysViewModel ...", category: Category.Info, priority: Priority.Low);
                NextButtonCommand = new DelegateCommand<object>(NextButtonCommandAction);
                ResetButtonCommand = new DelegateCommand<object>(ResetButtonCommandAction);
                SaveButtonCommand = new DelegateCommand<object>(SaveButtonCommandAction);
                Init();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                initComplete = true;
                GeosApplication.Instance.Logger.Log("Constructor EmployeeBacklogHolidaysViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                isBusy = false;
                initComplete = true;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor EmployeeBacklogHolidaysViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion

        #region Properties

        public bool ShowBacklogDays
        {
            get { return showBacklogDays; }
            set
            {
                showBacklogDays = value;
                OnPropertyChanged("ShowBacklogDays");
            }
        }

        public Visibility ShowNextButton
        {
            get { return showNextButton; }
            set
            {
                showNextButton = value;
                OnPropertyChanged("ShowNextButton");
            }
        }
        public Visibility ShowSaveButton
        {
            get { return showSaveButton; }
            set
            {
                showSaveButton = value;
                OnPropertyChanged("ShowSaveButton");
            }
        }



        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("EmployeeList");
            }
        }

        public bool EnableFilters
        {
            get { return enableFilters; }
            set
            {
                enableFilters = value;
                OnPropertyChanged("EnableFilters");
            }
        }
        #endregion

        #region Commands
        public ICommand NextButtonCommand { get; set; }
        public ICommand ResetButtonCommand { get; set; }
        public ICommand SaveButtonCommand { get; set; }


        #endregion

        #region Methods      

        private void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);
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
                ShowBacklogDays = false;
                ShowSaveButton = Visibility.Collapsed;
                ShowNextButton = Visibility.Visible;
                EnableFilters = true;
                FillRegions();
                FillDepartment();
                FillEmployeesByDepartmentAndCompany();
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                isBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void FillRegions()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillRegions ...", category: Category.Info, priority: Priority.Low);

               
                GeosApplication.Instance.Logger.Log("Method FillRegions() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillRegions() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillRegions() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillRegions() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillDepartment()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDepartment ...", category: Category.Info, priority: Priority.Low);
                //DepartmentList = new ObservableCollection<Department>(HrmService.GetAllDepartments());
                GeosApplication.Instance.Logger.Log("Method FillDepartment() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillEmployeesByDepartmentAndCompany()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDepartment ...", category: Category.Info, priority: Priority.Low);
                //EmployeeList = HrmService.GetEmployeesByDepartmentAndCompany_V2590();
                //EmployeeListFiltered = new ObservableCollection<Employee>();
                ////EmployeeList.ForEach(x => { EmployeeListFiltered.Add((Employee)x.Clone()); });
                //EmployeeListFiltered.AddRange(EmployeeList.Select(x => (Employee)x.Clone()));
                GeosApplication.Instance.Logger.Log("Method FillDepartment() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SaveEmployeeBacklogHours()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDepartment ...", category: Category.Info, priority: Priority.Low);
                //EmployeeList = HrmService.GetEmployeesByDepartmentAndCompany();
                GeosApplication.Instance.Logger.Log("Method FillDepartment() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDepartment() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ReloadGridByRegion()
        {
           
        }

        private void ReloadGridByCountry()
        {
           
        }

        private void ReloadGridBySites()
        {
           
        }
        private void ReloadGridByDepartment()
        {
           
        }

        #endregion

        #region Event Handlers
		// [nsatpute][17-12-2024][GEOS2-5747]
        private void NextButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NextButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                //if (EmployeeListFiltered.Where(x => x.IsSelected).Count() == 0)
                {
                    CustomMessageBox.Show(Application.Current.Resources["Automaticbacklog_Pleaseselectanyrecord"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
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

                //SelectedEmployeeList = new List<Employee>();
                //EmployeeListFiltered.ForEach(x => { SelectedEmployeeList.Add((Employee)x.Clone()); });
                //EmployeeListFiltered = EmployeeListFiltered.Where(x => x.IsSelected).ToObservableCollection();
                //EmployeeListFiltered = HrmService.GetEmployeeBacklogHours_V2600(EmployeeListFiltered.ToList()).ToObservableCollection(); // [nsatpute][16-01-2025][GEOS2-6862]
                ShowSaveButton = Visibility.Visible;
                ShowNextButton = Visibility.Collapsed;
                EnableFilters = false;
                ShowBacklogDays = true;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method NextButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method NextButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ResetButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ResetButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                
                ShowSaveButton = Visibility.Collapsed; ;
                ShowNextButton = Visibility.Visible;
                ShowBacklogDays = false;
                EnableFilters = true;
                GeosApplication.Instance.Logger.Log("Method ResetButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ResetButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SaveButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
               // if (EmployeeListFiltered.Where(x => x.IsSelected).Count() == 0)
                {
                    CustomMessageBox.Show(Application.Current.Resources["Automaticbacklog_Pleaseselectanyrecord"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
               // HrmService.SaveEmployeeBacklogHours(EmployeeListFiltered.Where(x => x.IsSelected).ToList());
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("Automaticbacklog_Backloghourshavebeensuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);                
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
             
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SaveButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SaveButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

}
