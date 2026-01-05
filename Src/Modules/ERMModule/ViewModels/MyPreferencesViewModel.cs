using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.LayoutControl;
using Emdep.Geos.Utility;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Data.Common;
using Prism.Logging;
using System.Linq;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.PLM;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.ERM.Views;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    class MyPreferencesViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

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

        #endregion // End Of Events 

        #region declaration
        MaximizedElementPosition selectedAppearanceItem;
        private List<Currency> currencies;
        private Currency selectedCurrency;
        private string selectedappearance;
        private ObservableCollection<BasePrice> basePricelist;
        private ObservableCollection<BasePrice> basePricelistFinal;
        private int idCurrency;

        #endregion declaration

        #region Properties
        //public ObservableCollection<BasePrice> BasePricelist
        //{
        //    get { return basePricelist; }
        //    set
        //    {
        //        basePricelist = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("BasePricelist"));
        //    }
        //}


        public string SelectedAppearance
        {
            get
            {
                return selectedappearance;
            }

            set
            {
                selectedappearance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAppearance"));
            }
        }

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

        public int IdCurrency
        {
            get { return idCurrency; }
            set
            {
                idCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCurrency"));
            }
        }

        public MaximizedElementPosition SelectedAppearanceItem
        {
            get { return selectedAppearanceItem; }
            set
            {
                selectedAppearanceItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAppearanceItem"));
            }
        }

        #endregion Properties


        #region ICommand

        public ICommand MyPreferencesViewCancelButtonCommand { get; set; }
        public ICommand MyPreferencesViewAcceptButtonCommand { get; set; }

        #endregion  ICommand

        #region Constructor
        public MyPreferencesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor MyPreferencesViewModel ...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); } //[GEOS2-4636][rupali sarode][04-07-2023]

                List<Tuple<string, string>> userConfigurations = new List<Tuple<string, string>>();

                MyPreferencesViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));

                MyPreferencesViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveMyPreference));


                if (!GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                {
                    GeosApplication.Instance.UserSettings.Add("ERM_SelectedCurrency", "EUR");
                }
                else if (GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                {
                    if (GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"] == "")
                    {
                        GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"] = "EUR";

                    }
                }

                //Appearance
                if (!GeosApplication.Instance.UserSettings.ContainsKey("ERM_Appearance"))
                {
                    GeosApplication.Instance.UserSettings.Add("ERM_Appearance", "Top");
                }
                else if (GeosApplication.Instance.UserSettings.ContainsKey("ERM_Appearance"))
                {
                    if (GeosApplication.Instance.UserSettings["ERM_Appearance"] == "")
                    {
                        GeosApplication.Instance.UserSettings["ERM_Appearance"] = "Top";

                    }
                }

                foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                {
                    userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                }
                ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");

                FillCurrencyDetails();
                //Appearance
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_Appearance"))
                {
                    if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["ERM_Appearance"].ToString()))
                    {
                        SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), "Top", true);
                    }
                    else
                    {
                        SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), GeosApplication.Instance.UserSettings["ERM_Appearance"].ToString(), true);
                    }
                }
                else
                {
                    SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), "Top", true);
                }

                FillapperanceDetails();


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][04-07-2023]

                GeosApplication.Instance.Logger.Log("Constructor MyPreferencesViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor MyPreferencesViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion Constructor


        #region Methods

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }


        /// <summary>
        /// Method for get fill currency list and get IdCurrency By current System Region Culture.
        /// </summary>
        /// 

        //[Aishwarya Ingale][4920][22/11/2023]
        public void FillapperanceDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillapperanceDetails ...", category: Category.Info, priority: Priority.Low);

                //  Appearance = GeosApplication.Instance.Appearance.ToList();

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_Appearance"))
                {
                    if (GeosApplication.Instance.UserSettings["ERM_Appearance"] == "")
                    {
                        GeosApplication.Instance.UserSettings["ERM_Appearance"] = "Top";
                        SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), "Top", true);
                    }
                    else
                        SelectedAppearance = GeosApplication.Instance.UserSettings["ERM_Appearance"];
                }

                GeosApplication.Instance.Logger.Log("Method FillapperanceDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillapperanceDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void FillCurrencyDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails ...", category: Category.Info, priority: Priority.Low);

                Currencies = GeosApplication.Instance.Currencies.ToList();

                

                //if (!GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))

                //    SelectedCurrency = Currencies.FirstOrDefault(i => i.Name == "EUR");


                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                {
                    if (GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"] == "")
                    {
                        GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"] = "EUR";
                        SelectedCurrency = Currencies.FirstOrDefault(i => i.Name == "EUR");
                    }
                    else
                        SelectedCurrency = Currencies.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"]);
                }

                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SaveMyPreference(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveMyPreference() ...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }  //[GEOS2-4636][rupali sarode][04-07-2023]

                //[Aishwarya Ingale][4920][22/11/2023]
                //Appearance
                List<Tuple<string, string>> userConfigurations = new List<Tuple<string, string>>();

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_Appearance"))
                {
                    GeosApplication.Instance.UserSettings["ERM_Appearance"] = SelectedAppearanceItem.ToString();
                }

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_Appearance"))
                {
                    SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), "Top", true);
                }


                if (!GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                {
                    GeosApplication.Instance.UserSettings.Add("ERM_SelectedCurrency", "EUR");
                }
                //selected currency
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                {
                    GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"] = SelectedCurrency.Name;
                }
                foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                {
                    userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                }

                ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                ERMCommon.Instance.ERM_Appearance = SelectedAppearanceItem.ToString();//[GEOS2-4920][Aishwarya Ingale][21-11-2023]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][04-07-2023]

                GeosApplication.Instance.Logger.Log(string.Format("Method SaveMyPreference()....executed successfully"), category: Category.Info, priority: Priority.Low);

                RequestClose(null, null);


            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log("Error in SaveMyPreference() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


    }
}
