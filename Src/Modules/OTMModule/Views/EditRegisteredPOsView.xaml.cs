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
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.OTM.Views
{
    /// <summary>
    /// Interaction logic for EditRegisteredPOsView.xaml
    /// </summary>
    public partial class EditRegisteredPOsView : WinUIDialogWindow
    {
        public EditRegisteredPOsView()
        {
            InitializeComponent();
        }

        private void TextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var editor = sender as DevExpress.Xpf.Editors.TextEdit;
            if (editor != null && editor.EditValue is double value && value < 0)
            {
                editor.EditValue = 0.00;
            }
        }
    }
}
