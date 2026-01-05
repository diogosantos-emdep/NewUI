using Microsoft.Web.WebView2.Core;
using ServiceStack;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.Hrm.Views
{
    /// <summary>
    /// Interaction logic for AttendanceView.xaml
    /// </summary>
    public partial class AddEditAttendanceView : UserControl
    {
        public AddEditAttendanceView()
        {
            InitializeComponent();

        }
        //private async void webView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.IsSuccess)
        //        {
        //            // Ensure CoreWebView2 is fully initialized
        //            await webView.EnsureCoreWebView2Async();

        //            // Set the source URL once initialized
        //            if (!string.IsNullOrEmpty(webView.Source.ToString()))
        //            {
        //                webView.Source = new Uri(webView.Source.ToString());
        //            }
        //        }
        //        else
        //        {
        //            UI.CustomControls.CustomMessageBox.Show($"WebView2 initialization failed.", Application.Current.Resources["PopUpWarningColor"].ToString(), UI.CustomControls.CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        UI.CustomControls.CustomMessageBox.Show($"Error initializing WebView2: {ex.Message}", Application.Current.Resources["PopUpWarningColor"].ToString(), UI.CustomControls.CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //}
    }
}
