using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using DevExpress.Xpf.Editors;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class NewProjectViewModel : NavigationViewModelBase, IDisposable
    {
        protected override void OnParameterChanged(object parameter)
        {
            ProjectData = (Project)parameter;
            base.OnParameterChanged(parameter);
        }

        #region Services

        IEpcService epcControl;

        #endregion

        #region ICommands

        public ICommand NewProjectAcceptButtonCommand { get; set; }
        public ICommand NewProjectCancelButtonCommand { get; set; }
        public ICommand SelectedTeamIndexCommand { get; set; }

        #endregion

        #region Collections

        public ObservableCollection<Customer> ProjectCustomerList { get; set; }
        public ObservableCollection<LookupValue> ProjectPriorityList { get; set; }
        public ObservableCollection<LookupValue> ProjectStatusList { get; set; }
        public ObservableCollection<LookupValue> ProjectCategoryList { get; set; }
        public ObservableCollection<Team> ProjectTeamList { get; set; }
        public ObservableCollection<LookupValue> ProjectTypeList { get; set; }

        private ObservableCollection<User> projectOwnerList;
        public ObservableCollection<User> ProjectOwnerList
        {
            get { return projectOwnerList; }
            set
            {
                SetProperty(ref projectOwnerList, value, () => ProjectOwnerList);
            }
        }

        public ICollectionView GroupTeam { get; set; }

        private Product productData;
        public Product ProductData
        {
            get { return productData; }
            set
            {
                SetProperty(ref productData, value, () => ProductData);
            }
        }

        private Project projectData;
        public Project ProjectData
        {
            get { return projectData; }
            set
            {
                SetProperty(ref projectData, value, () => ProjectData);
            }
        }

        List<object> selectedTeam;
        public List<object> SelectedTeam
        {
            get { return selectedTeam; }
            set
            {
                SetProperty(ref selectedTeam, value, () => SelectedTeam);
            }
        }

        private int selectUserIndex;
        public int SelectUserIndex
        {
            get { return selectUserIndex; }
            set
            {
                SetProperty(ref selectUserIndex, value, () => SelectUserIndex);
            }
        }
        
        public bool ISave { get; set; }
        #endregion

        #region Constructor
        public NewProjectViewModel()
        {
            try
            {
                epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));

                if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_PROJECTSTATUS"))
                {
                    ProjectStatusList = (ObservableCollection<LookupValue>)GeosApplication.Instance.ObjectPool["EPC_PROJECTSTATUS"];
                }
                else
                {
                    ProjectStatusList = new ObservableCollection<LookupValue>(epcControl.GetLookupValues(2).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("EPC_PROJECTSTATUS", ProjectStatusList);
                }

                if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_PROJECTCATEGORIES"))
                {
                    ProjectCategoryList = (ObservableCollection<LookupValue>)GeosApplication.Instance.ObjectPool["EPC_PROJECTCATEGORIES"];
                }
                else
                {
                    ProjectCategoryList = new ObservableCollection<LookupValue>(epcControl.GetLookupValues(3).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("EPC_PROJECTCATEGORIES", ProjectCategoryList);
                }

                if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_PROJECTPRIORITY"))
                {
                    ProjectPriorityList = (ObservableCollection<LookupValue>)GeosApplication.Instance.ObjectPool["EPC_PROJECTPRIORITY"];
                }
                else
                {
                    ProjectPriorityList = new ObservableCollection<LookupValue>(epcControl.GetLookupValues(1).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("EPC_PROJECTPRIORITY", ProjectPriorityList);
                }

                ProjectData = new Data.Common.Epc.Project();
                ProjectCustomerList = new ObservableCollection<Customer>(epcControl.GetCustomers().AsEnumerable());
                ProjectTypeList = new ObservableCollection<LookupValue>(epcControl.GetLookupValues(18).AsEnumerable());

                List<Team> list = epcControl.GetTeams().ToList();
                list.RemoveAll(x => x.IdParent == null);
                ProjectTeamList = new ObservableCollection<Team>(list);
                GroupTeam = CollectionViewSource.GetDefaultView(ProjectTeamList);
                GroupTeam.GroupDescriptions.Add(new PropertyGroupDescription("ParentTeam.Name"));

                //ProjectOwnerList = new ObservableCollection<User>(epcControl.GetUsers().AsEnumerable());

                SelectedTeam = new List<object>();
                foreach (var item in GroupTeam)
                {
                    SelectedTeam.Add(item);
                }

                //List<Team> newTeams = SelectedTeam.Cast<Team>().ToList();
                ProjectOwnerList = new ObservableCollection<User>(epcControl.GetUserByTeamId(SelectedTeam.Cast<Team>().ToList()).AsEnumerable());

                NewProjectCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                NewProjectAcceptButtonCommand = new RelayCommand(new Action<object>(SaveNewProject));

                SelectedTeamIndexCommand = new RelayCommand(new Action<object>(SelectedTeamIndexAction));
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
            }
        }

        #endregion

        #region Methods

        public event EventHandler RequestClose;
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void SaveNewProject(object probj)
        {
            ProjectData = epcControl.AddProject(ProjectData, (SelectedTeam.Cast<Team>().ToList()).Select(i =>i.IdTeam).ToList());

            var v = ProjectCustomerList.FirstOrDefault(x => x.IdCustomer == ProjectData.IdCustomer);
            if (v != null)
            {
                ProjectData.Customer = v;
            }

            if (ProjectData.ProjectName != null)
            {
                ISave = true;
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("NewProjectSaved").ToString(), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
            }
            else
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("NewProjectNotSaved").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            RequestClose(null, null);
        }

        public void SelectedTeamIndexAction(object e)
        {
            var obj = (EditValueChangingEventArgs)e;
            SelectedTeam= (List<object>)obj.NewValue;

            if (SelectedTeam == null)
            {
                ProjectOwnerList = null;
                return;
            }

            List<Team> newTeams = SelectedTeam.Cast<Team>().ToList();
            ProjectOwnerList = new ObservableCollection<User>(epcControl.GetUserByTeamId(newTeams).AsEnumerable());
        }

        public void Dispose()
        {
        }

        #endregion
    }
}
