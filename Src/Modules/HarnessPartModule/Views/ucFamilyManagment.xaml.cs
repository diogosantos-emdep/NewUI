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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Emdep.Geos.Modules.HarnessPart.Class;

namespace Emdep.Geos.Modules.HarnessPart.Views
{
    /// <summary>
    /// Interaction logic for ucFamilyManagment.xaml
    /// </summary>
    public partial class ucFamilyManagment : UserControl
    {
        public ucFamilyManagment()
        {
            InitializeComponent();
            fillFamily();
        }

        private void fillFamily()
        {

            clsFamilyInfo storLoc1 = new clsFamilyInfo();
            storLoc1.ListFamilyInfo = new ObservableCollection<clsFamilyGrid>();

            storLoc1.ListFamilyInfo.Add(new clsFamilyGrid() { Reference = "069099", ImageConnector = GetImage("/Image/con2.jpg"), Cavities = 23, ColorName = "Pink" });
            storLoc1.ListFamilyInfo.Add(new clsFamilyGrid() { Reference = "069099", ImageConnector = GetImage("/Image/con2.jpg"), Cavities = 10, ColorName = "Red" });
            storLoc1.ListFamilyInfo.Add(new clsFamilyGrid() { Reference = "069099", ImageConnector = GetImage("/Image/con2.jpg"), Cavities = 13, ColorName = "Black" });
            storLoc1.ListFamilyInfo.Add(new clsFamilyGrid() { Reference = "069099", ImageConnector = GetImage("/Image/con2.jpg"), Cavities = 08, ColorName = "Pink" });
            storLoc1.ListFamilyInfo.Add(new clsFamilyGrid() { Reference = "069099", ImageConnector = GetImage("/Image/con2.jpg"), Cavities = 03, ColorName = "gray" });
            storLoc1.ListFamilyInfo.Add(new clsFamilyGrid() { Reference = "069099", ImageConnector = GetImage("/Image/con2.jpg"), Cavities = 03, ColorName = "gray" });


            clsFamilyMain storLoc2 = new clsFamilyMain();
            storLoc2.ListFamilyMain = new ObservableCollection<clsFamilyInfo>();
            storLoc2.ListFamilyMain.Add(new clsFamilyInfo() { ListfamilyName = "Connector1", ListFamilyInfo = storLoc1.ListFamilyInfo });
            storLoc2.ListFamilyMain.Add(new clsFamilyInfo() { ListfamilyName = "Connector2", ListFamilyInfo = storLoc1.ListFamilyInfo });
            storLoc2.ListFamilyMain.Add(new clsFamilyInfo() { ListfamilyName = "Connector3", ListFamilyInfo = storLoc1.ListFamilyInfo });






            ObservableCollection<clsFamilyMain> listFamilies = new ObservableCollection<clsFamilyMain>();
            listFamilies.Add(new clsFamilyMain() { ConnectorFamilyName = "Connector", ListFamilyMain = storLoc2.ListFamilyMain });
            listFamilies.Add(new clsFamilyMain() { ConnectorFamilyName = "Componet", ListFamilyMain = storLoc2.ListFamilyMain });
            listFamilies.Add(new clsFamilyMain() { ConnectorFamilyName = "Terminal", ListFamilyMain = storLoc2.ListFamilyMain });

           




            this.DataContext = listFamilies;
            
        }

        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        private void MyHyperlink_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
