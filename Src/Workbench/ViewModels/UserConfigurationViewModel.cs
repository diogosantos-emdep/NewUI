using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Adapters.Logging;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Workbench.ViewModels
{
    class UserConfigurationViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Declaration

        ObservableCollection<string> listGeosServiceProviders;

        public ObservableCollection<string> ListGeosServiceProviders
        {
            get { return listGeosServiceProviders; }
            set { listGeosServiceProviders = value; }
        }
        private string selectedIndexGeosServiceProviders;
        public string SelectedIndexGeosServiceProviders
        {
            get { return selectedIndexGeosServiceProviders; }
            set { selectedIndexGeosServiceProviders = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexGeosServiceProviders")); }
        }
        string servicePath;
        public string ServicePath
        {
            get
            {
                return servicePath;
            }

            set
            {
                servicePath = value; OnPropertyChanged(new PropertyChangedEventArgs("ServicePath"));
            }
        }
        #endregion  //Declaration

        #region Properties

        #endregion  //Properties

        #region Command
        public ICommand ServiceProvidersSelectionChangedCommand { get; set; }
        public ICommand UserConfigurationSaveButtonCommand { get; set; }
        public ICommand UserConfigurationCancelButtonCommand { get; set; }

        #endregion  //Command

        #region Constructor

        public UserConfigurationViewModel()
        {
            ListGeosServiceProviders = new System.Collections.ObjectModel.ObservableCollection<string>(GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Select(serviceProviderName => serviceProviderName.Name).ToList());
            ListGeosServiceProviders.Insert(0, "---");
            SelectedIndexGeosServiceProviders = "---";
            UserConfigurationSaveButtonCommand = new RelayCommand(new Action<object>(SaveUserConfiguration));
            ServiceProvidersSelectionChangedCommand = new RelayCommand(new Action<object>(ServiceProvidersSelection));
            UserConfigurationCancelButtonCommand = new RelayCommand(new Action<object>(UserConfigurationWindowClose));
        }

        #endregion  //Constructor

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

        #endregion  //Events

        #region Methods

        /// <summary>
        /// Method for save application setting.
        /// </summary>
        /// <param name="obj"></param>

        public void SaveUserConfiguration(object obj)
        {
            //IsBusy = true;


            GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Select(ServiceProvider => { if (ServiceProvider.Name.Contains(SelectedIndexGeosServiceProviders)) ServiceProvider.IsSelected = true; return ServiceProvider; }).ToList();
            string tempSelectedIndexGeosServiceProviders = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.Name).FirstOrDefault();
            try
            {
                string logfilepath = GeosApplication.Instance.ApplicationLogFilePath;
                if (!File.Exists(GeosApplication.Instance.ApplicationLogFilePath))
                {
                    if (!Directory.Exists(Directory.GetParent(logfilepath).FullName))
                        Directory.CreateDirectory(Directory.GetParent(logfilepath).FullName);
                    File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + GeosApplication.Instance.ApplicationLogFileName, logfilepath);
                }

                FileInfo file = new FileInfo(GeosApplication.Instance.ApplicationLogFilePath);

                GeosApplication.Instance.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                if (!string.IsNullOrEmpty(tempSelectedIndexGeosServiceProviders))
                {

                    if (GeosApplication.Instance.ApplicationSettings.ContainsKey("ServicePath"))
                    {
                        GeosApplication.Instance.ApplicationSettings["ServicePath"] = ServicePath;
                    }
                    else
                    {
                        GeosApplication.Instance.ApplicationSettings.Add("ServicePath", ServicePath);
                    }


                    GeosApplication.Instance.GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Select(serviceProvider =>
                    {
                        if (serviceProvider.Name.Contains(tempSelectedIndexGeosServiceProviders)) serviceProvider.IsSelected = false;
                        if (serviceProvider.Name.Contains(SelectedIndexGeosServiceProviders)) { serviceProvider.IsSelected = true; serviceProvider.ServiceProviderUrl = ServicePath; }
                        return serviceProvider;
                    }).ToList();


                    GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider = GeosApplication.Instance.GeosServiceProviderList;
                    GeosApplication.Instance.WriteApplicationSettingFile(GeosApplication.Instance.GeosServiceProviderList);



                    GeosApplication.Instance.ApplicationSettings["ServicePath"] = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.ServiceProviderUrl).FirstOrDefault();

                    bool IsExceptionIsThrown = false;

                    try
                    {
                        //IsBusy = true;

                        IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(
                           GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                        GeosApplication.Instance.ServerDateTime = workbenchControl.GetServerDateTime();

                        // ApplicationOperation.CreateNewSetting(lstApplicationConfiguration, GeosApplication.Instance.ApplicationSettingFilePath, "ApplicationSettings");
                        if (GeosApplication.Instance.ApplicationSettings != null && GeosApplication.Instance.ApplicationSettings.Count > 0)
                        { }
                        // GetUpdatedWorkbenchVersion();
                        //IsBusy = false;

                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        IsExceptionIsThrown = true;
                        //GeosApplication.Instance.Logger.Log("Get an error in SaveUserConfiguration() method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
                    {
                        IsExceptionIsThrown = true;
                        //IsBusy = false;
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    finally
                    {
                        //This code is written in finally block because it is to be exceuted in all catch blocks.
                        if (IsExceptionIsThrown)        // If any exception then revert changes.
                        {
                            string temp = string.Copy(SelectedIndexGeosServiceProviders);
                            SelectedIndexGeosServiceProviders = tempSelectedIndexGeosServiceProviders;



                            GeosApplication.Instance.GeosServiceProviderList = ((App)Application.Current).GeosServiceProviderList.Select(serviceProvider =>
                            {
                                if (serviceProvider.Name.Contains(temp)) serviceProvider.IsSelected = false;
                                if (serviceProvider.Name.Contains(SelectedIndexGeosServiceProviders)) { serviceProvider.IsSelected = true; serviceProvider.ServiceProviderUrl = ServicePath; }
                                return serviceProvider;
                            }).ToList();

                            GeosApplication.Instance.ApplicationSettings["ServicePath"] = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.ServiceProviderUrl).FirstOrDefault();


                            GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider = GeosApplication.Instance.GeosServiceProviderList;
                            GeosApplication.Instance.WriteApplicationSettingFile(GeosApplication.Instance.GeosServiceProviderList);


                        }
                    }


                }
                else
                {
                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("UserConfigurationWindowIpError").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                RequestClose(null, null);

            }
            catch (Exception ex)
            {

            }


            try
            {





            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                // IsBusy = false;
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }

            //IsBusy = false;

        }
        public void UserConfigurationWindowClose(object obj)
        {
            Environment.Exit(0);
            return;
        }

        #endregion  //Methods
        private void ServiceProvidersSelection(object obj)
        {

            ServicePath = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(serviceProviderName => serviceProviderName.Name.Contains(SelectedIndexGeosServiceProviders)).Select(serviceProviderPublicNetworkIp => serviceProviderPublicNetworkIp.ServiceProviderUrl).FirstOrDefault();



        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
