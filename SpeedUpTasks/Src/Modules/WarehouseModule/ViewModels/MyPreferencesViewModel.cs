using DevExpress.Mvvm;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Emdep.Geos.Utility;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using WarehouseCommon.Wmi;
using Emdep.Geos.Data.Common;
using System.Windows;
using Emdep.Geos.Hardware.Balances;
using Emdep.Geos.Hardware;
using System.IO.Ports;
using DevExpress.Xpf.LayoutControl;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class MyPreferencesViewModel
    {
        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Task Log
        //[001][skale][16/10/2019][GEOS2-1783]- Ajuste pantalla picking
        #endregion

        #region Declaration

        public List<string> PrinterList { get; set; }
        private string defaultPrinter;
        GeosApplication gapp;
        private ObservableCollection<LookupValue> modelList;
        //private int selectedIndexForModel;
        //private List<string> parallelPortList;
        private List<string> parallelPortList;
        private string defaultLabelPrinter;
        private LookupValue selectedModel;
        private string selectedParallelPort;
        private Warehouses selectedwarehouse;
        private bool isStartTimer=false;
        private List<string> scaleModelList;
        private string selectedScaleModel;
        private List<string> parityList;
        private List<string> portList;
        private List<string> stopBitsList;
        private List<string> supportedBaudRates;
        private List<string> supportedDataBits;
        private string selectedParity;
        private string selectedPort;
        private string selectedStopBit;
        private string selectedBaudRate;
        private string selectedDataBit;
        private bool isComSettingsEnable;
        //[001] added
        MaximizedElementPosition selectedAppearanceItem;
        private List<Currency> currencies;
        private Currency selectedCurrency;
             
        #endregion

        #region public ICommand

        public ICommand MyPreferencesViewAcceptButtonCommand { get; set; }
        public ICommand MyPreferencesViewCancelButtonCommand { get; set; }

        #endregion // ICommand

        #region Constructor
        /// <summary>
        /// [001][skale][16/10/2019][GEOS2-1783]- Ajuste pantalla picking
        /// </summary>
        public MyPreferencesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor MyPreferencesViewModel ...", category: Category.Info, priority: Priority.Low);

                MyPreferencesViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveMyPreference));
                MyPreferencesViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                PrinterList = GeosApplication.Instance.FillPrinterList();

                //Paper
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedPrinter"))
                {
                    DefaultPrinter = GeosApplication.Instance.UserSettings["SelectedPrinter"].ToString();
                }
                else
                {
                    PrinterSettings settings = new PrinterSettings();
                    DefaultPrinter = settings.PrinterName;
                }

                //Label
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("LabelPrinter"))
                {
                    DefaultLabelPrinter = GeosApplication.Instance.UserSettings["LabelPrinter"].ToString();
                }
                else
                {
                    PrinterSettings LabelPrintersettings = new PrinterSettings();
                    DefaultLabelPrinter = LabelPrintersettings.PrinterName;
                }
                if (WarehouseCommon.Instance.WarehouseList != null && WarehouseCommon.Instance.WarehouseList.Count > 0)
                {
                    //Setting Default warehouse
                    if (GeosApplication.Instance.UserSettings.ContainsKey("SelectedwarehouseId"))
                    {
                        if (WarehouseCommon.Instance.WarehouseList.FirstOrDefault(x => x.IdWarehouse.ToString() == GeosApplication.Instance.UserSettings["SelectedwarehouseId"]) != null)
                            Selectedwarehouse = WarehouseCommon.Instance.WarehouseList.FirstOrDefault(x => x.IdWarehouse.ToString() == GeosApplication.Instance.UserSettings["SelectedwarehouseId"]);
                        else
                            Selectedwarehouse = WarehouseCommon.Instance.WarehouseList[0];
                    }

                    else
                    {
                        Selectedwarehouse = WarehouseCommon.Instance.WarehouseList[0];
                    }
                }
                //Start Timer
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PickingTimer"))
                {

                    if (Convert.ToBoolean(GeosApplication.Instance.UserSettings["PickingTimer"].ToString()))
                        IsStartTimer = true;
                }
                //[001] added
                //Appearance
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Appearance"))
                {

                    if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["Appearance"].ToString()))
                    {
                        SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), "Right", true);
                    }
                    else
                    {
                        SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), GeosApplication.Instance.UserSettings["Appearance"].ToString(), true);
                    }
                }
                else
                {
                    SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), "Right", true);
                }

                FillModelList();
                FillPortList();
                
                FillScaleModelList();
                FillCurrencyDetails();
                GeosApplication.Instance.Logger.Log("Constructor MyPreferencesViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in MyPreferencesViewModel() Constructor " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        #endregion

        #region Properties

        public string DefaultPrinter
        {
            get { return defaultPrinter; }
            set
            {
                defaultPrinter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DefaultPrinter"));
            }
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

        public List<string> ParallelPortList
        {
            get { return parallelPortList; }
            set
            {
                parallelPortList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParallelPortList"));
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

        public string SelectedParallelPort
        {
            get { return selectedParallelPort; }
            set
            {
                selectedParallelPort = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedParallelPort"));
            }
        }
        public Warehouses Selectedwarehouse
        {
            get { return selectedwarehouse; }
            set
            {
                selectedwarehouse = value;

            }
        }

        public bool IsStartTimer
        {
            get
            {
                return isStartTimer;
            }

            set
            {
                isStartTimer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsStartTimer"));
            }
        }

        public List<string> ScaleModelList
        {
            get
            {
                return scaleModelList;
            }

            set
            {
                scaleModelList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ScaleModelList"));
            }
        }

        public string SelectedScaleModel
        {
            get
            {
                return selectedScaleModel;
            }

            set
            {
                selectedScaleModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedScaleModelIndex"));
            }
        }


        public List<string> ParityList
        {
            get
            {
                return parityList;
            }

            set
            {
                parityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParityList"));
            }
        }

        public List<string> PortList
        {
            get
            {
                return portList;
            }

            set
            {
                portList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PortList"));
            }
        }

        public List<string> StopBitsList
        {
            get
            {
                return stopBitsList;
            }

            set
            {
                stopBitsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StopBitsList"));
            }
        }

        public List<string> SupportedBaudRates
        {
            get
            {
                return supportedBaudRates;
            }

            set
            {
                supportedBaudRates = value; 
                OnPropertyChanged(new PropertyChangedEventArgs("SupportedBaudRates"));
            }
        }

        public List<string> SupportedDataBits
        {
            get
            {
                return supportedDataBits;
            }

            set
            {
                supportedDataBits = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SupportedDataBits"));
            }
        }

        public string SelectedParity
        {
            get
            {
                return selectedParity;
            }

            set
            {
                selectedParity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedParity"));
            }
        }

        public string SelectedPort
        {
            get
            {
                return selectedPort;
            }

            set
            {
                selectedPort = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPort"));
            }
        }

        public string SelectedStopBit
        {
            get
            {
                return selectedStopBit;
            }

            set
            {
                selectedStopBit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStopBit"));
            }
        }

        public string SelectedBaudRate
        {
            get
            {
                return selectedBaudRate;
            }

            set
            {
                selectedBaudRate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBaudRate"));
            }
        }

        public string SelectedDataBit
        {
            get
            {
                return selectedDataBit;
            }

            set
            {
                selectedDataBit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDataBit"));
            }
        }

        public bool IsComSettingsEnable
        {
            get
            {
                return isComSettingsEnable;
            }

            set
            {
                isComSettingsEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsComSettingsEnable"));
            }
        }
        //[001] added
        public MaximizedElementPosition SelectedAppearanceItem
        {
            get { return selectedAppearanceItem; }
            set
            {
                selectedAppearanceItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAppearanceItem"));
            }
        }
        public List<Currency> Currencies
        {
            get
            {
                return currencies;
            }

            set
            {
                currencies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Currencies"));
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
        #endregion

        #region Methods

        /// <summary>
        /// Method to save preferences.
        /// [001][skale][16/10/2019][GEOS2-1783]- Ajuste pantalla picking
        /// </summary>
        private void SaveMyPreference(object obj)
        {
            List<Tuple<string, string>> userConfigurations = new List<Tuple<string, string>>();

            try
            {
                //Default Printer.
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedPrinter"))
                {
                    GeosApplication.Instance.UserSettings["SelectedPrinter"] = DefaultPrinter;
                }
                else
                {
                    GeosApplication.Instance.UserSettings["SelectedPrinter"] = DefaultPrinter;
                }

                //Label Printer
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("LabelPrinter"))
                {
                    GeosApplication.Instance.UserSettings["LabelPrinter"] = DefaultLabelPrinter;
                }
                else
                {
                    GeosApplication.Instance.UserSettings["LabelPrinter"] = DefaultLabelPrinter;
                }

                //Label Printer
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("LabelPrinterModel"))
                {
                    if (SelectedModel != null)
                        GeosApplication.Instance.UserSettings["LabelPrinterModel"] = SelectedModel.Value;
                }
                else
                {
                    if (SelectedModel != null)
                        GeosApplication.Instance.UserSettings["LabelPrinterModel"] = SelectedModel.Value;
                }

                //Parallel Port.
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ParallelPort"))
                {
                    if (SelectedParallelPort != null)
                        GeosApplication.Instance.UserSettings["ParallelPort"] = SelectedParallelPort;
                }
                else
                {
                    if (SelectedParallelPort != null)
                        GeosApplication.Instance.UserSettings["ParallelPort"] = SelectedParallelPort;
                }


                //Ware House .
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedwarehouseId"))
                {
                    if (Selectedwarehouse != null)
                        GeosApplication.Instance.UserSettings["SelectedwarehouseId"] = Selectedwarehouse.IdWarehouse.ToString();
                }
                else
                {
                    if (Selectedwarehouse != null)
                        GeosApplication.Instance.UserSettings["SelectedwarehouseId"] = Selectedwarehouse.IdWarehouse.ToString(); ;
                }

                //Timer
                if(GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PickingTimer"))
                {
                    GeosApplication.Instance.UserSettings["PickingTimer"] = IsStartTimer.ToString();
                }

                //Scale Model
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedScaleModel"))
                {
                    if(SelectedScaleModel != null)
                        GeosApplication.Instance.UserSettings["SelectedScaleModel"] = SelectedScaleModel;
                    else
                        GeosApplication.Instance.UserSettings["SelectedScaleModel"] = "";
                }

                //Selected Port
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedPort"))
                {
                    if(SelectedPort != null)
                        GeosApplication.Instance.UserSettings["SelectedPort"] = SelectedPort;
                    else
                        GeosApplication.Instance.UserSettings["SelectedPort"] = "";
                }

                //Selected Parity Bit
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedParity"))
                {
                    if(SelectedParity != null)
                        GeosApplication.Instance.UserSettings["SelectedParity"] = SelectedParity;
                    else
                        GeosApplication.Instance.UserSettings["SelectedParity"] = "";
                }

                //Selected Stop Bit
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedStopBit"))
                {
                    if(SelectedStopBit != null)
                        GeosApplication.Instance.UserSettings["SelectedStopBit"] = SelectedStopBit;
                    else
                        GeosApplication.Instance.UserSettings["SelectedStopBit"] = "";
                }

                //Selected Baud Rate
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedBaudRate"))
                {
                    if(SelectedBaudRate != null)
                        GeosApplication.Instance.UserSettings["SelectedBaudRate"] = SelectedBaudRate;
                    else
                        GeosApplication.Instance.UserSettings["SelectedBaudRate"] = "";
                }

                //Selected Data Bit
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedDataBit"))
                {
                    if (SelectedDataBit != null)
                        GeosApplication.Instance.UserSettings["SelectedDataBit"] = SelectedDataBit;
                    else
                        GeosApplication.Instance.UserSettings["SelectedDataBit"] = "";
                    
                }
                //Appearance
                // [001] added 
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Appearance"))
                {
                    GeosApplication.Instance.UserSettings["Appearance"] = SelectedAppearanceItem.ToString();
                }

                //selected currency
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedCurrency_Warehouse"))
                {
                    GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"] = SelectedCurrency.Name;
                }

                foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                {
                    userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                }

                ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");               
                GeosApplication.Instance.SelectedPrinter = Convert.ToString(GeosApplication.Instance.UserSettings["SelectedPrinter"]);
                GeosApplication.Instance.LabelPrinter = Convert.ToString(GeosApplication.Instance.UserSettings["LabelPrinter"]);
                GeosApplication.Instance.LabelPrinterModel = Convert.ToString(GeosApplication.Instance.UserSettings["LabelPrinterModel"]);
                GeosApplication.Instance.ParallelPort = Convert.ToString(GeosApplication.Instance.UserSettings["ParallelPort"]);
                WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur = false;
                WarehouseCommon.Instance.Selectedwarehouse = WarehouseCommon.Instance.WarehouseList.FirstOrDefault(x => x.IdWarehouse.ToString() == GeosApplication.Instance.UserSettings["SelectedwarehouseId"]);
                WarehouseCommon.Instance.IsWarehouseChangedEventCanOccur = true;
                WarehouseCommon.Instance.IsPickingTimer = Convert.ToBoolean(IsStartTimer);
                WarehouseCommon.Instance.SelectedScaleModel = SelectedScaleModel;
                WarehouseCommon.Instance.SelectedPort  = SelectedPort;
                WarehouseCommon.Instance.SelectedBaudRate = SelectedBaudRate;
                WarehouseCommon.Instance.SelectedParity = SelectedParity;
                WarehouseCommon.Instance.SelectedDataBit = SelectedDataBit;
                WarehouseCommon.Instance.SelectedStopBit = SelectedStopBit;
                WarehouseCommon.Instance.Appearance = SelectedAppearanceItem.ToString();
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in SaveMyPreference() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for close Window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void FillModelList()
        {
            try
            {
                ModelList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(38));

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("LabelPrinterModel"))
                {
                    if (ModelList != null && ModelList.Count > 0)
                    {
                        SelectedModel = ModelList.FirstOrDefault(x => x.Value == GeosApplication.Instance.UserSettings["LabelPrinterModel"]);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillModelList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillPortList()
        {
            try
            {
                //    Connection wmiConnection = new Connection();
                //    Win32_ParallelPort win32_ParallelPort = new Win32_ParallelPort(wmiConnection);
                ParallelPortList = new List<string>();


                for (int port = 1; port < 10; port++)
                {
                    ParallelPortList.Add("LPT" + port);
                }

                //foreach (var parallelPort in win32_ParallelPort.GetPropertyValues())
                //{
                //    ParallelPortList.Add(parallelPort);
                //}

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ParallelPort"))
                {
                    if (ParallelPortList != null && ParallelPortList.Count > 0)
                    {
                        SelectedParallelPort = ParallelPortList.FirstOrDefault(x => x == GeosApplication.Instance.UserSettings["ParallelPort"]); 
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPortList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to get scale model List
        /// </summary>
        private void FillScaleModelList()
        {
            try
            {
                ScaleModelList = new BalanceFactory().GetBalanceModels().ToList();
                SelectedScaleModel = ScaleModelList.FirstOrDefault(x=>x.Equals(WarehouseCommon.Instance.SelectedScaleModel));
                PortList = SerialPort.GetPortNames().ToList();
                SelectedPort = PortList.FirstOrDefault(x=>x.Equals(WarehouseCommon.Instance.SelectedPort));
                ParityList = Enum.GetNames(typeof(Parity)).ToList();
                SelectedParity = ParityList.FirstOrDefault(x => x.Equals(WarehouseCommon.Instance.SelectedParity));
                StopBitsList = Enum.GetNames(typeof(StopBits)).ToList();
                SelectedStopBit = StopBitsList.FirstOrDefault(x=>x.Equals(WarehouseCommon.Instance.SelectedStopBit));
                SupportedBaudRates = new List<string>
                {
                    "300",
                    "600",
                    "1200",
                    "2400",
                    "4800",
                    "9600",
                    "19200",
                    "38400",
                    "57600",
                    "115200",
                    "230400",
                    "460800",
                    "921600"
                };
                SelectedBaudRate = SupportedBaudRates.FirstOrDefault(x=>x.Equals(WarehouseCommon.Instance.SelectedBaudRate));

                SupportedDataBits = new List<string>
                {
                    "4",
                    "5",
                    "6",
                    "7",
                    "8"
                };
                SelectedDataBit = SupportedDataBits.FirstOrDefault(x => x.Equals(WarehouseCommon.Instance.SelectedDataBit));
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPortList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for get fill currency list and get IdCurrency By current System Region Culture.
        /// </summary>
        public void FillCurrencyDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails ...", category: Category.Info, priority: Priority.Low);

                Currencies = GeosApplication.Instance.Currencies.ToList();
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SelectedCurrency_Warehouse"))
                {
                    SelectedCurrency = Currencies.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency_Warehouse"]);
                }

                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

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

        #endregion // Events
    }
}
