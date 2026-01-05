using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.OTM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.OTM.ViewModels
{
    //[pramod.misal][GEOS2-6461][24-10-2024] https://helpdesk.emdep.com/browse/GEOS2-6461
    public class MyPreferencesViewModel : ViewModelBase, INotifyPropertyChanged
    {

        #region Services
        IOTMService OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IOTMService OTMService = new OTMServiceController("localhost:6699");

        #endregion Services

        #region Public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        // new public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region declaration
        private List<Currency> currencies;
        private Currency selectedCurrency;
        #endregion declaration

        #region Properties
        public List<Currency> Currencies
        {
            get
            {
                return currencies;
            }

            set
            {
                currencies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Currencies"));
            }
        }

        public Currency SelectedCurrency
        {
            get
            {
                return selectedCurrency;
            }

            set
            {
                selectedCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrency"));
            }
        }
        #endregion end Properties

        #region ICommand
        public ICommand MyPreferencesViewAcceptButtonCommand { get; set; }
        public ICommand MyPreferencesViewCancelButtonCommand { get; set; }

        #endregion

        #region Constructor
        public MyPreferencesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor MyPreferencesViewModel ...", category: Category.Info, priority: Priority.Low);

                MyPreferencesViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                MyPreferencesViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveMyPreference));

                if (!GeosApplication.Instance.UserSettings.ContainsKey("OTM_SelectedCurrency"))
                {
                    GeosApplication.Instance.UserSettings.Add("OTM_SelectedCurrency", "EUR");
                }
                else if (GeosApplication.Instance.UserSettings.ContainsKey("OTM_SelectedCurrency"))
                {
                    if (GeosApplication.Instance.UserSettings["OTM_SelectedCurrency"] == "")
                    {
                        GeosApplication.Instance.UserSettings["OTM_SelectedCurrency"] = "EUR";

                    }
                }

                FillCurrencyDetails();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor MyPreferencesViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor MyPreferencesViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        //[pramod.misal][GEOS2-6461][24.10.2024]
        public void SaveMyPreference(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveMyPreference() ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                List<Tuple<string, string>> userConfigurations = new List<Tuple<string, string>>();

                if (!GeosApplication.Instance.UserSettings.ContainsKey("OTM_SelectedCurrency"))
                {
                    GeosApplication.Instance.UserSettings.Add("OTM_SelectedCurrency", "EUR");
                }

                //selected currency
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("OTM_SelectedCurrency"))
                {
                    GeosApplication.Instance.UserSettings["OTM_SelectedCurrency"] = SelectedCurrency.Name;
                }
                foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                {
                    userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                }

                ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Method SaveMyPreference()....executed successfully"), category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in SaveMyPreference() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void FillCurrencyDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails ...", category: Category.Info, priority: Priority.Low);


                Currencies = GeosApplication.Instance.Currencies.ToList();

                if (Currencies != null)
                {
                    foreach (var bpItem in Currencies.GroupBy(tpa => tpa.Name))
                    {
                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                        bpItem.ToList().Where(oti => oti.Name == bpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage);
                    }
                }

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("OTM_SelectedCurrency"))
                {
                    if (GeosApplication.Instance.UserSettings["OTM_SelectedCurrency"] == "")
                    {
                        GeosApplication.Instance.UserSettings["OTM_SelectedCurrency"] = "EUR";
                        SelectedCurrency = Currencies.FirstOrDefault(i => i.Name == "EUR");
                    }
                    else
                        SelectedCurrency = Currencies.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["OTM_SelectedCurrency"]);
                }

                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }
        #endregion

    }
}
