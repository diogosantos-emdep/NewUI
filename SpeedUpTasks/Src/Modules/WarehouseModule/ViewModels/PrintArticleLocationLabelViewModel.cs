using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Warehouse.Common_Classes;
using Emdep.Geos.Modules.Warehouse.Reports;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing.Printing;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.BarCode;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class PrintArticleLocationLabelViewModel
    {
        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Declaration

        private ObservableCollection<LookupValue> labelSizeList;
        private LookupValue selectedLabelSize;

        private ObservableCollection<Article> articlesList;
        private List<LabelStyle> images;
        private LabelStyle selectedImageStyle;
        private bool isIncludeArticlePhoto;

        #endregion // Declaration

        #region Properties
        public ObservableCollection<LookupValue> LabelSizeList
        {
            get { return labelSizeList; }
            set
            {
                labelSizeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LabelSizeList"));
            }
        }

        public LookupValue SelectedLabelSize
        {
            get { return selectedLabelSize; }
            set
            {
                selectedLabelSize = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLabelSize"));
            }
        }

        public ObservableCollection<Article> ArticlesList
        {
            get
            {
                return articlesList;
            }

            set
            {
                articlesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationsList"));
            }
        }

        public List<LabelStyle> Images
        {
            get
            {
                return images;
            }
            set
            {
                images = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Images"));
            }
        }

        public LabelStyle SelectedImageStyle
        {
            get
            {
                return selectedImageStyle;
            }
            set
            {
                selectedImageStyle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImageStyle"));
            }
        }

        public bool IsIncludeArticlePhoto
        {
            get
            {
                return isIncludeArticlePhoto;
            }
            set
            {
                isIncludeArticlePhoto = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsIncludeArticlePhoto"));
            }
        }
        #endregion

        #region Commands
        public ICommand CloseWindowCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
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

        public PrintArticleLocationLabelViewModel()
        {
            CloseWindowCommand = new DelegateCommand<object>(CloseWindowAction);
            AcceptButtonCommand = new DelegateCommand<object>(PrintArticleLocationLabelCommandAction);
        }

        #endregion

        #region Methods

        /// <summary>
        /// [000][skale][6-11-2019][GEOS2-70] Add new option in warehouse to print Reference labels
        /// </summary>
        /// <param name="articlesList"></param>
        public void Init(ObservableCollection<Article> articlesList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init....", category: Category.Info, priority: Priority.Low);

                FillLabelSizeList();
                FillLabelStyleList();
                ArticlesList = articlesList;
                IsIncludeArticlePhoto = true;

                GeosApplication.Instance.Logger.Log("Method Init....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// method is for to fill the Images List
        /// </summary>
        private void FillLabelStyleList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLabelStyleList()...", category: Category.Info, priority: Priority.Low);
                Images = new List<LabelStyle>();
                LabelStyle labelStyleOne = new LabelStyle();
                labelStyleOne.Image = "/Emdep.Geos.Modules.Warehouse;component/Assets/Images/w_LabelStyleOne.png";
                LabelStyle labelStyleTwo = new LabelStyle();
                labelStyleTwo.Image = "/Emdep.Geos.Modules.Warehouse;component/Assets/Images/w_LabelStyleTwo.png";
                Images.Add(labelStyleOne);
                Images.Add(labelStyleTwo);
                SelectedImageStyle = Images[0];
                GeosApplication.Instance.Logger.Log("Method FillLabelStyleList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLabelStyleList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this method use for fill Lable Size 
        /// [000][skale][6-11-2019][GEOS2-70] Add new option in warehouse to print Reference labels
        /// </summary>
        private void FillLabelSizeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLabelSizeList()...", category: Category.Info, priority: Priority.Low);
                LabelSizeList = new ObservableCollection<LookupValue>(CrmService.GetLookupValues(55).ToList());
                SelectedLabelSize = LabelSizeList[0];
                GeosApplication.Instance.Logger.Log("Method FillLabelSizeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLabelSizeList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLabelSizeList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillLabelSizeList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Close window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindowAction(object obj)
        {
            try
            {
                //GeosApplication.Instance.Logger.Log("Method CloseWindowAction()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                //GeosApplication.Instance.Logger.Log("Method CloseWindowAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseWindowAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PrintArticleLocationLabelCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintArticleLocationLabelCommandAction()...", category: Category.Info, priority: Priority.Low);

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

                string[] splitLabelSize = SelectedLabelSize.Value.Split('x');

                if (SelectedImageStyle == Images[0])
                {
                    PrintArticleLocationLabelFirstStyle(Convert.ToInt32(splitLabelSize[0]), Convert.ToInt32(splitLabelSize[1]));
                }
                else
                {
                    PrintArticleLocationLabel(Convert.ToInt32(splitLabelSize[0]), Convert.ToInt32(splitLabelSize[1]));
                }

                // PrintArticleLocationLabel(Convert.ToInt32(splitLabelSize[0]), Convert.ToInt32(splitLabelSize[1]));
                //PrintArticleLocationLabelNoPic(Convert.ToInt32(splitLabelSize[0]), Convert.ToInt32(splitLabelSize[1]));
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintArticleLocationLabelCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintArticleLocationLabelCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this method use for print Article label
        /// [000][skale][6-11-2019][GEOS2-70] Add new option in warehouse to print Reference labels
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void PrintArticleLocationLabel(int width, int height)
        {
            try
            {
                //double convertMmToPixel = 3.7795275591;
                double MmToPixel = 3.969000400416382; //convert 1mm to pixel

                WarehoseLocationsLabelReport articlesLocationLabelReport = new WarehoseLocationsLabelReport();
                articlesLocationLabelReport.xrRefBarCode.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrArticleBarcode_BeforePrint);

                // convert mm to pixel
                double mainPanelheight = Convert.ToDouble(height) * MmToPixel;
                double mainPanelWidth = Convert.ToDouble(width) * MmToPixel;

                //main panel
                articlesLocationLabelReport.xrPanel.HeightF = (float)mainPanelheight;
                articlesLocationLabelReport.xrPanel.WidthF = (float)mainPanelWidth;
                articlesLocationLabelReport.xrPanel.LocationF = new PointF(10f, 10.42f);
                //articlesLocationLabelReport.xrPanel.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Top;

                //Article Location Name1
                double articleLocationHeight1 = ((float)height * 26) / 100;
                double articleLocationWidth1 = ((float)width * 8) / 100;           //13.5
                articlesLocationLabelReport.xrlblLocation1.HeightF = (float)(articleLocationHeight1 * MmToPixel);
                articlesLocationLabelReport.xrlblLocation1.WidthF = (float)(articleLocationWidth1 * MmToPixel);
                //articlesLocationLabelReport.xrlblLocation1.LocationF = new PointF(0f, (float)(articleLocationBarcodeHeight * MmToPixel));

                //Article Location Name2
                double articleLocationHeight2 = ((float)height * 26) / 100;
                double articleLocationWidth2 = ((float)width * 8) / 100;               //13.5
                articlesLocationLabelReport.xrlblLocation2.HeightF = (float)(articleLocationHeight2 * MmToPixel);
                articlesLocationLabelReport.xrlblLocation2.WidthF = (float)(articleLocationWidth2 * MmToPixel);
                articlesLocationLabelReport.xrlblLocation2.LocationF = new PointF((float)(articleLocationWidth1 * MmToPixel), 0.4f);         //(float)(articleLocationBarcodeHeight * MmToPixel)

                //Article Location Name3
                double articleLocationHeight3 = ((float)height * 26) / 100;
                double articleLocationWidth3 = ((float)width * 14) / 100;                     //27
                articlesLocationLabelReport.xrlblLocation3.HeightF = (float)(articleLocationHeight3 * MmToPixel);
                articlesLocationLabelReport.xrlblLocation3.WidthF = (float)(articleLocationWidth3 * MmToPixel);
                articlesLocationLabelReport.xrlblLocation3.LocationF = new PointF(((float)(articleLocationWidth1 * MmToPixel) + (float)(articleLocationWidth2 * MmToPixel)), 0.4f);

                //Article Location Barcode
                double articleLocationBarcodeHeight = ((float)height * 26) / 100;
                double articleLocationBarcodeWidth = ((float)width * 40) / 100;             //27
                articlesLocationLabelReport.xrLocationBarCode.HeightF = (float)(articleLocationBarcodeHeight * MmToPixel);
                articlesLocationLabelReport.xrLocationBarCode.WidthF = (float)(articleLocationBarcodeWidth * MmToPixel);
                articlesLocationLabelReport.xrLocationBarCode.LocationF = new PointF(((float)(articleLocationWidth1 * MmToPixel) + (float)(articleLocationWidth2 * MmToPixel) + (float)(articleLocationWidth3 * MmToPixel)), 0.4f);

                if (width == 80)
                {
                    //articlesLocationLabelReport.xrLocationBarCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 18, 4, 4);
                    //articlesLocationLabelReport.xrRefBarCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5);
                }


                //Article Image
                if (IsIncludeArticlePhoto == true)
                {
                    double articleImageHeight = height;
                    double articleImageWidth = ((float)width * 78) / 100;             //22
                    articlesLocationLabelReport.xrImage.HeightF = (float)(articleImageHeight * MmToPixel);
                    articlesLocationLabelReport.xrImage.WidthF = (float)(articleImageWidth * MmToPixel);
                    articlesLocationLabelReport.xrImage.LocationF = new PointF(((float)(articleLocationWidth1 * MmToPixel) + (float)(articleLocationWidth2 * MmToPixel) + (float)(articleLocationWidth3 * MmToPixel) + (float)(articleLocationBarcodeWidth * MmToPixel)), 0.4f);

                    //Article Reference
                    double articleReferenceHeight = ((float)height * 32) / 100;
                    double articleReferenceWidth = ((float)width * 70) / 100;                   //51.5
                    articlesLocationLabelReport.xrRefrence.HeightF = (float)(articleReferenceHeight * MmToPixel);
                    articlesLocationLabelReport.xrRefrence.WidthF = (float)(articleReferenceWidth * MmToPixel);
                    articlesLocationLabelReport.xrRefrence.LocationF = new PointF(0f, (float)(articleLocationHeight1 * MmToPixel));               //(float)(articleReferenceWeight * MmToPixel)/4
                    articlesLocationLabelReport.xrRefrence.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 24, System.Drawing.FontStyle.Bold);


                    //Article Barcode
                    double articleBarcodeHeight = ((float)height * 36) / 100;
                    double articleBarcodeWidth = ((float)width * 70) / 100;                     //51.5

                    articlesLocationLabelReport.xrRefBarCode.HeightF = (float)(articleBarcodeHeight * MmToPixel);
                    articlesLocationLabelReport.xrRefBarCode.WidthF = (float)(articleBarcodeWidth * MmToPixel);
                    articlesLocationLabelReport.xrRefBarCode.LocationF = new PointF(0f, (float)(articleLocationHeight1 * MmToPixel) + (float)(articleReferenceHeight * MmToPixel));

                }
                else
                {
                    articlesLocationLabelReport.xrImage.Visible = false;

                    //Article Location Barcode
                    //double articleLocationBarcodeHeight = ((float)height * 26) / 100;
                    double articleLocationBarcodeWidthWithoutImage = ((float)width * 60) / 100;             //27
                    articlesLocationLabelReport.xrLocationBarCode.HeightF = (float)(articleLocationBarcodeHeight * MmToPixel);
                    articlesLocationLabelReport.xrLocationBarCode.WidthF = (float)(articleLocationBarcodeWidthWithoutImage * MmToPixel);
                    articlesLocationLabelReport.xrLocationBarCode.LocationF = new PointF(((float)(articleLocationWidth1 * MmToPixel) + (float)(articleLocationWidth2 * MmToPixel) + (float)(articleLocationWidth3 * MmToPixel)), 0.4f);
                    //articlesLocationLabelReport.xrLocationBarCode.CanGrow = false;
                    if (width == 80)
                    {
                        //articlesLocationLabelReport.xrLocationBarCode.Alignment = new DevExpress.XtraPrinting.TextAlignment();
                        //articlesLocationLabelReport.xrLocationBarCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(10,10, 4, 4);
                        //articlesLocationLabelReport.xrLocationBarCode.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 4, 4);
                        //articlesLocationLabelReport.xrRefBarCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5);
                    }

                    //Article Reference
                    double articleReferenceHeight = ((float)height * 32) / 100;
                    double articleReferenceWidth = width;// ((float)Width * 70) / 100;                   //51.5
                    articlesLocationLabelReport.xrRefrence.HeightF = (float)(articleReferenceHeight * MmToPixel);
                    articlesLocationLabelReport.xrRefrence.WidthF = (float)(articleReferenceWidth * MmToPixel);
                    articlesLocationLabelReport.xrRefrence.LocationF = new PointF(0f, (float)(articleLocationHeight1 * MmToPixel));               //(float)(articleReferenceWeight * MmToPixel)/4
                    articlesLocationLabelReport.xrRefrence.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 24, System.Drawing.FontStyle.Bold);

                    //Article Barcode
                    double articleBarcodeHeight = ((float)height * 36) / 100;
                    double articleBarcodeWidth = width;// ((float)Width * 70) / 100;                     //51.5

                    articlesLocationLabelReport.xrRefBarCode.HeightF = (float)(articleBarcodeHeight * MmToPixel);
                    articlesLocationLabelReport.xrRefBarCode.WidthF = (float)(articleBarcodeWidth * MmToPixel);
                    articlesLocationLabelReport.xrRefBarCode.LocationF = new PointF(0f, (float)(articleLocationHeight1 * MmToPixel) + (float)(articleReferenceHeight * MmToPixel));               // (float)(articleReferenceHeight * MmToPixel)
                }

                ////Article Reference
                //double articleReferenceHeight = ((float)height * 32) / 100;
                //double articleReferenceWidth = ((float)weight * 70) / 100;                   //51.5
                //articlesLocationLabelReport.xrRefrence.HeightF = (float)(articleReferenceHeight * MmToPixel);
                //articlesLocationLabelReport.xrRefrence.WidthF = (float)(articleReferenceWeight * MmToPixel);
                //articlesLocationLabelReport.xrRefrence.LocationF = new PointF(0f,(float)(articleLocationHeight1* MmToPixel));               //(float)(articleReferenceWeight * MmToPixel)/4
                //articlesLocationLabelReport.xrRefrence.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 24, System.Drawing.FontStyle.Bold);

                ////Article Barcode
                //double articleBarcodeHeight = ((float)height * 36) / 100;                       
                //double articleBarcodeWeight = ((float)weight * 70) / 100;                     //51.5

                //articlesLocationLabelReport.xrRefBarCode.HeightF = (float)(articleBarcodeHeight * MmToPixel);
                //articlesLocationLabelReport.xrRefBarCode.WidthF = (float)(articleBarcodeWeight * MmToPixel);
                //articlesLocationLabelReport.xrRefBarCode.LocationF = new PointF(0f,(float)(articleLocationHeight1 * MmToPixel)+ (float)(articleReferenceHeight * MmToPixel));               // (float)(articleReferenceHeight * MmToPixel)

                articlesLocationLabelReport.Detail.HeightF = (float)mainPanelheight + 26;

                articlesLocationLabelReport.xrlblLocation1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrlblLocation1_BeforePrint);
                articlesLocationLabelReport.xrlblLocation2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrlblLocation2_BeforePrint);

                if (IsIncludeArticlePhoto)
                {
                    List<Article> FinalArticlesList = WarehouseService.GetSelectedArticleImageInBytes(ArticlesList.ToList());
                    articlesLocationLabelReport.DataSource = FinalArticlesList;
                }
                else
                {
                    articlesLocationLabelReport.DataSource = ArticlesList;
                }

                //foreach (var item in FinalArticlesList)
                //{
                //    if (item.ArticleWarehouseLocation.WarehouseLocation.HtmlColor != null)
                //    {
                //        articlesLocationLabelReport.xrlblLocation1.BackColor = ColorTranslator.FromHtml(item.ArticleWarehouseLocation.WarehouseLocation.HtmlColor.ToString());
                //        articlesLocationLabelReport.xrlblLocation2.BackColor = ColorTranslator.FromHtml(item.ArticleWarehouseLocation.WarehouseLocation.HtmlColor.ToString());
                //    }
                //}

                //if (FinalArticlesList[0].ArticleWarehouseLocation.WarehouseLocation.HtmlColor!=null)

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = articlesLocationLabelReport;
                articlesLocationLabelReport.CreateDocument();
                window.Show();
                window.Activate();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintArticleLocationLabel...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// method for Style One 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void PrintArticleLocationLabelFirstStyle(int width, int height)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("PrintArticleLocationLabelFirstStyle....", category: Category.Info, priority: Priority.Low);
                //double convertMmToPixel = 3.7795275591;
                double MmToPixel = 3.969000400416382; //convert 1mm to pixel

                WarehoseLocationsLabelReportStyle articlesLocationLabelReport = new WarehoseLocationsLabelReportStyle();
                articlesLocationLabelReport.xrRefBarCode.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrArticleBarcode_BeforePrint);

                // convert mm to pixel
                double mainPanelheight = Convert.ToDouble(height) * MmToPixel;
                double mainPanelWidth = Convert.ToDouble(width) * MmToPixel;

                //main panel
                articlesLocationLabelReport.xrPanel.HeightF = (float)mainPanelheight;
                articlesLocationLabelReport.xrPanel.WidthF = (float)mainPanelWidth;
                articlesLocationLabelReport.xrPanel.LocationF = new PointF(10f, 10.42f);

                if (IsIncludeArticlePhoto == true)
                {
                    //Article Location Barcode
                    double articleLocationBarcodeHeight = ((float)height * 35) / 100;
                    double articleLocationBarcodeWidth = ((float)width * 23) / 100;
                    articlesLocationLabelReport.xrLocationBarCode.HeightF = (float)(articleLocationBarcodeHeight * MmToPixel);
                    articlesLocationLabelReport.xrLocationBarCode.WidthF = (float)(articleLocationBarcodeWidth * MmToPixel);

                    if (width == 80)
                    {
                        articlesLocationLabelReport.xrLocationBarCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 4, 4);
                    }

                    //Article Location Name1
                    double articleLocationHeight1 = ((float)height * 33) / 100;
                    double articleLocationWidth1 = ((float)width * 11.5) / 100;
                    articlesLocationLabelReport.xrlblLocation1.HeightF = (float)(articleLocationHeight1 * MmToPixel);
                    articlesLocationLabelReport.xrlblLocation1.WidthF = (float)(articleLocationWidth1 * MmToPixel);
                    articlesLocationLabelReport.xrlblLocation1.LocationF = new PointF(0f, (float)(articleLocationBarcodeHeight * MmToPixel));

                    //Article Location Name2
                    double articleLocationHeight2 = ((float)height * 33) / 100;
                    double articleLocationWidth2 = ((float)width * 11.5) / 100;
                    articlesLocationLabelReport.xrlblLocation2.HeightF = (float)(articleLocationHeight2 * MmToPixel);
                    articlesLocationLabelReport.xrlblLocation2.WidthF = (float)(articleLocationWidth2 * MmToPixel);
                    articlesLocationLabelReport.xrlblLocation2.LocationF = new PointF((float)(articleLocationWidth1 * MmToPixel), (float)(articleLocationBarcodeHeight * MmToPixel));

                    //Article Location Name3
                    double articleLocationHeight3 = ((float)height * 35) / 100;
                    double articleLocationWidth3 = ((float)width * 23) / 100;
                    articlesLocationLabelReport.xrlblLocation3.HeightF = (float)(articleLocationHeight3 * MmToPixel);
                    articlesLocationLabelReport.xrlblLocation3.WidthF = (float)(articleLocationWidth3 * MmToPixel);
                    articlesLocationLabelReport.xrlblLocation3.LocationF = new PointF(0f, ((float)(articleLocationBarcodeHeight * MmToPixel) + (float)(articleLocationHeight1 * MmToPixel)));

                    //Article Reference
                    double articleReferenceHeight = ((float)height * 50.3) / 100;
                    double articleReferenceWidth = ((float)width * 54.5) / 100;
                    articlesLocationLabelReport.xrRefrence.HeightF = (float)(articleReferenceHeight * MmToPixel);
                    articlesLocationLabelReport.xrRefrence.WidthF = (float)(articleReferenceWidth * MmToPixel);
                    articlesLocationLabelReport.xrRefrence.LocationF = new PointF((float)(articleLocationWidth3 * MmToPixel), 0.4f);               //(float)(articleReferenceWeight * MmToPixel)/4
                    articlesLocationLabelReport.xrRefrence.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 24, System.Drawing.FontStyle.Bold);

                    //Article Barcode
                    double articleBarcodeHeight = ((float)height * 52) / 100;       //50.3
                    double articleBarcodeWidth = ((float)width * 54.5) / 100;

                    articlesLocationLabelReport.xrRefBarCode.HeightF = (float)(articleBarcodeHeight * MmToPixel);
                    articlesLocationLabelReport.xrRefBarCode.WidthF = (float)(articleBarcodeWidth * MmToPixel);
                    articlesLocationLabelReport.xrRefBarCode.LocationF = new PointF((float)(articleLocationWidth3 * MmToPixel), (float)(articleReferenceHeight * MmToPixel));               //(float)(articleReferenceWeight * MmToPixel)/4

                    //Article Image
                    double articleImageHeight = height;
                    double articleImageWidth = ((float)width * 23) / 100;

                    articlesLocationLabelReport.xrImage.HeightF = (float)(articleImageHeight * MmToPixel);
                    articlesLocationLabelReport.xrImage.WidthF = (float)(articleImageWidth * MmToPixel);
                    articlesLocationLabelReport.xrImage.LocationF = new PointF(((float)(articleReferenceWidth * MmToPixel) + (float)(articleLocationWidth3 * MmToPixel)), 0.4f);
                }
                else
                {
                    //Article Location Barcode
                    double articleLocationBarcodeHeight = ((float)height * 35) / 100;
                    double articleLocationBarcodeWidth = ((float)width * 40) / 100;
                    articlesLocationLabelReport.xrLocationBarCode.HeightF = (float)(articleLocationBarcodeHeight * MmToPixel);
                    articlesLocationLabelReport.xrLocationBarCode.WidthF = (float)(articleLocationBarcodeWidth * MmToPixel);

                    if (width == 80)
                    {
                        articlesLocationLabelReport.xrLocationBarCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 4, 4);
                    }

                    //Article Location Name1
                    double articleLocationHeight1 = ((float)height * 33) / 100;
                    double articleLocationWidth1 = ((float)width * 20) / 100;
                    articlesLocationLabelReport.xrlblLocation1.HeightF = (float)(articleLocationHeight1 * MmToPixel);
                    articlesLocationLabelReport.xrlblLocation1.WidthF = (float)(articleLocationWidth1 * MmToPixel);
                    articlesLocationLabelReport.xrlblLocation1.LocationF = new PointF(0f, (float)(articleLocationBarcodeHeight * MmToPixel));

                    //Article Location Name2
                    double articleLocationHeight2 = ((float)height * 33) / 100;
                    double articleLocationWidth2 = ((float)width * 20) / 100;
                    articlesLocationLabelReport.xrlblLocation2.HeightF = (float)(articleLocationHeight2 * MmToPixel);
                    articlesLocationLabelReport.xrlblLocation2.WidthF = (float)(articleLocationWidth2 * MmToPixel);
                    articlesLocationLabelReport.xrlblLocation2.LocationF = new PointF((float)(articleLocationWidth1 * MmToPixel), (float)(articleLocationBarcodeHeight * MmToPixel));

                    //Article Location Name3
                    double articleLocationHeight3 = ((float)height * 35) / 100;
                    double articleLocationWidth3 = ((float)width * 40) / 100;
                    articlesLocationLabelReport.xrlblLocation3.HeightF = (float)(articleLocationHeight3 * MmToPixel);
                    articlesLocationLabelReport.xrlblLocation3.WidthF = (float)(articleLocationWidth3 * MmToPixel);
                    articlesLocationLabelReport.xrlblLocation3.LocationF = new PointF(0f, ((float)(articleLocationBarcodeHeight * MmToPixel) + (float)(articleLocationHeight1 * MmToPixel)));

                    //Article Image
                    articlesLocationLabelReport.xrImage.Visible = false;
                    //double articleImageWeight = ((float)weight * 23) / 100;

                    //Article Reference
                    double articleReferenceHeight = ((float)height * 50.3) / 100;
                    double articleReferenceWidth = ((float)width * 60) / 100;
                    articlesLocationLabelReport.xrRefrence.HeightF = (float)(articleReferenceHeight * MmToPixel);
                    articlesLocationLabelReport.xrRefrence.WidthF = (float)(articleReferenceWidth * MmToPixel); // + (float)(articleImageWeight * MmToPixel);
                    articlesLocationLabelReport.xrRefrence.LocationF = new PointF((float)(articleLocationWidth3 * MmToPixel), 0.4f);               //(float)(articleReferenceWeight * MmToPixel)/4
                    articlesLocationLabelReport.xrRefrence.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 24, System.Drawing.FontStyle.Bold);

                    //Article Barcode
                    double articleBarcodeHeight = ((float)height * 52) / 100;                       //50.3
                    double articleBarcodeWidth = ((float)width * 60) / 100;

                    articlesLocationLabelReport.xrRefBarCode.HeightF = (float)(articleBarcodeHeight * MmToPixel);
                    articlesLocationLabelReport.xrRefBarCode.WidthF = (float)(articleBarcodeWidth * MmToPixel); // + (float)(articleImageWeight * MmToPixel); 
                    articlesLocationLabelReport.xrRefBarCode.LocationF = new PointF((float)(articleLocationWidth3 * MmToPixel), (float)(articleReferenceHeight * MmToPixel));               //(float)(articleReferenceWeight * MmToPixel)/4                
                }

                articlesLocationLabelReport.Detail.HeightF = (float)mainPanelheight + 26;

                articlesLocationLabelReport.xrlblLocation1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrlblLocation1_BeforePrint);
                articlesLocationLabelReport.xrlblLocation2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrlblLocation2_BeforePrint);

                if (IsIncludeArticlePhoto)
                {
                    List<Article> FinalArticlesList = WarehouseService.GetSelectedArticleImageInBytes(ArticlesList.ToList());
                    articlesLocationLabelReport.DataSource = FinalArticlesList;
                }
                else
                {
                    articlesLocationLabelReport.DataSource = ArticlesList;
                }

                //foreach (var item in FinalArticlesList)
                //{
                //    if (item.ArticleWarehouseLocation.WarehouseLocation.HtmlColor != null)
                //    {
                //        articlesLocationLabelReport.xrlblLocation1.BackColor = ColorTranslator.FromHtml(item.ArticleWarehouseLocation.WarehouseLocation.HtmlColor.ToString());
                //        articlesLocationLabelReport.xrlblLocation2.BackColor = ColorTranslator.FromHtml(item.ArticleWarehouseLocation.WarehouseLocation.HtmlColor.ToString());
                //    }
                //}

                //if (FinalArticlesList[0].ArticleWarehouseLocation.WarehouseLocation.HtmlColor!=null)

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = articlesLocationLabelReport;
                articlesLocationLabelReport.CreateDocument();
                window.Show();
                window.Activate();
                GeosApplication.Instance.Logger.Log("Method PrintArticleLocationLabelFirstStyle....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintArticleLocationLabelFirstStyle...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void PrintArticleLocationLabelNoPic(int weight, int height)
        {
            try
            {
                //double convertMmToPixel = 3.7795275591;
                double MmToPixel = 3.969000400416382; //convert 1mm to pixel

                WarehoseLocationsLabelReportNoPic articlesLocationLabelReport = new WarehoseLocationsLabelReportNoPic();
                articlesLocationLabelReport.xrRefBarCode.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrArticleBarcode_BeforePrint);

                // convert mm to pixel
                double mainPanelheight = Convert.ToDouble(height) * MmToPixel;
                double mainPanelweight = Convert.ToDouble(weight) * MmToPixel;

                //main panel
                articlesLocationLabelReport.xrPanel.HeightF = (float)mainPanelheight;
                articlesLocationLabelReport.xrPanel.WidthF = (float)mainPanelweight;
                articlesLocationLabelReport.xrPanel.LocationF = new PointF(10f, 10.42f);
                //articlesLocationLabelReport.xrPanel.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Top;

                //Article Location Name1
                double articleLocationHeight1 = ((float)height * 26) / 100;
                double articleLocationWeight1 = ((float)weight * 8) / 100;           //13.5
                articlesLocationLabelReport.xrlblLocation1.HeightF = (float)(articleLocationHeight1 * MmToPixel);
                articlesLocationLabelReport.xrlblLocation1.WidthF = (float)(articleLocationWeight1 * MmToPixel);
                //articlesLocationLabelReport.xrlblLocation1.LocationF = new PointF(0f, (float)(articleLocationBarcodeHeight * MmToPixel));

                //Article Location Name2
                double articleLocationHeight2 = ((float)height * 26) / 100;
                double articleLocationWeight2 = ((float)weight * 8) / 100;               //13.5
                articlesLocationLabelReport.xrlblLocation2.HeightF = (float)(articleLocationHeight2 * MmToPixel);
                articlesLocationLabelReport.xrlblLocation2.WidthF = (float)(articleLocationWeight2 * MmToPixel);
                articlesLocationLabelReport.xrlblLocation2.LocationF = new PointF((float)(articleLocationWeight1 * MmToPixel), 0.4f);         //(float)(articleLocationBarcodeHeight * MmToPixel)

                //Article Location Name3
                double articleLocationHeight3 = ((float)height * 26) / 100;
                double articleLocationWeight3 = ((float)weight * 14) / 100;                     //27
                articlesLocationLabelReport.xrlblLocation3.HeightF = (float)(articleLocationHeight3 * MmToPixel);
                articlesLocationLabelReport.xrlblLocation3.WidthF = (float)(articleLocationWeight3 * MmToPixel);
                articlesLocationLabelReport.xrlblLocation3.LocationF = new PointF(((float)(articleLocationWeight1 * MmToPixel) + (float)(articleLocationWeight2 * MmToPixel)), 0.4f);

                //Article Location Barcode
                double articleLocationBarcodeHeight = ((float)height * 26) / 100;
                double articleLocationBarcodeWeight = ((float)weight * 60) / 100;             //27
                articlesLocationLabelReport.xrLocationBarCode.HeightF = (float)(articleLocationBarcodeHeight * MmToPixel);
                articlesLocationLabelReport.xrLocationBarCode.WidthF = (float)(articleLocationBarcodeWeight * MmToPixel);
                articlesLocationLabelReport.xrLocationBarCode.LocationF = new PointF(((float)(articleLocationWeight1 * MmToPixel) + (float)(articleLocationWeight2 * MmToPixel) + (float)(articleLocationWeight3 * MmToPixel)), 0.4f);

                if (weight == 80)
                {
                    //articlesLocationLabelReport.xrLocationBarCode.Alignment = new DevExpress.XtraPrinting.TextAlignment();
                    //articlesLocationLabelReport.xrLocationBarCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(10,10, 4, 4);
                    //articlesLocationLabelReport.xrLocationBarCode.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 4, 4);
                    //articlesLocationLabelReport.xrRefBarCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5);
                }

                //Article Reference
                double articleReferenceHeight = ((float)height * 32) / 100;
                double articleReferenceWeight = weight;// ((float)weight * 70) / 100;                   //51.5
                articlesLocationLabelReport.xrRefrence.HeightF = (float)(articleReferenceHeight * MmToPixel);
                articlesLocationLabelReport.xrRefrence.WidthF = (float)(articleReferenceWeight * MmToPixel);
                articlesLocationLabelReport.xrRefrence.LocationF = new PointF(0f, (float)(articleLocationHeight1 * MmToPixel));               //(float)(articleReferenceWeight * MmToPixel)/4
                articlesLocationLabelReport.xrRefrence.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 24, System.Drawing.FontStyle.Bold);

                //Article Barcode
                double articleBarcodeHeight = ((float)height * 36) / 100;
                double articleBarcodeWeight = weight;// ((float)weight * 70) / 100;                     //51.5

                articlesLocationLabelReport.xrRefBarCode.HeightF = (float)(articleBarcodeHeight * MmToPixel);
                articlesLocationLabelReport.xrRefBarCode.WidthF = (float)(articleBarcodeWeight * MmToPixel);
                articlesLocationLabelReport.xrRefBarCode.LocationF = new PointF(0f, (float)(articleLocationHeight1 * MmToPixel) + (float)(articleReferenceHeight * MmToPixel));               // (float)(articleReferenceHeight * MmToPixel)

                articlesLocationLabelReport.Detail.HeightF = (float)mainPanelheight + 26;

                articlesLocationLabelReport.xrlblLocation1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrlblLocation1_BeforePrint);
                articlesLocationLabelReport.xrlblLocation2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrlblLocation2_BeforePrint);

                List<Article> FinalArticlesList = WarehouseService.GetSelectedArticleImageInBytes(ArticlesList.ToList());
                articlesLocationLabelReport.DataSource = FinalArticlesList;

                //foreach (var item in FinalArticlesList)
                //{
                //    if (item.ArticleWarehouseLocation.WarehouseLocation.HtmlColor != null)
                //    {
                //        articlesLocationLabelReport.xrlblLocation1.BackColor = ColorTranslator.FromHtml(item.ArticleWarehouseLocation.WarehouseLocation.HtmlColor.ToString());
                //        articlesLocationLabelReport.xrlblLocation2.BackColor = ColorTranslator.FromHtml(item.ArticleWarehouseLocation.WarehouseLocation.HtmlColor.ToString());
                //    }
                //}

                //if (FinalArticlesList[0].ArticleWarehouseLocation.WarehouseLocation.HtmlColor!=null)

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = articlesLocationLabelReport;
                articlesLocationLabelReport.CreateDocument();
                window.Show();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintArticleLocationLabel...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void xrArticleBarcode_BeforePrint(object sender, PrintEventArgs e)
        {
            XRBarCode label = (XRBarCode)sender;
            label.AutoModule = false;

            BarCodeError berror = label.Validate();
            if (berror == BarCodeError.ControlBoundsTooSmall)
            {
                label.AutoModule = true;
            }
        }

        private void xrlblLocation1_BeforePrint(object sender, PrintEventArgs e)
        {
            XRLabel label = (XRLabel)sender;

            Article art = label.RootReport.GetCurrentRow() as Article;
            if (art != null && art.ArticleWarehouseLocation.WarehouseLocation.HtmlColor != null)
            {
                SetColors(label, art.ArticleWarehouseLocation.WarehouseLocation.HtmlColor);
            }
            else
            {
                label.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                label.ForeColor = ColorTranslator.FromHtml("#000000");
            }
        }

        private void xrlblLocation2_BeforePrint(object sender, PrintEventArgs e)
        {
            XRLabel label = (XRLabel)sender;

            Article art = label.RootReport.GetCurrentRow() as Article;

            if (art != null && art.ArticleWarehouseLocation.WarehouseLocation.HtmlColor != null)
            {
                SetColors(label, art.ArticleWarehouseLocation.WarehouseLocation.HtmlColor);
            }
            else
            {
                label.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                label.ForeColor = ColorTranslator.FromHtml("#000000");
            }
        }

        void SetColors(XRLabel label, string colorString)
        {
            //string colorString = art.ArticleWarehouseLocation.WarehouseLocation.HtmlColor;
            System.Drawing.Color _color = ColorTranslator.FromHtml(colorString);
            var brightness = 0;
            brightness = Brightness(_color);
            label.BackColor = _color;
            label.ForeColor = brightness > 150 ? System.Drawing.Color.Black : System.Drawing.Color.White;
        }


        private static int Brightness(System.Drawing.Color c)
        {
            return (int)Math.Sqrt(
               c.R * c.R * .241 +
               c.G * c.G * .691 +
               c.B * c.B * .068);
        }

        #endregion
    }
}
