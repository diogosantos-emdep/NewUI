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
using Emdep.Geos.Modules.HarnessPart.Class;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Emdep.Geos.Modules.HarnessPart
{
    /// <summary>
    /// Interaction logic for HarnessPartSearch.xaml
    /// </summary>
    public partial class ucHarnessPartSearch : UserControl
    {
        public ucHarnessPartSearch()
        {
            InitializeComponent();
            ObservableCollection<clsType> customers = new ObservableCollection<clsType>();
            //            Abrazadera
            //Angular
            //Auxiliar
            //Bare Terminal

            //Blind Plate
            //Brida
            //Bridge
            //Closed
            //Electronic
            //Flat
            //Gaveta
            //Gaveta
            //Mechanical
            //Pneumatic
            //Screw
            //Standard
            //Terminal
            //Terminal Splice
            //Universal part
            //Vision


            //List<clsHarnessPartCategory> stuff = new List<clsHarnessPartCategory>();
            //stuff.Add(new clsHarnessPartCategory() { IdFamily = 1, ParentID = 0, FamilyName = "Ground-Terrminal" });
            //stuff.Add(new clsHarnessPartCategory() { IdFamily = 2, ParentID = 1, FamilyName = "Angular" });
            //stuff.Add(new clsHarnessPartCategory() { IdFamily = 3, ParentID = 1, FamilyName = "Bar Terrminal"});
            //stuff.Add(new clsHarnessPartCategory() { IdFamily = 4, ParentID = 1, FamilyName = "Battery" });
            //stuff.Add(new clsHarnessPartCategory() { IdFamily = 5, ParentID = 1, FamilyName = "Karen J. Kelly"});



            //stuff.Add(new clsHarnessPartCategory() { IdFamily = 7, ParentID = 2, FamilyName = "Irma R. Marshall" });
            //stuff.Add(new clsHarnessPartCategory() { IdFamily = 8, ParentID = 2, FamilyName = "John C. Powell" });
            //stuff.Add(new clsHarnessPartCategory() { IdFamily = 9, ParentID = 2, FamilyName = "Christian P. Laclair" });
            //stuff.Add(new clsHarnessPartCategory() { IdFamily = 10, ParentID = 2, FamilyName = "Karen J. Kelly" });
            //lookUpEdit.ItemsSource = stuff;
        }
        public class Employee
        {
            public int ID { get; set; }
            public int ParentID { get; set; }
            public string Name { get; set; }
            public string Position { get; set; }
            public string Department { get; set; }
        }
        public static class Stuff
        {
            public static List<Employee> GetStuff()
            {
                List<Employee> stuff = new List<Employee>();
                stuff.Add(new Employee() { ID = 1, ParentID = 0, Name = "Ground-Terrminal" });
                stuff.Add(new Employee() { ID = 2, ParentID = 1, Name = "Angular" });
                stuff.Add(new Employee() { ID = 3, ParentID = 1, Name = "Bar Terrminal" });
                stuff.Add(new Employee() { ID = 4, ParentID = 1, Name = "Battery" });
                stuff.Add(new Employee() { ID = 5, ParentID = 1, Name = "Karen J. Kelly" });
               

                //stuff.Add(new Employee() { ID = 6, ParentID = 2, Name = "Brian C. Cowling", Department = "Marketing", Position = "Manager" });
                //stuff.Add(new Employee() { ID = 7, ParentID = 2, Name = "Thomas C. Dawson", Department = "Marketing", Position = "Manager" });
                //stuff.Add(new Employee() { ID = 8, ParentID = 2, Name = "Angel M. Wilson", Department = "Marketing", Position = "Manager" });
                //stuff.Add(new Employee() { ID = 9, ParentID = 2, Name = "Bryan R. Henderson", Department = "Marketing", Position = "Manager" });

                //stuff.Add(new Employee() { ID = 10, ParentID = 3, Name = "Harold S. Brandes", Department = "Operations", Position = "Manager" });
                //stuff.Add(new Employee() { ID = 11, ParentID = 3, Name = "Michael S. Blevins", Department = "Operations", Position = "Manager" });
                //stuff.Add(new Employee() { ID = 12, ParentID = 3, Name = "Jan K. Sisk", Department = "Operations", Position = "Manager" });
                //stuff.Add(new Employee() { ID = 13, ParentID = 3, Name = "Sidney L. Holder", Department = "Operations", Position = "Manager" });

                //stuff.Add(new Employee() { ID = 14, ParentID = 4, Name = "James L. Kelsey", Department = "Production", Position = "Manager" });
                //stuff.Add(new Employee() { ID = 15, ParentID = 4, Name = "Howard M. Carpenter", Department = "Production", Position = "Manager" });
                //stuff.Add(new Employee() { ID = 16, ParentID = 4, Name = "Jennifer T. Tapia", Department = "Production", Position = "Manager" });

                //stuff.Add(new Employee() { ID = 17, ParentID = 5, Name = "Judith P. Underhill", Department = "Finance", Position = "Manager" });
                //stuff.Add(new Employee() { ID = 18, ParentID = 5, Name = "Russell E. Belton", Department = "Finance", Position = "Manager" });
                return stuff;
            }
        }
        //public static class Stuff
        //{
        //    public static List<clsHarnessPartCategory> GetStuff()
        //    {
        //        List<clsHarnessPartCategory> stuff = new List<clsHarnessPartCategory>();
        //        stuff.Add(new clsHarnessPartCategory() { IdFamily = 1, ParentID = 0, FamilyName = "Ground-Terrminal" });
        //        stuff.Add(new clsHarnessPartCategory() { IdFamily = 2, ParentID = 1, FamilyName = "Angular" });
        //        stuff.Add(new clsHarnessPartCategory() { IdFamily = 3, ParentID = 1, FamilyName = "Bar Terrminal" });
        //        stuff.Add(new clsHarnessPartCategory() { IdFamily = 4, ParentID = 1, FamilyName = "Battery" });
        //        stuff.Add(new clsHarnessPartCategory() { IdFamily = 5, ParentID = 1, FamilyName = "Karen J. Kelly" });



        //        stuff.Add(new clsHarnessPartCategory() { IdFamily = 7, ParentID = 2, FamilyName = "Irma R. Marshall" });
        //        stuff.Add(new clsHarnessPartCategory() { IdFamily = 8, ParentID = 2, FamilyName = "John C. Powell" });
        //        stuff.Add(new clsHarnessPartCategory() { IdFamily = 9, ParentID = 2, FamilyName = "Christian P. Laclair" });
        //        stuff.Add(new clsHarnessPartCategory() { IdFamily = 10, ParentID = 2, FamilyName = "Karen J. Kelly" });
        //        //stuff.Add(new Employee() { ID = 6, ParentID = 2, Name = "Brian C. Cowling", Department = "Marketing", Position = "Manager" });
        //        //stuff.Add(new Employee() { ID = 7, ParentID = 2, Name = "Thomas C. Dawson", Department = "Marketing", Position = "Manager" });
        //        //stuff.Add(new Employee() { ID = 8, ParentID = 2, Name = "Angel M. Wilson", Department = "Marketing", Position = "Manager" });
        //        //stuff.Add(new Employee() { ID = 9, ParentID = 2, Name = "Bryan R. Henderson", Department = "Marketing", Position = "Manager" });

        //        //stuff.Add(new Employee() { ID = 10, ParentID = 3, Name = "Harold S. Brandes", Department = "Operations", Position = "Manager" });
        //        //stuff.Add(new Employee() { ID = 11, ParentID = 3, Name = "Michael S. Blevins", Department = "Operations", Position = "Manager" });
        //        //stuff.Add(new Employee() { ID = 12, ParentID = 3, Name = "Jan K. Sisk", Department = "Operations", Position = "Manager" });
        //        //stuff.Add(new Employee() { ID = 13, ParentID = 3, Name = "Sidney L. Holder", Department = "Operations", Position = "Manager" });

        //        //stuff.Add(new Employee() { ID = 14, ParentID = 4, Name = "James L. Kelsey", Department = "Production", Position = "Manager" });
        //        //stuff.Add(new Employee() { ID = 15, ParentID = 4, Name = "Howard M. Carpenter", Department = "Production", Position = "Manager" });
        //        //stuff.Add(new Employee() { ID = 16, ParentID = 4, Name = "Jennifer T. Tapia", Department = "Production", Position = "Manager" });

        //        //stuff.Add(new Employee() { ID = 17, ParentID = 5, Name = "Judith P. Underhill", Department = "Finance", Position = "Manager" });
        //        //stuff.Add(new Employee() { ID = 18, ParentID = 5, Name = "Russell E. Belton", Department = "Finance", Position = "Manager" });
        //        return stuff;
        //    }
        //}
        private void TextEdit_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (e.Value == null) return;
            if (e.Value.ToString().Length > 4) return;
            e.IsValid = false;
            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
            e.ErrorContent = "User ID is less than five symbols. Please correct.";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            this.DataContext = this;
            lookUpEdit.ItemsSource = Stuff.GetStuff();
        
            ObservableCollection<clsType> customers = new ObservableCollection<clsType>();
            customers.Add(new clsType() { ID = 0, Name = "Reference" });
            customers.Add(new clsType() { ID = 1, Name = "Way" });
            customers.Add(new clsType() { ID = 2, Name = "Family" });


            comboBox.ItemsSource = customers;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");
            //CultureInfo.CreateSpecificCulture(Emdep.Geos.Modules.HarnessPart.Properties.Settings.Default.DefaultLanguage);
            //CultureInfo cultureReport = CultureInfo.CreateSpecificCulture("en-GB");
            //textEditInternalD.MaskCulture = culture;
            FromInternalDiameter.MaskCulture = culture;
            toInternalDiameter.MaskCulture = culture;
            ObservableCollection<clsAccessories> listclsAccessories = new ObservableCollection<clsAccessories>();
            listclsAccessories.Add(new clsAccessories() { Color = Colors.Purple, Status=true, Name = "Spacer",Reference="00014" });
            listclsAccessories.Add(new clsAccessories() { Color = Colors.Purple, Status = false, Name = "Clip", Reference = "00013" });
            listclsAccessories.Add(new clsAccessories() { Color = Colors.Purple, Status = null, Name = "Levic", Reference = "00015" });
            listclsAccessories.Add(new clsAccessories() { Color = Colors.White, Status = true, Name = "Spacer", Reference = "00015" });
            listclsAccessories.Add(new clsAccessories() { Color = Colors.Black, Status = true, Name = "Spacer", Reference = "00020" });
            //listclsAccessories.Add(new clsType() { ID = 1, Name = "Way" });
            //listclsAccessories.Add(new clsType() { ID = 2, Name = "Family" });
            gridACCESSORIES.ItemsSource = listclsAccessories;


        }
    }
}
