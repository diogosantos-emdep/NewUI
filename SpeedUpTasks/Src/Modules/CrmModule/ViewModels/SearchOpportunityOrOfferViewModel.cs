using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class SearchOpportunityOrOfferViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        List<TimelineGrid> timelineGridList;
        TimelineGrid selectedTimeline;
        string code;
        private string visible;
        #endregion

        #region Properties
        public List<TimelineGrid> TimelineGridList
        {
            get
            {
                return timelineGridList;
            }

            set
            {
                timelineGridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimelineGridList"));
            }
        }

        public TimelineGrid SelectedTimeline
        {
            get
            {
                return selectedTimeline;
            }

            set
            {
                selectedTimeline = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTimeline"));
            }
        }
        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
            }
        }

        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
        }
        #endregion

        #region ICommand
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand EditOpportunityOrOrderCommand { get; set; }

        public ICommand SearchButtonCommand { get; set; }

        public ICommand CommandTextInput { get; set; }

        #endregion

        #region Constructor

        public SearchOpportunityOrOfferViewModel()
        {
            EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
            CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
            EditOpportunityOrOrderCommand = new DelegateCommand<object>(EditOpportunityOrOrder);
            SearchButtonCommand = new DelegateCommand<object>(SearchOpportunityOrOrder);
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
            //FillTimelineList();
            TimelineGridList = new List<TimelineGrid>();

            //set hide/show shortcuts on permissions
            Visible = Visibility.Visible.ToString();
            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                Visible = Visibility.Hidden.ToString();
            }
            else
            {
                Visible = Visibility.Visible.ToString();
            }
        }


        #endregion

        #region Methods

        public void Init()
        {
        }

        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SearchOpportunityOrOrder(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SearchOpportunityOrOrder()...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
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

                    FillLeadsByUser();

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                }
                GeosApplication.Instance.Logger.Log("Method SearchOpportunityOrOrder()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SearchOpportunityOrOrder() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillLeadsByUser()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLeadsByUser ...", category: Category.Info, priority: Priority.Low);
                List<TimelineGrid> AllTimelineGridList = new List<TimelineGrid>();

                OffersOptionsList offersOptionsLst = new OffersOptionsList();

                #region Roll 22

                if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                    {
                        List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                        var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ShortName));

                        foreach (var itemPlantOwnerUsers in plantOwners)
                        {
                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.ShortName;

                                List<TimelineGrid> timelineGridListByPlant = new List<TimelineGrid>();

                                TimelineParams objTimelineParams = new TimelineParams();
                                objTimelineParams.idCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                                objTimelineParams.idUser = GeosApplication.Instance.ActiveUser.IdUser;
                                objTimelineParams.idsSelectedUser = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                                objTimelineParams.idZone = GeosApplication.Instance.ActiveUser.Company.Country.IdZone;
                                //[001] Added 
                                objTimelineParams.activeSite = new ActiveSite { IdSite = Convert.ToInt32(itemPlantOwnerUsers.ConnectPlantId), SiteAlias = Convert.ToString(itemPlantOwnerUsers.Alias), SiteServiceProvider = itemPlantOwnerUsers.ServiceProviderUrl };
                                objTimelineParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                                objTimelineParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                                objTimelineParams.Roles = RoleType.SalesGlobalManager;

                                if (Code == null) { Code = ""; }
                                timelineGridListByPlant = CRMService.GetOfferAndOrderDetailsByOfferCode(Code, objTimelineParams).ToList();

                                AllTimelineGridList.AddRange(timelineGridListByPlant);
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in FillLeadsByUser() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in FillLeadsByUser() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillLeadsByUser() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }

                        TimelineGridList = new List<TimelineGrid>(AllTimelineGridList);
                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    }
                }
                #endregion

                GeosApplication.Instance.Logger.Log("Method FillLeadsByUser() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadsByUser() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillLeadsByUser() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error on FillLeadsByUser() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }




        private void EditOpportunityOrOrder(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditOpportunityOrOrder...", category: Category.Info, priority: Priority.Low);

                if (SelectedTimeline != null)
                {
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        // DXSplashScreen.Show<SplashScreenView>(); 
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

                    string offerCode = SelectedTimeline.Code;
                    long offerId = SelectedTimeline.IdOffer;
                    Int32 ConnectPlantId = SelectedTimeline.ConnectPlantId;
                    ActiveSite offerActiveSite = SelectedTimeline.ActiveSite;
                    IList<Offer> TempLeadsList = new List<Offer>();
                    //[001] added Change Method
                    ICrmService CrmStartUpOfferActiveSite = new CrmServiceController(offerActiveSite.SiteServiceProvider);
                    //[001] Changed service method and controller
                    TempLeadsList.Add(CrmStartUpOfferActiveSite.GetOfferDetailsById_V2040(offerId, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, offerActiveSite));
                    //[001] Changed service method and controller
                    LostReasonsByOffer lostReasonsByOffer = CrmStartUpOfferActiveSite.GetLostReasonsByOffer_V2040(offerId);

                    if (lostReasonsByOffer != null)
                    {
                        TempLeadsList[0].LostReasonsByOffer = lostReasonsByOffer;
                    }

                    LeadsEditViewModel leadsEditViewModel = new LeadsEditViewModel();
                    LeadsEditView leadsEditView = new LeadsEditView();

                    if (GeosApplication.Instance.IsPermissionReadOnly)
                    {
                        leadsEditViewModel.IsControlEnable = true;
                        //leadsEditViewModel.IsControlEnableorder = false;
                        leadsEditViewModel.IsStatusChangeAction = true;
                        leadsEditViewModel.IsAcceptControlEnableorder = false;

                        //[CRM-M040-02] (#49016) Validate Eng. Analysis 
                        //If user has engineering permission then he can edit eng analysis IsCompleted not other fields and save
                        leadsEditViewModel.IsAcceptEnable = false;
                        if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 27))
                        {
                            leadsEditViewModel.IsAcceptEnable = true;
                        }

                        leadsEditViewModel.InItLeadsEditReadonly(TempLeadsList);
                    }
                    else
                    {
                        leadsEditViewModel.ForLeadOpen = true;
                        leadsEditViewModel.InIt(TempLeadsList);
                    }

                    EventHandler handle = delegate { leadsEditView.Close(); };
                    leadsEditViewModel.RequestClose += handle;
                    leadsEditView.DataContext = leadsEditViewModel;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }

                    leadsEditView.ShowDialogWindow();
                }
                GeosApplication.Instance.Logger.Log("Method EditOpportunityOrOrder... executed", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditOpportunityOrOrder() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditOpportunityOrOrder() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditOpportunityOrOrder() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
                {
                    return;
                }
                CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

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
