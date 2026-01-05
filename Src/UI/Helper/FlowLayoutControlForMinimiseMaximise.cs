using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.LayoutControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DevExpress.Mvvm.UI;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
   public class FlowLayoutControlForMinimiseMaximise : Behavior<FlowLayoutControl>
    {
        static double height = 0;
        static double SRMwidth = 0;//[pramod.misal][GEOS2-4452][11-07-2023]
        static double SRMheight = 0;//[pramod.misal][GEOS2-4452][11-07-2023]
        static double OTMChnageLogheight = 850;//[pramod.misal][GEOS2-4452][11-07-2023]

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MaximizedElementChanged += AssociatedObject_MaximizedElementChanged;
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            var flc = sender as FlowLayoutControl;         
            if (flc.Children != null)
            {
                flc.MaximizedElement = flc.Children[0] as FrameworkElement;
            }
        }

        private void AssociatedObject_MaximizedElementChanged(object sender, DevExpress.Xpf.Core.ValueChangedEventArgs<System.Windows.FrameworkElement> e)
        {
            #region [rdixit][GEOS2-4215][15.02.2023]
            if (e.NewValue != null)
            {
                if (e.NewValue.Name.ToString() == "CRM_Leads_Comments" || e.NewValue.Name.ToString() == "CRM_Leads_Attachments" || e.NewValue.Name.ToString() == "CRM_Leads_Activities" ||
                e.NewValue.Name.ToString() == "CRM_Leads_Shipment" || e.NewValue.Name.ToString() == "CRM_Leads_ChangeLog")
                {
                    e.NewValue.Height = height;
                    e.NewValue.Width = e.NewValue.ActualWidth;
                }
                else if (e.NewValue.Name.ToString() == "CRM_LeadsEdit_Information")
                {
                    height = e.NewValue.ActualHeight;
                }              
                else if (e.NewValue.Name.ToString() == "SRM_Suplier_Contacts")//[pramod.misal][GEOS2-4452][11-07-2023]
                {
                    e.NewValue.Height = SRMheight;
                    e.NewValue.Width = SRMwidth;
                }
                else if ( e.NewValue.Name.ToString() == "CRM_Custedit" || e.NewValue.Name.ToString() == "CRM_EditAsSalesOwner")//[pramod.misal][GEOS2-4452][11-07-2023]
                {
                    //e.NewValue.Height = SRMheight;
                    e.NewValue.Width = SRMwidth;
                }
                else if (e.NewValue.Name.ToString() == "SRM_Suplier_Information")//[pramod.misal][GEOS2-4452][11-07-2023] 
                {
                    e.NewValue.Width = Convert.ToDouble(System.Windows.SystemParameters.WorkArea.Width) - 320;
                    SRMheight = e.NewValue.ActualHeight;
                    SRMwidth = e.NewValue.Width;
                }
                else if (e.NewValue.Name.ToString() == "CRM_EditCusInformation")//[pramod.misal][GEOS2-4452][11-07-2023] 
                {
                   
                    //SRMheight = e.NewValue.ActualHeight;
                    SRMwidth = e.NewValue.ActualWidth;
                }
                else if (e.NewValue.Name.ToString() == "OTM_ChangLog")//[pramod.misal][GEOS2-9139][18-07-2025] 
                {

                    e.NewValue.Height = OTMChnageLogheight;
                }
            }
            #endregion
            if (e.NewValue == null)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var flc = sender as FlowLayoutControl;
                    if (flc.Children != null)
                    {
                        flc.MaximizedElement = flc.Children[0] as FrameworkElement;
                    }
                }));
            }
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.MaximizedElementChanged -= AssociatedObject_MaximizedElementChanged;
            base.OnDetaching();
        }




    }
}
