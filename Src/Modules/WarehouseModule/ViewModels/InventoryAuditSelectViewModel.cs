using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
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

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class InventoryAuditSelectViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services       
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Task log
        //[000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        #endregion

        #region Declaration
        private string error;
        private string windowHeader;
        private int selectedInventoryAuditIndex;
        private List<WarehouseInventoryAudit> inventoryAuditList;
        private WarehouseInventoryAudit selectedInventoryAudit;

        #endregion

        #region  Properties

        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public List<WarehouseInventoryAudit> InventoryAuditList
        {
            get
            {
                return inventoryAuditList;
            }
            set
            {
                inventoryAuditList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InventoryAuditList"));
            }
        }

        public WarehouseInventoryAudit SelectedInventoryAudit
        {
            get { return selectedInventoryAudit; }
            set { selectedInventoryAudit = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedInventoryAudit")); }
        }

        public int SelectedInventoryAuditIndex
        {
            get { return selectedInventoryAuditIndex; }
            set { selectedInventoryAuditIndex = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedInventoryAuditIndex")); }
        }
        #endregion

        #region Pubilc Event
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

        #region Public Icommands
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        #endregion

        #region Command Action
        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="obj"></param>
        private void WarehouseInventory(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WarehouseInventory...", category: Category.Info, priority: Priority.Low);

                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedInventoryAuditIndex"));

                if (error != null)
                {
                    return;
                }

                if (!DXSplashScreen.IsActive)
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

                InventoryView inventoryView = new InventoryView();
                InventoryViewModel inventoryViewModel = new InventoryViewModel();
                EventHandler Close = delegate { inventoryView.Close(); };
                inventoryViewModel.RequestClose += Close;
                inventoryViewModel.Init(SelectedInventoryAudit);
                inventoryView.DataContext = inventoryViewModel;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                var ownerInfo = (obj as FrameworkElement);
                inventoryView.Owner = Window.GetWindow(ownerInfo);
                inventoryView.ShowDialog();

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method WarehouseInventory()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
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
                    me[BindableBase.GetPropertyName(() => SelectedInventoryAuditIndex)];

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

                string selectedInventoryAuditIndex = BindableBase.GetPropertyName(() => SelectedInventoryAuditIndex);

                if (columnName == selectedInventoryAuditIndex)
                    return WarehouseInventoryAuditValidation.GetErrorMessage(selectedInventoryAuditIndex, SelectedInventoryAuditIndex);

                return null;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                WindowHeader = Application.Current.FindResource("InventoryViewHeader").ToString();

                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AcceptButtonCommand = new RelayCommand(new Action<object>(WarehouseInventory));

                FillInventoryAuditList();

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [000][skale][03-12-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        public void FillInventoryAuditList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillInventoryAuditList()...", category: Category.Info, priority: Priority.Low);

                InventoryAuditList = new List<WarehouseInventoryAudit>();
                InventoryAuditList = WarehouseService.GetOpenWarehouseInventoryAudits(WarehouseCommon.Instance.Selectedwarehouse);
                InventoryAuditList.Insert(0, new WarehouseInventoryAudit() { Name = "---" });

                if (InventoryAuditList.Count == 2)
                    SelectedInventoryAuditIndex = 1;
                else
                    SelectedInventoryAuditIndex = 0;
                GeosApplication.Instance.Logger.Log("Method FillInventoryAuditList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillInventoryAuditList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillInventoryAuditList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
