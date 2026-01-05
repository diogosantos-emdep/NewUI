using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SAM;
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
    class WorkOrderScanValidationViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion //End Of Services

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
       
        //private List<ValidateItem> otValidateItemList;


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


        //public List<ValidateItem> OtValidateItemList
        //{
        //    get
        //    {
        //        return otValidateItemList;
        //    }
        //    set
        //    {
        //        otValidateItemList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("OtValidateItemList"));
        //    }
        //}
        //end
        #endregion

        #region Command       
        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandScanBarcode { get; set; }

       


        #endregion

        #region Constructor 

        public WorkOrderScanValidationViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor WorkOrderScanValidationViewModel....", category: Category.Info, priority: Priority.Low);

            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            CommandScanBarcode = new RelayCommand(new Action<object>(ScanBarcodeAction));
            GeosApplication.Instance.Logger.Log("Constructor WorkOrderScanValidationViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Init WorkOrderScanValidationViewModel....", category: Category.Info, priority: Priority.Low);
                WorkOrderBarcodeErrorVisibility = Visibility.Hidden;
                this.OtsList = OtsList;
                GeosApplication.Instance.Logger.Log("Init WorkOrderScanValidationViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrderScanValidationViewModel Init() " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("ScanBarcodeAction WorkOrderScanValidationViewModel....", category: Category.Info, priority: Priority.Low);

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
                                ValidationView validationView = new ValidationView();
                                ValidationViewModel validationViewModel = new ValidationViewModel();
                                validationViewModel.Init(ot);

                                EventHandler handler = delegate { validationView.Close(); };
                                validationViewModel.RequestClose += handler;
                                validationView.DataContext = validationViewModel;
                                validationView.ShowDialogWindow();

                                WorkOrderBarcode = string.Empty;
                                if (validationViewModel.IsCanceled)
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

                GeosApplication.Instance.Logger.Log("ScanBarcodeAction WorkOrderScanValidationViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrderScanValidationViewModel ScanBarcodeAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }


        }
       
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

            GeosApplication.Instance.Logger.Log("CommandCancelAction CommandCancelAction....", category: Category.Info, priority: Priority.Low);
            RequestClose(null, null);
            GeosApplication.Instance.Logger.Log("CommandCancelAction CommandCancelAction() executed successfully", category: Category.Info, priority: Priority.Low);
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
