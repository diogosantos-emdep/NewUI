using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
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

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class ObjectivesViewModel : INotifyPropertyChanged
    {

        #region services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declarations
        private bool isBusy;
        private ProfessionalObjective selectedObjective;
        private List<ProfessionalObjective> objectivesList;
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        #endregion

        #region Properties
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }

            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public ProfessionalObjective SelectedObjective
        {
            get
            {
                return selectedObjective;
            }

            set
            {
                selectedObjective = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObjective"));
            }
        }

        public List<ProfessionalObjective> ObjectivesList
        {
            get
            {
                return objectivesList;
            }

            set
            {
                objectivesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjectivesList"));
            }
        }

        #endregion

        #region Public ICommands
        public ICommand AddNewObjectivesCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand EditObjectivesDoubleClickCommand { get; set; }
        #endregion

        #region Public Events
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
        public ObjectivesViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor ObjectivesViewModel()...", category: Category.Info, priority: Priority.Low);

            AddNewObjectivesCommand = new DelegateCommand<object>(AddNewObjectivesInformation);
            PrintButtonCommand = new RelayCommand(new Action<object>(PrintObjectivesList));
            ExportButtonCommand = new RelayCommand(new Action<object>(ExportObjectivesList));
            RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshObjectivesList));
            EditObjectivesDoubleClickCommand = new RelayCommand(new Action<object>(EditObjectivesInformation));

            GeosApplication.Instance.Logger.Log("Constructor ObjectivesViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method for Intialize....
        /// </summary>
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

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

                ObjectivesList = new List<ProfessionalObjective>(HrmService.GetProfessionalObjectives());

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Adding New Objectives
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewObjectivesInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewObjectivesInformation()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                AddEditObjectivesView addEditObjectivesView = new AddEditObjectivesView();
                AddEditObjectivesViewModel addEditObjectivesViewModel = new AddEditObjectivesViewModel();
                EventHandler handle = delegate { addEditObjectivesView.Close(); };
                addEditObjectivesViewModel.RequestClose += handle;
                addEditObjectivesViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddNewObjectives").ToString();
                addEditObjectivesViewModel.IsNew = true;
                addEditObjectivesViewModel.Init();

                addEditObjectivesView.DataContext = addEditObjectivesViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                addEditObjectivesView.Owner = Window.GetWindow(ownerInfo);
                addEditObjectivesView.ShowDialog();

                if (addEditObjectivesViewModel.IsSave)
                {
                    ObjectivesList.Add(addEditObjectivesViewModel.NewProfessionalObjective);
                    SelectedObjective = addEditObjectivesViewModel.NewProfessionalObjective;
                    RefreshObjectivesList(obj);
                }

                GeosApplication.Instance.Logger.Log("Method AddNewObjectivesInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewObjectivesInformation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method For Edit the Objectives Information
        /// </summary>
        /// <param name="obj"></param>
        private void EditObjectivesInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditObjectivesInformation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                ProfessionalObjective profObj = (ProfessionalObjective)detailView.DataControl.CurrentItem;
                SelectedObjective = profObj;
                if (profObj != null)
                {
                    AddEditObjectivesView addEditObjectivesView = new AddEditObjectivesView();
                    AddEditObjectivesViewModel addEditObjectivesViewModel = new AddEditObjectivesViewModel();
                    EventHandler handle = delegate { addEditObjectivesView.Close(); };
                    addEditObjectivesViewModel.RequestClose += handle;
                    addEditObjectivesViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditObjectives").ToString();
                    addEditObjectivesViewModel.IsNew = false;
                    addEditObjectivesViewModel.EditInit(ObjectivesList, SelectedObjective);

                    addEditObjectivesView.DataContext = addEditObjectivesViewModel;
                    var ownerInfo = (obj as FrameworkElement);
                    addEditObjectivesView.Owner = Window.GetWindow(ownerInfo);
                    addEditObjectivesView.ShowDialog();

                    if (addEditObjectivesViewModel.IsSave)
                    {
                        profObj = addEditObjectivesViewModel.EditProfessionalObjective;
                        SelectedObjective = profObj;
                        RefreshObjectivesList(null);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditObjectivesInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditObjectivesInformation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Print Objectives Details List
        /// </summary>
        /// <param name="obj"></param>
        private void PrintObjectivesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintObjectivesList()...", category: Category.Info, priority: Priority.Low);

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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["ObjectivesReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["ObjectivesReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintObjectivesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintObjectivesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Export Objective Details in Excel Sheet.
        /// </summary>
        /// <param name="obj"></param>
        private void ExportObjectivesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportObjectivesList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Objective List";
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
                    TableView tableView = ((TableView)obj);
                    tableView.ShowTotalSummary = false;
                    tableView.ShowFixedTotalSummary = false;
                    tableView.ExportToXlsx(ResultFileName);


                    IsBusy = false;
                    tableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportObjectivesList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportObjectivesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Refresh Objectives List. 
        /// </summary>
        public void RefreshObjectivesList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshObjectivesList()...", category: Category.Info, priority: Priority.Low);

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

                ObjectivesList = new List<ProfessionalObjective>(HrmService.GetProfessionalObjectives());

                IsBusy = false;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshObjectivesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshObjectivesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
