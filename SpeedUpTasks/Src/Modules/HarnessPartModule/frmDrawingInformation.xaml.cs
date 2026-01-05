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
using System.Windows.Forms;
using System.Collections.ObjectModel;
using Emdep.Geos.Modules.HarnessPart.Class;
using System.Threading;


namespace Emdep.Geos.Modules.HarnessPart
{
    /// <summary>
    /// Interaction logic for frmDrawingInformation.xaml
    /// </summary>
    public partial class frmDrawingInformation : DXWindow
    {
        AxEModelView.AxEModelViewControl ctrlEdrawings = new AxEModelView.AxEModelViewControl();
        public frmDrawingInformation()
        {
            InitializeComponent();

            fillRevision();
            
            gd1.ColumnDefinitions[0].Width = new GridLength(100, GridUnitType.Star);
            gd1.ColumnDefinitions[1].Width = new GridLength(0);
            //AxEModelView.AxEModelViewControl ctrlEdrawings = new AxEModelView.AxEModelViewControl();
            // FillBOM();
            // ctrlEdrawings = new AxEModelView.AxEModelViewControl();
            // ctrlEdrawings.OnComponentSelectionNotify += new AxEModelView._IEModelViewControlEvents_OnComponentSelectionNotifyEventHandler(this.ctrlEdrawings_OnComponentSelectionNotify);
            // ctrlEdrawings.OnFinishedLoadingDocument += new AxEModelView._IEModelViewControlEvents_OnFinishedLoadingDocumentEventHandler(this.ctrlEdrawings_OnFinishedLoadingDocument);
            // ctrlEdrawings.OnComponentMouseOverNotify += new AxEModelView._IEModelViewControlEvents_OnComponentMouseOverNotifyEventHandler(this.ctrlEdrawings_OnComponentMouseOverNotify);
            // ctrlEdrawings.BackgroundColorGradient = true;
            // ctrlEdrawings.BackgroundColorOverride = true;
            //// string path = @"\\10.0.9.2\rd$\Projects\Solidworks\Drawings\142434\1 - Especial + OTH + COMPONENTE";
            // string path = @"E:\CONECTOR.SLDPRT";
            // //path = String.Format(path, "CONECTOR.SLDPRT");
            // ctrlEdrawings.OpenDoc(path, false, false, false, "");

            //// ctrlEdrawings.ViewOperator = EModelView.EMVOperators.eMVOperatorSelect;

            // ctrlEdrawings.OpenDoc(@"C:\Program Files\SolidWorks Corp\SolidWorks\samples\tutorial\edraw\claw\claw-mechanism.edrw", False, False, False, "")
            opendoc();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            AxEModelView.AxEModelViewControl ctrlEdrawings = new AxEModelView.AxEModelViewControl();
            ctrlEdrawings.OnComponentSelectionNotify += new AxEModelView._IEModelViewControlEvents_OnComponentSelectionNotifyEventHandler(this.ctrlEdrawings_OnComponentSelectionNotify);
            ctrlEdrawings.OnFinishedLoadingDocument += new AxEModelView._IEModelViewControlEvents_OnFinishedLoadingDocumentEventHandler(this.ctrlEdrawings_OnFinishedLoadingDocument);
            ctrlEdrawings.OnComponentMouseOverNotify += new AxEModelView._IEModelViewControlEvents_OnComponentMouseOverNotifyEventHandler(this.ctrlEdrawings_OnComponentMouseOverNotify);
            //ctrlEdrawings.BackgroundColorGradient = true;
            //ctrlEdrawings.BackgroundColorOverride = true;
            string path = System.IO.Path.GetFullPath("CONJUNT.SLDASM");
            //string path = @"\\10.0.9.2\rd$\Projects\Solidworks\Drawings\142434\1 - Especial + OTH + COMPONENTE\CONJUNT.SLDASM";      
            //WindowsFormsHost1.Child = ctrlEdrawings;         
            System.Windows.Controls.Button newBtn = new System.Windows.Controls.Button();
            ctrlEdrawings.CreateControl();
            ctrlEdrawings.OpenDoc(path, false, false, false, "");
        }

        private void ctrlEdrawings_OnComponentSelectionNotify(object sender, AxEModelView._IEModelViewControlEvents_OnComponentSelectionNotifyEvent e)
        {
            // throw new NotImplementedException();
        }

        private void ctrlEdrawings_OnFinishedLoadingDocument(object sender, AxEModelView._IEModelViewControlEvents_OnFinishedLoadingDocumentEvent e)
        {
            // throw new NotImplementedException();
        }

        private void ctrlEdrawings_OnComponentMouseOverNotify(object sender, AxEModelView._IEModelViewControlEvents_OnComponentMouseOverNotifyEvent e)
        {
            //throw new NotImplementedException();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            AxEModelView.AxEModelViewControl ctrlEdrawings = new AxEModelView.AxEModelViewControl();
            ctrlEdrawings.OnComponentSelectionNotify += new AxEModelView._IEModelViewControlEvents_OnComponentSelectionNotifyEventHandler(this.ctrlEdrawings_OnComponentSelectionNotify);
            ctrlEdrawings.OnFinishedLoadingDocument += new AxEModelView._IEModelViewControlEvents_OnFinishedLoadingDocumentEventHandler(this.ctrlEdrawings_OnFinishedLoadingDocument);
            ctrlEdrawings.OnComponentMouseOverNotify += new AxEModelView._IEModelViewControlEvents_OnComponentMouseOverNotifyEventHandler(this.ctrlEdrawings_OnComponentMouseOverNotify);
            //ctrlEdrawings.BackgroundColorGradient = true;
            //ctrlEdrawings.BackgroundColorOverride = true;
            string path = System.IO.Path.GetFullPath("CONECTOR.SLDPRT");
            // string path = @"\\10.0.9.2\rd$\Projects\Solidworks\Drawings\142434\1 - Especial + OTH + COMPONENTE\CONECTOR.SLDPRT";      
            //WindowsFormsHost1.Child = ctrlEdrawings;         
            System.Windows.Controls.Button newBtn = new System.Windows.Controls.Button();
            ctrlEdrawings.CreateControl();
            ctrlEdrawings.OpenDoc(path, false, false, false, "");

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AxEModelView.AxEModelViewControl ctrlEdrawings = new AxEModelView.AxEModelViewControl();
            ctrlEdrawings.OnComponentSelectionNotify += new AxEModelView._IEModelViewControlEvents_OnComponentSelectionNotifyEventHandler(this.ctrlEdrawings_OnComponentSelectionNotify);
            ctrlEdrawings.OnFinishedLoadingDocument += new AxEModelView._IEModelViewControlEvents_OnFinishedLoadingDocumentEventHandler(this.ctrlEdrawings_OnFinishedLoadingDocument);
            ctrlEdrawings.OnComponentMouseOverNotify += new AxEModelView._IEModelViewControlEvents_OnComponentMouseOverNotifyEventHandler(this.ctrlEdrawings_OnComponentMouseOverNotify);
            //ctrlEdrawings.BackgroundColorGradient = true;
            //ctrlEdrawings.BackgroundColorOverride = true;

            string path = System.IO.Path.GetFullPath("PLANOL.SLDDRW");
            // string path = @"\\10.0.9.2\rd$\Projects\Solidworks\Drawings\142434\1 - Especial + OTH + COMPONENTE\PLANOL.SLDDRW";      
            //WindowsFormsHost1.Child = ctrlEdrawings;         
            System.Windows.Controls.Button newBtn = new System.Windows.Controls.Button();
            ctrlEdrawings.CreateControl();
            ctrlEdrawings.OpenDoc(path, false, false, false, "");

        }

        private void FillBOM()
        {

            string path = "";
            ObservableCollection<clsBOM> listclsBOM = new ObservableCollection<clsBOM>();
            clsBOM objclsBOM = new clsBOM();
            listclsBOM.Add(new clsBOM() { Description = "MICRO DETECCIÓN [PRT.- Presencia Via] (1-91) [1,2,3,4,5,6,7,8,9,10,11,", Qty = 0, Reference = "02P202GF801", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = " ->VAINA", Qty = 91, Reference = "02H875KB", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = "MICRO DETECCIÓN [.- Over Pull] [94]", Qty = 1, Reference = "02MSS", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = "MICRO DETECCIÓN [.- Pull STD] [93]", Qty = 1, Reference = "02MSS", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = "MUELLE RODAMIENTO", Qty = 91, Reference = "04M840723", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = "MICRO ACTIVACIÓN ACTIVACIÓN", Qty = 4, Reference = "02P201GFS1", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = "->VAINA", Qty = 1, Reference = "02H875KB", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = "MICRO DETECCIÓN [.- EXTRA-Presence] [92]", Qty = 1, Reference = "02P201GFS1", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = " ->VAINA", Qty = 1, Reference = "02H875KB", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = "LED ENCLAVADO", Qty = 91, Reference = "02LDB3", ImageLoad = GetImage(path) });

            listclsBOM.Add(new clsBOM() { Description = "RESISTENCIA", Qty = 1, Reference = "02RS2K", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = "PULSADOR", Qty = 1, Reference = "02PSR", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = " ->RESISTENCIA", Qty = 1, Reference = "02RS2K", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = "COLUMNA EMPUJE", Qty = 4, Reference = "04C670", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = " ->TUERCA", Qty = 4, Reference = "04TC4Z", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = "RODAMIENTOS", Qty = 4, Reference = "04KH0622B", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = "PRISIONERO INMOVILIZADOR", Qty = 2, Reference = "04PR5X10", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = "CILINDRO SUJECIÓN", Qty = 4, Reference = "03NCS1202", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = " ->Tornillo M-4x20 Din 912 8.8 Allen zincado", Qty = 4, Reference = "04TOM04AZ20", ImageLoad = GetImage(path) });

            listclsBOM.Add(new clsBOM() { Description = "COLUMNA E.V", Qty = 1, Reference = "04C6515", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = " ->Tornillo M-4x8 Din 912 8.8 Allen zincado", Qty = 1, Reference = "04TOM04AZ08", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = " ->Tornillo M-4x70 Din 912 8.8 Allen zincado", Qty = 1, Reference = "04TOM04AZ70", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = "PLACAS EV EV EV", Qty = 1, Reference = "05PLSR9B", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = "ELECTROVALVULA SUJECIÓN", Qty = 1, Reference = "02ESY24M2", ImageLoad = GetImage(path) });
            listclsBOM.Add(new clsBOM() { Description = " ->ELECTROVALVULA SUJECIÓN", Qty = 1, Reference = "02ESY24M2", ImageLoad = GetImage(path) });
            gridBOM.ItemsSource = listclsBOM;


        }

        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        private void TileNavSubItem_Click(object sender, EventArgs e)
        {
            header.SelectedIndex = 0;
            gd1.ColumnDefinitions[0].Width = new GridLength(100, GridUnitType.Star);
            gd1.ColumnDefinitions[1].Width = new GridLength(0);
            //WindowsFormsHost1.Visibility = Visibility.Visible;
            AxEModelView.AxEModelViewControl ctrlEdrawings = new AxEModelView.AxEModelViewControl();
            ctrlEdrawings.OnComponentSelectionNotify += new AxEModelView._IEModelViewControlEvents_OnComponentSelectionNotifyEventHandler(this.ctrlEdrawings_OnComponentSelectionNotify);
            ctrlEdrawings.OnFinishedLoadingDocument += new AxEModelView._IEModelViewControlEvents_OnFinishedLoadingDocumentEventHandler(this.ctrlEdrawings_OnFinishedLoadingDocument);
            ctrlEdrawings.OnComponentMouseOverNotify += new AxEModelView._IEModelViewControlEvents_OnComponentMouseOverNotifyEventHandler(this.ctrlEdrawings_OnComponentMouseOverNotify);
            //ctrlEdrawings.BackgroundColorGradient = true;
            //ctrlEdrawings.BackgroundColorOverride = true;
            string path = System.IO.Path.GetFullPath("CONJUNT.SLDASM");



            // string path = @"\\10.0.9.2\rd$\Projects\Solidworks\Drawings\142434\1 - Especial + OTH + COMPONENTE\CONECTOR.SLDPRT";      
            WindowsFormsHost1.Child = ctrlEdrawings;
            // viewdrawing.WindowsFormsHost1.Child=ctrlEdrawings;
            ctrlEdrawings.CreateControl();
            ctrlEdrawings.OpenDoc(path, false, false, false, "");
        }

        private void TileNavSubItem_Click_1(object sender, EventArgs e)
        {

            header.SelectedIndex = 0;
            gd1.ColumnDefinitions[0].Width = new GridLength(100, GridUnitType.Star);
            gd1.ColumnDefinitions[1].Width = new GridLength(0);
            // WindowsFormsHost1.Visibility = Visibility.Visible;
            AxEModelView.AxEModelViewControl ctrlEdrawings = new AxEModelView.AxEModelViewControl();
            ctrlEdrawings.OnComponentSelectionNotify += new AxEModelView._IEModelViewControlEvents_OnComponentSelectionNotifyEventHandler(this.ctrlEdrawings_OnComponentSelectionNotify);
            ctrlEdrawings.OnFinishedLoadingDocument += new AxEModelView._IEModelViewControlEvents_OnFinishedLoadingDocumentEventHandler(this.ctrlEdrawings_OnFinishedLoadingDocument);
            ctrlEdrawings.OnComponentMouseOverNotify += new AxEModelView._IEModelViewControlEvents_OnComponentMouseOverNotifyEventHandler(this.ctrlEdrawings_OnComponentMouseOverNotify);
            //ctrlEdrawings.BackgroundColorGradient = true;
            //ctrlEdrawings.BackgroundColorOverride = true;

            string path = System.IO.Path.GetFullPath("CONECTOR.SLDPRT");

            // string path = @"\\10.0.9.2\rd$\Projects\Solidworks\Drawings\142434\1 - Especial + OTH + COMPONENTE\CONECTOR.SLDPRT";      
            WindowsFormsHost1.Child = ctrlEdrawings;
            // viewdrawing.WindowsFormsHost1.Child=ctrlEdrawings;
            ctrlEdrawings.CreateControl();
            ctrlEdrawings.OpenDoc(path, false, false, false, "");
        }

        private void NavButton_Click(object sender, EventArgs e)
        {
            header.SelectedIndex = 1;
            //WindowsFormsHost1.Visibility = Visibility.Hidden;
            // WindowsFormsHost1.Visibility = Visibility.Visible;
            FillBOM();
        }

        private void ListBoxEdit_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

            if (ctrlEdrawings.FileName.ToLower().EndsWith("slddrw"))
            {
                ctrlEdrawings.ShowSheet(lstSheets.SelectedIndex);
                WindowsFormsHost1.Child = ctrlEdrawings;
            }

        }

        private void fillRevision()
        {
            treeListControl1.ItemsSource = null;
            ObservableCollection<clsRevision> lstRevision = new ObservableCollection<clsRevision>();
            // lstRevision.Add(new clsRevision { IdDrawing = 1, RevisionNumber = "300462", ParentID = 0 });
            lstRevision.Add(new clsRevision { IdDrawing = 2, ParentID = 0, Type = "REWORK", RevisionNumber = "Rev. 2", Site = "Spain", Problem = "26/09/2014 17.42 -- juan Carlos Cid [140622P028001] Diagnosis de puntos correctos correctos.pierde continuidad", RootCause = "mal concepto", ChangeLog = "añadimos micros", CreatedIn = DateTime.Now, CreatedBy = "(System)", AproovedIn = DateTime.Now, AproovedBy = "said El Moussaoui" ,IsShow=true });
            lstRevision.Add(new clsRevision { IdDrawing = 3, ParentID = 0, Type = "REWORK", RevisionNumber = "Rev. 1", Site = "Spain", Problem = "26/09/2014 17.42 -- juan Carlos Cid [140622P028001] Test de detecciones. no aseguara tamaño terminal pierde continuidad", RootCause = "cambiare a micro para el tamaño terminales", ChangeLog = "cambiare a micro para el tamaño terminales", CreatedIn = DateTime.Now, CreatedBy = "josep M Figuerola", AproovedIn = DateTime.Now, AproovedBy = "Andrés pérez", IsShow = false });
            lstRevision.Add(new clsRevision { IdDrawing = 4, ParentID = 0, Type = "REWORK", RevisionNumber = "Rev. 0", Site = "Spain", Problem = "", RootCause = "", ChangeLog = "", CreatedIn = DateTime.Now, CreatedBy = "josep M Figuerola", AproovedIn = DateTime.Now, AproovedBy = "said El Moussaoui", IsShow = true });
            //lstRevision.Add(new clsRevision { IdDrawing = 5, RevisionNumber = "300463", ParentID = 2 });
            //lstRevision.Add(new clsRevision { IdDrawing = 6, RevisionNumber = "Rev. 2", ParentID = 2 });

            treeListControl1.ItemsSource = lstRevision;

        }

        private void NavButton_Click_1(object sender, EventArgs e)
        {
            header.SelectedIndex = 2;

        }

        private void TileNavSubItem_Click_2(object sender, EventArgs e)
        {
            header.SelectedIndex = 0;

            gd1.ColumnDefinitions[0].Width = new GridLength(70, GridUnitType.Star);
            gd1.ColumnDefinitions[1].Width = new GridLength(200);

            ctrlEdrawings.OnComponentSelectionNotify += new AxEModelView._IEModelViewControlEvents_OnComponentSelectionNotifyEventHandler(this.ctrlEdrawings_OnComponentSelectionNotify);
            ctrlEdrawings.OnFinishedLoadingDocument += new AxEModelView._IEModelViewControlEvents_OnFinishedLoadingDocumentEventHandler(this.ctrlEdrawings_OnFinishedLoadingDocument);
            ctrlEdrawings.OnComponentMouseOverNotify += new AxEModelView._IEModelViewControlEvents_OnComponentMouseOverNotifyEventHandler(this.ctrlEdrawings_OnComponentMouseOverNotify);

            string path = System.IO.Path.GetFullPath("PLANOL.SLDDRW");

            // string path = @"\\10.0.9.2\rd$\Projects\Solidworks\Drawings\142434\1 - Especial + OTH + COMPONENTE\CONECTOR.SLDPRT";      

            WindowsFormsHost1.Child = ctrlEdrawings;
            ctrlEdrawings.CreateControl();
            ctrlEdrawings.OpenDoc(path, false, false, false,"");

            lstSheets.Items.Clear();

               int milliseconds = 2000;
               Thread.Sleep(milliseconds); int r = 0;
              //  System.Windows.MessageBox.Show("0");
              

                   for (int i = 0; i < ctrlEdrawings.SheetCount; i++)
                   {
                       lstSheets.Items.Add(ctrlEdrawings.get_SheetName(i));
                   }
           
            
        }


        private void opendoc()
        {
            WindowsFormsHost1.Child = ctrlEdrawings;
            ctrlEdrawings.CreateControl();
            string path = System.IO.Path.GetFullPath("PLANOL.SLDDRW");
            ctrlEdrawings.Refresh();
        }
    }
}
