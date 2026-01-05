using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Emdep.Geos.Modules.PCM.Views;
using System.Windows;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class DuplicateModulesAdditionalInformationViewModel : INotifyPropertyChanged
    {
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

        #region Declarations
        #region[GEOS2-4262][rdixit][29.03.2023]
        private Visibility isDuplicateDetectionButtonVisible;
        Visibility isDuplicateModuleButtonVisible;
        private bool isCheckedPrice;
        #endregion
        private bool isDuplicateModulesButtonEnabled;
        private string duplicateCode;
        public bool isDuplicateClicked = false;

        //DuplicateUserControl
        private bool isCheckedImages;
        private bool isCheckedAttachment;
        private bool isCheckedLinks;
        private bool isCheckedCustomers;
        private bool isCheckedCompatibility;

        private bool isCheckedRealtedModules;//[Sudhir.Jangra][GEOS2-4468][31/05/2023]

        #endregion

        #region Properties
        #region[GEOS2-4262][rdixit][29.03.2023]
        public Visibility IsDuplicateDetectionButtonVisible
        {
            get
            {
                return isDuplicateDetectionButtonVisible;
            }

            set
            {
                isDuplicateDetectionButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDuplicateDetectionButtonVisible"));
            }
        }
        public Visibility IsDuplicateModuleButtonVisible
        {
            get
            {
                return isDuplicateModuleButtonVisible;
            }

            set
            {
                isDuplicateModuleButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDuplicateModuleButtonVisible"));
            }
        }
        public bool IsCheckedPrice
        {
            get
            {
                return isCheckedPrice;
            }

            set
            {
                isCheckedPrice = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedPrice"));
            }
        }
        #endregion
        public bool IsDuplicateModulesButtonEnabled
        {
            get
            {
                return isDuplicateModulesButtonEnabled;
            }

            set
            {
                isDuplicateModulesButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDuplicateModulesButtonEnabled"));
            }
        }
        public string DuplicateCode
        {
            get
            {
                return duplicateCode;
            }

            set
            {
                duplicateCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DuplicateCode"));
            }
        }

        public bool IsCheckedImages
        {
            get { return isCheckedImages; }
            set { isCheckedImages = value; }
        }

        public bool IsCheckedAttachment
        {
            get { return isCheckedAttachment; }
            set { isCheckedAttachment = value; }
        }

        public bool IsCheckedLinks
        {
            get { return isCheckedLinks; }
            set { isCheckedLinks = value; }
        }
        public bool IsCheckedCustomers
        {
            get { return isCheckedCustomers; }
            set { isCheckedCustomers = value; }
        }
        public bool IsCheckedCompatibility
        {
            get { return isCheckedCompatibility; }
            set { isCheckedCompatibility = value; }
        }
        public bool IsCheckedRelatedModules  //[Sudhir.Jangra][GEOS2-4468][31/05/2023]
        {
            get { return isCheckedRealtedModules; }
            set { isCheckedRealtedModules = value; }
        }
        #endregion

        #region ICommands
        public ICommand DuplicateAcceptButtonCommand { get; set; }
        public ICommand DuplicateCancelButtonCommand { get; set; }

        #endregion

        #region Constructor
        public DuplicateModulesAdditionalInformationViewModel(DuplicateModulesAdditionalInformationView duplicateModulesAdditionalInformationView)
        {
            DuplicateAcceptButtonCommand = new RelayCommand(new Action<object>(DuplicateAcceptCommandAction));
            DuplicateCancelButtonCommand = new RelayCommand(new Action<object>(DuplicateCancelWindow));

            EventHandler handle = delegate { duplicateModulesAdditionalInformationView.Close(); };
            this.RequestClose += handle;
            duplicateModulesAdditionalInformationView.DataContext = this;

            // IsCheckedAttachment = IsCheckedCompatibility = IsCheckedCustomers = IsCheckedImages = IsCheckedLinks = IsCheckedPrice = true;
            //[Sudhir.Jangra][GEOS2-4468][31/05/2023] Added RelatedModules
            IsCheckedAttachment = IsCheckedCompatibility = IsCheckedCustomers = IsCheckedImages = IsCheckedLinks = IsCheckedRelatedModules = IsCheckedPrice = true;
        }
        #endregion

        #region Method
        //DuplicateUserControl
        void DuplicateAcceptCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DuplicateAcceptCommandAction()...", category: Category.Info, priority: Priority.Low);
                isDuplicateClicked = true;
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method DuplicateAcceptCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DuplicateAcceptCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DuplicateCancelWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DuplicateCancelWindow()...", category: Category.Info, priority: Priority.Low);
                isDuplicateClicked = false;
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method DuplicateCancelWindow()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DuplicateCancelWindow()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


    }
}
