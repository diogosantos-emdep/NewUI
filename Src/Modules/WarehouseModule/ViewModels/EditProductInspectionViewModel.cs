using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Gauges;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Warehouse.Views;
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
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class EditProductInspectionViewModel
    {//[Sudhir.Jangra][GEOS2-3531][17/01/2023]
        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
         IPCMService PCMService = new PCMServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
        #endregion

        #region Declaration
        ObservableCollection<ProductInspectionArticles> productInspectionArticlesList;
        ObservableCollection<EditProductInspectionArticles> editProductInspectionArticlesList;
        ObservableCollection<ProductInspectionImageInfo> productInspectionImageList;
        private bool isSaveChanges;
        private string reference;
        private string deliveryNote;
        private long quantity;
        private string description;
        private string status;
        private long quantityInspected;
        long idDeliveryNote;
        private string supplier;
        private decimal weight;
        private decimal length;
        private decimal width;
        private decimal height;
        private string size;
        private ImageSource informationImage;
        private Visibility readMeEntriesTitleVisibility = Visibility.Visible;
        private string readMeEntriesTitle;
        private int symbolCount;
        #endregion

        #region Public Properties
        public ObservableCollection<ProductInspectionArticles> ProductInspectionArticlesList
        {
            get { return productInspectionArticlesList; }
            set
            {
                productInspectionArticlesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductInspectionArticlesList"));
            }
        }
        public bool IsSaveChanges
        {
            get { return isSaveChanges; }
            set
            {
                isSaveChanges = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaveChanges"));
            }
        }
        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Reference"));
            }
        }
        public string DeliveryNote
        {
            get { return deliveryNote; }
            set
            {
                deliveryNote = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeliveryNote"));
            }
        }
        public long Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Quantity"));
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }
        public long QuantityInspected
        {
            get { return quantityInspected; }
            set
            {
                quantityInspected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("QuantityInspected"));
            }
        }
        public string Supplier
        {
            get { return supplier; }
            set
            {
                supplier = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Supplier"));
            }
        }
        public decimal Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Weight"));
            }
        }
        public decimal Length
        {
            get { return length; }
            set
            {
                length = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Length"));
            }
        }
        public decimal Width
        {
            get { return width; }
            set
            {
                width = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Width"));
            }
        }
        public decimal Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Height"));
            }
        }
        public string Size
        {
            get { return size; }
            set
            {
                size = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Size"));
            }
        }
        public ObservableCollection<EditProductInspectionArticles> EditProductInspectionArticlesList
        {
            get { return editProductInspectionArticlesList; }
            set
            {
                editProductInspectionArticlesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EditProductInspectionArticlesList"));
            }
        }

        public ObservableCollection<ProductInspectionImageInfo> ProductInspectionImageList
        {
            get { return productInspectionImageList; }
            set
            {
                productInspectionImageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductInspectionImageList"));
            }
        }
        public ImageSource InformationImage
        {
            get { return informationImage; }
            set
            {
                informationImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationImage"));
            }
        }
        string comments;
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comments"));
            }
        }
        public IList<LookupValue> tempStatusList { get; set; }
        private List<LookupValue> statusList;
        public List<LookupValue> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }
        string articleDescription;
        public string ArticleDescription
        {
            get { return articleDescription; }
            set
            {
                articleDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleDescription"));
            }
        }
        public long IdArticleWarehouseInspection { get; set; }
        string weightNew;
        public string WeightNew
        {
            get { return weightNew; }
            set
            {
                weightNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WeightNew"));
            }
        }

        
       List<ProductInspectionReworkCauses> reworkCauses;
        public List<ProductInspectionReworkCauses> ReworkCauses
        {
            get
            {
                return reworkCauses;
            }

            set
            {
                reworkCauses = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReworkCauses"));
            }
        }
        ProductInspectionReworkCauses selectedReworkCauses;
        public ProductInspectionReworkCauses SelectedReworkCauses
        {
            get
            {
                return selectedReworkCauses;
            }

            set
            {
                selectedReworkCauses = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedReworkCauses"));
            }
        }

        public Visibility ReadMeEntriesTitleVisibility
        {
            get
            {
                return readMeEntriesTitleVisibility;
            }

            set
            {
                readMeEntriesTitleVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReadMeEntriesTitleVisibility"));
            }
        }
        public string ReadMeEntriesTitle
        {
            get
            {
                return readMeEntriesTitle;
            }

            set
            {
                readMeEntriesTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReadMeEntriesTitle"));
            }
        }

        public SymbolsAnimation MatrixView5x8Animation { get; set; }

        public int SymbolCount
        {
            get
            {
                return symbolCount;
            }

            set
            {
                symbolCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SymbolCount"));
            }
        }
        public bool PreviousTimerValue { get; set; }
        #endregion

        #region ICommands
        public ICommand EditProductInspectionCancelButtonCommand { get; set; }
        public ICommand EditProductInspectionReferenceHyperLinkCommand { get; set; }
        public ICommand EditProductInspectionDeliveryNoteHyperLinkCommand { get; set; }
        public ICommand EditProductInspectionAcceptButtonCommand { get; set; }
        public ICommand ProductInspectationMouseDoubleClickCommand { get; set; }
        public ICommand ProductInspectationMouseClickActionB_RightCommand { get; set; }
        public ICommand ProductInspectationMouseClickActionB_WrongCommand { get; set; }
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
        public EditProductInspectionViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditProductInspectionDetailsViewModel()...", category: Category.Info, priority: Priority.Low);
                EditProductInspectionCancelButtonCommand = new DelegateCommand<object>(EditProductInspectionCancelButtonCommandAction);
                EditProductInspectionReferenceHyperLinkCommand = new DelegateCommand<object>(EditProductInspectionReferenceHyperLinkCommandAction);
                EditProductInspectionDeliveryNoteHyperLinkCommand = new DelegateCommand<object>(EditProductInspectionDeliveryNoteHyperLinkCommandAction);
                EditProductInspectionAcceptButtonCommand = new DelegateCommand<object>(EditProductInspectionAcceptButtonCommandAction);
                //ProductInspectationMouseDoubleClickCommand= new RelayCommand(new Action<object>(ProductInspectationMouseDoubleAction));
                ProductInspectationMouseClickActionB_RightCommand = new DelegateCommand<object>(ProductInspectationMouseClickB_RightAction);
                ProductInspectationMouseClickActionB_WrongCommand = new DelegateCommand<object>(ProductInspectationMouseClickB_WrongAction);

                //EditProductInspectionDeliveryNoteHyperLinkCommand = new DelegateCommand<object>(ArticleDetailsViewDNHyperlinkClickCommandAction); //[002] added for hyperlink for Delivery Note
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EditProductInspectionDetailsViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods

        public void Init(ProductInspectionArticles temp)
        {
            try
            {
                IdArticleWarehouseInspection = temp.IdArticleWarehouseInspection;
                FillStatusList();
                Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                ProductInspectionArticlesList = new ObservableCollection<ProductInspectionArticles>(WarehouseService.GetArticlesProductInspection(warehouse));
                //EditProductInspectionArticlesList = new ObservableCollection<EditProductInspectionArticles>(WarehouseService.GetArticlesProductInspection_V2350(warehouse, temp.IdArticleWarehouseInspection));
                //Shubham[skadam] GEOS2-3531 [QUALITY_INSPECTION] Add Product Inspection Screen 13 02 2023
                EditProductInspectionArticlesList = new ObservableCollection<EditProductInspectionArticles>(WarehouseService.GetArticlesProductInspection_V2360(warehouse, temp.IdArticleWarehouseInspection));
                //ProductInspectionImageList = new ObservableCollection<ProductInspectionImageInfo>(WarehouseService.GetProductInspectionImageInBytes(warehouse, temp.IdArticle));
                Reference = temp.Reference;
                DeliveryNote = temp.Code;
                Quantity = temp.Quantity;
                QuantityInspected = temp.QuantityInspected;
                idDeliveryNote = temp.IdWareHouseDeliveryNote;
                Comments = string.Empty;
                Comments = EditProductInspectionArticlesList.FirstOrDefault().Comments;
                ArticleDescription = EditProductInspectionArticlesList.First().ArticleDescription;
                //foreach (var image in ProductInspectionImageList)
                //{
                //    InformationImage = ByteArrayToBitmapImage(image.ImageInByte);
                //}
                foreach (var image in EditProductInspectionArticlesList)
                {
                    InformationImage = ByteArrayToBitmapImage(image.ArticleImageInBytes);
                }
                ReworkCauses = new List<ProductInspectionReworkCauses>();
                ProductInspectionReworkCauses ProductInspectionReworkCauses = new ProductInspectionReworkCauses();
                ProductInspectionReworkCauses.ReworkCause = "---";
                ProductInspectionReworkCauses.IdArticleCategory = 0;
                ProductInspectionReworkCauses.IdReworkCause = 0;
                ReworkCauses.Add(ProductInspectionReworkCauses);
                var tempReworkCauses = EditProductInspectionArticlesList.FirstOrDefault().ReworkCauses;
                if (tempReworkCauses != null)
                    ReworkCauses.AddRange(tempReworkCauses);
                SelectedReworkCauses = ReworkCauses.FirstOrDefault();
                foreach (var item in EditProductInspectionArticlesList)
                {
                    try
                    {
                        Supplier = item.SupplierName;
                        Description = item.Description;
                        //Status = "OK";
                        //item.Status.Value = Status;
                         Weight = item.Weight; //[rdixit][GEOS2-5851][23.01.2025]
                        /*decimal gram = item.Weight * 1000;
                        if (item.Weight >= 1)
                        {
                            Weight = Math.Round(item.Weight, 2);
                            WeightNew = Weight + " kg";
                        }
                        else
                        {
                            Weight = Math.Round(gram, 2);
                            WeightNew = Weight + " gm";
                        }*/

                        Length = item.Length;
                        Width = item.Width;
                        Height = item.Height;
                        Size = Length + " x " + Width + " x " + height;
                        item.QuantityInspected = QuantityInspected;
                        if (item.Status != null)
                            Status = item.Status.Value;

                    }
                    catch (Exception ex)
                    {
                    }
                }
                ReadMeEntriesTitle = WarehouseService.GetArticle_Comment(warehouse, temp.IdArticle);
                //[rdixit][GEOS2-4209][16.03.2023]
                if (!string.IsNullOrEmpty(ReadMeEntriesTitle))
                {
                    ReadMeEntriesTitle = ReadMeEntriesTitle.Replace(System.Environment.NewLine, " | ").Trim(' ', '|', ' ').ToUpper();
                    if (ReadMeEntriesTitle.Length <= 40)
                    {
                        MatrixView5x8Animation = new BlinkingAnimation() { RefreshTime = new TimeSpan(0, 0, 1) };
                        SymbolCount = ReadMeEntriesTitle.Length;
                    }
                    else
                    {
                        MatrixView5x8Animation = new CreepingLineAnimation() { RefreshTime = new TimeSpan(0, 0, 0, 0, 200), Repeat = true };
                        SymbolCount = 54;
                    }
                }
                else
                    ReadMeEntriesTitleVisibility = Visibility.Collapsed;
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();

                using (var mem = new MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        private void EditProductInspectionCancelButtonCommandAction(object obj)
        {
            RequestClose(null, null);
        }

        public void EditProductInspectionReferenceHyperLinkCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditProductInspectionReferenceHyperLinkCommandAction....", category: Category.Info, priority: Priority.Low);
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

                //  TreeListView detailView = (TreeListView)obj;
                string reference = Reference;
                ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel();
                ArticleDetailsView articleDetailsView = new ArticleDetailsView();
                EventHandler handle = delegate { articleDetailsView.Close(); };
                articleDetailsViewModel.RequestClose += handle;
                Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                long warehouseId = warehouse.IdWarehouse;
                articleDetailsViewModel.Init(reference, warehouseId);
                articleDetailsView.DataContext = articleDetailsViewModel;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                //  var ownerInfo = (detailView as FrameworkElement);
                //  articleDetailsView.Owner = Window.GetWindow(ownerInfo);
                articleDetailsView.ShowDialog();
                //[001] added
                if (articleDetailsViewModel.IsResult)
                {
                    //  if (articleDetailsViewModel.UpdateArticle.MyWarehouse != null)
                    //ArticleMinimumStock = articleDetailsViewModel.UpdateArticle.MyWarehouse.MinimumStock;
                }
                //end
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method EditProductInspectionReferenceHyperLinkCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method EditProductInspectionReferenceHyperLinkCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditProductInspectionDeliveryNoteHyperLinkCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditProductInspectionDeliveryNoteHyperLinkCommandAction....", category: Category.Info, priority: Priority.Low);

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

                // TableView tblView = (TableView)obj;
                //  ArticlesStock ac = (ArticlesStock)tblView.DataControl.CurrentItem;
                long idwarehouseDN = idDeliveryNote;
                EditDeliveryNoteView editDeliveryNoteView = new EditDeliveryNoteView();
                EditDeliveryNoteViewModel editDeliveryNoteViewModel = new EditDeliveryNoteViewModel();
                EventHandler handle = delegate { editDeliveryNoteView.Close(); };
                editDeliveryNoteViewModel.RequestClose += handle;
               // WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById(idwarehouseDN);

                //[pramo.misal][GEOS2-4543][10-10-2023]
                //WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2440(idwarehouseDN);
                //Shubham[skadam] GEOS2-5226 NO SE PUEDE DESBLOQUEAR UN ALBARÁN DE LA REFERENCIA 02MOTTRONIC 12 01 2024
               // WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2480(idwarehouseDN);
                //[Sudhir.jangra][GEOS2-5457]
                WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2510(idwarehouseDN);


                editDeliveryNoteViewModel.Init(wdn);
                editDeliveryNoteView.DataContext = editDeliveryNoteViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                 //var ownerInfo = (tblView as FrameworkElement);
                 //editDeliveryNoteView.Owner = Window.GetWindow(ownerInfo);
                editDeliveryNoteView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method EditProductInspectionDeliveryNoteHyperLinkCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditProductInspectionDeliveryNoteHyperLinkCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [SP66][001][avpawar][03/07/2019][GEOS2-1604][Add new colums "DN" and "Location" in Article Stock History]
        /// Method to Open Delivery Note on hyperlink click
        /// </summary>
        /// <param name="obj"></param>
        public void ArticleDetailsViewDNHyperlinkClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleDetailsViewDNHyperlinkClickCommandAction....", category: Category.Info, priority: Priority.Low);

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

               // DevExpress.Xpf.Grid.TableView tblView = (DevExpress.Xpf.Grid.TableView)obj;
               // ProductInspectionArticles ac = (ProductInspectionArticles)tblView.DataControl.CurrentItem;
                EditDeliveryNoteView editDeliveryNoteView = new EditDeliveryNoteView();
                EditDeliveryNoteViewModel editDeliveryNoteViewModel = new EditDeliveryNoteViewModel();
                EventHandler handle = delegate { editDeliveryNoteView.Close(); };
                editDeliveryNoteViewModel.RequestClose += handle;
               //WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById(idDeliveryNote);

                //[pramo.misal][GEOS2-4543][10-10-2023]
                //WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2440(idDeliveryNote);
                //Shubham[skadam] GEOS2-5226 NO SE PUEDE DESBLOQUEAR UN ALBARÁN DE LA REFERENCIA 02MOTTRONIC 12 01 2024
               // WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2480(idDeliveryNote);
                //[Sudhir.Jangra][GEOS2-5457]
                WarehouseDeliveryNote wdn = WarehouseService.GetWarehouseDeliveryNoteById_V2510(idDeliveryNote);


                editDeliveryNoteViewModel.Init(wdn);
                editDeliveryNoteView.DataContext = editDeliveryNoteViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                //var ownerInfo = (tblView as FrameworkElement);r
                //editDeliveryNoteView.Owner = Window.GetWindow(ownerInfo);
                editDeliveryNoteView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method ArticleDetailsViewDNHyperlinkClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ArticleDetailsViewDNHyperlinkClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //Shubham[skadam] GEOS2-3531 [QUALITY_INSPECTION] Add Product Inspection Screen 15 02 2023
        public void EditProductInspectionAcceptButtonCommandAction(object obj)
        {
            try
            {
                if (QuantityInspected> Quantity)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProductInspectationQuantity").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                //if (EditProductInspectionArticlesList.Any(a=>a.Description.Trim().Equals(SelectedReworkCauses.ReworkCause.Trim())))
                //{
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProductInspectationReworkCauses").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //    return;
                //}
                Warehouses warehouse = WarehouseCommon.Instance.Selectedwarehouse;
                List<EditProductInspectionArticles> ProductInspectionArticles = new List<EditProductInspectionArticles>();
                ProductInspectionArticles.AddRange(EditProductInspectionArticlesList);
                ProductInspectionArticles.ForEach(f => f.QuantityInspected = QuantityInspected);
                //ProductInspectionReworkCauses NewReworkCauses = new ProductInspectionReworkCauses();
                //if (SelectedReworkCauses.IdReworkCause!=0)
                //{
                //    NewReworkCauses = SelectedReworkCauses;
                //}
                bool IsInserted = WarehouseService.IsInsertedProductInspection_V2360(warehouse, Comments, IdArticleWarehouseInspection, ProductInspectionArticles, QuantityInspected);
                IsSaveChanges = true;
                if (IsInserted)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ProductInspectationUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method EditProductInspectionAcceptButtonCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditProductInspectionAcceptButtonCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillStatusList..."), category: Category.Info, priority: Priority.Low);

                tempStatusList = PCMService.GetLookupValues(108);
                StatusList = new List<LookupValue>(tempStatusList);
                //StatusList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                //SelectedStatus = StatusList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log(string.Format("Method FillStatusList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] GEOS2-3531 [QUALITY_INSPECTION] Add Product Inspection Screen 16 02 2023
        public void ProductInspectationMouseDoubleAction(object obj)
        {
            try
            {
                int i = 0;
                LookupValue previous;
                MouseButtonEventArgs newobj = (MouseButtonEventArgs)obj;
                TableView view = (newobj.Source as TableView);
                TableViewHitInfo hitInfo = view.CalcHitInfo(newobj.OriginalSource as DependencyObject);
                if (hitInfo.Column.Header.Equals("Status"))
                {
                    var EditProductInspectionArticles = (EditProductInspectionArticles)((DevExpress.Xpf.Grid.DataViewBase)((System.Windows.RoutedEventArgs)obj).Source).DataControl.CurrentItem;
                    foreach (EditProductInspectionArticles itemEditProductInspection in EditProductInspectionArticlesList)
                    {
                    goUp:
                        i = i + 1;
                        if (itemEditProductInspection.Status != null && itemEditProductInspection.IdArticleWarehouseInspectionItem == EditProductInspectionArticles.IdArticleWarehouseInspectionItem)
                        {
                            LookupValue tempLookupValue = StatusList.SkipWhile(x => x != itemEditProductInspection.Status).Skip(1).DefaultIfEmpty(StatusList[0]).FirstOrDefault();
                            if (tempLookupValue.IdLookupValue == itemEditProductInspection.Status.IdLookupValue)
                            {
                                if (i > 10)
                                {
                                    itemEditProductInspection.Status = StatusList.Last();
                                    break;
                                }
                                goto goUp;

                            }
                            itemEditProductInspection.Status = StatusList.Where(w => w.IdLookupValue == tempLookupValue.IdLookupValue).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void ProductInspectationMouseClickB_RightAction(object obj)
        {
            try
            {
                EditProductInspectionArticles EditProductInspectionArticles = (EditProductInspectionArticles)obj;
                if (EditProductInspectionArticles.Status.IdLookupValue== 1721)
                {
                    EditProductInspectionArticles.Status = StatusList.Where(w => w.IdLookupValue == 1722).FirstOrDefault();
                }
                else if (EditProductInspectionArticles.Status.IdLookupValue == 1723)
                {
                    EditProductInspectionArticles.Status = StatusList.Where(w => w.IdLookupValue == 1722).FirstOrDefault();
                }
                else
                {
                    EditProductInspectionArticles.Status = StatusList.Where(w => w.IdLookupValue == 1721).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
            }
        }
        public void ProductInspectationMouseClickB_WrongAction(object obj)
        {
            try
            {
                EditProductInspectionArticles EditProductInspectionArticles = (EditProductInspectionArticles)obj;
                if (EditProductInspectionArticles.Status.IdLookupValue == 1721)
                {
                    EditProductInspectionArticles.Status = StatusList.Where(w => w.IdLookupValue == 1723).FirstOrDefault();
                }
                else if (EditProductInspectionArticles.Status.IdLookupValue == 1722)
                {
                    EditProductInspectionArticles.Status = StatusList.Where(w => w.IdLookupValue == 1723).FirstOrDefault();
                }
                else
                {
                    EditProductInspectionArticles.Status = StatusList.Where(w => w.IdLookupValue == 1721).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion


    }
}
