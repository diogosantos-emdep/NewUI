using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Epc.Common.EPC;
using Emdep.Geos.Modules.Epc.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class ProductViewModel : NavigationViewModelBase, IDisposable
    {
        #region Services

        static ProductView productView;
        INavigationService Service { get { return ServiceContainer.GetService<INavigationService>(); } }
        IEpcService epcControl;

        #endregion

        #region ViewModels

        public NewProjectViewModel NewProjectViewModel { get; private set; }
        public NewRoadMapViewModel NewRoadMapViewModel { get; private set; }

        #endregion

        #region ICommands

        public ICommand NewRoadMapAndProjectButtonClickCommand { get; set; }
        public ICommand ProductViewTabSelectionChangedCommand { get; set; }
        public ICommand ProjectViewLoadCommand { get; set; }
        public ICommand RoadmapAddNodeCommand { get; set; }
        public ICommand SelectedProjectCommand { get; set; }

        #endregion

        #region Collections

        private ObservableCollection<Project> projectList = new ObservableCollection<Project>();
        public ObservableCollection<Project> ProjectList
        {
            get { return projectList; }
            set
            {
                SetProperty(ref projectList, value, () => ProjectList);
            }
        }

        private ObservableCollection<ProductRoadmap> roadMapList = new ObservableCollection<ProductRoadmap>();
        public ObservableCollection<ProductRoadmap> RoadMapList
        {
            get { return roadMapList; }
            set
            {
                SetProperty(ref roadMapList, value, () => RoadMapList);
            }
        }

        private Product productData = new Product();
        public Product ProductData
        {
            get { return productData; }
            set
            {
                SetProperty(ref productData, value, () => ProductData);
            }
        }

        private ObservableCollection<Emdep.Geos.Data.Common.Epc.LookupValue> roadmapSourceList = new ObservableCollection<Data.Common.Epc.LookupValue>();
        public ObservableCollection<Emdep.Geos.Data.Common.Epc.LookupValue> RoadmapSourceList
        {
            get { return roadmapSourceList; }
            set
            {
                SetProperty(ref roadmapSourceList, value, () => RoadmapSourceList);
            }
        }

        private ObservableCollection<Emdep.Geos.Data.Common.Epc.LookupValue> roadmapStatusList = new ObservableCollection<Data.Common.Epc.LookupValue>();
        public ObservableCollection<Emdep.Geos.Data.Common.Epc.LookupValue> RoadmapStatusList
        {
            get { return roadmapStatusList; }
            set
            {
                SetProperty(ref roadmapStatusList, value, () => RoadmapStatusList);
            }
        }

        #endregion

        #region Properties

        private object pageAdornerVisibility;
        public object PageAdornerVisibility
        {
            get { return pageAdornerVisibility; }
            set
            {
                SetProperty(ref pageAdornerVisibility, value, () => PageAdornerVisibility);
            }
        }

        private int isSelected;
        public int IsSelected
        {
            get { return isSelected; }
            set
            {
                SetProperty(ref isSelected, value, () => IsSelected);
            }
        }

        private ProjectViewModel projectViewModel;
        public ProjectViewModel ProjectViewModel
        {
            get { return projectViewModel; }
            set
            {
                SetProperty(ref projectViewModel, value, () => ProjectViewModel);
            }
        }

        Project activeProject;
        public Project ActiveProject
        {
            get { return activeProject; }
            set
            {
                SetProperty(ref activeProject, value, () => ActiveProject);
            }
        }


        private Project selectedProjet;
        public Project SelectedProjet
        {
            get { return selectedProjet; }
            set
            {
                selectedProjet = value; NotifyPropertyChanged("SelectedProjet");
            }
        }

        private ObservableCollection<ProductVersion> productVersionsList = new ObservableCollection<ProductVersion>();
        public ObservableCollection<ProductVersion> ProductVersionsList
        {
            get { return productVersionsList; }
            set
            {
                SetProperty(ref productVersionsList, value, () => ProductVersionsList);
            }
        }

        private ObservableCollection<DataHelper> listDataHelper = new ObservableCollection<DataHelper>();
        public ObservableCollection<DataHelper> ListDataHelper
        {
            get { return listDataHelper; }
            set
            {
                SetProperty(ref listDataHelper, value, () => ListDataHelper);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Constructor
        public ProductViewModel()
        {
            try
            {
                IsSelected = 0;
                epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));
                if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_ROADMAPSTATUS"))
                {
                    RoadmapStatusList = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["EPC_ROADMAPSTATUS"];
                }
                else
                {
                    RoadmapStatusList = new ObservableCollection<Data.Common.Epc.LookupValue>(epcControl.GetLookupValues(8).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("EPC_ROADMAPSTATUS", RoadmapStatusList);
                }

                RoadmapSourceList = new ObservableCollection<Data.Common.Epc.LookupValue>(epcControl.GetLookupValues(7).AsEnumerable());
                NewRoadMapAndProjectButtonClickCommand = new RelayCommand(new Action<object>(CreateNewRoadMapAndProject));
                SelectedProjectCommand = new RelayCommand(SelectedProject);
                ProjectViewLoadCommand = new DelegateCommand<object>(projectViewLoadCommandAction);
                RoadmapAddNodeCommand = new DelegateCommand<object>(RoadmapAddNodeCommandAction);
                ProductViewTabSelectionChangedCommand = new Prism.Commands.DelegateCommand<TabControlSelectionChangedEventArgs>(TabSelectionChanged);
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Navigation Methods
        protected override void OnNavigatedTo()
        {
            try
            {
                base.OnNavigatedTo();

                if (this.Parameter is Product)
                {
                    PageAdornerVisibility = Visibility.Hidden;
                    ProductData = (Product)this.Parameter;
                    Init();
                }
                else if (this.Parameter is Project)
                {
                    PageAdornerVisibility = Visibility.Visible;

                    Project project = (Project)this.Parameter;
                    ProductData = epcControl.GetProductByProductId(project.IdProduct);
                    Init();
                    productView.tabControlProduct.SelectedIndex = 1;
                    int index = ProjectList.IndexOf(ProjectList.Where(x => x.IdProject == project.IdProject).FirstOrDefault());
                    productView.tabProject.Focus();
                    productView.projectsGroupBox.Focus();
                    productView.projectsListBoxEdit.Focus();
                    productView.projectsListBoxEdit.SelectedIndex = index;
                }
            }
            catch
            { }
        }

        protected override void OnNavigatedFrom()
        {
            try
            {
                base.OnNavigatedFrom();
            }
            catch
            { }
        }

        #endregion

        #region Methods
        void Init()
        {
            var projectList = epcControl.GetProjectsByProductId(this.ProductData.IdProduct);
            if (projectList != null)
            {
                ProjectList.AddRange(projectList);
            }

            var roadMapsList = new ObservableCollection<ProductRoadmap>(epcControl.GetProductRoadmapByProductId(Convert.ToInt32(this.ProductData.IdProduct)).AsEnumerable());
            RoadMapList.Add(new ProductRoadmap() { IdProductRoadmap = 23, Title = "Proposals", RoadmapImageIcon = GetImage("/Emdep.Geos.Modules.Epc;component/Images/Common/RoadmapTypes/Proposal.png") });
            RoadMapList.Add(new ProductRoadmap() { IdProductRoadmap = 24, Title = "Bugs", RoadmapImageIcon = GetImage("/Emdep.Geos.Modules.Epc;component/Images/Common/RoadmapTypes/Bug.png") });
            RoadMapList.Add(new ProductRoadmap() { IdProductRoadmap = 66, Title = "Working Order", RoadmapImageIcon = GetImage("/Emdep.Geos.Modules.Epc;component/Images/Common/RoadmapTypes/PO.png") });

            RoadMapList.AddRange(roadMapsList);

            var versionlist = new ObservableCollection<ProductVersion>(epcControl.GetProductVersionByProductId(this.ProductData.IdProduct));

            ProductVersion p;

            foreach (var item in versionlist)
            {
                DataHelper help = new DataHelper();
                help.StringValue1 = "V" + item.IdProductVersion.ToString();
                help.DateTime1 = item.VersionBetaDate;
                help.DateTime2 = item.VersionReleaseDate;
                help.StringValue3 = item.ProductVersionNumber;
                ListDataHelper.Add(help);

                foreach (var item2 in item.ProductVersionItems)
                {
                    DataHelper help2 = new DataHelper();
                    help2.StringValue1 = "I" + item2.IdProductRoadmap.ToString();
                    help2.StringValue2 = help.StringValue1;
                    if (item2.ProductRoadmap != null)
                        help2.StringValue3 = item2.ProductRoadmap.Title;
                    ListDataHelper.Add(help2);
                }
            }
        }

        public void SelectedProject(object o)
        {
            if (ProjectList.Count > 0)
            {
                if (ProjectViewModel == null)
                    this.ProjectViewModel = ViewModelSource.Create(() => new ProjectViewModel());
                else
                    this.ProjectViewModel = ViewModelSource.Create(() => new ProjectViewModel());

                this.ProjectViewModel.ProjectList = this.ProjectList;
                this.ProjectViewModel.ProjectData = ActiveProject;
                this.ProjectViewModel.Init();
            }
        }

        public void projectViewLoadCommandAction(object e)
        {
            productView = (ProductView)e;
        }

        public void RoadmapAddNodeCommandAction(object obj)
        {
            int idLookUpValue = Convert.ToInt32(((Emdep.Geos.Data.Common.Epc.ProductRoadmap)(((DevExpress.Xpf.Grid.TreeList.TreeListRowData)(obj)).Node.Content)).IdProductRoadmap);

            NewRoadMapView newRoadMapView = new NewRoadMapView();
            NewRoadMapViewModel newRoadMapViewModel = new NewRoadMapViewModel(idLookUpValue);
            ProductRoadmap projectRoadMap = new ProductRoadmap() { IdProduct = this.ProductData.IdProduct };
            ((ISupportParameter)newRoadMapViewModel).Parameter = projectRoadMap;
            EventHandler handle = delegate { newRoadMapView.Close(); };
            newRoadMapViewModel.RequestClose += handle;
            newRoadMapView.DataContext = newRoadMapViewModel;
            newRoadMapView.ShowDialogWindow();

            if (newRoadMapViewModel.ISave)
            {
                newRoadMapViewModel.RoadMapData.IdRoadmapStatus = RoadmapStatusList[2].IdLookupValue;
                RoadMapList.Add(newRoadMapViewModel.RoadMapData);
            }
        }

        /// <summary>
        /// Method for Creating New Project or New RoadMap
        /// </summary>
        /// <param name="ICommand object"></param>
        public void CreateNewRoadMapAndProject(object obj)
        {
            if (IsSelected == 0)
            {
                //NewRoadMapView newRoadMapView = new NewRoadMapView();
                //NewRoadMapViewModel newRoadMapViewModel = new NewRoadMapViewModel();
                //ProductRoadmap projectRoadMap = new ProductRoadmap() { IdProduct = this.ProductData.IdProduct };
                //((ISupportParameter)newRoadMapViewModel).Parameter = projectRoadMap;
                //EventHandler handle = delegate { newRoadMapView.Close(); };
                //newRoadMapViewModel.RequestClose += handle;
                //newRoadMapView.DataContext = newRoadMapViewModel;
                //newRoadMapView.ShowDialogWindow();
                //if (newRoadMapViewModel.ISave)
                //{
                //    newRoadMapViewModel.RoadMapData.IdRoadmapStatus = RoadmapStatusList[2].IdLookupValue;
                //    RoadMapList.Add(newRoadMapViewModel.RoadMapData);
                //}
            }
            else if (IsSelected == 1)
            {
                NewProjectView newProjectView = new NewProjectView();
                NewProjectViewModel newProjectViewModel = new NewProjectViewModel();
                Project project = new Project()
                {
                    IdProduct = this.ProductData.IdProduct,
                    IdCreator = GeosApplication.Instance.ActiveUser.IdUser,
                    CreationDate = DateTime.Now
                };
                ((ISupportParameter)newProjectViewModel).Parameter = project;
                EventHandler handle = delegate { newProjectView.Close(); };
                newProjectViewModel.RequestClose += handle;
                newProjectView.DataContext = newProjectViewModel;
                newProjectView.ShowDialogWindow();

                if (newProjectViewModel.ISave)
                {
                    ProjectList.Add(newProjectViewModel.ProjectData);
                    ActiveProject = newProjectViewModel.ProjectData;
                }
            }
            else if (IsSelected == 2)
            {
            }
        }

        public void TabSelectionChanged(TabControlSelectionChangedEventArgs obj)
        {
            if (obj.NewSelectedIndex == 0)
            {
                IsSelected = 0;
            }
            if (obj.NewSelectedIndex == 1)
            {
                IsSelected = 1;
            }
            if (obj.NewSelectedIndex == 2)
            {
                IsSelected = 2;
            }
        }

        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        public void Dispose()
        {
        }

        #endregion
    }
}
