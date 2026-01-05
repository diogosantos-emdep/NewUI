using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    class NewFollowupViewModel : NavigationViewModelBase, IDisposable
    {
        protected override void OnParameterChanged(object parameter)
        {
            ProjectFollowupData = (ProjectFollowup)parameter;
            base.OnParameterChanged(parameter);
        }

        #region Services
        IEpcService epcControl;
        #endregion

        #region ICommands
        public ICommand NewFollowupAcceptButtonCommand { get; set; }
        public ICommand NewFollowupCancelButtonCommand { get; set; }
        #endregion

        #region Properties

        public bool ISave { get; set; }

        private ProjectFollowup projectFollowupData;
        public ProjectFollowup ProjectFollowupData
        {
            get { return projectFollowupData; }
            set
            {
                SetProperty(ref projectFollowupData, value, () => ProjectFollowupData);
            }
        }

        #endregion

        #region Constructor
        public NewFollowupViewModel()
        {
            epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));

            NewFollowupAcceptButtonCommand = new DelegateCommand<object>(NewFollowupAccept);
            NewFollowupCancelButtonCommand = new DelegateCommand<object>(CloseWindow);
        }
        #endregion

        private void NewFollowupAccept(object obj)
        {
            ProjectFollowupData = epcControl.AddProjectFollowup(ProjectFollowupData);
            ProjectFollowupData.User = GeosApplication.Instance.ActiveUser;

            if (ProjectFollowupData.IdProjectFollowup > 0)
            {
                ISave = true;
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("NewFollowupSaved").ToString(), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
            }
            else
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("NewFollowupNotSaved").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }

            RequestClose(null, null);
        }

        public event EventHandler RequestClose;
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void Dispose()
        {
        }
    }
}
