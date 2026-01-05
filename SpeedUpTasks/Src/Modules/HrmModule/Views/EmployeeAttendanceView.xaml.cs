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

namespace Emdep.Geos.Modules.Hrm.Views
{
    /// <summary>
    /// Interaction logic for EmployeeAttendanceView.xaml
    /// </summary>
    public partial class EmployeeAttendanceView : UserControl
    {
        public EmployeeAttendanceView()
        {
            InitializeComponent();
            DateTime now = DateTime.Now;
            //var startDate = new DateTime(now.Year, now.Month, 1);
            //scheduler.Month = startDate;
            DateTime start = DateTime.Today;
            DateTime end = start.AddDays(1);
            scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(start, end);


            DateTime startDate = new DateTime(Convert.ToInt32(HrmCommon.Instance.SelectedPeriod), DateTime.Now.Month, 1);
            scheduler.Month = startDate;
            scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(DateTime.Today, DateTime.Today.AddDays(1));
        }
    }
}
