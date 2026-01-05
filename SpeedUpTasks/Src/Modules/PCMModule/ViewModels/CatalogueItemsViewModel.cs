using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Modules.PCM.Views;
using Emdep.Geos.UI.Common;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Prism.Logging;
using System.Windows.Input;
using Emdep.Geos.UI.Commands;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PCM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Xpf.Grid;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class CatalogueItemsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Declarations

        private ObservableCollection<CatalogueItem> catalogueItemsMenulist;
        private CatalogueItem selectedGridRow;
        private CatalogueItem selectedCatalogueItem;
        private bool isDeleted;
       // private string Code;

        #endregion

        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region ICommands

        public ICommand CatalogueItemDoubleClickCommand { get; set; }
        public ICommand AddCatalogueItemButtonCommand { get; set; }
        public ICommand RefreshCatelogueViewCommand { get; set; }
        public ICommand DeleteCatalogueItemCommand { get; set; }
        #endregion

        #region Properties

        public ObservableCollection<CatalogueItem> CatalogueItemsMenulist
        {
            get { return catalogueItemsMenulist; }
            set
            {
                catalogueItemsMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CatalogueItemsMenulist"));
            }
        }

        public CatalogueItem SelectedGridRow
        {
            get { return selectedGridRow; }
            set
            {
                selectedGridRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGridRow"));
            }
        }

        public CatalogueItem SelectedCatalogueItem
        {
            get
            {
                return selectedCatalogueItem;
            }

            set
            {
                selectedCatalogueItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCatalogueItem"));
            }
        }

        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }

            set
            {
                isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
            }
        }

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
        #endregion

        #region Constructor

        public CatalogueItemsViewModel()
        {
            CatalogueItemDoubleClickCommand = new RelayCommand(new Action<object>(EditCatalogueItem));
            AddCatalogueItemButtonCommand = new RelayCommand(new Action<object>(AddCatalogueItem));
            RefreshCatelogueViewCommand = new RelayCommand(new Action<object>(RefreshCatelogueView));
            DeleteCatalogueItemCommand = new RelayCommand(new Action<object>(DeleteCatalogueItem));
            AddCatalogueItemsMenu();
        }

        #endregion

        #region Methods

        protected override void OnParameterChanged(object parameter)
        {
            try
            {
                CatalogueItem previousItem = (CatalogueItem)parameter;
                AddCatalogueItemsMenu();
                CatalogueItem updatedCatalogueItem = CatalogueItemsMenulist.FirstOrDefault(x => x.IdCatalogueItem == previousItem.IdCatalogueItem);

                updatedCatalogueItem.Description = previousItem.Description;
                updatedCatalogueItem.Code = previousItem.Code;
                updatedCatalogueItem.IdTemplate = previousItem.IdTemplate;
                updatedCatalogueItem.Name = previousItem.Name;
                updatedCatalogueItem.LastUpdate = previousItem.LastUpdate;
                updatedCatalogueItem.IdStatus = previousItem.IdStatus;

                updatedCatalogueItem.IdCatalogueItem = previousItem.IdCatalogueItem;
                updatedCatalogueItem.Description = previousItem.Description;
                updatedCatalogueItem.Description_es = previousItem.Description_es;
                updatedCatalogueItem.Description_fr = previousItem.Description_fr;
                updatedCatalogueItem.Description_pt = previousItem.Description_pt;
                updatedCatalogueItem.Description_ro = previousItem.Description_ro;
                updatedCatalogueItem.Description_ru = previousItem.Description_ru;
                updatedCatalogueItem.Description_zh = previousItem.Description_zh;
                updatedCatalogueItem.IdCPType = previousItem.ProductType.IdCPType;

                updatedCatalogueItem.Name = previousItem.Name;
                updatedCatalogueItem.Name_es = previousItem.Name_es;
                updatedCatalogueItem.Name_fr = previousItem.Name_fr;
                updatedCatalogueItem.Name_pt = previousItem.Name_pt;
                updatedCatalogueItem.Name_ro = previousItem.Name_ro;
                updatedCatalogueItem.Name_ru = previousItem.Name_ru;
                updatedCatalogueItem.Name_zh = previousItem.Name_zh;

                SelectedCatalogueItem = updatedCatalogueItem;

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnParameterChanged() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            base.OnParameterChanged(parameter);
        }

        private void EditCatalogueItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditItemList()...", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;

                CatalogueItem catalogueItem = null;

                if (obj is TableView)
                {
                    TableView detailView = (TableView)obj;
                    catalogueItem = (CatalogueItem)detailView.DataControl.CurrentItem;
                }

                if (catalogueItem != null)
                {
                    EditCatalogueItemViewModel editCatalogueItemViewModel = new EditCatalogueItemViewModel();


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
                  //  editCatalogueItemViewModel.Init(catalogueItem);
                    Service.Navigate("Emdep.Geos.Modules.PCM.Views.EditCatalogueItemView", editCatalogueItemViewModel, catalogueItem, this, true);
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditItemList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditItemList()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCatalogueItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCatalogueItem().", category: Category.Info, priority: Priority.Low);

                Service.Navigate("Emdep.Geos.Modules.PCM.Views.AddCatalogueItemView", new AddCatalogueItemViewModel(), null, this, true);

                GeosApplication.Instance.Logger.Log("Method AddCatalogueItem(). executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddItemList()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCatalogueItemsMenu()
        {
            try
            {
                CatalogueItemsMenulist = new ObservableCollection<CatalogueItem>(PCMService.GetAllCatalogueItems()); // new ObservableCollection<CatalogueItem>(PCMService.GetAllCatalogueItems());
                if (CatalogueItemsMenulist.Count > 0)
                    SelectedCatalogueItem = CatalogueItemsMenulist.FirstOrDefault();
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCatalogueItemsMenuInAddCatalogue() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCatalogueItemsMenuInAddCatalogue() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCatalogueItemsMenuInAddCatalogue()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshCatelogueView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshCatelogueView()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

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

                CatalogueItemsMenulist = new ObservableCollection<CatalogueItem>(PCMService.GetAllCatalogueItems());
                SelectedCatalogueItem = CatalogueItemsMenulist.First();
                detailView.SearchString = null;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshCatelogueView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefreshCatelogueView() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefreshCatelogueView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshCatelogueView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteCatalogueItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteCatalogueItem()...", category: Category.Info, priority: Priority.Low);
                string msg = SelectedCatalogueItem.Code == null ? "" : "[" + SelectedCatalogueItem.Code.ToString() + "]";
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteCatelogueItemMessage"].ToString(),msg), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsDeleted = PCMService.IsDeletedCatalogueItem(SelectedCatalogueItem.IdCatalogueItem);
                    if (IsDeleted)
                    {
                        CatalogueItemsMenulist.Remove(SelectedCatalogueItem);
                        CatalogueItemsMenulist = new ObservableCollection<CatalogueItem>(CatalogueItemsMenulist);
                        SelectedCatalogueItem = CatalogueItemsMenulist.FirstOrDefault();
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CatelogueItemDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DeleteCatalogueItem()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteCatalogueItem() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteCatalogueItem() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteCatalogueItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion


    }
}
