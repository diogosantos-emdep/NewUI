using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class EditPlantQuotaViewModel : NavigationViewModelBase, IDisposable
    {
        #region Services
        ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Declartion 

        #region Old Code
        //  private DataTable dtPlant;
        //   private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        //  DateTime? exchangeRateDate;
        // IList<Currency> currencies;
        //   private DataRowView selectedObject; 
        #endregion

        private int selectedCurrency;
        private string windowHeader;
        private double currencyConversionRate;
        private PlantBusinessUnitSalesQuota selectedPlantBusinessUnitSalesQuota;
        private ObservableCollection<PlantBusinessUnitSalesQuota> plantBusinessUnitSalesQuota;

        #endregion

        #region Public Properties


        public int PlantId { get; set; }


        public PlantBusinessUnitSalesQuota SelectedPlantBusinessUnitSalesQuota
        {
            get { return selectedPlantBusinessUnitSalesQuota; }
            set { selectedPlantBusinessUnitSalesQuota = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantBusinessUnitSalesQuota")); }
        }

        public ObservableCollection<PlantBusinessUnitSalesQuota> PlantBusinessUnitSalesQuota
        {
            get { return plantBusinessUnitSalesQuota; }
            set
            {
                plantBusinessUnitSalesQuota = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantBusinessUnitSalesQuota"));
            }
        }

        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public double CurrencyConversionRate
        {
            get
            {
                return currencyConversionRate;
            }

            set
            {
                currencyConversionRate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyConversionRate"));
            }
        }



        #region OldCode

        //public DateTime? ExchangeRateDate
        //{
        //    get
        //    {
        //        return exchangeRateDate;
        //    }

        //    set
        //    {
        //        exchangeRateDate = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ExchangeRateDate"));

        //    }
        //}

        //public DataRowView SelectedObject
        //{
        //    get { return selectedObject; }
        //    set
        //    {
        //        selectedObject = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedObject"));
        //    }
        //}

        //public DataTable DtPlant
        //{
        //    get { return dtPlant; }
        //    set
        //    {
        //        dtPlant = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("DtPlant"));
        //    }
        //}

        //public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        //{
        //    get { return columns; }
        //    set
        //    {
        //        columns = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
        //    }
        //}

        //public IList<Currency> Currencies
        //{
        //    get
        //    {
        //        return currencies;
        //    }

        //    set
        //    {
        //        currencies = value;
        //    }
        //}


        //public IList<LookupValue> BusinessUnitList { get; set; }
        //public IList<Offer> OfferTargetForecast { get; set; } 
        #endregion
        public bool IsUpdated { get; set; }

        public int SelectedCurrency
        {
            get
            {
                return selectedCurrency;
            }

            set
            {
                selectedCurrency = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrency"));

            }
        }
        #endregion

        #region ICommands
        public ICommand EditPlantQuotaViewAcceptButtonCommand { get; set; }
        public ICommand EditPlantQuotaViewCancelButtonCommand { get; set; }

        #region OldCode
        //public ICommand ExchangeRateMouseDownCommand { get; set; }
        //public ICommand CommandGridDoubleClick { get; set; }
        // public ICommand ExchangeRateEditCommand { get; set; } 
        #endregion
        public ICommand CurrencyTypeSelectedIndexChangedCommand { get; set; }
        public ICommand ExchangeRateLostFocusCommand { get; set; }
        public ICommand EditPlantQuotaCancelCommand { get; set; }

        public ICommand SalesQuotaAmountChangedCommand { get; set; }
        public ICommand SalesQuotaAmountLostFocusCommand { get; set; }
        //   public ICommand PlantBusinessUnitSalesQuotaChangedCommand { get; set; }
        public ICommand CurrencyRateDateEditValueChangingCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Constructor
        public EditPlantQuotaViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditPlantQuotaViewModel ...", category: Category.Info, priority: Priority.Low);

                EditPlantQuotaViewCancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
                EditPlantQuotaViewAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(EditPlantQuotaInformation);
                CurrencyTypeSelectedIndexChangedCommand = new Prism.Commands.DelegateCommand<object>(CurrencyTypeChanged);
                EditPlantQuotaCancelCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
                SalesQuotaAmountLostFocusCommand = new DelegateCommand<object>(SalesQuotaAmountLostFocusAction);
                SalesQuotaAmountChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(SalesQuotaAmountChangedAction);
                // PlantBusinessUnitSalesQuotaChangedCommand = new Prism.Commands.DelegateCommand<object>(TabChanged);
                CurrencyRateDateEditValueChangingCommand = new Prism.Commands.DelegateCommand<object>(CurrencyRateDateChanged);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                GeosApplication.Instance.Logger.Log("Constructor EditPlantQuotaViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditPlantQuotaViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        #endregion // Events

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public void InIt()
        {
            GeosApplication.Instance.Logger.Log("EditPlantQuotaViewModel Method InIt ...", category: Category.Info, priority: Priority.Low);

            foreach (var Quota in PlantBusinessUnitSalesQuota)
            {
                int CurrencyID;


                CurrencyID= Quota.LookupValue.Any(X => X.IdSalesQuotaCurrency > 0 )
                    ?Quota.LookupValue.FirstOrDefault(x => x.IdSalesQuotaCurrency > 0).IdSalesQuotaCurrency 
                    : GeosApplication.Instance.IdCurrencyByRegion;
                Quota.ExchangeRateDate = Quota.LookupValue.Any(X => X.ExchangeRateDate != null)
                    ? Quota.LookupValue.FirstOrDefault(x => x.ExchangeRateDate != null).ExchangeRateDate
                    :DateTime.Today;

                Quota.IdPlant = PlantId;
                Quota.LookupValue.ForEach(x => x.IdSalesQuotaCurrency = (byte)CurrencyID);
                Quota.AmountHeader = "Amount (" + GeosApplication.Instance.Currencies.FirstOrDefault(i => i.IdCurrency == CurrencyID).Name + ")";
                Quota.AmountHeaderApplicationPrefer = "Amount (" + GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString() + ")";
                Quota.IdSalesQuotaCurrency = Convert.ToByte(CurrencyID - 1);

                //Quota.SelectedLookupValue = Quota.LookupValue[0];
                //Quota.SelectedLookupValue.ExchangeRateDate = Quota.LookupValue[0].ExchangeRateDate;
                GeosApplication.Instance.Logger.Log(" EditPlantQuotaViewModel() Method InIt executed successfully", category: Category.Info, priority: Priority.Low);
            }
            OnPropertyChanged(new PropertyChangedEventArgs("PlantBusinessUnitSalesQuota"));
        }



        #region old Code

        //public void FillCurrencyDetails()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails ...", category: Category.Info, priority: Priority.Low);

        //        Currencies = GeosApplication.Instance.Currencies;

        //        //  SelectedCurrency = Currencies.FirstOrDefault(i => i.IdCurrency == GeosApplication.Instance.IdCurrencyByRegion);

        //        GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }

        //}
        //public void EditPlantQuota(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method EditPlantQuota ...", category: Category.Info, priority: Priority.Low);

        //    if (DtPlant != null)
        //    {
        //        string plant = DtPlant.Rows[0].ItemArray[0].ToString();
        //        int plantId = Convert.ToInt32((DtPlant.Rows[0].ItemArray[1].ToString()));

        //        List<PlantBusinessUnitSalesQuota> TempPlantList = new List<PlantBusinessUnitSalesQuota>();

        //        List<LookupValue> TemplookupList = crmControl.GetLookupvaluesWithoutRestrictedBU(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission).ToList();

        //        List<LookupValue> lookupList = new List<LookupValue>();
        //        foreach (DataColumn dataColumn in DtPlant.Columns)
        //        {
        //            LookupValue businessUnit = TemplookupList.FirstOrDefault(x => x.Value == Convert.ToString(dataColumn.ColumnName));

        //            if (businessUnit != null && !DBNull.Value.Equals(DtPlant.Rows[0][Convert.ToString(dataColumn.ColumnName)]))
        //            {
        //                businessUnit.SalesQuotaAmount = Convert.ToDouble(DtPlant.Rows[0][Convert.ToString(dataColumn.ColumnName)]);
        //                businessUnit.IdSalesQuotaCurrency = Convert.ToByte(GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.IdCurrency).SingleOrDefault());
        //                lookupList.Add(businessUnit);
        //            }
        //        }

        //        TempPlantList.Add(new PlantBusinessUnitSalesQuota { ShortName = plant, IdPlant = plantId, LookupValue = lookupList, Year = Convert.ToInt32(GeosApplication.Instance.CrmOfferYear) });

        //        foreach (PlantBusinessUnitSalesQuota item in TempPlantList)
        //        {
        //            if (item.IdPlant != 0 || item.ShortName != null)
        //                isUpdated = crmControl.UpdatePlantBusinessUnitSalesQuotaWithYear(item);
        //        }

        //        if (isUpdated == true)
        //            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditPlantQuotaViewSuccessMessage").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
        //        else
        //            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditPlantQuotaViewFailMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

        //        GeosApplication.Instance.Logger.Log("Method EditPlantQuota() executed Successfully", category: Category.Info, priority: Priority.Low);
        //    }


        //    RequestClose(null, null);
        //}
        //private void AddColumnsToDataTable()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable ...", category: Category.Info, priority: Priority.Low);

        //        Columns = new ObservableCollection<Emdep.Geos.UI.Helper.Column>() {
        //                new Emdep.Geos.UI.Helper.Column() { FieldName="PlantName",HeaderText="Plant Name", Settings = SettingsType.Default, AllowCellMerge=false, Width=110,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=true  },
        //    };

        //        DtPlant = new DataTable();
        //        DtPlant.Columns.Add("PlantName", typeof(string));

        //        BusinessUnitList = crmControl.GetLookupvaluesWithoutRestrictedBU(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.IdUserPermission);

        //        List<LookupValue> TempBusinessUnitList = new List<LookupValue>(BusinessUnitList.ToList());

        //        List<int> TempLookupListId = new List<int>() { 5, 4, 3 };

        //        TempBusinessUnitList = TempBusinessUnitList.OrderBy(Or => TempLookupListId.IndexOf(Or.IdLookupValue)).ToList();

        //        for (int i = 0; i < TempBusinessUnitList.Count; i++)
        //        {
        //            if (!DtPlant.Columns.Contains(TempBusinessUnitList[i].Value))
        //            {
        //                Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = TempBusinessUnitList[i].Value.ToString(), HeaderText = TempBusinessUnitList[i].Value.ToString(), Settings = SettingsType.Amount, AllowCellMerge = false, Width = 110, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = true });
        //                DtPlant.Columns.Add(TempBusinessUnitList[i].Value.ToString(), typeof(double));
        //            }
        //        }

        //        GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTable() executed Successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in AddColumnsToDataTable() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in AddColumnsToDataTable() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in AddColumnsToDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //} 

        #endregion


        /// <summary>
        /// Method do for Change Sales Quota Amount Lost Focus
        /// </summary>
        public void SalesQuotaAmountLostFocusAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("EditPlantQuotaViewModel Method SalesQuotaAmountLostFocusAction ...", category: Category.Info, priority: Priority.Low);

                CalculateCurrencyExchangeRate(Convert.ToDouble(SelectedPlantBusinessUnitSalesQuota.SelectedLookupValue.SalesQuotaAmount), SelectedPlantBusinessUnitSalesQuota.SelectedLookupValue.IdLookupValue);

                GeosApplication.Instance.Logger.Log("EditPlantQuotaViewModel Method SalesQuotaAmountLostFocusAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditPlantQuotaViewModel in Method SalesQuotaAmountLostFocusAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method do for Change Sales Quota Amount Changed
        /// </summary>


        public void SalesQuotaAmountChangedAction(EditValueChangedEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("EditPlantQuotaViewModel Method SalesQuotaAmountChangeAction ...", category: Category.Info, priority: Priority.Low);
                double price;

                if (string.IsNullOrEmpty(e.NewValue.ToString()))
                {
                    price = 0;
                }
                if (e.OldValue.ToString() == e.NewValue.ToString())
                    return;
                price = Convert.ToDouble(e.NewValue.ToString());
                CalculateCurrencyExchangeRate(price, SelectedPlantBusinessUnitSalesQuota.SelectedLookupValue.IdLookupValue);
                GeosApplication.Instance.Logger.Log("EditPlantQuotaViewModel Method SalesQuotaAmountChangeAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditPlantQuotaViewModel Method SalesQuotaAmountChangeAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method do for Calculate Currency Exchange Rate
        /// </summary>
        public void CalculateCurrencyExchangeRate(double amount, int IdLookUpValue)
        {
            try
            {
                int IdSalesQuotaCurrency = SelectedPlantBusinessUnitSalesQuota.IdSalesQuotaCurrency;
                GeosApplication.Instance.Logger.Log("EditPlantQuotaViewModel Method CalculateCurrencyExchangeRate ...", category: Category.Info, priority: Priority.Low);

                DailyCurrencyConversion dailyCurrencyConversion = new DailyCurrencyConversion();
                dailyCurrencyConversion.CurrencyConversionDate = (DateTime)SelectedPlantBusinessUnitSalesQuota.ExchangeRateDate;
                dailyCurrencyConversion.IdCurrencyConversionFrom = Convert.ToByte(IdSalesQuotaCurrency + 1);
                dailyCurrencyConversion.IdCurrencyConversionTo = GeosApplication.Instance.IdCurrencyByRegion;
                dailyCurrencyConversion = crmControl.GetCurrencyRateByDateAndId(dailyCurrencyConversion);
                CurrencyConversionRate = dailyCurrencyConversion.CurrencyConversationRate;
                CurrencyConversionRate = Math.Round(CurrencyConversionRate, 4) * Math.Round(amount, 4);
                CurrencyConversionRate = Math.Round(CurrencyConversionRate, 4);
                SelectedPlantBusinessUnitSalesQuota.LookupValue.FirstOrDefault(X => X.IdLookupValue == IdLookUpValue).SalesQuotaAmountWithExchangeRate = CurrencyConversionRate;

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantBusinessUnitSalesQuota"));
                GeosApplication.Instance.Logger.Log("EditPlantQuotaViewModel Method CalculateCurrencyExchangeRate() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditPlantQuotaViewModel Method CalculateCurrencyExchangeRate() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void EditPlantQuotaInformation(object obj)
        {
            try
            {
                IsUpdated = crmControl.UpdatePlantQuota(new List<PlantBusinessUnitSalesQuota>(PlantBusinessUnitSalesQuota as ObservableCollection<PlantBusinessUnitSalesQuota>));
                if (IsUpdated)
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditPlantQuotaViewSuccessMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                else
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditPlantQuotaViewFailMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditPlantQuotaInformation() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditPlantQuotaInformation() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditPlantQuotaInformation() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            RequestClose(null, null);
        }



        public void TabChanged(object obj)
        {
            try
            {
                // SelectedPlantBusinessUnitSalesQuota.SelectedLookupValue = SelectedPlantBusinessUnitSalesQuota.LookupValue[0];
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TabChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        public void CurrencyRateDateChanged(object obj)
        {
            try
            {
                var Event = (EditValueChangedEventArgs)obj;
                SelectedPlantBusinessUnitSalesQuota.ExchangeRateDate = (DateTime)Event.NewValue;
                SelectedPlantBusinessUnitSalesQuota.LookupValue.ForEach(X => X.ExchangeRateDate = (DateTime)Event.NewValue);
                SelectedPlantBusinessUnitSalesQuota.LookupValue.ForEach(x =>
                {
                    CalculateCurrencyExchangeRate(Convert.ToDouble(x.SalesQuotaAmount), x.IdLookupValue);
                });

                //   OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantBusinessUnitSalesQuota"));
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantBusinessUnitSalesQuota"));
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CurrencyRateDateChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// To close window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        /// <summary>
        /// Updateing selected Currency in header
        /// </summary>
        /// <param name="obj"></param>
        private void CurrencyTypeChanged(object obj)
        {
            byte IdSalesQuotaCurrency = 1;
            IdSalesQuotaCurrency += SelectedPlantBusinessUnitSalesQuota.IdSalesQuotaCurrency;
            SelectedPlantBusinessUnitSalesQuota.AmountHeader = "Amount (" + GeosApplication.Instance.Currencies.FirstOrDefault(i => i.IdCurrency == SelectedPlantBusinessUnitSalesQuota.IdSalesQuotaCurrency + 1).Name + ")";
            SelectedPlantBusinessUnitSalesQuota.LookupValue.ForEach(X => X.IdSalesQuotaCurrency = IdSalesQuotaCurrency);
            SelectedPlantBusinessUnitSalesQuota.LookupValue.ForEach(x =>
            {
                CalculateCurrencyExchangeRate(Convert.ToDouble(x.SalesQuotaAmount), x.IdLookupValue);
            });
            OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantBusinessUnitSalesQuota"));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
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
