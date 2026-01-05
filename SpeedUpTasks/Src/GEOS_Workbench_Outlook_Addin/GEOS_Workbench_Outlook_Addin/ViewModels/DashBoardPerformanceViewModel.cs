using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class DahsBoardPerformanceViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        private double successRate;
        private double targetAchieved;
        private string strSuccessRate;
        private string strTargetAchieved;
        private ObservableCollection<Offer> offerLeadSourcelst;
        public IList<SalesStatusType> SalesStatusTypesPipeline { get; set; }
        // private ObservableCollection<SalesStatusType> finalSalesStatusTypesPipeline;
        private ObservableCollection<LostReasonsByOffer> lostReasonsByOfferlst;

        List<Offer> salesPerformanceList;
        //List<Offer> offersPipelineLogentryList;
        List<string> failedPlants;
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;

        #endregion // Declaration

        #region public Properties

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


        public double SuccessRate
        {
            get
            {
                return successRate;
            }

            set
            {
                successRate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SuccessRate"));
            }
        }

        public double TargetAchieved
        {
            get
            {
                return targetAchieved;
            }

            set
            {
                targetAchieved = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TargetAchieved"));
            }
        }

        public string StrTargetAchieved
        {
            get
            {
                return strTargetAchieved;
            }

            set
            {
                strTargetAchieved = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StrTargetAchieved"));
            }
        }

        public string StrSuccessRate
        {
            get
            {
                return strSuccessRate;
            }

            set
            {
                strSuccessRate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StrSuccessRate"));
            }
        }

        public List<Offer> SalesPerformanceList
        {
            get { return salesPerformanceList; }
            set { salesPerformanceList = value; }
        }

        //public List<Offer> OffersPipelineLogentryList
        //{
        //    get
        //    {
        //        return offersPipelineLogentryList;
        //    }

        //    set
        //    {
        //        offersPipelineLogentryList = value;
        //    }
        //}


        public ObservableCollection<Offer> OfferLeadSourcelst
        {
            get { return offerLeadSourcelst; }
            set
            {
                offerLeadSourcelst = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferLeadSourcelst"));
            }
        }

        //public ObservableCollection<SalesStatusType> FinalSalesStatusTypesPipeline
        //{
        //    get { return finalSalesStatusTypesPipeline; }
        //    set
        //    {
        //        finalSalesStatusTypesPipeline = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("FinalSalesStatusTypesPipeline"));
        //    }
        //}

        public ObservableCollection<LostReasonsByOffer> LostReasonsByOfferlst
        {
            get { return lostReasonsByOfferlst; }
            set
            {
                lostReasonsByOfferlst = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LostReasonsByOfferlst"));
            }
        }

        #endregion // Properties

        #region Commands

        public ICommand SalesOwnerPopupClosedCommand { get; private set; }
        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        public ICommand RefreshDahsBoardPerformanceViewCommand { get; set; }

        #endregion // Commands

        #region public event

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Constructor

        public DahsBoardPerformanceViewModel()
        {
            FailedPlants = new List<string>();
            IsShowFailedPlantWarning = false;
            WarningFailedPlants = String.Empty;

            GeosApplication.Instance.Logger.Log("Constructor DahsBoardPerformanceViewModel ...", category: Category.Info, priority: Priority.Low);
            GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
            GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);

            SalesOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(SalesOwnerPopupClosedCommandAction);
            PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
            RefreshDahsBoardPerformanceViewCommand = new DevExpress.Mvvm.DelegateCommand<object>(RefreshDashBoardPerformanceDetails);

            FillCmbSalesOwner();

            GeosApplication.Instance.Logger.Log("Constructor DahsBoardPerformanceViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion //Constructor

        #region Method

        /// <summary>
        /// Method for Fill Dashboard Sales details.
        /// </summary>
        private void FillCmbSalesOwner()
        {
            GeosApplication.Instance.Logger.Log("Method FillCmbSalesOwner ...", category: Category.Info, priority: Priority.Low);

            /* This Section for Sales Owner, Period and Remaining Day Header */


            if (GeosApplication.Instance.IdUserPermission == 21)
            {

                // Call for 1st time.
                FillDashBoardPerformanceDetailsByUsers();
            }
            else if (GeosApplication.Instance.IdUserPermission == 22)
            {
                FillDashBoardPerformanceDetailsByPlant();
            }
            else
            {
                FillDashBoardPerformanceDetails();
            }
            /* This Section for Sales Owner, Period and Remaining Day Header*/

            GeosApplication.Instance.Logger.Log("Constructor FillCmbSalesOwner executed Successfully!", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for refresh DahsBoard Performance details.
        /// </summary>
        private void FillDashBoardPerformanceDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDashBoardPerformanceDetails ...", category: Category.Info, priority: Priority.Low);

                SalesPerformanceList = new List<Offer>();
                List<Offer> tempOfferLeadSourcelst = new List<Offer>();
                List<LostReasonsByOffer> tempLostReasonsByOfferlst = new List<LostReasonsByOffer>();

                // Continue loop although some plant is not available and Show error message.
                foreach (var item in GeosApplication.Instance.CompanyList)
                {
                    try
                    {
                        GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.Alias;

                        SalesPerformanceList.AddRange(CrmStartUp.GetSalesStatus_V2035(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                GeosApplication.Instance.ActiveUser.IdUser.ToString(),
                                                                                GeosApplication.Instance.SelectedyearStarDate, 
                                                                                GeosApplication.Instance.SelectedyearEndDate,
                                                                                item, GeosApplication.Instance.ActiveUser.IdUser,
                                                                                GeosApplication.Instance.IdUserPermission).ToList());

                        tempOfferLeadSourcelst.AddRange(CrmStartUp.GetOfferLeadSourceCompanyWise(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                                GeosApplication.Instance.ActiveUser.IdUser,
                                                                                                GeosApplication.Instance.ActiveUser.Company.Country.IdZone,
                                                                                                 GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                                                item,
                                                                                                GeosApplication.Instance.IdUserPermission));

                        tempLostReasonsByOfferlst.AddRange(CrmStartUp.GetOfferLostReasonsDetailsCompanyWise(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                                            GeosApplication.Instance.ActiveUser.IdUser,
                                                                                                            GeosApplication.Instance.ActiveUser.Company.Country.IdZone,
                                                                                                           GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                                                            item,
                                                                                                            GeosApplication.Instance.IdUserPermission));
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

                CalculateSalesPerformance();

                // Lead Source
                OfferLeadSourcelst = new ObservableCollection<Offer>();
                foreach (var item in tempOfferLeadSourcelst.GroupBy(ols => ols.IdSource))
                {
                    Offer tempOffer = new Offer();
                    tempOffer.NumberOfOffers = tempOfferLeadSourcelst.Where(off => off.IdSource == item.Key.Value).Select(offs => offs.NumberOfOffers).Sum();
                    tempOffer.IdSource = item.Key.Value;
                    tempOffer.Source = new Data.Common.Epc.LookupValue();
                    tempOffer.Source = tempOfferLeadSourcelst.Where(off => off.IdSource == item.Key.Value).Select(tolsl => tolsl.Source).FirstOrDefault();
                    OfferLeadSourcelst.Add(tempOffer);
                }

                // Lost Reason
                LostReasonsByOfferlst = new ObservableCollection<LostReasonsByOffer>();
                foreach (var item in tempLostReasonsByOfferlst.GroupBy(ols => ols.OfferLostReason.IdLostReason))
                {
                    LostReasonsByOffer tempLostReasonsByOffer = new LostReasonsByOffer();
                    tempLostReasonsByOffer.NumberOfOffers = tempLostReasonsByOfferlst.Where(off => off.OfferLostReason.IdLostReason == item.Key).Select(offs => offs.NumberOfOffers).Sum();
                    tempLostReasonsByOffer.OfferLostReason = tempLostReasonsByOfferlst.Where(off => off.OfferLostReason.IdLostReason == item.Key).Select(offs => offs.OfferLostReason).FirstOrDefault();
                    LostReasonsByOfferlst.Add(tempLostReasonsByOffer);
                }

                GeosApplication.Instance.Logger.Log("Constructor DashBoardPerformanceViewModel executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in DahsBoardPerformanceViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in DahsBoardPerformanceViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in DahsBoardPerformanceViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        /// <summary>
        /// Method for refresh DahsBoard Performance details by plant.
        /// </summary>
        private void FillDashBoardPerformanceDetailsByPlant()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDashBoardPerformanceDetailsByPlant ...", category: Category.Info, priority: Priority.Low);

                SalesPerformanceList = new List<Offer>();
                List<Offer> tempOfferLeadSourcelst = new List<Offer>();
                List<LostReasonsByOffer> tempLostReasonsByOfferlst = new List<LostReasonsByOffer>();

                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ShortName));
                    PreviouslySelectedPlantOwners = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                    // Continue loop although some plant is not available and Show error message.
                    foreach (var item in plantOwners)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.ShortName;

                            SalesPerformanceList.AddRange(CrmStartUp.GetSalesStatus_V2035(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                    GeosApplication.Instance.ActiveUser.IdUser.ToString(),
                                                                                  
                                                                                    GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                                    item, GeosApplication.Instance.ActiveUser.IdUser,
                                                                                    GeosApplication.Instance.IdUserPermission).ToList());

                            tempOfferLeadSourcelst.AddRange(CrmStartUp.GetOfferLeadSourceCompanyWise(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                                     GeosApplication.Instance.ActiveUser.IdUser,
                                                                                                     GeosApplication.Instance.ActiveUser.Company.Country.IdZone,
                                                                                                      GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                                                     item,
                                                                                                     GeosApplication.Instance.IdUserPermission));

                            tempLostReasonsByOfferlst.AddRange(CrmStartUp.GetOfferLostReasonsDetailsCompanyWise(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                                                GeosApplication.Instance.ActiveUser.IdUser,
                                                                                                                GeosApplication.Instance.ActiveUser.Company.Country.IdZone,
                                                                                                               GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate,
                                                                                                                item,
                                                                                                                GeosApplication.Instance.IdUserPermission));
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.ShortName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(item.ShortName);
                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ShortName, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.ShortName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(item.ShortName);

                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }

                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", item.ShortName, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                FailedPlants.Add(item.ShortName);

                            if (FailedPlants != null && FailedPlants.Count > 0)
                            {
                                IsShowFailedPlantWarning = true;
                                WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                                WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", item.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;

                    if (FailedPlants == null || FailedPlants.Count == 0)
                    {
                        IsShowFailedPlantWarning = false;
                        WarningFailedPlants = string.Empty;
                    }
                }
                else
                {
                    SalesPerformanceList = new List<Offer>();
                    tempOfferLeadSourcelst = new List<Offer>();
                    tempLostReasonsByOfferlst = new List<LostReasonsByOffer>();
                }

                CalculateSalesPerformanceByUsers();

                //  FinalStatusWiseList();
                //FinalSalesStatusTypesPipeline = SalesStatusTypesPipeline.OrderByDescending(i => i.TotalAmount).ToList();
                //FinalSalesStatusTypesPipeline = new ObservableCollection<SalesStatusType>(SalesStatusTypesPipeline.ToList());

                // Lead Source
                OfferLeadSourcelst = new ObservableCollection<Offer>();
                foreach (var item in tempOfferLeadSourcelst.GroupBy(ols => ols.IdSource))
                {
                    Offer tempOffer = new Offer();
                    tempOffer.NumberOfOffers = tempOfferLeadSourcelst.Where(off => off.IdSource == item.Key.Value).Select(offs => offs.NumberOfOffers).Sum();
                    tempOffer.IdSource = item.Key.Value;
                    tempOffer.Source = new Data.Common.Epc.LookupValue();
                    tempOffer.Source = tempOfferLeadSourcelst.Where(off => off.IdSource == item.Key.Value).Select(tolsl => tolsl.Source).FirstOrDefault();
                    OfferLeadSourcelst.Add(tempOffer);
                }

                // Lost Reason
                LostReasonsByOfferlst = new ObservableCollection<LostReasonsByOffer>();
                foreach (var item in tempLostReasonsByOfferlst.GroupBy(ols => ols.OfferLostReason.IdLostReason))
                {
                    LostReasonsByOffer tempLostReasonsByOffer = new LostReasonsByOffer();
                    tempLostReasonsByOffer.NumberOfOffers = tempLostReasonsByOfferlst.Where(off => off.OfferLostReason.IdLostReason == item.Key).Select(offs => offs.NumberOfOffers).Sum();
                    tempLostReasonsByOffer.OfferLostReason = tempLostReasonsByOfferlst.Where(off => off.OfferLostReason.IdLostReason == item.Key).Select(offs => offs.OfferLostReason).FirstOrDefault();
                    LostReasonsByOfferlst.Add(tempLostReasonsByOffer);
                }

                GeosApplication.Instance.Logger.Log("Constructor FillDashBoardPerformanceDetailsByPlant executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillDashBoardPerformanceDetailsByPlant() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillDashBoardPerformanceDetailsByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillDashBoardPerformanceDetailsByPlant() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for refresh DahsBoard Performance details.
        /// </summary>
        private void FillDashBoardPerformanceDetailsByUsers()
        {
            GeosApplication.Instance.Logger.Log("Method FillDashBoardPerformanceDetailsByUsers ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SalesPerformanceList = new List<Offer>();
                List<Offer> tempOfferLeadSourcelst = new List<Offer>();
                List<LostReasonsByOffer> tempLostReasonsByOfferlst = new List<LostReasonsByOffer>();

                var salesOwnersIds = "";
                if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    PreviouslySelectedSalesOwners = salesOwnersIds;

                    // Continue loop although some plant is not available and Show error message.
                    foreach (var item in GeosApplication.Instance.CompanyList)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + item.Alias;

                            SalesPerformanceList.AddRange(CrmStartUp.GetSalesStatus_V2035(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                               salesOwnersIds,
                                                                                               GeosApplication.Instance.SelectedyearStarDate,
                                                                                               GeosApplication.Instance.SelectedyearEndDate,
                                                                                               item,
                                                                                               GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission).ToList());

                            tempOfferLeadSourcelst.AddRange(CrmStartUp.GetSelectedUsersOfferLeadSourceCompanyWise(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                                                 salesOwnersIds,
                                                                                                                 GeosApplication.Instance.SelectedyearStarDate,
                                                                                                                 GeosApplication.Instance.SelectedyearEndDate,
                                                                                                                 item,
                                                                                                                 GeosApplication.Instance.ActiveUser.IdUser));

                            tempLostReasonsByOfferlst.AddRange(CrmStartUp.GetSelectedUsersOfferLostReasonsDetailsCompanyWise(GeosApplication.Instance.IdCurrencyByRegion,
                                                                                                                            GeosApplication.Instance.SelectedyearStarDate,
                                                                                                                            GeosApplication.Instance.SelectedyearEndDate,
                                                                                                                            salesOwnersIds,
                                                                                                                            item,
                                                                                                                            GeosApplication.Instance.ActiveUser.IdUser));
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
                }
                else
                {
                    SalesPerformanceList = new List<Offer>();
                    tempOfferLeadSourcelst = new List<Offer>();
                    tempLostReasonsByOfferlst = new List<LostReasonsByOffer>();
                }

                GeosApplication.Instance.SplashScreenMessage = string.Empty;

                if (FailedPlants == null || FailedPlants.Count == 0)
                {
                    IsShowFailedPlantWarning = false;
                    WarningFailedPlants = string.Empty;
                }

                CalculateSalesPerformanceByUsers();

                // FinalStatusWiseList();
                // FinalSalesStatusTypesPipeline = new ObservableCollection<SalesStatusType>(SalesStatusTypesPipeline.ToList());

                // Lead Source
                OfferLeadSourcelst = new ObservableCollection<Offer>();
                foreach (var item in tempOfferLeadSourcelst.GroupBy(ols => ols.IdSource))
                {
                    Offer tempOffer = new Offer();
                    tempOffer.NumberOfOffers = tempOfferLeadSourcelst.Where(off => off.IdSource == item.Key.Value).Select(offs => offs.NumberOfOffers).Sum();
                    tempOffer.IdSource = item.Key.Value;
                    tempOffer.Source = new Data.Common.Epc.LookupValue();
                    tempOffer.Source = tempOfferLeadSourcelst.Where(off => off.IdSource == item.Key.Value).Select(tolsl => tolsl.Source).FirstOrDefault();
                    OfferLeadSourcelst.Add(tempOffer);
                }

                // Lost Reason
                LostReasonsByOfferlst = new ObservableCollection<LostReasonsByOffer>();
                foreach (var item in tempLostReasonsByOfferlst.GroupBy(ols => ols.OfferLostReason.IdLostReason))
                {
                    LostReasonsByOffer tempLostReasonsByOffer = new LostReasonsByOffer();
                    tempLostReasonsByOffer.NumberOfOffers = tempLostReasonsByOfferlst.Where(off => off.OfferLostReason.IdLostReason == item.Key).Select(offs => offs.NumberOfOffers).Sum();
                    tempLostReasonsByOffer.OfferLostReason = tempLostReasonsByOfferlst.Where(off => off.OfferLostReason.IdLostReason == item.Key).Select(offs => offs.OfferLostReason).FirstOrDefault();
                    LostReasonsByOfferlst.Add(tempLostReasonsByOffer);
                }

                GeosApplication.Instance.Logger.Log("Constructor DashBoardPerformanceViewModel executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in DahsBoardPerformanceViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in DahsBoardPerformanceViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in DahsBoardPerformanceViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }


        //private void FinalStatusWiseList()
        //{
        //    SalesStatusTypesPipeline = CrmStartUp.GetAllSalesStatusType();
        //    List<GeosStatus> GeosStatusList = new List<GeosStatus>(CrmStartUp.GetGeosOfferStatus());

        //    foreach (var item in OffersPipelineLogentryList)
        //    {
        //        if (!string.IsNullOrEmpty(item.LogEntryByOffers[0].Comments))
        //        {
        //            string[] str = item.LogEntryByOffers[0].Comments.Split(':');

        //            // Get Last item in str array (Status)
        //            if (str.Count() > 0)
        //            {
        //                GeosStatus geosStatus = GeosStatusList.FirstOrDefault(x => x.Name == str[str.Count() - 1].Trim());
        //                if (geosStatus != null)
        //                {
        //                    SalesStatusType salesStatus = SalesStatusTypesPipeline.FirstOrDefault(x => x.IdSalesStatusType == geosStatus.IdSalesStatusType);
        //                    if (salesStatus != null)
        //                        salesStatus.NumberOfOffers = salesStatus.NumberOfOffers + 1;
        //                }
        //            }

        //            // Prev and This. Both logic are same
        //            //foreach (var itemss in SalesStatusTypesPipeline)
        //            //{
        //            //    foreach (var itemx in GeosStatusList.Where(x => x.IdSalesStatusType == itemss.IdSalesStatusType).ToList())
        //            //    {
        //            //        if (str.Count() > 0)
        //            //        {
        //            //            if (str[str.Count() - 1].Trim() == itemx.Name)
        //            //            {
        //            //                itemss.NumberOfOffers = itemss.NumberOfOffers + 1;
        //            //            }
        //            //        }
        //            //    }
        //            //}
        //        }
        //    }
        //}

        /// <summary>
        /// Method for refresh DahsBoard Performance From database by refresh Button.
        /// </summary>
        /// <param name="obj"></param>
        private void RefreshDashBoardPerformanceDetails(object obj)
        {
            FailedPlants = new List<string>();
            IsShowFailedPlantWarning = false;
            WarningFailedPlants = String.Empty;

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

            FillCmbSalesOwner();

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }

        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void SalesOwnerPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null && GeosApplication.Instance.SelectedSalesOwnerUsersList.Count > 0)
            {
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    //  DXSplashScreen.Show<SplashScreenView>(); 
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

                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

                FillDashBoardPerformanceDetailsByUsers();
                if (FailedPlants != null && FailedPlants.Count > 0)
                {

                    IsShowFailedPlantWarning = true;
                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                }


                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            else
            {
                SuccessRate = 0;
                TargetAchieved = 0;
                StrSuccessRate = "";
                StrTargetAchieved = "";
                //FinalSalesStatusTypesPipeline = new ObservableCollection<SalesStatusType>();
                OfferLeadSourcelst = new ObservableCollection<Offer>();
                LostReasonsByOfferlst = new ObservableCollection<LostReasonsByOffer>();
                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;

            }
        }

        /// <summary>
        /// This method is used to get data by salesowner group
        /// </summary>
        /// <param name="obj"></param>
        private void PlantOwnerPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }

            if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null && GeosApplication.Instance.SelectedPlantOwnerUsersList.Count > 0)
            {
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Show<SplashScreenView>(); }
                FailedPlants = new List<string>();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;
                FillDashBoardPerformanceDetailsByPlant();
                if (FailedPlants != null && FailedPlants.Count > 0)
                {

                    IsShowFailedPlantWarning = true;
                    WarningFailedPlants = string.Join(",", FailedPlants.Select(x => x.ToString()).ToArray());
                    WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), WarningFailedPlants.ToString());
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            else
            {
                SuccessRate = 0;
                TargetAchieved = 0;
                StrSuccessRate = "";
                StrTargetAchieved = "";
                //FinalSalesStatusTypesPipeline = new ObservableCollection<SalesStatusType>();
                OfferLeadSourcelst = new ObservableCollection<Offer>();
                LostReasonsByOfferlst = new ObservableCollection<LostReasonsByOffer>();
                FailedPlants.Clear();
                IsShowFailedPlantWarning = false;
                WarningFailedPlants = String.Empty;


            }
        }


        private void CalculateSalesPerformance()
        {
            try
            {
                //IList<SalesTargetBySite> SalesPerformanceTargetList = new List<SalesTargetBySite>();
                //SalesPerformanceTargetList = CrmStartUp.GetSalesStatusTargetDashboard(GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.CrmOfferYear, GeosApplication.Instance.IdUserPermission);
                //double TargetAmount = Convert.ToDouble(SalesPerformanceTargetList[0].TargetAmount);

                var salesOwnersIds = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                double TargetAmount = 0;

                //SalesUserQuota salesUserQuota = CrmStartUp.GetTotalSalesQuotaBySelectedUserIdAndYear(salesOwnersIds.ToString(), GeosApplication.Instance.IdCurrencyByRegion, Convert.ToInt32(GeosApplication.Instance.CrmOfferYear));
                SalesUserQuota salesUserQuota = CrmStartUp.GetTotalSalesQuotaBySelectedUserIdAndYearWithExchangeRate(salesOwnersIds.ToString(), GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate);

                TargetAmount = salesUserQuota.SalesQuotaAmountWithExchangeRate;

                double Won = SalesPerformanceList.Where(s => s.Status == "WON").Select(x => x.Value).ToList().Sum();
                double Lost = SalesPerformanceList.Where(s => s.Status == "LOST").Select(x => x.Value).ToList().Sum();
                SuccessRate = ((Won + Lost) > 0) ? Math.Round(((Won / (Won + Lost)) * 100), 2) : 0;
                StrSuccessRate = SuccessRate.ToString() + " %";

                if (TargetAmount == 0)
                {
                    TargetAchieved = Math.Round(0.00, 2);
                }
                else
                {
                    TargetAchieved = Math.Round(((Won / TargetAmount) * 100), 2);
                }

                StrTargetAchieved = TargetAchieved.ToString() + " %";
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CalculateSalesPerformance() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CalculateSalesPerformance() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CalculateSalesPerformance() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CalculateSalesPerformanceByUsers()
        {
            try
            {
                double TargetAmount = 0;
                //if User Permission in 22
                if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                    //PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = CrmStartUp.GetTotalPlantQuotaSelectedPlantWiseAndYear(plantOwnersIds, GeosApplication.Instance.IdCurrencyByRegion, Convert.ToInt32(GeosApplication.Instance.CrmOfferYear), GeosApplication.Instance.ActiveUser.IdUser);

                    PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = CrmStartUp.GetTotalPlantQuotaSelectedPlantWiseAndYearWithExchangeDate(plantOwnersIds, GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate, GeosApplication.Instance.ActiveUser.IdUser);
                    TargetAmount = plantBusinessUnitSalesQuota.QuotaAmountWithExchangeRate;
                }

                //if user User Permission in 20 and 21
                if (GeosApplication.Instance.IdUserPermission == 20 || GeosApplication.Instance.IdUserPermission == 21)
                {
                    var salesOwnersIds = "";

                    if (GeosApplication.Instance.IdUserPermission == 21)
                    {
                        List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                        salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    }

                    if (GeosApplication.Instance.IdUserPermission == 20)
                    {
                        salesOwnersIds = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                    }

                    //SalesUserQuota salesUserQuota = CrmStartUp.GetTotalSalesQuotaBySelectedUserIdAndYear(salesOwnersIds.ToString(), GeosApplication.Instance.IdCurrencyByRegion, Convert.ToInt32(GeosApplication.Instance.CrmOfferYear));
                    SalesUserQuota salesUserQuota = CrmStartUp.GetTotalSalesQuotaBySelectedUserIdAndYearWithExchangeRate(salesOwnersIds.ToString(), GeosApplication.Instance.IdCurrencyByRegion, GeosApplication.Instance.SelectedyearStarDate, GeosApplication.Instance.SelectedyearEndDate);
                    TargetAmount = salesUserQuota.SalesQuotaAmountWithExchangeRate;
                }

                double Won = SalesPerformanceList.Where(s => s.Status == "WON").Select(x => x.Value).ToList().Sum();
                double Lost = SalesPerformanceList.Where(s => s.Status == "LOST").Select(x => x.Value).ToList().Sum();
                SuccessRate = ((Won + Lost) > 0) ? Math.Round(((Won / (Won + Lost)) * 100), 2) : 0;
                StrSuccessRate = SuccessRate.ToString() + " %";

                if (TargetAmount == 0)
                {
                    TargetAchieved = Math.Round(0.00, 2);
                }
                else
                {
                    TargetAchieved = Math.Round(((Won / TargetAmount) * 100), 2);
                }

                StrTargetAchieved = TargetAchieved.ToString() + " %";
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CalculateSalesPerformanceByUsers() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CalculateSalesPerformanceByUsers() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CalculateSalesPerformanceByUsers() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // Method
    }

}
