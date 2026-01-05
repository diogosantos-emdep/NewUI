using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
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
    public class NewRequestAssistanceViewModel : NavigationViewModelBase, IDisposable
    {
        protected override void OnParameterChanged(object parameter)
        {
            TaskAssistanceData = (TaskAssistance)parameter;
            base.OnParameterChanged(parameter);
        }

        #region Services
        IEpcService epcControl;
        #endregion

        #region ICommands
        public ICommand NewRequestAssistanceAcceptButtonCommand { get; set; }
        public ICommand NewRequestAssistanceCancelButtonCommand { get; set; }
        #endregion

        #region Properties
        private TaskAssistance taskAssistanceData;
        public TaskAssistance TaskAssistanceData
        {
            get { return taskAssistanceData; }
            set { SetProperty(ref taskAssistanceData, value, () => TaskAssistanceData); }
        }

        private ObservableCollection<User> taskAssistanceRequestUserList;
        public ObservableCollection<User> TaskAssistanceRequestUserList
        {
            get { return taskAssistanceRequestUserList; }
            set { SetProperty(ref taskAssistanceRequestUserList, value, () => TaskAssistanceRequestUserList); }
        }

        private User activeUser;

        public User ActiveUser
        {
            get { return activeUser; }
            set
            {
                SetProperty(ref activeUser, value, () => ActiveUser);
            }
        }


        //private int selectedUserIndex;
        //public int SelectedUserIndex
        //{
        //    get { return selectedUserIndex; }
        //    set { selectedUserIndex = value; }
        //}
        #endregion

        public bool ISave { get; set; }
        public ObservableCollection<LookupValue> TaskAssistanceStatus { get; set; }

        public NewRequestAssistanceViewModel()
        {
            epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));
            NewRequestAssistanceAcceptButtonCommand = new DelegateCommand<object>(NewRequestAssistanceAccept);
            NewRequestAssistanceCancelButtonCommand = new DelegateCommand<object>(CloseWindow);

            TaskAssistanceStatus = new ObservableCollection<LookupValue>(epcControl.GetLookupValues(16).AsEnumerable());
            TaskAssistanceRequestUserList = new ObservableCollection<User>(epcControl.GetUsers().AsEnumerable());
            ActiveUser = GeosApplication.Instance.ActiveUser;
            //SelectedUserIndex = TaskAssistanceRequestUserList.FindIndex(x => x.IdUser == GeosApplication.Instance.ActiveUser.IdUser);
        }

        private void NewRequestAssistanceAccept(object obj)
        {
            TaskAssistanceData = epcControl.AddTaskAssistance(TaskAssistanceData);
            TaskAssistanceData.RequestFrom = TaskAssistanceRequestUserList.FirstOrDefault(x => x.IdUser == TaskAssistanceData.IdRequestFrom);
            TaskAssistanceData.RequestTo = TaskAssistanceRequestUserList.FirstOrDefault(x => x.IdUser == TaskAssistanceData.IdRequestTo);
            TaskAssistanceData.TaskAssistanceStatus = TaskAssistanceStatus.FirstOrDefault(x => x.IdLookupValue == 75);

            if (TaskAssistanceData.IdTaskAssistance != null)
            {
                ISave = true;
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("NewRequestAssistanceSaved").ToString(), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
            }
            else
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("NewRequestAssistanceNotSaved").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
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
