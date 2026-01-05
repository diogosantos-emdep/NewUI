using DevExpress.Data.Filtering;
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
    public class TasksViewModel : INotifyPropertyChanged
    {
        #region services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declarations
        private bool isBusy;
        private ProfessionalTask selectedTasks;
        private List<ProfessionalTask> tasksList;
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

        public ProfessionalTask SelectedTask
        {
            get
            {
                return selectedTasks;
            }

            set
            {
                selectedTasks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTasks"));
            }
        }

        public List<ProfessionalTask> TasksList
        {
            get
            {
                return tasksList;
            }

            set
            {
                tasksList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TasksList"));
            }
        }

        #endregion

        #region Public ICommands
        public ICommand AddNewTasksCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand EditTasksDoubleClickCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }
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
        public TasksViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor TasksViewModel()...", category: Category.Info, priority: Priority.Low);

            AddNewTasksCommand = new DelegateCommand<object>(AddNewTasksInformation);
            PrintButtonCommand = new RelayCommand(new Action<object>(PrintTasksList));
            ExportButtonCommand = new RelayCommand(new Action<object>(ExportTasksList));
            RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshTasksList));
            EditTasksDoubleClickCommand = new DelegateCommand<object>(EditTasksInformation);
            CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);

            GeosApplication.Instance.Logger.Log("Constructor TasksViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
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

                TasksList = new List<ProfessionalTask>(HrmService.GetAllProfessionalTask());

                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Adding New Tasks
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewTasksInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewTasksInformation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;

                AddEditTasksView addEditTasksView = new AddEditTasksView();
                AddEditTasksViewModel addEditTasksViewModel = new AddEditTasksViewModel();
                EventHandler handle = delegate { addEditTasksView.Close(); };
                addEditTasksViewModel.RequestClose += handle;
                addEditTasksViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddNewTasks").ToString();
                addEditTasksViewModel.IsNew = true;
                addEditTasksViewModel.Init();

                addEditTasksView.DataContext = addEditTasksViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditTasksView.Owner = Window.GetWindow(ownerInfo);
                addEditTasksView.ShowDialog();

                if (addEditTasksViewModel.IsSave)
                {
                    TasksList.Add(addEditTasksViewModel.NewProfessionalTask);
                    SelectedTask = addEditTasksViewModel.NewProfessionalTask;
                    RefreshTasksList(obj);
                }

                GeosApplication.Instance.Logger.Log("Method AddNewTasksInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewTasksInformation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method For Edit the Tasks Information
        /// </summary>
        /// <param name="obj"></param>
        private void EditTasksInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditTasksInformation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;

                ProfessionalTask profTask = (ProfessionalTask)detailView.DataControl.CurrentItem;
                SelectedTask = profTask;
                if (profTask != null)
                {
                    AddEditTasksView addEditTasksView = new AddEditTasksView();
                    AddEditTasksViewModel addEditTasksViewModel = new AddEditTasksViewModel();
                    EventHandler handle = delegate { addEditTasksView.Close(); };
                    addEditTasksViewModel.RequestClose += handle;
                    addEditTasksViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditTasks").ToString();
                    addEditTasksViewModel.IsNew = false;
                    addEditTasksViewModel.EditInit(SelectedTask);

                    addEditTasksView.DataContext = addEditTasksViewModel;
                    var ownerInfo = (obj as FrameworkElement);
                    addEditTasksView.Owner = Window.GetWindow(ownerInfo);
                    addEditTasksView.ShowDialog();

                    if (addEditTasksViewModel.IsSave)
                    {
                        profTask = addEditTasksViewModel.EditProfessionalTask;
                        SelectedTask = profTask;
                        RefreshTasksList(obj);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditTasksInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditTasksInformation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Print Tasks Details List
        /// </summary>
        /// <param name="obj"></param>
        private void PrintTasksList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintTasksList()...", category: Category.Info, priority: Priority.Low);

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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["TasksReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["TasksReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintTasksList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintTasksList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Export Tasks Details in Excel Sheet.
        /// </summary>
        /// <param name="obj"></param>
        private void ExportTasksList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportTasksList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Task List";
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

                    GeosApplication.Instance.Logger.Log("Method ExportTasksList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportTasksList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Refresh Tasks List. 
        /// </summary>
        public void RefreshTasksList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshTasksList()...", category: Category.Info, priority: Priority.Low);

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

                TasksList = new List<ProfessionalTask>(HrmService.GetAllProfessionalTask());

                IsBusy = false;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshTasksList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshTasksList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Custom filter Pop up 
        /// </summary>
        /// <param name="e"></param>
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "Skills")
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();

                if (e.Column.FieldName == "Skills")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([Skills])")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([Skills])")
                    });

                    foreach (var dataObject in TasksList)
                    {
                        if (dataObject.Skills == null)
                        {
                            continue;
                        }
                        else if (dataObject.Skills != null)
                        {
                            if (dataObject.Skills.Contains("\n"))
                            {
                                string tempSkills = dataObject.Skills;
                                for (int index = 0; index < tempSkills.Length; index++)
                                {
                                    string empSkills = tempSkills.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empSkills))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empSkills;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Skills Like '%{0}%'", empSkills));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempSkills.Contains("\n"))
                                        tempSkills = tempSkills.Remove(0, empSkills.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == TasksList.Where(y => y.Skills == dataObject.Skills).Select(slt => slt.Skills).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = TasksList.Where(y => y.Skills == dataObject.Skills).Select(slt => slt.Skills).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Skills Like '%{0}%'", TasksList.Where(y => y.Skills == dataObject.Skills).Select(slt => slt.Skills).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }

                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();

                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopup() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        #endregion
    }
}
