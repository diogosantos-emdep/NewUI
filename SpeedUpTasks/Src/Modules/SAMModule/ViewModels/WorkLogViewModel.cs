using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.SAM.Views;
using Emdep.Geos.Services.Contracts;
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
using System.Windows.Media;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class WorkLogViewModel : INotifyPropertyChanged, IDisposable
    {

        #region Services
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion //End Of Services

        #region Task Log
        //[000][skale][11-12-2019][GEOS2-1881] Add new option to Log the worked time in an OT
        #endregion

        #region Declaration


        // [000] Added
        private ObservableCollection<OTWorkingTime> otWorkingTimeList;
        private OTWorkingTime selectedWorkLog;
        private double dialogHeight;
        private double dialogWidth;
        private string windowHeader;
        private string operatorBarcode;
        private string operatorBarcodeError;
        private string operatorBarcodeErrorColour;
        private string workOrder;
        private Visibility operatorBarcodeErrorVisibility;
        private Ots otObj;
        private OTWorkingTime otWorkingTime;
        //end

        #endregion

        #region Properties
        //[000]added
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
        public ObservableCollection<OTWorkingTime> OtWorkingTimeList
        {
            get { return otWorkingTimeList; }
            set { otWorkingTimeList = value; OnPropertyChanged(new PropertyChangedEventArgs("OtWorkingTimeList")); }
        }
        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }
        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }
        public string OperatorBarcode
        {
            get { return operatorBarcode; }
            set
            {
                operatorBarcode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OperatorBarcode"));
            }
        }
        private string BarcodeStr { get; set; }
        public string OperatorBarcodeError
        {
            get { return operatorBarcodeError; }
            set
            {
                operatorBarcodeError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OperatorBarcodeError"));
            }
        }
        public string OperatorBarcodeErrorColour
        {
            get { return operatorBarcodeErrorColour; }
            set
            {
                operatorBarcodeErrorColour = value;
                OnPropertyChanged(new PropertyChangedEventArgs("operatorBarcodeErrorColour"));
            }
        }
        public string WorkOrder
        {
            get { return workOrder; }
            set { workOrder = value; OnPropertyChanged(new PropertyChangedEventArgs("OperatorBarcodeErrorVisibility")); }
        }
        public Visibility OperatorBarcodeErrorVisibility
        {
            get { return operatorBarcodeErrorVisibility; }
            set
            {
                operatorBarcodeErrorVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OperatorBarcodeErrorVisibility"));
            }
        }
        public Ots OtObj
        {
            get { return otObj; }
            set
            {
                otObj = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtObj"));
            }
        }
        public OTWorkingTime SelectedWorkLog
        {
            get { return selectedWorkLog; }
            set { selectedWorkLog = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkLog")); }
        }

        public OTWorkingTime OtWorkingTime
        {
            get { return otWorkingTime; }
            set { otWorkingTime = value; OnPropertyChanged(new PropertyChangedEventArgs("OtWorkingTime")); }
        }
        public bool IsCanceled { get; set; }

        #endregion

        #region Command
        //[000]added
        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandBackButton { get; set; }
        public ICommand CommandScanBarcode { get; set; }

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


        #region Constructor 

        public WorkLogViewModel()
        {
            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            CommandBackButton = new DelegateCommand<object>(CommandBackAction);
            CommandScanBarcode = new DelegateCommand<TextCompositionEventArgs>(ScanBarcodeAction);
            OtWorkingTime = new OTWorkingTime();
        }
        #endregion

        #region Command Action
        private void CommandCancelAction(object obj)
        {
            IsCanceled = true;
            RequestClose(null, null);
        }
        private void CommandBackAction(object obj)
        {
            RequestClose(null, null);
        }
        /// <summary>
        /// [000][skale][13-12-2019][GEOS2-1953] At the time to scan OT & operator code by normal way that time beep sound not get
        /// </summary>
        /// <param name="obj"></param>
        private void ScanBarcodeAction(TextCompositionEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);
            try
            {
                if (obj.Text == "\r")
                {
                    OperatorBarcode = string.Copy(BarcodeStr);

                    if (OtWorkingTimeList.Any(x => x.UserShortDetail.CompanyCode == OperatorBarcode))
                    {

                        OTWorkingTime otWorkingTimeobj = OtWorkingTimeList.Where(x => x.UserShortDetail.CompanyCode == OperatorBarcode).FirstOrDefault();

                        if (!otWorkingTimeobj.IsTimerStarted)
                        {
                            WorkLogViewModel workLogViewModel = new WorkLogViewModel();
                            //OtWorkingTime.IdStage = otWorkingTimeobj.IdStage;
                            OtWorkingTime.IdStage = 4;
                            OtWorkingTime.IdOT = otWorkingTimeobj.IdOT;
                            OtWorkingTime.IdOperator = otWorkingTimeobj.IdOperator;
                            OtWorkingTime.StartTime = GeosApplication.Instance.ServerDateTime;
                            otWorkingTimeobj.StartTime = GeosApplication.Instance.ServerDateTime;
                            otWorkingTimeobj.EndTime = null;
                            OtWorkingTime = SAMService.AddOTWorkingTime(otWorkingTimeobj.Company, OtWorkingTime);
                            otWorkingTimeobj.IdOTWorkingTime = OtWorkingTime.IdOTWorkingTime;
                            otWorkingTimeobj.IsTimerStarted = true;
                            workLogViewModel.IsCanceled = true;
                            if (workLogViewModel.IsCanceled)
                                RequestClose(null, null);

                        }
                        else
                        {
                            WorkLogViewModel workLogViewModel = new WorkLogViewModel();
                            // OtWorkingTime.IdStage = otWorkingTimeobj.IdStage;
                            OtWorkingTime.IdStage = 4;
                            OtWorkingTime.IdOT = otWorkingTimeobj.IdOT;
                            OtWorkingTime.IdOperator = otWorkingTimeobj.IdOperator;
                            OtWorkingTime.EndTime = GeosApplication.Instance.ServerDateTime;
                            OtWorkingTime.IdOTWorkingTime = otWorkingTimeobj.IdOTWorkingTime;
                            SAMService.UpdateOTWorkingTime(otWorkingTimeobj.Company, OtWorkingTime);
                            otWorkingTimeobj.IsTimerStarted = false;
                            workLogViewModel.IsCanceled = true;
                            if (workLogViewModel.IsCanceled)
                                RequestClose(null, null);

                        }

                        OperatorBarcodeError = string.Empty;
                        OperatorBarcodeErrorVisibility = Visibility.Hidden;
                        BarcodeStr = string.Empty;
                        SelectedWorkLog = OtWorkingTime;
                        //[000]added
                        GeosApplication.Instance.PlaySound(SAMCommon.Instance.BeepOkFilePath);

                    }
                    else
                    {

                        SetViewInErrorState();
                        OperatorBarcode = string.Empty;
                        BarcodeStr = string.Empty;
                    }

                }
                else
                {
                    BarcodeStr += obj.Text;
                }

                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ScanBarcodeAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ScanBarcodeAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error  ScanBarcodeAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }


        }
        #endregion

        #region Method
        public void Init(List<OTWorkingTime> tempOtWorkingTimeList, Ots ot)
        {
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            WindowHeader = Application.Current.FindResource("LogWork").ToString();
            OtWorkingTimeList = new ObservableCollection<OTWorkingTime>(tempOtWorkingTimeList.ToList());
            //DialogHeight = 455;
            //DialogWidth = System.Windows.SystemParameters.PrimaryScreenWidth - 140;
            OtObj = (Ots)ot.Clone();
            WorkOrder = OtObj.MergeCode;
            OperatorBarcodeErrorVisibility = Visibility.Hidden;

        }

        public void Dispose()
        {

        }
        /// <summary>
        /// [000][skale][13-12-2019][GEOS2-1953] At the time to scan OT & operator code by normal way that time beep sound not get
        /// </summary>
        private void SetViewInErrorState()
        {
            try
            {
                string operatorName = WorkbenchService.GetUserNameByCompanyCode(BarcodeStr);
                if (string.IsNullOrEmpty(WorkbenchService.GetUserNameByCompanyCode(BarcodeStr)))
                {
                    OperatorBarcodeError = string.Format(" Wrong Operator {0}", OperatorBarcode);
                }
                else
                    OperatorBarcodeError = string.Format(" Operator {0} is not assigned to work order {1}", operatorName, OtObj.MergeCode);

                OperatorBarcode = string.Empty;
                OperatorBarcodeErrorColour = "#FFFF0000";
                OperatorBarcodeErrorVisibility = Visibility.Visible;

                //[000]added
                GeosApplication.Instance.PlaySound(SAMCommon.Instance.BeepNotOkFilePath);
                //end

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetViewInErrorState() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetViewInErrorState() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetViewInErrorState  " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion

    }
}
