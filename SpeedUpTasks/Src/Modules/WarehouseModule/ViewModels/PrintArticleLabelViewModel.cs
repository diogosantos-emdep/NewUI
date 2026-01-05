using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
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
    public class PrintArticleLabelViewModel
    {

        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Declaration

        private ObservableCollection<LookupValue> labelSizeList;
        private LookupValue selectedLabelSize;

        private ObservableCollection<Article> articlesList;
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
                OnPropertyChanged(new PropertyChangedEventArgs("ArticlesList"));
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

        public PrintArticleLabelViewModel()
        {
            CloseWindowCommand = new DelegateCommand<object>(CloseWindowAction);
            AcceptButtonCommand = new DelegateCommand<object>(PrintArticleLabelCommandAction);
        }

        #endregion

        #region METHOD
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

                ArticlesList = articlesList;

                GeosApplication.Instance.Logger.Log("Method Init....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                LabelSizeList = new ObservableCollection<LookupValue>(CrmService.GetLookupValues(47).ToList());
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
        private void PrintArticleLabelCommandAction(object obj)
        {
            
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintArticleLabelCommandAction()...", category: Category.Info, priority: Priority.Low);

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

           
                string [] splitLabelSize = SelectedLabelSize.Value.ToString().Split('x');

                PrintArticleLabel(Convert.ToInt32(splitLabelSize[0]), Convert.ToInt32(splitLabelSize[1]));

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                     GeosApplication.Instance.Logger.Log("Method PrintArticleLabelCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
              
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintArticleLabelCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }


        }
        /// <summary>
        /// this method use for print Article label
        /// [000][skale][6-11-2019][GEOS2-70] Add new option in warehouse to print Reference labels
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="height"></param>
        public void PrintArticleLabel(int weight, int height)
        {
            try
            {
                //double convertMmToPixel = 3.7795275591;
                double MmToPixel = 3.969000400416382; //convert 1mm to pixel

                ArticlesLabelReport articlesLabelReport = new ArticlesLabelReport();
                articlesLabelReport.xrArticleBarcode.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrArticleBarcode_BeforePrint);
                // convert mm to pixel
                double mainPanelheight = Convert.ToDouble(height) * MmToPixel;
                double mainPanelweight = Convert.ToDouble(weight) * MmToPixel;

                //main panel
                articlesLabelReport.xrMainPanel.HeightF = (float)mainPanelheight;
                articlesLabelReport.xrMainPanel.WidthF = (float)mainPanelweight;
                articlesLabelReport.xrMainPanel.LocationF = new PointF(10f,13.42f);
                //Article Reference
                double articleReferenceHeight = ((float)height * 50)/100;
                double articleReferenceWeight = ((float)weight * 80)/100;
                articlesLabelReport.xrlblArticleReference.HeightF = (float)(articleReferenceHeight * MmToPixel);
                articlesLabelReport.xrlblArticleReference.WidthF = (float)(articleReferenceWeight * MmToPixel);
                articlesLabelReport.xrlblArticleReference.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 65, System.Drawing.FontStyle.Bold);
                //Article Barcode
                double articleBarcodeHeight = ((float)height * 50)/100;
                double articleBarcodeWeight = ((float)weight * 80)/100;

                articlesLabelReport.xrArticleBarcode.HeightF = (float)(articleBarcodeHeight * MmToPixel);
                articlesLabelReport.xrArticleBarcode.WidthF = (float)(articleBarcodeWeight * MmToPixel);
                articlesLabelReport.xrArticleBarcode.LocationF = new PointF(0f, (float)(articleReferenceHeight * MmToPixel));
                //Article Image
                double articleImageHeight = height;
                double articleImageWeight = ((float)weight * 20)/100;

                articlesLabelReport.xrArticleImage.HeightF = (float)(articleImageHeight * MmToPixel);
                articlesLabelReport.xrArticleImage.WidthF = (float)(articleImageWeight * MmToPixel);
                articlesLabelReport.xrArticleImage.LocationF = new PointF((float)(articleReferenceWeight * MmToPixel),0.4f);

                articlesLabelReport.Detail.HeightF = (float)mainPanelheight + 26;

                List<Article> FinalArticlesList = WarehouseService.GetSelectedArticleImageInBytes(ArticlesList.ToList());
                articlesLabelReport.DataSource = FinalArticlesList;

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = articlesLabelReport;
                articlesLabelReport.CreateDocument();
                window.Show();
               
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintArticleLabel...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        #endregion

    }
}
