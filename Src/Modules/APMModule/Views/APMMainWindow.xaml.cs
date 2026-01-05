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
using Emdep.Geos.UI.Common;
using Prism.Logging;

namespace Emdep.Geos.Modules.APM.Views
{
    /// <summary>
    /// Interaction logic for APMMainWindow.xaml
    /// </summary>
    public partial class APMMainWindow : UserControl
    {
        public APMMainWindow()
        {
            GeosApplication.Instance.Logger.Log("APMMainWindow: Constructor start", category: Category.Info, priority: Priority.Low);
            try
            {
                InitializeComponent();
                GeosApplication.Instance.Logger.Log("APMMainWindow: Constructor end", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"APMMainWindow: InitializeComponent exception: {ex}", category: Category.Exception, priority: Priority.High);
                throw;
            }
        }
    }
}
