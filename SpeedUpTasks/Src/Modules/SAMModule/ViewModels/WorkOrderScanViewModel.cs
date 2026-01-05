using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.SAM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    class WorkOrderScanViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {

        #region Services
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
         
        #endregion //End Of Services

        #region TaskLog
        //[000][skale][11-12-2019][GEOS2-1881] Add new option to Log the worked time in an OT
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

        #endregion // End Of Events

        #region Declaration
        //[000] added
        private string workOrderBarcode;
        private string workOrderBarcodeError;
        private Visibility workOrderBarcodeErrorVisibility;
        private string workOrderErrorColour;
        private string windowHeader;
        private List<OTWorkingTime> otWorkingTimeList;
        //end
        #endregion

        #region Properties
        //[000] added
        public string WorkOrderBarcode
        {
            get { return workOrderBarcode; }
            set
            {
                workOrderBarcode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOrderBarcode"));
            }
        }
        public string WorkOrderBarcodeError
        {
            get { return workOrderBarcodeError; }
            set
            {
                workOrderBarcodeError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOrderBarcodeError"));
            }
        }
        public Visibility WorkOrderBarcodeErrorVisibility
        {
            get { return workOrderBarcodeErrorVisibility; }
            set
            {
                workOrderBarcodeErrorVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOrderBarcodeErrorVisibility"));
            }
        }
        public string WorkOrderErrorColour
        {
            get { return workOrderErrorColour; }
            set
            {
                workOrderErrorColour = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOrderErrorColour"));
            }
        }
        public List<Ots> OtsList { get; set; }
       
        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public List<OTWorkingTime> OTWorkingTimeList
        {
            get { return otWorkingTimeList; }
            set { otWorkingTimeList = value; OnPropertyChanged(new PropertyChangedEventArgs("OTWorkingTimeList")); }
        }
        //end
        #endregion

        #region Command       
        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandNextButton { get; set; }
        public ICommand CommandScanBarcode { get; set; }
        public ICommand CommandOnLoaded { get; set; }

        #endregion

        #region Constructor 

        public WorkOrderScanViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor WorkOrderScanViewModel....", category: Category.Info, priority: Priority.Low);

            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            CommandScanBarcode = new RelayCommand(new Action<object>(ScanBarcodeAction));
            GeosApplication.Instance.Logger.Log("Constructor WorkOrderScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

        #region Methods

        /// <summary>
        /// method to initialize
        /// </summary>
        /// <param name="OtsList">List of ots to compare</param>
        /// 
        public void Init(List<Ots> OtsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Init WorkOrderScanViewModel....", category: Category.Info, priority: Priority.Low);
                WorkOrderBarcodeErrorVisibility = Visibility.Hidden;
                this.OtsList = OtsList;
                GeosApplication.Instance.Logger.Log("Init WorkOrderScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrderScanViewModel Init() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Scaning barcode Method
        /// [000][skale][13-12-2019][GEOS2-1953] At the time to scan OT & operator code by normal way that time beep sound not get
        /// </summary>
        /// <param name="obj"></param>
        private void ScanBarcodeAction(object obj)
        {

           try
            {
                GeosApplication.Instance.Logger.Log("ScanBarcodeAction WorkOrderScanViewModel....", category: Category.Info, priority: Priority.Low);

                if (((KeyEventArgs)obj).Key == Key.Enter)
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
                            return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }

                    string strNumOt = string.Empty;
                        string workOrderCode = string.Empty;
                        byte numOt;

                    #region Barcode Validation 

                    if (WorkOrderBarcode.Contains('.'))
                    {
                        strNumOt = WorkOrderBarcode.Substring(WorkOrderBarcode.IndexOf('.') + 1, WorkOrderBarcode.Length - ((WorkOrderBarcode.IndexOf('.') + 1)));
                        workOrderCode = WorkOrderBarcode.Substring(0, WorkOrderBarcode.IndexOf('.'));

                        if (!byte.TryParse(strNumOt, out numOt))
                        {
                            SetViewInErrorState();
                            return;
                        }

                        if (OtsList.Any(Ot => ((Ot.NumOT.ToString().Length == 1 ? "0" + Ot.NumOT : Ot.NumOT.ToString()) == strNumOt) && (Ot.Code == workOrderCode)))
                        {
                            Ots ot = OtsList.FirstOrDefault(x => x.Code == workOrderCode && x.NumOT == numOt);
                            try
                            {
                                OTWorkingTimeList = new List<OTWorkingTime>(SAMService.GetOTWorkingTimeByIdOT_V2040(ot.Site, ot.IdOT));
                                WorkLogView workLogView = new WorkLogView();
                                WorkLogViewModel workLogViewModel = new WorkLogViewModel();
                                workLogViewModel.Init(OTWorkingTimeList, ot);
                                EventHandler handler = delegate { workLogView.Close(); };
                                workLogViewModel.RequestClose += handler;
                                workLogView.DataContext = workLogViewModel;
                                //[000]added
                                GeosApplication.Instance.PlaySound(SAMCommon.Instance.BeepOkFilePath);
                                workLogView.ShowDialogWindow();
                                WorkOrderBarcode = string.Empty;
                                if (workLogViewModel.IsCanceled)
                                    RequestClose(null, null);

                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                                GeosApplication.Instance.Logger.Log("Get an error in ScanBarcodeAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                GeosApplication.Instance.Logger.Log("Get an error in ScanBarcodeAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                            }
                            catch (Exception ex)
                            {
                                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                                GeosApplication.Instance.Logger.Log("Get an error in ScanBarcodeAction  " + ex.Message, category: Category.Exception, priority: Priority.Low);
                            }
                        }
                        else
                        {
                            SetViewInErrorState();
                        }
                    }
                    else
                    {
                        SetViewInErrorState();
                        return;
                    }
                    #endregion
                }
                
                GeosApplication.Instance.Logger.Log("ScanBarcodeAction WorkOrderScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrderScanViewModel ScanBarcodeAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

          
        }
        /// <summary>
        /// [000][skale][13-12-2019][GEOS2-1953] At the time to scan OT & operator code by normal way that time beep sound not get
        /// </summary>
        private void SetViewInErrorState()
        {
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            WorkOrderBarcodeError = " Wrong Work Order " + WorkOrderBarcode;
            WorkOrderBarcode = string.Empty;
            WorkOrderErrorColour = "#FFFF0000";
            WorkOrderBarcodeErrorVisibility = Visibility.Visible;
            //[000] added
            GeosApplication.Instance.PlaySound(SAMCommon.Instance.BeepNotOkFilePath);
        }

        /// <summary>
        /// Cancel Action
        /// </summary>
        /// <param name="obj"></param>
        private void CommandCancelAction(object obj)
        {

           GeosApplication.Instance.Logger.Log("CommandCancelAction WorkOrderScanViewModel....", category: Category.Info, priority: Priority.Low);
           RequestClose(null, null);
          GeosApplication.Instance.Logger.Log("CommandCancelAction WorkOrderScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

    /// <summary>
    /// obj Dispose
    /// </summary>
        public void Dispose()
        {
            GeosApplication.Instance.Logger.Log("Dispose WorkOrderScanViewModel....", category: Category.Info, priority: Priority.Low);
            GC.SuppressFinalize(this);
            GeosApplication.Instance.Logger.Log("Dispose WorkOrderScanViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion
    }
}
