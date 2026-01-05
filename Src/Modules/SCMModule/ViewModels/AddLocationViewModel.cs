using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    //[rushikesh.gaikwad][GEOS2-5752][05.08.2024]
    public class AddLocationViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // ISCMService SCMService = new SCMServiceController("localhost:6699");
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
        #endregion

        #region Declarations
        bool isSave = false;
        private string windowHeader;
        ConnectorLocation selectedPlants;
        private string error = string.Empty;
        private bool isNew;
        private Data.Common.Company selectedPlant;
        private string location;
        private int withcables;
        private int withoutcables;
        private bool onlydamaged;
        public bool isAcceptButtonEnabled; //[rushikesh.gaikwad][GEOS2-5752][16.08.2024]
        #endregion

        #region Properties

        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
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

       

        public ConnectorLocation SelectedPlants
        {
            get { return selectedPlants; }
            set
            {
                selectedPlants = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlants"));
            }
        }

        public bool IsNew
        {
            get
            {
                return isNew;
            }

            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }

        public Data.Common.Company SelectedPlant
        {
            get
            {
                return selectedPlant;
            }

            set
            {
                if (value != null)
                {
                    selectedPlant = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
                }
            }
        }
        ObservableCollection<Data.Common.Company> plantList;
        public ObservableCollection<Data.Common.Company> PlantList
        {
            get
            {
                return plantList;
            }

            set
            {
                plantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantList"));
            }
        }
        public string Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Location"));
                if (string.IsNullOrEmpty(Location)) //[rushikesh.gaikwad][GEOS2-5752][16.08.2024]
                {
                    IsAcceptButtonEnabled = false;
                }
                else
                {
                    IsAcceptButtonEnabled = true;
                }
            }
        }

        public bool IsAcceptButtonEnabled  //[rushikesh.gaikwad][GEOS2-5752][16.08.2024]
        {
            get
            {
                return isAcceptButtonEnabled;
            }

            set
            {
                isAcceptButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptButtonEnabled"));
            }
        }

        public int Withcables
        {
            get
            {
                return withcables;
            }

            set
            {
                withcables = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Withcables"));
            }
        }
        public int Withoutcables
        {
            get
            {
                return withoutcables;
            }

            set
            {
                withoutcables = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Withoutcables"));
            }
        }
        public bool Onlydamaged
        {
            get
            {
                return onlydamaged;
            }

            set
            {
                onlydamaged = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Onlydamaged"));
            }
        }
        #endregion

        #region ICommand
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AddLocationAccept { get; set; }
        public ICommand LocationViewEscapeButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }  //[shweta.thube][GEOS2-6630][04.04.2025]

        #endregion

        #region Constructor

        public AddLocationViewModel()
        {
            try
            {
               
                GeosApplication.Instance.Logger.Log("Method AddNewConnectorViewModel()...", category: Category.Info, priority: Priority.Low);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                AddLocationAccept = new DelegateCommand<object>(AcceptButtonAction);
                LocationViewEscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                PlantList = new ObservableCollection<Data.Common.Company>(SCMCommon.Instance.PlantList);


                // Set the first plant as the default selection
               // [rushikesh.gaikwad][GEOS2-5752][14.08.204]
                if (selectedPlant != null)
                {
                    SelectedPlant = selectedPlant;
                }
                else
                {
                    SelectedPlant = SCMCommon.Instance.PlantList.FirstOrDefault();
                }
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);  //[shweta.thube][GEOS2-6630][04.04.2025]
                GeosApplication.Instance.Logger.Log("Method AddNewConnectorViewModel()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewConnectorViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region  Methods

        public void EditInit(ConnectorLocation selecedLocation)
        {
            if (selecedLocation != null)
            {
                SelectedPlant = PlantList.FirstOrDefault(i => i.IdCompany == selecedLocation.Idsite);
                SelectedPlants = selecedLocation;
                Location = selecedLocation.Location;
                Withcables = selecedLocation.Quantity;
                Withoutcables = selecedLocation.QuantityWithoutWires;
                Onlydamaged = selecedLocation.IsDamaged;
            }
        }

        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ProductTypeLinkAction()...", category: Category.Info, priority: Priority.Low);

                SelectedPlants = new ConnectorLocation();
                SelectedPlants.ShortName = SelectedPlant.Alias;
                SelectedPlants.Idsite = SelectedPlant.IdCompany;
                SelectedPlants.Location = Location;
                SelectedPlants.Quantity = Withcables;
                SelectedPlants.QuantityWithoutWires = Withoutcables;
                SelectedPlants.IsDamaged = Onlydamaged;
                SelectedPlants.CountryName = SelectedPlant.Country?.Name;
                SelectedPlants.CreatorId = GeosApplication.Instance.ActiveUser.IdUser;
                SelectedPlants.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + SelectedPlant.Country?.Iso + ".png";

                IsSave = true;
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {

                SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                if (SCMShortcuts.Instance.IsActive)
                {
                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
                return error;
            return null;
        }
        #endregion
    }
}
