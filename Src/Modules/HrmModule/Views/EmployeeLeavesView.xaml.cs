using System;
using System.Collections.Generic;
using System.Globalization;
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
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Scheduling;
using DevExpress.Mvvm;

namespace Emdep.Geos.Modules.Hrm.Views
{
    /// <summary>
    /// Interaction logic for EmployeeLeaves.xaml
    /// </summary>
    public partial class EmployeeLeavesView : UserControl
    {
        public EmployeeLeavesView()
        {
            InitializeComponent();

            DateTime startDate = new DateTime (Convert.ToInt32(HrmCommon.Instance.SelectedPeriod), DateTime.Now.Month, 1);
            scheduler.Month = startDate;
            scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(DateTime.Today, DateTime.Today.AddDays(1));
        }
    }
}
