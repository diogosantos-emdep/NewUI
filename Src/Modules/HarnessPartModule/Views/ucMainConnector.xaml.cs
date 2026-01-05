using DevExpress.Xpf.PivotGrid;
using DevExpress.XtraPrinting;
using DevExpress.Xpf;
using Emdep.Geos.Modules.HarnessPart.Class;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.IO;
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
using DevExpress.Xpf.PivotGrid.Internal;
using DevExpress.Xpf.Editors;
using System.Collections;

namespace Emdep.Geos.Modules.HarnessPart.Views
{
    /// <summary>
    /// Interaction logic for ucmainconnector.xaml
    /// </summary>
    public partial class ucMainConnector : UserControl
    {
        clsHarnessInfo _Info = new clsHarnessInfo();

        public clsHarnessInfo Info
        {
            get { return _Info; }
            set { _Info = value; }
        }




        public ucMainConnector()
        {
            InitializeComponent();
            // this.DataContext = this;
            //buttonEdit1.Properties.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            //buttonEdit1.Properties.Buttons[0].Image = Image.FromFile("C:\\MyImage.bmp"); 
            lookUpEdit1.ItemsSource = Stuff.GetStuff();
            lgPartNumber1.Height = 470;
            fillgriddoc();
            FillgridTestProbes();
            fillgridTextfeauter2();
            fillgridSample();
            fillgridAccessory();
            fillgridAccessory2();
            fillRelatedparts();
            cmbColors.ItemsSource = comboColorList();
            FillGridFamilies();
            // fillComboType();
            fillLogs();
            //_Info.IsEdit = false;
            this.DataContext = Info;
            
                //20090519111054.wtg
            string file = System.IO.Path.GetFullPath("sample.dxf");
            //  string file = @"E:\Repository\2014-PR2012\analysis\WTG\WTG\20090519111054.wtg";
            // string file = @"E:\Repository\2014-PR2012\analysis\WTG\WTG\20131118151259.wtg";


            try
            {
                string text = File.ReadAllText(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }

            Stream s = new FileStream(file, FileMode.Open, FileAccess.Read);
            //picBox.Image = ReadWTG(s);
            System.Drawing.Image b;
            BitmapImage btd = new BitmapImage();


            b = WTGLoad.ReadWTG(s);
            btd = ToBitmapImage(b);
            img.Source = btd;




            SetPanel();
        }

        private List<clsColor> comboColorList()
        {

            List<clsColor> combocolor = new List<clsColor>();
            combocolor.Add(new clsColor() { ID = 1, HtmlCode = "#FF0000", Name = "Red" });
            combocolor.Add(new clsColor() { ID = 2, HtmlCode = "#00FF00", Name = "Green" });
            combocolor.Add(new clsColor() { ID = 3, HtmlCode = "#0000FF ", Name = "Blue" });
            combocolor.Add(new clsColor() { ID = 4, HtmlCode = "#FFFF00", Name = "Yellow" });
            combocolor.Add(new clsColor() { ID = 5, HtmlCode = "#CCEEFF", Name = "Sky" });
            combocolor.Add(new clsColor() { ID = 6, HtmlCode = "#ADA96E", Name = "Khaki" });
            combocolor.Add(new clsColor() { ID = 7, HtmlCode = "#800080", Name = "Purple" });
            combocolor.Add(new clsColor() { ID = 8, HtmlCode = "#000000", Name = "Black" });
            combocolor.Add(new clsColor() { ID = 9, HtmlCode = "#FFFFFF", Name = "White" });
            combocolor.Add(new clsColor() { ID = 10, HtmlCode = "#C0C0C0", Name = "Silver" });
            combocolor.Add(new clsColor() { ID = 11, HtmlCode = "#FFA500", Name = "Orange" });
            combocolor.Add(new clsColor() { ID = 12, HtmlCode = "#0000A0", Name = "Dark Blue" });
            combocolor.Add(new clsColor() { ID = 13, HtmlCode = "#FFFFCC", Name = "Cream" });

            return combocolor;

        }

        private void btnnew_Click(object sender, RoutedEventArgs e)
        {
            //gridVisabilty
            //Resources["gridVisabilty"] = "string";

            // txtBlock.Text = FindResource("myStr").ToString();  
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            lgPartNumber1.Collapsed += lgPartNumber1_Collapsed;
            lgPartNumber1.Expanded += lgPartNumber1_Expanded;
        }
        public void fillgriddoc()
        {

            clsharnessPartAccessoryTypes obj = new clsharnessPartAccessoryTypes();
            //SelectedCompanyId ="office"SelectedCompanyId ="emdep",
            GridDocumentation.ItemsSource = null;
            List<clsCompany> objlistcompany = new List<clsCompany>()
       {new clsCompany() { Name = "A&C", IdCustomer = "1" },
       new clsCompany() { Name = "A.RAYMOND", IdCustomer = "2" },
       new clsCompany() { Name = "A.S.I", IdCustomer = "3" },
       new clsCompany() { Name = "ACE", IdCustomer = "4" },
       new clsCompany() { Name = "ACIMUT", IdCustomer = "5" },
       new clsCompany() { Name = "ACR COMPONENTES", IdCustomer = "6" },
       new clsCompany() { Name = "ACTS", IdCustomer = "7" },
       new clsCompany() { Name = "ALCOA EES", IdCustomer = "8" }};
            List<clsharnessPartAccessoryTypes> objclsharnessPartAccessoryTypes = new List<clsharnessPartAccessoryTypes>()
       {            new clsharnessPartAccessoryTypes(){IdType=1,TypeName="2D Drawing"},
           new clsharnessPartAccessoryTypes(){IdType=2,TypeName="3D Drawing"},
           new clsharnessPartAccessoryTypes(){IdType=3,TypeName="Specs"}};



            GridDocumentation.DataSource = new List<clsHarnessPartDocuments>() {
                new clsHarnessPartDocuments {  OriginalFileName= "32343243",  Company = objlistcompany,  Description="Test Data ",Type=objclsharnessPartAccessoryTypes},
                new clsHarnessPartDocuments {  OriginalFileName= "32343243",  Company = objlistcompany,  Description="Test Data ",Type=objclsharnessPartAccessoryTypes},
                
            };
        }
        void lgPartNumber1_Expanded(object sender, EventArgs e)
        {
            lgPartNumber1.Height = 470;
        }

        void lgPartNumber1_Collapsed(object sender, EventArgs e)
        {
            lgPartNumber1.Height = Double.NaN;
        }
        public void FillgridTestProbes()
        {
            ObservableCollection<clsTestProbes> listclsTestProbes = new ObservableCollection<clsTestProbes>();
            listclsTestProbes.Add(new clsTestProbes() { Reference = " 02P201GFs1", Function = "Presencia V.spacer" });
            listclsTestProbes.Add(new clsTestProbes() { Reference = " 02L291806565", Function = "[2-8,10-16]" });
            listclsTestProbes.Add(new clsTestProbes() { Reference = " 02MSS", Function = "[17,23]" });
            listclsTestProbes.Add(new clsTestProbes() { Reference = " 02P15GC1", Function = "[17,23]" });
            listclsTestProbes.Add(new clsTestProbes() { Reference = " 02P201GC1", Function = "[1.9.18-22]" });

            GridTestProbes.DataSource = listclsTestProbes;
        }
        public void fillgridTextfeauter()
        {
            //PivotGridControl pivotGridControl1 = new PivotGridControl();
            //TextFixtures.Children.Add(pivotGridControl1);

            pivotGridControl1.DataSource = null;

            //  string selectSQL = "SELECT DISTINCT  t.Name as templatename,  c.Name as cptype,detectiontypes.Name as Sections," +
            //" d.Name as DetectionTypeName,de.Quantity , dw.IdDrawing FROM detectionsbydrawing de inner join detections d on de.iddetection = d.iddetection" +
            // " INNER JOIN detectiontypes  ON detectiontypes.IdDetectionType = d.IdDetectionType " +
            // "inner join drawings dw ON dw.IdDrawing = de.idDrawing " +
            // " inner join    cptypes c on dw.idcptype = c.idcptype " +
            // "  inner join templates t on dw.IdTemplate = t.IdTemplate " +
            //  " inner join drawingsbyconnector dbc on  dbc.IdDrawing = dw.IdDrawing " +
            //   " inner join connectors conn on  conn.IdConnector=  dbc.IdConnector " +
            //   " where  conn.Ref = 050036  ORDER BY detectiontypes.IdDetectionType";
            //  MySqlConnection con = new MySqlConnection("Data Source=10.0.9.7;Database=geos;User ID=GeosUsr;Password=GEOS");
            //  con.Close();
            //  MySqlCommand cmd = new MySqlCommand(selectSQL, con);
            //  MySqlDataAdapter projectAdagpter = new MySqlDataAdapter(cmd);
            //  con.Open();
            //  DataSet myNewDataSet = new DataSet();
            //  var dtCounts = new DataTable();
            // // projectAdagpter.Fill(myNewDataSet, "tblProjects");
            //  projectAdagpter.Fill(dtCounts);
            //  con.Close();
            //  pivotGridControl1.DataSource = dtCounts;
            // myNewDataSet.Tables["tblProjects"];



            DataTable table = new DataTable();
            table.Columns.Add("templatename", typeof(string));
            table.Columns.Add("cptype", typeof(string));
            table.Columns.Add("Sections", typeof(string));
            table.Columns.Add("DetectionTypeName", typeof(string));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("IdDrawing", typeof(int));



            // Here we add five DataRows.
            table.Rows.Add("PNEUMATIC", "Pushback", "Way", "Super Push back", 10, 294915);
            table.Rows.Add("PNEUMATIC", "Pushback", "Way", "Standard", 10, 291579);
            table.Rows.Add("PNEUMATIC", "Fixed Block", "Way", "Standard", 10, 294304);
            table.Rows.Add("PNEUMATIC", "Pushback", "Way", "Push Back", 10, 300362);
            table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Spacer Closed V", 1, 263433);
            table.Rows.Add("ASSEMBLY AND CONTROL", "Connector Assembly and Control", "Detection", "Presence V", 1, 223272);
            table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Spacer Closed V", 1, 200003);
            table.Rows.Add("ASSEMBLY AND CONTROL", "Connector Assembly and Control", "Detection", "Presence V", 1, 280766);
            table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Open V", 1, 273215);
            table.Rows.Add("HYBRID", "Hybrid", "Detection", "Presence V", 1, 238324);
            table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Open V", 1, 208906);
            table.Rows.Add("ASSEMBLY AND CONTROL", "Connector Assembly and Control", "Detection", "Presence H", 1, 298768);
            table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Open V", 1, 291579);
            table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Latch V", 1, 273215);
            table.Rows.Add("PNEUMATIC", "Zero Force", "Detection", "Latch H", 1, 238863);
            table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Closed V", 1, 300362);
            table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Spacer Closed V", 1, 275130);
            table.Rows.Add("PNEUMATIC", "Zero Force", "Detection", "Spacer Closed V", 1, 269939);
            table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Latch V", 1, 200003);
            table.Rows.Add("ASSEMBLY AND CONTROL", "Connector Assembly and Control", "Detection", "Presence V", 1, 298763);
            table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Spacer Closed V", 1, 294304);
            table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Closed V", 1, 273215);
            table.Rows.Add("PNEUMATIC", "Zero Force", "Detection", "Spacer Closed V	", 1, 238863);
            table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Closed V	", 1, 208906);
            table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Open V", 1, 300362);
            table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Closed V	", 1, 294915);
            table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Closed V	", 1, 291579);
            table.Rows.Add("PNEUMATIC", "Pushback", "Option", "Spacer Closing Unit V", 1, 273215);
            table.Rows.Add("PNEUMATIC", "Pushback", "Option", "Spacer Closing Unit V", 1, 300362);
            table.Rows.Add("PNEUMATIC", "Pushback", "Option", "Spacer Closing Unit V", 1, 208906);
            table.Rows.Add("PNEUMATIC", "Pushback", "Option", "Spacer Closing Unit V", 1, 294915);
            table.Rows.Add("PNEUMATIC", "Pushback", "Option", "Spacer Closing Unit V", 1, 291579);
            table.Rows.Add("COMPONENT", "Spare", "Spare Part", "Pins alignement plate", 1, 232542);
            table.Rows.Add("COMPONENT", "Spare", "Spare Part", "Sustandur Block	", 1, 282300);
            table.Rows.Add("PNEUMATIC", "Pushback", "Z", "", 10, 294915);
            //table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
            //table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
            //table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
            //table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);
            pivotGridControl1.DataSource = table;
            pivotGridControl1.ShowColumnHeaders = false;
            pivotGridControl1.ShowRowHeaders = false;
            pivotGridControl1.ShowColumnGrandTotals = false;
            pivotGridControl1.ShowRowGrandTotals = false;
            pivotGridControl1.ShowColumnTotals = false;
            pivotGridControl1.ShowDataHeaders = false;
            pivotGridControl1.ShowRowGrandTotalHeader = false;
            pivotGridControl1.ShowRowTotals = false;
            pivotGridControl1.ExpandValue(false);
            pivotGridControl1.AllowExpand = false;
            pivotGridControl1.AllowSort = false;


            pivotGridControl1.RowTotalsLocation = FieldRowTotalsLocation.Near;


            pivotGridControl1.BestFit();

            PivotGridField fieldtemplatename = new PivotGridField("templatename", FieldArea.RowArea);
            fieldtemplatename.ValueTemplate = pivotGridControl1.FindResource("FieldValueTemplate2") as DataTemplate;
            //e.ColumnField.Appearance.FieldHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            PivotGridField fieldcptype = new PivotGridField("cptype", FieldArea.RowArea);
            fieldcptype.ValueTemplate = pivotGridControl1.FindResource("FieldValueTemplate2") as DataTemplate;
            PivotGridField fieldIdDrawing = new PivotGridField("IdDrawing", FieldArea.RowArea);
            // fieldIdDrawing.Tag = @"E:\Repository\Workbench\Workbench\SrcUI\HarnessPart\Image\con4.JPG";
            fieldIdDrawing.ValueTemplate = pivotGridControl1.FindResource("FieldValueTemplate1") as DataTemplate;
            fieldIdDrawing.Width = 70;

            //fieldIdDrawing.MinWidth = 0;


            PivotGridField fieldDetectionTypeName = new PivotGridField("DetectionTypeName", FieldArea.ColumnArea);
            //PivotGridControl.FieldValueTemplateProperty=t
            PivotGridField fieldSections = new PivotGridField("Sections", FieldArea.ColumnArea);
            PivotGridField fieldQuantity = new PivotGridField("Quantity", FieldArea.DataArea);

            fieldtemplatename.TotalsVisibility = FieldTotalsVisibility.None;

            fieldDetectionTypeName.ValueFormat = VerticalContentAlignment.ToString();
            fieldDetectionTypeName.ValueTemplate = pivotGridControl1.FindResource("FieldValueTemplate") as DataTemplate;
            fieldDetectionTypeName.Height = 150;
            fieldDetectionTypeName.Width = 30;

            // fieldSections.RaiseE. = 10;
            //fieldtemplatename.ShowTotals = true;
            //fieldtemplatename.ShowGrandTotal = true;

            pivotGridControl1.Fields.AddRange(fieldtemplatename, fieldcptype, fieldIdDrawing, fieldSections,
            fieldDetectionTypeName, fieldQuantity);
            pivotGridControl1.MouseEnter += pivotGridControl1_MouseEnter;
            pivotGridControl1.MouseMove += pivotGridControl1_MouseMove;
            pivotGridControl1.CustomUnboundFieldData += pivotGridControl1_CustomUnboundFieldData;
            //pivotGridControl1.CustomValueAppearance += pivotGridControl1_CustomValueAppearance;
            //pivotGridControl1.FieldValueDisplayText+=pivotGridControl1_FieldValueDisplayText;
            //pivotGridControl1.CustomCellDisplayText += pivotGridControl1_CustomCellDisplayText;


            //pivotGridControl1.FieldValueDisplayText += pivotGridControl1_FieldValueDisplayText;   
            //pivotGridControl1.CustomValueAppearance += pivotGridControl1_CustomValueAppearance;


            pivotGridControl1.CustomCellValue += pivotGridControl1_CustomCellValue;
            //pivotGridControl1.EndUpdate();

        }


        public void fillgridTextfeauter2()
        {
            //PivotGridControl pivotGridControl1 = new PivotGridControl();
            //TextFixtures.Children.Add(pivotGridControl1);

            pivotGridControl1.DataSource = null;

            //  string selectSQL = "SELECT DISTINCT  t.Name as templatename,  c.Name as cptype,detectiontypes.Name as Sections," +
            //" d.Name as DetectionTypeName,de.Quantity , dw.IdDrawing FROM detectionsbydrawing de inner join detections d on de.iddetection = d.iddetection" +
            // " INNER JOIN detectiontypes  ON detectiontypes.IdDetectionType = d.IdDetectionType " +
            // "inner join drawings dw ON dw.IdDrawing = de.idDrawing " +
            // " inner join    cptypes c on dw.idcptype = c.idcptype " +
            // "  inner join templates t on dw.IdTemplate = t.IdTemplate " +
            //  " inner join drawingsbyconnector dbc on  dbc.IdDrawing = dw.IdDrawing " +
            //   " inner join connectors conn on  conn.IdConnector=  dbc.IdConnector " +
            //   " where  conn.Ref = 050036  ORDER BY detectiontypes.IdDetectionType";
            //  MySqlConnection con = new MySqlConnection("Data Source=10.0.9.7;Database=geos;User ID=GeosUsr;Password=GEOS");
            //  con.Close();
            //  MySqlCommand cmd = new MySqlCommand(selectSQL, con);
            //  MySqlDataAdapter projectAdagpter = new MySqlDataAdapter(cmd);
            //  con.Open();
            //  DataSet myNewDataSet = new DataSet();
            //  var dtCounts = new DataTable();
            // // projectAdagpter.Fill(myNewDataSet, "tblProjects");
            //  projectAdagpter.Fill(dtCounts);
            //  con.Close();
            //  pivotGridControl1.DataSource = dtCounts;
            // myNewDataSet.Tables["tblProjects"];



            DataTable table = new DataTable();
            table.Columns.Add("templatename", typeof(string));
            table.Columns.Add("cptype", typeof(string));
            table.Columns.Add("Sections", typeof(string));
            //table.Columns.Add("DetectionTypeName", typeof(string));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("IdDrawing", typeof(int));



            // Here we add five DataRows.
            table.Rows.Add("PNEUMATIC", "Fixed Block", "Way",  10, 294304);
            table.Rows.Add("PNEUMATIC", "Fixed Block", "Way", 80, 294304);
            table.Rows.Add("PNEUMATIC", "Fixed Block", "Way1", 20, 294304);
            table.Rows.Add("PNEUMATIC", "Fixed Block", "Way1", 100, 294304);
      
            table.Rows.Add("PNEUMATIC", "Pushback", "Way1",  10, 291579);        
            table.Rows.Add("PNEUMATIC", "Pushback", "Way",  10, 300362);
            //table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Spacer Closed V", 1, 263433);
            //table.Rows.Add("ASSEMBLY AND CONTROL", "Connector Assembly and Control", "Detection", "Presence V", 1, 223272);
            //table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Spacer Closed V", 1, 200003);
            //table.Rows.Add("ASSEMBLY AND CONTROL", "Connector Assembly and Control", "Detection", "Presence V", 1, 280766);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Open V", 1, 273215);
            //table.Rows.Add("HYBRID", "Hybrid", "Detection", "Presence V", 1, 238324);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Open V", 1, 208906);
            //table.Rows.Add("ASSEMBLY AND CONTROL", "Connector Assembly and Control", "Detection", "Presence H", 1, 298768);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Open V", 1, 291579);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Latch V", 1, 273215);
            //table.Rows.Add("PNEUMATIC", "Zero Force", "Detection", "Latch H", 1, 238863);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Closed V", 1, 300362);
            //table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Spacer Closed V", 1, 275130);
            //table.Rows.Add("PNEUMATIC", "Zero Force", "Detection", "Spacer Closed V", 1, 269939);
            //table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Latch V", 1, 200003);
            //table.Rows.Add("ASSEMBLY AND CONTROL", "Connector Assembly and Control", "Detection", "Presence V", 1, 298763);
            //table.Rows.Add("PNEUMATIC", "Fixed Block", "Detection", "Spacer Closed V", 1, 294304);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Closed V", 1, 273215);
            //table.Rows.Add("PNEUMATIC", "Zero Force", "Detection", "Spacer Closed V	", 1, 238863);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Closed V	", 1, 208906);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Open V", 1, 300362);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Closed V	", 1, 294915);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Detection", "Spacer Closed V	", 1, 291579);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Option", "Spacer Closing Unit V", 1, 273215);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Option", "Spacer Closing Unit V", 1, 300362);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Option", "Spacer Closing Unit V", 1, 208906);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Option", "Spacer Closing Unit V", 1, 294915);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Option", "Spacer Closing Unit V", 1, 291579);
            //table.Rows.Add("COMPONENT", "Spare", "Spare Part", "Pins alignement plate", 1, 232542);
            //table.Rows.Add("COMPONENT", "Spare", "Spare Part", "Sustandur Block	", 1, 282300);
            //table.Rows.Add("PNEUMATIC", "Pushback", "Z", "", 10, 294915);
            //table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
            //table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
            //table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
            //table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);
            pivotGridControl1.DataSource = table;
            pivotGridControl1.ShowColumnHeaders = false;
            pivotGridControl1.ShowRowHeaders = false;
            pivotGridControl1.ShowColumnGrandTotals = false;
            pivotGridControl1.ShowRowGrandTotals = false;
            pivotGridControl1.ShowColumnTotals = false;
            pivotGridControl1.ShowDataHeaders = false;
            pivotGridControl1.ShowRowGrandTotalHeader = false;
            pivotGridControl1.ShowRowTotals = false;
            pivotGridControl1.ExpandValue(false);
            pivotGridControl1.AllowExpand = false;
            pivotGridControl1.AllowSort = false;


            pivotGridControl1.RowTotalsLocation = FieldRowTotalsLocation.Near;


            pivotGridControl1.BestFit();

            PivotGridField fieldtemplatename = new PivotGridField("templatename", FieldArea.RowArea);
            fieldtemplatename.ValueTemplate = pivotGridControl1.FindResource("FieldValueTemplate2") as DataTemplate;
            //e.ColumnField.Appearance.FieldHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            PivotGridField fieldcptype = new PivotGridField("cptype", FieldArea.RowArea);
            fieldcptype.ValueTemplate = pivotGridControl1.FindResource("FieldValueTemplate2") as DataTemplate;
            PivotGridField fieldIdDrawing = new PivotGridField("IdDrawing", FieldArea.RowArea);
            // fieldIdDrawing.Tag = @"E:\Repository\Workbench\Workbench\SrcUI\HarnessPart\Image\con4.JPG";
            fieldIdDrawing.ValueTemplate = pivotGridControl1.FindResource("FieldValueTemplate1") as DataTemplate;
            fieldIdDrawing.Width = 70;

            //fieldIdDrawing.MinWidth = 0;


            //PivotGridField fieldDetectionTypeName = new PivotGridField("DetectionTypeName", FieldArea.ColumnArea);
            //PivotGridControl.FieldValueTemplateProperty=t
            PivotGridField fieldSections = new PivotGridField("Sections", FieldArea.ColumnArea);
            PivotGridField fieldQuantity = new PivotGridField("Quantity", FieldArea.DataArea);

            fieldtemplatename.TotalsVisibility = FieldTotalsVisibility.None;

            //fieldDetectionTypeName.ValueFormat = VerticalContentAlignment.ToString();
            ////fieldDetectionTypeName.ValueTemplate = pivotGridControl1.FindResource("FieldValueTemplate") as DataTemplate;
            //fieldDetectionTypeName.Height = 150;
            //fieldDetectionTypeName.Width = 30;

            // fieldSections.RaiseE. = 10;
            //fieldtemplatename.ShowTotals = true;
            //fieldtemplatename.ShowGrandTotal = true;

            pivotGridControl1.Fields.AddRange(fieldtemplatename, fieldcptype, fieldIdDrawing, fieldQuantity, fieldSections
            );
            //pivotGridControl1.MouseEnter += pivotGridControl1_MouseEnter;
            //pivotGridControl1.MouseMove += pivotGridControl1_MouseMove;
            //pivotGridControl1.CustomUnboundFieldData += pivotGridControl1_CustomUnboundFieldData;
            //pivotGridControl1.CustomValueAppearance += pivotGridControl1_CustomValueAppearance;
            //pivotGridControl1.FieldValueDisplayText+=pivotGridControl1_FieldValueDisplayText;
            //pivotGridControl1.CustomCellDisplayText += pivotGridControl1_CustomCellDisplayText;


            //pivotGridControl1.FieldValueDisplayText += pivotGridControl1_FieldValueDisplayText;   
            //pivotGridControl1.CustomValueAppearance += pivotGridControl1_CustomValueAppearance;


           // pivotGridControl1.CustomCellValue += pivotGridControl1_CustomCellValue;
            //pivotGridControl1.EndUpdate();

        }

        private void pivotGridControl1_CustomUnboundFieldData(object sender, PivotCustomFieldDataEventArgs e)
        {

        }

        void pivotGridControl1_MouseMove(object sender, MouseEventArgs e)
        {

            //if (e.ColumnField != null && e.ColumnField.FieldName == "DetectionTypeName")
            //{ }
            //if (e.Source.Equals("DetectionTypeName"))
            //{ }
        }

        void pivotGridControl1_MouseEnter(object sender, MouseEventArgs e)
        {
            //PivotGridField tb = (PivotGridField)sender;
            //if (tb.FieldName.Equals("IdDrawing"))
            //{
            //}
            //if (e.Source.Equals("IdDrawing"))
            //{ }
        }


        public void fillgridSample()
        {
            gridSample.DataSource = null;

            ObservableCollection<clsSample> listclsSample = new ObservableCollection<clsSample>();
            listclsSample.Add(new clsSample() { Qty = 2, Location = "E34", LocationIn = "Spain" });
            listclsSample.Add(new clsSample() { Qty = 4, Location = "T23", LocationIn = "India" });
            listclsSample.Add(new clsSample() { Qty = 5, Location = "y454", LocationIn = "India" });

            gridSample.DataSource = listclsSample;
        }

        public void fillgridAccessory()
        {
            gridACCESSORIES4.DataSource = null;


            List<clsharnessPartAccessoryTypes> objTypes = new List<clsharnessPartAccessoryTypes>()
            { new clsharnessPartAccessoryTypes(){IdType=1,TypeName="FUSE"},
              new clsharnessPartAccessoryTypes(){IdType=2,TypeName="Cable"},
              new clsharnessPartAccessoryTypes(){IdType=3,TypeName="SHUNT"}
            };

            gridACCESSORIES4.DataSource = new List<clsAccessories>() { new clsAccessories() { Type = objTypes, Colorname = "RED", Reference = "32343243" } ,
                                                                       new clsAccessories() { Type = objTypes, Colorname = "YELLOW-", Reference = "2343422" },
                                                                       new clsAccessories() { Type = objTypes, Colorname = "BLUE", Reference = "SDFSDSFS3"}
            };
        }

        public void fillgridAccessory2()
        {
            gridACCESSORIES2.DataSource = null;
            ObservableCollection<clsAccessories> listAccessories = new ObservableCollection<clsAccessories>();
            listAccessories.Add(new clsAccessories { Reference = "32343243", Companyname = "LEAR" });
            listAccessories.Add(new clsAccessories { Reference = "2343422", Companyname = "YAZAKI" });
            listAccessories.Add(new clsAccessories { Reference = "SDFSDSFS3", Companyname = "TCA" });


            List<clsCompany> objCompany = new List<clsCompany>() {new clsCompany() { IdCustomer = "1", Name = "LEAR" },
                                                                          new clsCompany() { IdCustomer = "2", Name = "YAZAKI" },
                                                                          new clsCompany() { IdCustomer = "3", Name = "TCA" } };

            gridACCESSORIES2.DataSource = new List<clsAccessories>() {new clsAccessories {IsShow=true, Reference = "32343243", Company = objCompany },
                                                                      new clsAccessories { IsShow = false, Reference = "2343422", Company = objCompany},
                                                                      new clsAccessories {IsShow=true, Reference = "SDFSDSFS3", Company = objCompany }};
        }


        public void fillComboType()
        {
            ObservableCollection<clsharnessPartAccessoryTypes> lstType = new ObservableCollection<clsharnessPartAccessoryTypes>();
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 1, TypeName = "Antenna" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 2, TypeName = "Bracket" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 3, TypeName = "Channel" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 4, TypeName = "Clip" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 5, TypeName = "Color Mark" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 6, TypeName = "Connector" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 7, TypeName = "Cover" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 8, TypeName = "Fiber Optic" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 9, TypeName = "Fusebox" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 10, TypeName = "Grommet" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 11, TypeName = "Ground-Terminal" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 12, TypeName = "Housing" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 13, TypeName = "Mechanical Component" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 14, TypeName = "Set" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 15, TypeName = "Sponge" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 16, TypeName = "Testboard Component" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 17, TypeName = "Thread" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 18, TypeName = "Tube" });
            lstType.Add(new clsharnessPartAccessoryTypes { IdType = 19, TypeName = "Warehouse Component" });

            //lookUpEdit1.ValueMember = "IdType";
            //cmbType.ItemsSource = lstType;


        }



        //void pivotGridControl1_CustomCellDisplayText(object sender, PivotCellDisplayTextEventArgs e)
        //{
        //    if (e.ColumnField != null && e.ColumnField.FieldName == "DetectionTypeName")
        //    //    ////if (e.Field != null && e.Field.Area == DevExpress.XtraPivotGrid.PivotArea.ColumnArea)
        //    {

        //        TextBrick tb = e.Value as TextBrick;
        //        LabelBrick lb = tb.PrintingSystem.CreateBrick("LabelBrick") as LabelBrick;
        //        lb.Padding = new PaddingInfo(0, 0, 0, 0, GraphicsUnit.Pixel);
        //        lb.Angle = 90;
        //        lb.Text = e.Value.ToString();
        //        lb.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
        //        lb.VertAlignment = DevExpress.Utils.VertAlignment.Center;
        //        // lb.Rect = GraphicsUnitConverter.DocToPixel(e.Value.ToString());
        //        tb.IsVisible = false;
        //        tb.PrintingSystem.Graph.DrawBrick(lb);


        //    }
        //}

        //void pivotGridControl1_FieldValueDisplayText(object sender, PivotFieldDisplayTextEventArgs e)
        //{
        //    if (e.Field != null && e.Field.FieldName == "DetectionTypeName")
        //    //    ////if (e.Field != null && e.Field.Area == DevExpress.XtraPivotGrid.PivotArea.ColumnArea)
        //    {

        //        TextBrick tb = e.Value as TextBrick;
        //        LabelBrick lb = tb.PrintingSystem.CreateBrick("LabelBrick") as LabelBrick;
        //        lb.Padding = new PaddingInfo(0, 0, 0, 0, GraphicsUnit.Pixel);
        //        lb.Angle = 90;
        //        lb.Text = e.Value.ToString();
        //        lb.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
        //        lb.VertAlignment = DevExpress.Utils.VertAlignment.Center;
        //       // lb.Rect = GraphicsUnitConverter.DocToPixel(e.Value.ToString());
        //        tb.IsVisible = false;
        //        tb.PrintingSystem.Graph.DrawBrick(lb);


        //    }
        //}

        //void pivotGridControl1_CustomValueAppearance(object sender, PivotCustomValueAppearanceEventArgs e)
        //{
        //    //if(e.Field!=null && e.RowField.FieldName == "DetectionTypeName")
        //    //{

        //    //}
        //    //if (e.Field != null && e.Field.FieldName == "DetectionTypeName")
        //    ////if (e.Field != null && e.Field.Area == DevExpress.XtraPivotGrid.PivotArea.ColumnArea)
        //    //{

        //    //    TextBrick tb = e.Value as TextBrick;
        //    //    LabelBrick lb = tb.PrintingSystem.CreateBrick("LabelBrick") as LabelBrick;
        //    //    lb.Padding = new PaddingInfo(0, 0, 0, 0, GraphicsUnit.Pixel);
        //    //    lb.Angle = 90;
        //    //    lb.Text = e.Value.ToString();
        //    //    lb.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
        //    //    lb.VertAlignment = DevExpress.Utils.VertAlignment.Center;
        //    //   // lb.Rect = GraphicsUnitConverter.DocToPixel(e.Value.Rect);
        //    //    tb.IsVisible = false;
        //    //    tb.PrintingSystem.Graph.DrawBrick(lb);


        //    //}
        //}

        private void buttonEdit1_DefaultButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hi");
        }

        private void pivotGridControl1_CustomCellValue(object sender, PivotCellValueEventArgs e)
        {
            if (e.DataField.FieldName.Equals("Quantity"))
            {
                e.GetFieldValue(e.ColumnField);
                var g = e.GetCellValue(e.ColumnIndex, e.RowIndex);


                if (e.Value != null)
                {
                    e.GetFieldValue(e.ColumnField).ToString();
                    e.Value.ToString();
                    e.GetColumnFields();
                    string str = e.GetFieldValue(e.GetColumnFields()[0], e.ColumnIndex).ToString();
                    if (str == "Option")
                    {
                        if (e.Value.ToString() == "1")
                        {
                            e.Value = " ●";

                        }


                    }

                    string str1 = e.GetFieldValue(e.GetColumnFields()[1], e.ColumnIndex).ToString();
                    if (str == "Detection")
                    {
                        if (e.ColumnField.ToString() == "Sections")
                        {
                            e.GetCellValue(e.ColumnIndex, e.RowIndex);
                            if (e.Value.ToString() == "1")
                            {
                                e.Value = " ●";

                            }

                            //if (e.Value.ToString() == "1")
                            //{


                            //} 
                        }




                    }
                    if (e.Value.ToString().Equals("269939"))
                    {

                        e.DataField.CellTemplate = pivotGridControl1.FindResource("FieldValueTemplateImg") as DataTemplate;
                        e.Value = e.DataField.CellTemplate;
                    }



                }

            }
        }

        public BitmapImage ToBitmapImage(System.Drawing.Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, image.RawFormat);
            ms.Seek(0, SeekOrigin.Begin);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }
        public void SetPanel()
        {
            //foreach (System.Windows.Forms.Label lba in WTGValues.LabelArrayList.Values)
            {
                foreach (string var in WTGValues.ObjectName)
                {
                    System.Windows.Forms.Panel p = WTGValues.Shapedetails[var];
                    byte b1 = WTGValues.ObjectType[var];
                    int h = WTGValues.Shapedetails[var].Size.Height;
                    int w = WTGValues.Shapedetails[var].Size.Width;
                    int px = p.Location.X;
                    int py = p.Location.Y;
                    if (b1 == 0)
                    {
                        System.Windows.Forms.Label lba = WTGValues.LabelArrayList[var];

                        StackPanel sp = new StackPanel();
                        StackPanel sp1 = new StackPanel();
                        //sp1.Width = 20;
                        //sp1.Height = 20;
                        System.Drawing.Point lbLoc = lba.Location;
                        lba.Location = new System.Drawing.Point(lbLoc.X, lbLoc.Y);
                        // double left = 5, top =5, right = 5, bottom = 5;
                        System.Windows.Shapes.Rectangle EllipseImage = new System.Windows.Shapes.Rectangle();
                        EllipseImage.Width = w;
                        EllipseImage.Height = h;

                        EllipseImage.StrokeThickness = 1;
                        EllipseImage.Stroke = System.Windows.Media.Brushes.Red;

                        //lbl.Margin = new Thickness(left);
                        //sp1.Children.Add(lbl);

                        sp.Height = h;
                        sp.Width = w;

                        Label lbl = new Label();
                        lbl.Content = lba.Text.ToString();
                        lbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        lbl.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        lbl.VerticalContentAlignment = System.Windows.VerticalAlignment.Top;
                        lbl.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
                        lbl.Margin = new Thickness(5, 5, 5, 5);
                        lbl.Height = 23;
                        lbl.Width = 23;
                        lbl.Background = System.Windows.Media.Brushes.White;
                        lbl.Foreground = System.Windows.Media.Brushes.Black;
                        sp1.Children.Add(lbl);

                        sp.Children.Add(EllipseImage);
                        Canvas.SetLeft(sp, px);
                        Canvas.SetTop(sp, py);
                        Canvas.SetLeft(sp1, px);
                        Canvas.SetTop(sp1, py);
                        //g.Children.Add(sp); g.Children.Add(sp1);
                        //teststack.Children.Add(sp);
                        //teststack.Children.Add(sp1);
                        CanvasImage.Children.Add(sp);
                        CanvasImage.Children.Add(sp1);
                    }
                    if (b1 == 1)
                    {
                        System.Windows.Forms.Label lba = WTGValues.LabelArrayList[var];

                        StackPanel sp = new StackPanel();
                        StackPanel sp1 = new StackPanel();
                        //sp1.Width = 20;
                        //sp1.Height = 20;
                        System.Drawing.Point lbLoc = lba.Location;
                        lba.Location = new System.Drawing.Point(lbLoc.X, lbLoc.Y);
                        // double left = 5, top =5, right = 5, bottom = 5;
                        System.Windows.Shapes.Ellipse EllipseImage = new System.Windows.Shapes.Ellipse();
                        EllipseImage.Width = w;
                        EllipseImage.Height = h;

                        EllipseImage.StrokeThickness = 1;
                        EllipseImage.Stroke = System.Windows.Media.Brushes.Red;

                        //lbl.Margin = new Thickness(left);
                        //sp1.Children.Add(lbl);

                        sp.Height = h;
                        sp.Width = w;

                        Label lbl = new Label();
                        lbl.Content = lba.Text.ToString();
                        lbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        lbl.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        lbl.VerticalContentAlignment = System.Windows.VerticalAlignment.Top;
                        lbl.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
                        lbl.Margin = new Thickness(5, 5, 5, 5);
                        lbl.Height = 23;
                        lbl.Width = 23;
                        lbl.Background = System.Windows.Media.Brushes.White;
                        sp1.Children.Add(lbl);

                        sp.Children.Add(EllipseImage);
                        Canvas.SetLeft(sp, px);
                        Canvas.SetTop(sp, py);
                        Canvas.SetLeft(sp1, px);
                        Canvas.SetTop(sp1, py);
                        //g.Children.Add(sp); g.Children.Add(sp1);
                        //teststack.Children.Add(sp);
                        //teststack.Children.Add(sp1);
                        CanvasImage.Children.Add(sp);
                        CanvasImage.Children.Add(sp1);
                    }

                }
            }

        }


        private void fillRelatedparts()
        {


            List<clsColor> objColor = new List<clsColor>();
            objColor = comboColorList();

            ObservableCollection<clsAccessories> listAccessories = new ObservableCollection<clsAccessories>();
            listAccessories.Add(new clsAccessories { Connector = "EPN050340", Cavities = 13, _Color = objColor });
            listAccessories.Add(new clsAccessories { Connector = "EPN050050", Cavities = 12, _Color = objColor });
            listAccessories.Add(new clsAccessories { Connector = "EPN050360", Cavities = 10, _Color = objColor });

            gridACCESSORIES.ItemsSource = listAccessories;
            // gridACCESSORIES.ItemsSource = new List<clsAccessories>() {new clsAccessories{ _Color = objColor }};

            // this.DataContext = ObjclsStorageLocations;
        }


        public static class Stuff
        {
            public static List<clsType> GetStuff()
            {
                List<clsType> stuff = new List<clsType>();
                stuff.Add(new clsType() { ID = 1, ParentID = 0, Name = "Bracket" });
                stuff.Add(new clsType() { ID = 2, ParentID = 0, Name = "Channel" });
                stuff.Add(new clsType() { ID = 3, ParentID = 0, Name = "Clip" });
                stuff.Add(new clsType() { ID = 4, ParentID = 0, Name = "Color Mark" });
                stuff.Add(new clsType() { ID = 5, ParentID = 0, Name = "Connector" });
                stuff.Add(new clsType() { ID = 6, ParentID = 1, Name = "Cover" });
                stuff.Add(new clsType() { ID = 7, ParentID = 1, Name = "Fiber Optic" });
                stuff.Add(new clsType() { ID = 8, ParentID = 1, Name = "Fusebox" });
                stuff.Add(new clsType() { ID = 9, ParentID = 1, Name = "Grommet" });
                stuff.Add(new clsType() { ID = 10, ParentID = 1, Name = "Ground-Terminal" });
                stuff.Add(new clsType() { ID = 11, ParentID = 2, Name = "Housing" });
                stuff.Add(new clsType() { ID = 12, ParentID = 2, Name = "Mechanical Component" });
                stuff.Add(new clsType() { ID = 13, ParentID = 2, Name = "Set" });
                stuff.Add(new clsType() { ID = 14, ParentID = 2, Name = "Sponge" });
                stuff.Add(new clsType() { ID = 15, ParentID = 2, Name = "Testboard Component" });
                stuff.Add(new clsType() { ID = 16, ParentID = 3, Name = "Thread" });
                stuff.Add(new clsType() { ID = 17, ParentID = 3, Name = "Tube" });
                stuff.Add(new clsType() { ID = 18, ParentID = 3, Name = "Warehouse Component" });
                stuff.Add(new clsType() { ID = 19, ParentID = 3, Name = "Battery" });
                stuff.Add(new clsType() { ID = 20, ParentID = 3, Name = "Karen J. Kelly" });

                return stuff;
            }

        }

        private void ColumnDefinition_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

        }

        private void MyHyperlink_Click(object sender, RoutedEventArgs e)
        {
            DXTabItem tabItem1 = new DXTabItem();
            tabItem1.Header = "Test";
            ;
            frmDrawingInformation objfrmDrawingInformation = new frmDrawingInformation();

            objfrmDrawingInformation.Show();

        }
        private void fillLogs()
        {
            //List<clsLogs> lstLog = new List<clsLogs>();
            ObservableCollection<clslogs> lstLog = new ObservableCollection<clslogs>();
            List<clslogs> lstLog1 = new List<clslogs>();
            lstLog.Add(new clslogs() { LogEntryDate = "[3/6/2014 05:25:15]---Carlos Duran ", LogEntry = "Mejorar la silueta del Postizo de Latón" });
            lstLog1.Add(new clslogs() { Listclslogs = lstLog, LogEntry = "Mejorar la silueta del Postizo de Latón" });
            //lstLog.Add(new clslogs() { LogEntry = "[5/7/2014 12:22:23]---Carlos Duran  Mejorar la silueta del Postizo de Latón" });
            //lstLog.Add(new clslogs() { LogEntry = "[6/9/2014 03:12:15]---Carlos Duran  Mejorar la silueta del Postizo de Latón" });
            //lstLog.Add(new clslogs() { LogEntry = "[5/10/2014 04:54:26]---Carlos Duran  Mejorar la silueta del Postizo de Latón" });
            dgvlogs.ItemsSource = lstLog1;
        }
        private void FillGridFamilies()
        {
            ObservableCollection<clsFamilyGrid> listFamilies = new ObservableCollection<clsFamilyGrid>();

            // string path = @"E:\Repository\Workbench\Workbench\SrcUI\HarnessPart\Image\con2.jpg";
            listFamilies.Add(new clsFamilyGrid() { Reference = "EPN069099", ImageConnector = GetImage("/Image/con2.jpg"), Cavities = 23, Connector = true, Component = true, Terminal = true, ColorName = "Pink" });
            listFamilies.Add(new clsFamilyGrid() { Reference = "EPN069099", ImageConnector = GetImage("/Image/con2.jpg"), Cavities = 10, Connector = true, Component = true, Terminal = true, ColorName = "Red" });
            listFamilies.Add(new clsFamilyGrid() { Reference = "EPN069099", ImageConnector = GetImage("/Image/con2.jpg"), Cavities = 13, Connector = true, Component = true, Terminal = true, ColorName = "Black" });
            listFamilies.Add(new clsFamilyGrid() { Reference = "EPN069099", ImageConnector = GetImage("/Image/con2.jpg"), Cavities = 08, Connector = true, Component = true, Terminal = true, ColorName = "Pink" });
            listFamilies.Add(new clsFamilyGrid() { Reference = "EPN069099", ImageConnector = GetImage("/Image/con2.jpg"), Cavities = 03, Connector = true, Component = true, Terminal = true, ColorName = "gray" });
            listFamilies.Add(new clsFamilyGrid() { Reference = "EPN069099", ImageConnector = GetImage("/Image/con2.jpg"), Cavities = 03, Connector = true, Component = true, Terminal = true, ColorName = "gray" });
            listFamilies.Add(new clsFamilyGrid() { Reference = "EPN069099", ImageConnector = GetImage("/Image/con2.jpg"), Cavities = 03, Connector = true, Component = true, Terminal = true, ColorName = "gray" });
            listFamilies.Add(new clsFamilyGrid() { Reference = "EPN069099", ImageConnector = GetImage("/Image/con2.jpg"), Cavities = 03, Connector = true, Component = true, Terminal = true, ColorName = "gray" });

            GridFamilies.ItemsSource = listFamilies;
        }

        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        private void MyHyperlink_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void MyHyperlink_MouseEnter(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("hi");

            //popup.IsOpen = true;
            //cImage.Visibility = Visibility.Visible;

        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            //popup.IsOpen = false;
            //cImage.Visibility = Visibility.Hidden;
        }

        //        private void cmbColors_CustomDisplayText(object sender, DevExpress.Xpf.Editors.CustomDisplayTextEventArgs e)
        //        {
        //            ComboBoxEdit editor = sender as ComboBoxEdit;
        //            cmbColors.ItemsSource = comboColorList();
        //            int ItemsSourceCount = ((IList)editor.ItemsSource).Count;
        //            if (editor.SelectedItems.Count > ItemsSourceCount - 1)
        //            {
        //                if (editor.SelectedItems.IndexOf(e.EditValue) > 0)
        //                    e.DisplayText = string.Empty;
        //                else
        //                    e.DisplayText = "ALL";
        //                editor.SeparatorString = "";
        //                e.Handled = true;
        //            }
        //            else editor.SeparatorString = ";";
        //        }

        //        private void cmbColors_Loaded(object sender, RoutedEventArgs e)
        //        {
        ////cmbColors.ItemsSource = comboColorList();
        //        }


    }


    public class FieldCellTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            CellsAreaItem cell = item as CellsAreaItem;




            if (cell.Value != null && cell.ColumnValueDisplayText.ToString() == "Latch V")
            {

                return OddTemplate;
            }

            if (cell.ColumnIndex == 13 && (cell.RowValue.ToString() == "223272" || cell.RowValue.ToString() == "232542" || cell.RowValue.ToString() == "294915"))
            {

                return ImageTemplate;
            } if (cell.ColumnIndex != 13 && (cell.RowValue.ToString() != "223272" || cell.RowValue.ToString() != "232542" || cell.RowValue.ToString() != "294915"))
            {

                return EvenTemplatecenter;
            }

            //if ((cell.Item.ColumnField).DataControllerColumnName.ToString() == "Sections")
            //{
            //   // MessageBox.Show("Hi");
            //    MessageBox.Show("");
            //    return ImageTemplate;
            //}






            else

                return EvenTemplate;
        }

        public DataTemplate OddTemplate { get; set; }
        public DataTemplate EvenTemplate { get; set; }

        public DataTemplate ImageTemplate { get; set; }
        public DataTemplate EvenTemplatecenter { get; set; }
        public DataTemplate EvenTemplatecenterHeader { get; set; }


    }
}


