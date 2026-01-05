using DevExpress.Mvvm;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class NewMilestoneViewModel : NavigationViewModelBase, IDisposable
    {
        protected override void OnParameterChanged(object parameter)
        {
            ProjectMilestoneData = (ProjectMilestone)parameter;
            base.OnParameterChanged(parameter);
        }

        #region Services
        IEpcService epcControl;
        #endregion

        #region ICommands
        public ICommand NewMilestoneAcceptButtonCommand { get; set; }
        public ICommand NewMilestoneCancelButtonCommand { get; set; }
        #endregion

        #region Properties
        public bool ISave { get; set; }

        private ProjectMilestone projectMilestoneData;
        public ProjectMilestone ProjectMilestoneData
        {
            get
            {
                return projectMilestoneData;
            }
            set
            {
                SetProperty(ref projectMilestoneData, value, () => ProjectMilestoneData);
            }
        }

        #endregion

        public NewMilestoneViewModel()
        {
            epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));
            NewMilestoneAcceptButtonCommand = new DelegateCommand<object>(NewMilestoneAccept);
            NewMilestoneCancelButtonCommand = new DelegateCommand<object>(CloseWindow);
        }

        public event EventHandler RequestClose;
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void NewMilestoneAccept(object obj)
        {
            ProjectMilestoneData = epcControl.AddProjectMilestone(ProjectMilestoneData);

            if (ProjectMilestoneData.ProjectMilestoneDates == null)
            {
                ProjectMilestoneData.ProjectMilestoneDates = new System.Collections.ObjectModel.ObservableCollection<ProjectMilestoneDate>();
            }

            ProjectMilestoneData.ProjectMilestoneDates.Add(new ProjectMilestoneDate() { TargetDate = ProjectMilestoneData.TargetDate, IdProjectMilestoneStatus = 92 });

            if (ProjectMilestoneData.IdProjectMilestone > 0)
            {
                ISave = true;
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("NewMilestoneSaved").ToString(), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
            }
            else
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("NewMilestoneNotSaved").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            RequestClose(null, null);
        }

        public void Dispose()
        {
        }

    }
}
