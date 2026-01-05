using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

namespace Emdep.Geos.Modules.HarnessPart.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        // public static readonly DependencyProperty SelectedItemProperty =
        //DependencyProperty.Register("SelectedItem", typeof(object), typeof(UserControl),
        //    new PropertyMetadata(new PropertyChangedCallback(OnSelectedItemChanged)));

        public DashboardView()
        {
            InitializeComponent();
            ThemeManager.ApplicationThemeName = "BlackAndBlue";
            txtDate.Text = System.DateTime.Now.ToString("HH:mm");

            string hostName = Dns.GetHostName(); // Retrive the Name of HOST

            // Get the IP
            //Dns.GetHostByName(hostName).AddressList[0].ToString();
            IPHostEntry hostname = Dns.GetHostByName(hostName.ToString());
            IPAddress[] ip = hostname.AddressList;
            txtip.Text = ip[0].ToString();
           // txtip.Text ="255.255.255.255";
            // tillscroll.BringIntoView(); 
        }
        //public class MyTileLayoutControl : TileLayoutControl
        //{
        //    protected override PanelControllerBase CreateController()
        //    {
        //        return new MyTileLayoutController(this);
        //    }
        //}
        //public class MyTileLayoutController : TileLayoutController
        //{
        //    public MyTileLayoutController(IFlowLayoutControl control) : base(control) { }
        //    protected override Rect GetHorzScrollBarBounds()
        //    {
        //        var result = ClientBounds;
        //        result.Y = result.Bottom;
        //        result.Height = HorzScrollBar.DesiredSize.Height;
        //        return result;
        //    }
        //    private void Tile_Click(object sender, EventArgs e)
        //    {
        //        //Mypopup.IsOpen = true;
        //    }

        //    private void Tile_Click_1(object sender, EventArgs e)
        //    {
        //       // tillscroll.BringIntoView();
        //    }


        //    //void OnSelectedItemChanged(object oldValue, object newVaue)
        //    //{
        //    //    if (oldValue is SampleLayoutItem)
        //    //        ((SampleLayoutItem)oldValue).IsSelected = false;
        //    //    if (oldValue is Tile)
        //    //    {
        //    //        var tile = (Tile)oldValue;
        //    //        tile.SetValue(Tile.CalculatedBackgroundProperty, SelectedTileCalculatedBackground);
        //    //        SelectedTileCalculatedBackground = null;
        //    //        tile.Loaded -= OnSelectedTileLoaded;
        //    //    }
        //    //    if (SelectedItem is Tile)
        //    //    {
        //    //        var tile = (Tile)SelectedItem;
        //    //        SelectedTileCalculatedBackground = tile.CalculatedBackground;
        //    //        UpdateSelectedTileBackgroundMask();
        //    //        tile.Loaded += OnSelectedTileLoaded;
        //    //    }
        //    //}
        //    private void Tile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //    {
        //        //Tileharnesspart.Background = GetSelectedTileBackgroundMask();
        //    }

        //    private void Tile_Click_2(object sender, EventArgs e)
        //    {
        //        Process.Start("calc.exe");
        //    }

        //}

        private void Tile_Click_1(object sender, EventArgs e)
        {

        }

        private void Tile_Click_2(object sender, EventArgs e)
        {

        }

        private void Tile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.emdep.com");
        }
    }
}
