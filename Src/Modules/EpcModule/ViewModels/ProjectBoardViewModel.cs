using DevExpress.Mvvm;
using DevExpress.Xpf.Grid.DragDrop;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class ProjectBoardViewModel : ViewModelBase, IDisposable
    {
        #region Services
        IEpcService epcControl;
        #endregion
        
        #region ICommands 
        public ICommand UpdateProjectStatusCommand { get; set; }

        #endregion
       
        #region Collections
        public ObservableCollection<Project> AllOpenProjectList { get; set; }
        public ObservableCollection<Project> UnderStudyProjectList { get; set; }
        public ObservableCollection<Project> AnalysisReadyProjectList { get; set; }
        public ObservableCollection<Project> DevelopmentProjectList { get; set; }
        public ObservableCollection<Project> DevelopmentOnHoldProjectList { get; set; }
        public ObservableCollection<Project> validationProjectList { get; set; }
        public ObservableCollection<Project> DeliveredProjectList { get; set; }

        #endregion

        #region Properties

        Project projectData;
        public Project ProjectData
        {
            get
            {
                return projectData;
            }

            set
            {
                SetProperty(ref projectData, value, () => ProjectData);

            }
        }

        #endregion

        #region Constructor
        public ProjectBoardViewModel()
        {

            try
            {
                AllOpenProjectList = new ObservableCollection<Project>();
                UnderStudyProjectList = new ObservableCollection<Project>();
                AnalysisReadyProjectList = new ObservableCollection<Project>();
                DevelopmentProjectList = new ObservableCollection<Project>();
                DevelopmentOnHoldProjectList = new ObservableCollection<Project>();
                validationProjectList = new ObservableCollection<Project>();
                DeliveredProjectList = new ObservableCollection<Project>();

                epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));

                AllOpenProjectList = new ObservableCollection<Project>(epcControl.GetOpenProjectOnBoard().AsEnumerable());

                foreach (var item in AllOpenProjectList)
                {
                    if (item.IdProjectStatus == 6)
                        UnderStudyProjectList.Add(item);
                    if (item.IdProjectStatus == 7)
                        AnalysisReadyProjectList.Add(item);
                    if (item.IdProjectStatus == 8)
                        DevelopmentProjectList.Add(item);
                    if (item.IdProjectStatus == 9)
                        DevelopmentOnHoldProjectList.Add(item);
                    if (item.IdProjectStatus == 10)
                        validationProjectList.Add(item);
                    if (item.IdProjectStatus == 11)
                        DeliveredProjectList.Add(item);                  

                }

                UpdateProjectStatusCommand = new Prism.Commands.DelegateCommand<ListBoxDropEventArgs>(UpdateProjectBoard);

            }
            catch (Exception ex)
            {


            }

        }
        #endregion

        #region Methods
        /// <summary>
        /// Method For Update Project Status On Drag and Drop
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateProjectBoard(ListBoxDropEventArgs obj)
        {
            int objStatus = Convert.ToInt32(obj.ListBoxEdit.Tag);

            foreach (Project item in obj.DraggedRows)
            {
                if (item is Project)
                {
                    epcControl.UpdateProjectStatusById(item, objStatus);

                }

            }

            obj.Handled = false;

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
