using DevExpress.Mvvm;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Utility;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Drawing.Printing;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    // [nsatpute][02-09-2024][GEOS2-5414] CCI-370 & CCI-420 Download Test Table Documentation from ECOS & Documentation in USB Pen Drive (9/10) 

    class MyPreferencesViewModel : ViewModelBase, INotifyPropertyChanged
    {

        #region Services

        //private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        //IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

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

        #endregion // End Of Events 

        #region Declarations
        private bool isAutoRefresh;
        private Visibility refreshIntervalVisibility;
        private int? autoRefreshInterval;
        private string defaultLabelPrinter;
        private ObservableCollection<string> printerList;
        private ObservableCollection<LookupValue> modelList;
        private LookupValue selectedModel;
        private List<string> parallelPortList;
        private string selectedParallelPort;
        private string defaultPrinter;
        #endregion

        #region Properties
        public bool IsAutoRefresh
        {
            get
            {
                return isAutoRefresh;
            }
            set
            {
                isAutoRefresh = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAutoRefresh"));

                if (IsAutoRefresh)
                {
                    RefreshIntervalVisibility = Visibility.Visible;
                }
                else
                {
                    RefreshIntervalVisibility = Visibility.Collapsed;
                }
            }
        }

        public Visibility RefreshIntervalVisibility
        {
            get
            {
                return refreshIntervalVisibility;
            }
            set
            {
                refreshIntervalVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RefreshIntervalVisibility"));
            }
        }

        public int? AutoRefreshInterval
        {
            get
            {
                return autoRefreshInterval;
            }
            set
            {
                autoRefreshInterval = value;
                OnParentViewModelChanged(new PropertyChangedEventArgs("AutoRefreshInterval"));
            }
        }

        #region [nsatpute][02-09-2024][GEOS2-5414] 
        public ObservableCollection<string> PrinterList
        {
            get { return printerList; }
            set { printerList = value; OnPropertyChanged(new PropertyChangedEventArgs("PrinterList")); }
        }
        public string DefaultLabelPrinter
        {
            get { return defaultLabelPrinter; }
            set
            {
                defaultLabelPrinter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DefaultLabelPrinter"));
            }
        }
        public ObservableCollection<LookupValue> ModelList
        {
            get { return modelList; }
            set
            {
                modelList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModelList"));

            }
        }
        public LookupValue SelectedModel
        {
            get { return selectedModel; }
            set
            {
                selectedModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedModel"));
            }
        }
        public List<string> ParallelPortList
        {
            get { return parallelPortList; }
            set
            {
                parallelPortList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParallelPortList"));
            }
        }
        public string SelectedParallelPort
        {
            get { return selectedParallelPort; }
            set
            {
                selectedParallelPort = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedParallelPort"));
            }
        }
        public string DefaultPrinter
        {
            get { return defaultPrinter; }
            set
            {
                defaultPrinter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DefaultPrinter"));
            }
        }
        #endregion

        #endregion

        #region ICommand

        public ICommand MyPreferencesViewCancelButtonCommand { get; set; }
        public ICommand MyPreferencesViewAcceptButtonCommand { get; set; }


        #endregion

        #region Constructor
        public MyPreferencesViewModel()
        {
            MyPreferencesViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            MyPreferencesViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveMyPreference));

            if (GeosApplication.Instance.UserSettings.ContainsKey("SAMAutoRefresh"))
            {
                if (GeosApplication.Instance.UserSettings["SAMAutoRefresh"].ToString() == "Yes")
                {
                    IsAutoRefresh = true;

                    if (GeosApplication.Instance.UserSettings["SAMAutoRefresh"].ToString() != "0")
                    {
                        AutoRefreshInterval = Convert.ToInt32(GeosApplication.Instance.UserSettings["SAMAutoRefreshInterval"]);
                    }
                    else
                    {
                        AutoRefreshInterval = 0;
                    }
                }
                else
                {
                    IsAutoRefresh = false;
                    AutoRefreshInterval = Convert.ToInt32(GeosApplication.Instance.UserSettings["SAMAutoRefreshInterval"]);
                }
            }
            PrinterList = new ObservableCollection<string>(GeosApplication.Instance.FillPrinterList());
            FillModelList();
            FillPortList();
            LoadPrinterDetailsFromUserSetting();
        }

        // [nsatpute][02-09-2024][GEOS2-5414] CCI-370 & CCI-420 Download Test Table Documentation from ECOS & Documentation in USB Pen Drive (9/10) 
        private void FillModelList()
        {
            try
            {
                ModelList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(38));

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SAM_LabelPrinterModel"))
                {
                    if (ModelList != null && ModelList.Count > 0)
                    {
                        SelectedModel = ModelList.FirstOrDefault(x => x.Value == GeosApplication.Instance.UserSettings["SAM_LabelPrinterModel"]);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillModelList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // [nsatpute][02-09-2024][GEOS2-5414] CCI-370 & CCI-420 Download Test Table Documentation from ECOS & Documentation in USB Pen Drive (9/10) 
        private void FillPortList()
        {
            try
            {
                ParallelPortList = new List<string>();


                for (int port = 1; port < 10; port++)
                {
                    ParallelPortList.Add("LPT" + port);
                }
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SAM_ParallelPort"))
                {
                    if (ParallelPortList != null && ParallelPortList.Count > 0)
                    {
                        SelectedParallelPort = ParallelPortList.FirstOrDefault(x => x == GeosApplication.Instance.UserSettings["SAM_ParallelPort"]);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPortList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        #region Methods

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        // [nsatpute][02-09-2024][GEOS2-5414] CCI-370 & CCI-420 Download Test Table Documentation from ECOS & Documentation in USB Pen Drive (9/10) 
        private void SaveMyPreference(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SaveMyPreference..."), category: Category.Info, priority: Priority.Low);

                List<Tuple<string, string>> userConfigurations = new List<Tuple<string, string>>();

                if (GeosApplication.Instance.UserSettings != null
                   && GeosApplication.Instance.UserSettings.ContainsKey("SAMAutoRefresh")
                   && GeosApplication.Instance.UserSettings.ContainsKey("SAMAutoRefreshInterval"))
                {
                    if (IsAutoRefresh)
                    {
                        GeosApplication.Instance.UserSettings["SAMAutoRefresh"] = "Yes";
                        GeosApplication.Instance.UserSettings["SAMAutoRefreshInterval"] = Convert.ToString(AutoRefreshInterval);
                    }
                    else
                    {
                        GeosApplication.Instance.UserSettings["SAMAutoRefresh"] = "No";
                        GeosApplication.Instance.UserSettings["SAMAutoRefreshInterval"] = Convert.ToString(5);
                    }
                }

                foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                {
                    userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                }

                ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");

                if (IsAutoRefresh)
                {
                    SAMCommon.Instance.AutoRefresh = "ON";
                    SAMCommon.Instance.AutoRefreshInterval = Convert.ToString(AutoRefreshInterval);
                }
                else
                {
                    SAMCommon.Instance.AutoRefresh = "OFF";
                    SAMCommon.Instance.AutoRefreshInterval = null;
                }

                //Default Printer.
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SAM_SelectedPrinter"))
                {
                    if (DefaultPrinter != null)
                        GeosApplication.Instance.UserSettings["SAM_SelectedPrinter"] = DefaultPrinter;
                }
                else
                {
                    if (DefaultPrinter != null)
                        GeosApplication.Instance.UserSettings["SAM_SelectedPrinter"] = DefaultPrinter;
                }

                //Label Printer
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SAM_DefaultLabelPrinter"))
                {
                    if (DefaultLabelPrinter != null)
                        GeosApplication.Instance.UserSettings["SAM_DefaultLabelPrinter"] = DefaultLabelPrinter;
                }
                else
                {
                    if (DefaultLabelPrinter != null)
                        GeosApplication.Instance.UserSettings["SAM_DefaultLabelPrinter"] = DefaultLabelPrinter;
                }

                //Parallel Port.
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SAM_ParallelPort"))
                {
                    if (SelectedParallelPort != null)
                        GeosApplication.Instance.UserSettings["SAM_ParallelPort"] = SelectedParallelPort;
                }
                else
                {
                    if (SelectedParallelPort != null)
                        GeosApplication.Instance.UserSettings["SAM_ParallelPort"] = SelectedParallelPort;
                }
                //Parallel Port.
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SAM_LabelPrinterModel"))
                {
                    if (SelectedModel != null)
                        GeosApplication.Instance.UserSettings["SAM_LabelPrinterModel"] = SelectedModel.ToString();
                }
                else
                {
                    if (SelectedModel != null)
                        GeosApplication.Instance.UserSettings["SAM_LabelPrinterModel"] = SelectedModel.ToString();
                }

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log(string.Format("Method SaveMyPreference..."), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method SaveMyPreference()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][02-09-2024][GEOS2-5414] 
        private void LoadPrinterDetailsFromUserSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method LoadPrinterDetailsFromUserSetting..."), category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SAM_DefaultLabelPrinter"))
                {
                    DefaultLabelPrinter = GeosApplication.Instance.UserSettings["SAM_DefaultLabelPrinter"].ToString();
                }
                else
                {
                    PrinterSettings LabelPrintersettings = new PrinterSettings();
                    DefaultLabelPrinter = LabelPrintersettings.PrinterName;
                }

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SAM_SelectedPrinter"))
                {
                    DefaultPrinter = GeosApplication.Instance.UserSettings["SAM_SelectedPrinter"].ToString();
                }

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SAM_ParallelPort"))
                {
                    SelectedParallelPort = GeosApplication.Instance.UserSettings["SAM_ParallelPort"].ToString();
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method LoadPrinterDetailsFromUserSetting..."), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method LoadPrinterDetailsFromUserSetting()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
