using DevExpress.Compression;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking.Platform;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Modules.APM.CommonClasses;
using Emdep.Geos.Modules.APM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Emdep.Geos.Modules.APM.ViewModels
{
    class SubTaskParticipantsViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {//[Shweta.Thube][GEOS2-7004]
        #region Services
        IAPMService APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IAPMService APMService = new APMServiceController("localhost:6699");

        #endregion

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

        public void Dispose()
        {
            //throw new NotImplementedException();
        }


        #endregion // Events

        #region Declarations
        private string windowHeader;
        private List<Responsible> responsibleList;
        private Responsible selectedResponsible;
        private ObservableCollection<Responsible> listParticipants;
        private bool isAddButtonEnabled;
        private List<Responsible> clonedListParticipants;
        private bool isNew;
        private List<Responsible> tempParticipantsList;
        private Int32 idTask;
        private bool isSave;
        private bool isSaveChanges;
        private Responsible selectedParticipant;
        #endregion

        #region  public Properties
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public List<Responsible> ResponsibleList
        {
            get { return responsibleList; }
            set
            {
                responsibleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResponsibleList"));
            }
        }
        public Responsible SelectedResponsible
        {
            get { return selectedResponsible; }
            set
            {
                selectedResponsible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedResponsible"));
                if (selectedResponsible != null)
                {
                    IsAddButtonEnabled = true;
                }
            }
        }
        public ObservableCollection<Responsible> ListParticipants
        {
            get { return listParticipants; }
            set
            {
                listParticipants = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListParticipants"));
            }
        }
        public bool IsAddButtonEnabled
        {
            get { return isAddButtonEnabled; }
            set
            {
                isAddButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddButtonEnabled"));
            }
        }
        public List<Responsible> ClonedListParticipants
        {
            get { return clonedListParticipants; }
            set
            {
                clonedListParticipants = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedListParticipants"));
            }
        }
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }
        public List<Responsible> TempParticipantsList
        {
            get { return tempParticipantsList; }
            set
            {
                tempParticipantsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempParticipantsList"));
            }
        }
        public Int32 IdTask
        {
            get { return idTask; }
            set
            {
                idTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdTask"));
            }
        }
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        public bool IsSaveChanges
        {
            get { return isSaveChanges; }
            set
            {
                isSaveChanges = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaveChanges"));
            }
        }

        public Responsible SelectedParticipant
        {
            get { return selectedParticipant; }
            set
            {
                selectedParticipant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedParticipant"));
                if (selectedParticipant != null)
                {
                    IsAddButtonEnabled = true;
                }
            }
        }
        #endregion

        #region ICommands
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand AddButtonActionCommand { get; set; }
        public ICommand DeleteTaskCommand { get; set; }
        #endregion

        #region Constructor
        public SubTaskParticipantsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SubTaskParticipantsViewModel ...", category: Category.Info, priority: Priority.Low);

                CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
                IsAddButtonEnabled = false;
                AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonActionCommandAction));
                AddButtonActionCommand = new RelayCommand(new Action<object>(AddButtonActionCommandAction));
                DeleteTaskCommand = new RelayCommand(new Action<object>(DeleteTaskCommandAction));


                GeosApplication.Instance.Logger.Log("Constructor SubTaskParticipantsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SubTaskParticipantsViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            
        }
        #endregion

        #region Methods

        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                IsSaveChanges = false;
                if (ListParticipants.Count != ClonedListParticipants.Count)
                {
                    IsSaveChanges = true;
                }

                if (IsSaveChanges)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ActionPlanUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        AcceptButtonActionCommandAction(null);
                    }
                }



                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            try
            {
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init(APMActionPlanSubTask selectedSubTask)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()..."), category: Category.Info, priority: Priority.Low);

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
                IdTask = (Int32)selectedSubTask.IdActionPlanTask;

                FillResponsibleList(selectedSubTask.IdCompany.ToString());
                FillParticipantsList(selectedSubTask.IdActionPlanTask);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-7008]
        private void FillResponsibleList(string idCompanies)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillResponsibleList ...", category: Category.Info, priority: Priority.Low);

                string idPeriods = string.Empty;
                if (APMCommon.Instance.SelectedPeriod != null)
                {
                    List<long> selectedPeriod = APMCommon.Instance.SelectedPeriod.Cast<long>().ToList();
                    idPeriods = string.Join(",", selectedPeriod);
                }
                //APMService = new APMServiceController("localhost:6699");
                var temp = APMService.GetParticipantsListAsPerLocation_V2670(idCompanies);//[shweta.thube][GEOS2-9354][03.09.2025]
                ResponsibleList = new List<Responsible>();
                ResponsibleList.AddRange(temp.Where(x => x.IdEmployeeStatus == 136).OrderBy(x => x.FullName));
                ResponsibleList = new List<Responsible>(ResponsibleList);

                GeosApplication.Instance.Logger.Log("Method FillResponsibleList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillResponsibleList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillParticipantsList(Int64 idTask)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAttachmentList ...", category: Category.Info, priority: Priority.Low);
                //APMService = new APMServiceController("localhost:6699");
                List<Responsible> temp = APMService.GetParticipantsByIdTask_V2620(idTask);
                ListParticipants = new ObservableCollection<Responsible>();
                foreach (Responsible item in temp)
                {
                    ListParticipants.Add((Responsible)item.Clone());
                }

                ClonedListParticipants = new List<Responsible>();
                if (ListParticipants != null && ListParticipants.Count > 0)
                {
                    foreach (Responsible item in ListParticipants)
                    {
                        ClonedListParticipants.Add((Responsible)item.Clone());
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillAttachmentList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAttachmentList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAttachmentList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAttachmentList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AcceptButtonActionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonActionCommandAction ...", category: Category.Info, priority: Priority.Low);
                if (TempParticipantsList != null)
                {
                    //APMService = new APMServiceController("localhost:6699");
                    TempParticipantsList = APMService.AddDeleteParticipantsByIdTask_V2620(TempParticipantsList);

                    IsSave = true;
                }
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddTaskParticipantsUpdatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonActionCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonActionCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonActionCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonActionCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddButtonActionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddButtonActionCommandAction ...", category: Category.Info, priority: Priority.Low);

                if (SelectedParticipant != null)
                {
                    SelectedResponsible = SelectedParticipant;
                    SelectedParticipant = null;
                    SelectedResponsible.IdTask = IdTask;
                    if (!ListParticipants.Any(x => x.IdEmployee == SelectedResponsible.IdEmployee))
                    {

                        ListParticipants.Add(SelectedResponsible);
                        IsAddButtonEnabled = false;
                    }

                    if (TempParticipantsList == null)
                    {
                        TempParticipantsList = new List<Responsible>();
                    }

                    if (!ClonedListParticipants.Any(x => x.IdEmployee == SelectedResponsible.IdEmployee))
                    {
                        SelectedResponsible.TransactionOperation = ModelBase.TransactionOperations.Add;
                        TempParticipantsList.Add(SelectedResponsible);
                    }

                    IsNew = true;
                    IsAddButtonEnabled = false;
                }

                GeosApplication.Instance.Logger.Log("Method AddButtonActionCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddButtonActionCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddButtonActionCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonActionCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void DeleteTaskCommandAction(object obj)
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeletParticipantsDetailsMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    Responsible Temp = (Responsible)obj;
                    if (SelectedResponsible == null)
                    {
                        SelectedResponsible = new Responsible();
                    }

                    if (TempParticipantsList == null)
                    {
                        TempParticipantsList = new List<Responsible>();
                    }

                    if (ListParticipants.Contains(Temp))
                    {
                        Temp.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        Temp.IdTask = IdTask;
                        TempParticipantsList.Add(Temp);
                        ListParticipants.Remove(Temp);
                    }
                    else
                    {
                        TempParticipantsList.Remove(Temp);
                    }

                }

                GeosApplication.Instance.Logger.Log("Method DeleteTaskCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteTaskCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Validation

        #endregion
    }
}
