using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
//using DevExpress.XtraGauges.Core.Resources;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.HarnessPart.Views
{
    /// <summary>
    /// Interaction logic for ucSampleManagment.xaml
    /// </summary>
    public partial class ucSampleManagment : UserControl
    {

        

        clsStorageLocations _addnewlocation = new clsStorageLocations();

        public clsStorageLocations Addnewlocation
        {
            get { return _addnewlocation; }
            set { _addnewlocation = value; }
        }
        private ObservableCollection<clsStorageLocations> _objclsStorageLocations;

        public ObservableCollection<clsStorageLocations> ObjclsStorageLocations
        {
            get { return _objclsStorageLocations; }
            set { _objclsStorageLocations = value; }
        }
        public ucSampleManagment()
        {
            InitializeComponent();
            var result1 = new ObservableCollection<clsGrid>();
            result1.Add(new clsGrid() { Reference = "EPN070047", Cavities = 13, Color = "Black", ConImage1 = GetImage(@"\Image\con1.jpg"), Damaged = "3", WithWires = "1", WithoutWires = "2", InHarness = "0",Code="11111111111" });
            result1.Add(new clsGrid() { Reference = "EPN050036", Cavities = 4, Color = "Green", ConImage1 = GetImage(@"\Image\NotAvailable.png"), Damaged = "3", WithWires = "1", WithoutWires = "2", InHarness = "0" });
            result1.Add(new clsGrid() { Reference = "EPN050015", Cavities = 0, Color = "Red", ConImage1 = GetImage(@"\Image\con3.jpg"), Damaged = "3", WithWires = "1", WithoutWires = "2", InHarness = "0" });
            //result1.Add(new clsGrid() { Reference = 050023, Cavities = 2, Color = "Black", ConImage1 = GetImage(@"\Image\con4.jpg") });
            //result1.Add(new clsGrid() { Reference = 50405, Cavities = 4, Color = "Black", ConImage1 = GetImage(@"\Image\con5.jpg") });
            //result1.Add(new clsGrid() { Reference = 50406, Cavities = 4, Color = "Black", ConImage1 = GetImage(@"\Image\con6.jpg") });
            //ObservableCollection<Brush> foos = new ObservableCollection<Brush>();
            ObservableCollection<Brush> result2 = new ObservableCollection<Brush>();
            result2.Add(Brushes.Red);
            //result2.Add(Brushes.Red);
            result2.Add(Brushes.Yellow);
            result2.Add(Brushes.Blue);
            result2.Add(Brushes.Brown);
            result2.Add(Brushes.Chartreuse);
            var result = new ObservableCollection<clsStorageLocationLanes>();
            result.Add(new clsStorageLocationLanes() { Cavities = "30 Cavities", LocationInfo = "FURD.1", CavitiesConnetor = "3-4 Cavities", Qty = "0", ListclsGrid = result1, ListColor = new ObservableCollection<Brush>() { Brushes.Turquoise }, Colorbcode = Brushes.Red });
            result.Add(new clsStorageLocationLanes() { Cavities = "30 Cavities", LocationInfo = "FURD.2", CavitiesConnetor = "2 Cavities", Qty = "20", ListclsGrid = result1, ListColor = new ObservableCollection<Brush>() { Brushes.Yellow, Brushes.Pink }, Colorbcode = Brushes.Red });
            result.Add(new clsStorageLocationLanes() { Cavities = "30 Cavities", LocationInfo = "FURD.3", CavitiesConnetor = "3 Cavities", Qty = "30", ListclsGrid = result1, ListColor = result2, Colorbcode = Brushes.Red });


            ObjclsStorageLocations = new ObservableCollection<clsStorageLocations>() {
                new clsStorageLocations { LocationName="FURD1", ListTorageLocationLanes=result},
                new clsStorageLocations { LocationName="B", ListTorageLocationLanes=result }, 
                new clsStorageLocations { LocationName="C", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="D", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="E", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="F", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="G", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="H", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="I", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="J", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="K", ListTorageLocationLanes=result}, 
                 new clsStorageLocations { LocationName="B", ListTorageLocationLanes=result }, 
                new clsStorageLocations { LocationName="C", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="D", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="E", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="F", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="G", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="H", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="I", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="J", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="K", ListTorageLocationLanes=result}, 
                 new clsStorageLocations { LocationName="B", ListTorageLocationLanes=result }, 
                new clsStorageLocations { LocationName="C", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="D", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="E", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="F", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="G", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="H", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="I", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="J", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="K", ListTorageLocationLanes=result}, 
                 new clsStorageLocations { LocationName="B", ListTorageLocationLanes=result }, 
                new clsStorageLocations { LocationName="C", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="D", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="E", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="F", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="G", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="H", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="I", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="J", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="K", ListTorageLocationLanes=result}, 
                 new clsStorageLocations { LocationName="B", ListTorageLocationLanes=result }, 
                new clsStorageLocations { LocationName="C", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="D", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="E", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="F", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="G", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="H", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="I", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="J", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="K", ListTorageLocationLanes=result}, 
                 new clsStorageLocations { LocationName="B", ListTorageLocationLanes=result }, 
                new clsStorageLocations { LocationName="C", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="D", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="E", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="F", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="G", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="H", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="I", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="J", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="K", ListTorageLocationLanes=result}, 
                 new clsStorageLocations { LocationName="B", ListTorageLocationLanes=result }, 
                new clsStorageLocations { LocationName="C", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="D", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="E", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="F", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="G", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="H", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="I", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="J", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="K", ListTorageLocationLanes=result}, 
                 new clsStorageLocations { LocationName="B", ListTorageLocationLanes=result }, 
                new clsStorageLocations { LocationName="C", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="D", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="E", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="F", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="G", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="H", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="I", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="J", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="K", ListTorageLocationLanes=result}, 
                 new clsStorageLocations { LocationName="B", ListTorageLocationLanes=result }, 
                new clsStorageLocations { LocationName="C", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="D", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="E", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="F", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="G", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="H", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="I", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="J", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="K", ListTorageLocationLanes=result}, 
                 new clsStorageLocations { LocationName="B", ListTorageLocationLanes=result }, 
                new clsStorageLocations { LocationName="C", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="D", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="E", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="F", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="G", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="H", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="I", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="J", ListTorageLocationLanes=result}, 
                new clsStorageLocations { LocationName="K", ListTorageLocationLanes=result}, 
            };
            
            this.DataContext = ObjclsStorageLocations;
            
        }
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }


        private void CloseAll_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {



        }


        private void flowControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void GroupBox_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void image_MouseEnter(object sender, MouseEventArgs e)
        {
            //flowControl.GetChildren(true,true,true);
            //object obChild;
            //obChild = VisualTreeHelper.GetChild(DependencyObject);
            if (RowData.GetRowData((DependencyObject)e.OriginalSource) != null)
            {

                int rowHandle = RowData.GetRowData((DependencyObject)e.OriginalSource).RowHandle.Value;
                GridControl foundTextBox = FindChild<GridControl>(this.flowControl, "Gridsample");
                image.DataContext = foundTextBox.GetCellValue(rowHandle, "ConImage1");
                image.Visibility = Visibility.Visible;
                image.Width = 300;
                image.Height = 300;
            }
            else
            {

                image.Visibility = Visibility.Hidden;
                image.DataContext = null;
            }
            ;


        }

        public static string GetName(object obj)
        {
            // First see if it is a FrameworkElement
            var element = obj as FrameworkElement;
            if (element != null)
                return element.Name;
            // If not, try reflection to get the value of a Name property.
            try
            {
                return (string)obj.GetType().GetProperty("Name").GetValue(obj, null);
            }
            catch
            {
                // Last of all, try reflection to get the value of a Name field.
                try
                {
                    return (string)obj.GetType().GetField("Name").GetValue(obj);
                }
                catch
                {
                    return (string)obj;
                }
            }
        }
        public static T FindChild<T>(DependencyObject parent, string childName)
   where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
        private void image_MouseLeave(object sender, MouseEventArgs e)
        {
            image.DataContext = null;
            image.Visibility = Visibility.Hidden;

        }


    }
}
