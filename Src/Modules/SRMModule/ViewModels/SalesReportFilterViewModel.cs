using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
   
    public class SalesReportFilterViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {

        #region TaskLog
        // [nsatpute][21-01-2025][GEOS2-5725]
        #endregion
        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SrmService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISRMService SrmService = new SRMServiceController("localhost:6699");
        #endregion // Services

        #region Declaration
        ModuleFamily selectedFamily;
        ModuleSubFamily selectedSubFamily;
        ObservableCollection<Customer> groupList;
        List<Company> plantList;


        Int16 selectedIndexGroup;
        Int16 selectedIndexPlant;

        ObservableCollection<ModuleFamily> familyList;
        ObservableCollection<ModuleSubFamily> subFamilyList;
        Int16 selectedIndexTemplate;
        Int16 selectedIndexCpType;
        Int16 selectedIndexOption;
        private DateTime fromDate;
        private DateTime toDate;
        private ObservableCollection<Company> entireCompanyPlantList;
        private ObservableCollection<Warehouses> warehouseList;
        private Company selectedCompanyIndex;
        private ObservableCollection<Article> articleList;
        private Article selectedArticle;
        private ObservableCollection<ArticleSupplier> supplierList;
        public bool GenerateReport;
        private ArticleSupplier selectedSupplier;
        private bool isBusy;
        private Visibility supplierClearButtonVisibility;
        private Visibility articleClearButtonVisibility;

        #endregion // Declaration

        #region Properties



        public virtual List<object> SelectedItems { get; set; }
        public ObservableCollection<Warehouses> WarehouseList
        {
            get
            {
                return warehouseList;
            }
            set
            {
                warehouseList = value;
                OnPropertyChanged("WarehouseList");
            }
        }

        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set { entireCompanyPlantList = value; }
        }
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }

        public ObservableCollection<Customer> GroupList
        {
            get { return groupList; }
            set
            {
                groupList = value;
                OnPropertyChanged("GroupList");

            }
        }
        public ObservableCollection<Article> ArticleList
        {
            get { return articleList; }
            set
            {
                articleList = value;
                OnPropertyChanged("ArticleList");

            }
        }
        public Article SelectedArticle
        {
            get { return selectedArticle; }
            set
            {
                selectedArticle = value;
                OnPropertyChanged("SelectedArticle");
                if (SelectedArticle != null)
                    ArticleClearButtonVisibility = Visibility.Visible;
            }
        }
        public ObservableCollection<ArticleSupplier> SupplierList
        {
            get { return supplierList; }
            set
            {
                supplierList = value;
                OnPropertyChanged("SupplierList");

            }
        }
        public ArticleSupplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set
            {
                selectedSupplier = value;
                OnPropertyChanged("SelectedSupplier");
                if (SelectedSupplier != null)
                    SupplierClearButtonVisibility = Visibility.Visible;
            }
        }

        public Visibility SupplierClearButtonVisibility
        {
            get { return supplierClearButtonVisibility; }
            set
            {
                supplierClearButtonVisibility = value;
                OnPropertyChanged("SupplierClearButtonVisibility");

            }
        }

        public Visibility ArticleClearButtonVisibility
        {
            get { return articleClearButtonVisibility; }
            set
            {
                articleClearButtonVisibility = value;
                OnPropertyChanged("ArticleClearButtonVisibility");

            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }
        #endregion // Properties




        #region public Events

        public event EventHandler RequestClose;
        // Property Change Logic 
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // Public Events

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
                    me[BindableBase.GetPropertyName(() => SelectedItems)];
                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                //if (!allowValidation) return null;
                string selectedItems = BindableBase.GetPropertyName(() => SelectedItems);

                if (columnName == selectedItems)
                    return PurchasingReportFilterValidation.GetErrorMessage(selectedItems, SelectedItems);

                return null;
            }
        }

        #endregion
        public void Dispose()
        {
        }

    }
}
