using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Modules.Epc.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.Services.Web.Workbench;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Converters;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class TaskDetailsViewModel : NavigationViewModelBase, IDisposable
    {
        #region Services

        IEpcService epcControl;
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Collections

        public ObservableCollection<LookupValue> TaskPriorityList { get; set; }
        public ObservableCollection<LookupValue> TaskTypeList { get; set; }

        private ObservableCollection<User> taskAssistanceUserList;
        public ObservableCollection<User> TaskAssistanceUserList
        {
            get { return taskAssistanceUserList; }
            set
            {
                SetProperty(ref taskAssistanceUserList, value, () => TaskAssistanceUserList);
            }
        }

        ProjectTask projectTaskView = new ProjectTask();
        public ProjectTask ProjectTaskView
        {
            get { return projectTaskView; }
            set
            {
                SetProperty(ref projectTaskView, value, () => ProjectTaskView);
            }
        }

        private TaskComment lastCommentRecord = new TaskComment();
        public TaskComment LastCommentRecord
        {
            get { return lastCommentRecord; }
            set
            {
                SetProperty(ref lastCommentRecord, value, () => LastCommentRecord);
            }
        }

        private TaskAttachment lastAttachmentRecord;
        public TaskAttachment LastAttachmentRecord
        {
            get { return lastAttachmentRecord; }
            set
            {
                SetProperty(ref lastAttachmentRecord, value, () => LastAttachmentRecord);
            }
        }

        byte[] UserProfileImageByte = null;

        private float totalTaskWorkingTime;
        public float TotalTaskWorkingTime
        {
            get { return totalTaskWorkingTime; }
            set
            {
                SetProperty(ref totalTaskWorkingTime, value, () => TotalTaskWorkingTime);
            }
        }

        private float taskEfficiency;
        public float TaskEfficiency
        {
            get { return taskEfficiency; }
            set
            {
                SetProperty(ref taskEfficiency, value, () => TaskEfficiency);
            }
        }

        private string taskComment;
        public string TaskComment
        {
            get { return taskComment; }
            set
            {
                taskComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskComment"));
            }
        }

        private string taskFileName;
        public string TaskFileName
        {
            get { return taskFileName; }
            set
            {
                taskFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskFileName"));
            }
        }

        #endregion

        #region ICommands

        public ICommand CommandAddAttachment { get; set; }
        public ICommand DeleteAttachmentRowCommand { get; set; }
        public ICommand AddNewCommentCommand { get; set; }
        public ICommand DeleteCommentRowCommand { get; set; }
        public ICommand DeleteTrackerHistoryRowCommand { get; set; }
        public ICommand DeleteWatcherRowCommand { get; set; }
        public ICommand AddNewRequestAssistanceCommand { get; set; }
        public ICommand DeleteTaskAssistanceRowCommand { get; set; }

        #endregion

        #region Constructor

        public TaskDetailsViewModel()
        {
            epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));

            if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_TASKTYPE"))
            {
                TaskTypeList = (ObservableCollection<LookupValue>)GeosApplication.Instance.ObjectPool["EPC_TASKTYPE"];
            }
            else
            {
                TaskTypeList = new ObservableCollection<LookupValue>(epcControl.GetLookupValues(12).AsEnumerable());
                GeosApplication.Instance.ObjectPool.Add("EPC_TASKTYPE", TaskTypeList);
            }

            if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_TASKPRIORITY"))
            {
                TaskPriorityList = (ObservableCollection<LookupValue>)GeosApplication.Instance.ObjectPool["EPC_TASKPRIORITY"];
            }
            else
            {
                TaskPriorityList = new ObservableCollection<LookupValue>(epcControl.GetLookupValues(10).AsEnumerable());
                GeosApplication.Instance.ObjectPool.Add("EPC_TASKPRIORITY", TaskPriorityList);
            }

            TaskAssistanceUserList = new ObservableCollection<User>(epcControl.GetUsers().AsEnumerable());
            //ProjectTaskView.TaskAssistances = new ObservableCollection<TaskAssistance>(ProjectTaskView.TaskAssistances);

            CommandAddAttachment = new DelegateCommand<object>(AddAttachment);
            DeleteAttachmentRowCommand = new DelegateCommand<object>(DeleteAttachmentCommandAction);
            AddNewCommentCommand = new DelegateCommand<string>(AddCommentCommandAction);
            DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);
            DeleteTrackerHistoryRowCommand = new DelegateCommand<object>(DeleteTrackerHistoryRowCommandAction);
            DeleteWatcherRowCommand = new DelegateCommand<object>(DeleteWatcherRowCommandAction);
            AddNewRequestAssistanceCommand = new DelegateCommand<object>(AddNewRequestAssistanceCommandAction);
            DeleteTaskAssistanceRowCommand = new DelegateCommand<object>(DeleteRequestAssistanceRowCommandAction);
        }

        #endregion

        #region Navigation Methods

        protected override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();
        }

        protected override void OnNavigatedTo()
        {
            base.OnNavigatedTo();

            ProjectTaskView = epcControl.GetTaskDetailsByTaskId(((ProjectTask)this.Parameter).IdTask);
            IList<TaskAttachment> taskAttachments = epcControl.GetTaskAttachmentByTaskId(((ProjectTask)this.Parameter).IdTask);
            ProjectTaskView.TaskAttachments = new ObservableCollection<TaskAttachment>(taskAttachments.ToList());

            //FileInfo file = new FileInfo(ProjectTaskView.TaskAttachments[0].FileName);
            SetUserProfileImage();

            try
            {
                if (ProjectTaskView.Description != null)
                {
                    if (!ProjectTaskView.Description.StartsWith("{\rtf1"))
                    {
                        ProjectTaskView.Description = @"{\rtf1 " + ProjectTaskView.Description + " }";
                    }
                }
            }
            catch (Exception ex)
            {
            }

            if (ProjectTaskView.TaskAttachments != null)
            {
                foreach (TaskAttachment taskAttachment in ProjectTaskView.TaskAttachments)
                {
                    try
                    {
                        if (taskAttachment.FileByte == null)
                        {
                            taskAttachment.TaskAttachmentImage = new BitmapImage(new Uri("pack://application:,,,/Emdep.Geos.Modules.Epc;component/Images/Common/Attachment.png", UriKind.RelativeOrAbsolute));
                        }
                        else if (taskAttachment.FileByte != null)
                        {
                            using (MemoryStream ms = new MemoryStream(taskAttachment.FileByte))
                            {
                                taskAttachment.TaskAttachmentImage = ToImageSource(new Icon(ms));
                            }

                            //byte[] fileInBytes = epcControl.GetTaskAttachment(Convert.ToInt32(item.IdTask));
                            //taskAttachment.TaskAttachmentImage = byteArrayToImage(taskAttachment.FileByte);
                        }
                    }
                    catch
                    {
                        taskAttachment.TaskAttachmentImage = new BitmapImage(new Uri("pack://application:,,,/Emdep.Geos.Modules.Epc;component/Images/Common/Attachment.png", UriKind.RelativeOrAbsolute));
                    }
                }
            }

            //foreach (var item in ProjectTaskView.TaskAttachments)
            //{
            //    try
            //    {
            //        byte[] fileInBytes = epcControl.GetTaskAttachment(Convert.ToInt32(item.IdTask));
            //        item.TaskAttachmentImage = byteArrayToImage(fileInBytes);
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}

            TotalTaskWorkingTime = epcControl.GetTotalTaskWorkingTime(projectTaskView.IdTask);

            if (TotalTaskWorkingTime != 0)
            {
                TaskEfficiency = (float)(Math.Round(((ProjectTaskView.PlannedHours / TotalTaskWorkingTime) * 100), 2));
            }

            if (TaskEfficiency < 100)
            {
                TaskEfficiency = TaskEfficiency;
            }

            if (TaskEfficiency > 120)
            {
                TaskEfficiency = 120;
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged; // = delegate { };
        private void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, propertyChangedEventArgs);
            }
        }

        #region Methods

        public void AddAttachment(object obj)
        {
            ProjectTask task = (ProjectTask)obj;
            IGeosRepositoryService fileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

            //byte[] bytes = epcControl.GetTaskAttachment(Convert.ToInt32(ProjectTaskView.IdTask));
            // List<TaskAttachment> ListTaskAttachment = new List<TaskAttachment>();
            //IList<TaskAttachment> taskAttachments = epcControl.GetTaskAttachmentByTaskId(task.IdTask);
            //ProjectTaskView.TaskAttachments = new ObservableCollection<TaskAttachment>(taskAttachments.ToList());
            //ProjectTaskView.TaskAttachments = ProjectTaskView.TaskAttachments.Select(taskAttachment => { taskAttachment.TaskAttachmentImage = ToImageSource(System.Drawing.Icon.ExtractAssociatedIcon(taskAttachment.FileName.ToString())); return taskAttachment; }).ToList();
            // objTaskAttachment.TaskAttachmentImage = ToImageSource(System.Drawing.Icon.ExtractAssociatedIcon(file.FullName.ToString()));

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.DefaultExt = ".*";
            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                FileInfo file = new FileInfo(openFileDialog.FileName);
                TaskAttachment objTaskAttachment = new TaskAttachment();

                objTaskAttachment.FileName = file.Name;
                objTaskAttachment.FileSize = file.Length;
                objTaskAttachment.IdTask = task.IdTask;
                objTaskAttachment = epcControl.AddTaskAttachment(objTaskAttachment);
                objTaskAttachment.TaskAttachmentImage = ToImageSource(System.Drawing.Icon.ExtractAssociatedIcon(file.FullName.ToString()));

                ProjectTaskView.TaskAttachments.Add(objTaskAttachment);
            }
        }

        public void DeleteAttachmentCommandAction(object obj)
        {
            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("DeleteAttachment").ToString(), "Transparent", CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                epcControl.DeleteTaskAttachmentById(((TaskAttachment)obj).IdTaskAttachment);
                ProjectTaskView.TaskAttachments.Remove((TaskAttachment)obj);
            }
        }

        public void AddCommentCommandAction(string taskComment)
        {
            if (!string.IsNullOrEmpty(TaskComment))
            {
                if (ProjectTaskView.TaskComments == null)
                {
                    ProjectTaskView.TaskComments = new ObservableCollection<TaskComment>();
                }

                TaskComment comment = new Data.Common.Epc.TaskComment()
                {
                    TransactionOperation = ModelBase.TransactionOperations.Add,
                    IdTask = this.ProjectTaskView.IdTask,
                    IdUser = this.ProjectTaskView.IdOwner,
                    CommentDate = DateTime.Now,
                    Comment = string.Copy(TaskComment),
                };

                comment = epcControl.AddTaskComment(comment);
                comment.User = this.ProjectTaskView.Owner;

                if (comment.IdTaskComment > 0)
                {
                    ProjectTaskView.TaskComments.Add(comment);
                }

                TaskComment = null;
            }
        }

        public void DeleteCommentCommandAction(object obj)
        {
            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("DeleteComment").ToString(), "Transparent", CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                epcControl.DeleteTaskCommentById(((TaskComment)obj).IdTaskComment);
                ProjectTaskView.TaskComments.Remove((TaskComment)obj);
            }
        }

        public void DeleteTrackerHistoryRowCommandAction(object obj)
        {
            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("DeleteTrackerHistory").ToString(), "Transparent", CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                epcControl.DeleteTaskWorkingTimeById(Convert.ToInt64(((TaskWorkingTime)obj).IdTaskWorkingTime));
                ProjectTaskView.TaskWorkingTimes.Remove((TaskWorkingTime)obj);
            }
        }

        public void DeleteWatcherRowCommandAction(object obj)
        {
            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("DeleteWatcher").ToString(), "Transparent", CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                epcControl.DeleteTaskWatcherById(((TaskWatcher)obj).IdTaskWatcher);
                ProjectTaskView.TaskWatchers.Remove((TaskWatcher)obj);
            }
        }

        public void AddNewRequestAssistanceCommandAction(object obj)
        {
            NewRequestAssistanceView newRequestAssistanceView = new NewRequestAssistanceView();
            NewRequestAssistanceViewModel newRequestAssistanceViewModel = new NewRequestAssistanceViewModel();

            TaskAssistance taskAssistance = new TaskAssistance()
            {
                IdTask = ProjectTaskView.IdTask,
                IdRequestFrom = GeosApplication.Instance.ActiveUser.IdUser,
                RequestDate = DateTime.Now,
                IdTaskAssistanceStatus = 75 // Task Assistance Status.
            };
            ((ISupportParameter)newRequestAssistanceViewModel).Parameter = taskAssistance;

            EventHandler handle = delegate { newRequestAssistanceView.Close(); };
            newRequestAssistanceViewModel.RequestClose += handle;
            newRequestAssistanceView.DataContext = newRequestAssistanceViewModel;
            newRequestAssistanceView.ShowDialogWindow();

            if (newRequestAssistanceViewModel.ISave)
            {
                ProjectTaskView.TaskAssistances.Add(newRequestAssistanceViewModel.TaskAssistanceData);
            }
        }

        public void DeleteRequestAssistanceRowCommandAction(object obj)
        {
            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("DeleteTaskAssistance").ToString(), "Transparent", CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                epcControl.DeleteTaskAssistanceById(((TaskAssistance)obj).IdTaskAssistance);
                ProjectTaskView.TaskAssistances.Remove((TaskAssistance)obj);
            }
        }

        public static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }

        public void Hide()
        {
            throw new NotImplementedException();
        }

        public Task<NotificationResult> ShowAsync()
        {
            throw new NotImplementedException();
        }

        public ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage BitmapImage = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            BitmapImage.BeginInit();
            BitmapImage.StreamSource = ms;
            BitmapImage.EndInit();
            ImageSource ImageSource = BitmapImage as ImageSource;

            return ImageSource;
        }

        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        public void Dispose()
        {
        }

        public void SetUserProfileImage()
        {
            for (int i = 0; i < ProjectTaskView.TaskUsers.Count; i++)
            {
                try
                {
                    UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImage(ProjectTaskView.TaskUsers[i].User.Login);
                    ProjectTaskView.TaskUsers[i].UserProfileImage = byteArrayToImage(UserProfileImageByte);
                }
                catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
                {
                    if (ThemeManager.ApplicationThemeName == "WhiteAndBlue")
                    {
                        ProjectTaskView.TaskUsers[i].UserProfileImage = new BitmapImage(new Uri("pack://application:,,,/Emdep.Geos.Modules.Epc;component/Images/LoginUserBlue.png", UriKind.RelativeOrAbsolute));
                    }
                    else
                    {
                        ProjectTaskView.TaskUsers[i].UserProfileImage = new BitmapImage(new Uri("pack://application:,,,/Emdep.Geos.Modules.Epc;component/Images/LoginUserWhite.png", UriKind.RelativeOrAbsolute));
                    }
                }
            }
        }

        public ImageSource byteArrayToImage(byte[] byteArrayIn)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(byteArrayIn);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();
            biImg.DecodePixelHeight = 10;
            biImg.DecodePixelWidth = 10;

            ImageSource imgSrc = biImg as ImageSource;
            return imgSrc;
        }

        /// <summary>
        ///  This method is for to convert Image to ImageSource
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource ConvertImageToImageSource(string path)
        {
            BitmapImage biImg = new BitmapImage(new Uri(path, UriKind.Relative));
            ImageSource imgSrc = biImg as ImageSource;
            return imgSrc;
        }

        #endregion

    }
}
