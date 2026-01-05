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
using Emdep.Geos.Data.Common.SAM;
using System.Windows.Media.Imaging;
using Emdep.Geos.Modules.Warehouse.ViewModels;
using Emdep.Geos.Modules.Warehouse.Views;
using DevExpress.Mvvm.DataAnnotations;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Xpf.Grid;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    class ValidationViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Services
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion //End Of Services

        #region Task Log
        //[000][psutar][04-05-2020][GEOS2-2045] Work order validation
        #endregion

        #region Declaration

        // [000] Added
        private double dialogHeight;
        private double dialogWidth;
        private string windowHeader;
        private string workOrder;
        private Ots otObj;
        private ObservableCollection<ValidateItem> otValidateItemList;
        private ValidateItem selectedOtValidateItem;
        private string customer;
        private ImageSource articleImage;
        private string wrongItem;
        private string barcodeStr;
        private Visibility isWrongItemErrorVisible;
        private bool isStatusColumnVisibility;
        private List<LookupValue> validationStatusList;
        private byte[] imageByte;
        //private bool isStatusItemEnabled;
        //private LookupValue selectedStatus;
        private bool focusUserControl;
        //end

        #endregion

        #region Properties

        public string tempSite { get; set; }
        public string tempCustomer { get; set; }
        public string tempPartNumber { get; set; }
        public string tempItem { get; set; }
        public string tempReference { get; set; }
        public string tempQuantity { get; set; }
        public string tempStatus { get; set; }
        public string BarcodeEntered { get; set; }
        public long TotalQuantity { get; set; }


        //[000]added
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
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

        public string BarcodeStr
        {
            get { return barcodeStr; }
            set
            {
                barcodeStr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BarcodeStr"));
            }
        }

        public string WorkOrder
        {
            get { return workOrder; }
            set
            {
                workOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOrder"));
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

        public bool IsCanceled { get; set; }

        public ObservableCollection<ValidateItem> OtValidateItemList
        {
            get { return otValidateItemList; }
            set
            {
                otValidateItemList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtValidateItemList"));
            }
        }

        public ValidateItem SelectedOtValidateItem
        {
            get { return selectedOtValidateItem; }
            set
            {
                selectedOtValidateItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOtValidateItem"));
            }
        }

        public string Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Customer"));
            }
        }

        public ImageSource ArticleImage
        {
            get { return articleImage; }
            set
            {
                articleImage = value;
                if (articleImage == null)
                {
                    ArticleImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/EmptyImage.png"));
                }
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleImage"));
            }
        }

        public string WrongItem
        {
            get { return wrongItem; }
            set
            {
                wrongItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WrongItem"));
            }
        }

        public Visibility IsWrongItemErrorVisible
        {
            get { return isWrongItemErrorVisible; }
            set
            {
                isWrongItemErrorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWrongItemErrorVisible"));
            }
        }

        public bool IsStatusColumnVisibility
        {
            get { return isStatusColumnVisibility; }
            set
            {
                isStatusColumnVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsStatusColumnVisibility"));
            }
        }

        public List<LookupValue> ValidationStatusList
        {
            get { return validationStatusList; }
            set
            {
                validationStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ValidationStatusList"));
            }
        }

        public byte[] ImageByte
        {
            get { return imageByte; }
            set
            {
                imageByte = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageByte"));
            }
        }

        public bool FocusUserControl
        {
            get { return focusUserControl; }
            set
            {
                focusUserControl = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FocusUserControl"));
            }
        }


        #endregion

        #region Command

        //[000]added
        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandBackButton { get; set; }
        public ICommand CommandScanBarcode { get; set; }
        public ICommand StatusCellValueChanged { get; set; }

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

        public ValidationViewModel()
        {
            FocusUserControl = true;
            IsWrongItemErrorVisible = Visibility.Hidden;
            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            CommandScanBarcode = new DelegateCommand<TextCompositionEventArgs>(ScanBarcodeAction);
            StatusCellValueChanged = new DelegateCommand<object>(StatusCellValueChangedCommandAction);
        }
        #endregion

        #region Command Action

        private void CommandCancelAction(object obj)
        {
            IsCanceled = true;
            RequestClose(null, null);
        }

        private void ScanBarcodeAction(TextCompositionEventArgs obj)
            {
            GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....", category: Category.Info, priority: Priority.Low);
            try
            {
                if (obj.Text == "\r")
                {
                    if (OtValidateItemList != null && OtValidateItemList.Count > 0)
                    {
                        ValidateItem temp = new ValidateItem();
                        temp = OtValidateItemList.FirstOrDefault(x => x.Barcodestring.ToUpper() == BarcodeStr.ToUpper());

                        if (temp != null)
                        {
                            SelectedOtValidateItem = temp;
                            GeosApplication.Instance.PlaySound(SAMCommon.Instance.BeepOkFilePath);
                            {
                                if (SelectedOtValidateItem.ImageInBytes == null)
                                {
                                    ImageByte = WarehouseService.GetArticleImageInBytes(SelectedOtValidateItem.ImagePath);
                                    SelectedOtValidateItem.ImageInBytes = ImageByte;
                                }

                                ArticleImage = SAMCommon.Instance.ByteArrayToBitmapImage(selectedOtValidateItem.ImageInBytes);
                                TotalQuantity = SelectedOtValidateItem.Quantity - 1;
                                SelectedOtValidateItem.IsEnabled = true;

                                BarcodeStr = string.Empty;
                                IsWrongItemErrorVisible= Visibility.Hidden;
                                WrongItem = string.Empty;
                            }
                        }
                        else
                        {
                            IsWrongItemErrorVisible = Visibility.Visible;
                            WrongItem = "Wrong Item " + BarcodeStr;
                            GeosApplication.Instance.PlaySound(SAMCommon.Instance.BeepNotOkFilePath);
                            BarcodeStr = string.Empty;
                        }
                    }
                }
                else
                {
                    BarcodeStr += obj.Text;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error  ScanBarcodeAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction....executed ScanBarcodeAction", category: Category.Info, priority: Priority.Low);
        }

        private void StatusCellValueChangedCommandAction(object obj)
        {
            CellValueEventArgs e = obj as CellValueEventArgs;

            if ((((CellValueEventArgs)e).Column).FieldName == "IdItemStatus")
            {
                SelectedOtValidateItem.IdItemStatus = (int)e.Value;
                bool isSave = SAMService.UpdateTestBoardPartNumberTracking(OtObj.Site, SelectedOtValidateItem.IdPartNumberTracking, SelectedOtValidateItem.IdItemStatus, GeosApplication.Instance.ActiveUser.IdUser);
                SelectedOtValidateItem.IsEnabled = false;
                e.Source.PostEditor();
            }
            FocusUserControl = true;
        }
        
        #endregion

        #region Method


        public void Dispose()
        {

        }

        public void Init(Ots ot)
        {
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            WindowHeader = Application.Current.FindResource("Validation").ToString();

            ValidationStatusList = CRMService.GetLookupValues(57).ToList();
            OtValidateItemList = new ObservableCollection<ValidateItem>(SAMService.GetWorkOrderItemsToValidate(ot.Site, ot.IdOT).ToList());

            ArticleImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SAM;component/Assets/Images/EmptyImage.png"));

            DialogHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 140;
            DialogWidth = System.Windows.SystemParameters.PrimaryScreenWidth - 30;

            if (OtValidateItemList != null)
            {
                tempCustomer = OtValidateItemList.FirstOrDefault().Customer;
                tempSite = OtValidateItemList.FirstOrDefault().SiteName;
            }

            OtObj = (Ots)ot.Clone();
            WorkOrder = OtObj.MergeCode;
            string dash = " - ";
            Customer = string.Concat(tempCustomer, dash, tempSite);
        }

        #endregion
    }
}
