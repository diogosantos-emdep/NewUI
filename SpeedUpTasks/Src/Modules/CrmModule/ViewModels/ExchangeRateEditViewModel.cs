using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.PivotGrid;
using DevExpress.Xpf.PivotGrid.Internal;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
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
    public class ExchangeRateEditViewModel : NavigationViewModelBase
    {
        //public DataTable dt1 = new DataTable();

        #region  public ICommand

        public ICommand ExchangeRateEditCommand { get; set; }
        public ICommand ExchangeRateEditAcceptCommand { get; set; }
        public ICommand ExchangeRateEditCancelCommand { get; set; }
        public ICommand ExchangeRateKeyDownCommand { get; set; }
        public ICommand ExchangeRateLostFocusCommand { get; set; }
        public ICommand ExchangeRateMouseDownCommand { get; set; }
        public ICommand CommandGridDoubleClick { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Declaration

        private DataTable dataTableExchangeRateEdit;
        private IList<CurrencyConversion> exchangeRateList;
        private List<CurrencyConversion> currencyConversionList;

        #endregion

        #region Public Properties

        public CurrencyConversion currencyConversion { get; set; }

        public DataTable DataTableExchangeRateEdit
        {
            get { return dataTableExchangeRateEdit; }
            set
            {
                dataTableExchangeRateEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableExchangeRateEdit"));
            }
        }

        public IList<CurrencyConversion> ExchangeRateList
        {
            get { return exchangeRateList; }
            set { exchangeRateList = value; }
        }

        public List<CurrencyConversion> CurrencyConversionList
        {
            get { return currencyConversionList; }
            set { currencyConversionList = value; }
        }

        #endregion

        #region Constructor

        public ExchangeRateEditViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor ExchangeRateEditViewModel ...", category: Category.Info, priority: Priority.Low);

            CommandGridDoubleClick = new DelegateCommand<object>(ExchangeRateViewdata);
            ExchangeRateMouseDownCommand = new RelayCommand(new Action<object>(TextEdit_MouseDown));
            ExchangeRateLostFocusCommand = new RelayCommand(new Action<object>(TextEdit_LostFocus));
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
            GeosApplication.Instance.Logger.Log("Constructor ExchangeRateEditViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

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

        #region Methods

        private void ExchangeRateViewdata(object obj)
        {
        }

        public void InIt(DataTable dataTableExchangeRate)
        {
            GeosApplication.Instance.Logger.Log("Method InIt ...", category: Category.Info, priority: Priority.Low);

            CurrencyConversionList = new List<CurrencyConversion>();
            ExchangeRateEditCommand = new RelayCommand(new Action<object>(ExchangeRateEdit));
            ExchangeRateEditAcceptCommand = new DelegateCommand<object[]>(ExchangeRateEditAccept);
            ExchangeRateEditCancelCommand = new RelayCommand(new Action<object>(CloseWindow));
            DataTableExchangeRateEdit = dataTableExchangeRate;

            GeosApplication.Instance.Logger.Log("Method InIt() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for textedit lost focus .
        /// </summary>
        /// <param name="sender"></param>
        private void TextEdit_LostFocus(object sender)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TextEdit_LostFocus ...", category: Category.Info, priority: Priority.Low);

                EditValue(sender);
                TextEdit edit = sender as TextEdit;
                edit.EditMode = EditMode.InplaceInactive;
                CellsAreaItem item = edit.DataContext as CellsAreaItem;
                item.PivotGrid.RefreshData();

                if (CurrencyConversionList.Exists(x => x.CurrencyFrom.Name == item.RowValueDisplayText.ToString().Substring(3) && x.CurrencyTo.Name == item.ColumnValueDisplayText.ToString()))
                {
                    CurrencyConversion currencyConversionold = new CurrencyConversion();
                    currencyConversionold = CurrencyConversionList.Find(i => i.CurrencyFrom.Name == item.RowValueDisplayText.ToString().Substring(3) && i.CurrencyTo.Name == item.ColumnValueDisplayText.ToString());
                    currencyConversionold.ExchangeRate = float.Parse(item.RowTotalValue.ToString());
                }
                else
                {
                    CurrencyConversion currencyConversion = new CurrencyConversion();
                    currencyConversion.CurrencyFrom = new Currency { Name = item.RowValueDisplayText.ToString().Substring(3) };
                    currencyConversion.CurrencyTo = new Currency { Name = item.ColumnValueDisplayText.ToString() };
                    currencyConversion.ExchangeRate = float.Parse(item.RowTotalValue.ToString());
                    CurrencyConversionList.Add(currencyConversion);
                }

                GeosApplication.Instance.Logger.Log("Method TextEdit_LostFocus() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TextEdit_LostFocus() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for textedit mouse down.
        /// </summary>
        /// <param name="sender"></param>
        private void TextEdit_MouseDown(object sender)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TextEdit_MouseDown ...", category: Category.Info, priority: Priority.Low);

                TextEdit edit = sender as TextEdit;
                if (edit == null || edit.DataContext as CellsAreaItem == null)
                {
                    return;
                }

                CellsAreaItem item = edit.DataContext as CellsAreaItem;
                if (item.IsTotalAppearance)
                {
                    return;
                }

                edit.EditMode = EditMode.InplaceActive;

                GeosApplication.Instance.Logger.Log("Method TextEdit_MouseDown() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TextEdit_MouseDown() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExchangeRateEditAccept(object[] obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExchangeRateEditAccept ...", category: Category.Info, priority: Priority.Low);

                GridControl g = new GridControl();
                ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                bool isupdated = CrmStartUp.UpdateCurrencyConversion(CurrencyConversionList);
                if (isupdated)
                {
                    CustomMessageBox.Show("Exchange rate updated successfully.", "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }

                GeosApplication.Instance.Logger.Log("Method ExchangeRateEditAccept() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ExchangeRateEditAccept() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ExchangeRateEditAccept() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ExchangeRateEditAccept() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExchangeRateEdit(object obj)
        {
            // EditValue(obj);
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void EditValue(object sender)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditValue ...", category: Category.Info, priority: Priority.Low);

                TextEdit edit = sender as TextEdit;
                if (edit == null || edit.DataContext as CellsAreaItem == null)
                {
                    return;
                }

                CellsAreaItem item = edit.DataContext as CellsAreaItem;
                decimal newValue;
                decimal oldValue;
                PivotGridControl pivotGrid = FindParentPivotGrid((DependencyObject)sender);

                if (edit.EditValue != null && decimal.TryParse(edit.EditValue.ToString(), out newValue))
                {
                    if (item.Value == null || !decimal.TryParse(item.Value.ToString(), out oldValue))
                    {
                        return;
                    }

                    PivotGridField dataField = pivotGrid.Fields["ExchangeRate"];
                    PivotDrillDownDataSource ds = pivotGrid.CreateDrillDownDataSource(item.ColumnIndex, item.RowIndex);
                    ds.SetValue(0, dataField, newValue);
                }

                GeosApplication.Instance.Logger.Log("Method EditValue() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditValue() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private static PivotGridControl FindParentPivotGrid(DependencyObject item)
        {
            DependencyObject parent = System.Windows.Media.VisualTreeHelper.GetParent(item);
            if (parent == null)
            {
                return null;
            }

            PivotGridControl pivot = parent as PivotGridControl;
            if (pivot != null)
            {
                return pivot;
            }

            return FindParentPivotGrid(parent);
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