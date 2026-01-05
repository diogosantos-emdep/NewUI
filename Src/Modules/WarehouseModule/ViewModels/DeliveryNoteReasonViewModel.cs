using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
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
    public class DeliveryNoteReasonViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services

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

        #region Declaration
        string windowHeader;
        private Visibility isOtherSelected;
        private bool isQualityReasonSelected;
        private bool isMissingReasonSelected;
        private bool isPendingReasonSelected;
        private bool isOtherReasonSelected;
        private string otherReasonComment;
        private WarehouseDeliveryNoteItem newReason;
        private bool isSave;
        #endregion

        #region Properties
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public Visibility IsOtherSelected
        {
            get { return isOtherSelected; }
            set
            {
                isOtherSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOtherSelected"));
            }
        }

        public bool IsQualityReasonSelected
        {
            get { return isQualityReasonSelected; }
            set
            {
                isQualityReasonSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsQualityReasonSelected"));
            }
        }

        public bool IsMissingReasonSelected
        {
            get { return isMissingReasonSelected; }
            set
            {
                isMissingReasonSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMissingReasonSelected"));
            }
        }

        public bool IsPendingReasonSelected
        {
            get { return isPendingReasonSelected; }
            set
            {
                isPendingReasonSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPendingReasonSelected"));
            }
        }

        public bool IsOtherReasonSelected
        {
            get { return isOtherReasonSelected; }
            set
            {
                isOtherReasonSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOtherReasonSelected"));
                if (IsOtherReasonSelected)
                {
                    IsOtherSelected = Visibility.Visible;
                }
                else
                {
                    IsOtherSelected = Visibility.Hidden;
                }
            }
        }

        public string OtherReasonComment
        {
            get { return otherReasonComment; }
            set
            {
                otherReasonComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtherReasonComment"));
            }
        }

        public WarehouseDeliveryNoteItem NewReason
        {
            get { return newReason; }
            set
            {
                newReason = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewReason"));
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
        #endregion

        #region Public ICommand
        public ICommand CancelButtonCommand { get; set; }

        public ICommand AcceptButtonCommand { get; set; }

      
        #endregion

        #region Constructor
        public DeliveryNoteReasonViewModel()
        {
            WindowHeader = Application.Current.FindResource("DeliveryNoteReasonHeader").ToString();
            CancelButtonCommand = new DelegateCommand<object>(CancelButtonAction);
            AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonAction);
            IsOtherReasonSelected = false;
            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }
        #endregion

        #region Method

        private void AcceptButtonAction(object obj)
        {
            try
            {
                if (NewReason==null)
                {
                    NewReason = new WarehouseDeliveryNoteItem();
                }
                if (!IsQualityReasonSelected&& !IsMissingReasonSelected&& !IsPendingReasonSelected&& !IsOtherReasonSelected)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeliveryNoteReasonValidationMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                if (IsQualityReasonSelected)
                {
                    NewReason.IdLockedReason = 1912;
                    NewReason.ReasonComment = "Quality";
                }
                else if (IsMissingReasonSelected)
                {
                    NewReason.IdLockedReason = 1913;
                    NewReason.ReasonComment = "Missing material from supplier";
                }
                else if (IsPendingReasonSelected)
                {
                    NewReason.IdLockedReason = 1914;
                    NewReason.ReasonComment = "Pending of adjustments";
                }
                else if (IsOtherReasonSelected)
                {
                    NewReason.IdLockedReason = 1915;
                    if (!string.IsNullOrEmpty(OtherReasonComment))
                    {
                        NewReason.ReasonComment = OtherReasonComment;
                    }
                    else
                    {
                        NewReason.ReasonComment = "Others";
                    }
                    
                }
                IsSave = true;
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CancelButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonAction()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CancelButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Validation
        public string Error => null;
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        public string this[string columnName]
        {
            get
            {
                if (columnName== "IsQualityReasonSelected"&&!IsQualityReasonSelected)
                {
                    return "Please select any reason.";
                }
                return null;
            }
        }
        #endregion
    }
}
