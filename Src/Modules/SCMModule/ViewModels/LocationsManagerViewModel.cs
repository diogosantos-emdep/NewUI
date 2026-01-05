using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.Modules.SCM.Common_Classes;
using DevExpress.Xpf.Printing;



namespace Emdep.Geos.Modules.SCM.ViewModels
{
    public class LocationsManagerViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
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

        #region Declarations
        private ObservableCollection<SCMLocationsManager> scmLocationList;  //[rushikesh.gaikwad][GEOS2-5524][04.07.2024]
        private SCMLocationsManager selectedSCMLocation;  //[rushikesh.gaikwad][GEOS2-5524][04.07.2024]
        private bool manufacturerVisibility;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        private Data.Common.Company selectedItem;
        #endregion

        #region Properties
        public ObservableCollection<SCMLocationsManager> SCMLocationList
        {
            get { return scmLocationList; }
            set
            {
                scmLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SCMLocationList"));
            }
        }

        public Data.Common.Company SelectedPlant
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));

            }

        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        public SCMLocationsManager SelectedSCMLocation
        {
            get { return selectedSCMLocation; }
            set
            {
                selectedSCMLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSCMLocation"));
            }
        }
       
        #endregion

        #region Public ICommand       
     
        public ICommand AddNewLocationsManagerButtonCommand { get; set; }//[shweta.thube][GEOS2-5524]

        public ICommand EditLocationDoubleCommand { get; set; } //[rushikesh.gaikwad][GEOS2-5524][04.07.2024]

        public ICommand PrintSCMLocationViewCommand { get; set; }  //[rushikesh.gaikwad][GEOS2-5524][04.07.2024]

        public ICommand ExportSCMLocationViewCommand { get; set; } //[rushikesh.gaikwad][GEOS2-5524][04.07.2024]

        public ICommand RefreshSCMLocationViewCommand { get; set; } //[rushikesh.gaikwad][GEOS2-5524][04.07.2024]
        public bool IsNew { get; set; }//[shweta.thube][GEOS2-5524][03.07.2024]
        public ICommand CommandSCMEditValueChanged { get;  set; } //[shweta.thube][GEOS2-5524][03.07.2024]
        public ICommand CommandTextInput { get; set; }  //[shweta.thube][GEOS2-6630][04.04.2025]
        #endregion

        #region Constructor
        //[shweta.thube][GEOS2-5524][03.07.2024]
        public LocationsManagerViewModel()
        {
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
               

                CommandSCMEditValueChanged = new DevExpress.Mvvm.DelegateCommand<object>(SCMEditValueChangedCommandAction);
                AddNewLocationsManagerButtonCommand = new RelayCommand(new Action<object>(AddNewLocationsManagerCommandAction));
                EditLocationDoubleCommand = new RelayCommand(new Action<object>(EditLocation));   
                PrintSCMLocationViewCommand = new RelayCommand(new Action<object>(PrintSCMLocationList));   
                ExportSCMLocationViewCommand = new RelayCommand(new Action<object>(ExportSCMLocationList)); 
                RefreshSCMLocationViewCommand = new RelayCommand(new Action<object>(RefreshSCMLocationList));


                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                Data.Common.Company selectedPlant = SCMCommon.Instance.PlantList.FirstOrDefault(x => x.Alias == serviceurl);
                if (selectedPlant != null)
                {
                   SelectedPlant = selectedPlant;
                   SCMCommon.Instance.SelectedSinglePlant = selectedPlant;
                }
                else
                {
                    SelectedPlant = SCMCommon.Instance.PlantList.FirstOrDefault();
                    SCMCommon.Instance.SelectedSinglePlant = SelectedPlant;
                }

                #region old code
                //if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMEditFiltersManager)
                //{

                //}
                //else if (GeosApplication.Instance.IsSCMViewConfigurationPermission)
                //{

                //}
                //else if (GeosApplication.Instance.IsSCMViewConfigurationPermission && GeosApplication.Instance.IsSCMEditFiltersManager)
                //{

                //}
                //if (GeosApplication.Instance.IsSCMPermissionReadOnly)
                //{

                //}
                #endregion

                FillSCMLocationsManagerList();
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);  //[shweta.thube][GEOS2-6630][04.04.2025]

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SearchFiltersManagerViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Method

        //[shweta.thube][GEOS2-5524][03.07.2024]
        private void SCMEditValueChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SCMEditValueChangedCommandAction...", category: Category.Info, priority: Priority.Low);
                if (SCMCommon.Instance.SelectedPlant != null)
                {
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

                    FillSCMLocationsManagerList();
                }

                // Close the splash screen if it's active and not a one-time load
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method SCMEditValueChangedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SCMEditValueChangedCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-5524][06.08.2024]
        /// <summary>
        /// 
        /// </summary>
        public void FillSCMLocationsManagerList() 
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSCMLocationsManagerList...", category: Category.Info, priority: Priority.Low);

                if (SCMCommon.Instance.SelectedSinglePlant != null)
                {
                    try
                    {
                        SCMService = new SCMServiceController((SelectedPlant.ServiceProviderUrl != null) ? SelectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        //SCMService = new SCMServiceController("localhost:6699");
                        SCMLocationList = new ObservableCollection<SCMLocationsManager>(SCMService.GetSCMLocationsManagerByIdSCM_V2550(SCMCommon.Instance.SelectedSinglePlant));
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Error in Method FillNewSamplesGridList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillSCMLocationsManagerList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSCMLocationsManagerList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSCMLocationsManagerList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSCMLocationsManagerList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //[pramod.misal][GEOS2-5524][05.08.2024]
        private void AddNewLocationsManagerCommandAction(object obj)
        {
            try
            {
                long NewLocationIdLocationByConnector;
                GeosApplication.Instance.Logger.Log("Method AddNewLocationsManagerCommandAction()....Started Execution", category: Category.Info, priority: Priority.Low);
                TreeListView detailView = (TreeListView)obj;
                AddLocationsManagerView addLocationsManagerView = new AddLocationsManagerView();
                AddLocationsManagerViewModel addLocationsManagerViewModel = new AddLocationsManagerViewModel();
                EventHandler handle = delegate { addLocationsManagerView.Close(); };
                addLocationsManagerViewModel.RequestClose += handle;
                addLocationsManagerView.DataContext = addLocationsManagerViewModel;
                addLocationsManagerViewModel.Init();
                addLocationsManagerViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddLocation").ToString();
                addLocationsManagerViewModel.IsNew = true;
                var ownerInfo = (detailView as FrameworkElement);
                addLocationsManagerView.Owner = Window.GetWindow(ownerInfo);            
                addLocationsManagerView.ShowDialogWindow();

                if (addLocationsManagerViewModel.IsSave== true)
                {
                    SCMLocationList.Add(addLocationsManagerViewModel.NewLocation);
                    NewLocationIdLocationByConnector = addLocationsManagerViewModel.NewLocation.IdSampleLocation;
                    RefreshSCMLocationList(new object());
                    SelectedSCMLocation = SCMLocationList.FirstOrDefault(x => x.IdSampleLocation == NewLocationIdLocationByConnector);
                }



                GeosApplication.Instance.Logger.Log("Method AddNewLocationsManagerCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewLocationsManagerCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //[rushikesh.gaikwad][GEOS2-5524][03.07.2024]
        private void RefreshSCMLocationList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshSCMLocationList...", category: Category.Info, priority: Priority.Low);
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
                //[pramod.misal][GEOS2-5524][06.08.2024]
                FillSCMLocationsManagerList();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshSCMLocationList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshSCMLocationList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-5524][07.07.2024]
        public void EditLocation(object obj)
        {
           
            //if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMEditLocationsManager)
            //{
                try
                {
                    long EditLocationIdLocation;
                    GeosApplication.Instance.Logger.Log("LocationViewModel Method Edit()...", category: Category.Info, priority: Priority.Low);
                    TreeListView detailView = (TreeListView)obj;
                    SCMLocationsManager scmlocationsmanager = (SCMLocationsManager)detailView.FocusedRow;
                    SelectedSCMLocation = scmlocationsmanager;
                    AddLocationsManagerView editLocationView = new AddLocationsManagerView();
                    AddLocationsManagerViewModel editLocationViewModel = new AddLocationsManagerViewModel();
                    EventHandler handle = delegate { editLocationView.Close(); };
                    editLocationViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditLocation").ToString();
                    editLocationViewModel.RequestClose += handle;
                    editLocationView.DataContext = editLocationViewModel;

                    if (SelectedSCMLocation.Parent!=0 && (SCMLocationList.FirstOrDefault(x=>x.IdSampleLocation == SelectedSCMLocation.Parent).IsLeaf==1))
                    {
                        editLocationViewModel.SCMLocationList.Add(SCMLocationList.FirstOrDefault(x => x.IdSampleLocation == SelectedSCMLocation.Parent));
                    }

                    editLocationViewModel.EditInit(scmlocationsmanager);
                    var ownerInfo = (detailView as FrameworkElement);
                    editLocationView.Owner = Window.GetWindow(ownerInfo);
                    editLocationView.ShowDialog();
                    if (editLocationViewModel.IsSave)
                    {
                        SelectedSCMLocation.FullName = editLocationViewModel.NewLocation.FullName;
                        SelectedSCMLocation.Title = editLocationViewModel.NewLocation.Title;
                        SelectedSCMLocation.HtmlColor = editLocationViewModel.NewLocation.HtmlColor;
                        SelectedSCMLocation.IdSampleLocation = editLocationViewModel.NewLocation.IdSampleLocation;
                        SelectedSCMLocation.IdSite = editLocationViewModel.NewLocation.IdSite;
                        SelectedSCMLocation.InUse = editLocationViewModel.NewLocation.InUse;
                        SelectedSCMLocation.Name = editLocationViewModel.NewLocation.Name;
                        SelectedSCMLocation.Parent = editLocationViewModel.NewLocation.Parent;
                        SelectedSCMLocation.Position = editLocationViewModel.NewLocation.Position;
                        SelectedSCMLocation.IsLeaf = editLocationViewModel.IsLeaf;
                        RefreshSCMLocationList(new object());
                        EditLocationIdLocation = editLocationViewModel.NewLocation.IdSampleLocation;
                        SelectedSCMLocation = SCMLocationList.FirstOrDefault(x => x.IdSampleLocation == EditLocationIdLocation);
                    }
                    detailView.Focus();
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in Method EditLocation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

            //}
            
        }

        //[pramod.misal][GEOS2-5524][06.07.2024]
        private void ExportSCMLocationList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportLocationList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Location List";
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

                    ResultFileName = (saveFile.FileName);
                    TreeListView locationTableView = ((TreeListView)obj);
                    locationTableView.ShowTotalSummary = false;
                    locationTableView.ShowFixedTotalSummary = false;
                    locationTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    locationTableView.ShowTotalSummary = true;

                    locationTableView.ShowFixedTotalSummary = true;

                    GeosApplication.Instance.Logger.Log("Method ExportLocationList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportLocationList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-5524][06.08.2024]
        //public void Init()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
        //        //[shweta.thube][GEOS2-5524][03.07.2024]
        //        FillSCMLocationsManagerList();
        //        GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in Method Init()...", ex.Message), category: Category.Exception, priority: Priority.Low);
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //    }
        //}

        //[pramod.misal][GEOS2-5524][06.07.2024]

        //[pramod.misal][GEOS2-5524][06.08.2024]
        private void PrintSCMLocationList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintSCMLocationList...", category: Category.Info, priority: Priority.Low);

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

                PrintableControlLink pcl = new PrintableControlLink((TreeListView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TreeListView)obj).Resources["SCMLocationListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TreeListView)obj).Resources["SCMLocationListReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;

                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintSCMLocationList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintSCMLocationList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {

                SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);                
                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

    }
}
