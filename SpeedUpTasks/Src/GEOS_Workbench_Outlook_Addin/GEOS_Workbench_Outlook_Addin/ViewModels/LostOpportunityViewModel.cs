using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    class LostOpportunityViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged
    {
        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services


        #region task log
        // [001][skale][30-09-2019][GEOS2-1756] Add the possibility to Edit the Offer LOST Date
        #endregion

        #region Declaration

        List<OfferLostReason> offerLostReasons;
        List<Competitor> competitors;
        string lostReasonDescription;
        public long IdOffer;
        List<object> selectedItems;
        List<object> selectedItemsCompetitors;
        public LostReasonsByOffer LostReasonsByOffer;
        private IList<Offer> offer;
        bool isBusy;

        // DateTime ? LostReasonDescription

        //[001] added
        private DateTime? offerLostDate;
        private DateTime? maxLostDate;
        private DateTime ? offerCloseDate;

      


        #endregion // Declaration

        #region  public Properties

        public bool IsCancel { get; set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public List<OfferLostReason> OfferLostReasons
        {
            get { return offerLostReasons; }
            set { offerLostReasons = value; }
        }

        public List<Competitor> Competitors
        {
            get { return competitors; }
            set { competitors = value; }
        }

        private Int32 selectedIndexCompetitors = -1;
        public Int32 SelectedIndexCompetitors
        {
            get { return selectedIndexCompetitors; }
            set
            {
                selectedIndexCompetitors = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompetitors"));
            }
        }
        public List<object> SelectedItems
        {
            get { return selectedItems; }
            set
            {
                SetProperty(ref selectedItems, value, () => SelectedItems);
            }
        }

        public string LostReasonDescription
        {
            get { return lostReasonDescription; }
            set
            {
                lostReasonDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LostReasonDescription"));
            }
        }

        public IList<Offer> Offer
        {
            get { return offer; }
            set { offer = value; }
        }
        //[001]added
        public DateTime? OfferLostDate
        {
            get { return offerLostDate; }
            set {
                   offerLostDate = value;
                  OnPropertyChanged(new PropertyChangedEventArgs("OfferLostDate"));
            }
        }

        public DateTime? MaxLostDate
        {
            get { return maxLostDate; }
            set {
                  maxLostDate = value;
                  OnPropertyChanged(new PropertyChangedEventArgs("MaxLostDate"));
            }
        }

        public DateTime? OfferCloseDate
        {
            get { return offerCloseDate; }
            set {
                 offerCloseDate = value;
                 OnPropertyChanged(new PropertyChangedEventArgs("OfferCloseDate"));
            }
        }

        #endregion // Properties

        #region public ICommand

        public ICommand LostOpportunityViewCancelButtonCommand { get; set; }
        public ICommand LostOpportunityViewAcceptButtonCommand { get; set; }
        public ICommand SelectedReasonIndexChangedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion // Command

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

        #region Constructor
       
        public LostOpportunityViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor LostOpportunityViewModel ...", category: Category.Info, priority: Priority.Low);

                LostOpportunityViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                SelectedReasonIndexChangedCommand = new Prism.Commands.DelegateCommand<object>(SelectedReasonIndex);

                LostOpportunityViewAcceptButtonCommand = new RelayCommand(new Action<object>(AcceptReason));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                GeosApplication.Instance.Logger.Log("Constructor LostOpportunityViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LostOpportunityViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion // Constructor

        #region validation

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
                    me[BindableBase.GetPropertyName(() => SelectedItems)] +
                    me[BindableBase.GetPropertyName(() => OfferLostDate)];

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
                string selectedItemsProp = BindableBase.GetPropertyName(() => SelectedItems);
                string LeadsLostDateProp = BindableBase.GetPropertyName(() => OfferLostDate);


                if (columnName == selectedItemsProp)
                    return RequiredValidationRule.GetErrorMessage(selectedItemsProp, SelectedItems);

                else if(columnName == LeadsLostDateProp)
                {
                    return RequiredValidationRule.GetErrorMessage(LeadsLostDateProp, OfferLostDate);
                }
                return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method for Initialize window for Lost Reason.
        /// [001][skale][30-09-2019][GEOS2-1756] Add the possibility to Edit the Offer LOST Date
        /// </summary>
        public void InIt()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InIt ...", category: Category.Info, priority: Priority.Low);

                OfferLostReasons = CrmStartUp.GetOfferLostReason();
                Competitors = CrmStartUp.GetCompetitors();


                MaxLostDate = GeosApplication.Instance.ServerDateTime.Date;


                if (offer[0].LostReasonsByOffer != null)
                {
                    if (!string.IsNullOrEmpty(offer[0].LostReasonsByOffer.Comments))
                    {
                        LostReasonDescription = offer[0].LostReasonsByOffer.Comments;
                    }

                    if (offer[0].LostReasonsByOffer.IdLostReasonList != null)
                    {
                        List<string> selectedReason = new List<string>(offer[0].LostReasonsByOffer.IdLostReasonList.Split(';'));
                        SelectedItems = new List<object>();
                        for (int i = 0; i < selectedReason.Count; i++)
                        {
                            SelectedItems.Add(OfferLostReasons.Where(n => n.IdLostReason == int.Parse(selectedReason[i].ToString())).SingleOrDefault());
                        }
                    }

                    SelectedIndexCompetitors = Competitors.FindIndex(i => i.IdCompetitor == offer[0].LostReasonsByOffer.IdCompetitor);

                    //[001] added
                    if (offer[0].LostReasonsByOffer.IdOffer > 0)
                        OfferLostDate = OfferCloseDate;
                    else
                        OfferLostDate = GeosApplication.Instance.ServerDateTime.Date;

                }

                GeosApplication.Instance.Logger.Log("Method InIt() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in InIt() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in InIt() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in InIt() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill selected reason on selected list.
        /// </summary>
        /// <param name="e"></param>
        public void SelectedReasonIndex(object e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedReasonIndex ...", category: Category.Info, priority: Priority.Low);

                var obj = (EditValueChangingEventArgs)e;
                List<object> selectedlist = (List<object>)obj.NewValue;
                List<object> oldselectedlist = (List<object>)obj.OldValue;
                SelectedItems = selectedlist;

                if (SelectedItems == null)
                {
                    return;
                }

                if (oldselectedlist == null || (selectedlist.Count > oldselectedlist.Count))
                {
                    SelectedItems = selectedlist;
                }
                else if (selectedlist.Count < oldselectedlist.Count)
                {
                    SelectedItems = selectedlist;
                }

                GeosApplication.Instance.Logger.Log("Method SelectedReasonIndex() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SelectedReasonIndex() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// After aacept fill reason and description on offer list.
        /// </summary>
        /// <param name="obj"></param>
        /// 
       //[lsharma][CRM-M040-04][(#49889) Lost offer information in grid columns]
       //Filling LostReasonsByOffer.Competitor and LostReasonsByOffer.OfferLostReason with stringLostReason
       //[CRM-M040-04]
        private void AcceptReason(object obj)
        {
            try
            {
                IsBusy = true;
                GeosApplication.Instance.Logger.Log("Method AcceptReason ...", category: Category.Info, priority: Priority.Low);

                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedItems"));
                PropertyChanged(this, new PropertyChangedEventArgs("OfferLostDate"));

                if (error != null)
                {
                    IsBusy = false;
                    return;
                }

                string idLostReasonStr = "";
                string stringLostReason = "";
                List<OfferLostReason> SelectedOfferLostReason = new List<OfferLostReason>();

                if (SelectedItems != null)
                {
                    foreach (OfferLostReason item in SelectedItems)
                    {
                        if (!string.IsNullOrEmpty(idLostReasonStr))
                        {
                            idLostReasonStr = idLostReasonStr + ";" + item.IdLostReason.ToString();
                            stringLostReason = stringLostReason + "\n" + item.Name.ToString();
                        }
                        else
                        {
                            idLostReasonStr = item.IdLostReason.ToString();
                            stringLostReason = item.Name.ToString();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(idLostReasonStr))
                {
                    LostReasonsByOffer = new LostReasonsByOffer();
                    LostReasonsByOffer.Comments = LostReasonDescription;
                    //LostReasonsByOffer.IdCompetitor = SelectedIndexCompetitors;
                    if (SelectedIndexCompetitors != -1)
                    {
                        LostReasonsByOffer.IdCompetitor = Competitors[SelectedIndexCompetitors].IdCompetitor;
                        LostReasonsByOffer.Competitor= new Competitor { Name= Competitors[SelectedIndexCompetitors].Name };
                    }
                    LostReasonsByOffer.IdLostReasonList = idLostReasonStr;
                    LostReasonsByOffer.OfferLostReason= new OfferLostReason {Name=stringLostReason };
                    LostReasonsByOffer.IdOffer = Offer[0].IdOffer;
                    //offer.LostReasonsByOffer = lostReasonsByOffer;
                    Offer[0].LostReasonsByOffer = LostReasonsByOffer;

                    RequestClose(null, null);
                }
              
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method AcceptReason() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in AcceptReason() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for close Window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            IsCancel = true;
            RequestClose(null, null);
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (GeosApplication.Instance.IsPermissionReadOnly)
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


