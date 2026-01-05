using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.PCM.Views;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.PCM.ViewModels
{//[Sudhir.Jangra][GEOS2-4901]
    public class AddEditHardLockLicensesViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
          IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
          //IPCMService PCMService = new PCMServiceController("localhost:6699");
        #endregion

        #region Public Events
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

        #region Declaration
        private string windowHeader;
        ObservableCollection<HardLockLicenses> referenceList;
        ObservableCollection<HardLockPlugins> hardLockPluginList;
        HardLockLicenses selectedReference;
        ObservableCollection<HardLockPlugins> supportedPluginList;
        ObservableCollection<HardLockPlugins> supportedPluginDuplicateList;
        HardLockPlugins selectedPlugin;
        bool isNew;
        bool isSave;
        private string error = string.Empty;
        private List<HardLockLicenses> hardLockGridList;
        private HardLockPlugins selectedHardLock;
        bool isDeleted;
        ObservableCollection<HardLockPlugins> deletedPluginList;
        #endregion

        #region Properties
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public ObservableCollection<HardLockLicenses> ReferenceList
        {
            get { return referenceList; }
            set
            {
                referenceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReferenceList"));
            }
        }
        public ObservableCollection<HardLockPlugins> HardLockPluginList
        {
            get { return hardLockPluginList; }
            set
            {
                hardLockPluginList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HardLockPluginList"));
            }
        }

        public HardLockLicenses SelectedReference
        {
            get { return selectedReference; }
            set
            {
                selectedReference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedReference"));
            }
        }

        public ObservableCollection<HardLockPlugins> SupportedPluginList
        {
            get { return supportedPluginList; }
            set
            {
                supportedPluginList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SupportedPluginList"));
            }
        }

        public HardLockPlugins SelectedPlugin
        {
            get { return selectedPlugin; }
            set
            {
                selectedPlugin = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlugin"));
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
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        public List<HardLockLicenses> HardLockGridList
        {
            get { return hardLockGridList; }
            set
            {
                hardLockGridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HardLockGridList"));
            }
        }

        public ObservableCollection<HardLockPlugins> SupportedPluginDuplicateList
        {
            get { return supportedPluginDuplicateList; }
            set
            {
                supportedPluginDuplicateList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SupportedPluginDuplicateList"));
            }
        }

        public HardLockPlugins SelectedHardLock
        {
            get { return selectedHardLock; }
            set
            {
                selectedHardLock = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedHardLock"));
            }
        }
        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
            }
        }
        public ObservableCollection<HardLockPlugins> DeletedPluginList
        {
            get { return deletedPluginList; }
            set
            {
                deletedPluginList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeletedPluginList"));
            }
        }
        #endregion

        #region ICommand
        /// <summary>
        /// 
        /// </summary>
        public ICommand CancelButtonCommand { get; set; }

        public ICommand CommandOnDragRecordOverSupportedPlugin { get; set; }
        public ICommand CommandCompleteRecordDragDropSupportedPlugin { get; set; }
        public ICommand CommandOnDragRecordOverSupportedPluginGrid { get; set; }

        public ICommand DeleteSupportedPluginCommand { get; set; }

        public ICommand AcceptButtonCommand { get; set; }

        public ICommand AddNewPluginCommand { get; set; }
        #endregion

        #region Constructor
        public AddEditHardLockLicensesViewModel()
        {
            CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
            CommandOnDragRecordOverSupportedPlugin = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverSupportedPlugin);
            CommandCompleteRecordDragDropSupportedPlugin = new DelegateCommand<CompleteRecordDragDropEventArgs>(CompleteRecordDragDropSupportedPlugin);
            CommandOnDragRecordOverSupportedPluginGrid = new DelegateCommand<DragRecordOverEventArgs>(OnDragRecordOverSupportedPluginGrid);
            DeleteSupportedPluginCommand = new RelayCommand(new Action<object>(DeleteSupportedPluginCommandAction));
            AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
            //[Sudhir.Jangra][GEOS2-4915]
            AddNewPluginCommand = new RelayCommand(new Action<object>(AddNewPluginCommandAction));

        }
        #endregion

        #region Method
        public void Init()
        {
            try
            {
                FillReferenceList();
                FillHardLockPlugin();
                SelectedReference = ReferenceList.FirstOrDefault();
                SupportedPluginList = new ObservableCollection<HardLockPlugins>();
                SupportedPluginDuplicateList = new ObservableCollection<HardLockPlugins>();
                DeletedPluginList = new ObservableCollection<HardLockPlugins>();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        public void EditInit(Articles editData)
        {
            try
            {
                ReferenceList = new ObservableCollection<HardLockLicenses>();
                // ReferenceList.Insert(0, new Articles() { Reference = "---" });
                ReferenceList.Insert(0, new HardLockLicenses() { IdArticle = editData.IdArticle,IdPCMArticle=editData.IdPCMArticle, Reference = editData.Reference, Description = editData.Description });
                SelectedReference = ReferenceList.FirstOrDefault();
                FillHardLockPlugin();
                FillSupportedPlugin(editData.IdArticle);

                if (SupportedPluginList != null)
                {
                    foreach (HardLockPlugins item in SupportedPluginList)
                    {
                        HardLockPluginList.Where(x => x.IdPlugin == item.IdPlugin).ToList().ForEach(a => a.IsCurrentPlugin = true);
                    }
                }
                DeletedPluginList = new ObservableCollection<HardLockPlugins>();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void FillReferenceList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillReferenceList()...", category: Category.Info, priority: Priority.Low);
                var temp = new ObservableCollection<HardLockLicenses>(PCMService.GetArticlesForAddEditHardLockLicense_V2450());
                ReferenceList = new ObservableCollection<HardLockLicenses>(temp.Where(x => !HardLockGridList.Any(b => b.IdArticle == x.IdArticle)));
                ReferenceList.Insert(0, new HardLockLicenses() { Reference = "---" });
                GeosApplication.Instance.Logger.Log("Method FillReferenceList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillReferenceList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillReferenceList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillReferenceList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void FillHardLockPlugin()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillHardLockPlugin()...", category: Category.Info, priority: Priority.Low);
                ObservableCollection<HardLockPlugins> temp = new ObservableCollection<HardLockPlugins>(PCMService.GetAllHardLockPluginForAddEditHardLockLicense_V2450());
                HardLockPluginList = new ObservableCollection<HardLockPlugins>(temp.GroupBy(h => h.Name).Select(g => g.First()));


                GeosApplication.Instance.Logger.Log("Method FillHardLockPlugin()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillHardLockPlugin() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillHardLockPlugin() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillHardLockPlugin() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverSupportedPlugin(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverSupportedPlugin()...", category: Category.Info, priority: Priority.Low);
                if (typeof(HardLockPlugins).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = false;
                }
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverSupportedPlugin()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverSupportedPlugin() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CompleteRecordDragDropSupportedPlugin(CompleteRecordDragDropEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropFamily()...", category: Category.Info, priority: Priority.Low);

                e.Handled = true;
                if (SupportedPluginList != null)
                    SupportedPluginList = new ObservableCollection<HardLockPlugins>(SupportedPluginList.GroupBy(opt => opt.IdPlugin).Select(g => g.First()));
                if (SupportedPluginList != null)
                {
                    foreach (HardLockPlugins item in SupportedPluginList)
                    {
                        HardLockPluginList.Where(a => a.IdPlugin == item.IdPlugin).ToList().ForEach(b => b.IsCurrentPlugin = true);
                    }
                }


                //if (SupportedPluginList != null)
                //    SupportedPluginList = new ObservableCollection<HardLockPlugins>(SupportedPluginList.GroupBy(opt => opt.IdPlugin).Select(g => g.First()));

                GeosApplication.Instance.Logger.Log("Method CompleteRecordDragDropFamily()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CompleteRecordDragDropFamily() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OnDragRecordOverSupportedPluginGrid(DragRecordOverEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverSupportedPluginGrid()...", category: Category.Info, priority: Priority.Low);


                if ((e.IsFromOutside) && typeof(HardLockPlugins).IsAssignableFrom(e.GetRecordType()))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
                SelectedHardLock = new HardLockPlugins();

                GeosApplication.Instance.Logger.Log("Method OnDragRecordOverSupportedPluginGrid()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OnDragRecordOverSupportedPluginGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteSupportedPluginCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteSupportedPluginCommandAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteSupportedPlugin"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (SupportedPluginDuplicateList.Contains(SelectedPlugin))
                    {
                        // IsDeleted = PCMService.DeleteSupportedPluginForHardLockLicense_V2450(SelectedPlugin.IdPlugin, SelectedReference.IdArticle);
                        DeletedPluginList.Add(SelectedPlugin);
                        HardLockPluginList.Where(a => a.IdPlugin == SelectedPlugin.IdPlugin).ToList().ForEach(b => b.IsCurrentPlugin = false);
                        SupportedPluginList.Remove(SelectedPlugin);
                        
                        SupportedPluginList = new ObservableCollection<HardLockPlugins>(SupportedPluginList);
                        SelectedPlugin = SupportedPluginList.FirstOrDefault();
                    }
                    else
                    {
                        HardLockPluginList.Where(a => a.IdPlugin == SelectedPlugin.IdPlugin).ToList().ForEach(b => b.IsCurrentPlugin = false);
                        SupportedPluginList.Remove(SelectedPlugin);
                        SupportedPluginList = new ObservableCollection<HardLockPlugins>(SupportedPluginList);
                        SelectedPlugin = SupportedPluginList.FirstOrDefault();
                    }


                }

                GeosApplication.Instance.Logger.Log("Method DeleteSupportedPluginCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteSupportedPluginCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedReference"));
                if (error != null)
                {
                    return;
                }
                if (IsNew)
                {
                    if (SupportedPluginList != null && SupportedPluginList.Count > 0)
                    {
                        IsSave = PCMService.AddHardLockLicense_V2450(SelectedReference.IdArticle, SupportedPluginList.ToList());
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("HardLockLicenseAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("HardLockLicenseAddedFailed").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    if (SupportedPluginList != null && SupportedPluginList.Count > 0)
                    {
                        var notsaved = SupportedPluginList.Except(SupportedPluginDuplicateList).ToList();
                        if (notsaved != null && notsaved.Count > 0)
                        {
                            IsSave = PCMService.AddHardLockLicense_V2450(SelectedReference.IdArticle, notsaved);
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("HardLockLicenseUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                        if (DeletedPluginList!=null&& DeletedPluginList.Count>0)
                        {
                            foreach (var item in DeletedPluginList)
                            {
                                IsDeleted = PCMService.DeleteSupportedPluginForHardLockLicense_V2450(item.IdPlugin, SelectedReference.IdArticle);
                            }
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("HardLockLicenseAddedFailed").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }

                }

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                RequestClose(null, null);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillSupportedPlugin(UInt32 idArticle)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillHardLockPlugin()...", category: Category.Info, priority: Priority.Low);
                #region old Service Commented
                //SupportedPluginList = new ObservableCollection<HardLockPlugins>(PCMService.GetSupportedPluginByIdArticle_V2450(idArticle));
                #endregion
                //[pramod.misal][GEOS2-5134][18-12-2023]
                SupportedPluginList = new ObservableCollection<HardLockPlugins>(PCMService.GetSupportedPluginByIdArticle_V2470(idArticle));

                SupportedPluginDuplicateList = new ObservableCollection<HardLockPlugins>(SupportedPluginList);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillHardLockPlugin()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSupportedPlugin() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSupportedPlugin() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillSupportedPlugin() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        //[Sudhir.Jangra][GEOS2-4915]
        private void AddNewPluginCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddHardLockLicenseButtonCommandAction().", category: Category.Info, priority: Priority.Low);
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

                AddEditPluginView addEditPluginView = new AddEditPluginView();
                AddEditPluginViewModel addEditPluginViewModel = new AddEditPluginViewModel();
                EventHandler handle = delegate { addEditPluginView.Close(); };
                addEditPluginViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddPluginHeader").ToString();
                addEditPluginViewModel.RequestClose += handle;
                addEditPluginView.DataContext = addEditPluginViewModel;
                addEditPluginViewModel.Init();
                addEditPluginView.ShowDialog();


                if (addEditPluginViewModel.IsSave)
                {

                    FillHardLockPlugin();
                    SelectedHardLock = HardLockPluginList.FirstOrDefault(x => x.Name == addEditPluginViewModel.Name);
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddHardLockLicenseButtonCommandAction(). executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddHardLockLicenseButtonCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        #region Validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
                return error;
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                                me[BindableBase.GetPropertyName(() => SelectedReference)];


                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;

                string selectedReferenceError = BindableBase.GetPropertyName(() => SelectedReference);


                if (columnName == selectedReferenceError)
                {
                    return AddEditHardLockLicenseValidation.GetErrorMessage(selectedReferenceError, SelectedReference.Reference);
                }



                return null;
            }
        }


        #endregion


    }
}
