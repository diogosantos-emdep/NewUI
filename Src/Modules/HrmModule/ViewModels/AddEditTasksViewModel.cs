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
using Emdep.Geos.UI.Validations;
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
    public class AddEditTasksViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo   ///NavigationViewModelBase
    {
        #region services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        private bool isSave;
        private bool isNew;
        private bool isBusy;
        private string tasksCode;
        private string description;
        private string jobrequirement;
        private ProfessionalObjective tasksObjective;
        private string tasksSkill;
        private bool tasksInUse;
        private int selectedAccountSkillsLinkedItemsCount;
        private ObservableCollection<ProfessionalSkill> skillsList;
        private double dialogHeight;
        private double dialogWidth;
        private Int32 columnWidth;
        private int windowHeight;
        private ulong idProfessionalTask;
        private string error = string.Empty;
        //private int screenWidth;
        //private int screenHeight;
        private ProfessionalObjective selectedObjective;
        private ObservableCollection<ProfessionalSkill> selectedSkillsLinkedItems;
        private ObservableCollection<ProfessionalSkill> clonedSkillsList;
        private ObservableCollection<ProfessionalSkill> deletedSkillsList;

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
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }

            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public string TasksCode
        {
            get
            {
                return tasksCode;
            }
            set
            {
                tasksCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TasksCode"));
            }
        }
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }
        public string TasksJobRequirement
        {
            get
            {
                return jobrequirement;
            }
            set
            {
                jobrequirement = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Jobrequirement"));
            }
        }

        public bool TasksInUse
        {
            get
            {
                return tasksInUse;
            }

            set
            {
                tasksInUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TasksInUse"));

            }
        }
        public ObservableCollection<ProfessionalSkill> SkillsList
        {
            get
            {
                return skillsList;
            }

            set
            {
                skillsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillsList"));
            }
        }

        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }

        public ulong IdProfessionalTask
        {
            get
            {
                return idProfessionalTask;
            }

            set
            {
                idProfessionalTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdProfessionalTask"));
            }
        }

        public int SelectedAccountSkillsLinkedItemsCount
        {
            get { return selectedAccountSkillsLinkedItemsCount; }
            set
            {
                selectedAccountSkillsLinkedItemsCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAccountSkillsLinkedItemsCount"));
            }
        }

        public ProfessionalTask NewProfessionalTask { get; set; }
        public ProfessionalTask EditProfessionalTask { get; set; }

        public ProfessionalObjective SelectedObjective
        {
            get
            {
                return selectedObjective;
            }
            set
            {
                selectedObjective = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObjective"));
            }
        }
        public ObservableCollection<ProfessionalSkill> SelectedSkillsLinkedItems
        {
            get { return selectedSkillsLinkedItems; }
            set
            {
                selectedSkillsLinkedItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSkillsLinkedItems"));
            }
        }

        public ObservableCollection<ProfessionalSkill> ClonedSkillsList
        {
            get { return clonedSkillsList; }
            set
            {
                clonedSkillsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedSkillsList"));
            }
        }

        public ObservableCollection<ProfessionalSkill> DeletedSkillsList
        {
            get { return deletedSkillsList; }
            set
            {
                deletedSkillsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeletedSkillsList"));
            }
        }

        public List<ProfessionalObjective> ObjectivesTypeList { get; set; }

        #endregion

        #region ICommands
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand AcceptFileActionCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand LinkedItemCancelCommand { get; set; }
        public ICommand SkillsRowMouseDoubleClickCommand { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for AddEditTasks
        /// </summary>
        public AddEditTasksViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditTasksViewModel ...", category: Category.Info, priority: Priority.Low);
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 70;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 90;
                AcceptFileActionCommand = new DelegateCommand<object>(TasksAcceptCommandAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                LinkedItemCancelCommand = new DelegateCommand<object>(LinkedItemCancelCommandAction);
                SkillsRowMouseDoubleClickCommand = new DelegateCommand<object>(SkillsRowMouseDoubleClickCommandAction);

                FillSkillsListItems();
                SelectedSkillsLinkedItems = new ObservableCollection<ProfessionalSkill>();

                GeosApplication.Instance.Logger.Log("Constructor AddEditTasksViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditTasksViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Intialize method....
        /// </summary>
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                SelectedSkillsLinkedItems = new ObservableCollection<ProfessionalSkill>();
                TasksCode = HrmService.GetLatestProfessionalTaskCode();
                Description = string.Empty;
                TasksJobRequirement = string.Empty;
                FillObjectivesList();
                SelectedObjective = ObjectivesTypeList.FirstOrDefault();
                TasksInUse = true;
                SelectedAccountSkillsLinkedItemsCount = SelectedSkillsLinkedItems.Count;

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method Init()...." + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method Init()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TasksList"></param>
        /// <param name="SelectedTask"></param>
        public void EditInit(ProfessionalTask SelectedTask)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                ProfessionalTask obj = HrmService.GetProfessionalTaskDetailsById(SelectedTask.IdProfessionalTask);
                ClonedSkillsList = new ObservableCollection<ProfessionalSkill>(obj.ProfessionalSkillList);

                TasksCode = obj.Code;
                Description = obj.Description;
                TasksJobRequirement = obj.JobRequirement;
                IdProfessionalTask = obj.IdProfessionalTask;
                FillObjectivesList();
                FillSkillsListItems();
                SelectedObjective = ObjectivesTypeList.FirstOrDefault(x => x.IdProfessionalObjective == SelectedTask.IdProfessionalObjective);
                TasksInUse = obj.InUse;
                //SelectedSkillsLinkedItems = new ObservableCollection<ProfessionalSkill>(ClonedSkillsList);
                foreach(ProfessionalSkill skill in ClonedSkillsList)
                {
                    ProfessionalSkill professionalSkill = new ProfessionalSkill();
                    professionalSkill = skill;

                    if(!skill.Code.Contains(" -"))
                        professionalSkill.Code = skill.Code + " - " + skill.SkillType.Value;
                    
                    SelectedSkillsLinkedItems.Add(professionalSkill);
                    //SelectedSkillsLinkedItems = new ObservableCollection<ProfessionalSkill>(ClonedSkillsList);
                }

                SelectedAccountSkillsLinkedItemsCount = SelectedSkillsLinkedItems.Count;

                foreach (ProfessionalSkill skill in SelectedSkillsLinkedItems)
                {
                    SkillsList.Remove(SkillsList.Where(x => x.IdProfessionalSkill == skill.IdProfessionalSkill).FirstOrDefault());
                }

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method EditInit()...." + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method EditInit()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Fill the Objectives List
        /// </summary>
        private void FillObjectivesList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillObjectivesList()...", category: Category.Info, priority: Priority.Low);

                IList<ProfessionalObjective> tempObjList = HrmService.GetProfessionalObjectives_ForDDL().OrderBy(a => a.CodeWithDescription).ToList();
                ObjectivesTypeList = new List<ProfessionalObjective>();
                ObjectivesTypeList.Insert(0, new ProfessionalObjective() { CodeWithDescription = "---" });
                ObjectivesTypeList.AddRange(tempObjList);

                GeosApplication.Instance.Logger.Log("Method FillObjectivesList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillObjectivesList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillObjectivesList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillObjectivesList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Fill the Skills List which are related to Tasks
        /// </summary>
        private void FillSkillsListItems()
        {
            GeosApplication.Instance.Logger.Log("Method FillSkillsListItems ...", category: Category.Info, priority: Priority.Low);
            try
            {
                IsBusy = true;
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                SkillsList = new ObservableCollection<ProfessionalSkill>(HrmService.GetProfessionalSkillsForSelection());
                SkillsList = new ObservableCollection<ProfessionalSkill>(SkillsList.Where(x => x.InUse == true).ToList());

                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method FillSkillsListItems() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSkillsListItems() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSkillsListItems() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSkillsListItems() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Accept Button Command ....
        /// </summary>
        /// <param name="obj"></param>
        public void TasksAcceptCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TasksAcceptCommandAction ...", category: Category.Info, priority: Priority.Low);
                
                error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedObjective"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedAccountSkillsLinkedItemsCount"));
                

                if (error != null)
                {
                    return;
                }

                char[] trimChars = { '\r', '\n' };
                Description = Description == null ? "" : Description;
                TasksJobRequirement = TasksJobRequirement == null ? "" : TasksJobRequirement;
                if (Description != null || TasksJobRequirement != null)
                {
                    if (Description.Contains("\r\n") || TasksJobRequirement.Contains("\r\n"))
                    {
                        Description = Description.TrimEnd(trimChars);
                        Description = Description.TrimStart(trimChars);
                        TasksJobRequirement = TasksJobRequirement.TrimEnd(trimChars);
                        TasksJobRequirement = TasksJobRequirement.TrimStart(trimChars);
                    }
                }

                #region Add New Task
                if (IsNew) // Add New Task
                {
                    NewProfessionalTask = new ProfessionalTask();

                    NewProfessionalTask.Code = TasksCode;
                    NewProfessionalTask.Description = Description;
                    NewProfessionalTask.JobRequirement = TasksJobRequirement;
                    NewProfessionalTask.ProfessionalObjective = SelectedObjective;
                    NewProfessionalTask.IdProfessionalObjective = SelectedObjective.IdProfessionalObjective;
                    NewProfessionalTask.ProfessionalSkillList = new List<ProfessionalSkill>(SelectedSkillsLinkedItems);
                    NewProfessionalTask.InUse = TasksInUse;
                    NewProfessionalTask.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    UInt64 result = Convert.ToUInt32(HrmService.AddProfessionalTask(NewProfessionalTask));

                    if (result > 0)
                        IsSave = true;
                    else
                        IsSave = false;

                    if(IsSave)
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TaskAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    RequestClose(null, null);
                }
                #endregion

                #region Edit Selected Task
                else // Edit Selected Task
                {
                    EditProfessionalTask = new ProfessionalTask();

                    EditProfessionalTask.Code = TasksCode;
                    EditProfessionalTask.Description = Description;
                    EditProfessionalTask.JobRequirement = TasksJobRequirement;
                    EditProfessionalTask.ProfessionalObjective = SelectedObjective;
                    EditProfessionalTask.IdProfessionalObjective = SelectedObjective.IdProfessionalObjective;

                    if (DeletedSkillsList == null)
                        DeletedSkillsList = new ObservableCollection<ProfessionalSkill>();

                    if (EditProfessionalTask.ProfessionalSkillList == null)
                        EditProfessionalTask.ProfessionalSkillList = new List<ProfessionalSkill>();

                    if (DeletedSkillsList.Count > 0) // if any current skills deleted
                    {
                        EditProfessionalTask.ProfessionalSkillList = new List<ProfessionalSkill>();
                        foreach(ProfessionalSkill skill in SelectedSkillsLinkedItems)
                        {
                            if (!ClonedSkillsList.Any(x => x.IdProfessionalSkill == skill.IdProfessionalSkill))
                                EditProfessionalTask.ProfessionalSkillList.Add(skill);
                        }
                        EditProfessionalTask.ProfessionalSkillList.AddRange(DeletedSkillsList);
                    }
                    else
                    {
                        foreach (ProfessionalSkill skill in SelectedSkillsLinkedItems)
                        {
                            if (!ClonedSkillsList.Any(x => x.IdProfessionalSkill == skill.IdProfessionalSkill))
                                EditProfessionalTask.ProfessionalSkillList.Add(skill);
                        }
                    }

                    EditProfessionalTask.InUse = TasksInUse;
                    EditProfessionalTask.IdProfessionalTask = IdProfessionalTask;


                    IsSave = HrmService.UpdateProfessionalTask(EditProfessionalTask);

                    if (IsSave)
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TaskUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    RequestClose(null, null);
                }
                #endregion

                GeosApplication.Instance.Logger.Log("Method TasksAcceptCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Error in Method TasksAcceptCommandAction()...." + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method TasksAcceptCommandAction()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method TasksAcceptCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Close Window
        /// </summary>
        /// <param name="obj"></param>
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

        /// <summary>
        /// Method to delete linkeditem from linked Skill items.
        /// </summary>
        /// <param name="obj"></param>
        private void LinkedItemCancelCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method LinkedItemCancelCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (obj is ProfessionalSkill)
                {
                    ProfessionalSkill linkedItem = obj as ProfessionalSkill;

                    if (DeletedSkillsList == null)
                        DeletedSkillsList = new ObservableCollection<ProfessionalSkill>();

                    if(linkedItem != null)
                    {
                        if(linkedItem.Code.Contains(" -"))
                            linkedItem.Code = linkedItem.Code.Substring(0, linkedItem.Code.IndexOf(" -"));
                        //else
                        //    linkedItem.Code = linkedItem.Code.Substring(0, linkedItem.Code.IndexOf(" -"));

                        linkedItem.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        SelectedSkillsLinkedItems.Remove(linkedItem);

                        if(ClonedSkillsList != null)
                        {
                            DeletedSkillsList.Add(linkedItem);
                        }

                        ProfessionalSkill pfSkill = new ProfessionalSkill();

                        if (linkedItem.Code.Contains(" -"))
                            pfSkill.Code = linkedItem.Code.Substring(0, linkedItem.Code.IndexOf(" -"));
                        else
                            pfSkill.Code = linkedItem.Code;

                        pfSkill.CreatedBy = linkedItem.CreatedBy;
                        pfSkill.CreatedIn = linkedItem.CreatedIn;
                        pfSkill.Description = linkedItem.Description;
                        pfSkill.IdProfessionalSkill = linkedItem.IdProfessionalSkill;
                        pfSkill.IdSkillType = linkedItem.IdSkillType;
                        pfSkill.InUse = linkedItem.InUse;
                        pfSkill.ModifiedBy = linkedItem.ModifiedBy;
                        pfSkill.ModifiedIn = linkedItem.ModifiedIn;
                        pfSkill.Name = linkedItem.Name;
                        pfSkill.SkillType = linkedItem.SkillType;
                        pfSkill.TransactionOperation = ModelBase.TransactionOperations.Add;

                        SkillsList.Add(pfSkill);
                    }
                }

                SkillsList = new ObservableCollection<ProfessionalSkill>( SkillsList.OrderBy(x => x.Code).ToList());   //  HrmService.GetAllJobDescriptions_V2046().OrderBy(a => a.JobDescriptionTitleAndCode).ToList();
                SelectedAccountSkillsLinkedItemsCount = SelectedSkillsLinkedItems.Count;

                GeosApplication.Instance.Logger.Log("Method LinkedItemCancelCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LinkedItemCancelCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Double Click on Selected Skill Row from Skills Grid
        /// </summary>
        /// <param name="obj"></param>
        private void SkillsRowMouseDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method SkillsRowMouseDoubleClickCommandAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                IsBusy = true;
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                if (obj is ProfessionalSkill)
                {
                    ProfessionalSkill professionalSkill = obj as ProfessionalSkill;

                    if (SelectedSkillsLinkedItems == null)
                        SelectedSkillsLinkedItems = new ObservableCollection<ProfessionalSkill>();

                    if(SelectedSkillsLinkedItems.Count >= 5)
                    {
                        IsBusy = false;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SkillsMaxCountWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    else
                    {
                        if (!SelectedSkillsLinkedItems.Any(x => x.IdProfessionalSkill == professionalSkill.IdProfessionalSkill && x.InUse == true))
                        {
                            ProfessionalSkill eachSkillLinkedItem = new ProfessionalSkill();

                            eachSkillLinkedItem.Code = professionalSkill.Code + " - " + professionalSkill.SkillType.Value;
                            eachSkillLinkedItem.CreatedBy = professionalSkill.CreatedBy;
                            eachSkillLinkedItem.CreatedIn = professionalSkill.CreatedIn;
                            eachSkillLinkedItem.Description = professionalSkill.Description;
                            eachSkillLinkedItem.IdProfessionalSkill = professionalSkill.IdProfessionalSkill;
                            eachSkillLinkedItem.IdSkillType = professionalSkill.IdSkillType;
                            eachSkillLinkedItem.InUse = professionalSkill.InUse;
                            eachSkillLinkedItem.ModifiedBy = professionalSkill.ModifiedBy;
                            eachSkillLinkedItem.ModifiedIn = professionalSkill.ModifiedIn;
                            eachSkillLinkedItem.Name = professionalSkill.Name;
                            eachSkillLinkedItem.SkillType = professionalSkill.SkillType;
                            eachSkillLinkedItem.TransactionOperation = ModelBase.TransactionOperations.Add;

                            SelectedSkillsLinkedItems.Add(eachSkillLinkedItem);
                            SkillsList.Remove(SkillsList.FirstOrDefault(x => x.IdProfessionalSkill == eachSkillLinkedItem.IdProfessionalSkill));
                        }
                    }
                }

                SelectedAccountSkillsLinkedItemsCount = SelectedSkillsLinkedItems.Count;
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SkillsRowMouseDoubleClickCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SkillsRowMouseDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                return error;
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => Description)] +
                    me[BindableBase.GetPropertyName(() => SelectedObjective)] +
                    me[BindableBase.GetPropertyName(() => SelectedAccountSkillsLinkedItemsCount)];


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
                string objectiveDescripton = BindableBase.GetPropertyName(() => Description);
                string _selectedObjective = BindableBase.GetPropertyName(() => SelectedObjective);
                string _selectedSkillsLinkedItemsCount = BindableBase.GetPropertyName(() => SelectedAccountSkillsLinkedItemsCount);

                if (columnName == objectiveDescripton)
                {
                    return TasksValidation.GetErrorMessage(objectiveDescripton, Description);
                }
                if (columnName == _selectedObjective)
                {
                    return TasksValidation.GetErrorMessage(_selectedObjective, SelectedObjective.CodeWithDescription);
                }
                if(columnName == _selectedSkillsLinkedItemsCount)
                {
                    return TasksValidation.GetErrorMessage(_selectedSkillsLinkedItemsCount, SelectedAccountSkillsLinkedItemsCount);
                }

                return null;
            }
        }
        #endregion
    }
}
