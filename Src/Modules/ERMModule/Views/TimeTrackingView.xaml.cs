using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.UI.Common;
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

namespace Emdep.Geos.Modules.ERM.Views
{
    /// <summary>
    /// Interaction logic for TimeTrackingView.xaml
    /// </summary>
    public partial class TimeTrackingView : UserControl
    {
        public TimeTrackingView()
        {
            InitializeComponent();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ERMCommon.Instance.UnloadAsyncTimetracking = false;
                ERMTimeTrackingGetData.tokenToLoadFullTimetrackingInAsync.Token.ThrowIfCancellationRequested();
                
                ERMTimeTrackingGetData.tokenToLoadFullTimetrackingInAsync.Cancel();
                ERMTimeTrackingGetData.tokenToLoadFullTimetrackingInAsync.Dispose();
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                ERMCommon.Instance.IsVisibleLabelTimetrackingInBackground = Visibility.Hidden;
                ERMCommon.Instance.PlantVisibleFlag = true;  //[gulab lakade][21 04 2023][Plant dropdown disable]

            }
            catch(Exception ex)
            {

            }
        }
    }
}
