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

namespace Emdep.Geos.Modules.APM.Views
{
    /// <summary>
    /// Interaction logic for AddEditActionPlanView.xaml
    /// </summary>
    public partial class AddEditActionPlanView : WinUIDialogWindow
    {
        public AddEditActionPlanView()
        {
            InitializeComponent();
        }

        private Button _lastClickedButton = null;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_lastClickedButton != null)
            {
                _lastClickedButton.ClearValue(Button.BackgroundProperty); // Reset previous button color
            }

            Button clickedButton = sender as Button;
            clickedButton.Background = Brushes.LightGreen; // Highlight clicked button

            _lastClickedButton = clickedButton; // Store reference to last clicked button
        }

    }
}
