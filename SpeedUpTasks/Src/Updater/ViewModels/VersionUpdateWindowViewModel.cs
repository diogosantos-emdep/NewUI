using DevExpress.Compression;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Emdep.Geos.Workbench.Updater;
using Emdep.Geos.Workbench.Updater.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Dynamic;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.Commands;
using System.IO.Compression;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Common;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Prism.Logging;
using Emdep.Geos.UI.Adapters.Logging;
using System.Xml.Serialization;
using Emdep.Geos.Utility.Text;
using DevExpress.Xpf.Core;
using System.Security.AccessControl;

namespace Emdep.Geos.Workbench.Downloader.ViewModels
{
    public enum InstallationStatus
    {
        Connecting,
        Download,
        BackupCreating,
        BackupCreated,
        Delete,
        Installation,
        Complete,
        Restore,
        BackupCreatingInRestore,
        BackupCreatedInRestore,
        DeleteInRestore,
        RestoringInRestore
    }

    public class VersionUpdateWindowViewModel : INotifyPropertyChanged
    {
        #region Thread

        Thread threadDownloadProcess;//Thread for download  
        Thread threadCancelDownloadProcess; // thread for cancel download
        BackgroundWorker bgwdownload; //background Thread for download  
        BackgroundWorker bgwdownloadcancel;
        #endregion

        int progressbardownloadvalue = 0;
        int progressbarcancelvalue = 0;
        string fulltemporaryinstallationdownloadpath = string.Empty;    // system Temp Folder path 
        string partialtemporaryinstallationdownloadpath = string.Empty; // system Temp Folder path 

        #region Declaration

        private int selectedViewIndex = 0;
        volatile bool stopProgress = false;
        bool isApplicationSettingAvailable = false;
        double u = 0;
        double u1 = 0;
        private Visibility progressbarVisibility = Visibility.Hidden; //   Progress bar visible  in SoftwareUpdateView    
        private Visibility doneButtonVisibility = Visibility.Hidden;  //   Done Button visible
        //private Visibility cancelButtonVisibility = Visibility.Hidden;//   cancel Button visible
        private Visibility cancelButtonVisibility;//   cancel Button visible
        private Visibility labelInstalledVersionVisibility; //   label Installed Version visible

        private Visibility downloadNowVisibility;
        private Visibility downloadLaterVisibility;
        private Visibility releaseNotesVisibility;
        private Visibility acceptButtonVisibility;
        private Visibility sameVersionOptionButtonVisibility;

        private Visibility repairButtonVisibility;
        private Visibility restoreButtonVisibility;
        private Visibility closeButtonVisibility;
        private Visibility configurationButtonVisibility;

        int releaseNoteType = 0;
        private Visibility progressbarImageVisibility = Visibility.Hidden; // Progress bar image visible  in SoftwareUpdateView    
        private GeosWorkbenchVersion workbenchVersionNumber; // get  Workbench Version Number
        private GeosWorkbenchVersionDownload workbenchVersionDownload;//  WorkbenchVersionDownload

        private ObservableCollection<GeosReleaseNote> listReleaseNotes; //list of ReleaseNotes
        private ObservableCollection<string> listGeosServiceProviders;

        private string installedAssemblyVersion; // installed Assembly Version
        private string latestAssemblyVersion; // Latest Assembly Version
        private FileTransferRequest fileTransferRequest; // class FileTransferRequest
        private double progressBarvalue = 0; //   Progress bar set value  in SoftwareUpdateView  
        private string downloadLogs = string.Empty;// Display download information 
        private InstallationStatus currentstatus; //enum  of InstallationStatus
        private InstallationStatus currentcancelstatus;
        private bool isWorkbench = false; // check  isWorkbench
        private string backupfileversion;
        private bool isEnabledCancel = true;
        private bool isClickOnSettingButton = false;
        public string tempPath1 { get; set; }
        public string restoreTempZipFilePath { get; set; }

        int timerInterval = 300;
        private bool isBusy;
        //IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        //GeosRepositoryServiceController fileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        GeosWorkbenchVersion geosWorkbenchVersion;
        bool isRepairORRestore = false;

        private List<Language> languages;
        private int languageSelectedIndex;
        string ip;
        string servicePath;
        decimal port;

        private bool downloadLaterEnabled;

        private string selectedIndexGeosServiceProviders;
        public bool IsNetworkIP { get; set; }

        #region Folder information

        private readonly string updateInstalledFolder; //path of  Installed update Folder
        private readonly string workbenchInstalledFolder; //path of  Installed workbench Folder
        private readonly string GeosFolderPath; //path of  Geos Folder
        private readonly string backupInstalledFolder; //path of  Installed backup Folder
        private readonly string backupRestoreZip; //path of  user temp backup file

        #endregion

        #endregion

        #region Command

        //declare ICommand
        public ICommand ReleaseNoteButtonCommand { get; set; }      //ReleaseNote display 
        public ICommand ReleaseNoteBackButtonCommand { get; set; }  // Release Note BackButton
        public ICommand DownloadButtonCommand { get; set; }         // Show Download Button 
        public ICommand CancelDownloadButtonCommand { get; set; }   //Cancel Button 
        public ICommand DoneButtonCommand { get; set; }             //Done Button 
        public ICommand DownloadLaterButtonCommand { get; set; }    //Download Later Button
        public ICommand AcceptDownloadButtonCommand { get; set; }   //Accept Download Button
        public ICommand RepairButtonCommand { get; set; }           // Show Repair Button 
        public ICommand RestoreButtonCommand { get; set; }          // Show Restore Button 
        public ICommand CancelButtonCommand { get; set; }           // Show Cancel Button 
        public ICommand UserConfigurationSaveButtonCommand { get; set; }    //UserConfigurationWindow Save Button 
        public ICommand UserConfigurationCancelButtonCommand { get; set; }  //UserConfigurationWindow Cancel Button
        public ICommand UserConfigurationShowWindowCommand { get; set; }
        public ICommand ServiceProvidersSelectionChangedCommand { get; set; }
        public ICommand UpdaterWindowMinimizeCommand { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// /
        /// </summary>
        public int VersionTitle { get; set; }

        public int SelectedViewIndex
        {
            get { return selectedViewIndex; }
            set
            {
                selectedViewIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedViewIndex"));
            }
        }

        public Visibility ProgressbarVisibility
        {
            get { return progressbarVisibility; }
            set
            {
                progressbarVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProgressbarVisibility"));
            }
        }

        public Visibility ReleaseNotesVisibility
        {
            get { return releaseNotesVisibility; }
            set
            {
                releaseNotesVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReleaseNotesVisibility"));
            }
        }

        public Visibility DoneButtonVisibility
        {
            get { return doneButtonVisibility; }
            set
            {
                doneButtonVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DoneButtonVisibility"));
            }
        }

        public Visibility ProgressbarImageVisibility
        {
            get { return progressbarImageVisibility; }
            set
            {
                progressbarImageVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProgressbarImageVisibility"));
            }
        }

        public Visibility CancelButtonVisibility
        {
            get { return cancelButtonVisibility; }
            set
            {
                cancelButtonVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CancelButtonVisibility"));
            }
        }

        public Visibility LabelInstalledVersionVisibility
        {
            get { return labelInstalledVersionVisibility; }
            set
            {
                labelInstalledVersionVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LabelInstalledVersionVisibility"));
            }
        }

        public Visibility AcceptButtonVisibility
        {
            get { return acceptButtonVisibility; }
            set
            {
                acceptButtonVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AcceptButtonVisibility"));
            }
        }

        public Visibility SameVersionOptionButtonVisibility
        {
            get { return sameVersionOptionButtonVisibility; }
            set
            {
                sameVersionOptionButtonVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SameVersionOptionButtonVisibility"));
            }
        }

        public GeosWorkbenchVersion WorkbenchVersionNumber
        {
            get { return workbenchVersionNumber; }
            set
            {
                workbenchVersionNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkbenchVersionNumber"));
            }
        }

        public Visibility RepairButtonVisibility
        {
            get { return repairButtonVisibility; }
            set
            {
                repairButtonVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RepairButtonVisibility"));
            }
        }

        public Visibility RestoreButtonVisibility
        {
            get { return restoreButtonVisibility; }
            set
            {
                restoreButtonVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RestoreButtonVisibility"));
            }
        }

        public Visibility CloseButtonVisibility
        {
            get { return closeButtonVisibility; }
            set
            {
                closeButtonVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CloseButtonVisibility"));
            }
        }

        public Visibility ConfigurationButtonVisibility
        {
            get { return configurationButtonVisibility; }
            set
            {
                configurationButtonVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConfigurationButtonVisibility"));
            }
        }

        public GeosWorkbenchVersionDownload WorkbenchVersionDownload
        {
            get { return workbenchVersionDownload; }
            set
            {
                workbenchVersionDownload = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkbenchVersionDownload"));
            }
        }

        public ObservableCollection<GeosReleaseNote> ReleaseNotesList
        {
            get { return listReleaseNotes; }
            set
            {
                listReleaseNotes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReleaseNotesList"));
            }
        }

        public string InstalledAssemblyVersion
        {
            get { return installedAssemblyVersion; }
            set
            {
                installedAssemblyVersion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InstalledAssemblyVersion"));
            }
        }

        public string LatestAssemblyVersion
        {
            get { return latestAssemblyVersion; }
            set
            {
                latestAssemblyVersion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LatestAssemblyVersion"));
            }
        }

        public FileTransferRequest FileTransferRequest
        {
            get { return fileTransferRequest; }
            set { fileTransferRequest = value; }
        }

        public string DownloadLogs
        {
            get { return downloadLogs; }
            set
            {
                downloadLogs = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DownloadLogs"));
            }
        }

        public ObservableCollection<string> ListGeosServiceProviders
        {
            get { return listGeosServiceProviders; }
            set
            {
                listGeosServiceProviders = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListGeosServiceProviders"));
            }
        }

        public string SelectedIndexGeosServiceProviders
        {
            get { return selectedIndexGeosServiceProviders; }
            set
            {
                selectedIndexGeosServiceProviders = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexGeosServiceProviders"));
            }
        }

        public bool IsWorkbench
        {
            get { return isWorkbench; }
            set { isWorkbench = value; }
        }

        public Visibility DownloadNowVisibility
        {
            get { return downloadNowVisibility; }
            set
            {
                downloadNowVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DownloadNowVisibility"));
            }
        }

        public Visibility DownloadLaterVisibility
        {
            get { return downloadLaterVisibility; }
            set
            {
                downloadLaterVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DownloadLaterVisibility"));
            }
        }

        public bool IsEnabledCancel
        {
            get { return isEnabledCancel; }
            set
            {
                isEnabledCancel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledCancel"));
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

        public List<Language> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Languages"));
            }
        }

        public int LanguageSelectedIndex
        {
            get { return languageSelectedIndex; }
            set
            {
                languageSelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LanguageSelectedIndex"));
            }
        }

        public string Ip
        {
            get { return ip; }
            set
            {
                ip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Ip"));
            }
        }

        public decimal Port
        {
            get { return port; }
            set
            {
                port = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Port"));
            }
        }

        public string ServicePath
        {
            get { return servicePath; }
            set
            {
                servicePath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ServicePath"));
            }
        }

        public bool IsClickOnSettingButton
        {
            get { return isClickOnSettingButton; }
            set
            {
                isClickOnSettingButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsClickOnSettingButton"));
            }
        }

        /// <summary>
        /// There are three type of Title for Releasenote window as follow
        /// 0 = What's New
        /// 1 = User Permission
        /// 2 = Service Error Message
        /// </summary>
        public int ReleaseNoteType
        {
            get { return releaseNoteType; }
            set
            {
                releaseNoteType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReleaseNoteType"));
            }
        }

        public bool DownloadLaterEnabled
        {
            get { return downloadLaterEnabled; }
            set
            {
                downloadLaterEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DownloadLaterEnabled"));
            }
        }

        #endregion

        #region Constructor
        //[001][01-06-2020][cpatil]GEOS2-82 Error in obsolete version installer
        public VersionUpdateWindowViewModel(bool chkisWorkbench)
        {
            IsNetworkIP = GeosApplication.Instance.IsPrivateNetworkIP;
            ReleaseNoteButtonCommand = new RelayCommand(new Action<object>(ShowReleaseNoteAction));
            ReleaseNoteBackButtonCommand = new RelayCommand(new Action<object>(BackReleaseNoteAction));
            DownloadButtonCommand = new RelayCommand(new Action<object>(StartDownloadAction));
            CancelDownloadButtonCommand = new RelayCommand(new Action<object>(CancelDownloadAction));
            DownloadLaterButtonCommand = new RelayCommand(new Action<object>(DownloadLater));
            DoneButtonCommand = new RelayCommand(new Action<object>(DoneDownloadAction));
            AcceptDownloadButtonCommand = new RelayCommand(new Action<object>(WindowClose));
            RestoreButtonCommand = new RelayCommand(new Action<object>(RestoreButton));
            RepairButtonCommand = new RelayCommand(new Action<object>(RepairButton));
            CancelButtonCommand = new RelayCommand(new Action<object>(WindowClose));
            UpdaterWindowMinimizeCommand = new RelayCommand(new Action<object>(WindowMinimise));

            UserConfigurationSaveButtonCommand = new RelayCommand(new Action<object>(SaveUserConfiguration));
            UserConfigurationCancelButtonCommand = new RelayCommand(new Action<object>(UserConfigurationWindowClose));
            UserConfigurationShowWindowCommand = new RelayCommand(new Action<object>(UserConfigurationWindowShow));
            ServiceProvidersSelectionChangedCommand = new RelayCommand(new Action<object>(ServiceProvidersSelection));
            DownloadLaterEnabled = true;
            AcceptButtonVisibility = Visibility.Hidden;
            RepairButtonVisibility = Visibility.Hidden;
            RestoreButtonVisibility = Visibility.Hidden;
            CloseButtonVisibility = Visibility.Hidden;
            IsWorkbench = chkisWorkbench;

            updateInstalledFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\";
            updateInstalledFolder = AppDomain.CurrentDomain.BaseDirectory;

            var directory = System.IO.Path.GetDirectoryName(updateInstalledFolder);

            DirectoryInfo directoryInfo = System.IO.Directory.GetParent(directory);
            GeosFolderPath = directoryInfo.FullName;
            workbenchInstalledFolder = directoryInfo.FullName + "\\Workbench";
            backupInstalledFolder = directoryInfo.FullName + "\\Backups";

            backupRestoreZip = System.IO.Path.GetTempPath() + "\\Backup_";

            if (!File.Exists(workbenchInstalledFolder + @"\GEOSWorkbench.exe"))
            {
                VersionTitle = 0;
                LabelInstalledVersionVisibility = Visibility.Hidden;
                InstalledAssemblyVersion = string.Empty;
            }
            else
            {
                VersionTitle = 1;
                InstalledAssemblyVersion = AssemblyVersion.GetAssemblyVersion(workbenchInstalledFolder + @"\GeosWorkbench.exe").ToString();
            }

            //define commands

            if (GeosApplication.Instance.ApplicationSettings != null)
            {
                if (GeosApplication.Instance.ApplicationSettings.Count == 0)
                {
                    isApplicationSettingAvailable = false;
                }

                if (GeosApplication.Instance.ApplicationSettings != null && GeosApplication.Instance.ApplicationSettings.Count > 0)
                {
                    GetUpdatedWorkbenchVersion();
                }

                if (WorkbenchVersionNumber != null)
                {
                    ServicePath = GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString();
                    IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    try
                    {
                        GeosApplication.Instance.Logger.Log("Status=Starts,Message=Get Back up of current version", category: Category.Info, priority: Priority.Low);

                        geosWorkbenchVersion = workbenchControl.GetWorkbenchBackVersionToRestoreById(WorkbenchVersionNumber.IdGeosWorkbenchVersion);

                        GeosApplication.Instance.Logger.Log("Status=Completed,Message=Get Back up of current version", category: Category.Info, priority: Priority.Low);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor VersionUpdateWindowViewModel() -FaultException- {0}" + ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor VersionUpdateWindowViewModel() -ServiceUnexceptedException- {0}" + ex.Message), category: Category.Exception, priority: Priority.Low);
                        RestoreRepairCatch("Method Name=GetWorkbenchBackVersionToRestoreById,", "Exception in get Back up of current version in restore ...,Status=Exception " + ex.ExceptionType + "", ex.ExceptionType);
                    }

                    if (InstalledAssemblyVersion == WorkbenchVersionNumber.VersionNumber.ToString())
                    {
                        VersionTitle = 2;
                        AcceptButtonVisibility = Visibility.Visible;
                        // SameVersionOptionButtonVisibility = Visibility.Visible;

                        if (geosWorkbenchVersion != null)
                        {
                            if (File.Exists(backupInstalledFolder + @"\" + "V" + geosWorkbenchVersion.VersionNumber.ToString() + ".Zip"))
                            {
                                RestoreButtonVisibility = Visibility.Visible;
                            }
                            else
                            {
                                RestoreButtonVisibility = Visibility.Hidden;
                            }
                        }

                        RepairButtonVisibility = Visibility.Visible;
                        isRepairORRestore = true;
                        CloseButtonVisibility = Visibility.Visible;
                        DownloadNowVisibility = Visibility.Hidden;
                        DownloadLaterVisibility = Visibility.Hidden;
                    }

                    FillLanguage();
                    //[001] Added
                    GeosWorkbenchVersion geosWorkbenchVersionExpiryDate = workbenchControl.GetWorkbenchVersionByVersionNumber(InstalledAssemblyVersion.ToString());
                    if (geosWorkbenchVersionExpiryDate.ExpiryDate != null)
                    {
                        GeosApplication.Instance.Logger.Log("Getting Server date and time from server", category: Category.Info, priority: Priority.Low);
                        GeosApplication.Instance.ServerDateTime = workbenchControl.GetServerDateTime();
                        GeosApplication.Instance.Logger.Log("Getting Server date and time from server Successfully", category: Category.Info, priority: Priority.Low);

                        GeosApplication.Instance.Logger.Log("Check For Version Expiry Date", category: Category.Info, priority: Priority.Low);
                        if (GeosApplication.Instance.ServerDateTime.Date >= geosWorkbenchVersionExpiryDate.ExpiryDate.Value.Date)
                        {
                            DownloadLaterEnabled = false;
                        }
                       
                    }
                }
                else
                {
                    if (isApplicationSettingAvailable)
                    {
                        MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ShowNoVersioinAvailable"].ToString(), "Transparent", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        App.Current.Shutdown();
                    }
                }
            }
        }

        #endregion

        #region Events

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double ProgressBarvalue
        {
            get { return progressBarvalue; }
            set
            {
                progressBarvalue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProgressBarvalue"));
            }
        }

        private void WindowClose(object obj)
        {
            App.Current.Shutdown();
            //Environment.Exit(0);
        }

        private void WindowMinimise(object obj)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
            //Environment.Exit(0);
        }

        private void UserConfigurationWindowClose(object obj)
        {
            if (IsClickOnSettingButton)
            {
                SelectedViewIndex = 0;
                IsClickOnSettingButton = false;
            }
            else
            {
                WindowClose(null);
            }
        }

        private void UserConfigurationWindowShow(object obj)
        {
            IsClickOnSettingButton = true;
            SelectedViewIndex = 3;
        }

        private void ServiceProvidersSelection(object obj)
        {
            //if (GeosApplication.Instance.IsPrivateNetworkIP == true) //private ip 
            //{
            //    Ip = ((App)Application.Current).GeosServiceProviderList.Where(serviceProviderName => serviceProviderName.Name.Contains(SelectedIndexGeosServiceProviders)).Select(serviceProviderPrivateNetworkIp => serviceProviderPrivateNetworkIp.PrivateNetwork.IP).FirstOrDefault();
            //    Port = Convert.ToDecimal(((App)Application.Current).GeosServiceProviderList.Where(serviceProviderName => serviceProviderName.Name.Contains(SelectedIndexGeosServiceProviders)).Select(serviceProviderPrivateNetworkIp => serviceProviderPrivateNetworkIp.PrivateNetwork.Port).FirstOrDefault());
            //    ServicePath = ((App)Application.Current).GeosServiceProviderList.Where(serviceProviderName => serviceProviderName.Name.Contains(SelectedIndexGeosServiceProviders)).Select(serviceProviderPrivateNetworkIp => serviceProviderPrivateNetworkIp.PrivateNetwork.ServicePath).FirstOrDefault();
            //}
            //else
            //{
            //    Ip = ((App)Application.Current).GeosServiceProviderList.Where(serviceProviderName => serviceProviderName.Name.Contains(SelectedIndexGeosServiceProviders)).Select(serviceProviderPublicNetworkIp => serviceProviderPublicNetworkIp.PublicNetwork.IP).FirstOrDefault();
            //    Port = Convert.ToDecimal(((App)Application.Current).GeosServiceProviderList.Where(serviceProviderName => serviceProviderName.Name.Contains(SelectedIndexGeosServiceProviders)).Select(serviceProviderPublicNetworkIp => serviceProviderPublicNetworkIp.PublicNetwork.Port).FirstOrDefault());
            //    ServicePath = ((App)Application.Current).GeosServiceProviderList.Where(serviceProviderName => serviceProviderName.Name.Contains(SelectedIndexGeosServiceProviders)).Select(serviceProviderPublicNetworkIp => serviceProviderPublicNetworkIp.PublicNetwork.ServicePath).FirstOrDefault();
            //}

            ServicePath = ((App)Application.Current).GeosServiceProviderList.Where(serviceProviderName => serviceProviderName.Name.Contains(SelectedIndexGeosServiceProviders)).Select(serviceProviderPublicNetworkIp => serviceProviderPublicNetworkIp.ServiceProviderUrl).FirstOrDefault();
        }

        private void RepairButton(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RepairButton Started", category: Category.Exception, priority: Priority.Low);

                if (!CheckPermission())
                {
                    return;
                }

                //MessageBox.Show("I am in Repair Button");
                RepairButtonVisibility = Visibility.Hidden;
                RestoreButtonVisibility = Visibility.Hidden;
                CloseButtonVisibility = Visibility.Hidden;
                ConfigurationButtonVisibility = Visibility.Hidden;
                // SameVersionOptionButtonVisibility = Visibility.Hidden;
                CancelButtonVisibility = Visibility.Visible;
                ProcessControl.ProcessKill("GeosWorkbench", true);
                IsEnabledCancel = true;
                DownloadNowVisibility = Visibility.Hidden;
                DownloadLaterVisibility = Visibility.Hidden;
                ProgressbarVisibility = System.Windows.Visibility.Visible;
                ProgressbarImageVisibility = Visibility.Visible;
                threadDownloadProcess = new Thread(RepairButtonProcess);
                threadDownloadProcess.Name = "threadDownloadProcess";

                threadDownloadProcess.Start();
                bgwdownload = new BackgroundWorker();
                bgwdownload.WorkerReportsProgress = true;
                bgwdownload.WorkerSupportsCancellation = true;
                bgwdownload.DoWork += DownloadProgressBar_DoWork;
                bgwdownload.ProgressChanged += DownloadProgressBar_ProgressChanged;
                bgwdownload.RunWorkerAsync();

                GeosApplication.Instance.Logger.Log("Method RepairButton Completed", category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method RepairButton -Exception- {0}", category: Category.Exception, priority: Priority.Low);

                //==============================================================================================================================
                ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                SelectedViewIndex = 1;
                ReleaseNoteType = 2;
                //==============================================================================================================================
            }
        }

        private void RestoreRepairCatch(string methodname, string message, ServiceExceptionType exceptionType)
        {
            GeosApplication.Instance.Logger.Log("Method RestoreRepairCatch Started", category: Category.Info, priority: Priority.Low);

            GeosApplication.Instance.Logger.Log(methodname + message + " Status=Exception " + exceptionType + "", category: Category.Exception, priority: Priority.Low);

            string strMessage = GeosApplication.Instance.ExceptionHandlingOperationString(exceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
            ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage.ToString() });
            SelectedViewIndex = 1;
            ReleaseNoteType = 2;

            if (geosWorkbenchVersion != null)
            {
                if (File.Exists(backupInstalledFolder + @"\" + "V" + geosWorkbenchVersion.VersionNumber.ToString() + ".Zip"))
                {
                    RestoreButtonVisibility = Visibility.Visible;
                }
                else
                {
                    RestoreButtonVisibility = Visibility.Hidden;
                }
            }

            RepairButtonVisibility = Visibility.Visible;
            isRepairORRestore = true;
            CloseButtonVisibility = Visibility.Visible;
            DownloadNowVisibility = Visibility.Hidden;
            DownloadLaterVisibility = Visibility.Hidden;
            ProgressbarVisibility = System.Windows.Visibility.Hidden;
            ProgressbarImageVisibility = Visibility.Hidden;
            ProgressBarvalue = 0;
            progressbardownloadvalue = 0;
            currentstatus = InstallationStatus.Connecting;

            GeosApplication.Instance.Logger.Log("Method RestoreRepairCatch Complated", category: Category.Info, priority: Priority.Low);
        }

        private void RestoreButton(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RestoreButton Started", category: Category.Info, priority: Priority.Low);

            if (!CheckPermission())
            {
                return;
            }

            RepairButtonVisibility = Visibility.Hidden;
            RestoreButtonVisibility = Visibility.Hidden;
            CloseButtonVisibility = Visibility.Hidden;
            ConfigurationButtonVisibility = Visibility.Hidden;
            //SameVersionOptionButtonVisibility = Visibility.Hidden;
            CancelButtonVisibility = Visibility.Visible;
            ProcessControl.ProcessKill("GeosWorkbench", true);
            IsEnabledCancel = true;
            DownloadNowVisibility = Visibility.Hidden;
            DownloadLaterVisibility = Visibility.Hidden;
            ProgressbarVisibility = System.Windows.Visibility.Visible;
            ProgressbarImageVisibility = Visibility.Visible;

            threadDownloadProcess = new Thread(RestoreButtonProcess);
            threadDownloadProcess.Name = "threadDownloadProcess";

            threadDownloadProcess.Start();
            bgwdownload = new BackgroundWorker();
            bgwdownload.WorkerReportsProgress = true;
            bgwdownload.WorkerSupportsCancellation = true;
            bgwdownload.DoWork += DownloadProgressBar_DoWork;
            bgwdownload.ProgressChanged += DownloadProgressBar_ProgressChanged;
            bgwdownload.RunWorkerAsync();

            GeosApplication.Instance.Logger.Log("Method RestoreButton Completed", category: Category.Info, priority: Priority.Low);
        }

        public void RestoreButtonProcess()
        {
            ProgressBarvalue = 0;
            DevExpress.Compression.ZipArchive archive = null;
            // DownloadLogs = "Downloading Files...";
            //GeosWorkbenchVersion geosWorkbenchVersion = null;
            timerInterval = 1000;

            try
            {
                GeosApplication.Instance.Logger.Log("Method RestoreButtonProcess Started", category: Category.Info, priority: Priority.Low);

                GeosApplication.Instance.Logger.Log("Phase 1 Status=Starts,Message=Restoring process starts", category: Category.Info, priority: Priority.Low);
                Thread.Sleep(1000);
                ProgressBarvalue = 0;

                currentstatus = InstallationStatus.BackupCreatingInRestore;
                DownloadLogs = Updater.App.Current.Resources["Backup"].ToString();              //"Creating Backup Of Current Version Files.....";
                restoreTempZipFilePath = backupRestoreZip + DateTime.Now.ToString("yyyyMMddHHmmss") + ".Zip";

                try
                {
                    GeosApplication.Instance.Logger.Log("Status=Starts,Message=Get Back up of current version", category: Category.Info, priority: Priority.Low);
                    IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    geosWorkbenchVersion = workbenchControl.GetWorkbenchBackVersionToRestoreById(WorkbenchVersionNumber.IdGeosWorkbenchVersion);
                    GeosApplication.Instance.Logger.Log("Status=Completed,Message=Get Back up of current version", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Phase 1 Error in Method RestoreButtonProcess -FaultException- {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Phase 1 Error in Method RestoreButtonProcess - ServiceUnexceptedException - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    RestoreRepairCatch("Method Name=GetWorkbenchBackVersionToRestoreById,", "Exception in get Back up of current version in restore ...,Status=Exception " + ex.ExceptionType + "", ex.ExceptionType);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Phase 1 Error in Method RestoreButtonProcess - Exception - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    //==============================================================================================================================
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    //==============================================================================================================================
                }

                try
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Phase 2 Method RestoreButtonProcess - DevExpress.Compression.ZipArchive - {0}"), category: Category.Info, priority: Priority.Low);

                    using (archive = new DevExpress.Compression.ZipArchive())
                    {
                        GeosApplication.Instance.Logger.Log("Archive current workbench folder location " + workbenchInstalledFolder + " to user temp location " + restoreTempZipFilePath + " is starts in restore...", category: Category.Info, priority: Priority.Low);
                        archive.Progress += archive_Progress;
                        archive.EncryptionType = EncryptionType.PkZip;
                        archive.AddDirectory(workbenchInstalledFolder, @"\");
                        archive.Save(restoreTempZipFilePath);
                        GeosApplication.Instance.Logger.Log("Archive current workbench folder location " + workbenchInstalledFolder + " to user temp location " + restoreTempZipFilePath + " is compelted in restore ...", category: Category.Info, priority: Priority.Low);
                    }

                    GeosApplication.Instance.Logger.Log(string.Format("Phase 2 Method RestoreButtonProcess - DevExpress.Compression.ZipArchive - {0}"), category: Category.Info, priority: Priority.Low);
                    archive.Dispose();
                    archive = null;
                }
                catch (FaultException<ServiceException> ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Phase 2 Method RestoreButtonProcess - FaultException - {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Phase 2 Method RestoreButtonProcess - ServiceUnexceptedException - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    RestoreRepairCatch("", "Exception in archive current workbench folder location " + workbenchInstalledFolder + " to user temp location " + restoreTempZipFilePath + " in restore ...,Status=Exception " + ex.ExceptionType + "", ex.ExceptionType);

                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    //DownloadLogs = ex.Message;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();

                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Phase 2 Method RestoreButtonProcess - Exception - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    //==============================================================================================================================
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    //==============================================================================================================================
                }

                timerInterval = 50;

                GeosApplication.Instance.Logger.Log(string.Format("Phase 3 Method RestoreButtonProcess - progress < 49"), category: Category.Info, priority: Priority.Low);

                while (progressbardownloadvalue < 49)
                {
                    Thread.Sleep(100);
                }

                timerInterval = 300;
                progressbardownloadvalue = 50;
                ProgressBarvalue = 50;

                currentstatus = InstallationStatus.BackupCreatedInRestore;

                tempPath1 = backupInstalledFolder + @"\" + "V" + geosWorkbenchVersion.VersionNumber.ToString() + ".Zip";
                progressbardownloadvalue = 65;
                ProgressBarvalue = 65;
                currentstatus = InstallationStatus.DeleteInRestore;
                DownloadLogs = Updater.App.Current.Resources["Deletingoldworkbench"].ToString();        //"Deleting old old workbench version on system.....";

                GeosApplication.Instance.Logger.Log(string.Format("Phase 3 Method Checking workbenchInstalledFolder is installed"), category: Category.Info, priority: Priority.Low);

                if (Directory.Exists(workbenchInstalledFolder))
                {
                    try
                    {
                        setAttributesNormal(workbenchInstalledFolder);
                        setDirectoryAttributes(workbenchInstalledFolder, FileAttributes.Normal);
                        GeosApplication.Instance.Logger.Log("Deleting workbench folder from " + workbenchInstalledFolder + " is starts in restore ...,Status=Starts", category: Category.Info, priority: Priority.Low);
                        Directory.Delete(workbenchInstalledFolder, true);
                        GeosApplication.Instance.Logger.Log("Deleted workbench folder from " + workbenchInstalledFolder + "  in restore...,Status=Completed", category: Category.Info, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Phase 3 Method RestoreButtonProcess - ServiceUnexceptedException - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                        RestoreRepairCatch("", "Exception in deleting workbench folder from " + workbenchInstalledFolder + " in restore ...,Status=Exception " + ex.ExceptionType + "", ex.ExceptionType);

                        if (archive != null)
                        {
                            archive.Dispose();
                            archive = null;
                        }

                        //DownloadLogs = ex.Message;
                        ProgressbarImageVisibility = Visibility.Hidden;

                        if (bgwdownload != null)
                            bgwdownload.CancelAsync();

                        //TODO:Ravi 21 Dec
                        /*if (Thread.CurrentThread.IsAlive)
                            Thread.CurrentThread.Abort();*/
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Phase 3 Method RestoreButtonProcess - Exception {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                        //==============================================================================================================================
                        ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                        ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                        SelectedViewIndex = 1;
                        ReleaseNoteType = 2;
                        //==============================================================================================================================
                    }
                }

                Thread.Sleep(1000);

                GeosApplication.Instance.Logger.Log(string.Format("Phase 4 Method RestoreButtonProcess - progress < 75"), category: Category.Info, priority: Priority.Low);

                while (progressbardownloadvalue < 75)
                {
                    Thread.Sleep(100);
                }

                progressbardownloadvalue = 75;
                ProgressBarvalue = 75;
                currentstatus = InstallationStatus.RestoringInRestore;
                DownloadLogs = Updater.App.Current.Resources["Restoring"].ToString();

                if (File.Exists(tempPath1))
                {
                    try
                    {
                        using (archive = DevExpress.Compression.ZipArchive.Read(tempPath1))
                        {
                            GeosApplication.Instance.Logger.Log("Extracting from back file" + tempPath1 + " in workbench folder " + workbenchInstalledFolder + " is starts in restore ...,Status=Starts", category: Category.Info, priority: Priority.Low);
                            archive.Extract(workbenchInstalledFolder, AllowFileOverwriteMode.Allow);
                            GeosApplication.Instance.Logger.Log("Extracted back file in workbench folder " + workbenchInstalledFolder + " is completed in restore ...,Status=Completed", category: Category.Info, priority: Priority.Low);
                        }
                        archive.Dispose();
                        archive = null;
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Phase 4 Method RestoreButtonProcess -ServiceUnexceptedException-", ex.Message), category: Category.Exception, priority: Priority.Low);

                        RestoreRepairCatch("", "Exception in extracting back file in workbench folder " + workbenchInstalledFolder + "  in restore...,Status=Exception " + ex.ExceptionType + "", ex.ExceptionType);

                        if (archive != null)
                        {
                            archive.Dispose();
                            archive = null;
                        }

                        //DownloadLogs = ex.Message;
                        ProgressbarImageVisibility = Visibility.Hidden;

                        if (bgwdownload != null)
                            bgwdownload.CancelAsync();

                        //TODO:Ravi 21 Dec
                        /*if (Thread.CurrentThread.IsAlive)
                            Thread.CurrentThread.Abort();*/
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Phase 4 Method RestoreButtonProcess -Exception-", ex.Message), category: Category.Exception, priority: Priority.Low);

                        //==============================================================================================================================
                        ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                        ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                        SelectedViewIndex = 1;
                        ReleaseNoteType = 2;
                        //==============================================================================================================================
                    }
                }

                while (progressbardownloadvalue < 85)
                {
                    Thread.Sleep(100);
                }

                currentstatus = InstallationStatus.Complete;

                GeosApplication.Instance.Logger.Log(string.Format("Phase 5 Method RestoreButtonProcess"), category: Category.Info, priority: Priority.Low);

                try
                {
                    GeosApplication.Instance.Logger.Log("Deleting current workbench temporary file in user temp location " + restoreTempZipFilePath + " is starts in restore ...,Status=Starts", category: Category.Info, priority: Priority.Low);
                    File.SetAttributes(restoreTempZipFilePath, FileAttributes.Normal);
                    File.Delete(restoreTempZipFilePath);
                    GeosApplication.Instance.Logger.Log("Deleted full downloaded temporary file " + restoreTempZipFilePath + " is completed  in restore...,Status=Completed", category: Category.Info, priority: Priority.Low);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Phase 5 Error in Method RestoreButtonProcess -ServiceUnexceptedException-{0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    RestoreRepairCatch("", "Exception in deleting current workbench temporary file in user temp location " + restoreTempZipFilePath + "  in restore...,Status=Exception " + ex.ExceptionType + "", ex.ExceptionType);

                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    //DownloadLogs = ex.Message;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();
                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Phase 5 Error in Method RestoreButtonProcess -Exception-{0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    //==============================================================================================================================
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    //==============================================================================================================================
                }

                //following code use for after complete the process temp file deleted

                DownloadLogs = Updater.App.Current.Resources["Installed"].ToString();  // DownloadLogs = "Installed";
                progressbardownloadvalue = 100;
                ProgressBarvalue = progressbardownloadvalue;
                DoneButtonVisibility = Visibility.Visible;
                CancelButtonVisibility = Visibility.Hidden;
                ProgressbarImageVisibility = Visibility.Hidden;

                ApplicationOperation.ApplicationShortcutToDesktop("Geos Workbench", workbenchInstalledFolder + @"\GeosWorkbench.exe");
                ApplicationOperation.ApplicationShortcutToStartMenu("Geos Workbench", @"Emdep\Geos\", workbenchInstalledFolder + @"\GeosWorkbench.exe");
                GeosApplication.Instance.Logger.Log("Status=Completed,Message=Restoring process completed", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Phase 5 Error in Method RestoreButtonProcess -ServiceUnexceptedException-{0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                RestoreRepairCatch("", "Exception in restore ...,Status=Exception " + ex.ExceptionType + "", ex.ExceptionType);

                if (archive != null)
                {
                    archive.Dispose();
                    archive = null;
                }

                //DownloadLogs = ex.Message;
                ProgressbarImageVisibility = Visibility.Hidden;
                if (bgwdownload != null)
                    bgwdownload.CancelAsync();

                //TODO:Ravi 21 Dec
                /*if (Thread.CurrentThread.IsAlive)
                    Thread.CurrentThread.Abort();*/
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Phase 5 Error in Method RestoreButtonProcess -Exception-{0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                //==============================================================================================================================
                ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                SelectedViewIndex = 1;
                ReleaseNoteType = 2;
                //==============================================================================================================================
            }

            GeosApplication.Instance.Logger.Log(string.Format("Method RestoreButtonProcess completed."), category: Category.Exception, priority: Priority.Low);
        }

        double archiveValue = 0;
        void archive_Progress(object sender, ProgressEventArgs args)
        {
            archiveValue = args.Progress;
            // args.CanContinue = !this.stopProgress;
        }

        public void RepairButtonProcess()
        {
            GeosApplication.Instance.Logger.Log(string.Format("Method RepairButtonProcess Started."), category: Category.Info, priority: Priority.Low);

            currentstatus = InstallationStatus.Connecting;
            DevExpress.Compression.ZipArchive archive = null;

            try
            {
                GeosApplication.Instance.Logger.Log("Phase 1 - RepairButtonProcess", category: Category.Info, priority: Priority.Low);

                Thread.Sleep(1000);
                ProgressBarvalue = 0;

                currentstatus = InstallationStatus.Download;
                DownloadLogs = Updater.App.Current.Resources["DownloadingFiles"].ToString();        // DownloadLogs = "Downloading Files...";

                timerInterval = 1000;
                fulltemporaryinstallationdownloadpath = System.IO.Path.GetTempPath() + "Full_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".Zip";
                partialtemporaryinstallationdownloadpath = System.IO.Path.GetTempPath() + "Partial_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".Zip";
                IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosRepositoryServiceController fileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                try
                {
                    GeosApplication.Instance.Logger.Log("Status=Starts,Message=Light weight downloading file starts in repair", category: Category.Info, priority: Priority.Low);
                    fileControl.IssueDownloadRequest(WorkbenchVersionNumber, partialtemporaryinstallationdownloadpath, ProgressChangeInFileDownloadForPartial, 20);
                    GeosApplication.Instance.Logger.Log("Status=Completed,Message=Light weight downloading file completed in repair", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    GeosApplication.Instance.Logger.Log("Phase 1 Error in RepairButtonProcess() method -FaultException- " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log("Phase 1 Error in RepairButtonProcess() method -ServiceUnexceptedException- " + ex.Message, category: Category.Exception, priority: Priority.Low);

                    RestoreRepairCatch("Method Name=IssueDownloadRequest,", "Status=Exception " + ex.ExceptionType + ",Message=Light weight downloading file exception occurs in repair", ex.ExceptionType);

                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    //DownloadLogs = ex.Message;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();

                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Phase 1 Error in RepairButtonProcess() method -Exception- " + ex.Message, category: Category.Exception, priority: Priority.Low);

                    //==============================================================================================================================
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    //==============================================================================================================================
                }


                try
                {
                    if (WorkbenchVersionNumber.IdFullVersion != null)
                    {
                        if (WorkbenchVersionNumber.IdFullVersion != 0)
                        {

                            //IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
                            GeosApplication.Instance.Logger.Log("Status=Starts,Message=Get workbench version by id as per id of full version of light weight download in repair ...", category: Category.Info, priority: Priority.Low);
                            GeosWorkbenchVersion tempWorkbenchVersionNumber = workbenchControl.GetWorkbenchVersionById(Convert.ToInt32(WorkbenchVersionNumber.IdFullVersion));
                            GeosApplication.Instance.Logger.Log("Status=Completed,Message=Get workbench version by id as per id of full version of light weight download in repair...", category: Category.Info, priority: Priority.Low);
                            try
                            {
                                if (WorkbenchVersionNumber != null)
                                {

                                    GeosApplication.Instance.Logger.Log("Status=Starts,Message=Full download starts from server in repair ...", category: Category.Info, priority: Priority.Low);
                                    fileControl.IssueDownloadRequest(tempWorkbenchVersionNumber, fulltemporaryinstallationdownloadpath, ProgressChangeInFileDownload, 50);
                                    GeosApplication.Instance.Logger.Log("Status=Completed,Message=Full download completed from server in repair ...", category: Category.Info, priority: Priority.Low);
                                }
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                //GeosApplication.Instance.Logger.Log("Get an error in RepairButtonProcess() method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                RestoreRepairCatch("Method Name=IssueDownloadRequest,", "Status=Exception " + ex.ExceptionType + ",Message=Full download exception occur from server in repair", ex.ExceptionType);

                                if (archive != null)
                                {
                                    archive.Dispose();
                                    archive = null;
                                }

                                //DownloadLogs = ex.Message;
                                ProgressbarImageVisibility = Visibility.Hidden;
                                if (bgwdownload != null)
                                    bgwdownload.CancelAsync();
                                //TODO:Ravi 21 Dec
                                /*if (Thread.CurrentThread.IsAlive)
                                    Thread.CurrentThread.Abort();*/
                            }
                        }
                    }
                }
                catch (ServiceUnexceptedException ex)
                {
                    RestoreRepairCatch("Method Name=GetWorkbenchVersionById,", "Status=Exception " + ex.ExceptionType + ",Message=Get workbench version by id as per id of full version of light weight download exception occurs in repair...", ex.ExceptionType);

                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    //DownloadLogs = ex.Message;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();
                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }
                catch (Exception ex)
                {
                    //==============================================================================================================================
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    //==============================================================================================================================

                    DownloadNowVisibility = Visibility.Visible;
                    DownloadLaterVisibility = Visibility.Visible;
                    ProgressbarVisibility = System.Windows.Visibility.Hidden;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    ProgressBarvalue = 0;
                    progressbardownloadvalue = 0;
                    currentstatus = InstallationStatus.Connecting;
                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    ProgressbarImageVisibility = Visibility.Hidden;
                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();
                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }


                timerInterval = 50;
                if (WorkbenchVersionNumber.IdFullVersion != null)
                {
                    if (WorkbenchVersionNumber.IdFullVersion != 0)
                    {
                        while (progressbardownloadvalue < 49)
                        {
                            Thread.Sleep(100);
                        }
                    }
                }
                timerInterval = 300;
                progressbardownloadvalue = 50;
                ProgressBarvalue = 50;
                CancelButtonVisibility = Visibility.Hidden;
                //while (progressbardownloadvalue < 64)
                //{
                //    Thread.Sleep(100);
                //}
                progressbardownloadvalue = 65;
                ProgressBarvalue = 65;
                DownloadLogs = Updater.App.Current.Resources["Deletingoldworkbench"].ToString();//"Deleting old old workbench version on system.....";

                currentstatus = InstallationStatus.Delete;


                if (Directory.Exists(workbenchInstalledFolder))
                {
                    try
                    {
                        setAttributesNormal(workbenchInstalledFolder);
                        setDirectoryAttributes(workbenchInstalledFolder, FileAttributes.Normal);
                        GeosApplication.Instance.Logger.Log("Deleting workbench folder from " + workbenchInstalledFolder + " is starts in repair ...,Status=Starts", category: Category.Info, priority: Priority.Low);
                        Directory.Delete(workbenchInstalledFolder, true);
                        GeosApplication.Instance.Logger.Log("Deleted workbench folder from " + workbenchInstalledFolder + "  in repair...,Status=Completed", category: Category.Info, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        RestoreRepairCatch("", "Exception in deleting workbench folder from " + workbenchInstalledFolder + " in repair ...,Status=Exception " + ex.ExceptionType + "", ex.ExceptionType);

                        if (archive != null)
                        {
                            archive.Dispose();
                            archive = null;
                        }

                        //DownloadLogs = ex.Message;
                        ProgressbarImageVisibility = Visibility.Hidden;
                        if (bgwdownload != null)
                            bgwdownload.CancelAsync();
                        //TODO:Ravi 21 Dec
                        /*if (Thread.CurrentThread.IsAlive)
                            Thread.CurrentThread.Abort();*/
                    }
                    catch (Exception ex)
                    {
                        //==============================================================================================================================
                        ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                        ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                        SelectedViewIndex = 1;
                        ReleaseNoteType = 2;
                        //==============================================================================================================================

                        DownloadNowVisibility = Visibility.Visible;
                        DownloadLaterVisibility = Visibility.Visible;
                        ProgressbarVisibility = System.Windows.Visibility.Hidden;
                        ProgressbarImageVisibility = Visibility.Hidden;
                        ProgressBarvalue = 0;
                        progressbardownloadvalue = 0;
                        currentstatus = InstallationStatus.Connecting;
                        if (archive != null)
                        {
                            archive.Dispose();
                            archive = null;
                        }

                        ProgressbarImageVisibility = Visibility.Hidden;
                        if (bgwdownload != null)
                            bgwdownload.CancelAsync();
                        //TODO:Ravi 21 Dec
                        /*if (Thread.CurrentThread.IsAlive)
                            Thread.CurrentThread.Abort();*/
                    }

                }
                if (!Directory.Exists(workbenchInstalledFolder))
                {
                    try
                    {
                        GeosApplication.Instance.Logger.Log("Creating workbench folder " + workbenchInstalledFolder + " is starts in repair ...,Status=Starts", category: Category.Info, priority: Priority.Low);
                        Directory.CreateDirectory(workbenchInstalledFolder);
                        GeosApplication.Instance.Logger.Log("Created workbench folder " + workbenchInstalledFolder + " is completed  in repair...,Status=Completed", category: Category.Info, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        RestoreRepairCatch("", "Exception in creating workbench folder " + workbenchInstalledFolder + "  in repair...,Status=Exception " + ex.ExceptionType + "", ex.ExceptionType);

                        if (archive != null)
                        {
                            archive.Dispose();
                            archive = null;
                        }

                        //DownloadLogs = ex.Message;
                        ProgressbarImageVisibility = Visibility.Hidden;
                        if (bgwdownload != null)
                            bgwdownload.CancelAsync();
                        //TODO:Ravi 21 Dec
                        /*if (Thread.CurrentThread.IsAlive)
                            Thread.CurrentThread.Abort();*/
                    }
                    catch (Exception ex)
                    {
                        //==============================================================================================================================
                        ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                        ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                        SelectedViewIndex = 1;
                        ReleaseNoteType = 2;
                        //==============================================================================================================================

                        DownloadNowVisibility = Visibility.Visible;
                        DownloadLaterVisibility = Visibility.Visible;
                        ProgressbarVisibility = System.Windows.Visibility.Hidden;
                        ProgressbarImageVisibility = Visibility.Hidden;
                        ProgressBarvalue = 0;
                        progressbardownloadvalue = 0;
                        currentstatus = InstallationStatus.Connecting;
                        if (archive != null)
                        {
                            archive.Dispose();
                            archive = null;
                        }

                        ProgressbarImageVisibility = Visibility.Hidden;
                        if (bgwdownload != null)
                            bgwdownload.CancelAsync();
                        //TODO:Ravi 21 Dec
                        /*if (Thread.CurrentThread.IsAlive)
                            Thread.CurrentThread.Abort();*/
                    }

                }

                while (progressbardownloadvalue < 75)
                {
                    Thread.Sleep(100);
                }

                progressbardownloadvalue = 75;
                ProgressBarvalue = 75;

                currentstatus = InstallationStatus.Installation;
                DownloadLogs = Updater.App.Current.Resources["InstallingLatestVersion"].ToString();   //"Installing Latest Version On System.....";

                if (WorkbenchVersionNumber.IdFullVersion != null)
                {
                    if (WorkbenchVersionNumber.IdFullVersion != 0)
                    {
                        using (archive = DevExpress.Compression.ZipArchive.Read(fulltemporaryinstallationdownloadpath))
                        {
                            try
                            {
                                GeosApplication.Instance.Logger.Log("Extracting from Full downloaded file" + fulltemporaryinstallationdownloadpath + " in workbench folder  in repair" + workbenchInstalledFolder + " is starts ...,Status=Starts", category: Category.Info, priority: Priority.Low);
                                archive.Extract(workbenchInstalledFolder, AllowFileOverwriteMode.Allow);
                                GeosApplication.Instance.Logger.Log("Extracted full downloaded file in workbench folder " + workbenchInstalledFolder + " is completed  in repair...,Status=Completed", category: Category.Info, priority: Priority.Low);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                RestoreRepairCatch("", "Exception in extracting full downloaded file in workbench folder " + workbenchInstalledFolder + "  in repair..,Status=Exception " + ex.ExceptionType + "", ex.ExceptionType);

                                if (archive != null)
                                {
                                    archive.Dispose();
                                    archive = null;
                                }

                                //DownloadLogs = ex.Message;
                                ProgressbarImageVisibility = Visibility.Hidden;
                                if (bgwdownload != null)
                                    bgwdownload.CancelAsync();
                                //TODO:Ravi 21 Dec
                                /*if (Thread.CurrentThread.IsAlive)
                                    Thread.CurrentThread.Abort();*/
                            }
                            catch (Exception ex)
                            {
                                //==============================================================================================================================
                                ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                                ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                                SelectedViewIndex = 1;
                                ReleaseNoteType = 2;
                                //==============================================================================================================================

                                DownloadNowVisibility = Visibility.Visible;
                                DownloadLaterVisibility = Visibility.Visible;
                                ProgressbarVisibility = System.Windows.Visibility.Hidden;
                                ProgressbarImageVisibility = Visibility.Hidden;
                                ProgressBarvalue = 0;
                                progressbardownloadvalue = 0;
                                currentstatus = InstallationStatus.Connecting;
                                if (archive != null)
                                {
                                    archive.Dispose();
                                    archive = null;
                                }

                                ProgressbarImageVisibility = Visibility.Hidden;
                                if (bgwdownload != null)
                                    bgwdownload.CancelAsync();
                                //TODO:Ravi 21 Dec
                                /*if (Thread.CurrentThread.IsAlive)
                                    Thread.CurrentThread.Abort();*/
                            }
                        }
                        archive.Dispose();
                        archive = null;
                    }
                }

                using (archive = DevExpress.Compression.ZipArchive.Read(partialtemporaryinstallationdownloadpath))
                {
                    try
                    {
                        GeosApplication.Instance.Logger.Log("Extracting from light weight downloaded file" + partialtemporaryinstallationdownloadpath + " in workbench folder " + workbenchInstalledFolder + " is starts in repair ...,Status=Starts", category: Category.Info, priority: Priority.Low);
                        archive.Extract(workbenchInstalledFolder, AllowFileOverwriteMode.Allow);
                        GeosApplication.Instance.Logger.Log("Extracted light weight file in workbench folder " + workbenchInstalledFolder + " is completed  in repair...,Status=Completed", category: Category.Info, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        RestoreRepairCatch("", "Exception in extracting light weight downloaded file in workbench folder " + workbenchInstalledFolder + "  in repair...,Status=Exception " + ex.ExceptionType + "", ex.ExceptionType);

                        if (archive != null)
                        {
                            archive.Dispose();
                            archive = null;
                        }

                        //DownloadLogs = ex.Message;
                        ProgressbarImageVisibility = Visibility.Hidden;
                        if (bgwdownload != null)
                            bgwdownload.CancelAsync();
                        //TODO:Ravi 21 Dec
                        /*if (Thread.CurrentThread.IsAlive)
                            Thread.CurrentThread.Abort();*/
                    }
                    catch (Exception ex)
                    {
                        //==============================================================================================================================
                        ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                        ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                        SelectedViewIndex = 1;
                        ReleaseNoteType = 2;
                        //==============================================================================================================================

                        DownloadNowVisibility = Visibility.Visible;
                        DownloadLaterVisibility = Visibility.Visible;
                        ProgressbarVisibility = System.Windows.Visibility.Hidden;
                        ProgressbarImageVisibility = Visibility.Hidden;
                        ProgressBarvalue = 0;
                        progressbardownloadvalue = 0;
                        currentstatus = InstallationStatus.Connecting;
                        if (archive != null)
                        {
                            archive.Dispose();
                            archive = null;
                        }

                        ProgressbarImageVisibility = Visibility.Hidden;
                        if (bgwdownload != null)
                            bgwdownload.CancelAsync();
                        //TODO:Ravi 21 Dec
                        /*if (Thread.CurrentThread.IsAlive)
                            Thread.CurrentThread.Abort();*/
                    }
                }
                archive.Dispose();
                archive = null;

                while (progressbardownloadvalue < 95)
                {
                    Thread.Sleep(100);
                }

                if (WorkbenchVersionNumber.IdFullVersion != null)
                {
                    if (WorkbenchVersionNumber.IdFullVersion != 0)
                    {
                        //following code use for after complete the process temp file deleted 
                        try
                        {
                            GeosApplication.Instance.Logger.Log("Deleting full downloaded temporary file " + fulltemporaryinstallationdownloadpath + " is starts in repair ...,Status=Starts", category: Category.Info, priority: Priority.Low);
                            File.SetAttributes(fulltemporaryinstallationdownloadpath, FileAttributes.Normal);
                            File.Delete(fulltemporaryinstallationdownloadpath);
                            GeosApplication.Instance.Logger.Log("Deleted full downloaded temporary file " + fulltemporaryinstallationdownloadpath + " is completed  in repair...,Status=Completed", category: Category.Info, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            RestoreRepairCatch("", "Exception in deleting full downloaded temporary file " + fulltemporaryinstallationdownloadpath + "  in repair...,Status=Exception " + ex.ExceptionType + "", ex.ExceptionType);

                            if (archive != null)
                            {
                                archive.Dispose();
                                archive = null;
                            }

                            //DownloadLogs = ex.Message;
                            ProgressbarImageVisibility = Visibility.Hidden;
                            if (bgwdownload != null)
                                bgwdownload.CancelAsync();
                            //TODO:Ravi 21 Dec
                            /*if (Thread.CurrentThread.IsAlive)
                                Thread.CurrentThread.Abort();*/
                        }
                        catch (Exception ex)
                        {
                            //==============================================================================================================================
                            ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                            ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                            SelectedViewIndex = 1;
                            ReleaseNoteType = 2;
                            //==============================================================================================================================

                            DownloadNowVisibility = Visibility.Visible;
                            DownloadLaterVisibility = Visibility.Visible;
                            ProgressbarVisibility = System.Windows.Visibility.Hidden;
                            ProgressbarImageVisibility = Visibility.Hidden;
                            ProgressBarvalue = 0;
                            progressbardownloadvalue = 0;
                            currentstatus = InstallationStatus.Connecting;
                            if (archive != null)
                            {
                                archive.Dispose();
                                archive = null;
                            }

                            ProgressbarImageVisibility = Visibility.Hidden;
                            if (bgwdownload != null)
                                bgwdownload.CancelAsync();
                            //TODO:Ravi 21 Dec
                            /*if (Thread.CurrentThread.IsAlive)
                                Thread.CurrentThread.Abort();*/
                        }
                    }
                }

                try
                {
                    GeosApplication.Instance.Logger.Log("Deleting light weight downloaded temporary file " + partialtemporaryinstallationdownloadpath + " is starts  in repair...,Status=Starts", category: Category.Info, priority: Priority.Low);
                    File.SetAttributes(partialtemporaryinstallationdownloadpath, FileAttributes.Normal);
                    File.Delete(partialtemporaryinstallationdownloadpath);
                    GeosApplication.Instance.Logger.Log("Deleted light weight downloaded temporary file " + partialtemporaryinstallationdownloadpath + " is completed  in repair...,Status=Completed", category: Category.Info, priority: Priority.Low);
                }
                catch (ServiceUnexceptedException ex)
                {
                    RestoreRepairCatch("", "Exception in deleting light weight downloaded temporary file " + partialtemporaryinstallationdownloadpath + "  in repair...,Status=Exception " + ex.ExceptionType + "", ex.ExceptionType);

                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    //DownloadLogs = ex.Message;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();

                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Error in Method Repair Button process -Exception- {0}", category: Category.Exception, priority: Priority.Low);

                    //==============================================================================================================================
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    //==============================================================================================================================

                    DownloadNowVisibility = Visibility.Visible;
                    DownloadLaterVisibility = Visibility.Visible;
                    ProgressbarVisibility = System.Windows.Visibility.Hidden;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    ProgressBarvalue = 0;
                    progressbardownloadvalue = 0;
                    currentstatus = InstallationStatus.Connecting;

                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    ProgressbarImageVisibility = Visibility.Hidden;
                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();

                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }

                currentstatus = InstallationStatus.Complete;

                DownloadLogs = Updater.App.Current.Resources["Installed"].ToString();  // DownloadLogs = "Installed";
                progressbardownloadvalue = 100;
                ProgressBarvalue = progressbardownloadvalue;
                ProgressbarImageVisibility = Visibility.Hidden;
                DoneButtonVisibility = Visibility.Visible;
                CancelButtonVisibility = Visibility.Hidden;
                ProgressbarImageVisibility = Visibility.Hidden;

                // SameVersionOptionButtonVisibility = Visibility.Visible;

                ApplicationOperation.ApplicationShortcutToDesktop("Geos Workbench", workbenchInstalledFolder + @"\GeosWorkbench.exe");
                ApplicationOperation.ApplicationShortcutToStartMenu("Geos Workbench", @"Emdep\Geos\", workbenchInstalledFolder + @"\GeosWorkbench.exe");
                GeosApplication.Instance.Logger.Log("Status=Completed,Message=Repairing process completed", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                RestoreRepairCatch("", "Exception in repair process ...,Status=Exception " + ex.ExceptionType + "", ex.ExceptionType);

                if (archive != null)
                {
                    archive.Dispose();
                    archive = null;
                }

                //DownloadLogs = ex.Message;
                ProgressbarImageVisibility = Visibility.Hidden;
                if (bgwdownload != null)
                    bgwdownload.CancelAsync();

                //TODO:Ravi 21 Dec
                /*if (Thread.CurrentThread.IsAlive)
                    Thread.CurrentThread.Abort();*/
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method Repair Button process -Exception- {0}", category: Category.Exception, priority: Priority.Low);
                //==============================================================================================================================
                ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                SelectedViewIndex = 1;
                ReleaseNoteType = 2;
                //==============================================================================================================================

                DownloadNowVisibility = Visibility.Visible;
                DownloadLaterVisibility = Visibility.Visible;
                ProgressbarVisibility = System.Windows.Visibility.Hidden;
                ProgressbarImageVisibility = Visibility.Hidden;
                ProgressBarvalue = 0;
                progressbardownloadvalue = 0;
                currentstatus = InstallationStatus.Connecting;

                if (archive != null)
                {
                    archive.Dispose();
                    archive = null;
                }

                ProgressbarImageVisibility = Visibility.Hidden;
                if (bgwdownload != null)
                    bgwdownload.CancelAsync();

                //TODO:Ravi 21 Dec
                /*if (Thread.CurrentThread.IsAlive)
                    Thread.CurrentThread.Abort();*/
            }

            GeosApplication.Instance.Logger.Log("Method Repair Button process completed", category: Category.Info, priority: Priority.Low);
        }

        private void DoneDownloadAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method DoneDownloadAction started", category: Category.Info, priority: Priority.Low);

            IsEnabledCancel = true;

            if (isRepairORRestore == false)
            {
                string hostName = Dns.GetHostName(); // Retrive the Name of HOST         
                //string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString(); // Get the IP
                string myIP = ApplicationOperation.GetEmdepGroupIP("10.0");
                GeosWorkbenchVersionDownload objGeosWorkbenchVersionDownload = new Data.Common.GeosWorkbenchVersionDownload();

                objGeosWorkbenchVersionDownload.IdGeosModuleVersion = WorkbenchVersionNumber.IdGeosWorkbenchVersion;
                objGeosWorkbenchVersionDownload.UserIP = myIP;
                try
                {
                    IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    GeosApplication.Instance.Logger.Log("Status=Starts,Message=Add download version By IP process is starts", category: Category.Info, priority: Priority.Low);

                    workbenchControl.AddDownloadVersionByIP(objGeosWorkbenchVersionDownload);

                    GeosApplication.Instance.Logger.Log("Status=Completed,Message=Add download version By IP process is completed", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in DoneDownloadAction() method -FaultException-{0}" + ex.Detail.ErrorMessage), category: Category.Info, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log("Method Name=AddDownloadVersionByIP,Status=Exception " + ex.ExceptionType + ",Message=Exception in add download version By IP process ", category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                }

                try
                {
                    if (File.Exists(workbenchInstalledFolder + @"\GeosWorkbench.exe"))
                    {
                        ProcessControl.ProcessStart(workbenchInstalledFolder + @"\GeosWorkbench.exe", "0");
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in Method DoneDownloadAction -Exception- {0}", ex.Message), category: Category.Info, priority: Priority.Low);

                    //==============================================================================================================================
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    //==============================================================================================================================

                    DownloadNowVisibility = Visibility.Visible;
                    DownloadLaterVisibility = Visibility.Visible;
                    ProgressbarVisibility = System.Windows.Visibility.Hidden;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    ProgressBarvalue = 0;
                    progressbardownloadvalue = 0;
                    currentstatus = InstallationStatus.Connecting;

                    ProgressbarImageVisibility = Visibility.Hidden;

                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();

                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }
            }

            GeosApplication.Instance.Logger.Log("Method DoneDownloadAction Completed", category: Category.Info, priority: Priority.Low);
            App.Current.Shutdown();

            // Environment.Exit(0);
        }

        #endregion

        #region Cancel download
        public void CancelDownloadAction(object obj)
        {
            try
            {
                IsEnabledCancel = false;

                threadDownloadProcess.Abort();
                bgwdownload.CancelAsync();
                bgwdownload.Dispose();

                while (true)
                {
                    if (threadDownloadProcess.IsAlive == false)
                    {
                        break;
                    }
                }

                threadCancelDownloadProcess = new Thread(CancelDownloadProcess);
                threadCancelDownloadProcess.Name = "threadCancelDownloadProcess";
                threadCancelDownloadProcess.Start();

                ProgressbarImageVisibility = Visibility.Visible;
                progressbarcancelvalue = Convert.ToInt32(ProgressBarvalue);

                bgwdownloadcancel = new BackgroundWorker();
                bgwdownloadcancel.WorkerReportsProgress = true;
                bgwdownloadcancel.WorkerSupportsCancellation = true;
                bgwdownloadcancel.DoWork += CancelDownloadProgressBar_DoWork;
                bgwdownloadcancel.ProgressChanged += CancelDownloadProgressBar_DoWork;
                bgwdownloadcancel.RunWorkerCompleted += bgwdownloadcancel_RunWorkerCompleted;
                bgwdownloadcancel.RunWorkerAsync();
                SelectedViewIndex = 0;
            }
            catch (Exception)
            {
                threadCancelDownloadProcess.Abort();
                threadCancelDownloadProcess = null;

                if (bgwdownloadcancel != null && bgwdownloadcancel.IsBusy)
                    bgwdownloadcancel.CancelAsync();
                //TODO:Ravi 21 Dec
                /*if (Thread.CurrentThread.IsAlive)
                    Thread.CurrentThread.Abort();*/
            }
        }

        void bgwdownloadcancel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ProgressBarvalue == 0)
            {
                ProgressbarVisibility = System.Windows.Visibility.Hidden;

                if (InstalledAssemblyVersion == WorkbenchVersionNumber.VersionNumber.ToString())
                {
                    RepairButtonVisibility = Visibility.Visible;

                    if (File.Exists(backupInstalledFolder + @"\" + "V" + geosWorkbenchVersion.VersionNumber.ToString() + ".Zip"))
                    {
                        RestoreButtonVisibility = Visibility.Visible;
                    }
                    else
                    {
                        RestoreButtonVisibility = Visibility.Hidden;
                    }

                    CloseButtonVisibility = Visibility.Visible;
                    DownloadNowVisibility = Visibility.Hidden;
                    DownloadLaterVisibility = Visibility.Hidden;
                }
                else
                {
                    DownloadNowVisibility = Visibility.Visible;
                    DownloadLaterVisibility = Visibility.Visible;
                }

                ProgressbarImageVisibility = Visibility.Hidden;
            }
        }

        public void CancelDownloadProcess()
        {
            DevExpress.Compression.ZipArchive archive = null;

            DownloadLogs = Updater.App.Current.Resources["Restoring"].ToString();
            double tempval = progressbarcancelvalue;
            currentcancelstatus = InstallationStatus.Restore;

            try
            {
                GeosApplication.Instance.Logger.Log("Status=Starts,Message=Cancelling process starts", category: Category.Info, priority: Priority.Low);

                if (tempval > 0 && tempval <= 50)
                {
                    if (currentstatus == InstallationStatus.BackupCreatingInRestore || currentstatus == InstallationStatus.BackupCreatedInRestore)
                    {
                        if (File.Exists(restoreTempZipFilePath))
                            File.Delete(restoreTempZipFilePath);
                    }
                    else
                    {
                        try
                        {
                            if (File.Exists(fulltemporaryinstallationdownloadpath))
                                File.Delete(fulltemporaryinstallationdownloadpath);

                            if (File.Exists(partialtemporaryinstallationdownloadpath))
                                File.Delete(partialtemporaryinstallationdownloadpath);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Cancel download progress -tempval > 0 && tempval <= 50- Exception-{0}", ex.Message), category: Category.Info, priority: Priority.Low);
                        }
                    }
                }
                else if (tempval > 50 && tempval < 65)
                {
                    if (currentstatus == InstallationStatus.DeleteInRestore)
                    {
                        if (Directory.Exists(workbenchInstalledFolder))
                        {
                            setAttributesNormal(workbenchInstalledFolder);
                            setDirectoryAttributes(workbenchInstalledFolder, FileAttributes.Normal);

                            try
                            {
                                Directory.Delete(workbenchInstalledFolder, true);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log(string.Format("Cancel download progress -tempval > 0 && tempval <= 50- Exception-{0}", ex.Message), category: Category.Info, priority: Priority.Low);
                            }

                            if (File.Exists(restoreTempZipFilePath))
                            {
                                using (archive = DevExpress.Compression.ZipArchive.Read(restoreTempZipFilePath))
                                {
                                    archive.Extract(workbenchInstalledFolder);
                                }

                                archive.Dispose();
                                archive = null;
                            }
                        }
                    }
                    else if (currentstatus == InstallationStatus.Delete)
                    {
                        try
                        {
                            if (File.Exists(fulltemporaryinstallationdownloadpath))
                                File.Delete(fulltemporaryinstallationdownloadpath);
                            if (File.Exists(partialtemporaryinstallationdownloadpath))
                                File.Delete(partialtemporaryinstallationdownloadpath);

                            if (File.Exists(tempPath1))
                                File.Delete(tempPath1);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Cancel download progress -currentstatus == InstallationStatus.Delete- Exception-{0}", ex.Message), category: Category.Info, priority: Priority.Low);
                        }
                    }
                }
                else if (tempval >= 65)
                {
                    if (currentstatus == InstallationStatus.DeleteInRestore || currentstatus == InstallationStatus.RestoringInRestore)
                    {
                        if (Directory.Exists(workbenchInstalledFolder))
                        {
                            setAttributesNormal(workbenchInstalledFolder);
                            setDirectoryAttributes(workbenchInstalledFolder, FileAttributes.Normal);

                            try
                            {
                                Directory.Delete(workbenchInstalledFolder, true);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log(string.Format("Cancel download progress -Directory.Delete(workbenchInstalledFolder- Exception-{0}", ex.Message), category: Category.Info, priority: Priority.Low);
                            }

                            if (File.Exists(restoreTempZipFilePath))
                            {
                                using (archive = DevExpress.Compression.ZipArchive.Read(restoreTempZipFilePath))
                                {
                                    archive.Extract(workbenchInstalledFolder);
                                }

                                archive.Dispose();
                                archive = null;
                            }
                        }
                    }
                    else if (currentstatus == InstallationStatus.Delete || currentstatus == InstallationStatus.Installation)
                    {
                        try
                        {
                            tempPath1 = backupInstalledFolder + @"\" + "V" + backupfileversion + ".Zip";
                            if (currentstatus == InstallationStatus.Delete || currentstatus == InstallationStatus.Installation)
                            {
                                if (Directory.Exists(workbenchInstalledFolder))
                                {
                                    setAttributesNormal(workbenchInstalledFolder);
                                    setDirectoryAttributes(workbenchInstalledFolder, FileAttributes.Normal);

                                    try
                                    {
                                        Directory.Delete(workbenchInstalledFolder, true);
                                    }
                                    catch (Exception ex)
                                    {
                                        GeosApplication.Instance.Logger.Log(string.Format("Cancel download progress - Exception-{0}", ex.Message), category: Category.Info, priority: Priority.Low);
                                    }
                                }

                                tempPath1 = backupInstalledFolder + @"\" + "V" + backupfileversion + ".Zip";
                                if (File.Exists(tempPath1))
                                {
                                    using (archive = DevExpress.Compression.ZipArchive.Read(tempPath1))
                                    {
                                        archive.Extract(workbenchInstalledFolder);
                                    }

                                    archive.Dispose();
                                    archive = null;
                                }
                            }

                            if (File.Exists(tempPath1))
                            {
                                File.Delete(tempPath1);
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Cancel download progress -InstallationStatus.Delete || InstallationStatus.Installation- Exception-{0}", ex.Message), category: Category.Info, priority: Priority.Low);
                        }
                    }

                    while (ProgressBarvalue > 64)
                    {
                        Thread.Sleep(100);
                    }

                    progressbarcancelvalue = 64;
                }

                //Delete temp download files process 
                if (tempval >= 50)
                {
                    currentcancelstatus = InstallationStatus.Delete;

                    try
                    {
                        if (File.Exists(fulltemporaryinstallationdownloadpath))
                            File.Delete(fulltemporaryinstallationdownloadpath);
                        if (File.Exists(partialtemporaryinstallationdownloadpath))
                            File.Delete(partialtemporaryinstallationdownloadpath);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Cancel download progress - Exception-{0}", ex.Message), category: Category.Info, priority: Priority.Low);
                    }

                    while (ProgressBarvalue > 49)
                    {
                        Thread.Sleep(100);
                    }

                    progressbarcancelvalue = 48;
                }

                while (ProgressBarvalue > 0)
                {
                    Thread.Sleep(100);
                }

                GeosApplication.Instance.Logger.Log("Status=Completed,Message=Cancelling process completed", category: Category.Info, priority: Priority.Low);
                ConfigurationButtonVisibility = Visibility.Visible;
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Cancel download progress - ServiceUnexceptedException-{0}", ex.Message), category: Category.Info, priority: Priority.Low);

                if (archive != null)
                {
                    archive.Dispose();
                    archive = null;
                }

                ProgressbarImageVisibility = Visibility.Hidden;
                //DownloadLogs = ex.Detail.ErrorDetails;
                if (bgwdownload != null)
                    bgwdownload.CancelAsync();

                //TODO:Ravi 21 Dec
                /*if (Thread.CurrentThread.IsAlive)
                    Thread.CurrentThread.Abort();*/
            }

        }

        private void setAttributesNormal(string dir)
        {
            try
            {
                string[] files = Directory.GetFiles(dir);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in SetAttributesNormal phase 8 - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void setDirectoryAttributes(string directoryPath, FileAttributes fileAttributes)
        {
            try
            {
                var dir = new DirectoryInfo(directoryPath);
                dir.Attributes = fileAttributes;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in SetDirectoryAttributes phase 8 - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        private void CancelDownloadProgressBar_DoWork(object sender, ProgressChangedEventArgs e)
        {
            progressbardownloadvalue = 0;
            ProgressBarvalue = progressbarcancelvalue;
        }

        private void CancelDownloadProgressBar_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (bgwdownloadcancel.CancellationPending)
                {
                    break;
                }
                Thread.Sleep(100);
                if (progressbarcancelvalue > 0 && progressbarcancelvalue <= 49)
                {
                    progressbarcancelvalue--;
                    bgwdownloadcancel.ReportProgress(progressbarcancelvalue);
                }
                else if (progressbarcancelvalue >= 50 && progressbarcancelvalue < 65 && currentcancelstatus == InstallationStatus.Delete)
                {
                    progressbarcancelvalue--;
                    bgwdownloadcancel.ReportProgress(progressbardownloadvalue);
                }
                else if (progressbarcancelvalue >= 65 && currentcancelstatus == InstallationStatus.Restore)
                {
                    progressbarcancelvalue--;
                    bgwdownloadcancel.ReportProgress(progressbardownloadvalue);
                }
                else if (progressbarcancelvalue == 0)
                {
                    bgwdownloadcancel.ReportProgress(progressbarcancelvalue);
                    break;
                }
            }

        }
        #endregion

        #region Download  Process
        public void StartDownloadAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method StartDownloadAction started", category: Category.Info, priority: Priority.Low);

            ReleaseNotesList = PreInstaller();

            if (ReleaseNotesList.Count > 0)
            {
                ReleaseNoteType = 1;
                SelectedViewIndex = 1;
            }
            else
            {
                ProcessControl.ProcessKill("GeosWorkbench", true);
                IsEnabledCancel = true;
                DownloadNowVisibility = Visibility.Hidden;
                DownloadLaterVisibility = Visibility.Hidden;
                ProgressbarVisibility = System.Windows.Visibility.Visible;
                ConfigurationButtonVisibility = Visibility.Hidden;

                ProgressbarImageVisibility = Visibility.Visible;
                threadDownloadProcess = new Thread(StartDownloadProcess);
                threadDownloadProcess.Name = "threadDownloadProcess";

                threadDownloadProcess.Start();
                bgwdownload = new BackgroundWorker();
                bgwdownload.WorkerReportsProgress = true;
                bgwdownload.WorkerSupportsCancellation = true;
                bgwdownload.DoWork += DownloadProgressBar_DoWork;
                bgwdownload.ProgressChanged += DownloadProgressBar_ProgressChanged;
                bgwdownload.RunWorkerAsync();
            }

            GeosApplication.Instance.Logger.Log("Method StartDownloadAction completed", category: Category.Info, priority: Priority.Low);
        }

        public bool CheckPermission()
        {
            bool IsAccess = true;

            try
            {
                File.WriteAllText(Path.Combine(GeosFolderPath, "test.txt"), "blah blah, text");
                IsAccess = true;
                File.Delete(Path.Combine(GeosFolderPath, "test.txt"));
            }
            catch (UnauthorizedAccessException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CheckPermission -UnauthorizedAccessException- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                //==============================================================================================================================
                ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = "Access Denied to the path -" + GeosFolderPath });
                SelectedViewIndex = 1;
                ReleaseNoteType = 2;
                //==============================================================================================================================

                DownloadNowVisibility = Visibility.Visible;
                DownloadLaterVisibility = Visibility.Visible;
                ProgressbarVisibility = System.Windows.Visibility.Hidden;
                ProgressbarImageVisibility = Visibility.Hidden;
                ProgressBarvalue = 0;
                progressbardownloadvalue = 0;
                currentstatus = InstallationStatus.Connecting;

                ProgressbarImageVisibility = Visibility.Hidden;
                IsAccess = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CheckPermission - Exception- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                //==============================================================================================================================
                ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                SelectedViewIndex = 1;
                ReleaseNoteType = 2;
                //==============================================================================================================================

                DownloadNowVisibility = Visibility.Visible;
                DownloadLaterVisibility = Visibility.Visible;
                ProgressbarVisibility = System.Windows.Visibility.Hidden;
                ProgressbarImageVisibility = Visibility.Hidden;
                ProgressBarvalue = 0;
                progressbardownloadvalue = 0;
                currentstatus = InstallationStatus.Connecting;

                ProgressbarImageVisibility = Visibility.Hidden;
                IsAccess = false;
            }

            return IsAccess;
        }

        public void StartDownloadProcess()
        {
            currentstatus = InstallationStatus.Connecting;
            DevExpress.Compression.ZipArchive archive = null;

            try
            {
                if (!CheckPermission())
                {
                    return;
                }

                GeosApplication.Instance.Logger.Log("Method StartDownloadProcess started", category: Category.Info, priority: Priority.Low);

                Thread.Sleep(1000);
                ProgressBarvalue = 0;

                currentstatus = InstallationStatus.Download;
                DownloadLogs = Updater.App.Current.Resources["DownloadingFiles"].ToString(); // DownloadLogs = "Downloading Files...";

                timerInterval = 1000;
                fulltemporaryinstallationdownloadpath = System.IO.Path.GetTempPath() + "Full_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".Zip";
                partialtemporaryinstallationdownloadpath = System.IO.Path.GetTempPath() + "Partial_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".Zip";

                try
                {
                    GeosApplication.Instance.Logger.Log("Method StartDownloadProcess phase 1 ", category: Category.Info, priority: Priority.Low);

                    GeosRepositoryServiceController fileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    fileControl.IssueDownloadRequest(WorkbenchVersionNumber, partialtemporaryinstallationdownloadpath, ProgressChangeInFileDownloadForPartial, 20);

                    GeosApplication.Instance.Logger.Log("Method StartDownloadProcess phase 1 completed ", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in StartDownloadProcess() method -FaultException- {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method StartDownloadProcess - {0}", ex.ExceptionType), category: Category.Exception, priority: Priority.Low);

                    if (WorkbenchVersionNumber == null)
                    {
                        string strMessage = string.Format(System.Windows.Application.Current.FindResource("ShowNoVersioinAvailable").ToString());
                        GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                        ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                        ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage });
                        SelectedViewIndex = 1;
                        ReleaseNoteType = 3;
                    }
                    else
                    {
                        string strMessage = GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                        ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                        ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage });
                        SelectedViewIndex = 1;
                        ReleaseNoteType = 2;
                    }

                    GeosApplication.Instance.Logger.Log(string.Format("Method StartDownloadProcess Catch block - Changed visibility of buttons", ex.ExceptionType), category: Category.Exception, priority: Priority.Low);

                    DownloadNowVisibility = Visibility.Visible;
                    DownloadLaterVisibility = Visibility.Visible;
                    ProgressbarVisibility = System.Windows.Visibility.Hidden;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    ProgressBarvalue = 0;
                    progressbardownloadvalue = 0;
                    currentstatus = InstallationStatus.Connecting;
                    //==============================================================================================================================
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    //==============================================================================================================================
                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    ProgressbarImageVisibility = Visibility.Hidden;

                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();

                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method StartDownloadProcess -Exception- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    //==============================================================================================================================
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    //==============================================================================================================================
                    DownloadNowVisibility = Visibility.Visible;
                    DownloadLaterVisibility = Visibility.Visible;
                    ProgressbarVisibility = System.Windows.Visibility.Hidden;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    ProgressBarvalue = 0;
                    progressbardownloadvalue = 0;
                    currentstatus = InstallationStatus.Connecting;

                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    ProgressbarImageVisibility = Visibility.Hidden;

                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();

                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }

                try
                {
                    if (WorkbenchVersionNumber.IdFullVersion != null)
                    {
                        if (WorkbenchVersionNumber.IdFullVersion != 0)
                        {
                            GeosApplication.Instance.Logger.Log("Method StartDownloadProcess phase 2 started", category: Category.Info, priority: Priority.Low);

                            IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                            GeosWorkbenchVersion tempWorkbenchVersionNumber = workbenchControl.GetWorkbenchVersionById(Convert.ToInt32(WorkbenchVersionNumber.IdFullVersion));

                            GeosApplication.Instance.Logger.Log("Method StartDownloadProcess phase 2 completed", category: Category.Info, priority: Priority.Low);

                            try
                            {
                                if (WorkbenchVersionNumber != null)
                                {
                                    GeosApplication.Instance.Logger.Log("Method StartDownloadProcess - Full download started from server.", category: Category.Info, priority: Priority.Low);

                                    GeosRepositoryServiceController fileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                                    fileControl.IssueDownloadRequest(tempWorkbenchVersionNumber, fulltemporaryinstallationdownloadpath, ProgressChangeInFileDownload, 50);

                                    GeosApplication.Instance.Logger.Log("Method StartDownloadProcess - Full download completed from server.", category: Category.Info, priority: Priority.Low);
                                }
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                GeosApplication.Instance.Logger.Log(string.Format("Error in StartDownloadProcess() method -FaultException- {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                GeosApplication.Instance.Logger.Log(string.Format("Error in StartDownloadProcess() method -ServiceUnexceptedException- Full download exception occur from server - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                                string strMessage = GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                                ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                                ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage });
                                SelectedViewIndex = 1;
                                ReleaseNoteType = 2;
                                DownloadNowVisibility = Visibility.Visible;
                                DownloadLaterVisibility = Visibility.Visible;
                                ProgressbarVisibility = System.Windows.Visibility.Hidden;
                                ProgressbarImageVisibility = Visibility.Hidden;
                                ProgressBarvalue = 0;
                                progressbardownloadvalue = 0;
                                currentstatus = InstallationStatus.Connecting;

                                if (archive != null)
                                {
                                    archive.Dispose();
                                    archive = null;
                                }

                                ProgressbarImageVisibility = Visibility.Hidden;
                                if (bgwdownload != null)
                                    bgwdownload.CancelAsync();

                                //TODO:Ravi 21 Dec
                                /*if (Thread.CurrentThread.IsAlive)
                                    Thread.CurrentThread.Abort();*/
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log(string.Format("Error in StartDownloadProcess() method -Exception- Phase 3 - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                                //==============================================================================================================================
                                ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                                ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                                SelectedViewIndex = 1;
                                ReleaseNoteType = 2;
                                //==============================================================================================================================

                                DownloadNowVisibility = Visibility.Visible;
                                DownloadLaterVisibility = Visibility.Visible;
                                ProgressbarVisibility = System.Windows.Visibility.Hidden;
                                ProgressbarImageVisibility = Visibility.Hidden;
                                ProgressBarvalue = 0;
                                progressbardownloadvalue = 0;
                                currentstatus = InstallationStatus.Connecting;

                                if (archive != null)
                                {
                                    archive.Dispose();
                                    archive = null;
                                }

                                ProgressbarImageVisibility = Visibility.Hidden;
                                if (bgwdownload != null)
                                    bgwdownload.CancelAsync();

                                //TODO:Ravi 21 Dec
                                /*if (Thread.CurrentThread.IsAlive)
                                    Thread.CurrentThread.Abort();*/
                            }
                        }
                    }
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in Method StartDownloadProcess : {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    string strMessage = GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    DownloadNowVisibility = Visibility.Visible;
                    DownloadLaterVisibility = Visibility.Visible;
                    ProgressbarVisibility = System.Windows.Visibility.Hidden;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    ProgressBarvalue = 0;
                    progressbardownloadvalue = 0;
                    currentstatus = InstallationStatus.Connecting;

                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    ProgressbarImageVisibility = Visibility.Hidden;
                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();

                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in Method StartDownloadProcess 4 - Exception : {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    //==============================================================================================================================
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    //==============================================================================================================================

                    DownloadNowVisibility = Visibility.Visible;
                    DownloadLaterVisibility = Visibility.Visible;
                    ProgressbarVisibility = System.Windows.Visibility.Hidden;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    ProgressBarvalue = 0;
                    progressbardownloadvalue = 0;
                    currentstatus = InstallationStatus.Connecting;

                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    ProgressbarImageVisibility = Visibility.Hidden;

                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();

                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }

                timerInterval = 50;
                if (WorkbenchVersionNumber.IdFullVersion != null && WorkbenchVersionNumber.IdFullVersion != 0)
                {
                    while (progressbardownloadvalue < 49)
                    {
                        Thread.Sleep(100);
                    }
                }

                timerInterval = 300;
                progressbardownloadvalue = 50;
                ProgressBarvalue = 50;
                backupfileversion = InstalledAssemblyVersion;
                currentstatus = InstallationStatus.BackupCreating;
                DownloadLogs = Updater.App.Current.Resources["Backup"].ToString(); //"Creating Backup Of Current Version Files.....";

                try
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Method StartDownloadProcess Phase 5"), category: Category.Exception, priority: Priority.Low);

                    if (!Directory.Exists(workbenchInstalledFolder))
                    {
                        try
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Phase 5 Creating workbench folder in {0}", workbenchInstalledFolder), category: Category.Info, priority: Priority.Low);

                            Directory.CreateDirectory(workbenchInstalledFolder);

                            GeosApplication.Instance.Logger.Log(string.Format("Phase 5 completed workbench folder in {0} ", workbenchInstalledFolder), category: Category.Info, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in StartDownloadProcess Phase 5 - {0}", workbenchInstalledFolder), category: Category.Exception, priority: Priority.Low);

                            //==============================================================================================================================
                            ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                            ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                            SelectedViewIndex = 1;
                            ReleaseNoteType = 2;
                            //==============================================================================================================================

                            DownloadNowVisibility = Visibility.Visible;
                            DownloadLaterVisibility = Visibility.Visible;
                            ProgressbarVisibility = System.Windows.Visibility.Hidden;
                            ProgressbarImageVisibility = Visibility.Hidden;
                            ProgressBarvalue = 0;
                            progressbardownloadvalue = 0;
                            currentstatus = InstallationStatus.Connecting;

                            if (archive != null)
                            {
                                archive.Dispose();
                                archive = null;
                            }

                            ProgressbarImageVisibility = Visibility.Hidden;
                            if (bgwdownload != null)
                                bgwdownload.CancelAsync();

                            //TODO:Ravi 21 Dec
                            /*if (Thread.CurrentThread.IsAlive)
                                Thread.CurrentThread.Abort();*/
                        }
                    }
                    else
                    {
                        try
                        {
                            if (!Directory.Exists(backupInstalledFolder))
                            {
                                GeosApplication.Instance.Logger.Log(String.Format("Phase 6 - Creating Backup folder in " + backupInstalledFolder), category: Category.Info, priority: Priority.Low);
                                Directory.CreateDirectory(backupInstalledFolder);
                                GeosApplication.Instance.Logger.Log(String.Format("Phase 6 - Created Backup folder in " + backupInstalledFolder), category: Category.Info, priority: Priority.Low);
                            }
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.Logger.Log(String.Format("Error in StartDownloadProcess Phase 6 - backup folder in ", backupInstalledFolder), category: Category.Exception, priority: Priority.Low);

                            string strMessage = GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                            ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                            ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage });
                            SelectedViewIndex = 1;
                            ReleaseNoteType = 2;
                            DownloadNowVisibility = Visibility.Visible;
                            DownloadLaterVisibility = Visibility.Visible;
                            ProgressbarVisibility = System.Windows.Visibility.Hidden;
                            ProgressbarImageVisibility = Visibility.Hidden;
                            ProgressBarvalue = 0;
                            progressbardownloadvalue = 0;
                            currentstatus = InstallationStatus.Connecting;

                            if (archive != null)
                            {
                                archive.Dispose();
                                archive = null;
                            }

                            ProgressbarImageVisibility = Visibility.Hidden;
                            if (bgwdownload != null)
                                bgwdownload.CancelAsync();

                            //TODO:Ravi 21 Dec
                            /*if (Thread.CurrentThread.IsAlive)
                                Thread.CurrentThread.Abort();*/
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in StartDownloadProcess Phase 6 - {0}", workbenchInstalledFolder), category: Category.Exception, priority: Priority.Low);

                            //==============================================================================================================================
                            ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                            ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                            SelectedViewIndex = 1;
                            ReleaseNoteType = 2;
                            //==============================================================================================================================

                            DownloadNowVisibility = Visibility.Visible;
                            DownloadLaterVisibility = Visibility.Visible;
                            ProgressbarVisibility = System.Windows.Visibility.Hidden;
                            ProgressbarImageVisibility = Visibility.Hidden;
                            ProgressBarvalue = 0;
                            progressbardownloadvalue = 0;
                            currentstatus = InstallationStatus.Connecting;

                            if (archive != null)
                            {
                                archive.Dispose();
                                archive = null;
                            }

                            ProgressbarImageVisibility = Visibility.Hidden;
                            if (bgwdownload != null)
                                bgwdownload.CancelAsync();

                            //TODO:Ravi 21 Dec
                            /*if (Thread.CurrentThread.IsAlive)
                                Thread.CurrentThread.Abort();*/
                        }

                        tempPath1 = backupInstalledFolder + @"\" + "V" + backupfileversion + ".Zip";
                        try
                        {
                            GeosApplication.Instance.Logger.Log("Phase 7 - Archive current workbench folder location " + workbenchInstalledFolder + " to backups folder file as per version in location " + tempPath1, category: Category.Info, priority: Priority.Low);

                            using (archive = new DevExpress.Compression.ZipArchive())
                            {
                                archive.EncryptionType = EncryptionType.PkZip;
                                archive.AddDirectory(workbenchInstalledFolder, @"\");
                                archive.Save(tempPath1);
                            }
                            archive.Dispose();
                            archive = null;

                            GeosApplication.Instance.Logger.Log("Phase 7 - Archive current workbench folder location " + workbenchInstalledFolder + " to backups folder file as per version in location " + tempPath1 + " is completed", category: Category.Info, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.Logger.Log("Error in StartDownloadProcess phase 7 - Archive current workbench folder location" + workbenchInstalledFolder + " to backups folder file as per version in location " + tempPath1, category: Category.Exception, priority: Priority.Low);

                            string strMessage = GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                            ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                            ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage.ToString() });
                            SelectedViewIndex = 1;
                            ReleaseNoteType = 2;
                            DownloadNowVisibility = Visibility.Visible;
                            DownloadLaterVisibility = Visibility.Visible;
                            ProgressbarVisibility = System.Windows.Visibility.Hidden;
                            ProgressbarImageVisibility = Visibility.Hidden;
                            ProgressBarvalue = 0;
                            progressbardownloadvalue = 0;
                            currentstatus = InstallationStatus.Connecting;

                            if (archive != null)
                            {
                                archive.Dispose();
                                archive = null;
                            }

                            //DownloadLogs = ex.Message;
                            ProgressbarImageVisibility = Visibility.Hidden;
                            if (bgwdownload != null)
                                bgwdownload.CancelAsync();

                            //TODO:Ravi 21 Dec
                            /*if (Thread.CurrentThread.IsAlive)
                                Thread.CurrentThread.Abort();*/
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Error in StartDownloadProcess Phase 7 - Archive current workbench folder location" + workbenchInstalledFolder + " to backups folder file as per version in location " + tempPath1 + ex.Message, category: Category.Exception, priority: Priority.Low);

                            //==============================================================================================================================
                            ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                            ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                            SelectedViewIndex = 1;
                            ReleaseNoteType = 2;
                            //==============================================================================================================================

                            DownloadNowVisibility = Visibility.Visible;
                            DownloadLaterVisibility = Visibility.Visible;
                            ProgressbarVisibility = System.Windows.Visibility.Hidden;
                            ProgressbarImageVisibility = Visibility.Hidden;
                            ProgressBarvalue = 0;
                            progressbardownloadvalue = 0;
                            currentstatus = InstallationStatus.Connecting;

                            if (archive != null)
                            {
                                archive.Dispose();
                                archive = null;
                            }

                            ProgressbarImageVisibility = Visibility.Hidden;
                            if (bgwdownload != null)
                                bgwdownload.CancelAsync();

                            //TODO:Ravi 21 Dec
                            /*if (Thread.CurrentThread.IsAlive)
                                Thread.CurrentThread.Abort();*/
                        }
                        Thread.Sleep(1000);
                    }
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log("Error in StartDownloadProcess phase 7 - Archive current workbench folder location" + workbenchInstalledFolder + " to backups folder file as per version in location " + tempPath1, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.Logger.Log("Exception in workbench folder location  " + workbenchInstalledFolder + "...,Status=Exception " + ex.ExceptionType + "", category: Category.Exception, priority: Priority.Low);

                    string strMessage = GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    DownloadNowVisibility = Visibility.Visible;
                    DownloadLaterVisibility = Visibility.Visible;
                    ProgressbarVisibility = System.Windows.Visibility.Hidden;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    ProgressBarvalue = 0;
                    progressbardownloadvalue = 0;
                    currentstatus = InstallationStatus.Connecting;
                    //==============================================================================================================================
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    //==============================================================================================================================
                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    ProgressbarImageVisibility = Visibility.Hidden;
                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();
                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/

                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in StartDownloadProcess phase 7 -Exception- {0} {1}", workbenchInstalledFolder, ex.Message), category: Category.Exception, priority: Priority.Low);


                    //==============================================================================================================================
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    //==============================================================================================================================

                    DownloadNowVisibility = Visibility.Visible;
                    DownloadLaterVisibility = Visibility.Visible;
                    ProgressbarVisibility = System.Windows.Visibility.Hidden;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    ProgressBarvalue = 0;
                    progressbardownloadvalue = 0;
                    currentstatus = InstallationStatus.Connecting;

                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    ProgressbarImageVisibility = Visibility.Hidden;
                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();

                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }

                GeosApplication.Instance.Logger.Log(string.Format("StartDownloadProcess phase 8 - {0}", progressbardownloadvalue), category: Category.Exception, priority: Priority.Low);

                while (progressbardownloadvalue < 65)
                {
                    Thread.Sleep(100);
                }

                progressbardownloadvalue = 65;
                ProgressBarvalue = 65;
                currentstatus = InstallationStatus.BackupCreated;
                DownloadLogs = Updater.App.Current.Resources["Deletingoldworkbench"].ToString();    //"Deleting old old workbench version on system.....";

                currentstatus = InstallationStatus.Delete;

                if (Directory.Exists(workbenchInstalledFolder))
                {
                    try
                    {
                        setAttributesNormal(workbenchInstalledFolder);
                        setDirectoryAttributes(workbenchInstalledFolder, FileAttributes.Normal);
                        GeosApplication.Instance.Logger.Log("StartDownloadProcess Deleting workbench folder from " + workbenchInstalledFolder + " is starts ...,Status=Starts", category: Category.Info, priority: Priority.Low);
                        Directory.Delete(workbenchInstalledFolder, true);
                        GeosApplication.Instance.Logger.Log("StartDownloadProcess Deleted workbench folder from " + workbenchInstalledFolder + " ...,Status=Completed", category: Category.Info, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.Logger.Log("Error in method phase 8 StartDownloadProcess deleting workbench folder from " + workbenchInstalledFolder + " ...,Status=Exception " + ex.ExceptionType + "", category: Category.Exception, priority: Priority.Low);

                        string strMessage = GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                        ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                        ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage });
                        SelectedViewIndex = 1;
                        ReleaseNoteType = 2;
                        DownloadNowVisibility = Visibility.Visible;
                        DownloadLaterVisibility = Visibility.Visible;
                        ProgressbarVisibility = System.Windows.Visibility.Hidden;
                        ProgressbarImageVisibility = Visibility.Hidden;
                        ProgressBarvalue = 0;
                        progressbardownloadvalue = 0;
                        currentstatus = InstallationStatus.Connecting;

                        if (archive != null)
                        {
                            archive.Dispose();
                            archive = null;
                        }

                        //DownloadLogs = ex.Message;
                        ProgressbarImageVisibility = Visibility.Hidden;

                        if (bgwdownload != null)
                            bgwdownload.CancelAsync();

                        //TODO:Ravi 21 Dec
                        /*if (Thread.CurrentThread.IsAlive)
                            Thread.CurrentThread.Abort();*/
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in method phase 8 - {0} ", ex.Message), category: Category.Exception, priority: Priority.Low);

                        //==============================================================================================================================
                        ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                        ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                        SelectedViewIndex = 1;
                        ReleaseNoteType = 2;
                        //==============================================================================================================================

                        DownloadNowVisibility = Visibility.Visible;
                        DownloadLaterVisibility = Visibility.Visible;
                        ProgressbarVisibility = System.Windows.Visibility.Hidden;
                        ProgressbarImageVisibility = Visibility.Hidden;
                        ProgressBarvalue = 0;
                        progressbardownloadvalue = 0;
                        currentstatus = InstallationStatus.Connecting;

                        if (archive != null)
                        {
                            archive.Dispose();
                            archive = null;
                        }

                        ProgressbarImageVisibility = Visibility.Hidden;
                        if (bgwdownload != null)
                            bgwdownload.CancelAsync();
                        //TODO:Ravi 21 Dec
                        /*if (Thread.CurrentThread.IsAlive)
                            Thread.CurrentThread.Abort();*/
                    }
                }

                GeosApplication.Instance.Logger.Log(string.Format("StartDownloadProcess directory exists in method phase 9"), category: Category.Info, priority: Priority.Low);

                if (!Directory.Exists(workbenchInstalledFolder))
                {
                    try
                    {
                        GeosApplication.Instance.Logger.Log("Phase 9 Creating workbench folder " + workbenchInstalledFolder + " is starts ...,Status=Starts", category: Category.Info, priority: Priority.Low);
                        Directory.CreateDirectory(workbenchInstalledFolder);
                        GeosApplication.Instance.Logger.Log("Phase 9 Created workbench folder " + workbenchInstalledFolder + " is completed ...,Status=Completed", category: Category.Info, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.Logger.Log("Error in phase 9 creating workbench folder " + workbenchInstalledFolder + " ...,Status=Exception " + ex.ExceptionType + "", category: Category.Exception, priority: Priority.Low);

                        string strMessage = GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                        ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                        ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage });
                        SelectedViewIndex = 1;
                        ReleaseNoteType = 2;
                        DownloadNowVisibility = Visibility.Visible;
                        DownloadLaterVisibility = Visibility.Visible;
                        ProgressbarVisibility = System.Windows.Visibility.Hidden;
                        ProgressbarImageVisibility = Visibility.Hidden;
                        ProgressBarvalue = 0;
                        progressbardownloadvalue = 0;
                        currentstatus = InstallationStatus.Connecting;

                        if (archive != null)
                        {
                            archive.Dispose();
                            archive = null;
                        }

                        //DownloadLogs = ex.Message;
                        ProgressbarImageVisibility = Visibility.Hidden;
                        if (bgwdownload != null)
                            bgwdownload.CancelAsync();
                        //TODO:Ravi 21 Dec
                        /*if (Thread.CurrentThread.IsAlive)
                            Thread.CurrentThread.Abort();*/
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("StartDownloadProcess phase 9 - Exception "), category: Category.Exception, priority: Priority.Low);

                        //==============================================================================================================================
                        ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                        ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                        SelectedViewIndex = 1;
                        ReleaseNoteType = 2;
                        //==============================================================================================================================

                        DownloadNowVisibility = Visibility.Visible;
                        DownloadLaterVisibility = Visibility.Visible;
                        ProgressbarVisibility = System.Windows.Visibility.Hidden;
                        ProgressbarImageVisibility = Visibility.Hidden;
                        ProgressBarvalue = 0;
                        progressbardownloadvalue = 0;
                        currentstatus = InstallationStatus.Connecting;

                        if (archive != null)
                        {
                            archive.Dispose();
                            archive = null;
                        }

                        ProgressbarImageVisibility = Visibility.Hidden;
                        if (bgwdownload != null)
                            bgwdownload.CancelAsync();

                        //TODO:Ravi 21 Dec
                        /*if (Thread.CurrentThread.IsAlive)
                            Thread.CurrentThread.Abort();*/
                    }
                }

                GeosApplication.Instance.Logger.Log(string.Format("StartDownloadProcess Phase 10 Progress < 75"), category: Category.Info, priority: Priority.Low);

                while (progressbardownloadvalue < 75)
                {
                    Thread.Sleep(100);
                }

                progressbardownloadvalue = 75;
                ProgressBarvalue = 75;

                currentstatus = InstallationStatus.Installation;
                DownloadLogs = Updater.App.Current.Resources["InstallingLatestVersion"].ToString();   //"Installing Latest Version On System.....";

                GeosApplication.Instance.Logger.Log(string.Format("StartDownloadProcess Phase 11 Extracting zip"), category: Category.Info, priority: Priority.Low);

                if (WorkbenchVersionNumber.IdFullVersion != null)
                {
                    if (WorkbenchVersionNumber.IdFullVersion != 0)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("StartDownloadProcess Phase 11 DevExpress.Compression.ZipArchive"), category: Category.Info, priority: Priority.Low);

                        using (archive = DevExpress.Compression.ZipArchive.Read(fulltemporaryinstallationdownloadpath))
                        {
                            try
                            {
                                GeosApplication.Instance.Logger.Log("Phase 11 Extracting from Full downloaded file" + fulltemporaryinstallationdownloadpath + " in workbench folder " + workbenchInstalledFolder + " is starts ...,Status=Starts", category: Category.Info, priority: Priority.Low);
                                archive.Extract(workbenchInstalledFolder, AllowFileOverwriteMode.Allow);
                                GeosApplication.Instance.Logger.Log("Phase 11 Extracted full downloaded file in workbench folder " + workbenchInstalledFolder + " is completed ...,Status=Completed", category: Category.Info, priority: Priority.Low);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                GeosApplication.Instance.Logger.Log("Phase 11 Error in extracting full downloaded file in workbench folder " + workbenchInstalledFolder + " ...,Status=Exception " + ex.ExceptionType + "", category: Category.Exception, priority: Priority.Low);

                                string strMessage = GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                                ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                                ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage });
                                SelectedViewIndex = 1;
                                ReleaseNoteType = 2;
                                DownloadNowVisibility = Visibility.Visible;
                                DownloadLaterVisibility = Visibility.Visible;
                                ProgressbarVisibility = System.Windows.Visibility.Hidden;
                                ProgressbarImageVisibility = Visibility.Hidden;
                                ProgressBarvalue = 0;
                                progressbardownloadvalue = 0;
                                currentstatus = InstallationStatus.Connecting;

                                if (archive != null)
                                {
                                    archive.Dispose();
                                    archive = null;
                                }

                                //DownloadLogs = ex.Message;
                                ProgressbarImageVisibility = Visibility.Hidden;
                                if (bgwdownload != null)
                                    bgwdownload.CancelAsync();

                                //TODO:Ravi 21 Dec
                                /*if (Thread.CurrentThread.IsAlive)
                                    Thread.CurrentThread.Abort();*/
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log(string.Format("Error in StartDownloadProcess Phase 11 DevExpress.Compression.ZipArchive - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                                //==============================================================================================================================
                                ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                                ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                                SelectedViewIndex = 1;
                                ReleaseNoteType = 2;
                                //==============================================================================================================================

                                DownloadNowVisibility = Visibility.Visible;
                                DownloadLaterVisibility = Visibility.Visible;
                                ProgressbarVisibility = System.Windows.Visibility.Hidden;
                                ProgressbarImageVisibility = Visibility.Hidden;
                                ProgressBarvalue = 0;
                                progressbardownloadvalue = 0;
                                currentstatus = InstallationStatus.Connecting;

                                if (archive != null)
                                {
                                    archive.Dispose();
                                    archive = null;
                                }

                                ProgressbarImageVisibility = Visibility.Hidden;
                                if (bgwdownload != null)
                                    bgwdownload.CancelAsync();

                                //TODO:Ravi 21 Dec
                                /*if (Thread.CurrentThread.IsAlive)
                                    Thread.CurrentThread.Abort();*/
                            }
                        }

                        archive.Dispose();
                        archive = null;
                    }
                }

                GeosApplication.Instance.Logger.Log(string.Format("StartDownloadProcess Phase 12 partialtemporaryinstallationdownloadpath"), category: Category.Info, priority: Priority.Low);

                using (archive = DevExpress.Compression.ZipArchive.Read(partialtemporaryinstallationdownloadpath))
                {
                    try
                    {
                        GeosApplication.Instance.Logger.Log("Phase 12 Extracting from light weight downloaded file" + partialtemporaryinstallationdownloadpath + " in workbench folder " + workbenchInstalledFolder + " is starts ...,Status=Starts", category: Category.Info, priority: Priority.Low);
                        archive.Extract(workbenchInstalledFolder, AllowFileOverwriteMode.Allow);
                        GeosApplication.Instance.Logger.Log("Phase 12 Extracted light weight file in workbench folder " + workbenchInstalledFolder + " is completed ...,Status=Completed", category: Category.Info, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.Logger.Log("Phase 12 Error in extracting light weight downloaded file in workbench folder " + workbenchInstalledFolder + " ...,Status=Exception " + ex.ExceptionType + "", category: Category.Exception, priority: Priority.Low);

                        string strMessage = GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                        ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                        ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage });
                        SelectedViewIndex = 1;
                        ReleaseNoteType = 2;
                        DownloadNowVisibility = Visibility.Visible;
                        DownloadLaterVisibility = Visibility.Visible;
                        ProgressbarVisibility = System.Windows.Visibility.Hidden;
                        ProgressbarImageVisibility = Visibility.Hidden;
                        ProgressBarvalue = 0;
                        progressbardownloadvalue = 0;
                        currentstatus = InstallationStatus.Connecting;

                        if (archive != null)
                        {
                            archive.Dispose();
                            archive = null;
                        }

                        //DownloadLogs = ex.Message;
                        ProgressbarImageVisibility = Visibility.Hidden;
                        if (bgwdownload != null)
                            bgwdownload.CancelAsync();
                        //TODO:Ravi 21 Dec
                        /*if (Thread.CurrentThread.IsAlive)
                            Thread.CurrentThread.Abort();*/
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in StartDownloadProcess Phase 12 DevExpress.Compression.ZipArchive - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                        //==============================================================================================================================
                        ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                        ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                        SelectedViewIndex = 1;
                        ReleaseNoteType = 2;
                        //==============================================================================================================================

                        DownloadNowVisibility = Visibility.Visible;
                        DownloadLaterVisibility = Visibility.Visible;
                        ProgressbarVisibility = System.Windows.Visibility.Hidden;
                        ProgressbarImageVisibility = Visibility.Hidden;
                        ProgressBarvalue = 0;
                        progressbardownloadvalue = 0;
                        currentstatus = InstallationStatus.Connecting;

                        if (archive != null)
                        {
                            archive.Dispose();
                            archive = null;
                        }

                        ProgressbarImageVisibility = Visibility.Hidden;
                        if (bgwdownload != null)
                            bgwdownload.CancelAsync();

                        //TODO:Ravi 21 Dec
                        /*if (Thread.CurrentThread.IsAlive)
                            Thread.CurrentThread.Abort();*/
                    }
                }

                archive.Dispose();
                archive = null;

                GeosApplication.Instance.Logger.Log(string.Format("StartDownloadProcess Phase 13 Progress < 95"), category: Category.Info, priority: Priority.Low);

                while (progressbardownloadvalue < 95)
                {
                    Thread.Sleep(100);
                }

                try
                {
                    GeosApplication.Instance.Logger.Log(string.Format("StartDownloadProcess Phase 13 Deleting"), category: Category.Info, priority: Priority.Low);

                    if (WorkbenchVersionNumber.IdFullVersion != null)
                    {
                        if (WorkbenchVersionNumber.IdFullVersion != 0)
                        {
                            GeosApplication.Instance.Logger.Log("Phase 13 Deleting full downloaded temporary file " + fulltemporaryinstallationdownloadpath + " is starts ...,Status=Starts", category: Category.Info, priority: Priority.Low);
                            File.SetAttributes(fulltemporaryinstallationdownloadpath, FileAttributes.Normal);
                            File.Delete(fulltemporaryinstallationdownloadpath);
                            GeosApplication.Instance.Logger.Log("Phase 13 Deleted full downloaded temporary file " + fulltemporaryinstallationdownloadpath + " is completed ...,Status=Completed", category: Category.Info, priority: Priority.Low);
                        }
                    }
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log("Error in deleting full downloaded temporary file " + fulltemporaryinstallationdownloadpath + "...,Status=Exception " + ex.ExceptionType + "", category: Category.Exception, priority: Priority.Low);

                    string strMessage = GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    DownloadNowVisibility = Visibility.Visible;
                    DownloadLaterVisibility = Visibility.Visible;
                    ProgressbarVisibility = System.Windows.Visibility.Hidden;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    ProgressBarvalue = 0;
                    progressbardownloadvalue = 0;
                    currentstatus = InstallationStatus.Connecting;

                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    //DownloadLogs = ex.Message;
                    ProgressbarImageVisibility = Visibility.Hidden;

                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();

                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in StartDownloadProcess Phase 13 {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    //==============================================================================================================================
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    //==============================================================================================================================

                    DownloadNowVisibility = Visibility.Visible;
                    DownloadLaterVisibility = Visibility.Visible;
                    ProgressbarVisibility = System.Windows.Visibility.Hidden;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    ProgressBarvalue = 0;
                    progressbardownloadvalue = 0;
                    currentstatus = InstallationStatus.Connecting;

                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    ProgressbarImageVisibility = Visibility.Hidden;
                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();

                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }

                try
                {
                    GeosApplication.Instance.Logger.Log("Phase 14 Deleting light weight downloaded temporary file " + partialtemporaryinstallationdownloadpath + " is starts ...,Status=Starts", category: Category.Info, priority: Priority.Low);

                    File.SetAttributes(partialtemporaryinstallationdownloadpath, FileAttributes.Normal);
                    File.Delete(partialtemporaryinstallationdownloadpath);

                    GeosApplication.Instance.Logger.Log("Phase 14 Deleted light weight downloaded temporary file " + partialtemporaryinstallationdownloadpath + " is completed ...,Status=Completed", category: Category.Info, priority: Priority.Low);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log("Phase 14 Error in deleting light weight downloaded temporary file " + partialtemporaryinstallationdownloadpath + "...,Status=Exception " + ex.ExceptionType + "", category: Category.Exception, priority: Priority.Low);

                    string strMessage = GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    DownloadNowVisibility = Visibility.Visible;
                    DownloadLaterVisibility = Visibility.Visible;
                    ProgressbarVisibility = System.Windows.Visibility.Hidden;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    ProgressBarvalue = 0;
                    progressbardownloadvalue = 0;
                    currentstatus = InstallationStatus.Connecting;

                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    //DownloadLogs = ex.Message;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();

                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in StartDownloadProcess Phase 14 {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    //==============================================================================================================================
                    ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                    ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                    SelectedViewIndex = 1;
                    ReleaseNoteType = 2;
                    //==============================================================================================================================

                    DownloadNowVisibility = Visibility.Visible;
                    DownloadLaterVisibility = Visibility.Visible;
                    ProgressbarVisibility = System.Windows.Visibility.Hidden;
                    ProgressbarImageVisibility = Visibility.Hidden;
                    ProgressBarvalue = 0;
                    progressbardownloadvalue = 0;
                    currentstatus = InstallationStatus.Connecting;
                    if (archive != null)
                    {
                        archive.Dispose();
                        archive = null;
                    }

                    ProgressbarImageVisibility = Visibility.Hidden;
                    if (bgwdownload != null)
                        bgwdownload.CancelAsync();
                    //TODO:Ravi 21 Dec
                    /*if (Thread.CurrentThread.IsAlive)
                        Thread.CurrentThread.Abort();*/
                }
                currentstatus = InstallationStatus.Complete;

                GeosApplication.Instance.Logger.Log(string.Format("Phase 14 StartDownloadProcess Installation complete"), category: Category.Info, priority: Priority.Low);

                DownloadLogs = Updater.App.Current.Resources["Installed"].ToString();   // DownloadLogs = "Installed";
                progressbardownloadvalue = 100;
                ProgressBarvalue = progressbardownloadvalue;
                DoneButtonVisibility = Visibility.Visible;
                CancelButtonVisibility = Visibility.Hidden;
                ProgressbarImageVisibility = Visibility.Hidden;

                //Environment.SpecialFolder.Desktop
                //object allUsersDesktop = "AllUsersDesktop";
                //IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShellClass();
                //string _path = shell.SpecialFolders.Item(ref allUsersDesktop).ToString();

                //string _path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                // MessageBox.Show(_path);
                ApplicationOperation.ApplicationShortcutToDesktop("Geos Workbench", workbenchInstalledFolder + @"\GeosWorkbench.exe");
                ApplicationOperation.ApplicationShortcutToStartMenu("Geos Workbench", @"Emdep\Geos\", workbenchInstalledFolder + @"\GeosWorkbench.exe");
                GeosApplication.Instance.Logger.Log("Status=Completed,Message=Completed download process", category: Category.Info, priority: Priority.Low);

            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Phase 14 Error in start downloading process ...,Status=Exception " + ex.ExceptionType + "", category: Category.Exception, priority: Priority.Low);

                string strMessage = GeosApplication.Instance.ExceptionHandlingOperationString(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = strMessage });
                SelectedViewIndex = 1;
                ReleaseNoteType = 2;
                DownloadNowVisibility = Visibility.Visible;
                DownloadLaterVisibility = Visibility.Visible;
                ProgressbarVisibility = System.Windows.Visibility.Hidden;
                ProgressbarImageVisibility = Visibility.Hidden;
                ProgressBarvalue = 0;
                progressbardownloadvalue = 0;
                currentstatus = InstallationStatus.Connecting;

                if (archive != null)
                {
                    archive.Dispose();
                    archive = null;
                }

                //DownloadLogs = ex.Message;
                ProgressbarImageVisibility = Visibility.Hidden;
                if (bgwdownload != null)
                    bgwdownload.CancelAsync();
                //TODO:Ravi 21 Dec
                /*if (Thread.CurrentThread.IsAlive)
                    Thread.CurrentThread.Abort();*/
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Phase 14 Error in StartDownloadProcess - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                //==============================================================================================================================
                ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                SelectedViewIndex = 1;
                ReleaseNoteType = 2;
                //==============================================================================================================================

                DownloadNowVisibility = Visibility.Visible;
                DownloadLaterVisibility = Visibility.Visible;
                ProgressbarVisibility = System.Windows.Visibility.Hidden;
                ProgressbarImageVisibility = Visibility.Hidden;
                ProgressBarvalue = 0;
                progressbardownloadvalue = 0;
                currentstatus = InstallationStatus.Connecting;

                if (archive != null)
                {
                    archive.Dispose();
                    archive = null;
                }

                ProgressbarImageVisibility = Visibility.Hidden;
                if (bgwdownload != null)
                    bgwdownload.CancelAsync();

                //TODO:Ravi 21 Dec
                /*if (Thread.CurrentThread.IsAlive)
                    Thread.CurrentThread.Abort();*/
            }
            finally
            {
                GeosApplication.Instance.Logger.Log(string.Format("Phase 14 Completed"), category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log(string.Format("StartDownloadProcess Completed method execution"), category: Category.Exception, priority: Priority.Low);
        }

        private void ProgressChangeInFileDownloadForPartial(int value)
        {
            if (value < 20 && value > 0)
            {
                progressbardownloadvalue = value;
                ProgressBarvalue = progressbardownloadvalue;
            }
        }

        private void ProgressChangeInFileDownload(int value)
        {
            if (value > 20 && value < 50)
            {
                progressbardownloadvalue = value;
                ProgressBarvalue = progressbardownloadvalue;
            }
        }

        private void DownloadProgressBar_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (bgwdownload.CancellationPending)
                {
                    e.Cancel = true;

                    break;
                }

                Thread.Sleep(timerInterval);

                if (progressbardownloadvalue < 49 && (currentstatus == InstallationStatus.Download || currentstatus == InstallationStatus.BackupCreatingInRestore))
                {
                    if (currentstatus == InstallationStatus.BackupCreatingInRestore)
                    {
                        progressbardownloadvalue = 1 + Convert.ToInt16(48 * archiveValue);
                    }

                    bgwdownload.ReportProgress(progressbardownloadvalue);
                }
                else if (progressbardownloadvalue >= 50 && progressbardownloadvalue < 65 && (currentstatus == InstallationStatus.BackupCreating || currentstatus == InstallationStatus.BackupCreatedInRestore))
                {
                    progressbardownloadvalue++;
                    bgwdownload.ReportProgress(progressbardownloadvalue);
                }
                else if (progressbardownloadvalue >= 65 && progressbardownloadvalue < 75 && (currentstatus == InstallationStatus.Delete || currentstatus == InstallationStatus.DeleteInRestore))
                {
                    progressbardownloadvalue++;
                    bgwdownload.ReportProgress(progressbardownloadvalue);
                }
                else if (progressbardownloadvalue >= 75 && progressbardownloadvalue < 95 && (currentstatus == InstallationStatus.Installation || currentstatus == InstallationStatus.RestoringInRestore))
                {
                    progressbardownloadvalue++;
                    bgwdownload.ReportProgress(progressbardownloadvalue);
                }
                else if (progressbardownloadvalue >= 98 && progressbardownloadvalue < 100 && currentstatus == InstallationStatus.Complete)
                {
                    progressbardownloadvalue++;
                    bgwdownload.ReportProgress(progressbardownloadvalue);
                }
                if (progressbardownloadvalue == 100)
                {
                    break;
                }
            }

        }

        private void DownloadProgressBar_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarvalue = progressbardownloadvalue;
        }

        #endregion

        #region Methods

        public void DownloadLater(object obj)
        {
            ProcessControl.ProcessKill("GeosWorkbench", true);

            if (IsWorkbench)
            {
               if (File.Exists(workbenchInstalledFolder + @"\GeosWorkbench.exe"))
                {
                    ProcessControl.ProcessStart(workbenchInstalledFolder + @"\GeosWorkbench.exe", "0");
                }
            }

            App.Current.Shutdown();
        }

        public void ShowReleaseNoteAction(object obj)
        {
            GetReleaseNoteDetail();
            SelectedViewIndex = 1;
        }

        public void BackReleaseNoteAction(object obj)
        {
            SelectedViewIndex = 0;
            if (ProgressbarVisibility != Visibility.Visible)
            {
                if (isRepairORRestore)
                {
                    DownloadLaterVisibility = Visibility.Hidden;
                    DownloadNowVisibility = Visibility.Hidden;
                    RepairButtonVisibility = Visibility.Visible;
                    RestoreButtonVisibility = Visibility.Visible;
                    CloseButtonVisibility = Visibility.Visible;
                }
                else
                {
                    RepairButtonVisibility = Visibility.Hidden;
                    RestoreButtonVisibility = Visibility.Hidden;
                    CloseButtonVisibility = Visibility.Hidden;
                    DownloadLaterVisibility = Visibility.Visible;
                    DownloadNowVisibility = Visibility.Visible;
                }
            }
        }

        public void GetUpdatedWorkbenchVersion()
        {
            GeosApplication.Instance.Logger.Log("Method GetUpdatedWorkbenchVersion started", category: Category.Info, priority: Priority.Low);

            IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

            User user = workbenchControl.GetUserByLoginName(System.Environment.UserName);

            if (user == null)
            {
                if (GeosApplication.Instance.GeosWorkbenchVersion == null)
                {
                    GeosApplication.Instance.GeosWorkbenchVersion = workbenchControl.GetCurrentPublishVersion();
                }
                else
                {
                    GeosApplication.Instance.GeosWorkbenchVersion = workbenchControl.GetCurrentPublishVersion();
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method GetUpdatedWorkbenchVersion 1 GeosWorkbenchVersion - {0}", GeosApplication.Instance.GeosWorkbenchVersion.VersionNumber), category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.GeosWorkbenchVersion != null && GeosApplication.Instance.GeosWorkbenchVersion.IsPublish == 1)
                {
                    LatestAssemblyVersion = GeosApplication.Instance.GeosWorkbenchVersion.VersionNumber;
                    WorkbenchVersionNumber = GeosApplication.Instance.GeosWorkbenchVersion;
                }
            }
            else
            {
                try
                {
                    if (GeosApplication.Instance.GeosWorkbenchVersion == null)// Publish version
                    {
                        GeosApplication.Instance.GeosWorkbenchVersion = workbenchControl.GetCurrentPublishVersion();
                    }
                    else
                    {
                        GeosApplication.Instance.GeosWorkbenchVersion = workbenchControl.GetCurrentPublishVersion();
                    }

                    GeosApplication.Instance.Logger.Log(string.Format("Method GetUpdatedWorkbenchVersion 2 GeosWorkbenchVersion - {0}", GeosApplication.Instance.GeosWorkbenchVersion.VersionNumber), category: Category.Info, priority: Priority.Low);

                    WorkbenchVersionNumber = workbenchControl.GetUserIsBetaCurrentVersion(user.IdUser); //beta version

                    if (WorkbenchVersionNumber == null)
                    {
                        if (GeosApplication.Instance.GeosWorkbenchVersion != null && GeosApplication.Instance.GeosWorkbenchVersion.IsPublish == 1)
                        {
                            LatestAssemblyVersion = GeosApplication.Instance.GeosWorkbenchVersion.VersionNumber;// publish version
                            WorkbenchVersionNumber = GeosApplication.Instance.GeosWorkbenchVersion;
                        }
                    }
                    else if (GeosApplication.Instance.GeosWorkbenchVersion == null)
                    {
                        LatestAssemblyVersion = WorkbenchVersionNumber.VersionNumber;// beta version
                        WorkbenchVersionNumber = WorkbenchVersionNumber;
                    }
                    else
                    {
                        if (WorkbenchVersionNumber.IdGeosWorkbenchVersion < GeosApplication.Instance.GeosWorkbenchVersion.IdGeosWorkbenchVersion)
                        {
                            LatestAssemblyVersion = GeosApplication.Instance.GeosWorkbenchVersion.VersionNumber;// publish version
                            WorkbenchVersionNumber = GeosApplication.Instance.GeosWorkbenchVersion;
                        }
                        else
                        {
                            LatestAssemblyVersion = WorkbenchVersionNumber.VersionNumber;   // beta version
                            WorkbenchVersionNumber = WorkbenchVersionNumber;
                        }
                    }

                    GeosApplication.Instance.Logger.Log(string.Format("Method GetUpdatedWorkbenchVersion phase 1 completed"), category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method GetUpdatedWorkbenchVersion -FaultException- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    SelectedViewIndex = 0;
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method GetUpdatedWorkbenchVersion -ServiceUnexceptedException- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                }

                if (WorkbenchVersionNumber != null)
                {
                    try
                    {
                        GeosApplication.Instance.Logger.Log("Method GetUpdatedWorkbenchVersion - Get latest version process started", category: Category.Info, priority: Priority.Low);

                        geosWorkbenchVersion = workbenchControl.GetWorkbenchBackVersionToRestoreById(WorkbenchVersionNumber.IdGeosWorkbenchVersion);

                        GeosApplication.Instance.Logger.Log(string.Format("Version - {0}", geosWorkbenchVersion.VersionNumber), category: Category.Info, priority: Priority.Low);

                        if (user != null)
                        {
                            bool isBetaTester = workbenchControl.IsGeosWorkbenchVersionBetaTester(WorkbenchVersionNumber.IdGeosWorkbenchVersion, user.IdUser);

                            if (WorkbenchVersionNumber.IsBeta == 1 && isBetaTester == true)
                            {
                                LatestAssemblyVersion = WorkbenchVersionNumber.VersionNumber + " (Beta)";
                                GeosApplication.Instance.Logger.Log(string.Format("Beta and tester version - {0}", LatestAssemblyVersion), category: Category.Info, priority: Priority.Low);
                            }
                            else
                            {
                                LatestAssemblyVersion = WorkbenchVersionNumber.VersionNumber;
                                GeosApplication.Instance.Logger.Log(string.Format("Publish version - {0}", LatestAssemblyVersion), category: Category.Info, priority: Priority.Low);
                            }

                            if (InstalledAssemblyVersion == LatestAssemblyVersion)
                            {
                                VersionTitle = 2;
                                AcceptButtonVisibility = Visibility.Visible;

                                if (geosWorkbenchVersion != null)
                                {
                                    if (File.Exists(backupInstalledFolder + @"\" + "V" + geosWorkbenchVersion.VersionNumber.ToString() + ".Zip"))
                                    {
                                        RestoreButtonVisibility = Visibility.Visible;
                                    }
                                    else
                                    {
                                        RestoreButtonVisibility = Visibility.Hidden;
                                    }
                                }
                                RepairButtonVisibility = Visibility.Visible;

                                isRepairORRestore = true;
                                CloseButtonVisibility = Visibility.Visible;
                                DownloadNowVisibility = Visibility.Hidden;
                                DownloadLaterVisibility = Visibility.Hidden;
                            }

                            GeosApplication.Instance.Logger.Log(string.Format("This is the latest version-{0}", LatestAssemblyVersion), category: Category.Info, priority: Priority.Low);
                        }
                        else
                        {
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            GeosApplication.Instance.Logger.Log(string.Format("GetUpdatedWorkbenchVersion -message-{0}", Application.Current.FindResource("SoftwareUpdateViewUser")), category: Category.Info, priority: Priority.Low);
                            CustomMessageBox.Show(Application.Current.FindResource("SoftwareUpdateViewUser").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }

                        GeosApplication.Instance.Logger.Log("Method GetUpdatedWorkbenchVersion is completed", category: Category.Info, priority: Priority.Low);
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in GetUpdatedWorkbenchVersion() method {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in GetUpdatedWorkbenchVersion() method-{0}" + ex.Message), category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        //==============================================================================================================================
                        GeosApplication.Instance.Logger.Log(string.Format("Error in GetUpdatedWorkbenchVersion() method-{0}" + ex.Message), category: Category.Exception, priority: Priority.Low);

                        ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
                        ReleaseNotesList.Add(new GeosReleaseNote() { IdType = 2, Description = ex.Message.ToString() });
                        SelectedViewIndex = 1;
                        ReleaseNoteType = 2;
                        //==============================================================================================================================
                    }
                }
            }
        }

        public void GetReleaseNoteDetail()
        {
            IsBusy = true;

            try
            {
                IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                GeosApplication.Instance.Logger.Log("Status=Starts,Message=Release note starts", category: Category.Info, priority: Priority.Low);
                ReleaseNotesList = new ObservableCollection<GeosReleaseNote>(workbenchControl.GetReleaseNotesByVersion(WorkbenchVersionNumber));
                ReleaseNoteType = 0;
                GeosApplication.Instance.Logger.Log("Status=Completed,Message=Release note completed", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in GetUpdatedWorkbenchVersion() method-{0}" + ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in GetUpdatedWorkbenchVersion() method-{0}" + ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }

            IsBusy = false;
        }


        /// <summary>
        /// This method is for to Fill language image in list as per Culture
        /// </summary>
        private void FillLanguage()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLanguage() started", category: Category.Info, priority: Priority.Low);

                IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                Languages = workbenchControl.GetAllLanguage();

                for (int i = 0; i < languages.Count; i++)
                {
                    Languages[i].LanguageImage = GetImage("/Assets/Images/" + Languages[i].Name + ".gif");
                }

                GeosApplication.Instance.Logger.Log("Method FillLanguage() completed", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in FillLanguage() method -FaultException- {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillLanguage() -ServiceUnexceptedException- {0}" + ex.ExceptionType), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
        }

        /// <summary>
        /// Method for save application setting.
        /// </summary>
        /// <param name="obj"></param>
        public void SaveUserConfiguration(object obj)
        {
            IsBusy = true;
            GeosApplication.Instance.Logger.Log("Method SaveUserConfiguration() started", category: Category.Info, priority: Priority.Low);

            ((App)Application.Current).GeosServiceProviderList = ((App)Application.Current).GeosServiceProviderList.Select(ServiceProvider => { if (ServiceProvider.Name.Contains(SelectedIndexGeosServiceProviders)) ServiceProvider.IsSelected = true; return ServiceProvider; }).ToList();
            string tempSelectedIndexGeosServiceProviders = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.Name).FirstOrDefault();

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

                    GeosApplication.Instance.GeosServiceProviderList = ((App)Application.Current).GeosServiceProviderList.Select(serviceProvider =>
                    {
                        if (serviceProvider.Name.Contains(tempSelectedIndexGeosServiceProviders))
                            serviceProvider.IsSelected = false;

                        if (serviceProvider.Name.Contains(SelectedIndexGeosServiceProviders))
                        {
                            serviceProvider.IsSelected = true;
                            serviceProvider.ServiceProviderUrl = ServicePath;
                        }

                        return serviceProvider;
                    }).ToList();

                    GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider = GeosApplication.Instance.GeosServiceProviderList;
                    GeosApplication.Instance.WriteApplicationSettingFile(GeosApplication.Instance.GeosServiceProviderList);

                    //if (IsNetworkIP == true)
                    //{
                    //    GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"] = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.PrivateNetwork.IP).FirstOrDefault();
                    //    GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"] = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.PrivateNetwork.Port).FirstOrDefault();
                    //    GeosApplication.Instance.ApplicationSettings["ServicePath"] = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.PrivateNetwork.ServicePath).FirstOrDefault();
                    //}
                    //else
                    //{
                    //    GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"] = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.PublicNetwork.IP).FirstOrDefault();
                    //    GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"] = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.PublicNetwork.Port).FirstOrDefault();
                    //    GeosApplication.Instance.ApplicationSettings["ServicePath"] = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.PublicNetwork.ServicePath).FirstOrDefault();
                    //}

                    GeosApplication.Instance.ApplicationSettings["ServicePath"] = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.ServiceProviderUrl).FirstOrDefault();

                    bool IsExceptionIsThrown = false;

                    try
                    {
                        IsBusy = true;
                        IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                        CreateApplicationFile(workbenchControl);

                        GeosApplication.Instance.ServerDateTime = workbenchControl.GetServerDateTime();

                        if (GeosApplication.Instance.ApplicationSettings != null && GeosApplication.Instance.ApplicationSettings.Count > 0)
                            GetUpdatedWorkbenchVersion();

                        IsBusy = false;
                        SelectedViewIndex = 0;
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        IsExceptionIsThrown = true;
                        GeosApplication.Instance.Logger.Log(string.Format("Error in SaveUserConfiguration() method -FaultException- {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        IsExceptionIsThrown = true;
                        IsBusy = false;
                        GeosApplication.Instance.Logger.Log(string.Format("Error in SaveUserConfiguration() method -FaultException- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    finally
                    {
                        // This code is written in finally block because it is to be exceuted in all catch blocks.
                        if (IsExceptionIsThrown)        // If any exception then revert changes.
                        {
                            string temp = string.Copy(SelectedIndexGeosServiceProviders);
                            SelectedIndexGeosServiceProviders = tempSelectedIndexGeosServiceProviders;

                            //if (IsNetworkIP)
                            //{
                            //    GeosApplication.Instance.GeosServiceProviderList = ((App)Application.Current).GeosServiceProviderList.Select(serviceProvider =>
                            //    {
                            //        if (serviceProvider.Name.Contains(temp)) serviceProvider.IsSelected = false;
                            //        if (serviceProvider.Name.Contains(SelectedIndexGeosServiceProviders)) { serviceProvider.IsSelected = true; serviceProvider.PrivateNetwork.IP = Ip; serviceProvider.PrivateNetwork.Port = Port.ToString(); }
                            //        return serviceProvider;
                            //    }).ToList();

                            //    GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"] = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.PrivateNetwork.IP).FirstOrDefault();
                            //    GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"] = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.PrivateNetwork.Port).FirstOrDefault();
                            //    GeosApplication.Instance.ApplicationSettings["ServicePath"] = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.PrivateNetwork.ServicePath).FirstOrDefault();
                            //}
                            //else
                            //{
                            //    GeosApplication.Instance.GeosServiceProviderList = ((App)Application.Current).GeosServiceProviderList.Select(serviceProvider =>
                            //    {
                            //        if (serviceProvider.Name.Contains(temp)) serviceProvider.IsSelected = false;
                            //        if (serviceProvider.Name.Contains(SelectedIndexGeosServiceProviders)) { serviceProvider.IsSelected = true; serviceProvider.PublicNetwork.IP = Ip; serviceProvider.PublicNetwork.Port = Port.ToString(); }
                            //        return serviceProvider;
                            //    }).ToList();

                            //    GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"] = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.PublicNetwork.IP).FirstOrDefault();
                            //    GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"] = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.PublicNetwork.Port).FirstOrDefault();
                            //    GeosApplication.Instance.ApplicationSettings["ServicePath"] = ((App)Application.Current).GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(c => c.PublicNetwork.ServicePath).FirstOrDefault();
                            //}

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

                    //XmlSerializer ser = new XmlSerializer(typeof(GeosServiceProviders));
                    //StringWriterUTF8 writer = new StringWriterUTF8();
                    //string xmlSerializerStringFile = null;
                    //XmlSerializerNamespaces xmlSerializerns = new XmlSerializerNamespaces();
                    //xmlSerializerns.Add("", "");

                    //StreamWriter readStream = new StreamWriter(GeosApplication.Instance.ApplicationSettingFilePath);
                    //ser.Serialize(readStream, GeosApplication.Instance.GeosServiceProviders, xmlSerializerns);
                    //readStream.Close();

                    //GeosApplication.Instance.ApplicationSettings.Add("ServiceProviderIP", objGeosServiceProvider.PrivateNetwork.IP);
                    //GeosApplication.Instance.ApplicationSettings.Add("ServiceProviderPort", objGeosServiceProvider.PrivateNetwork.Port);
                }
                else
                {
                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("UserConfigurationWindowIpError").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in last SaveUserConfiguration() method -Exception- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            IsBusy = false;
        }

        /// <summary>
        /// Method if there is some changes in application setting then create new one as per database.
        /// </summary>
        private void CreateApplicationFile(IWorkbenchStartUp control)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateApplicationFile started 1", category: Category.Info, priority: Priority.Low);

                int count = 0;

                List<GeosProvider> geosProviderList = new List<GeosProvider>();
                List<Company> UnCommonCompanyList = new List<Company>();

                List<Company> CompanyList = control.GetCompanyList();

                UnCommonCompanyList = CompanyList.Where(p => !GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Any(emp => p.Alias == emp.Name)).ToList();
                GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(g => CompanyList.Any(p => g.Name == p.Alias)).ToList();

                List<GeosProvider> finalGeosProviderList = control.GetGeosProviderList();

                geosProviderList = finalGeosProviderList.Where(gs => UnCommonCompanyList.Any(unc => unc.IdCompany == gs.IdCompany)).ToList();
                geosProviderList = geosProviderList.Where(gs => UnCommonCompanyList.Any(unc => unc.IdCompany == gs.IdCompany)).ToList();

                GeosApplication.Instance.Logger.Log("Method CreateApplicationFile get data 2", category: Category.Info, priority: Priority.Low);

                if (geosProviderList != null && geosProviderList.Count > 0)
                {
                    foreach (var item in geosProviderList)
                    {
                        GeosServiceProvider geosServiceProvider = new GeosServiceProvider();
                        geosServiceProvider.Name = UnCommonCompanyList.Where(u => u.IdCompany == item.IdCompany).Select(i => i.Alias).FirstOrDefault();
                        geosServiceProvider.IsSelected = false;
                        geosServiceProvider.ServiceProviderUrl = UnCommonCompanyList.Where(u => u.IdCompany == item.IdCompany).Select(i => i.Alias).FirstOrDefault().ToString().ToLower() + "." + "emdep.com:" + item.ServiceServerPublicPort + "/WebServices";

                        GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Add(geosServiceProvider);
                        count = 1;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method CreateApplicationFile completed loop 3", category: Category.Info, priority: Priority.Low);

                foreach (var itemGS in finalGeosProviderList.Where(p => GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Any(gs => gs.ServiceProviderUrl != p.ServiceProviderUrl)).ToList())
                {
                    GeosServiceProvider geosServiceProvider = new GeosServiceProvider();
                    geosServiceProvider = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(gs => gs.Name == itemGS.Company.Alias).FirstOrDefault();
                    int indexof = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.FindIndex(gs => gs.Name == itemGS.Company.Alias);
                    GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider[indexof].ServiceProviderUrl = itemGS.ServiceProviderUrl;
                    count = 1;
                }

                GeosApplication.Instance.Logger.Log("Method CreateApplicationFile completed loop 4", category: Category.Info, priority: Priority.Low);

                if (count == 1)
                    GeosApplication.Instance.WriteApplicationSettingFile(GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider);

                ((App)Application.Current).GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;

                GeosApplication.Instance.Logger.Log("Method CreateApplicationFile completed ", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in CreateApplicationFile() Method -FaultException-{0}" + ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in CreateApplicationFile() Method - ServiceUnexceptedException - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in CreateApplicationFile() Method -Exception- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///  This method is for to get image in bitmap.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        /// <summary>
        /// This method is to get all preinstaller notes
        /// </summary>
        /// <returns>List of all preinstaller notes</returns>
        public ObservableCollection<GeosReleaseNote> PreInstaller()
        {
            ReleaseNotesList = new ObservableCollection<GeosReleaseNote>();
            bool checkAccess = false;

            try
            {
                GeosApplication.Instance.Logger.Log("Method PreInstaller Started", category: Category.Info, priority: Priority.Low);

                foreach (var process in Process.GetProcessesByName("GeosWorkbench"))
                {
                    GeosReleaseNote geosReleaseNote = new GeosReleaseNote();
                    geosReleaseNote.IdReleaseNote = 1;
                    geosReleaseNote.IdType = 3;
                    geosReleaseNote.Description = "Workbench Application is running";
                    ReleaseNotesList.Add(geosReleaseNote);
                    break;
                }

                // Check Access permission for folder C:\Users\username\AppData\Local\Temp
                GeosApplication.Instance.Logger.Log("Method PreInstaller 1.Workbench Application is running 2. Check access permission", category: Category.Info, priority: Priority.Low);

                try
                {
                    checkAccess = ApplicationOperation.HasWriteAccessToDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp");
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Exception in method PreInstaller 1 -Exception-{0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    GeosReleaseNote geosReleaseNote = new GeosReleaseNote();
                    geosReleaseNote.IdReleaseNote = 2;
                    geosReleaseNote.IdType = 2;
                    geosReleaseNote.Description = "Unauthorized access in " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp";
                    ReleaseNotesList.Add(geosReleaseNote);
                }

                ////Check Space for folder C:\Users\username\AppData\Local\Temp
                //DriveInfo driveTemp = null;
                //bool checkSpace = false;
                //try
                //{
                //    driveTemp = new DriveInfo(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp"));
                //    checkSpace = ApplicationOperation.HasSpaceAvaiableInDrive(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp", WorkbenchVersionNumber.FileSize);
                //    if (!checkSpace)
                //    {
                //        GeosReleaseNote geosReleaseNote = new GeosReleaseNote();
                //        geosReleaseNote.IdReleaseNote = 3;
                //        geosReleaseNote.IdType = 3;
                //        geosReleaseNote.Description = "No Free space available in " + driveTemp.Name + " drive";
                //        ReleaseNotesList.Add(geosReleaseNote);
                //    }
                //}
                //catch (Exception)
                //{
                //    GeosReleaseNote geosReleaseNote = new GeosReleaseNote();
                //    geosReleaseNote.IdReleaseNote = 3;
                //    geosReleaseNote.IdType = 3;
                //    geosReleaseNote.Description = "No Free space available in " + driveTemp.Name + " drive";
                //    ReleaseNotesList.Add(geosReleaseNote);
                //}

                // Check Access permission for folder Workbench
                // string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Replace(@"\Geos Workbench Installer", "").ToString();

                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Replace(@"\Installer", "").ToString();
                GeosApplication.Instance.Logger.Log(string.Format("Method PreInstaller 3.Check access permission - {0}", path), category: Category.Info, priority: Priority.Low);

                try
                {
                    checkAccess = ApplicationOperation.HasWriteAccessToDirectory(path);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Exception in method PreInstaller 2 -Exception-{0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    GeosReleaseNote geosReleaseNote = new GeosReleaseNote();
                    geosReleaseNote.IdReleaseNote = 4;
                    geosReleaseNote.IdType = 2;
                    geosReleaseNote.Description = "Unauthorized access in " + path;
                    ReleaseNotesList.Add(geosReleaseNote);
                }

                ////Check Space for folder Workbench
                //bool checkWorkbenchSpace = false;
                //try
                //{
                //    checkWorkbenchSpace = ApplicationOperation.HasSpaceAvaiableInDrive(Path.GetPathRoot(path), WorkbenchVersionNumber.FileSize);
                //    if (!checkSpace)
                //    {
                //        GeosReleaseNote geosReleaseNote = new GeosReleaseNote();
                //        geosReleaseNote.IdReleaseNote = 5;
                //        geosReleaseNote.IdType = 3;
                //        geosReleaseNote.Description = "No Free space available in " + "C:\\" + " drive";
                //        ReleaseNotesList.Add(geosReleaseNote);
                //    }
                //}
                //catch (Exception)
                //{

                //    GeosReleaseNote geosReleaseNote = new GeosReleaseNote();
                //    geosReleaseNote.IdReleaseNote = 5;
                //    geosReleaseNote.IdType = 3;
                //    geosReleaseNote.Description = "No Free space available in " + "C:\\" + " drive";
                //    ReleaseNotesList.Add(geosReleaseNote);
                //}

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Exception in method PreInstaller 3 -Exception-{0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return ReleaseNotesList;
        }

        #endregion
    }
}


