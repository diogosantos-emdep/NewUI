using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common;
using DevExpress.Xpf.Core;
using DevExpress.Mvvm.UI;
using System.Windows;
using System.Windows.Media;
using Prism.Logging;
using System.Windows.Input;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using System.Drawing;
using System.Text.RegularExpressions;
using Emdep.Geos.Modules.Warehouse.Reports;
using DevExpress.Xpf.Printing;
using System.Globalization;
using Emdep.Geos.Modules.Warehouse.Views;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class WarehouseLocationPrintViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        List<WarehouseLocation> warehouseLocationList;
        List<ImageDirection> imageDirectionList;
        private List<string> labelSizeList;
        private Int16 selectedIndexLabelSize;

        #endregion // Declaration

        #region Properties

        public Int16 SelectedIndexLabelSize
        {
            get { return selectedIndexLabelSize; }
            set
            {
                selectedIndexLabelSize = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexLabelSize"));
            }
        }

        public List<string> LabelSizeList
        {
            get { return labelSizeList; }
            set
            {
                labelSizeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LabelSizeList"));
            }
        }

        public List<WarehouseLocation> WarehouseLocationList
        {
            get { return warehouseLocationList; }
            set
            {
                warehouseLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseLocationList"));
            }
        }

        public List<ImageDirection> ImageDirectionList
        {
            get { return imageDirectionList; }
            set
            {
                imageDirectionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageDirectionList"));
            }
        }

        #endregion // Properties

        #region Commands

        public ICommand CloseWindowCommand { get; set; }
        public ICommand PrintLocationCommand { get; set; }

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

        public WarehouseLocationPrintViewModel()
        {
            CloseWindowCommand = new DelegateCommand<object>(CloseWindowAction);
            PrintLocationCommand = new DelegateCommand<object>(PrintLocationAction);
        }

        #endregion // Constructor

        #region Method

        /// <summary>
        ///  Method for fill all data on form initialization .
        /// </summary>
        public void InIt()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InIt....", category: Category.Info, priority: Priority.Low);

                FillWarehouseLocationList();
                ImageDiractionList();
                FillLabelSize();

                GeosApplication.Instance.Logger.Log("Method InIt....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InIt...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for print location report.
        /// [001][avpawar][8-7-2020][GEOS2-2448][Small location labels with 3 digits not ok displayed]
        /// </summary>
        /// <param name="obj"></param>
        private void PrintLocationAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PrintLocationAction....", category: Category.Info, priority: Priority.Low);

            try
            {
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

                List<WarehouseLocation> SelectedWarehouseLocation = WarehouseLocationList.Where(wh => wh.IsChecked == true).Select(w => w).Distinct().ToList();

                // code for split location in small part for print.
                foreach (WarehouseLocation item in SelectedWarehouseLocation)
                {
                    char[] delimiters = new char[] { '-', '.' };
                    string[] words = item.FullName.Split(delimiters);

                    if (words.Length > 0)
                    {
                        int cnt = 0;

                        item.LocationParent = words[0];
                        cnt++;

                        if (words.Length > cnt)
                        {
                            item.Locationchild1 = words[1];
                            cnt++;
                        }

                        if (words.Length > cnt)
                        {
                            item.Locationchild2 = words[2];
                        }
                    }
                }

                WarehoseLocationsReport warehoseLocationsReport = new WarehoseLocationsReport();
                warehoseLocationsReport.DataSource = SelectedWarehouseLocation;

                if (SelectedWarehouseLocation != null && SelectedWarehouseLocation.Count != 0)
                {
                    warehoseLocationsReport.xrlblLocation1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 65, System.Drawing.FontStyle.Bold);
                    warehoseLocationsReport.xrlblLocation2.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 65, System.Drawing.FontStyle.Bold);
                    warehoseLocationsReport.xrlblLocation3.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 70, System.Drawing.FontStyle.Bold);

                    if (SelectedIndexLabelSize == 1)
                    {
                        warehoseLocationsReport.xrlblLocation1.WidthF = 130;
                        warehoseLocationsReport.xrlblLocation1.HeightF = 58;
                        warehoseLocationsReport.xrlblLocation1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 40, System.Drawing.FontStyle.Bold);

                        warehoseLocationsReport.xrlblLocation2.LocationF = new PointF(130F, 0F);
                        warehoseLocationsReport.xrlblLocation2.WidthF = 130;
                        warehoseLocationsReport.xrlblLocation2.HeightF = 58;
                        warehoseLocationsReport.xrlblLocation2.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 40, System.Drawing.FontStyle.Bold);

                        //[001] change widthF 118 to 158
                        warehoseLocationsReport.xrlblLocation3.WidthF = 158; 
                        warehoseLocationsReport.xrlblLocation3.HeightF = 143;
                        warehoseLocationsReport.xrlblLocation3.LocationF = new PointF(260F, 0F);
                        warehoseLocationsReport.xrlblLocation3.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 30, System.Drawing.FontStyle.Bold);

                        warehoseLocationsReport.xrLocationBarCode.LocationF = new PointF(1.21F, 57F);
                        warehoseLocationsReport.xrLocationBarCode.WidthF = 260; // 285;
                        warehoseLocationsReport.xrLocationBarCode.HeightF = 86;
                        //warehoseLocationsReport.xrLocationBarCode.AutoModule = true;

                        //[001] change LocationF (378F, 0F) to (418F, 0F)
                        //[001] change widthF 107 to 67
                        warehoseLocationsReport.xrPictureBoxDirection.LocationF = new PointF(418F, 0F); 
                        warehoseLocationsReport.xrPictureBoxDirection.WidthF = 67;
                        warehoseLocationsReport.xrPictureBoxDirection.HeightF = 143;
                        //warehoseLocationsReport.xrPictureBoxDirection.

                        warehoseLocationsReport.xrPanel.WidthF = 485;
                        warehoseLocationsReport.xrPanel.HeightF = 143;
                    }

                    DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                    window.PreviewControl.DocumentSource = warehoseLocationsReport;
                    warehoseLocationsReport.CreateDocument();
                    window.Show();
                }
                else
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WarehouseLocationPrintFailedPrint").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintLocationAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintLocationAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill All location of warehouse.
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// </summary>
        private void FillWarehouseLocationList()
        {
            GeosApplication.Instance.Logger.Log("Method FillWarehouseLocationList....", category: Category.Info, priority: Priority.Low);

            try
            {
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
                // [001] Changed Service method
                WarehouseLocationList = WarehouseService.GetAllWarehouseLocationById_V2034(WarehouseCommon.Instance.Selectedwarehouse);

                GeosApplication.Instance.Logger.Log("Method FillWarehouseLocationList....executed successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseLocationList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseLocationList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillWarehouseLocationList...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for create list with image direction.
        /// </summary>
        private void ImageDiractionList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ImageDiractionList()...", category: Category.Info, priority: Priority.Low);

                ImageDirectionList = new List<ImageDirection>() {   new ImageDirection { IdImage=0, Name = "---", BitmapDirection = null },
                                                                    new ImageDirection { IdImage=3, Name = "Up", BitmapDirection = new Bitmap(Emdep.Geos.Modules.Warehouse.Properties.Resources.bUpArrow) },
                                                                    new ImageDirection { IdImage=4, Name = "Down", BitmapDirection = new Bitmap(Emdep.Geos.Modules.Warehouse.Properties.Resources.bDownArrow) },
                                                                };

                foreach (var item in WarehouseLocationList)
                {
                    item.ImageDirection = ImageDirectionList.First();
                }

                GeosApplication.Instance.Logger.Log("Method ImageDiractionList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ImageDiractionList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to fill Label size Combo
        /// </summary>
        public void FillLabelSize()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLabelSize()...", category: Category.Info, priority: Priority.Low);
                LabelSizeList = new List<string>();
                LabelSizeList.Add("200x55");
                LabelSizeList.Add("122x37");
                SelectedIndexLabelSize = 0;
                GeosApplication.Instance.Logger.Log("Method FillLabelSize()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillLabelSize()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Method CloseWindowAction()...", category: Category.Info, priority: Priority.Low);

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindowAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseWindowAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion // Method
    }
}
