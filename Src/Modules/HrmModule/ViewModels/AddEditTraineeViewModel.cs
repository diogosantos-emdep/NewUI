using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddEditTraineeViewModel : ViewModelBase, INotifyPropertyChanged
    {

        #region Service
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public Events
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


        #region Declaration
        private string windowHeader;
        private ObservableCollection<Employee> allTraineesList;
        private bool isNew;
        private ObservableCollection<Employee> traineesListForMainGrid;
        private ObservableCollection<Employee> existingTraineesList;
        private bool isSave;
        private Visibility isVisible;
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

        public ObservableCollection<Employee> AllTraineesList
        {
            get { return allTraineesList; }
            set
            {
                allTraineesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllTraineesList"));
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

        public ObservableCollection<Employee> TraineesListForMainGrid
        {
            get
            {
                return traineesListForMainGrid;
            }

            set
            {
                traineesListForMainGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TraineesListForMainGrid"));
            }
        }


        public bool IsSave
        {
            get
            {
                return isSave;
            }

            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        
        #endregion

        #region Icommands
        public ICommand CancelButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand AddEditTraineeAcceptButtonCommand { get; set; }

        public ICommand CommandShowFilterPopupForTranieeClick { get; set; }
        
        #endregion

        #region Constructor
        public AddEditTraineeViewModel()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Constructor AddEditTraineeViewModel ...", category: Category.Info, priority: Priority.Low);

                AddEditTraineeAcceptButtonCommand = new DelegateCommand<object>(AddEditTraineeAcceptButtonCommandAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                CommandShowFilterPopupForTranieeClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupForTrainee);
                GeosApplication.Instance.Logger.Log("Constructor AddEditTraineeViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddEditTraineeViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                FillAllTraineesList();
                //ExistingTraineesList = new ObservableCollection<Employee>();
                //ExistingTraineesList = EmployeeDetails.EmployeeShifts.Select(x => (EmployeeShift)x.Clone()).ToList();
                //RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// [001][cpatil][28-03-2022][GEOS2-3567]HRM - Allow add future Job descriptions [#ERF97] - 6
        private void CustomShowFilterPopupForTrainee(FilterPopupEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopupForTrainee ...", category: Category.Info, priority: Priority.Low);

               if (e.Column.FieldName == "EmployeeDepartments")
                {
                    if (e.Column.FieldName != "EmployeeDepartments")
                    {
                        return;
                    }

                    try
                    {
                        List<object> filterItems = new List<object>();

                        if (e.Column.FieldName == "EmployeeDepartments")
                        {
                            filterItems.Add(new CustomComboBoxItem()
                            {
                                DisplayValue = "(Blanks)",
                                EditValue = CriteriaOperator.Parse("IsNull([EmployeeDepartments])")//[002] added
                            });

                            filterItems.Add(new CustomComboBoxItem()
                            {
                                DisplayValue = "(Non blanks)",
                                EditValue = CriteriaOperator.Parse("!IsNull([EmployeeDepartments])")
                            });

                            foreach (var dataObject in AllTraineesList)
                            {
                                if (dataObject.EmployeeDepartments == null)
                                {
                                    continue;
                                }
                                else if (dataObject.EmployeeDepartments != null)
                                {
                                    if (dataObject.EmployeeDepartments.Contains("\n"))
                                    {
                                        string tempDepartments = dataObject.EmployeeDepartments;
                                        for (int index = 0; index < tempDepartments.Length; index++)
                                        {
                                            string empDepartments = tempDepartments.Split('\n').First();

                                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empDepartments))
                                            {
                                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                                customComboBoxItem.DisplayValue = empDepartments;
                                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDepartments Like '%{0}%'", empDepartments));
                                                filterItems.Add(customComboBoxItem);
                                            }
                                            if (tempDepartments.Contains("\n"))
                                                tempDepartments = tempDepartments.Remove(0, empDepartments.Length + 1);
                                            else
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == AllTraineesList.Where(y => y.EmployeeDepartments == dataObject.EmployeeDepartments).Select(slt => slt.EmployeeDepartments).FirstOrDefault().Trim()))
                                        {
                                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                            customComboBoxItem.DisplayValue = AllTraineesList.Where(y => y.EmployeeDepartments == dataObject.EmployeeDepartments).Select(slt => slt.EmployeeDepartments).FirstOrDefault().Trim();
                                            customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeDepartments Like '%{0}%'", AllTraineesList.Where(y => y.EmployeeDepartments == dataObject.EmployeeDepartments).Select(slt => slt.EmployeeDepartments).FirstOrDefault().Trim()));
                                            filterItems.Add(customComboBoxItem);
                                        }
                                    }
                                }
                            }
                        }
                        e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
                        GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopupForTrainee() executed successfully", category: Category.Info, priority: Priority.Low);

                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupForTrainee() method " + ex.Message, category: Category.Info, priority: Priority.Low);
                    }
                }

                       else if (e.Column.FieldName == "Organization")
                        {
                    List<object> filterItems = new List<object>();
                    try
                    {
                        filterItems.Add(new CustomComboBoxItem()
                        {
                            DisplayValue = "(Blanks)",
                            EditValue = CriteriaOperator.Parse("IsNull([Organization])")//[002] added
                        });

                        filterItems.Add(new CustomComboBoxItem()
                        {
                            DisplayValue = "(Non blanks)",
                            EditValue = CriteriaOperator.Parse("!IsNull([Organization])")
                        });

                        foreach (var dataObject in AllTraineesList)
                        {
                            if (dataObject.Organization == null)
                            {
                                continue;
                            }
                            else if (dataObject.Organization != null)
                            {
                                if (dataObject.Organization.Contains("\n"))
                                {
                                    string tempOrganization = dataObject.Organization;
                                    for (int index = 0; index < tempOrganization.Length; index++)
                                    {
                                        string empOrganization = tempOrganization.Split('\n').First();

                                        if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empOrganization))
                                        {
                                            CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                            customComboBoxItem.DisplayValue = empOrganization;
                                            customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Organization Like '%{0}%'", empOrganization));
                                            filterItems.Add(customComboBoxItem);
                                        }
                                        if (tempOrganization.Contains("\n"))
                                            tempOrganization = tempOrganization.Remove(0, empOrganization.Length + 1);
                                        else
                                            break;
                                    }
                                }
                                else
                                {
                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == AllTraineesList.Where(y => y.Organization == dataObject.Organization).Select(slt => slt.Organization).FirstOrDefault().Trim()))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = AllTraineesList.Where(y => y.Organization == dataObject.Organization).Select(slt => slt.Organization).FirstOrDefault().Trim();
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Organization Like '%{0}%'", AllTraineesList.Where(y => y.Organization == dataObject.Organization).Select(slt => slt.Organization).FirstOrDefault().Trim()));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                }
                            }
                        }

                        e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
                        GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopupForTrainee() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupForTrainee() method " + ex.Message, category: Category.Info, priority: Priority.Low);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopupForTrainee() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method CustomShowFilterPopupForTrainee()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// [001][cpatil][23-03-2022][GEOS2-3567]HRM - Allow add future Job descriptions [#ERF97] - 6
        private void FillAllTraineesList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAllTraineesList()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    //[001]
                    AllTraineesList = new ObservableCollection<Employee>(HrmService.GetAllEmployeesWithoutInactiveStatus_V2250(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillAllTraineesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAllTraineesList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAllTraineesList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                //CustomMessageBox.Show(string.Format("Could not find file '{0}'.", employeeEducationQualification.QualificationFileName), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillAllTraineesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// CloseWindow Method is used for Cancel Button for Both Add and Edit Trainee
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
               // IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void AddEditTraineeAcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEditTraineeAcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                TraineesListForMainGrid = new ObservableCollection<Employee>();
                foreach (Employee temp in AllTraineesList.Where(x => x.IsChecked).ToList())
                {
                    TraineesListForMainGrid.Add(temp);
                }
                IsSave = true;
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddEditTraineeAcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddEditTraineeAcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
