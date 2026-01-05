using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Spreadsheet;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Commands;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class ExchangeRateViewModel : NavigationViewModelBase,INotifyPropertyChanged
    {
        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }

        #endregion // Services

        #region Declaration
        private IList<DailyCurrencyConversion> exchangeRateList;
        bool isBusy;
        IList<Currency> currencies;
        private TileBarItemsHelper selectedCurrency;
        private ObservableCollection<DailyCurrencyConversion> filteredExchangeRateList;
        public ObservableCollection<CurrencySeries> currencySeries;
        DataTable dtCurrencyExchangeRate;

        #endregion // Declaration

        #region Properties   
        public DataTable DtCurrencyExchangeRate
        {
            get {return dtCurrencyExchangeRate; }
            set { dtCurrencyExchangeRate = value;}
        }
        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }
        public Int16 IdHighestCurrencyConverstionRate { get; set; }
        public ObservableCollection<CurrencySeries> CurrencySeries
        {
            get { return currencySeries; }
            set
            {
                currencySeries = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencySeries"));
            }
        }
        public TileBarItemsHelper SelectedCurrency
        {
            get { return selectedCurrency; }
            set
            {
                selectedCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrency"));
                FillCurrencyExchageByFilter();
                
            }
        }
        public ObservableCollection<TileBarItemsHelper> TileCollectionCurrency { get; set; }
        public IList<Currency> Currencies
        {
            get { return currencies; }
            set { currencies = value; }
        }
        public IList<DailyCurrencyConversion> ExchangeRateList
        {
            get { return exchangeRateList; }
            set { exchangeRateList = value; }
        }

        public ObservableCollection<DailyCurrencyConversion> FilteredExchangeRateList
        {
            get { return filteredExchangeRateList; }
            set { filteredExchangeRateList = value; }
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

        #endregion // Properties

        #region ICommand
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }

       
        #endregion

        #region Events

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
        public ExchangeRateViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ExchangeRateViewModel ...", category: Category.Info, priority: Priority.Low);
                CreateCurrencyTiles();
                FillExchangeRateList();
                SelectedCurrency = TileCollectionCurrency.Where(c => c.Id == GeosApplication.Instance.IdCurrencyByRegion).SingleOrDefault();
                RefreshButtonCommand = new DevExpress.Mvvm.DelegateCommand<Object>(RefreshExchangeRateViewCommandAction);
                PrintButtonCommand = new DevExpress.Mvvm.DelegateCommand<Object>(PrintExchangeRateViewCommandAction);
                ExportButtonCommand = new DevExpress.Mvvm.DelegateCommand<Object>(ExportExchangeRateViewCommandAction);
                MyPreferencesViewModel.IsPreferenceChanged = false;
                GeosApplication.Instance.Logger.Log("Constructor ExchangeRateViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ExchangeRateViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // Constructor

        #region Methods
        /// <summary>
        /// Method for Print Chart's underlaying data as a tabular format
        /// </summary>
        /// <param name="obj"></param>
        public void PrintExchangeRateViewCommandAction(Object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintExchangeRateViewCommandAction ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
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
                //creating a grid Control dynamically 
                System.Windows.Controls.Grid TempGrid = obj as System.Windows.Controls.Grid;   
                GridControl grid = new GridControl();
                grid.View = new TableView();
                grid.AutoGenerateColumns = AutoGenerateColumnsMode.AddNew;
                grid.ItemsSource = CreateDataTable(FilteredExchangeRateList);

                // Passing dynamically created tableview of grid to PrintableControlLink
                PrintableControlLink pcl = new PrintableControlLink((TableView)grid.View);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate =(DataTemplate)(TempGrid.Resources["pageHeader"]);
                pcl.PageFooterTemplate =(DataTemplate)(TempGrid.Resources["pageFooter"]);
                pcl.Landscape = true;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                pcl.CreateDocument(false);

                //Opening Preview Window
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintExchangeRateViewCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintExchangeRateViewCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method to create Datatable for dynamic grid item source binding
        /// </summary>
        /// <param name="tempList"></param>
        /// <returns></returns>
        public DataTable CreateDataTable(ObservableCollection<DailyCurrencyConversion> tempList)
        {
            DtCurrencyExchangeRate = new DataTable();
            DtCurrencyExchangeRate.Columns.Add("Currency From", typeof(string));
            DtCurrencyExchangeRate.Columns.Add("Currency To", typeof(string));
            DtCurrencyExchangeRate.Columns.Add("Date", typeof(DateTime));
            DtCurrencyExchangeRate.Columns.Add("Exchange Rate", typeof(decimal));

            foreach (DailyCurrencyConversion item in tempList)
            {
                if (item.IdCurrencyConversionFrom != item.IdCurrencyConversionTo)
                {
                    DataRow dr = DtCurrencyExchangeRate.NewRow();
                    dr["Currency From"] = item.CurrencyFrom.Name.ToString();
                    dr["Currency To"] = item.CurrencyTo.Name.ToString();
                    dr["Date"] = item.CurrencyConversionDate.Date.ToShortDateString();
                    dr["Exchange Rate"] =Math.Round(item.CurrencyConversationRate,2);
                    DtCurrencyExchangeRate.Rows.Add(dr);
                }
            }
            return DtCurrencyExchangeRate;
        }

        /// <summary>
        /// Method for Export to excel chart's underlaying data
        /// </summary>
        /// <param name="obj"></param>
        public void ExportExchangeRateViewCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportExchangeRateViewCommandAction ...", category: Category.Info, priority: Priority.Low);
                SaveFileDialogService.DefaultExt = "xlsx";
                SaveFileDialogService.DefaultFileName = SelectedCurrency.Caption+"_Currency Exchange Rate";
                SaveFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (.)|*.*";
                SaveFileDialogService.FilterIndex = 1;
                DialogResult = SaveFileDialogService.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    IsBusy = true;
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

                    ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;         
                    Workbook workbook = new Workbook();
                    Worksheet ws = workbook.Worksheets["Sheet1"];

                    ws.Cells[0, 0].Value = "Currency From";
                    ws.Cells[0, 1].Value = "Currency To";
                    ws.Cells[0, 2].Value = "Date";       
                    ws.Cells[0, 3].Value = "Exchange Rate";

                    ws.Range["A1:D1"].Font.Bold = true;
                    ws.Range["A1:D1"].ColumnWidth = 500;
                    ws.Range["A1:D1"].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);
                    //ws.Range["A1:C1"].Fill.BackgroundColor = System.Drawing.Color.LightGray;

                    int counter = 1;
                    foreach (DailyCurrencyConversion curr in FilteredExchangeRateList)
                    {
                        if(curr.IdCurrencyConversionFrom!=curr.IdCurrencyConversionTo)
                        {
                            ws.Cells[counter, 0].Value = curr.CurrencyFrom.Name.ToString();
                            ws.Cells[counter, 0].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                            ws.Cells[counter, 1].Value = curr.CurrencyTo.Name.ToString();
                            ws.Cells[counter, 1].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                            ws.Cells[counter, 2].Value = curr.CurrencyConversionDate.Date.ToShortDateString();
                            ws.Cells[counter, 2].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                            ws.Cells[counter, 3].Value =Math.Round(curr.CurrencyConversationRate,2);
                            ws.Cells[counter, 3].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);
                            counter++;
                        }
                    }

                    using (FileStream stream = new FileStream(ResultFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        workbook.SaveDocument(stream, DocumentFormat.OpenXml);
                    }
                    IsBusy = false;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                }
               
                GeosApplication.Instance.Logger.Log("Method ExportExchangeRateViewCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportExchangeRateViewCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }

        /// <summary>
        /// Method for Refresh View
        /// </summary>
        /// <param name="obj"></param>
        public void RefreshExchangeRateViewCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshExchangeRateViewCommandAction ...", category: Category.Info, priority: Priority.Low);
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

                FillExchangeRateList();
                if (MyPreferencesViewModel.IsPreferenceChanged)
                    SelectedCurrency = TileCollectionCurrency.Where(c => c.Id == GeosApplication.Instance.IdCurrencyByRegion).SingleOrDefault();
                else
                    SelectedCurrency = SelectedCurrency;
                MyPreferencesViewModel.IsPreferenceChanged = false;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshExchangeRateViewCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshExchangeRateViewCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshExchangeRateViewCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshExchangeRateViewCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method To Create Currency Tiles
        /// </summary>
        public void CreateCurrencyTiles()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateCurrencyTiles ...", category: Category.Info, priority: Priority.Low);
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


                Currencies = CrmStartUp.GetCurrencies();
                //For Currency Tile collection
                TileCollectionCurrency = new ObservableCollection<TileBarItemsHelper>();
                string strCurrency = "";
                foreach (var curr in Currencies)
                {
                    switch (curr.Name)
                    {
                        case "INR":
                            strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/INR.png";
                            break;

                        case "BRL":
                            strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/BRL.png";
                            break;

                        case "CNY":
                            strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/CNY.png";
                            break;

                        case "EUR":
                            strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/EUR.png";
                            break;

                        case "HNL":
                            strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/HNL.png";
                            break;

                        case "MAD":
                            strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/MAD.png";
                            break;

                        case "MXN":
                            strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/MXN.png";
                            break;

                        case "PYG":
                            strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/PYG.png";
                            break;

                        case "RON":
                            strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/RON.png";
                            break;

                        case "RUB":
                            strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/RUB.png";
                            break;

                        case "TND":
                            strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/TND.png";
                            break;

                        case "USD":
                            strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/USD.png";
                            break;

                        case "GBP":
                            strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/GBP.png";
                            break;

                        case "CAD":
                            strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/CAD.png";
                            break;
                        default:
                            if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                            {
                                strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/NotAvailable.png";
                            }
                            else
                            {
                                strCurrency = @"/Emdep.Geos.Modules.Crm;component/Assets/Images/NotAvailableBlue.png";
                            }
                            break;
                    }
                    TileCollectionCurrency.Add(new TileBarItemsHelper()
                    {
                        Tag = curr,
                        Id = curr.IdCurrency,
                        Caption = curr.Name,
                        GlyphUri = strCurrency,
                        Height = 30,

                    });

                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method CreateCurrencyTiles() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CreateCurrencyTiles() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
      
        /// <summary>
        /// Method to Get All Exchange Rate Data
        /// </summary>
        public void FillExchangeRateList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillExchangeRateList ...", category: Category.Info, priority: Priority.Low);
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
                ExchangeRateList = new List<DailyCurrencyConversion>();
                ExchangeRateList = CrmStartUp.GetAllDailyCurrencyConversionsByDate(GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate);
                
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillExchangeRateList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExchangeRateList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExchangeRateList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExchangeRateList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for getting series data as per Currency tile selection
        /// </summary>
        public void FillCurrencyExchageByFilter()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrencyExchageByFilter ...", category: Category.Info, priority: Priority.Low);
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
                FilteredExchangeRateList = new ObservableCollection<DailyCurrencyConversion>();

                foreach (DailyCurrencyConversion item in ExchangeRateList)
                {
                        if (item.IdCurrencyConversionFrom == SelectedCurrency.Id)
                                FilteredExchangeRateList.Add(new DailyCurrencyConversion() { CurrencyConversationRate = item.CurrencyConversationRate, IdCurrencyConversionFrom = item.IdCurrencyConversionFrom, IdCurrencyConversionTo = item.IdCurrencyConversionTo, CurrencyConversionDate = item.CurrencyConversionDate, CurrencyFrom = item.CurrencyFrom, CurrencyTo = item.CurrencyTo, });
                }

                CurrencySeries = new ObservableCollection<ViewModels.CurrencySeries>();

                if (FilteredExchangeRateList != null && FilteredExchangeRateList.Count > 0)
                {
                    IdHighestCurrencyConverstionRate = FilteredExchangeRateList.Where(p => p.CurrencyConversationRate == FilteredExchangeRateList.Where(c => c.IdCurrencyConversionFrom == Convert.ToByte(SelectedCurrency.Id.ToString())).Max(o => o.CurrencyConversationRate)).FirstOrDefault().IdCurrencyConversionTo;

                    foreach (var item in Currencies)
                    {
                        if (SelectedCurrency.Id != item.IdCurrency)
                        {
                            if (item.IdCurrency != IdHighestCurrencyConverstionRate)
                                CurrencySeries.Add(new ViewModels.CurrencySeries { CurrencySeriesName = SelectedCurrency.Caption + "-" + item.Name, Values = new ObservableCollection<DailyCurrencyConversion> { }, SeriesColor = FillSeriesColor(item.IdCurrency), AxisYName = "AxisY1" });
                            else
                                CurrencySeries.Add(new ViewModels.CurrencySeries { CurrencySeriesName = SelectedCurrency.Caption + "-" + item.Name, Values = new ObservableCollection<DailyCurrencyConversion> { }, SeriesColor = FillSeriesColor(item.IdCurrency), AxisYName = "AxisY2" });
                        }
                    }

                    foreach (DailyCurrencyConversion curr in FilteredExchangeRateList)
                    {
                        foreach (var item in CurrencySeries)
                        {
                            if (item.CurrencySeriesName == SelectedCurrency.Caption + "-" + curr.CurrencyTo.Name)
                                item.Values.Add(new DailyCurrencyConversion { CurrencyConversationRate = float.Parse(curr.CurrencyConversationRate.ToString("n2")), CurrencyConversionDate = curr.CurrencyConversionDate, AxisYName = item.AxisYName });
                        }
                    }
                }
                FilteredExchangeRateList = new ObservableCollection<DailyCurrencyConversion>(FilteredExchangeRateList.OrderBy(x => x.IdCurrencyConversionTo).ToList());
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillCurrencyExchageByFilter() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyExchageByFilter() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for setting series color
        /// </summary>
        /// <param name="ColorId"></param>
        /// <returns></returns>
        public SolidColorBrush FillSeriesColor(Int32 ColorId)
        {
            try
            {
                switch (ColorId)
                {
                    case 1:
                        return Brushes.LimeGreen;
                        break;

                    case 2:
                        return Brushes.IndianRed;
                        break;

                    case 3:
                        return Brushes.DarkOrange;
                        break;

                    case 4:
                        return Brushes.RoyalBlue;
                        break;

                    case 5:
                        return Brushes.DarkOliveGreen;
                        break;

                    case 6:
                        return Brushes.DarkGreen;
                        break;

                    case 7:
                        return Brushes.Magenta;
                        break;

                    case 8:
                        return Brushes.SandyBrown;
                        break;

                    case 9:
                        return Brushes.PaleVioletRed;
                        break;

                    case 10:
                        return Brushes.OrangeRed;
                        break;

                    case 11:
                        return Brushes.DarkSalmon;
                        break;

                    case 12:
                        return Brushes.DarkOrchid;
                        break;

                    case 13:
                        return Brushes.Firebrick;
                        break;

                    case 14:
                        return Brushes.DarkTurquoise;
                        break;
                    default:
                        return Brushes.Navy;
                        break;
                }
                return Brushes.Transparent;
            }
            catch (Exception ex)
            {
                return Brushes.Transparent;
            }
        }

        #endregion //Methods
    }

    #region MappedClass
    public class CurrencySeries
    {
        public String CurrencySeriesName { get; set; }
        public SolidColorBrush SeriesColor { get; set; }
        public ObservableCollection<DailyCurrencyConversion> Values { get; set; }
        public string AxisYName { get; set; }
    }
    #endregion

}
