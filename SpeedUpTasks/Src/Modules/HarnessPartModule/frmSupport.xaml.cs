using System;
using System.Collections.Generic;
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
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using System.IO;
using System.Collections.ObjectModel;
using Emdep.Geos.Modules.HarnessPart.Class;

namespace Emdep.Geos.Modules.HarnessPart
{
    /// <summary>
    /// Interaction logic for frmSupport.xaml
    /// </summary>
    public partial class frmSupport : WinUIDialogWindow
    {
        List<string> listofFile = new List<string>();

        List<object> tags = new List<object>();
        List<object> selectedContacts = new List<object>();
        string filename = "";
        public List<object> Selection
        {
            get
            {
                return selectedContacts;
            }
            set
            {
                selectedContacts = value;
            }
        }
        public frmSupport()
        {
            ThemeManager.ApplicationThemeName = "BlackAndBlue";
            InitializeComponent();
        }

        private void objfrmSupport_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Escape")
            {
                Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        int cnt = 0;
        private void buttonOpenDialog_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".*";
            // dlg.Filter = "Text documents (.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 

            if (result == true)
            {
                // Open document
                filename = dlg.FileName;
                FileInfo flinfo = new FileInfo(filename);
                //listofFile.Add(flinfo.Name);
                //cbWrappedToken.Items.Insert(cbWrappedToken.Items.Count,flinfo.Name);
                //cbWrappedToken.Items.Add(flinfo.Name);

                //cnt++;




                tags.Add(flinfo.Name.ToString());
               
            }
            ObservableCollection<clsType> customers = new ObservableCollection<clsType>();
            for (int i = 0; i < 1; i++)
            {
                  customers.Add(new clsType() { ID = 0, Name = "Reference", Tags = tags });
            }
          
            //customers.Add(new clsType() { ID = 1, Name = "Way" });
            //customers.Add(new clsType() { ID = 2, Name = "Family" });

           // cbWrappedToken.ItemsSource = customers;
            selectedContacts.Add(customers);
            grid.ItemsSource = customers;
            //cbWrappedToken.SelectAllItems();
            // cbWrappedToken.DataContext = listofFile;
        }

    }
}
