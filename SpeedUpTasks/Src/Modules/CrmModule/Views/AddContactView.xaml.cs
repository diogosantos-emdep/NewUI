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
using DevExpress.Xpf.LayoutControl;

namespace Emdep.Geos.Modules.Crm.Views
{
    /// <summary>
    /// Interaction logic for AddContactView.xaml
    /// </summary>
    public partial class AddContactView : WinUIDialogWindow
    {
        public AddContactView()
        {
            InitializeComponent();
        }

        private void GroupBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var groupBox = (DevExpress.Xpf.LayoutControl.GroupBox)sender;
            groupBox.State = groupBox.State == GroupBoxState.Normal ? GroupBoxState.Maximized : GroupBoxState.Normal;
        }
    }
}
