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
using Emdep.Geos.Modules.APM.ViewModels;
using DevExpress.Xpf.WindowsUI;

namespace Emdep.Geos.Modules.APM.Views
{
    /// <summary>
    /// Interaction logic for AddActionPlansView.xaml
    /// </summary>
    public partial class AddEditTaskView : WinUIDialogWindow
    {
        public AddEditTaskView()
        {
            InitializeComponent();
        }

        //[rdixit][30.09.2024][GEOS2-6496]
        private void tbeValue_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            DevExpress.Xpf.Editors.TrackBarEdit text = (DevExpress.Xpf.Editors.TrackBarEdit)sender;
            if (text.EditValue?.ToString() == "0")
                text.Foreground = Brushes.Red;
            else if (text.EditValue?.ToString() == "25")
                text.Foreground = Brushes.DarkOrange;
            else if (text.EditValue?.ToString() == "50")
                text.Foreground = Brushes.Orange;
            else if (text.EditValue?.ToString() == "75")
                text.Foreground = Brushes.Yellow;
            else if (text.EditValue?.ToString() == "100")
                text.Foreground = Brushes.Green;
        }
    }
}
