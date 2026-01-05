using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common.Epc;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class SkillsViewModel : INotifyPropertyChanged
    {
        #region services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declarations
        private bool isBusy;
        private ProfessionalSkill selectedSkill;
        private List<ProfessionalSkill> skillsList;
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        private IList<LookupValue> skillsVisibilityList;
        private LookupValue selectedSkillsVisibility;
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

        public ProfessionalSkill SelectedSkill
        {
            get
            {
                return selectedSkill;
            }

            set
            {
                selectedSkill = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSkill"));
            }
        }

        public List<ProfessionalSkill> SkillsList
        {
            get
            {
                return skillsList;
            }

            set
            {
                skillsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillsList"));
            }
        }

        public IList<LookupValue> SkillsVisibilityList
        {
            get
            {
                return skillsVisibilityList;
            }
            set
            {
                skillsVisibilityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillsVisibilityList"));
            }
        }

        public LookupValue SelectedSkillsVisibility
        {
            get
            {
                return selectedSkillsVisibility;
            }

            set
            {
                selectedSkillsVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSkillsVisibility"));
            }
        }

        public IList<LookupValue> tempECOSVisibilityList { get; set; }

        #endregion

        #region Public ICommands
        public ICommand AddNewSkillsCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand EditSkillsDoubleClickCommand { get; set; }
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
        public SkillsViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor SkillsViewModel()...", category: Category.Info, priority: Priority.Low);

            AddNewSkillsCommand = new DelegateCommand<object>(AddNewSkillsInformation);
            PrintButtonCommand = new RelayCommand(new Action<object>(PrintSkillsList));
            ExportButtonCommand = new RelayCommand(new Action<object>(ExportSkillsList));
            RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshSkillsList));
            EditSkillsDoubleClickCommand = new RelayCommand(new Action<object>(EditSkillsInformation));

            GeosApplication.Instance.Logger.Log("Constructor SkillsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
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

                SkillsList = new List<ProfessionalSkill>(HrmService.GetAllProfessionalSkill());
                SelectedSkill = SkillsList.FirstOrDefault();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Adding New Skills
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewSkillsInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewSkillsInformation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;

                AddEditSkillsView addEditSkillsView = new AddEditSkillsView();
                AddEditSkillsViewModel addEditSkillsViewModel = new AddEditSkillsViewModel();
                EventHandler handle = delegate { addEditSkillsView.Close(); };
                addEditSkillsViewModel.RequestClose += handle;
                addEditSkillsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddNewSkills").ToString();
                addEditSkillsViewModel.IsNew = true;
                addEditSkillsViewModel.Init();

                addEditSkillsView.DataContext = addEditSkillsViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditSkillsView.Owner = Window.GetWindow(ownerInfo);
                addEditSkillsView.ShowDialog();

                if (addEditSkillsViewModel.IsSave)
                {
                    SkillsList.Add(addEditSkillsViewModel.NewProfessionalSkill);
                    SelectedSkill = addEditSkillsViewModel.NewProfessionalSkill;
                    RefreshSkillsList(obj);
                }

                GeosApplication.Instance.Logger.Log("Method AddNewSkillsInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewSkillsInformation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method For Edit the Skills Information
        /// </summary>
        /// <param name="obj"></param>
        private void EditSkillsInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditSkillsInformation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                ProfessionalSkill profSkill = (ProfessionalSkill) detailView.DataControl.CurrentItem;
                SelectedSkill = profSkill;

                if (SelectedSkill != null)
                {
                    AddEditSkillsView addEditSkillsView = new AddEditSkillsView();
                    AddEditSkillsViewModel addEditSkillsViewModel = new AddEditSkillsViewModel();
                    EventHandler handle = delegate { addEditSkillsView.Close(); };
                    addEditSkillsViewModel.RequestClose += handle;
                    addEditSkillsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditSkills").ToString();
                    addEditSkillsViewModel.IsNew = false;
                    addEditSkillsViewModel.EditInit(SkillsList, SelectedSkill);

                    addEditSkillsView.DataContext = addEditSkillsViewModel;
                    var ownerInfo = (obj as FrameworkElement);
                    addEditSkillsView.Owner = Window.GetWindow(ownerInfo);
                    addEditSkillsView.ShowDialog();

                    if (addEditSkillsViewModel.IsSave)
                    {
                        profSkill = addEditSkillsViewModel.EditProfessionalSkill;
                        SelectedSkill = profSkill;
                        RefreshSkillsList(obj);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditSkillsInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditSkillsInformation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Print Skills Details List
        /// </summary>
        /// <param name="obj"></param>
        private void PrintSkillsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintSkillsList()...", category: Category.Info, priority: Priority.Low);

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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["SkillsReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["SkillsReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintSkillsList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintSkillsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Export Skills Details in Excel Sheet.
        /// </summary>
        /// <param name="obj"></param>
        private void ExportSkillsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportSkillsList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Skill List";
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

                    GeosApplication.Instance.Logger.Log("Method ExportSkillsList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportSkillsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Refresh Skills List. 
        /// </summary>
        public void RefreshSkillsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshSkillsList()...", category: Category.Info, priority: Priority.Low);

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

                SkillsList = new List<ProfessionalSkill>(HrmService.GetAllProfessionalSkill());

                IsBusy = false;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshSkillsList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshSkillsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill the Skills Visibility list
        /// </summary>
        
        #endregion

        #region Validation
        #endregion
    }
}
