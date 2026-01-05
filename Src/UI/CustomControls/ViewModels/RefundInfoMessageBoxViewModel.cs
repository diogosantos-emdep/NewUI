using DevExpress.Mvvm;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.UI.CustomControls.ViewModels
{

    internal class RefundInfoMessageBoxViewModel : INotifyPropertyChanged
    {
        #region ICommands

        public ICommand CancelButtonCommand { get; set; }
        public ICommand NextButtonCommand { get; set; }
        #endregion

        #region Public Events

        public event EventHandler RequestClose;

    
        #endregion
        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Message"));
            }
        }
        private bool isNext;

        public bool IsNext
        {
            get { return isNext; }
            set
            {
                isNext = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNext"));
            }
        }


        private bool isCancel;

        public bool IsCancel
        {
            get { return isCancel; }
            set
            {
                isCancel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCancel"));
            }
        }

        private string color;

        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Color"));
            }
        }

        private string imagePath;

        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImagePath"));
            }
        }
        public RefundInfoMessageBoxViewModel()
        {
            //DownloadLogs = "test";
            CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
            NextButtonCommand = new DelegateCommand<object>(NextButtonCommandAction);

        }
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                IsCancel = true;
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method CancelButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void NextButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NextButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                IsNext = true;
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method NextButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method NextButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
    }

}
