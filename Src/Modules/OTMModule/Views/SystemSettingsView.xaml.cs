using DevExpress.Xpf.Editors;
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

namespace Emdep.Geos.Modules.OTM.Views
{
    /// <summary>
    /// Interaction logic for SystemSettingsView.xaml
    /// </summary>
    public partial class SystemSettingsView : WinUIDialogWindow
    {
        public SystemSettingsView()
        {
            InitializeComponent();
        }
        private object _previousCheckedItems;

        private void ComboBoxEdit_PopupOpening(object sender, DevExpress.Xpf.Editors.OpenPopupEventArgs e)
        {

            if (sender is ComboBoxEdit comboBox)
            {
                _previousCheckedItems = comboBox.EditValue;
            }
        }
        private void ComboBoxEdit_PopupClosing(object sender, DevExpress.Xpf.Editors.ClosingPopupEventArgs e)
        {

            if (e.CloseMode == PopupCloseMode.Cancel && sender is ComboBoxEdit comboBox)
            {
                if (_previousCheckedItems != null)
                {
                    comboBox.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var old = _previousCheckedItems;


                        comboBox.EditValue = null;
                        comboBox.EditValue = old;


                        comboBox.UpdateLayout();
                        comboBox.Focus();
                        Keyboard.ClearFocus();
                    }), System.Windows.Threading.DispatcherPriority.Background);
                }
            }

        }

    }
}
