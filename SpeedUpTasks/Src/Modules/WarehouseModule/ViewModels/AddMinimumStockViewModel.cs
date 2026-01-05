using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class AddMinimumStockViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Declaration

        private string windowHeader;
        private string fullName;
        private long minStock;
        private bool isNew;
        private bool isSave;
        private long maxStock;

        #endregion

        #region Public ICommand
        public ICommand AddLocationsMinimumStockViewCancelButtonCommand { get; set; }
        public ICommand AddLocationsMinimumStockViewAcceptButtonCommand { get; set; }

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

        #endregion

        #region Properties

        public WarehouseLocation ExistWarehouseLocation { get; set; }
        public ArticleWarehouseLocations NewArticleWarehouseLocations { get; set; }
        public ArticlesStock NewArticlesStock { get; set; }
        public ArticleWarehouseLocations UpdateArticleWarehouseLocations { get; set; }
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FullName"));
            }
        }

        public long MinStock
        {
            get { return minStock; }
            set
            {
                minStock = value;
                string Error = EnableValidationAndGetError();
                OnPropertyChanged(new PropertyChangedEventArgs("MaxStock"));
                OnPropertyChanged(new PropertyChangedEventArgs("MinStock"));
            }
        }

        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }

        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        public long MaxStock
        {
            get { return maxStock; }
            set
            {
                maxStock = value;
                string Error = EnableValidationAndGetError();
                OnPropertyChanged(new PropertyChangedEventArgs("MaxStock"));
                OnPropertyChanged(new PropertyChangedEventArgs("MinStock"));
            }
        }

        #endregion

        #region Constructor
        public AddMinimumStockViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddLocationsMinimumStockViewModel()...", category: Category.Info, priority: Priority.Low);
                AddLocationsMinimumStockViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddLocationsMinimumStockViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddLocationsMinimumStock));
                GeosApplication.Instance.Logger.Log("Constructor AddLocationsMinimumStockViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddLocationsMinimumStockViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Mehods

        public void Init(WarehouseLocation wLocation)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = System.Windows.Application.Current.FindResource("AddArticleLocation").ToString();
                ExistWarehouseLocation = wLocation;
                FullName = ExistWarehouseLocation.FullName;
                MinStock = 0;
                MaxStock = 0;
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to Close Window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void AddLocationsMinimumStock(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddLocationsMinimumStock()...", category: Category.Info, priority: Priority.Low);
                string Error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("MaxStock"));
                if (!string.IsNullOrEmpty(Error))
                {
                    return;
                }
               
               if (IsNew)
               {
                   NewArticlesStock = new ArticlesStock()
                   {
                        Quantity = 0
                   };

                   NewArticleWarehouseLocations = new ArticleWarehouseLocations()
                   {
                         WarehouseLocation = ExistWarehouseLocation,
                         IdWarehouseLocation = ExistWarehouseLocation.IdWarehouseLocation,
                         ArticlesStock = NewArticlesStock,
                         MinimumStock = MinStock,
                         MaximumStock = MaxStock
                   };

                   IsSave = true;
                   CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LocationsMinimumStockInformationAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                   RequestClose(null, null);
               }
               else
               {
                  UpdateArticleWarehouseLocations.MinimumStock = MinStock;
                  UpdateArticleWarehouseLocations.MaximumStock = MaxStock;
                  IsSave = true;
                  CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LocationsMinimumStockInformationUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                  RequestClose(null, null);
               }
                GeosApplication.Instance.Logger.Log("Method AddLocationsMinimumStock()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddLocationsMinimumStock()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(ArticleWarehouseLocations articleWarehouseLocations)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = System.Windows.Application.Current.FindResource("EditArticleLocation").ToString();
                UpdateArticleWarehouseLocations = articleWarehouseLocations;
                FullName = UpdateArticleWarehouseLocations.WarehouseLocation.FullName;
                MinStock = UpdateArticleWarehouseLocations.MinimumStock;
                MaxStock = UpdateArticleWarehouseLocations.MaximumStock;
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region VAlidation
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
        public string Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo DataError = (IDataErrorInfo)this;
                string error = DataError[BindableBase.GetPropertyName(() => MaxStock)];
                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";
                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string max = BindableBase.GetPropertyName(() => MaxStock);
                if (columnName == max)
                {
                    if (MaxStock < MinStock)
                        return (System.Windows.Application.Current.FindResource("MaximumStockErrorMessage").ToString());
                }
                string min = BindableBase.GetPropertyName(() => MinStock);
                if (columnName == min)
                {
                    if (MaxStock < MinStock)
                    {
                        return (System.Windows.Application.Current.FindResource("MinimumStockErrorMessage").ToString());
                    }
                }
                return null;
            }

        }
        #endregion

    }
}
