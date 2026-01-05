using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.ViewModels;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for CAMCADTimeTrackingView.xaml
    /// </summary>
    public partial class CAMCADTimeTrackingView : UserControl
    {
        public CAMCADTimeTrackingView()
        {
            InitializeComponent();
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ERMCommon.Instance.UnloadAsyncTimetracking = false;
                if (ERMCamCadTimeTrackingGetData.tokenToLoadFullCamCadTimetrackingInAsync != null)
                {
                    ERMCamCadTimeTrackingGetData.tokenToLoadFullCamCadTimetrackingInAsync.Token.ThrowIfCancellationRequested();
                    ERMCamCadTimeTrackingGetData.tokenToLoadFullCamCadTimetrackingInAsync.Cancel();
                    ERMCamCadTimeTrackingGetData.tokenToLoadFullCamCadTimetrackingInAsync.Dispose();
                }                

                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                ERMCommon.Instance.IsVisibleLabelTimetrackingInBackground = Visibility.Hidden;
                ERMCommon.Instance.PlantVisibleFlag = true;  //[gulab lakade][21 04 2023][Plant dropdown disable]

            }
            catch (Exception ex)
            {

            }
        }
    }
}
