using DevExpress.Mvvm;
using DevExpress.Utils;
using DevExpress.Xpf;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraScheduler.Outlook.Native;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Modules.OTM.CommonClass;
using Emdep.Geos.Modules.OTM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Utility;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.OTM.ViewModels
{
    public class POEmailConfirmationViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {

        #region TaskLog
        /// <summary>
        /// //[pramod.misal][GEOS2-6465][16.12.2024]
        /// </summary>
        /// </summary>
        #endregion

        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IOTMService OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IOTMService OTMService = new OTMServiceController("localhost:6699");

        #endregion

        #region ICommands

        public ICommand POEmailConfirmationViewCancelButtonCommand { get; set; }
        public ICommand DeleteToContactButtonCommand { get; set; }
        public ICommand DeleteCcContactButtonCommand { get; set; }
        public ICommand POEmailConfirmationViewAcceptButtonCommand { get; set; }
        //public ICommand AddToButtonCommand { get; set; }
        //public ICommand AddCCButtonCommand { get; set; }
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

        #endregion

        #region Declaration
        private ObservableCollection<Language> languages;
        private string settingWindowLanguageSelectedIndex;
        private List<People> toContactList;
        private List<People> ccContactList;
        private string htmlEmailtemplate;
        private string informationError;
        bool isBusy;
        private int selectedIndexTOType;
        private PORegisteredDetails poregistereddetailsforemail;
        private ObservableCollection<People> customerList;

        //private List<Tuple<int, int>> editableRanges;
        //[Rahul.Gadhave][GEOS2-7079][Date:20-03-2025]
        List<People> excludedContactList;
        List<People> excludedCCContactList;
        ObservableCollection<People> includedContactList;
        #endregion

        #region Properties

        //public List<Tuple<int, int>> EditableRanges
        //{
        //    get => editableRanges;
        //    set
        //    {
        //        editableRanges = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("EditableRanges"));
        //    }
        //}
        public int SelectedIndexTOType
        {
            get { return selectedIndexTOType; }
            set
            {
                selectedIndexTOType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexPoType"));
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
        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }
        public PORegisteredDetails Poregistereddetailsforemail
        {
            get { return poregistereddetailsforemail; }
            set
            {
                poregistereddetailsforemail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Poregistereddetailsforemail"));
            }
        }
        public string HtmlEmailtemplate
        {
            get => htmlEmailtemplate;
            set
            {
                if (htmlEmailtemplate != value)
                {
                    htmlEmailtemplate = value;
                    
                    OnPropertyChanged(new PropertyChangedEventArgs("HtmlEmailtemplate"));
                }
            }
        }
        public ObservableCollection<Language> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Languages"));
            }
        }


        public string SettingWindowLanguageSelectedIndex
        {
            get { return settingWindowLanguageSelectedIndex; }
            set
            {

                settingWindowLanguageSelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SettingWindowLanguageSelectedIndex"));
                GetEmaillanguagesWiseTemplates(Poregistereddetailsforemail);



            }
        }

        public List<People> ToContactList
        {
            get { return toContactList; }
            set
            {
                toContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToContactList"));

                if (toContactList.Count == 0)
                {
                    SelectedIndexTOType = -1;
                }
            }
        }


        public List<People> CcContactList
        {
            get { return ccContactList; }
            set
            {
                ccContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CcContactList"));
            }
        }

        
        public ObservableCollection<People> CustomerList
        {
            get { return customerList; }
            set
            {
                customerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerList"));
            }
        }
        //[Rahul.Gadhave][GEOS2-7079][Date:20-03-2025]
        public List<People> ExcludedContactList
        {
            get { return excludedContactList; }
            set
            {
                excludedContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExcludedContactList"));
            }
        }
        //[Rahul.Gadhave][GEOS2-7079][Date:20-03-2025]
        public List<People> ExcludedCCContactList
        {
            get { return excludedCCContactList; }
            set
            {
                excludedCCContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExcludedCCContactList"));
            }
        }
        //[Rahul.Gadhave][GEOS2-7079][Date:20-03-2025]
        private List<Object> selectedOffer;
        public List<Object> SelectedOffer
        {
            get { return selectedOffer; }
            set
            {
                selectedOffer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOffer"));
            }
        }
        //[Rahul.Gadhave][GEOS2-7079][Date:20-03-2025]
        private List<Object> selectedCcList;
        public List<Object> SelectedCcList
        {
            get { return selectedCcList; }
            set
            {
                selectedCcList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCcList"));
            }
        }
        #endregion

        #region Constructor
        public POEmailConfirmationViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor POEmailConfirmationViewModel...", category: Category.Info, priority: Priority.Low);

                POEmailConfirmationViewCancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                DeleteToContactButtonCommand = new RelayCommand(new Action<object>(DeleteToContactButtonCommandAction));
                DeleteCcContactButtonCommand = new RelayCommand(new Action<object>(DeleteCcContactButtonCommandAction));
                POEmailConfirmationViewAcceptButtonCommand = new DelegateCommand<object>(POEmailConfirmationAction);
                //AddToButtonCommand = new RelayCommand(new Action<object>(AddToButtonCommandAction));
                //AddCCButtonCommand = new RelayCommand(new Action<object>(AddCCButtonCommandAction));
                GeosApplication.Instance.Logger.Log("Constructor POEmailConfirmationViewModel() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                
                GeosApplication.Instance.Logger.Log("Get an error in Method POEmailConfirmationViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Method

        //[pramod.nisal][GEOS2-6463][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        //public void AddToButtonCommandAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method AddToButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

        //        if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
        //        {
        //            DXSplashScreen.Show(x =>
        //            {
        //                Window win = new Window()
        //                {
        //                    ShowActivated = false,
        //                    WindowStyle = WindowStyle.None,
        //                    ResizeMode = ResizeMode.NoResize,
        //                    AllowsTransparency = true,
        //                    Background = new SolidColorBrush(Colors.Transparent),
        //                    ShowInTaskbar = false,
        //                    Topmost = true,
        //                    SizeToContent = SizeToContent.WidthAndHeight,
        //                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
        //                };
        //                DevExpress.Mvvm.UI.WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
        //                win.Topmost = false;
        //                return win;
        //            }, x =>
        //            {
        //                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
        //            }, null, null);
        //        }

        //        AddToAndCcEmailView addToAndCcEmailView = new AddToAndCcEmailView();
        //        AddToAndCcEmailViewModel addToAndCcEmailViewModel = new AddToAndCcEmailViewModel();
        //        EventHandler handle = delegate { addToAndCcEmailView.Close(); };
        //        addToAndCcEmailViewModel.RequestClose += handle;

        //        addToAndCcEmailViewModel.Init(CustomerList,ToContactList);
        //        addToAndCcEmailView.DataContext = addToAndCcEmailViewModel;
        //        //if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        addToAndCcEmailView.ShowDialog();
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

        //        if (addToAndCcEmailViewModel.IsSave)
        //        {
                    
        //            ToContactList = new ObservableCollection<People>(addToAndCcEmailViewModel.IncludedContactList);
        //        }
        //        //if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Method AddToButtonCommandAction()...Executed", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in Method AddToButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}


        //[pramod.nisal][GEOS2-6463][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        //public void AddCCButtonCommandAction(object gcComments)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method AddCCButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

        //        if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
        //        {
        //            DXSplashScreen.Show(x =>
        //            {
        //                Window win = new Window()
        //                {
        //                    ShowActivated = false,
        //                    WindowStyle = WindowStyle.None,
        //                    ResizeMode = ResizeMode.NoResize,
        //                    AllowsTransparency = true,
        //                    Background = new SolidColorBrush(Colors.Transparent),
        //                    ShowInTaskbar = false,
        //                    Topmost = true,
        //                    SizeToContent = SizeToContent.WidthAndHeight,
        //                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
        //                };
        //                DevExpress.Mvvm.UI.WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
        //                win.Topmost = false;
        //                return win;
        //            }, x =>
        //            {
        //                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
        //            }, null, null);
        //        }

        //        AddToAndCcEmailView addToAndCcEmailView = new AddToAndCcEmailView();
        //        AddToAndCcEmailViewModel addToAndCcEmailViewModel = new AddToAndCcEmailViewModel();
        //        EventHandler handle = delegate { addToAndCcEmailView.Close(); };
        //        addToAndCcEmailViewModel.RequestClose += handle;

        //        addToAndCcEmailViewModel.Init(CustomerList, CcContactList);
        //        addToAndCcEmailView.DataContext = addToAndCcEmailViewModel;
        //        //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        addToAndCcEmailView.ShowDialog();
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        if (addToAndCcEmailViewModel.IsSave)
        //        {
        //            CcContactList = new ObservableCollection<People>(addToAndCcEmailViewModel.IncludedContactList);
        //        }

        //        GeosApplication.Instance.Logger.Log("Method AddCCButtonCommandAction()...Executed", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in Method AddCCButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void DeleteCcContactButtonCommandAction(object obj)
        {
            try
            {
                People con = obj as People;
                CcContactList.Remove(con);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteOrdersToButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void DeleteToContactButtonCommandAction(object obj)
        {
            try
            {
                People con = obj as People;
                ToContactList.Remove(con);
                if (ToContactList.Count ==0)
                {
                    SelectedIndexTOType = -1;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteOrdersToButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-6465][16.12.2024]
        private void POEmailConfirmationAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("POEmailConfirmationAction...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                InformationError = null;
                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedOffer"));
                if (error != null)
                {
                    IsBusy = false;
                    return;
                }
                var richEdit = obj as RichEditControl;
                try
                {
                    GeosApplication.Instance.Logger.Log("POEmailConfirmationAction() Method - RichEditControl Started", category: Category.Info, priority: Priority.Low);
                    if (richEdit != null)
                    {
                        richEdit.Options.Export.Html.EmbedImages = true;
                        // Get updated HTML content
                        // Export the document content to HTML
                        string updatedHtmlContent;
                        using (var memoryStream = new MemoryStream())
                        {
                            richEdit.SaveDocument(memoryStream, DevExpress.XtraRichEdit.DocumentFormat.Html);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            using (var reader = new StreamReader(memoryStream))
                            {
                                updatedHtmlContent = reader.ReadToEnd();
                            }
                        }
                        HtmlEmailtemplate = updatedHtmlContent;
                    }


                    GeosApplication.Instance.Logger.Log("Method - RichEditControl completed", category: Category.Info, priority: Priority.Low);
                    GeosApplication.Instance.Logger.Log("HtmlEmailtemplate Started " +HtmlEmailtemplate.ToString(), category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in POEmailConfirmationAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                string fName = GeosApplication.Instance.ActiveUser.FirstName;
                string Lname = GeosApplication.Instance.ActiveUser.LastName;
                string fromMail = fName + " " + Lname;
                string EmailSubject = "Emdep Purchase Order Confirmation " + Poregistereddetailsforemail.Code + " " + "[" + Poregistereddetailsforemail.Group + " -" + Poregistereddetailsforemail.Plant + "(" + Poregistereddetailsforemail.Country + ")" + "/" + Poregistereddetailsforemail.LinkedOffer + "]";

                // Get the RichEditControl instance and extract updated HTML content
                //bool IsEmailSend = OTMService.POEmailSend_V2590(EmailSubject, HtmlEmailtemplate.ToString(), ToContactList, CcContactList, fromMail, null);
                ToContactList = SelectedOffer.Cast<People>().ToList();
                CcContactList = SelectedCcList.Cast<People>().ToList();

                try
                {
                    if (OTMCommon.Instance.SelectedPlantForRegisteredPO == null)
                    {
                        string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                        Data.Common.Company selectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                        OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                    }
                    else
                    {
                        Data.Common.Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedPlantForRegisteredPO.IdCompany);
                        OTMService = new OTMServiceController((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                            selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in POEmailConfirmationAction() Method - ServiceProviderUrl " + ex.Message, category: Category.Exception, priority: Priority.Low);

                }
                //OTMService = new OTMServiceController("localhost:6699");
                if (HtmlEmailtemplate != null && fromMail!= null && ToContactList!=null)
                {
                    try
                    {
                        //[Rahul.Gadhave][GEOS2-7079][Date:25-03-2025]
                        bool IsEmailSend =  OTMService.POEmailSend_V2630(EmailSubject, HtmlEmailtemplate.ToString(), ToContactList, CcContactList, fromMail, null);
                        if (IsEmailSend)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PoconfirmationSendMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            RequestClose(null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in POEmailConfirmationAction() Method - POEmailSend_V2630 " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
                GeosApplication.Instance.Logger.Log("Constructor POEmailConfirmationAction() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method POEmailConfirmationAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method POEmailConfirmationAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in POEmailConfirmationAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
          

        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        public void InIt(PORegisteredDetails poregistereddetailsforemail)
        {
            if (poregistereddetailsforemail != null)
            Poregistereddetailsforemail = poregistereddetailsforemail;
            SettingWindowLanguageSelectedIndex = "0";
            FillCustomers(poregistereddetailsforemail);
            FillExcludedCCContactList();
            FillLanguage();
            FillToContactList(poregistereddetailsforemail);
            FillCcContactList(poregistereddetailsforemail);
            
            //GetEmaillanguagesWiseTemplates();
        }
        //[pramod.misal][GEOS2-6465][16.12.2024]
        private void FillCustomers(PORegisteredDetails poregistereddetailsforemail)
        {
            try
            { 
                GeosApplication.Instance.Logger.Log("Getting All CcContact on list - FillCustomers()", category: Category.Info, priority: Priority.Low);
                //CustomerList = new ObservableCollection<People>(OTMService.GetPeopleByEMDEPcustomer_V2590());
                //OTMService = new OTMServiceController("localhost:6699");
                CustomerList = new ObservableCollection<People>(OTMService.GetPeopleByEMDEPcustomer_V2630(poregistereddetailsforemail));
                ExcludedContactList = new List<People>(CustomerList);
                GeosApplication.Instance.Logger.Log("Getting All CcContact on list successfully - FillCustomers()", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomers() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomers() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCustomers() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }
        //[Rahul.Gadhave][GEOS2-7079][Date:25-03-2025]
        private void FillExcludedCCContactList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Getting All CcContact on list - FillExcludedCCContactList()", category: Category.Info, priority: Priority.Low);
                long plantIds;
                plantIds = OTMCommon.Instance.SelectedPlantForRegisteredPO.IdCompany;
                //OTMService = new OTMServiceController("localhost:6699");
                GeosAppSetting CustomerPOConfirmationJD = OTMService.GetGeosAppSettings(146);
                ExcludedCCContactList = new List<People>(OTMService.GetPeopleByJobDescriptions_V2630(CustomerPOConfirmationJD, plantIds));
                GeosApplication.Instance.Logger.Log("Getting All CcContact on list successfully - FillExcludedCCContactList()", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExcludedCCContactList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExcludedCCContactList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillExcludedCCContactList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }
        //[pramod.misal][GEOS2-6465][16.12.2024]
        private void FillCcContactList(PORegisteredDetails poregistereddetailsforemail)
        {
            try
            {   //GeosApplication.Instance.ActiveUser.IdUser
                Int64 SelectedIdPo = poregistereddetailsforemail.IdPO;
                GeosApplication.Instance.Logger.Log("Getting All CcContact on list - FillCcContactList()", category: Category.Info, priority: Priority.Low);
                if (OTMCommon.Instance.SelectedPlantForRegisteredPO == null)
                {
                    string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                    Data.Common.Company selectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                    OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                }
                else
                {
                    Data.Common.Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedPlantForRegisteredPO.IdCompany);
                    OTMService = new OTMServiceController((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                        selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                }
                //OTMService = new OTMServiceController("localhost:6699");
                //var TempCcContactList = OTMService.GetPOReceptionEmailCCFeilds_V2590(SelectedIdPo) ?? new List<People>();
                //[Rahul.Gadhave][GEOS2-9113][Date:01-08-2025]
                var TempCcContactList = OTMService.GetPOReceptionEmailCCFeilds_V2660(SelectedIdPo) ?? new List<People>();
                //var LoginUserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName;
                //CcContactList = new ObservableCollection<People>(TempCcContactList);
                CcContactList = new List<People>(TempCcContactList.Where(person => person.Email != null));
                var activeUserId = GeosApplication.Instance.ActiveUser.IdUser;
                if (!CcContactList.Any(person => person.IdPerson == activeUserId))
                {
                    // Add the active user's ID
                    CcContactList.Add(new People
                    {
                        IdPerson = GeosApplication.Instance.ActiveUser.IdUser,
                        Name = GeosApplication.Instance.ActiveUser.FirstName,
                        Surname = GeosApplication.Instance.ActiveUser.LastName,
                        Email = GeosApplication.Instance.ActiveUser.CompanyEmail
                    });
                }
                // Ensure unique names again after adding the active user
                CcContactList = new List<People>(
                    CcContactList
                    .GroupBy(person => person.FullName)
                    .Select(group => group.First())
                );
                //[Rahul.Gadhave][GEOS2-7079][Date:25-03-2025]
                foreach (var person in CcContactList)
                {
                    if (!ExcludedCCContactList.Any(x => x.IdPerson == person.IdPerson))
                    {
                        ExcludedCCContactList.Add(person);
                    }
                }
                if (CcContactList != null)
                {
                    SelectedCcList = new List<object>();
                    foreach (People item in CcContactList)
                    {
                        SelectedCcList.Add(ExcludedCCContactList.FirstOrDefault(x => x.IdPerson == item.IdPerson));
                    }
                }
                GeosApplication.Instance.Logger.Log("Getting All CcContact on list successfully - FillCcContactList()", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCcContactList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCcContactList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCcContactList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-6465][16.12.2024]
        private void FillToContactList(PORegisteredDetails poregistereddetailsforemail)
        {
            try
            {
                Int64 SelectedIdPo = poregistereddetailsforemail.IdPO;
                GeosApplication.Instance.Logger.Log("Getting All ToContact on list - FillToContactList()", category: Category.Info, priority: Priority.Low);
                if (OTMCommon.Instance.SelectedPlantForRegisteredPO == null)
                {
                    string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                    Data.Common.Company selectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                    OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                }
                else
                {
                    Data.Common.Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedPlantForRegisteredPO.IdCompany);
                    OTMService = new OTMServiceController((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                        selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                }
                //OTMService = new OTMServiceController("localhost:6699");
                var TemoToContactList = new List<People>(OTMService.GetPOReceptionEmailToFeilds_V2590(SelectedIdPo));
                var uniqueContacts = TemoToContactList.GroupBy(person => person.FullName).Select(group => group.First()).ToList();
                ToContactList = new List<People>(uniqueContacts);
                //[Rahul.Gadhave][GEOS2-7079][Date:25-03-2025]
                foreach (var person in ToContactList)
                {
                    if (!ExcludedContactList.Any(x => x.IdPerson == person.IdPerson))
                    {
                        ExcludedContactList.Add(person);
                    }
                }
                if (ToContactList != null && ExcludedContactList!=null)
                {
                    SelectedOffer = new List<object>();
                    foreach (People item in ToContactList)
                    {
                        SelectedOffer.Add(ExcludedContactList.FirstOrDefault(x => x.IdPerson == item.IdPerson));
                    }
                }

                GeosApplication.Instance.Logger.Log("Getting All ToContact on list successfully - FillToContactList()", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillToContactList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillToContactList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillToContactList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        //[pramod.misal][GEOS2-6465][16.12.2024]
        private void GetEmaillanguagesWiseTemplates(PORegisteredDetails Poregistereddetailsforemail)
        {
            try
            {
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window
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
                        DevExpress.Mvvm.UI.WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x => new SplashScreenView { DataContext = new SplashScreenViewModel() }, null, null);
                }

                
                if (OTMCommon.Instance.SelectedPlantForRegisteredPO == null)
                {
                    string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                    Data.Common.Company selectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                    OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                }
                else
                {
                    Data.Common.Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedPlantForRegisteredPO.IdCompany);
                    OTMService = new OTMServiceController((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                        selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                }
                //OTMService = new OTMServiceController("localhost:6699");
                //string selectedPlantName = OTMCommon.Instance.SelectedSinglePlant.Alias.ToLower() ;
                string selectedPlantName = OTMCommon.Instance.SelectedPlantForRegisteredPO.Alias.ToLower();
                if (SettingWindowLanguageSelectedIndex == "0")
                {
                    HtmlEmailtemplate = OTMService.ReadMailTemplate("CustomerPurchaseOrderReceptionConfirmationMailFormat.html");
                    OTMCommon.Instance.SettingWindowLanguageSelectedIndex = "0";
                }
                else if (SettingWindowLanguageSelectedIndex == "1")
                {
                    HtmlEmailtemplate = OTMService.ReadMailTemplate("CustomerPurchaseOrderReceptionConfirmationMailFormat_es.html");
                    OTMCommon.Instance.SettingWindowLanguageSelectedIndex = "1";
                }
                else if (SettingWindowLanguageSelectedIndex == "2")
                {
                    HtmlEmailtemplate = OTMService.ReadMailTemplate("CustomerPurchaseOrderReceptionConfirmationMailFormat_fr.html");
                    OTMCommon.Instance.SettingWindowLanguageSelectedIndex = "2";
                }
                else if (SettingWindowLanguageSelectedIndex == "3")
                {
                    HtmlEmailtemplate = OTMService.ReadMailTemplate("CustomerPurchaseOrderReceptionConfirmationMailFormat_pt.html");
                    OTMCommon.Instance.SettingWindowLanguageSelectedIndex = "3";
                }
                else if (SettingWindowLanguageSelectedIndex == "4")
                {
                    HtmlEmailtemplate = OTMService.ReadMailTemplate("CustomerPurchaseOrderReceptionConfirmationMailFormat_ro.html");
                    OTMCommon.Instance.SettingWindowLanguageSelectedIndex = "4";
                }
                else if (SettingWindowLanguageSelectedIndex == "5")
                {
                    HtmlEmailtemplate = OTMService.ReadMailTemplate("CustomerPurchaseOrderReceptionConfirmationMailFormat_ru.html");
                    OTMCommon.Instance.SettingWindowLanguageSelectedIndex = "5";
                }
                else if (SettingWindowLanguageSelectedIndex == "6")
                {
                    HtmlEmailtemplate = OTMService.ReadMailTemplate("CustomerPurchaseOrderReceptionConfirmationMailFormat_ch.html");
                    OTMCommon.Instance.SettingWindowLanguageSelectedIndex = "6";
                }

                if (!string.IsNullOrEmpty(HtmlEmailtemplate))
                {
                    HtmlEmailtemplate = HtmlEmailtemplate.Replace("[CUSTOMER]", Poregistereddetailsforemail.Group +" -" + Poregistereddetailsforemail.Plant +"("+ Poregistereddetailsforemail.Country+")")
                   .Replace("[CONTACT]", Poregistereddetailsforemail.Sender)
                   .Replace("[PO_NUMBER]", Poregistereddetailsforemail.Code)
                   .Replace("[PO_RECEPTION_DATE]", Poregistereddetailsforemail.ReceptionDate.Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture))
                   .Replace("[ORDER]", Poregistereddetailsforemail.LinkedOffer)
                   .Replace("[PLANT]", selectedPlantName);

                }
               
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetEmaillanguagesWiseTemplates() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
        }
        //[pramod.misal][GEOS2-6465][16.12.2024]
        private void FillLanguage()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Getting All Language on list - FillLanguage()", category: Category.Info, priority: Priority.Low);
                //OTMService = new OTMServiceController("localhost:6699");
                if (OTMCommon.Instance.SelectedPlantForRegisteredPO == null)
                {
                    string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                    Data.Common.Company selectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                    OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                }
                else
                {
                    Data.Common.Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedPlantForRegisteredPO.IdCompany);
                    OTMService = new OTMServiceController((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                        selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                }
                Languages = new ObservableCollection<Language>(OTMService.GetAllLanguages_V2590());
                GeosApplication.Instance.Logger.Log("Getting All Language on list successfully - FillLanguage()", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguage() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        //[pramod.misal][GEOS2-6465][16.12.2024]
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        public void Dispose()
        {

        }

        #endregion

        #region Validation
        bool allowValidation = false;

        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => SelectedOffer)];

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
                string TOType = BindableBase.GetPropertyName(() => SelectedOffer);


                if (columnName == TOType)
                {
                    if (SelectedOffer == null || !SelectedOffer.Any()) // Check if the list is empty
                    {
                        return PoEmailEditValidation.GetErrorMessage(TOType, SelectedOffer);
                    }

                }

                return null;
            }
        }



        #endregion




    }
}
