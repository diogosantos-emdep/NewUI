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

namespace Emdep.Geos.Modules.Crm.Views
{
    /// <summary>
    /// Interaction logic for EditActionsView.xaml
    /// </summary>
    public partial class EditActionsView : WinUIDialogWindow
    {
        public EditActionsView()
        {
            InitializeComponent();
        }

        private void TextEdit_TextInput(object sender, TextCompositionEventArgs e)
        {
      
        }

        private void TextEdit_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}
