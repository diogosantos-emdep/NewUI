using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Xpf.Editors.Filtering;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using Emdep.Geos.UI.Helper;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using Emdep.Geos.UI.Validations;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class FilterEditorViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
        #region Declaration

        private string filterName;
        private bool isSave;
        private string filterCriteria;
        private bool isNew;
        private bool isDelete;
        private bool isCancel;
        private ObservableCollection<TileBarFilters> filterStatusList;

        #endregion

        #region Properties
        FilterControl filterControl { get; set; }
        public string ExistFilterName { get; set; }
        
        public string FilterName
        {
            get
            {
                return filterName;
            }

            set
            {
                filterName = value.TrimStart();
                OnPropertyChanged(new PropertyChangedEventArgs("FilterName"));
            }
        }
        public bool IsSave
        {
            get
            {
                return isSave;
            }

            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        public string FilterCriteria
        {
            get
            {
                return filterCriteria;
            }

            set
            {
                filterCriteria = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterCriteria"));
            }
        }
        public bool IsNew
        {
            get
            {
                return isNew;
            }

            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }
        public bool IsDelete
        {
            get
            {
                return isDelete;
            }

            set
            {
                isDelete = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDelete"));
            }
        }
        public bool IsCancel
        {
            get
            {
                return isCancel;
            }

            set
            {
                isCancel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCancel"));
            }
        }
        public ObservableCollection<TileBarFilters> FilterStatusList
        {
            get
            {
                return filterStatusList;
            }

            set
            {
                filterStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterStatusList"));
            }
        }

        #endregion

        #region public ICommand
        public ICommand FilterEditorViewCancelCommand { get; set; }
        public ICommand FilterEditorViewAcceptCommand { get; set; }
        public ICommand FilterEditorViewDeleteButtonCommand { get; set; }
        public ICommand DXWindowClosingCommand { get; set; }

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

        #region Constructor
        public FilterEditorViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FilterEditorViewModel...", category: Category.Info, priority: Priority.Low);
                FilterEditorViewCancelCommand = new DelegateCommand<object>(FilterEditorViewCancelCommandAction);
                FilterEditorViewAcceptCommand = new DelegateCommand<object>(FilterEditorViewAcceptCommandAction);
                FilterEditorViewDeleteButtonCommand = new DelegateCommand<object>(FilterEditorViewDeleteButtonCommandAction);
                DXWindowClosingCommand = new DelegateCommand<object>(DXWindowClosingCommandAction);
                GeosApplication.Instance.Logger.Log("Constructor FilterEditorViewModel executed successfully", category: Category.Info, priority: Priority.Low);
            }
           
            catch (Exception ex)
            {                
                GeosApplication.Instance.Logger.Log("Get an error in FilterEditorViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        private void FilterEditorViewAcceptCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FilterEditorViewAcceptCommandAction()...", category: Category.Info, priority: Priority.Low);
                string error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("FilterName"));

                if (error != null)
                {
                    return;
                }
                var existFilter = FilterStatusList.FirstOrDefault(x => x.Caption.Equals(FilterName));
                if (IsNew)
                {
                   
                        if (existFilter == null)
                        {
                            filterControl.ApplyFilter();
                           FilterCriteria = filterControl.FilterCriteria.ToString();
                            IsDelete = false;
                            IsCancel = true;
                            RequestClose(null, null);
                        }
                        else
                        {
                            IsSave = false;
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomFilterNameExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                    
                }
                else
                {
                    if (existFilter == null || ExistFilterName.Equals(FilterName))
                    {
                        filterControl.ApplyFilter();
                        FilterCriteria = filterControl.FilterCriteria.ToString();
                        IsDelete = false;
                        IsCancel = true;
                        RequestClose(null, null);
                    }
                    else
                    {
                        IsSave = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomFilterNameExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FilterEditorViewAcceptCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FilterEditorViewAcceptCommandAction() Method....executed successfully", category: Category.Info, priority: Priority.Low);
            }
        }
        private void FilterEditorViewCancelCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FilterEditorViewCancelCommandAction()...", category: Category.Info, priority: Priority.Low);                
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method FilterEditorViewCancelCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FilterEditorViewCancelCommandAction() Method", category: Category.Info, priority: Priority.Low);
            }
        }

        public void Init(FilterControl Control, ObservableCollection<TileBarFilters> filterStatusListOfTile)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);              
                filterControl = Control;
                FilterStatusList = filterStatusListOfTile;
                ExistFilterName = FilterName;
                IsSave = true;
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method", category: Category.Info, priority: Priority.Low);
            }
        }

        private void FilterEditorViewDeleteButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FilterEditorViewDeleteButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (FilterName != null && !string.IsNullOrEmpty(FilterName) && IsSave)
                {
                    IsDelete = true;
                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log("Method FilterEditorViewDeleteButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FilterEditorViewDeleteButtonCommandAction() Method", category: Category.Info, priority: Priority.Low);
            }
        }

        private void DXWindowClosingCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DXWindowClosingCommandAction()...", category: Category.Info, priority: Priority.Low);
                FilterCriteria = filterControl.FilterCriteria.ToString();
                GeosApplication.Instance.Logger.Log("Method DXWindowClosingCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DXWindowClosingCommandAction() Method", category: Category.Info, priority: Priority.Low);
            }           
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
                    me[BindableBase.GetPropertyName(() => FilterName)];

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
                string Name = BindableBase.GetPropertyName(() => FilterName);
                if (columnName == Name)
                {
                    return CustomFilterEditorValidation.GetErrorMessage(Name, FilterName);
                }

                return null;
            }
        }
        #endregion
    }
}
