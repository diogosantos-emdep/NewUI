using DevExpress.Data;
using DevExpress.Xpf.Grid;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
namespace Emdep.Geos.Modules.SRM.Views
{
    /// <summary>
    /// Interaction logic for EditPendingReviewView.xaml
    /// </summary>
    public partial class EditPendingReviewView : WinUIDialogWindow
    {
        public EditPendingReviewView()
        {
            InitializeComponent();
        }
        //[nsatpute][GEOS2-7033][27-02-2025]
        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {                
                if (sender is DataControlBase cel)
                {
                    var cellValue = cel.CurrentCellValue;
                    System.Windows.Clipboard.SetText(cellValue?.ToString() ?? string.Empty);
                }
            }
        }
    }
}