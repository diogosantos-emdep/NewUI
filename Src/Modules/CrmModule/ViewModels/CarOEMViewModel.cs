using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using DevExpress.Xpf.Spreadsheet;
using System;
using DevExpress.Spreadsheet;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DevExpress.XtraPrinting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Logging;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.UI.Commands;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using System.Windows.Media.Imaging;
using System.IO;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Grid;
using System.Data;
using System.Windows.Controls;



namespace Emdep.Geos.Modules.Crm.ViewModels
{
    //[001][kshinde][11/06/2022][GEOS2-243]
    public class CarOEMViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {
        #region Services
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region Declarations

        bool isBusy;
        private ObservableCollection<CarOEM> carOEMList;

        #endregion

        #region Properties
        private ImageSource defaultReferenceImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Warehouse;component/Assets/Images/ImageEditLogo.png"));
        public ObservableCollection<CarOEM> CarOEMList
        {
            get { return carOEMList; }
            set
            {
                carOEMList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CarOEMList"));
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
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }

        #endregion

        #region ICommand
        public ICommand CommandAddCarOEMClick { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand RefreshAccountViewCommand { get; set; }

        #endregion

        #region Constructor

        public CarOEMViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor CarOEMViewModel....", category: Category.Info, priority: Priority.Low);

            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
            {
                DXSplashScreen.Show(x =>
                {
                    System.Windows.Window win = new Window()
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
            PrintButtonCommand = new RelayCommand(new Action<object>(PrintCarOEMAction));
            ExportButtonCommand = new RelayCommand(new Action<object>(ExportCarOEMAction));
            RefreshAccountViewCommand = new RelayCommand(new Action<object>(RefreshCarOEMAction));
            CommandAddCarOEMClick = new RelayCommand(new Action<object>(CommandAddCarOEMAction));
            CommandGridDoubleClick = new RelayCommand(new Action<object>(CommandGridDoubleClickAction));
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Constructor CarOEMViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

        #region methods

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CarOEMViewModel.Init...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

             

                CarOEMList=new ObservableCollection<CarOEM>( CRMService.GetAllCarOEM());

                foreach (CarOEM itemCarOEM in CarOEMList)
                {
                    if (itemCarOEM.CarOEMFileBytes != null)
                    {
                        itemCarOEM.CarOEMImage = ByteArrayToBitmapImage(itemCarOEM.CarOEMFileBytes);
                    }
                    else
                    {
                        itemCarOEM.CarOEMImage = defaultReferenceImage;
                    }
                   
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method CarOEMViewModel.Init() executed successfully...", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in CarOEMViewModel.Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in CarOEMViewModel.Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in CarOEMViewModel.Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
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
        //[001][kshinde][GEOS2-243][21/06/2022]
        private void CommandAddCarOEMAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandAddCarOEMAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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
                //[rdixit][GEOS2-244][22/06/2022]
                AddNewCarOEMView addNewCarOEMView = new AddNewCarOEMView();
                AddNewCarOEMViewModel addNewCarOEMViewModel = new AddNewCarOEMViewModel();
                EventHandler handle = delegate { addNewCarOEMView.Close(); };
                addNewCarOEMViewModel.RequestClose += handle;
                addNewCarOEMViewModel.WindowHeader = Application.Current.FindResource("AddCarOEMViewHeader").ToString();
                addNewCarOEMViewModel.IsNew = true;
                addNewCarOEMViewModel.Init();
                addNewCarOEMView.DataContext = addNewCarOEMViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addNewCarOEMView.ShowDialog();
                if (addNewCarOEMViewModel.IsSave == true)
                {
                    RefreshCarOEMAction(null);
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandAddCarOEMAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][rdixit][GEOS2-243][21/06/2022]
        private void PrintCarOEMAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintCarOEMAction()...", category: Category.Info, priority: Priority.Low);
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
                PrintableControlLink pcl = new PrintableControlLink((CardView)obj);
                
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                
                pcl.PageHeaderTemplate = (DataTemplate)((CardView)obj).Resources["ManagementOrderPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((CardView)obj).Resources["ManagementOrderPrintFooterTemplate"];
                pcl.Landscape = true;
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                pcl.CreateDocument();
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method PrintCarOEMAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintCarOEMAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][rdixit][GEOS2-243][21/06/2022]
        public void ExportCarOEMAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("CarOEMViewModel Method ExportCarOEMAction()...", category: Category.Info, priority: Priority.Low);
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
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "CarOEM List";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (Boolean)saveFile.ShowDialog();             
                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    IsBusy = true;
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
                    ResultFileName = (saveFile.FileName);
                    SpreadsheetControl control = new SpreadsheetControl();
                    if(!File.Exists(ResultFileName))
                    {
                        FileStream fs = new FileStream(ResultFileName, FileMode.Create);
                        fs.Close();
                    }
                    control.LoadDocument(ResultFileName);
                    Worksheet worksheet = control.ActiveWorksheet;                    
                    FileStream stream = null;
                    using (stream = new FileStream(ResultFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        worksheet.Cells[0, 0].Value = "CarOEM Names" ;
                        for (int i = 0; i < CarOEMList.Count; i++)
                        {
                            worksheet.Cells[i + 1, 0].Value = CarOEMList[i].Name;
                        }
                    }                   
                    DevExpress.Spreadsheet.CellRange supplierNamesRange = worksheet.Range["A1:AZ1"];
                    supplierNamesRange.Font.Bold = true;
                    control.SaveDocument();
                    IsBusy = false;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    GeosApplication.Instance.Logger.Log("CarOEMViewModel Method ExportCarOEMAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in CarOEMViewModel Method ExportCarOEMAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][rdixit][GEOS2-243][20/06/2022]
        public void RefreshCarOEMAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CarOEMViewModel.RefreshCarOEMAction()...", category: Category.Info, priority: Priority.Low);
            try
            {
                Init();
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in CarOEMViewModel Method RefreshCarOEMAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][GEOS2-245][25/06/2022]
        public void CommandGridDoubleClickAction(object obj)
        {
            if (GeosApplication.Instance.IsPermissionAdminOnly)
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method CommandGridDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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
                    CardView detailView = (CardView)obj;
                    GridControl gridControl = (detailView).Grid;
                    CarOEM car = (CarOEM)detailView.DataControl.CurrentItem;
                    AddNewCarOEMView addNewCarOEMView = new AddNewCarOEMView();
                    AddNewCarOEMViewModel addNewCarOEMViewModel = new AddNewCarOEMViewModel();
                    EventHandler handle = delegate { addNewCarOEMView.Close(); };
                    addNewCarOEMViewModel.RequestClose += handle;
                    addNewCarOEMViewModel.WindowHeader = Application.Current.FindResource("EditCarOEMViewHeader").ToString();
                    addNewCarOEMViewModel.IsNew = false;
                    addNewCarOEMViewModel.EditInit(car);
                    addNewCarOEMView.DataContext = addNewCarOEMViewModel;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    addNewCarOEMView.ShowDialog();
                    if (addNewCarOEMViewModel.IsUpdated == true)
                    {
                        RefreshCarOEMAction(null);
                    }
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in Method CommandGridDoubleClickAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }          
        }

        #endregion
    }
}
