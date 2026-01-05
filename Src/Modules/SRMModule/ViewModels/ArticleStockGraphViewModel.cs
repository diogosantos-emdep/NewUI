using ClosedXML.Excel;
using DevExpress.Mvvm;
using DevExpress.Xpf.Charts;
using DevExpress.XtraExport.Helpers;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AxisTitle = DevExpress.Xpf.Charts.AxisTitle;


namespace Emdep.Geos.Modules.SRM.ViewModels
{
    //[rdixit][GEOS2-8247][04.11.2025]
    public class ArticleStockGraphViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {

        #region Services
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null)
            ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl
            : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        CultureInfo selectedCulture;
        string wareHouseName;
        string preOrderAERO;
        string actualStock;
        string stock12MonthsAgo;
        string averageLast3Months;
        string average9to12MonthsAgo;
        private string informationError;
        string windowHeader;
        bool is12MonthAvgGreater;
        bool is3MonthAvgGreater;
        DataTable dataTableAricleGraph;
        DataTable dataTableGraph;
        string averageLast3MonthsPercentage;
        string average9to12MonthsAgoPercentage;
        #endregion

        #region Properties
        public bool Is12MonthAvgGreater
        {
            get
            {
                return is12MonthAvgGreater;
            }
            set
            {
                is12MonthAvgGreater = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Is12MonthAvgGreater"));
            }
        }

        public bool Is3MonthAvgGreater
        {
            get
            {
                return is3MonthAvgGreater;
            }
            set
            {
                is3MonthAvgGreater = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Is3MonthAvgGreater"));
            }
        }
        public CultureInfo SelectedCulture
        {
            get
            {
                return selectedCulture;
            }
            set
            {
                selectedCulture = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCulture"));
            }
        }
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }
        public DataTable DataTableGraph
        {
            get { return dataTableGraph; }
            set
            {
                dataTableGraph = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableGraph"));
            }
        }
        public System.Data.DataTable DataTableAricleGraph
        {
            get { return dataTableAricleGraph; }
            set
            {
                dataTableAricleGraph = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableAricleGraph"));
            }
        }
        public string WareHouseName
        {
            get { return wareHouseName; }
            set
            {
                wareHouseName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WareHouseName"));
            }
        }
        public string PreOrderAERO
        {
            get { return preOrderAERO; }
            set
            {
                preOrderAERO = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreOrderAERO"));
            }
        }
        public string ActualStock
        {
            get { return actualStock; }
            set
            {
                actualStock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActualStock"));
            }
        }
        public string Stock12MonthsAgo
        {
            get { return stock12MonthsAgo; }
            set
            {
                stock12MonthsAgo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Stock12MonthsAgo"));
            }
        }
        public string AverageLast3Months
        {
            get { return averageLast3Months; }
            set
            {
                averageLast3Months = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AverageLast3Months"));
            }
        }
        public string Average9to12MonthsAgo
        {
            get { return average9to12MonthsAgo; }
            set
            {
                average9to12MonthsAgo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Average9to12MonthsAgo"));
            }
        }
        public string AverageLast3MonthsPercentage
        {
            get { return averageLast3MonthsPercentage; }
            set
            {
                averageLast3MonthsPercentage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AverageLast3MonthsPercentage"));
            }
        }
        public string Average9to12MonthsAgoPercentage
        {
            get { return average9to12MonthsAgoPercentage; }
            set
            {
                average9to12MonthsAgoPercentage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Average9to12MonthsAgoPercentage"));
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

        public void Dispose()
        {
        }
        #endregion

        #region Commands
        public ICommand ChartArticleGraphLoadCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        #endregion

        #region Constructor
        public ArticleStockGraphViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ArticleStockGraphViewModel...", category: Category.Info, priority: Priority.Low);            
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                ChartArticleGraphLoadCommand = new DelegateCommand<RoutedEventArgs>(ChartArticleGraphLoadCommandAction);
                GeosApplication.Instance.Logger.Log("Constructor ArticleStockGraphViewModel() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (System.Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in ArticleStockGraphViewModel() Constructor - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods

        public void Init(PreOrder preOrder)
        {
            try
            {
                if (preOrder != null)
                {
                   
                    CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                    SelectedCulture = cultures.ToList().FirstOrDefault(i => i.TwoLetterISOLanguageName?.ToLower() == preOrder.Currency.ISOCode?.ToLower());
                    WindowHeader = "Graphs and Metrics - ReOrder Code: " + preOrder.Code;
                    PreOrderAERO = Math.Round(preOrder.TotalValue, 2, MidpointRounding.AwayFromZero).ToString("N", SelectedCulture);
                    WareHouseName = preOrder.Warehouse?.Name + " Warehouse Material KPI";
                }
            }          
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void LoadExcelToDataTable(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                    throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

                if (!File.Exists(filePath))
                    throw new FileNotFoundException("Excel file not found.", filePath);

                DataTableAricleGraph = new System.Data.DataTable();
                DataTableAricleGraph.Columns.Add("CW", typeof(string));
                DataTableAricleGraph.Columns.Add("WeekDate", typeof(DateTime));
                DataTableAricleGraph.Columns.Add("YearWeek", typeof(string));
                DataTableAricleGraph.Columns.Add("Last_0_90", typeof(double));
                DataTableAricleGraph.Columns.Add("Last_270_365", typeof(double));

                using (var workbook = new XLWorkbook(filePath))
                {
                    var ws = workbook.Worksheet(1);
                    int cwRow = 1;
                    int dateRow = 2;
                    int yearWeek = 3;
                    int last090Row = 4;
                    int last270365Row = 5;                
                    int col = 3;
                    CalculateValues(ws, last090Row, cwRow);

                    // Loop until CW cell is empty
                    while (!ws.Cell(cwRow, col).IsEmpty())
                    {
                        try
                        {
                            string cw = ws.Cell(cwRow, col).GetString();
                            string yw = ws.Cell(yearWeek, col).GetString();
                            string dateText = ws.Cell(dateRow, col).GetString();

                            // Parse values safely
                            double.TryParse(ws.Cell(last090Row, col).GetString(), out double last090);
                            double.TryParse(ws.Cell(last270365Row, col).GetString(), out double last270365);

                            // Parse date safely
                            DateTime.TryParse(dateText, out DateTime date);

                            // Add row
                            DataTableAricleGraph.Rows.Add(cw, date, yw, last090, last270365);

                            col++;
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log($"Unexpected error while loading Excel data: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }
            }
            catch (FileNotFoundException fnfEx)
            {
                GeosApplication.Instance.Logger.Log($"File not found: {fnfEx.Message}", category: Category.Exception, priority: Priority.Low);
            }
            catch (IOException ioEx)
            {
                GeosApplication.Instance.Logger.Log($"File I/O error: {ioEx.Message}", category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Unexpected error while loading Excel data: {ex.Message}", category: Category.Exception, priority: Priority.Low);
            }
        }

        public void CalculateValues(IXLWorksheet ws, int last090Row,int cwRow)
        {
            try
            {
                int weeksAgo = 52;
                var cuweek = Convert.ToString(ws.Cell(1, 3)).Take(2);

                var actualstockValue = Math.Round(ws.Cell(4, 3).GetDouble(), 2, MidpointRounding.AwayFromZero);
                var avg3Months = Math.Round(ws.Cell(4, 2).GetDouble(), 2, MidpointRounding.AwayFromZero);
                var avg9to12Months = Math.Round(ws.Cell(5, 2).GetDouble(), 2, MidpointRounding.AwayFromZero);
                var avgLast3MonthsPer = Math.Round((avg3Months - actualstockValue) / avg3Months, 2, MidpointRounding.AwayFromZero);
                var avgLast9to12MonthsPer = Math.Round((avg9to12Months - actualstockValue) / avg9to12Months, 2, MidpointRounding.AwayFromZero);


                ActualStock = Math.Round(ws.Cell(4, 3).GetDouble(), 2, MidpointRounding.AwayFromZero).ToString("N", SelectedCulture);
                Stock12MonthsAgo = Math.Round(ws.Cell(last090Row, cwRow + weeksAgo).GetDouble(), 2, MidpointRounding.AwayFromZero).ToString("N", SelectedCulture);
                AverageLast3Months = avg3Months.ToString("N", SelectedCulture);
                Average9to12MonthsAgo = avg9to12Months.ToString("N", SelectedCulture);
                AverageLast3MonthsPercentage = avgLast3MonthsPer.ToString("N", SelectedCulture) + "%";
                Average9to12MonthsAgoPercentage = avgLast9to12MonthsPer.ToString("N", SelectedCulture) + "%";

                if (avgLast3MonthsPer < 0)
                {
                    Is3MonthAvgGreater = true;
                }
                else
                {
                    Is3MonthAvgGreater = false;
                }
                if (avgLast9to12MonthsPer < 0)
                {
                    Is12MonthAvgGreater = true;
                }
                else
                {
                    Is12MonthAvgGreater = false;
                }
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Unexpected error while loading Excel data: {ex.Message}", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChartArticleGraphLoadCommandAction(RoutedEventArgs obj)
        {
            try
            {                
                ChartControl chartControl = (ChartControl)obj?.OriginalSource;
                if (chartControl == null)
                    return;

                ArrangeTableBackword();
                chartControl.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();

                diagram.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
                diagram.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                diagram.ActualAxisY.Label = new AxisLabel();
                diagram.ActualAxisY.Label.TextPattern = "{V:N2}";
                diagram.ActualAxisX.Label = new AxisLabel();
                diagram.ActualAxisX.Label.Visible = false;

                AreaSeries2D areaSeries = new AreaSeries2D
                {
                    ArgumentDataMember = "YearWeek",
                    ValueDataMember = "Last_0_90",
                    ArgumentScaleType = ScaleType.Qualitative,
                    ValueScaleType = ScaleType.Numerical,
                    Brush = new SolidColorBrush(Color.FromRgb(20, 50, 120)), // Dark blue fill
                    Transparency = 0.25,
                    Border = new SeriesBorder(),
                    BorderThickness = new System.Windows.Thickness(2),
                    DisplayName = "Stock Value"
                };
                diagram.Series.Add(areaSeries);


                LineSeries2D lineDashedTargetPlant = new LineSeries2D();
                lineDashedTargetPlant.LineStyle = new LineStyle();
                lineDashedTargetPlant.LineStyle.DashStyle = new DashStyle();
                lineDashedTargetPlant.LineStyle.Thickness = 2;
                lineDashedTargetPlant.Brush = new SolidColorBrush(Colors.Red);
                lineDashedTargetPlant.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
                lineDashedTargetPlant.ArgumentScaleType = ScaleType.Qualitative;
                lineDashedTargetPlant.ValueScaleType = ScaleType.Numerical;
                lineDashedTargetPlant.DisplayName = "Lenear Graph";
                //lineDashedTargetPlant.CrosshairLabelPattern = "{S} : {V:c2}";

                lineDashedTargetPlant.ArgumentDataMember = "YearWeek";
                lineDashedTargetPlant.ValueDataMember = "Last_270_365";
                diagram.Series.Add(lineDashedTargetPlant);

                chartControl.Diagram = diagram;
                chartControl.EndInit();
                chartControl.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControl.Animate();
                
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in ChartArticleGraphLoadCommandAction: {ex.Message}", Category.Exception, Priority.Low);
            }
        }


        public void ArrangeTableBackword()//OldWeeks first
        {
            try
            {
                var temp = new DataTable();
                temp.Columns.Add("YearWeek", typeof(string));
                temp.Columns.Add("Last_0_90", typeof(double));
                temp.Columns.Add("Last_270_365", typeof(double));

                int ind = DataTableAricleGraph.Rows.Count;

                for (int i = ind - 1; i >= 0; i--) // include index 0
                {
                    try
                    {

                        var sourceRow = DataTableAricleGraph.Rows[i];
                        var newRow = temp.NewRow();

                        newRow["YearWeek"] = sourceRow["YearWeek"];
                        newRow["Last_0_90"] = sourceRow["Last_0_90"];
                        newRow["Last_270_365"] = sourceRow["Last_270_365"];

                        temp.Rows.Add(newRow);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log($"Error in ArrangeTableBackword Method: {ex.Message}", Category.Exception, Priority.Low);
                    }
                }
                DataTableGraph = temp;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in ArrangeTableBackword Method: {ex.Message}", Category.Exception, Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        #endregion
    }
}
