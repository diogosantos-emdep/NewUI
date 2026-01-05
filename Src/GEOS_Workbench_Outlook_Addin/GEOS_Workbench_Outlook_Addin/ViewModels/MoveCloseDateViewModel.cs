using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    class MoveCloseDateViewModel : INotifyPropertyChanged
    {
        #region Declaration

        private DateTime offerCloseDateMinValue;
        private DateTime offerCloseDate;

        #endregion // Declaration

        #region  public Properties

        public DateTime OfferCloseDateMinValue
        {
            get { return offerCloseDateMinValue; }
            set
            {
                offerCloseDateMinValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferCloseDateMinValue"));
            }
        }

        public DateTime OfferCloseDate
        {
            get { return offerCloseDate; }
            set
            {
                offerCloseDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferCloseDate"));
            }
        }

        #endregion // Properties

        #region public ICommand

        public ICommand MoveCloseDateViewCancelButtonCommand { get; set; }
        public ICommand MoveCloseDateViewAcceptButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion // ICommand

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

        public MoveCloseDateViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor MoveCloseDateViewModel ...", category: Category.Info, priority: Priority.Low);

            MoveCloseDateViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            OfferCloseDateMinValue = GeosApplication.Instance.ServerDateTime.Date;
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
            GeosApplication.Instance.Logger.Log("Constructor MoveCloseDateViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion // Constructor

        #region Methods

        /// <summary>
        /// Method for close Window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
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
        #endregion // Methods
    }
}
