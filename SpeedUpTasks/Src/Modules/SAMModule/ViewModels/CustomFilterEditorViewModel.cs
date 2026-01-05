using DevExpress.Mvvm;
using DevExpress.Xpf.Editors.Filtering;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Emdep.Geos.UI.Helper;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using Emdep.Geos.UI.Validations;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class CustomFilterEditorViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Declearation

        private string filterCriteria;
        private string filterName;
        private bool isSave;
        private bool isDelete;
        private ObservableCollection<TileBarFilters> filterLocationList;
        private bool isNew;
        private bool isCancel;

        #endregion

        #region Properties

        FilterControl filterControl { get; set; }
        public string ExistFilterName { get; set; }
        public string FilterName
        {
            get { return filterName; }
            set
            {
                filterName = value.TrimStart();
                OnPropertyChanged(new PropertyChangedEventArgs("FilterName"));
            }
        }

        public string FilterCriteria
        {
            get { return filterCriteria; }
            set
            {
                filterCriteria = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterCriteria"));
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

        public bool IsDelete
        {
            get { return isDelete; }
            set
            {
                isDelete = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDelete"));
            }
        }

        public ObservableCollection<TileBarFilters> FilterLocationList
        {
            get { return filterLocationList; }
            set
            {
                filterLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterLocationList"));
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

        public bool IsCancel
        {
            get { return isCancel; }
            set
            {
                isCancel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCancel"));
            }
        }

        #endregion

        #region ICommand

        public ICommand CustomFilterEditorAcceptCommand { get; set; }
        public ICommand CustomFilterEditorCancelCommand { get; set; }
        public ICommand CustomFilterEditorDeleteCommand { get; set; }
        public ICommand DXWindowCloseCommand { get; set; }

        #endregion

        #region Public Event

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RequestClose;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region Constructor

        public CustomFilterEditorViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor CustomFilterEditorViewModel...", category: Category.Info, priority: Priority.Low);

                CustomFilterEditorAcceptCommand = new DelegateCommand<object>(AcceptCustomFilterEditorAction);
                CustomFilterEditorCancelCommand = new DelegateCommand<object>(CancelCustomFilterEditorAction);
                CustomFilterEditorDeleteCommand = new DelegateCommand<object>(DeleteCustomFilterEditor);
                DXWindowCloseCommand = new DelegateCommand<object>(CloseDXWindow);

                GeosApplication.Instance.Logger.Log("Constructor CustomFilterEditorViewModel executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomFilterEditorViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        private void AcceptCustomFilterEditorAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptCustomFilterEditor()...", category: Category.Info, priority: Priority.Low);
                string error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("FilterName"));

                if (error != null)
                {
                    return;
                }
                var existFilter = FilterLocationList.FirstOrDefault(x => x.Caption.Equals(FilterName));
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

                GeosApplication.Instance.Logger.Log("Method AcceptCustomFilterEditor()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptCustomFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CancelCustomFilterEditorAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelCustomFilterEditor()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CancelCustomFilterEditor()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelCustomFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteCustomFilterEditor(Object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteCustomFilterEditor()...", category: Category.Info, priority: Priority.Low);
                if (FilterName != null && !string.IsNullOrEmpty(FilterName) && IsSave)
                {
                    IsDelete = true;
                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log("Method DeleteCustomFilterEditor()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteCustomFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseDXWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method  CloseDXWindow()...", category: Category.Info, priority: Priority.Low);
                FilterCriteria = filterControl.FilterCriteria.ToString();
                GeosApplication.Instance.Logger.Log("Method  CloseDXWindow()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseDXWindow() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init(FilterControl Control, ObservableCollection<TileBarFilters> FirstLevelLocationListOfTile)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                filterControl = Control;
                FilterLocationList = FirstLevelLocationListOfTile;
                ExistFilterName = FilterName;
                IsSave = true;
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init() " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
