using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.UI.Common;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.Data.Common;

namespace Emdep.Geos.Modules.OTM.ViewModels
{
    public class AddToAndCcEmailViewModel : INotifyPropertyChanged
    {
        #region Services

        #endregion

        #region Public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events 

        #region Declaration
        ObservableCollection<People> excludedContactList;
        ObservableCollection<People> includedContactList;
        bool isSave;
        #endregion

        #region Properties
        public ObservableCollection<People> ExcludedContactList
        {
            get { return excludedContactList; }
            set
            {
                excludedContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExcludedContactList"));
            }
        }

        public ObservableCollection<People> IncludedContactList
        {
            get { return includedContactList; }
            set
            {
                includedContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IncludedContactList"));
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
        public AddToAndCcEmailViewModel()
        {
            CancelButtonCommand = new DelegateCommand<object>(CancelButtonAction);
            AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
        }
        #endregion

        #region Methods
        private void AcceptButtonCommandAction(object obj)
        {
            isSave = true;
            RequestClose(null, null);
        }
        public void Init(ObservableCollection<People> excludedList, ObservableCollection<People> includedList)
        {
            ExcludedContactList = new ObservableCollection<People>(excludedList);
            IncludedContactList = new ObservableCollection<People>(includedList);

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }

        public ImageSource byteArrayToImage(byte[] byteArrayIn)
        {
            ImageSource imgSrc = null;
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(byteArrayIn);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                biImg.DecodePixelHeight = 10;
                biImg.DecodePixelWidth = 10;

                imgSrc = biImg as ImageSource;

                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return imgSrc;
        }

        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
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
    }
}

