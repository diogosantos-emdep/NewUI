using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class PendingStorageScanViewModel : INotifyPropertyChanged
    {
        #region Declaration

        private string pendingStorageBarcode;
        private string pendingStorageBarcodeError;
        private Visibility pendingStorageBarcodeErrorVisibility;
        private List<PendingStorageArticles> pendingStorageArticlesList;

        #endregion

        #region Properties

        public string PendingStorageBarcode
        {
            get { return pendingStorageBarcode; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    pendingStorageBarcode = value.Trim();
                else
                    pendingStorageBarcode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PendingStorageBarcode"));
            }
        }

        public string PendingStorageBarcodeError
        {
            get { return pendingStorageBarcodeError; }
            set
            {
                pendingStorageBarcodeError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PendingStorageBarcodeError"));
            }
        }

        public Visibility PendingStorageBarcodeErrorVisibility
        {
            get { return pendingStorageBarcodeErrorVisibility; }
            set
            {
                pendingStorageBarcodeErrorVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PendingStorageBarcodeErrorVisibility"));
            }
        }

        public List<PendingStorageArticles> PendingStorageArticlesList
        {
            get { return pendingStorageArticlesList; }
            set
            {
                pendingStorageArticlesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PendingStorageArticlesList"));
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

        #endregion // End Of Events

        #region Command

        public ICommand CancelButtonCommand { get; set; }
        public ICommand SkipButtonCommand { get; set; }
        public ICommand CommandScanBarcode { get; set; }

        #endregion

        #region Constructor

        public PendingStorageScanViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PendingStorageScanViewModel....", category: Category.Info, priority: Priority.Low);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                SkipButtonCommand = new DelegateCommand<object>(SkipButtonAction);
                CommandScanBarcode = new RelayCommand(new Action<object>(ScanBarcodeAction));
                GeosApplication.Instance.Logger.Log("Constructor PendingStorageScanViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor PendingStorageScanViewModel...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method to Close Window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void SkipButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SkipButtonAction()...", category: Category.Info, priority: Priority.Low);
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                if (PendingStorageArticlesList.Count > 0)
                {
                    LocateMaterialsView locateMaterialsView = new LocateMaterialsView();
                    LocateMaterialsViewModel locateMaterialsViewModel = new LocateMaterialsViewModel();
                    EventHandler handle = delegate { locateMaterialsView.Close(); };
                    locateMaterialsViewModel.RequestClose += handle;
                    locateMaterialsViewModel.IsScaned = false;
                    locateMaterialsViewModel.InIt(PendingStorageArticlesList);
                    locateMaterialsView.DataContext = locateMaterialsViewModel;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    RequestClose(null, null);
                    locateMaterialsView.ShowDialog();
                }

                GeosApplication.Instance.Logger.Log("Method SkipButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SkipButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ScanBarcodeAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction()...", category: Category.Info, priority: Priority.Low);

                if (((System.Windows.Input.KeyEventArgs)obj).Key == System.Windows.Input.Key.Enter)
                {
                    if (string.IsNullOrEmpty(PendingStorageBarcode))
                    {
                        SetErrorView();
                    }
                    else
                    {
                        LocateMaterialsView locateMaterialsView = new LocateMaterialsView();
                        LocateMaterialsViewModel locateMaterialsViewModel = new LocateMaterialsViewModel();
                        EventHandler handle = delegate { locateMaterialsView.Close(); };
                        locateMaterialsViewModel.RequestClose += handle;
                        locateMaterialsViewModel.IsScaned = true;
                        locateMaterialsViewModel.ScanedBarCode = PendingStorageBarcode;
                        locateMaterialsViewModel.InIt(PendingStorageArticlesList);

                        if (!locateMaterialsViewModel.IsWrongBarCode)
                        {
                            SetErrorView();
                        }
                        else
                        {
                            locateMaterialsView.DataContext = locateMaterialsViewModel;
                            RequestClose(null, null);
                            locateMaterialsView.ShowDialog();
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ScanBarcodeAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ScanBarcodeAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init(List<PendingStorageArticles> tempPendingStorageArticlesList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init...", category: Category.Info, priority: Priority.Low);
                PendingStorageBarcodeErrorVisibility = Visibility.Hidden;
                PendingStorageArticlesList = tempPendingStorageArticlesList;
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to set Error View
        /// </summary>
        private void SetErrorView()
        {
            PendingStorageBarcodeErrorVisibility = Visibility.Visible;
            PendingStorageBarcodeError = string.Format(" Wrong Location {0}", PendingStorageBarcode);
            PendingStorageBarcode = string.Empty;
        }

        #endregion
    }
}
