using Emdep.Geos.Modules.HarnessPart.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.HarnessPart
{
    /// <summary>
    /// Interaction logic for frmattachments.xaml
    /// </summary>
    public partial class frmattachments : Window
    {
        public frmattachments()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            ObservableCollection<clsHarnessPartVisualAidsDoc> listclsAccessories = new ObservableCollection<clsHarnessPartVisualAidsDoc>();
            
            listclsAccessories.Add(new clsHarnessPartVisualAidsDoc() { IdHarnessPart = true, CategoryName = "Commercial Visual Aid", FileName = "000011.bmp" });
            listclsAccessories.Add(new clsHarnessPartVisualAidsDoc() { IdHarnessPart = true, CategoryName = "Wintestem  Visual Aid", FileName = "05551.wtg" });
            listclsAccessories.Add(new clsHarnessPartVisualAidsDoc() { IdHarnessPart = true, CategoryName = "Documentation", FileName = "01.bmp" });
            listclsAccessories.Add(new clsHarnessPartVisualAidsDoc() { IdHarnessPart = true, CategoryName = "Documentation", FileName = "01.bmp" });
            listclsAccessories.Add(new clsHarnessPartVisualAidsDoc() { IdHarnessPart = true, CategoryName = "Documentation", FileName = "01.bmp" });
            listclsAccessories.Add(new clsHarnessPartVisualAidsDoc() { IdHarnessPart = true, CategoryName = "Documentation", FileName = "01.pdf" });
            //listclsAccessories.Add(new clsType() { ID = 1, Name = "Way" });
            //listclsAccessories.Add(new clsType() { ID = 2, Name = "Family" });
            gridAttachments.ItemsSource = listclsAccessories;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
