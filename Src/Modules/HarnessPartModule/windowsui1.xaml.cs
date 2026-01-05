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
using System.Net;
using DevExpress.Xpf.Map;
using System.Windows.Controls.Primitives;
using Emdep.Geos.Modules.HarnessPart.Views;


namespace Emdep.Geos.Modules.HarnessPart
{
    /// <summary>
    /// Interaction logic for windowsui1.xaml
    /// </summary>
    public partial class windowsui : DXWindow
    {
        public windowsui()
        {
            InitializeComponent();
            ////Theme theme = new Theme("BlackWithBlue", "DevExpress.Xpf.Themes.BlackWithBlue.v15.1");
            ////theme.AssemblyName = "DevExpress.Xpf.Themes.BlackWithBlue.v15.1";
            ////Theme.RegisterTheme(theme);
            ////Theme theme1 = new Theme("colorBlue", "DevExpress.Xpf.Themes.colorBlue.v15.1");
            ////theme1.AssemblyName = "DevExpress.Xpf.Themes.colorBlue.v15.1";
            ////Theme.RegisterTheme(theme1);
            ThemeManager.ApplicationThemeName = "BlackAndBlue";
            //txtDate.Text = System.DateTime.Now.ToString("HH:mm");

            //string hostName = Dns.GetHostName(); // Retrive the Name of HOST

            //// Get the IP
            ////Dns.GetHostByName(hostName).AddressList[0].ToString();
            //IPHostEntry hostname = Dns.GetHostByName(hostName.ToString());
            //IPAddress[] ip = hostname.AddressList;
            //txtip.Text = ip[0].ToString();


        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //popup.PlacementTarget = (Ellipse)sender;
            //popup.Placement = PlacementMode.Bottom;
            //popup.IsOpen = true;
            //EditProfile ep = new EditProfile();
            //ep.ShowDialogWindow();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EmdepSite emdepsite = new EmdepSite();

            emdepsite.ShowDialogWindow();
            if (emdepsite.ActiveItemIndexValue != null)
                sitename.Content = emdepsite.ActiveItemIndexValue.ToString();

            //MapControl map = new MapControl();

            //// Create a layer.
            //ImageTilesLayer layer = new ImageTilesLayer();
            //map.Layers.Add(layer);

            //// Create a data provider.
            //OpenStreetMapDataProvider provider = new OpenStreetMapDataProvider();
            //layer.DataProvider = provider;

            //// Add the map control to the window.
            //this.Content = map;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = true;
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            popup.PlacementTarget = (Image)sender;
            popup.Placement = PlacementMode.Bottom;
            popup.IsOpen = true;

            //     Popup1 popup =
            //new Popup1
            //{
            //    Width = 200,
            //    Height = 20,
            //    Child = new Rectangle { Fill = Brushes.Fuchsia },
            //    PlacementTarget = (Button)sender,
            //    Placement = PlacementMode.Bottom,
            //    IsOpen = true,
            //};
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //EditProfile ep = new EditProfile();
            //ep.ShowDialogWindow();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            frmSetting frmsetting = new frmSetting();
            frmsetting.ShowDialogWindow();
            //Login loginuser = new Login();
            //loginuser.Show();
        }

        private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            popup.PlacementTarget = (Ellipse)sender;
            popup.Placement = PlacementMode.Bottom;
            popup.IsOpen = true;
        }

        private void Image_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Image_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //ucOfficeLogin uc1 = new ucOfficeLogin();
            //uc1.ShowDialogWindow
            frmSupport frmsupport = new frmSupport();
            frmsupport.ShowDialogWindow();

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

        }
    }
}
