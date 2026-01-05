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
    public class HRMTrainingsViewModel : ViewModelBase, INotifyPropertyChanged 
    {
        #region Services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declarations
        private List<ProfessionalTraining> trainingsList;
        private ProfessionalTraining selectedTraining;
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        string myFilterString;

        private bool isBusy;
        private object _selectedItem;
        private bool isDeleted;
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
        public List<ProfessionalTraining> TrainingsList
        {
            get
            {
                return trainingsList;
            }
            set
            {
                trainingsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TrainingsList"));
            }
        }

        public ProfessionalTraining SelectedTraining
        {
            get
            {
                return selectedTraining;
            }

            set
            {
                selectedTraining = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTraining"));
            }
        }

        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        public virtual object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
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

        #region public ICommand
        public ICommand AddNewTrainingsCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand EditTrainingCommand { get; set; }
        public ICommand PlantOwnerEditValueChangedCommand { get; private set; }
        public ICommand SelectedYearChangedCommand { get; private set; }
        public ICommand DeleteTrainingCommand { get; set; }

        #endregion

        #region Constructor
        public HRMTrainingsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor HRMTrainingsViewModel()...", category: Category.Info, priority: Priority.Low);

                AddNewTrainingsCommand = new RelayCommand(new Action<object>(AddNewTrainingsInformation));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintTrainingsList));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportTrainingsList));
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshTrainingsList));
                EditTrainingCommand = new RelayCommand(new Action<object>(EditTrainingsInformation));
                PlantOwnerEditValueChangedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerEditValueChangedCommandAction);
                SelectedYearChangedCommand = new DelegateCommand<object>(SelectedYearChangedCommandAction);
                DeleteTrainingCommand = new RelayCommand(new Action<object>(DeleteTrainingCommandAction));

                GeosApplication.Instance.Logger.Log("Constructor HRMTrainingsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor HRMTrainingsViewModel()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
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

                TrainingsList = new List<ProfessionalTraining>(HrmService.GetAllProfessionalTraining());
                SelectedTraining = TrainingsList.FirstOrDefault();

                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Adding New Trainings
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewTrainingsInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewTrainingsInformation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;

                AddEditTrainingsView addEditTrainingsView = new AddEditTrainingsView();
                AddEditTrainingsViewModel addEditTrainingsViewModel = new AddEditTrainingsViewModel();
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                EventHandler handle = delegate { addEditTrainingsView.Close(); };
                addEditTrainingsViewModel.RequestClose += handle;
                addEditTrainingsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddNewTrainings").ToString();
                addEditTrainingsViewModel.IsNew = true;
                addEditTrainingsViewModel.Init();

                addEditTrainingsView.DataContext = addEditTrainingsViewModel;
                var ownerInfo = (obj as FrameworkElement);
                addEditTrainingsView.Owner = Window.GetWindow(ownerInfo);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                addEditTrainingsView.ShowDialog();

                if (addEditTrainingsViewModel.IsSave)
                {
                    //TrainingsList.Add(addEditTrainingsViewModel.NewTrainings);
                    //SelectedTraining = addEditTrainingsViewModel.NewTrainings;
                    //RefreshTrainingsList(obj);

                    ProfessionalTraining tempProfessionalTraining = new ProfessionalTraining();

                    TableView tableView = (TableView)obj;
                    tempProfessionalTraining = (ProfessionalTraining)tableView.DataControl.CurrentItem;
                    tempProfessionalTraining.IdProfessionalTraining = addEditTrainingsViewModel.NewProfessionalTraining.IdProfessionalTraining;
                    tempProfessionalTraining.Code = addEditTrainingsViewModel.NewProfessionalTraining.Code;
                    tempProfessionalTraining.Name = addEditTrainingsViewModel.NewProfessionalTraining.Name;
                    tempProfessionalTraining.Description = addEditTrainingsViewModel.NewProfessionalTraining.Description;
                    tempProfessionalTraining.ExpectedDate = addEditTrainingsViewModel.NewProfessionalTraining.ExpectedDate;
                    tempProfessionalTraining.FinalizationDate = addEditTrainingsViewModel.NewProfessionalTraining.FinalizationDate;
                    tempProfessionalTraining.Duration = addEditTrainingsViewModel.NewProfessionalTraining.Duration;
                    tempProfessionalTraining.Status = addEditTrainingsViewModel.NewProfessionalTraining.Status;
                    tempProfessionalTraining.Type = addEditTrainingsViewModel.NewProfessionalTraining.Type;
                    tempProfessionalTraining.Acceptance = addEditTrainingsViewModel.NewProfessionalTraining.Acceptance;
                    tempProfessionalTraining.Status = addEditTrainingsViewModel.NewProfessionalTraining.Status;
                    tempProfessionalTraining.AcceptanceValue = addEditTrainingsViewModel.NewProfessionalTraining.AcceptanceValue;
                    tempProfessionalTraining.Responsible = addEditTrainingsViewModel.NewProfessionalTraining.Responsible;
                    tempProfessionalTraining.Trainer = addEditTrainingsViewModel.NewProfessionalTraining.Trainer;
                    tempProfessionalTraining.ExternalTrainer = addEditTrainingsViewModel.NewProfessionalTraining.ExternalTrainer;
                    tempProfessionalTraining.ExternalEntity = addEditTrainingsViewModel.NewProfessionalTraining.ExternalEntity;

                    TrainingsList.Add(tempProfessionalTraining);
                    SelectedTraining = tempProfessionalTraining;
                    TrainingsList.OrderByDescending(x => x.Code);
                    detailView.ImmediateUpdateRowPosition = true;
                    detailView.EnableImmediatePosting = true;
                    RefreshTrainingsList(detailView);



                }

                GeosApplication.Instance.Logger.Log("Method AddNewTrainingsInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewTrainingsInformation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Adding Edit Trainings
        /// </summary>
        /// <param name="obj"></param>
        //private void EditTrainingsInformation(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method EditTrainingsInformation()...", category: Category.Info, priority: Priority.Low);
        //        if (obj == null) return;
        //        if (obj is TableView)
        //        {

        //            TableView detailView = (TableView)obj;
        //            ProfessionalTraining profTraining = (ProfessionalTraining)detailView.DataControl.CurrentItem;
        //            SelectedTraining= profTraining;
        //            AddEditTrainingsView addEditTrainingsView = new AddEditTrainingsView();
        //            AddEditTrainingsViewModel addEditTrainingsViewModel = new AddEditTrainingsViewModel();
        //            if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

        //            EventHandler handle = delegate { addEditTrainingsView.Close(); };
        //            addEditTrainingsViewModel.RequestClose += handle;
        //            addEditTrainingsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditTrainings").ToString();
        //            addEditTrainingsViewModel.IsNew = false;
        //            addEditTrainingsViewModel.EditInit(SelectedTraining);

        //            addEditTrainingsView.DataContext = addEditTrainingsViewModel;
        //            var ownerInfo = (obj as FrameworkElement);
        //            addEditTrainingsView.Owner = Window.GetWindow(ownerInfo);
        //            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

        //            addEditTrainingsView.ShowDialog();

        //            if (addEditTrainingsViewModel.IsSave)
        //            {
        //                profTraining = addEditTrainingsViewModel.UpdateProfessionalTraining;
        //                SelectedTraining= profTraining;
        //                TrainingsList.OrderByDescending(x => x.Code);
        //                detailView.ImmediateUpdateRowPosition = true;
        //                detailView.EnableImmediatePosting = true;
        //                RefreshTrainingsList(obj);
        //            }
        //          //  GeosApplication.Instance.Logger.Log("Method EditTrainingsInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
        //        }
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Method EditTrainingsInformation() executed successfully", category: Category.Info, priority: Priority.Low);

        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in method EditTrainingsInformation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void EditTrainingsInformation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditTrainingsInformation()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                ProfessionalTraining profTraining = (ProfessionalTraining)detailView.DataControl.CurrentItem;
                SelectedTraining = profTraining;

                if (SelectedTraining != null)
                {
                    AddEditTrainingsView addEditTrainingsView = new AddEditTrainingsView();
                    AddEditTrainingsViewModel addEditTrainingsViewModel = new AddEditTrainingsViewModel();

                    EventHandler handle = delegate { addEditTrainingsView.Close(); };
                    addEditTrainingsViewModel.RequestClose += handle;
                    addEditTrainingsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditTrainings").ToString();
                    addEditTrainingsViewModel.IsNew = false;
                    addEditTrainingsViewModel.EditInit(SelectedTraining);

                    addEditTrainingsView.DataContext = addEditTrainingsViewModel;
                    var ownerInfo = (obj as FrameworkElement);
                    addEditTrainingsView.Owner = Window.GetWindow(ownerInfo);
                    addEditTrainingsView.ShowDialog();

                    if (addEditTrainingsViewModel.IsSave)
                    {
                        profTraining = addEditTrainingsViewModel.UpdateProfessionalTraining;
                        SelectedTraining = profTraining;
                        RefreshTrainingsList(obj);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditTrainingsInformation()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditTrainingsInformation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for Print Trainings Details List
        /// </summary>
        /// <param name="obj"></param>
        private void PrintTrainingsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintTrainingsList()...", category: Category.Info, priority: Priority.Low);

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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["TrainingsReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["TrainingsReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintTrainingsList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintTrainingsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Export Trainings Details in Excel Sheet.
        /// </summary>
        /// <param name="obj"></param>
        private void ExportTrainingsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportTrainingsList()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Training List";
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

                    GeosApplication.Instance.Logger.Log("Method ExportTrainingsList()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportTrainingsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Refresh Trainings
        /// </summary>
        /// <param name="obj"></param>
        public void RefreshTrainingsList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshTrainingsList()...", category: Category.Info, priority: Priority.Low);

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

                TrainingsList = new List<ProfessionalTraining>(HrmService.GetAllProfessionalTraining());

                IsBusy = false;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshTrainingsList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshTrainingsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Change the Plant Owner
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerEditValueChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PlantOwnerEditValueChangedCommandAction ...", category: Category.Info, priority: Priority.Low);

                FillTrainingListByPlant();

                GeosApplication.Instance.Logger.Log("Method PlantOwnerEditValueChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PlantOwnerEditValueChangedCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Change the PeriodYear
        /// </summary>
        /// <param name="obj"></param>
        private void SelectedYearChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction ...", category: Category.Info, priority: Priority.Low);

                FillTrainingListByPlant();

                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedYearChangedCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillTrainingListByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTrainingListByPlant ...", category: Category.Info, priority: Priority.Low);
                HrmCommon.Instance.IsVisibleLabelLoadingFullYearAttendanceInBackground = Visibility.Visible;
                FillHrmDataInObjectsByCallingLatestServiceMethods.ShowPleaseWaitScreen();

                this.SelectedItem = null;
                

                FillHrmDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
                GeosApplication.Instance.Logger.Log("Method FillTrainingListByPlant() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTrainingListByPlant() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTrainingListByPlant() Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillContactListByPlant()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteTrainingCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteTrainingCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (SelectedTraining.IdStatus == 1485)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["TrainingDetailsMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {

                        IsDeleted = HrmService.IsDeleteProfessionalTraining(SelectedTraining.IdProfessionalTraining);

                        if (IsDeleted)
                        {
                            TrainingsList.Remove(SelectedTraining);
                            TrainingsList = new List<ProfessionalTraining>(TrainingsList);
                            SelectedTraining = TrainingsList.FirstOrDefault();
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TrainingDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                }
                else
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TrainingDeletedUnsuccessful").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                //  if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method DeleteTrainingCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteTrainingCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteTrainingCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteTrainingCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
