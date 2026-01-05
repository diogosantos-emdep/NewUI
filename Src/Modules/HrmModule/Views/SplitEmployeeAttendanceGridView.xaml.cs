using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
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
using Emdep.Geos.UI.CustomControls;

namespace Emdep.Geos.Modules.Hrm.Views
{
    /// <summary>
    /// Interaction logic for SplitEmployeeAttendanceGridView.xaml
    /// </summary>
    public partial class SplitEmployeeAttendanceGridView : WinUIDialogWindow
    {
        public SplitEmployeeAttendanceGridView()
        {
            InitializeComponent();
        }
        //[GEOS2-7973][30.05.2025][rdixit]
        private void GridControlEmpolyeeAttendance_Loaded(object sender, RoutedEventArgs e)
        {
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["EmployeeSplitAttendanceIncompleteDateRangeMessage"].ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        }
    }
}
