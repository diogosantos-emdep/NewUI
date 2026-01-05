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
using Emdep.Geos.Modules.PCM.Common_Classes;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class MyPreferencesViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());



        #endregion

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

        #region Declarations

        MaximizedElementPosition selectedAppearanceItem;


        #endregion

        #region Properties

        public MaximizedElementPosition SelectedAppearanceItem
        {
            get { return selectedAppearanceItem; }
            set
            {
                selectedAppearanceItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAppearanceItem"));
            }
        }

        #endregion

        #region ICommand

        public ICommand MyPreferencesViewCancelButtonCommand { get; set; }
        public ICommand MyPreferencesViewAcceptButtonCommand { get; set; }


        #endregion

        #region Constructor

        public MyPreferencesViewModel()
        {
            MyPreferencesViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            MyPreferencesViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveMyPreference));

            //Appearance
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCM_Appearance"))
            {
                if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["PCM_Appearance"].ToString()))
                {
                    SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), "Right", true);
                }
                else
                {
                    SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), GeosApplication.Instance.UserSettings["PCM_Appearance"].ToString(), true);
                }
            }
            else
            {
                SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), "Right", true);
            }

        }

       

        #endregion

        #region Methods

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void SaveMyPreference(object obj)
        {
            //Appearance

            List<Tuple<string, string>> userConfigurations = new List<Tuple<string, string>>();

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCM_Appearance"))
            {
                GeosApplication.Instance.UserSettings["PCM_Appearance"] = SelectedAppearanceItem.ToString();
            }

            foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
            {
                userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
            }

            ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
            GeosApplication.Instance.SelectedPrinter = Convert.ToString(GeosApplication.Instance.UserSettings["SelectedPrinter"]);
            GeosApplication.Instance.LabelPrinter = Convert.ToString(GeosApplication.Instance.UserSettings["LabelPrinter"]);
            GeosApplication.Instance.LabelPrinterModel = Convert.ToString(GeosApplication.Instance.UserSettings["LabelPrinterModel"]);
            GeosApplication.Instance.ParallelPort = Convert.ToString(GeosApplication.Instance.UserSettings["ParallelPort"]);

            PCMCommon.Instance.PCM_Appearance = SelectedAppearanceItem.ToString();
            RequestClose(null, null);
        }

        #endregion

    }
}
