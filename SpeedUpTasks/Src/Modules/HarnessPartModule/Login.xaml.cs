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
using System.Xml;
using System.IO;



namespace Emdep.Geos.Modules.HarnessPart
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : System.Windows.Window
    {
        public Login()
        {
            InitializeComponent();
            //Theme theme = new Theme("BlackAndBlue", "DevExpress.Xpf.Themes.BlackAndBlue.v15.1");
            //theme.AssemblyName = "DevExpress.Xpf.Themes.BlackAndBlue.v15.1";
            //Theme.RegisterTheme(theme);
            //ThemeManager.ApplicationThemeName = "BlackAndBlue";
            //Theme theme1 = new Theme("colorBlue", "DevExpress.Xpf.Themes.colorBlue.v15.1");
            //theme1.AssemblyName = "DevExpress.Xpf.Themes.colorBlue.v15.1";
            //Theme.RegisterTheme(theme1);
            //ThemeManager.ApplicationThemeName = "BlackAndBlue";

            XmlDataDocument xmldoc = new XmlDataDocument();
            XmlNodeList xmlnode;
            int i = 0;
            string str = null;
            FileStream fs = new FileStream(@"Employees.xml", FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);
            xmlnode = xmldoc.GetElementsByTagName("Employee");
            for (i = 0; i <= xmlnode.Count - 1; i++)
            {
                str = xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
            }
            fs.Close();
            usr1.btnforgotpass.Click += btnforgotpass_Click;
            usr3.btnBack.Click += btnBack_Click;
            usr3.btnNext.Click += btnNext_Click;
            usr4.btnCancel.Click += btnCancel_Click;
            usr1.btnAccept.Click += btnAccept_Click;
            usr1.btnforgotpass.Click += btnforgotpass_Click;
            Productionusr.KeyUp += Productionusr_KeyUp;
          
            if (str.Contains("0"))
            {


                PageView1.SelectedIndex = 0;
                PageViewItem1.HeaderTemplate = null;
                PageViewItem1.Height = 0;
                PageViewItem2.HeaderTemplate = null;
                PageViewItem2.Height = 0;
                test.HeaderTemplate = null;
                test.Height = 0;
                this.WindowStyle = System.Windows.WindowStyle.ThreeDBorderWindow;
                //this.ShowIcon = false;

            }
            if (str.Contains("1"))
            {
                PageView1.SelectedIndex = 1;
                PageViewItem2.HeaderTemplate = null;
                PageViewItem2.Height = 0;
                PageViewItem1.HeaderTemplate = null;
                PageViewItem1.Height = 0;
                test.HeaderTemplate = null;
                test.Height = 0;
                test1.HeaderTemplate = null;
                test1.Height = 0;
                this.ResizeMode = System.Windows.ResizeMode.NoResize;
                this.WindowStyle = System.Windows.WindowStyle.None;
                // this.WindowState = System.Windows.WindowState.Normal; 
                CanvasExit.Visibility = System.Windows.Visibility.Visible;
               
            }
            if (str.Contains("2"))
            {
                PageView1.SelectedIndex = 0;
                test.HeaderTemplate = null;
                test.Height = 0;
                test1.HeaderTemplate = null;
                test1.Height = 0;
             
            }
        }

      

        void Productionusr_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Escape")
            {
                Close();
            }  
        }

        void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            windowsui objwindowsui = new windowsui();
            objwindowsui.Show();

            this.Close();
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            PageView1.SelectedIndex = 0;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            PageView1.SelectedIndex = 3;
            usr3.imagemail.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            PageView1.SelectedIndex = 0;
            usr3.imagemail.Visibility = System.Windows.Visibility.Hidden;
     
        }

        private void btnforgotpass_Click(object sender, RoutedEventArgs e)
        {
            PageView1.SelectedIndex = 2;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // GetFecha();
            AdminLogin objAdminLogin = new AdminLogin();
            objAdminLogin.Show();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            ProductionLogin objAdminLogin = new ProductionLogin();
            objAdminLogin.ShowDialogWindow();


            //MessageAdornerBox msgb = new MessageAdornerBox("Internet Connection Error",
            //                                    "There was a problem connecting to the internet, would you like to try again?",
            //                                    new SolidColorBrush(Colors.White),
            //                                    new SolidColorBrush(Colors.Black)) { FirstButtonCaption = "Yes", SecondButtonCaption = "No" };
            //if (msgb.ShowDialog() == true)
            //    MessageBox.Show("Office login");
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Image_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {

        }
        //public static string GetFecha()
        //{
        //    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("es-EC");
        //    System.Threading.Thread.CurrentThread.CurrentCulture = culture;

        //    // maldita sea!
        //    string strDate = culture.TextInfo.ToTitleCase(DateTime.Now.ToLongDateString());
        //    string f= strDate.Replace("De", "de");
        //    return f;


        //}
    }
}
