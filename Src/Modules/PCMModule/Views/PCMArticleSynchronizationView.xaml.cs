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
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;

namespace Emdep.Geos.Modules.PCM.Views
{
    /// <summary>
    /// Interaction logic for PCMArticleSynchronizationView.xaml
    /// </summary>
    public partial class PCMArticleSynchronizationView : WinUIDialogWindow
    {
        public PCMArticleSynchronizationView()
        {
            InitializeComponent();
        }

        private void Currency_Loaded(object sender, RoutedEventArgs e)
        {
            var t = (DevExpress.Xpf.Editors.ComboBoxEdit)e.OriginalSource;
            var c = (System.Windows.Media.SolidColorBrush)t.Foreground;
            if (ApplicationThemeHelper.ApplicationThemeName.ToString() == "BlackAndBlue")
                t.Foreground = Brushes.White;
            else
                t.Foreground = Brushes.Black;
        }

        private void LostFocus_textblockArticle(object sender, RoutedEventArgs e)
        {


            ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;
            if (!comboBoxEdit.IsKeyboardFocusWithin)
            {
                var t = (DevExpress.Xpf.Editors.ComboBoxEdit)e.Source;
                var c = (System.Windows.Media.SolidColorBrush)t.Foreground;
                if (ApplicationThemeHelper.ApplicationThemeName.ToString() == "BlackAndBlue")
                    t.Foreground = Brushes.White;
                else
                    t.Foreground = Brushes.Black;
            }
        }

        private void EditValue_textblockArticle(object sender, RoutedEventArgs e)
        {
            var t = ((System.Windows.Controls.Control)e.OriginalSource);  
            if (ApplicationThemeHelper.ApplicationThemeName.ToString() == "BlackAndBlue")
                t.Foreground = Brushes.Black;
            else
                t.Foreground = Brushes.White;
        }


        private void EditValue_textblockCurrency(object sender, RoutedEventArgs e)
        {
            var t = ((System.Windows.Controls.Control)e.OriginalSource);       
            if (ApplicationThemeHelper.ApplicationThemeName.ToString() == "BlackAndBlue")
                t.Foreground = Brushes.Black;
            else
                t.Foreground = Brushes.White;
        }
    }
}
