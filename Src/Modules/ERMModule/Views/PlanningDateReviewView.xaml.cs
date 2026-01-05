using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.ERM.Views
{
    /// <summary>
    /// Interaction logic for PlanningDateReviewView.xaml
    /// </summary>
    public partial class PlanningDateReviewView : UserControl
    {
        public PlanningDateReviewView()
        {
            InitializeComponent();
           
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            scheduler.Month = startDate;
            //DateTime start = DateTime.Today;
            //DateTime end = start.AddDays(1);
            //scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(start, end);


            //DateTime startDate = new DateTime(Convert.ToInt32(DateTime.Now), DateTime.Now.Month, 1);
            //scheduler.Month = startDate;
            scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(DateTime.Today, DateTime.Today.AddDays(1));
        }

        private void txtSearch_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var temp= (DevExpress.Xpf.Editors.TextEdit)e.OriginalSource;
                temp.Focus();
                var tabke1 = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(temp), 0, Key.Tab);
                tabke1.RoutedEvent = Keyboard.KeyDownEvent;
                InputManager.Current.ProcessInput(tabke1);

            }
            catch(Exception ex)
            {

            }
        }

       
    }
}
