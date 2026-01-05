using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class NewLocationViewModel
    {

        #region Services
        IWarehouseService WarehouseService = new UI.ServiceProcess.WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Declaration
        private PickingMaterials selectedPickingMaterial;
        private List<ArticleWarehouseLocations> allPossibleLocationOfArticleList;
        private ArticleWarehouseLocations selectedAllPossibleLocationOfArticle;
        private bool isSave;
        private bool isScanSerialNumber;
        private bool isBackButtonPressed;
        private long scannedQty;
        private bool isDeductFromRefund;
        private TransferMaterials selectedRefundMaterial;
        private string oldLocationName;
        private long totalCurrentItemStock;
        private string barcodeStr;
        #endregion // Declaration


        #region Properties
        public string BarcodeStr
        {
            get { return barcodeStr; }
            set
            {
                barcodeStr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BarcodeStr"));
            }
        }

        public PickingMaterials SelectedPickingMaterial
        {
            get { return selectedPickingMaterial; }
            set
            {
                selectedPickingMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPickingMaterial"));
            }
        }

        public TransferMaterials SelectedRefundMaterial
        {
            get { return selectedRefundMaterial; }
            set
            {
                selectedRefundMaterial = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRefundMaterial"));
            }
        }

        public List<ArticleWarehouseLocations> AllPossibleLocationOfArticleList
        {
            get { return allPossibleLocationOfArticleList; }
            set
            {
                allPossibleLocationOfArticleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllPossibleLocationOfArticleList"));
            }
        }

        public ArticleWarehouseLocations SelectedAllPossibleLocationOfArticle
        {
            get { return selectedAllPossibleLocationOfArticle; }
            set
            {
                selectedAllPossibleLocationOfArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("selectedAllPossibleLocationOfArticle"));
            }
        }

        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        public bool IsScanSerialNumber
        {
            get { return isScanSerialNumber; }
            set
            {
                isScanSerialNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("isScanSerialNumber"));
            }
        }

        public bool IsBackButtonPressed
        {
            get { return isBackButtonPressed; }
            set
            {
                isBackButtonPressed = value;
                OnPropertyChanged(new PropertyChangedEventArgs("isBackButtonPressed"));
            }
        }

        public long ScannedQty
        {
            get { return scannedQty; }
            set
            {
                scannedQty = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ScannedQty"));
            }
        }

        public bool IsDeductFromRefund
        {
            get { return isDeductFromRefund; }
            set
            {
                isDeductFromRefund = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeductFromRefund"));
            }
        }

        public string OldLocationName
        {
            get { return oldLocationName; }
            set
            {
                oldLocationName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldLocationName"));
            }
        }

        public Int64 TotalCurrentItemStock
        {
            get { return totalCurrentItemStock; }
            set
            {
                totalCurrentItemStock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalCurrentItemStock"));
            }
        }
        #endregion

        #region Commands
        public ICommand CloseWindowCommand { get; set; }
        public ICommand ConfirmButtonCommand { get; set; }
        #endregion // Commands

        #region public Events

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

        #region Constructor

        public NewLocationViewModel()
        {
            CloseWindowCommand = new DelegateCommand<object>(CloseWindowAction);
            ConfirmButtonCommand = new DelegateCommand<object>(ConfirmButtonCommandAction);
        }

        #endregion


        #region method

        public void Init(PickingMaterials selectedPickingMaterial, List<ArticleWarehouseLocations> allPossibleLocationOfArticleList, bool isScanSerialNumber, long scannedQty, string locationName, Int64 totalCurrentItemStock,string barcodeString)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                SelectedPickingMaterial = selectedPickingMaterial;
                //SelectedRefundMaterial = selectedPickingMaterial;
                AllPossibleLocationOfArticleList = new List<ArticleWarehouseLocations>();
                AllPossibleLocationOfArticleList = allPossibleLocationOfArticleList;
                SelectedAllPossibleLocationOfArticle = AllPossibleLocationOfArticleList.FirstOrDefault();
                IsScanSerialNumber = isScanSerialNumber;
                ScannedQty = scannedQty;
                OldLocationName = locationName;
                TotalCurrentItemStock = totalCurrentItemStock;
                BarcodeStr = barcodeString;
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindowAction(object obj)
        {
            try
            {
                IsBackButtonPressed = true;
                RequestClose(null, null);  
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseWindowAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void ConfirmButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ConfirmButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                
                //refund entry
                bool IsRefundSaved = WarehouseService.InsertIntoArticleStockForRefund_V2170(SelectedPickingMaterial);

                TransferMaterials SelectedRefundMaterial = new TransferMaterials();
                SelectedRefundMaterial.IdArticle = SelectedPickingMaterial.IdArticle;
                SelectedRefundMaterial.CostPrice = SelectedPickingMaterial.CostPrice;
                SelectedRefundMaterial.UnitPrice = SelectedPickingMaterial.UnitPrice;
                SelectedRefundMaterial.IdWarehouseLocation = SelectedPickingMaterial.IdWarehouseLocation;
                SelectedRefundMaterial.IdWarehouse = SelectedPickingMaterial.IdWarehouse;
                SelectedRefundMaterial.IdWareHouseDeliveryNoteItem = SelectedPickingMaterial.IdWareHouseDeliveryNoteItem;
                SelectedRefundMaterial.ScannedQty = (-1) * SelectedPickingMaterial.ScannedQty;
                SelectedRefundMaterial.Comments = "Transfer from " + "\"" + OldLocationName + "\"" + " location to " + "\"" + SelectedAllPossibleLocationOfArticle.FullName + "\"" + " [STOCK -> " + Convert.ToString(TotalCurrentItemStock) + "]";
                SelectedRefundMaterial.ModifiedBy = SelectedPickingMaterial.ModifiedBy;
                SelectedRefundMaterial.IdCurrency = SelectedPickingMaterial.IdCurrency;
               
                IsDeductFromRefund = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(SelectedRefundMaterial);

                TransferMaterials SelectedRefundMaterial1 = new TransferMaterials();
                //SelectedRefundMaterial1 = SelectedRefundMaterial;
                SelectedRefundMaterial1.IdArticle = SelectedPickingMaterial.IdArticle;
                SelectedRefundMaterial1.CostPrice = SelectedPickingMaterial.CostPrice;
                SelectedRefundMaterial1.UnitPrice = SelectedPickingMaterial.UnitPrice;
                //SelectedRefundMaterial1.IdWarehouseLocation = SelectedAllPossibleLocationOfArticle.IdWarehouseLocation;
                SelectedRefundMaterial1.IdWarehouse = SelectedPickingMaterial.IdWarehouse;
                SelectedRefundMaterial1.IdWareHouseDeliveryNoteItem = SelectedPickingMaterial.IdWareHouseDeliveryNoteItem;
                //SelectedRefundMaterial1.ScannedQty = (-1) * SelectedPickingMaterial.ScannedQty;
                SelectedRefundMaterial1.Comments = "Transfer from " + "\"" + OldLocationName + "\"" + " location to " + "\"" + SelectedAllPossibleLocationOfArticle.FullName + "\"" + " [STOCK -> " + Convert.ToString(TotalCurrentItemStock) + "]";
                SelectedRefundMaterial1.ModifiedBy = SelectedPickingMaterial.ModifiedBy;
                SelectedRefundMaterial1.IdCurrency = SelectedPickingMaterial.IdCurrency;
                SelectedRefundMaterial1.ScannedQty = (1) * ScannedQty;

                SelectedRefundMaterial1.IdWarehouseLocation = SelectedAllPossibleLocationOfArticle.IdWarehouseLocation;
               // SelectedRefundMaterial.ScannedQty = ScannedQty;
                if (IsScanSerialNumber == true)
                {
                    //foreach (var temp in SelectedPickingMaterial.ScanSerialNumbers)
                    //{
                    if (SelectedPickingMaterial.ScanSerialNumbers.Any(ss => ss.Code == BarcodeStr))
                    {
                        SerialNumber SerialNumber = SelectedPickingMaterial.ScanSerialNumbers.Where(ss => ss.Code == BarcodeStr).FirstOrDefault();
                        SerialNumber.TransactionOperation = ModelBase.TransactionOperations.Add;
                        SerialNumber.IdWarehouseLocation = SelectedAllPossibleLocationOfArticle.IdWarehouseLocation;
                        SerialNumber.IdWarehouse = Convert.ToInt32(WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);

                        if (SelectedRefundMaterial1.ScanSerialNumbers == null)
                            SelectedRefundMaterial1.ScanSerialNumbers = new List<SerialNumber>();

                        SelectedRefundMaterial1.ScanSerialNumbers.Add(SerialNumber);
                        SelectedRefundMaterial1.ScanSerialNumbers.ToList().ForEach(SS => SS.MasterItem = null);
                    }
                       
                    //}
                    //SelectedRefundMaterial.ScanSerialNumbers = new List<SerialNumber>();
                
                }

                
                IsSave = WarehouseService.InsertIntoArticleStockForTransferMaterial_V2150(SelectedRefundMaterial1);

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method ConfirmButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ConfirmButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
