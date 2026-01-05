using DevExpress.Mvvm;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.UI.CustomControls;
using System.ServiceModel;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
     public class TransferOfferViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region declaration 
        private string currentPlantString { get; set; }
        private List<UserSiteGeosServiceProvider> allPlantList { get; set; }
        private UserSiteGeosServiceProvider selectedAllPlant { get; set; }
        private UserSiteGeosServiceProvider currentPlant { get; set; }
        private bool isChecked;
        private bool isOfferSaved;
        private Offer selectedOffer;
        private bool isDeleteFromCurrentPlant;
        private Offer finalSavedOffer;
        #endregion

        #region properties
        public string CurrentPlantString
        {
            get
            {
                return currentPlantString;
            }
            set
            {
                currentPlantString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentPlantString"));
            }
        }

        public List<UserSiteGeosServiceProvider> AllPlantList
        {
            get
            {
                return allPlantList;
            }
            set
            {
                allPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllPlantList"));
            }
        }

        public UserSiteGeosServiceProvider SelectedAllPlant
        {
            get
            {
                return selectedAllPlant;
            }
            set
            {
                selectedAllPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAllPlant"));
            }
        }

        public UserSiteGeosServiceProvider CurrentPlant
        {
            get
            {
                return currentPlant;
            }
            set
            {
                currentPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentPlant"));
            }
        }

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsChecked"));
            }
        }

        public bool IsOfferSaved
        {
            get
            {
                return isOfferSaved;
            }
            set
            {
                isOfferSaved = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOfferSaved"));
            }
        }
        public bool IsDeleteFromCurrentPlant
        {
            get
            {
                return isDeleteFromCurrentPlant;
            }
            set
            {
                isDeleteFromCurrentPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleteFromCurrentPlant"));
            }
        }

        public Offer SelectedOffer
        {
            get
            {
                return selectedOffer;
            }
            set
            {
                selectedOffer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOffer"));
            }
        }

        public Offer FinalSavedOffer
        {
            get
            {
                return finalSavedOffer;
            }
            set
            {
                finalSavedOffer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FinalSavedOffer"));
            }
        }

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
        #endregion


        #region Commands
        public ICommand CloseWindowCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        #endregion

        #region Constructor

        public TransferOfferViewModel()
        {
            CloseWindowCommand = new DelegateCommand<object>(CloseWindowAction);
            AcceptButtonCommand = new DelegateCommand<object>(AcceptCommandAction);
        }



        #endregion

        #region methods

        public void InIt(Offer selectedOffer)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InIt()...", category: Category.Info, priority: Priority.Low);
                IsChecked = false;
                SelectedOffer = selectedOffer;
                CurrentPlantString = SelectedOffer.Site.ShortName;
                FillAllPlantList();
                GeosApplication.Instance.Logger.Log("Method InIt()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InIt()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for Close window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindowAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindowAction()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CloseWindowAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseWindowAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AcceptCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptCommandAction()...", category: Category.Info, priority: Priority.Low);
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

                IsOfferSaved = false;
                if (IsChecked == false) // cut from existing plant and paste in newly selected plant
                { 
                    CrmStartUp = new CrmServiceController(SelectedAllPlant.ServiceProviderUrl); //paste in newly selected plant
                    SelectedOffer.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    FinalSavedOffer = CrmStartUp.AddOffer_V2090(SelectedOffer, GeosApplication.Instance.ActiveIdSite, GeosApplication.Instance.ActiveUser.IdUser);

                    if (SelectedOffer.IdOffer == FinalSavedOffer.IdOffer)
                    {
                        // offer is not added
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TransferOfferIsNotAdded").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }

                    CrmStartUp = new CrmServiceController(CurrentPlant.ServiceProviderUrl); // delete from existing plant
                    IsDeleteFromCurrentPlant = CrmStartUp.IsDeletedOfferDetails(Convert.ToUInt64(SelectedOffer.IdOffer));

                    if(IsDeleteFromCurrentPlant == false)
                    {
                        //offer is not deleted
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TransferOfferIsNotDeleted").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }

                    IsOfferSaved = true;
                }
                else                // put Copy in existing plant and also paste in newly selected plant
                {
                    CrmStartUp = new CrmServiceController(SelectedAllPlant.ServiceProviderUrl); //paste in newly selected plant
                    SelectedOffer.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    FinalSavedOffer = CrmStartUp.AddOffer_V2090(SelectedOffer, GeosApplication.Instance.ActiveIdSite, GeosApplication.Instance.ActiveUser.IdUser);
                    if (SelectedOffer.IdOffer == FinalSavedOffer.IdOffer)
                    {
                        // offer is not added
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TransferOfferIsNotAdded").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }

                    IsOfferSaved = true;
                }

                if (IsOfferSaved == true)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TransferOfferTansferSuccessfullyMessage").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TransferOfferTansferFailedMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    RequestClose(null, null);
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AcceptCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            { 
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this method use for fill Site
        /// </summary>
        private void FillAllPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAllPlantList()...", category: Category.Info, priority: Priority.Low);

                if (CRMCommon.Instance.CommonAllPlantList == null || CRMCommon.Instance.CommonAllPlantList.Count == 0)
                {
                    CRMCommon.Instance.CommonAllPlantList = new List<UserSiteGeosServiceProvider>(CrmStartUp.GetAllCompaniesWithServiceProvider(GeosApplication.Instance.ActiveUser.IdUser));
                }

                AllPlantList = CRMCommon.Instance.CommonAllPlantList;
                AllPlantList.Remove(AllPlantList.FirstOrDefault(x => x.ShortName == SelectedOffer.Site.ShortName));
                SelectedAllPlant = AllPlantList.FirstOrDefault();
                CurrentPlant = AllPlantList.Where(x => x.ShortName == SelectedOffer.Site.ShortName).FirstOrDefault();

                GeosApplication.Instance.Logger.Log("Method FillAllPlantList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAllPlantList() " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAllPlantList() " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAllPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
