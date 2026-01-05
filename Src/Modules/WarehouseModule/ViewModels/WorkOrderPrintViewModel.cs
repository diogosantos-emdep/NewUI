using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Common_Classes;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class WorkOrderPrintViewModel : INotifyPropertyChanged
    {
        #region TaskLog

        /// <summary>
        /// WMS	M059-06	(#65934) Add option to print wo labels anytime [adaibathina]
        /// [GEOS2-263][Allow to print WO label in Kit articles][adaibathina]
        /// [GEOS2-264][Select DN when printing WO label][adaibathina]
        /// </summary>

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

        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion //End Of Services

        #region Declaration

        private int quantity = 1;
        private int maxQuantity = 1;
        private int copies = 1;
        private OtItem otItem;
        public Ots ots;
        public List<OtItem> pickingMaterialList;
        private List<ArticlesStock> articlesStockMainList;
        private List<ArticlesStock> articlesStockUIList = new List<ArticlesStock>();
        private ArticlesStock selectedArticlesStock;
        private OtItem parentOtItem = null;
        private Visibility serialNoVisibility = Visibility.Collapsed;
        private List<SerialNumber> serialNumberList;
        private SerialNumber selectedSerialNumber;

        #endregion

        #region Properties

        PrintLabel PrintLabel { get; set; }
        PickingMaterials PickingMaterial { get; set; }
        Dictionary<string, string> PrintValues { get; set; }

        public OtItem OtItem
        {
            get { return otItem; }
            set
            {
                otItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtItem"));
            }
        }

        public Ots Ots
        {
            get { return ots; }
            set
            {
                ots = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Ots"));
            }
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Quantity"));
            }
        }

        public int MaxQuantity
        {
            get { return maxQuantity; }
            set
            {
                maxQuantity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaxQuantity"));
            }
        }

        public int Copies
        {
            get { return copies; }
            set
            {
                copies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Copies"));
            }
        }

        public List<OtItem> PickingMaterialList
        {
            get { return pickingMaterialList; }
            set
            {
                pickingMaterialList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PickingMaterialList"));
            }
        }


        public List<ArticlesStock> ArticlesStockMainList
        {
            get { return articlesStockMainList; }
            set
            {
                articlesStockMainList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticlesStockMainList"));
            }
        }
        public List<ArticlesStock> ArticlesStockUIList
        {
            get { return articlesStockUIList; }
            set
            {
                articlesStockUIList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticlesStockMainList"));
            }
        }
        public ArticlesStock SelectedArticlesStock
        {
            get { return selectedArticlesStock; }
            set { selectedArticlesStock = value; OnPropertyChanged(new PropertyChangedEventArgs("ArticlesStock")); }
        }

        public Visibility SerialNoVisibility
        {
            get { return serialNoVisibility; }
            set { serialNoVisibility = value; OnPropertyChanged(new PropertyChangedEventArgs("SerialNoVisibility")); }
        }

        public List<SerialNumber> SerialNumberList
        {
            get { return serialNumberList; }
            set
            {
                serialNumberList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SerialNumberList"));
            }
        }

        public SerialNumber SelectedSerialNumber
        {
            get { return selectedSerialNumber; }
            set { selectedSerialNumber = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedSerialNumber")); }
        }

        #endregion

        #region Command
        public ICommand CommandCancelButton { get; set; }
        public ICommand CommandPrintButton { get; set; }
        public ICommand CommandDNSelectionChanged { get; set; }

        #endregion

        #region Constructor

        public WorkOrderPrintViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor WorkOrderPrintViewModel....", category: Category.Info, priority: Priority.Low);
            CommandCancelButton = new DelegateCommand<object>(CommandCancelAction);
            CommandPrintButton = new DelegateCommand<object>(PrintAction);
            CommandDNSelectionChanged = new DelegateCommand<object>(DNSelectionChanged);
            GeosApplication.Instance.Logger.Log("Constructor WorkOrderPrintViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

        #region Methods
        /// <summary>
        /// [001][GEOS2-263][Allow to print WO label in Kit articles][adaibathina]
        /// [002][GEOS2-264][Select DN when printing WO label][adaibathina]
        /// </summary>
        /// <param name="otItem"></param>
        /// <param name="ots"></param>
        public void Init(OtItem otItem, Ots ots)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Init WorkOrderPrintViewModel....", category: Category.Info, priority: Priority.Low);
                //[002]
                if (otItem.RevisionItem.WarehouseProduct.IdComponent == 0)
                {   //Normal  article
                    // ArticlesStockMainList = WarehouseService.GetWarehouseDeliveryNoteCode_V2032(otItem.IdOTItem, WarehouseCommon.Instance.Selectedwarehouse, null);
                    ArticlesStockMainList = WarehouseService.GetWarehouseDeliveryNoteCode_V2060(otItem.IdOTItem, WarehouseCommon.Instance.Selectedwarehouse, null);
                }
                else
                {   //decomposition  article
                    // ArticlesStockMainList = WarehouseService.GetWarehouseDeliveryNoteCode_V2032(otItem.IdOTItem, WarehouseCommon.Instance.Selectedwarehouse, otItem.RevisionItem.WarehouseProduct.IdComponent);
                    ArticlesStockMainList = WarehouseService.GetWarehouseDeliveryNoteCode_V2060(otItem.IdOTItem, WarehouseCommon.Instance.Selectedwarehouse, otItem.RevisionItem.WarehouseProduct.IdComponent);
                }

                //[001]
                this.ots = ots;
                if (ArticlesStockMainList.Count == 0)
                {
                    Quantity = 0;
                    MaxQuantity = 0;
                    return;
                }
                this.OtItem = otItem;

                FilArticlesStockListForUI();
                SelectedArticlesStock = ArticlesStockUIList[0];
                //   [001] IdArticleType=4 is a kit type
                if (SelectedArticlesStock != null)
                    if (otItem.RevisionItem.WarehouseProduct.Article.IdArticleType == 4)
                    {
                        MaxQuantity = int.Parse(Math.Abs(SelectedArticlesStock.Quantity).ToString());
                    }
                    else
                    {
                        long TotalQuantityOfSameMadeIn = ArticlesStockUIList.Sum(x => x.Quantity);
                        MaxQuantity = int.Parse(Math.Abs(TotalQuantityOfSameMadeIn).ToString());
                    }

                //if (SelectedArticlesStock.WarehouseDeliveryNoteItem.SerialNumbers != null)
                //    SerialNoVisibility = Visibility.Visible;

                if (SelectedArticlesStock.SerialNumbers != null && SelectedArticlesStock.SerialNumbers.Count>0)
                {
                    SerialNumberList = SelectedArticlesStock.SerialNumbers.ToList();
                    SelectedSerialNumber = SerialNumberList.FirstOrDefault();
                    SerialNoVisibility = Visibility.Visible;
                }

                GetPickingMaterial();
                GeosApplication.Instance.Logger.Log("Init WorkOrderPrintViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method Init() WorkOrderPrintViewModel." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FilArticlesStockListForUI()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("WorkOrderPrintViewModel FilArticlesStockListForUI....", category: Category.Info, priority: Priority.Low);
                List<long> allIdCountries = ArticlesStockMainList.Select(x => x.WarehouseDeliveryNoteItem.Producer.IdCountry).Distinct().ToList();

                foreach (var idCountry in allIdCountries)
                {
                    var ArticlesStockWithCountry = ArticlesStockMainList.Where(x => x.WarehouseDeliveryNoteItem.Producer.IdCountry ==
                                                                             idCountry).ToList();
                    for (int i = 1; i < ArticlesStockWithCountry.Count; i++)
                    {
                        ArticlesStockWithCountry[0].WarehouseDeliveryNoteItem.WarehouseDeliveryNote.Code += "\n" +
                                                                                                            ArticlesStockWithCountry[i].WarehouseDeliveryNoteItem.WarehouseDeliveryNote.Code;
                        ArticlesStockWithCountry[0].WarehouseDeliveryNoteItem.WarehouseDeliveryNote.Quantity +=
                         
                                                                                                               ArticlesStockWithCountry[i].WarehouseDeliveryNoteItem.WarehouseDeliveryNote.Quantity;
                        if(ArticlesStockWithCountry[i].SerialNumbers != null && ArticlesStockWithCountry[i].SerialNumbers.Count()>0)
                        {
                            if(ArticlesStockWithCountry[0].SerialNumbers == null)
                            {
                                ArticlesStockWithCountry[0].SerialNumbers = new List<SerialNumber>();
                            }

                            ArticlesStockWithCountry[0].SerialNumbers.AddRange(ArticlesStockWithCountry[i].SerialNumbers);
                        }
                        
                    }
                    ArticlesStockUIList.Add(ArticlesStockWithCountry[0]);
                }
                GeosApplication.Instance.Logger.Log(" WorkOrderPrintViewModel FilArticlesStockListForUI executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FilArticlesStockListForUI()...." + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }




        /// <summary>
        /// [001][GEOS2-264][Select DN when printing WO label][adaibathina]//MaxQuantity has been moved from Init to this method
        /// </summary>
        /// <param name="obj"></param>
        private void DNSelectionChanged(object obj)
        {
            try
            {
               
                GeosApplication.Instance.Logger.Log("WorkOrderPrintViewModel DNSelectionChanged....", category: Category.Info, priority: Priority.Low);

                SerialNumberList = new List<SerialNumber>();
                //   [001] IdArticleType=4 is a kit type
                if (OtItem.RevisionItem.WarehouseProduct.Article.IdArticleType == 4)
                {
                    MaxQuantity = int.Parse(Math.Abs(SelectedArticlesStock.Quantity).ToString());
                    if (SelectedArticlesStock.SerialNumbers != null && SelectedArticlesStock.SerialNumbers.Count > 0)
                    {
                        SerialNumberList.AddRange(SelectedArticlesStock.SerialNumbers);
                        SerialNumberList = new List<SerialNumber>(SerialNumberList);
                        SelectedSerialNumber = SerialNumberList.FirstOrDefault();
                    }
                }
                else
                {
                    long TotalQuantityOfSameMadeIn = ArticlesStockMainList.Where(x => x.WarehouseDeliveryNoteItem.Producer.IdCountry ==
                                                                                  SelectedArticlesStock.WarehouseDeliveryNoteItem.Producer.IdCountry)
                                                                                  .Sum(x => x.Quantity);

                    MaxQuantity = int.Parse(Math.Abs(TotalQuantityOfSameMadeIn).ToString());

                   SerialNumberList.AddRange(ArticlesStockMainList.Where(x => x.WarehouseDeliveryNoteItem.Producer.IdCountry ==
                                                                                  SelectedArticlesStock.WarehouseDeliveryNoteItem.Producer.IdCountry).ToList().SelectMany(x => x.SerialNumbers));
                    if (SerialNumberList != null && SerialNumberList.Count > 0)
                    {
                        SerialNumberList = new List<SerialNumber>(SerialNumberList);
                        SelectedSerialNumber = SerialNumberList.FirstOrDefault();
                    }
                    
                }
                Quantity = MaxQuantity < Quantity ? 1 : Quantity;
                //[001]

                GetPickingMaterial();
                GeosApplication.Instance.Logger.Log(" WorkOrderPrintViewModel DNSelectionChanged executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DNSelectionChanged()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetPickingMaterial()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("WorkOrderPrintViewModel GetPickingMaterial....", category: Category.Info, priority: Priority.Low);

                PickingMaterial = WarehouseService.GetMadeInPartNumberDetail(otItem.IdOTItem, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                PickingMaterial.MadeIn = WarehouseService.GetMadeInByIdWareHouseDeliveryNoteItem((long)SelectedArticlesStock.IdWarehouseDeliveryNoteItem, WarehouseCommon.Instance.Selectedwarehouse);
                PickingMaterial.Reference = otItem.RevisionItem.WarehouseProduct.Article.Reference;
                if (ots.OtItems != null)
                {
                    PickingMaterial.RevisionItem = ots.OtItems.FirstOrDefault(x => x.IdOTItem == otItem.IdOTItem && x.ParentId == -1).RevisionItem;

                    if (otItem.ParentId >= 0)
                        parentOtItem = Ots.OtItems.FirstOrDefault(x => x.IdOTItem == otItem.IdOTItem && x.ParentId == -1);
                }

                GeosApplication.Instance.Logger.Log(" WorkOrderPrintViewModel GetPickingMaterial executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetPickingMaterial() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetPickingMaterial() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetPickingMaterial()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void PrintAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("WorkOrderPrintViewModel PrintAction....", category: Category.Info, priority: Priority.Low);

                if (Quantity <= 0)
                {
                    GeosApplication.Instance.Logger.Log(string.Format(Application.Current.Resources["PrintError"].ToString(), Quantity), category: Category.Info, priority: Priority.Low);
                    CustomMessageBox.Show(string.Format(Application.Current.Resources["PrintError"].ToString(), Quantity), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

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
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                PrintValues = new Dictionary<string, string>();
                byte[] printFile = GeosRepositoryService.GetPrintLabelFile(GeosApplication.Instance.UserSettings["LabelPrinterModel"]);
                PrintLabel = new PrintLabel(PrintValues, printFile);

                CreatePrintValues(PickingMaterial, parentOtItem);

                for (int Copy = 1; Copy <= Copies; Copy++)
                {
                    PrintLabel.Print();
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("WorkOrderPrintViewModel() PrintAction executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintAction()....executed successfully" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CreatePrintValues(PickingMaterials pickingMaterial, OtItem ParentOtitem = null)
        {
            GeosApplication.Instance.Logger.Log("WorkOrderPrintViewModel Method CreatePrintValues....", category: Category.Info, priority: Priority.Low);

            try
            {
                string UserCode = WarehouseService.GetEmployeeCodeByIdUser(GeosApplication.Instance.ActiveUser.IdUser); //[GEOS2-4014][rdixit][15.12.2022]
                string barcode = pickingMaterial.PartNumberCode;
                var revisionNumber = pickingMaterial.RevisionItem.NumItem;
                string otForPrinting = string.Empty;

                if (string.IsNullOrEmpty(OtItem.RevisionItem.NumItem))
                {
                    otForPrinting = Ots.Code;
                }
                else
                {
                    otForPrinting = Ots.Code + " (Item " + OtItem.RevisionItem.NumItem + ")";
                }

                string splBarCode = string.Empty;
                PrintValues.Add("@CUSTOMER00", String.Format("{0} - {1}", Ots.Quotation.Site.Customer.CustomerName, Ots.Quotation.Site.SiteNameWithoutCountry));
                PrintValues.Add("@CUSTOMER01", "");
                PrintValues.Add("@QUANTITY", Quantity.ToString());
                PrintValues.Add("@USER", UserCode);
                PrintValues.Add("@OT", otForPrinting);

                //PrintValues.Add("@REFERENCE", pickingMaterial.Reference);

                if (ParentOtitem != null &&
                    ParentOtitem.RevisionItem != null &&
                    ParentOtitem.RevisionItem.WarehouseProduct != null &&
                    ParentOtitem.RevisionItem.WarehouseProduct.Article != null)
                    PrintValues.Add("@REFERENCE", string.Format("{0} / {1}", ParentOtitem.RevisionItem.WarehouseProduct.Article.Reference, pickingMaterial.Reference));
                else
                    PrintValues.Add("@REFERENCE", pickingMaterial.Reference);

                if (SelectedSerialNumber != null)
                {
                    if (!String.IsNullOrEmpty(SelectedSerialNumber.Code))
                        PrintValues.Add("@SN_VALUE", SelectedSerialNumber.Code);
                    else
                        PrintValues.Add("@SN_VALUE", "");
                }
                else
                {
                    PrintValues.Add("@SN_VALUE", "");
                }

                PrintValues.Add("@REFBARCODE", "");

                if (OtItem.RevisionItem.WarehouseProduct.Article.IdArticleType != 4)
                {
                    PrintValues.Add("@MADEIN", pickingMaterial.MadeIn);
                }
                else
                {
                    PrintValues.Add("@MADEIN", string.Empty);
                }

                PrintValues.Add("@PN", "");


                //[adhatkar][GEOS2-3249][qty with 6 digit]. 
                barcode = barcode.Remove(barcode.Length - 3);
                string QuanityWith6Digit = Convert.ToString(Quantity).PadLeft(6, '0');
                barcode = barcode + QuanityWith6Digit;

                if (!string.IsNullOrEmpty(barcode))
                    splBarCode = PrintLabel.SplitStringForBarcode(barcode);

                PrintValues.Add("@PBARCODE", splBarCode);
                PrintValues.Add("@WAREHOUSE", WarehouseCommon.Instance.Selectedwarehouse.Name);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in WorkOrderPrintViewModel() - print label {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("WorkOrderPrintViewModel Method WorkOrderPrintViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// Cancel Action
        /// </summary>
        /// <param name="obj"></param>
        private void CommandCancelAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("CommandCancelAction WorkOrderPrintViewModel....", category: Category.Info, priority: Priority.Low);
            RequestClose(null, null);
            GeosApplication.Instance.Logger.Log("CommandCancelAction WorkOrderPrintViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// obj Dispose
        /// </summary>
        private void Dispose()
        {
            GeosApplication.Instance.Logger.Log("Dispose WorkOrderPrintViewModel....", category: Category.Info, priority: Priority.Low);
            GC.SuppressFinalize(this);
            GeosApplication.Instance.Logger.Log("Dispose WorkOrderPrintViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

    }
}
