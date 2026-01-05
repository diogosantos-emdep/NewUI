using DevExpress.Export.Xl;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Modules.PCM.Views;
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.PCM.ViewModels
{

    //[PRAMOD.MISAL][GEOS2-4442][29-08-2023]
    public class FreePluginsViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {

        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
      // IPCMService PCMService = new PCMServiceController("localhost:6699");


        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler RequestClose;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region Declaration

        private ObservableCollection<FreePlugins> freePluginslist;
        private bool isSituationEnabled;
        private bool isBusy;

        private FreePlugins deletedFreePluginList;//[Sudhir.Jangra][GEOS2-4444][20/09/2023]

        private FreePlugins selectedFreePlugins;

        #endregion


        #region Properties

        public ObservableCollection<FreePlugins> FreePluginslist
        {
            get { return freePluginslist; }
            set
            {
                freePluginslist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FreePluginslist"));
            }
        }

        public bool IsSituationEnabled
        {
            get { return isSituationEnabled; }
            set
            {
                isSituationEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSituationEnabled"));
            }
        }


        public FreePlugins SelectedFreePlugins
        {
            get { return selectedFreePlugins; }
            set
            {
                selectedFreePlugins = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFreePlugins"));
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


        public FreePlugins DeleteFreePluginList
        {
            get { return deletedFreePluginList; }
            set
            {
                deletedFreePluginList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeleteFreePluginList"));
            }
        }


        #endregion


        #region ICommands
        public ICommand AddFreePluginsListCommand { get; set; }

        public ICommand RefreshPluginsListCommand { get; set; }

        public ICommand PrintPluginsListCommand { get; set; }

        public ICommand ExportPluginsListCommand { get; set; }

        public ICommand DeleteFreepluginsCommand { get; set; }

        public ICommand EditFreepluginsCommand { get; set; }





        #endregion


        #region Constructor
        public FreePluginsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FreePluginsViewModel() ...", category: Category.Info, priority: Priority.Low);
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
                Init();
                PrintPluginsListCommand = new RelayCommand(new Action<object>(PrintFreePluinsAction));
                ExportPluginsListCommand = new RelayCommand(new Action<object>(ExportFreePluginsAction));
                RefreshPluginsListCommand = new RelayCommand(new Action<object>(RefreshFreePluginsction));
                AddFreePluginsListCommand = new DelegateCommand<object>(AddNewFreePlugins);
                DeleteFreepluginsCommand = new DelegateCommand<object>(DeleteFreepluginsCommandAction);
                // EditFreepluginsCommand = new DelegateCommand<object>(EditFreepluginsCommandAction);
                if (GeosApplication.Instance.IsPCMEditFreePluginsPermission)
                {
                    IsSituationEnabled = true;
                    EditFreepluginsCommand = new DelegateCommand<object>(EditFreepluginsCommandAction);
                }
                else
                {
                    IsSituationEnabled = false;

                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor  WarehouseBulkPickingViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  WarehouseQuotaViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        #region Methods

        public void Init()
        {
            FillFreePlugins();
        }


        public void FillFreePlugins()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                FreePluginslist = new ObservableCollection<FreePlugins>(PCMService.GetAllFreePlugins_byPermission());


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillFreePlugins() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillFreePlugins() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillFreePlugins()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void PrintFreePluinsAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintFreePluinsAction()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["FreePluginsPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["FreePluginsPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintFreePluinsAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintFreePluinsAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void ExportFreePluginsAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportFreePluginsAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Free Plugins";
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
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportFreePluginsAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportFreePluginsAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }


        private void RefreshFreePluginsction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshFreePluginsction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                FillFreePlugins();
                detailView.SearchString = null;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshFreePluginsction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshFreePluginsction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshFreePluginsction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshFreePluginsction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddNewFreePlugins(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewFreePlugins()...", category: Category.Info, priority: Priority.Low);
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

                AddFreePluginsView addFreePluginsView = new AddFreePluginsView();
                AddFreePluginsViewModel addFreePluginsViewModel = new AddFreePluginsViewModel();
                EventHandler handle = delegate { addFreePluginsView.Close(); };
                addFreePluginsViewModel.RequestClose += handle;

                addFreePluginsView.DataContext = addFreePluginsViewModel;
                addFreePluginsViewModel.FreePluginGridList = FreePluginslist;
                addFreePluginsViewModel.Init();
                addFreePluginsViewModel.IsNew = true;
                addFreePluginsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddFreePluginsTitle").ToString();
                addFreePluginsView.ShowDialog();

                FreePlugins tempfreeplugins = new FreePlugins();

                if (addFreePluginsViewModel.IsSave)
                {
                    FillFreePlugins();
                    try
                    {
                        SelectedFreePlugins = FreePluginslist.FirstOrDefault(x => x.IdPlugin == addFreePluginsViewModel.NewfreePlugins.IdPlugin
                        && x.IdCustomer == addFreePluginsViewModel.NewfreePlugins.IdCustomer
                        && x.IdRegion == addFreePluginsViewModel.NewfreePlugins.IdRegion
                        &&  x.IdCountry == addFreePluginsViewModel.NewfreePlugins.IdCountry 
                        && x.IdSite == addFreePluginsViewModel.NewfreePlugins.IdSite);
                        if (SelectedFreePlugins == null)
                        {
                            SelectedFreePlugins = addFreePluginsViewModel.NewfreePlugins;
                        }

                        try
                        {
                            if (SelectedFreePlugins == null)
                            {
                                SelectedFreePlugins = addFreePluginsViewModel.NewfreePlugins;
                            }
                            ObservableCollection<FreePlugins> tempFreePluginslist = new ObservableCollection<FreePlugins>();
                            tempFreePluginslist = new ObservableCollection<FreePlugins>(FreePluginslist);
                            ObservableCollection<FreePlugins> FinalfreePlugins = new ObservableCollection<FreePlugins>();
                            FinalfreePlugins = new ObservableCollection<FreePlugins>(addFreePluginsViewModel.FinalfreePlugins.Where(s => s.TransactionOperation.Equals(Emdep.Geos.Data.Common.ModelBase.TransactionOperations.Add)));
                            ObservableCollection<FreePlugins> SelectedFreePluginList = new ObservableCollection<FreePlugins>();
                            foreach (FreePlugins FreePluginsitem in FinalfreePlugins)
                            {
                                FreePlugins freePlugins = tempFreePluginslist.Where(w => w.IdPlugin == FreePluginsitem.IdPlugin && w.IdCustomer == FreePluginsitem.IdCustomer
                                && w.IdRegion == FreePluginsitem.IdRegion && w.IdCountry == FreePluginsitem.IdCountry && w.IdSite == FreePluginsitem.IdSite).FirstOrDefault();
                                tempFreePluginslist.Remove(freePlugins);
                                SelectedFreePluginList.Add(freePlugins);
                            }
                            FreePluginslist = new ObservableCollection<FreePlugins>(SelectedFreePluginList);
                            foreach (FreePlugins FreePluginsitem in tempFreePluginslist)
                            {
                                FreePluginslist.Add(FreePluginsitem);
                            }
                        }
                        catch (Exception ex) { FillFreePlugins(); }
                    }
                    catch (Exception)
                    {
                    }
                   
                }


                #region old code



                //if (addFreePluginsViewModel.IsSave)
                //{
                //    // tempfreeplugins.Region = string.Join("/n", addFreePluginsViewModel.PlantList.Select(s => s.RegionName));
                //    tempfreeplugins.Group = addFreePluginsViewModel.SelectedGroup.GroupName;
                //    tempfreeplugins.Name = addFreePluginsViewModel.NewFreePluginsList.Name;
                //    if (addFreePluginsViewModel.SelectedRegion == null)
                //    {
                //        tempfreeplugins.Region = "All";
                //    }
                //    else
                //    {
                //        tempfreeplugins.Region = string.Join("/n", addFreePluginsViewModel.SelectedRegion.Select(s => ((Region)s).RegionName));
                //    }

                //    if (addFreePluginsViewModel.SelectedPlant == null)
                //    {
                //        tempfreeplugins.Plant = "All";
                //    }
                //    else
                //    {
                //        tempfreeplugins.Plant = string.Join("/n", addFreePluginsViewModel.SelectedPlant.Select(s => ((Site)s).Name));

                //    }

                //    if (addFreePluginsViewModel.SelectedCountry == null)
                //    {
                //        tempfreeplugins.Country = "All";
                //    }
                //    else
                //    {
                //        tempfreeplugins.Country = string.Join("/n", addFreePluginsViewModel.SelectedCountry.Select(s => ((Country)s).Name));
                //    }
                //    // tempfreeplugins.Plant = string.Join("/n", addFreePluginsViewModel.SelectedPlant.Select(s => ((Site)s).Name));
                //    // tempfreeplugins.Country = string.Join("/n", addFreePluginsViewModel.SelectedCountry.Select(s => ((Country)s).Name));


                //    FreePluginslist.Add(tempfreeplugins);

                //}

                #endregion old code
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddNewFreePlugins()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewFreePlugins()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //chitra[cgirigosavi][GEOS2-4443][08/09/2023]
        private void EditFreepluginsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditBulkPickingCommandAction()...", category: Category.Info, priority: Priority.Low);


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


                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                FreePlugins selectedRow = (FreePlugins)detailView.FocusedRow;
                long IdPlugin = selectedRow.IdPlugin;

                AddFreePluginsView addFreePluginsView = new AddFreePluginsView();
                AddFreePluginsViewModel addFreePluginsViewModel = new AddFreePluginsViewModel();
                EventHandler handle = delegate { addFreePluginsView.Close(); };
                addFreePluginsViewModel.RequestClose += handle;
                addFreePluginsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditFreePluginsTitle").ToString();
                addFreePluginsView.DataContext = addFreePluginsViewModel;
                addFreePluginsViewModel.FreePluginGridList = FreePluginslist;
                addFreePluginsViewModel.EditINIT(selectedRow);
                addFreePluginsViewModel.IsNew = false;
                addFreePluginsView.ShowDialogWindow();

                if (addFreePluginsViewModel.IsSave == true)
                {
                    FillFreePlugins();
                    SelectedFreePlugins = FreePluginslist.FirstOrDefault(x => x.IdPlugin == addFreePluginsViewModel.NewfreePlugins.IdPlugin
                    && x.IdCustomer == addFreePluginsViewModel.NewfreePlugins.IdCustomer
                    && x.IdRegion == addFreePluginsViewModel.NewfreePlugins.IdRegion
                    &&    x.IdCountry == addFreePluginsViewModel.NewfreePlugins.IdCountry
                  //  && x.IdSite == addFreePluginsViewModel.NewfreePlugins.IdSite
                 );
                    try
                    {
                        if (SelectedFreePlugins == null)
                        {
                            SelectedFreePlugins = addFreePluginsViewModel.NewfreePlugins;
                        }
                        ObservableCollection<FreePlugins> tempFreePluginslist = new ObservableCollection<FreePlugins>();
                        tempFreePluginslist = new ObservableCollection<FreePlugins>(FreePluginslist);
                        ObservableCollection<FreePlugins> FinalfreePlugins = new ObservableCollection<FreePlugins>();
                        FinalfreePlugins = new ObservableCollection<FreePlugins>(addFreePluginsViewModel.FinalfreePlugins.Where(s => s.TransactionOperation.Equals(Emdep.Geos.Data.Common.ModelBase.TransactionOperations.Add)));
                        ObservableCollection<FreePlugins> SelectedFreePluginList = new ObservableCollection<FreePlugins>();
                        foreach (FreePlugins FreePluginsitem in FinalfreePlugins)
                        {
                            FreePlugins freePlugins = tempFreePluginslist.Where(w => w.IdPlugin == FreePluginsitem.IdPlugin && w.IdCustomer == FreePluginsitem.IdCustomer
                            && w.IdRegion == FreePluginsitem.IdRegion && w.IdCountry == FreePluginsitem.IdCountry && w.IdSite == FreePluginsitem.IdSite).FirstOrDefault();
                            tempFreePluginslist.Remove(freePlugins);
                            SelectedFreePluginList.Add(freePlugins);
                        }
                        FreePluginslist = new ObservableCollection<FreePlugins>(SelectedFreePluginList);
                        foreach (FreePlugins FreePluginsitem in tempFreePluginslist)
                        {
                            FreePluginslist.Add(FreePluginsitem);
                        }
                    }
                    catch (Exception ex) { FillFreePlugins(); }

                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditBulkPickingCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditBulkPickingCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //[Sudhir.Jangra][GEOS2-4444][20/09/2023]
        private void DeleteFreepluginsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteFreepluginsCommandAction()...", category: Category.Info, priority: Priority.Low);
                FreePlugins selectedRow = (FreePlugins)obj;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteFreePluginMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    DeleteFreePluginList = new FreePlugins();
                    DeleteFreePluginList.TransactionOperation = ModelBase.TransactionOperations.Delete;
                    DeleteFreePluginList.IdPlugin = selectedRow.IdPlugin;
                    DeleteFreePluginList.IdCustomer = selectedRow.IdCustomer;
                    DeleteFreePluginList.IdRegion = selectedRow.IdRegion;
                    DeleteFreePluginList.IdCountry = selectedRow.IdCountry;
                    DeleteFreePluginList.IdSite = selectedRow.IdSite;
                    DeleteFreePluginList = PCMService.AddUpdateFreePlugins(DeleteFreePluginList);
                    FreePluginslist.Remove(selectedRow);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeleteFreePluginMessageSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                }
                GeosApplication.Instance.Logger.Log("Method DeleteFreepluginsCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteFreepluginsCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteFreepluginsCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteFreepluginsCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion


        #region implimentation IDataErrorInfo
        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion IDataErrorInfo





    }
}
