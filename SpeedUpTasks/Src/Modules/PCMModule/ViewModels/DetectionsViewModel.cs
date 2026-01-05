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
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class DetectionsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

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

        #region Declarations

        private ObservableCollection<DetectionDetails> detectionsMenulist;
        private DetectionDetails selectedDetection;
        private bool isDeleted;
        private bool isBusy;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }

        private ObservableCollection<TestTypes> testTypesList;
        private TestTypes selectedTestTypes;

        private List<LookupValue> statusList;
        private int selectedStatusIndex;
        private LookupValue selectedStatus;

        #endregion

        #region Properties

        public ObservableCollection<DetectionDetails> DetectionsMenulist
        {
            get { return detectionsMenulist; }
            set
            {
                detectionsMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionsMenulist"));
            }
        }

        public DetectionDetails SelectedDetection
        {
            get
            {
                return selectedDetection;
            }
            set
            {
                selectedDetection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDetection"));
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

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public ObservableCollection<TestTypes> TestTypesList
        {
            get
            {
                return testTypesList;
            }

            set
            {
                testTypesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TestTypesList"));
            }
        }

        public TestTypes SelectedTestTypes
        {
            get
            {
                return selectedTestTypes;
            }

            set
            {
                selectedTestTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTestTypes"));
            }
        }

        public List<LookupValue> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }

        public int SelectedStatusIndex
        {
            get { return selectedStatusIndex; }
            set
            {
                selectedStatusIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatusIndex"));
            }
        }

        public LookupValue SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatus"));
            }
        }

        #endregion

        #region ICommands

        public ICommand DetectionsDoubleClickCommand { get; set; }
        public ICommand AddDetectionButtonCommand { get; set; }
        public ICommand RefreshDetectionsCommand { get; set; }
        public ICommand DeleteDetectionCommand { get; set; }
        public ICommand PrintDetectionsCommand { get; set; }
        public ICommand ExportDetectionsCommand { get; set; }

        public ICommand CellValueUpdatedCommnad { get; set; }



        #endregion

        #region Constructor

        public DetectionsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor DetectionsViewModel ...", category: Category.Info, priority: Priority.Low);

                DetectionsDoubleClickCommand = new RelayCommand(new Action<object>(EditDetectionItem));
                AddDetectionButtonCommand = new RelayCommand(new Action<object>(AddDetectionItem));
                RefreshDetectionsCommand = new RelayCommand(new Action<object>(RefreshDetectionView));
                DeleteDetectionCommand = new RelayCommand(new Action<object>(DeleteDetectionItem));
                PrintDetectionsCommand = new RelayCommand(new Action<object>(PrintDetections));
                ExportDetectionsCommand = new RelayCommand(new Action<object>(ExportDetections));
                CellValueUpdatedCommnad = new DelegateCommand<CellValueChangedEventArgs>(CellValueUpdatedCommnadAction);

                GeosApplication.Instance.Logger.Log("Constructor DetectionsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor DetectionsViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CellValueUpdatedCommnadAction(CellValueChangedEventArgs obj)
        {
           // if (obj.Column.FieldName == "DetectionTypes.Name")
                obj.Source.PostEditor();
        }

        private void DeleteDetectionItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteDetectionItem()...", category: Category.Info, priority: Priority.Low);

                if (SelectedDetection.Name == null)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteDetectionDetailsMessageWithoutName"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        IsDeleted = PCMService.IsDeletedDetection(SelectedDetection.IdDetections);
                        
                        if (IsDeleted)
                        {
                            DetectionsMenulist.Remove(SelectedDetection);
                            DetectionsMenulist = new ObservableCollection<DetectionDetails>(DetectionsMenulist);
                            SelectedDetection = DetectionsMenulist.FirstOrDefault();
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DetectionDetailsDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                }
                else
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeleteDetectionDetailsMessage"].ToString(), "[" + SelectedDetection.Name.ToString() + "]"), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        IsDeleted = PCMService.IsDeletedDetection(SelectedDetection.IdDetections);

                        if (IsDeleted)
                        {
                            DetectionsMenulist.Remove(SelectedDetection);
                            DetectionsMenulist = new ObservableCollection<DetectionDetails>(DetectionsMenulist);
                            SelectedDetection = DetectionsMenulist.FirstOrDefault();
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DetectionDetailsDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DeleteDetectionItem()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteDetectionItem() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteDetectionItem() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteDetectionItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        public void Init()
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

                DetectionsMenulist = new ObservableCollection<DetectionDetails>(PCMService.GetAllDetectionsWaysOptionsSpareParts());
                //DetectionsMenulist.OrderBy(x=>x.Name);
                if (DetectionsMenulist.Count > 0)
                    SelectedDetection = DetectionsMenulist.FirstOrDefault();

                FillTestTypes();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillTestTypes()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillTestTypes..."), category: Category.Info, priority: Priority.Low);

                IList<TestTypes> tempTestTypesList = PCMService.GetAllTestTypes();
                TestTypesList = new ObservableCollection<TestTypes>();
                TestTypesList = new ObservableCollection<TestTypes>(tempTestTypesList);
                SelectedTestTypes = TestTypesList.FirstOrDefault();

                GeosApplication.Instance.Logger.Log(string.Format("Method FillTestTypes()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTestTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTestTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTestTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempStatusList = PCMService.GetLookupValues(45);
                StatusList = new List<LookupValue>();
                StatusList = new List<LookupValue>(tempStatusList);
                SelectedStatusIndex = StatusList.FindIndex(x => x.IdLookupValue == 225);

                GeosApplication.Instance.Logger.Log("Method FillStatusList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditDetectionItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditDetectionItem()...", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;

                DetectionDetails detections = null;

                if (obj is TableView)
                {
                    TableView detailView = (TableView)obj;
                    detections = (DetectionDetails)detailView.DataControl.CurrentItem;


                    if (detections != null)
                    {
                        if (detections.DetectionTypes.IdDetectionType == 2 || detections.DetectionTypes.IdDetectionType == 3)
                        {
                            EditDetectionView addEditOptionsDetectionsView = new EditDetectionView();
                            EditDetectionViewModel addEditOptionsDetectionsViewModel = new EditDetectionViewModel();
                            EventHandler handle = delegate { addEditOptionsDetectionsView.Close(); };
                            addEditOptionsDetectionsViewModel.RequestClose += handle;
                            
                            addEditOptionsDetectionsViewModel.IsNew = false;
                            addEditOptionsDetectionsViewModel.IsSelectedTestReadOnly = true;
                            addEditOptionsDetectionsViewModel.IsStackPanelVisible = Visibility.Visible;

                            if (detections.DetectionTypes.IdDetectionType == 2)
                            {
                                addEditOptionsDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditDetectionHeader").ToString();
                                addEditOptionsDetectionsViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionDetections").ToString());
                                addEditOptionsDetectionsViewModel.EditInitDetections(detections);
                            }
                            else if (detections.DetectionTypes.IdDetectionType == 3)
                            {
                                addEditOptionsDetectionsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditOptionHeader").ToString();
                                addEditOptionsDetectionsViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionOptions").ToString());
                                addEditOptionsDetectionsViewModel.EditInitDetections(detections);
                            }
                            addEditOptionsDetectionsView.DataContext = addEditOptionsDetectionsViewModel;
                            var ownerInfo = (detailView as FrameworkElement);
                            addEditOptionsDetectionsView.Owner = Window.GetWindow(ownerInfo);
                            addEditOptionsDetectionsView.ShowDialog();

                            if (addEditOptionsDetectionsViewModel.IsOptionSave || addEditOptionsDetectionsViewModel.IsDetectionSave)
                            {
                                detections.IdDetections = addEditOptionsDetectionsViewModel.UpdatedItem.IdDetections;
                                detections.Name = addEditOptionsDetectionsViewModel.UpdatedItem.Name;
                                detections.Description = addEditOptionsDetectionsViewModel.UpdatedItem.Description;
                                detections.IdDetectionType = (byte)addEditOptionsDetectionsViewModel.UpdatedItem.IdDetectionType;
                                detections.Code = addEditOptionsDetectionsViewModel.UpdatedItem.Code;
                                detections.IdTestType = addEditOptionsDetectionsViewModel.UpdatedItem.IdTestType;
                                detections.TestTypes.Name = addEditOptionsDetectionsViewModel.SelectedTestType.Name;
                                detections.IdGroup = addEditOptionsDetectionsViewModel.UpdatedItem.IdGroup;
                                if (detections.DetectionGroup == null)
                                {
                                    detections.DetectionGroup = new DetectionGroup();
                                }
                                detections.DetectionGroup.Name = addEditOptionsDetectionsViewModel.SelectedOrder.Name;
                                detections.WeldOrder = addEditOptionsDetectionsViewModel.UpdatedItem.WeldOrder;
                                detections.Code = addEditOptionsDetectionsViewModel.UpdatedItem.Code;
                                detections.DetectionTypes.Name = addEditOptionsDetectionsViewModel.SelectedTest.Name;
                                detections.LastUpdate = addEditOptionsDetectionsViewModel.UpdatedItem.LastUpdate;
                            }
                            detailView.DataControl.CurrentItem = detections;
                            detailView.ImmediateUpdateRowPosition = true;
                            detailView.EnableImmediatePosting = true;
                        }

                        else if (detections.DetectionTypes.IdDetectionType == 1 || detections.DetectionTypes.IdDetectionType == 4)
                        {
                            EditDetectionView addOptionWayDetectionSparePartView = new EditDetectionView();
                            EditDetectionViewModel addOptionWayDetectionSparePartViewModel = new EditDetectionViewModel();
                            EventHandler handleWaysSparePart = delegate { addOptionWayDetectionSparePartView.Close(); };
                            addOptionWayDetectionSparePartViewModel.RequestClose += handleWaysSparePart;

                            addOptionWayDetectionSparePartViewModel.IsNew = false;
                            addOptionWayDetectionSparePartViewModel.IsStackPanelVisible = Visibility.Hidden;
                            addOptionWayDetectionSparePartViewModel.IsSelectedTestReadOnly = true;

                            if (detections.DetectionTypes.IdDetectionType == 1)
                            {
                                addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditWayHeader").ToString();
                                addOptionWayDetectionSparePartViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionWay").ToString());
                                addOptionWayDetectionSparePartViewModel.EditInitWaysAndSparepart(detections);
                            }

                            else if (detections.DetectionTypes.IdDetectionType == 4)
                            {
                                addOptionWayDetectionSparePartViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditSparePartHeader").ToString();
                                addOptionWayDetectionSparePartViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionSpareParts").ToString());
                                addOptionWayDetectionSparePartViewModel.EditInitWaysAndSparepart(detections);
                            }
                            addOptionWayDetectionSparePartView.DataContext = addOptionWayDetectionSparePartViewModel;
                            var ownerInfo = (detailView as FrameworkElement);
                            addOptionWayDetectionSparePartView.Owner = Window.GetWindow(ownerInfo);
                            addOptionWayDetectionSparePartView.ShowDialog();

                            if (addOptionWayDetectionSparePartViewModel.IsWaySave || addOptionWayDetectionSparePartViewModel.IsSparepartSave)
                            {
                                detections.IdDetections = addOptionWayDetectionSparePartViewModel.UpdatedItem.IdDetections;
                                detections.Name = addOptionWayDetectionSparePartViewModel.UpdatedItem.Name;
                                detections.Description = addOptionWayDetectionSparePartViewModel.UpdatedItem.Description;
                                detections.IdDetectionType = (byte)addOptionWayDetectionSparePartViewModel.UpdatedItem.IdDetectionType;
                                detections.Code = addOptionWayDetectionSparePartViewModel.UpdatedItem.Code;
                                detections.IdTestType = addOptionWayDetectionSparePartViewModel.UpdatedItem.IdTestType;
                                detections.TestTypes.Name = addOptionWayDetectionSparePartViewModel.SelectedTestType.Name;
                                detections.IdGroup = addOptionWayDetectionSparePartViewModel.UpdatedItem.IdGroup;
                                detections.WeldOrder = addOptionWayDetectionSparePartViewModel.UpdatedItem.WeldOrder;
                                detections.Code = addOptionWayDetectionSparePartViewModel.UpdatedItem.Code;
                                detections.LastUpdate = addOptionWayDetectionSparePartViewModel.UpdatedItem.LastUpdate;
                                detections.DetectionTypes.Name = addOptionWayDetectionSparePartViewModel.SelectedTest.Name;
                                detections.LastUpdate = addOptionWayDetectionSparePartViewModel.UpdatedItem.LastUpdate;
                            }
                            detailView.DataControl.CurrentItem = detections;
                            detailView.ImmediateUpdateRowPosition = true;
                            detailView.EnableImmediatePosting = true;
                        }
                    }
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditDetectionItem()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditDetectionItem()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddDetectionItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDetectionItem()...", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;

                DetectionDetails detections = null;

                if (obj is TableView)
                {
                    TableView detailView = (TableView)obj;
                    detections = (DetectionDetails)detailView.DataControl.CurrentItem;

                    AddDetectionView addDetectionView = new AddDetectionView();
                    AddDetectionViewModel addDetectionViewModel = new AddDetectionViewModel();

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


                    EventHandler handleWaysSparePart = delegate { addDetectionView.Close(); };
                    addDetectionViewModel.RequestClose += handleWaysSparePart;

                    addDetectionViewModel.IsStackPanelVisible = Visibility.Collapsed;
                    addDetectionViewModel.IsSelectedTestReadOnly = false;
                    addDetectionViewModel.IsNew = true;
                    addDetectionViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddWayHeader").ToString();
                    addDetectionViewModel.EditInit(System.Windows.Application.Current.FindResource("CaptionWay").ToString());

                    addDetectionView.DataContext = addDetectionViewModel;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                    var ownerInfo = (detailView as FrameworkElement);
                    addDetectionView.Owner = Window.GetWindow(ownerInfo);
                    addDetectionView.ShowDialog();

                    if (addDetectionViewModel.IsWaySave)
                    {
                        DetectionDetails tempWay = new DetectionDetails();

                        TableView tableView = (TableView)obj;
                        tempWay = (DetectionDetails)tableView.DataControl.CurrentItem;

                        if (tempWay.IdGroup != null)
                            tempWay.IdGroup = null;
                        tempWay.IdDetections = addDetectionViewModel.NewWay.IdDetections;
                        tempWay.Name = addDetectionViewModel.NewWay.Name;
                        tempWay.Description = addDetectionViewModel.NewWay.Description;
                        tempWay.IdDetectionType = (byte)addDetectionViewModel.NewWay.IdDetectionType;
                        if (tempWay.DetectionTypes == null)
                        {
                            tempWay.DetectionTypes = new DetectionTypes();
                        }
                        tempWay.DetectionTypes.Name = addDetectionViewModel.SelectedTest.Name;
                        tempWay.IdTestType = addDetectionViewModel.SelectedTestType.IdTestType;
                        if (tempWay.TestTypes == null)
                        {
                            tempWay.TestTypes = new TestTypes();
                        }
                        tempWay.TestTypes.Name = addDetectionViewModel.SelectedTestType.Name;
                        tempWay.WeldOrder = addDetectionViewModel.NewWay.WeldOrder;
                        tempWay.Code = addDetectionViewModel.NewWay.Code;
                        tempWay.LastUpdate = addDetectionViewModel.NewWay.LastUpdate;
                        DetectionsMenulist.Add(tempWay);
                        SelectedDetection = tempWay;
                        DetectionsMenulist.OrderBy(x => x.Name); 
                    }

                    if (addDetectionViewModel.IsSparepartSave)
                    {
                        DetectionDetails tempSparePart = new DetectionDetails();

                        TableView tableView = (TableView)obj;
                        tempSparePart = (DetectionDetails)tableView.DataControl.CurrentItem;

                        if (tempSparePart.IdGroup != null)
                            tempSparePart.IdGroup = null;
                        tempSparePart.IdDetections = addDetectionViewModel.NewSparePart.IdDetections;
                        tempSparePart.Name = addDetectionViewModel.NewSparePart.Name;
                        tempSparePart.Description = addDetectionViewModel.NewSparePart.Description;
                        tempSparePart.IdDetectionType = (byte)addDetectionViewModel.NewSparePart.IdDetectionType;
                        if (tempSparePart.DetectionTypes == null)
                        {
                            tempSparePart.DetectionTypes = new DetectionTypes();
                        }
                        tempSparePart.DetectionTypes.Name = addDetectionViewModel.SelectedTest.Name;
                        tempSparePart.IdTestType = addDetectionViewModel.SelectedTestType.IdTestType;
                        if (tempSparePart.TestTypes == null)
                        {
                            tempSparePart.TestTypes = new TestTypes();
                        }
                        tempSparePart.TestTypes.Name = addDetectionViewModel.SelectedTestType.Name;
                        tempSparePart.WeldOrder = addDetectionViewModel.NewSparePart.WeldOrder;
                        tempSparePart.Code = addDetectionViewModel.NewSparePart.Code;
                        tempSparePart.LastUpdate = addDetectionViewModel.NewSparePart.LastUpdate;
                        DetectionsMenulist.Add(tempSparePart);
                        DetectionsMenulist.OrderBy(x => x.Name);
                    }

                    if (addDetectionViewModel.IsDetectionSave)
                    {
                        DetectionDetails tempDetection = new DetectionDetails();
                        TableView tableView = (TableView)obj;
                        tempDetection = (DetectionDetails)tableView.DataControl.CurrentItem;

                        tempDetection.IdDetections = addDetectionViewModel.NewDetection.IdDetections;
                        tempDetection.Name = addDetectionViewModel.NewDetection.Name;
                        tempDetection.Description = addDetectionViewModel.NewDetection.Description;
                        tempDetection.IdDetectionType = (byte)addDetectionViewModel.NewDetection.IdDetectionType;
                        tempDetection.Code = addDetectionViewModel.NewDetection.Code;
                        if (tempDetection.DetectionTypes == null)
                        {
                            tempDetection.DetectionTypes = new DetectionTypes();
                        }
                        tempDetection.DetectionTypes.Name = addDetectionViewModel.SelectedTest.Name;
                        if (tempDetection.TestTypes == null)
                        {
                            tempDetection.TestTypes = new TestTypes();
                        }
                        tempDetection.TestTypes.Name = addDetectionViewModel.SelectedTestType.Name;
                        tempDetection.IdGroup = addDetectionViewModel.NewDetection.IdGroup;
                        if (tempDetection.DetectionGroup == null)
                        {
                            tempDetection.DetectionGroup = new DetectionGroup();
                        }
                        tempDetection.DetectionGroup.Name = addDetectionViewModel.SelectedOrder.Name;
                        tempDetection.WeldOrder = addDetectionViewModel.NewDetection.WeldOrder;
                        tempDetection.LastUpdate = addDetectionViewModel.NewDetection.LastUpdate;
                        DetectionsMenulist.Add(tempDetection);
                        DetectionsMenulist.OrderBy(x => x.Name);
                    }

                    if (addDetectionViewModel.IsOptionSave)
                    {
                        DetectionDetails tempOption = new DetectionDetails();
                        TableView tableView = (TableView)obj;
                        tempOption = (DetectionDetails)tableView.DataControl.CurrentItem;

                        tempOption.IdDetections = addDetectionViewModel.NewOption.IdDetections;
                        tempOption.Name = addDetectionViewModel.NewOption.Name;
                        tempOption.Description = addDetectionViewModel.NewOption.Description;
                        tempOption.IdDetectionType = (byte)addDetectionViewModel.NewOption.IdDetectionType;
                        tempOption.Code = addDetectionViewModel.NewOption.Code;
                        if (tempOption.DetectionTypes == null)
                        {
                            tempOption.DetectionTypes = new DetectionTypes();
                        }
                        tempOption.DetectionTypes.Name = addDetectionViewModel.SelectedTest.Name;
                        if (tempOption.TestTypes == null)
                        {
                            tempOption.TestTypes = new TestTypes();
                        }
                        tempOption.TestTypes.Name = addDetectionViewModel.SelectedTestType.Name;
                        tempOption.IdGroup = addDetectionViewModel.NewOption.IdGroup;
                        if (tempOption.DetectionGroup == null)
                        {
                            tempOption.DetectionGroup = new DetectionGroup();
                        }
                        tempOption.DetectionGroup.Name = addDetectionViewModel.SelectedOrder.Name;
                        tempOption.WeldOrder = addDetectionViewModel.NewOption.WeldOrder;
                        tempOption.LastUpdate = addDetectionViewModel.NewOption.LastUpdate;
                        DetectionsMenulist.Add(tempOption);
                        DetectionsMenulist.OrderBy(x => x.Name);
                    }
                    detailView.DataControl.CurrentItem = detections;
                    detailView.ImmediateUpdateRowPosition = true;
                    detailView.EnableImmediatePosting = true;
                    RefreshDetectionView(detailView);
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddDetectionItem()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddDetectionItem()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshDetectionView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshDetectionView()...", category: Category.Info, priority: Priority.Low);
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

                DetectionsMenulist = new ObservableCollection<DetectionDetails>(PCMService.GetAllDetectionsWaysOptionsSpareParts());
                SelectedDetection = DetectionsMenulist.FirstOrDefault();
                detailView.SearchString = null;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshDetectionView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshDetectionView() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshDetectionView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshDetectionView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PrintDetections(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPurchaseOrderList()...", category: Category.Info, priority: Priority.Low);

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
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["DetectionsReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["DetectionsReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintPurchaseOrderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintPurchaseOrderList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportDetections(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportDetections()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Detections List";
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

                    GeosApplication.Instance.Logger.Log("Method ExportDetections()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportDetections()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Options_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }

        #endregion
    }
}
