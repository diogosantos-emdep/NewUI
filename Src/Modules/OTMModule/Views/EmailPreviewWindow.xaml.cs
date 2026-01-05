using Emdep.Geos.Modules.OTM.CommonClass;
using Emdep.Geos.Modules.OTM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.OTM.Views
{
    /// <summary>
    /// Interaction logic for EmailPreviewWindow.xaml
    /// </summary>
    public partial class EmailPreviewWindow : Window
    {
        private DevExpress.Xpf.RichEdit.RichEditControl _richEdit;
        
        public EmailPreviewWindow()
        {
            InitializeComponent();
        }


        //[pramod.misal][GEOS2-8307][23-06-2025]
        private void richEdit_Loaded(object sender, RoutedEventArgs e)
        {
            OTMCommon.Instance.Zoomfactor = "100%";
            var control = sender as DevExpress.Xpf.RichEdit.RichEditControl;
            _richEdit = control;
            if (control != null)
            {
                control.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (control.ActiveView != null)
                    {
                        //control.ActiveView.ZoomFactor = 0.48f;
                       
                        control.ActiveView.ZoomFactor = 1f;
                       
                        //control.ActiveViewType = RichEditViewType.Simple;

                    }
                }), System.Windows.Threading.DispatcherPriority.Loaded);
            }

            if (DataContext is EmailPreviewWindowModel vm)
            {
                vm.Zoomfactor = OTMCommon.Instance.Zoomfactor;
                // Remove the trailing '%' if it exists
                vm.NewZoomfactor = vm.Zoomfactor.EndsWith("%")
                    ? vm.Zoomfactor.TrimEnd('%')
                    : vm.Zoomfactor;
            }
        }

        //[pramod.misal][GEOS2-8307][23-06-2025]
        private void ZoomBar_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (_richEdit == null)
                return;

            double zoomPercent = Convert.ToDouble(e.NewValue);
            richEdit.ActiveView.ZoomFactor = (float)(zoomPercent / 100.0);
            OTMCommon.Instance.Zoomfactor = Convert.ToString(zoomPercent) + "%";

            if (DataContext is EmailPreviewWindowModel vm)
            {
                vm.Zoomfactor = OTMCommon.Instance.Zoomfactor;
                vm.NewZoomfactor = zoomPercent.ToString("F0");
            }

        }

        /// <summary>
        ///  //[pramod.misal][GEOS2-8307][23-06-2025]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richEdit_ZoomChanged(object sender, EventArgs e)
        {
            var zoomBar = this.FindName("ZoomBar") as DevExpress.Xpf.Editors.TrackBarEdit;


            var editor = sender as DevExpress.Xpf.RichEdit.RichEditControl;
            if (editor == null)
                return;

            // Get current zoom as percentage
            double zoomPercent = editor.ActiveView.ZoomFactor * 100.0;
            // Clamp between 10% and 500%
            zoomPercent = Math.Max(10, Math.Min(500, zoomPercent));

            if (zoomBar != null)
            {
                zoomBar.Value = zoomPercent;
            }


            // Clamp to min/max range
            if (zoomPercent < 10)
                zoomPercent = 10;
            else if (zoomPercent > 500)
                zoomPercent = 500;

            // Apply back to control if outside range
            editor.ActiveView.ZoomFactor = (float)(zoomPercent / 100.0);

            // Update ViewModel
            if (DataContext is EmailPreviewWindowModel vm)
            {
                vm.Zoomfactor = $"{zoomPercent:F0}%";
                vm.NewZoomfactor = zoomPercent.ToString("F0");
                //OTMCommon.Instance.Zoomfactor = vm.Zoomfactor;
            }
        }
    }
}
