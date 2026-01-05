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
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.HarnessPart.Class;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Bars;
using Emdep.Geos.Modules.HarnessPart;
using Emdep.Geos.Modules.HarnessPart.Report;
using DevExpress.Xpf.Printing;
using DevExpress.XtraReports.UI;
using System.Drawing;
using System.Collections.ObjectModel;
using System.Reflection;
using DevExpress.XtraPrinting;
//using DevExpress.DataAccess.ConnectionParameters;
//using DevExpress.DataAccess.Sql;
using System.Configuration;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using DevExpress.XtraReports.UI.PivotGrid;
using DevExpress.XtraPivotGrid;
using System.ComponentModel;
using Emdep.Geos.Modules.HarnessPart.Views;
using DevExpress.Xpf.Ribbon;
using DevExpress.Utils;
using System.IO;



namespace Workbranch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UserControl
    {

        string _getLocationName;
        AddSampleManagment objaddsample;
        ucSampleManagment _objSampleManagment;
       
        public ucSampleManagment ObjSampleManagment
        {
            get { return _objSampleManagment; }
            set { _objSampleManagment = value; }
        }
        private clsGrid objclsGrid;

        public clsGrid ObjclsGrid
        {
            get { return objclsGrid; }
            set { objclsGrid = value; }
        }
        public MainWindow()
        {

            //"MetropolisDark";



            //Theme theme1 = new Theme("BlackWithBlue", "DevExpress.Xpf.Themes.BlackWithBlue.v15.1");
            //theme1.AssemblyName = "DevExpress.Xpf.Themes.BlackWithBlue.v15.1";
            //Theme.RegisterTheme(theme1);
            //Theme theme2 = new Theme("colorBlue", "DevExpress.Xpf.Themes.colorBlue.v15.1");
            //theme2.AssemblyName = "DevExpress.Xpf.Themes.colorBlue.v15.1";
            //Theme.RegisterTheme(theme2);
            ThemeManager.ApplicationThemeName = "BlackAndBlue";
            InitializeComponent();


            ElementRegistrator.GlobalSkipUniquenessCheck = true;

            CommandBinding cb = new CommandBinding(MyCommand, HelpExecuted, HelpCanExecute);
            MyCommandButton.Command = MyCommand;
            KeyGesture kg = new KeyGesture(Key.M, ModifierKeys.Control);
            InputBinding ib = new InputBinding(MyCommand, kg);
            this.InputBindings.Add(ib);

            CommandBinding cb1 = new CommandBinding(MyCommand1, ResultExecuted, ResultCanExecute);
            this.CommandBindings.Add(cb1);
            MyCommandButton1.Command = MyCommand1;
            this.CommandBindings.Add(cb);

            CommandBinding cb2 = new CommandBinding(MyCommand2, HelpExecutedSample, HelpCanExecute);

            this.CommandBindings.Add(cb2);
            MyCommandButton3.Command = MyCommand2;
            this.CommandBindings.Add(cb);

            CommandBinding cb3 = new CommandBinding(MyCommand3, HelpExecutedExcel, HelpCanExecute);

            this.CommandBindings.Add(cb3);
            MyCommandButton4.Command = MyCommand3;
            this.CommandBindings.Add(cb);

        }
        Emdep.Geos.Modules.HarnessPart.Views.ucGridView ucgridview = new Emdep.Geos.Modules.HarnessPart.Views.ucGridView();



        public static RoutedCommand MyCommand = new RoutedCommand();
        public static RoutedCommand MyCommand1 = new RoutedCommand();
        public static RoutedCommand MyCommand2 = new RoutedCommand();
        public static RoutedCommand MyCommand3 = new RoutedCommand();
        private bool _helpCanExecute = true;



        //ucMainConnector1
        void btnsearch_Click(object sender, RoutedEventArgs e)
        {

            List<clsGrid> f = new List<clsGrid>();
            ucgridview.dgvsearch.ItemsSource = null;
            ucgridview.dgvsearch.DataSource = new List<clsGrid>() {
                new clsGrid { ReferenceS= "EPN150900", Cavities= 13,  Type= "Connector Standard", Color= "Black", Gender= "Female", Saled= "sealed", Mylocation= "L17", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con1.jpg"),Isduplication=true},
                 new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "Connector Standard", Color= "White", Gender= "Male", Saled= "sealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/NotAvailable.png"),Isduplication=true  },
                  new clsGrid { ReferenceS= "EPN150900", Cavities= 0,  Type= "Clip Abrazadera", Color= "Transparent", Gender= "---", Saled= "sealed", Mylocation= "K11", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con3.jpg"),Isduplication=false  },
                   new clsGrid { ReferenceS= "EPN150900", Cavities= 2,  Type= "conn", Color= "Blue", Gender= "Male", Saled= "sealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con4.jpg"),Isduplication=true  },
                    new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "conn", Color= "Pink", Gender= "Male", Saled= "unsealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con5.jpg"),Isduplication=false  },
                     new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "conn", Color= "Green", Gender= "Male", Saled= "sealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con6.jpg"), Isduplication=false },
                     new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "conn", Color= "Red", Gender= "Male", Saled= "unsealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con1.jpg"),Isduplication=false  },
                 new clsGrid { ReferenceS= "EPN150900", Cavities= 5,  Type= "conn", Color= "Yellow", Gender= "Male", Saled= "sealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con2.jpg"), Isduplication=true },
                  new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "conn", Color= "black", Gender= "Male", Saled= "sealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con3.jpg"),Isduplication=false  },
                   new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "conn", Color= "black", Gender= "Male", Saled= "unsealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con4.jpg"),Isduplication=true  },
                    new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "conn", Color= "black", Gender= "Male", Saled= "sealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con5.jpg"),Isduplication=false  },
                     new clsGrid { ReferenceS= "EPN150900", Cavities= 4,  Type= "conn", Color= "black", Gender= "Male", Saled= "sealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con6.jpg"),Isduplication=true  },
               
            };
            ucgridview.dgvsearch1.DataSource = new List<clsGrid>() {
                new clsGrid { Reference= "EPN50403", Cavities= 14,  Type= "conn", Color= "Red", Gender= "Male", Saled= "sealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con1.jpg") },
                 new clsGrid { Reference= "EPN50403", Cavities= 2,  Type= "conn", Color= "Green", Gender= "Male", Saled= "sealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con2.jpg") },
                  new clsGrid { Reference= "EPN50403", Cavities= 5,  Type= "conn", Color= "Blue", Gender= "Male", Saled= "sealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con3.jpg") },
                   new clsGrid { Reference= "EPN50403", Cavities= 6,  Type= "conn", Color= "Pink", Gender= "Male", Saled= "sealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con4.jpg") },
                    new clsGrid { Reference= "EPN50403", Cavities= 4,  Type= "conn", Color= "Black", Gender= "Male", Saled= "sealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con5.jpg") },
                     new clsGrid { Reference= "EPN50403", Cavities= 4,  Type= "conn", Color= "White", Gender= "Male", Saled= "sealed", Mylocation= "W443", Partner= "EPN050503", Dimendions1= 111, ConImage1 = GetImage("/Image/con6.jpg") },
              
            };
          
            ucgridview.dgvsearch.PreviewMouseDown += new MouseButtonEventHandler(dgvsearch_PreviewMouseDown);
            ucgridview.dgvsearch.CurrentItemChanged += new CurrentItemChangedEventHandler(dgvsearch_CurrentItemChanged);
            ucgridview.tableView1.FocusedRowChanged += tableView1_FocusedRowChanged;
         
            ResultSearch.Content = ucgridview;
        }

        private void tableView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            //DataViewBase view = ucgridview.dgvsearch.View;
            //object row = ucgridview.dgvsearch.GetRow(ucgridview.dgvsearch.GetSelectedRowHandles()[0]);


            //GridColumn column = ucgridview.dgvsearch.Columns[0];
            //Style oldStyle = column.CellStyle;

            //int selectedRowhandle = ucgridview.dgvsearch.View.GetSelectedRowHandles()[0];
            
            //ucgridview.dgvsearch.Columns[0].CellStyle = new System.Windows.Style();

            //if (selectedRowhandle > 0)
            //{
            //   // ucgridview.dgvsearch.Columns[8].CellTemplate = FindResource("FieldValuegrid") as DataTemplate;
           
            //}
            //FieldCellTemplateSelector CellTemplate 
           // ucgridview.dgvsearch.Columns[8].CellStyle = ((Style)FindResource("backDemoButtonStylegrid"));
           // ucgridview.dgvsearch.Columns[2].CellStyle = ((Style)FindResource("CellStyle12"));
            //column.CellStyle = ucgridview.dgvsearch.FindResource("FieldValueTemplate") as DataTemplate;
            //Style newStyle = new Style(typeof(GridRowContent), oldStyle);
            //newStyle.Setters.Add(new Setter(CellContentPresenter.BackgroundProperty, System.Windows.Media.Brushes.Yellow));
            //column.CellStyle = new System.Windows.Style();
            //column.CellStyle = newStyle;
           // ObjclsGrid.Isduplication = true;
            //((Emdep.Geos.Modules.HarnessPart.Class.clsGrid)(ucgridview.dgvsearch.GetFocusedRow())).referenceS	"EPN150900"	string
            //if (ucgridview.dgvsearch.Columns[0].a.FieldName.Equals("ReferenceS"))
            //{

            //}
           // GridCellContentPresenter cell = (GridCellContentPresenter)sender;
            
        
        }
        //private void gridViewExcel_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        //{
        //    GridView View = sender as GridView;
        //    string firstName = View.GetRowCellDisplayText(e.RowHandle, View.Columns["FirstName"]);

        //    if (firstName = "Sharp")
        //    {
        //        e.Appearance.BackColor = Color.Salmon;
        //        e.Appearance.BackColor2 = Color.White;
        //    }
        //    else
        //    {
        //        //I want to append another column in the end to the dataset that is bound to the grid.
        //        //With an error message...(see below)
        //    }
        //}
        void dgvsearch_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            if (e.OldItem != null && e.OldItem.Equals(e.NewItem))
            {
                return;
            }
            //{Emdep.Geos.Modules.HarnessPart.Class.clsGrid}
            if (e.OldItem.Equals("referenceS"))
            {
            }


        }



        void dgvsearch_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            //throw new NotImplementedException();
            TableViewHitInfo hit = ucgridview.tableView1.CalcHitInfo(e.OriginalSource as DependencyObject);
            if (hit.InRowCell)
            {

                ucgridview.dgvsearch.View.FocusedRowHandle = hit.RowHandle;

                if (e.ClickCount == 1)
                {
                    if (e.XButton1 == MouseButtonState.Released)
                    {

                        if (hit.Column.FieldName == "ReferenceS")
                        {
                            var row = ucgridview.dgvsearch.GetFocusedRow();
                            DXTabItem tabItem1 = new DXTabItem();
                            tabItem1.Header = ((Emdep.Geos.Modules.HarnessPart.Class.clsGrid)(row)).ReferenceS.ToString();
                            tabItem1.FontFamily = new System.Windows.Media.FontFamily("calibri");
                            tabItem1.FontSize = 14;
                            tabItem1.Content = new Emdep.Geos.Modules.HarnessPart.Views.ucMainConnector();
                            dxtabcontrol.Items.Add(tabItem1);

                        }
                        if (e.RightButton == MouseButtonState.Pressed)
                        {
                            if (hit.Column.FieldName == "ConImage1")
                            {
                                ucgridview.dgvsearch.View.FocusedRowHandle = hit.RowHandle;
                                var row = ucgridview.dgvsearch.GetFocusedRow();
                                MessageBox.Show("Image");


                            }
                        }
                    }
                }

            }
        }

        private void HelpCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {


            ResultSearch.Content = new Emdep.Geos.Modules.HarnessPart.Views.ucThumbnailImage();
            e.Handled = true;
        }

        private void HelpExecutedSample(object sender, ExecutedRoutedEventArgs e)
        {
            DXTabItem tabitemsample = new DXTabItem();
            tabitemsample.Header = "SampleManagment";
            tabitemsample.FontFamily = new System.Windows.Media.FontFamily("calibri");
            tabitemsample.FontSize = 14;
            tabitemsample.Content = new Emdep.Geos.Modules.HarnessPart.Views.ucSampleManagment();
            dxtabcontrol.Items.Add(tabitemsample);
            e.Handled = true;
        }
        private void HelpExecutedExcel(object sender, ExecutedRoutedEventArgs e)
        {
            DXTabItem tabitemsample = new DXTabItem();
           
            tabitemsample.Header = "SampleManagment";
            tabitemsample.FontFamily = new System.Windows.Media.FontFamily("calibri");
            tabitemsample.FontSize = 14;
            tabitemsample.Content = new Emdep.Geos.Modules.HarnessPart.Views.ucSampleManagment();
            dxtabcontrol.Items.Add(tabitemsample);
            e.Handled = true;
        }
        private void ResultCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void ResultExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            btnsearch_Click(sender, e);
            e.Handled = true;
        }
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        private void CloseAll_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            dxtabcontrol.Items.Clear();
        }

        private void CloseAllButThis_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var item = ((sender as BarButtonItem).Parent as PopupMenu).PlacementTarget;
            for (int i = 0; i < dxtabcontrol.Items.Count; i++)
            {
                if (((DevExpress.Xpf.Core.DXTabItem)(dxtabcontrol.Items[i])).Header.ToString() != "Home" && dxtabcontrol.Items[i] != dxtabcontrol.SelectedItem)
                {
                    dxtabcontrol.Items.RemoveAt(i--);
                }
            }


        }

        private void dxtabcontrol_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void BarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            Attachement wind = new Attachement();
            wind.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void BarButtonItem_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            XtraReport1 report = new XtraReport1();
            ObservableCollection<clsGrid> listgrid1 = new ObservableCollection<clsGrid>();
            listgrid1.Add(new clsGrid() { Reference = "50403", Refclient = "7287-5157-30 7YBL-1365 7287-1465-30", Connector = "Connector", Cavities = 4, Type = "conn", Color = "black", Gender = "Male", Saled = "Saled", Mylocation = "W443", Partner = "EPN050503", Dimendions1 = 111, ConImage1 = GetImage("/Image/con1.jpg"), Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con1, Hyperlink = "50403" });
            listgrid1.Add(new clsGrid() { Reference = "50403", Refclient = "7287-5157-30 7YBL-1365 7287-1465-31", Connector = "clip", Cavities = 4, Type = "conn", Color = "black", Gender = "Male", Saled = "saled", Mylocation = "W443", Partner = "EPN050503", Dimendions1 = 111, ConImage1 = GetImage("/Image/con2.jpg"), Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con12, Hyperlink = "50404" });
            listgrid1.Add(new clsGrid() { Reference = "50403", Connector = "clip", Cavities = 14, Type = "conn", Color = "RED", Gender = "Male", Saled = "saled", Mylocation = "W443", Partner = "EPN050503", Dimendions1 = 111, ConImage1 = GetImage("/Image/con3.jpg"), Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con3, Hyperlink = "50401" });
            listgrid1.Add(new clsGrid() { Reference = "50403", Connector = "Connector", Cavities = 5, Type = "conn", Color = "Pink", Gender = "Male", Saled = "saled", Mylocation = "W443", Partner = "EPN050503", Dimendions1 = 111, ConImage1 = GetImage("/Image/con4.jpg"), Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con4, Hyperlink = "50408" });
            listgrid1.Add(new clsGrid() { Reference = "50403", Refclient = "7247-5920 7123-5912-30 7183-2129-30", Connector = "clip", Cavities = 2, Type = "conn", Color = "black", Gender = "Female", Saled = "Saled", Mylocation = "W443", Partner = "EPN050503", Dimendions1 = 111, ConImage1 = GetImage("/Image/con5.jpg"), Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con5, Hyperlink = "50409" });
            listgrid1.Add(new clsGrid() { Reference = "50403", Refclient = "7247-5920 7123-5912-30 7183-2129-30", Connector = "clip", Cavities = 4, Type = "conn", Color = "black", Gender = "Female", Saled = "Saled", Mylocation = "W443", Partner = "EPN050503", Dimendions1 = 111, ConImage1 = GetImage("/Image/con6.jpg"), Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con6, Hyperlink = "50504" });

            listgrid1.Add(new clsGrid() { Reference = "50403", Connector = "Antenna", Cavities = 5, Type = "conn", Color = "Blue", Gender = "Male", Saled = "saled", Mylocation = "W443", Partner = "EPN050503", Dimendions1 = 111, ConImage1 = GetImage("/Image/con4.jpg"), Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con4, Hyperlink = "50408" });
            listgrid1.Add(new clsGrid() { Reference = "50403", Refclient = "7247-5920 7123-5912-30 7183-2129-30", Connector = "Antenna", Cavities = 2, Type = "conn", Color = "black", Gender = "Female", Saled = "Saled", Mylocation = "W443", Partner = "EPN050503", Dimendions1 = 111, ConImage1 = GetImage("/Image/con5.jpg"), Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con5, Hyperlink = "50409" });
            listgrid1.Add(new clsGrid() { Reference = "50403", Refclient = "7247-5920 7123-5912-30 7183-2129-30", Connector = "Antenna", Cavities = 4, Type = "conn", Color = "black", Gender = "Female", Saled = "Saled", Mylocation = "W443", Partner = "EPN050503", Dimendions1 = 111, ConImage1 = GetImage("/Image/con6.jpg"), Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con6, Hyperlink = "50504" });

            listgrid1.Add(new clsGrid() { Reference = "50403", Connector = "Bracket", Cavities = 5, Type = "conn", Color = "Yellow", Gender = "Male", Saled = "saled", Mylocation = "W443", Partner = "EPN050503", Dimendions1 = 111, ConImage1 = GetImage("/Image/con4.jpg"), Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con4, Hyperlink = "50408" });
            listgrid1.Add(new clsGrid() { Reference = "50403", Refclient = "7247-5920 7123-5912-30 7183-2129-30", Connector = "Bracket", Cavities = 2, Type = "conn", Color = "black", Gender = "Female", Saled = "Saled", Mylocation = "W443", Partner = "EPN050503", Dimendions1 = 111, ConImage1 = GetImage("/Image/con5.jpg"), Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con5, Hyperlink = "50409" });
            listgrid1.Add(new clsGrid() { Reference = "50403", Refclient = "7247-5920 7123-5912-30 7183-2129-30", Connector = "Bracket", Cavities = 4, Type = "conn", Color = "black", Gender = "Female", Saled = "Saled", Mylocation = "W443", Partner = "EPN050503", Dimendions1 = 111, ConImage1 = GetImage("/Image/con6.jpg"), Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con6, Hyperlink = "50504" });

            report.DataSource = listgrid1;

            PrintHelper.ShowPrintPreview(this, report);

        }


        //private XtraReport CreateReport()
        //{
        //    // Create a blank report.

        //    XtraReport crossTabReport = new XtraReport();

        //    // Create a detail band and add it to the report.
        //    //DetailBand detail = new DetailBand();
        //    ReportHeaderBand detail = new ReportHeaderBand();
        //    crossTabReport.Bands.Add(detail);

        //    // Create a pivot grid and add it to the Detail band.
        //    XRPivotGrid pivotGrid = new XRPivotGrid();
        //    detail.Controls.Add(pivotGrid);
        //    //DISTINCT
        //    string selectSQL = "SELECT DISTINCT  t.Name as templatename,  c.Name as cptype,detectiontypes.Name as Sections," +
        //   " d.Name as DetectionTypeName,de.Quantity , dw.IdDrawing FROM detectionsbydrawing de inner join detections d on de.iddetection = d.iddetection" +
        //    " INNER JOIN detectiontypes  ON detectiontypes.IdDetectionType = d.IdDetectionType " +
        //    "inner join drawings dw ON dw.IdDrawing = de.idDrawing " +
        //    " inner join    cptypes c on dw.idcptype = c.idcptype " +
        //    "  inner join templates t on dw.IdTemplate = t.IdTemplate " +
        //     " inner join drawingsbyconnector dbc on  dbc.IdDrawing = dw.IdDrawing " +
        //      " inner join connectors conn on  conn.IdConnector=  dbc.IdConnector " +
        //      " where  conn.Ref = 050036  ORDER BY detectiontypes.IdDetectionType";


        //    MySqlConnection con = new MySqlConnection("Data Source=10.0.9.44;Database=geos;User ID=GeosUsr;Password=GEOS");
        //    con.Close();
        //    MySqlCommand cmd = new MySqlCommand(selectSQL, con);
        //    MySqlDataAdapter projectAdagpter = new MySqlDataAdapter(cmd);
        //    con.Open();
        //    DataSet myNewDataSet = new DataSet();
        //    var dtCounts = new DataTable();
        //    projectAdagpter.Fill(dtCounts);
        //    projectAdagpter.Fill(myNewDataSet, "PIStaff");
        //    con.Close();
        //    crossTabReport.DataSource = dtCounts;
        //    crossTabReport.DataMember = "dtCounts";





        //   //// DataTable table = new DataTable();
        //   //// table.Columns.Add("templatename", typeof(string));
        //   //// table.Columns.Add("cptype", typeof(string));
        //   //// table.Columns.Add("Sections", typeof(string));
        //   //// table.Columns.Add("DetectionTypeName", typeof(string));
        //   //// table.Columns.Add("Quantity", typeof(int));
        //   //// table.Columns.Add("IdDrawing", typeof(int));


        //   //// // Here we add five DataRows.
        //   //// table.Rows.Add("PNEUMATIC", "Pushback", "Way", "Super Push back", 10, 294915);
        //   //// table.Rows.Add("PNEUMATIC", "Pushback", "Way", "Standard", 10, 291579);
        //   //// table.Rows.Add("PNEUMATIC", "Fixed Block", "Way", "Standard", 10, 294304);
        //   //// table.Rows.Add("PNEUMATIC", "Pushback", "Way", "Push Back", 10, 300362);
        //   //// table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Spacer Closed V", 1, 263433);
        //   //// table.Rows.Add("ASSEMBLY AND CONTROL", "Connector Assembly and Control", "Detection", "Presence V", 1, 223272);
        //   //// table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Spacer Closed V", 1, 200003);
        //   //// table.Rows.Add("ASSEMBLY AND CONTROL", "Connector Assembly and Control", "Detection",	"Presence V", 1, 280766);
        //   //// table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Open V", 1, 273215);
        //   //// table.Rows.Add("HYBRID","Hybrid", "Detection", "Presence V", 1, 238324);
        //   //// table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Open V", 1, 208906);
        //   //// table.Rows.Add("ASSEMBLY AND CONTROL", "Connector Assembly and Control", "Detection", "Presence H", 1, 298768);
        //   //// table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Open V", 1, 291579);
        //   //// table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Latch V", 1, 273215);
        //   //// table.Rows.Add("PNEUMATIC", "Zero Force", "Detection", "Latch H", 1, 238863);
        //   //// table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Closed V", 1, 300362);
        //   //// table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Spacer Closed V", 1, 275130);
        //   //// table.Rows.Add("PNEUMATIC", "Zero Force", "Detection", "Spacer Closed V", 1, 269939);
        //   //// table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Latch V", 1, 200003);
        //   //// table.Rows.Add("ASSEMBLY AND CONTROL", "Connector Assembly and Control", "Detection", "Presence V", 1, 298763);
        //   //// table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Spacer Closed V", 1, 294304);
        //   //// table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Closed V", 1, 273215);
        //   //// table.Rows.Add("PNEUMATIC", "	Zero Force", "	Detection", "	Spacer Closed V	", 1, 238863);
        //   //// table.Rows.Add("PNEUMATIC	", "Pushback", "	Detection", "	Spacer Closed V	", 1, 208906);
        //   //// table.Rows.Add("PNEUMATIC	", "Pushback", "	Detection", "	Spacer Open V", 1, 300362);
        //   //// table.Rows.Add("PNEUMATIC	", "Pushback", "	Detection", "	Spacer Closed V	", 1, 294915);
        //   //// table.Rows.Add("PNEUMATIC", "	Pushback", "	Detection", "	Spacer Closed V	", 1, 291579);
        //   //// table.Rows.Add("PNEUMATIC", "	Pushback", "	Option", "	Spacer Closing Unit V", 1, 273215);
        //   //// table.Rows.Add("PNEUMATIC", "	Pushback", "	Option", "	Spacer Closing Unit V", 1, 300362);
        //   //// table.Rows.Add("PNEUMATIC", "	Pushback", "	Option", "	Spacer Closing Unit V", 1, 208906);
        //   //// table.Rows.Add("PNEUMATIC", "	Pushback", "	Option", "	Spacer Closing Unit V", 1, 294915);
        //   //// table.Rows.Add("PNEUMATIC", "	Pushback", "	Option", "	Spacer Closing Unit V", 1, 291579);
        //   //// table.Rows.Add("COMPONENT", "	Spare", "	Spare Part", "	Pins alignement plate", 1, 232542);
        //   //// table.Rows.Add("COMPONENT", "	Spare", "	Spare Part", "	Sustandur Block	", 1, 282300);
        //   //// //table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
        //   //// //table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
        //   //// //table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
        //   //// //table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);

        //   ////// var dtCounts = new DataTable();
        //   //// DataSet projectAdagpter = new DataSet();
        //   //// projectAdagpter.Tables.Add(table);
        //   //// // projectAdagpter.Fill(myNewDataSet, "PIStaff");

        //   //// crossTabReport.DataSource = projectAdagpter;
        //   //// crossTabReport.DataMember = "table";
        //    pivotGrid.DataSource = dtCounts;
        //    pivotGrid.DataMember = "dtCounts";
        //    pivotGrid.OptionsView.ShowColumnHeaders = false;
        //    pivotGrid.OptionsView.ShowRowHeaders = false;
        //    pivotGrid.OptionsView.ShowColumnGrandTotals = false;
        //    pivotGrid.OptionsView.ShowRowGrandTotals = false;
        //    pivotGrid.OptionsView.ShowColumnTotals = false;
        //    pivotGrid.OptionsView.ShowDataHeaders = false;
        //    pivotGrid.OptionsView.ShowRowGrandTotalHeader = false;
        //    pivotGrid.OptionsView.HideAllTotals();
        //    pivotGrid.BestFit();
        //    // Generate pivot grid's fields.
        //    XRPivotGridField fieldtemplatename = new XRPivotGridField("templatename", PivotArea.RowArea);
        //    XRPivotGridField fieldcptype = new XRPivotGridField("cptype", PivotArea.RowArea);
        //    XRPivotGridField fieldIdDrawing = new XRPivotGridField("IdDrawing", PivotArea.RowArea);

        //    //fieldIdDrawing.MinWidth = 0;


        //    XRPivotGridField fieldDetectionTypeName = new XRPivotGridField("DetectionTypeName", PivotArea.ColumnArea);
        //    XRPivotGridField fieldSections = new XRPivotGridField("Sections", PivotArea.ColumnArea);
        //    XRPivotGridField fieldQuantity = new XRPivotGridField("Quantity", PivotArea.DataArea);
        //    pivotGrid.CustomCellDisplayText += pivotGrid_CustomCellDisplayText;

        //    fieldtemplatename.Width = 100;
        //    fieldtemplatename.RowValueLineCount = 2;
        //    fieldQuantity.Width = 20;
        //    fieldIdDrawing.Width = 0;
        //    fieldIdDrawing.MinWidth = 0;
        //    fieldIdDrawing.Width = 0;
        //    fieldDetectionTypeName.Width = 20;
        //    fieldcptype.Width = 70;
        //    fieldcptype.RowValueLineCount = 2;
        //    fieldSections.Width = 40;
        //    fieldSections.ColumnValueLineCount = 3;
        //    fieldSections.RowValueLineCount = 3;
        //    fieldDetectionTypeName.ColumnValueLineCount = 10;
        //    fieldQuantity.CellFormat.FormatType = DevExpress.Utils.FormatType.Custom;



        //    pivotGrid.Fields.AddRange(new XRPivotGridField[] {fieldtemplatename, fieldcptype,fieldIdDrawing, fieldSections,         
        //    fieldDetectionTypeName, fieldQuantity});
        //    pivotGrid.PrintFieldValue += pivotGrid_PrintFieldValue;
        //    pivotGrid.CustomCellValue += pivotGrid_CustomCellValue;         
        //    PivotCellBaseEventArgs cellInfo = pivotGrid.GetCellInfo(0, 0);


        //    return crossTabReport;
        //}
        private XtraReport CreateReport()
        {
            // Create a blank report.

            XtraReport crossTabReport = new XtraReport();

            // Create a detail band and add it to the report.
            //DetailBand detail = new DetailBand();
            ReportHeaderBand detail = new ReportHeaderBand();
            crossTabReport.Bands.Add(detail);

            // Create a pivot grid and add it to the Detail band.
            XRPivotGrid pivotGrid = new XRPivotGrid();
            detail.Controls.Add(pivotGrid);
            //DISTINCT
            string selectSQL = "SELECT DISTINCT  t.Name as templatename,  c.Name as cptype,detectiontypes.Name as Sections," +
           " d.Name as DetectionTypeName,de.Quantity , dw.IdDrawing FROM detectionsbydrawing de inner join detections d on de.iddetection = d.iddetection" +
            " INNER JOIN detectiontypes  ON detectiontypes.IdDetectionType = d.IdDetectionType " +
            "inner join drawings dw ON dw.IdDrawing = de.idDrawing " +
            " inner join    cptypes c on dw.idcptype = c.idcptype " +
            "  inner join templates t on dw.IdTemplate = t.IdTemplate " +
             " inner join drawingsbyconnector dbc on  dbc.IdDrawing = dw.IdDrawing " +
              " inner join connectors conn on  conn.IdConnector=  dbc.IdConnector " +
              " where  conn.Ref = 050036  ORDER BY detectiontypes.IdDetectionType";


            MySqlConnection con = new MySqlConnection("Data Source=10.0.9.40;Database=geos;User ID=GeosUsr;Password=GEOS");
            con.Close();
            con.Open();
            MySqlCommand cmd = new MySqlCommand(selectSQL, con);
            MySqlDataAdapter projectAdagpter = new MySqlDataAdapter(cmd);


            DataSet dset = new DataSet(); //Creating instance of DataSet
            projectAdagpter.Fill(dset, "student_detail");

            //  DataSet myNewDataSet = new DataSet();
            //  DataTable dtCounts = new DataTable();
            //// projectAdagpter.Fill(dtCounts);
            //  projectAdagpter.Fill(myNewDataSet, "dtCounts");
            con.Close();
            crossTabReport.DataSource = dset.Tables["student_detail"];
            crossTabReport.DataMember = "student_detail";
            // Bind the pivot grid to data.
            pivotGrid.DataSource = dset.Tables["student_detail"];
            pivotGrid.DataMember = "student_detail";
            pivotGrid.OptionsView.ShowColumnHeaders = false;
            pivotGrid.OptionsView.ShowRowHeaders = false;
            pivotGrid.OptionsView.ShowColumnGrandTotals = false;
            pivotGrid.OptionsView.ShowRowGrandTotals = false;
            pivotGrid.OptionsView.ShowColumnTotals = false;
            pivotGrid.OptionsView.ShowDataHeaders = false;
            pivotGrid.OptionsView.ShowRowGrandTotalHeader = false;
            pivotGrid.OptionsView.HideAllTotals();
            pivotGrid.BestFit();
            // Generate pivot grid's fields.
            XRPivotGridField fieldtemplatename = new XRPivotGridField("templatename", PivotArea.RowArea);
            XRPivotGridField fieldcptype = new XRPivotGridField("cptype", PivotArea.RowArea);
            XRPivotGridField fieldIdDrawing = new XRPivotGridField("IdDrawing", PivotArea.RowArea);

            //fieldIdDrawing.MinWidth = 0;


            XRPivotGridField fieldDetectionTypeName = new XRPivotGridField("DetectionTypeName", PivotArea.ColumnArea);
            XRPivotGridField fieldSections = new XRPivotGridField("Sections", PivotArea.ColumnArea);
            XRPivotGridField fieldQuantity = new XRPivotGridField("Quantity", PivotArea.DataArea);
            pivotGrid.CustomCellDisplayText += pivotGrid_CustomCellDisplayText;

            fieldtemplatename.Width = 100;
            fieldtemplatename.RowValueLineCount = 2;
            fieldQuantity.Width = 20;
            fieldIdDrawing.Width = 0;
            fieldIdDrawing.MinWidth = 0;
            fieldIdDrawing.Width = 0;
            fieldDetectionTypeName.Width = 20;
            fieldcptype.Width = 70;
            fieldcptype.RowValueLineCount = 2;
            fieldSections.Width = 40;
            fieldSections.ColumnValueLineCount = 3;
            fieldSections.RowValueLineCount = 3;
            fieldDetectionTypeName.ColumnValueLineCount = 10;
            fieldQuantity.CellFormat.FormatType = DevExpress.Utils.FormatType.Custom;



            pivotGrid.Fields.AddRange(new XRPivotGridField[] {fieldtemplatename, fieldcptype,fieldIdDrawing, fieldSections,         
            fieldDetectionTypeName, fieldQuantity});
            pivotGrid.PrintFieldValue += pivotGrid_PrintFieldValue;
            pivotGrid.CustomCellValue += pivotGrid_CustomCellValue;
            PivotCellBaseEventArgs cellInfo = pivotGrid.GetCellInfo(0, 0);


            return crossTabReport;
        }

        void pivotGrid_CustomCellDisplayText(object sender, PivotCellDisplayTextEventArgs e)
        {

        }

        void pivotGrid_CustomCellValue(object sender, PivotCellValueEventArgs e)
        {
            //if(e.DataField) 
            e.ColumnField.Appearance.FieldHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            e.ColumnField.Appearance.FieldHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            e.DataField.Appearance.Cell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            e.DataField.Appearance.Cell.TextVerticalAlignment = DevExpress.Utils.VertAlignment.Center;
            if (e.DataField.FieldName.Equals("Quantity"))
            {
                e.GetFieldValue(e.ColumnField);
                var g = e.GetCellValue(e.ColumnIndex, e.RowIndex);


                if (e.Value != null)
                {
                    //MessageBox.Show(e.GetPrevColumnCellValue(e.DataField));
                    e.GetFieldValue(e.ColumnField).ToString();
                    e.Value.ToString();
                    // MessageBox.Show(e.GetPrevColumnCellValue(e.DataField).ToString());
                    e.GetColumnFields();
                    string str = e.GetFieldValue(e.GetColumnFields()[0], e.ColumnIndex).ToString();
                    // MessageBox.Show(e.GetFieldValue(e.GetColumnFields()[0],e.RowIndex).ToString());
                    if (str == "Option")
                    {
                        if (e.Value.ToString() == "1")
                        {
                            e.Value = "●";

                        }
                    }

                }

            }
        }

        private XtraReport FindFixtureFeatures(object emdepCode)
        {
            XtraReport crossTabReport = new XtraReport();

            // Create a detail band and add it to the report.
            //DetailBand detail = new DetailBand();
            ReportHeaderBand detail = new ReportHeaderBand();
            crossTabReport.Bands.Add(detail);

            // Create a pivot grid and add it to the Detail band.
            XRPivotGrid pivotGrid = new XRPivotGrid();
            detail.Controls.Add(pivotGrid);
            string lang = "_en";
            MySqlConnection con = new MySqlConnection("Data Source=10.0.9.7;Database=geos;User ID=GeosUsr;Password=GEOS");
            // MySqlCommand cmd = new MySqlCommand(selectSQL, con);
            //  MySqlDataAdapter projectAdagpter = new MySqlDataAdapter(cmd);
            con.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            // Searching for all detections of the given connector.
            DataTable detections = new DataTable();
            adapter.SelectCommand.CommandText = "SELECT IFNULL(CONCAT(', SUM(IF(detection=\"',detection,'\", tmp.quantity, 0)) AS `',detection,'`'), '') AS expr FROM (SELECT det.name" + lang + " AS detection FROM connectors con INNER JOIN drawingsbyconnector dbc ON con.idconnector=dbc.idconnector LEFT JOIN detectionsbydrawing dbd ON dbc.iddrawing=dbd.iddrawing INNER JOIN drawings d ON dbc.iddrawing=d.iddrawing INNER JOIN cptypes c ON d.idcptype=c.idcptype INNER JOIN templates t ON d.idtemplate=t.idtemplate LEFT JOIN detections det ON dbd.iddetection=det.iddetection WHERE con.ref='" + emdepCode.ToString() + "' AND d.isobsolete=0 GROUP BY det.iddetection ORDER BY det.iddetection) AS tmp;";
            adapter.Fill(detections);

            DataTable fixtureFeatures = new DataTable();
            adapter.SelectCommand.CommandText = "SELECT tmp.template AS '', tmp.cptype As ''";

            // Building the next query based on the results of the previous query.
            foreach (DataRow r in detections.Rows)
                if (!r["expr"].ToString().Equals(""))
                    adapter.SelectCommand.CommandText += r["expr"].ToString();

            // Searching for the fixture features and detections quantities.
            adapter.SelectCommand.CommandText += ", IFNULL(tmp.comments, '') AS '' FROM (SELECT d.iddrawing, d.comments, IFNULL(dbd.emdepcomment, '') AS emdepcomment, t.idtemplate, c.idcptype, t.name" + lang + " AS template, c.name" + lang + " AS CPtype, det.name" + lang + " AS detection, SUM(dbd.quantity) AS quantity FROM connectors con INNER JOIN drawingsbyconnector dbc ON con.idconnector=dbc.idconnector LEFT JOIN detectionsbydrawing dbd ON dbc.iddrawing=dbd.iddrawing INNER JOIN drawings d ON dbc.iddrawing=d.iddrawing INNER JOIN cptypes c ON d.idcptype=c.idcptype INNER JOIN templates t ON d.idtemplate=t.idtemplate LEFT JOIN detections det ON dbd.iddetection=det.iddetection WHERE con.ref='" + emdepCode.ToString() + "' AND d.isobsolete=0 GROUP BY d.iddrawing, c.idcptype, det.iddetection) AS tmp GROUP BY tmp.iddrawing, tmp.template, tmp.cptype ORDER BY tmp.idtemplate, tmp.idcptype, tmp.iddrawing;";
            adapter.Fill(fixtureFeatures);

            // Getting the detections' comments.
            DataTable emdepComments = new DataTable();
            adapter.SelectCommand.CommandText = adapter.SelectCommand.CommandText.Replace("SUM(IF", "GROUP_CONCAT(IF");
            adapter.SelectCommand.CommandText = adapter.SelectCommand.CommandText.Replace("tmp.quantity, 0)) AS", "tmp.emdepcomment, '') SEPARATOR '') AS");
            adapter.Fill(emdepComments);
            crossTabReport.DataSource = fixtureFeatures;
            crossTabReport.DataMember = "tblProjects";

            // Bind the pivot grid to data.
            pivotGrid.DataSource = fixtureFeatures;
            pivotGrid.DataMember = "tblProjects";
            pivotGrid.OptionsView.ShowColumnHeaders = false;
            pivotGrid.OptionsView.ShowRowHeaders = false;
            pivotGrid.OptionsView.ShowColumnGrandTotals = false;
            pivotGrid.OptionsView.ShowRowGrandTotals = false;
            pivotGrid.OptionsView.ShowColumnTotals = false;
            pivotGrid.OptionsView.ShowDataHeaders = false;
            pivotGrid.OptionsView.ShowRowGrandTotalHeader = false;
            pivotGrid.OptionsView.HideAllTotals();
            // Generate pivot grid's fields.
            XRPivotGridField fieldtemplatename = new XRPivotGridField("templatename", PivotArea.RowArea);
            XRPivotGridField fieldcptype = new XRPivotGridField("cptype", PivotArea.RowArea);
            XRPivotGridField fieldDetectionTypeName = new XRPivotGridField("DetectionTypeName", PivotArea.ColumnArea);
            XRPivotGridField fieldSections = new XRPivotGridField("Sections", PivotArea.ColumnArea);
            XRPivotGridField fieldQuantity = new XRPivotGridField("Quantity", PivotArea.DataArea);
            //fieldQuantity.Width = 50;
            //XRPivotGridField fieldtype = new XRPivotGridField("Type", PivotArea.DataArea);
            //XRPivotGridField fieldExtendedPrice = new XRPivotGridField("Extended Price", PivotArea.DataArea);
            // Add these fields to the pivot grid.


            fieldtemplatename.Width = 150;
            fieldtemplatename.RowValueLineCount = 2;
            fieldQuantity.Width = 40;
            fieldDetectionTypeName.Width = 50;
            fieldcptype.Width = 150;
            fieldcptype.RowValueLineCount = 2;
            fieldDetectionTypeName.ColumnValueLineCount = 5;
            pivotGrid.Fields.AddRange(new XRPivotGridField[] {fieldtemplatename, fieldcptype, fieldSections,         
            fieldDetectionTypeName, fieldQuantity});
            pivotGrid.PrintFieldValue += pivotGrid_PrintFieldValue;

            //if (multiView.ActiveViewIndex == 1)
            //{
            //    // For the view 1 (connector's details)

            //    fixtureFeaturesTable.DataSource = fixtureFeatures.DefaultView;
            //    fixtureFeaturesTable.DataBind();

            //    emdepCommentsTable.DataSource = emdepComments.DefaultView;
            //    emdepCommentsTable.DataBind();
            //}
            //else
            //{
            //    // For the view 2 (partner's details)

            //    fixtureFeaturesTable2.DataSource = fixtureFeatures.DefaultView;
            //    fixtureFeaturesTable2.DataBind();

            //    emdepCommentsTable2.DataSource = emdepComments.DefaultView;
            //    emdepCommentsTable2.DataBind();
            //}
            return crossTabReport;
        }
        void pivotGrid_PrintFieldValue(object sender, CustomExportFieldValueEventArgs e)
        {
            if (e.Field != null && e.Field.FieldName == "DetectionTypeName")
            //if (e.Field != null && e.Field.Area == DevExpress.XtraPivotGrid.PivotArea.ColumnArea)
            {

                TextBrick tb = e.Brick as TextBrick;

                if (tb.PrintingSystem != null)
                {
                    // LabelBrick lb = tb.PrintingSystem.CreateBrick("LabelBrick") as LabelBrick;
                    LabelBrick lb = new LabelBrick();
                    lb = tb.PrintingSystem.CreateBrick("LabelBrick") as LabelBrick;
                    lb.Padding = new PaddingInfo(0, 0, 0, 0, GraphicsUnit.Pixel);
                    lb.Angle = 90;
                    lb.Text = e.Text;
                    lb.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
                    lb.VertAlignment = DevExpress.Utils.VertAlignment.Center;
                    lb.Rect = GraphicsUnitConverter.DocToPixel(e.Brick.Rect);
                    tb.IsVisible = false;
                    tb.PrintingSystem.Graph.DrawBrick(lb);
                }


            }

            e.Field.Appearance.FieldHeader.WordWrap = true;
            e.Field.Appearance.FieldHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            e.Field.Appearance.FieldHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            e.Field.Appearance.FieldHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            e.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            e.Appearance.WordWrap = true;
            //e.DataField.Appearance.FieldValue.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            //e.DataField.Appearance.FieldValue.TextVerticalAlignment = DevExpress.Utils.VertAlignment.Center;
            e.DataField.Appearance.Cell.TextHorizontalAlignment = DevExpress.Utils.HorzAlignment.Center;
            e.DataField.Appearance.Cell.TextVerticalAlignment = DevExpress.Utils.VertAlignment.Center;
            if (e.Field != null && e.Field.FieldName == "IdDrawing")
            {
                //e.Field.MinWidth = 0;
                //e.Field.Width = 0;

                //
            }
            if (e.Field != null && e.Field.FieldName == "Sections")
            {

                if (e.Value.Equals("Option") && e.DataField.FieldName == "Quantity")
                {
                    if (e.Field.FieldName == "DetectionTypeName")
                    {
                        //if(e.DataField.ToString() =="")
                        TextBrick tb = e.Brick as TextBrick;
                        tb.Font = new Font("Wingdings", 6);
                        //tb.Text = "l";
                    }
                    //Font = new Font("Arial", 24,FontStyle.Bold);
                }
            }
            if (e.DataField.FieldName == "Quantity")
            {

                // MessageBox.Show(e.Value.ToString());
            }


        }

        private void BarButtonItem_ItemClick_2(object sender, ItemClickEventArgs e)
        {
            XtraReport3 report3 = new XtraReport3();
            ObservableCollection<clsGrid> listgrid = new ObservableCollection<clsGrid>();
            listgrid.Add(new clsGrid() { Reference = "50403", Refclient = "7287-5157-30 7YBL-1365 7287-1465-30", Connector = "Connector", Cavities = 4, Type = "conn", Color = "black", Gender = "Male", Saled = "Saled", Mylocation = "W443", Partner = "EPN050503", Dimendions1 = 111, ConImage1 = GetImage("/Image/con1.jpg"), Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con1, Hyperlink = "50403" });
            report3.DataSource = listgrid;
            //report3.xrPivotGrid1.DataSource = listgrid;
            // clsAccessories objclsAccessories = new clsAccessories();
            ObservableCollection<clsAccessories> listobjclsAccessories = new ObservableCollection<clsAccessories>();
            listobjclsAccessories.Add(new clsAccessories() { Colorname = "Red", Reference = "7287-5157-30" });
            listobjclsAccessories.Add(new clsAccessories() { Colorname = "Black", Reference = "7YBL-1365" });
            listobjclsAccessories.Add(new clsAccessories() { Colorname = "Black", Reference = "7287-1465-30" });
            listobjclsAccessories.Add(new clsAccessories() { Colorname = "Black", Reference = "364874" });
            //objclsAccessories.Colorname = "REd";

            XtraReport4 report4 = new XtraReport4();
            report4.bindingSource2.DataSource = listgrid;
            report4.bindingSource1.DataSource = listobjclsAccessories;
            //detailReport = report3.Bands[BandKind.Detail].FindControl("report4", true) as XRSubreport;
            report3.xrSubreport1.ReportSource = report4;



            ObservableCollection<clsOtherReference> listclsOtherReference = new ObservableCollection<clsOtherReference>();
            listclsOtherReference.Add(new clsOtherReference() { Reference = "33501075", Empresa = " DELPHI", });
            listclsOtherReference.Add(new clsOtherReference() { Reference = "DH2950", Empresa = " DELPHI", });
            listclsOtherReference.Add(new clsOtherReference() { Reference = "15470597", Empresa = " DELPHI", });
            listclsOtherReference.Add(new clsOtherReference() { Reference = "33501072", Empresa = " DELPHI", });
            listclsOtherReference.Add(new clsOtherReference() { Reference = "DH2627", Empresa = " DELPHI", });
            listclsOtherReference.Add(new clsOtherReference() { Reference = "DH7666-01", Empresa = " DELPHI", });
            listclsOtherReference.Add(new clsOtherReference() { Reference = "2202010131", Empresa = " DELPHI", });
            listclsOtherReference.Add(new clsOtherReference() { Reference = "6S65-144489-AA", Empresa = " DELPHI", });
            rptOtherReference objOtherReference = new rptOtherReference();
            objOtherReference.DataSource = listclsOtherReference;
            report3.xrSubreport4.ReportSource = objOtherReference;


            ObservableCollection<clsTestProbes> listclsTestProbes = new ObservableCollection<clsTestProbes>();
            listclsTestProbes.Add(new clsTestProbes() { Reference = " 02P201GFs1", Type = "Microswitch", Function = "Spare Closed H", Feinmetallreference = "F15092.00556.2354", Ptrreference = "XXX-ERET.ert" });
            listclsTestProbes.Add(new clsTestProbes() { Reference = " 02L291806565", Type = "Microswitch", Function = "Spare Closed H", Feinmetallreference = "F15092.00556.2354", Ptrreference = "XXX-ERET.ert" });
            listclsTestProbes.Add(new clsTestProbes() { Reference = " 02MSS", Type = "Microswitch", Function = "Spare Closed H", Feinmetallreference = "F15092.00556.2354", Ptrreference = "XXX-ERET.ert" });
            listclsTestProbes.Add(new clsTestProbes() { Reference = " 02P15GC1", Type = "Microswitch", Function = "Spare Closed H", Feinmetallreference = "F15092.00556.2354", Ptrreference = "XXX-ERET.ert" });
            listclsTestProbes.Add(new clsTestProbes() { Reference = " 02P201GC1", Type = "Microswitch", Function = "Spare Closed H", Feinmetallreference = "F15092.00556.2354", Ptrreference = "XXX-ERET.ert" });
            rptTestProbes objrptTestProbes = new rptTestProbes();
            objrptTestProbes.DataSource = listclsTestProbes;
            report3.xrSubreport5.ReportSource = objrptTestProbes;


            ObservableCollection<clsGrid> listConnectorImages = new ObservableCollection<clsGrid>();
            listConnectorImages.Add(new clsGrid() { Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con1 });
            listConnectorImages.Add(new clsGrid() { Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con12 });
            listConnectorImages.Add(new clsGrid() { Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con3 });
            listConnectorImages.Add(new clsGrid() { Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con4 });
            listConnectorImages.Add(new clsGrid() { Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con5 });
            listConnectorImages.Add(new clsGrid() { Img = Emdep.Geos.Modules.HarnessPart.Properties.Resources.con6 });
            rptConnectorImages objrptConnectorImages = new rptConnectorImages();
            objrptConnectorImages.DataSource = listConnectorImages;
            report3.xrSubreport3.ReportSource = objrptConnectorImages;


            rptTextFixtures objrptTextFixtures = new rptTextFixtures();

            objrptTextFixtures.xrSubreporttext.ReportSource = CreateReport();

            // Show its Print Preview.
            //report1.ShowPreview();
            //PrintHelper.ShowPrintPreview(this, report3);
            report3.xrSubreport2.ReportSource = objrptTextFixtures;
            PrintHelper.ShowPrintPreview(this, report3);
            //ReportPrintTool tool = new ReportPrintTool(report3);
            //tool.ShowPreviewDialog();


        }

        private void BarButtonItem_ItemClick_3(object sender, ItemClickEventArgs e)
        {
            if (dxtabcontrol.SelectedIndex >= 2)
            {
                DXTabItem tb = (DXTabItem)dxtabcontrol.Items[dxtabcontrol.SelectedIndex];
                ucMainConnector s = (ucMainConnector)tb.Content;
                s.Info.IsEdit = true;
            }

        }

        //private void BarButtonItem_ItemClick_4(object sender, ItemClickEventArgs e)
        //{
        //    SampleManagment wind = new SampleManagment();
        //    wind.Show();
        //}

        private void BarButtonItem_ItemClick_5(object sender, ItemClickEventArgs e)
        {

        }
        //ImageSource GetImage(string path)
        //{
        //    return new BitmapImage(new Uri(path, UriKind.Relative));
        //}
        private void BarButtonItem_ItemClick_4(object sender, ItemClickEventArgs e)
        {



        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
        void cv1_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            _getLocationName += "." + objaddsample.cv1.Text;
        }

        void cv_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            _getLocationName = objaddsample.cv.Text;
            //ObjSampleManagment.ObjclsStorageLocations.Add(new clsStorageLocations { LocationName = _getLocationName });
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            var result = new ObservableCollection<clsStorageLocationLanes>();
            result.Add(new clsStorageLocationLanes() { LocationInfo = _getLocationName });
            ObjSampleManagment.ObjclsStorageLocations.Add(new clsStorageLocations { LocationName = _getLocationName, ListTorageLocationLanes = result });
            // ObjSampleManagment.ObjclsStorageLocations[ObjSampleManagment.ObjclsStorageLocations.Count].ListTorageLocationLanes[0].LocationInfo = "";
            objaddsample.Hide();
        }
        protected virtual void OnThemeDropDownGalleryInit(object sender, DropDownGalleryEventArgs e)
        {
            Gallery gallery = e.DropDownGallery.Gallery;
            gallery.AllowHoverImages = false;
            gallery.IsItemCaptionVisible = true;
            gallery.ItemGlyphLocation = Dock.Top;
            gallery.IsGroupCaptionVisible = DefaultBoolean.True;
        }
        protected virtual void OnThemeItemClick(object sender, GalleryItemEventArgs e)
        {
            string themeName = (string)e.Item.Caption;
            ThemeManager.SetThemeName(this, themeName);

            ThemeManager.ApplicationThemeName = themeName;


        }

        private void dxtabcontrol_SelectionChanged(object sender, TabControlSelectionChangedEventArgs e)
        {

        }

        private void MyCommandButton4_ItemClick(object sender, ItemClickEventArgs e)
        {
            DXTabItem tb = (DXTabItem)dxtabcontrol.Items[dxtabcontrol.SelectedIndex];

            if (tb.Header.ToString() == "SampleManagment")
            {
                ObjSampleManagment = (ucSampleManagment)tb.Content;
                objaddsample = new AddSampleManagment();
                string pt = @"E:\grid1.xls";
                DevExpress.Xpf.Grid.GridControl gridControl1 = new DevExpress.Xpf.Grid.GridControl();
                gridControl1.AutoGenerateColumns = AutoGenerateColumnsMode.AddNew;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("LocationName", typeof(string));
                dataTable.Columns.Add("LocationInfo", typeof(string));
                dataTable.Columns.Add("Reference", typeof(string));
                dataTable.Columns.Add("Cavities", typeof(string));
                dataTable.Columns.Add("Qty", typeof(string));
                dataTable.Columns.Add("VisualAid", typeof(System.Drawing.Image));

                for (int i = 0; i < ObjSampleManagment.ObjclsStorageLocations.Count; i++)
                {
                    for (int j = 0; j < ObjSampleManagment.ObjclsStorageLocations[i].ListTorageLocationLanes.Count; j++)
                    {
                        string defaultFolder = System.AppDomain.CurrentDomain.BaseDirectory;
                        string navigateToFolder = "..\\..\\";
                        string sourceDir = System.IO.Path.Combine(defaultFolder, navigateToFolder);
                        string[] fileEntries = Directory.GetFiles(sourceDir);
                        var uriSource = new Uri(sourceDir + ObjSampleManagment.ObjclsStorageLocations[i].ListTorageLocationLanes[j].ListclsGrid[j].ConImage1.ToString(), UriKind.Relative);
                        string FileName = sourceDir + ObjSampleManagment.ObjclsStorageLocations[i].ListTorageLocationLanes[j].ListclsGrid[j].ConImage1.ToString();
                        System.Drawing.Image img = System.Drawing.Image.FromFile(FileName);
                        Bitmap b = new Bitmap(img, new System.Drawing.Size(25, 100));
                        //Bitmap img = new Bitmap(FileName, new  System.Drawing.Size(20, 20));
                        dataTable.Rows.Add(ObjSampleManagment.ObjclsStorageLocations[i].LocationName,
                        ObjSampleManagment.ObjclsStorageLocations[i].ListTorageLocationLanes[j].LocationInfo, ObjSampleManagment.ObjclsStorageLocations[i].ListTorageLocationLanes[j].ListclsGrid[j].Reference, ObjSampleManagment.ObjclsStorageLocations[i].ListTorageLocationLanes[j].ListclsGrid[j].Cavities, ObjSampleManagment.ObjclsStorageLocations[i].ListTorageLocationLanes[j].Qty, b);
                    }
                }
                gridControl1.ItemsSource = dataTable;
                gridControl1.Columns[4].SetResourceReference(Control.StyleProperty, "ImageColumnPrintingStyle");
                TableView tableView1 = new TableView();
                tableView1.DataContext = dataTable;
                tableView1.AutoWidth = true;
                tableView1.RowMinHeight = 2;
                gridControl1.View = tableView1;
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Value;
                options.ExportMode = DevExpress.XtraPrinting.XlsExportMode.SingleFile;
                ((TableView)gridControl1.View).ExportToXls(@"Sample.xls", new XlsExportOptionsEx() { ExportType = DevExpress.Export.ExportType.WYSIWYG });


            }
        }

        private void NewSample_ItemClick(object sender, ItemClickEventArgs e)
        {
            DXTabItem tb = (DXTabItem)dxtabcontrol.Items[dxtabcontrol.SelectedIndex];

            if (tb.Header.ToString() == "SampleManagment")
            {
                ObjSampleManagment = (ucSampleManagment)tb.Content;
                objaddsample = new AddSampleManagment();
                _getLocationName = string.Empty;
                objaddsample.Show();
                objaddsample.cv.EditValueChanged += cv_EditValueChanged;
                objaddsample.cv1.EditValueChanged += cv1_EditValueChanged;
                objaddsample.btnAccept.Click += btnAccept_Click;

            }

        }

        private void GalleryItem_Click(object sender, EventArgs e)
        {

            ThemeManager.ApplicationThemeName = "BlackAndBlue";
            //GeosLogo.Source = GetImage(@"\Emdep.Geos.Modules.HarnessPart;component\Image\logo.png");
            //CompanyLogo.Source = GetImage(@"\Emdep.Geos.Modules.HarnessPart;component\Image\Emdep_logo.png");
            txtabout.Foreground = System.Windows.Media.Brushes.White;

        }

        private void GalleryItem_Click_1(object sender, EventArgs e)
        {

            ThemeManager.ApplicationThemeName = "colorBlue";
            //GeosLogo.Source = GetImage(@" \Emdep.Geos.Modules.HarnessPart;component\Image\logoBlue.png");
            //CompanyLogo.Source = GetImage(@"\Emdep.Geos.Modules.HarnessPart;component\Image\Emdep_logo_White.png");
            txtabout.Foreground = System.Windows.Media.Brushes.Black;

        }

        private void MyCommandButton5_ItemClick(object sender, ItemClickEventArgs e)
        {
            DXTabItem tabitemsample = new DXTabItem();
            tabitemsample.Header = "Family";
            tabitemsample.Content = new Emdep.Geos.Modules.HarnessPart.Views.ucFamilyManagment();
            dxtabcontrol.Items.Add(tabitemsample);
            e.Handled = true;
        }




    }
}
