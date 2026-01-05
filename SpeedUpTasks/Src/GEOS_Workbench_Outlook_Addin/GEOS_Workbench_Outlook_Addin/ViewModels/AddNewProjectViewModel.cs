using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AddNewProjectViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Service

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        public bool IsSave { get; set; }
        private string projectName;
        private List<CarOEM> caroemsList;
        private int selectedIndexCarOEM;
        public CarProject NewGeosProject { get; set; }
        private bool isBusy;
        private List<CarProject> projectNameList;
        public List<CarProject> GeosProjectsList = new List<CarProject>();

        private Visibility alertVisibility;
        private List<string> projectNameStrList;
        private string visible;
        #endregion

        #region Properties

        public List<CarProject> ProjectNameList
        {
            get { return projectNameList; }
            set { projectNameList = value; OnPropertyChanged(new PropertyChangedEventArgs("ProjectNameList")); }
        }

        public List<string> ProjectNameStrList
        {
            get { return projectNameStrList; }
            set { projectNameStrList = value; OnPropertyChanged(new PropertyChangedEventArgs("ProjectNameStrList")); }
        }

        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value.TrimStart();
                OnPropertyChanged(new PropertyChangedEventArgs("ProjectName"));
                ShowPopupAsPerProjectName(projectName);
            }

        }

        public int SelectedIndexCarOEM
        {
            get { return selectedIndexCarOEM; }
            set
            {
                selectedIndexCarOEM = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCarOEM"));
            }
        }

        public List<CarOEM> CaroemsList
        {
            get { return caroemsList; }
            set
            {
                caroemsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CaroemsList"));
            }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public Visibility AlertVisibility
        {
            get
            {
                return alertVisibility;
            }

            set
            {
                alertVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AlertVisibility"));
            }
        }
        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
        }
        #endregion

        #region Validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;

            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => ProjectName)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexCarOEM)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string ProjectNameProp = BindableBase.GetPropertyName(() => ProjectName);
                string SelectedIndexCarOEMProp = BindableBase.GetPropertyName(() => SelectedIndexCarOEM);

                if (columnName == ProjectNameProp)
                    return RequiredValidationRule.GetErrorMessage(ProjectNameProp, ProjectName);
                if (columnName == SelectedIndexCarOEMProp)
                    return RequiredValidationRule.GetErrorMessage(SelectedIndexCarOEMProp, SelectedIndexCarOEM);

                return null;
            }
        }

        #endregion

        #region ICommands

        public ICommand AddNewProjectViewCancelButtonCommand { get; set; }
        public ICommand AddNewProjectViewAcceptButtonCommand { get; set; }

        public ICommand CommandTextInput { get; set; }
        #endregion

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

        #region Constructor

        public AddNewProjectViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddNewProjectViewModel ...", category: Category.Info, priority: Priority.Low);

                AddNewProjectViewAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(AddNewProjectAccept);
                AddNewProjectViewCancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                CaroemsList = CrmStartUp.GetCarOEM();
                SelectedIndexCarOEM = -1;
                AlertVisibility = Visibility.Hidden;
                GeosApplication.Instance.IsCarOEMExist = false;

                string error = EnableValidationAndGetError();
                OnPropertyChanged(new PropertyChangedEventArgs("ProjectName"));
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCarOEM"));

                //set hide/show shortcuts on permissions
                Visible = Visibility.Visible.ToString();
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    Visible = Visibility.Hidden.ToString();
                }
                else
                {
                    Visible = Visibility.Visible.ToString();
                }
                GeosApplication.Instance.Logger.Log("Constructor AddNewProjectViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewProjectViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Methods
        /// <summary>
        /// Method for add new project.
        /// </summary>
        /// <param name="obj"></param>
        public void AddNewProjectAccept(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log(" AddNewProjectAccept() Method ...", category: Category.Info, priority: Priority.Low);

                if (!string.IsNullOrEmpty(ProjectName.Trim()))
                {
                    string error = EnableValidationAndGetError();
                    OnPropertyChanged(new PropertyChangedEventArgs("ProjectName"));
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCarOEM"));

                    ProjectName = ProjectName.Trim();
                    if (error != null)
                    {
                        IsBusy = false;
                        return;
                    }

                    //bool isContainOEMname = CaroemsList.Any(cl => ProjectName.ToUpper().Contains(cl.Name.ToUpper()));
                    //if (isContainOEMname)
                    //{
                    //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddNewProjectViewOEMNameError").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //    return;
                    //}

                    bool isExist = CrmStartUp.IsExistCarProject(ProjectName.Trim());

                    if (!isExist)
                    {
                        NewGeosProject = new CarProject() { Name = ProjectName.ToUpper(), IdCarOem = CaroemsList[SelectedIndexCarOEM].IdCarOEM, CreationDate = GeosApplication.Instance.ServerDateTime, CarOEM = CaroemsList[SelectedIndexCarOEM] };
                        IsSave = true;
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddNewProjectViewFailMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }

                    RequestClose(null, null);

                    GeosApplication.Instance.Logger.Log("Method AddNewProjectAccept() executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewProjectAccept() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewProjectAccept() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewProjectAccept() Constructor " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for search similar word.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        double StringSimilarityScore(string name, string searchString)
        {
            if (name.Contains(searchString))
            {
                return (double)searchString.Length / (double)name.Length;
            }

            return 0;
        }
        private void ShowPopupAsPerProjectName(string ProjectName)
        {
            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerProjectName ...", category: Category.Info, priority: Priority.Low);

            ProjectNameList = GeosProjectsList.ToList();

            if (ProjectNameList != null && !string.IsNullOrEmpty(projectName))
            {
                if (projectName.Length > 1)
                {
                    ProjectNameList = ProjectNameList.Where(h => h.Name.ToUpper().Contains(projectName.ToUpper()) || h.Name.ToUpper().StartsWith(projectName.Substring(0, 2).ToUpper())
                                                            || h.Name.ToUpper().EndsWith(projectName.Substring(projectName.Length - 2).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, projectName)).ToList();
                    ProjectNameStrList = ProjectNameList.Select(pn => pn.Name).ToList();
                }
                else
                {
                    ProjectNameList = ProjectNameList.Where(h => h.Name.ToUpper().Contains(projectName.ToUpper()) || h.Name.ToUpper().StartsWith(projectName.Substring(0, 1).ToUpper())
                                                            || h.Name.ToUpper().EndsWith(projectName.Substring(projectName.Length - 1).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, projectName)).ToList();
                    ProjectNameStrList = ProjectNameList.Select(pn => pn.Name).ToList();
                }

                GeosApplication.Instance.IsCarOEMExist = CaroemsList.Any(cl => ProjectName.ToUpper().Contains(cl.Name.ToUpper()));
            }
            else
            {
                ProjectNameList = new List<CarProject>();
                ProjectNameStrList = new List<string>();
                GeosApplication.Instance.IsCarOEMExist = false;
            }

            //For alert Icon visibility
            if (ProjectNameStrList.Count > 0)
            {
                AlertVisibility = Visibility.Visible;

                //If CarOEM name is exist then AlertVisibility is Hidden. 
                if (GeosApplication.Instance.IsCarOEMExist)
                    AlertVisibility = Visibility.Hidden;
            }
            else
                AlertVisibility = Visibility.Hidden;

            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerProjectName() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for close window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            IsSave = false;
            ProjectName = string.Empty;
            RequestClose(null, null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
                {
                    return;
                }
                CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
