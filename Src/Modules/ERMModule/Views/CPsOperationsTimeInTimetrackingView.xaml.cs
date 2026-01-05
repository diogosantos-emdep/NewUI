using DevExpress.Data;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.ViewModels;
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

namespace Emdep.Geos.Modules.ERM.Views
{
    /// <summary>
    /// Interaction logic for CPsOperationsTimeInTimetrackingView.xaml
    /// </summary>
    public partial class CPsOperationsTimeInTimetrackingView : Window
    {
        CPsOperationsTimeInTimetrackingViewModel CPsOperationsTimeInTimetracking = new CPsOperationsTimeInTimetrackingViewModel();

        public CPsOperationsTimeInTimetrackingView()
        {
            InitializeComponent();
            DataContext = new CPsOperationsTimeInTimetrackingViewModel();
        }


        public TimeSpan ConvertfloattoTimespan(string observedtime)
        {
            TimeSpan UITempobservedTime;
            try
            {
                #region GEOS2-3954 Time format HH:MM:SS
                var currentculter = CultureInfo.CurrentCulture;
                string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                string tempd = Convert.ToString(observedtime);
                string[] parts = new string[2];
                int i1 = 0;
                int i2 = 0;
                if (tempd.Contains(culterseparator))
                {
                    parts = tempd.Split(Convert.ToChar(culterseparator));
                    i1 = int.Parse(parts[0]);
                    i2 = int.Parse(parts[1]);

                    if (Convert.ToString(parts[1]).Length == 1)
                    {
                        i1 = (i1 * 60) + i2 * 10;
                    }
                    else
                    {
                        i1 = (i1 * 60) + i2;
                    }
                    UITempobservedTime = TimeSpan.FromSeconds(i1);
                    int ts1 = UITempobservedTime.Hours;
                    int ts2 = UITempobservedTime.Minutes;
                    int ts3 = UITempobservedTime.Seconds;
                }
                else
                {
                    //parts = tempd.Split(Convert.ToChar(culterseparator));
                    //i1 = int.Parse(parts[0]);
                    //i1 = (i1 * 60);

                    UITempobservedTime = TimeSpan.FromSeconds(Convert.ToInt64(tempd) * 60);  //GEOS2-4045 Gulab Lakade time coversio issue
                    int ts1 = UITempobservedTime.Hours;
                    int ts2 = UITempobservedTime.Minutes;
                    int ts3 = UITempobservedTime.Seconds;
                }

                #endregion
                return UITempobservedTime;
            }
            catch (Exception ex)
            {
                UITempobservedTime = TimeSpan.FromSeconds(0);
                return UITempobservedTime;
            }

        }

        private void ModuleGridParentMenuTreeList_CustomSummary(object sender, TreeListCustomSummaryEventArgs e)
        {
            var viewModel = DataContext as CPsOperationsTimeInTimetrackingViewModel;
            if (viewModel != null)
            {
                viewModel.CustomSummaryCommandAction(e);
            }
        }

        private void OptionsGridParentMenuTreeList_CustomSummary(object sender, TreeListCustomSummaryEventArgs e)
        {
            var viewModel = DataContext as CPsOperationsTimeInTimetrackingViewModel;
            if (viewModel != null)
            {
                viewModel.OptionCustomSummaryCommandAction(e);
            }
        }

        private void DetectionGridParentMenuTreeList_CustomSummary(object sender, TreeListCustomSummaryEventArgs e)
        {
            var viewModel = DataContext as CPsOperationsTimeInTimetrackingViewModel;
            if (viewModel != null)
            {
                viewModel.DetectionCustomSummaryCommandAction(e);
            }
        }

        private void WaysGridParentMenuTreeList_CustomSummary(object sender, TreeListCustomSummaryEventArgs e)
        {
            var viewModel = DataContext as CPsOperationsTimeInTimetrackingViewModel;
            if (viewModel != null)
            {
                viewModel.WaysCustomSummaryCommandAction(e);
            }
        }
    }
}
