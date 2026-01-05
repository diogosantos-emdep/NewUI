using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class EditTargetAndForecastViewModel : NavigationViewModelBase, IDisposable, INotifyPropertyChanged
    {
        #region Services

        ICrmService crmControl = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declartion

        IList<Currency> currencies;
        private int selectedIndexCurrency = 0;
        DevExpress.Xpf.Grid.TableView tableViewObject;
        ObservableCollection<SalesTargetBySite> targetAndForecast = new ObservableCollection<SalesTargetBySite>();
        private DateTime? currencyRateDate;
        private DateTime? currencyRateMinDate;
        private DateTime? currencyRateMaxDate;
        private double currencyConversionRate;
        private string toCurrency;
        private int tabIndex;
        private double salesQuotaAmount;
        private decimal targetAmount;
        private decimal targetAmountWithExchangeRate;
        private ObservableCollection<SalesTargetAndForcastBySite> salesTargetAndForcastBySiteList;
        private byte IdCurrencyFrom;
        private Currency selectedCurrency;
        private int idSite;
        bool isBusy;
        ObservableCollection<SalesTargetBySite> tempTargetAndForecast = new ObservableCollection<SalesTargetBySite>();
        #endregion

        #region Properties
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public int IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdSite"));
            }
        }
        public ObservableCollection<SalesTargetAndForcastBySite> SalesTargetAndForcastBySiteList
        {
            get
            {
                return salesTargetAndForcastBySiteList;
            }

            set
            {
                salesTargetAndForcastBySiteList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesTargetAndForcastBySiteList"));
            }
        }

        public decimal TargetAmount
        {
            get
            {
                return targetAmount;
            }

            set
            {
                targetAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TargetAmount"));
            }
        }
        public decimal TargetAmountWithExchangeRate
        {
            get
            {
                return targetAmountWithExchangeRate;
            }

            set
            {
                targetAmountWithExchangeRate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TargetAmountWithExchangeRate"));
            }
        }
        public double SalesQuotaAmount
        {
            get
            {
                return salesQuotaAmount;
            }

            set
            {
                salesQuotaAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesQuotaAmount"));
            }
        }
        public string ToCurrency
        {
            get
            {
                return toCurrency;
            }

            set
            {
                toCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToCurrency"));
            }
        }
        public DateTime? CurrencyRateMinDate
        {
            get
            {
                return currencyRateMinDate;
            }
            set
            {
                currencyRateMinDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyRateMinDate"));
            }
        }
        public DateTime? CurrencyRateMaxDate
        {
            get
            {
                return currencyRateMaxDate;
            }
            set
            {
                currencyRateMaxDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyRateMaxDate"));
            }
        }
        public Currency SelectedCurrency
        {
            get
            {
                return selectedCurrency;
            }

            set
            {
                selectedCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrency"));
            }
        }
        public int TabIndex
        {
            get
            {
                return tabIndex;
            }

            set
            {
                tabIndex = value;
                int Year = SalesTargetAndForcastBySiteList[TabIndex].Year;
                SelectedCurrency = Currencies.FirstOrDefault(x => x.IdCurrency == SalesTargetAndForcastBySiteList[TabIndex].IdCurrency);
                if (DateTime.Now.Year == Year)
                {
                    CurrencyRateMinDate = new DateTime(Year, 1, 1);
                    CurrencyRateMaxDate = DateTime.Now;
                }
                else
                {
                    CurrencyRateMinDate = new DateTime(Year, 1, 1);
                    CurrencyRateMaxDate = new DateTime(Year, 12, 31);
                }
                OnPropertyChanged(new PropertyChangedEventArgs("TabIndex"));
            }
        }

        public IList<Offer> OfferTargetForecast { get; set; }
        public bool IsUpdated { get; set; }

        public IList<Currency> Currencies
        {
            get { return currencies; }
            set { currencies = value; }
        }

        public DevExpress.Xpf.Grid.TableView TableViewObject
        {
            get { return tableViewObject; }
            set { tableViewObject = value; }
        }

        public ObservableCollection<SalesTargetBySite> TargetAndForecast
        {
            get { return targetAndForecast; }
            set
            {
                //targetAndForecast = value; 
                SetProperty(ref targetAndForecast, value, () => TargetAndForecast);
            }
        }
        
        public ObservableCollection<SalesTargetBySite> TempTargetAndForecast
        {
            get { return tempTargetAndForecast; }
            set
            {
                //targetAndForecast = value; 
                SetProperty(ref tempTargetAndForecast, value, () => TempTargetAndForecast);
            }
        }

        #endregion

        #region ICommands

        public ICommand EditTargetAndForecastViewAcceptButtonCommand { get; set; }
        public ICommand EditTargetAndForecastViewCancelButtonCommand { get; set; }
        public ICommand CurrencyRateDateEditValueChangingCommand { get; set; }
        public ICommand TargetAmountChangedCommand { get; set; }
        public ICommand TargetAmountLostFocusCommand { get; set; }
        public ICommand TargetCurrencyChangedCommand { get; set; }

        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Constructor

        public EditTargetAndForecastViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditTargetAndForecastViewModel ...", category: Category.Info, priority: Priority.Low);

                EditTargetAndForecastViewAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(EditTargetForecast);
                EditTargetAndForecastViewCancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);

                TargetAmountChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(TargetAmountChangeAction);
                TargetAmountLostFocusCommand = new DelegateCommand<object>(TargetAmountLostFocusAction);
                CurrencyRateDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(CurrencyRateDateEditValueChanging);
                TargetCurrencyChangedCommand = new DelegateCommand<EditValueChangingEventArgs>(TargetCurrencyChangeAction);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                GeosApplication.Instance.IdCurrencyByRegion = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.IdCurrency).SingleOrDefault();
                GeosApplication.Instance.CurrentCurrencySymbol = GeosApplication.Instance.Currencies.Where(i => i.Name == GeosApplication.Instance.UserSettings["SelectedCurrency"].ToString()).Select(cur => cur.Symbol).SingleOrDefault();
                CurrencyRateMinDate = GeosApplication.Instance.SelectedyearStarDate;
                CurrencyRateMaxDate = GeosApplication.Instance.SelectedyearEndDate;
                FillCurrencyDetails();
                Currency curr = new Currency();
                curr = Currencies.FirstOrDefault(x => x.IdCurrency == GeosApplication.Instance.IdCurrencyByRegion);
                ToCurrency = curr.Name;
                GeosApplication.Instance.Logger.Log("Constructor EditTargetAndForecastViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditTargetAndForecastViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        /// Method for edit target forcast.
        /// </summary>
        /// <param name="obj"></param>
        public void EditTargetForecast(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method EditTargetForecast ...", category: Category.Info, priority: Priority.Low);

            //foreach (var item in SalesTargetAndForcastBySiteList)
            //{
            IsBusy = true;


            string error1 = SalesTargetAndForcastBySiteList[0].CheckValidation();
            if (error1 != null)
            {
                IsBusy = false;
                return;
            }
            //}

            TargetAndForecast = new ObservableCollection<SalesTargetBySite>();

            for (int i = 0; i < SalesTargetAndForcastBySiteList.Count; i++)
            {
                SalesTargetBySite objTargetAndForcast = new SalesTargetBySite();
                objTargetAndForcast.IdSite = IdSite;
                objTargetAndForcast.IdCurrency = SalesTargetAndForcastBySiteList[i].IdCurrency;
                objTargetAndForcast.ExchangeRateDate = SalesTargetAndForcastBySiteList[i].ExchangeRateDate;
                objTargetAndForcast.TargetAmount = SalesTargetAndForcastBySiteList[i].TargetAmount;
                objTargetAndForcast.TargetAmountWithExchangeRate = SalesTargetAndForcastBySiteList[i].TargetAmountWithExchangeRate;
                objTargetAndForcast.Year = SalesTargetAndForcastBySiteList[i].Year;
                TargetAndForecast.Add(objTargetAndForcast);
            }

            if (TargetAndForecast != null)
            {
                foreach (SalesTargetBySite item in TargetAndForecast)
                {
                    if (item.IdSite != 0 || item.IdCurrency != 0)
                    {
                        IsUpdated = crmControl.UpdateSalesTargetBySiteDate(item);
                    }
                }

                if (IsUpdated)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditTargetAndForecastViewSuccessMessage").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditTargetAndForecastViewFailMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
            }

            GeosApplication.Instance.Logger.Log("Method EditTargetForecast() executed successfully", category: Category.Info, priority: Priority.Low);

            RequestClose(null, null);
        }

        /// <summary>
        /// Methdo for fill currency details
        /// Task  [CRM-M039-02] Specify the date of the exchange rates in targets
        /// Amit
        /// </summary>
        public void FillCurrencyDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails ...", category: Category.Info, priority: Priority.Low);

                Currencies = GeosApplication.Instance.Currencies;

                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method do for Calculate Currency Exchange Rate
        /// </summary>
        public void CalculateCurrencyExchangeRate(decimal amount)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CalculateCurrencyExchangeRate ...", category: Category.Info, priority: Priority.Low);

                DailyCurrencyConversion dailyCurrencyConversion = new DailyCurrencyConversion();
                dailyCurrencyConversion.CurrencyConversionDate = (DateTime)SalesTargetAndForcastBySiteList[TabIndex].ExchangeRateDate;
                dailyCurrencyConversion.IdCurrencyConversionFrom = SalesTargetAndForcastBySiteList[TabIndex].IdCurrency;
                dailyCurrencyConversion.IdCurrencyConversionTo = GeosApplication.Instance.IdCurrencyByRegion;
                dailyCurrencyConversion = crmControl.GetCurrencyRateByDateAndId(dailyCurrencyConversion);
                TargetAmountWithExchangeRate = Convert.ToDecimal(dailyCurrencyConversion.CurrencyConversationRate);
                TargetAmountWithExchangeRate = Math.Round(TargetAmountWithExchangeRate, 4) * Math.Round(amount, 4);
                TargetAmountWithExchangeRate = Math.Round(TargetAmountWithExchangeRate, 4);

                SalesTargetAndForcastBySiteList[tabIndex].TargetAmountWithExchangeRate = TargetAmountWithExchangeRate;

                GeosApplication.Instance.Logger.Log("Method CalculateCurrencyExchangeRate() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CalculateCurrencyExchangeRate() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///Get Currency Value based on Currency Rate Date
        ///Task  [CRM-M039-02] Specify the date of the exchange rates in targets
        ///Amit

        public void CurrencyRateDateEditValueChanging(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CurrencyRateDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                SalesTargetAndForcastBySiteList[TabIndex].ExchangeRateDate = Convert.ToDateTime(e.NewValue);
                CalculateCurrencyExchangeRate(SalesTargetAndForcastBySiteList[TabIndex].TargetAmount);

                GeosApplication.Instance.Logger.Log("Method CurrencyRateDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CurrencyRateDateEditValueChanging() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method do for Change Target Currency Change Action
        /// </summary>
        public void TargetCurrencyChangeAction(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TargetCurrencyChangeAction ...", category: Category.Info, priority: Priority.Low);

                Currency curr = (Currency)e.NewValue;
                SalesTargetAndForcastBySiteList[TabIndex].IdCurrency = curr.IdCurrency;
                CalculateCurrencyExchangeRate(SalesTargetAndForcastBySiteList[TabIndex].TargetAmount);

                GeosApplication.Instance.Logger.Log("Method TargetCurrencyChangeAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TargetCurrencyChangeAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method do for Change Sales Quota Amount Changed
        /// </summary>
        public void TargetAmountChangeAction(EditValueChangedEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TargetAmountChangeAction ...", category: Category.Info, priority: Priority.Low);

                SalesTargetAndForcastBySiteList[TabIndex].TargetAmount = Convert.ToDecimal(e.NewValue);

                GeosApplication.Instance.Logger.Log("Method TargetAmountChangeAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TargetAmountChangeAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method do for Change Target Amount Lost Focus
        /// </summary>
        public void TargetAmountLostFocusAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SalesQuotaAmountLostFocusAction ...", category: Category.Info, priority: Priority.Low);

                CalculateCurrencyExchangeRate(SalesTargetAndForcastBySiteList[TabIndex].TargetAmount);

                GeosApplication.Instance.Logger.Log("Method SalesQuotaAmountLostFocusAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SalesQuotaAmountLostFocusAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        protected override void OnParameterChanged(object parameter)
        {
            GeosApplication.Instance.Logger.Log("Method OnParameterChanged ...", category: Category.Info, priority: Priority.Low);

            int previousYear = 0;
            int PreCounter = 0;
            int NextCounter =0;
            TableViewObject = (DevExpress.Xpf.Grid.TableView)parameter;
            GridControl grid = ((DevExpress.Xpf.Grid.TableView)(TableViewObject)).Grid;
  
            object rowr = grid.GetRow(grid.GetRowVisibleIndexByHandle(TableViewObject.FocusedRowData.RowHandle.Value));
            DataRowView selectedRow = (DataRowView)rowr;

            int SiteId = Convert.ToInt32(selectedRow.Row.ItemArray[4]);
            ObservableCollection<SalesTargetBySite> tempTargetAndForecast = new ObservableCollection<SalesTargetBySite>();

            tempTargetAndForecast = new ObservableCollection<SalesTargetBySite>(crmControl.GetSalesTargetBySiteDetailByIdSite(SiteId));
            foreach (var item in grid.ColumnsSource)
            {
                NextCounter++;
                PreCounter = NextCounter - 1;
                Emdep.Geos.UI.Helper.Column colItem = new Emdep.Geos.UI.Helper.Column();
                colItem = ((Emdep.Geos.UI.Helper.Column)(item));
                if (colItem.HeaderText.StartsWith("Target"))
                {
                    string str = colItem.FieldName.ToString();
                    int year = Convert.ToInt32(str);
                    SalesTargetBySite selectTarget = new SalesTargetBySite();
                    selectTarget.Year = year;
                    selectTarget.Currencies = new List<Currency>(Currencies);
                    selectTarget.Currency = new Currency();
                    if (year != null)
                    {
                        for (int i = 0; i < (TableViewObject).Grid.SelectedItems.Count; i++)
                        {

                            foreach (DataRowView drv in grid.SelectedItems)
                            {
                                SalesTargetBySite tempselectTarget = new SalesTargetBySite();
                                tempselectTarget = tempTargetAndForecast.FirstOrDefault(x => x.Year == year);
                                DataRow row = drv.Row;
                                if (row.ItemArray[4] != DBNull.Value)
                                {
                                    selectTarget.IdSite = Convert.ToInt32(row.ItemArray[4]);
                                }
                                else
                                {
                                    selectTarget.IdSite = Convert.ToInt32(row.ItemArray[4]);
                                }
                                if (tempselectTarget != null)
                                {
                                    if (tempselectTarget.IdCurrency != null)
                                    {
                                        selectTarget.IdCurrency = tempselectTarget.IdCurrency;
                                    }
                                    else
                                    {
                                        selectTarget.IdCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                                    }

                                    if (previousYear == 0)
                                    {
                                        if (row.ItemArray[PreCounter] != DBNull.Value)
                                        {
                                            selectTarget.TargetAmount = Convert.ToDecimal(row.ItemArray[NextCounter]);
                                        }

                                    }
                                    else
                                    {
                                        if (previousYear == year)
                                        {
                                            int flag = TargetAndForecast.Count;

                                            (TargetAndForecast[flag - 1]).TargetAmountWithExchangeRate = Convert.ToDecimal(row.ItemArray[PreCounter - 1]);
                                            TempTargetAndForecast.Add(TargetAndForecast[flag - 1]);
                                        }
                                        else
                                        {
                                            if (row.ItemArray[NextCounter] != DBNull.Value)
                                                selectTarget.TargetAmount = Convert.ToDecimal(row.ItemArray[NextCounter]);
                                        }

                                    }

                                    if (tempselectTarget.ExchangeRateDate == null)
                                    {
                                        selectTarget.ExchangeRateDate = new DateTime(year, 1, 1);
                                    }
                                    else
                                    {
                                        selectTarget.ExchangeRateDate = tempselectTarget.ExchangeRateDate;
                                    }
                                    previousYear = selectTarget.Year;
                                }
                                else
                                {
                                    if (previousYear == year)
                                    {
                                        int flag = TargetAndForecast.Count;

                                        (TargetAndForecast[flag - 1]).TargetAmountWithExchangeRate = 0;
                                        TempTargetAndForecast.Add(TargetAndForecast[flag - 1]);
                                    }
                                    else
                                    {
                                        selectTarget.IdCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                                        selectTarget.TargetAmount = 0;
                                        selectTarget.TargetAmountWithExchangeRate = 0;
                                        selectTarget.ExchangeRateDate = new DateTime(year, 1, 1);

                                    }
                                    previousYear = selectTarget.Year;
                                }
                            }
                        }
                    }
                    TargetAndForecast.Add(selectTarget);
                }
            }
            TargetAndForecast = TempTargetAndForecast;
            SalesTargetAndForcastBySiteList = new ObservableCollection<SalesTargetAndForcastBySite>();

            for (int i = 0; i < TargetAndForecast.Count; i++)
            {
                SalesTargetAndForcastBySite objTargetAndForcast = new SalesTargetAndForcastBySite();
                objTargetAndForcast.IdCurrency = TargetAndForecast[i].IdCurrency;
                objTargetAndForcast.ExchangeRateDate = TargetAndForecast[i].ExchangeRateDate;
                objTargetAndForcast.TargetAmount = TargetAndForecast[i].TargetAmount;
                objTargetAndForcast.TargetAmountWithExchangeRate = TargetAndForecast[i].TargetAmountWithExchangeRate;
                objTargetAndForcast.Year = TargetAndForecast[i].Year;
                SalesTargetAndForcastBySiteList.Add(objTargetAndForcast);
            }
            IdSite = TargetAndForecast[0].IdSite;
            base.OnParameterChanged(parameter);
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
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


        public class SalesTargetAndForcastBySite : INotifyPropertyChanged, IDataErrorInfo
        {

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, e);
                }
            }

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
            string IDataErrorInfo.this[string columnName]
            {
                get
                {
                    if (!allowValidation) return null;


                    string targetAmountWithExchangeRateProp = BindableBase.GetPropertyName(() => TargetAmountWithExchangeRate);

                    if (columnName == targetAmountWithExchangeRateProp)
                        return UserValidation.GetErrorMessage(targetAmountWithExchangeRateProp, TargetAmountWithExchangeRate);

                    return null;
                }
            }

            string IDataErrorInfo.Error
            {
                get
                {
                    //if (!allowValidation) return null;
                    IDataErrorInfo me = (IDataErrorInfo)this;
                    string error =

                    me[BindableBase.GetPropertyName(() => TargetAmountWithExchangeRate)];

                    if (!string.IsNullOrEmpty(error))
                        return "Please check inputted data.";

                    return null;
                }
            }


            #endregion

            #region Fields

            Int32 year;
            decimal targetAmount;
            byte idCurrency;
            DateTime? exchangeRateDate;
            decimal targetAmountWithExchangeRate;

            #endregion

            #region Constructor
            public SalesTargetAndForcastBySite()
            {

            }
            #endregion

            #region Properties
            public byte IdCurrency
            {
                get
                {
                    return idCurrency;
                }

                set
                {
                    idCurrency = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IdCurrency"));
                }
            }
            public Int32 Year
            {
                get
                {
                    return year;
                }

                set
                {
                    year = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Year"));

                }
            }


            public DateTime? ExchangeRateDate
            {
                get
                {
                    return exchangeRateDate;
                }

                set
                {
                    exchangeRateDate = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ExchangeRateDate"));

                }
            }


            public decimal TargetAmount
            {
                get
                {
                    return targetAmount;
                }

                set
                {
                    targetAmount = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("TargetAmount"));

                }
            }


            public decimal TargetAmountWithExchangeRate
            {
                get
                {
                    return targetAmountWithExchangeRate;
                }

                set
                {
                    targetAmountWithExchangeRate = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("TargetAmountWithExchangeRate"));

                }
            }

            #endregion

            #region Methods
            public string CheckValidation()
            {
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("TargetAmountWithExchangeRate"));

                return error;
            }
            #endregion
        }
    }
}
