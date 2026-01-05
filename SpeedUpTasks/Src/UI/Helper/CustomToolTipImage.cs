using DevExpress.Mvvm.UI.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using DevExpress.XtraScheduler;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.Drawing;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class CustomToolTipImage : Behavior<SchedulerControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.AppointmentViewInfoCustomizing += AssociatedObject_AppointmentViewInfoCustomizing;
        }

        Dictionary<Resource, BitmapImage> resourceImages = new Dictionary<Resource, BitmapImage>();

        private void AssociatedObject_AppointmentViewInfoCustomizing(object sender, AppointmentViewInfoCustomizingEventArgs e)
        {
            AppointmentViewInfo viewInfo = e.ViewInfo;

            CustomAppointmentData cad = new CustomAppointmentData();

            object tooltipTitle = e.ViewInfo.Appointment.CustomFields["TooltipTitle"];

            if (tooltipTitle != null && tooltipTitle != DBNull.Value)
                cad.TooltipTitle = Convert.ToString(tooltipTitle);

            object tooltipSubject = e.ViewInfo.Appointment.CustomFields["TooltipSubject"];

            if (tooltipSubject != null && tooltipSubject != DBNull.Value)
                cad.TooltipSubject = Convert.ToString(tooltipSubject);

            object OwnerInfo = e.ViewInfo.Appointment.CustomFields["OwnerInfo"];

            if (OwnerInfo != null && OwnerInfo != DBNull.Value)
                cad.Owner = Convert.ToString(OwnerInfo);

            Resource resource = this.AssociatedObject.Storage.ResourceStorage.GetResourceById(viewInfo.Appointment.ResourceId);
            if (resource == ResourceEmpty.Resource || resource.GetImage() == null)
                cad.Picture = null;
            else
            {
                if (!this.resourceImages.ContainsKey(resource))
                    this.resourceImages[resource] = resource.GetImage() as BitmapImage;

                cad.Picture = this.resourceImages[resource];
            }

            object IsCriticalInfo = e.ViewInfo.Appointment.CustomFields["IsCritical"];

            if (IsCriticalInfo != null && IsCriticalInfo != DBNull.Value)
            {
                if (Convert.ToBoolean(IsCriticalInfo))
                {
                    Resource resource2 = this.AssociatedObject.Storage.ResourceStorage.GetResourceById(-1);
                    if (resource2 == ResourceEmpty.Resource || resource2.GetImage() == null)
                        cad.Picture2 = null;
                    else
                    {
                        if (!this.resourceImages.ContainsKey(resource2))
                            this.resourceImages[resource2] = resource2.GetImage() as BitmapImage;

                        cad.Picture2 = this.resourceImages[resource2];
                    }
                }
            }

            viewInfo.CustomViewInfo = cad;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.AppointmentViewInfoCustomizing -= AssociatedObject_AppointmentViewInfoCustomizing;
            base.OnDetaching();
        }
    }


    public class CustomAppointmentData : DependencyObject
    {
        public static readonly DependencyProperty TooltipTitleProperty = DependencyProperty.Register("TooltipTitle", typeof(string), typeof(CustomAppointmentData));
        public string TooltipTitle { get { return (string)GetValue(TooltipTitleProperty); } set { SetValue(TooltipTitleProperty, value); } }

        public static readonly DependencyProperty TooltipSubjectProperty = DependencyProperty.Register("TooltipSubject", typeof(string), typeof(CustomAppointmentData));
        public string TooltipSubject { get { return (string)GetValue(TooltipSubjectProperty); } set { SetValue(TooltipSubjectProperty, value); } }

        public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register("Owner", typeof(string), typeof(CustomAppointmentData));
        public string Owner { get { return (string)GetValue(OwnerProperty); } set { SetValue(OwnerProperty, value); } }

        public static readonly DependencyProperty PictureProperty = DependencyProperty.Register("Picture", typeof(object), typeof(CustomAppointmentData));
        public object Picture { get { return (object)GetValue(PictureProperty); } set { SetValue(PictureProperty, value); } }

        public static readonly DependencyProperty Picture2Property = DependencyProperty.Register("Picture2", typeof(object), typeof(CustomAppointmentData));
        public object Picture2 { get { return (object)GetValue(Picture2Property); } set { SetValue(Picture2Property, value); } }

    }
}
