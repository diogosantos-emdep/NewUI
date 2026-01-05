using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AttachToOpportunityViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Service

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        private string emailItemPath;
        private string emailItemSubject;
        private object selectedOffer;
        public bool IsSave { get; set; }
        private bool isBusy;
        private List<TimelineGrid> opportunityList;
        //private List<Tag> tempTagList;
        #endregion

        #region Properties

        public List<TimelineGrid> OpportunityList
        {
            get { return opportunityList; }
            set
            {
                opportunityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OpportunityList"));
            }
        }

        public object SelectedOffer
        {
            get { return selectedOffer; }
            set
            {
                selectedOffer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOffer"));
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

        #endregion


        #region ICommands

        public ICommand AttachToOpportunityViewAcceptButtonCommand { get; set; }
        public ICommand AttachToOpportunityViewCancelButtonCommand { get; set; }

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

        #region Constructor

        public AttachToOpportunityViewModel(string Subject, string mailPath)
        {
            try
            {
                emailItemPath = mailPath;
                emailItemSubject = Subject;
                CRMCommon.Instance.Init();
                GeosApplication.Instance.Logger.Log("Constructor AttachToOpportunityViewModel ...", category: Category.Info, priority: Priority.Low);
                FillOpportunities();

                AttachToOpportunityViewAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(AttachOpportunityAccept);
                AttachToOpportunityViewCancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);

                GeosApplication.Instance.Logger.Log("Constructor AttachToOpportunityViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AttachToOpportunityViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
        private void FillOpportunities()
        {
            //TimelineParams objTimelineParams = new TimelineParams();
            OpportunityList = new List<TimelineGrid>();
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
                            List<TimelineGrid> timelineGridListtemp = new List<TimelineGrid>();
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

                            timelineGridListtemp = CrmStartUp.GetOpportunities(objTimelineParams).ToList();
                            OpportunityList.AddRange(timelineGridListtemp);
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                //    FailedPlants.Add(itemPlantOwnerUsers.ShortName);
                                //if (FailedPlants != null && FailedPlants.Count > 0)
                                //{
                                //    IsShowFailedPlantWarning = true;
                                //    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                //    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                //}
                                System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                //    FailedPlants.Add(itemPlantOwnerUsers.ShortName);
                                //if (FailedPlants != null && FailedPlants.Count > 0)
                                //{
                                //    IsShowFailedPlantWarning = true;
                                //    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                //    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                //}
                                System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                //    FailedPlants.Add(itemPlantOwnerUsers.ShortName);

                                //if (FailedPlants != null && FailedPlants.Count > 0)
                                //{
                                //    IsShowFailedPlantWarning = true;
                                //    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                //    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                                //}

                                System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;

                    //if (FailedPlants == null || FailedPlants.Count == 0)
                    //{
                    //    IsShowFailedPlantWarning = false;
                    //    WarningFailedPlants = string.Empty;
                    //}
                }
            }
            else if (GeosApplication.Instance.IdUserPermission == 21)
            {
                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                    foreach (var itemCompaniesDetails in GeosApplication.Instance.CompanyList)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemCompaniesDetails.Alias;

                            //==========================================================================================
                            List<TimelineGrid> timelineGridListtemp = new List<TimelineGrid>();
                            TimelineParams objTimelineParams = new TimelineParams();

                            objTimelineParams.idCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                            objTimelineParams.idsSelectedUser = salesOwnersIds;
                            objTimelineParams.idUser = GeosApplication.Instance.ActiveUser.IdUser;
                            objTimelineParams.idZone = GeosApplication.Instance.ActiveUser.Company.Country.IdZone;
                            //[001] Added 
                            objTimelineParams.activeSite = new ActiveSite { IdSite = Convert.ToInt32(itemCompaniesDetails.ConnectPlantId), SiteAlias = Convert.ToString(itemCompaniesDetails.Alias), SiteServiceProvider = itemCompaniesDetails.ServiceProviderUrl };
                            objTimelineParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                            objTimelineParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                            objTimelineParams.Roles = RoleType.SalesPlantManager;

                            timelineGridListtemp = CrmStartUp.GetOpportunities(objTimelineParams).ToList();
                            //==========================================================================================
                            OpportunityList.AddRange(timelineGridListtemp);
                            //offersOptionsLst = CrmStartUp.GetSelectedUsersOffersWithoutPurchaseOrderReturnListDatatable(GeosApplication.Instance.IdCurrencyByRegion, salesOwnersIds, GeosApplication.Instance.CrmOfferYear, itemCompaniesDetails, GeosApplication.Instance.ActiveUser.IdUser);
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                            //if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            //    FailedPlants.Add(itemCompaniesDetails.ShortName);
                            //if (FailedPlants != null && FailedPlants.Count > 0)
                            //{
                            //    IsShowFailedPlantWarning = true;
                            //    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            //    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            //}
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                            System.Threading.Thread.Sleep(1000);
                            //if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            //    FailedPlants.Add(itemCompaniesDetails.ShortName);
                            //if (FailedPlants != null && FailedPlants.Count > 0)
                            //{
                            //    IsShowFailedPlantWarning = true;
                            //    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            //    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            //}
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                            //if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            //    FailedPlants.Add(itemCompaniesDetails.ShortName);
                            //if (FailedPlants != null && FailedPlants.Count > 0)
                            //{
                            //    IsShowFailedPlantWarning = true;
                            //    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            //    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            //}
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;

                    //if (FailedPlants == null || FailedPlants.Count == 0)
                    //{
                    //    IsShowFailedPlantWarning = false;
                    //    WarningFailedPlants = string.Empty;
                    //}
                }
            }
            else                      //IdPermission 20
            {
                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    //PreviouslySelectedSalesOwners = salesOwnersIds;

                    foreach (var itemCompaniesDetails in GeosApplication.Instance.CompanyList)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemCompaniesDetails.Alias;

                            //==========================================================================================
                            List<TimelineGrid> timelineGridListtemp = new List<TimelineGrid>();
                            TimelineParams objTimelineParams = new TimelineParams();

                            objTimelineParams.idCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                            objTimelineParams.idsSelectedUser = GeosApplication.Instance.ActiveUser.IdUser.ToString();// salesOwnersIds;
                            objTimelineParams.idUser = GeosApplication.Instance.ActiveUser.IdUser;
                            objTimelineParams.idZone = GeosApplication.Instance.ActiveUser.Company.Country.IdZone;
                            //[001] Added 
                            objTimelineParams.activeSite = new ActiveSite { IdSite = Convert.ToInt32(itemCompaniesDetails.ConnectPlantId), SiteAlias = Convert.ToString(itemCompaniesDetails.Alias), SiteServiceProvider = itemCompaniesDetails.ServiceProviderUrl };
                            objTimelineParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                            objTimelineParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                            objTimelineParams.Roles = RoleType.SalesAssistant;

                            timelineGridListtemp = CrmStartUp.GetOpportunities(objTimelineParams).ToList();
                            //==========================================================================================
                            OpportunityList.AddRange(timelineGridListtemp);
                            //offersOptionsLst = CrmStartUp.GetSelectedUsersOffersWithoutPurchaseOrderReturnListDatatable(GeosApplication.Instance.IdCurrencyByRegion, salesOwnersIds, GeosApplication.Instance.CrmOfferYear, itemCompaniesDetails, GeosApplication.Instance.ActiveUser.IdUser);
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                            //if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            //    FailedPlants.Add(itemCompaniesDetails.ShortName);
                            //if (FailedPlants != null && FailedPlants.Count > 0)
                            //{
                            //    IsShowFailedPlantWarning = true;
                            //    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            //    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            //}
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                            System.Threading.Thread.Sleep(1000);
                            //if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            //    FailedPlants.Add(itemCompaniesDetails.ShortName);
                            //if (FailedPlants != null && FailedPlants.Count > 0)
                            //{
                            //    IsShowFailedPlantWarning = true;
                            //    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            //    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            //}
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed");
                            //if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                            //    FailedPlants.Add(itemCompaniesDetails.ShortName);
                            //if (FailedPlants != null && FailedPlants.Count > 0)
                            //{
                            //    IsShowFailedPlantWarning = true;
                            //    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                            //    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            //}
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemCompaniesDetails.Alias, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;

                    //if (FailedPlants == null || FailedPlants.Count == 0)
                    //{
                    //    IsShowFailedPlantWarning = false;
                    //    WarningFailedPlants = string.Empty;
                    //}
                }
            }
            #endregion

            //GeosApplication.Instance.Logger.Log("Method FillLeadsByUser() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for add new tag.
        /// </summary>
        /// <param name="obj"></param>
        public void AttachOpportunityAccept(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("AttachOpportunityAccept() Method ...", category: Category.Info, priority: Priority.Low);

                TimelineGrid timelineGrid = SelectedOffer as TimelineGrid;

                Offer offer = new Offer();
                offer.IdOffer = timelineGrid.IdOffer;
                offer.Code = timelineGrid.Code;
                offer.Year = timelineGrid.Year;

                offer.OutLookMailCopy = new Data.Common.File.FileDetail();
                offer.OutLookMailCopy.FileName = emailItemSubject + ".msg";
                offer.OutLookMailCopy.FileByte = System.IO.File.ReadAllBytes(emailItemPath);

                offer.LogEntryByOffers = new List<LogEntryByOffer>();

                LogEntryByOffer comment = new LogEntryByOffer();
                comment.Comments = emailItemSubject;
                comment.IdLogEntryType = 1;
                comment.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                comment.IsRtfText = false;
                comment.IdOffer = offer.IdOffer;

                offer.LogEntryByOffers.Add(comment);

                LogEntryByOffer logEntry = new LogEntryByOffer();
                logEntry.Comments = "The offer attachment from outlook has been added.";
                logEntry.IdLogEntryType = 9;
                logEntry.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                logEntry.IsRtfText = false;
                logEntry.IdOffer = offer.IdOffer;

                offer.LogEntryByOffers.Add(logEntry);

                IsSave = CrmStartUp.UpdateAttachOffer(offer);

                if (IsSave)
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditOfferAttachmentAddedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AttachOpportunityAccept() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AttachOpportunityAccept() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AttachOpportunityAccept() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AttachOpportunityAccept() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for search similar word.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        //double StringSimilarityScore(string name, string searchString)
        //{
        //    if (name.Contains(searchString))
        //    {
        //        return (double)searchString.Length / (double)name.Length;
        //    }

        //    return 0;
        //}
        private void ShowPopupAsPerTagName(string ProjectName)
        {
            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerTagName ...", category: Category.Info, priority: Priority.Low);

            //TagNameList = CrmStartUp.GetAllTags().ToList();

            //if (TagNameList != null && !string.IsNullOrEmpty(TagName))
            //{
            //    if (TagName.Length > 1)
            //    {
            //        TagNameList = TagNameList.Where(h => h.Name.ToUpper().Contains(TagName.ToUpper()) || h.Name.ToUpper().StartsWith(TagName.Substring(0, 2).ToUpper())
            //                                                || h.Name.ToUpper().EndsWith(TagName.Substring(TagName.Length - 2).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, TagName)).ToList();
            //        TagNameStrList = TagNameList.Select(pn => pn.Name).ToList();
            //    }
            //    else
            //    {
            //        TagNameList = TagNameList.Where(h => h.Name.ToUpper().Contains(TagName.ToUpper()) || h.Name.ToUpper().StartsWith(TagName.Substring(0, 1).ToUpper())
            //                                                || h.Name.ToUpper().EndsWith(TagName.Substring(TagName.Length - 1).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, TagName)).ToList();
            //        TagNameStrList = TagNameList.Select(pn => pn.Name).ToList();

            //    }
            //}

            //else
            //{
            //    TagNameList = new List<Tag>();
            //    TagNameStrList = new List<string>();
            //}

            ////For alert Icon visibility
            //if (TagNameStrList.Count > 0)
            //{
            //    AlertVisibility = Visibility.Visible;
            //}
            //else
            //    AlertVisibility = Visibility.Hidden;

            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerTagName() executed successfully", category: Category.Info, priority: Priority.Low);

        }

        /// <summary>
        /// Method for close window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            IsSave = false;
            //TagName = string.Empty;
            RequestClose(null, null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


    }
}
