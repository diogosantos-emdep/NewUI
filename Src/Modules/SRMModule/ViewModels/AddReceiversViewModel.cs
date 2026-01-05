using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class AddReceiversViewModel : INotifyPropertyChanged
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
        ObservableCollection<Contacts> excludedContactList;
        ObservableCollection<Contacts> includedContactList;
        bool isSave;
        #endregion

        #region Properties
        public ObservableCollection<Contacts> ExcludedContactList
        {
            get { return excludedContactList; }
            set
            {
                excludedContactList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExcludedContactList"));
            }
        }

        public ObservableCollection<Contacts> IncludedContactList
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
        public AddReceiversViewModel()
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
        public void Init(List<Contacts> excludedList, ObservableCollection<Contacts> includedList)
        {
            ExcludedContactList = new ObservableCollection<Contacts>(excludedList);
            IncludedContactList = new ObservableCollection<Contacts>(includedList);

            foreach (Contacts contacts in ExcludedContactList)
            {
                if (contacts.OwnerImage == null)
                {
                    if (!string.IsNullOrEmpty(contacts.ImageText))
                    {
                        byte[] imageBytes = Convert.FromBase64String(contacts.ImageText);
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                        contacts.OwnerImage = byteArrayToImage(imageBytes);
                    }
                    else    // If User is Null then Show temporary image by gender.
                    {
                        if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (contacts.IdGender == 1)
                                contacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else if (contacts.IdGender == 2)
                                contacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        }
                        else
                        {
                            if (contacts.IdGender == 1)
                                contacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else if (contacts.IdGender == 2)
                                contacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        }
                    }
                }
            }


            foreach (Contacts contacts in IncludedContactList)
            {
                if (contacts.OwnerImage == null)
                {
                    if (!string.IsNullOrEmpty(contacts.ImageText))
                    {
                        byte[] imageBytes = Convert.FromBase64String(contacts.ImageText);
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                        contacts.OwnerImage = byteArrayToImage(imageBytes);
                    }
                    else    // If User is Null then Show temporary image by gender.
                    {
                        if (ThemeManager.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (contacts.IdGender == 1)
                                contacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else if (contacts.IdGender == 2)
                                contacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        }
                        else
                        {
                            if (contacts.IdGender == 1)
                                contacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else if (contacts.IdGender == 2)
                                contacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        }
                    }
                }
            }


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
