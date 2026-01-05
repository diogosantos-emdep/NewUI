using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Modules.OTM.Views;
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.OTM.ViewModels
{
    /// [Rahul.Gadhave][[GEOS2-9281][Date:05-09-2025]
    public class SystemSettingsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services
        IOTMService OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IOTMService OTMService = new OTMServiceController("localhost:6699");

        #endregion Services

        #region Public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        // new public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region declaration
        private ObservableCollection<POEmployeeInfo> Jobdescriptionslist;
        private ObservableCollection<POEmployeeInfo> addeddescriptionslist;
        private List<Object> selectedJobDescriptions;
        #endregion declaration


        #region Properties
        public ObservableCollection<POEmployeeInfo> AddedJobDescriptionsList
        {
            get
            {
                return addeddescriptionslist;
            }
            set
            {
                addeddescriptionslist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddedJobDescriptionsList"));
            }
        }
        public ObservableCollection<POEmployeeInfo> JobDescriptionsList
        {
            get
            {
                return Jobdescriptionslist;
            }
            set
            {
                Jobdescriptionslist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobDescriptionsList"));
            }
        }
        public List<Object> SelectedJobDescriptions
        {
            get { return selectedJobDescriptions; }
            set
            {
                selectedJobDescriptions = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedJobDescriptions"));
            }
        }
        #endregion Properties

        #region ICommand
        public ICommand MyPreferencesViewAcceptButtonCommand { get; set; }
        public ICommand MyPreferencesViewCancelButtonCommand { get; set; }
        public ICommand AddJobDescriptonInGrid { get; set; }
        public ICommand RemoveJobDescriptionCommand { get; set; }

        #endregion

        #region Constructor
        public SystemSettingsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SystemSettingsViewModel ...", category: Category.Info, priority: Priority.Low);

                MyPreferencesViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddJobDescriptonInGrid = new RelayCommand(new Action<object>(AddJobDescriptonInGridAction));
                RemoveJobDescriptionCommand = new RelayCommand(new Action<object>(RemoveJobDescriptionGridAction));
                MyPreferencesViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveJobDescriptonAction));
                FillAddedJobDescriptions();
                FillJobDescriptions();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor SystemSettingsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor SystemSettingsViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        #region Methods
        private void AddJobDescriptonInGridAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddJobDescriptonInGridAction()...", category: Category.Info, priority: Priority.Low);

                if (SelectedJobDescriptions != null && SelectedJobDescriptions.Any())
                {
                    if (AddedJobDescriptionsList == null)
                        AddedJobDescriptionsList = new ObservableCollection<POEmployeeInfo>();

                    // Filter only new JobDescriptions (avoid duplicates)
                    var newJobs = SelectedJobDescriptions
                        .OfType<POEmployeeInfo>()
                        .Where(job => !AddedJobDescriptionsList.Any(x => x.IdJobDescription == job.IdJobDescription))
                        .ToList();

                    // Add new JobDescriptions to the list
                    foreach (var job in newJobs)
                    {
                        AddedJobDescriptionsList.Add(job);
                    }

                    // Call service once with the new items
                    //if (newJobs.Any())
                    //{
                    //    OTMService = new OTMServiceController("localhost:6699");
                    //    OTMService.UpdateConfirmationJDSetting_V2670(newJobs);
                    //}

                    // Clear the selected items
                    SelectedJobDescriptions.Clear();
                    SelectedJobDescriptions=null;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedJobDescriptions)));
                }

                GeosApplication.Instance.Logger.Log("Method AddJobDescriptonInGridAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddJobDescriptonInGridAction(): " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RemoveJobDescriptionGridAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RemoveJobDescriptionGridAction()...", category: Category.Info, priority: Priority.Low);

                // DevExpress passes GridCellData in CommandParameter
                if (obj is DevExpress.Xpf.Grid.EditGridCellData cellData)
                {
                    // The actual bound data object of the row
                    var item = cellData.RowData.Row as POEmployeeInfo;
                    if (item != null && AddedJobDescriptionsList != null)
                    {
                        AddedJobDescriptionsList.Remove(item);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method RemoveJobDescriptionGridAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RemoveJobDescriptionGridAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillJobDescriptions()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillJobDescriptions()...", category: Category.Info, priority: Priority.Low);
                //OTMService = new OTMServiceController("localhost:6699");
                var all = OTMService.GetJob_DescriptionsList_V2670();

                if (AddedJobDescriptionsList != null && AddedJobDescriptionsList.Count > 0)
                {
                    var addedIds = AddedJobDescriptionsList.Select(x => x.IdJobDescription).ToList();
                    all = all.Where(x => !addedIds.Contains(x.IdJobDescription)).ToList();
                }

                JobDescriptionsList = new ObservableCollection<POEmployeeInfo>(all);
                GeosApplication.Instance.Logger.Log("Method FillJobDescriptions()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in FillJobDescriptions() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in FillJobDescriptions() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in Method FillJobDescriptions()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void FillAddedJobDescriptions()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillJobDescriptions()...", category: Category.Info, priority: Priority.Low);
                //OTMService = new OTMServiceController("localhost:6699");
                AddedJobDescriptionsList = new ObservableCollection<POEmployeeInfo>(OTMService.GetAddedJob_DescriptionsList_V2670());
                GeosApplication.Instance.Logger.Log("Method FillJobDescriptions()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in FillJobDescriptions() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in FillJobDescriptions() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                CloseProcessing();
                GeosApplication.Instance.Logger.Log("Get an error in Method FillJobDescriptions()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void SaveJobDescriptonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveMyPreference() ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
               
                    //OTMService = new OTMServiceController("localhost:6699");
                    OTMService.UpdateConfirmationJDSetting_V2670(AddedJobDescriptionsList.ToList());


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Method SaveMyPreference()....executed successfully"), category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in SaveMyPreference() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void CloseProcessing()
        {
            GeosApplication.Instance.Logger.Log("Method CloseProcessing()...", category: Category.Info, priority: Priority.Low);
            if (DXSplashScreen.IsActive)
            {
                DXSplashScreen.Close();
            }
            GeosApplication.Instance.Logger.Log("Method CloseProcessing()....executed successfully", category: Category.Info, priority: Priority.Low);
        }
        public void CloseWindow(object obj)
        {
            //IsPreferenceChanged = false;
            RequestClose(null, null);

        }
        #endregion Methods
    }
}
