using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Internal;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Modules.OTM.CommonClass;
using Emdep.Geos.Modules.OTM.ViewModels;
using Emdep.Geos.UI.Converters;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for EditPORequestsView.xaml
    /// </summary>
    public partial class EditPORequestsView : WinUIDialogWindow
    {
        
        public EditPORequestsView()
        {
            InitializeComponent();
         

        }

      

        private DevExpress.Xpf.RichEdit.RichEditControl _richEdit;
        private void richEdit_Loaded(object sender, RoutedEventArgs e)
        {
            if (OTMCommon.Instance.Zoomfactor ==null)
            {
                OTMCommon.Instance.Zoomfactor = "80%";
            }
            if (OTMCommon.Instance.Zoomfactor == "80%")
            {
                var control = sender as DevExpress.Xpf.RichEdit.RichEditControl;
                _richEdit = control;
                if (control != null)
                {
                    control.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (control.ActiveView != null)
                        {
                            control.ActiveView.ZoomFactor = 0.80f;


                        }
                    }), System.Windows.Threading.DispatcherPriority.Loaded);
                }
                control.DocumentSource = ((EditPORequestsViewModel)dockManager1.DataContext).Emailbody;
                if (DataContext is EditPORequestsViewModel vm)
                {
                    vm.Zoomfactor = OTMCommon.Instance.Zoomfactor;
                }

            }
            else
            {
                var control = sender as DevExpress.Xpf.RichEdit.RichEditControl;
                _richEdit = control;
                if (control != null)
                {
                    // Parse Zoomfactor string (e.g., "105%") to float
                    string zoomStr = OTMCommon.Instance.Zoomfactor?.TrimEnd('%');
                    if (float.TryParse(zoomStr, out float zoomValue))
                    {
                        float zoomFactor = zoomValue / 100f; // convert % to factor

                        control.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (control.ActiveView != null)
                            {
                                control.ActiveView.ZoomFactor = zoomFactor;
                            }
                        }), System.Windows.Threading.DispatcherPriority.Loaded);
                    }
                }
                control.DocumentSource = ((EditPORequestsViewModel)dockManager1.DataContext).Emailbody;
                if (DataContext is EditPORequestsViewModel vm)
                {
                    vm.Zoomfactor = OTMCommon.Instance.Zoomfactor;
                    
                }
                //OTMCommon.Instance.Zoomfactor = null;
            }
            
        }

        private void senderImage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                          
                EmployeeCodeInitialToImageConverter o = new EmployeeCodeInitialToImageConverter();
                byte[] imgbyte = (byte[])o.Convert(((EditPORequestsViewModel)dockManager1.DataContext).SenderNameEmployeeCodesWithInitialLetters, null, null, null);
                ((Image)sender).Source = ByteArrayToImageSource(imgbyte);
                ((Image)sender).UpdateLayout();
            }
            catch { }
        }

        public static ImageSource ByteArrayToImageSource(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;
            string zoomPercent =(OTMCommon.Instance.Zoomfactor);
            
            try
            {
                using (var ms = new MemoryStream(imageData))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; // ensures stream can be closed
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    bitmap.Freeze(); // makes it cross-thread safe
                    return bitmap;
                }
            }
            catch
            {
                return null;
            }

        }

        //[pramod.misal][10-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9297]
        private void ZoomBar_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (_richEdit == null)
                return;

            double zoomPercent = Convert.ToDouble(e.NewValue);

            _richEdit.ActiveView.ZoomFactor = (float)(zoomPercent / 100.0) ; 

            //OTMCommon.Instance.Zoomfactor = Convert.ToString(zoomPercent) +"%";

            if (DataContext is EditPORequestsViewModel vm)
            {
                vm.Zoomfactor = Convert.ToString(zoomPercent) + "%";
                vm.NewZoomfactor = zoomPercent.ToString("F0");
                OTMCommon.Instance.Zoomfactor = vm.Zoomfactor;

                //OTMCommon.Instance.Zoomfactor = OTMCommon.Instance.NewZoomfactor;
            }


        }
        /// <summary>
        ///[pramod.misal][10-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9297]
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
            if (DataContext is EditPORequestsViewModel vm)
            {
                vm.Zoomfactor = $"{zoomPercent:F0}%";
                vm.NewZoomfactor = zoomPercent.ToString("F0"); 
                //OTMCommon.Instance.Zoomfactor = vm.Zoomfactor;
            }



        }
    }
}
