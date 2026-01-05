using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules;
using Emdep.Geos.UI.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Workbench.ViewModels
{
    [POCOViewModel]
    public class CustomNotificationViewModel : IDisposable
    {
        #region Declaration

        private string caption;

        #endregion  // Declaration

        #region Properties

        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        public virtual string Content { get; set; }

        #endregion  // Properties

        //public CustomNotificationViewModel()
        //{
        //    //NotificationService service = new NotificationService();
        //    Caption = "Custom Notification";
        //    Content = String.Format("Time: {0}", DateTime.Now);
        //    ////CustomNotificationViewModel CustomNotificationViewModel = new ViewModels.CustomNotificationViewModel();
        //    //var notification = service.CreateCustomNotification(this);
        //    //notification.ShowAsync();

        //}
        //public string Textname;
        //[ServiceProperty(Key = "ServiceWithDefaultNotifications")]
        //protected virtual INotificationService DefaultNotificationService { get { return null; } }
        //[ServiceProperty(Key = "ServiceWithCustomNotifications")]
        //protected virtual INotificationService CustomNotificationService { get { return null; } }

        //public ICommand ShowDefaultNotificationCommand { get; set; }

        //public void ShowDefaultNotification(object obj)
        //{
        //    INotification notification = DefaultNotificationService.CreatePredefinedNotification
        //        ("Predefined Notification", "First line", "fff", GetImage("/Assets/Images/UserProfile.png"));
        //    notification.ShowAsync();
        //}


        ////public void ShowCustomNotification() {
        ////    CustomNotificationViewModel vm = ViewModelSource.Create(() => new CustomNotificationViewModel());
        ////    vm.Caption = "Custom Notification";
        ////    vm.Content = String.Format("Time: {0}", DateTime.Now);
        ////    INotification notification = CustomNotificationService.CreateCustomNotification(vm);
        ////    notification.ShowAsync();
        ////}
        //public CustomNotificationViewModel()
        //{
        //    ShowDefaultNotificationCommand = new RelayCommand(new Action<object>(ShowDefaultNotification));
        //    //if (GeosApplication.Notification != null)
        //    //{
        //    //    ShowDefaultNotification(GeosApplication.Notification.Message, "", "", GetImage("/Assets/Images/UserProfile.png"));
        //    //}
        //}
        //public ImageSource GetImage(string path)
        //{
        //    return new BitmapImage(new Uri(path, UriKind.Relative));
        //}
        //public void ShowDefaultNotification(string Text1, string Text2, string Text3, ImageSource ImageSource)
        //{
        //    INotification notification = DefaultNotificationService.CreatePredefinedNotification
        //        (Text1, Text2, Text3, ImageSource);
        //    notification.ShowAsync();
        //}

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Environment.Exit(0);
        }
    }

}
