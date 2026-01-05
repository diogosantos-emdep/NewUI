using DevExpress.Mvvm;
using DevExpress.Xpf.Grid.DragDrop;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System.Data;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class LeadsPipelineViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        public List<Offer> LeadsList;
        private Color hoverBackground;
        private Color selectedBackground;
        private int selectedStarIndex;
        private bool isLostCanceled;
        private string leadsEditViewCloseDate;
        private ObservableCollection<SalesStatusType> listSalesStatusType = new ObservableCollection<SalesStatusType>();

        private string pipelineFilterCriteria;

        List<string> failedPlants;
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;

        private List<LogEntryByOffer> changeLogsEntry;
        private List<SalesStatusType> copyPipelineMainSalesStatusTypeList;
        private List<SalesStatusType> pipelineMainSalesStatusTypeList3;
        private bool isOldOT;
        private long OfferNumber;
        private string offerCode;
        SalesStatusType salesStatusType;
        private bool isCodeUpdated = false;

        //Double max_Value_TaskSuggestion;
        private List<ActivityTemplateTrigger> activityTemplateTriggers;

        private bool isFromRefreshOrSalesOwner = false;

        #endregion // Declaration

        #region Public Properties

        public IList<Currency> Currencies { get; set; }

        /// <summary>
        /// Max_Value_TaskSuggestion for Offer Amount to show Task suggestion.
        /// </summary>
        //public Double Max_Value_TaskSuggestion
        //{
        //    get { return max_Value_TaskSuggestion; }
        //    set
        //    {
        //        max_Value_TaskSuggestion = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("Max_Value_TaskSuggestion"));
        //    }
        //}

        public List<SalesStatusType> CopyPipelineMainSalesStatusTypeList
        {
            get { return copyPipelineMainSalesStatusTypeList; }
            set { copyPipelineMainSalesStatusTypeList = value; }
        }

        public List<SalesStatusType> PipelineMainSalesStatusTypeList3
        {
            get { return pipelineMainSalesStatusTypeList3; }
            set { pipelineMainSalesStatusTypeList3 = value; }
        }

        public string PipelineFilterCriteria
        {
            get { return pipelineFilterCriteria; }
            set
            {
                if (value == null)
                    pipelineFilterCriteria = "";
                else
                    pipelineFilterCriteria = value;

                OnPropertyChanged(new PropertyChangedEventArgs("PipelineFilterCriteria"));

                try
                {
                    string _tempPipelineFilterCriteria = pipelineFilterCriteria;
                    //if Filter string is null or Empty
                    if (string.IsNullOrWhiteSpace(pipelineFilterCriteria))
                    {

                        foreach (SalesStatusType item in ListSalesStatusType)
                        {
                            SalesStatusType copySalesStatusType = CopyPipelineMainSalesStatusTypeList.FirstOrDefault(x => x.IdSalesStatusType == item.IdSalesStatusType);
                            if (copySalesStatusType.OffersInObject != null)
                                item.OffersInObject = new ObservableCollection<Offer>(((List<Offer>)copySalesStatusType.OffersInObject).ToList());
                            item.TotalAmount = copySalesStatusType.TotalAmount;
                        }
                    }
                    else
                    {
                        CreateSeperateLists();
                        foreach (SalesStatusType item in ListSalesStatusType)
                        {
                            SalesStatusType copySalesStatusType = CopyPipelineMainSalesStatusTypeList.FirstOrDefault(x => x.IdSalesStatusType == item.IdSalesStatusType);
                            item.OffersInObject = new ObservableCollection<Offer>(((List<Offer>)copySalesStatusType.OffersInObject).Where(x => (x.Site != null && x.Site.Customers != null && x.Site.Customers.Count > 0 && x.Site.Customers[0].CustomerName.ToUpper().Contains(_tempPipelineFilterCriteria.Trim().ToUpper()))
                                                                                                                                                || (x.Code != null && x.Code.ToUpper().Contains(_tempPipelineFilterCriteria.Trim().ToUpper()))
                                                                                                                                                || (x.Site != null && x.Site.Name != null && x.Site.Name.ToUpper().Contains(_tempPipelineFilterCriteria.Trim().ToUpper()))
                                                                                                                                                || (x.Description != null && x.Description.ToUpper().Contains(_tempPipelineFilterCriteria.Trim().ToUpper()))
                                ).ToList());

                            item.TotalAmount = Math.Round(((ObservableCollection<Offer>)item.OffersInObject).Select(v => v.Value).Sum(), 2);
                        }

                        _tempPipelineFilterCriteria = "";
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(" Get an error in Property PipelineFilterCriteria " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }
        public string PreviouslySelectedSalesOwners { get; set; }
        public string PreviouslySelectedPlantOwners { get; set; }

        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }

        public Boolean IsShowFailedPlantWarning
        {
            get { return isShowFailedPlantWarning; }
            set
            {
                isShowFailedPlantWarning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsShowFailedPlantWarning"));
            }
        }

        public string WarningFailedPlants
        {
            get { return warningFailedPlants; }
            set
            {
                warningFailedPlants = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarningFailedPlants"));
            }
        }

        public ObservableCollection<Offer> LeadsPipelineList { get; set; }
        public double TotalOnlyQuatedOffer { get; set; }
        public double TotalWaitingForInvoiceOffer { get; set; }
        public double TotalInProductionOffer { get; set; }
        public virtual Color Background { get; set; }

        public ObservableCollection<SalesStatusType> ListSalesStatusType
        {
            get { return listSalesStatusType; }
            set
            {
                SetProperty(ref listSalesStatusType, value, () => ListSalesStatusType);
                //listSalesStatusType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListSalesStatusType"));
            }
        }

        public List<LogEntryByOffer> ChangeLogsEntry
        {
            get { return changeLogsEntry; }
            set { changeLogsEntry = value; }
        }
        public virtual Color SelectedBackground
        {
            get { return selectedBackground; }
            set
            {
                selectedBackground = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBackground"));
            }
        }

        public virtual Color HoverBackground
        {
            get { return hoverBackground; }
            set
            {
                hoverBackground = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HoverBackground"));
            }
        }

        public int SelectedStarIndex
        {
            get { return selectedStarIndex; }
            set
            {
                selectedStarIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStarIndex"));

                switch (selectedStarIndex)
                {
                    case 1:
                        SelectedBackground = Colors.Red;
                        break;

                    case 2:
                        SelectedBackground = Colors.Orange;
                        break;

                    case 3:
                        SelectedBackground = Colors.Yellow;
                        break;

                    case 4:
                        SelectedBackground = Colors.DeepSkyBlue;
                        break;

                    case 5:
                        SelectedBackground = Colors.Green;
                        break;

                    default:
                        SelectedBackground = Colors.Transparent;
                        break;
                }
            }
        }

        public virtual Offer CurrentItem { get; set; }

        public bool IsLostCanceled
        {
            get { return isLostCanceled; }
            set { isLostCanceled = value; }
        }

        public string LeadsEditViewCloseDate
        {
            get { return leadsEditViewCloseDate; }
            set { leadsEditViewCloseDate = value; }
        }

        public GridLength WidthSalesOwnerImg { get; set; }
        public GridLength WidthSalesOwnerCombobox { get; set; }
        public GridLength WidthSpaceAfterSalesOwner { get; set; }

        public bool IsOldOT
        {
            get { return isOldOT; }
            set { isOldOT = value; }
        }

        public string OfferCode
        {
            get { return offerCode; }
            set
            {
                offerCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferCode"));
            }
        }

        public bool IsCodeUpdated
        {
            get { return isCodeUpdated; }
            set { isCodeUpdated = value; }
        }

        #endregion // Properties

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

        #endregion // Events

        #region Public Command

        public ICommand UpdateLeadStatusCommand { get; set; }
        public ICommand OnDroppedCommand { get; set; }
        public ICommand MouseDoubleClickActionCommand { get; set; }
        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand RefreshPipelineViewCommand { get; set; }

        #endregion

        #region Constructor

        public LeadsPipelineViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor LeadsPipelineViewModel ...", category: Category.Info, priority: Priority.Low);

                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                //Parameter 2 for show task suggestion.
                //Max_Value_TaskSuggestion = CrmStartUp.GetOfferMaxValueById(2);
                activityTemplateTriggers = CrmStartUp.GetActivityTemplateTriggers();

                Currencies = CrmStartUp.GetCurrencyByExchangeRate();

                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

                UpdateLeadStatusCommand = new Prism.Commands.DelegateCommand<ListBoxDropEventArgs>(UpdateLeadStatus);
                OnDroppedCommand = new Prism.Commands.DelegateCommand<ListBoxDroppedEventArgs>(OnDroppedAction);
                MouseDoubleClickActionCommand = new Prism.Commands.DelegateCommand<MouseButtonEventArgs>(MouseDoubleClickLeadPipeline);
                RefreshPipelineViewCommand = new RelayCommand(new Action<object>(RefreshPipelineDetails));
                SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);

                FillCmbSalesOwner();

                GeosApplication.Instance.Logger.Log("Constructor LeadsPipelineViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in LeadsPipelineViewModel() Constructor " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in LeadsPipelineViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in LeadsPipelineViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        #endregion // Constructor

        #region Methods

        /// <summary>
        /// Method for fill data as per user permission.
        /// </summary>
        private void FillCmbSalesOwner()
        {
            GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
            GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

            if (GeosApplication.Instance.IdUserPermission == 21 || GeosApplication.Instance.IdUserPermission == 22)
            {
                WidthSalesOwnerImg = new GridLength(85, GridUnitType.Star);
                WidthSalesOwnerCombobox = new GridLength(150);
                WidthSpaceAfterSalesOwner = new GridLength(30);
                FillPipelineDetailsByUser();
            }
            else
            {
                WidthSalesOwnerImg = new GridLength(0);
                WidthSalesOwnerCombobox = new GridLength(0);
                WidthSpaceAfterSalesOwner = new GridLength(0);
                FillPipelineDetails();
            }
        }

        private void SalesOwnerPopupClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SalesOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);

            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

            try
            {
                isFromRefreshOrSalesOwner = true;
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

                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    FailedPlants = new List<string>();
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = String.Empty;

                    FillPipelineDetailsByUser();
                    if (FailedPlants != null && FailedPlants.Count > 0)
                    {
                        IsShowFailedPlantWarning = true;
                        WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                        WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                    }
                }
                else if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    FailedPlants = new List<string>();
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = String.Empty;

                    FillPipelineDetailsByUser();
                    if (FailedPlants != null && FailedPlants.Count > 0)
                    {

                        IsShowFailedPlantWarning = true;
                        WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                        WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                    }
                }
                else
                {
                    foreach (SalesStatusType salesStatusType in ListSalesStatusType)
                    {
                        salesStatusType.OffersInObject = new ObservableCollection<Offer>();
                        salesStatusType.TotalAmount = Math.Round(0.0);

                        // If No SalesOwner Selected from list then make copy offers to 0. 
                        SalesStatusType copySalesStatusType = CopyPipelineMainSalesStatusTypeList.FirstOrDefault(x => x.IdSalesStatusType == salesStatusType.IdSalesStatusType);
                        copySalesStatusType.OffersInObject = new List<Offer>();
                        copySalesStatusType.TotalAmount = Math.Round(0.0, 2);
                    }

                    FailedPlants.Clear();
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = String.Empty;
                }

                isFromRefreshOrSalesOwner = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SalesOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                isFromRefreshOrSalesOwner = false;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SalesOwnerPopupClosedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillPipelineDetailsByUser()
        {
            GeosApplication.Instance.Logger.Log("Method FillPipelineDetailsByUser ...", category: Category.Info, priority: Priority.Low);

            LeadsList = new List<Offer>();
            List<Offer> OffersByUsers = new List<Offer>();

            #region Roll 22

            if (GeosApplication.Instance.IdUserPermission == 22)
            {
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ShortName));
                    PreviouslySelectedPlantOwners = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.ShortName;
                            OffersByUsers.AddRange(CrmStartUp.GetOffersPipeline_V2035(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, itemPlantOwnerUsers, GeosApplication.Instance.IdUserPermission).ToList());
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(itemPlantOwnerUsers.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(itemPlantOwnerUsers.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(itemPlantOwnerUsers.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;

                    if (FailedPlants == null || FailedPlants.Count == 0)
                    {
                        IsShowFailedPlantWarning = false;
                        WarningFailedPlants = string.Empty;
                    }
                }

                LeadsList = OffersByUsers;

                if (ListSalesStatusType.Count == 0)
                {
                    ListSalesStatusType = new ObservableCollection<SalesStatusType>(CrmStartUp.GetAllSalesStatusType().AsEnumerable());
                }
                if (CopyPipelineMainSalesStatusTypeList == null)
                {
                    CopyPipelineMainSalesStatusTypeList = new List<SalesStatusType>(CrmStartUp.GetAllSalesStatusType().AsEnumerable()); // Copy of main Pipeline List.
                }

                CreateSeperateLists();

                Background = Colors.Transparent;
                HoverBackground = Colors.Red;
                SelectedBackground = Colors.Transparent;
                SelectedStarIndex = 10;
            }
            #endregion

            #region Roll 21
            else
            {
                if (GeosApplication.Instance.IdUserPermission == 21 && GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {

                    if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                    {
                        List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                        PreviouslySelectedSalesOwners = salesOwnersIds;

                        foreach (var itemCompaniesDetails in GeosApplication.Instance.CompanyList)
                        {
                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemCompaniesDetails.Alias;

                                OffersByUsers.AddRange(CrmStartUp.GetOffersPipeline_V2035(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                             salesOwnersIds, GeosApplication.Instance.ActiveUser.IdUser,
                                                                                             GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                                             itemCompaniesDetails, GeosApplication.Instance.IdUserPermission));
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(itemCompaniesDetails.ShortName);
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(itemCompaniesDetails.ShortName);
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                    FailedPlants.Add(itemCompaniesDetails.ShortName);
                                if (FailedPlants != null && FailedPlants.Count > 0)
                                {
                                    IsShowFailedPlantWarning = true;
                                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                }
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }

                        GeosApplication.Instance.SplashScreenMessage = string.Empty;

                        if (FailedPlants == null || FailedPlants.Count == 0)
                        {
                            IsShowFailedPlantWarning = false;
                            WarningFailedPlants = string.Empty;
                        }

                        LeadsList = OffersByUsers;

                        if (ListSalesStatusType.Count == 0)
                        {
                            ListSalesStatusType = new ObservableCollection<SalesStatusType>(CrmStartUp.GetAllSalesStatusType().AsEnumerable());
                        }
                        if (CopyPipelineMainSalesStatusTypeList == null)
                        {
                            CopyPipelineMainSalesStatusTypeList = new List<SalesStatusType>(CrmStartUp.GetAllSalesStatusType().AsEnumerable()); // Copy of main Pipeline List.
                        }

                        CreateSeperateLists();

                        Background = Colors.Transparent;
                        HoverBackground = Colors.Red;
                        SelectedBackground = Colors.Transparent;
                        SelectedStarIndex = 10;
                    }
                }
                #endregion

                else      // If SelectedSalesOwnerUsersList is null then display empty Pipeline
                {
                    if (ListSalesStatusType.Count == 0)
                    {
                        ListSalesStatusType = new ObservableCollection<SalesStatusType>(CrmStartUp.GetAllSalesStatusType().AsEnumerable());
                    }
                    if (CopyPipelineMainSalesStatusTypeList == null)
                    {
                        CopyPipelineMainSalesStatusTypeList = new List<SalesStatusType>(CrmStartUp.GetAllSalesStatusType().AsEnumerable()); // Copy of main Pipeline List.
                    }

                    // If No SalesOwner Selected from list then make copy offers to 0. 
                    foreach (SalesStatusType salesStatusType in ListSalesStatusType)
                    {
                        salesStatusType.OffersInObject = new ObservableCollection<Offer>();
                        salesStatusType.TotalAmount = Math.Round(0.0);

                        // If No SalesOwner Selected from list then make copy offers to 0. 
                        SalesStatusType copySalesStatusType = CopyPipelineMainSalesStatusTypeList.FirstOrDefault(x => x.IdSalesStatusType == salesStatusType.IdSalesStatusType);
                        copySalesStatusType.OffersInObject = new List<Offer>();
                        copySalesStatusType.TotalAmount = Math.Round(0.0, 2);
                    }

                    Background = Colors.Transparent;
                    HoverBackground = Colors.Red;
                    SelectedBackground = Colors.Transparent;
                    SelectedStarIndex = 10;
                }
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
            GeosApplication.Instance.Logger.Log("Method FillPipelineDetailsByUser() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for refresh Pipeline details.
        /// </summary>
        private void FillPipelineDetails()
        {
            GeosApplication.Instance.Logger.Log("Method FillPipelineDetails ...", category: Category.Info, priority: Priority.Low);
            try
            {
                LeadsList = new List<Offer>();
                List<Offer> offerList = new List<Offer>();

                // Continue loop although some plant is not available and Show error message.
                foreach (var item in GeosApplication.Instance.CompanyList)
                {
                    try
                    {
                        GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.Alias;
                        offerList.AddRange(CrmStartUp.GetOffersPipeline_V2035(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser.ToString(), GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, item, GeosApplication.Instance.IdUserPermission));
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            FailedPlants.Add(item.ShortName);
                        if (FailedPlants != null && FailedPlants.Count > 0)
                        {
                            IsShowFailedPlantWarning = true;
                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                        }
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            FailedPlants.Add(item.ShortName);
                        if (FailedPlants != null && FailedPlants.Count > 0)
                        {
                            IsShowFailedPlantWarning = true;
                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                        }
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.Alias, " Failed");
                        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            FailedPlants.Add(item.ShortName);
                        if (FailedPlants != null && FailedPlants.Count > 0)
                        {
                            IsShowFailedPlantWarning = true;
                            WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                        }
                        System.Threading.Thread.Sleep(1000);
                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                }

                GeosApplication.Instance.SplashScreenMessage = string.Empty;

                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }

                LeadsList = offerList;
                ListSalesStatusType = new ObservableCollection<SalesStatusType>(CrmStartUp.GetAllSalesStatusType().AsEnumerable());
                if (CopyPipelineMainSalesStatusTypeList == null)
                {
                    CopyPipelineMainSalesStatusTypeList = new List<SalesStatusType>(CrmStartUp.GetAllSalesStatusType().AsEnumerable()); // Copy of main Pipeline List.
                }

                CreateSeperateLists();

                Background = Colors.Transparent;
                HoverBackground = Colors.Red;
                SelectedBackground = Colors.Transparent;
                SelectedStarIndex = 10;

                GeosApplication.Instance.Logger.Log("Method FillPipelineDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPipelineDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        public void CreateSeperateLists()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateSeperateLists ...", category: Category.Info, priority: Priority.Low);

                if (isFromRefreshOrSalesOwner)
                    PipelineFilterCriteria = null;

                foreach (SalesStatusType item in ListSalesStatusType)
                {
                    List<Offer> tempOffer = new List<Offer>(LeadsList.Where(lead => lead.GeosStatus.SalesStatusType.Name.ToUpper() == item.Name).ToList());

                    foreach (var Offeritem in tempOffer)
                    {
                        if (Offeritem.IdSalesOwner != null)
                        {
                            Offeritem.SalesOwner = new People();
                            PeopleDetails peopleDetails = new PeopleDetails();
                            peopleDetails = GeosApplication.Instance.PeopleList.Where(x => x.IdPerson == Offeritem.IdSalesOwner).FirstOrDefault();
                            if (peopleDetails != null)
                            {
                                Offeritem.SalesOwner.IdPerson = peopleDetails.IdPerson;
                                Offeritem.SalesOwner.Name = peopleDetails.Name;
                                Offeritem.SalesOwner.Surname = peopleDetails.Surname;
                                Offeritem.SalesOwner.FullName = peopleDetails.FullName;
                                Offeritem.SalesOwner.Email = peopleDetails.Email;
                            }

                        }
                    }

                    item.OffersInObject = new ObservableCollection<Offer>(tempOffer.ToList());

                    // Added this as item.GeosStatus.SalesStatusType.HtmlColor is used in pipeline.
                    item.GeosStatus.SalesStatusType = new SalesStatusType();
                    item.GeosStatus.SalesStatusType.IdSalesStatusType = item.IdSalesStatusType;
                    item.GeosStatus.SalesStatusType.Name = item.Name;
                    item.GeosStatus.SalesStatusType.HtmlColor = item.HtmlColor;

                    item.TotalAmount = Math.Round(LeadsList.Where(lead => lead.GeosStatus.SalesStatusType.Name.ToUpper() == item.Name).Select(A => A.Value).ToList().Sum(), 2);

                    if (item.IdSalesStatusType == 4)
                    {
                        LeadsEditViewCloseDate = System.Windows.Application.Current.FindResource("LeadEditPODate").ToString();
                    }
                    else
                    {
                        LeadsEditViewCloseDate = System.Windows.Application.Current.FindResource("LeadPipeline").ToString();
                    }

                    item.IdSalesStatus = LeadsList.Where(lead => lead.GeosStatus.SalesStatusType.Name.ToUpper() == item.Name).Select(bh => bh.GeosStatus.IdOfferStatusType).FirstOrDefault();
                    item.SalesOwner = LeadsList.Where(lead => lead.GeosStatus.SalesStatusType.Name.ToUpper() == item.Name).Select(sa => sa.SalesOwner).FirstOrDefault();

                    // Make copy of list
                    SalesStatusType copySalesStatusType = CopyPipelineMainSalesStatusTypeList.FirstOrDefault(x => x.IdSalesStatusType == item.IdSalesStatusType);
                    copySalesStatusType.OffersInObject = tempOffer.ToList();
                    copySalesStatusType.TotalAmount = item.TotalAmount;
                }

                List<Offer> _TempLeadList = LeadsList.Where(leadlst => leadlst.DeliveryDate < GeosApplication.Instance.ServerDateTime.Date).ToList();

                foreach (Offer of in _TempLeadList)
                {
                    if (of.GeosStatus.SalesStatusType.IdSalesStatusType != 5)
                        of.IsCloseDateExceed = true;
                }

                GeosApplication.Instance.Logger.Log("Method CreateSeperateLists() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateSeperateLists() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for refresh Pipeline From database by refresh Button.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshPipelineDetails(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method RefreshPipelineDetails ...", category: Category.Info, priority: Priority.Low);

            isFromRefreshOrSalesOwner = true;

            FailedPlants = new List<string>();
            IsShowFailedPlantWarning = false;
            WarningFailedPlants = String.Empty;

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

            FillCmbSalesOwner();
            isFromRefreshOrSalesOwner = false;
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

            GeosApplication.Instance.Logger.Log("Method RefreshPipelineDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// [001][SP-67][skale][16-07-2019][GEOS2-1641]fechas vencidas en sistema CRM
        /// </summary>
        /// <param name="obj"></param>
        private void MouseDoubleClickLeadPipeline(MouseButtonEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method MouseDoubleClickLeadPipeline ...", category: Category.Info, priority: Priority.Low);

                ListBoxEdit pcl = (ListBoxEdit)(obj.Source);
                SalesStatusType salesStatusType = (SalesStatusType)(pcl.Tag);
                Offer selectedItem = (Offer)pcl.SelectedItem;

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
                Offer of = new Offer();

                //of = CrmStartUp.GetOfferDetailsById(selectedItem.IdOffer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.CrmOfferYear, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ((Offer)pcl.SelectedItem).Site.ConnectPlantId).FirstOrDefault());
                //[001] added Change Method 
                of = CrmStartUp.GetOfferDetailsById_V2037(selectedItem.IdOffer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ((Offer)pcl.SelectedItem).Site.ConnectPlantId).FirstOrDefault());

                if (of != null)
                {
                    //if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Show<SplashScreenView>(); }

                    List<Offer> TempLeadsList = new List<Offer>();
                    TempLeadsList.Add(of);

                    LostReasonsByOffer lostReasonsByOffer = CrmStartUp.GetLostReasonsByOffer(Convert.ToInt64(TempLeadsList[0].IdOffer),
                                                                                             GeosApplication.Instance.CompanyList.Where(cmp => cmp.ConnectPlantId == ((Offer)pcl.SelectedItem).Site.ConnectPlantId).Select(x => x.ConnectPlantConstr).FirstOrDefault());

                    if (lostReasonsByOffer != null)
                    {
                        TempLeadsList[0].LostReasonsByOffer = lostReasonsByOffer;
                    }

                    if (of.GeosStatus.IdSalesStatusType == 4)
                    {
                        OrderEditViewModel orderEditViewModel = new OrderEditViewModel();
                        OrderEditView orderEditView = new OrderEditView();

                        orderEditViewModel.IsControlEnable = true;
                        orderEditViewModel.IsControlEnableorder = false;
                        orderEditViewModel.IsStatusChangeAction = true;
                        orderEditViewModel.IsAcceptControlEnableorder = false;
                        orderEditViewModel.InItOrder(TempLeadsList);

                        EventHandler handle = delegate { orderEditView.Close(); };
                        orderEditViewModel.RequestClose += handle;
                        orderEditView.DataContext = orderEditViewModel;

                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                        {
                            DXSplashScreen.Close();
                        }

                        orderEditView.ShowDialogWindow();

                        if (orderEditViewModel.OfferData != null)
                        {
                            selectedItem.Code = orderEditViewModel.OfferData.Code;

                            if (selectedItem.Site != null)
                            {
                                if (orderEditViewModel.OfferData.Site.Customers != null && orderEditViewModel.OfferData.Site.Customers.Count > 0)
                                {
                                    selectedItem.Site.Customers[0] = orderEditViewModel.OfferData.Site.Customers[0];
                                }
                                selectedItem.Site.Name = orderEditViewModel.OfferData.Site.SiteNameWithoutCountry;
                            }

                            //**This code is temporary comment... because it is not useful.
                            //if (orderEditViewModel.OfferData.CarProject != null)
                            //{
                            //    selectedItem.CarProject = new CarProject();
                            //    selectedItem.CarProject = orderEditViewModel.OfferData.CarProject;
                            //}
                            //if (orderEditViewModel.OfferData.CarOEM != null )
                            //{
                            //    selectedItem.CarOEM = new CarOEM();
                            //    selectedItem.CarOEM = orderEditViewModel.OfferData.CarOEM;
                            //}
                            //if (orderEditViewModel.OfferData.Source != null)
                            //{
                            //    selectedItem.Source = new Data.Common.Epc.LookupValue();
                            //    selectedItem.Source.Value = orderEditViewModel.OfferData.Source.Value;
                            //}
                            //if (orderEditViewModel.OfferData.BusinessUnit != null )
                            //{
                            //    selectedItem.BusinessUnit = new Data.Common.Epc.LookupValue();
                            //    selectedItem.BusinessUnit = orderEditViewModel.OfferData.BusinessUnit;
                            //}
                            //**This code is temporary comment... because it is not useful.

                            CreateSeperateLists();
                        }
                    }

                    else
                    {
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

                        if (leadsEditViewModel.OfferData != null)
                        {
                            selectedItem.Code = leadsEditViewModel.OfferData.Code;
                            selectedItem.IdOfferType = leadsEditViewModel.OfferData.IdOfferType;

                            if (selectedItem.Site != null)
                            {
                                if (leadsEditViewModel.OfferData.Site.Customers != null && leadsEditViewModel.OfferData.Site.Customers.Count > 0)
                                {
                                    selectedItem.Site.Customers[0] = leadsEditViewModel.OfferData.Site.Customers[0];
                                }
                                selectedItem.Site.Name = leadsEditViewModel.OfferData.Site.SiteNameWithoutCountry;
                            }

                            selectedItem.Description = leadsEditViewModel.OfferData.Description;
                            selectedItem.DeliveryDate = leadsEditViewModel.OfferData.DeliveryDate;
                            selectedItem.Value = leadsEditViewModel.OfferData.Value;
                            selectedItem.GeosStatus = leadsEditViewModel.OfferData.GeosStatus;
                            selectedItem.ProbabilityOfSuccess = leadsEditViewModel.OfferData.ProbabilityOfSuccess;
                            selectedItem.IdSalesOwner = leadsEditViewModel.OfferData.IdSalesOwner;
                            selectedItem.SalesOwner = leadsEditViewModel.OfferData.SalesOwner;

                            CreateSeperateLists();
                        }

                        if (leadsEditViewModel.OfferDataLst != null)
                        {
                            LeadsList.AddRange(leadsEditViewModel.OfferDataLst);
                            CreateSeperateLists();
                        }

                    }


                }

                GeosApplication.Instance.Logger.Log("Method MouseDoubleClickLeadPipeline() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in MouseDoubleClickLeadPipeline() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in MouseDoubleClickLeadPipeline() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in MouseDoubleClickLeadPipeline() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UpdateLeadStatus(ListBoxDropEventArgs obj)
        {
            try
            {
                if (!GeosApplication.Instance.IsPermissionReadOnly)
                {
                    GeosApplication.Instance.Logger.Log("Method UpdateLeadStatus ...", category: Category.Info, priority: Priority.Low);

                    // IList<OfferType> OfferTypeList = CrmStartUp.GetOfferType();
                    int SelectedIndexOfferType = 0;
                    string _oldOfferCode = "";
                    bool isLostReasonFill = true;
                    ListBoxEdit pcl = (ListBoxEdit)(obj.SourceControl);

                    SalesStatusType pclSourceSalesStatusType = (SalesStatusType)(pcl.Tag);

                    // Source Pipeline is WON then It is not possible to move from WON.
                    if (pclSourceSalesStatusType.IdSalesStatusType == 4)
                    {
                        obj.Handled = true;
                        return;
                    }

                    // Destination Pipeline is WON then It is not possible to move to WON.
                    ListBoxEdit destListBox = (ListBoxEdit)(obj.ListBoxEdit);
                    SalesStatusType destSalesStatusType = (SalesStatusType)(destListBox.Tag);
                    if (destSalesStatusType.IdSalesStatusType == 4)
                    {
                        obj.Handled = true;
                        return;
                    }

                    Offer of1 = new Offer();
                    Offer of2 = new Offer();
                    of1 = (Offer)(obj.DraggedRows[0]);
                    Offer of = new Offer();
                    of = (Offer)pcl.SelectedItem;
                    List<Offer> TempLeadsList = new List<Offer>();

                    salesStatusType = (SalesStatusType)(obj.ListBoxEdit.Tag);

                    of2.GeosStatus = salesStatusType.GeosStatus;

                    //If offer move to Lost
                    if (salesStatusType.IdSalesStatusType == 5)
                    {
                        List<Offer> TempLeadsList1 = new List<Offer>();
                        TempLeadsList1.Add(of2);

                        LostOpportunityView lostOpportunityView = new LostOpportunityView();
                        LostOpportunityViewModel lostOpportunityViewModel = new LostOpportunityViewModel();
                        EventHandler handle = delegate { lostOpportunityView.Close(); };
                        lostOpportunityViewModel.RequestClose += handle;
                        lostOpportunityViewModel.Offer = TempLeadsList1;
                        lostOpportunityViewModel.InIt();
                        lostOpportunityView.DataContext = lostOpportunityViewModel;
                        lostOpportunityView.ShowDialog();

                        of2.LostReasonsByOffer = lostOpportunityViewModel.Offer[0].LostReasonsByOffer;

                        if (!lostOpportunityViewModel.IsCancel)
                        {
                            of2.LostReasonsByOffer = lostOpportunityViewModel.Offer[0].LostReasonsByOffer;

                            if (of2.LostReasonsByOffer == null)
                            {
                                isLostReasonFill = false;
                            }
                        }
                        else
                        {
                            obj.Handled = true;
                            IsLostCanceled = true;
                            return;
                        }
                    }

                    // For Only quoted....
                    if (salesStatusType.IdSalesStatusType == 3)
                    {
                        //if it is Only Quoted then set SelectedIndexOfferType=1
                        SelectedIndexOfferType = 1;
                        IsOldOT = false;

                        DateTime QuoteSentIn = GeosApplication.Instance.ServerDateTime;
                        if (of.SendIn != null)
                        {
                            QuoteSentIn = (DateTime)of.SendIn;
                        }

                        SendQuoteView sendQuoteView = new SendQuoteView();
                        SendQuoteViewModel sendQuoteViewModel = new SendQuoteViewModel();

                        if (of1.SendIn != null && of1.SendIn != DateTime.MinValue)
                            sendQuoteViewModel.QuoteSentDate = of1.SendIn.Value;

                        EventHandler handle = delegate { sendQuoteView.Close(); };
                        sendQuoteViewModel.RequestClose += handle;
                        sendQuoteView.DataContext = sendQuoteViewModel;
                        sendQuoteView.ShowDialog();

                        if (sendQuoteViewModel.IsSave)
                        {
                            if (sendQuoteViewModel.QuoteSentDate != null)
                            {
                                QuoteSentIn = (DateTime)sendQuoteViewModel.QuoteSentDate;
                            }

                            foreach (Offer item in obj.DraggedRows)
                            {
                                if (item is Offer)
                                {
                                    _oldOfferCode = item.Code;

                                    if (of2.LostReasonsByOffer != null)
                                    {
                                        of2.LostReasonsByOffer.IdOffer = item.IdOffer;
                                    }

                                    if (item.IdOfferType == 1)
                                    {
                                        IsOldOT = true;
                                    }
                                    if (!IsOldOT)
                                    {
                                        item.IdOfferType = Convert.ToByte(SelectedIndexOfferType);

                                        if (item.IdOfferType == 1 || item.IdOfferType == 2 || item.IdOfferType == 3 || item.IdOfferType == 4)
                                        {
                                            OfferNumber = CrmStartUp.GetNextNumberOfSuppliesFromGCM(item.IdOfferType);
                                        }
                                        else
                                        {
                                            OfferNumber = CrmStartUp.GetNextNumberOfOfferFromCounters(item.IdOfferType, GeosApplication.Instance.CompanyList.Where(com => com.ConnectPlantId == item.Site.ConnectPlantId).Select(companyname => companyname.ConnectPlantConstr).FirstOrDefault(), GeosApplication.Instance.ActiveUser.IdUser);
                                        }

                                        item.Number = OfferNumber;
                                        OfferCode = CrmStartUp.MakeOfferCode(Convert.ToByte(SelectedIndexOfferType), GeosApplication.Instance.ActiveUser.IdCompany.Value, GeosApplication.Instance.CompanyList.Where(com => com.ConnectPlantId == item.Site.ConnectPlantId).Select(companyname => companyname.ConnectPlantConstr).FirstOrDefault(), GeosApplication.Instance.ActiveUser.IdUser);
                                        item.Code = OfferCode;
                                        item.Comments = "";
                                        item.Rfq = "";

                                        item.IsUpdateLeadToOT = true;
                                        item.Site.FullName = item.Site.Customers[0].CustomerName + " - " + item.Site.Name + "(" + item.Site.Country.Name + ")";

                                        IsCodeUpdated = true;
                                    }
                                    else
                                    {
                                        item.Rfq = "";
                                    }

                                    //[Start] ** Code for add change log if QuoteSentIn date added or modify.

                                    List<LogEntryByOffer> logEntryByOfferList = new List<LogEntryByOffer>();
                                    if (QuoteSentIn != null && item.SendIn.Value.Date == DateTime.MinValue && QuoteSentIn.Date != item.SendIn.Value.Date)
                                    {
                                        //log entry for date change.
                                        LogEntryByOffer logEntryByOfferQuoteSentIn = new LogEntryByOffer()
                                        {
                                            IdOffer = of1.IdOffer,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            DateTime = GeosApplication.Instance.ServerDateTime,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewQuoteSentDate­­Add").ToString(),
                                                       Convert.ToDateTime(QuoteSentIn.Date).ToShortDateString()),
                                            IdLogEntryType = 7
                                        };

                                        logEntryByOfferList.Add(logEntryByOfferQuoteSentIn);

                                    }
                                    else if ((QuoteSentIn != null && QuoteSentIn != DateTime.MinValue) && item.SendIn.Value.Date != QuoteSentIn.Date)
                                    {
                                        LogEntryByOffer logEntryByOfferQuoteSentIn = new LogEntryByOffer()
                                        {
                                            IdOffer = of1.IdOffer,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            DateTime = GeosApplication.Instance.ServerDateTime,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewQuoteSentDateChd").ToString(),
                                                       Convert.ToDateTime(item.SendIn.Value).ToShortDateString(), Convert.ToDateTime(QuoteSentIn.Date).ToShortDateString()),
                                            IdLogEntryType = 7
                                        };

                                        logEntryByOfferList.Add(logEntryByOfferQuoteSentIn);

                                    }
                                    //[End]** Code for add change log if QuoteSentIn date added or modify.

                                    //log entry for status Change.
                                    LogEntryByOffer logEntryByOffer = new LogEntryByOffer() { IdOffer = of1.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewGeosStatusChanged").ToString(), of1.GeosStatus.Name, of2.GeosStatus.Name), IdLogEntryType = 7 };
                                    logEntryByOfferList.Add(logEntryByOffer);

                                    //if code is change then add log entry about it.
                                    if (string.Compare(_oldOfferCode, item.Code) != 0)
                                    {
                                        logEntryByOffer = new LogEntryByOffer()
                                        {
                                            IdOffer = of1.IdOffer,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            DateTime = GeosApplication.Instance.ServerDateTime,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewOfferCodeChanged").ToString(),
                                                                      _oldOfferCode, item.Code),
                                            IdLogEntryType = 7
                                        };
                                        logEntryByOfferList.Add(logEntryByOffer);
                                    }

                                    if (logEntryByOfferList != null && logEntryByOfferList.Count > 0)
                                    {
                                        item.LogEntryByOffers = logEntryByOfferList;
                                    }


                                    item.SendIn = QuoteSentIn;
                                    bool isOfferUpdate = false;
                                    if (IsCodeUpdated)
                                    {

                                        item.Comments = "";
                                        bool isupdate = CrmStartUp.UpdateOfferStatus(item.IdOffer, salesStatusType.GeosStatus.IdOfferStatusType, GeosApplication.Instance.ActiveUser.IdUser, QuoteSentIn, Convert.ToInt64(salesStatusType.GeosStatus.SalesStatusType.IdSalesStatusType), IsCodeUpdated, of2.LostReasonsByOffer, GeosApplication.Instance.CompanyList.Where(com => com.ConnectPlantId == item.Site.ConnectPlantId).Select(companyname => companyname.ConnectPlantConstr).FirstOrDefault(), item, GeosApplication.Instance.ActiveUser.IdCompany.Value);
                                        isOfferUpdate = isupdate;

                                        if (isupdate && item.IdOfferType == 1 && item.IsUpdateLeadToOT == true)
                                        {
                                            EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(item.Site.ConnectPlantId));
                                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == emdepSite.ShortName).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();

                                            ICrmService CrmStartUpCreateFolder = new CrmServiceController(serviceurl);
                                            if (item.IsUpdateLeadToOT)
                                            {
                                                item.Year = GeosApplication.Instance.ServerDateTime.Year;
                                            }
                                            bool isCreated = CrmStartUpCreateFolder.CreateFolderOffer(item, true);
                                        }
                                    }
                                    else
                                    {
                                        item.Comments = "";
                                        bool isupdate = CrmStartUp.UpdateOfferStatus(item.IdOffer, salesStatusType.GeosStatus.IdOfferStatusType, GeosApplication.Instance.ActiveUser.IdUser, QuoteSentIn, Convert.ToInt64(salesStatusType.GeosStatus.SalesStatusType.IdSalesStatusType), IsCodeUpdated, of2.LostReasonsByOffer, GeosApplication.Instance.CompanyList.Where(com => com.ConnectPlantId == item.Site.ConnectPlantId).Select(companyname => companyname.ConnectPlantConstr).FirstOrDefault(), item);

                                        isOfferUpdate = isupdate;
                                    }

                                    //**[start] This code for create activity as per conditions. 

                                    List<ActivityTemplate> activityTemplateList = new List<ActivityTemplate>();
                                    string activityMsg = "";

                                    if (isOfferUpdate && item.Value > 0)
                                    {
                                        foreach (var activityTemplateTrigger in activityTemplateTriggers)
                                        {
                                            if (string.Format(salesStatusType.GeosStatus.IdOfferStatusType + "(" + salesStatusType.GeosStatus.Name + ")") == activityTemplateTrigger.LinkedObjectFieldValue)
                                            {
                                                Currency selectedcurrency = Currencies.FirstOrDefault(c => c.Name == activityTemplateTrigger.ActivityTemplateTriggerCondition.ConditionFieldType);
                                                Currency offerCurrency = Currencies.FirstOrDefault(x => x.IdCurrency == item.Currency.IdCurrency);

                                                Double amount = item.Value * (offerCurrency.CurrencyConversions.Count > 0 ? offerCurrency.CurrencyConversions[0].ExchangeRate : selectedcurrency.IdCurrency);

                                                if (Operator(activityTemplateTrigger.ActivityTemplateTriggerCondition.ConditionOperator, amount, Convert.ToDouble(activityTemplateTrigger.ActivityTemplateTriggerCondition.ConditionFieldValue)))
                                                {
                                                    if (activityTemplateTrigger.ActivityTemplateTriggerCondition.IsUserConfirmationRequired == 1)
                                                    {


                                                        foreach (var template1 in activityTemplateTrigger.ActivityTemplates)
                                                        {
                                                            if (activityTemplateTrigger.ActivityTemplates.Count > 1)
                                                            {
                                                                if (string.IsNullOrWhiteSpace(activityMsg))
                                                                {
                                                                    activityMsg = string.Format(Application.Current.Resources["ActivityCreateMoreThenOne"].ToString());
                                                                    activityMsg += System.Environment.NewLine + "-" + "\"" + activityTemplateTrigger.ActivityTemplates[0].Subject + "\" " + activityTemplateTrigger.ActivityTemplates[0].ActivityType.Value + " activity";
                                                                }

                                                                else
                                                                    activityMsg += System.Environment.NewLine + "-" + "\"" + activityTemplateTrigger.ActivityTemplates[0].Subject + "\" " + activityTemplateTrigger.ActivityTemplates[0].ActivityType.Value + " activity";
                                                            }
                                                            else
                                                            {
                                                                activityMsg = string.Format(Application.Current.Resources["ActivityCreate"].ToString(), activityTemplateTrigger.ActivityTemplates[0].Subject, activityTemplateTrigger.ActivityTemplates[0].ActivityType.Value);
                                                            }

                                                            activityTemplateList.Add(template1);
                                                        }


                                                    }
                                                    else
                                                    {
                                                        foreach (ActivityTemplate activityTemplate in activityTemplateTrigger.ActivityTemplates)
                                                        {
                                                            AddActivity(item, Convert.ToInt32(item.Site.ConnectPlantId), activityTemplate);
                                                        }
                                                    }
                                                }
                                            }
                                        }


                                        if (activityTemplateList.Count > 0)
                                        {
                                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(activityMsg, Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                                            if (MessageBoxResult == MessageBoxResult.Yes)
                                            {
                                                foreach (ActivityTemplate activityTemplate in activityTemplateList)
                                                {
                                                    AddActivity(item, Convert.ToInt32(item.Site.ConnectPlantId), activityTemplate);
                                                }
                                            }
                                        }
                                    }

                                    //[End]code for create activity as per conditions. 

                                    if (isOfferUpdate)
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SendQuoteViewSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                                    else
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SendQuoteViewFailure").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                                    IsLostCanceled = false;
                                }
                            }
                        }
                        else
                        {
                            obj.Handled = true;
                            IsLostCanceled = true;
                            return;
                        }
                    }

                    // For Waiting for quote
                    if (salesStatusType.IdSalesStatusType == 6)
                    {
                        //if it is waiting for quote then set SelectedIndexOfferType=1
                        SelectedIndexOfferType = 1;
                        IsOldOT = false;

                        DateTime RfqReception = GeosApplication.Instance.ServerDateTime;
                        if (of.RFQReception != null)
                        {
                            RfqReception = (DateTime)of.RFQReception;
                        }

                        RfqReceptionView rfqReceptionView = new RfqReceptionView();
                        RfqReceptionViewModel rfqReceptionViewModel = new RfqReceptionViewModel();

                        if (of1.RFQReception != null && of1.RFQReception != DateTime.MinValue)
                            rfqReceptionViewModel.RFQReceptionDate = of1.RFQReception.Value;

                        EventHandler handle = delegate { rfqReceptionView.Close(); };
                        rfqReceptionViewModel.RequestClose += handle;
                        rfqReceptionView.DataContext = rfqReceptionViewModel;
                        rfqReceptionView.ShowDialog();

                        if (rfqReceptionViewModel.IsSave)
                        {
                            if (rfqReceptionViewModel.RFQReceptionDate != null)
                            {
                                RfqReception = (DateTime)rfqReceptionViewModel.RFQReceptionDate;
                            }
                            foreach (Offer item in obj.DraggedRows)
                            {
                                if (item is Offer)
                                {
                                    _oldOfferCode = item.Code;

                                    if (of2.LostReasonsByOffer != null)
                                    {
                                        of2.LostReasonsByOffer.IdOffer = item.IdOffer;
                                    }
                                    if (item.IdOfferType == 1)
                                    {
                                        IsOldOT = true;
                                    }
                                    if (!IsOldOT)
                                    {
                                        item.IdOfferType = Convert.ToByte(SelectedIndexOfferType);

                                        if (item.IdOfferType == 1 || item.IdOfferType == 2 || item.IdOfferType == 3 || item.IdOfferType == 4)
                                        {
                                            OfferNumber = CrmStartUp.GetNextNumberOfSuppliesFromGCM(item.IdOfferType);
                                        }
                                        else
                                        {
                                            OfferNumber = CrmStartUp.GetNextNumberOfOfferFromCounters(item.IdOfferType, GeosApplication.Instance.CompanyList.Where(com => com.ConnectPlantId == item.Site.ConnectPlantId).Select(companyname => companyname.ConnectPlantConstr).FirstOrDefault(), GeosApplication.Instance.ActiveUser.IdUser);
                                        }

                                        item.Number = OfferNumber;
                                        OfferCode = CrmStartUp.MakeOfferCode(Convert.ToByte(SelectedIndexOfferType), GeosApplication.Instance.ActiveUser.IdCompany.Value, GeosApplication.Instance.CompanyList.Where(com => com.ConnectPlantId == item.Site.ConnectPlantId).Select(companyname => companyname.ConnectPlantConstr).FirstOrDefault(), GeosApplication.Instance.ActiveUser.IdUser);
                                        item.Code = OfferCode;
                                        item.Comments = "";
                                        item.Rfq = "";
                                        item.SendIn = GeosApplication.Instance.ServerDateTime;
                                        item.IsUpdateLeadToOT = true;
                                        item.Site.FullName = item.Site.Customers[0].CustomerName + " - " + item.Site.Name + "(" + item.Site.Country.Name + ")";

                                        IsCodeUpdated = true;
                                    }
                                    else
                                    {
                                        item.Rfq = "";
                                        item.SendIn = GeosApplication.Instance.ServerDateTime;
                                    }


                                    //** This code block is for to save change RFQReception date log as per condition (Start..)

                                    List<LogEntryByOffer> logEntryByOfferList = new List<LogEntryByOffer>();

                                    if (item.RFQReception.Value.Date != null && item.RFQReception.Value.Date == DateTime.MinValue)
                                    {
                                        LogEntryByOffer logEntryByOfferRFQReception = new LogEntryByOffer()
                                        {
                                            IdOffer = of1.IdOffer,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            DateTime = GeosApplication.Instance.ServerDateTime,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewRFQReceptionDate­­Add").ToString(),
                                                       Convert.ToDateTime(RfqReception.Date).ToShortDateString()),
                                            IdLogEntryType = 7
                                        };

                                        logEntryByOfferList.Add(logEntryByOfferRFQReception);


                                    }
                                    else if (item.RFQReception.Value.Date != null && item.RFQReception.Value.Date != DateTime.MinValue
                                             && item.RFQReception.Value.Date != RfqReception.Date)
                                    {
                                        LogEntryByOffer logEntryByOfferRFQReception = new LogEntryByOffer()
                                        {
                                            IdOffer = of1.IdOffer,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            DateTime = GeosApplication.Instance.ServerDateTime,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewRFQReceptionDateChd").ToString(),
                                                       Convert.ToDateTime(item.RFQReception.Value).ToShortDateString(), Convert.ToDateTime(RfqReception.Date).ToShortDateString()),
                                            IdLogEntryType = 7
                                        };

                                        logEntryByOfferList.Add(logEntryByOfferRFQReception);
                                    }
                                    //** This code block is for to save change RFQReception date log as per condition (End.)


                                    //log entry for status Change.
                                    LogEntryByOffer logEntryByOffer = new LogEntryByOffer() { IdOffer = of1.IdOffer, IdUser = GeosApplication.Instance.ActiveUser.IdUser, DateTime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewGeosStatusChanged").ToString(), of1.GeosStatus.Name, of2.GeosStatus.Name), IdLogEntryType = 7 };
                                    logEntryByOfferList.Add(logEntryByOffer);


                                    //if code is change then add log entry about it.
                                    if (string.Compare(_oldOfferCode, item.Code) != 0)
                                    {
                                        logEntryByOffer = new LogEntryByOffer()
                                        {
                                            IdOffer = of1.IdOffer,
                                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                            DateTime = GeosApplication.Instance.ServerDateTime,
                                            Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewOfferCodeChanged").ToString(),
                                                                                              _oldOfferCode, item.Code),
                                            IdLogEntryType = 7
                                        };

                                        logEntryByOfferList.Add(logEntryByOffer);
                                    }



                                    if (logEntryByOfferList != null && logEntryByOfferList.Count > 0)
                                    {
                                        item.LogEntryByOffers = logEntryByOfferList;
                                    }



                                    item.RFQReception = RfqReception;
                                    if (IsCodeUpdated)
                                    {

                                        item.Comments = "";
                                        bool isupdate = CrmStartUp.UpdateOfferStatus(item.IdOffer, salesStatusType.GeosStatus.IdOfferStatusType, GeosApplication.Instance.ActiveUser.IdUser, RfqReception, Convert.ToInt64(salesStatusType.IdSalesStatusType), IsCodeUpdated, of2.LostReasonsByOffer, GeosApplication.Instance.CompanyList.Where(com => com.ConnectPlantId == item.Site.ConnectPlantId).Select(companyname => companyname.ConnectPlantConstr).FirstOrDefault(), item, GeosApplication.Instance.ActiveUser.IdCompany.Value);

                                        if (isupdate && item.IdOfferType == 1 && item.IsUpdateLeadToOT == true)
                                        {
                                            EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(item.Site.ConnectPlantId));
                                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == emdepSite.ShortName).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();

                                            ICrmService CrmStartUpCreateFolder = new CrmServiceController(serviceurl);
                                            if (item.IsUpdateLeadToOT)
                                            {
                                                item.Year = GeosApplication.Instance.ServerDateTime.Year;
                                            }
                                            bool isCreated = CrmStartUpCreateFolder.CreateFolderOffer(item, true);
                                        }

                                        if (isupdate)
                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SendQuoteViewSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                                        else
                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SendQuoteViewFailure").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    }
                                    else
                                    {
                                        item.Comments = "";
                                        bool isupdate = CrmStartUp.UpdateOfferStatus(item.IdOffer, salesStatusType.GeosStatus.IdOfferStatusType, GeosApplication.Instance.ActiveUser.IdUser, RfqReception, Convert.ToInt64(salesStatusType.IdSalesStatusType), IsCodeUpdated, of2.LostReasonsByOffer, GeosApplication.Instance.CompanyList.Where(com => com.ConnectPlantId == item.Site.ConnectPlantId).Select(companyname => companyname.ConnectPlantConstr).FirstOrDefault(), item);

                                        if (isupdate)
                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SendQuoteViewSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                                        else
                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SendQuoteViewFailure").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    }

                                    //item.Site.ConnectPlantConstr);
                                    IsLostCanceled = false;
                                }
                            }
                        }
                        else
                        {
                            obj.Handled = true;
                            IsLostCanceled = true;
                            return;
                        }
                    }

                    // for other of the stauts type
                    if ((salesStatusType.IdSalesStatusType != 4 && salesStatusType.IdSalesStatusType != 3 && salesStatusType.IdSalesStatusType != 6) && isLostReasonFill != false)
                    {
                        bool _isOpportunity = true;
                        IsCodeUpdated = false;

                        SelectedIndexOfferType = 10;
                        foreach (Offer item in obj.DraggedRows)
                        {
                            if (item is Offer)
                            {
                                _oldOfferCode = item.Code;

                                if (item.IdOfferType == 1)
                                {
                                    _isOpportunity = false;
                                }

                                if (!_isOpportunity && salesStatusType.IdSalesStatusType != 5)
                                {
                                    item.IdOfferType = Convert.ToByte(SelectedIndexOfferType);

                                    if (item.IdOfferType == 1 || item.IdOfferType == 2 || item.IdOfferType == 3 || item.IdOfferType == 4)
                                    {
                                        OfferNumber = CrmStartUp.GetNextNumberOfSuppliesFromGCM(item.IdOfferType);
                                    }
                                    else
                                    {
                                        OfferNumber = CrmStartUp.GetNextNumberOfOfferFromCounters(item.IdOfferType, GeosApplication.Instance.CompanyList.Where(com => com.ConnectPlantId == item.Site.ConnectPlantId).Select(companyname => companyname.ConnectPlantConstr).FirstOrDefault(), GeosApplication.Instance.ActiveUser.IdUser);
                                    }

                                    item.Number = OfferNumber;
                                    OfferCode = CrmStartUp.MakeOfferCode(Convert.ToByte(SelectedIndexOfferType), GeosApplication.Instance.ActiveUser.IdCompany.Value, GeosApplication.Instance.CompanyList.Where(com => com.ConnectPlantId == item.Site.ConnectPlantId).Select(companyname => companyname.ConnectPlantConstr).FirstOrDefault(), GeosApplication.Instance.ActiveUser.IdUser);
                                    item.Code = OfferCode;
                                    item.Comments = "";
                                    // item.Rfq = "";
                                    // item.SendIn = QuoteSentIn;
                                    IsCodeUpdated = true;
                                }

                                //of2.IdOffer = item.IdOffer;
                                if (of2.LostReasonsByOffer != null)
                                {
                                    of2.LostReasonsByOffer.IdOffer = item.IdOffer;
                                }

                                // Log Entry for Change log.
                                List<LogEntryByOffer> logEntryByOfferList = new List<LogEntryByOffer>();

                                LogEntryByOffer logEntryByOffer = new LogEntryByOffer()
                                {
                                    IdOffer = of1.IdOffer,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    DateTime = GeosApplication.Instance.ServerDateTime,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewGeosStatusChanged").ToString(),
                                                                                          of1.GeosStatus.Name, of2.GeosStatus.Name),
                                    IdLogEntryType = 7
                                };
                                logEntryByOfferList.Add(logEntryByOffer);


                                //if code is change then add log entry about it.
                                if (string.Compare(_oldOfferCode, item.Code) != 0)
                                {
                                    logEntryByOffer = new LogEntryByOffer()
                                    {
                                        IdOffer = of1.IdOffer,
                                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                        DateTime = GeosApplication.Instance.ServerDateTime,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("LeadsEditViewOfferCodeChanged").ToString(),
                                                                                          _oldOfferCode, item.Code),
                                        IdLogEntryType = 7
                                    };

                                    logEntryByOfferList.Add(logEntryByOffer);
                                }

                                if (logEntryByOfferList != null && logEntryByOfferList.Count > 0)
                                {
                                    item.LogEntryByOffers = logEntryByOfferList;
                                }

                                // bool isupdate = CrmStartUp.UpdateOfferStatus(item.IdOffer, salesStatusType.GeosStatus.IdOfferStatusType, GeosApplication.Instance.ActiveUser.IdUser, of2.LostReasonsByOffer);
                                if (IsCodeUpdated)
                                {
                                    item.Comments = "";
                                    bool isupdate = CrmStartUp.UpdateOfferStatus(item.IdOffer, salesStatusType.GeosStatus.IdOfferStatusType,
                                                                                 GeosApplication.Instance.ActiveUser.IdUser, DateTime.Now,
                                                                                 Convert.ToInt64(salesStatusType.IdSalesStatusType), IsCodeUpdated,
                                                                                 of2.LostReasonsByOffer, GeosApplication.Instance.CompanyList.Where(com => com.ConnectPlantId == item.Site.ConnectPlantId).Select(companyname => companyname.ConnectPlantConstr).FirstOrDefault(),
                                                                                 item, GeosApplication.Instance.ActiveUser.IdCompany.Value);

                                    if (isupdate)
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SendQuoteViewSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                                    else
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SendQuoteViewFailure").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    // bool isupdate = CrmStartUp.UpdateOfferStatus(item.IdOffer, salesStatusType.GeosStatus.IdOfferStatusType, GeosApplication.Instance.ActiveUser.IdUser, DateTime.Now, Convert.ToInt64(salesStatusType.GeosStatus.SalesStatusType.IdSalesStatusType), IsCodeUpdated, of2.LostReasonsByOffer, Convert.ToInt32(item.Site.ConnectPlantId));
                                }
                                else
                                {
                                    //item.GeosStatus = salesStatusType.GeosStatus;
                                    item.Comments = "";
                                    bool isupdate = CrmStartUp.UpdateOfferStatus(item.IdOffer,
                                                                                 salesStatusType.GeosStatus.IdOfferStatusType,
                                                                                 GeosApplication.Instance.ActiveUser.IdUser,
                                                                                 GeosApplication.Instance.ServerDateTime,
                                                                                 Convert.ToInt64(salesStatusType.GeosStatus.SalesStatusType.IdSalesStatusType),
                                                                                 IsCodeUpdated, of2.LostReasonsByOffer,
                                                                                 GeosApplication.Instance.CompanyList.Where(com => com.ConnectPlantId == item.Site.ConnectPlantId).Select(companyname => companyname.ConnectPlantConstr).FirstOrDefault(), item);
                                }


                                IsLostCanceled = false;
                            }
                        }
                    }

                }
                else
                {
                    obj.Handled = true;
                    return;
                }
                // CreateSeperateLists();
                obj.Handled = false;

                GeosApplication.Instance.Logger.Log("Method UpdateLeadStatus() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateLeadStatus() Constructor " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateLeadStatus() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UpdateLeadStatus() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public Boolean Operator(string logic, double amount, double ConditionFieldValue)
        {
            switch (logic)
            {
                case ">": return amount > ConditionFieldValue;
                case "<": return amount < ConditionFieldValue;
                case ">=": return amount >= ConditionFieldValue;
                case "<=": return amount <= ConditionFieldValue;
                case "==": return amount == ConditionFieldValue;
                case "!=": return amount != ConditionFieldValue;
                default: return false;
            }
        }

        /// <summary>
        /// OnDroppedAction is used to focus the Element that dropped on List
        /// </summary>
        /// <param name="e">The ListBoxDroppedEventArgs.</param>
        private void OnDroppedAction(ListBoxDroppedEventArgs e)
        {
            try
            {
                if (!GeosApplication.Instance.IsPermissionReadOnly)
                {
                    GeosApplication.Instance.Logger.Log("Method OnDroppedAction ...", category: Category.Info, priority: Priority.Low);

                    ListBoxEdit sourceListBoxEdit = (ListBoxEdit)e.SourceControl;
                    ListBoxEdit destListBoxEdit = (ListBoxEdit)(e.ListBoxEdit);

                    // Source Pipeline is WON then It is not possible to move from WON.
                    if (((SalesStatusType)(sourceListBoxEdit.Tag)).IdSalesStatusType == 4)
                    {
                        return;
                    }

                    // Destination Pipeline is WON then It is not possible to move to WON.
                    if (((SalesStatusType)(destListBoxEdit.Tag)).IdSalesStatusType == 4)
                    {
                        return;
                    }

                    var items = (IList<object>)e.DraggedRows;
                    CurrentItem = (Offer)items.Last();

                    // Added as Lost DailogWindow is Canceled then not to change Focus
                    if (!IsLostCanceled)
                    {
                        // Destination ListBox.
                        SalesStatusType destSalesStatusType = (SalesStatusType)(destListBoxEdit.Tag);
                        CurrentItem.GeosStatus = destSalesStatusType.GeosStatus;

                        destSalesStatusType.TotalAmount = Math.Round(((ObservableCollection<Offer>)destSalesStatusType.OffersInObject).Select(v => v.Value).Sum(), 2);

                        // Source ListBox.
                        SalesStatusType sourceSalesStatusType = (SalesStatusType)(sourceListBoxEdit.Tag);
                        sourceSalesStatusType.TotalAmount = Math.Round(((ObservableCollection<Offer>)sourceSalesStatusType.OffersInObject).Select(v => v.Value).Sum(), 2);

                        destListBoxEdit.SelectedItem = CurrentItem;
                        destListBoxEdit.Focus();
                    }

                    GeosApplication.Instance.Logger.Log("Method OnDroppedAction() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnDroppedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for add new activity.
        /// </summary>
        /// <param name="obj"></param>
        private void AddActivityViewWindowShow(Offer offer, int idSite, ActivityTemplate activityTemplate)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddActivityViewWindowShow...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                AddActivityView addActivityView = new AddActivityView();
                AddActivityViewModel addActivityViewModel = new AddActivityViewModel();

                List<Activity> _ActivityList = new List<Activity>();

                //**[Start] code for add Account Detail.

                Activity _Activity = new Activity();
                _Activity.ActivityLinkedItem = new List<ActivityLinkedItem>();

                //Fill Account details.
                ActivityLinkedItem _AliAccount = new ActivityLinkedItem();
                _AliAccount.Company = new Company();
                _AliAccount.Company.Customers = new List<Customer>();
                _AliAccount.IdLinkedItemType = 42;
                _AliAccount.Company = offer.Site;
                _AliAccount.Company.Customers.Add(offer.Site.Customers[0]);
                _AliAccount.IdSite = offer.Site.IdCompany;
                _AliAccount.Name = offer.Site.Customers[0].CustomerName + " - " + offer.Site.Name;

                _AliAccount.LinkedItemType = new LookupValue();
                _AliAccount.LinkedItemType.IdLookupValue = 42;
                _AliAccount.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

                _AliAccount.IsVisible = false;
                _Activity.ActivityLinkedItem.Add(_AliAccount);

                //Fill Opportunity details.
                ActivityLinkedItem _aliOpportunity = new ActivityLinkedItem();
                _aliOpportunity.IdLinkedItemType = 44;
                _aliOpportunity.Name = offer.Code;
                _aliOpportunity.IdSite = null;
                _aliOpportunity.IdOffer = offer.IdOffer;
                _aliOpportunity.IdEmdepSite = idSite;

                _aliOpportunity.LinkedItemType = new LookupValue();
                _aliOpportunity.LinkedItemType.IdLookupValue = 44;
                _aliOpportunity.LinkedItemType.Value = "Opportunity";

                _aliOpportunity.IsVisible = false;
                _Activity.ActivityLinkedItem.Add(_aliOpportunity);

                _Activity.Location = offer.Site.Address;
                _Activity.Latitude = offer.Site.Latitude;
                _Activity.Longitude = offer.Site.Longitude;

                _ActivityList.Add(_Activity);

                addActivityViewModel.IsAddedFromOutSide = true;
                addActivityViewModel.SelectedIndexCompanyGroup = addActivityViewModel.CompanyGroupList.IndexOf(addActivityViewModel.CompanyGroupList.FirstOrDefault(x => x.IdCustomer == offer.Site.Customers[0].IdCustomer));
                addActivityViewModel.SelectedIndexCompanyPlant = addActivityViewModel.CompanyPlantList.IndexOf(addActivityViewModel.CompanyPlantList.FirstOrDefault(x => x.IdCompany == offer.Site.IdCompany));

                addActivityViewModel.Init(_ActivityList);

                //if (IsActivityCreateFromSaveOffer)
                //{
                addActivityViewModel.SelectedIndexType = addActivityViewModel.TypeList.IndexOf(addActivityViewModel.TypeList.FirstOrDefault(tl => tl.IdLookupValue == activityTemplate.IdActivityType));
                addActivityViewModel.Subject = activityTemplate.Subject;
                addActivityViewModel.Description = activityTemplate.Description;
                addActivityViewModel.DueDate = GeosApplication.Instance.ServerDateTime.AddDays(activityTemplate.DueDaysAfterCreation);
                //}
                //**[End] code for add Account Detail.

                EventHandler handle = delegate { addActivityView.Close(); };
                addActivityViewModel.RequestClose += handle;
                addActivityView.DataContext = addActivityViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                addActivityView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method AddActivityViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddActivityViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for add new activity for offer.
        /// </summary>
        private void AddActivity(Offer offer, int idSite, ActivityTemplate activityTemplate)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCtivity ...", category: Category.Info, priority: Priority.Low);

                List<ActivityLinkedItem> listActivityLinkedItems = new List<ActivityLinkedItem>();
                List<LogEntriesByActivity> logEntriesByActivity = new List<LogEntriesByActivity>();
                Activity NewActivity = new Activity();

                NewActivity.IdActivityType = activityTemplate.IdActivityType;
                NewActivity.Subject = activityTemplate.Subject;
                NewActivity.Description = activityTemplate.Description;
                NewActivity.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                NewActivity.IsCompleted = 0;
                NewActivity.IdOwner = offer.IdSalesOwner.Value; //SalesOwnerList[SelectedIndexSalesOwner].IdPerson;               // ActivityOwnerList[SelectedIndexOwner].IdUser;
                NewActivity.ActivityTags = null;
                NewActivity.IsSentMail = 0;
                NewActivity.FromDate = GeosApplication.Instance.ServerDateTime.AddDays(activityTemplate.DueDaysAfterCreation);
                NewActivity.ToDate = GeosApplication.Instance.ServerDateTime.AddDays(activityTemplate.DueDaysAfterCreation);

                //Fill Account details.
                ActivityLinkedItem _aliAccount = new ActivityLinkedItem();
                _aliAccount.IdLinkedItemType = 42;


                // For Add Attendies
                List<ActivityAttendees> listattendees = new List<ActivityAttendees>();
                listattendees.Add(new ActivityAttendees() { IdUser = offer.IdSalesOwner.Value });
                NewActivity.ActivityAttendees = listattendees;

                //_aliAccount.Company = new Company();
                //_aliAccount.Company.Customers = new List<Customer>();
                //_aliAccount.Company = offer.Site;
                //_aliAccount.Company.Customers.Add(offer.Site.Customers[0]);

                _aliAccount.IdSite = offer.Site.IdCompany;
                _aliAccount.Name = offer.Site.Customers[0].CustomerName + " - " + offer.Site.Name;

                //_aliAccount.LinkedItemType = new LookupValue();
                //_aliAccount.LinkedItemType.IdLookupValue = 42;
                //_aliAccount.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();

                _aliAccount.IsVisible = false;
                listActivityLinkedItems.Add(_aliAccount);

                //Fill Opportunity details.
                ActivityLinkedItem _aliOpportunity = new ActivityLinkedItem();
                _aliOpportunity = (ActivityLinkedItem)_aliAccount.Clone();
                _aliOpportunity.IdLinkedItemType = 44;
                _aliOpportunity.Name = offer.Code;
                _aliOpportunity.IdSite = null;
                _aliOpportunity.IdOffer = offer.IdOffer;
                _aliOpportunity.IdEmdepSite = idSite;

                //_aliOpportunity.LinkedItemType = new LookupValue();
                //_aliOpportunity.LinkedItemType.IdLookupValue = 44;
                //_aliOpportunity.LinkedItemType.Value = "Opportunity";

                listActivityLinkedItems.Add(_aliOpportunity);

                foreach (ActivityLinkedItem item in listActivityLinkedItems)
                {
                    item.ActivityLinkedItemImage = null;

                    if (item.IdLinkedItemType == 42)        //Account
                    {
                        logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddAccount").ToString(), item.Name), IdLogEntryType = 2 });
                    }
                    else if (item.IdLinkedItemType == 44)   // Opportunity
                    {
                        logEntriesByActivity.Add(new LogEntriesByActivity() { IdActivity = NewActivity.IdActivity, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityChangeLogAddOpportunity").ToString(), item.Name), IdLogEntryType = 2 });
                    }
                }

                NewActivity.LogEntriesByActivity = logEntriesByActivity;
                NewActivity.ActivityLinkedItem = listActivityLinkedItems;
                NewActivity.IsDeleted = 0;
                NewActivity = CrmStartUp.AddActivity(NewActivity);

                GeosApplication.Instance.Logger.Log("Method AddCtivity() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCtivity() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCtivity() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddCtivity() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
