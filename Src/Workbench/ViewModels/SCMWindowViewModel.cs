using System;
using DevExpress.Mvvm;
using System.ComponentModel;
using System.Windows.Input;
using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.Common;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Prism.Logging;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Services.Contracts;
using System.Collections.ObjectModel;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;

namespace Workbench.ViewModels
{
    public class SCMWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // ISCMService SCMService = new SCMServiceController("localhost:6699");
        #region Declaration

        private string showHideMenuButtonToolTip;

        private string moduleName;
        private string moduleShortName;
        public string ShowHideMenuButtonToolTip
        {
            get { return showHideMenuButtonToolTip; }
            set
            {
                showHideMenuButtonToolTip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowHideMenuButtonToolTip"));
            }
        }

        public string ModuleName
        {
            get { return moduleName; }
            set
            {
                moduleName = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ModuleName"));
            }
        }

        #endregion

        #region Public Commands

        public ICommand HideTileBarButtonClickCommand { get; set; }
        public ICommand CommandTextInput { get; set; } //[shweta.thube][GEOS2-6630][04.04.2025]
        ObservableCollection<LookUpValues> connectorTypeList;//[shweta.thube][GEOS2-6630][04.04.2025]
        public ObservableCollection<LookUpValues> ConnectorTypeList
        {
            get
            {
                return connectorTypeList;
            }
            set
            {
                connectorTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorTypeList"));
            }
        }
        #endregion  // Public Commands

        public string ModuleShortName
        {
            get
            {
                return moduleShortName;
            }

            set
            {
               moduleShortName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleShortName"));
            }
        }

        #region Constructor

        public SCMWindowViewModel()
        {
            HideTileBarButtonClickCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(HideTileBarButtonClickCommandAction);
            ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("HideMenuButtonToolTip").ToString();  //Hide menu
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction); //[shweta.thube][GEOS2-6630][04.04.2025]
            if (GeosApplication.Instance.ObjectPool.ContainsKey("GeosModuleNameList"))
            {
                ModuleShortName = ((List<GeosModule>)GeosApplication.Instance.ObjectPool["GeosModuleNameList"]).Where(s => s.IdGeosModule == 1).Select(s => s.Acronym).FirstOrDefault();
                ModuleName = ((List<GeosModule>)GeosApplication.Instance.ObjectPool["GeosModuleNameList"]).Where(s => s.IdGeosModule == 1).Select(s => s.Name).FirstOrDefault();
            }
            else
            {
                ModuleShortName = "SCM";
                ModuleName = "Samples Catalogue Management";
            }
            GetConnectorType();//[shweta.thube][GEOS2-6630][04.04.2025]
        }

        #endregion

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Methods

        private void HideTileBarButtonClickCommandAction(RoutedEventArgs obj)
        {
            if (GeosApplication.Instance.TileBarVisibility == Visibility.Collapsed)
            {
                GeosApplication.Instance.TileBarVisibility = Visibility.Visible;
                ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("HideMenuButtonToolTip").ToString(); //Hide menu
            }
            else
            {
                GeosApplication.Instance.TileBarVisibility = Visibility.Collapsed;
                ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("ShowMenuButtonToolTip").ToString(); // ShowMenu
            }
        }

        public void Dispose()
        {
        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);


                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        void GetConnectorType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Started GetConnectorType()....", Category.Info, priority: Priority.Low);
                ConnectorTypeList = new ObservableCollection<LookUpValues>(SCMService.GetAllLookUpValuesRecordByIDLookupkey(145));
                SCMCommon.Instance.SelectedTypeList = ConnectorTypeList.Where(i => i.IdLookupValue == 1900).ToList();
                GeosApplication.Instance.Logger.Log("Ended GetConnectorType()....", Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetConnectorType() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetConnectorType() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetConnectorType()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion // Methods
    }
}